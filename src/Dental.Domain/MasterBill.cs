using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{
    [Table(Name = MasterBillColumn.TABLE_NAME)]
    public class MasterBill : BaseDomain<MasterBill>, ICommonFunctions<MasterBill>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterBill));

        private int _OfficeCd;
        private int _BillCd;
        private string _BillNm;
        private string _BillStatementNm;        
        private string _BillPostalCd;
        private string _BillAddress1;
        private string _BillAddress2;
        private string _BillTEL;
        private string _BillFAX;
        private string _BillContactPerson;        
        private System.Nullable<double> _CreditLimit;
        private System.Nullable<int> _BillFlg;
        private int _BillClosingDay;
        private System.Nullable<int> _BankCd;
        private System.Nullable<int> _SupplierCd;

        #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int OfficeCd
        {
            get { return _OfficeCd; }
            set { _OfficeCd = value; }
        }

        [ColumnAttribute(Name = MasterBillColumn.BILL_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int BillCd
        {
            get { return this._BillCd; }
            set { this._BillCd = value; }
        }

        [ColumnAttribute(Name = MasterBillColumn.BILL_NM, CanBeNull = false)]
        public string BillNm
        {
            get { return this._BillNm; }
            set { this._BillNm = value; }
        }

        [ColumnAttribute(Name = MasterBillColumn.BILL_STATEMENT_NM, CanBeNull = true)]
        public string BillStatementNm
        {
            get { return this._BillStatementNm; }
            set { this._BillStatementNm = value; }
        }

        [ColumnAttribute(Name = MasterBillColumn.BILL_POSTAL_CODE)]
        public string BillPostalCd
        {
            get { return this._BillPostalCd; }
            set { this._BillPostalCd = value; }
        }

        [ColumnAttribute(Name = MasterBillColumn.BILL_ADDRESS1)]
        public string BillAddress1
        {
            get { return this._BillAddress1; }
            set { this._BillAddress1 = value; }
        }

        [ColumnAttribute(Name = MasterBillColumn.BILL_ADDRESS2)]
        public string BillAddress2
        {
            get { return this._BillAddress2; }
            set { this._BillAddress2 = value; }
        }

        [ColumnAttribute(Name = MasterBillColumn.BILL_TEL)]
        public string BillTEL
        {
            get { return this._BillTEL; }
            set { this._BillTEL = value; }
        }

        [ColumnAttribute(Name = MasterBillColumn.BILL_FAX)]
        public string BillFAX
        {
            get { return this._BillFAX; }
            set { this._BillFAX = value; }
        }

        [ColumnAttribute(Name = MasterBillColumn.BILL_CONTACTPERSON)]
        public string BillContactPerson
        {
            get { return this._BillContactPerson; }
            set { this._BillContactPerson = value; }
        }

        [ColumnAttribute(Name = MasterBillColumn.CREDIT_LIMIT, CanBeNull = true)]
        public System.Nullable<double> CreditLimit
        {
            get { return this._CreditLimit; }
            set { this._CreditLimit = value; }
        }

        [ColumnAttribute(Name = MasterBillColumn.BILL_FLG, CanBeNull = true)]
        public System.Nullable<int> BillFlg
        {
            get { return _BillFlg; }
            set { _BillFlg = value; }
        }

        [ColumnAttribute(Name = MasterBillColumn.BILL_CLOSING_DAY)]
        public int BillClosingDay
        {
            get { return _BillClosingDay; }
            set { _BillClosingDay = value; }
        }

        [ColumnAttribute(Name = MasterBillColumn.BANK_CD, CanBeNull = true)]
        public System.Nullable<int> BankCd
        {
            get { return _BankCd; }
            set { _BankCd = value; }
        }

        [ColumnAttribute(Name = MasterBillColumn.SUPPLIER_CD, CanBeNull = true)]
        public System.Nullable<int> SupplierCd
        {
            get { return _SupplierCd; }
            set { _SupplierCd = value; }
        }

        [ColumnAttribute(Name = BaseColumn.CREATE_DATE)]
        public System.DateTime CreateDate
        {
            get  {  return this._CreateDate; }
            set { this._CreateDate = value;  }
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
            get {  return this._ModifiedDate; }
            set {    this._ModifiedDate = value;}
        }

        [ColumnAttribute(Name = BaseColumn.MODIFIED_ACCOUNT)]
        public string ModifiedAccount
        {
            get { return this._ModifiedAccount; }
            set  {    this._ModifiedAccount = value; }
        }
        #endregion

        #region ICommonFunction
        public MasterBill GetByPrimaryKey()
        {
            Table<MasterBill> table = GetTable();

            MasterBill item = table.Single(d => (d.BillCd == this.BillCd && d.OfficeCd == this.OfficeCd));
            if (item != null)
                item.Detach<MasterBill>();
            return item;
        }
        #endregion

        #region Method

        public static MasterBill GetBillMaster(int officeCd, int billCd)
        {
            return ( from item in GetTable()
                         where item.OfficeCd == officeCd && item.BillCd == billCd
                         select item).FirstOrDefault();  
        }
        public static List<MasterBill> GetBillMasters(int officeCd)
        {
            return (from item in GetTable()
                    where
                      (
                         item.OfficeCd == officeCd
                      )
                    select item).ToList();
        }

        public static List<MasterBill> GetBillMasterSearch(int officeCd, string billCd, string billName)
        {
            return  (from item in GetTable()
                         where
                           (
                              item.OfficeCd == officeCd
                           && (billCd == string.Empty || item.BillCd.ToString().Contains(billCd))
                           && (billName == string.Empty || item.BillNm.Contains(billName))                           
                           )
                         select item).ToList();
        }
        #endregion                
    }
}
