using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Git.Framework.DataTypes.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static int DaysInMonth(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return Date.LastDayOfMonth().Day;
        }

        public static int DaysLeftInMonth(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return (Thread.CurrentThread.CurrentCulture.Calendar.GetDaysInMonth(Date.Year, Date.Month) - Date.Day);
        }

        public static int DaysLeftInWeek(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return (7 - ((int)Date.DayOfWeek + 1));
        }

        public static int DaysLeftInYear(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return (Thread.CurrentThread.CurrentCulture.Calendar.GetDaysInYear(Date.Year) - Date.DayOfYear);
        }

        public static DateTime FirstDayOfMonth(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return new DateTime(Date.Year, Date.Month, 1);
        }

        public static DateTime FirstDayOfWeek(this DateTime Date, CultureInfo CultureInfo = null)
        {
            Date.ThrowIfNull("Date");
            return Date.AddDays((double) (CultureInfo.NullCheck<CultureInfo>(CultureInfo.CurrentCulture).DateTimeFormat.FirstDayOfWeek - Date.DayOfWeek)).Date;
        }

        public static DateTime FromUnixTime(this int Date)
        {
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return new DateTime((Date * 0x989680L) + time.Ticks, DateTimeKind.Utc);
        }

        public static DateTime FromUnixTime(this long Date)
        {
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return new DateTime((Date * 0x989680L) + time.Ticks, DateTimeKind.Utc);
        }

        public static bool IsInFuture(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return (DateTime.Now < Date);
        }

        public static bool IsInPast(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return (DateTime.Now > Date);
        }

        public static bool IsWeekDay(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return ((Date.DayOfWeek < DayOfWeek.Saturday) && (Date.DayOfWeek > DayOfWeek.Sunday));
        }

        public static bool IsWeekEnd(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return !Date.IsWeekDay();
        }

        public static DateTime LastDayOfMonth(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return Date.AddMonths(1).FirstDayOfMonth().AddDays(-1.0).Date;
        }

        public static DateTime LastDayOfWeek(this DateTime Date, CultureInfo CultureInfo = null)
        {
            Date.ThrowIfNull("Date");
            return Date.FirstDayOfWeek(CultureInfo.NullCheck<CultureInfo>(CultureInfo.CurrentCulture)).AddDays(6.0);
        }

        public static string To(this DateTime Date)
        {
            if ((Date == DateTime.MinValue) || (Date.ToString("yyyy-MM-dd") == "1970-01-01"))
            {
                return string.Empty;
            }
            return Date.ToString();
        }

        public static string To(this DateTime Date, string Format)
        {
            if ((Date == DateTime.MinValue) || (Date.ToString("yyyy-MM-dd") == "1970-01-01"))
            {
                return string.Empty;
            }
            return Date.ToString(Format);
        }

        public static int ToUnix(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            TimeSpan span = (TimeSpan) (Date.ToUniversalTime() - new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return (int) (span.Ticks / 0x989680L);
        }
    }
}

