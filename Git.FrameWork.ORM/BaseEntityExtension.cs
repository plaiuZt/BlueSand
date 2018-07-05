using Git.Framework.DataTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Git.Framework.ORM
{

    public static class BaseEntityExtension
    {
        public static T And<T>(this T entity, Expression<Func<T, bool>> func) where T: BaseEntity
        {
            if (func.Body is BinaryExpression)
            {
                BinaryExpression body = (BinaryExpression) func.Body;
                Params<string, ECondition, object[]> @params = LambdaProvider.BinaryProvider(body.Left, body.Right, body.NodeType);
                if (@params != null)
                {
                    entity.And(@params.Item1, @params.Item2, @params.Item3);
                }
                return entity;
            }
            return entity;
        }

        public static T And<T>(this T entity, string propertyName, ECondition condition) where T: BaseEntity
        {
            object obj2 = Enumerable.Single<PropertyInfo>(EntityTypeCache.Get(entity.GetType()).Properties, (Func<PropertyInfo, bool>) (item => (item.Name == propertyName))).GetValue(entity, null);
            entity.And(propertyName, condition, new object[] { obj2 });
            return entity;
        }

        public static T And<T>(this T entity, string propertyName, ECondition condition, params object[] values) where T: BaseEntity
        {
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.And
            };
            Params<string, ECondition, object[]> params2 = new Params<string, ECondition, object[]> {
                Item1 = propertyName,
                Item2 = condition,
                Item3 = values
            };
            item.Item2 = params2;
            entity.Conditions.Add(item);
            return entity;
        }

        public static T AndBegin<T>(this T entity) where T: BaseEntity
        {
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.AndBegin,
                Item2 = null
            };
            entity.Conditions.Add(item);
            return entity;
        }

        public static T Begin<T>(this T entity) where T: BaseEntity
        {
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.Begin,
                Item2 = null
            };
            entity.Conditions.Add(item);
            return entity;
        }

        public static T Between<T>(this T entity, string propertyName, object[] items) where T: BaseEntity
        {
            if ((items == null) || (items.Length != 2))
            {
                throw new Exception("Between方法输入参数必须为两个");
            }
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.And
            };
            Params<string, ECondition, object[]> params2 = new Params<string, ECondition, object[]> {
                Item1 = propertyName,
                Item2 = ECondition.Between,
                Item3 = items
            };
            item.Item2 = params2;
            entity.Conditions.Add(item);
            return entity;
        }

        public static void Clear<T>(this T entity) where T: BaseEntity
        {
            entity.IncludeColumn.Clear();
            entity.ColumnList.Clear();
        }

        public static T End<T>(this T entity) where T: BaseEntity
        {
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.End,
                Item2 = null
            };
            entity.Conditions.Add(item);
            return entity;
        }

        public static T Exclude<T, TKey>(this T entity, Expression<Func<T, TKey>> keySelector) where T: BaseEntity
        {
            MemberExpression expression4;
            if (keySelector.Body is BinaryExpression)
            {
                BinaryExpression body = (BinaryExpression) keySelector.Body;
                Params<string, ECondition, object[]> @params = LambdaProvider.BinaryProvider(body.Left, body.Right, body.NodeType);
                if (@params != null)
                {
                    if (entity.ColumnList.Contains(@params.Item1))
                    {
                        entity.ColumnList.Remove(@params.Item1);
                    }
                    if (entity.IncludeColumn.ContainsKey(@params.Item1))
                    {
                        entity.IncludeColumn.Remove(@params.Item1);
                    }
                }
                return entity;
            }
            if (keySelector.Body is NewExpression)
            {
                NewExpression expression2 = (NewExpression) keySelector.Body;
                ReadOnlyCollection<Expression> arguments = expression2.Arguments;
                ReadOnlyCollection<MemberInfo> members = expression2.Members;
                for (int i = 0; i < arguments.Count; i++)
                {
                    Expression expression3 = arguments[i];
                    string item = string.Empty;
                    if (expression3 is MemberExpression)
                    {
                        expression4 = (MemberExpression) expression3;
                        item = expression4.Member.Name;
                    }
                    MemberInfo info = members[i];
                    string name = info.Name;
                    if (string.IsNullOrEmpty(name))
                    {
                        name = item;
                    }
                    if (name.StartsWith("get_"))
                    {
                        name = name.Substring(4);
                    }
                    if (entity.ColumnList.Contains(item))
                    {
                        entity.ColumnList.Remove(item);
                    }
                    if (entity.IncludeColumn.ContainsKey(item))
                    {
                        entity.IncludeColumn.Remove(item);
                    }
                }
                return entity;
            }
            if (keySelector.Body is ParameterExpression)
            {
                ParameterExpression expression5 = keySelector.Body as ParameterExpression;
                if (expression5.Type == entity.GetType())
                {
                    entity.Clear<T>();
                }
                return entity;
            }
            if (keySelector.Body is MemberInitExpression)
            {
                MemberInitExpression expression6 = keySelector.Body as MemberInitExpression;
                ReadOnlyCollection<MemberBinding> bindings = expression6.Bindings;
                foreach (MemberBinding binding in bindings)
                {
                    if (entity.ColumnList.Contains(binding.Member.Name))
                    {
                        entity.ColumnList.Remove(binding.Member.Name);
                    }
                    if (entity.IncludeColumn.ContainsKey(binding.Member.Name))
                    {
                        entity.IncludeColumn.Remove(binding.Member.Name);
                    }
                }
                return entity;
            }
            if (keySelector.Body is MemberExpression)
            {
                expression4 = (MemberExpression) keySelector.Body;
                if (expression4 == null)
                {
                    return entity;
                }
                if (entity.ColumnList.Contains(expression4.Member.Name))
                {
                    entity.ColumnList.Remove(expression4.Member.Name);
                }
                if (entity.IncludeColumn.ContainsKey(expression4.Member.Name))
                {
                    entity.IncludeColumn.Remove(expression4.Member.Name);
                }
            }
            return entity;
        }

        public static T Exclude<T>(this T entity, string propertyName) where T: BaseEntity
        {
            if (entity.ColumnList.Contains(propertyName))
            {
                entity.ColumnList.Remove(propertyName);
            }
            if (entity.IncludeColumn.ContainsKey(propertyName))
            {
                entity.IncludeColumn.Remove(propertyName);
            }
            return entity;
        }

        public static V Full<T, V>(this V target, T entity, params Params<string, string>[] param) where T: BaseEntity where V: BaseEntity
        {
            List<Params<Type, string, Type, string>> list = new List<Params<Type, string, Type, string>>();
            if (param != null)
            {
                foreach (Params<string, string> @params in param)
                {
                    Params<Type, string, Type, string> params2 = new Params<Type, string, Type, string> {
                        Item1 = target.GetType(),
                        Item2 = @params.Item1,
                        Item3 = entity.GetType(),
                        Item4 = @params.Item2
                    };
                    list.Add(params2);
                }
            }
            Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType> item = new Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType> {
                Item1 = entity,
                Item2 = list,
                Item3 = JoinType.Full
            };
            target.JoinColumn.Add(item);
            return target;
        }

        public static V Full<T, V, TKey>(this V target, T entity, Expression<Func<V, TKey>> keyLeft, Expression<Func<T, TKey>> keyRight) where T: BaseEntity where V: BaseEntity
        {
            MemberExpression body;
            string name = "";
            string str2 = "";
            Params<string, string>[] param = new Params<string, string>[] { new Params<string, string>() };
            if (keyLeft.Body is MemberExpression)
            {
                body = (MemberExpression) keyLeft.Body;
                name = body.Member.Name;
            }
            if (keyRight.Body is MemberExpression)
            {
                body = (MemberExpression) keyLeft.Body;
                str2 = body.Member.Name;
            }
            if (!(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(str2)))
            {
                param[0].Item1 = name;
                param[0].Item2 = str2;
                return target.Full<T, V>(entity, param);
            }
            return target;
        }

        public static T Group<T, TKey>(this T entity, Expression<Func<T, TKey>> keySelector) where T: BaseEntity
        {
            if (keySelector.Body is MemberExpression)
            {
                MemberExpression body = (MemberExpression) keySelector.Body;
                entity.Group(body.Member.Name);
                return entity;
            }
            return entity;
        }

        public static T Group<T>(this T entity, string propertyName) where T: BaseEntity
        {
            if (!entity.GroupColumn.Contains(propertyName))
            {
                entity.GroupColumn.Add(propertyName);
            }
            return entity;
        }

        public static T In<T>(this T entity, string propertyName, object[] items) where T: BaseEntity
        {
            if ((items == null) || (items.Count<object>() == 0))
            {
                throw new Exception("In方法输入参数不能为空");
            }
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.And
            };
            Params<string, ECondition, object[]> params2 = new Params<string, ECondition, object[]> {
                Item1 = propertyName,
                Item2 = ECondition.In,
                Item3 = items
            };
            item.Item2 = params2;
            entity.Conditions.Add(item);
            return entity;
        }

        public static T Include<T, TKey>(this T entity, Expression<Func<T, TKey>> keySelector) where T: BaseEntity
        {
            MemberExpression expression4;
            if (keySelector.Body is BinaryExpression)
            {
                BinaryExpression body = (BinaryExpression) keySelector.Body;
                Params<string, ECondition, object[]> @params = LambdaProvider.BinaryProvider(body.Left, body.Right, body.NodeType);
                if ((@params != null) && !entity.ColumnList.Contains(@params.Item1))
                {
                    entity.ColumnList.Add(@params.Item1);
                }
                return entity;
            }
            if (keySelector.Body is NewExpression)
            {
                NewExpression expression2 = (NewExpression) keySelector.Body;
                ReadOnlyCollection<Expression> arguments = expression2.Arguments;
                ReadOnlyCollection<MemberInfo> members = expression2.Members;
                for (int i = 0; i < arguments.Count; i++)
                {
                    Expression expression3 = arguments[i];
                    string key = string.Empty;
                    if (expression3 is MemberExpression)
                    {
                        expression4 = (MemberExpression) expression3;
                        key = expression4.Member.Name;
                    }
                    MemberInfo info = members[i];
                    string name = info.Name;
                    if (string.IsNullOrEmpty(name))
                    {
                        name = key;
                    }
                    if (name.StartsWith("get_"))
                    {
                        name = name.Substring(4);
                    }
                    entity.IncludeColumn.Add(key, name);
                }
                return entity;
            }
            if (keySelector.Body is ParameterExpression)
            {
                ParameterExpression expression5 = keySelector.Body as ParameterExpression;
                if (expression5.Type == entity.GetType())
                {
                    entity.IncludeAll();
                }
                return entity;
            }
            if (keySelector.Body is MemberInitExpression)
            {
                MemberInitExpression expression6 = keySelector.Body as MemberInitExpression;
                ReadOnlyCollection<MemberBinding> bindings = expression6.Bindings;
                foreach (MemberBinding binding in bindings)
                {
                    if (!entity.ColumnList.Contains(binding.Member.Name))
                    {
                        entity.ColumnList.Add(binding.Member.Name);
                    }
                }
                return entity;
            }
            if (keySelector.Body is MemberExpression)
            {
                expression4 = (MemberExpression) keySelector.Body;
                if (expression4 == null)
                {
                    return entity;
                }
                if (!entity.ColumnList.Contains(expression4.Member.Name))
                {
                    entity.ColumnList.Add(expression4.Member.Name);
                }
            }
            return entity;
        }

        public static T Include<T>(this T entity, string propertyName) where T: BaseEntity
        {
            if (!entity.ColumnList.Contains(propertyName))
            {
                entity.ColumnList.Add(propertyName);
            }
            return entity;
        }

        public static T Include<T>(this T entity, string propertyName, string alias) where T: BaseEntity
        {
            if (!entity.IncludeColumn.ContainsKey(propertyName))
            {
                entity.IncludeColumn.Add(propertyName, alias);
            }
            return entity;
        }

        public static V Inner<T, V>(this V target, T entity, params Params<string, string>[] param) where T: BaseEntity where V: BaseEntity
        {
            List<Params<Type, string, Type, string>> list = new List<Params<Type, string, Type, string>>();
            if (param != null)
            {
                foreach (Params<string, string> @params in param)
                {
                    Params<Type, string, Type, string> params2 = new Params<Type, string, Type, string> {
                        Item1 = target.GetType(),
                        Item2 = @params.Item1,
                        Item3 = entity.GetType(),
                        Item4 = @params.Item2
                    };
                    list.Add(params2);
                }
            }
            Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType> item = new Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType> {
                Item1 = entity,
                Item2 = list,
                Item3 = JoinType.Inner
            };
            target.JoinColumn.Add(item);
            return target;
        }

        public static V Inner<T, V, TKey>(this V target, T entity, Expression<Func<V, TKey>> keyLeft, Expression<Func<T, TKey>> keyRight) where T: BaseEntity where V: BaseEntity
        {
            MemberExpression body;
            string name = "";
            string str2 = "";
            Params<string, string>[] param = new Params<string, string>[] { new Params<string, string>() };
            if (keyLeft.Body is MemberExpression)
            {
                body = (MemberExpression) keyLeft.Body;
                name = body.Member.Name;
            }
            if (keyRight.Body is MemberExpression)
            {
                body = (MemberExpression) keyLeft.Body;
                str2 = body.Member.Name;
            }
            if (!(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(str2)))
            {
                param[0].Item1 = name;
                param[0].Item2 = str2;
                return target.Inner<T, V>(entity, param);
            }
            return target;
        }

        public static V Left<T, V>(this V target, T entity, params Params<string, string>[] param) where T: BaseEntity where V: BaseEntity
        {
            List<Params<Type, string, Type, string>> list = new List<Params<Type, string, Type, string>>();
            if (param != null)
            {
                foreach (Params<string, string> @params in param)
                {
                    Params<Type, string, Type, string> params2 = new Params<Type, string, Type, string> {
                        Item1 = target.GetType(),
                        Item2 = @params.Item1,
                        Item3 = entity.GetType(),
                        Item4 = @params.Item2
                    };
                    list.Add(params2);
                }
            }
            Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType> item = new Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType> {
                Item1 = entity,
                Item2 = list,
                Item3 = JoinType.Left
            };
            target.JoinColumn.Add(item);
            return target;
        }

        public static V Left<T, V, TKey>(this V target, T entity, Expression<Func<V, TKey>> keyLeft, Expression<Func<T, TKey>> keyRight) where T: BaseEntity where V: BaseEntity
        {
            MemberExpression body;
            string name = "";
            string str2 = "";
            Params<string, string>[] param = new Params<string, string>[] { new Params<string, string>() };
            if (keyLeft.Body is MemberExpression)
            {
                body = (MemberExpression) keyLeft.Body;
                name = body.Member.Name;
            }
            if (keyRight.Body is MemberExpression)
            {
                body = (MemberExpression) keyLeft.Body;
                str2 = body.Member.Name;
            }
            if (!(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(str2)))
            {
                param[0].Item1 = name;
                param[0].Item2 = str2;
                return target.Left<T, V>(entity, param);
            }
            return target;
        }

        public static bool Like(this string target, string value)
        {
            return true;
        }

        public static T Like<T>(this T entity, string propertyName, string keyword) where T: BaseEntity
        {
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.And
            };
            Params<string, ECondition, object[]> params2 = new Params<string, ECondition, object[]> {
                Item1 = propertyName,
                Item2 = ECondition.Like,
                Item3 = new object[] { keyword }
            };
            item.Item2 = params2;
            entity.Conditions.Add(item);
            return entity;
        }

        public static T Or<T>(this T entity, Expression<Func<T, bool>> func) where T: BaseEntity
        {
            if (func.Body is BinaryExpression)
            {
                BinaryExpression body = (BinaryExpression) func.Body;
                Params<string, ECondition, object[]> @params = LambdaProvider.BinaryProvider(body.Left, body.Right, body.NodeType);
                if (@params != null)
                {
                    entity.Or(@params.Item1, @params.Item2, @params.Item3);
                }
                return entity;
            }
            return entity;
        }

        public static T Or<T>(this T entity, string propertyName, ECondition condition) where T: BaseEntity
        {
            object obj2 = Enumerable.Single<PropertyInfo>(EntityTypeCache.Get(entity.GetType()).Properties, (Func<PropertyInfo, bool>) (item => (item.Name == propertyName))).GetValue(entity, null);
            entity.Or(propertyName, condition, new object[] { obj2 });
            return entity;
        }

        public static T Or<T>(this T entity, string propertyName, ECondition condition, params object[] values) where T: BaseEntity
        {
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.Or
            };
            Params<string, ECondition, object[]> params2 = new Params<string, ECondition, object[]> {
                Item1 = propertyName,
                Item2 = condition,
                Item3 = values
            };
            item.Item2 = params2;
            entity.Conditions.Add(item);
            return entity;
        }

        public static T OrBegin<T>(this T entity) where T: BaseEntity
        {
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.OrBegin,
                Item2 = null
            };
            entity.Conditions.Add(item);
            return entity;
        }

        public static T OrderBy<T, TKey>(this T entity, Expression<Func<T, TKey>> keySelector, EOrderBy orderBy) where T: BaseEntity
        {
            if (keySelector.Body is MemberExpression)
            {
                MemberExpression body = (MemberExpression) keySelector.Body;
                entity.OrderBy(body.Member.Name, orderBy);
                return entity;
            }
            return entity;
        }

        public static T OrderBy<T>(this T entity, string propertyName, EOrderBy orderBy) where T: BaseEntity
        {
            if (!entity.OrderByColumn.ContainsKey(propertyName))
            {
                entity.OrderByColumn.Add(propertyName, orderBy);
            }
            return entity;
        }

        public static V Right<T, V>(this V target, T entity, params Params<string, string>[] param) where T: BaseEntity where V: BaseEntity
        {
            List<Params<Type, string, Type, string>> list = new List<Params<Type, string, Type, string>>();
            if (param != null)
            {
                foreach (Params<string, string> @params in param)
                {
                    Params<Type, string, Type, string> params2 = new Params<Type, string, Type, string> {
                        Item1 = target.GetType(),
                        Item2 = @params.Item1,
                        Item3 = entity.GetType(),
                        Item4 = @params.Item2
                    };
                    list.Add(params2);
                }
            }
            Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType> item = new Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType> {
                Item1 = entity,
                Item2 = list,
                Item3 = JoinType.Right
            };
            target.JoinColumn.Add(item);
            return target;
        }

        public static V Right<T, V, TKey>(this V target, T entity, Expression<Func<V, TKey>> keyLeft, Expression<Func<T, TKey>> keyRight) where T: BaseEntity where V: BaseEntity
        {
            MemberExpression body;
            string name = "";
            string str2 = "";
            Params<string, string>[] param = new Params<string, string>[] { new Params<string, string>() };
            if (keyLeft.Body is MemberExpression)
            {
                body = (MemberExpression) keyLeft.Body;
                name = body.Member.Name;
            }
            if (keyRight.Body is MemberExpression)
            {
                body = (MemberExpression) keyLeft.Body;
                str2 = body.Member.Name;
            }
            if (!(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(str2)))
            {
                param[0].Item1 = name;
                param[0].Item2 = str2;
                return target.Right<T, V>(entity, param);
            }
            return target;
        }

        public static T Where<T>(this T entity, Expression<Func<T, bool>> func) where T: BaseEntity
        {
            Params<string, ECondition, object[]> @params;
            if (func.Body is BinaryExpression)
            {
                BinaryExpression body = (BinaryExpression) func.Body;
                @params = LambdaProvider.BinaryProvider(body.Left, body.Right, body.NodeType);
                if (@params != null)
                {
                    entity.Where(@params.Item1, @params.Item2, @params.Item3);
                }
                return entity;
            }
            if (func.Body is MethodCallExpression)
            {
                ReadOnlyCollection<Expression> arguments;
                MemberExpression expression3;
                MethodCallExpression expression2 = (MethodCallExpression) func.Body;
                MethodInfo method = expression2.Method;
                if ((method != null) && (method.Name == "Like"))
                {
                    arguments = expression2.Arguments;
                    if ((arguments != null) && (arguments.Count == 2))
                    {
                        expression3 = (MemberExpression) arguments[0];
                        ConstantExpression expression4 = (ConstantExpression) arguments[1];
                        @params = new Params<string, ECondition, object[]> {
                            Item1 = expression3.Member.Name,
                            Item2 = ECondition.Like,
                            Item3 = new object[] { expression4.Value }
                        };
                        entity.Where(@params.Item1, @params.Item2, @params.Item3);
                    }
                    return entity;
                }
                if ((method != null) && (method.Name == "Contains"))
                {
                    arguments = expression2.Arguments;
                    if ((arguments != null) && (arguments.Count == 1))
                    {
                        expression3 = (MemberExpression) arguments[0];
                        MemberExpression expression5 = (MemberExpression) expression2.Object;
                        ConstantExpression expression = (ConstantExpression) expression5.Expression;
                        object obj2 = expression.Value;
                        List<string> list = obj2.GetType().GetField("list").GetValue(obj2) as List<string>;
                        if ((list != null) && (list.Count > 0))
                        {
                            @params = new Params<string, ECondition, object[]> {
                                Item1 = expression3.Member.Name,
                                Item2 = ECondition.In,
                                Item3 = list.ToArray()
                            };
                            entity.Where(@params.Item1, @params.Item2, @params.Item3);
                        }
                    }
                }
            }
            return entity;
        }

        public static T Where<T>(this T entity, string propertyName, ECondition condition) where T: BaseEntity
        {
            object obj2 = Enumerable.Single<PropertyInfo>(EntityTypeCache.Get(entity.GetType()).Properties, (Func<PropertyInfo, bool>) (item => (item.Name == propertyName))).GetValue(entity, null);
            entity.Where(propertyName, condition, new object[] { obj2 });
            return entity;
        }

        public static T Where<T>(this T entity, string propertyName, ECondition condition, params object[] values) where T: BaseEntity
        {
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.Where
            };
            Params<string, ECondition, object[]> params2 = new Params<string, ECondition, object[]> {
                Item1 = propertyName,
                Item2 = condition,
                Item3 = values
            };
            item.Item2 = params2;
            entity.Conditions.Add(item);
            return entity;
        }

        public static T WhereBegin<T>(this T entity) where T: BaseEntity
        {
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.WhereBegin,
                Item2 = null
            };
            entity.Conditions.Add(item);
            return entity;
        }
    }
}

