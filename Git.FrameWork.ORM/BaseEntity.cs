using Git.Framework.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;

namespace Git.Framework.ORM
{
    [Serializable]
    public class BaseEntity : IEntity, IDisposable
    {
        [NonSerialized, ScriptIgnore]
        public List<string> ColumnList = new List<string>();
        [NonSerialized, ScriptIgnore]
        public List<Params<JointType, Params<string, ECondition, object[]>>> ConditionCollection = new List<Params<JointType, Params<string, ECondition, object[]>>>();
        [NonSerialized, ScriptIgnore]
        public List<Params<JointType, Params<string, ECondition, object[]>>> Conditions = new List<Params<JointType, Params<string, ECondition, object[]>>>();
        [NonSerialized, ScriptIgnore]
        public Dictionary<string, Params<string, ECondition, object>> DicColumn = new Dictionary<string, Params<string, ECondition, object>>();
        [NonSerialized, ScriptIgnore]
        public Dictionary<string, Params<ECondition, object>> DicOperate = new Dictionary<string, Params<ECondition, object>>();
        [NonSerialized, ScriptIgnore]
        public List<Params<JointType, Params<string, ECondition, object[]>>> ExistsConditions = new List<Params<JointType, Params<string, ECondition, object[]>>>();
        [NonSerialized, ScriptIgnore]
        public List<string> GroupColumn = new List<string>();
        [NonSerialized, ScriptIgnore]
        public Dictionary<string, string> IncludeColumn = new Dictionary<string, string>();
        [NonSerialized, ScriptIgnore]
        public int Index;
        [NonSerialized, ScriptIgnore]
        public List<Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType>> JoinColumn = new List<Params<BaseEntity, List<Params<Type, string, Type, string>>, JoinType>>();
        [NonSerialized, ScriptIgnore]
        public Dictionary<string, EOrderBy> OrderByColumn = new Dictionary<string, EOrderBy>();
        [NonSerialized, ScriptIgnore]
        public string TabAlias;

        public void And(string propertyName, ECondition condition)
        {
            object obj2 = Enumerable.Single<PropertyInfo>(EntityTypeCache.Get(base.GetType()).Properties, (Func<PropertyInfo, bool>) (item => (item.Name == propertyName))).GetValue(this, null);
            this.And(propertyName, condition, new object[] { obj2 });
        }

        public void And(string propertyName, ECondition condition, params object[] values)
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
            this.Conditions.Add(item);
        }

        public void AndBegin()
        {
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.AndBegin,
                Item2 = null
            };
            this.Conditions.Add(item);
        }

        public void Begin()
        {
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.Begin,
                Item2 = null
            };
            this.Conditions.Add(item);
        }

        public void Between(string propertyName, object[] items)
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
            this.Conditions.Add(item);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void End()
        {
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.End,
                Item2 = null
            };
            this.Conditions.Add(item);
        }

        public void Exclude(string propertyName)
        {
            if (this.ColumnList.Contains(propertyName))
            {
                this.ColumnList.Remove(propertyName);
            }
            if (this.IncludeColumn.ContainsKey(propertyName))
            {
                this.IncludeColumn.Remove(propertyName);
            }
        }

        public void Full<T>(T entity, params Params<string, string>[] param) where T: BaseEntity
        {
            List<Params<Type, string, Type, string>> list = new List<Params<Type, string, Type, string>>();
            if (param != null)
            {
                foreach (Params<string, string> @params in param)
                {
                    Params<Type, string, Type, string> params2 = new Params<Type, string, Type, string> {
                        Item1 = base.GetType(),
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
            this.JoinColumn.Add(item);
        }

        public void Group(string propertyName)
        {
            if (!this.GroupColumn.Contains(propertyName))
            {
                this.GroupColumn.Add(propertyName);
            }
        }

        public void In(string propertyName, object[] items)
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
            this.Conditions.Add(item);
        }

        public void Include(string propertyName, ECondition condition, object value)
        {
            Params<string, ECondition, object> @params = null;
            @params = new Params<string, ECondition, object> {
                Item1 = propertyName,
                Item2 = condition,
                Item3 = value
            };
            if (this.DicColumn.ContainsKey(propertyName))
            {
                this.DicColumn[propertyName] = @params;
            }
            else
            {
                this.DicColumn.Add(propertyName, @params);
            }
        }

        public void IncludeAll()
        {
            PropertyInfo[] infoArray = EntityTypeCache.GeProperties(base.GetType());
            foreach (PropertyInfo info in infoArray)
            {
                if (((!this.ColumnList.Contains(info.Name) && EntityTypeCache.Get(base.GetType()).PDRelation.ContainsKey(info)) && (EntityTypeCache.Get(base.GetType()).PDRelation[info] != null)) && EntityTypeCache.Get(base.GetType()).PDRelation[info].IsMap)
                {
                    this.ColumnList.Add(info.Name);
                }
            }
        }

        public void Inner<T>(T entity, params Params<string, string>[] param) where T: BaseEntity
        {
            List<Params<Type, string, Type, string>> list = new List<Params<Type, string, Type, string>>();
            if (param != null)
            {
                foreach (Params<string, string> @params in param)
                {
                    Params<Type, string, Type, string> params2 = new Params<Type, string, Type, string> {
                        Item1 = base.GetType(),
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
            this.JoinColumn.Add(item);
        }

        public void Left<T>(T entity, params Params<string, string>[] param) where T: BaseEntity
        {
            List<Params<Type, string, Type, string>> list = new List<Params<Type, string, Type, string>>();
            if (param != null)
            {
                foreach (Params<string, string> @params in param)
                {
                    Params<Type, string, Type, string> params2 = new Params<Type, string, Type, string> {
                        Item1 = base.GetType(),
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
            this.JoinColumn.Add(item);
        }

        public void Like(string propertyName, string keyword)
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
            this.Conditions.Add(item);
        }

        public void Or(string propertyName, ECondition condition)
        {
            object obj2 = Enumerable.Single<PropertyInfo>(EntityTypeCache.Get(base.GetType()).Properties, (Func<PropertyInfo, bool>) (item => (item.Name == propertyName))).GetValue(this, null);
            this.Or(propertyName, condition, new object[] { obj2 });
        }

        public void Or(string propertyName, ECondition condition, params object[] values)
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
            this.Conditions.Add(item);
        }

        public void OrBegin()
        {
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.OrBegin,
                Item2 = null
            };
            this.Conditions.Add(item);
        }

        public void OrderBy(string propertyName, EOrderBy orderBy)
        {
            if (!this.OrderByColumn.ContainsKey(propertyName))
            {
                this.OrderByColumn.Add(propertyName, orderBy);
            }
        }

        public void Right<T>(T entity, params Params<string, string>[] param) where T: BaseEntity
        {
            List<Params<Type, string, Type, string>> list = new List<Params<Type, string, Type, string>>();
            if (param != null)
            {
                foreach (Params<string, string> @params in param)
                {
                    Params<Type, string, Type, string> params2 = new Params<Type, string, Type, string> {
                        Item1 = base.GetType(),
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
            this.JoinColumn.Add(item);
        }

        public void Where(string propertyName, ECondition condition)
        {
            object obj2 = Enumerable.Single<PropertyInfo>(EntityTypeCache.Get(base.GetType()).Properties, (Func<PropertyInfo, bool>) (item => (item.Name == propertyName))).GetValue(this, null);
            this.Where(propertyName, condition, new object[] { obj2 });
        }

        public void Where(string propertyName, ECondition condition, params object[] values)
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
            this.Conditions.Add(item);
        }

        public void WhereBegin()
        {
            Params<JointType, Params<string, ECondition, object[]>> item = new Params<JointType, Params<string, ECondition, object[]>> {
                Item1 = JointType.WhereBegin,
                Item2 = null
            };
            this.Conditions.Add(item);
        }
    }
}

