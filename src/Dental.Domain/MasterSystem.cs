using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using log4net;
using System.Data.Linq;

namespace Dental.Domain
{    
    [Table(Name = MasterSystemColumn.TABLE_NAME)]
    public class MasterSystem : BaseDomain<MasterSystem>, ICommonFunctions<MasterSystem>
    {
        readonly static ILog logger = LogManager.GetLogger(typeof(MasterSystem));

       #region PrivateMember

       private string _Parameter;
       private string _Value;     
       
       #endregion

       #region Property
       
       [ColumnAttribute(Name = MasterSystemColumn.PARAMETER, IsPrimaryKey = true, CanBeNull = false)]
       public string Parameter
       {
           get { return this._Parameter; }
           set { this._Parameter = value; }
       }

       [ColumnAttribute(Name = MasterSystemColumn.VALUE, CanBeNull = false)]
       public string Value
       {
           get { return this._Value; }
           set { this._Value = value; }
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
       public MasterSystem GetByPrimaryKey()
       {
           Table<MasterSystem> table = GetTable();

           MasterSystem item = table.Single(d => (d.Parameter == this.Parameter));
           if (item != null)
               item.Detach<MasterSystem>();

           return item;
       }
       #endregion 

       #region Method

       public static MasterSystem GetSystemMaster(string Parameter)
       {
           var result = from objsystem in GetTable()
                            where objsystem.Parameter == Parameter
                            select objsystem;

           return (MasterSystem)result.FirstOrDefault();
       }

       #endregion
    }    
}

