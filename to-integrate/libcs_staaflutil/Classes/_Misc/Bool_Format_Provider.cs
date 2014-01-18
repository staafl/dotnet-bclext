using System;

namespace Fairweather.Service
{
    // source : worldspawn[] @ http://forums.asp.net/t/1127044.aspx

    public class Bool_Format_Provider : IFormatProvider, ICustomFormatter
    {
        // ICustomFormatter

        public string Format(string format, object arg, IFormatProvider formatProvider) {

            var value = (bool)arg;

            format = (format == null ? null : format.Trim().ToLower());

            if (format == "yn")
                return value ? "Yes" : "No";

            var split = format.Split(',');
            if (split.Length == 2)
                return split[value ? 0 : 1];

            if (arg is IFormattable)
                return ((IFormattable)arg).ToString(format, formatProvider);
            else
                return arg.ToString();
        }

        // IFormatProvider

        public object GetFormat(Type formatType) {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

    }
}
