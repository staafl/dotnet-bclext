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
#warning modifies the data
    public class TT_Rules : Rule_Base
    {
        public const string CC_Required_Error = "' Transactions require a COST_CODE field for posting against a Project.";


        public TT_Rules(Sage_Context context,
                        bool check,
                        bool change) {
            this.context = context;
            this.check = check;
            this.change = change;



        }


        // verified on 03.06.10
        // see sit_docs/old tt validation.txt
        public override bool
        Inspect_Line(Data_Line data, int ix, bool is_last,
                     out bool skip, out IEnumerable<Validation_Error> errors) {

            H.assign(out skip, out errors);

            var type_str = data.Clean(STR_TYPE);

            if (type_str.Trim().IsNullOrEmpty())
                // not our problem
                return true;

            TransType type = 0;
            try {
                type = data.TYPE;
            }
            catch (InvalidCastException) {
            }
            var ss = type.Short_String();

            var acc_ref = data.Clean(STR_ACCOUNT_REF);
            var nom_code = data.Clean(STR_NOMINAL_CODE);
            var bank_code = data.Clean(STR_BANK_CODE);

            var proj = data.Clean(STR_PROJECT_REF);
            var cc = data.Clean(STR_COST_CODE);

            var lst = new List<Validation_Error>();
            errors = lst;

            Action<string> err = _s =>
            {
                var _error = new Validation_Error(ix, _s);
                lst.Add(_error);

            };

            if (!type.Supported(context.Version)) {
                if (type.Supported(null)) {
                    err("Transaction Type '" + ss + "' is not available in your version of Sage.");
                }
                else {
                    // "Transaction Type \"{0}\" is not a valid Transaction Type."
                    err("Invalid Transaction Type: " + ss);
                }
                return false;
            }


            var mode = type.Get_TT_Mode();



            /* As you can see, the following logic does a lot of juggling with
             * 'acc_ref', 'bank_code' and 'nom_code', in order to give the csv creator
             * the greatest possible ability to omit values and leave their inference
             * to us. 
             * The logic has been carefully checked in order to ensure its correctness -
             * all default assignments happen before the field is first used in any flow
             * path. If you modify the logic, take care to keep it that way.
             */

            // ****************************

            if (mode == Record_Type.Bank) {

                if (bank_code.IsNullOrEmpty()) {
                    // ****************************
                    /*       Assignment          */
                    // ****************************

                    bank_code = acc_ref; // will not be used if (mode == Record_Type.Bank) 

                }
                else if (bank_code != acc_ref) {
                    err(
"With Transaction Type '" + ss + "', BANK_CODE should either be empty or the same as ACCOUNT_REF.");

                }

                BankType bank_type;
                if (context.Check_Bank(acc_ref, out bank_type)) {

                    if (check) {

                        var needed_type = type.Correct_Trans_Type(bank_type);

                        if (change || needed_type == type) {
                            // ****************************
                            /*       Assignment          */
                            // ****************************

                            if (change) {
                                // this preserves (mode == Record_Type.Bank)
                                //(type == needed_type || 
                                // Get_TT_Mode(needed_type)==Record_Type.Bank).tiff();

                                type = needed_type;

                            }

                        }
                        else {
                            err("The provided Transaction Type is not valid for this Bank Account.");

                        }
                    }

                }
                else {
                    err("Bank Ledger Account '" + acc_ref + "' does not exist.");

                }

            }

            // ****************************

            Check_Project_Cost_Code(type, err, proj, cc);

            // ****************************

            if (mode == Record_Type.Expense) {

#warning verify this
                if (!bank_code.IsNullOrEmpty()) {
                    err("BANK_CODE is not relevant to Transaction Type '" + ss + "'.");

                }

                if (acc_ref.IsNullOrEmpty() && nom_code.IsNullOrEmpty()) {
                    err("With Transaction Type '" + ss + "', fields ACCOUNT_REF and NOMINAL_CODE cannot both be empty.");

                }
                else {
                    if (acc_ref.IsNullOrEmpty()) {
                        // ****************************
                        /*       Assignment          */
                        // ****************************

                        acc_ref = nom_code;

                    }
                    else if (nom_code.IsNullOrEmpty()) {
                        // ****************************
                        /*       Assignment          */
                        // ****************************

                        nom_code = acc_ref;

                    }

                    if (acc_ref != nom_code) {
                        err(
"With Transaction Type '" + ss + "', fields ACCOUNT_REF and NOMINAL_CODE should either be equal, or one should be empty.");

                    }
                    else {
                        if (!context.Exists(Record_Type.Expense, nom_code)) {
                            err("Nominal Ledger Account '" + nom_code + "' does not exist.");

                        }
                    }

                }
            }
            else {
                if (acc_ref.IsNullOrEmpty()) {
                    err("With Transaction Type '" + ss + "', field ACCOUNT_REF should not be empty.");

                }
                if (nom_code.IsNullOrEmpty()) {
                    err("With Transaction Type '" + ss + "', field NOMINAL_CODE should not be empty.");
                }
            }

            // ****************************

            if (mode == Record_Type.Sales ||
                mode == Record_Type.Purchase) {

                if (!context.Exists(mode, acc_ref)) {
                    var msg = "";
                    if (mode == Record_Type.Sales)
                        msg = "Sales";
                    else if (mode == Record_Type.Purchase)
                        msg = "Purchase";
                    else
                        true.tift();

                    msg += " Ledger Account '";
                    msg += acc_ref + "' does not exist.";
                    err(msg);

                }

#warning wtf? compare this to the if(){} below
                if (type.Should_Be_Bank()) {

                    if (bank_code.IsNullOrEmpty()) {
                        // ****************************
                        /*       Assignment          */
                        // ****************************

                        bank_code = nom_code;

                    }
                    else if (bank_code != nom_code) {
                        err(
        "With Transaction Type '" + ss + "', BANK_CODE should either be the same as NOMINAL_CODE,"
        + " or empty, in order to be filled automatically.");
                    }
                }

            }

            if (mode == Record_Type.Sales ||
                mode == Record_Type.Purchase ||
                mode == Record_Type.Bank) {

                bool major;
                NominalType nom_type;

                if (!nom_code.IsNullOrEmpty()) {

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
                    else {
                        // "Nominal Ledger Account '" + nom_code + "' does not exist."
                        err("NOMINAL_CODE should be a valid Account entry in the Nominal Ledger.");

                    }
                }

            }


            // ****************************


            data[STR_ACCOUNT_REF] = acc_ref;
            data[STR_NOMINAL_CODE] = nom_code;
            data[STR_BANK_CODE] = bank_code;
            data[STR_PROJECT_REF] = proj;
            data[STR_COST_CODE] = cc;
            data[STR_TYPE] = type;

            errors = lst;

            return lst.Count == 0;

        }

        void Check_Project_Cost_Code(TransType type, Action<string> err, string proj, string cc) {

            string ss = type.Short_String();


            int ver = context.Version;

            if (!proj.IsNullOrEmpty()) {

                if (ver <= 11) {

                    // should be taken care of in General_Rules

                    //if (!proj.IsNullOrEmpty()) {
                    //    lst.Add(error);
                    //}
                    //if(!cc.IsNullOrEmpty()) {
                    //    lst.Add(error);
                    //}

                }
                else {

                    var allowed_at_all = type.Projects_Supported(null);

                    if (allowed_at_all) {
                        if (!type.Projects_Supported(ver)) {
                            err(
"Sage Line 50 Versions " + ver + " and earlier do not allow posting of '" + ss + "' Transactions against a project.");

                        }
                        else if (cc.IsNullOrEmpty() && type.CC_Required(ver)) {
                            err(
"'" + ss + CC_Required_Error);

                        }
                    }
                    else {
                        err(
"Sage Line 50 does not allow posting of '" + ss + "' Transactions against a Project.");

                    }

                }
            }

            if (!cc.IsNullOrEmpty()) {

                if (!type.CC_Allowed(ver)) {
                    err("Cost Codes are not applicable to Transaction Type '" + ss + "'.");
                }

                if (!context.Cost_Code_Exists(cc)) {
                    err("Cost Code '" + cc + "' is not valid.");
                }

                if (proj.IsNullOrEmpty()) {
                    err("Unable to provide a Cost Code without a Project Reference.");
                }

            }
        }





        readonly Sage_Context context;
        readonly bool check, change;





    }
}
