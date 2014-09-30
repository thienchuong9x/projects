using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{
    [Table(Name = MasterBillStatementNoColumn.TABLE_NAME)]
    public class MasterBillStatementNo : BaseDomain<MasterBillStatementNo>, ICommonFunctions<MasterBillStatementNo>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterBillStatementNo));

        private int _OfficeCd;
        private int _BillCd;
        private int _Year;
        private int _BlanchNo;

        #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int OfficeCd
        {
            get { return _OfficeCd; }
            set { _OfficeCd = value; }
        }

        [ColumnAttribute(Name = MasterBillStatementNoColumn.BILL_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int BillCd
        {
            get { return this._BillCd; }
            set { this._BillCd = value; }
        }

        [ColumnAttribute(Name = MasterBillStatementNoColumn.YEAR, IsPrimaryKey = true, CanBeNull = false)]
        public int Year
        {
            get { return this._Year; }
            set { this._Year = value; }
        }

        [ColumnAttribute(Name = MasterBillStatementNoColumn.BLANCH_NO, CanBeNull = false)]
        public int BlanchNo
        {
            get { return this._BlanchNo; }
            set { this._BlanchNo = value; }
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
        public MasterBillStatementNo GetByPrimaryKey()
        {
            Table<MasterBillStatementNo> table = GetTable();

            MasterBillStatementNo item = table.Single(d => (d.BillCd == this.BillCd && d.Year == this.Year && d.OfficeCd == this.OfficeCd));
            if (item != null)
                item.Detach<MasterBillStatementNo>();
            return item;
        }

        #endregion

        #region Method

        public static MasterBillStatementNo GetBillStatementNo(int officeCd, int billCd, int Year)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd && item.BillCd == billCd && item.Year == Year
                    select item).FirstOrDefault();
        }        
        #endregion

    }
}
