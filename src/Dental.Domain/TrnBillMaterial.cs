using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace Dental.Domain
{
    [Serializable]
    [Table(Name = TrnBillMaterialColumn.TABLE_NAME)]
    public class TrnBillMaterial : BaseDomain<TrnBillMaterial>, ICommonFunctions<TrnBillMaterial>
    {
        #region Private Member
        private int _OfficeCd;
        private double _BillSeq;
        private int _MaterialCd;
        private string _MaterialNm;

        private double _MaterialCarryOver;
        private double _MaterialBorrow;

        private double _MaterialUsed;
        private double _MaterialStock;
        private System.Nullable<bool> _DelFlg;
        #endregion

        #region Property Member
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true)]
        public int OfficeCd
        {
            get { return this._OfficeCd; }
            set { this._OfficeCd = value; }
        }
        [ColumnAttribute(Name = TrnBillMaterialColumn.BILL_SEQ, IsPrimaryKey = true)]
        public double BillSeq
        {
            get { return _BillSeq; }
            set { _BillSeq = value; }
        }
        [ColumnAttribute(Name = TrnBillMaterialColumn.MATERIAL_CD, IsPrimaryKey = true)]
        public int MaterialCd
        {
            get { return _MaterialCd; }
            set { _MaterialCd = value; }
        }
        [ColumnAttribute(Name = TrnBillMaterialColumn.MATERIAL_NM)]
        public string MaterialNm
        {
            get { return _MaterialNm; }
            set { _MaterialNm = value; }
        }
        [ColumnAttribute(Name = TrnBillMaterialColumn.MATERIAL_CARRY_OVER)]
        public double MaterialCarryOver
        {
            get { return _MaterialCarryOver; }
            set { _MaterialCarryOver = value; }
        }
        [ColumnAttribute(Name = TrnBillMaterialColumn.MATERIAL_BORROW)]
        public double MaterialBorrow
        {
            get { return _MaterialBorrow; }
            set { _MaterialBorrow = value; }
        }
          [ColumnAttribute(Name = TrnBillMaterialColumn.MATERIAL_USED)]
        public double MaterialUsed
        {
            get { return _MaterialUsed; }
            set { _MaterialUsed = value; }
        }
        [ColumnAttribute(Name = TrnBillMaterialColumn.MATERIAL_STOCK)]
        public double MaterialStock
        {
            get { return _MaterialStock; }
            set { _MaterialStock = value; }
        }
         [ColumnAttribute(Name = TrnBillMaterialColumn.DEL_FLG)]
          public System.Nullable<bool> DelFlg
          {
              get { return _DelFlg; }
              set { _DelFlg = value; }
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

        public TrnBillMaterial GetByPrimaryKey()
        {
             Table<TrnBillMaterial> table = GetTable();

             TrnBillMaterial item = table.Single(d => (d.OfficeCd == this.OfficeCd && d.BillSeq == this.BillSeq && d.MaterialCd == this.MaterialCd));
             if (item != null)
                 item.Detach<TrnBillMaterial>();
             return item;
        }

        public static List<TrnBillMaterial> GetListTrnBillMaterialSearch(int officeCd, double billSeq)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd
                        && item.BillSeq == billSeq
                    select item).ToList();
        }

        public static double GetMaterialStock(int officeCd, int materialCd, int billCd)
        {
            var context = new DBContext();

            var firstMaterial =
                   (from item in context.GetTable<TrnBillMaterial>()
                    join
                       header in context.GetTable<TrnBillHeader>() on new { item.OfficeCd, item.BillSeq } equals new { header.OfficeCd, header.BillSeq }
                    orderby item.BillSeq descending 
                    where
                    (
                           item.OfficeCd == officeCd
                         && item.MaterialCd == materialCd
                         && header.BillCd == billCd 
                         && (item.DelFlg == null || item.DelFlg.Value == false)
                         && (header.DelFlg == null || header.DelFlg.Value == false)
                      )
                    select item).FirstOrDefault();
            if (firstMaterial == null)
                return 0;
            else
                return firstMaterial.MaterialStock;
            
        }
    }
}
