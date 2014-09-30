using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{   
    [Table(Name = MasterOutsourceLabColumn.TABLE_NAME)]
    public class MasterOutsourceLab : BaseDomain<MasterOutsourceLab>, ICommonFunctions<MasterOutsourceLab>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterOutsourceLab));

       #region PrivateMember

       private int _OfficeCd;
       private int _OutsourceCd;
       private string _OutsourceNm;  
       private string _OutsourcePostalCd;
       private string _OutsourceAddress1;
       private string _OutsourceAddress2;
       private string _OutsourceTEL;
       private string _OutsourceFAX;
       private string _OutsourceContactPerson; 
       #endregion

       #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int OfficeCd
       {
           get { return _OfficeCd; }
           set { _OfficeCd = value; }
       } 

       [ColumnAttribute(Name = MasterOutsourceLabColumn.OUTSOURCE_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int OutsourceCd
       {
           get { return this._OutsourceCd; }
           set { this._OutsourceCd = value; }
       }

       [ColumnAttribute(Name = MasterOutsourceLabColumn.OUTSOURCE_NM, CanBeNull = true)]
       public string OutsourceNm
       {
           get { return this._OutsourceNm; }
           set { this._OutsourceNm = value; }
       }

       [ColumnAttribute(Name = MasterOutsourceLabColumn.OUTSOURCE_POSTAL_CODE)]
       public string OutsourcePostalCd
       {
           get{ return this._OutsourcePostalCd; }
           set{ this._OutsourcePostalCd = value; }
       }

       [ColumnAttribute(Name = MasterOutsourceLabColumn.OUTSOURCE_ADDRESS1)]
       public string OutsourceAddress1
       {
           get{ return this._OutsourceAddress1; }
           set{ this._OutsourceAddress1 = value; }
       }

      [ColumnAttribute(Name = MasterOutsourceLabColumn.OUTSOURCE_ADDRESS2)]
       public string OutsourceAddress2
       {
           get{ return this._OutsourceAddress2; }
           set{ this._OutsourceAddress2 = value; }
       }

       [ColumnAttribute(Name = MasterOutsourceLabColumn.OUTSOURCE_TEL)]
       public string OutsourceTEL
       {
           get{ return this._OutsourceTEL; }
           set{ this._OutsourceTEL = value; }
       }

       [ColumnAttribute(Name = MasterOutsourceLabColumn.OUTSOURCE_FAX)]
       public string OutsourceFAX
       {
           get{ return this._OutsourceFAX; }
           set{ this._OutsourceFAX = value; }
       }

       [ColumnAttribute(Name = MasterOutsourceLabColumn.OUTSOURCE_CONTACT_PERSON)]
       public string OutsourceContactPerson
       {
           get { return this._OutsourceContactPerson; }
           set { this._OutsourceContactPerson = value; }
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
       public MasterOutsourceLab GetByPrimaryKey()
       {
           Table<MasterOutsourceLab> table = GetTable();

           MasterOutsourceLab item = table.Single(d => (d.OfficeCd == this.OfficeCd && d.OutsourceCd == this.OutsourceCd ));
           if (item != null)
               item.Detach<MasterOutsourceLab>();

           return item;
       }
       #endregion 

       #region Method
       public static MasterOutsourceLab GetOutsourceLabMaster(int officeCd, int outsourceCd)
       {
           return (from item in GetTable()
                        where item.OfficeCd == officeCd && item.OutsourceCd == outsourceCd 
                        select item).FirstOrDefault();
       }
       public static List<MasterOutsourceLab> GetOutsourceLabMasters(int officeCd)
       {
           return (from item in GetTable()
                   where item.OfficeCd == officeCd
                   select item).ToList();
       }

       public static List<MasterOutsourceLab> GetOutSourceLabSearch(int officeCd, string OutSourceCd, string OutsourceName)
       {
           return (from item in GetTable()
                   where item.OfficeCd == officeCd && item.OutsourceCd.ToString().StartsWith(OutSourceCd)
                   && item.OutsourceNm.Contains(OutsourceName)
                   select item).ToList();
       }
       

       #endregion      
    }
    
}
