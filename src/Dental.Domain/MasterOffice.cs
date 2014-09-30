using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{    
    [Table(Name = MasterOfficeColumn.TABLE_NAME)]
    public class MasterOffice : BaseDomain<MasterOffice>, ICommonFunctions<MasterOffice>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterOffice));

       #region PrivateMember

       private int _OfficeCd;       
       private string _OfficeNm;      
       private string _OfficePostalCd;
       private string _OfficeAddress1;
       private string _OfficeAddress2;
       private string _OfficeTEL;
       private string _OfficeFAX;
       private Nullable<bool> _IsDeleted;
      
       #endregion

       #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int OfficeCd
       {
           get { return _OfficeCd; }
           set { _OfficeCd = value; }
       } 

       [ColumnAttribute(Name = MasterOfficeColumn.OFFICE_NAME, CanBeNull = false)]
        public string OfficeNm
       {
           get { return this._OfficeNm; }
           set { this._OfficeNm = value; }
       }
       
       [ColumnAttribute(Name = MasterOfficeColumn.OFFICE_POSTAL_CODE)]
       public string OfficePostalCd
       {
           get{ return this._OfficePostalCd; }
           set{ this._OfficePostalCd = value; }
       }

       [ColumnAttribute(Name = MasterOfficeColumn.OFFICE_ADDRESS1)]
       public string OfficeAddress1
       {
           get{ return this._OfficeAddress1; }
           set{ this._OfficeAddress1 = value; }
       }

      [ColumnAttribute(Name = MasterOfficeColumn.OFFICE_ADDRESS2)]
       public string OfficeAddress2
       {
           get{ return this._OfficeAddress2; }
           set{ this._OfficeAddress2 = value; }
       }

       [ColumnAttribute(Name = MasterOfficeColumn.OFFICE_TEL)]
       public string OfficeTEL
       {
           get{ return this._OfficeTEL; }
           set{ this._OfficeTEL = value; }
       }

       [ColumnAttribute(Name = MasterOfficeColumn.OFFICE_FAX)]
       public string OfficeFAX
       {
           get{ return this._OfficeFAX; }
           set{ this._OfficeFAX = value; }
       }

       [ColumnAttribute(Name = MasterOfficeColumn.IS_DELETED)]
       public Nullable<bool> IsDeleted
       {
           get{ return this._IsDeleted; }
           set { this._IsDeleted = value; }
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
       public MasterOffice GetByPrimaryKey()
       {
           Table<MasterOffice> table = GetTable();

           MasterOffice item = table.Single(d => (d.OfficeCd == this.OfficeCd));
           if (item != null)
               item.Detach<MasterOffice>();

           return item;
       }
       #endregion 

       #region Method

       public static List<MasterOffice> GetOfficeMasters()
       {
           return (from item in GetTable()
                   select item).ToList();
       }

       public static MasterOffice GetOfficeMaster(int officeCd)
       {
           return (from item in GetTable()
                        where item.OfficeCd == officeCd
                        select item).FirstOrDefault();
       }

       public static List<MasterOffice> GetOfficeMasterSearch(string officeCd, string OfficeName)
       {
           return (from item in GetTable()
                   where
                     (
                        (officeCd == string.Empty || item.OfficeCd.ToString() == officeCd)
                     && (OfficeName == string.Empty || item.OfficeNm.Contains(OfficeName))
                     )

                   select item).ToList();
       }


       #endregion
    }
    
}


