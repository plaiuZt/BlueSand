using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Git.Framework.Json
{
    public class JsonProperty
    {
        private bool _bool;
        private List<JsonProperty> _list;
        private double _number;
        private JsonObject _object;
        private JsonPropertyType _type;
        private string _value;

        public JsonProperty()
        {
            this._type = JsonPropertyType.Null;
            this._value = null;
            this._object = null;
            this._list = null;
        }

        public JsonProperty(object value)
        {
            this.SetValue(value);
        }

        public JsonProperty(string jsonString)
        {
            this.Parse(ref jsonString);
        }

        public JsonProperty Add(object value)
        {
            if ((this._type != JsonPropertyType.Null) && (this._type != JsonPropertyType.Array))
            {
                throw new NotSupportedException("Json属性不是Array类型，无法添加元素!");
            }
            if (this._list == null)
            {
                this._list = new List<JsonProperty>();
            }
            JsonProperty item = new JsonProperty(value);
            this._list.Add(item);
            this._type = JsonPropertyType.Array;
            return item;
        }

        public void Clear()
        {
            this._type = JsonPropertyType.Null;
            this._value = string.Empty;
            this._object = null;
            if (this._list != null)
            {
                this._list.Clear();
                this._list = null;
            }
        }

        public object GetValue()
        {
            if (this._type == JsonPropertyType.String)
            {
                return this._value;
            }
            if (this._type == JsonPropertyType.Object)
            {
                return this._object;
            }
            if (this._type == JsonPropertyType.Array)
            {
                return this._list;
            }
            if (this._type == JsonPropertyType.Bool)
            {
                return this._bool;
            }
            if (this._type == JsonPropertyType.Number)
            {
                return this._number;
            }
            return null;
        }

        public virtual T GetValue<T>() where T: class
        {
            return (this.GetValue() as T);
        }

        public void Parse(ref string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString.Trim()))
            {
                this.SetValue(null);
            }
            else
            {
                jsonString = jsonString.Trim();
                string str = jsonString.Substring(0, 1);
                string str2 = jsonString.Substring(jsonString.Length - 1, 1);
                if ((str == "[") && (str2 == "]"))
                {
                    this.SetValue(this.ParseArray(ref jsonString));
                }
                else if ((str == "{") && (str2 == "}"))
                {
                    this.SetValue(this.ParseObject(ref jsonString));
                }
                else if (((str == "'") || (str == "\"")) && (str == str2))
                {
                    this.SetValue(this.ParseString(ref jsonString));
                }
                else if ((jsonString == "true") || (jsonString == "false"))
                {
                    this.SetValue(jsonString == "true");
                }
                else if (jsonString == "null")
                {
                    this.SetValue(null);
                }
                else
                {
                    double result = 0.0;
                    if (double.TryParse(jsonString, out result))
                    {
                        this.SetValue(result);
                    }
                    else
                    {
                        this.SetValue(jsonString);
                    }
                }
            }
        }

        private List<JsonProperty> ParseArray(ref string jsonString)
        {
            List<JsonProperty> list = new List<JsonProperty>();
            int length = jsonString.Length;
            StringBuilder builder = new StringBuilder();
            Stack<char> stack = new Stack<char>();
            Stack<char> stack2 = new Stack<char>();
            bool flag = false;
            for (int i = 1; i <= (length - 2); i++)
            {
                char c = jsonString[i];
                if (char.IsWhiteSpace(c) && (stack.Count == 0))
                {
                    builder.Append(c);
                }
                else if ((((c == '\'') && (stack.Count == 0)) && (!flag && (stack2.Count == 0))) || ((((c == '"') && (stack.Count == 0)) && !flag) && (stack2.Count == 0)))
                {
                    builder.Length = 0;
                    builder.Append(c);
                    stack.Push(c);
                }
                else if (!(((c != '\\') || (stack.Count <= 0)) || flag))
                {
                    builder.Append(c);
                    flag = true;
                }
                else if (flag)
                {
                    flag = false;
                    if (c == 'u')
                    {
                        builder.Append(new char[] { c, jsonString[i + 1], jsonString[i + 2], jsonString[i + 3] });
                        i += 4;
                    }
                    else
                    {
                        builder.Append(c);
                    }
                }
                else if (((((c == '\'') || (c == '"')) && (!flag && (stack.Count > 0))) && (stack.Peek() == c)) && (stack2.Count == 0))
                {
                    builder.Append(c);
                    list.Add(new JsonProperty(builder.ToString()));
                    stack.Pop();
                }
                else if (((c == '[') || (c == '{')) && (stack.Count == 0))
                {
                    if (stack2.Count == 0)
                    {
                        builder.Length = 0;
                    }
                    builder.Append(c);
                    stack2.Push((c == '[') ? ']' : '}');
                }
                else if ((((c == ']') || (c == '}')) && ((stack.Count == 0) && (stack2.Count > 0))) && (stack2.Peek() == c))
                {
                    builder.Append(c);
                    stack2.Pop();
                    if (stack2.Count == 0)
                    {
                        list.Add(new JsonProperty(builder.ToString()));
                        builder.Length = 0;
                    }
                }
                else if (((c == ',') && (stack.Count == 0)) && (stack2.Count == 0))
                {
                    if (builder.Length > 0)
                    {
                        list.Add(new JsonProperty(builder.ToString()));
                        builder.Length = 0;
                    }
                }
                else
                {
                    builder.Append(c);
                }
            }
            if ((stack.Count > 0) || (stack2.Count > 0))
            {
                list.Clear();
                throw new ArgumentException("无法解析Json Array对象!");
            }
            if (builder.ToString().Trim().Length > 0)
            {
                list.Add(new JsonProperty(builder.ToString()));
            }
            return list;
        }

        private JsonObject ParseObject(ref string jsonString)
        {
            return new JsonObject(jsonString);
        }

        private string ParseString(ref string jsonString)
        {
            int length = jsonString.Length;
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            for (int i = 1; i <= (length - 2); i++)
            {
                char ch = jsonString[i];
                if (!((ch != '\\') || flag))
                {
                    flag = true;
                }
                else
                {
                    if (!flag)
                    {
                        goto Label_0166;
                    }
                    flag = false;
                    if ((((ch == '\\') || (ch == '"')) || (ch == '\'')) || (ch == '/'))
                    {
                        builder.Append(ch);
                    }
                    else if (ch == 'u')
                    {
                        string str = new string(new char[] { ch, jsonString[i + 1], jsonString[i + 2], jsonString[i + 3] });
                        builder.Append((char) Convert.ToInt32(str, 0x10));
                        i += 4;
                    }
                    else
                    {
                        switch (ch)
                        {
                            case 'n':
                                builder.Append('\n');
                                break;

                            case 'r':
                                builder.Append('\r');
                                break;

                            case 't':
                                builder.Append('\t');
                                break;

                            case 'f':
                                builder.Append('\f');
                                break;

                            case 'b':
                                goto Label_0151;
                        }
                    }
                }
                continue;
            Label_0151:
                builder.Append('\b');
                continue;
            Label_0166:
                builder.Append(ch);
            }
            return builder.ToString();
        }

        public virtual void SetValue(object value)
        {
            if (value is string)
            {
                this._type = JsonPropertyType.String;
                this._value = (string) value;
            }
            else if (value is List<JsonProperty>)
            {
                this._list = (List<JsonProperty>) value;
                this._type = JsonPropertyType.Array;
            }
            else if (value is JsonObject)
            {
                this._object = (JsonObject) value;
                this._type = JsonPropertyType.Object;
            }
            else if (value is bool)
            {
                this._bool = (bool) value;
                this._type = JsonPropertyType.Bool;
            }
            else if (value == null)
            {
                this._type = JsonPropertyType.Null;
            }
            else
            {
                double result = 0.0;
                if (!double.TryParse(value.ToString(), out result))
                {
                    throw new ArgumentException("错误的参数类型!");
                }
                this._number = result;
                this._type = JsonPropertyType.Number;
            }
        }

        public override string ToString()
        {
            return this.ToString("");
        }

        public virtual string ToString(string format)
        {
            StringBuilder builder = new StringBuilder();
            if (this._type == JsonPropertyType.String)
            {
                builder.Append("\"").Append(this._value.Replace(@"\", @"\\").Replace("\"", "\\\"").Replace("\n", @"\n").Replace("\r", @"\r")).Append("\"");
                return builder.ToString();
            }
            if (this._type == JsonPropertyType.Bool)
            {
                return (this._bool ? "true" : "false");
            }
            if (this._type == JsonPropertyType.Number)
            {
                return this._number.ToString();
            }
            if (this._type == JsonPropertyType.Null)
            {
                return "null";
            }
            if (this._type == JsonPropertyType.Object)
            {
                return this._object.ToString();
            }
            if ((this._list == null) || (this._list.Count == 0))
            {
                builder.Append("[]");
            }
            else
            {
                builder.Append("[");
                if (this._list.Count > 0)
                {
                    foreach (JsonProperty property in this._list)
                    {
                        builder.Append(property.ToString());
                        builder.Append(", ");
                    }
                    builder.Length -= 2;
                }
                builder.Append("]");
            }
            return builder.ToString();
        }

        public virtual int Count
        {
            get
            {
                int count = 0;
                if (this._type == JsonPropertyType.Array)
                {
                    if (this._list != null)
                    {
                        count = this._list.Count;
                    }
                    return count;
                }
                return 1;
            }
        }

        public JsonProperty this[int index]
        {
            get
            {
                JsonProperty property = null;
                if (this._type == JsonPropertyType.Array)
                {
                    if ((this._list != null) && ((this._list.Count - 1) >= index))
                    {
                        property = this._list[index];
                    }
                    return property;
                }
                if (index == 0)
                {
                    return this;
                }
                return property;
            }
        }

        public JsonProperty this[string PropertyName]
        {
            get
            {
                if (this._type == JsonPropertyType.Object)
                {
                    return this._object[PropertyName];
                }
                return null;
            }
            set
            {
                if (this._type != JsonPropertyType.Object)
                {
                    throw new NotSupportedException("Json属性不是对象类型!");
                }
                this._object[PropertyName] = value;
            }
        }

        public List<JsonProperty> Items
        {
            get
            {
                if (this._type == JsonPropertyType.Array)
                {
                    return this._list;
                }
                return new List<JsonProperty> { this };
            }
        }

        public double Number
        {
            get
            {
                if (this._type == JsonPropertyType.Number)
                {
                    return this._number;
                }
                return double.NaN;
            }
        }

        public JsonObject Object
        {
            get
            {
                if (this._type == JsonPropertyType.Object)
                {
                    return this._object;
                }
                return null;
            }
        }

        public JsonPropertyType Type
        {
            get
            {
                return this._type;
            }
        }

        public string Value
        {
            get
            {
                if (this._type == JsonPropertyType.String)
                {
                    return this._value;
                }
                if (this._type == JsonPropertyType.Number)
                {
                    return this._number.ToString();
                }
                if (this._type == JsonPropertyType.Bool)
                {
                    return this._bool.ToString();
                }
                return null;
            }
        }
    }
}

