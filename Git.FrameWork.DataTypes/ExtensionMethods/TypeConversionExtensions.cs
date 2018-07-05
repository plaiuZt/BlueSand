namespace Git.Framework.DataTypes.ExtensionMethods
{
    using Git.Framework.DataTypes.Comparison;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class TypeConversionExtensions
    {
        private static object CallMethod(string MethodName, object Object, params object[] InputVariables)
        {
            if (string.IsNullOrEmpty(MethodName) || Object.IsNull())
            {
                return null;
            }
            Type type = Object.GetType();
            MethodInfo method = null;
            if (InputVariables.IsNotNull())
            {
                Type[] types = new Type[InputVariables.Length];
                for (int i = 0; i < InputVariables.Length; i++)
                {
                    types[i] = InputVariables[i].GetType();
                }
                method = type.GetMethod(MethodName, types);
                if (method > null)
                {
                    return method.Invoke(Object, InputVariables);
                }
            }
            method = type.GetMethod(MethodName);
            return (method.IsNull() ? null : method.Invoke(Object, null));
        }

        public static string FormatToString(this object Input, string Format)
        {
            if (Input.IsNull())
            {
                return "";
            }
            return (!string.IsNullOrEmpty(Format) ? ((string) CallMethod("ToString", Input, new object[] { Format })) : Input.ToString());
        }

        public static bool IsDefault<T>(this T Object, IEqualityComparer<T> EqualityComparer = null)
        {
            return EqualityComparer.NullCheck<IEqualityComparer<T>>(new Git.Framework.DataTypes.Comparison.GenericEqualityComparer<T>()).Equals(Object, default(T));
        }

        public static bool IsNotDefault<T>(this T Object, IEqualityComparer<T> EqualityComparer = null)
        {
            return !EqualityComparer.NullCheck<IEqualityComparer<T>>(new Git.Framework.DataTypes.Comparison.GenericEqualityComparer<T>()).Equals(Object, default(T));
        }

        public static bool IsNotNull(this object Object)
        {
            return (Object > null);
        }

        public static bool IsNotNullOrDBNull(this object Object)
        {
            return ((Object != null) && !Convert.IsDBNull(Object));
        }

        public static bool IsNull(this object Object)
        {
            return (Object == null);
        }

        public static bool IsNullOrDBNull(this object Object)
        {
            return ((Object == null) || Convert.IsDBNull(Object));
        }

        public static T NullCheck<T>(this T Object, T DefaultValue = null)
        {
            return ((Object == null) ? DefaultValue : Object);
        }

        public static void ThrowIfDefault<T>(this T Item, string Name, IEqualityComparer<T> EqualityComparer = null)
        {
            if (Item.IsDefault<T>(EqualityComparer))
            {
                throw new ArgumentNullException(Name);
            }
        }

        public static void ThrowIfNull(this object Item, string Name)
        {
            if (Item.IsNull())
            {
                throw new ArgumentNullException(Name);
            }
        }

        public static void ThrowIfNullOrDBNull(this object Item, string Name)
        {
            if (Item.IsNullOrDBNull())
            {
                throw new ArgumentNullException(Name);
            }
        }

        public static void ThrowIfNullOrEmpty<T>(this IEnumerable<T> Item, string Name)
        {
            if (Item.IsNullOrEmpty<T>())
            {
                throw new ArgumentNullException(Name);
            }
        }

        public static DbType ToDbType(this SqlDbType Type)
        {
            SqlParameter parameter = new SqlParameter {
                SqlDbType = Type
            };
            return parameter.DbType;
        }

        public static DbType ToDbType(this Type Type)
        {
            if (Type == typeof(byte))
            {
                return DbType.Byte;
            }
            if (Type == typeof(sbyte))
            {
                return DbType.SByte;
            }
            if (Type == typeof(short))
            {
                return DbType.Int16;
            }
            if (Type == typeof(ushort))
            {
                return DbType.UInt16;
            }
            if (Type != typeof(int))
            {
                if (Type == typeof(uint))
                {
                    return DbType.UInt32;
                }
                if (Type == typeof(long))
                {
                    return DbType.Int64;
                }
                if (Type == typeof(ulong))
                {
                    return DbType.UInt64;
                }
                if (Type == typeof(float))
                {
                    return DbType.Single;
                }
                if (Type == typeof(double))
                {
                    return DbType.Double;
                }
                if (Type == typeof(decimal))
                {
                    return DbType.Decimal;
                }
                if (Type == typeof(bool))
                {
                    return DbType.Boolean;
                }
                if (Type == typeof(string))
                {
                    return DbType.String;
                }
                if (Type == typeof(char))
                {
                    return DbType.StringFixedLength;
                }
                if (Type == typeof(Guid))
                {
                    return DbType.Guid;
                }
                if (Type == typeof(DateTime))
                {
                    return DbType.DateTime2;
                }
                if (Type == typeof(DateTimeOffset))
                {
                    return DbType.DateTimeOffset;
                }
                if (Type == typeof(byte[]))
                {
                    return DbType.Binary;
                }
                if (Type == typeof(byte?))
                {
                    return DbType.Byte;
                }
                if (Type == typeof(sbyte?))
                {
                    return DbType.SByte;
                }
                if (Type == typeof(short?))
                {
                    return DbType.Int16;
                }
                if (Type == typeof(ushort?))
                {
                    return DbType.UInt16;
                }
                if (Type == typeof(int?))
                {
                    return DbType.Int32;
                }
                if (Type == typeof(uint?))
                {
                    return DbType.UInt32;
                }
                if (Type == typeof(long?))
                {
                    return DbType.Int64;
                }
                if (Type == typeof(ulong?))
                {
                    return DbType.UInt64;
                }
                if (Type == typeof(float?))
                {
                    return DbType.Single;
                }
                if (Type == typeof(double?))
                {
                    return DbType.Double;
                }
                if (Type == typeof(decimal?))
                {
                    return DbType.Decimal;
                }
                if (Type == typeof(bool?))
                {
                    return DbType.Boolean;
                }
                if (Type == typeof(char?))
                {
                    return DbType.StringFixedLength;
                }
                if (Type == typeof(Guid?))
                {
                    return DbType.Guid;
                }
                if (Type == typeof(DateTime?))
                {
                    return DbType.DateTime2;
                }
                if (Type == typeof(DateTimeOffset?))
                {
                    return DbType.DateTimeOffset;
                }
            }
            return DbType.Int32;
        }

        public static SqlDbType ToSqlDbType(this DbType Type)
        {
            SqlParameter parameter = new SqlParameter {
                DbType = Type
            };
            return parameter.SqlDbType;
        }

        public static SqlDbType ToSQLDbType(this Type Type)
        {
            return Type.ToDbType().ToSqlDbType();
        }

        public static Type ToType(this DbType Type)
        {
            if (Type == DbType.Byte)
            {
                return typeof(byte);
            }
            if (Type == DbType.SByte)
            {
                return typeof(sbyte);
            }
            if (Type == DbType.Int16)
            {
                return typeof(short);
            }
            if (Type == DbType.UInt16)
            {
                return typeof(ushort);
            }
            if (Type != DbType.Int32)
            {
                if (Type == DbType.UInt32)
                {
                    return typeof(uint);
                }
                if (Type == DbType.Int64)
                {
                    return typeof(long);
                }
                if (Type == DbType.UInt64)
                {
                    return typeof(ulong);
                }
                if (Type == DbType.Single)
                {
                    return typeof(float);
                }
                if (Type == DbType.Double)
                {
                    return typeof(double);
                }
                if (Type == DbType.Decimal)
                {
                    return typeof(decimal);
                }
                if (Type == DbType.Boolean)
                {
                    return typeof(bool);
                }
                if (Type == DbType.String)
                {
                    return typeof(string);
                }
                if (Type == DbType.StringFixedLength)
                {
                    return typeof(char);
                }
                if (Type == DbType.Guid)
                {
                    return typeof(Guid);
                }
                if (Type == DbType.DateTime2)
                {
                    return typeof(DateTime);
                }
                if (Type == DbType.DateTime)
                {
                    return typeof(DateTime);
                }
                if (Type == DbType.DateTimeOffset)
                {
                    return typeof(DateTimeOffset);
                }
                if (Type == DbType.Binary)
                {
                    return typeof(byte[]);
                }
            }
            return typeof(int);
        }

        public static Type ToType(this SqlDbType Type)
        {
            return Type.ToDbType().ToType();
        }

        public static R TryTo<T, R>(this T Object, R DefaultValue = null)
        {
            try
            {
                if (Object.IsNullOrDBNull())
                {
                    return DefaultValue;
                }
                if ((Object as string).IsNotNull())
                {
                    string str = Object as string;
                    if (typeof(R).IsEnum)
                    {
                        return (R) Enum.Parse(typeof(R), str, true);
                    }
                    if (str.IsNullOrEmpty<char>())
                    {
                        return DefaultValue;
                    }
                }
                if ((Object as IConvertible).IsNotNull())
                {
                    return (R) Convert.ChangeType(Object, typeof(R));
                }
                if (typeof(R).IsAssignableFrom(Object.GetType()))
                {
                    return (R) Object;
                }
                TypeConverter converter = TypeDescriptor.GetConverter(Object.GetType());
                if (converter.CanConvertTo(typeof(R)))
                {
                    return (R) converter.ConvertTo(Object, typeof(R));
                }
                if ((Object as string).IsNotNull())
                {
                    return Object.ToString().TryTo<string, R>(DefaultValue);
                }
            }
            catch
            {
            }
            return DefaultValue;
        }
    }
}

