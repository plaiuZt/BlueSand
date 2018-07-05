namespace Git.Framework.DataTypes.ExtensionMethods
{
    using Git.Framework.DataTypes.Comparison;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class IComparableExtensions
    {
        public static bool Between<T>(this T Value, T Min, T Max, IComparer<T> Comparer = null) where T: IComparable
        {
            Comparer = Comparer.NullCheck<IComparer<T>>(new Git.Framework.DataTypes.Comparison.GenericComparer<T>());
            return ((Comparer.Compare(Max, Value) >= 0) && (Comparer.Compare(Value, Min) >= 0));
        }
    }
}

