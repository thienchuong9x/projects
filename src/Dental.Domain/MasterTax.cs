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
    [Table(Name = MasterTaxColumn.TABLE_NAME)]
    public class MasterTax : BaseDomain<MasterTax>, ICommonFunctions<MasterTax>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterTax));

       #region PrivateMember
      
       private int _TaxCd;
       private double _TaxRate;
       private DateTime _StartDate;
       private Nullable<DateTime> _EndDate;
       private Nullable<int> _RoundFraction; 
       #endregion

       #region Property
       
       [ColumnAttribute(Name = MasterTaxColumn.TAX_CD, IsPrimaryKey = true, CanBeNull = false)]
       public int TaxCd
       {
           get { return this._TaxCd; }
           set { this._TaxCd = value; }
       }

       [ColumnAttribute(Name = MasterTaxColumn.TAX_RATE, CanBeNull = false)]
       public double TaxRate
       {
           get { return this._TaxRate; }
           set { this._TaxRate = value; }
       }

       [ColumnAttribute(Name = MasterTaxColumn.START_DATE, CanBeNull = false)]
       public DateTime StartDate
       {
           get { return this._StartDate; }
           set { this._StartDate = value; }
       }

       [ColumnAttribute(Name = MasterTaxColumn.END_DATE, CanBeNull = true)]
       public Nullable<DateTime> EndDate
       {
           get { return this._EndDate; }
           set { this._EndDate = value; }
       }

       [ColumnAttribute(Name = MasterTaxColumn.ROUND_FRACTION, CanBeNull = true)]
       public Nullable<int> RoundFraction
       {
           get { return this._RoundFraction; }
           set { this._RoundFraction = value; }
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
       public MasterTax GetByPrimaryKey()
       {
           Table<MasterTax> table = GetTable();

           MasterTax item = table.Single(d => (d.TaxCd == this.TaxCd));
           if (item != null)
               item.Detach<MasterTax>();

           return item;
       }
       #endregion 

       #region Method
       public static MasterTax GetTaxMaster(int taxCd)
       {
           var result = from item in GetTable()
                        where item.TaxCd == taxCd
                        select item;
           return (MasterTax)result.FirstOrDefault();
       }
       public static List<MasterTax> GetTaxMasterSearch( string taxCd, string date, int check)
       {
           if (date == "")
               date = "1/1/2000";
            return (from item in GetTable()
                    where
                    (
                        
                        (taxCd == string.Empty || item.TaxCd.ToString().Contains(taxCd))
                        && (
                         (check == 0 && ((date == "1/1/2000" && item.StartDate.Date <= DateTime.Now.Date && (item.EndDate == null || DateTime.Now.Date <= item.EndDate.Value.Date))
                                      || (item.StartDate.Date <= Convert.ToDateTime(date).Date && (item.EndDate == null || Convert.ToDateTime(date).Date <= item.EndDate.Value.Date) && item.StartDate.Date <= DateTime.Now.Date && (item.EndDate == null || DateTime.Now.Date <= item.EndDate.Value.Date))))
                        || (check == 1 && ((date == "1/1/2000")
                                      || (item.StartDate.Date <= Convert.ToDateTime(date).Date && (item.EndDate == null || Convert.ToDateTime(date).Date <= item.EndDate.Value.Date))))
                    
                        ) 
                    )
                    select item).ToList();
       }
       public static MasterTax SearchTax(DateTime orderDate)
       {
           return (from item in GetTable()
                    where
                    (      
                        (item.StartDate.Date <= orderDate.Date)
                        &&(item.EndDate == null || item.EndDate.Value.Date >= orderDate.Date)
                    )
                    select item).FirstOrDefault();
       }
       #endregion        
    
      
    }    
}
