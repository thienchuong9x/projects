using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{    
    [Table(Name = MasterItemColumn.TABLE_NAME)]
    public class MasterItem : BaseDomain<MasterItem>, ICommonFunctions<MasterItem>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterItem));
        
        private string _ItemCathegory;
        private int _ItemNo;
        private string _ItemNm;
        private string _ItemValue;
        private Nullable<int> _ViewOrder;
        private bool _IsDeleted;      

        #region Property       
        [ColumnAttribute(Name = MasterItemColumn.ITEM_CATHEGORY, IsPrimaryKey = true, CanBeNull = false)]
        public string ItemCathegory
        {
            get { return this._ItemCathegory; }
            set { this._ItemCathegory = value; }
        }

        [ColumnAttribute(Name = MasterItemColumn.ITEM_NO, IsPrimaryKey = true, CanBeNull = false)]
        public int ItemNo
        {
            get { return this._ItemNo; }
            set { this._ItemNo = value; }
        }

        [ColumnAttribute(Name = MasterItemColumn.ITEM_NM, CanBeNull = true)]
        public string ItemNm
        {
            get { return this._ItemNm; }
            set { this._ItemNm = value; }
        }

        [ColumnAttribute(Name = MasterItemColumn.ITEM_VALUE, CanBeNull = true)]
        public string ItemValue
        {
            get { return this._ItemValue; }
            set { this._ItemValue = value; }
        }

        [ColumnAttribute(Name = MasterItemColumn.VIEW_ORDER, CanBeNull = true)]
        public Nullable<int> ViewOrder
        {
            get { return this._ViewOrder; }
            set { this._ViewOrder = value; }
        }

        [ColumnAttribute(Name = MasterItemColumn.IS_DELETED, CanBeNull = false)]
        public bool IsDeleted
        {
            get { return this._IsDeleted; }
            set { this._IsDeleted = value; }
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
        public MasterItem GetByPrimaryKey()
        {
            Table<MasterItem> table = GetTable();

            MasterItem item = table.Single(d => (d.ItemCathegory == this.ItemCathegory && d.ItemNo == this.ItemNo));
            if (item != null)
                item.Detach<MasterItem>();
            return item;
        }

        #endregion

        #region Method

        public static MasterItem GetItemMaster(string itemCathegory, int itemNo)
        {
            return (from item in GetTable()
                    where item.ItemCathegory == itemCathegory && item.ItemNo == itemNo
                    select item).FirstOrDefault();
        }
        #endregion

        public static List<MasterItem> GetItemMasterSearch(string itemCathegory, string itemName)
        {
            return (from item in GetTable()
                    where item.ItemCathegory.Contains(itemCathegory) && item.ItemNm.Contains(itemName)
                    select item).ToList();
        }

    }
}
