namespace Git.Framework.ORM
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class TableInfo
    {
        public IDictionary<string, PropertyInfo> ANameRelation = new Dictionary<string, PropertyInfo>();
        public IDictionary<PropertyInfo, DataMappingAttribute> PDRelation = new Dictionary<PropertyInfo, DataMappingAttribute>();
        public IDictionary<string, DataMappingAttribute> PNameRelation = new Dictionary<string, DataMappingAttribute>();

        public DataMappingAttribute[] DataMappings { get; set; }

        public System.Reflection.FieldInfo[] FieldInfo { get; set; }

        public LinkTableAttribute[] LinkTable { get; set; }

        public LinkTablesAttribute[] LinkTables { get; set; }

        public PropertyInfo[] Properties { get; set; }

        public TableAttribute Table { get; set; }
    }
}

