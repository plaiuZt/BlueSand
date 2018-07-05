namespace Git.Framework.ORM
{
    using System;
    using System.Data;
    using System.Reflection;

    public class DataMappingAttribute : Attribute
    {
        private bool _isMap;
        private bool m_AutoIncrement;
        private bool m_CanNull;
        private string m_ColumnName;
        private Git.Framework.ORM.ColumnType m_columnType;
        private System.Data.DbType m_DbType;
        private object m_DefaultValue;
        private System.Reflection.FieldInfo m_FieldInfo;
        private int m_Length;
        private bool m_PrimaryKey;
        private System.Reflection.PropertyInfo m_PropertyInfo;

        public DataMappingAttribute()
        {
            this.m_columnType = Git.Framework.ORM.ColumnType.Common;
            this._isMap = false;
        }

        public DataMappingAttribute(string columnName, System.Data.DbType dbType)
        {
            this.m_columnType = Git.Framework.ORM.ColumnType.Common;
            this._isMap = false;
            this.m_ColumnName = columnName;
            this.m_DbType = dbType;
        }

        public bool AutoIncrement
        {
            get
            {
                return this.m_AutoIncrement;
            }
            set
            {
                this.m_AutoIncrement = value;
            }
        }

        public bool CanNull
        {
            get
            {
                return this.m_CanNull;
            }
            set
            {
                this.m_CanNull = value;
            }
        }

        public string ColumnName
        {
            get
            {
                return this.m_ColumnName;
            }
            set
            {
                this.m_ColumnName = value;
            }
        }

        public Git.Framework.ORM.ColumnType ColumnType
        {
            get
            {
                return this.m_columnType;
            }
            set
            {
                this.m_columnType = value;
            }
        }

        public System.Data.DbType DbType
        {
            get
            {
                return this.m_DbType;
            }
            set
            {
                this.m_DbType = value;
            }
        }

        public object DefaultValue
        {
            get
            {
                return this.m_DefaultValue;
            }
            set
            {
                this.m_DefaultValue = value;
            }
        }

        public System.Reflection.FieldInfo FieldInfo
        {
            get
            {
                return this.m_FieldInfo;
            }
            set
            {
                this.m_FieldInfo = value;
            }
        }

        public bool IsMap
        {
            get
            {
                return this._isMap;
            }
            set
            {
                this._isMap = value;
            }
        }

        public int Length
        {
            get
            {
                return this.m_Length;
            }
            set
            {
                this.m_Length = value;
            }
        }

        public bool PrimaryKey
        {
            get
            {
                return this.m_PrimaryKey;
            }
            set
            {
                this.m_PrimaryKey = value;
            }
        }

        public System.Reflection.PropertyInfo PropertyInfo
        {
            get
            {
                return this.m_PropertyInfo;
            }
            set
            {
                this.m_PropertyInfo = value;
            }
        }
    }
}

