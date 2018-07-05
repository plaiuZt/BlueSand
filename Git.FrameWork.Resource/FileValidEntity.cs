using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Git.Framework.Resource
{
    [Serializable]
    public class FileValidEntity
    {
        public List<string> ContentType { get; set; }

        public List<string> FileExtension { get; set; }

        public string FilePath { get; set; }

        public List<int[]> FileSize { get; set; }

        public string FileType { get; set; }

        public int Length { get; set; }

        public int SourceID { get; set; }
    }
}

