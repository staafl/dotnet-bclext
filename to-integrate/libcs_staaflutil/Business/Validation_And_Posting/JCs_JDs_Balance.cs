using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using System.Linq;
using Common.Sage;
using TT = Versioning.TransType;

namespace Common.Posting
{
#warning has state
    public class JCs_JDs_Balance : Rule_Base
    {
        JCs_JDs_Calc calc = new JCs_JDs_Calc();
        class JCs_JDs_Calc
        {

            decimal jc_amt = 0.0m;
            decimal jd_amt = 0.0m;

            bool inside = false;
            int start = -1;
            int end = -1;
            int prev_row;
            DateTime? last_date;
            string last_acc;

            const TT
            jc = TT.sdoJC,
            jd = TT.sdoJD;

            /// <summary>
            /// If !return => an error was detected.
            /// </summary>
            public bool
            Check_Line(Data_Line line,
                        int row,
                        bool is_last,
                        out Triple<int, int, decimal>? error1,
                        out Triple<int, int, decimal>? error2) {

                error1 = error2 = null;

                var type = line.TYPE;
                var is_jx = type == jc || type == jd;

                while (true) { // we will need to loop iff 'jx_ends_stretch'

                    var starting_fresh = false;
                    var same_stretch = false;
                    var jx_ends_stretch = false;
                    var no_jx_ends_stretch = false;

                    if (inside) {
                        same_stretch = is_jx &&
                                       (last_date != default(DateTime)) &&
                                       (last_date == line.DATE)
                            //&& !last_acc.IsNullOrEmpty() &&
                            //(last_acc == line.ACCOUNT_REF)
                            ;

                        jx_ends_stretch = is_jx && !same_stretch;
                        no_jx_ends_stretch = !is_jx;

                    }
                    else {
                        starting_fresh = is_jx;

                    }

                    last_date = is_jx ? line.DATE : (DateTime?)null;
                    last_acc = is_jx ? line.ACCOUNT_REF : null;

                    if (starting_fresh || same_stretch) {

                        // jx_ends_stretch.tift();

                        // same or brand new stretch

                        if (starting_fresh) {

                            // vrand new stretch

                            inside = true;
                            start = row;
                            jc_amt = jd_amt = 0.0m;

                        }

                        var net = line.NET_AMOUNT;

                        if (type == jc)
                            jc_amt += net;
                        else
                            jd_amt += net;

                    }

                    // keep this here
                    var last_ends_stretch = is_last && inside;

                    if (jx_ends_stretch ||
                        no_jx_ends_stretch ||
                        last_ends_stretch) {

                        // end of stretch

                        inside = false;

                        end = (jx_ends_stretch || no_jx_ends_stretch)
                              ? prev_row
                              : row
                              ;

                        if (jc_amt != jd_amt) {
                            var error = Triple.Make(start, end, jc_amt - jd_amt);
                            if (error1 == null)
                                error1 = error;
                            else
                                error2 = error;
                        }
                    }

                    if (jx_ends_stretch)
                        // we need to take this line into account as well
                        continue;

                    prev_row = row;

                    return (error1 == null && error2 == null);

                }



            }
        }


        public override void Clear() {
            calc = new JCs_JDs_Calc();
        }

        public override bool
        Inspect_Line(Data_Line line,
                     int ix,
                     bool is_last,

                     out bool to_skip,
                     out IEnumerable<Validation_Error> errors
                ) {

            H.assign(out to_skip, out errors);
            Triple<int, int, decimal>? triple1, triple2;

            var ret = calc.Check_Line(line, ix, is_last, out triple1, out triple2);

            // a bit of a hack?
            if (is_last)
                Clear();

            if (ret)
                return true;


            Validation_Error? error_tmp = null;
            foreach (var null_tuple in new[] { triple1, triple2 }) {

                if (null_tuple == null)
                    continue;

                var tuple = null_tuple.Value;

                var start = tuple.First;
                var end = tuple.Second;
                var off_by = tuple.Third;

                var description =
                    (start == end)
                    ? "Single Debit or Credit entry is not permitted." // "Single JC/JD entry not allowed."
                    // "Total debit amount does not equal to the total credit amount."
                    : "Total debit amount on lines {0}-{1} does not equal to the total credit amount.".spf(start, end);
                //"JC/JD amounts mismatched by " + off_by + ".";

                var error = new Validation_Error(
                    end,
                    description,
                    Magic.range(start, end));

                if (error_tmp == null) {
                    error_tmp = error;
                }
                else {
                    errors = new[] { error_tmp.Value, error };
                    return false;
                }

            }

            if (error_tmp == null)
                return true;

            // error_tmp.tifn();

            errors = new[] { error_tmp.Value };
            return false;

        }

    }
}
