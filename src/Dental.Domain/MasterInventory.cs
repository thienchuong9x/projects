using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{
    [Table(Name = MasterInventoryColumn.TABLE_NAME)]
    public class MasterInventory : BaseDomain<MasterInventory>, ICommonFunctions<MasterInventory>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterInventory));

        private int _MaterialCd;
        private int _SupplierCd;
        private int _Amount;
        private int _AmountUnitCd;
        private int _Fraction;
        private int _FractionUnitCd;      

        #region Property       
        [ColumnAttribute(Name = MasterInventoryColumn.MATERIAL_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int MaterialCd
        {
            get { return this._MaterialCd; }
            set { this._MaterialCd = value; }
        }

        [ColumnAttribute(Name = MasterInventoryColumn.SUPPLIER_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int SupplierCd
        {
            get { return this._SupplierCd; }
            set { this._SupplierCd = value; }
        }

        [ColumnAttribute(Name = MasterInventoryColumn.AMOUNT, CanBeNull = false)]
        public int Amount
        {
            get { return this._Amount; }
            set { this._Amount = value; }
        }

        [ColumnAttribute(Name = MasterInventoryColumn.AMOUNT_UNIT_CD, CanBeNull = false)]
        public int AmountUnitCd
        {
            get { return this._AmountUnitCd; }
            set { this._AmountUnitCd = value; }
        }

        [ColumnAttribute(Name = MasterInventoryColumn.FRACTION, CanBeNull = false)]
        public int Fraction
        {
            get { return this._Fraction; }
            set { this._Fraction = value; }
        }        

        [ColumnAttribute(Name = MasterInventoryColumn.FRACTION_UNIT_CD, CanBeNull = false)]
        public int FractionUnitCd
        {
            get { return this._FractionUnitCd; }
            set { this._FractionUnitCd = value; }
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
        public MasterInventory GetByPrimaryKey()
        {
            Table<MasterInventory> table = GetTable();

            MasterInventory item = table.Single(d => (d.MaterialCd == this.MaterialCd && d.SupplierCd == this.SupplierCd));
            if (item != null)
                item.Detach<MasterInventory>();
            return item;
        }

        #endregion

        #region Method

        public static MasterInventory GetInventoryMaster(int materialCd, int supplierCd)
        {
            return (from item in GetTable()
                    where item.MaterialCd == materialCd && item.SupplierCd == supplierCd
                    select item).FirstOrDefault();
        }
        #endregion

    }
}
