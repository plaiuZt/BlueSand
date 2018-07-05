using System;
using System.Runtime.CompilerServices;

namespace Git.Framework.Cache
{
    public class CacheBody
    {
        public object Body { get; set; }

        public string DependencyFile { get; set; }

        public DateTime Expiration { get; set; }
    }
}

