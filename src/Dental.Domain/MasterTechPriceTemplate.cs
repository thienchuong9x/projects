using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{    
    [Table(Name = MasterTechPriceTemplateColumn.TABLE_NAME)]
    public class MasterTechPriceTemplate : BaseDomain<MasterTechPriceTemplate>, ICommonFunctions<MasterTechPriceTemplate>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterTechPriceTemplate));

       #region PrivateMember

       private int _OfficeCd;
       private int _ProsthesisCd;
       private int _TechCd;
       private int _DisplayOrder;

       #endregion

       #region Property
       [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int OfficeCd
       {
           get { return _OfficeCd; }
           set { _OfficeCd = value; }
       }

       [ColumnAttribute(Name = MasterTechPriceTemplateColumn.PROSTHESIS_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int ProsthesisCd
       {
           get { return this._ProsthesisCd; }
           set { this._ProsthesisCd = value; }
       }

       [ColumnAttribute(Name = MasterTechPriceTemplateColumn.TECH_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int TechCd
       {
           get { return this._TechCd; }
           set { this._TechCd = value; }
       } 

       [ColumnAttribute(Name = MasterTechPriceTemplateColumn.DISPLAY_ORDER, CanBeNull = false)]
       public int DisplayOrder
       {
           get { return this._DisplayOrder; }
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
       
       #region ICommonFunction
       public MasterTechPriceTemplate GetByPrimaryKey()
       {
           Table<MasterTechPriceTemplate> table = GetTable();

           MasterTechPriceTemplate item = table.Single(d => (d.ProsthesisCd == this.ProsthesisCd && d.TechCd == this.TechCd && d.OfficeCd == this.OfficeCd));
           if (item != null)
               item.Detach<MasterTechPriceTemplate>();
           return item;
       }
       #endregion 

       #region Method
       public static MasterTechPriceTemplate GetTechPriceTempleteMaster(int OfficeCd, int ProsthesisCd, int TechCd)
       {
           return (from obj in GetTable()
                   where obj.OfficeCd == OfficeCd && obj.ProsthesisCd == ProsthesisCd && obj.TechCd == TechCd
                   select obj).FirstOrDefault();
       }
       #endregion

       public static List<MasterTechPriceTemplate> GetListTechPriceTemplateByProsthesisCd(int officeCd, int prosthesisCd)
       {
           return (from item in GetTable()
                   where item.OfficeCd == officeCd && item.ProsthesisCd == prosthesisCd
                        select item).ToList();

       }
    } 
}
