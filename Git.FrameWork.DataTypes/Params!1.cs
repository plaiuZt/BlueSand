namespace Git.Framework.DataTypes
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class Params<T1>
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T1 <Item1>k__BackingField;

        public T1 Item1 { get; set; }
    }
}

