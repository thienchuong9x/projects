using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace Dental.Domain
{
    [Serializable]
    [Table(Name = TrnBillMoneyColumn.TABLE_NAME)]
    public class TrnBillMoney : BaseDomain<TrnBillMoney>, ICommonFunctions<TrnBillMoney>
    {
        #region Private Member

        private int _OfficeCd;
        private double _BillSeq;
        private System.DateTime _PayDateTime;
        private int _BillCd;
        private System.Nullable<double> _PaidMoney;
        private System.Nullable<double> _Balance;



        #endregion

        #region Property Member
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true)]
        public int OfficeCd
        {
            get { return this._OfficeCd; }
            set { this._OfficeCd = value; }
        }
        [ColumnAttribute(Name = TrnBillMoneyColumn.BILL_SEQ, IsPrimaryKey = true)]
        public double BillSeq
        {
            get { return _BillSeq; }
            set { _BillSeq = value; }
        }
        [ColumnAttribute(Name = TrnBillMoneyColumn.PAY_DATE)]
        public System.DateTime PayDateTime
        {
            get { return _PayDateTime; }
            set { _PayDateTime = value; }
        }
        [ColumnAttribute(Name = TrnBillMoneyColumn.BILL_CD)]
        public int BillCd
        {
            get { return _BillCd; }
            set { _BillCd = value; }
        }
        [ColumnAttribute(Name = TrnBillMoneyColumn.PAID_MONEY, CanBeNull = true)]
        public System.Nullable<double> PaidMoney
        {
            get { return _PaidMoney; }
            set { _PaidMoney = value; }
        }
        [ColumnAttribute(Name = TrnBillMoneyColumn.BALANCE, CanBeNull = true)]
        public System.Nullable<double> Balance
        {
            get { return _Balance; }
            set { _Balance = value; }
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

        public TrnBillMoney GetByPrimaryKey()
        {
            Table<TrnBillMoney> table = GetTable();

            TrnBillMoney item = table.Single(d => (d.OfficeCd == this.OfficeCd && d.BillSeq == this.BillSeq && d.PayDateTime == this.PayDateTime && d.BillCd == this.BillCd ));
            if (item != null)
                item.Detach<TrnBillMoney>();
            return item;
        }
    }
}
