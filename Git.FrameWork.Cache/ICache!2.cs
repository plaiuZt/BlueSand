using System;

namespace Git.Framework.Cache
{
    public interface ICache<T, V>
    {
        int Add(T argKey, V argValue);
        int Add(T argKey, V argValue, DateTime expiration);
        void Clear();
        V Get(T argKey);
        T[] GetKeys();
        V[] GetValues();
        int Insert(T argKey, V argValue);
        int Insert(T argKey, V argValue, DateTime expiration);
        int Length();
        int Remove(T argKey);
        int Replace(T argKey, V argValue);
        int Replace(T argKey, V argValue, DateTime expiration);

        int Count { get; }

        T[] Keys { get; }

        V[] Values { get; }
    }
}

