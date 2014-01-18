using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using System.Linq;
using Common.Sage;
using System.Globalization;
using Versioning;

namespace Common.Posting
{
    public class Inv_Ref_Exists : Rule_Base
    {
        readonly Sage_Context context;
        public Inv_Ref_Exists(Sage_Context context) {
            this.context = context;
        }

        public override bool
        Inspect_Line(Data_Line line,
                     int ix,
                     bool is_last,
                     out bool skip,
                     out IEnumerable<Validation_Error> errors) {

            H.assign(out skip, out errors);

            var inv_ref = line.INV_REF;

            if (inv_ref.IsNullOrEmpty())
                return true;

            var type = line.TYPE;

            if (context.Document_Exists(inv_ref, type))
                return true;

            errors = new[] { new Validation_Error(ix, "Document '" + inv_ref + "' does not exist in the records.") };
            return false;

        }
    }
}
