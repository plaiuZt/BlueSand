using System;
using System.Collections.Generic;
using System.Linq;

namespace Git.Framework.Cache
{
    public class Cache<T, V> : ICache<T, V>
    {
        private object _object;
        private static Dictionary<T, CacheBody<V>> Dic;

        static Cache()
        {
            Git.Framework.Cache.Cache<T, V>.Dic = null;
            Git.Framework.Cache.Cache<T, V>.Dic = new Dictionary<T, CacheBody<V>>();
        }

        public Cache()
        {
            this._object = new object();
        }

        public int Add(T argKey, V argValue)
        {
            lock (this._object)
            {
                if (!((Git.Framework.Cache.Cache<T, V>.Dic == null) || Git.Framework.Cache.Cache<T, V>.Dic.ContainsKey(argKey)))
                {
                    CacheBody<V> body = new CacheBody<V> {
                        Body = argValue,
                        DependencyFile = string.Empty,
                        Expiration = DateTime.MaxValue
                    };
                    Git.Framework.Cache.Cache<T, V>.Dic.Add(argKey, body);
                    return 1;
                }
                return 0;
            }
        }

        public int Add(T argKey, V argValue, DateTime expiration)
        {
            lock (this._object)
            {
                if (!((Git.Framework.Cache.Cache<T, V>.Dic == null) || Git.Framework.Cache.Cache<T, V>.Dic.ContainsKey(argKey)))
                {
                    CacheBody<V> body = new CacheBody<V> {
                        Body = argValue,
                        DependencyFile = string.Empty,
                        Expiration = expiration
                    };
                    Git.Framework.Cache.Cache<T, V>.Dic.Add(argKey, body);
                    return 1;
                }
                return 0;
            }
        }

        public void Clear()
        {
            if (Git.Framework.Cache.Cache<T, V>.Dic != null)
            {
                Git.Framework.Cache.Cache<T, V>.Dic.Clear();
            }
        }

        public V Get(T argKey)
        {
            if ((Git.Framework.Cache.Cache<T, V>.Dic != null) && Git.Framework.Cache.Cache<T, V>.Dic.ContainsKey(argKey))
            {
                if (DateTime.Now <= Git.Framework.Cache.Cache<T, V>.Dic[argKey].Expiration)
                {
                    return Git.Framework.Cache.Cache<T, V>.Dic[argKey].Body;
                }
                this.Remove(argKey);
            }
            return default(V);
        }

        public T[] GetKeys()
        {
            if (Git.Framework.Cache.Cache<T, V>.Dic != null)
            {
                return Git.Framework.Cache.Cache<T, V>.Dic.Keys.ToArray<T>();
            }
            return null;
        }

        public V[] GetValues()
        {
            if (Git.Framework.Cache.Cache<T, V>.Dic != null)
            {
                return (from item in Git.Framework.Cache.Cache<T, V>.Dic.Values
                    where item.Expiration >= DateTime.Now
                    select item.Body).ToArray<V>();
            }
            return null;
        }

        public int Insert(T argKey, V argValue)
        {
            lock (this._object)
            {
                if (Git.Framework.Cache.Cache<T, V>.Dic != null)
                {
                    if (Git.Framework.Cache.Cache<T, V>.Dic.ContainsKey(argKey))
                    {
                        Git.Framework.Cache.Cache<T, V>.Dic[argKey].Body = argValue;
                    }
                    else
                    {
                        CacheBody<V> body = new CacheBody<V> {
                            Body = argValue,
                            DependencyFile = string.Empty,
                            Expiration = DateTime.MaxValue
                        };
                        Git.Framework.Cache.Cache<T, V>.Dic.Add(argKey, body);
                    }
                    return 1;
                }
                return 0;
            }
        }

        public int Insert(T argKey, V argValue, DateTime expiration)
        {
            lock (this._object)
            {
                if (Git.Framework.Cache.Cache<T, V>.Dic != null)
                {
                    if (Git.Framework.Cache.Cache<T, V>.Dic.ContainsKey(argKey))
                    {
                        Git.Framework.Cache.Cache<T, V>.Dic[argKey].Body = argValue;
                    }
                    else
                    {
                        CacheBody<V> body = new CacheBody<V> {
                            Body = argValue,
                            DependencyFile = string.Empty,
                            Expiration = expiration
                        };
                        Git.Framework.Cache.Cache<T, V>.Dic.Add(argKey, body);
                    }
                    return 1;
                }
                return 0;
            }
        }

        public int Length()
        {
            if (Git.Framework.Cache.Cache<T, V>.Dic != null)
            {
                return Git.Framework.Cache.Cache<T, V>.Dic.Count;
            }
            return 0;
        }

        public int Remove(T argKey)
        {
            lock (this._object)
            {
                if ((Git.Framework.Cache.Cache<T, V>.Dic != null) && Git.Framework.Cache.Cache<T, V>.Dic.ContainsKey(argKey))
                {
                    Git.Framework.Cache.Cache<T, V>.Dic.Remove(argKey);
                    return 1;
                }
                return 0;
            }
        }

        public int Replace(T argKey, V argValue)
        {
            lock (this._object)
            {
                if ((Git.Framework.Cache.Cache<T, V>.Dic != null) && Git.Framework.Cache.Cache<T, V>.Dic.ContainsKey(argKey))
                {
                    Git.Framework.Cache.Cache<T, V>.Dic[argKey].Body = argValue;
                    return 1;
                }
                return 0;
            }
        }

        public int Replace(T argKey, V argValue, DateTime expiration)
        {
            lock (this._object)
            {
                if ((Git.Framework.Cache.Cache<T, V>.Dic != null) && Git.Framework.Cache.Cache<T, V>.Dic.ContainsKey(argKey))
                {
                    Git.Framework.Cache.Cache<T, V>.Dic[argKey].Body = argValue;
                    Git.Framework.Cache.Cache<T, V>.Dic[argKey].Expiration = expiration;
                    return 1;
                }
                return 0;
            }
        }

        public int Count
        {
            get
            {
                return this.Length();
            }
        }

        public T[] Keys
        {
            get
            {
                return this.GetKeys();
            }
        }

        public V[] Values
        {
            get
            {
                return this.GetValues();
            }
        }
    }
}

