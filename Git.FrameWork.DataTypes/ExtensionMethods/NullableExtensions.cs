namespace Git.Framework.DataTypes.ExtensionMethods
{
    using System;
    using System.Runtime.CompilerServices;

    public static class NullableExtensions
    {
        public static string To(this DateTime? Date)
        {
            if (!Date.HasValue)
            {
                return string.Empty;
            }
            if ((Date.Value == DateTime.MinValue) || (Date.Value.ToString("yyyy-MM-dd") == "1970-01-01"))
            {
                return string.Empty;
            }
            return Date.Value.ToString();
        }

        public static string To(this DateTime? Date, string Format)
        {
            if (!Date.HasValue)
            {
                return string.Empty;
            }
            if ((Date.Value == DateTime.MinValue) || (Date.Value.ToString("yyyy-MM-dd") == "1970-01-01"))
            {
                return string.Empty;
            }
            return Date.Value.ToString();
        }

        public static string To(this double? Value, string Format)
        {
            if (!Value.HasValue)
            {
                return string.Empty;
            }
            return Value.Value.ToString(Format);
        }

        public static string To(this int? Value, string Format)
        {
            if (!Value.HasValue)
            {
                return string.Empty;
            }
            return Value.Value.ToString(Format);
        }

        public static string To(this float? Value, string Format)
        {
            if (!Value.HasValue)
            {
                return string.Empty;
            }
            return Value.Value.ToString(Format);
        }
    }
}

