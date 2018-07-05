namespace Git.Framework.ORM
{
    using Git.Framework.Cache;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class EntityTypeCache
    {
        private static ICache<Type, TableInfo> cache = new Git.Framework.Cache.Cache<Type, TableInfo>();

        public static PropertyInfo[] GeProperties<T>() where T: IEntity
        {
            PropertyInfo[] properties = null;
            TableInfo info = Get<T>();
            if (info != null)
            {
                properties = info.Properties;
            }
            return properties;
        }

        public static PropertyInfo[] GeProperties(IEntity entity)
        {
            PropertyInfo[] properties = null;
            TableInfo info = Get(entity);
            if (info != null)
            {
                properties = info.Properties;
            }
            return properties;
        }

        public static PropertyInfo[] GeProperties(Type type)
        {
            PropertyInfo[] properties = null;
            TableInfo info = Get(type);
            if (info != null)
            {
                properties = info.Properties;
            }
            return properties;
        }

        public static TableInfo Get<T>() where T: IEntity
        {
            Type type = typeof(T);
            return Get(type);
        }

        public static TableInfo Get(IEntity entity)
        {
            return Get(entity.GetType());
        }

        public static TableInfo Get(Type type)
        {
            return GetTableInfo(type);
        }

        public static Type[] GetAllType()
        {
            return cache.GetKeys();
        }

        public static DataMappingAttribute GetDataMapping(Type type, PropertyInfo property)
        {
            TableInfo info = Get(type);
            if ((info != null) && info.PDRelation.ContainsKey(property))
            {
                return info.PDRelation[property];
            }
            return null;
        }

        public static DataMappingAttribute[] GetDataMappingAttribute<T>() where T: IEntity
        {
            DataMappingAttribute[] dataMappings = null;
            TableInfo info = Get<T>();
            if (info != null)
            {
                dataMappings = info.DataMappings;
            }
            return dataMappings;
        }

        public static DataMappingAttribute[] GetDataMappingAttribute(IEntity entity)
        {
            DataMappingAttribute[] dataMappings = null;
            TableInfo info = Get(entity);
            if (info != null)
            {
                dataMappings = info.DataMappings;
            }
            return dataMappings;
        }

        public static DataMappingAttribute[] GetDataMappingAttribute(Type type)
        {
            DataMappingAttribute[] dataMappings = null;
            TableInfo info = Get(type);
            if (info != null)
            {
                dataMappings = info.DataMappings;
            }
            return dataMappings;
        }

        public static FieldInfo[] GetFieldInfo<T>() where T: IEntity
        {
            FieldInfo[] fieldInfo = null;
            TableInfo info = Get<T>();
            if (info != null)
            {
                fieldInfo = info.FieldInfo;
            }
            return fieldInfo;
        }

        public static FieldInfo[] GetFieldInfo(IEntity entity)
        {
            FieldInfo[] fieldInfo = null;
            TableInfo info = Get(entity);
            if (info != null)
            {
                fieldInfo = info.FieldInfo;
            }
            return fieldInfo;
        }

        public static FieldInfo[] GetFieldInfo(Type type)
        {
            FieldInfo[] fieldInfo = null;
            TableInfo info = Get(type);
            if (info != null)
            {
                fieldInfo = info.FieldInfo;
            }
            return fieldInfo;
        }

        public static TableAttribute GetTableAttribute<T>() where T: IEntity
        {
            TableAttribute table = null;
            TableInfo info = Get<T>();
            if (info != null)
            {
                table = info.Table;
            }
            return table;
        }

        public static TableAttribute GetTableAttribute(IEntity entity)
        {
            TableAttribute table = null;
            TableInfo info = Get(entity);
            if (info != null)
            {
                table = info.Table;
            }
            return table;
        }

        public static TableAttribute GetTableAttribute(Type type)
        {
            TableAttribute table = null;
            TableInfo info = Get(type);
            if (info != null)
            {
                table = info.Table;
            }
            return table;
        }

        private static TableInfo GetTableInfo(Type type)
        {
            TableInfo argValue = cache.Get(type);
            if (argValue == null)
            {
                TableAttribute[] customAttributes = type.GetCustomAttributes(typeof(TableAttribute), false) as TableAttribute[];
                List<DataMappingAttribute> list = new List<DataMappingAttribute>();
                List<LinkTableAttribute> list2 = new List<LinkTableAttribute>();
                List<LinkTablesAttribute> list3 = new List<LinkTablesAttribute>();
                PropertyInfo[] properties = type.GetProperties();
                FieldInfo[] fields = type.GetFields();
                argValue = new TableInfo();
                foreach (PropertyInfo info2 in properties)
                {
                    if (info2.GetCustomAttributes(typeof(DataMappingAttribute), false).Length > 0)
                    {
                        DataMappingAttribute item = info2.GetCustomAttributes(typeof(DataMappingAttribute), false)[0] as DataMappingAttribute;
                        item.PropertyInfo = info2;
                        list.Add(item);
                        argValue.PDRelation.Add(info2, item);
                        argValue.PNameRelation.Add(info2.Name, item);
                        argValue.ANameRelation.Add(item.ColumnName, info2);
                    }
                    if (info2.GetCustomAttributes(typeof(LinkTableAttribute), false).Length > 0)
                    {
                        list2.Add(info2.GetCustomAttributes(typeof(LinkTableAttribute), false)[0] as LinkTableAttribute);
                    }
                    if (info2.GetCustomAttributes(typeof(LinkTablesAttribute), false).Length > 0)
                    {
                        list3.Add(info2.GetCustomAttributes(typeof(LinkTablesAttribute), false)[0] as LinkTablesAttribute);
                    }
                }
                argValue.Table = customAttributes[0];
                argValue.DataMappings = list.ToArray();
                argValue.LinkTable = list2.ToArray();
                argValue.LinkTables = list3.ToArray();
                argValue.Properties = properties;
                argValue.FieldInfo = fields;
                cache.Insert(type, argValue);
            }
            return argValue;
        }

        public static void InsertTableInfo(IEntity entity, TableInfo tableInfo)
        {
            InsertTableInfo(entity.GetType(), tableInfo);
        }

        public static void InsertTableInfo(Type type, TableInfo tableInfo)
        {
            cache.Insert(type, tableInfo);
        }
    }
}

