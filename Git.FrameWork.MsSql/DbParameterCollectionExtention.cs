using Git.Framework.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace Git.Framework.MsSql
{
    public static class DbParameterCollectionExtention
    {
        public static void AddRange(this DbParameterCollection collection, List<SqlParameter> list)
        {
            list.ThrowIfNullOrEmpty<SqlParameter>("添加占位符缺少必要参数");
            foreach (SqlParameter parameter in list)
            {
                collection.Add(parameter);
            }
        }
    }
}

