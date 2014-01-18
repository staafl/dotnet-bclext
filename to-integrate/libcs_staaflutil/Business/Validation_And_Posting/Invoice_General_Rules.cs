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
    public class Invoice_General_Rules : Rule_Base
    {
        public Invoice_General_Rules(Sage_Context context, Sage_Int.Sit_Company_Settings settings, Record_Type mode) {
            this.context = context;

            this.auto = settings.Documents_Numbering_Auto == true;
            this.relative = settings.Documents_Grouping_Relative == true;
            this.absolute = !auto;
            this.mode = mode;

            (relative && !auto).tift();
            (auto || absolute).tiff();

        }

        public override void Clear() {
            base.Clear();
            seen_invoice_numbers.Clear();
            // no need for this - context.Clear();
        }

        static Set<InvoiceType>
        supported_invoice_types = new Set<InvoiceType>
        {
            InvoiceType.sdoProductInvoice,
            InvoiceType.sdoProductCredit,
        };

        int? current_invoice_number;
        Set<int> seen_invoice_numbers = new Set<int>();

        readonly Sage_Context context;
        readonly bool auto, relative, absolute;
        readonly Record_Type mode;

        public override bool
        Inspect_Line(Data_Line line,
                     int ix,
                     bool is_last,
                     out bool skip,
                     out IEnumerable<Validation_Error> errors) {

            H.assign(out skip, out errors);

            InvoiceType type = 0;

            try {
                type = line.INVOICE_TYPE;
            }
            catch (InvalidCastException) {
            }

            var lst = new List<Validation_Error>();
            Action<string> err = _s =>
            {
                var _error = new Validation_Error(ix, _s);
                lst.Add(_error);

            };

            var str = type.String();
            if (!supported_invoice_types[type]) {

                err("Document Type '" + str + "' not supported.");

            }

            var acc_ref = line.ACCOUNT_REF.Clean();
            var prod_code = line.STOCK_CODE.Clean();

            if (!context.Exists(Record_Type.Sales, acc_ref)) {

                var msg = "Sales Ledger Account '" + acc_ref + "' does not exist.";
                err(msg);

            }

            if (!context.Exists(Record_Type.Stock, prod_code)) {

                var msg = "Product Entry '" + prod_code + "' does not exist.";
                err(msg);

            }

            if (line.Contains_Nonempty("BANK_CODE")) {
                if (!context.Exists(Record_Type.Bank, line.BANK_CODE)) {

                    var msg = "Bank Ledger Account '" + acc_ref + "' does not exist.";
                    err(msg);

                }
            }

            bool has_inv_number = line.Contains_Nonempty("INVOICE_NUMBER");

            if (auto && !relative) {
                if (has_inv_number) {
                    var msg = "When using the Documents Auto Grouping option, field INVOICE_NUMBER must NOT be present.";
                    err(msg);
                }
            }

            if (absolute || relative) {
                if (has_inv_number) {

                    var invoice_number = line.INVOICE_NUMBER;

                    if (invoice_number <= 0) {
                        var msg = "Invalid Invoice Number: '" + invoice_number + "'.";
                        err(msg);
                    }
                    if (absolute) {
                        if (context.Exists(Record_Type.Document, invoice_number)) {
                            var msg = "Document Entry with number '" + invoice_number + "' already exists.";
                            err(msg);
                        }
                    }

                    if (seen_invoice_numbers[invoice_number] && current_invoice_number != invoice_number) {
                        var msg = "Invoice Number '" + invoice_number + "' found on non-consecutive lines.";
                        err(msg);
                    }

                    current_invoice_number = invoice_number;
                    seen_invoice_numbers[invoice_number] = true;
                }
                else {
                    var msg = "Unless using the Documents Auto Grouping option, field INVOICE_NUMBER must be present.";
                    err(msg);
                }

            }

            if (line.Contains_Nonempty("PAYMENT_TYPE")) {

                TransType payment_type = 0;
                try {
                    payment_type = line.PAYMENT_TYPE;
                }
                catch (InvalidCastException) {
                }

                if (payment_type != TransType.sdoSR &&
                    payment_type != TransType.sdoSA) {
                    var msg = "Invalid Payment Type: '" + payment_type.Short_String() + "'.";
                    err(msg);
                }

                line["PAYMENT_TYPE"] = payment_type;
            }

            errors = lst;

            return lst.Count == 0;
        }
    }
}
