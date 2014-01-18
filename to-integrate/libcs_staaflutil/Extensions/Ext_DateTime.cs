using System;
using System.Collections.Generic;

namespace Fairweather.Service
{
    static partial class Extensions
    {

        public static DateTime 
        Next_Financial_Period_Start(this DateTime start) {

            var ret = start.AddYears(1);
            if (start.Month == 2 &&
                start.Day == 29) {

                ret = ret.AddDays(1);

            }

            return ret;

        }

        public static bool Between(this DateTime date, DateTime start, DateTime end,
                                   bool start_inclusive, bool end_inclusive) {

            return DateTime.Compare(date, start) > (start_inclusive ? -1 : 0) &&
                   DateTime.Compare(date, end) < (end_inclusive ? 1 : 0);

        }
        /*       To test        */

        public static string To_Sortable(this DateTime date, bool time) {

            return date.ToString("yyyy-MM-dd" + (time ? " hh-mm-ss" : ""));

        }

        public static DateTime Next_Day(this DateTime date) {

            var ret = date.AddDays(1.0).Date;

            return ret;
        }

        public static int Comparable_Day(this DateTime date) {

            var ret = date.Year * 400;
            ret += date.DayOfYear;

            return ret;

        }


        // Monday is assumed to be 1, Sunday - 7
        public static string Get_Weekday_Abbr(this int weekday, bool long_name) {

            (weekday >= 1 && weekday <= 7).tiff<ArgumentOutOfRangeException>();

            // 1.1.2006 was a Sunday
            var temp = new DateTime(2006, 1, 1 + weekday);
            string ret = long_name ? temp.ToString("dddd") :
                                     temp.ToString("ddd");

            if (ret.Length > 2)
                ret = ret.Remove(2);

            return ret;
        }

        static public DateTime First_Date_Of_Month(this DateTime date) {

            int d = date.Day;

            DateTime ret = (date.AddDays(1 - d));

            return ret;
        }

        static public DateTime Last_Date_Of_Month(this DateTime date) {

            DateTime ret = new DateTime(date.Year,
                                        date.Month,
                                        DateTime.DaysInMonth(date.Year,
                                                             date.Month)
                                        );

            return ret;
        }

        /// <summary> Assumes months and days are counted from 1.
        /// </summary>
        static public bool Is_Valid_Date(this int year, int month, int day) {

            bool ret = true;

            if (month < 1 || month > 12)
                ret = false;

            else if (Math.Min(year, day) <= 0)
                ret = false;

            else if (day > DateTime.DaysInMonth(year, month))
                ret = false;

            else {
                try {
                    var dateTime = new DateTime(year, month, day);
                    H.Void(dateTime);
                }
                catch (ArgumentException) {
                    ret = false;
                }
            }

            return ret;
        }

        static public DateTime First_Weekday_Of_Month(this DateTime date, DayOfWeek weekday) {
            DateTime ret = date.First_Date_Of_Month();

            for (int ii = 1; ii < 7; ++ii) {
                if (ret.DayOfWeek == weekday)
                    return ret;
                ret = ret.AddDays(1);
            }

            return ret;
        }

        static public int Get_Row_Of_Date(this DateTime date) {

            DateTime temp = date.First_Date_Of_Month();

            int num_1st = (int)temp.DayOfWeek - 1;

            if (temp.DayOfWeek == DayOfWeek.Sunday)
                num_1st = 6;

            else if (temp.DayOfWeek == DayOfWeek.Monday)
                num_1st = 7;

            int k = date.Day + num_1st;

            return k / 7;
        }

        static public bool Same_Month(this DateTime date1, DateTime date2) {

            bool b1 = date1.Year == date2.Year;
            bool b2 = date1.Month == date2.Month;

            return (b1 && b2);
        }

        static public bool Same_Day(this DateTime date1, DateTime date2) {

            bool b1 = date1.Year == date2.Year;
            bool b2 = date1.Month == date2.Month;
            bool b3 = date1.Day == date2.Day;

            return (b3 && b1 && b2);
        }

        static public DateTime Combine_Date_Time(this DateTime date, DateTime time) {

            DateTime ret = date.Date;
            ret = ret.Add(time.TimeOfDay);

            return ret;
        }

        static public List<DateTime> Days_Between(this DateTime date_from,
                                             DateTime date_to,
                                             bool to_inclusive) {

            (date_from <= date_to).tiff();
            (date_from != date_to || to_inclusive).tiff();

            var time_span = date_to - date_from;

            int days = time_span.Days;

            if (!to_inclusive)
                --days;

            var ret = new List<DateTime>(days);

            for (int ii = 0; ii < days; ++ii) {

                ret.Add(date_from.AddDays(ii));

            }

            return ret;

        }

        static public bool Is_Weekend(this DateTime date) {

            var ret = date.DayOfWeek == DayOfWeek.Saturday ||
                      date.DayOfWeek == DayOfWeek.Sunday;

            return ret;

        }

        static public int Number_Of_Workdays(this DateTime date_from,
                                             DateTime date_to,
                                             bool to_inclusive,
                                             params DateTime[] public_holidays) {

            var days = Days_Between(date_from, date_to, to_inclusive);

            int cnt_holidays = public_holidays.Length;
            bool has_holidays = cnt_holidays != 0;

            int holiday = 0;
            int ret = 0;

            if (has_holidays)
                Array.Sort(public_holidays);

            foreach (var day in days) {

                bool is_holiday = false;

                if (day.Is_Weekend())
                    continue;

                if (has_holidays) {

                    int day_1, day_2;

                    while (true) {
                        day_1 = public_holidays[holiday].Comparable_Day();
                        day_2 = day.Comparable_Day();

                        if (day_1 < day_2)
                            ++holiday;

                        if (holiday >= cnt_holidays)
                            break;

                    }

                    if (holiday == cnt_holidays) {
                        has_holidays = false;
                    }
                    else {
                        is_holiday = (day_1 == day_2);

                    }
                }

                if (!is_holiday)
                    ++ret;

            }

            return ret;


        }
    }
}