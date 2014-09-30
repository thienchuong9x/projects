using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{    
    [Table(Name = MasterUnitColumn.TABLE_NAME)]
    [Serializable]
    public class MasterUnit : BaseDomain<MasterUnit>, ICommonFunctions<MasterUnit>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterUnit));

        #region PrivateMember

        private int _OfficeCd;
        private string _UnitCd;
        private int _MaterialCd;
        private string _UnitNm;
        private bool _Priority;
        private System.Nullable<int> _AmountByMinimumUnit;
        private string _UnitNote;
        private System.Nullable<System.DateTime> _ApplyDate;
        private System.Nullable<System.DateTime> _TerminateDate;        

        #endregion

        #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int OfficeCd
        {
            get { return _OfficeCd; }
            set { _OfficeCd = value; }
        }

        [ColumnAttribute(Name = MasterUnitColumn.UNIT_CD, IsPrimaryKey = true, CanBeNull = false)]
        public string UnitCd
        {
            get { return this._UnitCd; }
            set { this._UnitCd = value; }
        }

        [ColumnAttribute(Name = MasterUnitColumn.MATERIAL_CD, CanBeNull = false)]
        public int MaterialCd
        {
            get { return this._MaterialCd; }
            set { this._MaterialCd = value; }
        }

        [ColumnAttribute(Name = MasterUnitColumn.UNIT_NM, CanBeNull = false)]
        public string UnitNm
        {
            get { return this._UnitNm; }
            set { this._UnitNm = value; }
        }

        [ColumnAttribute(Name = MasterUnitColumn.PRIORITY, CanBeNull = false)]
        public bool Priority
        {
            get { return this._Priority; }
            set { this._Priority = value; }
        }

        [ColumnAttribute(Name = MasterUnitColumn.AMOUNT_BY_MINIMUM_UNIT, CanBeNull = true)]
        public Nullable<int> AmountByMinimumUnit
        {
            get { return this._AmountByMinimumUnit; }
            set { this._AmountByMinimumUnit = value; }
        }

        [ColumnAttribute(Name = MasterUnitColumn.UNIT_NOTE, CanBeNull = true)]
        public string UnitNote
        {
            get { return this._UnitNote; }
            set { this._UnitNote = value; }
        }

        [ColumnAttribute(Name = MasterUnitColumn.APPLY_DATE, CanBeNull = true)]
        public Nullable<DateTime> ApplyDate
        {
            get { return this._ApplyDate; }
            set { this._ApplyDate = value; }
        }

        [ColumnAttribute(Name = MasterUnitColumn.TERMINATE_DATE, CanBeNull = true)]
        public Nullable<DateTime> TerminateDate
        {
            get { return this._TerminateDate; }
            set { this._TerminateDate = value; }
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
        public MasterUnit GetByPrimaryKey()
        {
            Table<MasterUnit> table = GetTable();

            MasterUnit item = table.Single(d => (d.UnitCd == this.UnitCd && d.OfficeCd == this.OfficeCd));
            if (item != null)
                item.Detach<MasterUnit>();
            return item;
        }
        #endregion

        #region Method
        public static List<MasterUnit> GetListUnitByMaterialCd(int officeCd, Nullable<int> materialCd, DateTime orderDate)
        {
            return (from item in GetTable()
                    where
                    (
                        item.OfficeCd == officeCd
                       && item.MaterialCd == materialCd
                       && 
                       (
                            (item.ApplyDate != null &&  item.TerminateDate != null && (item.ApplyDate.Value.Date <= orderDate.Date && orderDate.Date <= item.TerminateDate))
                         || (item.ApplyDate != null &&  item.TerminateDate == null && orderDate.Date >= item.ApplyDate)
                       )
                    )
                    orderby item.Priority
                    select item).ToList();
        }

        public static string GetUnitCd(int officeCd, int materialCd)
        {
            return (from item in GetTable()
                    where
                    (
                        item.OfficeCd == officeCd
                       && item.MaterialCd == materialCd
                    )
                    orderby item.UnitCd descending
                    select item.UnitCd).First();
        }        
        #endregion 
    
        public static List<MasterUnit> GetMstUnitByMaterialCd(int _all, int officeCd, int materialCd)
        {
            return (from item in GetTable()
                    where
                    (
                       item.OfficeCd == officeCd                       
                       && item.MaterialCd == materialCd
                       && (_all == 1 || (item.TerminateDate.Value.Date >= DateTime.Now.Date || item.TerminateDate == null))
                    )
                    select item).ToList();	
        }
        public static List<MasterUnit> DeleteMstUnitByMaterial(int officeCd, int materialCd)
        {
            return (from item in GetTable()
                    where
                    (
                        item.OfficeCd == officeCd
                       && item.MaterialCd == materialCd
                    )
                    select item).ToList();	
        }

        public static MasterUnit GetMstUnit(int officeCd, string unitCd)
        {
            return (from item in GetTable()
                    where
                    (
                        item.OfficeCd == officeCd
                       && item.UnitCd == unitCd
                    )
                    select item).FirstOrDefault();	
        }

        public static int GetPriorityOfUnitByMaterial(int officeCd, string materialCd, string unitCd, string applyDate, string terminateDate)
        {
            if(applyDate == string.Empty)
                applyDate ="1/1/2000";
            if(terminateDate == string.Empty)
                terminateDate ="1/1/6000";

            return (from item in GetTable()
                    where
                    (
                        item.OfficeCd == officeCd
                       && item.UnitCd != unitCd
                       && item.MaterialCd.ToString() == materialCd
                       && item.Priority == true
                       && item.ApplyDate != null
                       && (item.TerminateDate == null || item.TerminateDate.Value.Date >= DateTime.Now.Date)
                       && (
                            (item.TerminateDate == null && item.ApplyDate.Value.Date <= Convert.ToDateTime(terminateDate).Date)
                            || (terminateDate == "1/1/6000" && Convert.ToDateTime(applyDate).Date >= item.ApplyDate.Value.Date && Convert.ToDateTime(applyDate).Date <= item.TerminateDate.Value.Date )
                            || (terminateDate == "1/1/6000" && Convert.ToDateTime(applyDate).Date <= item.ApplyDate.Value.Date && item.TerminateDate.Value.Date >= DateTime.Now.Date)
                            || (terminateDate == "1/1/6000" && item.TerminateDate == null)
                            || (item.ApplyDate.Value.Date <= Convert.ToDateTime(applyDate).Date && item.TerminateDate.Value.Date >= Convert.ToDateTime(terminateDate).Date)
                            || (item.ApplyDate.Value.Date >= Convert.ToDateTime(applyDate).Date && item.TerminateDate.Value.Date <= Convert.ToDateTime(terminateDate).Date)
                            || (item.ApplyDate.Value.Date <= Convert.ToDateTime(terminateDate).Date && item.TerminateDate.Value.Date >= Convert.ToDateTime(terminateDate).Date)
                            || (item.ApplyDate.Value.Date <= Convert.ToDateTime(applyDate).Date && item.TerminateDate.Value.Date >= Convert.ToDateTime(applyDate).Date)
                       )
                    )
                    select item).Count();	
        }
       
    }
}
