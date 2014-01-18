using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Common;
using Versioning;

using Fairweather.Service;
using Common.Posting;
using Line = Fairweather.Service.Pair<Common.Posting.Data_Line, Sage_Int.Line_Data>;

namespace Sage_Int
{
    class Field_Count_Validator
    {
        int min, max;

        public int Min {
            get {
                return min;
            }
        }
        public int Max {
            get {
                return max;
            }
        }

        public bool
        Check(int cnt /* count of delimiting commas + 1 */,
              bool end_on_comma,
              out string desc) {

            desc = null;

            // common cases first
            if (min == cnt &&
                max == cnt)
                return true;

            if (end_on_comma &&
                min == cnt - 1 &&
                max == cnt - 1)
                return true;

            // ****************************

            // alpha,bravo,         --> cnt = 3, min = 2, max = 3
            // alpha,bravo,charlie  --> cnt = 3, min = 3, max = 3

            // minimum number of fields this row could really contain
            var this_min = end_on_comma ? cnt - 1 : cnt;

            // maximum number of fields this row could really contain
            var this_max = cnt;


            if (min == 0) {
                min = this_min;
                max = this_max;
            }



            if (this_min > max || this_max < min) {

                var expected = min + ((min == max) ? "" : "or " + max);

                desc = "Unexpected number of fields: expected {0}, found {1}.".spf(expected, cnt);

                return false;

            }

            if (this_min == max) {          // min max
                                            //     this_min this_max
                min = this_min;
                return true;

            }
            else if (this_max == min) {     // this_min this_max
                                            //          min       max
                max = this_max;
                return true;

            }



            // not possible
            throw new InvalidOperationException();
        }

        public bool Check(int cnt, bool end_on_comma) {
            string desc;
            return Check(cnt, end_on_comma, out desc);
        }
    }
}
