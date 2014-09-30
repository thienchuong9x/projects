using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{    
    [Table(Name = MasterMaterialColumn.TABLE_NAME)]
    [Serializable]
    public class MasterMaterial : BaseDomain<MasterMaterial>, ICommonFunctions<MasterMaterial>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterDentalOffice));

        #region PrivateMember

        private int _OfficeCd;        
        private int _MaterialCd;
        private string _MaterialNm;
        private string _MaterialCADNm;
        private string _ProductMaker;
        private string _ProductCd;
        private string _ProductNm;
        private Nullable<double> _LentPrice;
        private Nullable<bool> _ShadeFlg;
        private Nullable<DateTime> _ApplyDate;
        private Nullable<DateTime> _TerminateDate;
        
        #endregion

        #region Property
        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int OfficeCd
        {
            get { return _OfficeCd; }
            set { _OfficeCd = value; }
        }

        [ColumnAttribute(Name = MasterMaterialColumn.MATERIAL_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int MaterialCd
        {
            get { return this._MaterialCd; }
            set { this._MaterialCd = value; }
        }

        [ColumnAttribute(Name = MasterMaterialColumn.MATERIAL_NM, CanBeNull = false)]
        public string MaterialNm
        {
            get { return this._MaterialNm; }
            set { this._MaterialNm = value; }
        }

        [ColumnAttribute(Name = MasterMaterialColumn.MATERIAL_CAD_NM, CanBeNull = false)]
        public string MaterialCADNm
        {
            get { return this._MaterialCADNm; }
            set { this._MaterialCADNm = value; }
        }


        [ColumnAttribute(Name = MasterMaterialColumn.PRODUCT_MAKER, CanBeNull = true)]
        public string ProductMaker
        {
            get { return this._ProductMaker; }
            set { this._ProductMaker = value; }
        }

        [ColumnAttribute(Name = MasterMaterialColumn.PRODUCT_CD, CanBeNull = true)]
        public string ProductCd
        {
            get { return this._ProductCd; }
            set { this._ProductCd = value; }
        }

        [ColumnAttribute(Name = MasterMaterialColumn.PRODUCT_NM, CanBeNull = true)]
        public string ProductNm
        {
            get { return this._ProductNm; }
            set { this._ProductNm = value; }
        }

        [ColumnAttribute(Name = MasterMaterialColumn.LENT_PRICE, CanBeNull = true)]
        public Nullable<double> LentPrice
        {
            get { return this._LentPrice; }
            set { this._LentPrice = value; }
        }

        [ColumnAttribute(Name = MasterMaterialColumn.SHADE_FLG, CanBeNull = true)]
        public Nullable<bool> ShadeFlg
        {
            get { return this._ShadeFlg; }
            set { this._ShadeFlg = value; }
        }

        [ColumnAttribute(Name = MasterMaterialColumn.APPLY_DATE, CanBeNull = true)]
        public Nullable<DateTime> ApplyDate
        {
            get { return this._ApplyDate; }
            set { this._ApplyDate = value; }
        }

        [ColumnAttribute(Name = MasterMaterialColumn.TERMINATE_DATE, CanBeNull = true)]
        public System.Nullable<DateTime> TerminateDate
        {
            get { return this._TerminateDate; }
            set { this._TerminateDate = value; }
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

        #region Custom Property
        public string StaffNm { get; set; }

        #endregion

        #region ICommonFunction
        public MasterMaterial GetByPrimaryKey()
        {
            Table<MasterMaterial> table = GetTable();

            MasterMaterial item = table.Single(d => (d.MaterialCd == this.MaterialCd && d.OfficeCd == this.OfficeCd));
            if (item != null)
                item.Detach<MasterMaterial>();

            return item;
        }
        #endregion

        #region Method
        public static MasterMaterial GetMaterialMaster(int officeCd, int materialCd)
        {

            return (from item in GetTable()
                         where item.MaterialCd == materialCd && item.OfficeCd == officeCd
                         select item).FirstOrDefault();
        }
        public static List<MasterMaterial> GetMstMaterial(int officeCd)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd
                    select item).ToList();
        }

        public static List<MasterMaterial> GetAllMstMaterials(int officeCd, string code, string name, bool cbShowTerminate)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd
                          && (cbShowTerminate == true || (item.TerminateDate == null || item.TerminateDate.Value.Date >= DateTime.Now.Date))   
                          && (code == string.Empty || item.MaterialCd.ToString().Contains(code))                         
                          && (name == string.Empty || item.MaterialNm.Contains(name))
                    select item).ToList();
        }

        public static List<MasterMaterial> GetMstMaterials(int officeCd, string code, string name)
        {
            return (from item in GetTable()
                    where item.OfficeCd == officeCd
                          && (item.TerminateDate == null || item.TerminateDate.Value.Date >= DateTime.Now.Date)
                          && (code == string.Empty || item.MaterialCd.ToString().Contains(code))
                          && (name == string.Empty || item.MaterialNm.Contains(name))
                    select item).ToList();
        }
        #endregion       
    }    
}
