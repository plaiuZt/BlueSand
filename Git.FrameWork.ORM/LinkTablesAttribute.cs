namespace Git.Framework.ORM
{
    using System;

    public sealed class LinkTablesAttribute : LinkTableAttribute
    {
        public LinkTablesAttribute()
        {
        }

        public LinkTablesAttribute(string name) : base(name)
        {
        }
    }
}

