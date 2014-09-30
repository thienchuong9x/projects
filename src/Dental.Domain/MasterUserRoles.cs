using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{
    [Table(Name = MasterUserRolesColumn.TABLE_NAME)]
    public class MasterUserRoles : BaseDomain<MasterUserRoles>, ICommonFunctions<MasterUserRoles>
    {

        readonly static ILog logger = LogManager.GetLogger(typeof(MasterUserRoles));

        private int _UserRoleID;
        private int _UserID;
        private int _RoleID;
        private Nullable<DateTime> _ExpiryDate;
        private Nullable<bool> _IsTrialUsed;
        private Nullable<DateTime> _EffectiveDate;
        private Nullable<int> _CreatedByUserID;
        private Nullable<DateTime> _CreatedOnDate;
        private Nullable<int> _LastModifiedByUserID;
        private Nullable<DateTime> _LastModifiedOnDate;
        private int _Status;       
        private bool _IsOwner;

        #region Property

        [ColumnAttribute(Name = MasterUserRolesColumn.USER_ROLE_ID, IsPrimaryKey = true, IsDbGenerated=true,  CanBeNull = false)]
        public int UserRoleID
        {
            get { return this._UserRoleID; }
            set { this._UserRoleID = value; }
        }
        public virtual MasterUsers Users { get; set; }

        [ColumnAttribute(Name = MasterUserRolesColumn.USER_ID, CanBeNull = false)]
        public int UserID
        {
            get { return this._UserID; }
            set { this._UserID = value; }
        }
        public virtual MasterRoles Roles { get; set; }

        //[Association(Name = MasterUserRolesColumn.ROLE_ID, Storage = "_RoleID", ThisKey = "RoleID", IsForeignKey = true)]   
        [ColumnAttribute(Name = MasterUserRolesColumn.ROLE_ID, CanBeNull = false)]
        public int RoleID
        {
            get { return this._RoleID; }
            set { this._RoleID = value; }
        }

        [ColumnAttribute(Name = MasterUserRolesColumn.EXPIRY_DATE, CanBeNull = true)]
        public Nullable<DateTime> ExpiryDate
        {
            get { return this._ExpiryDate; }
            set { this._ExpiryDate = value; }
        }
        [ColumnAttribute(Name = MasterUserRolesColumn.IS_TRIAL_USED, CanBeNull = true)]
        public Nullable<bool> IsTrialUsed
        {
            get { return _IsTrialUsed; }
            set { _IsTrialUsed = value; }
        }
        [ColumnAttribute(Name = MasterUserRolesColumn.EFFECTIVE_DATE, CanBeNull = true)]
        public System.Nullable<DateTime> EffectiveDate
        {
            get { return _EffectiveDate; }
            set { _EffectiveDate = value; }
        }        
        [ColumnAttribute(Name = MasterUserRolesColumn.CREATED_BY_USER_ID, CanBeNull = true)]
        public System.Nullable<int> CreatedByUserID
        {
            get { return _CreatedByUserID; }
            set { _CreatedByUserID = value; }
        }
        [ColumnAttribute(Name = MasterUserRolesColumn.CREATED_ON_DATE, CanBeNull = true)]
        public System.Nullable<DateTime> CreatedOnDate
        {
            get { return _CreatedOnDate; }
            set { _CreatedOnDate = value; }
        }
        [ColumnAttribute(Name = MasterUserRolesColumn.LAST_MODIFIED_BY_USER_ID, CanBeNull = true)]
        public System.Nullable<int> LastModifiedByUserID
        {
            get { return _LastModifiedByUserID; }
            set { _LastModifiedByUserID = value; }
        }
        [ColumnAttribute(Name = MasterUserRolesColumn.LAST_MODIFIED_ON_DATE, CanBeNull = true)]
        public System.Nullable<DateTime> LastModifiedOnDate
        {
            get { return _LastModifiedOnDate; }
            set { _LastModifiedOnDate = value; }
        }
        [ColumnAttribute(Name = MasterUserRolesColumn.STATUS, CanBeNull = false)]
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        [ColumnAttribute(Name = MasterUserRolesColumn.IS_OWNER, CanBeNull = false)]
        public bool IsOwner
        {
            get { return _IsOwner; }
            set { _IsOwner = value; }
        }       
        #endregion

        #region ICommonFunction
        public MasterUserRoles GetByPrimaryKey()
        {
            Table<MasterUserRoles> table = GetTable();
            MasterUserRoles item = table.Single(d => (d.UserRoleID == this.UserRoleID));
            if (item != null)
                item.Detach<MasterUserRoles>();
            return item;
        }
        #endregion

        #region Method
        public static List<MasterUserRoles> GetUserRoles(int UserId)
        {
            return (from item in GetTable()
                    where
                      (
                         (item.UserID == UserId)
                      )

                    select item).ToList();
        }

        public static int isUserRole(int UserId, int Role)
        {
            return (from item in GetTable()
                    where item.UserID == UserId && item.RoleID == Role
                    select item).Count();
        }

        
        //public static int isSalesman(int UserId)
        //{
        //    return (from item in GetTable()
        //            where item.UserID == UserId && item.RoleID == 12
        //            select item).Count();
        //}

        //public static int isTechnician(int UserId)
        //{
        //    return (from item in GetTable()
        //            where item.UserID == UserId && item.RoleID == 13
        //            select item).Count();
        //}

        public static MasterUserRoles GetUserRole(int UserId,int RoleId)
        {
            return (from item in GetTable()
                    where
                      (
                         (item.UserID == UserId && item.RoleID == RoleId)
                      )

                    select item).FirstOrDefault();
        }

        public static void AddRemoveUserRole(int UserId, int RoleId, Boolean isAdd)
        {
            MasterUserRoles tmp = GetUserRole(UserId, RoleId);

            if (tmp != null)
            {
                if (!isAdd)
                    tmp.Delete();

            }
            else
            {
                
                if (isAdd)
                {
                    tmp = new MasterUserRoles();
                    tmp.UserID = UserId;
                    tmp.RoleID = RoleId;
                    tmp.Status = 1;
                    tmp.IsOwner = false;
                    tmp.CreatedOnDate = tmp.LastModifiedOnDate = DateTime.Now;                    
                    tmp.Insert();
                }
            }
        }


        public static void RemoveUserRole(int RoleId)
        {
            var list = (from item in GetTable()
             where
               (
                  (item.RoleID == RoleId)
               )

             select item).ToList();
            foreach (MasterUserRoles obj in list)
            {
                obj.Delete();
            }
        }


        #endregion        
    }
}