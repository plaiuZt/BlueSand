using System;

namespace Git.Framework.Io
{
    [Serializable]
    public class FileItem
    {
        private DateTime _creationDate;
        private int _fileCount;
        private string _fullName;
        private bool _isFolder;
        private DateTime _lastAccessDate;
        private DateTime _lastWriteDate;
        private string _name;
        private long _size;
        private int _subFolderCount;

        public DateTime CreationDate
        {
            get
            {
                return this._creationDate;
            }
            set
            {
                this._creationDate = value;
            }
        }

        public int FileCount
        {
            get
            {
                return this._fileCount;
            }
            set
            {
                this._fileCount = value;
            }
        }

        public string FullName
        {
            get
            {
                return this._fullName;
            }
            set
            {
                this._fullName = value;
            }
        }

        public bool IsFolder
        {
            get
            {
                return this._isFolder;
            }
            set
            {
                this._isFolder = value;
            }
        }

        public DateTime LastAccessDate
        {
            get
            {
                return this._lastAccessDate;
            }
            set
            {
                this._lastAccessDate = value;
            }
        }

        public DateTime LastWriteDate
        {
            get
            {
                return this._lastWriteDate;
            }
            set
            {
                this._lastWriteDate = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public long Size
        {
            get
            {
                return this._size;
            }
            set
            {
                this._size = value;
            }
        }

        public int SubFolderCount
        {
            get
            {
                return this._subFolderCount;
            }
            set
            {
                this._subFolderCount = value;
            }
        }
    }
}

