namespace Git.Framework.DataTypes.Formatters
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public class GenericStringFormatter : IFormatProvider, ICustomFormatter
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private char <AlphaChar>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private char <DigitChar>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private char <EscapeChar>k__BackingField;

        public GenericStringFormatter()
        {
            this.DigitChar = '#';
            this.AlphaChar = '@';
            this.EscapeChar = '\\';
        }

        public virtual string Format(string Input, string FormatPattern)
        {
            if (!this.IsValid(FormatPattern))
            {
                throw new ArgumentException("FormatPattern is not valid");
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < FormatPattern.Length; i++)
            {
                if (FormatPattern[i] == this.EscapeChar)
                {
                    i++;
                    builder.Append(FormatPattern[i]);
                }
                else
                {
                    char matchChar = '\0';
                    Input = this.GetMatchingInput(Input, FormatPattern[i], out matchChar);
                    if (matchChar > '\0')
                    {
                        builder.Append(matchChar);
                    }
                }
            }
            return builder.ToString();
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return this.Format(arg.ToString(), format);
        }

        public object GetFormat(Type formatType)
        {
            return ((formatType == typeof(ICustomFormatter)) ? this : null);
        }

        protected virtual string GetMatchingInput(string Input, char FormatChar, out char MatchChar)
        {
            bool flag = FormatChar == this.DigitChar;
            bool flag2 = FormatChar == this.AlphaChar;
            if (!flag && !flag2)
            {
                MatchChar = FormatChar;
                return Input;
            }
            int startIndex = 0;
            MatchChar = '\0';
            for (int i = 0; i < Input.Length; i++)
            {
                if ((flag && char.IsDigit(Input[i])) || (flag2 && char.IsLetter(Input[i])))
                {
                    MatchChar = Input[i];
                    startIndex = i + 1;
                    break;
                }
            }
            return Input.Substring(startIndex);
        }

        protected virtual bool IsValid(string FormatPattern)
        {
            bool flag = false;
            for (int i = 0; i < FormatPattern.Length; i++)
            {
                if (((flag && (FormatPattern[i] != this.DigitChar)) && (FormatPattern[i] != this.AlphaChar)) && (FormatPattern[i] != this.EscapeChar))
                {
                    return false;
                }
                if (flag)
                {
                    flag = false;
                }
                else if (FormatPattern[i] == this.EscapeChar)
                {
                    flag = true;
                }
            }
            if (flag)
            {
                return false;
            }
            return true;
        }

        public virtual char AlphaChar { get; protected set; }

        public virtual char DigitChar { get; protected set; }

        public virtual char EscapeChar { get; protected set; }
    }
}

