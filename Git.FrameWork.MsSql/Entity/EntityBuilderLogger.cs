using System;
using System.Diagnostics;
using System.Text;

namespace Git.Framework.MsSql.Entity
{
    internal static class EntityBuilderLogger
    {
        private const int AddTypeInfo = 1;
        private const int GetPropertyBindingInfo = 2;
        private const string LogCategory = "Framework.EntityBuilder";

        [Conditional("TRACE")]
        public static void LogAddTypeInfo(Type type)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Add type info to Cache: ");
            builder.Append(type.ToString());
        }

        [Conditional("TRACE")]
        public static void LogGetPropertyBindingInfoException(Type type, string columnName, Exception e)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Faile to get binding Info. Type: ");
            builder.Append(type.ToString());
            builder.Append(". Column name: ");
            builder.Append(columnName + Environment.NewLine);
            builder.Append("Exception: " + e.ToString());
        }
    }
}

