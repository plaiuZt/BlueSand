﻿namespace Git.Framework.DataTypes
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class Params<T1, T2>
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T1 <Item1>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T2 <Item2>k__BackingField;

        public T1 Item1 { get; set; }

        public T2 Item2 { get; set; }
    }
}
