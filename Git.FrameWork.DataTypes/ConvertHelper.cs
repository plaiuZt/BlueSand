namespace Git.Framework.DataTypes
{
    using Microsoft.JScript;
    using System;
    using System.Globalization;

    public class ConvertHelper
    {
        public static readonly GList<string> TrueStringList;

        static ConvertHelper()
        {
            GList<string> list1 = new GList<string>();
            list1.Add("TRUE");
            list1.Add("YES");
            list1.Add("ON");
            list1.Add("1");
            TrueStringList = list1;
        }

        public static string Escape(string value)
        {
            return GlobalObject.escape(value);
        }

        public static string NewGuid()
        {
            return Guid.NewGuid().ToString("N").ToUpper();
        }

        private static bool ToBoolean(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            return TrueStringList.Contains(value.Trim().ToUpper());
        }

        public static short ToShort(string value)
        {
            short num = 0;
            try
            {
                num = short.Parse(value);
            }
            catch
            {
            }
            return num;
        }

        public static short ToShort(string value, short defaultValue)
        {
            short num = defaultValue;
            try
            {
                num = short.Parse(value);
            }
            catch
            {
            }
            return num;
        }

        public static T ToType<T>(string value)
        {
            object obj2 = default(T);
            if (string.IsNullOrEmpty(value))
            {
                return (T) obj2;
            }
            return (T) ToType(value, typeof(T));
        }

        private static object ToType(string value, Type conversionType)
        {
            if (conversionType == typeof(string))
            {
                return value;
            }
            if (conversionType == typeof(int))
            {
                return ((value == null) ? 0 : int.Parse(value, NumberStyles.Any));
            }
            if (conversionType == typeof(bool))
            {
                return ToBoolean(value);
            }
            if (conversionType == typeof(float))
            {
                return ((value == null) ? 0f : float.Parse(value, NumberStyles.Any));
            }
            if (conversionType == typeof(double))
            {
                return ((value == null) ? 0.0 : double.Parse(value, NumberStyles.Any));
            }
            if (conversionType == typeof(decimal))
            {
                return ((value == null) ? decimal.Zero : decimal.Parse(value, NumberStyles.Any));
            }
            if (conversionType == typeof(DateTime))
            {
                return ((value == null) ? DateTimeUtil.MinValue : DateTime.Parse(value, CultureInfo.CurrentCulture, DateTimeStyles.None));
            }
            if (conversionType == typeof(char))
            {
                return System.Convert.ToChar(value);
            }
            if (conversionType == typeof(sbyte))
            {
                return sbyte.Parse(value, NumberStyles.Any);
            }
            if (conversionType == typeof(byte))
            {
                return byte.Parse(value, NumberStyles.Any);
            }
            if (conversionType == typeof(short))
            {
                return ((value == null) ? 0 : ((int) short.Parse(value)));
            }
            if (conversionType == typeof(ushort))
            {
                return ((value == null) ? 0 : ((int) ushort.Parse(value, NumberStyles.Any)));
            }
            if (conversionType == typeof(uint))
            {
                return ((value == null) ? ((object) 0) : ((object) uint.Parse(value, NumberStyles.Any)));
            }
            if (conversionType == typeof(long))
            {
                return ((value == null) ? 0L : long.Parse(value, NumberStyles.Any));
            }
            if (conversionType == typeof(ulong))
            {
                return ((value == null) ? ((object) 0L) : ((object) ulong.Parse(value, NumberStyles.Any)));
            }
            if (conversionType == typeof(Guid))
            {
                return ((value == null) ? Guid.Empty : new Guid(value));
            }
            return null;
        }

        public static T ToType<T>(string value, T defaultValue)
        {
            T local = defaultValue;
            if (string.IsNullOrEmpty(value))
            {
                return local;
            }
            try
            {
                return ToType<T>(value);
            }
            catch (Exception)
            {
                return local;
            }
        }

        public static string UnEscapge(string value)
        {
            return GlobalObject.unescape(value);
        }
    }
}

