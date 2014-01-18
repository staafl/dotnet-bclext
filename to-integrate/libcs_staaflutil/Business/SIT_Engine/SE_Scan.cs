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
    partial class SIT_Engine
    {
        /// <summary>
        /// Prepares the program for scanning the csv file and hands control to the appropriate subroutine
        /// NOTE: here, instead of performing several loosely related operations, the routine could simply prepare
        /// a list of errors in the CSV and hand them over to the caller to deal with as required.
        /// That would be much more logical
        /// </summary>
        bool
        Scan(Sage_Context context,
            Sit_General_Settings sett_global,
            Sit_Company_Settings sett_company,
            Credentials creds,
            Record_Type mode,
            Dictionary<string, object> defaults,
            string file,
            string[] headers,
            IEnumerable<Line> lines,
            Action<string> report) {




            int errors = 0;


            if (Scan_Headers(headers, sett_company, context.Version, mode, report, ref errors)) {

                report("Inspecting records:");
                report("");

                /*       Scan records        */

                var rules = Get_Rules(sett_company, mode, context);

                int rec_ix = 0;

                Line_Data? null_line = null;
                Action tell = () =>
                {
                    if (null_line == null)
                        return;
                    Tell(true, mode, null, null_line.Value);
                };

                var ticker = new Ticker(TimeSpan.FromSeconds(0.25), tell);

                tell();
                foreach (var pair_pair in lines.Mark_Last()) {

                    var data = pair_pair.First.First;
                    var line = pair_pair.First.Second;
                    var is_last = pair_pair.Second;

                    null_line = line;
                    rec_ix = line.Record;

                    bool to_skip = false;
                    bool wrote_header = false;

                    // we don't care about the defaults when validating
                    //bool is_new = false;
                    //bool edit = (() => throw new InvalidOperationException())();
                    //if (edit && accounts[mode]) {
                    //    var record = 
                    //    record["ACCOUNT_REF"] = data.ACCOUNT_REF;
                    //    is_new = record.Find(false);
                    //}

                    // Maybe_Enter_Defaults(data, is_new, defaults);

                    ticker.Tick(is_last);

                    Action<bool> maybe_write_header = _write_rec =>
                    {
                        if (!H.Set(ref wrote_header, true)) {
                            var _str = "Line: " + line.User_Line;
                            if (_write_rec) {
                                _str = ("Record: " + line.User_Record + " " + _str);
                            }
                            report(_str);
                        }
                    };

                    var csv_error = line.Error;
                    if (csv_error != null) {

                        if (csv_error.Error_Type != Csv_Error_Type.Runaway_Quote) {
                            maybe_write_header(csv_error.Parsed);
                            report("Character on line: " + csv_error.Column);
                        }

                        report("CSV data appears to be malformed.");

                        ++errors;

                        report(csv_error.Description);

                        if (csv_error.Fatal) {
                            report("");
                            report("Unable to continue scanning.");
                        }
                        else {
                            if (!is_last) {
                                report("");
                                report("Attempting to move to next record...");
                            }
                        }

                        report("");

                        //    ret.Add("CSV structure error at line {0}.", line);
                        //    ret.Add("Invalid number of fields.");
                        //    ret.Add("The structure of the csv file appears corrupted,");
                        //    ret.Add("and other error messages might not be accurate.");
                        //    ret.Add("");

                        continue;

                    }

                    IEnumerable<Validation_Error> errors_on_line;

                    foreach (var rule in rules) {

                        if (rule.Inspect_Line(data, rec_ix, is_last, out to_skip, out errors_on_line))
                            continue;

                        maybe_write_header(true);

                        foreach (var error in errors_on_line) {
                            ++errors;
                            string desc;
                            if (error == null)
                                desc = "[null]";
                            else
                                desc = error.Description;

                            report(desc);

                        }

                        if (to_skip) {
                            report("Further scanning of record abandoned.");
                            break;
                        }
                    }

                    if (wrote_header)
                        report("");


                }


                tell();

            }
            else {
                report("Further scanning of file abandoned.");
                report("");

            }

            report("Total number of errors: " + errors);

            report("");




            return errors == 0;
        }


        bool
        Scan_Headers(
           string[] headers,
           Sit_Company_Settings sett_company,
           int ver,
           Record_Type mode,
           Action<string> report,
           ref int errors) {

            var fields = Common.Sage.Sage_Fields.Fields(mode);

            if (headers == null) {
                ++errors;
                report("Unable to read file headers - file is probably corrupted.");
                report("");

                return false;
            }
            else if (headers.Length == 0 || headers.All(_h => _h.IsNullOrEmpty())) {
                ++errors;
                report("No field headers detected.");
                report("");

                return false;
            }

            if (sett_company.Has_Headers == true) {
                report("Inspecting field headers:");
                report("");
                report(headers.Pretty_Print(_s => _s.Csv_Escape() + ", ").TrimEnd(',', ' '));
                report("");

            }


            var counter = new Counter<string>(headers, false);
            var dupes = new Set<string>();
            int start_errors = errors;

            foreach (var pair in counter.Ixed()) {
                var col = pair.First;
                var header = pair.Second.First ?? "";
                var cnt = pair.Second.Second;

                if (header.Trim().IsNullOrEmpty()) {
                    ++errors;
                    report("Empty field header (no. " + (col + 1) + ").");

                    continue;
                }

                if (!fields.ContainsKey(header)) {
                    if (mode == Record_Type.Audit_Trail &&
                            (header == "PROJECT_REF" ||
                             header == "COST_CODE")) {
                        // special exceptions
                    }
                    else {
                        ++errors;
                        report("Field Header '{0}' not accepted for this type of interface.".spf(header));
                        //ret.Add("Unrecognized field header: " + header);
                    }
                }


                if (cnt > 1) {
                    if (!dupes[header]) {
                        ++errors;
                        report("Duplicate field header: " + header);
                        dupes[header] = true;
                    }
                }

            }

            foreach (var header in Required_Fields(mode, sett_company)) {

                if (counter[header] <= 0) {
                    ++errors;
                    report("Mandatory field {0} not found in file headers.".spf(header));
                    //"Required field header not present: " + header);

                }

            }

            if (mode == Record_Type.Audit_Trail) {

                var proj = counter["PROJECT_REF"];
                var cc = counter["COST_CODE"];
                if (proj + cc > 0 && ver < 12) {
                    ++errors;
                    report("Field Headers PROJECT_REF and COST_CODE can only be used in Sage Version 12 and greater.");
                }

                if (cc > 0 && proj <= 0) {
                    ++errors;
                    report("Cannot enter cost codes for transactions if the CSV file does not include a Project Reference (PROJECT_REF) column.");
                }

                if (counter["INV_REF"] <= 0 && sett_company.Group_Transactions_Any == true) {
                    ++errors;
                    report("Cannot group Transactions into Headers for a CSV file that does not include a Reference (INV_REF) column.");
                    report("Either switch off the parameter 'Group Transactions into Headers', or include the Reference (INV_REF) column in the CSV file.");
                }


            }

            if (errors > start_errors)
                report("");

            return errors == 0;
        }




        IEnumerable<IRule>
        Get_Rules(Sit_Company_Settings sett_company, Record_Type mode, Sage_Context context) {

            var ret = new List<IRule>();


            foreach (var field in Required_Fields(mode, sett_company))
                ret.Add(new Required_Field_Rule(field));

            if (mode == Record_Type.Expense)
                ret.Add(new Required_Field_Rule("ACCOUNT_TYPE", false));

            ret.Add(new General_Rules(mode));

            // switched from allowed to forbidden for the sake of unicode
            // characters

            // const string allowed = "-.'\"!#$%&()_/A-Za-z0-9";
            const string forbidden = "~`@^*?<>,\\|{}[]=+;:";

            if (mode == Record_Type.Sales ||
                mode == Record_Type.Purchase) {

                ret.Add(new Def_Nom_Code_Rule(context));


            }
            if (accounts[mode] ||
                docs_modes[mode] ||
                mode == Record_Type.Audit_Trail) {
                ret.Add(new Forbidden_Chars_Rule("ACCOUNT_REF", forbidden.arr()));

            }

            if (docs_modes[mode]) {
                ret.Add(new Invoice_General_Rules(context, sett_company, mode));
                ret.Add(new Header_Fields_Same(Items_Same_Document(sett_company), Get_Fields_For_Type(Record_Type.Invoice), "document"));
            }


            if (mode == Record_Type.Stock ||
                mode == Record_Type.Stock_Tran) {
                ret.Add(new Forbidden_Chars_Rule("STOCK_CODE", forbidden.arr()));

            }

            if (mode == Record_Type.Audit_Trail) {

                ret.Add(Get_TT_Rules(context, sett_company));
                ret.Add(new Header_Fields_Same((_dl1, _dl2) => _dl1.Same_Header(_dl2), Get_Fields_For_Type(Record_Type.Header_Data), "header"));


            }

            if (mode == Record_Type.Audit_Trail ||
                mode == Record_Type.Stock_Tran) {

                ret.Add(new Has_Amount());

            }


            return ret;

        }


        IRule
        Get_TT_Rules(Sage_Context context, Sit_Company_Settings sett_company) {
            return new TT_Rules(context, sett_company.Check_Type == true, sett_company.Change_Type == true);
        }

        // e09293e9-2cf9-4922-9e06-52c4227b167b

        IEnumerable<string>
        Required_Fields(Record_Type mode, Sit_Company_Settings sett_company) {

            if (accounts[mode]) {
                return new[] { "ACCOUNT_REF" };
            }
            if (docs_modes[mode]) {
                if (sett_company.Documents_Numbering_Auto == true &&
                    sett_company.Documents_Grouping_Relative == false)
                    return new[] { "ACCOUNT_REF", "INVOICE_DATE", "INVOICE_TYPE_CODE", "STOCK_CODE" };

                return new[] { "ACCOUNT_REF", "INVOICE_DATE", "INVOICE_TYPE_CODE", "STOCK_CODE", "INVOICE_NUMBER" };
            }
            if (mode == Record_Type.Stock) {
                return new[] { "STOCK_CODE" };
            }
            if (mode == Record_Type.Audit_Trail) {
                return new[] { "TYPE", "DATE", 
                               /*"ACCOUNT_REF",*/ 
                               /*"NOMINAL_CODE",*/ // this one is handled specially
                               "NET_AMOUNT", "TAX_CODE"  /*"TAX_AMOUNT",*/ /*"INV_REF",*/ };
            }
            if (mode == Record_Type.Stock_Tran) {
                return new[] { "TYPE", "DATE", "STOCK_CODE", "QUANTITY", "NET_AMOUNT", "TAX_CODE"/*"TAX_AMOUNT",*/ };

            }



            return new string[0];
        }

        // f7229d3a-1204-4592-af15-8e0088e066ad

        /* Set<string>
         Allowed_Headers(Record_Type mode) {

             var dict = new Dictionary<Record_Type, string[]>
             {
                 {Record_Type.Sales,sales_headers},
                 {Record_Type.Purchase,purchase_headers},
                 {Record_Type.Expense,expense_headers},
                 {Record_Type.Bank,bank_headers},
                 {Record_Type.Stock,stock_headers},
                 {Record_Type.Document,document_headers},
             };

             string[] allowed_headers;
             dict.TryGetValue(mode, out allowed_headers);

             var ret = new Set<string>(allowed_headers);

             return ret;
         }*/





    }
}