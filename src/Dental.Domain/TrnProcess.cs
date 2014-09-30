using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace Dental.Domain
{
    [Serializable]
    [Table(Name = TrnProcessColumn.TABLE_NAME)]
    public class TrnProcess : BaseDomain<TrnProcess>, ICommonFunctions<TrnProcess>
    {
        private int _OfficeCd;
        private double _OrderSeq;
        private int _DetailSeq;

       
        private int _ProcessNo;
        private int _ProcessCd;
        private System.Nullable<int> _StaffCd;
        private System.Nullable<int> _ProcessTime;

        #region Property 
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true)]
        public int OfficeCd
        {
            get { return this._OfficeCd; }
            set { this._OfficeCd = value; }
        }
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.ORDER_SEQ, CanBeNull = false, IsPrimaryKey = true)]
        public double OrderSeq
        {
            get { return this._OrderSeq; }
            set { this._OrderSeq = value; }
        }
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.DETAIL_SEQ , CanBeNull = false, IsPrimaryKey = true)]
        public int DetailSeq
        {
            get { return _DetailSeq; }
            set { _DetailSeq = value; }
        }

        [ColumnAttribute(Name = TrnProcessColumn.PROCESS_NO, IsPrimaryKey = true)]
        public int ProcessNo
        {
            get
            {
                return this._ProcessNo;
            }
            set
            {
                this._ProcessNo = value;
            }
        }

        [ColumnAttribute(Name = TrnProcessColumn.PROCESS_CD)]
        public int ProcessCd
        {
            get
            {
                return this._ProcessCd;
            }
            set
            {
                this._ProcessCd = value;
            }
        }

        [ColumnAttribute(Name = TrnProcessColumn.STAFF_CD)]
        public System.Nullable<int> StaffCd
        {
            get
            {
                return this._StaffCd;
            }
            set
            {
                this._StaffCd = value;
            }
        }

        [ColumnAttribute(Name = TrnProcessColumn.WORKTIME)]
        public System.Nullable<int> ProcessTime
        {
            get
            {
                return this._ProcessTime;
            }
            set
            {
                this._ProcessTime = value;
            }
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
        public TrnProcess GetByPrimaryKey()
        {
            Table<TrnProcess> table = GetTable();

            TrnProcess item = table.Single(d => (d.OfficeCd == this.OfficeCd && d.OrderSeq == this.OrderSeq && d.DetailSeq  == this.DetailSeq && d.ProcessNo == this.ProcessNo));
            if (item != null)
                item.Detach<TrnProcess>();

            return item;
        }
        #endregion 

        #region Method
        #endregion 

  
        public static TrnProcess GetTrnProcess(int officeCd, double orderSeq,int detailSeq,int processNo)
        {  
            return (from item in GetTable()
                    where item.OfficeCd == officeCd && item.OrderSeq == orderSeq && item.DetailSeq == detailSeq && item.ProcessNo == processNo 
                    select item).FirstOrDefault();  

        }
        public static List<TrnProcess> GetTrnProcessList(int officeCd, double orderSeq)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd && item.OrderSeq == orderSeq 
                    select item).ToList(); 
        }
    }
}
