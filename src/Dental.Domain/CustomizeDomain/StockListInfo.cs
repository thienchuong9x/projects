using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
namespace Dental.Domain
{
    public class StockListInfo
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(StockListInfo));
        #region Propertise
        public int MaterialCd
        {
            get;
            set;
        }
        public string MaterialNm
        {
            get;
            set;
        }
        public string Maker
        {
            get;
            set;
        }
        public string Unit
        {
            get;
            set;
        }
        public string Lender
        {
            get;
            set;
        }
        public string Borrower
        {
            get;
            set;
        }
        public Nullable<double> Qty
        {
            get;
            set;
        }
        public Nullable<double> BorrowedQty
        {
            get;
            set;
        }
        public Nullable<double> LendedQty
        {
            get;
            set;
        }
        public int SupplierCd { get; set; }
        public int OutsourceCd { get; set; }
        #endregion

        #region Method
        

        public static List<StockListInfo> GetStockListSeach(int OfficeCd, int fromMaterialCd, int toMaterialCd, int fromSupplierCd, int toSupplierCd, int fromOutSourceCd, int toOutsourceCd)
        {
            var dbc = new DBContext();
            return (from s in dbc.GetTable<MasterStock>()
                    from m in dbc.GetTable<MasterMaterial>()
                    where s.MaterialCd == m.MaterialCd
                    && s.OfficeCd == m.OfficeCd
                    && s.MaterialCd >= fromMaterialCd && s.MaterialCd <= toMaterialCd
                    && s.OfficeCd == OfficeCd
                    
                    orderby s.MaterialCd
                    select new StockListInfo
                    {
                        MaterialCd = s.MaterialCd,
                        MaterialNm = m.MaterialNm,
                        Maker = m.ProductMaker,
                        Unit = (from u in dbc.GetTable<MasterUnit>()
                                where s.MaterialCd == u.MaterialCd && s.UnitCd == u.UnitCd
                                && s.MaterialCd >= fromMaterialCd && s.MaterialCd <= toMaterialCd
                                && u.OfficeCd == OfficeCd
                                select u.UnitNm).FirstOrDefault(),

                        Qty = s.Amount,

                        Borrower = (from o in dbc.GetTable<MasterOutsourceLab>()
                                    where s.OutsourceLabCd == o.OutsourceCd
                                        && o.OutsourceCd != 0 && o.OfficeCd == OfficeCd
                                        && s.OutsourceLabCd >= fromOutSourceCd && s.OutsourceLabCd <= toOutsourceCd
                                    select o.OutsourceNm).FirstOrDefault(),

                        BorrowedQty = s.Amount,

                        Lender = (from l in dbc.GetTable<MasterSupplier>()
                                  where s.SupplierCd == l.SupplierCd && l.SupplierCd != 0 && l.OfficeCd == OfficeCd
                                  && s.SupplierCd >= fromSupplierCd && s.SupplierCd <= toSupplierCd
                                  select l.SupplierNm).FirstOrDefault(),

                        LendedQty = s.Amount,
                        SupplierCd = s.SupplierCd,
                        OutsourceCd = s.OutsourceLabCd
                    }

                ).Distinct().ToList();
        }

        public static Nullable<int> CheckAvailable(int OfficeCd, int MaterialCd, int Condition, int SupplierCd, int OutsourceCd, string UnitCd)
        {
            Nullable<int> result = null;

            if (Condition == 1)//check MaterialCd if available in MstStock
            {
                result = (from s in MasterStock.GetTable()
                          where s.MaterialCd == MaterialCd && s.UnitCd == UnitCd && s.OutsourceLabCd == 0 && s.SupplierCd == 0
                          && s.OfficeCd == OfficeCd
                          select s.MaterialCd).ToList().Count;
            }
            if (Condition == 2)//check SupplierCd if available in MstStock
            {
                result = (from s in MasterStock.GetTable()
                          where s.MaterialCd == MaterialCd && s.UnitCd == UnitCd && s.OutsourceLabCd == 0 && s.SupplierCd == SupplierCd
                          && s.OfficeCd == OfficeCd
                          select s.SupplierCd).ToList().Count;
            }
            if (Condition == 3)//check if OutSourceCd available in MstStock
            {
                result = (from s in MasterStock.GetTable()
                          where s.MaterialCd == MaterialCd && s.UnitCd == UnitCd && s.OutsourceLabCd == OutsourceCd && s.SupplierCd == 0
                          && s.OfficeCd == OfficeCd
                          select s.OutsourceLabCd).ToList().Count;
            }
            
            return result;
        }

        public static string GetPriorityUnit(int MaterialCd, DateTime RegisterDate, int OfficeCd)
        {
            return (from u in MasterUnit.GetTable()
                          where u.Priority == true
                          && u.MaterialCd == MaterialCd && u.OfficeCd == OfficeCd
                          && (u.TerminateDate >= RegisterDate || u.TerminateDate == null)
                          && u.ApplyDate <= RegisterDate
                          select u.UnitCd).FirstOrDefault();

        }

        public static Nullable<double> GetStock(int OfficeCd, int MaterialCd, int SupplierCd, int OutSourceCd, string UnitCdMain)//Get Amount of Material in MasterStock
        {
            Nullable<double> Amount = (from s in MasterStock.GetTable()
                             where s.OfficeCd == OfficeCd
                             && s.MaterialCd == MaterialCd
                             && s.SupplierCd == SupplierCd
                             && s.OutsourceLabCd == OutSourceCd
                             && s.UnitCd == UnitCdMain
                             select s.Amount).FirstOrDefault();
            if (Amount == null) return 0;
            else
                return Amount;
        }

        public static void TransactionManageStock(MasterStock stock,int SupplierCd, int OutSourceCd, int OfficeCd, Double AmountLend, TrnStockInOut StockInOut, TrnPurchase Purchase, int Condition)
        {
            using (DBContext db = new DBContext())
            {
                using (System.Data.Common.DbTransaction tran = db.UseTransaction())
                {
                    try
                    {

                        if (Condition == 1)//insert MasterStock
                        {
                            stock.SupplierCd = SupplierCd;
                            stock.OutsourceLabCd = OutSourceCd;
                            stock.Insert();
                            //---insert into stockinout----
                            TrnStockInOut.InsertWithoutKey(db,StockInOut);

                        }
                        if (Condition == 2)//Update MasterStock
                        {
                            
                            stock.SupplierCd = SupplierCd;
                            stock.OutsourceLabCd = OutSourceCd;
                            stock.Update();
                            //---insert into stockinout
                            TrnStockInOut.InsertWithoutKey(db, StockInOut);
                        }
                        if (Condition == 3)//insert + update MasterStock with different Amount
                        {
                            //----inser MasterStock------
                            stock.SupplierCd = SupplierCd;
                            stock.OutsourceLabCd = OutSourceCd;
                            stock.Amount = AmountLend;
                            stock.Insert();

                            //----update masterStock
                            MasterStock mstStock = new MasterStock();
                            mstStock = MasterStock.GetAll().Where(s => (s.OfficeCd == stock.OfficeCd && s.MaterialCd == stock.MaterialCd && s.SupplierCd == 0 && s.OutsourceLabCd == 0 && s.UnitCd == stock.UnitCd)).FirstOrDefault();
                            stock.SupplierCd = 0;
                            stock.OutsourceLabCd = 0;
                            stock.Amount = mstStock.Amount - stock.Amount;
                            stock.Update();
                            //---insert into StockInOut
                            TrnStockInOut.InsertWithoutKey(db, StockInOut);

                        }
                        if (Condition == 4)//update MasterStock
                        {

                            stock.SupplierCd = 0;
                            stock.OutsourceLabCd = 0;
                            stock.Update();
                            //----------------
                            stock.SupplierCd = SupplierCd;
                            stock.OutsourceLabCd = OutSourceCd;
                            stock.Amount = AmountLend;
                            stock.Update();
                            //---insert into StockInOut
                            TrnStockInOut.InsertWithoutKey(db, StockInOut);
                        }
                        if (StockInOut.InOutKbn == 11)
                        {
                            Nullable<int> PurchaseSeq=null;
                            Nullable<DateTime> EndDate=null;
                            Nullable<double> RegularPrice=null;

                            PurchaseSeq = (from p in db.GetTable<TrnPurchase>()
                                           where p.OfficeCd == OfficeCd
                                           orderby p.PurchaseSeq descending
                                           select p.PurchaseSeq).FirstOrDefault() + 1;
                            if (PurchaseSeq == null) PurchaseSeq = 1;

                            EndDate = (from m in db.GetTable<MasterMaterialPrice>()
                                       where m.StartDate<Purchase.PurchaseDate && m.MaterialCd == stock.MaterialCd && m.SupplierCd == StockInOut.SupplierCd
                                       && m.OfficeCd == OfficeCd orderby m.StartDate descending select m.EndDate).FirstOrDefault();
                            if (EndDate != null)
                            {
                                RegularPrice = (from m in db.GetTable<MasterMaterialPrice>()
                                                where m.StartDate < @Purchase.PurchaseDate
                                                && m.MaterialCd == stock.MaterialCd
                                                && m.SupplierCd == StockInOut.SupplierCd
                                                && m.OfficeCd == OfficeCd
                                                orderby m.StartDate descending
                                                select m.Price).FirstOrDefault();
                            }
                            Purchase.PurchaseSeq = (int)PurchaseSeq;
                            Purchase.RegularPrice = RegularPrice;
                            Purchase.Insert();
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

    public class CurrentStock
    {
        #region Propertise
        public Nullable<double> Amount { get; set; }
        public string UnitNm { get; set; }
        public Nullable<int> AmountByMiniUnit { get; set; }
        #endregion

        #region Method

        public static List<CurrentStock> GetCurrentStock(int MaterialCd, int OfficeCd)
        {
            var dbc = new DBContext();
            return (from s in dbc.GetTable<MasterStock>()
                    from u in dbc.GetTable<MasterUnit>()
                    where
                    s.UnitCd == u.UnitCd
                    && s.MaterialCd == MaterialCd
                    && s.SupplierCd == 0 && s.OutsourceLabCd == 0 && s.OfficeCd == OfficeCd
                    && u.OfficeCd == OfficeCd
                    
                    select new CurrentStock
                    {
                        Amount = s.Amount,
                        UnitNm = u.UnitNm,
                        AmountByMiniUnit = u.AmountByMinimumUnit

                    }).ToList();
                    
        }

        #endregion
    }
}
