namespace Git.Framework.DataTypes.ExtensionMethods
{
    using Git.Framework.DataTypes;
    using Git.Framework.DataTypes.Formatters;
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string AlphaCharactersOnly(this string Input)
        {
            return Input.KeepFilterText("[a-zA-Z]");
        }

        public static string AlphaNumericOnly(this string Input)
        {
            return Input.KeepFilterText("[a-zA-Z0-9]");
        }

        public static string Encode(this string Input, Encoding OriginalEncodingUsing = null, Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Input))
            {
                return "";
            }
            OriginalEncodingUsing = OriginalEncodingUsing.NullCheck<Encoding>(new ASCIIEncoding());
            EncodingUsing = EncodingUsing.NullCheck<Encoding>(new UTF8Encoding());
            return Encoding.Convert(OriginalEncodingUsing, EncodingUsing, Input.ToByteArray(OriginalEncodingUsing)).ToEncodedString(EncodingUsing, 0, -1);
        }

        public static string Escape(this string value)
        {
            if (!value.IsEmpty())
            {
                return ConvertHelper.Escape(value);
            }
            return string.Empty;
        }

        public static string FilterOutText(this string Input, string Filter)
        {
            if (string.IsNullOrEmpty(Input))
            {
                return "";
            }
            return (string.IsNullOrEmpty(Filter) ? Input : new Regex(Filter).Replace(Input, ""));
        }

        public static string FormatString(this string Input, string Format)
        {
            return new GenericStringFormatter().Format(Input, Format);
        }

        public static byte[] FromBase64(this string Input)
        {
            return (string.IsNullOrEmpty(Input) ? new byte[0] : Convert.FromBase64String(Input));
        }

        public static string FromBase64(this string Input, Encoding EncodingUsing)
        {
            if (string.IsNullOrEmpty(Input))
            {
                return "";
            }
            byte[] bytes = Convert.FromBase64String(Input);
            return EncodingUsing.NullCheck<Encoding>(new UTF8Encoding()).GetString(bytes);
        }

        public static bool IsEmpty(this string targetValue)
        {
            return string.IsNullOrEmpty(targetValue);
        }

        public static bool IsNotEmpty(this string targetValue)
        {
            return !targetValue.IsEmpty();
        }

        public static bool IsUnicode(this string Input)
        {
            return (string.IsNullOrEmpty(Input) || (Regex.Replace(Input, @"[^\u0000-\u007F]", "") != Input));
        }

        public static string KeepFilterText(this string Input, string Filter)
        {
            if (string.IsNullOrEmpty(Input) || string.IsNullOrEmpty(Filter))
            {
                return "";
            }
            MatchCollection matchs = new Regex(Filter).Matches(Input);
            StringBuilder builder = new StringBuilder();
            foreach (Match match in matchs)
            {
                builder.Append(match.Value);
            }
            return builder.ToString();
        }

        public static int LastIndex(this string sourceValue, string targetValue)
        {
            if (!sourceValue.IsEmpty() && !targetValue.IsEmpty())
            {
                return sourceValue.LastIndexOf(targetValue);
            }
            return -1;
        }

        public static string Left(this string Input, int Length)
        {
            return (string.IsNullOrEmpty(Input) ? "" : Input.Substring(0, (Input.Length > Length) ? Length : Input.Length));
        }

        public static string NoHtml(this string sourceValue)
        {
            sourceValue = Regex.Replace(sourceValue, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            sourceValue = new Regex("<.+?>", RegexOptions.IgnoreCase).Replace(sourceValue, "");
            sourceValue = Regex.Replace(sourceValue, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            sourceValue = Regex.Replace(sourceValue, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            sourceValue = Regex.Replace(sourceValue, "-->", "", RegexOptions.IgnoreCase);
            sourceValue = Regex.Replace(sourceValue, "<!--.*", "", RegexOptions.IgnoreCase);
            sourceValue = Regex.Replace(sourceValue, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            sourceValue = Regex.Replace(sourceValue, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
            sourceValue = Regex.Replace(sourceValue, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
            sourceValue = Regex.Replace(sourceValue, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
            sourceValue = Regex.Replace(sourceValue, "&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            sourceValue = Regex.Replace(sourceValue, "&(iexcl|#161);", "\x00a1", RegexOptions.IgnoreCase);
            sourceValue = Regex.Replace(sourceValue, "&(cent|#162);", "\x00a2", RegexOptions.IgnoreCase);
            sourceValue = Regex.Replace(sourceValue, "&(pound|#163);", "\x00a3", RegexOptions.IgnoreCase);
            sourceValue = Regex.Replace(sourceValue, "&(copy|#169);", "\x00a9", RegexOptions.IgnoreCase);
            sourceValue = Regex.Replace(sourceValue, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            sourceValue.Replace("<", "");
            sourceValue.Replace(">", "");
            sourceValue.Replace("\r\n", "");
            return sourceValue;
        }

        public static int NumberTimesOccurs(this string Input, string Match)
        {
            return (string.IsNullOrEmpty(Input) ? 0 : new Regex(Match).Matches(Input).Count);
        }

        public static string NumericOnly(this string Input, bool KeepNumericPunctuation = true)
        {
            return (KeepNumericPunctuation ? Input.KeepFilterText(@"[0-9\.]") : Input.KeepFilterText("[0-9]"));
        }

        public static string RegexFormat(this string Input, string Format, string OutputFormat, RegexOptions Options = 0)
        {
            Input.ThrowIfNullOrEmpty<char>("Input");
            return Regex.Replace(Input, Format, OutputFormat, Options);
        }

        public static string Reverse(this string Input)
        {
            return new string(Input.Reverse<char>().ToArray<char>());
        }

        public static string Right(this string Input, int Length)
        {
            if (string.IsNullOrEmpty(Input))
            {
                return "";
            }
            Length = (Input.Length > Length) ? Length : Input.Length;
            return Input.Substring(Input.Length - Length, Length);
        }

        public static string SubStr(this string sourceValue, int startIndex, int endIndex)
        {
            if (sourceValue.IsEmpty() || (startIndex >= sourceValue.Length))
            {
                return string.Empty;
            }
            return sourceValue.SubString(startIndex, ((int) (endIndex - startIndex)));
        }

        public static string SubStr(this string sourceValue, int startIndex, int endIndex, string append)
        {
            if (sourceValue.IsEmpty() || (startIndex >= sourceValue.Length))
            {
                return string.Empty;
            }
            return (sourceValue.SubString(startIndex, ((int) (endIndex - startIndex))) + append);
        }

        public static string SubString(this string sourceValue, int length)
        {
            if (sourceValue.IsEmpty())
            {
                return sourceValue;
            }
            return ((sourceValue.Length <= length) ? sourceValue : sourceValue.Substring(0, length));
        }

        public static string SubString(this string sourceValue, int startIndex, int length)
        {
            if (sourceValue.IsEmpty())
            {
                return sourceValue;
            }
            return (((startIndex + length) <= sourceValue.Length) ? sourceValue.Substring(startIndex, length) : sourceValue.Substring(startIndex));
        }

        public static string SubString(this string sourceValue, int length, string append)
        {
            if (sourceValue.IsEmpty())
            {
                return sourceValue;
            }
            return ((sourceValue.Length <= length) ? sourceValue : (sourceValue.Substring(0, length) + append));
        }

        public static string SubString(this string sourceValue, int startIndex, int length, string append)
        {
            if (sourceValue.IsEmpty())
            {
                return sourceValue;
            }
            return (((startIndex + length) <= sourceValue.Length) ? (sourceValue.Substring(startIndex, length) + append) : sourceValue.Substring(startIndex));
        }

        public static string ToBase64(this string Input, Encoding OriginalEncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Input))
            {
                return "";
            }
            return Convert.ToBase64String(OriginalEncodingUsing.NullCheck<Encoding>(new UTF8Encoding()).GetBytes(Input));
        }

        public static byte[] ToByteArray(this string Input, Encoding EncodingUsing = null)
        {
            return (string.IsNullOrEmpty(Input) ? null : EncodingUsing.NullCheck<Encoding>(new UTF8Encoding()).GetBytes(Input));
        }

        public static string ToFirstCharacterUpperCase(this string Input)
        {
            if (string.IsNullOrEmpty(Input))
            {
                return "";
            }
            char[] chArray = Input.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                if ((chArray[i] != ' ') && (chArray[i] != '\t'))
                {
                    chArray[i] = char.ToUpper(chArray[i]);
                    break;
                }
            }
            return new string(chArray);
        }

        public static string ToSentenceCapitalize(this string Input)
        {
            if (string.IsNullOrEmpty(Input))
            {
                return "";
            }
            string[] separator = new string[] { ".", "?", "!" };
            string[] strArray2 = Input.Split(separator, StringSplitOptions.None);
            for (int i = 0; i < strArray2.Length; i++)
            {
                if (!string.IsNullOrEmpty(strArray2[i]))
                {
                    Regex regex = new Regex(strArray2[i]);
                    strArray2[i] = strArray2[i].ToFirstCharacterUpperCase();
                    Input = regex.Replace(Input, strArray2[i]);
                }
            }
            return Input;
        }

        public static string ToTitleCase(this string Input)
        {
            if (string.IsNullOrEmpty(Input))
            {
                return "";
            }
            string[] separator = new string[] { " ", ".", "\t", Environment.NewLine, "!", "?" };
            string[] strArray2 = Input.Split(separator, StringSplitOptions.None);
            for (int i = 0; i < strArray2.Length; i++)
            {
                if (!string.IsNullOrEmpty(strArray2[i]) && (strArray2[i].Length > 3))
                {
                    Regex regex = new Regex(strArray2[i]);
                    strArray2[i] = strArray2[i].ToFirstCharacterUpperCase();
                    Input = regex.Replace(Input, strArray2[i]);
                }
            }
            return Input;
        }

        public static string UnEscapge(this string value)
        {
            if (!value.IsEmpty())
            {
                return ConvertHelper.UnEscapge(value);
            }
            return string.Empty;
        }

        public static NameValueCollection UrlParam(this string url)
        {
            if (!url.IsEmpty())
            {
                if (url.Contains("?"))
                {
                    url = url.Substring(url.IndexOf("?") + 1);
                }
                char[] separator = new char[] { '&' };
                string[] strArray = url.Split(separator);
                if (!strArray.IsNullOrEmpty<string>())
                {
                    NameValueCollection values2 = new NameValueCollection();
                    string[] strArray2 = null;
                    foreach (string str in strArray)
                    {
                        char[] chArray2 = new char[] { '=' };
                        strArray2 = str.Split(chArray2);
                        values2.Add(strArray2[0], strArray2[1]);
                    }
                    return values2;
                }
            }
            return null;
        }

        public static string UrlParam(this string url, string paramName)
        {
            NameValueCollection values = url.UrlParam();
            if (!values.IsNull())
            {
                return values[paramName];
            }
            return string.Empty;
        }

        public static string ZipHtml(this string sourceValue)
        {
            sourceValue = Regex.Replace(sourceValue, @">\s+?<", "><");
            sourceValue = Regex.Replace(sourceValue, @"\r\n\s*", "");
            sourceValue = Regex.Replace(sourceValue, @"<body([\s|\S]*?)>([\s|\S]*?)</body>", "<body$1>$2</body>", RegexOptions.IgnoreCase);
            return sourceValue;
        }
    }
}

