using System;
using System.Runtime.CompilerServices;

namespace Git.Framework.Cache
{
    public class CacheBody<T>
    {
        public T Body { get; set; }

        public string DependencyFile { get; set; }

        public DateTime Expiration { get; set; }
    }
}

