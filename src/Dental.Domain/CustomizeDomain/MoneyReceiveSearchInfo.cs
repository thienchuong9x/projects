using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Dental.Domain
{
    public class MoneyReceiveSearchInfo
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MoneyReceiveSearchInfo));

        #region Propertise
        public int DentalOfficeCd { get; set; }
        public string DentalOfficeNm { get; set; }
        public int OfficeCd { get; set; }
        public DateTime PayDate { get; set; }
        public Nullable<DateTime> PayDateTime { get; set; }
        public Nullable<double> Balance { get; set; }
        public int BillCd { get; set; }
        public string BillNm { get; set; }
        public Nullable<double> PayAmount { get; set; }
        public Nullable<double> SubtractFee { get; set; }
        public double SumPayAmount { get; set; }
        public string SumPayAmountCurrent { get; set; }
        public string UnchargerPay { get; set; }
        public Nullable<int> BankCd { get; set; }
        public string Note { get; set; }
        public string ReceiveBranchCd { get; set; }
        public double BillSeq { get; set; }
        public string BillStatementNo { get; set; }
        public DateTime BillIssueDate { get; set; }
        public string BillYear { get; set; }
        public string BillMonth { get; set; }
        public double PreviousSumPrice { get; set; }
        public double PreviousPaidMoney { get; set; }
        public double CarryOver { get; set; }
        public double CurrentTechPrice { get; set; }
        public double CurrentMaterialPrice { get; set; }
        public double CurrentPrice { get; set; }
        public double CurrentDiscount { get; set; }
        public double CurrentTax { get; set; }
        public double CurrentSumPrice { get; set; }
        public double CurrentBillSumPrice { get; set; }
        public double CurrentPaidMoney { get; set; }
        public Nullable<double> PaidMoney { get; set; }
        public Nullable<DateTime> LastPayDate { get; set; }
        public bool DelFlg { get; set; }
        public Nullable<int> PayCompleteFlg { get; set; }
        public int Count { get; set; }
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public string Message { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateAccount { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedAccount { get; set; }
        #endregion

        #region method
        public static List<MoneyReceiveSearchInfo> GetMoneyReciveSearch(string BillCd, string BillStatementNo, DateTime BillIssueDateFrom, DateTime BillIssueDateTo, DateTime PayDateFrom, DateTime PayDateTo, int OfficeCd, int Checked)
        {
            var db = new DBContext();
            if (Checked == 1 && BillCd == "-1")
            {
                return (from bh in db.GetTable<TrnBillHeader>()
                        join bm in db.GetTable<TrnBillMoney>()
                        on new { bh.OfficeCd, bh.BillSeq } equals new { bm.OfficeCd, bm.BillSeq }
                        into a
                        from b in a.DefaultIfEmpty()
                        where (bh.BillIssueDate >= BillIssueDateFrom && bh.BillIssueDate <= BillIssueDateTo)
                        && ((bh.LastPayDate >= PayDateFrom && bh.LastPayDate <= PayDateTo) || bh.LastPayDate == null)
                        && bh.PayCompleteFlg == 1
                        && (bh.DelFlg == false || bh.DelFlg == null)
                        && bh.OfficeCd == OfficeCd
                        orderby bh.BillIssueDate, bh.BillCd

                        select new MoneyReceiveSearchInfo
                        {
                            OfficeCd = bh.OfficeCd,
                            BillSeq = bh.BillSeq,
                            BillStatementNo = bh.BillStatementNo,
                            BillIssueDate = bh.BillIssueDate,
                            LastPayDate = bh.LastPayDate,
                            BillCd = bh.BillCd,
                            BillNm = bh.BillNm,
                            CurrentSumPrice = bh.CurrentSumPrice,
                            PayDateTime = b.PayDateTime,
                            Balance = b.Balance,
                            PaidMoney = b.PaidMoney


                        }).ToList();
            }
            if (Checked == 1 && BillCd != "-1")
            {
                return (from bh in db.GetTable<TrnBillHeader>()
                        join bm in db.GetTable<TrnBillMoney>()
                        on new { bh.OfficeCd, bh.BillSeq } equals new { bm.OfficeCd, bm.BillSeq }
                        into a
                        from b in a.DefaultIfEmpty()
                        where (bh.BillIssueDate >= BillIssueDateFrom && bh.BillIssueDate <= BillIssueDateTo)
                        && ((bh.LastPayDate >= PayDateFrom && bh.LastPayDate <= PayDateTo) || bh.LastPayDate == null)
                        && bh.PayCompleteFlg == 1
                        && (bh.DelFlg == false || bh.DelFlg == null)
                        && bh.OfficeCd == OfficeCd
                        && bh.BillCd.ToString() == BillCd
                        orderby bh.BillIssueDate, bh.BillCd

                        select new MoneyReceiveSearchInfo
                        {
                            OfficeCd = bh.OfficeCd,
                            BillSeq = bh.BillSeq,
                            BillStatementNo = bh.BillStatementNo,
                            BillIssueDate = bh.BillIssueDate,
                            LastPayDate = bh.LastPayDate,
                            BillCd = bh.BillCd,
                            BillNm = bh.BillNm,
                            CurrentSumPrice = bh.CurrentSumPrice,
                            PayDateTime = b.PayDateTime,
                            Balance = b.Balance,
                            PaidMoney = b.PaidMoney


                        }).ToList();
            }
            if (Checked == 0 && BillCd == "-1" && PayDateTo == Convert.ToDateTime("1/1/4000 23:59:59"))
            {

                return (from bh in db.GetTable<TrnBillHeader>()
                        join bm in db.GetTable<TrnBillMoney>()
                        on new { bh.OfficeCd, bh.BillSeq } equals new { bm.OfficeCd, bm.BillSeq }
                        into a
                        from b in a.DefaultIfEmpty()
                        where (bh.BillIssueDate >= BillIssueDateFrom && bh.BillIssueDate <= BillIssueDateTo)
                        && ((bh.LastPayDate >= PayDateFrom && bh.LastPayDate <= PayDateTo) || bh.LastPayDate == null)
                        && (bh.PayCompleteFlg !=1 || bh.PayCompleteFlg == null)
                        && (bh.DelFlg == false || bh.DelFlg == null)
                        && bh.OfficeCd == OfficeCd
                        orderby bh.BillIssueDate, bh.BillCd

                        select new MoneyReceiveSearchInfo
                        {
                            OfficeCd = bh.OfficeCd,
                            BillSeq = bh.BillSeq,
                            BillStatementNo = bh.BillStatementNo,
                            BillIssueDate = bh.BillIssueDate,
                            LastPayDate = bh.LastPayDate,
                            BillCd = bh.BillCd,
                            BillNm = bh.BillNm,
                            CurrentSumPrice = bh.CurrentSumPrice,
                            PayDateTime = b.PayDateTime,
                            Balance = b.Balance,
                            PaidMoney = b.PaidMoney


                        }).ToList();
            }
            if (Checked == 0 && @BillCd =="-1" && PayDateTo !=Convert.ToDateTime("1/1/4000 23:59:59"))
            {
                return (from bh in db.GetTable<TrnBillHeader>()
                        join bm in db.GetTable<TrnBillMoney>()
                        on new { bh.OfficeCd, bh.BillSeq } equals new { bm.OfficeCd, bm.BillSeq }
                        into a
                        from b in a.DefaultIfEmpty()
                        where (bh.BillIssueDate >= BillIssueDateFrom && bh.BillIssueDate <= BillIssueDateTo)
                        && ((bh.LastPayDate >= PayDateFrom && bh.LastPayDate <= PayDateTo))
                        && (bh.PayCompleteFlg != 1 || bh.PayCompleteFlg == null)
                        && (bh.DelFlg == false || bh.DelFlg == null)
                        && bh.OfficeCd == OfficeCd
                        orderby bh.BillIssueDate, bh.BillCd

                        select new MoneyReceiveSearchInfo
                        {
                            OfficeCd = bh.OfficeCd,
                            BillSeq = bh.BillSeq,
                            BillStatementNo = bh.BillStatementNo,
                            BillIssueDate = bh.BillIssueDate,
                            LastPayDate = bh.LastPayDate,
                            BillCd = bh.BillCd,
                            BillNm = bh.BillNm,
                            CurrentSumPrice = bh.CurrentSumPrice,
                            PayDateTime = b.PayDateTime,
                            Balance = b.Balance,
                            PaidMoney = b.PaidMoney


                        }).ToList();
            }

            if (Checked == 0 && @BillCd != "-1" && PayDateTo == Convert.ToDateTime("1/1/4000 23:59:59"))
            {
                return (from bh in db.GetTable<TrnBillHeader>()
                        join bm in db.GetTable<TrnBillMoney>()
                        on new { bh.OfficeCd, bh.BillSeq } equals new { bm.OfficeCd, bm.BillSeq }
                        into a
                        from b in a.DefaultIfEmpty()
                        where (bh.BillIssueDate >= BillIssueDateFrom && bh.BillIssueDate <= BillIssueDateTo)
                        && ((bh.LastPayDate >= PayDateFrom && bh.LastPayDate <= PayDateTo) || bh.LastPayDate == null)
                        && (bh.PayCompleteFlg != 1 || bh.PayCompleteFlg == null)
                        && (bh.DelFlg == false || bh.DelFlg == null)
                        && bh.OfficeCd == OfficeCd
                        && bh.BillCd.ToString() == BillCd
                        orderby bh.BillIssueDate, bh.BillCd

                        select new MoneyReceiveSearchInfo
                        {
                            OfficeCd = bh.OfficeCd,
                            BillSeq = bh.BillSeq,
                            BillStatementNo = bh.BillStatementNo,
                            BillIssueDate = bh.BillIssueDate,
                            LastPayDate = bh.LastPayDate,
                            BillCd = bh.BillCd,
                            BillNm = bh.BillNm,
                            CurrentSumPrice = bh.CurrentSumPrice,
                            PayDateTime = b.PayDateTime,
                            Balance = b.Balance,
                            PaidMoney = b.PaidMoney


                        }).ToList();
            }
            if (Checked == 0 && BillCd != "-1" && PayDateTo != Convert.ToDateTime("1/1/4000 23:59:59"))
            {
                return (from bh in db.GetTable<TrnBillHeader>()
                        join bm in db.GetTable<TrnBillMoney>()
                        on new { bh.OfficeCd, bh.BillSeq } equals new { bm.OfficeCd, bm.BillSeq }
                        into a
                        from b in a.DefaultIfEmpty()
                        where (bh.BillIssueDate >= BillIssueDateFrom && bh.BillIssueDate <= BillIssueDateTo)
                        && ((bh.LastPayDate >= PayDateFrom && bh.LastPayDate <= PayDateTo))
                        && (bh.PayCompleteFlg != 1 || bh.PayCompleteFlg == null)
                        && (bh.DelFlg == false || bh.DelFlg == null)
                        && bh.OfficeCd == OfficeCd
                        && bh.BillCd.ToString() == BillCd
                        orderby bh.BillIssueDate, bh.BillCd

                        select new MoneyReceiveSearchInfo
                        {
                            OfficeCd = bh.OfficeCd,
                            BillSeq = bh.BillSeq,
                            BillStatementNo = bh.BillStatementNo,
                            BillIssueDate = bh.BillIssueDate,
                            LastPayDate = bh.LastPayDate,
                            BillCd = bh.BillCd,
                            BillNm = bh.BillNm,
                            CurrentSumPrice = bh.CurrentSumPrice,
                            PayDateTime = b.PayDateTime,
                            Balance = b.Balance,
                            PaidMoney = b.PaidMoney


                        }).ToList();
            }
            else
            {
                return null;
            }

        }

        public static void TransactionMoneyRecieve(MoneyReceiveSearchInfo obj)
        {
            using (DBContext db = new DBContext())
            {
                using (System.Data.Common.DbTransaction tran = db.UseTransaction())
                {
                    try
                    {
                        //update trnBillHeader
                        TrnBillHeader BillHeader = new TrnBillHeader();
                        BillHeader = TrnBillHeader.GetAll().Where(t => (t.OfficeCd == obj.OfficeCd && t.BillSeq == obj.BillSeq)).FirstOrDefault();
                        BillHeader.LastPayDate = obj.PayDate;
                        BillHeader.PayCompleteFlg = obj.PayCompleteFlg;
                        BillHeader.ModifiedDate = obj.ModifiedDate;
                        BillHeader.ModifiedAccount = obj.ModifiedAccount;
                        BillHeader.Update();

                        //insert trnBillMoney
                        int Count = (from billmoney in TrnBillMoney.GetTable()
                                     where billmoney.OfficeCd == obj.OfficeCd
                                     && billmoney.BillSeq == obj.BillSeq
                                     && billmoney.PayDateTime == obj.PayDateTime
                                     && billmoney.BillCd == obj.BillCd
                                     select billmoney.OfficeCd).Count();
                        if (obj.Count == Count)
                        {
                            TrnBillMoney billMoney = new TrnBillMoney();
                            billMoney.OfficeCd = obj.OfficeCd;
                            billMoney.BillSeq = obj.BillSeq;
                            billMoney.PayDateTime = (DateTime)obj.PayDateTime;
                            billMoney.BillCd = obj.BillCd;
                            billMoney.PaidMoney = obj.SumPayAmount;
                            billMoney.Balance = obj.Balance;
                            billMoney.CreateAccount = obj.CreateAccount;
                            billMoney.Insert();
                        }
                        //update BillMoney
                        else
                        {
                            TrnBillMoney billMoney = new TrnBillMoney();
                            billMoney = TrnBillMoney.GetAll().Where(t => (t.OfficeCd == obj.OfficeCd && t.BillSeq == obj.BillSeq && t.PayDateTime == obj.PayDateTime && t.BillCd == obj.BillCd)).FirstOrDefault();
                            billMoney.PaidMoney = obj.SumPayAmount;
                            billMoney.Balance = obj.Balance;
                            billMoney.ModifiedAccount = obj.ModifiedAccount;
                            billMoney.ModifiedDate = obj.ModifiedDate;
                            billMoney.Update();
                        }

                        //insert TrnMoneyRecieveHistory
                        TrnMoneyReceiveHistory MoneyRecive = new TrnMoneyReceiveHistory();
                        MoneyRecive.OfficeCd = obj.OfficeCd;
                        MoneyRecive.PayDate = obj.PayDate;
                        MoneyRecive.BillCd = obj.BillCd;
                        MoneyRecive.BillNm = obj.BillNm;
                        MoneyRecive.PayAmount = obj.PayAmount;
                        MoneyRecive.SubtractFee = obj.SubtractFee;
                        MoneyRecive.SumPayAmount = obj.SumPayAmount;
                        MoneyRecive.BankCd = obj.BankCd;
                        MoneyRecive.Note = obj.Note;
                        MoneyRecive.CreateAccount = obj.CreateAccount;
                        MoneyRecive.Insert();

                        //check if user return paidmoney(paidmoney = 0) => delete BillMoney
                        Nullable<double> paidMoney = (from BillMoney in TrnBillMoney.GetTable()
                                                      where BillMoney.OfficeCd == obj.OfficeCd
                                                      && BillMoney.BillCd == obj.BillCd
                                                      && BillMoney.BillSeq == obj.BillSeq
                                                      && BillMoney.PayDateTime == obj.PayDateTime
                                                      select BillMoney.PaidMoney).FirstOrDefault();
                        if (paidMoney == 0)
                        {
                            //delete BillMoney
                            TrnBillMoney billMoney = new TrnBillMoney();
                            billMoney.OfficeCd = obj.OfficeCd;
                            billMoney.BillCd = obj.BillCd;
                            billMoney.BillSeq = obj.BillSeq;
                            billMoney.PayDateTime = (DateTime)obj.PayDateTime;
                            billMoney.Delete();

                            //update BillHeader
                            TrnBillHeader billheader = new TrnBillHeader();
                            billheader = TrnBillHeader.GetAll().Where(t => (t.OfficeCd == obj.OfficeCd && t.BillSeq == obj.BillSeq)).FirstOrDefault();
                            billheader.LastPayDate = null;
                            billheader.ModifiedAccount = obj.ModifiedAccount;
                            billheader.ModifiedDate = obj.ModifiedDate;
                            billheader.Update();
                        }
                       
                        tran.Commit();

                       
                       
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        logger.Error("Error Transaction", ex);
                        throw ex;
                    }
                }
            }

        }
        #endregion
    }
}
