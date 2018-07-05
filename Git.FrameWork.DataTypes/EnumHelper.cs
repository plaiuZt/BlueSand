namespace Git.Framework.DataTypes
{
    using Git.Framework.Cache;
    using Git.Framework.DataTypes.ExtensionMethods;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    public class EnumHelper
    {
        private static ICache<Type, List<ReadEnum>> cache = new Git.Framework.Cache.Cache<Type, List<ReadEnum>>();

        public static string GetEnumDesc<T>(short value)
        {
            List<ReadEnum> enumList = GetEnumList<T>();
            if (enumList.IsNullOrEmpty<ReadEnum>())
            {
                return string.Empty;
            }
            ReadEnum enum2 = Enumerable.SingleOrDefault<ReadEnum>(enumList, (Func<ReadEnum, bool>) (item => (item.Value.ToString() == value.ToString())));
            return (enum2.IsNull() ? string.Empty : enum2.Description);
        }

        public static string GetEnumDesc<T>(int value)
        {
            List<ReadEnum> enumList = GetEnumList<T>();
            if (enumList.IsNullOrEmpty<ReadEnum>())
            {
                return string.Empty;
            }
            ReadEnum enum2 = Enumerable.SingleOrDefault<ReadEnum>(enumList, (Func<ReadEnum, bool>) (item => (item.Value.ToString() == value.ToString())));
            return (enum2.IsNull() ? string.Empty : enum2.Description);
        }

        public static string GetEnumDesc<T>(object value)
        {
            List<ReadEnum> enumList = GetEnumList<T>();
            if (enumList.IsNullOrEmpty<ReadEnum>())
            {
                return string.Empty;
            }
            ReadEnum enum2 = Enumerable.SingleOrDefault<ReadEnum>(enumList, (Func<ReadEnum, bool>) (item => (item.Value.ToString() == value.ToString())));
            return (enum2.IsNull() ? string.Empty : enum2.Description);
        }

        public static string GetEnumDesc<T>(string name)
        {
            List<ReadEnum> enumList = GetEnumList<T>();
            if (enumList.IsNullOrEmpty<ReadEnum>())
            {
                return string.Empty;
            }
            ReadEnum enum2 = Enumerable.SingleOrDefault<ReadEnum>(enumList, (Func<ReadEnum, bool>) (item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase)));
            return (enum2.IsNull() ? string.Empty : enum2.Description);
        }

        public static string GetEnumDesc<T>(T value)
        {
            return GetEnumDesc<T>(value.ToString());
        }

        public static List<ReadEnum> GetEnumList<T>()
        {
            Type type = typeof(T);
            return GetEnumList(type);
        }

        public static List<ReadEnum> GetEnumList(Type type)
        {
            List<ReadEnum> list = cache.Get(type);
            if (list.IsNullOrEmpty<ReadEnum>())
            {
                list = new List<ReadEnum>();
                ReadEnum item = null;
                foreach (int num in Enum.GetValues(type))
                {
                    item = new ReadEnum {
                        Value = num.ToString()
                    };
                    string name = Enum.GetName(type, num);
                    item.Name = name;
                    object[] customAttributes = type.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (customAttributes.Length > 0)
                    {
                        DescriptionAttribute attribute = customAttributes[0] as DescriptionAttribute;
                        if (attribute > null)
                        {
                            item.Description = attribute.Description;
                        }
                    }
                    list.Add(item);
                }
                if (!list.IsNullOrEmpty<ReadEnum>())
                {
                    cache.Insert(type, list);
                }
            }
            return list;
        }

        public static T GetModel<T>(short value)
        {
            return (T) Enum.Parse(typeof(T), value.ToString(), true);
        }

        public static T GetModel<T>(int value)
        {
            return (T) Enum.Parse(typeof(T), value.ToString(), true);
        }

        public static T GetModel<T>(object value)
        {
            return (T) Enum.Parse(typeof(T), value.ToString(), true);
        }

        public static string GetOptions<T>(short value)
        {
            StringBuilder builder = new StringBuilder();
            string format = "<option value=\"{0}\" {1}>{2}</option>";
            builder.AppendFormat(format, string.Empty, string.Empty, "请选择");
            List<ReadEnum> enumList = GetEnumList<T>();
            if (!enumList.IsNullOrEmpty<ReadEnum>())
            {
                foreach (ReadEnum enum2 in enumList)
                {
                    builder.AppendFormat(format, enum2.Value, string.Equals(value.ToString(), enum2.Value, StringComparison.OrdinalIgnoreCase) ? "selected=\"selected\"" : string.Empty, enum2.Description);
                }
            }
            return builder.ToString();
        }

        public static string GetOptions<T>(int value)
        {
            StringBuilder builder = new StringBuilder();
            string format = "<option value=\"{0}\" {1}>{2}</option>";
            builder.AppendFormat(format, string.Empty, string.Empty, "请选择");
            List<ReadEnum> enumList = GetEnumList<T>();
            if (!enumList.IsNullOrEmpty<ReadEnum>())
            {
                foreach (ReadEnum enum2 in enumList)
                {
                    builder.AppendFormat(format, enum2.Value, string.Equals(value.ToString(), enum2.Value, StringComparison.OrdinalIgnoreCase) ? "selected=\"selected\"" : string.Empty, enum2.Description);
                }
            }
            return builder.ToString();
        }

        public static string GetOptions<T>(object value)
        {
            StringBuilder builder = new StringBuilder();
            string format = "<option value=\"{0}\" {1}>{2}</option>";
            builder.AppendFormat(format, string.Empty, string.Empty, "请选择");
            List<ReadEnum> enumList = GetEnumList<T>();
            if (!enumList.IsNullOrEmpty<ReadEnum>())
            {
                foreach (ReadEnum enum2 in enumList)
                {
                    builder.AppendFormat(format, enum2.Value, string.Equals(value.ToString(), enum2.Value, StringComparison.OrdinalIgnoreCase) ? "selected=\"selected\"" : string.Empty, enum2.Description);
                }
            }
            return builder.ToString();
        }

        public static string GetOptions<T>(T value)
        {
            StringBuilder builder = new StringBuilder();
            string format = "<option value=\"{0}\" {1}>{2}</option>";
            builder.AppendFormat(format, string.Empty, string.Empty, "请选择");
            List<ReadEnum> enumList = GetEnumList<T>();
            if (!enumList.IsNullOrEmpty<ReadEnum>())
            {
                foreach (ReadEnum enum2 in enumList)
                {
                    builder.AppendFormat(format, enum2.Value, string.Equals(GetValue<T>(value.ToString()), enum2.Value, StringComparison.OrdinalIgnoreCase) ? "selected=\"selected\"" : string.Empty, enum2.Description);
                }
            }
            return builder.ToString();
        }

        public static string GetOptions<T>(short value, string defaultValue)
        {
            defaultValue = defaultValue.IsEmpty() ? "请选择" : defaultValue;
            StringBuilder builder = new StringBuilder();
            string format = "<option value=\"{0}\" {1}>{2}</option>";
            builder.AppendFormat(format, string.Empty, string.Empty, defaultValue);
            List<ReadEnum> enumList = GetEnumList<T>();
            if (!enumList.IsNullOrEmpty<ReadEnum>())
            {
                foreach (ReadEnum enum2 in enumList)
                {
                    builder.AppendFormat(format, enum2.Value, string.Equals(value.ToString(), enum2.Value, StringComparison.OrdinalIgnoreCase) ? "selected=\"selected\"" : string.Empty, enum2.Description);
                }
            }
            return builder.ToString();
        }

        public static string GetOptions<T>(int value, string defaultValue)
        {
            defaultValue = defaultValue.IsEmpty() ? "请选择" : defaultValue;
            StringBuilder builder = new StringBuilder();
            string format = "<option value=\"{0}\" {1}>{2}</option>";
            builder.AppendFormat(format, string.Empty, string.Empty, defaultValue);
            List<ReadEnum> enumList = GetEnumList<T>();
            if (!enumList.IsNullOrEmpty<ReadEnum>())
            {
                foreach (ReadEnum enum2 in enumList)
                {
                    builder.AppendFormat(format, enum2.Value, string.Equals(value.ToString(), enum2.Value, StringComparison.OrdinalIgnoreCase) ? "selected=\"selected\"" : string.Empty, enum2.Description);
                }
            }
            return builder.ToString();
        }

        public static string GetOptions<T>(object value, string defaultValue)
        {
            defaultValue = defaultValue.IsEmpty() ? "请选择" : defaultValue;
            StringBuilder builder = new StringBuilder();
            string format = "<option value=\"{0}\" {1}>{2}</option>";
            builder.AppendFormat(format, string.Empty, string.Empty, defaultValue);
            List<ReadEnum> enumList = GetEnumList<T>();
            if (!enumList.IsNullOrEmpty<ReadEnum>())
            {
                foreach (ReadEnum enum2 in enumList)
                {
                    builder.AppendFormat(format, enum2.Value, string.Equals(value.ToString(), enum2.Value, StringComparison.OrdinalIgnoreCase) ? "selected=\"selected\"" : string.Empty, enum2.Description);
                }
            }
            return builder.ToString();
        }

        public static string GetOptions<T>(T value, string defaultValue)
        {
            StringBuilder builder = new StringBuilder();
            string format = "<option value=\"{0}\" {1}>{2}</option>";
            builder.AppendFormat(format, string.Empty, string.Empty, defaultValue);
            List<ReadEnum> enumList = GetEnumList<T>();
            if (!enumList.IsNullOrEmpty<ReadEnum>())
            {
                foreach (ReadEnum enum2 in enumList)
                {
                    builder.AppendFormat(format, enum2.Value, string.Equals(GetValue<T>(value.ToString()), enum2.Value, StringComparison.OrdinalIgnoreCase) ? "selected=\"selected\"" : string.Empty, enum2.Description);
                }
            }
            return builder.ToString();
        }

        public static string GetValue<T>(string name)
        {
            List<ReadEnum> enumList = GetEnumList<T>();
            if (enumList.IsNullOrEmpty<ReadEnum>())
            {
                return string.Empty;
            }
            return Enumerable.SingleOrDefault<ReadEnum>(enumList, (Func<ReadEnum, bool>) (item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase))).Value;
        }
    }
}

