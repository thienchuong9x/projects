using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using Dental.Utilities;

namespace Dental.Domain
{
    public partial class DBContext : System.Data.Linq.DataContext
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[Constant.DENTAL_CONNECTIONSTRING] != null ? System.Configuration.ConfigurationManager.ConnectionStrings[Constant.DENTAL_CONNECTIONSTRING].ConnectionString : null;
        private bool isDispose = false;

        public bool IsDispose
        {
            get
            {
                return isDispose;
            }
        }

       // private static DBContext commonDBContext = null;
        public DBContext()
            : base(connectionString)
        {

        }

        public DBContext(string connection)
            : base(connection)
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.isDispose = true;
        }

        public System.Data.Common.DbTransaction UseTransaction()
        {
            this.Connection.Open();
            this.CommandTimeout = 3600;

            System.Data.Common.DbTransaction tran = this.Connection.BeginTransaction();
            this.Transaction = tran;

            return tran;
        }
        
        #region Table

        //public Table<MasterDentalOffice> MasterDentalOffice
        //{
        //    get
        //    {
        //        return this.GetTable<MasterDentalOffice>();
        //    }
        //}
        //public Table<MasterBill> MasterBill
        //{
        //    get
        //    {
        //        return this.GetTable<MasterBill>();
        //    }
        //}

        //public Table<DentalOrderHeader> DentalOrderHeader
        //{
        //    get
        //    {
        //        return this.GetTable<DentalOrderHeader>();
        //    }
        //}
        //public Table<DentalOrderDetail> DentalOrderDetail
        //{
        //    get
        //    {
        //        return this.GetTable<DentalOrderDetail>();
        //    }
        //}

        #endregion
       

        #region Solve DataContext Problem

        public void Delete<T>(T item) where T : class, ICommonFunctions<T>
        {
            try
            {
                Table<T> table = this.GetTable<T>();
                item = item.GetByPrimaryKey();
                table.Attach(item as T);
                table.DeleteOnSubmit(item as T);
                this.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void Insert<T>(T item) where T : class, ICommonFunctions<T>
        {
            try
            {
                item.SetDefaultValueWhenInsert(true);
                Table<T> table = this.GetTable<T>();
                table.InsertOnSubmit(item as T);
                this.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void InsertAll<T>(List<T> list) where T : class, ICommonFunctions<T>
        {
            try
            {
                if (list != null)
                {
                    foreach (var item in list)
                      item.SetDefaultValueWhenInsert(true);
                    Table<T> table = this.GetTable<T>();

                    table.InsertAllOnSubmit(list);
                    this.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void Update<T>(T item) where T : class, ICommonFunctions<T>
        {
            try
            {
                item.SetDefaultValueWhenInsert(false);
                T t = item.GetByPrimaryKey();
                GenericUtil.ShallowCopy<T>(item, t);
               // this.GetTable<T>().Attach(t as T, true);
                this.GetTable<T>().Attach(t as T);
                this.Refresh(RefreshMode.KeepCurrentValues, t as T);
                this.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
