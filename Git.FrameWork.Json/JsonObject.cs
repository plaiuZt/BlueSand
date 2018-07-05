using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Git.Framework.Json
{
    public class JsonObject
    {
        private Dictionary<string, JsonProperty> _property;

        public JsonObject()
        {
            this._property = null;
        }

        public JsonObject(SetProperties callback)
        {
            if (callback != null)
            {
                callback(this);
            }
        }

        public JsonObject(string jsonString)
        {
            this.Parse(ref jsonString);
        }

        public JsonProperty AddProperty(string PropertyName)
        {
            if (this._property == null)
            {
                this._property = new Dictionary<string, JsonProperty>(StringComparer.OrdinalIgnoreCase);
            }
            if (!this._property.ContainsKey(PropertyName))
            {
                this._property.Add(PropertyName, new JsonProperty());
            }
            return this._property[PropertyName];
        }

        public JsonProperty AddProperty(string PropertyName, object Value)
        {
            if (this._property == null)
            {
                this._property = new Dictionary<string, JsonProperty>(StringComparer.OrdinalIgnoreCase);
            }
            if (!this._property.ContainsKey(PropertyName))
            {
                this._property.Add(PropertyName, new JsonProperty(Value));
            }
            return this._property[PropertyName];
        }

        public JsonProperty AddProperty(string PropertyName, params object[] Values)
        {
            if (this._property == null)
            {
                this._property = new Dictionary<string, JsonProperty>(StringComparer.OrdinalIgnoreCase);
            }
            if (!this._property.ContainsKey(PropertyName))
            {
                JsonProperty property = new JsonProperty();
                foreach (object obj2 in Values)
                {
                    property.Add(obj2);
                }
                this._property.Add(PropertyName, property);
            }
            return this._property[PropertyName];
        }

        public T GetEntity<T>() where T: new()
        {
            T local = new T();
            foreach (string str in this.GetPropertyNames())
            {
                PropertyInfo property = local.GetType().GetProperty(str, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property != null)
                {
                    string str2 = this[str].Value;
                    Type propertyType = property.PropertyType;
                    if (string.IsNullOrEmpty(str2))
                    {
                        if (propertyType.Equals(TypeCode.String))
                        {
                            property.SetValue(local, "", null);
                        }
                    }
                    else if (propertyType.Equals(TypeCode.Boolean))
                    {
                        property.SetValue(local, str2 == "1", null);
                    }
                    else if (propertyType.Equals(TypeCode.DateTime))
                    {
                        DateTime minValue;
                        if (!DateTime.TryParse(str2, out minValue))
                        {
                            minValue = DateTime.MinValue;
                        }
                        property.SetValue(local, minValue, null);
                    }
                    else
                    {
                        property.SetValue(local, Convert.ChangeType(str2, propertyType), null);
                    }
                }
            }
            return local;
        }

        public static T GetEntityFromJson<T>(JsonObject js, ref T a)
        {
            if (((T) a) != null)
            {
                if (js == null)
                {
                    return a;
                }
                PropertyInfo[] properties = a.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo info in properties)
                {
                    string s = null;
                    if (js.HasProperty(info.Name) || js.HasProperty(info.Name.ToLower()))
                    {
                        if (js[info.Name] == null)
                        {
                            if (js[info.Name.ToLower()] == null)
                            {
                                continue;
                            }
                            s = js[info.Name.ToLower()].Value;
                        }
                        else
                        {
                            s = js[info.Name].Value;
                        }
                        TypeCode typeCode = Type.GetTypeCode(info.PropertyType);
                        try
                        {
                            if (((s == null) || (s.Length == 0)) && (typeCode == TypeCode.String))
                            {
                                info.SetValue((T) a, "", null);
                            }
                            if (typeCode == TypeCode.Boolean)
                            {
                                info.SetValue((T) a, s == "1", null);
                            }
                            else if (typeCode == TypeCode.DateTime)
                            {
                                DateTime minValue;
                                if (!DateTime.TryParse(s, out minValue))
                                {
                                    minValue = DateTime.MinValue;
                                }
                                info.SetValue((T) a, minValue, null);
                            }
                            else
                            {
                                info.SetValue((T) a, Convert.ChangeType(s, info.PropertyType), null);
                            }
                        }
                        catch (Exception exception)
                        {
                            throw new ArgumentException("[" + info.Name + "]Json绑定失败：" + exception.Message);
                        }
                    }
                }
            }
            return a;
        }

        public string[] GetPropertyNames()
        {
            if (this._property == null)
            {
                return null;
            }
            string[] array = null;
            if (this._property.Count > 0)
            {
                array = new string[this._property.Count];
                this._property.Keys.CopyTo(array, 0);
            }
            return array;
        }

        public bool HasProperty(string PropertyName)
        {
            return ((this._property != null) && this._property.ContainsKey(PropertyName));
        }

        public bool IsNull()
        {
            return (this._property == null);
        }

        private void Parse(ref string jsonString)
        {
            string str;
            int length = jsonString.Length;
            if ((string.IsNullOrEmpty(jsonString) || (jsonString.Substring(0, 1) != "{")) || (jsonString.Substring(jsonString.Length - 1, 1) != "}"))
            {
                throw new ArgumentException("传入文本不符合Json格式!" + jsonString);
            }
            Stack<char> stack = new Stack<char>();
            Stack<char> stack2 = new Stack<char>();
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            bool flag2 = false;
            JsonProperty property = null;
            for (int i = 1; i <= (length - 2); i++)
            {
                char ch = jsonString[i];
                if (ch == '}')
                {
                }
                if ((ch == ' ') && (stack.Count == 0))
                {
                    builder.Append(ch);
                }
                else if (!((((ch != '\'') && (ch != '"')) || (flag || (stack.Count != 0))) || flag2))
                {
                    builder.Length = 0;
                    stack.Push(ch);
                }
                else if (!(((((ch != '\'') && (ch != '"')) || (flag || (stack.Count <= 0))) || (stack.Peek() != ch)) || flag2))
                {
                    stack.Pop();
                }
                else if (((ch == '[') || (ch == '{')) && (stack.Count == 0))
                {
                    stack2.Push((ch == '[') ? ']' : '}');
                    builder.Append(ch);
                }
                else if ((((ch == ']') || (ch == '}')) && (stack.Count == 0)) && (stack2.Peek() == ch))
                {
                    stack2.Pop();
                    builder.Append(ch);
                }
                else if (!((((ch != ':') || (stack.Count != 0)) || (stack2.Count != 0)) || flag2))
                {
                    property = new JsonProperty();
                    this[builder.ToString()] = property;
                    flag2 = true;
                    builder.Length = 0;
                }
                else if (((ch == ',') && (stack.Count == 0)) && (stack2.Count == 0))
                {
                    if (property != null)
                    {
                        str = builder.ToString();
                        property.Parse(ref str);
                    }
                    flag2 = false;
                    builder.Length = 0;
                }
                else
                {
                    builder.Append(ch);
                }
            }
            if (((builder.Length > 0) && (property != null)) && (property.Type == JsonPropertyType.Null))
            {
                str = builder.ToString();
                property.Parse(ref str);
            }
        }

        public virtual T Properties<T>(string PropertyName) where T: class
        {
            JsonProperty property = this[PropertyName];
            if (property != null)
            {
                return property.GetValue<T>();
            }
            return default(T);
        }

        public JsonProperty RemoveProperty(string PropertyName)
        {
            if ((this._property != null) && this._property.ContainsKey(PropertyName))
            {
                JsonProperty property = this._property[PropertyName];
                this._property.Remove(PropertyName);
                return property;
            }
            return null;
        }

        public JsonProperty Replace(string PropertyName, object Value)
        {
            if (this._property == null)
            {
                this._property = new Dictionary<string, JsonProperty>(StringComparer.OrdinalIgnoreCase);
            }
            if (this._property.ContainsKey(PropertyName))
            {
                this._property[PropertyName] = new JsonProperty(Value);
            }
            return this._property[PropertyName];
        }

        public override string ToString()
        {
            return this.ToString("");
        }

        public virtual string ToString(string format)
        {
            if (this.IsNull())
            {
                return "{}";
            }
            StringBuilder builder = new StringBuilder();
            foreach (string str in this._property.Keys)
            {
                builder.Append(",");
                builder.Append(string.Format("\"{0}\"", str)).Append(": ");
                builder.Append(this._property[str].ToString(format));
            }
            if (this._property.Count > 0)
            {
                builder.Remove(0, 1);
            }
            builder.Insert(0, "{");
            builder.Append("}");
            return builder.ToString();
        }

        public JsonProperty this[string PropertyName]
        {
            get
            {
                if ((this._property != null) && this._property.ContainsKey(PropertyName))
                {
                    return this._property[PropertyName];
                }
                return this.AddProperty(PropertyName);
            }
            set
            {
                if (this._property == null)
                {
                    this._property = new Dictionary<string, JsonProperty>(StringComparer.OrdinalIgnoreCase);
                }
                if (this._property.ContainsKey(PropertyName))
                {
                    this._property[PropertyName] = value;
                }
                else
                {
                    this._property.Add(PropertyName, value);
                }
            }
        }
    }
}

