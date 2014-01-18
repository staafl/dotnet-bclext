using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using System.Linq;
using Common.Sage;

namespace Common.Posting
{
    public class Def_Nom_Code_Rule : Rule_Base
    {
        readonly Sage_Context context;
        public Def_Nom_Code_Rule(Sage_Context context) {
            this.context = context;
        }

        public override bool
        Inspect_Line(Data_Line line,
                     int ix,
                     bool is_last,

                     out bool to_skip,
                     out IEnumerable<Validation_Error> errors) {

            H.assign(out to_skip, out errors);

            object obj;
            if (!line.Dict.TryGetValue("DEF_NOM_CODE", out obj))
                return true;

            var as_str = obj.strdef();
            if (as_str.IsNullOrEmpty())
                return true;

            string error_msg = null;
            bool major_control;
            Versioning.NominalType type;

            if (context.Check_Nominal(as_str, out major_control, out type)) {
                if (type == Versioning.NominalType.sdoTypeBank)
                    error_msg = "DEF_NOM_CODE should not be a Bank Account in the Nominal Ledger.";
                else if (major_control)
                    error_msg = "DEF_NOM_CODE should not be a Major Control Account in the Nominal Ledger.";

            }
            else {
                error_msg = "DEF_NOM_CODE should be a valid entry in the Nominal Ledger.";
            }


            if (error_msg == null)
                return true;

            var error = new Validation_Error(ix, error_msg);
            errors = new[] { error };

            return false;


        }

    }
}
