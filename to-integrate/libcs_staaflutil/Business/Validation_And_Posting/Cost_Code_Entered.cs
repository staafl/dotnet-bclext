using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using System.Linq;
using Common.Sage;
using Versioning;

namespace Common.Posting
{
    public class Cost_Code_Entered : Rule_Base
    {
        readonly int ver;
        public Cost_Code_Entered(int ver) {
            this.ver = ver;
        }

        public override bool
        Inspect_Line(Data_Line data, int ix, bool is_last,
             out bool skip, out IEnumerable<Validation_Error> errors) {

            H.assign(out skip, out errors);

            var type_str = data.Clean(TT_Rules.STR_TYPE);

            if (type_str.Trim().IsNullOrEmpty())
                // not our problem
                return true;

            var type = data.TYPE;

            if (!type.CC_Required(ver))
                return true;

            var cc = data.Clean(TT_Rules.STR_COST_CODE);

            if (cc.IsNullOrEmpty())
                return true;

            var proj = data.Clean(TT_Rules.STR_PROJECT_REF);
            if (!proj.IsNullOrEmpty())
                return true;

            errors = new[] { new Validation_Error(ix, "'" + type.Short_String() + TT_Rules.CC_Required_Error) };

            return false;

        }

    }
}
