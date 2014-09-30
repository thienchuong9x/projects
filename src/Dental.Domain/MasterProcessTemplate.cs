using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{    
    [Table(Name = MasterProcessTemplateColumn.TABLE_NAME)]
    public class MasterProcessTemplate : BaseDomain<MasterProcessTemplate>, ICommonFunctions<MasterProcessTemplate>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterProcessTemplate));

       #region PrivateMember

       private int _OfficeCd;
       private int _ProsthesisCd;
       private int _ProcessCd;
       private int _DisplayOrder;
      
       #endregion

       #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int OfficeCd
       {
           get { return _OfficeCd; }
           set { _OfficeCd = value; }
       }

       [ColumnAttribute(Name = MasterProcessTemplateColumn.PROSTHESIS_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int ProsthesisCd
       {
           get { return this._ProsthesisCd; }
           set { this._ProsthesisCd = value; }
       }

       [ColumnAttribute(Name = MasterProcessTemplateColumn.PROCESS_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int ProcessCd
       {
           get { return this._ProcessCd; }
           set { this._ProcessCd = value; }
       }    
       
       [ColumnAttribute(Name = MasterProcessTemplateColumn.DISPLAY_ORDER, CanBeNull = false)]
       public int DisplayOrder
       {
           get{ return this._DisplayOrder; }
           set { this._DisplayOrder = value; }
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
       public MasterProcessTemplate GetByPrimaryKey()
       {
           Table<MasterProcessTemplate> table = GetTable();

           MasterProcessTemplate item = table.Single(d => (d.OfficeCd == this.OfficeCd && d.ProsthesisCd == this.ProsthesisCd && d.ProcessCd == this.ProcessCd));
           if (item != null)
               item.Detach<MasterProcessTemplate>();

           return item;
       }
       #endregion 

       #region Method
       public static MasterProcessTemplate GetProcessTempalteMaster(int officeCd, int prothesisCd, int processCd)
       {
           return (from item in GetTable()
                        where item.OfficeCd == officeCd && item.ProsthesisCd == prothesisCd && item.ProcessCd == processCd
                        select item).FirstOrDefault();
       }
     

       #endregion
    }
    
}
