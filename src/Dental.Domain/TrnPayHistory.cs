using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace Dental.Domain
{
    [Serializable]
    [Table(Name = TrnPayHistoryColumn.TABLE_NAME)]
    public class TrnPayHistory : BaseDomain<TrnPayHistory>, ICommonFunctions<TrnPayHistory>
    {
        #region Private Member
        private int _OfficeCd;
        private int _SupplierOutsourceCd;
        private int _PurchaseSeq;
        private System.DateTime _PayDate;


        private System.Nullable<double> _PayPrice;
        private System.Nullable<double> _PayFee;
        private System.Nullable<int> _BankCd;
        #endregion

        #region Property Member
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true)]
        public int OfficeCd
        {
            get { return this._OfficeCd; }
            set { this._OfficeCd = value; }
        }
        [ColumnAttribute(Name = TrnPayColumn.SUPPLIER_OUTSOURCE_CD, IsPrimaryKey = true)]
        public int SupplierOutsourceCd
        {
            get { return _SupplierOutsourceCd; }
            set { _SupplierOutsourceCd = value; }
        }
        [ColumnAttribute(Name = TrnPayColumn.PURCHARSE_SEQ, IsPrimaryKey = true)]
        public int PurchaseSeq
        {
            get { return _PurchaseSeq; }
            set { _PurchaseSeq = value; }
        }
        [ColumnAttribute(Name = TrnPayColumn.PAY_DATE, IsPrimaryKey = true)]
        public System.DateTime PayDate
        {
            get { return _PayDate; }
            set { _PayDate = value; }
        }


        [ColumnAttribute(Name = TrnPayColumn.PAY_PRICE, CanBeNull = true)]
        public System.Nullable<double> PayPrice
        {
            get { return _PayPrice; }
            set { _PayPrice = value; }
        }
        [ColumnAttribute(Name = TrnPayColumn.PAY_FEE, CanBeNull = true)]
        public System.Nullable<double> PayFee
        {
            get { return _PayFee; }
            set { _PayFee = value; }
        }
        [ColumnAttribute(Name = TrnMoneyReceiveColumn.BANK_CD, CanBeNull = true)]
        public System.Nullable<int> BankCd
        {
            get { return _BankCd; }
            set { _BankCd = value; }
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

        public TrnPayHistory GetByPrimaryKey()
        {
            Table<TrnPayHistory> table = GetTable();

            TrnPayHistory item = table.Single(d => (d.OfficeCd == this.OfficeCd && d.PayDate == this.PayDate && d.SupplierOutsourceCd == this.SupplierOutsourceCd && this.PurchaseSeq == d.PurchaseSeq ));
            if (item != null)
                item.Detach<TrnPayHistory>();
            return item;
        }
    }
}
