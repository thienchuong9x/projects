using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{    
    [Serializable]
    [Table(Name = MasterTechPriceColumn.TABLE_NAME)]
    public class MasterTechPrice : BaseDomain<MasterTechPrice>, ICommonFunctions<MasterTechPrice>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterTechPrice));

       #region PrivateMember

       private int _OfficeCd;      
       private int _TechCd;      
       private DateTime _StartDate;
       private int _DentalOfficeCd;          
       private int _TechPrice;

       public bool Editable { get; set; }
       public string TechNm { get; set; }
       #endregion


       public MasterTechPrice()
       {
       }

       #region Property
       [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int OfficeCd
       {
           get { return _OfficeCd; }
           set { _OfficeCd = value; }
       }

       [ColumnAttribute(Name = MasterTechPriceColumn.TECH_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int TechCd
       {
           get { return this._TechCd; }
           set { this._TechCd = value; }
       }      

       [ColumnAttribute(Name = MasterTechPriceColumn.START_DATE, IsPrimaryKey = true, CanBeNull = false)]
       public DateTime StartDate
       {
           get { return this._StartDate; }
           set { this._StartDate = value; }
       }

       [ColumnAttribute(Name = MasterTechPriceColumn.DENTAL_OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int DentalOfficeCd
       {
           get { return this._DentalOfficeCd; }
           set { this._DentalOfficeCd = value; }
       }

       [ColumnAttribute(Name = MasterTechPriceColumn.TECH_PRICE, CanBeNull = false)]
       public int TechPrice
       {
           get { return this._TechPrice; }
           set { this._TechPrice = value; }
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
       public MasterTechPrice GetByPrimaryKey()
       {
           Table<MasterTechPrice> table = GetTable();

           MasterTechPrice item = table.Single(d => (d.TechCd == this.TechCd && d.StartDate == this.StartDate && d.DentalOfficeCd == this.DentalOfficeCd && d.OfficeCd == this.OfficeCd));
           if (item != null)
               item.Detach<MasterTechPrice>();
           return item;
       }
       #endregion 

       #region Method
       public static MasterTechPrice GetMstTechPrice(int OfficeCd, int TechCd, DateTime StartDate, int DentalOfficeCd)
       {
           return (from obj in GetTable()
                   where obj.OfficeCd == OfficeCd && obj.TechCd == TechCd && obj.StartDate == StartDate && obj.DentalOfficeCd == DentalOfficeCd
                   select obj).FirstOrDefault();
       }
       #endregion

       public static List<MasterTechPrice> GetListMasterTechPriceByOrderDate(int officeCd, string strOrderDate, int dentalOfficeCd)
       {
           List<MasterTechPrice> list = new List<MasterTechPrice>();
           if (strOrderDate == string.Empty)
               return list;
           DateTime orderDate = Convert.ToDateTime(strOrderDate);
           var context = new DBContext();

           var listTech = (from mTech in context.GetTable<MasterTech>()
                           where
                              (mTech.OfficeCd == officeCd 
                                 &&
                       (
                        (mTech.EndDate != null && (mTech.StartDate <= orderDate && orderDate <= mTech.EndDate))
                        || (mTech.EndDate == null && mTech.StartDate <= orderDate)
                       )    
                       ) 
                           select mTech).ToList();


           list = (from item in context.GetTable<MasterTechPrice>()
                   join mTech in context.GetTable<MasterTech>() on new { item.OfficeCd, item.TechCd } equals new { mTech.OfficeCd, mTech.TechCd }
                   where
                       (
                         item.OfficeCd == officeCd
                      && item.DentalOfficeCd == dentalOfficeCd
                      &&
                       (
                        (mTech.EndDate != null && (mTech.StartDate <= orderDate && orderDate <= mTech.EndDate))
                        || (mTech.EndDate == null && mTech.StartDate <= orderDate)
                       )
                   )
                   select item).GroupBy(i=>i.TechCd).Select(g => g.First()).ToList();
        

           foreach (MasterTechPrice temp in list)
           {
                MasterTech mTech = listTech.Where(p=>p.TechCd == temp.TechCd).FirstOrDefault();

                temp.TechNm = mTech.TechNm;
                temp.Editable = mTech.Editable;
                if (temp.TechPrice == 0)
                    temp.TechPrice = mTech.StandardTechPrice;
           }
           return list;


       }
    } 
}
