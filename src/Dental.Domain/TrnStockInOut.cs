using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using Dental.Utilities;

namespace Dental.Domain
{
    [Table(Name = TrnStockInOutColumn.TABLE_NAME)]
    public class TrnStockInOut : BaseDomain<TrnStockInOut>, ICommonFunctions<TrnStockInOut>
    {
        private int _OfficeCd;
        private System.Nullable<double> _OrderSeq;
        private System.Nullable<int> _DetailSeq;
        private System.DateTime _RegisterDate;
        private int _InOutKbn;
        private System.Nullable<int> _SupplierCd;
        private System.Nullable<int> _OutsourceLabCd;
        private int _MaterialCd;
        private double _Amount;
        private string _UnitCd;
        private System.Nullable<double> _Price;
        private System.Nullable<double> _SumPrice;
        private string _Comment;

        public string MaterialStock { get; set; }
        public string MaterialNm { get; set; } 

        #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD)]
        public int OfficeCd
        {
            get { return this._OfficeCd; }
            set { this._OfficeCd = value; }
        }
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.ORDER_SEQ, CanBeNull = true)]
        public System.Nullable<double> OrderSeq
        {
            get { return this._OrderSeq; }
            set { this._OrderSeq = value; }
        }
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.DETAIL_SEQ, CanBeNull = true)]
        public System.Nullable<int> DetailSeq
        {
            get { return this._DetailSeq; }
            set { this._DetailSeq = value; }
        }
        [ColumnAttribute(Name = TrnStockInOutColumn.REGISTER_DATE)]
        public System.DateTime RegisterDate
        {
            get { return _RegisterDate; }
            set { _RegisterDate = value; }
        }
        [ColumnAttribute(Name = TrnStockInOutColumn.IN_OUT_KBN)]
        public int InOutKbn
        {
            get { return this._InOutKbn; }
            set { this._InOutKbn = value; }
        }

        [ColumnAttribute(Name = TrnStockInOutColumn.SUPPLIER_CD)]
        public System.Nullable<int> SupplierCd
        {
            get { return this._SupplierCd; }
            set { this._SupplierCd = value; }
        }

        [ColumnAttribute(Name = TrnStockInOutColumn.OUTSOURCE_LAB_CD)]
        public System.Nullable<int> OutsourceLabCd
        {
            get
            {
                return this._OutsourceLabCd;
            }
            set
            {
                this._OutsourceLabCd = value;
            }
        }

        [ColumnAttribute(Name = TrnStockInOutColumn.MATERIAL_CD)]
        public int MaterialCd
        {
            get
            {
                return this._MaterialCd;
            }
            set
            {
                this._MaterialCd = value;
            }
        }

        [ColumnAttribute(Name = TrnStockInOutColumn.AMOUNT)]
        public double Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        [ColumnAttribute(Name = TrnStockInOutColumn.UNIT_CD)]
        public string UnitCd
        {
            get
            {
                return this._UnitCd;
            }
            set
            {
                this._UnitCd = value;
            }
        }

        [ColumnAttribute(Name = TrnStockInOutColumn.PRICE)]
        public System.Nullable<double> Price
        {
            get { return this._Price; }
            set { this._Price = value; }
        }

        [ColumnAttribute(Name = TrnStockInOutColumn.SUM_PRICE)]
        public System.Nullable<double> SumPrice
        {
            get { return this._SumPrice; }
            set { this._SumPrice = value; }
        }
        [ColumnAttribute(Name = TrnStockInOutColumn.COMMENT)]
        public string Comment
        {
            get { return _Comment; }
            set { _Comment = value; }
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
        public TrnStockInOut GetByPrimaryKey()
        {
            Table<TrnStockInOut> table = GetTable();

            TrnStockInOut item = table.Single(d => (d.OfficeCd == this.OfficeCd));
            if (item != null)
                item.Detach<TrnStockInOut>();

            return item;
        }
        #endregion

        public static List<TrnStockInOut> GetListBorrowedMaterialStockInOut(int officeCd, int supplierCd, int billCd, DateTime? RegisterDateFrom, DateTime RegisterDateTo)
        {
            List<TrnStockInOut> list = new List<TrnStockInOut>();
            DateTime dateFrom = RegisterDateFrom == null ? DateTime.MinValue : RegisterDateFrom.Value;
 

            var context = new DBContext();

            list = (from item in context.GetTable<TrnStockInOut>()
                    join
                        mMaterial in context.GetTable<MasterMaterial>() on new { item.OfficeCd, item.MaterialCd } equals new { mMaterial.OfficeCd, mMaterial.MaterialCd }
                    where
                    (
                           item.OfficeCd == officeCd
                         && item.SupplierCd == supplierCd
                         && (item.InOutKbn == 12 || item.InOutKbn == 23)
                         && item.RegisterDate.Date <= RegisterDateTo.Date
                         && (dateFrom == DateTime.MinValue || item.RegisterDate.Date >= dateFrom.Date)
                    )
                    select new TrnStockInOut()
                    {
                        InOutKbn = item.InOutKbn , 
                        Amount = item.Amount,
                        MaterialCd = item.MaterialCd ,
                        MaterialNm = mMaterial.MaterialNm,
                        MaterialStock = null
                    }
                    ).ToList();

            return list;
        }

        public static void InsertWithoutKey(DBContext dbc,  TrnStockInOut stockInOut)
        {
            List<object> list = new List<object>();
            list.Add(stockInOut.OfficeCd);
            list.Add(stockInOut.OrderSeq);
            list.Add(stockInOut.DetailSeq);
            list.Add(stockInOut.RegisterDate);
            list.Add(stockInOut.InOutKbn);
            list.Add(stockInOut.SupplierCd);
            list.Add(stockInOut.OutsourceLabCd);
            list.Add(stockInOut.MaterialCd);
            list.Add(stockInOut.Amount);
            list.Add(stockInOut.UnitCd);
            list.Add(stockInOut.Price);
            list.Add(stockInOut.SumPrice);
            list.Add(stockInOut.Comment);
            list.Add(stockInOut.CreateDate);
            list.Add(stockInOut.CreateAccount);
            list.Add(stockInOut.ModifiedDate);
            list.Add(stockInOut.ModifiedAccount);

            if(dbc == null) 
              dbc = new DBContext();
            StringBuilder query = new StringBuilder();
            int i = 0;
            query.Append("insert into  DENTAL_TrnStockInOut values (");
            foreach (var p in list.ToArray())
            {
                if (p == null)
                {
                    query.Append("null,");
                    list.RemoveAt(i);
                }
                else
                {
                    query.Append("{"+i+"},");
                    i++;
                }
            }
            query.Remove(query.Length - 1, 1);
            query.Append(")");
            
            dbc.ExecuteCommand(query.ToString(), list.ToArray());

        }

        public static void UpdateWithoutKey(DBContext dbc, TrnStockInOut stockInOut)
        {
            if (dbc == null)
                dbc = new DBContext();

            StringBuilder query = new StringBuilder();
            query.Append("Update DENTAL_TrnStockInOut set ");
            query.Append("OrderSeq={0},");
            query.Append("DetailSeq={1},");
            query.Append("RegisterDate={2},");
            query.Append("InOutKbn={3},");
            query.Append("Amount={4},");
            query.Append("Price={5},");
            query.Append("SumPrice={6},");
            query.Append("Comment={7},");
            query.Append("ModifiedDate={8},");
            query.Append("ModifiedAccount={9}");
            query.Append(" ");
            query.Append("where OfficeCd={10} and MaterialCd={11} and SupplierCd={12} and OutsourceLabCd={13} and UnitCd={14}");
            dbc.ExecuteCommand(query.ToString(), stockInOut.OrderSeq, stockInOut.DetailSeq, stockInOut.RegisterDate, stockInOut.InOutKbn, stockInOut.Amount,stockInOut.Price,stockInOut.SumPrice,stockInOut.Comment,stockInOut.ModifiedDate,stockInOut.ModifiedAccount,stockInOut.OfficeCd,stockInOut.MaterialCd,stockInOut.SupplierCd,stockInOut.OutsourceLabCd,stockInOut.UnitCd);
            
        }
        
    }
}
