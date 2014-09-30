using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{
    
    [Table(Name = MasterSupplierColumn.TABLE_NAME)]
    public class MasterSupplier : BaseDomain<MasterSupplier>, ICommonFunctions<MasterSupplier>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterSupplier));

       #region PrivateMember

       private int _OfficeCd;
       private int _SupplierCd;
       private string _SupplierNm;
       private string _SupplierAbbNm;
       private string _SupplierPostalCd;
       private string _SupplierAddress1;
       private string _SupplierAddress2;
       private string _SupplierTEL;
       private string _SupplierFAX;
       private string _SupplierStaff;       
       
       #endregion

       #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int OfficeCd
       {
           get { return _OfficeCd; }
           set { _OfficeCd = value; }
       } 

       [ColumnAttribute(Name = MasterSupplierColumn.SUPPLIER_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int SupplierCd
       {
           get {  return this._SupplierCd; }
           set {   this._SupplierCd = value; }
       }

       [ColumnAttribute(Name = MasterSupplierColumn.SUPPLIER_NM)]
       public string SupplierNm
       {
           get{ return this._SupplierNm; }
           set{ this._SupplierNm = value; }
       }

       [ColumnAttribute(Name = MasterSupplierColumn.SUPPLIER_ABB_NM)]
       public string SupplierAbbNm
       {
           get{ return this._SupplierAbbNm; }
           set{ this._SupplierAbbNm = value; }
       }

       [ColumnAttribute(Name = MasterSupplierColumn.SUPPLIER_POSTAL_CODE)]
       public string SupplierPostalCd
       {
           get{ return this._SupplierPostalCd; }
           set{ this._SupplierPostalCd = value; }
       }

     [ColumnAttribute(Name = MasterSupplierColumn.SUPPLIER_ADDRESS1)]
       public string SupplierAddress1
       {
           get{ return this._SupplierAddress1; }
           set{ this._SupplierAddress1 = value; }
       }

      [ColumnAttribute(Name = MasterSupplierColumn.SUPPLIER_ADDRESS2)]
       public string SupplierAddress2
       {
           get{ return this._SupplierAddress2; }
           set{ this._SupplierAddress2 = value; }
       }

       [ColumnAttribute(Name = MasterSupplierColumn.SUPPLIER_TEL)]
       public string SupplierTEL
       {
           get{ return this._SupplierTEL; }
           set{ this._SupplierTEL = value; }
       }

       [ColumnAttribute(Name = MasterSupplierColumn.SUPPLIER_FAX)]
       public string SupplierFAX
       {
           get{ return this._SupplierFAX; }
           set{ this._SupplierFAX = value; }
       }

       [ColumnAttribute(Name = MasterSupplierColumn.SUPPLIER_STAFF)]
       public string SupplierStaff
       {
           get { return this._SupplierStaff; }
           set { this._SupplierStaff = value; }
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
       public MasterSupplier GetByPrimaryKey()
       {
           Table<MasterSupplier> table = GetTable();

           MasterSupplier item = table.Single(d => (d.SupplierCd == this.SupplierCd && d.OfficeCd == this.OfficeCd ));
           if (item != null)
               item.Detach<MasterSupplier>();

           return item;
       }
       #endregion 

       #region Method
       public static MasterSupplier GetSupplierMaster(int officeCd, int supplierCd)
       {

           var result = from item in GetTable()
                        where item.SupplierCd == supplierCd && item.OfficeCd == officeCd 
                        select item;
           return (MasterSupplier)result.FirstOrDefault();
       }
       public static List<MasterSupplier> GetSupplierMasters(int officeCd)
       {
           return (from item in GetTable()
                   where item.OfficeCd == officeCd
                   select item).ToList();
       }
       #endregion



       public static List<MasterSupplier> GetSupplierMasters(int officeCd, string supplierCd, string supplierNmOrAbbNm)
       {
           return (from item in GetTable()
                   where
                   (
                        item.OfficeCd == officeCd
                        && (supplierCd == string.Empty || item.SupplierCd.ToString().Contains(supplierCd))
                        && (supplierNmOrAbbNm == string.Empty || (item.SupplierNm.Contains(supplierNmOrAbbNm) || item.SupplierAbbNm.Contains(supplierNmOrAbbNm)))
                   )
                   select item).ToList();
       }
    }    
}

