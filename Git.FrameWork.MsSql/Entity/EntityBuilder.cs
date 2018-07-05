using Git.Framework.MsSql;
using Git.Framework.ORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Git.Framework.MsSql.Entity
{
    public static class EntityBuilder
    {
        private static readonly Type s_RootType = typeof(object);
        private static object s_SyncMappingInfo = new object();
        private static Dictionary<Type, Dictionary<string, PropertyDataBindingInfo>> s_TypeMappingInfo = new Dictionary<Type, Dictionary<string, PropertyDataBindingInfo>>();
        private static Dictionary<Type, Dictionary<string, DataMappingAttribute>> s_TypePropertyInfo = new Dictionary<Type, Dictionary<string, DataMappingAttribute>>();
        private static Dictionary<Type, List<ReferencedTypeBindingInfo>> s_TypeReferencedList = new Dictionary<Type, List<ReferencedTypeBindingInfo>>();

        private static void AddTypeInfo(Type type)
        {
            Dictionary<string, PropertyDataBindingInfo> dictionary4;
            List<ReferencedTypeBindingInfo> list;
            Dictionary<string, DataMappingAttribute> dictionary5;
            Dictionary<Type, Dictionary<string, PropertyDataBindingInfo>> dictionary = new Dictionary<Type, Dictionary<string, PropertyDataBindingInfo>>(s_TypeMappingInfo);
            Dictionary<Type, List<ReferencedTypeBindingInfo>> dictionary2 = new Dictionary<Type, List<ReferencedTypeBindingInfo>>(s_TypeReferencedList);
            Dictionary<Type, Dictionary<string, DataMappingAttribute>> dictionary3 = new Dictionary<Type, Dictionary<string, DataMappingAttribute>>(s_TypePropertyInfo);
            GetTypeInfo(type, out dictionary4, out list, out dictionary5);
            dictionary[type] = dictionary4;
            dictionary2[type] = list;
            dictionary3[type] = dictionary5;
            s_TypeMappingInfo = dictionary;
            s_TypeReferencedList = dictionary2;
            s_TypePropertyInfo = dictionary3;
        }

        public static T BuildEntity<T>(DataRow dr) where T: class, new()
        {
            return BuildEntity<T>(new DataRowEntitySource(dr), string.Empty);
        }

        internal static T BuildEntity<T>(IDataReader dr) where T: class, new()
        {
            return BuildEntity<T>(new DataReaderEntitySource(dr), string.Empty);
        }

        private static T BuildEntity<T>(IEntityDataSource ds, string prefix) where T: class, new()
        {
            T local = Activator.CreateInstance<T>();
            FillEntity(ds, local, typeof(T), prefix);
            return local;
        }

        private static object BuildEntity(IEntityDataSource ds, Type type, string prefix)
        {
            object obj2 = Activator.CreateInstance(type);
            FillEntity(ds, obj2, type, prefix);
            return obj2;
        }

        public static List<T> BuildEntityList<T>(DataRow[] rows) where T: class, new()
        {
            if (rows == null)
            {
                return new List<T>(0);
            }
            List<T> list = new List<T>(rows.Length);
            foreach (DataRow row in rows)
            {
                list.Add(BuildEntity<T>(row));
            }
            return list;
        }

        public static List<T> BuildEntityList<T>(DataTable table) where T: class, new()
        {
            if (table == null)
            {
                return new List<T>(0);
            }
            List<T> list = new List<T>(table.Rows.Count);
            foreach (DataRow row in table.Rows)
            {
                list.Add(BuildEntity<T>(row));
            }
            return list;
        }

        private static void DoFillEntity(IEntityDataSource ds, object obj, Type type, string prefix)
        {
            foreach (string str in ds)
            {
                string columnName = str.ToUpper();
                if (!string.IsNullOrEmpty(prefix) && columnName.StartsWith(prefix.ToUpper()))
                {
                    columnName = columnName.Substring(prefix.Length);
                }
                if (string.IsNullOrEmpty(prefix))
                {
                    prefix = string.Empty;
                }
                PropertyDataBindingInfo propertyInfo = GetPropertyInfo(type, columnName);
                if ((propertyInfo != null) && (str.ToUpper() == (prefix.ToUpper() + columnName)))
                {
                    if ((ds[str] != DBNull.Value) && ValidateData(propertyInfo, ds[str]))
                    {
                        object obj2 = ds[str];
                        if (propertyInfo.PropertyInfo.PropertyType == typeof(string))
                        {
                            obj2 = obj2.ToString().Trim();
                        }
                        propertyInfo.PropertyInfo.SetValue(obj, obj2, null);
                    }
                    else if (((ds[str] == DBNull.Value) && ValidateData(propertyInfo, ds[str])) && (propertyInfo.PropertyInfo.PropertyType == typeof(string)))
                    {
                        propertyInfo.PropertyInfo.SetValue(obj, string.Empty, null);
                    }
                }
            }
            List<ReferencedTypeBindingInfo> referenceObjects = GetReferenceObjects(type);
            foreach (ReferencedTypeBindingInfo info2 in referenceObjects)
            {
                if (TryFill(ds, info2))
                {
                    info2.PropertyInfo.SetValue(obj, BuildEntity(ds, info2.Type, info2.Prefix), null);
                }
            }
        }

        private static void FillEntity(IEntityDataSource ds, object obj, Type type, string prefix)
        {
            Type baseType = type.BaseType;
            if (!s_RootType.Equals(baseType))
            {
                FillEntity(ds, obj, baseType, prefix);
            }
            DoFillEntity(ds, obj, type, prefix);
        }

        private static string GetBindingColumnName(Type type, string propertyName, string prefix)
        {
            string columnName = null;
            try
            {
                Dictionary<string, DataMappingAttribute> dictionary;
                DataMappingAttribute attribute;
                s_TypePropertyInfo.TryGetValue(type, out dictionary);
                if (dictionary == null)
                {
                    lock (s_SyncMappingInfo)
                    {
                        s_TypePropertyInfo.TryGetValue(type, out dictionary);
                        if (dictionary == null)
                        {
                            AddTypeInfo(type);
                            dictionary = s_TypePropertyInfo[type];
                        }
                    }
                }
                dictionary.TryGetValue(propertyName, out attribute);
                if (attribute == null)
                {
                    columnName = null;
                }
                else
                {
                    columnName = attribute.ColumnName;
                }
            }
            catch
            {
                columnName = null;
            }
            if (columnName == null)
            {
                if (!(s_RootType.Equals(type.BaseType) || s_RootType.Equals(type)))
                {
                    return GetBindingColumnName(type.BaseType, propertyName, prefix);
                }
                return null;
            }
            return (prefix + columnName);
        }

        private static PropertyDataBindingInfo GetPropertyInfo(Type type, string columnName)
        {
            Dictionary<string, PropertyDataBindingInfo> dictionary;
            PropertyDataBindingInfo info;
            try
            {
                s_TypeMappingInfo.TryGetValue(type, out dictionary);
                if (dictionary == null)
                {
                    lock (s_SyncMappingInfo)
                    {
                        s_TypeMappingInfo.TryGetValue(type, out dictionary);
                        if (dictionary == null)
                        {
                            AddTypeInfo(type);
                            dictionary = s_TypeMappingInfo[type];
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
            dictionary.TryGetValue(columnName, out info);
            return info;
        }

        private static List<ReferencedTypeBindingInfo> GetReferenceObjects(Type type)
        {
            List<ReferencedTypeBindingInfo> list;
            s_TypeReferencedList.TryGetValue(type, out list);
            if (list == null)
            {
                lock (s_SyncMappingInfo)
                {
                    s_TypeReferencedList.TryGetValue(type, out list);
                    if (list == null)
                    {
                        AddTypeInfo(type);
                        list = s_TypeReferencedList[type];
                    }
                }
            }
            return list;
        }

        private static void GetTypeInfo(Type type, out Dictionary<string, PropertyDataBindingInfo> dataMappingInfos, out List<ReferencedTypeBindingInfo> referObjs, out Dictionary<string, DataMappingAttribute> propertyInfos)
        {
            PropertyInfo[] properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            dataMappingInfos = new Dictionary<string, PropertyDataBindingInfo>();
            referObjs = new List<ReferencedTypeBindingInfo>();
            propertyInfos = new Dictionary<string, DataMappingAttribute>(new CaseInsensitiveStringEqualityComparer());
            foreach (PropertyInfo info in properties)
            {
                object[] customAttributes = info.GetCustomAttributes(false);
                foreach (object obj2 in customAttributes)
                {
                    if (obj2 is DataMappingAttribute)
                    {
                        DataMappingAttribute mapping = obj2 as DataMappingAttribute;
                        dataMappingInfos[mapping.ColumnName.ToUpper()] = new PropertyDataBindingInfo(mapping, info);
                        propertyInfos.Add(info.Name, mapping);
                    }
                    else if (obj2 is ReferencedEntityAttribute)
                    {
                        ReferencedEntityAttribute attri = obj2 as ReferencedEntityAttribute;
                        referObjs.Add(new ReferencedTypeBindingInfo(attri, info));
                    }
                }
            }
        }

        private static bool TryFill(IEntityDataSource ds, ReferencedTypeBindingInfo refObj)
        {
            if (string.IsNullOrEmpty(refObj.ConditionalProperty))
            {
                return true;
            }
            string columnName = GetBindingColumnName(refObj.Type, refObj.ConditionalProperty, refObj.Prefix);
            if (columnName == null)
            {
                return false;
            }
            return ds.ContainsColumn(columnName);
        }

        private static bool ValidateData(PropertyDataBindingInfo bindingInfo, object dbValue)
        {
            return true;
        }

        private class PropertyDataBindingInfo
        {
            public DataMappingAttribute DataMapping;
            public System.Reflection.PropertyInfo PropertyInfo;

            public PropertyDataBindingInfo(DataMappingAttribute mapping, System.Reflection.PropertyInfo propertyInfo)
            {
                this.DataMapping = mapping;
                this.PropertyInfo = propertyInfo;
            }
        }

        private class ReferencedTypeBindingInfo
        {
            private System.Reflection.PropertyInfo m_PropertyInfo;
            private ReferencedEntityAttribute m_ReferencedEntityAttribute;

            public ReferencedTypeBindingInfo(ReferencedEntityAttribute attri, System.Reflection.PropertyInfo propertyInfo)
            {
                this.m_ReferencedEntityAttribute = attri;
                this.m_PropertyInfo = propertyInfo;
            }

            public string ConditionalProperty
            {
                get
                {
                    return this.m_ReferencedEntityAttribute.ConditionalProperty;
                }
            }

            public string Prefix
            {
                get
                {
                    return this.m_ReferencedEntityAttribute.Prefix;
                }
            }

            public System.Reflection.PropertyInfo PropertyInfo
            {
                get
                {
                    return this.m_PropertyInfo;
                }
            }

            public System.Type Type
            {
                get
                {
                    return this.m_ReferencedEntityAttribute.Type;
                }
            }
        }
    }
}

