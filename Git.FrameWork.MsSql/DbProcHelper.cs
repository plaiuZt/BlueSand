using Git.Framework.MsSql.DataAccess;
using Git.Framework.ORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Git.Framework.MsSql
{

    public class DbProcHelper<T> : IDbProcHelper<T>, IDisposable where T: BaseEntity, new()
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public V ExceuteEntity<V>(T entity) where V: class, new()
        {
            Func<PropertyInfo, bool> func = null;
            Func<PropertyInfo, bool> func2 = null;
            Func<PropertyInfo, bool> func3 = null;
            TableInfo tableInfo = EntityTypeCache.Get(entity);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.StoredProcedure, tableInfo.Table.Name);
            if ((tableInfo.Properties != null) && (tableInfo.Properties.Count<PropertyInfo>() > 0))
            {
                SqlParameter parameter;
                if (func == null)
                {
                    func = item => tableInfo.PDRelation[item].ColumnType == ColumnType.InPut;
                }
                foreach (PropertyInfo info in Enumerable.Where<PropertyInfo>(tableInfo.Properties, func))
                {
                    object obj2 = info.GetValue(entity, null);
                    parameter = new SqlParameter("@" + tableInfo.PDRelation[info].ColumnName, obj2) {
                        Size = tableInfo.PDRelation[info].Length,
                        DbType = tableInfo.PDRelation[info].DbType
                    };
                    command.Command.Parameters.Add(parameter);
                }
                if (func2 == null)
                {
                    func2 = item => tableInfo.PDRelation[item].ColumnType == ColumnType.InOutPut;
                }
                foreach (PropertyInfo info in Enumerable.Where<PropertyInfo>(tableInfo.Properties, func2))
                {
                    parameter = new SqlParameter("@" + tableInfo.PDRelation[info].ColumnName, null) {
                        Direction = ParameterDirection.Output,
                        Size = tableInfo.PDRelation[info].Length,
                        DbType = tableInfo.PDRelation[info].DbType
                    };
                    command.Command.Parameters.Add(parameter);
                }
            }
            V local = command.ExecuteEntity<V>();
            if ((tableInfo.Properties != null) && (tableInfo.Properties.Count<PropertyInfo>() > 0))
            {
                if (func3 == null)
                {
                    func3 = item => tableInfo.PDRelation[item].ColumnType == ColumnType.InOutPut;
                }
                foreach (PropertyInfo info in Enumerable.Where<PropertyInfo>(tableInfo.Properties, func3))
                {
                    object parameterValue = command.GetParameterValue("@" + tableInfo.PDRelation[info].ColumnName);
                    info.SetValue(entity, parameterValue, null);
                }
            }
            return local;
        }

        public List<V> ExceuteEntityList<V>(T entity) where V: class, new()
        {
            Func<PropertyInfo, bool> func = null;
            Func<PropertyInfo, bool> func2 = null;
            Func<PropertyInfo, bool> func3 = null;
            TableInfo tableInfo = EntityTypeCache.Get(entity);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.StoredProcedure, tableInfo.Table.Name);
            if ((tableInfo.Properties != null) && (tableInfo.Properties.Count<PropertyInfo>() > 0))
            {
                SqlParameter parameter;
                if (func == null)
                {
                    func = item => tableInfo.PDRelation[item].ColumnType == ColumnType.InPut;
                }
                foreach (PropertyInfo info in Enumerable.Where<PropertyInfo>(tableInfo.Properties, func))
                {
                    object obj2 = info.GetValue(entity, null);
                    parameter = new SqlParameter("@" + tableInfo.PDRelation[info].ColumnName, obj2) {
                        Size = tableInfo.PDRelation[info].Length,
                        DbType = tableInfo.PDRelation[info].DbType
                    };
                    command.Command.Parameters.Add(parameter);
                }
                if (func2 == null)
                {
                    func2 = item => tableInfo.PDRelation[item].ColumnType == ColumnType.InOutPut;
                }
                foreach (PropertyInfo info in Enumerable.Where<PropertyInfo>(tableInfo.Properties, func2))
                {
                    parameter = new SqlParameter("@" + tableInfo.PDRelation[info].ColumnName, null) {
                        Direction = ParameterDirection.Output,
                        Size = tableInfo.PDRelation[info].Length,
                        DbType = tableInfo.PDRelation[info].DbType
                    };
                    command.Command.Parameters.Add(parameter);
                }
            }
            List<V> list = command.ExecuteEntityList<V>();
            if ((tableInfo.Properties != null) && (tableInfo.Properties.Count<PropertyInfo>() > 0))
            {
                if (func3 == null)
                {
                    func3 = item => tableInfo.PDRelation[item].ColumnType == ColumnType.InOutPut;
                }
                foreach (PropertyInfo info in Enumerable.Where<PropertyInfo>(tableInfo.Properties, func3))
                {
                    object parameterValue = command.GetParameterValue("@" + tableInfo.PDRelation[info].ColumnName);
                    if (parameterValue != DBNull.Value)
                    {
                        info.SetValue(entity, parameterValue, null);
                    }
                }
            }
            return list;
        }

        public int ExecuteNonQuery(T entity)
        {
            Func<PropertyInfo, bool> func = null;
            Func<PropertyInfo, bool> func2 = null;
            Func<PropertyInfo, bool> func3 = null;
            TableInfo tableInfo = EntityTypeCache.Get(entity);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.StoredProcedure, tableInfo.Table.Name);
            if ((tableInfo.Properties != null) && (tableInfo.Properties.Count<PropertyInfo>() > 0))
            {
                SqlParameter parameter;
                if (func == null)
                {
                    func = item => tableInfo.PDRelation[item].ColumnType == ColumnType.InPut;
                }
                foreach (PropertyInfo info in Enumerable.Where<PropertyInfo>(tableInfo.Properties, func))
                {
                    object obj2 = info.GetValue(entity, null);
                    parameter = new SqlParameter("@" + tableInfo.PDRelation[info].ColumnName, obj2) {
                        Size = tableInfo.PDRelation[info].Length,
                        DbType = tableInfo.PDRelation[info].DbType
                    };
                    command.Command.Parameters.Add(parameter);
                }
                if (func2 == null)
                {
                    func2 = item => tableInfo.PDRelation[item].ColumnType == ColumnType.InOutPut;
                }
                foreach (PropertyInfo info in Enumerable.Where<PropertyInfo>(tableInfo.Properties, func2))
                {
                    parameter = new SqlParameter("@" + tableInfo.PDRelation[info].ColumnName, null) {
                        Direction = ParameterDirection.Output,
                        Size = tableInfo.PDRelation[info].Length,
                        DbType = tableInfo.PDRelation[info].DbType
                    };
                    command.Command.Parameters.Add(parameter);
                }
            }
            int num = command.ExecuteNonQuery();
            if ((tableInfo.Properties != null) && (tableInfo.Properties.Count<PropertyInfo>() > 0))
            {
                if (func3 == null)
                {
                    func3 = item => tableInfo.PDRelation[item].ColumnType == ColumnType.InOutPut;
                }
                foreach (PropertyInfo info in Enumerable.Where<PropertyInfo>(tableInfo.Properties, func3))
                {
                    object parameterValue = command.GetParameterValue("@" + tableInfo.PDRelation[info].ColumnName);
                    info.SetValue(entity, parameterValue, null);
                }
            }
            return num;
        }

        public object ExecuteScalar(T entity)
        {
            Func<PropertyInfo, bool> func = null;
            Func<PropertyInfo, bool> func2 = null;
            Func<PropertyInfo, bool> func3 = null;
            TableInfo tableInfo = EntityTypeCache.Get(entity);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.StoredProcedure, tableInfo.Table.Name);
            if ((tableInfo.Properties != null) && (tableInfo.Properties.Count<PropertyInfo>() > 0))
            {
                SqlParameter parameter;
                if (func == null)
                {
                    func = item => tableInfo.PDRelation[item].ColumnType == ColumnType.InPut;
                }
                foreach (PropertyInfo info in Enumerable.Where<PropertyInfo>(tableInfo.Properties, func))
                {
                    object obj2 = info.GetValue(entity, null);
                    parameter = new SqlParameter("@" + tableInfo.PDRelation[info].ColumnName, obj2) {
                        Size = tableInfo.PDRelation[info].Length,
                        DbType = tableInfo.PDRelation[info].DbType
                    };
                    command.Command.Parameters.Add(parameter);
                }
                if (func2 == null)
                {
                    func2 = item => tableInfo.PDRelation[item].ColumnType == ColumnType.InOutPut;
                }
                foreach (PropertyInfo info in Enumerable.Where<PropertyInfo>(tableInfo.Properties, func2))
                {
                    parameter = new SqlParameter("@" + tableInfo.PDRelation[info].ColumnName, null) {
                        Direction = ParameterDirection.Output,
                        Size = tableInfo.PDRelation[info].Length,
                        DbType = tableInfo.PDRelation[info].DbType
                    };
                    command.Command.Parameters.Add(parameter);
                }
            }
            object obj3 = command.ExecuteScalar();
            if ((tableInfo.Properties != null) && (tableInfo.Properties.Count<PropertyInfo>() > 0))
            {
                if (func3 == null)
                {
                    func3 = item => tableInfo.PDRelation[item].ColumnType == ColumnType.InOutPut;
                }
                foreach (PropertyInfo info in Enumerable.Where<PropertyInfo>(tableInfo.Properties, func3))
                {
                    object parameterValue = command.GetParameterValue("@" + tableInfo.PDRelation[info].ColumnName);
                    info.SetValue(entity, parameterValue, null);
                }
            }
            return obj3;
        }

        public V ExecuteScalar<V>(T entity)
        {
            Func<PropertyInfo, bool> func = null;
            Func<PropertyInfo, bool> func2 = null;
            Func<PropertyInfo, bool> func3 = null;
            TableInfo tableInfo = EntityTypeCache.Get(entity);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.StoredProcedure, tableInfo.Table.Name);
            if ((tableInfo.Properties != null) && (tableInfo.Properties.Count<PropertyInfo>() > 0))
            {
                SqlParameter parameter;
                if (func == null)
                {
                    func = item => tableInfo.PDRelation[item].ColumnType == ColumnType.InPut;
                }
                foreach (PropertyInfo info in Enumerable.Where<PropertyInfo>(tableInfo.Properties, func))
                {
                    object obj2 = info.GetValue(entity, null);
                    parameter = new SqlParameter("@" + tableInfo.PDRelation[info].ColumnName, obj2) {
                        Size = tableInfo.PDRelation[info].Length,
                        DbType = tableInfo.PDRelation[info].DbType
                    };
                    command.Command.Parameters.Add(parameter);
                }
                if (func2 == null)
                {
                    func2 = item => tableInfo.PDRelation[item].ColumnType == ColumnType.InOutPut;
                }
                foreach (PropertyInfo info in Enumerable.Where<PropertyInfo>(tableInfo.Properties, func2))
                {
                    parameter = new SqlParameter("@" + tableInfo.PDRelation[info].ColumnName, null) {
                        Direction = ParameterDirection.Output,
                        Size = tableInfo.PDRelation[info].Length,
                        DbType = tableInfo.PDRelation[info].DbType
                    };
                    command.Command.Parameters.Add(parameter);
                }
            }
            V local = command.ExecuteScalar<V>();
            if ((tableInfo.Properties != null) && (tableInfo.Properties.Count<PropertyInfo>() > 0))
            {
                if (func3 == null)
                {
                    func3 = item => tableInfo.PDRelation[item].ColumnType == ColumnType.InOutPut;
                }
                foreach (PropertyInfo info in Enumerable.Where<PropertyInfo>(tableInfo.Properties, func3))
                {
                    object parameterValue = command.GetParameterValue("@" + tableInfo.PDRelation[info].ColumnName);
                    info.SetValue(entity, parameterValue, null);
                }
            }
            return local;
        }
    }
}

