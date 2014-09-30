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
    [Table(Name = TrnPurchaseColumn.TABLE_NAME)]
    public class TrnPurchase : BaseDomain<TrnPurchase>, ICommonFunctions<TrnPurchase>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(TrnPurchase));

        #region Private Member

        private int _OfficeCd;     
        private int _SupplierOutsourceCd;
        private int _PurchaseSeq;
        private Nullable<DateTime> _PurchaseDate;
        private Nullable<int> _PurchaseCategory;
        private string _PurchaseItems;
        private Nullable<double> _PurchasePrice;
        private Nullable<double> _RegularPrice;
        private Nullable<double> _PurchaseFee;
        private Nullable<double> _PaidMoney;
        private Nullable<double> _Balance;
        private string _Note;

        #endregion

        #region Property Member
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true)]
        public int OfficeCd
        {
            get { return _OfficeCd; }
            set { _OfficeCd = value; }
        }
        [ColumnAttribute(Name = TrnPurchaseColumn.SUPPLIER_OUTSOURCE_CD, IsPrimaryKey = true)]
        public int SupplierOutsourceCd
        {
            get { return _SupplierOutsourceCd; }
            set { _SupplierOutsourceCd = value; }
        }
        [ColumnAttribute(Name = TrnPurchaseColumn.PURCHASE_SEQ, IsPrimaryKey = true)]
        public int PurchaseSeq
        {
            get { return _PurchaseSeq; }
            set { _PurchaseSeq = value; }
        }
        [ColumnAttribute(Name = TrnPurchaseColumn.PURCHASE_DATE, CanBeNull = true)]
        public Nullable<DateTime> PurchaseDate
        {
            get { return _PurchaseDate; }
            set { _PurchaseDate = value; }
        }
        [ColumnAttribute(Name = TrnPurchaseColumn.PURCHASE_CATEGORY, CanBeNull = true)]
        public Nullable<int> PurchaseCategory
        {
            get { return _PurchaseCategory; }
            set { _PurchaseCategory = value; }
        }
        [ColumnAttribute(Name = TrnPurchaseColumn.PURCHASE_ITEMS, CanBeNull = true)]
        public string PurchaseItems
        {
            get { return _PurchaseItems; }
            set { _PurchaseItems = value; }
        }
        [ColumnAttribute(Name = TrnPurchaseColumn.PURCHASE_PRICE, CanBeNull = true)]
        public System.Nullable<double> PurchasePrice
        {
            get { return _PurchasePrice; }
            set { _PurchasePrice = value; }
        } 
        [ColumnAttribute(Name = TrnPurchaseColumn.REGULAR_PRICE, CanBeNull = true)]
        public System.Nullable<double> RegularPrice
        {
            get { return _RegularPrice; }
            set { _RegularPrice = value; }
        }

        [ColumnAttribute(Name = TrnPurchaseColumn.PURCHASE_FEE, CanBeNull = true)]
        public System.Nullable<double> PurchaseFee
        {
            get { return _PurchaseFee; }
            set { _PurchaseFee = value; }
        }
        [ColumnAttribute(Name = TrnPurchaseColumn.PAID_MONEY, CanBeNull = true)]
        public System.Nullable<double> PaidMoney
        {
            get { return _PaidMoney; }
            set { _PaidMoney = value; }
        }
        [ColumnAttribute(Name = TrnPurchaseColumn.BALANCE, CanBeNull = true)]
        public System.Nullable<double> Balance
        {
            get { return _Balance; }
            set { _Balance = value; }
        }   
        [ColumnAttribute(Name = TrnPurchaseColumn.NOTE)]
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

        #region "Method"
        public TrnPurchase GetByPrimaryKey()
        {
            Table<TrnPurchase> table = GetTable();
            TrnPurchase item = table.Single(d => (d.OfficeCd == this.OfficeCd && d.SupplierOutsourceCd == this.SupplierOutsourceCd && d.PurchaseSeq == this.PurchaseSeq));
            if (item != null)
                item.Detach<TrnPurchase>();

            return item;
        }
        public static int GetNextPurchaseSeq(int officeCd)
        {
            if (GetTable().Count() == 0)
                return 1; 
            else 
              return GetTable().Where(p => p.OfficeCd == officeCd).Max(p => p.PurchaseSeq) +1;
   
        }
        public static TrnPurchase GetTrnPurchase(int officeCd,  int outsourceCd, int purchaseSeq)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd && item.SupplierOutsourceCd == outsourceCd && item.PurchaseSeq == purchaseSeq
                    select item).FirstOrDefault();

        }
        #endregion
    }
}
