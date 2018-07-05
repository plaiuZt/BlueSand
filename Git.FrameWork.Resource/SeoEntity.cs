using System;

namespace Git.Framework.Resource
{
    [Serializable]
    public class SeoEntity
    {
        private string _description;
        private string _key;
        private string _keyWords;
        private string _title;

        public SeoEntity()
        {
        }

        public SeoEntity(string argKey, string argTitle, string argKeyWords, string argDescription)
        {
            this._key = argKey;
            this._title = argTitle;
            this._keyWords = argKeyWords;
            this._description = argDescription;
        }

        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
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

        public string KeyWords
        {
            get
            {
                return this._keyWords;
            }
            set
            {
                this._keyWords = value;
            }
        }

        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }
    }
}

