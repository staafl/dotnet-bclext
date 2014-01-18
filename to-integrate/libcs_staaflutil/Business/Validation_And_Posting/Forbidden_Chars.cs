using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using System.Linq;
using Common.Sage;

namespace Common.Posting
{
    //    public class Max_Length : RuleBase
    //    {
    //        readonly string field;
    //        readonly int len;

    //        public Max_Length(string field, int len) {
    //            this.field = field;
    //            this.len = len;
    //        }

    //        public override bool
    //        Inspect_Line(Data_Line line,
    //                     int ix,
    //bool is_last,
    //                     out bool to_skip,
    //                     out IEnumerable<Validation_Error> errors
    //                     ) {

    //            H.assign(out to_skip, out errors);

    //            object val;
    //            if (!line.Dict.TryGetValue(field, out val))
    //                return true;

    //            var as_string = val as string;

    //            if (as_string == null)
    //                return true;

    //            if (as_string.Length <= len)
    //                return true;

    //            var error = new Validation_Error(ix, "Field '{0}' should not be longer than {1} chars.".spf(field, len));
    //            errors = new[] { error };
    //            return false;

    //        }

    //    }

    public class Forbidden_Chars_Rule : Rule_Base
    {
        readonly string field;
        readonly Set<Char> set;
        readonly string msg;
        public Forbidden_Chars_Rule(string field, params char[] chs) {
            this.field = field;
            this.set = new Set<char>(chs);
            this.msg = "Field '" + field + "' should not contain any of the following characters: " + set.Aggregate("", (_s, _c1) => _s + _c1);

        }

        public override bool
        Inspect_Line(Data_Line line,
                     int ix,
                     bool is_last,
                out bool to_skip,
                out IEnumerable<Validation_Error> errors
                ) {

            H.assign(out to_skip, out errors);

            object val;
            if (!line.Dict.TryGetValue(field, out val))
                return true;

            var as_str = val.strdef();

            // Set<char> errors = null;

            foreach (var ch in as_str) {
                if (!set[ch])
                    continue;
                var error = new Validation_Error(ix, msg);
                errors = new[] { error };
                to_skip = true;
                break;
                //if (errors == null)
                //errors = new Set<char>();
                //errors[ch] = true;
            }

            return errors == null;
        }


    }
}
