using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using log4net;

namespace Dental.Domain
{    
    [Table(Name = MasterStockColumn.TABLE_NAME)]
    [Serializable]
    public class MasterStock : BaseDomain<MasterStock>, ICommonFunctions<MasterStock>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterBank));
        private int _OfficeCd;
        private int _MaterialCd;
        private int _SupplierCd;
        private int _OutsourceLabCd;
        private string _UnitCd;
        private double _Amount;

        #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int OfficeCd
        {
            get { return this._OfficeCd; }
            set { this._OfficeCd = value; }
        }

        [ColumnAttribute(Name = MasterStockColumn.MATERIAL_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int MaterialCd
        {
            get { return this._MaterialCd; }
            set { this._MaterialCd = value; }
        }

        [ColumnAttribute(Name = MasterStockColumn.SUPPLIER_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int SupplierCd
        {
            get { return this._SupplierCd; }
            set { this._SupplierCd = value; }
        }

        [ColumnAttribute(Name = MasterStockColumn.OUTSOURCE_LAB_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int OutsourceLabCd
        {
            get { return this._OutsourceLabCd; }
            set { this._OutsourceLabCd = value; }
        } 
      
        [ColumnAttribute(Name = MasterStockColumn.UNIT_CD, IsPrimaryKey = true, CanBeNull = false)]
        public string UnitCd
        {
            get { return this._UnitCd; }
            set { this._UnitCd = value; }
        } 

        [ColumnAttribute(Name = MasterStockColumn.AMOUNT, CanBeNull = false)]
        public double Amount
        {
            get { return this._Amount; }
            set { this._Amount = value; }
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
        public MasterStock GetByPrimaryKey()
        {
            Table<MasterStock> table = GetTable();

            MasterStock item = table.Single(d => (d.MaterialCd == this.MaterialCd && d.SupplierCd == this.SupplierCd && d.OutsourceLabCd == this.OutsourceLabCd && d.UnitCd == this.UnitCd && d.OfficeCd == this.OfficeCd));
            if (item != null)
                item.Detach<MasterStock>();

            return item;
        }
        #endregion

        #region Method
        public static List<MasterStock> getStockMasters(int officeCd, int supplierCd)
        {
            return (from item in GetTable()
                    where
                      (
                         item.OfficeCd == officeCd
                         && item.SupplierCd == supplierCd
                         && item.Amount != 0
                      )
                    select item).ToList();
        }
        public static MasterStock GetStockMaster(int officeCd,int materialCd, int supplierCd , int outsourceCd ,string unitCd)
        {
            return (from item in GetTable()
                    where
                      (
                         item.OfficeCd == officeCd
                         && item.SupplierCd == supplierCd
                         && item.MaterialCd == materialCd 
                         && item.OutsourceLabCd == outsourceCd 
                         && item.UnitCd == unitCd 
                      )
                    select item).FirstOrDefault();
        }
        #endregion

        public static double? GetAmountInMstStock(int officeCd, int materialCd, int supplierCd, int outSourceLabCd, string unitCd)
        {
            MasterStock itemStock = (from item in GetTable()
                    where
                    (
                        item.OfficeCd == officeCd
                       && item.MaterialCd == materialCd
                       && item.SupplierCd == supplierCd 
                       && item.OutsourceLabCd == outSourceLabCd  
                       && item.UnitCd == unitCd 
                    )
                    select item).FirstOrDefault();
            if (itemStock == null)
                return null;
            return itemStock.Amount;

        }
        public static List<MasterStock> GetAmountStock(int officeCd, int materialCd)
        {            
            return (from item in GetTable()
                    where
                    (
                        item.OfficeCd == officeCd
                        && item.MaterialCd == materialCd
                    )
                    select item).ToList();            
            
        }

        public static List<MasterStock> DeleteMstStockWithMaterial(int officeCd, int materialCd)
        {            
            return (from item in GetTable()
                    where
                    (
                        item.OfficeCd == officeCd
                       && item.MaterialCd == materialCd
                    )
                    select item).ToList();
        }

        public static double GetMstStockWithMaterial(int officeCd, int materialCd, string unitCd)
        {
            return (from item in GetTable()
                where
                (
                    item.OfficeCd == officeCd
                    && item.MaterialCd == materialCd
                    && item.UnitCd == unitCd
                )
                select item).Sum(c=>c.Amount);	
        }
    }
}
