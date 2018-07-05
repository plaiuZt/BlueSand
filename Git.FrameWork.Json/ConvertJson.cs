using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;

namespace Git.Framework.Json
{
    public class ConvertJson
    {
        public static T Deserialize<T>(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return (T) serializer.ReadObject(stream);
        }

        public static T GetObjectJson<T>(string json)
        {
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            return (T) serializer.ReadObject(stream);
        }

        public static object GetObjectJson(string json, Type type)
        {
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
            return serializer.ReadObject(stream);
        }

        public static string ListToJson<T>(T t)
        {
            return new JavaScriptSerializer().Serialize(t);
        }

        public static string ListToJson<T>(List<T> list)
        {
            return ListToJson<List<T>>(list);
        }

        public static string ListToJson<T>(T t, int totalCount)
        {
            JsonShowModel<T> model = new JsonShowModel<T>(totalCount, t);
            return new JavaScriptSerializer().Serialize(model);
        }

        public static string ListToJson<T>(List<T> list, string jsonName)
        {
            if (string.IsNullOrEmpty(jsonName))
            {
                jsonName = "d";
            }
            return ("{\"" + jsonName + "\":" + ListToJson<T>(list) + "}");
        }

        public static string Serializer<T>(T t)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, t);
            string str = Encoding.UTF8.GetString(stream.ToArray());
            stream.Close();
            return str;
        }

        public static string SerializerForList<T>(List<T> list)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            foreach (T local in list)
            {
                builder.AppendFormat("{0},", Serializer<T>(local));
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append("]");
            return builder.ToString();
        }

        public static string SerializerForList1<T>(List<T> list)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            foreach (T local in list)
            {
                builder.AppendFormat("{0},", Serializer<T>(local));
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append("]");
            return builder.ToString();
        }

        private static string String2Json(string s)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s.ToCharArray()[i];
                switch (ch)
                {
                    case '/':
                    {
                        builder.Append(@"\/");
                        continue;
                    }
                    case '\\':
                    {
                        builder.Append(@"\\");
                        continue;
                    }
                    case '\b':
                    {
                        builder.Append(@"\b");
                        continue;
                    }
                    case '\t':
                    {
                        builder.Append(@"\t");
                        continue;
                    }
                    case '\n':
                    {
                        builder.Append(@"\n");
                        continue;
                    }
                    case '\f':
                    {
                        builder.Append(@"\f");
                        continue;
                    }
                    case '\r':
                    {
                        builder.Append(@"\r");
                        continue;
                    }
                    case '"':
                    {
                        builder.Append("\\\"");
                        continue;
                    }
                }
                builder.Append(ch);
            }
            return builder.ToString();
        }

        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
                return str;
            }
            if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
                return str;
            }
            if (type == typeof(bool))
            {
                str = str.ToLower();
                return str;
            }
            if ((type != typeof(string)) && string.IsNullOrEmpty(str))
            {
                str = "\"" + str + "\"";
            }
            return str;
        }

        public static string ToJson(DbDataReader dataReader)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            while (dataReader.Read())
            {
                builder.Append("{");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Type fieldType = dataReader.GetFieldType(i);
                    string name = dataReader.GetName(i);
                    string str = dataReader[i].ToString();
                    builder.Append("\"" + name + "\":");
                    str = StringFormat(str, fieldType);
                    if (i < (dataReader.FieldCount - 1))
                    {
                        builder.Append(str + ",");
                    }
                    else
                    {
                        builder.Append(str);
                    }
                }
                builder.Append("},");
            }
            dataReader.Close();
            builder.Remove(builder.Length - 1, 1);
            builder.Append("]");
            return builder.ToString();
        }

        public static string ToJson(DataTable dt)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            DataRowCollection rows = dt.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                builder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string columnName = dt.Columns[j].ColumnName;
                    string str = rows[i][j].ToString();
                    Type dataType = dt.Columns[j].DataType;
                    builder.Append("\"" + columnName + "\":");
                    str = StringFormat(str, dataType);
                    if (j < (dt.Columns.Count - 1))
                    {
                        builder.Append(str + ",");
                    }
                    else
                    {
                        builder.Append(str);
                    }
                }
                builder.Append("},");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append("]");
            return builder.ToString();
        }

        public static string ToJson(object source)
        {
            return ToJson(source, source.GetType());
        }

        public static string ToJson(DataTable dt, string jsonName)
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
            {
                jsonName = dt.TableName;
            }
            builder.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    builder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        builder.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (j < (dt.Columns.Count - 1))
                        {
                            builder.Append(",");
                        }
                    }
                    builder.Append("}");
                    if (i < (dt.Rows.Count - 1))
                    {
                        builder.Append(",");
                    }
                }
            }
            builder.Append("]}");
            return builder.ToString();
        }

        public static string ToJson(object source, Type type)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
            using (Stream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, source);
                stream.Flush();
                stream.Position = 0L;
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }
    }
}

