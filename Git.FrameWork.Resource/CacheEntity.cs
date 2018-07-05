using System;

namespace Git.Framework.Resource
{
    [Serializable]
    public class CacheEntity
    {
        private bool _autoLoad;
        private string _dependencies;
        private int _expires;
        private string _key;

        public CacheEntity()
        {
        }

        public CacheEntity(string key, bool autoLoad, int expires, string dependencies)
        {
            this._key = key;
            this._autoLoad = autoLoad;
            this._expires = expires;
            this._dependencies = dependencies;
        }

        public bool AutoLoad
        {
            get
            {
                return this._autoLoad;
            }
            set
            {
                this._autoLoad = value;
            }
        }

        public string Dependencies
        {
            get
            {
                return this._dependencies;
            }
            set
            {
                this._dependencies = value;
            }
        }

        public int Expires
        {
            get
            {
                return this._expires;
            }
            set
            {
                this._expires = value;
            }
        }

        public string Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }
    }
}

