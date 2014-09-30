using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Linq.Mapping;

namespace Dental.Domain
{
    public static class GenericUtil
    {
        public static void Detach<TEntity>(this TEntity entity)
        {
            foreach (FieldInfo fi in entity.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (fi.FieldType.ToString().Contains("EntityRef"))
                {
                    var value = fi.GetValue(entity);
                    if (value != null)
                    {
                        fi.SetValue(entity, null);
                    }
                }
                if (fi.FieldType.ToString().Contains("EntitySet"))
                {
                    var value = fi.GetValue(entity);
                    if (value != null)
                    {
                        MethodInfo mi = value.GetType().GetMethod("Clear");
                        if (mi != null)
                        {
                            mi.Invoke(value, null);
                        }
                        fi.SetValue(entity, value);
                    }
                }
            }
        }
        public static void ShallowCopy<T>(this T source, out T dest)
        {
            PropertyInfo[] sourcePropInfos = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] destinationPropInfos = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // create an object to copy values into
            Type entityType = source.GetType();
            dest = (T)(Activator.CreateInstance(entityType));

            foreach (PropertyInfo sourcePropInfo in sourcePropInfos)
            {
                if (Attribute.GetCustomAttribute(sourcePropInfo, typeof(ColumnAttribute), false) != null)
                {
                    PropertyInfo destPropInfo = destinationPropInfos.Where(pi => pi.Name == sourcePropInfo.Name).First();
                    destPropInfo.SetValue(dest, sourcePropInfo.GetValue(source, null), null);
                }
            }

        }

        public static void ShallowCopy<T>(this T source, T dest)
        {
            PropertyInfo[] sourcePropInfos = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] destinationPropInfos = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo sourcePropInfo in sourcePropInfos)
            {
                if (Attribute.GetCustomAttribute(sourcePropInfo, typeof(ColumnAttribute), false) != null)
                {
                    PropertyInfo destPropInfo = destinationPropInfos.Where(pi => pi.Name == sourcePropInfo.Name).First();
                    if (!destPropInfo.PropertyType.FullName.Equals("System.Data.Linq.Binary"))
                    {
                        //Except copy CreateDate and CreateAccount 
                        if( destPropInfo.Name != BaseColumn.CREATE_DATE  && destPropInfo.Name != BaseColumn.CREATE_ACCOUNT )
                          destPropInfo.SetValue(dest, sourcePropInfo.GetValue(source, null), null);
                    }
                }
            }

        }

    }
}
