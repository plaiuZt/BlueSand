namespace Git.Framework.DataTypes.ExtensionMethods
{
    using Git.Framework.DataTypes.Comparison;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class IDictionaryExtensions
    {
        public static IDictionary<TKey, TValue> CopyTo<TKey, TValue>(this IDictionary<TKey, TValue> Dictionary, IDictionary<TKey, TValue> Target)
        {
            foreach (KeyValuePair<TKey, TValue> pair in Dictionary)
            {
                Target.SetValue<TKey, TValue>(pair.Key, pair.Value);
            }
            return Dictionary;
        }

        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> Dictionary, TKey Key, TValue Default = null)
        {
            TValue local = Default;
            return (Dictionary.TryGetValue(Key, out local) ? local : Default);
        }

        public static IDictionary<TKey, TValue> SetValue<TKey, TValue>(this IDictionary<TKey, TValue> Dictionary, TKey Key, TValue Value)
        {
            if (Dictionary.ContainsKey(Key))
            {
                Dictionary[Key] = Value;
                return Dictionary;
            }
            Dictionary.Add(Key, Value);
            return Dictionary;
        }

        public static IDictionary<T1, T2> Sort<T1, T2>(this IDictionary<T1, T2> Dictionary, IComparer<T1> Comparer = null) where T1: IComparable
        {
            Dictionary.ThrowIfNull("Dictionary");
            return new SortedDictionary<T1, T2>(Dictionary, Comparer.NullCheck<IComparer<T1>>(new Git.Framework.DataTypes.Comparison.GenericComparer<T1>()));
        }

        public static IDictionary<T1, T2> SortByValue<T1, T2>(this IDictionary<T1, T2> Dictionary, IComparer<T1> Comparer = null) where T1: IComparable
        {
            Dictionary.ThrowIfNull("Dictionary");
            return Enumerable.ToDictionary<KeyValuePair<T1, T2>, T1, T2>(Enumerable.OrderBy<KeyValuePair<T1, T2>, T2>(new SortedDictionary<T1, T2>(Dictionary, Comparer.NullCheck<IComparer<T1>>(new Git.Framework.DataTypes.Comparison.GenericComparer<T1>())), <>c__1<T1, T2>.<>9__1_0 ?? (<>c__1<T1, T2>.<>9__1_0 = new Func<KeyValuePair<T1, T2>, T2>(<>c__1<T1, T2>.<>9.<SortByValue>b__1_0))), <>c__1<T1, T2>.<>9__1_1 ?? (<>c__1<T1, T2>.<>9__1_1 = new Func<KeyValuePair<T1, T2>, T1>(<>c__1<T1, T2>.<>9.<SortByValue>b__1_1)), <>c__1<T1, T2>.<>9__1_2 ?? (<>c__1<T1, T2>.<>9__1_2 = new Func<KeyValuePair<T1, T2>, T2>(<>c__1<T1, T2>.<>9.<SortByValue>b__1_2)));
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c__1<T1, T2> where T1: IComparable
        {
            public static readonly IDictionaryExtensions.<>c__1<T1, T2> <>9;
            public static Func<KeyValuePair<T1, T2>, T2> <>9__1_0;
            public static Func<KeyValuePair<T1, T2>, T1> <>9__1_1;
            public static Func<KeyValuePair<T1, T2>, T2> <>9__1_2;

            static <>c__1()
            {
                IDictionaryExtensions.<>c__1<T1, T2>.<>9 = new IDictionaryExtensions.<>c__1<T1, T2>();
            }

            internal T2 <SortByValue>b__1_0(KeyValuePair<T1, T2> x)
            {
                return x.Value;
            }

            internal T1 <SortByValue>b__1_1(KeyValuePair<T1, T2> x)
            {
                return x.Key;
            }

            internal T2 <SortByValue>b__1_2(KeyValuePair<T1, T2> x)
            {
                return x.Value;
            }
        }
    }
}

