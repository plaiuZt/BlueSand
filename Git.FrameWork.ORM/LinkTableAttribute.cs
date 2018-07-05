namespace Git.Framework.ORM
{
    using System;

    public class LinkTableAttribute : Attribute
    {
        private Type _tableType;
        private string name;
        private string sqlPrefix;

        public LinkTableAttribute()
        {
        }

        public LinkTableAttribute(string name)
        {
            this.name = name;
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

        public string SqlPrefix
        {
            get
            {
                return this.sqlPrefix;
            }
            set
            {
                this.sqlPrefix = value;
            }
        }

        public Type TableType
        {
            get
            {
                return this._tableType;
            }
            set
            {
                this._tableType = value;
            }
        }
    }
}

