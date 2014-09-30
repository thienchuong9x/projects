using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dental.Utilities;

namespace Dental.Domain
{
    [Serializable]
    public class PaymentsInfo : TrnOutsource
    {
        // public properties 
        //public int OfficeCd { get; set; }
        public int SupplierOutsourceCd { get; set; }
        //public int PurchaseSeq { get; set; }
        public Nullable<DateTime> PurchaseDate { get; set; }
        public Nullable<int> PurchaseCategory { get; set; }
        public string PurchaseItems { get; set; }
        public Nullable<double> PurchasePrice { get; set; }
        public Nullable<double> RegularPrice { get; set; }
        public Nullable<double> PurchaseFee { get; set; }
        public Nullable<double> PaidMoney { get; set; }
        public Nullable<double> Balance { get; set; }
        public string Note { get; set; }
        public string PayDate { get; set; }
        public string PayPrice { get; set; }
        public string PayFee { get; set; }
        public string BankCd { get; set; }
        public string Message { get; set; }
        //public System.DateTime CreateDate { get; set; }
        //public string CreateAccount { get; set; }
        //public System.DateTime ModifiedDate { get; set; }
        //public string ModifiedAccount { get; set; }

        public static List<PaymentsInfo> GetPaymentsInfoSearch(string sellerCd, string outsourceCd, string buyDateFrom, string buyDateTo, int officeCd, int check)
        {
            if (buyDateFrom == string.Empty)
                buyDateFrom = "1/1/2000";
            if (buyDateTo == string.Empty)
                buyDateTo = "1/1/6000";
            var context = new DBContext();
            return (from item in context.GetTable<TrnPurchase>()
                    from itemout in context.GetTable<TrnOutsource>()
                    where
                    (                    
                        (officeCd == item.OfficeCd)
                        && officeCd == itemout.OfficeCd
                        && item.PurchaseDate != null
                        && item.PurchasePrice > 0
                        && item.PurchasePrice != null                        
                        && (
                            (sellerCd == string.Empty && outsourceCd == string.Empty)
                            || (sellerCd != string.Empty && item.SupplierOutsourceCd.ToString() == sellerCd && itemout.PurchaseSeq != item.PurchaseSeq)
                            || (outsourceCd != string.Empty && item.SupplierOutsourceCd.ToString() == outsourceCd && itemout.PurchaseSeq == item.PurchaseSeq && itemout.OutsourceCd.ToString() == outsourceCd)                            
                        )
                        && (
                            (check == 1 && item.Balance == 0)
                            || (check == 0 && (item.Balance != 0 || item.Balance == null))
                        )
                        && (                           
                           (Convert.ToDateTime(buyDateFrom).Date <= item.PurchaseDate.Value.Date && item.PurchaseDate.Value.Date <= Convert.ToDateTime(buyDateTo).Date )
                        )                        
                    )
                    select new PaymentsInfo() {
                         OfficeCd = item.OfficeCd,
                         SupplierOutsourceCd = item.SupplierOutsourceCd,
                         PurchaseSeq = item.PurchaseSeq,
                         PurchaseDate = item.PurchaseDate,
                         PurchaseCategory = item.PurchaseCategory,
                         PurchaseItems = item.PurchaseItems,
                         PurchasePrice = item.PurchasePrice,
                         RegularPrice = item.RegularPrice,
                         PurchaseFee = item.PurchaseFee,
                         PaidMoney = item.PaidMoney,
                         Balance = item.Balance,                         
                         Note = item.Note,                        

                    }).Distinct().ToList();
        }
    }
    public class ConstantIndex
    {
        public static int index_PurchaseDate = 1;
        public static int index_Balance = 6;
        public static int index_PurchaseSeq = 10;
        public static int index_SupplierOutsourceCd = 11;
        public static int index_PurchaseCategory = 12;
    }
}
