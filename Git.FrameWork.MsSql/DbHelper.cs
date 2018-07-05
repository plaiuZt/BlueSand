using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Log;
using Git.Framework.MsSql.DataAccess;
using Git.Framework.ORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Git.Framework.MsSql
{
    public class DbHelper<T> : IDbHelper<T>, IDisposable where T: BaseEntity, new()
    {
        private Func<string, BaseEntity, TableInfo, object[], string> funcIn;
        private Git.Framework.Log.Log log;

        public DbHelper()
        {
            this.log = Git.Framework.Log.Log.Instance(typeof(T));
            this.funcIn = delegate (string propertyName, BaseEntity item, TableInfo tableInfo, object[] values) {
                StringBuilder builder = new StringBuilder();
                builder.Append("(");
                if ((((((tableInfo.PNameRelation[propertyName].DbType == DbType.Decimal) || (tableInfo.PNameRelation[propertyName].DbType == DbType.Double)) || ((tableInfo.PNameRelation[propertyName].DbType == DbType.Int16) || (tableInfo.PNameRelation[propertyName].DbType == DbType.Int32))) || (((tableInfo.PNameRelation[propertyName].DbType == DbType.Int64) || (tableInfo.PNameRelation[propertyName].DbType == DbType.UInt16)) || ((tableInfo.PNameRelation[propertyName].DbType == DbType.UInt32) || (tableInfo.PNameRelation[propertyName].DbType == DbType.UInt64)))) || (tableInfo.PNameRelation[propertyName].DbType == DbType.Single)) || (tableInfo.PNameRelation[propertyName].DbType == DbType.SByte))
                {
                    foreach (object obj2 in values)
                    {
                        builder.AppendFormat("{0},", obj2.ToString());
                    }
                }
                else
                {
                    foreach (object obj2 in values)
                    {
                        builder.AppendFormat("'{0}',", obj2.ToString());
                    }
                }
                builder = builder.Remove(builder.Length - 1, 1);
                builder.Append(")");
                return builder.ToString();
            };
        }

        public int Add(T entity)
        {
            return this.Add(entity, false);
        }

        public int Add(List<T> list)
        {
            return this.Add(list, false);
        }

        private int Add(T entity, TableInfo tableInfo)
        {
            string format = string.Empty;
            format = "INSERT INTO [dbo].[{0}]({1}) VALUES({2});SELECT TOP 1 {4} FROM [dbo].[{3}] WHERE [{5}]=@@IDENTITY ";
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            if (entity.IncludeColumn.Count > 0)
            {
                foreach (KeyValuePair<string, string> pair in entity.IncludeColumn)
                {
                    if (!entity.ColumnList.Contains(pair.Key))
                    {
                        entity.ColumnList.Add(pair.Key);
                    }
                }
            }
            foreach (string str2 in entity.ColumnList)
            {
                if (!tableInfo.PNameRelation[str2].AutoIncrement)
                {
                    builder.AppendFormat("[{0}],", tableInfo.PNameRelation[str2].ColumnName);
                    builder2.AppendFormat("@{0},", tableInfo.PNameRelation[str2].ColumnName);
                }
            }
            if ((builder.Length > 0) && (builder2.Length > 0))
            {
                builder.Remove(builder.Length - 1, 1);
                builder2.Remove(builder2.Length - 1, 1);
            }
            else
            {
                this.log.Info(string.Format("添加数据SQL语句缺少必要参数列: {0}", tableInfo.Table.DbName));
                throw new Exception(string.Format("添加数据SQL语句缺少必要参数列: {0}", tableInfo.Table.DbName));
            }
            format = string.Format(format, new object[] { tableInfo.Table.Name, builder.ToString(), builder2.ToString(), tableInfo.Table.Name, tableInfo.Table.PrimaryKeyName, tableInfo.Table.PrimaryKeyName });
            this.log.Info(" SQL :" + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            using (List<string>.Enumerator enumerator2 = entity.ColumnList.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    Func<PropertyInfo, bool> func = null;
                    string item = enumerator2.Current;
                    if (!tableInfo.PNameRelation[item].AutoIncrement)
                    {
                        SqlParameter parameter2 = new SqlParameter {
                            ParameterName = "@" + tableInfo.PNameRelation[item].ColumnName
                        };
                        if (func == null)
                        {
                            func = p => p.Name == item;
                        }
                        parameter2.Value = Enumerable.Single<PropertyInfo>(tableInfo.Properties, func).GetValue(entity, null);
                        SqlParameter parameter = parameter2;
                        if ((((tableInfo.PNameRelation[item].DbType == DbType.Date) || (tableInfo.PNameRelation[item].DbType == DbType.DateTime)) || (tableInfo.PNameRelation[item].DbType == DbType.DateTime2)) || (tableInfo.PNameRelation[item].DbType == DbType.DateTimeOffset))
                        {
                            parameter.SqlDbType = SqlDbType.DateTime;
                            if (Convert.ToDateTime(parameter.Value) == DateTime.MinValue)
                            {
                                parameter.Value = DBNull.Value;
                            }
                        }
                        parameter.Value = (parameter.Value == null) ? (parameter.Value = DBNull.Value) : parameter.Value;
                        command.Command.Parameters.Add(parameter);
                    }
                }
            }
            return command.ExecuteScalar<int>();
        }

        public int Add(T entity, bool isOpenTrans)
        {
            TableInfo tableInfo = EntityTypeCache.Get(entity);
            string primaryKeyName = tableInfo.Table.PrimaryKeyName;
            if (!(string.IsNullOrEmpty(primaryKeyName) || !tableInfo.PNameRelation[primaryKeyName].AutoIncrement))
            {
                this.log.Info(string.Format("添加数据行主键为标识列: {0}.{1}", tableInfo.Table.DbName, primaryKeyName));
                return this.Add(entity, tableInfo);
            }
            string format = "INSERT INTO [dbo].[{0}]({1}) VALUES({2})";
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            if (entity.IncludeColumn.Count > 0)
            {
                foreach (KeyValuePair<string, string> pair in entity.IncludeColumn)
                {
                    if (!entity.ColumnList.Contains(pair.Key))
                    {
                        entity.ColumnList.Add(pair.Key);
                    }
                }
            }
            foreach (string str3 in entity.ColumnList)
            {
                builder.AppendFormat("[{0}],", tableInfo.PNameRelation[str3].ColumnName);
                builder2.AppendFormat("@{0},", tableInfo.PNameRelation[str3].ColumnName);
            }
            if ((builder.Length > 0) && (builder2.Length > 0))
            {
                builder.Remove(builder.Length - 1, 1);
                builder2.Remove(builder2.Length - 1, 1);
            }
            else
            {
                this.log.Info(string.Format("添加数据SQL语句缺少必要参数列: {0}", tableInfo.Table.DbName));
                throw new Exception(string.Format("添加数据SQL语句缺少必要参数列: {0}", tableInfo.Table.DbName));
            }
            format = string.Format(format, tableInfo.Table.Name, builder.ToString(), builder2.ToString());
            this.log.Info(" SQL--> " + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            using (List<string>.Enumerator enumerator2 = entity.ColumnList.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    Func<PropertyInfo, bool> func = null;
                    string item = enumerator2.Current;
                    SqlParameter parameter2 = new SqlParameter {
                        ParameterName = "@" + tableInfo.PNameRelation[item].ColumnName
                    };
                    if (func == null)
                    {
                        func = p => p.Name == item;
                    }
                    parameter2.Value = Enumerable.Single<PropertyInfo>(tableInfo.Properties, func).GetValue(entity, null);
                    SqlParameter parameter = parameter2;
                    if ((((tableInfo.PNameRelation[item].DbType == DbType.Date) || (tableInfo.PNameRelation[item].DbType == DbType.DateTime)) || (tableInfo.PNameRelation[item].DbType == DbType.DateTime2)) || (tableInfo.PNameRelation[item].DbType == DbType.DateTimeOffset))
                    {
                        parameter.SqlDbType = SqlDbType.DateTime;
                        if (Convert.ToDateTime(parameter.Value) == DateTime.MinValue)
                        {
                            parameter.Value = DBNull.Value;
                        }
                    }
                    parameter.Value = (parameter.Value == null) ? (parameter.Value = DBNull.Value) : parameter.Value;
                    command.Command.Parameters.Add(parameter);
                }
            }
            return command.ExecuteNonQuery(isOpenTrans);
        }

        public int Add(List<T> list, bool isOpenTrans)
        {
            list.ThrowIfNullOrEmpty<T>("批量添加实体对象集合不能为空");
            StringBuilder builder = new StringBuilder();
            string format = "INSERT INTO [dbo].[{0}]({1}) VALUES({2});";
            List<SqlParameter> list2 = new List<SqlParameter>();
            for (int i = 0; i < list.Count; i++)
            {
                StringBuilder builder2 = new StringBuilder();
                StringBuilder builder3 = new StringBuilder();
                TableInfo info = EntityTypeCache.Get(list[i]);
                if (list[i].IncludeColumn.Count > 0)
                {
                    foreach (KeyValuePair<string, string> pair in list[i].IncludeColumn)
                    {
                        if (!list[i].ColumnList.Contains(pair.Key))
                        {
                            list[i].ColumnList.Add(pair.Key);
                        }
                    }
                }
                using (List<string>.Enumerator enumerator2 = list[i].ColumnList.GetEnumerator())
                {
                    while (enumerator2.MoveNext())
                    {
                        Func<PropertyInfo, bool> func = null;
                        string item = enumerator2.Current;
                        if (!info.PNameRelation[item].AutoIncrement)
                        {
                            builder2.AppendFormat("[{0}],", info.PNameRelation[item].ColumnName);
                            builder3.AppendFormat("@{0}_{1},", info.PNameRelation[item].ColumnName, i);
                            SqlParameter parameter2 = new SqlParameter {
                                ParameterName = string.Concat(new object[] { "@", info.PNameRelation[item].ColumnName, "_", i })
                            };
                            if (func == null)
                            {
                                func = p => p.Name == item;
                            }
                            parameter2.Value = Enumerable.Single<PropertyInfo>(info.Properties, func).GetValue(list[i], null);
                            SqlParameter parameter = parameter2;
                            if ((((info.PNameRelation[item].DbType == DbType.Date) || (info.PNameRelation[item].DbType == DbType.DateTime)) || (info.PNameRelation[item].DbType == DbType.DateTime2)) || (info.PNameRelation[item].DbType == DbType.DateTimeOffset))
                            {
                                parameter.SqlDbType = SqlDbType.DateTime;
                                if (Convert.ToDateTime(parameter.Value) == DateTime.MinValue)
                                {
                                    parameter.Value = DBNull.Value;
                                }
                            }
                            parameter.Value = (parameter.Value == null) ? (parameter.Value = DBNull.Value) : parameter.Value;
                            list2.Add(parameter);
                        }
                    }
                }
                if ((builder2.Length > 0) && (builder3.Length > 0))
                {
                    builder2.Remove(builder2.Length - 1, 1);
                    builder3.Remove(builder3.Length - 1, 1);
                }
                else
                {
                    this.log.Info("添加数据SQL语句缺少必要参数列");
                    throw new Exception("添加数据SQL语句缺少必要参数列");
                }
                builder.Append(string.Format(format, new object[] { info.Table.Name, builder2.ToString(), builder3.ToString(), info.Table.DbName, info.Table.Name, info.Table.PrimaryKeyName, info.Table.PrimaryKeyName }));
            }
            this.log.Info(" SQL :" + builder.ToString());
            DataCommand command = DataCommandManager.CreateCustomDataCommand(EntityTypeCache.Get<T>().Table.DbName, CommandType.Text, builder.ToString());
            command.Command.Parameters.AddRange(list2.ToArray());
            return command.ExecuteNonQuery(isOpenTrans);
        }

        public int Delete(IEnumerable<int> ids)
        {
            return this.Delete(ids, false);
        }

        public int Delete(int id)
        {
            return this.Delete(id, false);
        }

        public int Delete(object value)
        {
            return this.Delete(value, false);
        }

        public int Delete(T entity)
        {
            return this.Delete(entity, false);
        }

        public int Delete(IEnumerable<int> ids, bool isOpenTrans)
        {
            if (ids.IsNullOrEmpty<int>())
            {
                return 0;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("(");
            foreach (int num in ids)
            {
                builder.AppendFormat("{0},", num.ToString());
            }
            builder = builder.Remove(builder.Length - 1, 1);
            builder.Append(")");
            string format = "DELETE FROM [dbo].[{0}] WHERE [{1}] IN {2}";
            TableInfo info = EntityTypeCache.Get<T>();
            format = string.Format(format, info.Table.Name, info.Table.PrimaryKeyName, builder.ToString());
            this.log.Info(" SQL: " + format);
            return DataCommandManager.CreateCustomDataCommand(info.Table.DbName, CommandType.Text, format).ExecuteNonQuery(isOpenTrans);
        }

        public int Delete(int id, bool isOpenTrans)
        {
            return this.Delete(id, isOpenTrans);
        }

        public int Delete(object value, bool isOpenTrans)
        {
            string format = "DELETE FROM [dbo].[{0}] WHERE [{1}]={2}";
            TableInfo info = EntityTypeCache.Get<T>();
            format = string.Format(format, info.Table.Name, info.Table.PrimaryKeyName, "@" + info.Table.PrimaryKeyName);
            this.log.Info(" SQL: " + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(info.Table.DbName, CommandType.Text, format);
            command.Command.Parameters.Add(new SqlParameter("@" + info.Table.PrimaryKeyName, value));
            return command.ExecuteNonQuery(isOpenTrans);
        }

        public int Delete(T entity, bool isOpenTrans)
        {
            string format = "DELETE FROM [dbo].[{0}] {1}";
            TableInfo argTableInfo = EntityTypeCache.Get<T>();
            string sqlCondition = this.GetSqlCondition(entity, argTableInfo);
            format = string.Format(format, argTableInfo.Table.Name, sqlCondition);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(argTableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, argTableInfo, command, EOperator.Delete);
            this.log.Info(" SQL: " + format);
            return command.ExecuteNonQuery(isOpenTrans);
        }

        public int DeleteBatch(IEnumerable<T> list, bool isOpenTrans)
        {
            list.ThrowIfNullOrEmpty<T>("批量修改集合为空");
            StringBuilder builder = new StringBuilder();
            List<SqlParameter> list2 = new List<SqlParameter>();
            TableInfo argTableInfo = EntityTypeCache.Get<T>();
            int index = 0;
            foreach (T local in list)
            {
                string format = "DELETE FROM [dbo].[{0}] {1};";
                string str2 = this.GetSqlCondition(local, argTableInfo, index);
                format = string.Format(format, argTableInfo.Table.Name, str2);
                builder.Append(format);
                this.SetSqlParamter(local, argTableInfo, ref list2, EOperator.Delete, index);
                index++;
            }
            DataCommand command = DataCommandManager.CreateCustomDataCommand(argTableInfo.Table.DbName, CommandType.Text, builder.ToString());
            command.Command.Parameters.AddRange(list2);
            return command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private void GetChildColumn(List<Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType>> items, ref StringBuilder sbColumn)
        {
            if (!items.IsNullOrEmpty<Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType>>())
            {
                foreach (Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType> @params in items)
                {
                    TableInfo info = EntityTypeCache.Get(@params.Item1.GetType());
                    if (@params.Item1.ColumnList.Count > 0)
                    {
                        foreach (string str in @params.Item1.ColumnList)
                        {
                            if (!@params.Item1.IncludeColumn.ContainsKey(str))
                            {
                                @params.Item1.IncludeColumn.Add(str, string.Empty);
                            }
                        }
                    }
                    foreach (string str2 in @params.Item1.IncludeColumn.Keys)
                    {
                        if (string.IsNullOrEmpty(@params.Item1.TabAlias))
                        {
                            sbColumn.AppendFormat("{0}.[{1}]{2},", info.Table.Name, info.PNameRelation[str2].ColumnName, @params.Item1.IncludeColumn[str2].IsEmpty() ? string.Empty : (" AS " + @params.Item1.IncludeColumn[str2]));
                        }
                        else
                        {
                            sbColumn.AppendFormat("{0}.[{1}]{2},", @params.Item1.TabAlias, info.PNameRelation[str2].ColumnName, @params.Item1.IncludeColumn[str2].IsEmpty() ? string.Empty : (" AS " + @params.Item1.IncludeColumn[str2]));
                        }
                    }
                    if (!@params.Item1.JoinColumn.IsNullOrEmpty<Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType>>())
                    {
                        this.GetChildColumn(@params.Item1.JoinColumn, ref sbColumn);
                    }
                }
            }
        }

        private string GetConditionExpress(Params<JointType, Params<string, ECondition, object[]>> param, TableInfo tableInfo, BaseEntity entity)
        {
            StringBuilder builder = new StringBuilder();
            if (param != null)
            {
                builder = new StringBuilder();
                if ((((ECondition) param.Item2.Item2) == ECondition.Is) || (((ECondition) param.Item2.Item2) == ECondition.IsNot))
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}]{2} NULL", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2));
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}]{2} NULL", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2));
                    }
                }
                else if (((ECondition) param.Item2.Item2) == ECondition.Between)
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}] BETWEEN @{2} AND @{3}", new object[] { tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, tableInfo.Table.Name + "_Begin_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName, tableInfo.Table.Name + "_End_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}] BETWEEN @{2} AND @{3}", new object[] { entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, entity.TabAlias + "_Begin_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName, entity.TabAlias + "_End_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                    }
                }
                else if (((ECondition) param.Item2.Item2) == ECondition.Like)
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}] LIKE {2}", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, "@" + tableInfo.Table.Name + "_" + param.Item2.Item1);
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}] LIKE {2}", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, "@" + entity.TabAlias + "_" + param.Item2.Item1);
                    }
                }
                else if (((ECondition) param.Item2.Item2) == ECondition.In)
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}] IN {2}", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, this.funcIn(param.Item2.Item1, entity, tableInfo, param.Item2.Item3));
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}] IN {2}", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, this.funcIn(param.Item2.Item1, entity, tableInfo, param.Item2.Item3));
                    }
                }
                else if (string.IsNullOrEmpty(entity.TabAlias))
                {
                    builder.AppendFormat("{0}.[{1}]{2}@{3}", new object[] { tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2), tableInfo.Table.Name + "_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                }
                else
                {
                    builder.AppendFormat("{0}.[{1}]{2}@{3}", new object[] { entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2), entity.TabAlias + "_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                }
            }
            return builder.ToString();
        }

        private string GetConditionExpress(Params<JointType, Params<string, ECondition, object[]>> param, TableInfo tableInfo, BaseEntity entity, int index)
        {
            StringBuilder builder = new StringBuilder();
            if (param != null)
            {
                builder = new StringBuilder();
                if ((((ECondition) param.Item2.Item2) == ECondition.Is) || (((ECondition) param.Item2.Item2) == ECondition.IsNot))
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}]{2} NULL", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2));
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}]{2} NULL", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2));
                    }
                }
                else if (((ECondition) param.Item2.Item2) == ECondition.OrIs)
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}]{2} NULL", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(ECondition.Is));
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}]{2} NULL", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(ECondition.Is));
                    }
                }
                else if (((ECondition) param.Item2.Item2) == ECondition.OrIsNot)
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}]{2} NULL", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(ECondition.IsNot));
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}]{2} NULL", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(ECondition.IsNot));
                    }
                }
                else if ((((ECondition) param.Item2.Item2) == ECondition.Between) || (((ECondition) param.Item2.Item2) == ECondition.OrBetween))
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat(string.Concat(new object[] { "{0}.[{1}] BETWEEN @{2}_", index, " AND @{3}_", index }), new object[] { tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, tableInfo.Table.Name + "_Begin_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName, tableInfo.Table.Name + "_End_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                    }
                    else
                    {
                        builder.AppendFormat(string.Concat(new object[] { "{0}.[{1}] BETWEEN @{2}_", index, " AND @{3}_", index }), new object[] { entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, entity.TabAlias + "_Begin_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName, entity.TabAlias + "_End_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                    }
                }
                else if ((((ECondition) param.Item2.Item2) == ECondition.Like) || (((ECondition) param.Item2.Item2) == ECondition.OrLike))
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}] LIKE {2}", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, "@" + tableInfo.Table.Name + "_" + param.Item2.Item1);
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}] LIKE {2}", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, "@" + entity.TabAlias + "_" + param.Item2.Item1);
                    }
                }
                else if ((((ECondition) param.Item2.Item2) == ECondition.In) || (((ECondition) param.Item2.Item2) == ECondition.OrIn))
                {
                    if ((param.Item2.Item3.Length == 1) && (param.Item2.Item3[0] is BaseEntity))
                    {
                        string sqlTemplate = this.GetSqlTemplate(param.Item2.Item3[0] as BaseEntity);
                        if (string.IsNullOrEmpty(entity.TabAlias))
                        {
                            builder.AppendFormat("{0}.[{1}] IN ({2})", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, sqlTemplate);
                        }
                        else
                        {
                            builder.AppendFormat("{0}.[{1}] IN ({2})", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, sqlTemplate);
                        }
                    }
                    else if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}] IN {2}", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, this.funcIn(param.Item2.Item1, entity, tableInfo, param.Item2.Item3));
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}] IN {2}", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, this.funcIn(param.Item2.Item1, entity, tableInfo, param.Item2.Item3));
                    }
                }
                else if (string.IsNullOrEmpty(entity.TabAlias))
                {
                    builder.AppendFormat("{0}.[{1}]{2}@{3}_" + index, new object[] { tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2), tableInfo.Table.Name + "_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                }
                else
                {
                    builder.AppendFormat("{0}.[{1}]{2}@{3}_" + index, new object[] { entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2), entity.TabAlias + "_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                }
            }
            return builder.ToString();
        }

        private string GetConditionExpress(Params<JointType, Params<string, ECondition, object[]>> param, int cursor, TableInfo tableInfo, BaseEntity entity)
        {
            StringBuilder builder = new StringBuilder();
            if (param != null)
            {
                builder = new StringBuilder();
                if ((((ECondition) param.Item2.Item2) == ECondition.Is) || (((ECondition) param.Item2.Item2) == ECondition.IsNot))
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}]{2} NULL", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2));
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}]{2} NULL", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2));
                    }
                }
                else if (((ECondition) param.Item2.Item2) == ECondition.Between)
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat(string.Concat(new object[] { "{0}.[{1}] BETWEEN @", cursor, "_{2} AND @", cursor, "_{3}" }), new object[] { tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, tableInfo.Table.Name + "_Begin_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName, tableInfo.Table.Name + "_End_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                    }
                    else
                    {
                        builder.AppendFormat(string.Concat(new object[] { "{0}.[{1}] BETWEEN @", cursor, "_{2} AND @", cursor, "_{3}" }), new object[] { entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, entity.TabAlias + "_Begin_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName, entity.TabAlias + "_End_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                    }
                }
                else if (((ECondition) param.Item2.Item2) == ECondition.Like)
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}] LIKE {2}", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, string.Concat(new object[] { "@", cursor, "_", tableInfo.Table.Name, "_", param.Item2.Item1 }));
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}] LIKE {2}", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, string.Concat(new object[] { "@", cursor, "_", entity.TabAlias, "_", param.Item2.Item1 }));
                    }
                }
                else if (((ECondition) param.Item2.Item2) == ECondition.In)
                {
                    if ((param.Item2.Item3.Length == 1) && (param.Item2.Item3[0] is BaseEntity))
                    {
                        string sqlTemplate = this.GetSqlTemplate(param.Item2.Item3[0] as BaseEntity);
                        if (string.IsNullOrEmpty(entity.TabAlias))
                        {
                            builder.AppendFormat("{0}.[{1}] IN ({2})", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, sqlTemplate);
                        }
                        else
                        {
                            builder.AppendFormat("{0}.[{1}] IN ({2})", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, sqlTemplate);
                        }
                    }
                    else if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}] IN {2}", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, this.funcIn(param.Item2.Item1, entity, tableInfo, param.Item2.Item3));
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}] IN {2}", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, this.funcIn(param.Item2.Item1, entity, tableInfo, param.Item2.Item3));
                    }
                }
                else if (string.IsNullOrEmpty(entity.TabAlias))
                {
                    builder.AppendFormat("{0}.[{1}]{2}@" + cursor + "_{3}", new object[] { tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2), tableInfo.Table.Name + "_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                }
                else
                {
                    builder.AppendFormat("{0}.[{1}]{2}@" + cursor + "_{3}", new object[] { entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2), entity.TabAlias + "_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                }
            }
            return builder.ToString();
        }

        private string GetConditionExpress(Params<JointType, Params<string, ECondition, object[]>> param, int cursor, TableInfo tableInfo, BaseEntity entity, int index)
        {
            StringBuilder builder = new StringBuilder();
            if (param != null)
            {
                builder = new StringBuilder();
                if ((((ECondition) param.Item2.Item2) == ECondition.Is) || (((ECondition) param.Item2.Item2) == ECondition.IsNot))
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}]{2} NULL", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2));
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}]{2} NULL", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2));
                    }
                }
                else if (((ECondition) param.Item2.Item2) == ECondition.Between)
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat(string.Concat(new object[] { "{0}.[{1}] BETWEEN @", cursor, "_{2}_", index, " AND @", cursor, "_{3}_", index }), new object[] { tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, tableInfo.Table.Name + "_Begin_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName, tableInfo.Table.Name + "_End_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                    }
                    else
                    {
                        builder.AppendFormat(string.Concat(new object[] { "{0}.[{1}] BETWEEN @", cursor, "_{2}_", index, " AND @", cursor, "_{3}_", index }), new object[] { entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, entity.TabAlias + "_Begin_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName, entity.TabAlias + "_End_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                    }
                }
                else if (((ECondition) param.Item2.Item2) == ECondition.Like)
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}] LIKE {2}", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, string.Concat(new object[] { "@", cursor, "_", tableInfo.Table.Name, "_", param.Item2.Item1 }));
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}] LIKE {2}", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, string.Concat(new object[] { "@", cursor, "_", entity.TabAlias, "_", param.Item2.Item1 }));
                    }
                }
                else if (((ECondition) param.Item2.Item2) == ECondition.In)
                {
                    if ((param.Item2.Item3.Length == 1) && (param.Item2.Item3[0] is BaseEntity))
                    {
                        string sqlTemplate = this.GetSqlTemplate(param.Item2.Item3[0] as BaseEntity);
                        if (string.IsNullOrEmpty(entity.TabAlias))
                        {
                            builder.AppendFormat("{0}.[{1}] IN ({2})", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, sqlTemplate);
                        }
                        else
                        {
                            builder.AppendFormat("{0}.[{1}] IN ({2})", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, sqlTemplate);
                        }
                    }
                    else if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}] IN {2}", tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, this.funcIn(param.Item2.Item1, entity, tableInfo, param.Item2.Item3));
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}] IN {2}", entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, this.funcIn(param.Item2.Item1, entity, tableInfo, param.Item2.Item3));
                    }
                }
                else if (string.IsNullOrEmpty(entity.TabAlias))
                {
                    builder.AppendFormat(string.Concat(new object[] { "{0}.[{1}]{2}@", cursor, "_{3}_", index }), new object[] { tableInfo.Table.Name, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2), tableInfo.Table.Name + "_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                }
                else
                {
                    builder.AppendFormat(string.Concat(new object[] { "{0}.[{1}]{2}@", cursor, "_{3}_", index }), new object[] { entity.TabAlias, tableInfo.PNameRelation[param.Item2.Item1].ColumnName, ConvertECondition.ToType(param.Item2.Item2), entity.TabAlias + "_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName });
                }
            }
            return builder.ToString();
        }

        public int GetCount()
        {
            return this.GetCount(false);
        }

        public int GetCount(bool isOpenTrans)
        {
            string format = "SELECT COUNT(*) FROM [dbo].[{0}]";
            TableInfo info = EntityTypeCache.Get<T>();
            format = string.Format(format, info.Table.Name);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(info.Table.DbName, CommandType.Text, format);
            this.log.Info(" SQL: " + format);
            return command.ExecuteScalar<int>(isOpenTrans);
        }

        public int GetCount(T entity)
        {
            return this.GetCount(entity, false);
        }

        public int GetCount(T entity, bool isOpenTrans)
        {
            this.ReName(entity);
            string format = "SELECT COUNT(*) FROM [dbo].[{0}] AS {3} {1} {2}";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string join = this.GetJoin(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            format = string.Format(format, new object[] { tableInfo.Table.Name, join, sqlCondition, entity.TabAlias });
            this.log.Info(" SQL: " + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            return command.ExecuteScalar<int>(isOpenTrans);
        }

        private string GetGroupColumn(T entity, TableInfo tableInfo)
        {
            StringBuilder builder = new StringBuilder();
            if (entity.GroupColumn.Count > 0)
            {
                foreach (string str in entity.GroupColumn)
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        builder.AppendFormat("{0}.[{1}],", tableInfo.Table.Name, str);
                    }
                    else
                    {
                        builder.AppendFormat("{0}.[{1}],", entity.TabAlias, str);
                    }
                }
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        private string GetJoin(BaseEntity entity, TableInfo tableInfo)
        {
            StringBuilder sb = new StringBuilder();
            if (entity.JoinColumn.Count > 0)
            {
                foreach (Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType> @params in entity.JoinColumn)
                {
                    int num;
                    if (((JoinType) @params.Item3) == JoinType.Left)
                    {
                        sb.AppendFormat(" LEFT JOIN [dbo].[{0}] AS {1} ON ", EntityTypeCache.Get(@params.Item1).Table.Name, @params.Item1.TabAlias);
                        num = 0;
                        while (num < @params.Item2.Count)
                        {
                            sb.AppendFormat(" {4}{0}.[{1}]={2}.[{3}]", new object[] { entity.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item1).PNameRelation[@params.Item2[num].Item2].ColumnName, @params.Item1.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item3).PNameRelation[@params.Item2[num].Item4].ColumnName, (num != 0) ? "AND " : string.Empty });
                            num++;
                        }
                    }
                    else if (((JoinType) @params.Item3) == JoinType.Right)
                    {
                        sb.AppendFormat(" RIGHT JOIN [dbo].[{0}] AS {1} ON ", EntityTypeCache.Get(@params.Item1).Table.Name, @params.Item1.TabAlias);
                        num = 0;
                        while (num < @params.Item2.Count)
                        {
                            sb.AppendFormat(" {4}{0}.[{1}]={2}.[{3}]", new object[] { entity.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item1).PNameRelation[@params.Item2[num].Item2].ColumnName, @params.Item1.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item3).PNameRelation[@params.Item2[num].Item4].ColumnName, (num != 0) ? "AND " : string.Empty });
                            num++;
                        }
                    }
                    else if (((JoinType) @params.Item3) == JoinType.Inner)
                    {
                        sb.AppendFormat(" INNER JOIN [dbo].[{0}] AS {1} ON ", EntityTypeCache.Get(@params.Item1).Table.Name, @params.Item1.TabAlias);
                        num = 0;
                        while (num < @params.Item2.Count)
                        {
                            sb.AppendFormat(" {4}{0}.[{1}]={2}.[{3}]", new object[] { entity.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item1).PNameRelation[@params.Item2[num].Item2].ColumnName, @params.Item1.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item3).PNameRelation[@params.Item2[num].Item4].ColumnName, (num != 0) ? "AND " : string.Empty });
                            num++;
                        }
                    }
                    else if (((JoinType) @params.Item3) == JoinType.Full)
                    {
                        sb.AppendFormat(" FULL OUTER JOIN [dbo].[{0}] AS {1} ON ", EntityTypeCache.Get(@params.Item1).Table.Name, @params.Item1.TabAlias);
                        for (num = 0; num < @params.Item2.Count; num++)
                        {
                            sb.AppendFormat(" {4}{0}.[{1}]={2}.[{3}]", new object[] { entity.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item1).PNameRelation[@params.Item2[num].Item2].ColumnName, @params.Item1.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item3).PNameRelation[@params.Item2[num].Item4].ColumnName, (num != 0) ? "AND " : string.Empty });
                        }
                    }
                    if (@params.Item1.JoinColumn.Count > 0)
                    {
                        this.GetJoin(@params.Item1, @params.Item1.JoinColumn, ref sb);
                    }
                }
            }
            return sb.ToString();
        }

        private string GetJoin(T entity, TableInfo tableInfo)
        {
            StringBuilder sb = new StringBuilder();
            if (entity.JoinColumn.Count > 0)
            {
                foreach (Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType> @params in entity.JoinColumn)
                {
                    int num;
                    if (((JoinType) @params.Item3) == JoinType.Left)
                    {
                        sb.AppendFormat(" LEFT JOIN [dbo].[{0}] AS {1} ON ", EntityTypeCache.Get(@params.Item1).Table.Name, @params.Item1.TabAlias);
                        num = 0;
                        while (num < @params.Item2.Count)
                        {
                            sb.AppendFormat(" {4}{0}.[{1}]={2}.[{3}]", new object[] { entity.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item1).PNameRelation[@params.Item2[num].Item2].ColumnName, @params.Item1.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item3).PNameRelation[@params.Item2[num].Item4].ColumnName, (num != 0) ? "AND " : string.Empty });
                            num++;
                        }
                    }
                    else if (((JoinType) @params.Item3) == JoinType.Right)
                    {
                        sb.AppendFormat(" RIGHT JOIN [dbo].[{0}] AS {1} ON ", EntityTypeCache.Get(@params.Item1).Table.Name, @params.Item1.TabAlias);
                        num = 0;
                        while (num < @params.Item2.Count)
                        {
                            sb.AppendFormat(" {4}{0}.[{1}]={2}.[{3}]", new object[] { entity.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item1).PNameRelation[@params.Item2[num].Item2].ColumnName, @params.Item1.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item3).PNameRelation[@params.Item2[num].Item4].ColumnName, (num != 0) ? "AND " : string.Empty });
                            num++;
                        }
                    }
                    else if (((JoinType) @params.Item3) == JoinType.Inner)
                    {
                        sb.AppendFormat(" INNER JOIN [dbo].[{0}] AS {1} ON ", EntityTypeCache.Get(@params.Item1).Table.Name, @params.Item1.TabAlias);
                        num = 0;
                        while (num < @params.Item2.Count)
                        {
                            sb.AppendFormat(" {4}{0}.[{1}]={2}.[{3}]", new object[] { entity.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item1).PNameRelation[@params.Item2[num].Item2].ColumnName, @params.Item1.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item3).PNameRelation[@params.Item2[num].Item4].ColumnName, (num != 0) ? "AND " : string.Empty });
                            num++;
                        }
                    }
                    else if (((JoinType) @params.Item3) == JoinType.Full)
                    {
                        sb.AppendFormat(" FULL OUTER JOIN [dbo].[{0}] AS {1} ON ", EntityTypeCache.Get(@params.Item1).Table.Name, @params.Item1.TabAlias);
                        for (num = 0; num < @params.Item2.Count; num++)
                        {
                            sb.AppendFormat(" {4}{0}.[{1}]={2}.[{3}]", new object[] { entity.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item1).PNameRelation[@params.Item2[num].Item2].ColumnName, @params.Item1.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item3).PNameRelation[@params.Item2[num].Item4].ColumnName, (num != 0) ? "AND " : string.Empty });
                        }
                    }
                    if (@params.Item1.JoinColumn.Count > 0)
                    {
                        this.GetJoin(@params.Item1, @params.Item1.JoinColumn, ref sb);
                    }
                }
            }
            return sb.ToString();
        }

        private void GetJoin(BaseEntity entity, List<Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType>> listItems, ref StringBuilder sb)
        {
            foreach (Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType> @params in listItems)
            {
                int num;
                if (((JoinType) @params.Item3) == JoinType.Left)
                {
                    sb.AppendFormat(" LEFT JOIN [dbo].[{0}] AS {1} ON ", EntityTypeCache.Get(@params.Item1).Table.Name, @params.Item1.TabAlias);
                    num = 0;
                    while (num < @params.Item2.Count)
                    {
                        sb.AppendFormat(" {4}{0}.[{1}]={2}.[{3}]", new object[] { entity.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item1).PNameRelation[@params.Item2[num].Item2].ColumnName, @params.Item1.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item3).PNameRelation[@params.Item2[num].Item4].ColumnName, (num != 0) ? "AND " : string.Empty });
                        num++;
                    }
                }
                else if (((JoinType) @params.Item3) == JoinType.Right)
                {
                    sb.AppendFormat(" RIGHT JOIN [dbo].[{0}] AS {1} ON ", EntityTypeCache.Get(@params.Item1).Table.Name, @params.Item1.TabAlias);
                    num = 0;
                    while (num < @params.Item2.Count)
                    {
                        sb.AppendFormat(" {4}{0}.[{1}]={2}.[{3}]", new object[] { entity.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item1).PNameRelation[@params.Item2[num].Item2].ColumnName, @params.Item1.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item3).PNameRelation[@params.Item2[num].Item4].ColumnName, (num != 0) ? "AND " : string.Empty });
                        num++;
                    }
                }
                else if (((JoinType) @params.Item3) == JoinType.Inner)
                {
                    sb.AppendFormat(" INNER JOIN [dbo].[{0}] AS {1} ON ", EntityTypeCache.Get(@params.Item1).Table.Name, @params.Item1.TabAlias);
                    num = 0;
                    while (num < @params.Item2.Count)
                    {
                        sb.AppendFormat(" {4}{0}.[{1}]={2}.[{3}]", new object[] { entity.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item1).PNameRelation[@params.Item2[num].Item2].ColumnName, @params.Item1.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item3).PNameRelation[@params.Item2[num].Item4].ColumnName, (num != 0) ? "AND " : string.Empty });
                        num++;
                    }
                }
                else if (((JoinType) @params.Item3) == JoinType.Full)
                {
                    sb.AppendFormat(" FULL OUTER JOIN [dbo].[{0}] AS {1} ON ", EntityTypeCache.Get(@params.Item1).Table.Name, @params.Item1.TabAlias);
                    for (num = 0; num < @params.Item2.Count; num++)
                    {
                        sb.AppendFormat(" {4}{0}.[{1}]={2}.[{3}]", new object[] { entity.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item1).PNameRelation[@params.Item2[num].Item2].ColumnName, @params.Item1.TabAlias, EntityTypeCache.Get(@params.Item2[num].Item3).PNameRelation[@params.Item2[num].Item4].ColumnName, (num != 0) ? "AND " : string.Empty });
                    }
                }
                if (@params.Item1.JoinColumn.Count > 0)
                {
                    this.GetJoin(@params.Item1, @params.Item1.JoinColumn, ref sb);
                }
            }
        }

        private string GetJointType(JointType jointType)
        {
            switch (jointType)
            {
                case JointType.Where:
                    return "AND";

                case JointType.And:
                    return "AND";

                case JointType.Or:
                    return "OR";

                case JointType.Begin:
                    return " AND (";

                case JointType.WhereBegin:
                    return "WHERE (";

                case JointType.AndBegin:
                    return " AND (";

                case JointType.OrBegin:
                    return " OR (";
            }
            return "AND";
        }

        public List<T> GetList()
        {
            return this.GetList(false);
        }

        public List<T> GetList(bool isOpenTrans)
        {
            string format = "SELECT * FROM [dbo].[{0}]";
            TableInfo info = EntityTypeCache.Get<T>();
            format = string.Format(format, info.Table.Name);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(info.Table.DbName, CommandType.Text, format);
            this.log.Info(" SQL: " + format);
            return command.ExecuteEntityList<T>(isOpenTrans);
        }

        public List<T> GetList(T entity)
        {
            return this.GetList(entity, false);
        }

        public List<V> GetList<V>(T entity) where V: class, new()
        {
            return this.GetList<V>(entity, false);
        }

        public List<T> GetList(T entity, bool isOpenTrans)
        {
            this.ReName(entity);
            string format = "SELECT {0} FROM [dbo].[{1}] AS {4} {2} {3} {5}";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            string orderByColumn = this.GetOrderByColumn(entity, tableInfo);
            format = string.Format(format, new object[] { sqlColumn, tableInfo.Table.Name, join, sqlCondition, entity.TabAlias, orderByColumn });
            this.log.Info(" SQL: " + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            return command.ExecuteEntityList<T>(isOpenTrans);
        }

        public List<V> GetList<V>(T entity, bool isOpenTrans) where V: class, new()
        {
            this.ReName(entity);
            string format = "SELECT {0} FROM [dbo].[{1}] AS {4} {2} {3} {5}";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string orderByColumn = this.GetOrderByColumn(entity, tableInfo);
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            format = string.Format(format, new object[] { sqlColumn, tableInfo.Table.Name, join, sqlCondition, entity.TabAlias, orderByColumn });
            this.log.Info(" SQL: " + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            return command.ExecuteEntityList<V>(isOpenTrans);
        }

        public List<T> GetList(T entity, int pageSize, int pageIndex, out int rowCount)
        {
            return this.GetList(entity, pageSize, pageIndex, out rowCount, false);
        }

        public List<V> GetList<V>(T entity, int pageSize, int pageIndex, out int rowCount) where V: class, new()
        {
            return this.GetList<V>(entity, pageSize, pageIndex, out rowCount, false);
        }

        public List<T> GetList(T entity, int pageSize, int pageIndex, out int rowCount, bool isOpenTrans)
        {
            this.ReName(entity);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            string format = "DECLARE @StartIndex INT ";
            format = ((((((format + "SET @StartIndex = 1 ") + "SELECT @RecordCount=COUNT(*) FROM [dbo].[{0}] AS {8} {1} {2} " + "IF (@PageIndex<=1) ") + "BEGIN " + "SET @PageIndex=1 ") + "END " + "SET @StartIndex = ( @PageIndex - 1 ) * @PageSize + 1 ") + "; " + "WITH TempTable ") + "AS( " + "SELECT ROW_NUMBER() OVER ({3}) RowNumber,{4} FROM [dbo].[{5}] AS {9} {6} {7} ") + ") " + "SELECT * FROM TempTable WHERE RowNumber BETWEEN (@StartIndex) AND (@PageIndex * @PageSize) ";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            string orderByColumn = this.GetOrderByColumn(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            format = string.Format(format, new object[] { tableInfo.Table.Name, join, sqlCondition, orderByColumn, sqlColumn, tableInfo.Table.Name, join, sqlCondition, entity.TabAlias, entity.TabAlias });
            this.log.Info(" SQL: " + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            command.Command.Parameters.Add(new SqlParameter("@PageIndex", pageIndex));
            command.Command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
            SqlParameter parameter = new SqlParameter("@RecordCount", SqlDbType.Int) {
                Direction = ParameterDirection.Output
            };
            command.Command.Parameters.Add(parameter);
            this.log.Info("分页查询SQL语句组装时间:" + stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();
            stopwatch.Reset();
            stopwatch.Start();
            List<T> list = command.ExecuteEntityList<T>(isOpenTrans);
            rowCount = (int) command.GetParameterValue("@RecordCount");
            this.log.Info("分页查询SQL语句查询时间:" + stopwatch.ElapsedMilliseconds);
            return list;
        }

        public List<V> GetList<V>(T entity, int pageSize, int pageIndex, out int rowCount, bool isOpenTrans) where V: class, new()
        {
            this.ReName(entity);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            string format = "DECLARE @StartIndex INT ";
            format = ((((((format + "SET @StartIndex = 1 ") + "SELECT @RecordCount=COUNT(*) FROM [dbo].[{0}] AS {8} {1} {2} " + "IF (@PageIndex<=1) ") + "BEGIN " + "SET @PageIndex=1 ") + "END " + "SET @StartIndex = ( @PageIndex - 1 ) * @PageSize + 1 ") + "; " + "WITH TempTable ") + "AS( " + "SELECT ROW_NUMBER() OVER ({3}) RowNumber,{4} FROM [dbo].[{5}] AS {9} {6} {7} ") + ") " + "SELECT * FROM TempTable WHERE RowNumber BETWEEN (@StartIndex) AND (@PageIndex * @PageSize) ";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            string orderByColumn = this.GetOrderByColumn(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            format = string.Format(format, new object[] { tableInfo.Table.Name, join, sqlCondition, orderByColumn, sqlColumn, tableInfo.Table.Name, join, sqlCondition, entity.TabAlias, entity.TabAlias });
            this.log.Info(" SQL: " + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            command.Command.Parameters.Add(new SqlParameter("@PageIndex", pageIndex));
            command.Command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
            SqlParameter parameter = new SqlParameter("@RecordCount", SqlDbType.Int) {
                Direction = ParameterDirection.Output
            };
            command.Command.Parameters.Add(parameter);
            this.log.Info("分页查询SQL语句组装时间:" + stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();
            stopwatch.Reset();
            stopwatch.Start();
            List<V> list = command.ExecuteEntityList<V>(isOpenTrans);
            rowCount = (int) command.GetParameterValue("@RecordCount");
            this.log.Info("分页查询SQL语句查询时间:" + stopwatch.ElapsedMilliseconds);
            return list;
        }

        private string GetOrderByColumn(T entity, TableInfo tableInfo)
        {
            Action<Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType>> action = null;
            StringBuilder sbOrder = new StringBuilder();
            if (entity.OrderByColumn.Count > 0)
            {
                sbOrder.Append("ORDER BY ");
                foreach (string str in entity.OrderByColumn.Keys)
                {
                    if (string.IsNullOrEmpty(entity.TabAlias))
                    {
                        sbOrder.AppendFormat("{0}.[{1}] {2},", tableInfo.Table.Name, tableInfo.PNameRelation[str].ColumnName, (((EOrderBy) entity.OrderByColumn[str]) == EOrderBy.ASC) ? "ASC" : "DESC");
                    }
                    else
                    {
                        sbOrder.AppendFormat("{0}.[{1}] {2},", entity.TabAlias, tableInfo.PNameRelation[str].ColumnName, (((EOrderBy) entity.OrderByColumn[str]) == EOrderBy.ASC) ? "ASC" : "DESC");
                    }
                }
                if (entity.JoinColumn.Count > 0)
                {
                    if (action == null)
                    {
                        action = delegate (Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType> item) {

                            TableInfo info = EntityTypeCache.Get(item.Item1.GetType());
                            foreach (string str in item.Item1.OrderByColumn.Keys)
                            {
                                if (string.IsNullOrEmpty(item.Item1.TabAlias))
                                {
                                    sbOrder.AppendFormat("{0}.[{1}] {2},", info.Table.Name, info.PNameRelation[str].ColumnName, (((EOrderBy)item.Item1.OrderByColumn[str]) == EOrderBy.ASC) ? "ASC" : "DESC");
                                }
                                else
                                {
                                    sbOrder.AppendFormat("{0}.[{1}] {2},", item.Item1.TabAlias, info.PNameRelation[str].ColumnName, (((EOrderBy)item.Item1.OrderByColumn[str]) == EOrderBy.ASC) ? "ASC" : "DESC");
                                }
                            }
                        };
                    }
                    entity.JoinColumn.ForEach(action);
                }
                sbOrder.Remove(sbOrder.Length - 1, 1);
            }
            return sbOrder.ToString();
        }

        public T GetSingle(int id)
        {
            return this.GetSingle(id, false);
        }

        public T GetSingle(object value)
        {
            string format = "SELECT * FROM [dbo].[{0}] WHERE [{1}]={2}";
            TableInfo info = EntityTypeCache.Get<T>();
            format = string.Format(format, info.Table.Name, info.Table.PrimaryKeyName, "@" + info.Table.PrimaryKeyName);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(info.Table.DbName, CommandType.Text, format);
            command.Command.Parameters.Add(new SqlParameter("@" + info.Table.PrimaryKeyName, value));
            this.log.Info(" SQL :" + format);
            return command.ExecuteEntity<T>();
        }

        public T GetSingle(T entity)
        {
            return this.GetSingle(entity, false);
        }

        public V GetSingle<V>(T entity) where V: class, new()
        {
            return this.GetSingle<V>(entity, false);
        }

        public T GetSingle(int id, bool isOpenTrans)
        {
            string format = "SELECT * FROM [dbo].[{0}] WHERE [{1}]={2}";
            TableInfo info = EntityTypeCache.Get<T>();
            format = string.Format(format, info.Table.Name, info.Table.PrimaryKeyName, "@" + info.Table.PrimaryKeyName);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(info.Table.DbName, CommandType.Text, format);
            command.Command.Parameters.Add(new SqlParameter("@" + info.Table.PrimaryKeyName, id));
            this.log.Info(" SQL :" + format);
            return command.ExecuteEntity<T>(isOpenTrans);
        }

        public T GetSingle(T entity, bool isOpenTrans)
        {
            this.ReName(entity);
            string format = "SELECT {0} FROM [dbo].[{1}] AS {4} {2} {3} {5}";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            string orderByColumn = this.GetOrderByColumn(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            format = string.Format(format, new object[] { sqlColumn, tableInfo.Table.Name, join, sqlCondition, entity.TabAlias, orderByColumn });
            this.log.Info(" SQL: " + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            return command.ExecuteEntity<T>(isOpenTrans);
        }

        public V GetSingle<V>(T entity, bool isOpenTrans) where V: class, new()
        {
            this.ReName(entity);
            string format = "SELECT {0} FROM [dbo].[{1}] AS {4} {2} {3} {5}";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            string orderByColumn = this.GetOrderByColumn(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            format = string.Format(format, new object[] { sqlColumn, tableInfo.Table.Name, join, sqlCondition, entity.TabAlias, orderByColumn });
            this.log.Info(" SQL: " + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            return command.ExecuteEntity<V>(isOpenTrans);
        }

        private string GetSqlColumn(BaseEntity entity, TableInfo tableInfo)
        {
            foreach (string str in entity.ColumnList)
            {
                if (!entity.IncludeColumn.ContainsKey(str))
                {
                    entity.IncludeColumn.Add(str, string.Empty);
                }
            }
            StringBuilder sbColumn = new StringBuilder();
            foreach (string str2 in entity.IncludeColumn.Keys)
            {
                if (string.IsNullOrEmpty(entity.TabAlias))
                {
                    sbColumn.AppendFormat("{0}.[{1}]{2},", tableInfo.Table.Name, tableInfo.PNameRelation[str2].ColumnName, entity.IncludeColumn[str2].IsEmpty() ? string.Empty : (" AS " + entity.IncludeColumn[str2]));
                }
                else
                {
                    sbColumn.AppendFormat("{0}.[{1}]{2},", entity.TabAlias, tableInfo.PNameRelation[str2].ColumnName, entity.IncludeColumn[str2].IsEmpty() ? string.Empty : (" AS " + entity.IncludeColumn[str2]));
                }
            }
            this.GetChildColumn(entity.JoinColumn, ref sbColumn);
            if (sbColumn.Length > 0)
            {
                sbColumn.Remove(sbColumn.Length - 1, 1);
            }
            return sbColumn.ToString();
        }

        private string GetSqlColumn(T entity, TableInfo tableInfo)
        {
            foreach (string str in entity.ColumnList)
            {
                if (!entity.IncludeColumn.ContainsKey(str))
                {
                    entity.IncludeColumn.Add(str, string.Empty);
                }
            }
            StringBuilder sbColumn = new StringBuilder();
            foreach (string str2 in entity.IncludeColumn.Keys)
            {
                if (string.IsNullOrEmpty(entity.TabAlias))
                {
                    sbColumn.AppendFormat("{0}.[{1}]{2},", tableInfo.Table.Name, tableInfo.PNameRelation[str2].ColumnName, entity.IncludeColumn[str2].IsEmpty() ? string.Empty : (" AS " + entity.IncludeColumn[str2]));
                }
                else
                {
                    sbColumn.AppendFormat("{0}.[{1}]{2},", entity.TabAlias, tableInfo.PNameRelation[str2].ColumnName, entity.IncludeColumn[str2].IsEmpty() ? string.Empty : (" AS " + entity.IncludeColumn[str2]));
                }
            }
            this.GetChildColumn(entity.JoinColumn, ref sbColumn);
            if (sbColumn.Length > 0)
            {
                sbColumn.Remove(sbColumn.Length - 1, 1);
            }
            return sbColumn.ToString();
        }

        private string GetSqlCondition(BaseEntity argEntity, TableInfo argTableInfo)
        {
            StringBuilder sbWhere = new StringBuilder();
            this.GetSqlCondition<BaseEntity>(argEntity, argTableInfo, ref sbWhere);
            return sbWhere.ToString();
        }

        private string GetSqlCondition(T argEntity, TableInfo argTableInfo)
        {
            StringBuilder sbWhere = new StringBuilder();
            this.GetSqlCondition<T>(argEntity, argTableInfo, ref sbWhere);
            return sbWhere.ToString();
        }

        private string GetSqlCondition(T argEntity, TableInfo argTableInfo, int index)
        {
            StringBuilder sbWhere = new StringBuilder();
            this.GetSqlCondition<T>(argEntity, argTableInfo, index, ref sbWhere);
            return sbWhere.ToString();
        }

        private void GetSqlCondition<V>(V entity, TableInfo tableInfo, ref StringBuilder sbWhere) where V: BaseEntity, new()
        {
            int num;
            Func<Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType>, BaseEntity> func = null;
            if (entity.ConditionCollection.Count > 0)
            {
                for (num = 0; num < entity.ConditionCollection.Count; num++)
                {
                    if ((num == 0) && (sbWhere.Length == 0))
                    {
                        if ((((((JointType) entity.ConditionCollection[num].Item1) == JointType.WhereBegin) || (((JointType) entity.ConditionCollection[num].Item1) == JointType.Begin)) || (((JointType) entity.ConditionCollection[num].Item1) == JointType.AndBegin)) || (((JointType) entity.ConditionCollection[num].Item1) == JointType.OrBegin))
                        {
                            sbWhere.AppendFormat("{0}", this.GetJointType(JointType.WhereBegin));
                        }
                        else
                        {
                            sbWhere.AppendFormat(" WHERE {0}", this.GetConditionExpress(entity.ConditionCollection[num], tableInfo, entity));
                        }
                    }
                    else if ((((((JointType) entity.ConditionCollection[num].Item1) == JointType.WhereBegin) || (((JointType) entity.ConditionCollection[num].Item1) == JointType.Begin)) || (((JointType) entity.ConditionCollection[num].Item1) == JointType.AndBegin)) || (((JointType) entity.ConditionCollection[num].Item1) == JointType.OrBegin))
                    {
                        entity.ConditionCollection[num].Item1 = (((JointType) entity.ConditionCollection[num].Item1) == JointType.WhereBegin) ? JointType.AndBegin : entity.ConditionCollection[num].Item1;
                        sbWhere.AppendFormat("{0}", this.GetJointType(entity.ConditionCollection[num].Item1));
                    }
                    else if (((JointType) entity.ConditionCollection[num].Item1) == JointType.End)
                    {
                        sbWhere.Append(")");
                    }
                    else if ((num > 0) && ((((((JointType) entity.ConditionCollection[num - 1].Item1) == JointType.WhereBegin) || (((JointType) entity.ConditionCollection[num - 1].Item1) == JointType.Begin)) || (((JointType) entity.ConditionCollection[num - 1].Item1) == JointType.AndBegin)) || (((JointType) entity.ConditionCollection[num - 1].Item1) == JointType.OrBegin)))
                    {
                        sbWhere.AppendFormat(" {0}", this.GetConditionExpress(entity.ConditionCollection[num], tableInfo, entity));
                    }
                    else
                    {
                        sbWhere.AppendFormat(" {0} {1}", this.GetJointType(entity.ConditionCollection[num].Item1), this.GetConditionExpress(entity.ConditionCollection[num], tableInfo, entity));
                    }
                }
            }
            if (entity.Conditions.Count > 0)
            {
                for (num = 0; num < entity.Conditions.Count; num++)
                {
                    if ((num == 0) && (sbWhere.Length == 0))
                    {
                        if ((((((JointType) entity.Conditions[num].Item1) == JointType.WhereBegin) || (((JointType) entity.Conditions[num].Item1) == JointType.Begin)) || (((JointType) entity.Conditions[num].Item1) == JointType.AndBegin)) || (((JointType) entity.Conditions[num].Item1) == JointType.OrBegin))
                        {
                            sbWhere.AppendFormat("{0}", this.GetJointType(JointType.WhereBegin));
                        }
                        else
                        {
                            sbWhere.AppendFormat(" WHERE {0}", this.GetConditionExpress(entity.Conditions[num], num, tableInfo, entity));
                        }
                    }
                    else if ((((((JointType) entity.Conditions[num].Item1) == JointType.WhereBegin) || (((JointType) entity.Conditions[num].Item1) == JointType.Begin)) || (((JointType) entity.Conditions[num].Item1) == JointType.AndBegin)) || (((JointType) entity.Conditions[num].Item1) == JointType.OrBegin))
                    {
                        entity.Conditions[num].Item1 = (((JointType) entity.Conditions[num].Item1) == JointType.WhereBegin) ? JointType.AndBegin : entity.Conditions[num].Item1;
                        sbWhere.AppendFormat("{0}", this.GetJointType(entity.Conditions[num].Item1));
                    }
                    else if (((JointType) entity.Conditions[num].Item1) == JointType.End)
                    {
                        sbWhere.Append(")");
                    }
                    else if ((num > 0) && ((((((JointType) entity.Conditions[num - 1].Item1) == JointType.WhereBegin) || (((JointType) entity.Conditions[num - 1].Item1) == JointType.Begin)) || (((JointType) entity.Conditions[num - 1].Item1) == JointType.AndBegin)) || (((JointType) entity.Conditions[num - 1].Item1) == JointType.OrBegin)))
                    {
                        sbWhere.AppendFormat(" {0}", this.GetConditionExpress(entity.Conditions[num], num, tableInfo, entity));
                    }
                    else
                    {
                        sbWhere.AppendFormat(" {0} {1}", this.GetJointType(entity.Conditions[num].Item1), this.GetConditionExpress(entity.Conditions[num], num, tableInfo, entity));
                    }
                }
            }
            if (entity.JoinColumn.Count > 0)
            {
                if (func == null)
                {
                    func = item => item.Item1;
                }
                foreach (BaseEntity entity2 in Enumerable.Select<Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType>, BaseEntity>(entity.JoinColumn, func))
                {
                    this.GetSqlCondition<BaseEntity>(entity2, EntityTypeCache.Get(entity2.GetType()), ref sbWhere);
                }
            }
        }

        private void GetSqlCondition<V>(V entity, TableInfo tableInfo, int index, ref StringBuilder sbWhere) where V: BaseEntity, new()
        {
            int num;
            Func<Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType>, BaseEntity> func = null;
            if (entity.ConditionCollection.Count > 0)
            {
                for (num = 0; num < entity.ConditionCollection.Count; num++)
                {
                    if ((num == 0) && (sbWhere.Length == 0))
                    {
                        if ((((((JointType) entity.ConditionCollection[num].Item1) == JointType.WhereBegin) || (((JointType) entity.ConditionCollection[num].Item1) == JointType.Begin)) || (((JointType) entity.ConditionCollection[num].Item1) == JointType.AndBegin)) || (((JointType) entity.ConditionCollection[num].Item1) == JointType.OrBegin))
                        {
                            sbWhere.AppendFormat("{0}", this.GetJointType(JointType.WhereBegin));
                        }
                        else
                        {
                            sbWhere.AppendFormat(" WHERE {0}", this.GetConditionExpress(entity.ConditionCollection[num], tableInfo, entity, index));
                        }
                    }
                    else if ((((((JointType) entity.ConditionCollection[num].Item1) == JointType.WhereBegin) || (((JointType) entity.ConditionCollection[num].Item1) == JointType.Begin)) || (((JointType) entity.ConditionCollection[num].Item1) == JointType.AndBegin)) || (((JointType) entity.ConditionCollection[num].Item1) == JointType.OrBegin))
                    {
                        entity.ConditionCollection[num].Item1 = (((JointType) entity.ConditionCollection[num].Item1) == JointType.WhereBegin) ? JointType.AndBegin : entity.ConditionCollection[num].Item1;
                        sbWhere.AppendFormat("{0}", this.GetJointType(entity.ConditionCollection[num].Item1));
                    }
                    else if (((JointType) entity.ConditionCollection[num].Item1) == JointType.End)
                    {
                        sbWhere.Append(")");
                    }
                    else if ((num > 0) && ((((((JointType) entity.ConditionCollection[num - 1].Item1) == JointType.WhereBegin) || (((JointType) entity.ConditionCollection[num - 1].Item1) == JointType.Begin)) || (((JointType) entity.ConditionCollection[num - 1].Item1) == JointType.AndBegin)) || (((JointType) entity.ConditionCollection[num - 1].Item1) == JointType.OrBegin)))
                    {
                        sbWhere.AppendFormat(" {0}", this.GetConditionExpress(entity.ConditionCollection[num], tableInfo, entity, index));
                    }
                    else
                    {
                        sbWhere.AppendFormat(" {0} {1}", this.GetJointType(entity.ConditionCollection[num].Item1), this.GetConditionExpress(entity.ConditionCollection[num], tableInfo, entity, index));
                    }
                }
            }
            if (entity.Conditions.Count > 0)
            {
                for (num = 0; num < entity.Conditions.Count; num++)
                {
                    if ((num == 0) && (sbWhere.Length == 0))
                    {
                        if ((((((JointType) entity.Conditions[num].Item1) == JointType.WhereBegin) || (((JointType) entity.Conditions[num].Item1) == JointType.Begin)) || (((JointType) entity.Conditions[num].Item1) == JointType.AndBegin)) || (((JointType) entity.Conditions[num].Item1) == JointType.OrBegin))
                        {
                            sbWhere.AppendFormat("{0}", this.GetJointType(JointType.WhereBegin));
                        }
                        else
                        {
                            sbWhere.AppendFormat(" WHERE {0}", this.GetConditionExpress(entity.Conditions[num], num, tableInfo, entity, index));
                        }
                    }
                    else if ((((((JointType) entity.Conditions[num].Item1) == JointType.WhereBegin) || (((JointType) entity.Conditions[num].Item1) == JointType.Begin)) || (((JointType) entity.Conditions[num].Item1) == JointType.AndBegin)) || (((JointType) entity.Conditions[num].Item1) == JointType.OrBegin))
                    {
                        entity.Conditions[num].Item1 = (((JointType) entity.Conditions[num].Item1) == JointType.WhereBegin) ? JointType.AndBegin : entity.Conditions[num].Item1;
                        sbWhere.AppendFormat("{0}", this.GetJointType(entity.Conditions[num].Item1));
                    }
                    else if (((JointType) entity.Conditions[num].Item1) == JointType.End)
                    {
                        sbWhere.Append(")");
                    }
                    else if ((num > 0) && ((((((JointType) entity.Conditions[num - 1].Item1) == JointType.WhereBegin) || (((JointType) entity.Conditions[num - 1].Item1) == JointType.Begin)) || (((JointType) entity.Conditions[num - 1].Item1) == JointType.AndBegin)) || (((JointType) entity.Conditions[num - 1].Item1) == JointType.OrBegin)))
                    {
                        sbWhere.AppendFormat(" {0}", this.GetConditionExpress(entity.Conditions[num], num, tableInfo, entity, index));
                    }
                    else
                    {
                        sbWhere.AppendFormat(" {0} {1}", this.GetJointType(entity.Conditions[num].Item1), this.GetConditionExpress(entity.Conditions[num], num, tableInfo, entity, index));
                    }
                }
            }
            if (entity.JoinColumn.Count > 0)
            {
                if (func == null)
                {
                    func = item => item.Item1;
                }
                foreach (BaseEntity entity2 in Enumerable.Select<Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType>, BaseEntity>(entity.JoinColumn, func))
                {
                    this.GetSqlCondition<BaseEntity>(entity2, EntityTypeCache.Get(entity2.GetType()), index, ref sbWhere);
                }
            }
        }

        private string GetSqlTemplate(BaseEntity entity)
        {
            string format = "SELECT {0} FROM [dbo].[{1}] {2} {3}";
            TableInfo tableInfo = EntityTypeCache.Get(entity);
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            format = string.Format(format, new object[] { sqlColumn, tableInfo.Table.Name, join, sqlCondition });
            this.log.Info(" SQL: " + format);
            return format;
        }

        public DataTable GetTable()
        {
            return this.GetTable(false);
        }

        public DataTable GetTable(bool isOpenTrans)
        {
            string format = "SELECT * FROM [dbo].[{0}]";
            TableInfo info = EntityTypeCache.Get<T>();
            format = string.Format(format, info.Table.Name);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(info.Table.DbName, CommandType.Text, format);
            this.log.Info(" SQL: " + format);
            DataSet set = command.ExecuteDataSet(isOpenTrans);
            if (((set != null) && (set.Tables != null)) && (set.Tables.Count > 0))
            {
                return set.Tables[0];
            }
            return null;
        }

        public DataTable GetTable(T entity)
        {
            return this.GetTable(entity, false);
        }

        public DataTable GetTable(T entity, bool isOpenTrans)
        {
            this.ReName(entity);
            string format = "SELECT {0} FROM [dbo].[{1}] AS {4} {2} {3}";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            format = string.Format(format, new object[] { sqlColumn, tableInfo.Table.Name, join, sqlCondition, entity.TabAlias });
            this.log.Info(" SQL: " + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            DataSet set = command.ExecuteDataSet(isOpenTrans);
            if (((set != null) && (set.Tables != null)) && (set.Tables.Count > 0))
            {
                return set.Tables[0];
            }
            return null;
        }

        public DataTable Group(T entity)
        {
            return this.Group(entity, false);
        }

        public DataTable Group(T entity, bool isOpenTrans)
        {
            this.ReName(entity);
            string format = "SELECT {0},COUNT(*) AS Total FROM [dbo].[{1}] {4} {2} GROUP BY {3}";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string groupColumn = this.GetGroupColumn(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            format = string.Format(format, new object[] { groupColumn, tableInfo.Table.Name, sqlCondition, groupColumn, entity.TabAlias });
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            this.log.Info(" SQL: " + format);
            DataSet set = command.ExecuteDataSet(isOpenTrans);
            if (((set != null) && (set.Tables != null)) && (set.Tables.Count > 0))
            {
                return set.Tables[0];
            }
            return null;
        }

        public IEnumerable<IGrouping<TKey, T>> Group<TKey>(T entity, Func<T, TKey> keySelector)
        {
            this.ReName(entity);
            string format = "SELECT {0} FROM [dbo].[{1}] AS {4} {2} {3}";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            format = string.Format(format, new object[] { sqlColumn, tableInfo.Table.Name, join, sqlCondition, entity.TabAlias });
            this.log.Info(" SQL: " + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            List<T> list = command.ExecuteEntityList<T>();
            IEnumerable<IGrouping<TKey, T>> enumerable = null;
            if (list != null)
            {
                enumerable = Enumerable.GroupBy<T, TKey>(list, keySelector);
            }
            return enumerable;
        }

        public V Max<V>(T entity)
        {
            return this.Max<V>(entity, false);
        }

        public V Max<V>(T entity, bool isOpenTrans)
        {
            this.ReName(entity);
            string format = "SELECT ISNULL(MAX({0}),0) FROM [dbo].[{1}] AS {4} {3} {2}";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            format = string.Format(format, new object[] { sqlColumn, tableInfo.Table.Name, sqlCondition, join, entity.TabAlias });
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            return command.ExecuteScalar<V>(isOpenTrans);
        }

        public V Min<V>(T entity)
        {
            return this.Min<V>(entity, false);
        }

        public V Min<V>(T entity, bool isOpenTrans)
        {
            this.ReName(entity);
            string format = "SELECT ISNULL(MIN({0}),0) FROM [dbo].[{1}] AS {4} {3} {2}";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            format = string.Format(format, new object[] { sqlColumn, tableInfo.Table.Name, sqlCondition, join, entity.TabAlias });
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            return command.ExecuteScalar<V>(isOpenTrans);
        }

        private void ReName(T entity)
        {
            int index = entity.Index;
            entity.TabAlias = "t" + index;
            if (entity.JoinColumn != null)
            {
                foreach (Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType> @params in entity.JoinColumn)
                {
                    index++;
                    @params.Item1.Index = index;
                    @params.Item1.TabAlias = "t" + index;
                    if (@params.Item1.JoinColumn != null)
                    {
                        this.ReName<BaseEntity>(@params.Item1, ref index);
                    }
                }
            }
        }

        private void ReName<TKey>(TKey entity, ref int index) where TKey: BaseEntity, new()
        {
            if (entity.JoinColumn != null)
            {
                foreach (Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType> @params in entity.JoinColumn)
                {
                    index++;
                    @params.Item1.Index = index;
                    @params.Item1.TabAlias = "t" + ((int) index);
                    if (@params.Item1 != null)
                    {
                        this.ReName<BaseEntity>(@params.Item1, ref index);
                    }
                }
            }
        }

        private void SetSqlParamter(BaseEntity entity, TableInfo tableInfo, ref DataCommand command)
        {
            BaseEntity entity2;
            TableInfo info;
            if (entity.ConditionCollection.Count > 0)
            {
                using (List<Params<JointType, Params<string, ECondition, object[]>>>.Enumerator enumerator = entity.ConditionCollection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Func<PropertyInfo, bool> func = null;
                        Params<JointType, Params<string, ECondition, object[]>> param = enumerator.Current;
                        if ((((((JointType) param.Item1) != JointType.Begin) && (((JointType) param.Item1) != JointType.WhereBegin)) && ((((JointType) param.Item1) != JointType.AndBegin) && (((JointType) param.Item1) != JointType.OrBegin))) && (((JointType) param.Item1) != JointType.End))
                        {
                            if (((ECondition) param.Item2.Item2) == ECondition.Between)
                            {
                                if (string.IsNullOrEmpty(entity.TabAlias))
                                {
                                    command.Command.Parameters.Add(new SqlParameter("@" + tableInfo.Table.Name + "_Begin_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName, param.Item2.Item3[0]));
                                    command.Command.Parameters.Add(new SqlParameter("@" + tableInfo.Table.Name + "_End_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName, param.Item2.Item3[1]));
                                }
                                else
                                {
                                    command.Command.Parameters.Add(new SqlParameter("@" + entity.TabAlias + "_Begin_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName, param.Item2.Item3[0]));
                                    command.Command.Parameters.Add(new SqlParameter("@" + entity.TabAlias + "_End_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName, param.Item2.Item3[1]));
                                }
                            }
                            else if (((ECondition) param.Item2.Item2) == ECondition.Like)
                            {
                                if (string.IsNullOrEmpty(entity.TabAlias))
                                {
                                    command.Command.Parameters.Add(new SqlParameter("@" + tableInfo.Table.Name + "_" + param.Item2.Item1, param.Item2.Item3[0]));
                                }
                                else
                                {
                                    command.Command.Parameters.Add(new SqlParameter("@" + entity.TabAlias + "_" + param.Item2.Item1, param.Item2.Item3[0]));
                                }
                            }
                            else if (((((ECondition) param.Item2.Item2) == ECondition.Is) || (((ECondition) param.Item2.Item2) == ECondition.IsNot)) || (((ECondition) param.Item2.Item2) == ECondition.In))
                            {
                                if (((((ECondition) param.Item2.Item2) == ECondition.In) && (param.Item2.Item3.Length == 1)) && (param.Item2.Item3[0] is BaseEntity))
                                {
                                    entity2 = param.Item2.Item3[0] as BaseEntity;
                                    info = EntityTypeCache.Get(entity2.GetType());
                                    this.SetSqlParamter(entity2, info, command, EOperator.Select);
                                }
                            }
                            else
                            {
                                if (func == null)
                                {
                                    func = p => p.Name == param.Item2.Item1;
                                }
                                object obj2 = Enumerable.Single<PropertyInfo>(tableInfo.Properties, func).GetValue(entity, null);
                                if (string.IsNullOrEmpty(entity.TabAlias))
                                {
                                    command.Command.Parameters.Add(new SqlParameter("@" + tableInfo.Table.Name + "_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName, obj2));
                                }
                                else
                                {
                                    command.Command.Parameters.Add(new SqlParameter("@" + entity.TabAlias + "_" + tableInfo.PNameRelation[param.Item2.Item1].ColumnName, obj2));
                                }
                            }
                        }
                    }
                }
            }
            if (entity.Conditions.Count > 0)
            {
                int num = 0;
                foreach (Params<JointType, Params<string, ECondition, object[]>> @params in entity.Conditions)
                {
                    if ((((((JointType) @params.Item1) != JointType.Begin) && (((JointType) @params.Item1) != JointType.WhereBegin)) && ((((JointType) @params.Item1) != JointType.AndBegin) && (((JointType) @params.Item1) != JointType.OrBegin))) && (((JointType) @params.Item1) != JointType.End))
                    {
                        if (((ECondition) @params.Item2.Item2) == ECondition.Between)
                        {
                            if (string.IsNullOrEmpty(entity.TabAlias))
                            {
                                command.Command.Parameters.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", tableInfo.Table.Name, "_Begin_", tableInfo.PNameRelation[@params.Item2.Item1].ColumnName }), @params.Item2.Item3[0]));
                                command.Command.Parameters.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", tableInfo.Table.Name, "_End_", tableInfo.PNameRelation[@params.Item2.Item1].ColumnName }), @params.Item2.Item3[1]));
                            }
                            else
                            {
                                command.Command.Parameters.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", entity.TabAlias, "_Begin_", tableInfo.PNameRelation[@params.Item2.Item1].ColumnName }), @params.Item2.Item3[0]));
                                command.Command.Parameters.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", entity.TabAlias, "_End_", tableInfo.PNameRelation[@params.Item2.Item1].ColumnName }), @params.Item2.Item3[1]));
                            }
                        }
                        else if (((ECondition) @params.Item2.Item2) == ECondition.Like)
                        {
                            if (string.IsNullOrEmpty(entity.TabAlias))
                            {
                                command.Command.Parameters.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", tableInfo.Table.Name, "_", @params.Item2.Item1 }), @params.Item2.Item3[0]));
                            }
                            else
                            {
                                command.Command.Parameters.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", entity.TabAlias, "_", @params.Item2.Item1 }), @params.Item2.Item3[0]));
                            }
                        }
                        else if (((((ECondition) @params.Item2.Item2) == ECondition.Is) || (((ECondition) @params.Item2.Item2) == ECondition.IsNot)) || (((ECondition) @params.Item2.Item2) == ECondition.In))
                        {
                            if (((((ECondition) @params.Item2.Item2) == ECondition.In) && (@params.Item2.Item3.Length == 1)) && (@params.Item2.Item3[0] is BaseEntity))
                            {
                                entity2 = @params.Item2.Item3[0] as BaseEntity;
                                info = EntityTypeCache.Get(entity2.GetType());
                                this.SetSqlParamter(entity2, info, command, EOperator.Select);
                            }
                        }
                        else if (string.IsNullOrEmpty(entity.TabAlias))
                        {
                            command.Command.Parameters.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", tableInfo.Table.Name, "_", tableInfo.PNameRelation[@params.Item2.Item1].ColumnName }), @params.Item2.Item3[0]));
                        }
                        else
                        {
                            command.Command.Parameters.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", entity.TabAlias, "_", tableInfo.PNameRelation[@params.Item2.Item1].ColumnName }), @params.Item2.Item3[0]));
                        }
                    }
                    num++;
                }
            }
            if (entity.JoinColumn.Count > 0)
            {
                foreach (BaseEntity entity3 in from item in entity.JoinColumn select item.Item1)
                {
                    this.SetSqlParamter(entity3, EntityTypeCache.Get(entity3.GetType()), ref command);
                }
            }
        }

        private void SetSqlParamter(BaseEntity argEntity, TableInfo argTableInfo, DataCommand command, EOperator argOperator)
        {
            if ((EOperator.Add == argOperator) || (EOperator.Update == argOperator))
            {
                using (List<string>.Enumerator enumerator = argEntity.ColumnList.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Func<PropertyInfo, bool> func = null;
                        string item = enumerator.Current;
                        if (!argTableInfo.PNameRelation[item].AutoIncrement && !argTableInfo.PNameRelation[item].PrimaryKey)
                        {
                            if (func == null)
                            {
                                func = p => p.Name == item;
                            }
                            object obj2 = Enumerable.Single<PropertyInfo>(argTableInfo.Properties, func).GetValue(argEntity, null);
                            SqlParameter parameter = new SqlParameter {
                                ParameterName = "@" + argTableInfo.PNameRelation[item].ColumnName,
                                Value = obj2
                            };
                            if ((((argTableInfo.PNameRelation[item].DbType == DbType.Date) || (argTableInfo.PNameRelation[item].DbType == DbType.DateTime)) || (argTableInfo.PNameRelation[item].DbType == DbType.DateTime2)) || (argTableInfo.PNameRelation[item].DbType == DbType.DateTimeOffset))
                            {
                                parameter.SqlDbType = SqlDbType.DateTime;
                                if (Convert.ToDateTime(parameter.Value) == DateTime.MinValue)
                                {
                                    parameter.Value = DBNull.Value;
                                }
                            }
                            parameter.Value = (parameter.Value == null) ? (parameter.Value = DBNull.Value) : parameter.Value;
                            command.Command.Parameters.Add(parameter);
                        }
                    }
                }
            }
            this.SetSqlParamter(argEntity, argTableInfo, ref command);
        }

        private void SetSqlParamter(T argEntity, TableInfo argTableInfo, DataCommand command, EOperator argOperator)
        {
            if ((EOperator.Add == argOperator) || (EOperator.Update == argOperator))
            {
                using (List<string>.Enumerator enumerator = argEntity.ColumnList.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Func<PropertyInfo, bool> func = null;
                        string item = enumerator.Current;
                        if (!argTableInfo.PNameRelation[item].AutoIncrement && !argTableInfo.PNameRelation[item].PrimaryKey)
                        {
                            string columnName = argTableInfo.PNameRelation[item].ColumnName;
                            object obj2 = null;
                            if (argEntity.DicColumn.ContainsKey(item))
                            {
                                columnName = argEntity.DicColumn[item].Item1;
                                obj2 = argEntity.DicColumn[item].Item3;
                            }
                            else
                            {
                                if (func == null)
                                {
                                    func = p => p.Name == item;
                                }
                                obj2 = Enumerable.Single<PropertyInfo>(argTableInfo.Properties, func).GetValue(argEntity, null);
                            }
                            SqlParameter parameter = new SqlParameter {
                                ParameterName = "@" + columnName,
                                Value = obj2
                            };
                            if ((((argTableInfo.PNameRelation[item].DbType == DbType.Date) || (argTableInfo.PNameRelation[item].DbType == DbType.DateTime)) || (argTableInfo.PNameRelation[item].DbType == DbType.DateTime2)) || (argTableInfo.PNameRelation[item].DbType == DbType.DateTimeOffset))
                            {
                                parameter.SqlDbType = SqlDbType.DateTime;
                                if (Convert.ToDateTime(parameter.Value) == DateTime.MinValue)
                                {
                                    parameter.Value = DBNull.Value;
                                }
                            }
                            parameter.Value = (parameter.Value == null) ? (parameter.Value = DBNull.Value) : parameter.Value;
                            command.Command.Parameters.Add(parameter);
                        }
                    }
                }
            }
            this.SetSqlParamter(argEntity, argTableInfo, ref command);
        }

        private void SetSqlParamter(BaseEntity entity, TableInfo tableInfo, ref List<SqlParameter> list, int index)
        {
            BaseEntity entity2;
            TableInfo info;
            if (entity.ConditionCollection.Count > 0)
            {
                using (List<Params<JointType, Params<string, ECondition, object[]>>>.Enumerator enumerator = entity.ConditionCollection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Func<PropertyInfo, bool> func = null;
                        Params<JointType, Params<string, ECondition, object[]>> param = enumerator.Current;
                        if ((((((JointType) param.Item1) != JointType.Begin) && (((JointType) param.Item1) != JointType.WhereBegin)) && ((((JointType) param.Item1) != JointType.AndBegin) && (((JointType) param.Item1) != JointType.OrBegin))) && (((JointType) param.Item1) != JointType.End))
                        {
                            if (((ECondition) param.Item2.Item2) == ECondition.Between)
                            {
                                if (string.IsNullOrEmpty(entity.TabAlias))
                                {
                                    list.Add(new SqlParameter(string.Concat(new object[] { "@", tableInfo.Table.Name, "_Begin_", tableInfo.PNameRelation[param.Item2.Item1].ColumnName, "_", index }), param.Item2.Item3[0]));
                                    list.Add(new SqlParameter(string.Concat(new object[] { "@", tableInfo.Table.Name, "_End_", tableInfo.PNameRelation[param.Item2.Item1].ColumnName, "_", index }), param.Item2.Item3[1]));
                                }
                                else
                                {
                                    list.Add(new SqlParameter(string.Concat(new object[] { "@", entity.TabAlias, "_Begin_", tableInfo.PNameRelation[param.Item2.Item1].ColumnName, "_", index }), param.Item2.Item3[0]));
                                    list.Add(new SqlParameter(string.Concat(new object[] { "@", entity.TabAlias, "_End_", tableInfo.PNameRelation[param.Item2.Item1].ColumnName, "_", index }), param.Item2.Item3[1]));
                                }
                            }
                            else if (((ECondition) param.Item2.Item2) == ECondition.Like)
                            {
                                if (string.IsNullOrEmpty(entity.TabAlias))
                                {
                                    list.Add(new SqlParameter("@" + tableInfo.Table.Name + "_" + param.Item2.Item1, param.Item2.Item3[0]));
                                }
                                else
                                {
                                    list.Add(new SqlParameter("@" + entity.TabAlias + "_" + param.Item2.Item1, param.Item2.Item3[0]));
                                }
                            }
                            else if (((((ECondition) param.Item2.Item2) == ECondition.Is) || (((ECondition) param.Item2.Item2) == ECondition.IsNot)) || (((ECondition) param.Item2.Item2) == ECondition.In))
                            {
                                if (((((ECondition) param.Item2.Item2) == ECondition.In) && (param.Item2.Item3.Length == 1)) && (param.Item2.Item3[0] is BaseEntity))
                                {
                                    entity2 = param.Item2.Item3[0] as BaseEntity;
                                    info = EntityTypeCache.Get(entity2.GetType());
                                    this.SetSqlParamter(entity2, info, ref list, EOperator.Select, index);
                                }
                            }
                            else
                            {
                                if (func == null)
                                {
                                    func = p => p.Name == param.Item2.Item1;
                                }
                                object obj2 = Enumerable.Single<PropertyInfo>(tableInfo.Properties, func).GetValue(entity, null);
                                if (string.IsNullOrEmpty(entity.TabAlias))
                                {
                                    list.Add(new SqlParameter(string.Concat(new object[] { "@", tableInfo.Table.Name, "_", tableInfo.PNameRelation[param.Item2.Item1].ColumnName, "_", index }), obj2));
                                }
                                else
                                {
                                    list.Add(new SqlParameter(string.Concat(new object[] { "@", entity.TabAlias, "_", tableInfo.PNameRelation[param.Item2.Item1].ColumnName, "_", index }), obj2));
                                }
                            }
                        }
                    }
                }
            }
            if (entity.Conditions.Count > 0)
            {
                int num = 0;
                foreach (Params<JointType, Params<string, ECondition, object[]>> @params in entity.Conditions)
                {
                    if ((((((JointType) @params.Item1) != JointType.Begin) && (((JointType) @params.Item1) != JointType.WhereBegin)) && ((((JointType) @params.Item1) != JointType.AndBegin) && (((JointType) @params.Item1) != JointType.OrBegin))) && (((JointType) @params.Item1) != JointType.End))
                    {
                        if (((ECondition) @params.Item2.Item2) == ECondition.Between)
                        {
                            if (string.IsNullOrEmpty(entity.TabAlias))
                            {
                                list.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", tableInfo.Table.Name, "_Begin_", tableInfo.PNameRelation[@params.Item2.Item1].ColumnName, "_", index }), @params.Item2.Item3[0]));
                                list.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", tableInfo.Table.Name, "_End_", tableInfo.PNameRelation[@params.Item2.Item1].ColumnName, "_", index }), @params.Item2.Item3[1]));
                            }
                            else
                            {
                                list.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", entity.TabAlias, "_Begin_", tableInfo.PNameRelation[@params.Item2.Item1].ColumnName, "_", index }), @params.Item2.Item3[0]));
                                list.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", entity.TabAlias, "_End_", tableInfo.PNameRelation[@params.Item2.Item1].ColumnName, "_", index }), @params.Item2.Item3[1]));
                            }
                        }
                        else if (((ECondition) @params.Item2.Item2) == ECondition.Like)
                        {
                            if (string.IsNullOrEmpty(entity.TabAlias))
                            {
                                list.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", tableInfo.Table.Name, "_", @params.Item2.Item1 }), @params.Item2.Item3[0]));
                            }
                            else
                            {
                                list.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", entity.TabAlias, "_", @params.Item2.Item1 }), @params.Item2.Item3[0]));
                            }
                        }
                        else if (((((ECondition) @params.Item2.Item2) == ECondition.Is) || (((ECondition) @params.Item2.Item2) == ECondition.IsNot)) || (((ECondition) @params.Item2.Item2) == ECondition.In))
                        {
                            if (((((ECondition) @params.Item2.Item2) == ECondition.In) && (@params.Item2.Item3.Length == 1)) && (@params.Item2.Item3[0] is BaseEntity))
                            {
                                entity2 = @params.Item2.Item3[0] as BaseEntity;
                                info = EntityTypeCache.Get(entity2.GetType());
                                this.SetSqlParamter(entity2, info, ref list, EOperator.Select, index);
                            }
                        }
                        else if (string.IsNullOrEmpty(entity.TabAlias))
                        {
                            list.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", tableInfo.Table.Name, "_", tableInfo.PNameRelation[@params.Item2.Item1].ColumnName, "_", index }), @params.Item2.Item3[0]));
                        }
                        else
                        {
                            list.Add(new SqlParameter(string.Concat(new object[] { "@", num, "_", entity.TabAlias, "_", tableInfo.PNameRelation[@params.Item2.Item1].ColumnName, "_", index }), @params.Item2.Item3[0]));
                        }
                    }
                    num++;
                }
            }
            if (entity.JoinColumn.Count > 0)
            {
                foreach (BaseEntity entity3 in from item in entity.JoinColumn select item.Item1)
                {
                    this.SetSqlParamter(entity3, EntityTypeCache.Get(entity3.GetType()), ref list, index);
                }
            }
        }

        private void SetSqlParamter(BaseEntity argEntity, TableInfo argTableInfo, ref List<SqlParameter> list, EOperator argOperator, int index)
        {
            list = list.IsNull() ? new List<SqlParameter>() : list;
            if ((EOperator.Add == argOperator) || (EOperator.Update == argOperator))
            {
                using (List<string>.Enumerator enumerator = argEntity.ColumnList.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Func<PropertyInfo, bool> func = null;
                        string item = enumerator.Current;
                        if (!argTableInfo.PNameRelation[item].AutoIncrement && !argTableInfo.PNameRelation[item].PrimaryKey)
                        {
                            if (func == null)
                            {
                                func = p => p.Name == item;
                            }
                            object obj2 = Enumerable.Single<PropertyInfo>(argTableInfo.Properties, func).GetValue(argEntity, null);
                            SqlParameter parameter = new SqlParameter {
                                ParameterName = string.Concat(new object[] { "@", argTableInfo.PNameRelation[item].ColumnName, "_", index }),
                                Value = obj2
                            };
                            if ((((argTableInfo.PNameRelation[item].DbType == DbType.Date) || (argTableInfo.PNameRelation[item].DbType == DbType.DateTime)) || (argTableInfo.PNameRelation[item].DbType == DbType.DateTime2)) || (argTableInfo.PNameRelation[item].DbType == DbType.DateTimeOffset))
                            {
                                parameter.SqlDbType = SqlDbType.DateTime;
                                if (Convert.ToDateTime(parameter.Value) == DateTime.MinValue)
                                {
                                    parameter.Value = DBNull.Value;
                                }
                            }
                            parameter.Value = (parameter.Value == null) ? (parameter.Value = DBNull.Value) : parameter.Value;
                            list.Add(parameter);
                        }
                    }
                }
            }
            this.SetSqlParamter(argEntity, argTableInfo, ref list, index);
        }

        private void SetSqlParamter(T argEntity, TableInfo argTableInfo, ref List<SqlParameter> list, EOperator argOperator, int index)
        {
            list = list.IsNull() ? new List<SqlParameter>() : list;
            if ((EOperator.Add == argOperator) || (EOperator.Update == argOperator))
            {
                using (List<string>.Enumerator enumerator = argEntity.ColumnList.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Func<PropertyInfo, bool> func = null;
                        string item = enumerator.Current;
                        if (!argTableInfo.PNameRelation[item].AutoIncrement && !argTableInfo.PNameRelation[item].PrimaryKey)
                        {
                            if (func == null)
                            {
                                func = p => p.Name == item;
                            }
                            object obj2 = Enumerable.Single<PropertyInfo>(argTableInfo.Properties, func).GetValue(argEntity, null);
                            SqlParameter parameter = new SqlParameter {
                                ParameterName = string.Concat(new object[] { "@", argTableInfo.PNameRelation[item].ColumnName, "_", index }),
                                Value = obj2
                            };
                            if ((((argTableInfo.PNameRelation[item].DbType == DbType.Date) || (argTableInfo.PNameRelation[item].DbType == DbType.DateTime)) || (argTableInfo.PNameRelation[item].DbType == DbType.DateTime2)) || (argTableInfo.PNameRelation[item].DbType == DbType.DateTimeOffset))
                            {
                                parameter.SqlDbType = SqlDbType.DateTime;
                                if (Convert.ToDateTime(parameter.Value) == DateTime.MinValue)
                                {
                                    parameter.Value = DBNull.Value;
                                }
                            }
                            parameter.Value = (parameter.Value == null) ? (parameter.Value = DBNull.Value) : parameter.Value;
                            list.Add(parameter);
                        }
                    }
                }
            }
            this.SetSqlParamter(argEntity, argTableInfo, ref list, index);
        }

        public V Sum<V>(T entity)
        {
            return this.Sum<V>(entity, false);
        }

        public V Sum<V>(T entity, bool isOpenTrans)
        {
            this.ReName(entity);
            string format = "SELECT ISNULL(SUM({0}),0) FROM [dbo].[{1}] AS {4} {3} {2}";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            format = string.Format(format, new object[] { sqlColumn, tableInfo.Table.Name, sqlCondition, join, entity.TabAlias });
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            return command.ExecuteScalar<V>(isOpenTrans);
        }

        public List<T> Top(T entity, int pageSize)
        {
            return this.Top(entity, pageSize, false);
        }

        public List<V> Top<V>(T entity, int pageSize) where V: class, new()
        {
            return this.Top<V>(entity, pageSize, false);
        }

        public List<T> Top(T entity, int pageSize, bool isOpenTrans)
        {
            this.ReName(entity);
            string format = ((string.Empty + "WITH TempTable ") + "AS( " + "SELECT ROW_NUMBER() OVER ({0}) RowNumber,{1} FROM [dbo].[{2}] AS {3} {4} {5} ") + ") " + "SELECT TOP ({6}) * FROM TempTable";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string orderByColumn = this.GetOrderByColumn(entity, tableInfo);
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            format = string.Format(format, new object[] { orderByColumn, sqlColumn, tableInfo.Table.Name, entity.TabAlias, join, sqlCondition, pageSize });
            this.log.Info(" SQL: " + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            return command.ExecuteEntityList<T>(isOpenTrans);
        }

        public List<V> Top<V>(T entity, int pageSize, bool isOpenTrans) where V: class, new()
        {
            this.ReName(entity);
            string format = ((string.Empty + "WITH TempTable ") + "AS( " + "SELECT ROW_NUMBER() OVER ({0}) RowNumber,{1} FROM [dbo].[{2}] AS {3} {4} {5} ") + ") " + "SELECT TOP ({6}) * FROM TempTable";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string orderByColumn = this.GetOrderByColumn(entity, tableInfo);
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            format = string.Format(format, new object[] { orderByColumn, sqlColumn, tableInfo.Table.Name, entity.TabAlias, join, sqlCondition, pageSize });
            this.log.Info(" SQL: " + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            return command.ExecuteEntityList<V>(isOpenTrans);
        }

        public List<T> Top(T entity, int skipSize, int pageSize)
        {
            return this.Top(entity, skipSize, pageSize, false);
        }

        public List<V> Top<V>(T entity, int skipSize, int pageSize) where V: class, new()
        {
            return this.Top<V>(entity, skipSize, pageSize, false);
        }

        public List<T> Top(T entity, int skipSize, int pageSize, bool isOpenTrans)
        {
            this.ReName(entity);
            string format = ((string.Empty + "WITH TempTable ") + "AS( " + "SELECT ROW_NUMBER() OVER ({0}) RowNumber,{1} FROM [dbo].[{2}] AS {3} {4} {5} ") + ") " + "SELECT TOP ({6}) * FROM TempTable WHERE RowNumber > {7}";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string orderByColumn = this.GetOrderByColumn(entity, tableInfo);
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            format = string.Format(format, new object[] { orderByColumn, sqlColumn, tableInfo.Table.Name, entity.TabAlias, join, sqlCondition, pageSize, skipSize });
            this.log.Info(" SQL: " + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            return command.ExecuteEntityList<T>(isOpenTrans);
        }

        public List<V> Top<V>(T entity, int skipSize, int pageSize, bool isOpenTrans) where V: class, new()
        {
            this.ReName(entity);
            string format = ((string.Empty + "WITH TempTable ") + "AS( " + "SELECT ROW_NUMBER() OVER ({0}) RowNumber,{1} FROM [dbo].[{2}] AS {3} {4} {5} ") + ") " + "SELECT TOP ({6}) * FROM TempTable WHERE RowNumber > {7}";
            TableInfo tableInfo = EntityTypeCache.Get<T>();
            string orderByColumn = this.GetOrderByColumn(entity, tableInfo);
            string sqlColumn = this.GetSqlColumn(entity, tableInfo);
            string join = this.GetJoin(entity, tableInfo);
            string sqlCondition = this.GetSqlCondition(entity, tableInfo);
            format = string.Format(format, new object[] { orderByColumn, sqlColumn, tableInfo.Table.Name, entity.TabAlias, join, sqlCondition, pageSize, skipSize });
            this.log.Info(" SQL: " + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(tableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, tableInfo, command, EOperator.Select);
            return command.ExecuteEntityList<V>(isOpenTrans);
        }

        public int Update(T entity)
        {
            return this.Update(entity, false);
        }

        public int Update(List<T> list)
        {
            return this.Update(list, false);
        }

        public int Update(T entity, bool isOpenTrans)
        {
            string format = "UPDATE [dbo].[{0}] SET {1} {2}";
            TableInfo argTableInfo = EntityTypeCache.Get<T>();
            StringBuilder builder = new StringBuilder();
            if (entity.IncludeColumn.Count > 0)
            {
                foreach (KeyValuePair<string, string> pair in entity.IncludeColumn)
                {
                    if (!entity.ColumnList.Contains(pair.Key))
                    {
                        entity.ColumnList.Add(pair.Key);
                    }
                }
            }
            foreach (string str2 in entity.ColumnList)
            {
                if (!argTableInfo.PNameRelation[str2].AutoIncrement && !argTableInfo.PNameRelation[str2].PrimaryKey)
                {
                    if (entity.DicColumn.ContainsKey(str2))
                    {
                        builder.AppendFormat("[{0}]=[{2}]{3}@{1},", new object[] { argTableInfo.PNameRelation[str2].ColumnName, entity.DicColumn[str2].Item1, entity.DicColumn[str2].Item1, ConvertECondition.ToType(entity.DicColumn[str2].Item2) });
                    }
                    else
                    {
                        builder.AppendFormat("[{0}]=@{1},", argTableInfo.PNameRelation[str2].ColumnName, argTableInfo.PNameRelation[str2].ColumnName);
                    }
                }
            }
            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - 1, 1);
            }
            else
            {
                this.log.Info("添加数据SQL语句缺少必要参数");
                throw new Exception("修改数据SQL语句缺少必要参数");
            }
            string sqlCondition = this.GetSqlCondition(entity, argTableInfo);
            format = string.Format(format, argTableInfo.Table.Name, builder.ToString(), sqlCondition);
            this.log.Info(" SQL :" + format);
            DataCommand command = DataCommandManager.CreateCustomDataCommand(argTableInfo.Table.DbName, CommandType.Text, format);
            this.SetSqlParamter(entity, argTableInfo, command, EOperator.Update);
            return command.ExecuteNonQuery(isOpenTrans);
        }

        public int Update(List<T> list, bool isOpenTrans)
        {
            list.ThrowIfNullOrEmpty<T>("批量修改集合为空");
            StringBuilder builder = new StringBuilder();
            List<SqlParameter> list2 = new List<SqlParameter>();
            TableInfo argTableInfo = EntityTypeCache.Get<T>();
            for (int i = 0; i < list.Count; i++)
            {
                string format = "UPDATE [dbo].[{0}] SET {1} {2} ;";
                StringBuilder builder2 = new StringBuilder();
                if (list[i].IncludeColumn.Count > 0)
                {
                    foreach (KeyValuePair<string, string> pair in list[i].IncludeColumn)
                    {
                        if (!list[i].ColumnList.Contains(pair.Key))
                        {
                            list[i].ColumnList.Add(pair.Key);
                        }
                    }
                }
                using (List<string>.Enumerator enumerator2 = list[i].ColumnList.GetEnumerator())
                {
                    while (enumerator2.MoveNext())
                    {
                        Func<PropertyInfo, bool> func = null;
                        string item = enumerator2.Current;
                        if (!argTableInfo.PNameRelation[item].AutoIncrement && !argTableInfo.PNameRelation[item].PrimaryKey)
                        {
                            builder2.AppendFormat("[{0}]=@{1}_" + i + ",", argTableInfo.PNameRelation[item].ColumnName, argTableInfo.PNameRelation[item].ColumnName);
                            if (func == null)
                            {
                                func = p => p.Name == item;
                            }
                            object obj2 = Enumerable.Single<PropertyInfo>(argTableInfo.Properties, func).GetValue(list[i], null);
                        }
                    }
                }
                if (builder2.Length > 0)
                {
                    builder2.Remove(builder2.Length - 1, 1);
                }
                else
                {
                    this.log.Info("添加数据SQL语句缺少必要参数");
                    throw new Exception("修改数据SQL语句缺少必要参数");
                }
                string str2 = this.GetSqlCondition(list[i], argTableInfo, i);
                format = string.Format(format, argTableInfo.Table.Name, builder2.ToString(), str2);
                this.log.Info(" SQL :" + format);
                this.SetSqlParamter(list[i], argTableInfo, ref list2, EOperator.Update, i);
                builder.Append(format);
            }
            DataCommand command = DataCommandManager.CreateCustomDataCommand(argTableInfo.Table.DbName, CommandType.Text, builder.ToString());
            command.Command.Parameters.AddRange(list2);
            return command.ExecuteNonQuery(isOpenTrans);
        }
    }
}

