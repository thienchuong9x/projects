using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{   
    [Table(Name = MasterTechColumn.TABLE_NAME)]
    public class MasterTech : BaseDomain<MasterTech>, ICommonFunctions<MasterTech>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterTech));

       #region PrivateMember

       private int _OfficeCd;      
       private int _TechCd;      
       private DateTime _StartDate;
       private Nullable<DateTime> _EndDate;       
       private string _TechNm;
       private int _StandardTechPrice;
       private bool _Editable;       
       private Nullable<int> _TechGroup;

       #endregion

       #region Property
       [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int OfficeCd
       {
           get { return _OfficeCd; }
           set { _OfficeCd = value; }
       }

       [ColumnAttribute(Name = MasterTechColumn.TECH_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int TechCd
       {
           get { return this._TechCd; }
           set { this._TechCd = value; }
       }      

       [ColumnAttribute(Name = MasterTechColumn.START_DATE, IsPrimaryKey = true, CanBeNull = false)]
       public DateTime StartDate
       {
           get { return this._StartDate; }
           set { this._StartDate = value; }
       }

       [ColumnAttribute(Name = MasterTechColumn.END_DATE, CanBeNull = true)]
       public Nullable<DateTime> EndDate
       {
           get { return this._EndDate; }
           set { this._EndDate = value; }
       }

       [ColumnAttribute(Name = MasterTechColumn.TECH_NM, CanBeNull = false)]
       public string TechNm
       {
           get { return this._TechNm; }
           set { this._TechNm = value; }
       }

       [ColumnAttribute(Name = MasterTechColumn.STANDARD_TECH_PRICE, CanBeNull = false)]
       public int StandardTechPrice
       {
           get { return this._StandardTechPrice; }
           set { this._StandardTechPrice = value; }
       }

       [ColumnAttribute(Name = MasterTechColumn.EDITABLE, CanBeNull = false)]
       public bool Editable
       {
           get { return this._Editable; }
           set { this._Editable = value; }
       }

       [ColumnAttribute(Name = MasterTechColumn.TECH_GROUP, CanBeNull = true)]
       public Nullable<int> TechGroup
       {
           get { return this._TechGroup; }
           set { this._TechGroup = value; }
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
       public MasterTech GetByPrimaryKey()
       {
           Table<MasterTech> table = GetTable();

           MasterTech item = table.Single(d => (d.TechCd == this.TechCd && d.StartDate == this.StartDate && d.OfficeCd == this.OfficeCd));
           if (item != null)
               item.Detach<MasterTech>();
           return item;
       }
       #endregion 

       #region Method
       public static List<MasterTech> GetTechMasterSearch(int OfficeCd,string TechCd, string TechNm, string StartDateMin, string StartDateMax)
       {
           if (StartDateMin==string.Empty)
               StartDateMin = "1/1/1973";
           if (StartDateMax==string.Empty)
               StartDateMax = "12/31/2999";
           return (from result in GetTable() where result.OfficeCd == OfficeCd 
                   && result.TechCd.ToString().StartsWith(TechCd)
                   && result.TechNm.Contains(TechNm)
                   && (result.StartDate >= Convert.ToDateTime(StartDateMin) && result.StartDate <= Convert.ToDateTime(StartDateMax))
                   select result).ToList();
          
       }

       public static MasterTech GetTechMaster(int OfficeCd, int TechCd, DateTime StartDate)
       {
           return (from obj in GetTable()
                   where obj.OfficeCd == OfficeCd && obj.TechCd == TechCd && obj.StartDate == StartDate
                   select obj).FirstOrDefault();
       }

       public static List<MasterTech> GetTechMasterByProsthesis(int OfficeCd, Nullable<int> ProsthesisCd)
       {
           var dc = new DBContext();
           return (from tech in dc.GetTable<MasterTech>()
                   from techtemp in dc.GetTable<MasterTechPriceTemplate>()
                   where tech.TechCd == techtemp.TechCd
                   && tech.OfficeCd == techtemp.OfficeCd
                   && tech.OfficeCd == OfficeCd
                   && techtemp.ProsthesisCd == ProsthesisCd
                   orderby techtemp.DisplayOrder
                   select new
                   {
                       OfficeCd = OfficeCd,
                       TechCd = tech.TechCd,
                       StartDate = tech.StartDate,
                       EndDate = tech.EndDate,
                       TechNm = tech.TechNm,
                       StandardTechPrice = tech.StandardTechPrice,
                       Editable = tech.Editable,
                       TechGroup = tech.TechGroup

                   }).AsEnumerable().Select(t => new MasterTech
                   {
                       OfficeCd = OfficeCd,
                       TechCd = t.TechCd,
                       StartDate = t.StartDate,
                       EndDate = t.EndDate,
                       TechNm = t.TechNm,
                       StandardTechPrice = t.StandardTechPrice,
                       Editable = t.Editable,
                       TechGroup = t.TechGroup

                   }).ToList();
       }
       #endregion
    } 
}
