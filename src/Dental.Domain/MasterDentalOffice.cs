using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace Dental.Domain
{
    [Table(Name = MasterDentalOfficeColumn.TABLE_NAME)]
    public class MasterDentalOffice : BaseDomain<MasterDentalOffice>, ICommonFunctions<MasterDentalOffice>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterDentalOffice));

       #region PrivateMember

       private int _OfficeCd;
       private int _DentalOfficeCd;
       private string _DentalOfficeNm;
       private string _DentalOfficeAbbNm;
       private Nullable<int> _StaffCd;
       private string _DentalOfficePostalCd;
       private string _DentalOfficeAddress1;
       private string _DentalOfficeAddress2;
       private string _DentalOfficeTEL;
       private string _DentalOfficeFAX;
       private Nullable<int> _TransferDays;
       private int _BillCd;

       public string BillNm { get; set; }
       #endregion

       #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int OfficeCd
       {
           get { return _OfficeCd; }
           set { _OfficeCd = value; }
       } 

       [ColumnAttribute(Name = MasterDentalOfficeColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int DentalOfficeCd
       {
           get {  return this._DentalOfficeCd; }
           set {   this._DentalOfficeCd = value; }
       }

       [ColumnAttribute(Name = MasterDentalOfficeColumn.OFFICE_NAME)]
       public string DentalOfficeNm
       {
           get{ return this._DentalOfficeNm; }
           set{ this._DentalOfficeNm = value; }
       }

       [ColumnAttribute(Name = MasterDentalOfficeColumn.OFFICE_ABB_NAME)]
       public string DentalOfficeAbbNm
       {
           get{ return this._DentalOfficeAbbNm; }
           set{ this._DentalOfficeAbbNm = value; }
       }


       [ColumnAttribute(Name = MasterDentalOfficeColumn.OFFICE_STAFF_CD, CanBeNull = true)]
       public Nullable<int> StaffCd
       {
           get{ return this._StaffCd; }
           set{ this._StaffCd = value; }
       }

       [ColumnAttribute(Name = MasterDentalOfficeColumn.OFFICE_POSTAL_CODE)]
       public string DentalOfficePostalCd
       {
           get{ return this._DentalOfficePostalCd; }
           set{ this._DentalOfficePostalCd = value; }
       }

     [ColumnAttribute(Name = MasterDentalOfficeColumn.OFFICE_ADDRESS1)]
       public string DentalOfficeAddress1
       {
           get{ return this._DentalOfficeAddress1; }
           set{ this._DentalOfficeAddress1 = value; }
       }

      [ColumnAttribute(Name = MasterDentalOfficeColumn.OFFICE_ADDRESS2)]
       public string DentalOfficeAddress2
       {
           get{ return this._DentalOfficeAddress2; }
           set{ this._DentalOfficeAddress2 = value; }
       }

       [ColumnAttribute(Name = MasterDentalOfficeColumn.OFFICE_TEL)]
       public string DentalOfficeTEL
       {
           get{ return this._DentalOfficeTEL; }
           set{ this._DentalOfficeTEL = value; }
       }

       [ColumnAttribute(Name = MasterDentalOfficeColumn.OFFICE_FAX)]
       public string DentalOfficeFAX
       {
           get{ return this._DentalOfficeFAX; }
           set{ this._DentalOfficeFAX = value; }
       }

       [ColumnAttribute(Name = MasterDentalOfficeColumn.TRANSFER_DAY)]
       public System.Nullable<int> TransferDays
       {
           get{ return this._TransferDays; }
           set{ this._TransferDays = value; }
       }
       [ColumnAttribute(Name = MasterDentalOfficeColumn.BILL_CD )]
       public int BillCd
       {
           get { return _BillCd; }
           set { _BillCd = value; }
       }
      
       [ColumnAttribute(Name = BaseColumn.CREATE_DATE)]
       public System.DateTime CreateDate
       {
           get { return this._CreateDate; }
           set { this._CreateDate = value; }
       }

       [ColumnAttribute(Name = BaseColumn.CREATE_ACCOUNT)]
       public string CreateAccount
       {
           get { return this._CreateAccount; }
           set { this._CreateAccount = value; }
       }

       [ColumnAttribute(Name = BaseColumn.MODIFIED_DATE)]
       public System.DateTime ModifiedDate
       {
           get { return this._ModifiedDate; }
           set { this._ModifiedDate = value; }
       }

       [ColumnAttribute(Name = BaseColumn.MODIFIED_ACCOUNT)]
       public string ModifiedAccount
       {
           get { return this._ModifiedAccount; }
           set { this._ModifiedAccount = value; }
       }
       #endregion

       #region Custom Property 
       public string StaffNm { get; set; } 

       #endregion 

       #region ICommonFunction
       public MasterDentalOffice GetByPrimaryKey()
       {
           Table<MasterDentalOffice> table = GetTable();

           MasterDentalOffice item = table.Single(d => (d.DentalOfficeCd == this.DentalOfficeCd && d.OfficeCd == this.OfficeCd ));
           if (item != null)
               item.Detach<MasterDentalOffice>();

           return item;
       }
       #endregion 

       #region Method
       public static MasterDentalOffice GetDentalOfficeMaster(int officeCd, int dentalOfficeCd)
       {

           var result = from item in GetTable()
                        where item.DentalOfficeCd == dentalOfficeCd && item.OfficeCd == officeCd 
                        select item;
           return (MasterDentalOffice)result.FirstOrDefault();
       }

       public static List<MasterDentalOffice> GetDentalOfficeMasterSearch(int officeCd, string dentalOfficeCd, string dentalOfficeName, string staffCd)
       {
           return (from item in GetTable()
                    where
                        ( 
                            item.OfficeCd  == officeCd 
                        && (dentalOfficeCd == string.Empty || item.DentalOfficeCd.ToString() ==  dentalOfficeCd)
                        && (dentalOfficeName == string.Empty || item.DentalOfficeNm.Contains(dentalOfficeName))
                        && (staffCd == string.Empty || item.StaffCd.ToString() == staffCd )
                        ) 

                    select item).ToList();
       }
       public static List<MasterDentalOffice> GetDentalOfficeMasters(int officeCd)
       {
           return (from item in GetTable()
                   where
                     (
                        item.OfficeCd == officeCd
                     )
                   select item).ToList();
       }       
       #endregion      
    
       
    }    
}
