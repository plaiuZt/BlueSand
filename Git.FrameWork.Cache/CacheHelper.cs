using System;
using System.Web;
using System.Web.Caching;

namespace Git.Framework.Cache
{
    public class CacheHelper
    {
        private static System.Web.Caching.Cache cache = HttpRuntime.Cache;

        public static int Add(string argKey, object argValue)
        {
            cache.Add(argKey, argValue, null, DateTime.MaxValue, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            return 1;
        }

        public static int Add(string argKey, object argValue, CacheDependency argDependency)
        {
            cache.Add(argKey, argValue, argDependency, DateTime.MaxValue, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            return 1;
        }

        public static int Add(string argKey, object argValue, CacheDependency argDependency, DateTime argExpiration)
        {
            cache.Add(argKey, argValue, argDependency, argExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            return 0;
        }

        public static int Count()
        {
            return cache.Count;
        }

        public static object Get(string argKey)
        {
            return cache[argKey];
        }

        public static T Get<T>(string argKey)
        {
            if (cache[argKey] != null)
            {
                return (T) cache[argKey];
            }
            return default(T);
        }

        public static int Insert(string argKey, object argValue)
        {
            cache.Insert(argKey, argValue);
            return 1;
        }

        public static int Insert(string argKey, object argValue, CacheDependency argDependency)
        {
            cache.Insert(argKey, argValue, argDependency);
            return 1;
        }

        public static int Insert(string argKey, object argValue, CacheDependency argDependency, DateTime argExpiration)
        {
            cache.Insert(argKey, argValue, argDependency, argExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
            return 1;
        }

        public static int Remove(string argKey)
        {
            cache.Remove(argKey);
            return 1;
        }
    }
}

