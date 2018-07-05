using System;
using System.Collections.Generic;

namespace Git.Framework.DataTypes.Comparison
{
    public class GenericComparer<T> : IComparer<T> where T: IComparable
    {
        public int Compare(T x, T y)
        {
            if (!typeof(T).IsValueType || (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition().IsAssignableFrom(typeof(Nullable<>))))
            {
                T objB = default(T);
                if (object.Equals(x, objB))
                {
                    return (object.Equals(y, default(T)) ? 0 : -1);
                }
                if (object.Equals(y, default(T)))
                {
                    return -1;
                }
            }
            if (!(x.GetType() == y.GetType()))
            {
                return -1;
            }
            if (x is IComparable<T>)
            {
                return ((IComparable<T>) x).CompareTo(y);
            }
            return x.CompareTo(y);
        }
    }
}

