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
#warning modifies the data
    /// <summary>
    /// * type correctness
    /// * maximum string length
    /// </summary>
    public class General_Rules : Rule_Base
    {
        readonly Record_Type type;
        readonly Dict_ro<string, Sage_Field> dict;
        public General_Rules(Record_Type type) {
            this.type = type;
            this.dict = Common.Sage.Sage_Fields.Fields(type);
        }

        public override bool
        Inspect_Line(Data_Line line,
                     int ix,
                     bool is_last,

                     out bool to_skip,
                     out IEnumerable<Validation_Error> errors) {

            to_skip = false;

            //var to_change = new List<Pair<string, object>>(line.Dict.Count);

            var errors_lst = new List<Validation_Error>(3 + line.Dict.Count);

            foreach (var kvp in line.Dict.lst()) {

                var key = kvp.Key;
                var value = kvp.Value;

                Sage_Field field;
                if (!dict.TryGetValue(key, out field)) {
                    // not our problem
                    continue;
                }

                if (value.IsNullOrEmpty()) {
                    // not our problem
                    continue;
                }

                var name = field.Name;
                var type = field.Type;

                if (type == TypeCode.String) {

                    var as_str = value.strdef();

                    if (name == "ACCOUNT_REF" ||
                        name == "STOCK_CODE")
                        as_str = as_str.ToUpper();

                    var len = as_str.Length;

                    var max_len = field.Length;
                    if (len > max_len) {
                        var error = new Validation_Error(ix,
                            "Field '" + key + "' should be " + max_len + " characters or shorter.");
                        errors_lst.Add(error);

                    }

                    if (!(value is string))
                        line[key] = as_str;

                }
                else {
                    string error_msg;
                    object coerced;
                    if (Check_Type(value, field, out error_msg, out coerced)) {
                        line[key] = coerced;
                    }
                    else {
                        var error = new Validation_Error(ix, error_msg);
                        errors_lst.Add(error);
                        to_skip = true;
                    }
                }
            }
            errors = errors_lst;
            return errors_lst.Count == 0;

        }

        static bool
        Known_Flag(string name) {
            switch (name) {

                case "ELEC_TRANS":
                case "STATUS":
                case "BACS":
                case "BANK_FLAG":
                case "DELETED_FLAG":
                case "IGNORE_STK_LVL_FLAG":
                case "NO_RECONCILIATION_FLAG":
                case "PAID_FLAG":
                case "POSTED_CODE":
                case "PRINTED_CODE":
                case "QUICK_RATIO_FLAG":
                case "SEND_VIA_EMAIL":
                case "SERVICE_FLAG":
                case "TAX_FLAG":
                case "TERMS_AGREED_FLAG":
                case "VAT_FLAG":
                case "WEB_PUBLISH":
                    //case "ELEC_TRANS": // present in version 11!
                    //case "FINANCE_CHARGE":
                    //case "RECORD_DELETED":
                    return true;
            }
            return false;


            #region grepped
            /*grep flag -i * | grep Byte
                BankRecord11.txt:NO_RECONCILIATION_FLAG Ignore Reconciliation 1 Optional Byte 
HeaderData11.txt:BANK_FLAG Bank Reconciled Flag 1 Optional Byte 
HeaderData11.txt:FINANCE_CHARGE Finance Charge Flag 1 Optional Byte 
HeaderData11.txt:PAID_FLAG Transaction Paid Flag 1 Optional Byte 
HeaderData11.txt:PRINTED_FLAG Printed Flag 1 Optional Byte 
HeaderData11.txt:VAT_FLAG Vat Flag 1 Optional Byte 
InvoiceData.txt:POSTED_CODE Posted Flag - Yes/No 1 Optional Byte 
InvoiceData.txt:PRINTED_CODE Printed Flag - Yes/No 1 Optional Byte 
invoiceitem.txt:SERVICE_FLAG Service Item Flag 1 Optional Byte 
invoiceitem.txt:TAX_FLAG  1 Optional Byte 
InvoicePost11.txt:POSTED_CODE Posted Flag - Yes/No 1 Optional Byte 
InvoicePost11.txt:PRINTED_CODE Printed Flag - Yes/No 1 Optional Byte 
InvoiceRecord11.txt:POSTED_CODE Posted Flag - Yes/No 1 Optional Byte 
InvoiceRecord11.txt:PRINTED_CODE Printed Flag - Yes/No 1 Optional Byte 
SplitData11.txt:BANK_FLAG Bank Reconciled Flag 1 Optional Byte 
SplitData11.txt:PAID_FLAG Paid Flag 1 Optional Byte 
SplitData11.txt:VAT_FLAG Vat Reconciled Flag 1 Optional Byte 
StockRecord.txt:IGNORE_STK_LVL_FLAG Ignore Stock Levels Flag 1 Optional Byte 
TransactionPost.txt:BANK_FLAG Bank Reconciled Flag 1 Optional Byte 
TransactionPost.txt:FINANCE_CHARGE Finance Charge Flag 1 Optional Byte 
TransactionPost.txt:PAID_FLAG Transaction Paid Flag 1 Optional Byte 
TransactionPost.txt:PRINTED_FLAG Printed Flag 1 Optional Byte 
TransactionPost.txt:VAT_FLAG Vat Flag 1 Optional Byte 
*/
            // ****************************

            /*grep flag -i *
             * BankRecord11.txt:DELETED_FLAG Is Deleted 2 Optional Small Integer 
BankRecord11.txt:NO_RECONCILIATION_FLAG Ignore Reconciliation 1 Optional Byte 
HeaderData11.txt:BANK_FLAG Bank Reconciled Flag 1 Optional Byte 
HeaderData11.txt:DELETED_FLAG Transaction Deleted Flag 2 Optional Small Integer 
HeaderData11.txt:ELEC_TRANS Electronic Transaction Flag 2 Optional Small Integer 
HeaderData11.txt:FINANCE_CHARGE Finance Charge Flag 1 Optional Byte 
HeaderData11.txt:PAID_FLAG Transaction Paid Flag 1 Optional Byte 
HeaderData11.txt:PRINTED_FLAG Printed Flag 1 Optional Byte 
HeaderData11.txt:VAT_FLAG Vat Flag 1 Optional Byte 
InvoiceData.txt:DELETED_FLAG Is Deleted 2 Optional Small Integer 
InvoiceData.txt:POSTED_CODE Posted Flag - Yes/No 1 Optional Byte 
InvoiceData.txt:PRINTED_CODE Printed Flag - Yes/No 1 Optional Byte 
invoiceitem.txt:SERVICE_FLAG Service Item Flag 1 Optional Byte 
invoiceitem.txt:TAX_FLAG  1 Optional Byte 
InvoicePost11.txt:DELETED_FLAG Is Deleted 2 Optional Small Integer 
InvoicePost11.txt:POSTED_CODE Posted Flag - Yes/No 1 Optional Byte 
InvoicePost11.txt:PRINTED_CODE Printed Flag - Yes/No 1 Optional Byte 
InvoiceRecord11.txt:DELETED_FLAG Is Deleted 2 Optional Small Integer 
InvoiceRecord11.txt:POSTED_CODE Posted Flag - Yes/No 1 Optional Byte 
InvoiceRecord11.txt:PRINTED_CODE Printed Flag - Yes/No 1 Optional Byte 
SalesData.txt:BACS Account Flagged for BACS 2 Optional Small Integer 
SalesData.txt:DELETED_FLAG Reserved for Future Use 2 Optional Small Integer 
SalesData.txt:TERMS_AGREED_FLAG Terms Agreed Flag 2 Optional Small Integer 
SalesRecord.txt:BACS Account Flagged for BACS 2 Optional Small Integer 
SalesRecord.txt:DELETED_FLAG Reserved for Future Use 2 Optional Small Integer 
SalesRecord.txt:TERMS_AGREED_FLAG Terms Agreed Flag 2 Optional Small Integer 
SplitData11.txt:BANK_FLAG Bank Reconciled Flag 1 Optional Byte 
SplitData11.txt:DELETED_FLAG Transaction Deleted Flag 2 Optional Small Integer 
SplitData11.txt:PAID_FLAG Paid Flag 1 Optional Byte 
SplitData11.txt:STATUS Status Flag 2 Optional Small Integer 
SplitData11.txt:VAT_FLAG Vat Reconciled Flag 1 Optional Byte 
StockRecord.txt:DELETED_FLAG Reserved for Future Use 2 Optional Small Integer 
StockRecord.txt:IGNORE_STK_LVL_FLAG Ignore Stock Levels Flag 1 Optional Byte 
StockRecord.txt:WEB_PUBLISH Web Publish Flag 2 Optional Small Integer 
TransactionPost.txt:BANK_FLAG Bank Reconciled Flag 1 Optional Byte 
TransactionPost.txt:DELETED_FLAG Transaction Deleted Flag 2 Optional Small Integer 
TransactionPost.txt:ELEC_TRANS Electronic Transaction Flag 2 Optional Small Integer 
TransactionPost.txt:FINANCE_CHARGE Finance Charge Flag 1 Optional Byte 
TransactionPost.txt:PAID_FLAG Transaction Paid Flag 1 Optional Byte 
TransactionPost.txt:PRINTED_FLAG Printed Flag 1 Optional Byte 
TransactionPost.txt:VAT_FLAG Vat Flag 1 Optional Byte 
             * */
            // ****************************

            //BankRecord11.txt:DELETED_FLAG Is Deleted 2 Optional Small Integer 
            //HeaderData11.txt:DELETED_FLAG Transaction Deleted Flag 2 Optional Small Integer 
            //HeaderData11.txt:ELEC_TRANS Electronic Transaction Flag 2 Optional Small Integer 
            //InvoiceData.txt:DELETED_FLAG Is Deleted 2 Optional Small Integer 
            //InvoicePost11.txt:DELETED_FLAG Is Deleted 2 Optional Small Integer 
            //InvoiceRecord11.txt:DELETED_FLAG Is Deleted 2 Optional Small Integer 
            //SalesData.txt:BACS Account Flagged for BACS 2 Optional Small Integer 
            //SalesData.txt:DELETED_FLAG Reserved for Future Use 2 Optional Small Integer 
            //SalesData.txt:TERMS_AGREED_FLAG Terms Agreed Flag 2 Optional Small Integer 
            //SalesRecord.txt:BACS Account Flagged for BACS 2 Optional Small Integer 
            //SalesRecord.txt:DELETED_FLAG Reserved for Future Use 2 Optional Small Integer 
            //SalesRecord.txt:TERMS_AGREED_FLAG Terms Agreed Flag 2 Optional Small Integer 
            //SplitData11.txt:DELETED_FLAG Transaction Deleted Flag 2 Optional Small Integer 
            //SplitData11.txt:STATUS Status Flag 2 Optional Small Integer 
            //StockRecord.txt:DELETED_FLAG Reserved for Future Use 2 Optional Small Integer 
            //StockRecord.txt:WEB_PUBLISH Web Publish Flag 2 Optional Small Integer 
            //TransactionPost.txt:DELETED_FLAG Transaction Deleted Flag 2 Optional Small Integer 
            //TransactionPost.txt:ELEC_TRANS Electronic Transaction Flag 2 Optional Small Integer 

            #endregion
        }
        // ****************************

        bool
        Check_Type(object value,
                   Sage_Field field,
                   out string error_msg, out object coerced) {

            coerced = value;
            error_msg = null;

            var description = "";
            var ret = false;

            var field_name = field.Name;
            var field_type = field.Type;

            bool is_enum = false;

            if (field_type == TypeCode.SByte ||
                (is_enum = Known_Flag(field_name))) { // flags ARE enums

                Triple<Type, string, IDictionary> triple;

                if (is_enum)
                    triple = yes_no;
                else
                    is_enum = enum_data.TryGetValue(Pair.Make(type, field_name), out triple);

                if (is_enum) {
                    description = triple.Second;
                    ret = Check_Sbyte_Enum_Value(
                        value,
                        triple.First,
                        triple.Third,
                        out coerced);
                }
                else {

                    ret = Check_Integer(value, 1, out description, out coerced);

                }
            }
            else if (field_name.Contains("TAX_CODE")) {
                description = "Tax Code";
                ret = Check_Tax_Code(value, out coerced);

            }
            else {
                switch (field_type) {
                    case TypeCode.Int16:
                        ret = Check_Integer(value, 2, out description, out coerced);
                        break;
                    case TypeCode.Int32:
                        ret = Check_Integer(value, 4, out description, out coerced);
                        break;
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        ret = Check_Decimal(value, field, out description, out coerced);
                        break;
                    case TypeCode.DateTime:
                        ret = Check_Date_Time(value, out description, out coerced);
                        break;
                    default:
                        error_msg = "Field '" + field_name + "' is of unrecognized type: " + field_type + ".";
                        return false;

                }
            }

            if (!ret)
                error_msg = "Field '" + field_name + "' should be a(n) " + description + ".";


            return ret;
        }

        static bool
        Check_Sbyte_Enum_Value(
            object value,
            Type enum_type,
            IDictionary synonyms,
            out object coerced) {

            coerced = value;

            var as_str = value.strdef().ToUpper();

            if (as_str.IsNullOrEmpty())
                return true;

            int as_int;
            bool is_int;
            if (synonyms.Contains(as_str)) {
                as_int = synonyms[as_str].ToInt32();
                is_int = true;
            }
            //else if (enum_strs[enum_type].TryGetValue(as_str, out as_int)) {
            //    is_int = true;
            //}
            else {
                is_int = int.TryParse(as_str, out as_int) &&
                         enum_vals[enum_type][as_int];
            }

            if (is_int)
                coerced = (sbyte)as_int;

            //if (!is_int)
            //    Debugger.Break();

            return is_int;

        }

        static bool
        Check_Date_Time(
            object value,
            out string description,
            out object coerced) {

            description = null;
            coerced = value;

            if (value is DateTime)
                return true;

            var as_str = value.strdef().ToUpper();
            if (as_str.IsNullOrEmpty())
                return true;

            const string STR_DdMMyyyy = "dd/MM/yyyy";
            const string STR_DdMyyyy = "dd/M/yyyy";
            const string STR_DMyyyy = "d/M/yyyy";
            const string STR_DMMyyyy = "d/MM/yyyy";


            try {
                /* interesting... 
                 * If format is a custom format pattern that does not include date or 
                 * time separators (such as "yyyyMMdd HHmm"), use the invariant culture 
                 * for the provider  parameter and the widest form of each custom format 
                 * specifier. For example, if you want to specify hours in the format 
                 * pattern, specify the wider form, "HH", instead of the narrower form, 
                 * "H".
                 **/
                var as_date_time = DateTime.ParseExact(
                    as_str.Trim(),
                    new[] { STR_DdMMyyyy, STR_DdMyyyy, STR_DMyyyy, STR_DMMyyyy },
                    culture,
                    DateTimeStyles.AllowWhiteSpaces);

                coerced = as_date_time;
                return true;
            }
            catch (FormatException) {
                description = "Date in " + STR_DdMMyyyy + " format";
                return false;
            }


        }


        static bool
        Check_Decimal(
            object value,
            Sage_Field field,
            out string description,
            out object coerced) {

            description = null;
            coerced = value;

            var as_str = value.strdef().ToUpper();
            if (as_str.IsNullOrEmpty())
                return true;

            decimal as_dec;

            var name = field.Name;
            var prec = (//name == "FOREIGN_RATE" ||
                        name.Contains("RATE") ||
                        name == "QTY" ||
                        name == "SALES_PRICE") ? 6 : 2;

            bool cast = false;
            if (value is decimal) {
                as_dec = (decimal)value;
                cast = true;
            }
            else if (decimal.TryParse(as_str.Trim(), out as_dec)) {
                cast = true;
            }

            if (cast && as_dec == as_dec.RoundDown(prec) && as_dec >= 0.00m) {
                coerced = as_dec;
                return true;
            }

            description = "valid positive number with precision no more than " + prec + " digits";
            if (as_dec < 0.00m)
                description = "non-negative " + description;

            return false;

        }

        static bool
        Check_Integer(
            object value,
            int bytes,
            out string description,
            out object coerced) {

            description = null;
            coerced = value;
            var as_str = value.strdef();
            if (as_str.IsNullOrEmpty())
                return true;


            int as_int;
            long max = ((long)1) << (8 * bytes);
            if (int.TryParse(as_str, out as_int) &&
                as_int <= max)
                return true;

            description = "Integer less than " + max;
            return false;

        }

        static bool
        Check_Tax_Code(
            object value,
            out object coerced) {

            coerced = value;

            var as_str = value.strdef();

            if (as_str.IsNullOrEmpty())
                return true;

            if (as_str[0].ToUpper() == 'T')
                as_str = as_str.Substring(1);


            if (as_str.Length == 0 || as_str.Length > 2)
                return false;

            const int ch0 = '0', ch9 = '9';

            foreach (var ch in as_str) {
                if (ch < ch0 || ch > ch9)
                    return false;

            }

            coerced = ushort.Parse(as_str);

            return true;
        }


        // ****************************

        static readonly System.Globalization.CultureInfo
        culture = null;

        // ****************************
        /*       Enumerations        */
        // ****************************

        static readonly Lazy_Dict<Type, Set<int>>
        enum_vals = new Lazy_Dict<Type, Set<int>>(_t => new Set<int>(from _v in Enum.GetValues(_t).Cast<Enum>()
                                                                     select _v.ToInt32()));

        static readonly Lazy_Dict<Type, Dictionary<string, int>>
        enum_strs = new Lazy_Dict<Type, Dictionary<string, int>>(
            _t => //new Dictionary<string, int>
                (
                from _value in Enum.GetValues(_t).Cast<object>()
                let _name = _value.ToString().ToUpper()
                //from _name in Enum.GetNames(_t)
                //let _value = Enum.Parse(_t, _name, true).ToInt32()
                select new KeyValuePair<string, int>(_name, _value.ToInt32())
                ).ToDictionary(_kvp => _kvp.Key, _kvp => _kvp.Value)
        );

        // synonyms

        static readonly Dictionary<string, TransType>
        trans_types = Versioning.Service.tt_to_short_str.ToDictionary(_p => _p.Second, _p => _p.First);

        static readonly Dictionary<string, StockTransType>
        stock_trans_types = Versioning.Service.stt_to_short_str.ToDictionary(_p => _p.Second, _p => _p.First);

        // 588f3694-1ec4-48bb-88e5-41c80250dc9e

        static readonly Dictionary<string, InvoiceType>
        invoice_types = new Dictionary<string, InvoiceType>
        {
            {"INV", InvoiceType.sdoProductInvoice},
            {"CRD", InvoiceType.sdoProductCredit},
        };

        static readonly Dictionary<string, ItemType>
        stock_item_types = new Dictionary<string, ItemType>
		        {
		            {"STOCK", ItemType.sdoStockItem},
		            {"NONSTOCK", ItemType.sdoNonStockItem},
		            {"SERVICE", ItemType.sdoServiceItem},
		        };
        static readonly Dictionary<string, BankType>
        bank_types = new Dictionary<string, BankType>
		        {
		            {"CHEQUE", BankType.sdoTypeCheque},
		            {"CASH", BankType.sdoTypeCash},

                    // ****************************

		            {"CREDIT", BankType.sdoTypeCredit},
		            {"CREDIT CARD", BankType.sdoTypeCredit},
		            {"CARD", BankType.sdoTypeCredit},
		            // {"VISA", BankType.sdoTypeCredit},
                    
		        };

        static readonly Dictionary<string, TransType>
        payment_types = new Dictionary<string, TransType>
                         {
                     {"SR", TransType.sdoSR},
                     {"SA", TransType.sdoSA},
                         };

        static readonly Dictionary<string, NominalType>
        nominal_types = new Dictionary<string, NominalType>
		        {
#warning is the user allowed to specify this?
		            {"NORMAL", NominalType.sdoTypeNormal},
		            {"BANK", NominalType.sdoTypeBank},
		            {"CONTROL", NominalType.sdoTypeControl},
		        };

        static Triple<Type, string, IDictionary>
        yes_no = Triple.Make(typeof(YesNo),
                             "Yes/No, True/False or 1/0 value",
                             (IDictionary)new Dictionary<string, YesNo>
                             {
                                 {"YES", YesNo.Yes},
                                 {"TRUE", YesNo.Yes},

                                 {"NO", YesNo.No},
                                 {"FALSE", YesNo.No},

                             });

        static Pair_Key_Dict<Record_Type, string, Triple<Type, string, IDictionary>>
        enum_data = new Pair_Key_Dict<Record_Type, string, Triple<Type, string, IDictionary>>
		        {
		            {Record_Type.Stock,"ITEM_TYPE", 
		            Triple.Make(
typeof(ItemType),
		            "valid Stock Item Type for a Stock Record",
		            (IDictionary)stock_item_types)},

		            {Record_Type.Bank,"ACCOUNT_TYPE", 
		            Triple.Make(
typeof(BankType),
		            "valid Account Type for a Bank Record",
		            (IDictionary)bank_types)},

		            {Record_Type.Expense,"ACCOUNT_TYPE", 
		            Triple.Make(
typeof(NominalType),
		            "valid Account Type for a Nominal Record",
		            (IDictionary)nominal_types)},

                    {Record_Type.Audit_Trail, "TYPE",
                    Triple.Make(
typeof(TransType),
                    "valid Type for an Audit Trail Transaction",
                     (IDictionary)trans_types)},

                    {Record_Type.Stock_Tran, "TYPE",
                    Triple.Make(
typeof(StockTransType),
                    "valid Type for a Stock Transaction",
                     (IDictionary)stock_trans_types)},

                    {Record_Type.Audit_Trail, "STATUS",
                     yes_no},


                    {Record_Type.Document, "INVOICE_TYPE_CODE", 
                    Triple.Make(typeof(InvoiceType),
                    "valid Document Type", 
                    (IDictionary)invoice_types)},

                    {Record_Type.Document, "PAYMENT_TYPE",
                    Triple.Make(typeof(TransType),
                    "valid Invoice Payment Type",
                     (IDictionary)payment_types)},
                     
                     // HACK!!!!
                    {Record_Type.Invoice_Or_Credit, "INVOICE_TYPE_CODE", 
                    Triple.Make(typeof(InvoiceType),
                    "valid Document Type", 
                    (IDictionary)invoice_types)},

                    {Record_Type.Invoice_Or_Credit, "PAYMENT_TYPE",
                    Triple.Make(typeof(TransType),
                    "valid Invoice Payment Type",
                     (IDictionary)payment_types)},

		        };


        enum YesNo
        {
            No = 0x0,
            Yes = 0x1,
        }








    }
}
