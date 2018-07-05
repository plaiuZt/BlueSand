using Git.Framework.Log;
using Git.Framework.MsSql.Entity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Git.Framework.MsSql.DataAccess
{
    public class DataCommand : ICloneable
    {
        private Git.Framework.Log.Log log;
        protected string m_DatabaseName;
        protected DbCommand m_DbCommand;

        private DataCommand()
        {
            this.log = Git.Framework.Log.Log.Instance(typeof(DataCommand));
        }

        internal DataCommand(string databaseName, DbCommand command)
        {
            this.log = Git.Framework.Log.Log.Instance(typeof(DataCommand));
            this.m_DatabaseName = databaseName;
            this.m_DbCommand = command;
            this.m_DbCommand.CommandTimeout = 600;
        }

        public void AddParameterValue(string paramName, object val, DbType dbType)
        {
            SqlParameter parameter = new SqlParameter(paramName, val) {
                DbType = dbType
            };
            this.m_DbCommand.Parameters.Add(parameter);
        }

        public object Clone()
        {
            DataCommand command = new DataCommand();
            if (this.m_DbCommand != null)
            {
                if (!(this.m_DbCommand is ICloneable))
                {
                    throw new ApplicationException("A class that implements IClonable is expected.");
                }
                command.m_DbCommand = ((ICloneable) this.m_DbCommand).Clone() as DbCommand;
            }
            command.m_DatabaseName = this.m_DatabaseName;
            return command;
        }

        private void CloseConnection(DbTransaction tran)
        {
            if (tran != null)
            {
                tran.Rollback();
                if ((tran.Connection != null) && (tran.Connection.State == ConnectionState.Open))
                {
                    tran.Connection.Close();
                }
            }
        }

        public IDataReader ExecuteDataReader()
        {
            return this.ExecuteDataReader(false);
        }

        public IDataReader ExecuteDataReader(bool isOpenTrans)
        {
            DbTransaction transaction = null;
            IDataReader reader;
            try
            {
                if (isOpenTrans)
                {
                    DbConnection connection = this.ActualDatabase.CreateConnection();
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    return this.ActualDatabase.ExecuteReader(this.m_DbCommand, transaction);
                }
                reader = this.ActualDatabase.ExecuteReader(this.m_DbCommand);
            }
            catch (Exception exception)
            {
                this.CloseConnection(transaction);
                DataAccessLogger.LogExecutionError(this.m_DbCommand, exception);
                this.log.Error(exception.Message);
                throw;
            }
            return reader;
        }

        public DataSet ExecuteDataSet()
        {
            return this.ExecuteDataSet(false);
        }

        public DataSet ExecuteDataSet(bool isOpenTrans)
        {
            DbTransaction transaction = null;
            DataSet set;
            try
            {
                if (isOpenTrans)
                {
                    DbConnection connection = this.ActualDatabase.CreateConnection();
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    return this.ActualDatabase.ExecuteDataSet(this.m_DbCommand, transaction);
                }
                set = this.ActualDatabase.ExecuteDataSet(this.m_DbCommand);
            }
            catch (Exception exception)
            {
                this.CloseConnection(transaction);
                DataAccessLogger.LogExecutionError(this.m_DbCommand, exception);
                this.log.Error(exception.Message);
                throw;
            }
            return set;
        }

        public T ExecuteEntity<T>() where T: class, new()
        {
            return this.ExecuteEntity<T>(false);
        }

        public T ExecuteEntity<T>(bool isOpenTrans) where T: class, new()
        {
            IDataReader dr = null;
            DbTransaction transaction = null;
            T local;
            try
            {
                if (isOpenTrans)
                {
                    DbConnection connection = this.ActualDatabase.CreateConnection();
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    dr = this.ActualDatabase.ExecuteReader(this.m_DbCommand, transaction);
                    transaction.Commit();
                }
                else
                {
                    dr = this.ActualDatabase.ExecuteReader(this.m_DbCommand);
                }
                if (dr.Read())
                {
                    return EntityBuilder.BuildEntity<T>(dr);
                }
                local = default(T);
            }
            catch (Exception exception)
            {
                this.CloseConnection(transaction);
                DataAccessLogger.LogExecutionError(this.m_DbCommand, exception);
                this.log.Error(exception.Message);
                throw;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Dispose();
                }
            }
            return local;
        }

        public K ExecuteEntityCollection<T, K>() where T: class, new() where K: ICollection<T>, new()
        {
            return this.ExecuteEntityCollection<T, K>(false);
        }

        public K ExecuteEntityCollection<T, K>(bool isOpenTrans) where T: class, new() where K: ICollection<T>, new()
        {
            IDataReader dr = null;
            DbTransaction transaction = null;
            K local2;
            try
            {
                if (isOpenTrans)
                {
                    DbConnection connection = this.ActualDatabase.CreateConnection();
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    dr = this.ActualDatabase.ExecuteReader(this.m_DbCommand, transaction);
                    transaction.Commit();
                }
                else
                {
                    dr = this.ActualDatabase.ExecuteReader(this.m_DbCommand);
                }
                ICollection<T> is2 = new K();
                while (dr.Read())
                {
                    T item = EntityBuilder.BuildEntity<T>(dr);
                    is2.Add(item);
                }
                local2 = (K) is2;
            }
            catch (Exception exception)
            {
                this.CloseConnection(transaction);
                DataAccessLogger.LogExecutionError(this.m_DbCommand, exception);
                this.log.Error(exception.Message);
                throw;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Dispose();
                }
            }
            return local2;
        }

        public List<T> ExecuteEntityList<T>() where T: class, new()
        {
            return this.ExecuteEntityList<T>(false);
        }

        public List<T> ExecuteEntityList<T>(bool isOpenTrans) where T: class, new()
        {
            IDataReader dr = null;
            DbTransaction transaction = null;
            List<T> list2;
            try
            {
                if (isOpenTrans)
                {
                    DbConnection connection = this.ActualDatabase.CreateConnection();
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    dr = this.ActualDatabase.ExecuteReader(this.m_DbCommand, transaction);
                    transaction.Commit();
                }
                else
                {
                    dr = this.ActualDatabase.ExecuteReader(this.m_DbCommand);
                }
                List<T> list = new List<T>();
                while (dr.Read())
                {
                    T item = EntityBuilder.BuildEntity<T>(dr);
                    list.Add(item);
                }
                list2 = list;
            }
            catch (Exception exception)
            {
                this.CloseConnection(transaction);
                DataAccessLogger.LogExecutionError(this.m_DbCommand, exception);
                this.log.Error(exception.Message);
                throw;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Dispose();
                }
            }
            return list2;
        }

        public int ExecuteNonQuery()
        {
            return this.ExecuteNonQuery(false);
        }

        public int ExecuteNonQuery(bool isOpenTrans)
        {
            DbTransaction tran = null;
            int num2;
            try
            {
                if (isOpenTrans)
                {
                    DbConnection connection = this.ActualDatabase.CreateConnection();
                    connection.Open();
                    tran = connection.BeginTransaction();
                    int num = this.ActualDatabase.ExecuteNonQuery(this.m_DbCommand);
                    tran.Commit();
                    return num;
                }
                num2 = this.ActualDatabase.ExecuteNonQuery(this.m_DbCommand);
            }
            catch (Exception exception)
            {
                this.CloseConnection(tran);
                DataAccessLogger.LogExecutionError(this.m_DbCommand, exception);
                this.log.Error(exception.Message);
                throw;
            }
            return num2;
        }

        public object ExecuteScalar()
        {
            return this.ExecuteScalar(false);
        }

        public T ExecuteScalar<T>()
        {
            return this.ExecuteScalar<T>(false);
        }

        public object ExecuteScalar(bool isOpenTrans)
        {
            DbTransaction tran = null;
            object obj3;
            try
            {
                if (isOpenTrans)
                {
                    DbConnection connection = this.ActualDatabase.CreateConnection();
                    connection.Open();
                    tran = connection.BeginTransaction();
                    object obj2 = this.ActualDatabase.ExecuteScalar(this.m_DbCommand);
                    tran.Commit();
                    return obj2;
                }
                obj3 = this.ActualDatabase.ExecuteScalar(this.m_DbCommand);
            }
            catch (Exception exception)
            {
                this.CloseConnection(tran);
                DataAccessLogger.LogExecutionError(this.m_DbCommand, exception);
                this.log.Error(exception.Message);
                throw;
            }
            return obj3;
        }

        public T ExecuteScalar<T>(bool isOpenTrans)
        {
            DbTransaction transaction = null;
            T local2;
            try
            {
                object obj2;
                T local;
                if (isOpenTrans)
                {
                    DbConnection connection = this.ActualDatabase.CreateConnection();
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    obj2 = this.ActualDatabase.ExecuteScalar(this.m_DbCommand, transaction);
                    local = (obj2 == null) ? default(T) : ((T) obj2);
                    transaction.Commit();
                    return local;
                }
                obj2 = this.ActualDatabase.ExecuteScalar(this.m_DbCommand);
                local = (obj2 == null) ? default(T) : ((T) obj2);
                local2 = local;
            }
            catch (Exception exception)
            {
                this.CloseConnection(transaction);
                DataAccessLogger.LogExecutionError(this.m_DbCommand, exception);
                this.log.Error(exception.Message);
                throw;
            }
            return local2;
        }

        public object GetParameterValue(string paramName)
        {
            return this.ActualDatabase.GetParameterValue(this.m_DbCommand, paramName);
        }

        public void ReplaceParameterValue(string paramName, string paramValue)
        {
            if (null != this.m_DbCommand)
            {
                this.m_DbCommand.CommandText = this.m_DbCommand.CommandText.Replace(paramName, paramValue);
            }
        }

        public void SetParameterValue(string paramName, object val)
        {
            this.ActualDatabase.SetParameterValue(this.m_DbCommand, paramName, val);
        }

        protected Database ActualDatabase
        {
            get
            {
                return DatabaseManager.GetDatabase(this.m_DatabaseName);
            }
        }

        public DbCommand Command
        {
            get
            {
                return this.m_DbCommand;
            }
        }
    }
}

