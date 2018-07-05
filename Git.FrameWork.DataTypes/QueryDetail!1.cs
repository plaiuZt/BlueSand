namespace Git.Framework.DataTypes
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class QueryDetail<T>
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T <Body>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Git.Framework.DataTypes.PageInfo <PageInfo>k__BackingField;

        public T Body { get; set; }

        public Git.Framework.DataTypes.PageInfo PageInfo { get; set; }
    }
}

