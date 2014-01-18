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
    public class TT_Brief : Rule_Base
    {
        readonly Sage_Context context;

        public TT_Brief(Sage_Context context) {
            this.context = context;
        }

        public override bool
        Inspect_Line(Data_Line data, int ix, bool is_last,
                     out bool skip, out IEnumerable<Validation_Error> errors) {

            H.assign(out skip, out errors);

            var type = data.TYPE;

            var mode = type.Get_TT_Mode();

            if (mode != Record_Type.Sales && mode != Record_Type.Purchase && mode != Record_Type.Bank)
                return true;

            var nom_code = data.Clean(TT_Rules.STR_NOMINAL_CODE);

            if (nom_code.IsNullOrEmpty())
                return true;

            var ss = type.Short_String();
            var lst = new List<Validation_Error>();
            Action<string> err = _s =>
            {
                var _error = new Validation_Error(ix, _s);
                lst.Add(_error);

            };

            bool major;
            NominalType nom_type;


            if (context.Check_Nominal(nom_code, out major, out nom_type)) {

                var is_bank = (nom_type == NominalType.sdoTypeBank);

                if (type.Should_Not_Be_Bank_Major()) {

                    if (is_bank) {
                        err(
"With Transaction Type '" + ss + "', NOMINAL_CODE should not be a Bank Account entry in the Nominal Ledger.");

                    }
                    else if (major) {
                        err(
"With Transaction Type '" + ss + "', NOMINAL_CODE should not be a Major Control Account in the Nominal Ledger.");

                    }


                }
                else if (type.Should_Be_Bank()) {

                    if (!is_bank)
                        err(
"With Transaction Type '" + ss + "', NOMINAL_CODE should be a Bank Account entry in the Nominal Ledger.");

                }
            }

            errors = lst;
            return errors.Count() == 0;
        }
    }
}
