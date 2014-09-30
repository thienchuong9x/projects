using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{    
    [Table(Name = MasterMaterialPriceColumn.TABLE_NAME)]
    [Serializable]
    public class MasterMaterialPrice : BaseDomain<MasterMaterialPrice>, ICommonFunctions<MasterMaterialPrice>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterMaterialPrice));

        private int _OfficeCd;
        private int _MaterialCd;
        private int _SupplierCd;        
        private DateTime _StartDate;
        private Nullable<DateTime> _EndDate;
        private string _PriceNm;
        private int _Price;      

        #region Property      
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int OfficeCd
        {
            get { return _OfficeCd; }
            set { _OfficeCd = value; }
        }

        [ColumnAttribute(Name = MasterMaterialPriceColumn.MATERIAL_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int MaterialCd
        {
            get { return this._MaterialCd; }
            set { this._MaterialCd = value; }
        }

        [ColumnAttribute(Name = MasterMaterialPriceColumn.SUPPLIER_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int SupplierCd
        {
            get { return this._SupplierCd; }
            set { this._SupplierCd = value; }
        }

        [ColumnAttribute(Name = MasterMaterialPriceColumn.START_DATE, IsPrimaryKey = true, CanBeNull = false)]
        public DateTime StartDate
        {
            get { return this._StartDate; }
            set { this._StartDate = value; }
        }

        [ColumnAttribute(Name = MasterMaterialPriceColumn.END_DATE, CanBeNull = true)]
        public Nullable<DateTime> EndDate
        {
            get { return this._EndDate; }
            set { this._EndDate = value; }
        }

        [ColumnAttribute(Name = MasterMaterialPriceColumn.PRICE_NM, CanBeNull = true)]
        public string PriceNm
        {
            get { return this._PriceNm; }
            set { this._PriceNm = value; }
        }

        [ColumnAttribute(Name = MasterMaterialPriceColumn.PRICE, CanBeNull = false)]
        public int Price
        {
            get { return this._Price; }
            set { this._Price = value; }
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
        public MasterMaterialPrice GetByPrimaryKey()
        {
            Table<MasterMaterialPrice> table = GetTable();

            MasterMaterialPrice item = table.Single(d => (d.MaterialCd == this.MaterialCd && d.SupplierCd == this.SupplierCd && d.StartDate == this.StartDate && d.OfficeCd == this.OfficeCd));
            if (item != null)
                item.Detach<MasterMaterialPrice>();
            return item;
        }

        #endregion

        #region Method

        public static MasterMaterialPrice GetBillStatementNo(int materialCd, int supplierCd, DateTime startDate, int officeCd)
        {
            return (from item in GetTable()
                    where item.MaterialCd == materialCd && item.SupplierCd == supplierCd && item.StartDate == startDate && item.OfficeCd == officeCd
                    select item).FirstOrDefault();
        }

        public static List<MasterMaterialPrice> DeleteMaterialPriceByMaterial(int materialCd, int officeCd)
        {
            return (from item in GetTable()
                    where item.MaterialCd == materialCd &&  item.OfficeCd == officeCd
                    select item).ToList();           
        }

        public static List<MasterMaterialPrice> GetMaterialPriceByMaterialCd(int _all, int officeCd, int materialCd)
        {
            return (from item in GetTable()
                    where item.MaterialCd == materialCd 
                          && item.OfficeCd == officeCd
                          && (_all == 1 || (item.EndDate >= DateTime.Now.Date || item.EndDate == null))
                    select item).ToList();  
        }

        public static MasterMaterialPrice GetMaterialPrice(int officeCd, int materialCd, string startDate, int supplierCd)
        {
            return (from item in GetTable()
                    where item.MaterialCd == materialCd && item.SupplierCd == supplierCd && item.StartDate.Date == Convert.ToDateTime(startDate).Date && item.OfficeCd == officeCd
                    select item).FirstOrDefault();
        }

        #endregion  
    }
}
