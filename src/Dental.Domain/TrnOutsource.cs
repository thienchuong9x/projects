using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;
namespace Dental.Domain
{
    [Serializable]
    [Table(Name = TrnOutsourceColumn.TABLE_NAME)]
    public class TrnOutsource : BaseDomain<TrnOutsource>, ICommonFunctions<TrnOutsource>
    {

        private int _OfficeCd;
        private double _OrderSeq;
        private int _DetailSeq;

        private int _OutsourceCd;
        private System.Nullable<System.DateTime> _OutsourceDate;
        private System.Nullable<System.DateTime> _ReceiveEstimateDate;
        private System.Nullable<System.DateTime> _ReceiveDate;
        private System.Nullable<double> _TechPrice;
        private System.Nullable<int> _PurchaseSeq;

        #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true)]
        public int OfficeCd
        {
            get { return this._OfficeCd; }
            set { this._OfficeCd = value; }
        }
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.ORDER_SEQ, CanBeNull = false, IsPrimaryKey = true)]
        public double OrderSeq
        {
            get { return this._OrderSeq; }
            set { this._OrderSeq = value; }
        }
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.DETAIL_SEQ, CanBeNull = false, IsPrimaryKey = true)]
        public int DetailSeq
        {
            get { return _DetailSeq; }
            set { _DetailSeq = value; }
        }
        [ColumnAttribute(Name = TrnOutsourceColumn.OUTSOURCE_CD)]
        public int OutsourceCd
        {
            get { return _OutsourceCd; }
            set { _OutsourceCd = value; }
        }

        [ColumnAttribute(Name = TrnOutsourceColumn.OUTSOURCE_DATE, CanBeNull = true)]
        public System.Nullable<System.DateTime> OutsourceDate
        {
            get { return _OutsourceDate; }
            set { _OutsourceDate = value; }
        }
        [ColumnAttribute(Name = TrnOutsourceColumn.RECEIVE_ESTIMATE_DATE, CanBeNull = true)]
        public System.Nullable<System.DateTime> ReceiveEstimateDate
        {
            get { return _ReceiveEstimateDate; }
            set { _ReceiveEstimateDate = value; }
        }
        [ColumnAttribute(Name = TrnOutsourceColumn.RECEIVE_DATE, CanBeNull = true)]
        public System.Nullable<System.DateTime> ReceiveDate
        {
            get { return _ReceiveDate; }
            set { _ReceiveDate = value; }
        }
        [ColumnAttribute(Name = TrnOutsourceColumn.TECH_PRICE, CanBeNull = true)]
        public System.Nullable<double> TechPrice
        {
            get { return _TechPrice; }
            set { _TechPrice = value; }
        }
        [ColumnAttribute(Name = TrnOutsourceColumn.PURCHASE_SEQ, CanBeNull = true)]
        public System.Nullable<int> PurchaseSeq
        {
            get { return _PurchaseSeq; }
            set { _PurchaseSeq = value; }
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
        public TrnOutsource GetByPrimaryKey()
        {
            Table<TrnOutsource> table = GetTable();

            TrnOutsource item = table.Single(d => (d.OfficeCd == this.OfficeCd && d.OrderSeq == this.OrderSeq && d.DetailSeq == this.DetailSeq));
            if (item != null)
                item.Detach<TrnOutsource>();

            return item;
        }
        #endregion 

        public static TrnOutsource GetTrnOutsource(int officeCd, double orderSeq,int detailSeq)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd && item.OrderSeq == orderSeq && item.DetailSeq == detailSeq 
                    select item).FirstOrDefault();
        }
        public static List<TrnOutsource> GetTrnOutsourceList(int officeCd, double orderSeq)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd && item.OrderSeq == orderSeq
                    select item).ToList();
        }
        public static TrnOutsource GetTrnOutsourceByTrnPurchase(int officeCd, int outSourceCd, int purchaseSeq)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd && item.OutsourceCd == outSourceCd && item.PurchaseSeq == purchaseSeq
                    select item).FirstOrDefault();
        }
    }
}
