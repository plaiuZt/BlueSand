namespace Git.Framework.DataTypes
{
    using System;
    using System.Runtime.CompilerServices;

    public static class DateTimeUtil
    {
        private static object lockObj = new object();
        public static readonly DateTime MaxValue = DateTime.MaxValue;
        public static readonly DateTime MinValue = new DateTime(0x76c, 1, 1);
        private static readonly string[] weekString = new string[] { "日", "一", "二", "三", "四", "五", "六" };

        public static string FormatLongString(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string FormatLongStringC(this DateTime time)
        {
            return time.ToString("yyyy年MM月dd日 HH:mm:ss");
        }

        public static string FormatShortString(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd");
        }

        public static string FormatShortStringC(this DateTime time)
        {
            return time.ToString("yyyy年MM月dd日");
        }

        public static string FormatWeekString(this DateTime time)
        {
            return ("星期" + weekString[(int) time.DayOfWeek]);
        }
    }
}

