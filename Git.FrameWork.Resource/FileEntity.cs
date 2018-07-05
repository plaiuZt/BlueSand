using Git.Framework.Io;
using System;

namespace Git.Framework.Resource
{
    [Serializable]
    public class FileEntity
    {
        private string _browser;
        private string _fileName;
        private string _filePath;
        private EFileType _fileType;

        public FileEntity()
        {
        }

        public FileEntity(string argFileName, string argFilePath, EFileType argFileType, string browser)
        {
            this._fileName = argFileName;
            this._filePath = argFilePath;
            this._fileType = argFileType;
            this._browser = browser;
        }

        public string Browser
        {
            get
            {
                return this._browser;
            }
            set
            {
                this._browser = value;
            }
        }

        public string FileName
        {
            get
            {
                return this._fileName;
            }
            set
            {
                this._fileName = value;
            }
        }

        public string FilePath
        {
            get
            {
                return this._filePath;
            }
            set
            {
                this._filePath = value;
            }
        }

        public EFileType FileType
        {
            get
            {
                return this._fileType;
            }
            set
            {
                this._fileType = value;
            }
        }
    }
}

