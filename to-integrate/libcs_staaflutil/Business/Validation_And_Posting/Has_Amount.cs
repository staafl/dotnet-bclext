using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using System.Linq;
using Common.Sage;

namespace Common.Posting
{
    public class Has_Amount : Rule_Base
    {

        public override bool
        Inspect_Line(Data_Line line,
                     int ix,
                     bool is_last,
                     out bool to_skip,
                     out IEnumerable<Validation_Error> errors
                     ) {

            H.assign(out to_skip, out errors);

            var net = line.NET_AMOUNT;
            var tax = line.TAX_AMOUNT;

            var total = net + tax;

            if (total == 0.0m) {
                // "The sum of NET_AMOUNT and TAX_AMOUNT fields should be greater than 0.00"
                var error = new Validation_Error(ix, 
                    "The sum of NET_AMOUNT and TAX_AMOUNT should be greater than 0.00"
                    //"Zero value transaction."
                    );
                errors = new[] { error };
                return false;
            }

            return true;

        }

    }
}
