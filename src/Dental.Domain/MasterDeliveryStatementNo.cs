using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{
    [Table(Name = MasterDeliveryStatementNoColumn.TABLE_NAME)]
    public class MasterDeliveryStatementNo : BaseDomain<MasterDeliveryStatementNo>, ICommonFunctions<MasterDeliveryStatementNo>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterDeliveryStatementNo));

        private int _OfficeCd;
        private int _DentalOfficeCd;
        private int _Year;
        private int _BlanchNo;

        #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int OfficeCd
        {
            get { return _OfficeCd; }
            set { _OfficeCd = value; }
        }

        [ColumnAttribute(Name = MasterDeliveryStatementNoColumn.DENTAL_OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int DentalOfficeCd
        {
            get { return this._DentalOfficeCd; }
            set { this._DentalOfficeCd = value; }
        }

        [ColumnAttribute(Name = MasterDeliveryStatementNoColumn.YEAR, IsPrimaryKey = true, CanBeNull = false)]
        public int Year
        {
            get { return this._Year; }
            set { this._Year = value; }
        }

        [ColumnAttribute(Name = MasterDeliveryStatementNoColumn.BLANCH_NO, CanBeNull = false)]
        public int BlanchNo
        {
            get { return this._BlanchNo; }
            set { this._BlanchNo = value; }
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
        public MasterDeliveryStatementNo GetByPrimaryKey()
        {
            Table<MasterDeliveryStatementNo> table = GetTable();

            MasterDeliveryStatementNo item = table.Single(d => (d.DentalOfficeCd == this.DentalOfficeCd && d.Year == this.Year && d.OfficeCd == this.OfficeCd));
            if (item != null)
                item.Detach<MasterDeliveryStatementNo>();
            return item;
        }

        #endregion

        #region Method

        public static MasterDeliveryStatementNo GetDeliveryStatementNo(int officeCd, int dentalOfficeCd, int year)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd && item.DentalOfficeCd == dentalOfficeCd && item.Year == year
                    select item).FirstOrDefault();
        }
        #endregion


        public static MasterDeliveryStatementNo SetDeliveryStatementNo(int officeCd, int dentalOfficeCd, int year, string userName)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd && item.DentalOfficeCd == dentalOfficeCd && item.Year == year
                    select item).FirstOrDefault();            
        }   
    }
}
