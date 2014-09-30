using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace Dental.Domain
{
    [Serializable]
    [Table(Name = TrnBillHeaderColumn.TABLE_NAME)]
    public class TrnBillHeader : BaseDomain<TrnBillHeader>, ICommonFunctions<TrnBillHeader>
    {
        #region Private Member
        private int _OfficeCd;
        private double _BillSeq;
        private string _BillStatementNo;
        private System.DateTime _BillIssueDate;
        private string _BillYear;
        private string _BillMonth;
        private string _BillPeriod;
        private int _BillCd;
        private string _BillNm;
        private double _PreviousSumPrice;
        private double _CarryOver;
        private double _CurrentTechPrice;
        private double _CurrentMaterialPrice;
        private double _CurrentInsuredPrice;
        private double _CurrentUnInsuredPrice;
        private double _CurrentPrice;
        private double _CurrentDiscount;
        private double _CurrentTax;
        private double _CurrentSumPrice;
        private double _CurrentPaidMoney;
        private System.Nullable<System.DateTime> _LastPayDate;
        private System.Nullable<bool> _DelFlg;
        private System.Nullable<int> _PayCompleteFlg;
        private string _Note;

      

        #endregion

        #region Property Member
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true)]
        public int OfficeCd
        {
            get { return this._OfficeCd; }
            set { this._OfficeCd = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.BILL_SEQ, IsPrimaryKey = true)]
        public double BillSeq
        {
            get { return _BillSeq; }
            set { _BillSeq = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.BILL_STATEMENT_NO)]
        public string BillStatementNo
        {
            get { return _BillStatementNo; }
            set { _BillStatementNo = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.BILL_ISSUE_DATE)]
        public System.DateTime BillIssueDate
        {
            get { return _BillIssueDate; }
            set { _BillIssueDate = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.BILL_YEAR)]
        public string BillYear
        {
            get { return _BillYear; }
            set { _BillYear = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.BILL_MONTH)]
        public string BillMonth
        {
            get { return _BillMonth; }
            set { _BillMonth = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.BILL_PERIOD)]
        public string BillPeriod
        {
            get { return _BillPeriod; }
            set { _BillPeriod = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.BILL_CD)]
        public int BillCd
        {
            get { return _BillCd; }
            set { _BillCd = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.BILL_NM)]
        public string BillNm
        {
            get { return _BillNm; }
            set { _BillNm = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.PREVIOUS_SUM_PRICE)]
        public double PreviousSumPrice
        {
            get { return _PreviousSumPrice; }
            set { _PreviousSumPrice = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.CARRY_OVER)]
        public double CarryOver
        {
            get { return _CarryOver; }
            set { _CarryOver = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.CURRENT_TECH_PRICE)]
        public double CurrentTechPrice
        {
            get { return _CurrentTechPrice; }
            set { _CurrentTechPrice = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.CURRENT_MATERIAL_PRICE)]
        public double CurrentMaterialPrice
        {
            get { return _CurrentMaterialPrice; }
            set { _CurrentMaterialPrice = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.CURRENT_INSURED_PRICE)]
        public double CurrentInsuredPrice
        {
            get { return _CurrentInsuredPrice; }
            set { _CurrentInsuredPrice = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.CURRENT_UNINSURED_PRICE)]
        public double CurrentUnInsuredPrice
        {
            get { return _CurrentUnInsuredPrice; }
            set { _CurrentUnInsuredPrice = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.CURRENT_PRICE)]
        public double CurrentPrice
        {
            get { return _CurrentPrice; }
            set { _CurrentPrice = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.CURRENT_DISCOUNT)]
        public double CurrentDiscount
        {
            get { return _CurrentDiscount; }
            set { _CurrentDiscount = value; }
        }

        [ColumnAttribute(Name = TrnBillHeaderColumn.CURRENT_TAX)]
        public double CurrentTax
        {
            get { return _CurrentTax; }
            set { _CurrentTax = value; }
        }
                [ColumnAttribute(Name = TrnBillHeaderColumn.CURRENT_SUM_PRICE)]
        public double CurrentSumPrice
        {
            get { return _CurrentSumPrice; }
            set { _CurrentSumPrice = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.CURRENT_PAID_MONEY)]
        public double CurrentPaidMoney
        {
            get { return _CurrentPaidMoney; }
            set { _CurrentPaidMoney = value; }
        }
        [ColumnAttribute(Name = TrnBillHeaderColumn.LAST_PAY_DATE)]

        public System.Nullable<System.DateTime> LastPayDate
        {
            get { return _LastPayDate; }
            set { _LastPayDate = value; }
        }

        [ColumnAttribute(Name = TrnBillHeaderColumn.DELETE_FLAG)]
        public System.Nullable<bool> DelFlg
        {
            get { return _DelFlg; }
            set { _DelFlg = value; }
        }

        [ColumnAttribute(Name = TrnBillHeaderColumn.PAY_COMPLETE_FLG)]
        public System.Nullable<int> PayCompleteFlg
        {
            get { return _PayCompleteFlg; }
            set { _PayCompleteFlg = value; }
        }
         [ColumnAttribute(Name = TrnBillHeaderColumn.NOTE)]
        public string Note
        {
            get { return _Note; }
            set { _Note = value; }
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

        public TrnBillHeader GetByPrimaryKey()
        {
            Table<TrnBillHeader> table = GetTable();

            TrnBillHeader item = table.Single(d => (d.OfficeCd == this.OfficeCd && d.BillSeq == this.BillSeq));
            if (item != null)
                item.Detach<TrnBillHeader>();
            return item;
        }

        public static List<TrnBillHeader> GetListTrnBillHeaderSearch(int officeCd, string billStatementNo)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd && item.BillStatementNo == billStatementNo
                    select item).ToList();
        }

        public static void DeleteIssueBillStatementNo(int officeCd, string billStatementNo, string modifyAccount)
        {
            //Delete Detail 
            TrnOrderDetail detail = (from item in TrnOrderDetail.GetTable()
                    where item.OfficeCd == officeCd && item.BillStatementNo == billStatementNo 
                    select item).FirstOrDefault();
            if (detail != null)
            {
                detail.BillStatementNo = null;
                detail.ModifiedAccount = modifyAccount;
                detail.Update();
            }

            //Delete TrnBillHeader 
            TrnBillHeader header = (from item in TrnBillHeader.GetTable()
                                     where item.OfficeCd == officeCd && item.BillStatementNo == billStatementNo
                                     select item).FirstOrDefault();
            if (header != null)
            {
                header.DelFlg = true;
                header.ModifiedAccount = modifyAccount;
                header.Update();

                //Delete TrnBillMaterial 
                TrnBillMaterial material = (from item in TrnBillMaterial.GetTable()
                                            where item.OfficeCd == officeCd && item.BillSeq == header.BillSeq
                                            select item).FirstOrDefault();
                if (material != null)
                {
                    material.DelFlg = true;
                    material.ModifiedAccount = modifyAccount;
                    material.Update();
                }
            }
        }

        public static double GetNextBillSeq(int officeCd)
        {
            var list = GetTable().Where(p => p.OfficeCd == officeCd);
            if (list.Count() == 0)
                return 1;
            else
                return list.Max(p => p.BillSeq) + 1;
        }

        public static List<TrnBillHeader> GetLastTrnBillHeader(int officeCd, int billCd)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd && item.BillCd == billCd
                    select item).ToList();
        }
        public static bool isFirstBillCd(int officeCd, int billCd, int year) 
        {
            var list=  (from item in GetTable()
                    where item.OfficeCd == officeCd && item.BillCd == billCd && Convert.ToInt32(item.BillYear) == year 
                    select item).ToList();
            if (list.Count == 0)
                return true;
            else
                return false;
        }
    }
}
