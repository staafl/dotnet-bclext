using System;
using System.Globalization;
using Common;
using Fairweather.Service;

namespace Standardization
{
    public static class Formatting
    {


         const string comma_format = "#,#0.00";

        public static string ToString(this decimal dec, bool use_default) {
            if (use_default)
                return dec.ToString(cst_decimal_format);
            else
                return dec.ToString();
        }
        public static string ToString(this decimal dec, bool use_default, bool use_commas) {

            if (use_default) {
                if (use_commas)
                    return dec.ToString(comma_format);
                else
                    return dec.ToString(cst_decimal_format);
            }
            else {
                return dec.ToString();
            }

        }

        public static string ToString(this decimal dec, int decimal_places) {
            return dec.ToString("F" + decimal_places.ToString());
        }

        public static string ToString(this DateTime dt, bool use_default) {
            return dt.ToString(use_default, false);
        }

        public static string ToString(this DateTime dt, bool use_default, bool sortable) {

            if (use_default)
                return dt.ToString(Data.Date_Format(sortable));
            else
                return dt.ToString();
        }

        public static decimal FromString(this string dec_str) {

            var ret = dec_str.DecimalOrZero(cst_decimal_precision);

            return ret;

        }

        public static DateTime FromString(this string date_str, bool sortable) {

            string format = Data.Date_Format(sortable);

            var ret = DateTime.ParseExact(date_str, format, null, DateTimeStyles.AllowWhiteSpaces);
            return ret;

        }

        static readonly string cst_decimal_format = "F" + cst_decimal_precision.ToString();
        const int cst_decimal_precision = Data.DEF_DISPLAY_PRECISION;

    }
}