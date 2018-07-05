using System;

namespace Git.Framework.MsSql.DataAccess
{
    public class DataCommandFileLoadException : Exception
    {
        public DataCommandFileLoadException(string fileName) : base("DataCommand file " + fileName + " not found or is invalid.")
        {
        }
    }
}

