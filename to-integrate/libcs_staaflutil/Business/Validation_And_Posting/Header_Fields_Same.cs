using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using System.Linq;
using Common.Sage;

namespace Common.Posting
{

    public class Header_Fields_Same : Rule_Base
    {

        /* 929872BF24234602BF1F13F2BA40E9E8 */ 

        public Header_Fields_Same(Func<Data_Line, Data_Line, bool> are_same_header,
                                  IEnumerable<string> header_fields,
                                  string header_term) {

            this.are_same_header = are_same_header;
            this.header_fields = header_fields;
            this.header_term = header_term ?? "header";
            
        }

        readonly IEnumerable<string> header_fields;
        readonly Func<Data_Line, Data_Line, bool> are_same_header;
        readonly string header_term;

        Data_Line header_start;
        int? null_header_start_ix;

        public override bool
        Inspect_Line(Data_Line line,
                     int ix,
                     bool is_last,
                     out bool to_skip,
                     out IEnumerable<Validation_Error> errors
                     ) {

            H.assign(out to_skip, out errors);

            if (header_start == null ||
                !are_same_header(header_start, line)) {
                header_start = line;
                null_header_start_ix = ix;
                return true;

            }

            var diff = new List<string>();

            foreach (var field in header_fields) {

                object header_field = header_start[field];
                object line_field = line[field];

                if (header_field.Safe_Equals(line_field))
                    continue;

                diff.Add(field);


            }

            if (diff.Count == 0)
                return true;


            var msg = "Record {0} is part of {1} starting with record {2}, but the two have different values for the following field(s): ";
            msg = msg.spf(ix + 1, header_term, null_header_start_ix + 1);
            msg += diff.Simple_Print();
            var error = new Validation_Error(ix, msg);
            errors = new[] { error };


            return false;

        }

        public override void Clear() {
            header_start = null;
            null_header_start_ix = null;
        }
    }
}
