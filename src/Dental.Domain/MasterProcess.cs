using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{    
    [Table(Name = MasterProcessColumn.TABLE_NAME)]
    public class MasterProcess : BaseDomain<MasterProcess>, ICommonFunctions<MasterProcess>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterProcess));

       #region PrivateMember

       private int _OfficeCd;
       private int _ProcessCd;
       private string _ProcessNm;       
       private Nullable<bool> _IsDeleted;
      
       #endregion

       #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int OfficeCd
       {
           get { return _OfficeCd; }
           set { _OfficeCd = value; }
       } 

       [ColumnAttribute(Name = MasterProcessColumn.PROCESS_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int ProcessCd
       {
           get { return this._ProcessCd; }
           set { this._ProcessCd = value; }
       }
       
       [ColumnAttribute(Name = MasterProcessColumn.PROCESS_NM, CanBeNull = false)]
       public string ProcessNm
       {
           get { return this._ProcessNm; }
           set { this._ProcessNm = value; }
       }

       [ColumnAttribute(Name = MasterProcessColumn.IS_DELETED, CanBeNull = true)]
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
       public MasterProcess GetByPrimaryKey()
       {
           Table<MasterProcess> table = GetTable();

           MasterProcess item = table.Single(d => (d.OfficeCd == this.OfficeCd && d.ProcessCd == this.ProcessCd));
           if (item != null)
               item.Detach<MasterProcess>();

           return item;
       }
       #endregion 

       #region Method
       public static MasterProcess GetProcessMaster(int officeCd, int processCd)
       {
           return (from item in GetTable()
                        where item.OfficeCd == officeCd && item.ProcessCd == processCd
                        select item).FirstOrDefault();
       }

       public static List<MasterProcess> GetProcessmasterSearch(int OfficeCd, string ProcessCd, string ProcessName)
       {
           return (from process in GetTable()
                   where process.OfficeCd == OfficeCd && process.ProcessCd.ToString().StartsWith(ProcessCd)
                   && process.ProcessNm.Contains(ProcessName)
                   select process).ToList();
       }

       public static List<MasterProcess> GetProcessMasterByProsthesis(int OfficeCd, Nullable<int> ProsthesisCd)
       {
           var dc = new DBContext();
           return (from process in dc.GetTable<MasterProcess>()
                   from processTemp in dc.GetTable<MasterProcessTemplate>()
                   where process.ProcessCd == processTemp.ProcessCd
                   && process.OfficeCd == processTemp.OfficeCd
                   && process.OfficeCd == OfficeCd
                   && processTemp.ProsthesisCd == ProsthesisCd
                   orderby processTemp.DisplayOrder
                   select new
                   {
                       OfficeCd = OfficeCd,
                       ProcessCd = process.ProcessCd,
                       ProcessNm = process.ProcessNm,
                       IsDeleted = process.IsDeleted

                   }).AsEnumerable().Select(p => new MasterProcess
                   {
                       OfficeCd =  p.OfficeCd,
                       ProcessCd = p.ProcessCd,
                       ProcessNm = p.ProcessNm,
                       IsDeleted = p.IsDeleted

                   }).ToList();

       }
       #endregion
    }
    
}
