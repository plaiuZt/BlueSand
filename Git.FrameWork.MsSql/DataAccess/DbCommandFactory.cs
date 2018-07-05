using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Git.Framework.MsSql.DataAccess
{
    internal static class DbCommandFactory
    {
        public static DbCommand CreateDbCommand()
        {
            return new SqlCommand();
        }
    }
}

