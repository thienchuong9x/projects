using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{
    [Table(Name = MasterStaffColumn.TABLE_NAME)]
    public class MasterStaff : BaseDomain<MasterStaff>, ICommonFunctions<MasterStaff>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterStaff));

        private int _StaffCd;
        private string _StaffNm;
        private string _StaffNmKana;        
        private System.Nullable<int> _SectionCd;
        private System.Nullable<bool> _SalesFlg;
        private System.Nullable<bool> _TechFlg;
        private bool _IsDeleted;
        private string _UserId;

        #region Property

        [ColumnAttribute(Name = MasterStaffColumn.STAFF_CODE, IsPrimaryKey = true)]
        public int StaffCd
        {
            get
            {
                return this._StaffCd;
            }
            set
            {
                if ((this._StaffCd != value))
                {
                    this._StaffCd = value;
                }
            }
        }

        [ColumnAttribute(Name = MasterStaffColumn.STAFF_NAME)]
        public string StaffNm
        {
            get
            {
                return this._StaffNm;
            }
            set
            {
                if ((this._StaffNm != value))
                {
                    this._StaffNm = value;
                }
            }
        }

        [ColumnAttribute(Name = MasterStaffColumn.STAFF_KANA_NAME)]
        public string StaffNmKana
        {
            get
            {
                return this._StaffNmKana;
            }
            set
            {
                if ((this._StaffNmKana != value))
                {
                    this._StaffNmKana = value;
                }
            }
        }

        [ColumnAttribute(Name = MasterStaffColumn.STAFF_SECTION_CODE)]
        public System.Nullable<int> SectionCd
        {
            get
            {
                return this._SectionCd;
            }
            set
            {
                if ((this._SectionCd != value))
                {
                    this._SectionCd = value;
                }
            }
        }

        [ColumnAttribute(Name = MasterStaffColumn.STAFF_SALE_FLG)]
        public System.Nullable<bool> SalesFlg
        {
            get
            {
                return this._SalesFlg;
            }
            set
            {
                if ((this._SalesFlg != value))
                {
                    this._SalesFlg = value;
                }
            }
        }

        [ColumnAttribute(Name = MasterStaffColumn.STAFF_TECH_FLG)]
        public System.Nullable<bool> TechFlg
        {
            get
            {
                return this._TechFlg;
            }
            set
            {
                if ((this._TechFlg != value))
                {
                    this._TechFlg = value;
                }
            }
        }

        [ColumnAttribute(Name = MasterStaffColumn.STAFF_IS_DELETED)]
        public bool IsDeleted
        {
            get
            {
                return this._IsDeleted;
            }
            set
            {
                if ((this._IsDeleted != value))
                {
                    this._IsDeleted = value;
                }
            }
        }
        [ColumnAttribute(Name = MasterStaffColumn.STAFF_USER_ID)]
        public string UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {               
                 this._UserId = value;                
            }
        }
        [ColumnAttribute(Name = BaseColumn.CREATE_DATE)]
        public System.DateTime CreateDate
        {
            get
            {
                return this._CreateDate;
            }
            set
            {
                this._CreateDate = value;
            }
        }

        [ColumnAttribute(Name = BaseColumn.CREATE_ACCOUNT)]
        public string CreateAccount
        {
            get
            {
                return this._CreateAccount;
            }
            set
            {
                this._CreateAccount = value;
            }
        }

        [ColumnAttribute(Name = BaseColumn.MODIFIED_DATE)]
        public System.DateTime ModifiedDate
        {
            get
            {
                return this._ModifiedDate;
            }
            set
            {
                this._ModifiedDate = value;
            }
        }

        [ColumnAttribute(Name = BaseColumn.MODIFIED_ACCOUNT)]
        public string ModifiedAccount
        {
            get
            {
                return this._ModifiedAccount;
            }
            set
            {
                this._ModifiedAccount = value;
            }
        }
        #endregion 

        #region ICommonFunction
        public MasterStaff GetByPrimaryKey()
        {
            Table<MasterStaff> table = GetTable();

            MasterStaff item = table.Single(d => (d.StaffCd == this.StaffCd));
            if (item != null)
                item.Detach<MasterStaff>();

            return item;
        }
        #endregion 

        #region Method
        public static List<MasterStaff> GetStaffwiOffice(int officeCd)
        {            
            var context = new DBContext();
            var list = (from item in context.GetTable<DentalPermission>()
                        join
                          mStaff in context.GetTable<MasterStaff>() on new { item.StaffCd } equals new { mStaff.StaffCd }
                        where
                          (
                                item.OfficeCd == officeCd 
                             && mStaff.IsDeleted == false 
                          )
                        select mStaff).ToList();
            list = list.Distinct().ToList();
            return list;

        }

        public static MasterStaff GetStaff(int staffCd)
        {
            return (from item in GetTable()
                         where item.StaffCd == staffCd && item.IsDeleted == false 
                         select item).FirstOrDefault();
        }

        public static List<MasterStaff> GetStaffMasterSearch(string StaffCode, string StaffName, string officeCd, bool IncludeIsDeleted)
        {            
            var context = new DBContext(); 
            var list = (from item in context.GetTable<MasterStaff>()
                        join item1 in context.GetTable<DentalPermission>() on item.StaffCd equals item1.StaffCd into Permission
                        from item1 in Permission.DefaultIfEmpty()
                    where
                      (
                         (StaffCode == string.Empty || item.StaffCd.ToString().Contains(StaffCode))                          
                      && (StaffName == string.Empty || item.StaffNm.Contains(StaffName))
                      && (officeCd == string.Empty || (item.StaffCd == item1.StaffCd && item1.OfficeCd.ToString() == officeCd))
                      && (IncludeIsDeleted || item.IsDeleted == IncludeIsDeleted)
                      )
                    select item).Distinct().ToList();
            return list;
        }

        public static int countUser(string UserId)
        {
            return (from item in GetTable()
                    where
                      (
                         (item.UserId == UserId)
                      )
                    select item).Count();
        }
        #endregion        
    
        public static int GetStaffCd(string userLogin)
        {
            return (from item in GetTable()
                        where
                          (
                           item.UserId == userLogin
                          )
                        select item.StaffCd).FirstOrDefault();
           
        }
    }
}
