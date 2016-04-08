using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daisybrand.forecaster.Extensions
{
    public static class _Date
    {
        public static DateTime LastYearDate(this DateTime d)
        {
            if (DateTime.IsLeapYear(d.Year) && d.Month == 2 && d.Day == 29)
                return new DateTime(d.Year - 4, d.Month, d.Day);
            return new DateTime(d.Year - 1, d.Month, d.Day);
        }

        public static IEnumerable<DateTime> GetAllDaysInWeek(this DateTime d)
        {
            var list = new List<DateTime>();
            for (int i = 0; i < 7; i++)
            {
                list.Add(d.AddDays(i));
            }
            return list;
        }

        public static int WeekNumber(this DateTime d, System.DayOfWeek firstDayOfWeek)
        {
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            System.DayOfWeek dayOfWeek = d.DayOfWeek;
            var ms = cal.GetWeekOfYear(new DateTime(d.Year, d.Month, d.Day), System.Globalization.CalendarWeekRule.FirstDay, firstDayOfWeek);
            return ms;
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek firstDayOfWeek)
        {
            int diff = dt.DayOfWeek - firstDayOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime lastDayOfLastWeek(this DateTime d, System.DayOfWeek firstDayOfWeek)
        {
            if (d.DayOfWeek == firstDayOfWeek) return d.AddDays(-1);
            return d.AddDays(-1).lastDayOfLastWeek(firstDayOfWeek);
        }
        public static System.DayOfWeek DayOfWeek(string customerNumber)
        {
            return System.DayOfWeek.Sunday;
        }

        public static System.DayOfWeek GetDayOfWeek(this string day, DayOfWeek ifNoValue)
        {
            if (day != null)
                foreach (DayOfWeek d in Enum.GetValues(typeof(DayOfWeek))
                                  .OfType<DayOfWeek>()
                                  .ToList())
                {
                    if (d.ToString().ToUpper() == day.ToUpper())
                        return d;
                }
            return ifNoValue;
        }

        /// <summary>
        /// RETURNS SHORT DATE STRING
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToString(this DateTime dt)
        {
            return dt.ToShortDateString();
        }

        public static DateTime ToDateTime(this string s)
        {
            var year = s.Substring(0, 4);
            var day = s.Substring(6, 2);
            var month = s.Substring(4, 2);
            return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));            
        }


    }
}
