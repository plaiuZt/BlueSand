using Git.Framework.Cache;
using System;
using System.Collections.Generic;

namespace Git.Framework.Resource
{
    [Serializable]
    public class FileGroup
    {
        private List<string> _fileKeys;
        private List<FileEntity> _fileList;
        private string _key;

        public FileGroup()
        {
        }

        public FileGroup(string argKey, List<FileEntity> argFileList)
        {
            this._key = argKey;
            this._fileList = argFileList;
        }

        public List<string> FileKeys
        {
            get
            {
                return this._fileKeys;
            }
            set
            {
                this._fileKeys = value;
            }
        }

        public List<FileEntity> FileList
        {
            get
            {
                if ((this.FileKeys != null) && (this.FileKeys.Count > 0))
                {
                    ICache<string, FileEntity> cache = new Git.Framework.Cache.Cache<string, FileEntity>();
                    this._fileList = new List<FileEntity>();
                    foreach (string str in this.FileKeys)
                    {
                        this._fileList.Add(cache.Get(str));
                    }
                }
                return this._fileList;
            }
            set
            {
                this._fileList = value;
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

