using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dental.Utilities;

namespace Dental.Domain
{
    [Serializable]
    public class CodeName
    {
        public string Code
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }

        public string HiddenValue
        {
            get;
            set;
        }
        public string StaffCd
        {
            get;
            set;
        }
        public CodeName()
        {
        }
        public CodeName(string _code, string _name)
        {
            Code = _code;
            Name = _name;
        }
        public static List<CodeName> GetMasterListCodeName(string dropDownName)
        {
            return GetMasterListCodeName(dropDownName, -1);
        }
        public static List<CodeName> GetMasterListCodeName(string dropDownName, int officeCd)
        {
            List<CodeName> list = new List<CodeName>();
            if (dropDownName == "DropDownProsthesis")
            {
                list = (from item in MasterProsthesis.GetTable()
                        where item.OfficeCd == officeCd
                        select new CodeName()
                        {
                            Code = item.ProsthesisCd.ToString(),
                            Name = item.ProsthesisNm
                        }).ToList();
            }
            else if (dropDownName == "DropDownMaterial")
            {
                list = (from item in MasterMaterial.GetTable()
                        where item.OfficeCd == officeCd
                        select new CodeName()
                        {
                            Code = item.MaterialCd.ToString(),
                            Name = item.MaterialNm
                        }).ToList();
            }
            else if (dropDownName == "DropDownDentalOffice")
            {

                list = (from item in MasterDentalOffice.GetTable()
                        where item.OfficeCd == officeCd
                        select new CodeName()
                        {
                            Code = item.DentalOfficeCd.ToString(),
                            Name = item.DentalOfficeNm,
                            HiddenValue = item.TransferDays == null ? "" : item.TransferDays.ToString()
                        }).ToList();
            }
            else if (dropDownName == "DropDownStaff")
            {
                list = (from item in MasterStaff.GetTable()
                        where item.IsDeleted == false
                        select new CodeName()
                        {
                            Code = item.StaffCd.ToString(),
                            Name = item.StaffNm
                        }).ToList();
            }
            else if (dropDownName == "DropDownSalesMan")
            {
                list = (from item in MasterStaff.GetTable()
                        where item.IsDeleted == false && item.SalesFlg == true
                        select new CodeName()
                        {
                            Code = item.StaffCd.ToString(),
                            Name = item.StaffNm
                        }).ToList();
            }
            else if (dropDownName == "DropDownOutsourceCompany")
            {
                list = (from item in MasterOutsourceLab.GetTable()
                        where item.OfficeCd == officeCd
                        select new CodeName()
                        {
                            Code = item.OutsourceCd.ToString(),
                            Name = item.OutsourceNm
                        }).ToList();
            }
            else if (dropDownName == "DropDownProcess")
            {
                list = (from item in MasterProcess.GetTable()
                        where item.OfficeCd == officeCd
                        select new CodeName()
                        {
                            Code = item.ProcessCd.ToString(),
                            Name = item.ProcessNm
                        }).ToList();
            }
            else if (dropDownName == "DropDownShape")
            {
                list = (from item in MasterItem.GetTable()
                        where item.ItemCathegory == Constant.ITEM_CATHEGORY_SHAPE && item.IsDeleted == false
                        select new CodeName()
                        {
                            Code = item.ItemNo.ToString(),
                            Name = item.ItemNm
                        }).ToList();
            }
            else if (dropDownName == "DropDownShade")
            {
                list = (from item in MasterItem.GetTable()
                        where item.ItemCathegory == Constant.ITEM_CATHEGORY_SHADE && item.IsDeleted == false
                        select new CodeName()
                        {
                            Code = item.ItemNo.ToString(),
                            Name = item.ItemNm
                        }).ToList();
            }
            else if (dropDownName == "DropDownAnatomyKit")
            {
                list = (from item in MasterItem.GetTable()
                        where item.ItemCathegory == Constant.ITEM_CATHEGORY_ANATOMYKIT && item.IsDeleted == false
                        select new CodeName()
                        {
                            Code = item.ItemNo.ToString(),
                            Name = item.ItemNm
                        }).ToList();
            }
            return list;
        }
        public static string GetNameFromCode(List<CodeName> list, string code)
        {
            try
            {
                if (list == null || list.Count == 0)
                    return "";
                return list.Find(p => p.Code == code).Name;
            }
            catch
            {
                return "";
            }
        }


        public static List<CodeName> GetListMaterialByOrderDate(int officeCd, string strOrderDate)
        {
            List<CodeName> list = new List<CodeName>();
            if (strOrderDate == string.Empty)
                return list;
            DateTime orderDate = Convert.ToDateTime(strOrderDate);
            list = (from item in MasterMaterial.GetTable()

                    where
                     (
                         item.OfficeCd == officeCd
                      &&
                     (
                     ((item.ApplyDate != null) && (item.TerminateDate != null) && (item.ApplyDate.Value.Date <= orderDate && orderDate <= item.TerminateDate))
                  || ((item.ApplyDate != null) && (item.TerminateDate == null) && orderDate.Date >= item.ApplyDate))
                     )
                    select new CodeName()
                    {
                        Code = item.MaterialCd.ToString(),
                        Name = item.MaterialNm
                    }).ToList();
            return list;
        }

        public static List<CodeName> GetListCodeNameDentalOfficeMasterByStaff(int officeCd, int? staffCd)
        {
            List<CodeName> list = new List<CodeName>();
            list = (from item in MasterDentalOffice.GetTable()
                    where
                     (
                         item.OfficeCd == officeCd
                      && (staffCd == null || item.StaffCd == staffCd)



                     )
                    select new CodeName()
                    {
                        Code = item.DentalOfficeCd.ToString(),
                        Name = item.DentalOfficeNm,
                        HiddenValue = Common.GetNullableString(item.TransferDays),
                        StaffCd = Common.GetNullableString(item.StaffCd)
                    }).ToList();
            return list;

        }
        public static List<CodeName> GetListUnitByMaterialCd(int officeCd, int? materialCd)
        {

            List<CodeName> list = new List<CodeName>();
            list = (from item in MasterUnit.GetTable()
                    where
                     (
                         item.OfficeCd == officeCd
                      && (materialCd == null || item.MaterialCd == materialCd)
                     )
                    select new CodeName()
                    {
                        Code = item.UnitCd,
                        Name = item.UnitNm
                    }).ToList();
            return list;
        }

        public static List<CodeName> GetTechnicalStaffListCodeName(int officeCd)
        {
            List<CodeName> list = new List<CodeName>();
            list = (from item in MasterStaff.GetTable()
                    where item.IsDeleted == false && item.TechFlg == true 
                    select new CodeName()
                    {
                        Code = item.StaffCd.ToString(),
                        Name = item.StaffNm
                    }).ToList();
            return list;
        }

        public static List<CodeName> GetListMstProcessTemplateByProsthesisCd(int officeCd, int prosthesisCd)
        {
            List<CodeName> list = new List<CodeName>();

            var context = new DBContext(); 
            list = (from item in context.GetTable<MasterProcessTemplate>()
                    join proc in context.GetTable<MasterProcess>() on new {item.OfficeCd , item.ProcessCd }  equals new { proc.OfficeCd , proc.ProcessCd }  
                    where
                        item.OfficeCd == officeCd
                     && item.ProsthesisCd == prosthesisCd
                     && proc.IsDeleted == false 
                    orderby item.DisplayOrder
                    select new CodeName()
                    {
                        Code = item.ProcessCd.ToString(),
                        Name = proc.ProcessNm,
                        HiddenValue = item.DisplayOrder.ToString()

                    }).ToList();
            return list;
        }

        public static List<CodeName> GetMasterStockListCodeNamePrice(int officeCd, string materialCd, DateTime orderDate)
        {
            List<CodeName> list = new List<CodeName>();

            var context = new DBContext();

            list = (from item in context.GetTable<MasterSupplier>()
                    join mprice in context.GetTable<MasterMaterialPrice>() on new { item.OfficeCd, item.SupplierCd } equals new { mprice.OfficeCd, mprice.SupplierCd }
                    where
                        (
                          item.OfficeCd == officeCd
                       && mprice.MaterialCd == Common.GetNullableInt(materialCd)
                       )
                    
                    select new CodeName()
                    {
                        Code = item.SupplierCd.ToString(),
                        Name = item.SupplierNm,
                        HiddenValue = mprice.Price.ToString()

                    }).ToList();
            return list;
        }      
      

    }



}
