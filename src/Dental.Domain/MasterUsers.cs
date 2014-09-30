using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using log4net;
using System.Data.Linq.Mapping;

namespace Dental.Domain
{
    [Table(Name = MasterUsersColumn.TABLE_NAME)]
    public class MasterUsers : BaseDomain<MasterUsers>, ICommonFunctions<MasterUsers>
    {

        readonly static ILog logger = LogManager.GetLogger(typeof(MasterUsers));

        private int _UserID;
        private string _Username;
        private string _FirstName;
        private string _LastName;
        private bool _IsSuperUser;        
        private Nullable<int> _AffiliateId;
        private string _Email;
        private string _DisplayName;        
        private bool _UpdatePassword;
        private string _LastIPAddress;
        private bool _IsDeleted;
        private Nullable<int> _CreatedByUserID;
        private Nullable<DateTime> _CreatedOnDate;
        private Nullable<int> _LastModifiedByUserID;
        private Nullable<DateTime> _LastModifiedOnDate;

        public virtual ICollection<MasterUserRoles> UserRoles { get; set; }

        #region Property

        [ColumnAttribute(Name = MasterUsersColumn.USER_ID, IsPrimaryKey = true, CanBeNull = false)]
        public int UserID
        {
            get { return this._UserID; }
            set { this._UserID = value; }
        }

        [ColumnAttribute(Name = MasterUsersColumn.USERNAME, CanBeNull = false)]
        public string Username
        {
            get { return this._Username; }
            set { this._Username = value; }
        }

        [ColumnAttribute(Name = MasterUsersColumn.FIRST_NAME, CanBeNull = false)]
        public string FirstName
        {
            get { return this._FirstName; }
            set { this._FirstName = value; }
        }

        [ColumnAttribute(Name = MasterUsersColumn.LAST_NAME, CanBeNull = false)]
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        [ColumnAttribute(Name = MasterUsersColumn.IS_SUPER_USER, CanBeNull = false)]
        public bool IsSuperUser
        {
            get { return _IsSuperUser; }
            set { _IsSuperUser = value; }
        }
        [ColumnAttribute(Name = MasterUsersColumn.AFFILIATE_ID, CanBeNull = true)]
        public Nullable<int> BillingFrequency
        {
            get { return _AffiliateId; }
            set { _AffiliateId = value; }
        }
        [ColumnAttribute(Name = MasterUsersColumn.EMAIL, CanBeNull = true)]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        [ColumnAttribute(Name = MasterUsersColumn.DISPLAY_NAME, CanBeNull = false)]
        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; }
        }
        [ColumnAttribute(Name = MasterUsersColumn.UPDATE_PASSWORD, CanBeNull = true)]
        public bool UpdatePassword
        {
            get { return _UpdatePassword; }
            set { _UpdatePassword = value; }
        }
        [ColumnAttribute(Name = MasterUsersColumn.LAST_IP_ADDRESS, CanBeNull = true)]
        public string LastIPAddress
        {
            get { return _LastIPAddress; }
            set { _LastIPAddress = value; }
        }
        [ColumnAttribute(Name = MasterUsersColumn.IS_DELETED, CanBeNull = false)]
        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }        
        [ColumnAttribute(Name = MasterUsersColumn.CREATED_BY_USER_ID, CanBeNull = true)]
        public System.Nullable<int> CreatedByUserID
        {
            get { return _CreatedByUserID; }
            set { _CreatedByUserID = value; }
        }
        [ColumnAttribute(Name = MasterUsersColumn.CREATED_ON_DATE, CanBeNull = true)]
        public System.Nullable<DateTime> CreatedOnDate
        {
            get { return _CreatedOnDate; }
            set { _CreatedOnDate = value; }
        }
        [ColumnAttribute(Name = MasterUsersColumn.LAST_MODIFIED_BY_USER_ID, CanBeNull = true)]
        public System.Nullable<int> LastModifiedByUserID
        {
            get { return _LastModifiedByUserID; }
            set { _LastModifiedByUserID = value; }
        }
        [ColumnAttribute(Name = MasterUsersColumn.LAST_MODIFIED_ON_DATE, CanBeNull = true)]
        public System.Nullable<DateTime> LastModifiedOnDate
        {
            get { return _LastModifiedOnDate; }
            set { _LastModifiedOnDate = value; }
        }        
        #endregion

        #region ICommonFunction
        public MasterUsers GetByPrimaryKey()
        {
            Table<MasterUsers> table = GetTable();
            MasterUsers item = table.Single(d => (d.UserID == this.UserID));
            if (item != null)
                item.Detach<MasterUsers>();
            return item;
        }
        #endregion
    }
}