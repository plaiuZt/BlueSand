using System;
using System.Data;

namespace Git.Framework.MsSql.DataAccess
{

    public class CustomDataCommand : DataCommand
    {
        internal CustomDataCommand(GitDatabase database) : base(database.ToString(), DbCommandFactory.CreateDbCommand())
        {
        }

        internal CustomDataCommand(GitDatabase database, System.Data.CommandType commandType) : this(database)
        {
            this.CommandType = commandType;
        }

        internal CustomDataCommand(GitDatabase database, System.Data.CommandType commandType, string commandText) : this(database, commandType)
        {
            this.CommandText = commandText;
        }

        internal CustomDataCommand(string database, System.Data.CommandType commandType, string commandText) : base(database, DbCommandFactory.CreateDbCommand())
        {
            this.CommandType = commandType;
            this.CommandText = commandText;
        }

        public void AddInputParameter(string name, DbType dbType)
        {
            base.ActualDatabase.AddInParameter(base.m_DbCommand, name, dbType);
        }

        public void AddInputParameter(string name, DbType dbType, object value)
        {
            base.ActualDatabase.AddInParameter(base.m_DbCommand, name, dbType, value);
        }

        public string CommandText
        {
            get
            {
                return base.m_DbCommand.CommandText;
            }
            set
            {
                base.m_DbCommand.CommandText = value;
            }
        }

        public int CommandTimeout
        {
            get
            {
                return base.m_DbCommand.CommandTimeout;
            }
            set
            {
                base.m_DbCommand.CommandTimeout = value;
            }
        }

        public System.Data.CommandType CommandType
        {
            get
            {
                return base.m_DbCommand.CommandType;
            }
            set
            {
                base.m_DbCommand.CommandType = value;
            }
        }

        public GitDatabase Database
        {
            get
            {
                return (GitDatabase) Enum.Parse(typeof(GitDatabase), base.m_DatabaseName);
            }
            set
            {
                base.m_DatabaseName = value.ToString();
            }
        }
    }
}

