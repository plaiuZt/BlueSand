namespace Git.Framework.DataTypes.ExtensionMethods
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class ValueTypeExtensions
    {
        public static bool IsUnicode(this byte[] Input)
        {
            return (Input.IsNull() || Input.ToEncodedString(new UnicodeEncoding(), 0, -1).IsUnicode());
        }

        public static string ToBase64String(this byte[] Input)
        {
            return (Input.IsNull() ? "" : Convert.ToBase64String(Input));
        }

        public static bool ToBool(this int Input)
        {
            return (Input > 0);
        }

        public static string ToEncodedString(this byte[] Input, Encoding EncodingUsing = null, int Index = 0, int Count = -1)
        {
            if (Input.IsNull())
            {
                return "";
            }
            if (Count == -1)
            {
                Count = Input.Length - Index;
            }
            return EncodingUsing.NullCheck<Encoding>(new UTF8Encoding()).GetString(Input, Index, Count);
        }

        public static int ToInt(this bool Value)
        {
            return (Value ? 1 : 0);
        }

        public static string ToString(this byte[] Input, Base64FormattingOptions Options, int Index = 0, int Count = -1)
        {
            if (Count == -1)
            {
                Count = Input.Length - Index;
            }
            return ((Input == null) ? "" : Convert.ToBase64String(Input, Index, Count, Options));
        }

        public static string ToString(this byte[] Input, Encoding EncodingUsing, int Index = 0, int Count = -1)
        {
            if (Input == null)
            {
                return "";
            }
            if (Count == -1)
            {
                Count = Input.Length - Index;
            }
            return EncodingUsing.Check<Encoding>(new UTF8Encoding()).GetString(Input, Index, Count);
        }
    }
}

