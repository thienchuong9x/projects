using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace Dental.Domain
{    
    [Serializable]
    [Table(Name = MasterProsthesisColumn.TABLE_NAME)]
    public class MasterProsthesis : BaseDomain<MasterProsthesis>, ICommonFunctions<MasterProsthesis>
    {
        private int _OfficeCd;
        private int _ProsthesisCd;
        private string _ProsthesisAbbNm;
        private string _ProsthesisNm;
        private string _ProsthesisType;
        private Nullable<bool> _StumpFlg;
        private int _MinimumProcess;

        #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int OfficeCd
        {
            get { return _OfficeCd; }
            set { _OfficeCd = value; }
        }

        [ColumnAttribute(Name = MasterProsthesisColumn.PROSTHESIS_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int ProsthesisCd
        {
            get { return this._ProsthesisCd; }
            set { this._ProsthesisCd = value; }
        }

        [ColumnAttribute(Name = MasterProsthesisColumn.PROSTHESIS_ABB_NM, CanBeNull = false)]
        public string ProsthesisAbbNm
        {
            get { return this._ProsthesisAbbNm; }
            set { this._ProsthesisAbbNm = value; }
        }
        [ColumnAttribute(Name = MasterProsthesisColumn.PROSTHESIS_NM, CanBeNull = false)]
        public string ProsthesisNm
        {
            get { return this._ProsthesisNm; }
            set { this._ProsthesisNm = value; }
        }
        [ColumnAttribute(Name = MasterProsthesisColumn.PROSTHESIS_TYPE, CanBeNull = false)]
        public string ProsthesisType
        {
            get { return this._ProsthesisType; }
            set { this._ProsthesisType = value; }
        }                

        [ColumnAttribute(Name = MasterProsthesisColumn.STUMP_FLG, CanBeNull = true)]
        public Nullable<bool> StumpFlg
        {
            get { return this._StumpFlg; }
            set { this._StumpFlg = value; }
        }

        [ColumnAttribute(Name = MasterProsthesisColumn.MINIUM_PROCESS, CanBeNull = false)]
        public int MinimumProcess
        {
            get { return this._MinimumProcess; }
            set { this._MinimumProcess = value; }
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
        public MasterProsthesis GetByPrimaryKey()
        {
            Table<MasterProsthesis> table = GetTable();

            MasterProsthesis item = table.Single(d => (d.ProsthesisCd == this.ProsthesisCd && d.OfficeCd == this.OfficeCd));
            if (item != null)
                item.Detach<MasterProsthesis>();

            return item;
        }
        #endregion

        #region Method
        public static List<MasterProsthesis> GetProsthesiss(int officeCd)
        {
            return (from item in GetTable()
                    where 
                      (
                         item.OfficeCd == officeCd
                      )
                    select item).ToList();
        }

        public static MasterProsthesis GetProsthesis(int officeCd, int prosthesisCd)
        {
            return (from item in GetTable()
                    where 
                      (
                         item.OfficeCd == officeCd
                        && item.ProsthesisCd == prosthesisCd
                      )
            select item).FirstOrDefault();
        }

        

        #endregion
    }
}
