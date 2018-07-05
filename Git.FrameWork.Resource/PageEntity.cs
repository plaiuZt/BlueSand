using Git.Framework.Cache;
using System;
using System.Collections.Generic;

namespace Git.Framework.Resource
{
    [Serializable]
    public class PageEntity
    {
        private List<string> _fileGroup;
        private List<Git.Framework.Resource.FileGroup> _fileGroupList;
        private string _key;
        private string _pagePath;
        private Git.Framework.Resource.SeoEntity _seoEntity;
        private string _seoKey;

        public PageEntity()
        {
        }

        public PageEntity(string argKey, string argPagePath, List<Git.Framework.Resource.FileGroup> argFileGroupList, Git.Framework.Resource.SeoEntity argSeoEntity)
        {
            this._key = argKey;
            this._pagePath = argPagePath;
            this._fileGroupList = argFileGroupList;
            this._seoEntity = argSeoEntity;
        }

        public List<string> FileGroup
        {
            get
            {
                return this._fileGroup;
            }
            set
            {
                this._fileGroup = value;
            }
        }

        public List<Git.Framework.Resource.FileGroup> FileGroupList
        {
            get
            {
                if ((this.FileGroup != null) && (this.FileGroup.Count > 0))
                {
                    this._fileGroupList = new List<Git.Framework.Resource.FileGroup>();
                    ICache<string, Git.Framework.Resource.FileGroup> cache = new Git.Framework.Cache.Cache<string, Git.Framework.Resource.FileGroup>();
                    foreach (string str in this.FileGroup)
                    {
                        this._fileGroupList.Add(cache.Get(str));
                    }
                }
                return this._fileGroupList;
            }
            set
            {
                this._fileGroupList = value;
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

        public string PagePath
        {
            get
            {
                return this._pagePath;
            }
            set
            {
                this._pagePath = value;
            }
        }

        public Git.Framework.Resource.SeoEntity SeoEntity
        {
            get
            {
                if (!string.IsNullOrEmpty(this.SeoKey))
                {
                    ICache<string, Git.Framework.Resource.SeoEntity> cache = new Git.Framework.Cache.Cache<string, Git.Framework.Resource.SeoEntity>();
                    this._seoEntity = cache.Get(this.SeoKey);
                }
                return this._seoEntity;
            }
            set
            {
                this._seoEntity = value;
            }
        }

        public string SeoKey
        {
            get
            {
                return this._seoKey;
            }
            set
            {
                this._seoKey = value;
            }
        }
    }
}

