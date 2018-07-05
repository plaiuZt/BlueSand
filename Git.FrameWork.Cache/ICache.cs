using System;

namespace Git.Framework.Cache
{
    public interface ICache
    {
        int Add(object argKey, object argValue);
        int Add(object argKey, object argValue, DateTime expiration);
        void Clear();
        object Get(object argKey);
        object[] GetKeys();
        object[] GetValues();
        int Insert(object argKey, object argValue);
        int Insert(object argKey, object argValue, DateTime expiration);
        int Length();
        int Remove(object argKey);
        int Replace(object argKey, object argValue);
        int Replace(object argKey, object argValue, DateTime expiration);

        int Count { get; }

        object[] Keys { get; }

        object[] Values { get; }
    }
}

