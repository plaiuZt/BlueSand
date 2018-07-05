namespace Git.Framework.DataTypes.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class TimeSpanExtensions
    {
        public static TimeSpan Average(this IEnumerable<TimeSpan> List)
        {
            List = List.Check<IEnumerable<TimeSpan>>(new List<TimeSpan>());
            return (List.Any<TimeSpan>() ? new TimeSpan((long) Enumerable.Average<TimeSpan>(List, <>c.<>9__0_0 ?? (<>c.<>9__0_0 = new Func<TimeSpan, long>(<>c.<>9.<Average>b__0_0)))) : new TimeSpan(0L));
        }

        public static int DaysRemainder(this TimeSpan Span)
        {
            DateTime time = DateTime.MinValue + Span;
            return (time.Day - 1);
        }

        public static int Months(this TimeSpan Span)
        {
            DateTime time = DateTime.MinValue + Span;
            return (time.Month - 1);
        }

        public static string ToStringFull(this TimeSpan Input)
        {
            string str = "";
            string str2 = "";
            if (Input.Years() > 0)
            {
                object[] objArray1 = new object[] { str, Input.Years(), " year", (Input.Years() > 1) ? "s" : "" };
                str = string.Concat(objArray1);
                str2 = ", ";
            }
            if (Input.Months() > 0)
            {
                object[] objArray2 = new object[] { str, str2, Input.Months(), " month", (Input.Months() > 1) ? "s" : "" };
                str = string.Concat(objArray2);
                str2 = ", ";
            }
            if (Input.DaysRemainder() > 0)
            {
                object[] objArray3 = new object[] { str, str2, Input.DaysRemainder(), " day", (Input.DaysRemainder() > 1) ? "s" : "" };
                str = string.Concat(objArray3);
                str2 = ", ";
            }
            if (Input.Hours > 0)
            {
                object[] objArray4 = new object[] { str, str2, Input.Hours, " hour", (Input.Hours > 1) ? "s" : "" };
                str = string.Concat(objArray4);
                str2 = ", ";
            }
            if (Input.Minutes > 0)
            {
                object[] objArray5 = new object[] { str, str2, Input.Minutes, " minute", (Input.Minutes > 1) ? "s" : "" };
                str = string.Concat(objArray5);
                str2 = ", ";
            }
            if (Input.Seconds > 0)
            {
                object[] objArray6 = new object[] { str, str2, Input.Seconds, " second", (Input.Seconds > 1) ? "s" : "" };
                str = string.Concat(objArray6);
                str2 = ", ";
            }
            return str;
        }

        public static int Years(this TimeSpan Span)
        {
            DateTime time = DateTime.MinValue + Span;
            return (time.Year - 1);
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly TimeSpanExtensions.<>c <>9 = new TimeSpanExtensions.<>c();
            public static Func<TimeSpan, long> <>9__0_0;

            internal long <Average>b__0_0(TimeSpan x)
            {
                return x.Ticks;
            }
        }
    }
}

