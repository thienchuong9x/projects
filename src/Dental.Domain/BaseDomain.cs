using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Data.Linq;
namespace Dental.Domain
{
    [Serializable]
    public class BaseDomain<T> where T : class,  ICommonFunctions<T>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(T));
        protected DateTime _CreateDate;
        protected string _CreateAccount;
        protected DateTime _ModifiedDate;
        protected string _ModifiedAccount;

        #region Class Method

        public T Clone()
        {
            return (T)this.MemberwiseClone();
        }
        public static Table<T> GetTable()
        {
            DBContext db = new DBContext();
            return db.GetTable<T>();
        }

        public static Table<T> GetTable(DBContext db)
        {
            return db.GetTable<T>();
        }

        public static List<T> GetAll()
        {
            var list = from i in GetTable() select i;
            logger.Debug("list.count: " + list.Count());
            return list.ToList();
        }


        #endregion

        #region New Insert, Delete, Update
        public void Insert()
        {
            DBContext db = new DBContext();
            db.Insert<T>(this as T);
        }

        public void Delete()
        {
            DBContext db = new DBContext();
            db.Delete<T>(this as T);
        }

        public void Update()
        {
            DBContext db = new DBContext();
            db.Update<T>(this as T);
        }

        public void SetDefaultValueWhenInsert(bool bInsert)
        {
            this._ModifiedDate = DateTime.Now;
            if (bInsert) // Case Insert
            {
                this._CreateDate = DateTime.Now;
                this._ModifiedAccount = this._CreateAccount;
            }
        }

        #endregion

    }
    public interface ICommonFunctions<T>
    {
        T GetByPrimaryKey();
        void SetDefaultValueWhenInsert(bool bInsert);
    }
}
