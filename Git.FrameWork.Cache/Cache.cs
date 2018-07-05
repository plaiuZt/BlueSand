using System;
using System.Collections.Generic;
using System.Linq;

namespace Git.Framework.Cache
{
    public class Cache : ICache
    {
        private object _object = new object();
        private static Dictionary<object, CacheBody> Dic = null;

        static Cache()
        {
            if (Dic == null)
            {
                Dic = new Dictionary<object, CacheBody>();
            }
        }

        public int Add(object argKey, object argValue)
        {
            lock (this._object)
            {
                if (!((Dic == null) || Dic.ContainsKey(argKey)))
                {
                    CacheBody body = new CacheBody {
                        Body = argValue,
                        Expiration = DateTime.MaxValue,
                        DependencyFile = string.Empty
                    };
                    Dic.Add(argKey, body);
                    return 1;
                }
                return 0;
            }
        }

        public int Add(object argKey, object argValue, DateTime expiration)
        {
            lock (this._object)
            {
                if (!((Dic == null) || Dic.ContainsKey(argKey)))
                {
                    CacheBody body = new CacheBody {
                        Body = argValue,
                        Expiration = expiration,
                        DependencyFile = string.Empty
                    };
                    Dic.Add(argKey, body);
                    return 1;
                }
                return 0;
            }
        }

        public void Clear()
        {
            lock (this._object)
            {
                if (Dic != null)
                {
                    Dic.Clear();
                }
            }
        }

        public object Get(object argKey)
        {
            if ((Dic != null) && Dic.ContainsKey(argKey))
            {
                if (DateTime.Now <= Dic[argKey].Expiration)
                {
                    return Dic[argKey].Body;
                }
                this.Remove(argKey);
            }
            return null;
        }

        public object[] GetKeys()
        {
            if (Dic != null)
            {
                return Dic.Keys.ToArray<object>();
            }
            return null;
        }

        public object[] GetValues()
        {
            if (Dic != null)
            {
                return (from item in Dic.Values
                    where DateTime.Now <= item.Expiration
                    select item.Body).ToArray<object>();
            }
            return null;
        }

        public int Insert(object argKey, object argValue)
        {
            lock (this._object)
            {
                if (Dic != null)
                {
                    if (Dic.ContainsKey(argKey))
                    {
                        Dic[argKey].Body = argValue;
                    }
                    else
                    {
                        CacheBody body = new CacheBody {
                            Body = argValue,
                            Expiration = DateTime.MaxValue,
                            DependencyFile = string.Empty
                        };
                        Dic.Add(argKey, body);
                    }
                    return 1;
                }
                return 0;
            }
        }

        public int Insert(object argKey, object argValue, DateTime expiration)
        {
            lock (this._object)
            {
                if (Dic != null)
                {
                    if (Dic.ContainsKey(argKey))
                    {
                        Dic[argKey].Body = argValue;
                    }
                    else
                    {
                        CacheBody body = new CacheBody {
                            Body = argValue,
                            Expiration = expiration,
                            DependencyFile = string.Empty
                        };
                        Dic.Add(argKey, body);
                    }
                    return 1;
                }
                return 0;
            }
        }

        public int Length()
        {
            if (Dic != null)
            {
                return Dic.Count<KeyValuePair<object, CacheBody>>();
            }
            return 0;
        }

        public int Remove(object argKey)
        {
            lock (this._object)
            {
                if ((Dic != null) && Dic.ContainsKey(argKey))
                {
                    Dic.Remove(argKey);
                    return 1;
                }
                return 0;
            }
        }

        public int Replace(object argKey, object argValue)
        {
            lock (this._object)
            {
                if ((Dic != null) && Dic.ContainsKey(argKey))
                {
                    Dic[argKey].Body = argValue;
                    return 1;
                }
                return 0;
            }
        }

        public int Replace(object argKey, object argValue, DateTime expiration)
        {
            lock (this._object)
            {
                if ((Dic != null) && Dic.ContainsKey(argKey))
                {
                    Dic[argKey].Body = argValue;
                    Dic[argKey].Expiration = expiration;
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

        public object[] Keys
        {
            get
            {
                return this.GetKeys();
            }
        }

        public object[] Values
        {
            get
            {
                return this.GetValues();
            }
        }
    }
}

