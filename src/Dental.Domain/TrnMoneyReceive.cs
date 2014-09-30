using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace Dental.Domain
{
    [Serializable]
    [Table(Name = TrnMoneyReceiveColumn.TABLE_NAME)]
    public class TrnMoneyReceive : BaseDomain<TrnMoneyReceive>, ICommonFunctions<TrnMoneyReceive>
    {
        #region Private Member
        private int _OfficeCd;
        private System.DateTime _PayDate;
        private int _BillCd;

        private string _BillNm;
        private System.Nullable<double> _PayAmount;
        private System.Nullable<double> _SubtractFee;
        private System.Nullable<double> _SumPayAmount;
        private System.Nullable<double> _UnchargerPay;
        private System.Nullable<int> _BankCd;
        private string _BillStatementNo;
        private string _Note;
        #endregion 

        #region Property Member
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true)]
        public int OfficeCd
        {
            get { return this._OfficeCd; }
            set { this._OfficeCd = value; }
        }
        [ColumnAttribute(Name = TrnMoneyReceiveColumn.PAY_DATE , IsPrimaryKey = true)]
        public System.DateTime PayDate
        {
            get { return _PayDate; }
            set { _PayDate = value; }
        }
        [ColumnAttribute(Name = TrnMoneyReceiveColumn.BILL_CD , IsPrimaryKey = true)]
        public int BillCd
        {
            get { return _BillCd; }
            set { _BillCd = value; }
        }
        [ColumnAttribute(Name = TrnMoneyReceiveColumn.BILL_NM)]
        public string BillNm
        {
            get { return _BillNm; }
            set { _BillNm = value; }
        }
        [ColumnAttribute(Name = TrnMoneyReceiveColumn.PAY_AMOUNT , CanBeNull = true)]
        public System.Nullable<double> PayAmount
        {
            get { return _PayAmount; }
            set { _PayAmount = value; }
        }
        [ColumnAttribute(Name = TrnMoneyReceiveColumn.SUBTRACT_FEE, CanBeNull = true)]
        public System.Nullable<double> SubtractFee
        {
            get { return _SubtractFee; }
            set { _SubtractFee = value; }
        }
        [ColumnAttribute(Name = TrnMoneyReceiveColumn.SUM_PAY_AMOUNT, CanBeNull = true)]
        public System.Nullable<double> SumPayAmount
        {
            get { return _SumPayAmount; }
            set { _SumPayAmount = value; }
        }
        [ColumnAttribute(Name = TrnMoneyReceiveColumn.UNCHARGE_PAY, CanBeNull = true)]
        public System.Nullable<double> UnchargerPay
        {
            get { return _UnchargerPay; }
            set { _UnchargerPay = value; }
        }
            [ColumnAttribute(Name = TrnMoneyReceiveColumn.BANK_CD , CanBeNull = true)]
        public System.Nullable<int> BankCd
        {
            get { return _BankCd; }
            set { _BankCd = value; }
        }
            [ColumnAttribute(Name = TrnMoneyReceiveColumn.BILL_STATEMENT_NO)]
        public string BillStatementNo
        {
            get { return _BillStatementNo; }
            set { _BillStatementNo = value; }
        }
          [ColumnAttribute(Name = TrnMoneyReceiveColumn.NOTE )]
        public string Note
        {
            get { return _Note; }
            set { _Note = value; }
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

        public TrnMoneyReceive GetByPrimaryKey()
        {
            Table<TrnMoneyReceive> table = GetTable();

            TrnMoneyReceive item = table.Single(d => (d.OfficeCd == this.OfficeCd && d.PayDate  == this.PayDate && d.BillCd == this.BillCd ));
            if (item != null)
                item.Detach<TrnMoneyReceive>();
            return item;
        }
    }
}
