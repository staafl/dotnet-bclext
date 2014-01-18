using System;

namespace Fairweather.Service
{
    public static partial class Magic
    {
        public static string
        get_free_name(string format, int start, int? end, Func<string, bool> predicate) {
            for (int ii = start; !(ii > end); ++ii) {
                var ret = format.spf(ii);
                if (predicate(ret))
                    return ret;

            }

            return null;
        }
    }
}
