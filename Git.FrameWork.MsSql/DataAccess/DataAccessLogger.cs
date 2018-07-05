using Git.Framework.Log;
using System;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Git.Framework.MsSql.DataAccess
{
    internal static class DataAccessLogger
    {
        private const int DBFileChanged = 10;
        private const int EXECUTION_ERROR = 20;
        private const int LoadCommandFile = 3;
        private const int LoadCommandInventoryFile = 2;
        private const int LoadDatabaseFile = 1;
        private static Git.Framework.Log.Log log = Git.Framework.Log.Log.Instance(typeof(DataAccessLogger));
        private const string LOG_CATEGORY_NAME = "Git.Framework.DataAccess";

        [Conditional("TRACE")]
        public static void LogDatabaseCommandFileLoaded(string fileName)
        {
            string message = "Data command file loaded: " + fileName;
            LogEvent(3, message);
        }

        [Conditional("TRACE")]
        public static void LogDatabaseFileChanged(FileSystemEventArgs arg)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("File name: " + arg.FullPath + Environment.NewLine);
            builder.Append("Change type: " + arg.ChangeType.ToString());
            LogEvent(10, builder.ToString());
        }

        [Conditional("TRACE")]
        public static void LogDatabaseFileLoaded(string fileName)
        {
            string message = "Database config file loaded: " + fileName;
            LogEvent(1, message);
        }

        [Conditional("TRACE")]
        public static void LogDataCommandInventoryFileLoaded(string fileName, int count)
        {
            string message = "Data command inventory file loaded: " + fileName + ". " + count.ToString() + " command file(s) found.";
            LogEvent(2, message);
        }

        private static void LogEvent(int eventId, string message)
        {
            if (20 == eventId)
            {
                log.Error(eventId + "  " + message);
            }
            else
            {
                log.Info(eventId + "  " + message);
            }
        }

        [Conditional("TRACE")]
        public static void LogExecutionError(DbCommand cmd, Exception ex)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\tDataCommand Execution error, command text:");
            builder.Append(Environment.NewLine);
            builder.Append(cmd.CommandText);
            builder.Append(Environment.NewLine);
            if (cmd != null)
            {
                builder.Append("command parameters inforamtion:");
                builder.Append(Environment.NewLine);
                for (int i = 0; i < cmd.Parameters.Count; i++)
                {
                    builder.AppendFormat("parameters name:{0}, parameters value:{1}, parameters type:{2}", cmd.Parameters[i].ParameterName, cmd.Parameters[i].Value, cmd.Parameters[i].DbType);
                    builder.Append(Environment.NewLine);
                }
            }
            builder.Append(Environment.NewLine);
            builder.Append("\tException: ");
            builder.Append(ex.ToString());
            LogEvent(20, builder.ToString());
        }
    }
}

