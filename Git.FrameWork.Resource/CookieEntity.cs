using System;
using System.Runtime.CompilerServices;

namespace Git.Framework.Resource
{
    [Serializable]
    public class CookieEntity
    {
        public long Expires { get; set; }

        public bool IsEncrypt { get; set; }

        public string Key { get; set; }
    }
}

