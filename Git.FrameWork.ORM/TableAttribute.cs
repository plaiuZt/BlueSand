namespace Git.Framework.ORM
{
    using System;

    public sealed class TableAttribute : Attribute
    {
        private string dbName;
        private string information;
        private bool isInternal;
        private Git.Framework.ORM.MapType mapType;
        private string name;
        private string primaryKeyName;

        public TableAttribute()
        {
            this.mapType = Git.Framework.ORM.MapType.Table;
        }

        public TableAttribute(string name, string information, bool isInternal)
        {
            this.mapType = Git.Framework.ORM.MapType.Table;
            this.name = name;
            this.information = information;
            this.isInternal = isInternal;
        }

        public string DbName
        {
            get
            {
                return this.dbName;
            }
            set
            {
                this.dbName = value;
            }
        }

        public string Information
        {
            get
            {
                return this.information;
            }
            set
            {
                this.information = value;
            }
        }

        public bool IsInternal
        {
            get
            {
                return this.isInternal;
            }
            set
            {
                this.isInternal = value;
            }
        }

        public Git.Framework.ORM.MapType MapType
        {
            get
            {
                return this.mapType;
            }
            set
            {
                this.mapType = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public string PrimaryKeyName
        {
            get
            {
                return this.primaryKeyName;
            }
            set
            {
                this.primaryKeyName = value;
            }
        }
    }
}

