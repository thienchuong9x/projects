using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dental.Domain
{
    [Serializable]
    public class DeliveryStatementInfo
    {
        // local property declarations 
        //hd.[OrderSeq], hd.[OrderNo],hd.[OfficeCd],[StaffCd],[OrderDate],[DeliverDate],[SetDate],[DentalOfficeCd],
        //[DentistNm],[PatientLastNm],[PatientFirstNm],dt.[BridgedID],dt.[ProsthesisCd],dt.[ProsthesisNm], dt.[ToothNumber] , dt.[Amount], dt.[DeliveryStatementNo]                  
        public int OfficeCd { get; set; }
        public double OrderSeq { get; set; }
        public int DetailSeq { get; set; }
        public string DetailNm { get; set; }
        public string OrderNo { get; set; }
        public Nullable<int> StaffCd { get; set; }
        public DateTime OrderDate { get; set; }
        public string ToothNumberReport { get; set; }
        public string InsuranceKbnReport { get; set; }
        //hd.[OrderSeq], hd.[OrderNo],hd.[OfficeCd],[StaffCd],[OrderDate],[DeliverDate],[SetDate],[DentalOfficeCd],
        //[DentistNm],[PatientLastNm],[PatientFirstNm],dt.[BridgedID],dt.[ProsthesisCd],dt.[ProsthesisNm], dt.[ToothNumber] , dt.[Amount], dt.[DeliveryStatementNo]                  

        public string OrderDateStr { get; set; }
        public string DeliverDateStr { get; set; }
        public Nullable<DateTime> CompleteDate { get; set; }
        public DateTime DeliverDate { get; set; }
        public Nullable<DateTime> SetDate { get; set; }
        public int DentalOfficeCd { get; set; }
        public string DentistNm { get; set; }
        public string PatientLastNm { get; set; }
        public string MaterialNm { get; set; }
        public string PatientFirstNm { get; set; }
        public string PatientNm { get; set; }
        public int ProsthesisCd { get; set; }
        public string ProsthesisNm { get; set; }
        public string ToothType { get; set; }
        public Nullable<int> ToothNumber { get; set; }
        public Nullable<int> BridgedID { get; set; }
        public string ToothNumberStr { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> MaterialPrice { get; set; }
        public Nullable<double> ProcessPrice { get; set; }
        public Nullable<double> TechPrice { get; set; }
        public string DeliveryStatementNo { get; set; }
        public Nullable<DateTime> DeliveredDate { get; set; }
        public Nullable<bool> InsuranceKbn { get; set; }
        public Nullable<bool> TrialOrderFlg { get; set; }
        public Nullable<bool> RemanufactureFlg { get; set; }
        public string TrialOrderFlgReport { get; set; }
        public string RemanufactureFlgReport { get; set; }
        public Nullable<bool> ChildFlg { get; set; }
        public string PageNumber { get; set; }
        public string TotalPage { get; set; }
        public string StumpFlg { get; set; }  
      
        public string OutsourceLabStaffNm { get; set; }
        public string StaffNm { get; set; }
        public int Note { get; set; }
        public string OfficeName { get; set; }
        public string DentalOfficeNm { get; set; }

        public static DeliveryStatementInfo getItem()
        {
            DeliveryStatementInfo obj = new DeliveryStatementInfo();
            #region
            //return obj;
            //OperationProcessInfo obj = new OperationProcessInfo();

            //obj.ToothBridgeNm = bridgeNm;
            //obj.Shape = item.Shape;
            //obj.Shade = item.Shade;
            //obj.ProsthesisNm = item.ProsthesisNm;
            //obj.MaterialNm = item.MaterialNm;
            //obj.MaterialCd = item.MaterialCd;
            //obj.Amount = item.Amount;
            //obj.CompleteDate = item.CompleteDate;
            //obj.MaterialPrice = item.MaterialPrice;
            //obj.UnitCd = item.UnitCd;
            //obj.SupplierCd = item.SupplierCd;
            //WriteLog("obj.MaterialCd = " + obj.MaterialCd);

            //obj.listUnit = CodeName.GetListUnitByMaterialCd(obj.MaterialCd);
            //WriteLog("obj.listUnit.count = " + obj.listUnit.Count);
            //obj.listStock = CodeName.GetListSupplierByMaterialCd(obj.MaterialCd, orderDate);
            //try
            //{
            //    obj.Price = obj.listStock.Find(p => p.Code == obj.SupplierCd).HiddenValue;
            //    if (string.IsNullOrEmpty(obj.Price))
            //        obj.Price = null;
            //}
            //catch
            //{
            //    obj.Price = null;
            //}
            //for (int i = 0; i < listProcess.Count; i++)
            //{
            //    CustomProcess temp = new CustomProcess();
            //    temp.ProcessCd = listProcess[i].ProcessCd;
            //    temp.ProcessTime = listProcess[i].ProcessTime;
            //    temp.StaffCd = listProcess[i].StaffCd;
            //    temp.ID = i + 1;

            //    temp.ProcessName = CodeName.GetNameFromCode(DropDownSource.listProcess, temp.ProcessCd.ToString());
            //    temp.StaffNm = CodeName.GetNameFromCode(DropDownSource.listStaff, temp.StaffCd);
            //    obj.listCustomProcess.Add(temp);
            //}

            //OperationProcessInfo.WriteLog(string.Format(" *** ToothBridgeNm = {0} ,  obj.listCustomProcess = {1} ", obj.ToothBridgeNm, obj.listCustomProcess.Count));
            #endregion
            return obj;
        }

        public double TechnicalPrice { get; set; }
        public double Tax { get; set; }
        public double TaxRate { get; set; }
        public string UnitCd { get; set; }
        public Nullable<bool> GapFlg { get; set; }
        public Nullable<int> MaterialCd { get; set; }
        public string Area { get; set; }

        public static List<DeliveryStatementInfo> GetDeliveryStatement(string OrderDateFrom, string OrderDateTo, string DeliverDateFrom, string DeliverDateTo, int OfficeCd, string OrderNo, string DentalOfficeCd, string StaffCd, bool DepositCompleteFlg)
        {
            if (OrderDateFrom == string.Empty)
                OrderDateFrom = "1/1/2000";
            if (OrderDateTo == string.Empty)
                OrderDateTo = "1/1/6000";
            if (DeliverDateFrom == string.Empty)
                DeliverDateFrom = "1/1/2000";
            if (DeliverDateTo == string.Empty)
                DeliverDateTo = "1/1/6000";
            
            var context = new DBContext();           
           
            return (from itemHeader in context.GetTable<TrnOrderHeader>()
                     join
                         itemDetail in context.GetTable<TrnOrderDetail>() on new { itemHeader.OfficeCd, itemHeader.OrderSeq } equals new { itemDetail.OfficeCd, itemDetail.OrderSeq }
                     where
                     (
                         itemHeader.OfficeCd == OfficeCd 
                         && (OrderNo == string.Empty || itemHeader.OrderNo.Contains(OrderNo))
                         && (DentalOfficeCd == string.Empty || itemHeader.DentalOfficeCd.ToString() == DentalOfficeCd)
                         && (StaffCd == string.Empty || itemHeader.StaffCd.Value.ToString() == StaffCd)
                         && (
                             (DepositCompleteFlg == true)
                            || (DepositCompleteFlg == false && itemDetail.DeliveryStatementNo == null)
                         )
                         && (itemDetail.BillStatementNo == null)
                         && (Convert.ToDateTime(OrderDateFrom).Date <= itemHeader.OrderDate.Date && itemHeader.OrderDate.Date <= Convert.ToDateTime(OrderDateTo).Date)
                         && (Convert.ToDateTime(DeliverDateFrom).Date <= itemHeader.DeliverDate.Date && itemHeader.DeliverDate.Date <= Convert.ToDateTime(DeliverDateTo).Date)
                     )
                     orderby itemHeader.OrderSeq, itemDetail.ToothNumber,itemDetail.ProsthesisCd   
                     select new DeliveryStatementInfo()
                     {
                         OfficeCd = itemHeader.OfficeCd,
                         OrderSeq = itemDetail.OrderSeq,
                         DetailSeq = itemDetail.DetailSeq,
                         DetailNm = itemDetail.DetailNm,
                         ToothNumber = itemDetail.ToothNumber.Value,
                         ToothNumberReport = itemDetail.ToothNumber.Value.ToString(),
                         ToothNumberStr = itemDetail.ToothNumber.Value.ToString(),
                         ChildFlg = itemDetail.ChildFlg.Value,
                         GapFlg = itemDetail.GapFlg.Value,
                         BridgedID = itemDetail.BridgedID.Value,
                         InsuranceKbn = itemDetail.InsuranceKbn.Value,
                         InsuranceKbnReport = itemDetail.InsuranceKbn.Value.ToString(),
                         ProsthesisCd = itemDetail.ProsthesisCd,
                         ProsthesisNm = itemDetail.ProsthesisNm,
                         MaterialCd = itemDetail.MaterialCd.Value,
                         MaterialNm = itemDetail.MaterialNm,
                         Amount = itemDetail.Amount.Value,
                         UnitCd = itemDetail.UnitCd,
                         Price = itemDetail.Price.Value,
                         MaterialPrice = itemDetail.MaterialPrice.Value,
                         ProcessPrice = itemDetail.ProcessPrice.Value,
                         CompleteDate = itemDetail.CompleteDate.Value,
                         DeliveryStatementNo = itemDetail.DeliveryStatementNo,
                         DeliveredDate = itemDetail.DeliveredDate.Value,
                         OrderNo = itemHeader.OrderNo,
                         TrialOrderFlg = itemHeader.TrialOrderFlg.Value,
                         TrialOrderFlgReport = itemHeader.TrialOrderFlg.Value.ToString(),
                         RemanufactureFlg = itemHeader.RemanufactureFlg.Value,
                         RemanufactureFlgReport = itemHeader.RemanufactureFlg.Value.ToString(),
                         StaffCd = itemHeader.StaffCd.Value,
                         OrderDate = itemHeader.OrderDate,
                         DeliverDate = itemHeader.DeliverDate,
                         SetDate = itemHeader.SetDate.Value,
                         DentalOfficeCd = itemHeader.DentalOfficeCd,
                         DentistNm = itemHeader.DentistNm,
                         PatientFirstNm = itemHeader.PatientFirstNm,
                         PatientLastNm = itemHeader.PatientLastNm,
                         PatientNm = itemHeader.PatientLastNm + " " + itemHeader.PatientFirstNm,
                        
                     }).ToList();            
            
        }

        public static DeliveryStatementInfo GetOrder(double orderSeq, int detailSeq, int officeCd)
        {
            var context = new DBContext();

            return (from itemHeader in context.GetTable<TrnOrderHeader>()
                    join
                        itemDetail in context.GetTable<TrnOrderDetail>() on new { itemHeader.OfficeCd, itemHeader.OrderSeq } equals new { itemDetail.OfficeCd, itemDetail.OrderSeq }
                    where
                    (
                        itemHeader.OfficeCd == officeCd
                        && (itemHeader.OrderSeq == orderSeq)
                        && (itemDetail.DetailSeq == detailSeq)
                    )
                    select new DeliveryStatementInfo()
                    {
                        OfficeCd = itemHeader.OfficeCd,
                        OrderSeq = itemDetail.OrderSeq,
                        DetailSeq = itemDetail.DetailSeq,
                        DetailNm = itemDetail.DetailNm,
                        ToothNumber = itemDetail.ToothNumber.Value,
                        ToothNumberReport = itemDetail.ToothNumber.Value.ToString(),
                        ToothNumberStr = itemDetail.ToothNumber.Value.ToString(),
                        ChildFlg = itemDetail.ChildFlg.Value,
                        GapFlg = itemDetail.GapFlg.Value,
                        BridgedID = itemDetail.BridgedID.Value,
                        InsuranceKbn = itemDetail.InsuranceKbn.Value,
                        InsuranceKbnReport = itemDetail.InsuranceKbn.Value.ToString(),
                        ProsthesisCd = itemDetail.ProsthesisCd,
                        ProsthesisNm = itemDetail.ProsthesisNm,
                        MaterialCd = itemDetail.MaterialCd.Value,
                        MaterialNm = itemDetail.MaterialNm,
                        Amount = itemDetail.Amount.Value,
                        UnitCd = itemDetail.UnitCd,
                        Price = itemDetail.Price.Value,
                        MaterialPrice = itemDetail.MaterialPrice.Value,
                        ProcessPrice = itemDetail.ProcessPrice.Value,
                        CompleteDate = itemDetail.CompleteDate.Value,
                        DeliveryStatementNo = itemDetail.DeliveryStatementNo,
                        DeliveredDate = itemDetail.DeliveredDate.Value,
                        OrderNo = itemHeader.OrderNo,
                        TrialOrderFlg = itemHeader.TrialOrderFlg.Value,
                        TrialOrderFlgReport = itemHeader.TrialOrderFlg.Value.ToString(),
                        RemanufactureFlg = itemHeader.RemanufactureFlg.Value,
                        RemanufactureFlgReport = itemHeader.RemanufactureFlg.Value.ToString(),
                        StaffCd = itemHeader.StaffCd.Value,
                        OrderDate = itemHeader.OrderDate,
                        DeliverDate = itemHeader.DeliverDate,
                        SetDate = itemHeader.SetDate.Value,
                        DentalOfficeCd = itemHeader.DentalOfficeCd,
                        DentistNm = itemHeader.DentistNm,
                        PatientFirstNm = itemHeader.PatientFirstNm,
                        PatientLastNm = itemHeader.PatientLastNm,
                        PatientNm = itemHeader.PatientLastNm + " " + itemHeader.PatientFirstNm,

                    }).FirstOrDefault();            
            
        }
    }

    public class ConstantIndexDelivelyStatement
    {
        public static int index_OrderSeq = 1;
        public static int index_DetailSeq = 2;
        public static int index_OrderNo = 3;
        public static int index_OrderDate = 4;
        public static int index_DeliverDate = 5;
        public static int index_DeliveredDate = 6;
        public static int index_DentalOfficeCd = 7;
        public static int index_PatientNm = 8;
        public static int index_ProsthesisNm = 9;
        public static int index_ToothNumberStr = 10;
        public static int index_InsuranceKbn = 11;
        public static int index_Trial = 12;
    }
}

