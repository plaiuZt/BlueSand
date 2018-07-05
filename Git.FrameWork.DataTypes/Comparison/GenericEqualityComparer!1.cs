using System;
using System.Collections;
using System.Collections.Generic;

namespace Git.Framework.DataTypes.Comparison
{
    public class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            if (!typeof(T).IsValueType || (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition().IsAssignableFrom(typeof(Nullable<>))))
            {
                T objB = default(T);
                if (object.Equals(x, objB))
                {
                    return object.Equals(y, default(T));
                }
                if (object.Equals(y, default(T)))
                {
                    return false;
                }
            }
            if (!(x.GetType() == y.GetType()))
            {
                return false;
            }
            if (!((x is IEnumerable) && (y is IEnumerable)))
            {
                if (x is IEquatable<T>)
                {
                    return ((IEquatable<T>) x).Equals(y);
                }
                if (x is IComparable<T>)
                {
                    return (((IComparable<T>) x).CompareTo(y) == 0);
                }
                if (x is IComparable)
                {
                    return (((IComparable) x).CompareTo(y) == 0);
                }
                return x.Equals(y);
            }
            Git.Framework.DataTypes.Comparison.GenericEqualityComparer<object> comparer = new Git.Framework.DataTypes.Comparison.GenericEqualityComparer<object>();
            IEnumerator enumerator = ((IEnumerable) x).GetEnumerator();
            IEnumerator enumerator2 = ((IEnumerable) y).GetEnumerator();
            while (true)
            {
                bool flag7 = !enumerator.MoveNext();
                bool flag8 = !enumerator2.MoveNext();
                if (flag7 | flag8)
                {
                    return (flag7 & flag8);
                }
                if (!comparer.Equals(enumerator.Current, enumerator2.Current))
                {
                    return false;
                }
            }
        }

        public int GetHashCode(T obj)
        {
            throw new NotImplementedException();
        }
    }
}

