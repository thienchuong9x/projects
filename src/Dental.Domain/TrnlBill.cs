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
    [Table(Name = TrnBillColumn.TABLE_NAME)]
    public class TrnBill : BaseDomain<TrnBill>, ICommonFunctions<TrnBill>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(TrnBill));

        #region Private Member
        private int _OfficeCd;
        private double _BillSeq;
        private int _BillDetailNO;
        private string _BillDetailNm;
        private string _BillStatamentNo;
        private System.Nullable<double> _BillDetailPrice;
        private System.Nullable<double> _BillSumPrice;    
        private System.DateTime _BillDate;
        private System.Nullable<double> _OrderSeq;
        private System.Nullable<int> _ToothNumber;
        private int _BillCd;
        private string _BillNm;
        private System.Nullable<int> _DentalOfficeCd;
        private string _DentalOfficeNm;
        private System.Nullable<double> _PaidMoney;
        private System.Nullable<int> _PayCompleteFlg;
        private System.Nullable<System.DateTime> _LastPayDate;
        private string _Note;
        #endregion 

        #region Property Member
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true)]
        public int OfficeCd
        {
            get { return _OfficeCd; }
            set { _OfficeCd = value; }
        }
        [ColumnAttribute(Name = TrnBillColumn.BILL_SEQ , IsPrimaryKey = true)]
        public double BillSeq
        {
            get { return _BillSeq; }
            set { _BillSeq = value; }
        }
        [ColumnAttribute(Name = TrnBillColumn.DETAIL_NO , IsPrimaryKey = true)]
        public int BillDetailNO
        {
            get { return _BillDetailNO; }
            set { _BillDetailNO = value; }
        }
        [ColumnAttribute(Name = TrnBillColumn.DETAIL_NM)]
        public string BillDetailNm
        {
            get { return _BillDetailNm; }
            set { _BillDetailNm = value; }
        }
           [ColumnAttribute(Name = TrnBillColumn.STATEMENT_NO)]
        public string BillStatamentNo
        {
            get { return _BillStatamentNo; }
            set { _BillStatamentNo = value; }
        }
        [ColumnAttribute(Name = TrnBillColumn.DETAIL_PRICE , CanBeNull = true)]
        public System.Nullable<double> BillDetailPrice
        {
            get { return _BillDetailPrice; }
            set { _BillDetailPrice = value; }
        }
         [ColumnAttribute(Name = TrnBillColumn.SUM_PRICE , CanBeNull = true)]
        public System.Nullable<double> BillSumPrice
        {
            get { return _BillSumPrice; }
            set { _BillSumPrice = value; }
        }
               [ColumnAttribute(Name = TrnBillColumn.BILL_DATE)]
        public System.DateTime BillDate
        {
            get { return _BillDate; }
            set { _BillDate = value; }
        }
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.ORDER_SEQ, CanBeNull = true)]
        public System.Nullable<double> OrderSeq
        {
            get { return _OrderSeq; }
            set { _OrderSeq = value; }
        }

         [ColumnAttribute(Name = TrnOrderDetailColumn.TOOTH_NUMBER , CanBeNull = true)]
        public System.Nullable<int> ToothNumber
        {
            get { return _ToothNumber; }
            set { _ToothNumber = value; }
        }
                 [ColumnAttribute(Name = TrnBillColumn.BILL_CD )]
        public int BillCd
        {
            get { return _BillCd; }
            set { _BillCd = value; }
        }
         [ColumnAttribute(Name = TrnBillColumn.BILL_NM)]
        public string BillNm
        {
            get { return _BillNm; }
            set { _BillNm = value; }
        }
         [ColumnAttribute(Name = TrnBillColumn.DENTAL_OFFICE_CD)]
        public System.Nullable<int> DentalOfficeCd
        {
            get { return _DentalOfficeCd; }
            set { _DentalOfficeCd = value; }
        }
        [ColumnAttribute(Name = TrnBillColumn.DENTAL_OFFICE_NM )]
        public string DentalOfficeNm
        {
            get { return _DentalOfficeNm; }
            set { _DentalOfficeNm = value; }
        }
                 [ColumnAttribute(Name = TrnBillColumn.PAID_MONEY , CanBeNull = true)]
        public System.Nullable<double> PaidMoney
        {
            get { return _PaidMoney; }
            set { _PaidMoney = value; }
        }
                 [ColumnAttribute(Name = TrnBillColumn.PAY_COMPLETE_FLG , CanBeNull = true)]
        public System.Nullable<int> PayCompleteFlg
        {
            get { return _PayCompleteFlg; }
            set { _PayCompleteFlg = value; }
        }
                 [ColumnAttribute(Name = TrnBillColumn.LAST_PAY_DATE , CanBeNull = true)]
        public System.Nullable<System.DateTime> LastPayDate
        {
            get { return _LastPayDate; }
            set { _LastPayDate = value; }
        }
         [ColumnAttribute(Name = TrnBillColumn.NOTE )]
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


        public TrnBill GetByPrimaryKey()
        {

             Table<TrnBill> table = GetTable();

             TrnBill item = table.Single(d => (d.OfficeCd == this.OfficeCd && d.BillSeq == this.BillSeq && d.BillDetailNO == this.BillDetailNO));
             if (item != null)
                 item.Detach<TrnBill>();

             return item;
        }

    }
}
