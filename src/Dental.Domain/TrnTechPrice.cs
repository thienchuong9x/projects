using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;
using Dental.Utilities;

namespace Dental.Domain
{
    
    [Serializable]
    [Table(Name = TrnTechPriceColumn.TABLE_NAME)]
    public class TrnTechPrice : BaseDomain<TrnTechPrice>, ICommonFunctions<TrnTechPrice>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(TrnTechPrice));

        #region Private Member

        private int _OfficeCd;
        private double _OrderSeq;
        private int _DetailSeq;
        private int _TechDetailNo;       
        private Nullable<int> _TechCd;
        private string _TechNm;
        private Nullable<double> _TechPrice;


        public TrnTechPrice(int id, string TechCd_2, string TechNm_2, string TechPrice_2)
        {
            // TODO: Complete member initialization
            this.TechDetailNo   = id;
            this.TechCd = Common.GetNullableInt(TechCd_2);
            this.TechNm = TechNm_2;
            this.TechPrice = Common.GetNullableDouble(TechPrice_2); ;
        }

        public TrnTechPrice()
        {
            // TODO: Complete member initialization
        }

        

        #endregion

        #region Property Member
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true)]
        public int OfficeCd
        {
            get { return _OfficeCd; }
            set { _OfficeCd = value; }
        }
        [ColumnAttribute(Name = TrnTechPriceColumn.ORDER_SEQ, IsPrimaryKey = true)]
        public double OrderSeq
        {
            get { return _OrderSeq; }
            set { _OrderSeq = value; }
        }
        [ColumnAttribute(Name = TrnTechPriceColumn.DETAIL_SEQ, IsPrimaryKey = true)]
        public int DetailSeq
        {
            get { return _DetailSeq; }
            set { _DetailSeq = value; }
        }
        [ColumnAttribute(Name = TrnTechPriceColumn.TECH_DETAIL_NO, IsPrimaryKey = true)]
        public int TechDetailNo
        {
            get { return _TechDetailNo; }
            set { _TechDetailNo = value; }
        }
        [ColumnAttribute(Name = TrnTechPriceColumn.TECH_CD, CanBeNull = true)]
        public Nullable<int> TechCd
        {
            get { return _TechCd; }
            set { _TechCd = value; }
        }
        [ColumnAttribute(Name = TrnTechPriceColumn.TECH_NM, CanBeNull = true)]
        public string TechNm
        {
            get { return _TechNm; }
            set { _TechNm = value; }
        }
        [ColumnAttribute(Name = TrnTechPriceColumn.TECH_PRICE, CanBeNull = true)]
        public System.Nullable<double> TechPrice
        {
            get { return _TechPrice; }
            set { _TechPrice = value; }
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

        public TrnTechPrice GetByPrimaryKey()
        {
            Table<TrnTechPrice> table = GetTable();
            TrnTechPrice item = table.Single(d => (d.OfficeCd == this.OfficeCd && d.OrderSeq == this.OrderSeq && d.DetailSeq == this.DetailSeq && d.TechDetailNo == this.TechDetailNo));
            if (item != null)
                item.Detach<TrnTechPrice>();

            return item;
        }
        #endregion

        public static List<TrnTechPrice> GetTrnTechPriceList(int officeCd, double orderSeq)
        {
            logger.Debug("Begin GetListDentalOrderDetail , OrderSeq = " + orderSeq);
            var result = from item in GetTable()
                         where item.OrderSeq == orderSeq && item.OfficeCd == officeCd
                         select item;
            return result.ToList();
        }
    }
}
