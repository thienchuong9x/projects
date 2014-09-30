using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{
    [Table(Name = MasterBankColumn.TABLE_NAME)]
    public class MasterBank : BaseDomain<MasterBank>, ICommonFunctions<MasterBank>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterBank));

        private int _OfficeCd;
        private int _BankCd;
        private string _BankAccount;
        private string _AccountOwner;
        private System.Nullable<bool> _ForReceiveFlg;
        private System.Nullable<bool> _ForPayFlg;        

        #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int OfficeCd
        {
            get { return _OfficeCd; }
            set { _OfficeCd = value; }
        }

        [ColumnAttribute(Name = MasterBankColumn.BANK_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int BankCd
        {
            get { return this._BankCd; }
            set { this._BankCd = value; }
        }

        [ColumnAttribute(Name = MasterBankColumn.BANK_ACCOUNT, CanBeNull = false)]
        public string BankAccount
        {
            get { return this._BankAccount; }
            set { this._BankAccount = value; }
        }

        [ColumnAttribute(Name = MasterBankColumn.ACCOUNT_OWNER, CanBeNull = false)]
        public string AccountOwner
        {
            get { return this._AccountOwner; }
            set { this._AccountOwner = value; }
        }

        [ColumnAttribute(Name = MasterBankColumn.FOR_RECEIVE_FLG, CanBeNull = true)]
        public System.Nullable<bool> ForReceiveFlg
        {
            get { return _ForReceiveFlg; }
            set { _ForReceiveFlg = value; }
        }

        [ColumnAttribute(Name = MasterBankColumn.FOR_PAY_FLG, CanBeNull = true)]
        public System.Nullable<bool> ForPayFlg
        {
            get { return _ForPayFlg; }
            set { _ForPayFlg = value; }
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
        public MasterBank GetByPrimaryKey()
        {
            Table<MasterBank> table = GetTable();
            MasterBank item = table.Single(d => (d.BankCd == this.BankCd && d.OfficeCd == this.OfficeCd));
            if (item != null)
                item.Detach<MasterBank>();
            return item;
        }

        #endregion

        #region Method

        public static MasterBank GetBankMaster(int officeCd, int bankCd)
        {
            return  (from item in GetTable()
                         where item.OfficeCd == officeCd && item.BankCd == bankCd
                     select item).FirstOrDefault();
        }

        public static List<MasterBank> GetBankMasterSearch(int officeCd, string bankCd, string bankAccount)
        {
            return (from item in GetTable()
                         where
                           (
                              item.OfficeCd == officeCd
                           && (bankCd == string.Empty || item.BankCd.ToString().Contains(bankCd))
                           && (bankAccount == string.Empty || item.BankAccount.Contains(bankAccount))
                           )
                         select item).ToList();
        }
        public static List<MasterBank> GetBankForFlg(int officeCd, bool forPayFlg, bool forReceiveFlg)
        {
            return (from item in GetTable()
                    where
                      (
                         item.OfficeCd == officeCd &&
                         ((forPayFlg == true && item.ForPayFlg == forPayFlg) ||( forReceiveFlg == true && item.ForReceiveFlg == forReceiveFlg))
                      )
                    select item).ToList();
        }
        #endregion
    }
}
