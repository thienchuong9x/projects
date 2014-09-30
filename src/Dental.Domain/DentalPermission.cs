using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace Dental.Domain
{
    [Table(Name = PermissionColumn.TABLE_NAME)]
    public class DentalPermission : BaseDomain<DentalPermission>, ICommonFunctions<DentalPermission>
    {
        private int _StaffCd;
        private int _OfficeCd;
        private string _Permission;
        private System.Nullable<bool> _DelFlg;


        [ColumnAttribute(Name = PermissionColumn.STAFF_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int StaffCd
        {
            get { return this._StaffCd; }
            set { this._StaffCd = value; }
        }

        [ColumnAttribute(Name = BaseDentalOrderKeyColumn.OFFICE_CD, IsPrimaryKey = true, CanBeNull = false)]
        public int OfficeCd
        {
            get { return this._OfficeCd; }
            set { this._OfficeCd = value; }
        }

        [ColumnAttribute(Name = PermissionColumn.PERMISSION)]
        public string Permission
        {
            get { return this._Permission; }
            set { this._Permission = value; }
        }
        [ColumnAttribute(Name = PermissionColumn.DEL_FLG)]
        public System.Nullable<bool> DelFlg
        {
            get { return _DelFlg; }
            set { _DelFlg = value; }
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



        public DentalPermission GetByPrimaryKey()
        {
            Table<DentalPermission> table = GetTable();

            DentalPermission item = table.Single(d => (d.OfficeCd == this.OfficeCd && d.StaffCd == this.StaffCd));
            if (item != null)
                item.Detach<DentalPermission>();
            return item;
        }

        public static List<MasterOffice> GetAllPermissionByUserName(int staffCd)
        {
            var context = new DBContext();

            return 
                   (from item in context.GetTable<DentalPermission>()
                    join
                       mOffice in context.GetTable<MasterOffice>() on new { item.OfficeCd } equals new { mOffice.OfficeCd }
                    where
                    (
                           item.StaffCd == staffCd
                         && (item.DelFlg == null || item.DelFlg.Value == false)
                         && (mOffice.IsDeleted  == null || mOffice.IsDeleted.Value == false)
                      )
                    select mOffice).ToList();
          
        }
        public static string GetPermissionForUser(int staffCd , string officeCd) 
        {
            if (string.IsNullOrEmpty(officeCd))
                return null;

            var itemPermission =  (from item in GetTable()
                                   where item.StaffCd == staffCd  && item.OfficeCd.ToString() == officeCd 
                                     && (item.DelFlg == null || item.DelFlg.Value == false)
                                   select item).FirstOrDefault();

            if (itemPermission == null)
                return null;
            else
                return itemPermission.Permission;
        }

        public static int HasRole(int StaffCd, int OfficeCd)
        {
            return (from item in GetTable()
                    where item.StaffCd == StaffCd && item.OfficeCd == OfficeCd
                    select item).Count();
        }

        public static DentalPermission GetRole(int StaffCd, int OfficeCd)
        {
            return (from item in GetTable()
                    where
                      (
                         (item.StaffCd == StaffCd && item.OfficeCd == OfficeCd)
                      )

                    select item).FirstOrDefault();
        }

        public static int HasAuthority(int StaffCd, int OfficeCd)
        {
            string s = "";
            s = (from item in GetTable()
                    where item.StaffCd == StaffCd && item.OfficeCd == OfficeCd
                    select item.Permission).FirstOrDefault();

            if (s == "OfficeChief")
                return 1;
            else
                return 0;

        }

    }
}
