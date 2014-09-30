using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{
    [Table(Name = MasterRolesColumn.TABLE_NAME)]
    public class MasterRoles : BaseDomain<MasterRoles>, ICommonFunctions<MasterRoles>
    {   
   
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterRoles));

        private int _RoleID;
        private int _PortalID;
        private string _RoleName;
        private string _Description;
        private Nullable<decimal> _ServiceFee;
        private string _BillingFrequency;
        private Nullable<int> _TrialPeriod;
        private string _TrialFrequency;
        private Nullable<int> _BillingPeriod;
        private Nullable<decimal> _TrialFee;
        private bool _IsPublic;
        private bool _AutoAssignment;
        private Nullable<int> _RoleGroupID;
        private string _RSVPCode;
        private string _IconFile;
        private Nullable<int> _CreatedByUserID;
        private Nullable<DateTime> _CreatedOnDate;
        private Nullable<int> _LastModifiedByUserID;
        private Nullable<DateTime> _LastModifiedOnDate;
        private int _Status;
        private int _SecurityMode;
        private bool _IsSystemRole;

        public virtual ICollection<MasterUserRoles> UserRoles { get; set; }

        #region Property      

        [ColumnAttribute(Name = MasterRolesColumn.ROLE_ID, IsPrimaryKey = true, CanBeNull = false)]
        public int RoleID
        {
            get { return this._RoleID; }
            set { this._RoleID = value; }
        }

        [ColumnAttribute(Name = MasterRolesColumn.PORTAL_ID, CanBeNull = false)]
        public int PortalID
        {
            get { return this._PortalID; }
            set { this._PortalID = value; }
        }

        [ColumnAttribute(Name = MasterRolesColumn.ROLE_NAME, CanBeNull = false)]
        public string RoleName
        {
            get { return this._RoleName; }
            set { this._RoleName = value; }
        }

        [ColumnAttribute(Name = MasterRolesColumn.DESCRIPTION, CanBeNull = true)]
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        [ColumnAttribute(Name = MasterRolesColumn.SERVICE_FEE, CanBeNull = true)]
        public System.Nullable<decimal> ServiceFee
        {
            get { return _ServiceFee; }
            set { _ServiceFee = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.BILLING_FREQUENCY, CanBeNull = true)]
        public string BillingFrequency
        {
            get { return _BillingFrequency; }
            set { _BillingFrequency = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.TRIAL_PERIOD, CanBeNull = true)]
        public System.Nullable<int> TrialPeriod
        {
            get { return _TrialPeriod; }
            set { _TrialPeriod = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.TRIAL_FREQUENCY, CanBeNull = true)]
        public string TrialFrequency
        {
            get { return _TrialFrequency; }
            set { _TrialFrequency = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.BILLING_PERIOD, CanBeNull = true)]
        public System.Nullable<int> BillingPeriod
        {
            get { return _BillingPeriod; }
            set { _BillingPeriod = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.TRIAL_FEE, CanBeNull = true)]
        public System.Nullable<decimal> TrialFee
        {
            get { return _TrialFee; }
            set { _TrialFee = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.IS_PUBLIC, CanBeNull = false)]
        public bool IsPublic
        {
            get { return _IsPublic; }
            set { _IsPublic = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.AUTO_ASSIGNMENT, CanBeNull = false)]
        public bool AutoAssignment
        {
            get { return _AutoAssignment; }
            set { _AutoAssignment = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.ROLE_GROUP_ID, CanBeNull = true)]
        public System.Nullable<int> RoleGroupID
        {
            get { return _RoleGroupID; }
            set { _RoleGroupID = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.RSVP_CODE, CanBeNull = true)]
        public string RSVPCode
        {
            get { return _RSVPCode; }
            set { _RSVPCode = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.ICON_FILE, CanBeNull = true)]
        public string IconFile
        {
            get { return _IconFile; }
            set { _IconFile = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.CREATED_BY_USER_ID, CanBeNull = true)]
        public System.Nullable<int> CreatedByUserID
        {
            get { return _CreatedByUserID; }
            set { _CreatedByUserID = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.CREATED_ON_DATE, CanBeNull = true)]
        public System.Nullable<DateTime> CreatedOnDate
        {
            get { return _CreatedOnDate; }
            set { _CreatedOnDate = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.LAST_MODIFIED_BY_USER_ID, CanBeNull = true)]
        public System.Nullable<int> LastModifiedByUserID
        {
            get { return _LastModifiedByUserID; }
            set { _LastModifiedByUserID = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.LAST_MODIFIED_ON_DATE, CanBeNull = true)]
        public System.Nullable<DateTime> LastModifiedOnDate
        {
            get { return _LastModifiedOnDate; }
            set { _LastModifiedOnDate = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.STATUS, CanBeNull = false)]
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.SECURITY_MODE, CanBeNull = false)]
        public int SecurityMode
        {
            get { return _SecurityMode; }
            set { _SecurityMode = value; }
        }
        [ColumnAttribute(Name = MasterRolesColumn.IS_SYSTEM_ROLE, CanBeNull = false)]
        public bool IsSystemRole
        {
            get { return _IsSystemRole; }
            set { _IsSystemRole = value; }
        }       
        #endregion

        #region ICommonFunction
        public MasterRoles GetByPrimaryKey()
        {
            Table<MasterRoles> table = GetTable();
            MasterRoles item = table.Single(d => (d.RoleID == this.RoleID));
            if (item != null)
                item.Detach<MasterRoles>();
            return item;
        }

        #endregion

        #region Method

        public static List<MasterRoles> GetRoleMasters()
        {
            return (from item in GetTable()
                    select item).ToList();
        }

        public static MasterRoles GetRoleMaster(int roleCd)
        {
            return (from item in GetTable()
                    where item.RoleID == roleCd
                    select item).FirstOrDefault();
        }

        public static List<MasterRoles> GetRoleMasterSearch(string roleCd, string roleName)
        {
            return (from item in GetTable()
                    where
                      (
                         (roleCd == string.Empty || item.RoleID.ToString() == roleCd)
                      && (roleName == string.Empty || item.RoleName.Contains(roleName))
                      )

                    select item).ToList();
        }


        #endregion
    }
}
