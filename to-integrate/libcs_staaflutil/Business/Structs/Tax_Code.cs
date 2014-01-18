using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fairweather.Service;

namespace Common
{
    [Serializable]
    [DebuggerStepThrough]
    public struct Tax_Code
    {
        public Tax_Code(short tax_code,
                        decimal vat_percentage,
                        bool throw_on_conflict)
            : this() {

            Assert_Not_Null();

            Assert_Valid(tax_code);
            Assert_Valid(vat_percentage);

            if (throw_on_conflict)
                Get_And_Throw(tax_code, vat_percentage);

            Tax_Code_ID = tax_code;
            st_data[tax_code] = vat_percentage;

        }

        public Tax_Code(short tax_code, decimal vat_ratio)
            : this(tax_code, vat_ratio, true) {
        }

        public Tax_Code(short tax_code)
            : this() {

            Assert_Not_Null();

            Get_Or_Throw(tax_code);

            Assert_Valid(tax_code);

            this.Tax_Code_ID = tax_code;

        }


        public short Tax_Code_ID {
            get;
            set;
        }

        /// <summary>
        /// The VAT percentage applicable to this item expressed as a fraction E [0.0m, 100.0m]
        /// </summary>
        public decimal VAT_Percentage {
            get {
                Assert_Not_Null();

                decimal ret;

                Get_Or_Throw(Tax_Code_ID, out ret);

                return ret;

            }
        }

        /// <summary>
        /// The VAT percentage applicable to this item expressed as a fraction E [0.0m, 1.0m]
        /// </summary>
        public decimal VAT_Ratio {
            get {

                return VAT_Percentage / 100.0m;

            }
        }

        public string Index_String {
            get {

                return Get_Index_String(Tax_Code_ID);

            }
        }

        public string Combobox_String {
            get {

                return Get_Combobox_String(this);

            }
        }



        // ****************************
        /*       Static methods      */
        // ****************************


        public static Tax_Code[] Instances {
            get {
                Assert_Not_Null();

                var ret = new Tax_Code[st_data.Count];

                int ii = 0;
                foreach (var key in st_data.Keys) {
                    ret[ii] = new Tax_Code(key);
                    ++ii;
                }

                return ret;
            }
        }

        public static string Get_Index_String(int tax_code) {
            string ret = "T" + tax_code.ToString();

            return ret;

        }

        public static string Get_Combobox_String(Tax_Code tax_code) {

            var ret = "T{0} - {1}%".spf(tax_code.Tax_Code_ID, tax_code.VAT_Percentage);

            return ret;

        }

        /// <summary>
        /// Sage's tax codes range from 0 to 99
        /// </summary>
        const int tax_code_cnt = 100;

        public static int Tax_Code_Count {
            get { return tax_code_cnt; }
        }


        const string cst_null_exception_msg = 
@"""Tax_Code.Data"" needs to be set up before using this struct (did you forget to call Initialize or FillTaxCodes?)";
        const string cst_unknown_code_msg = "Unrecognized tax code: ";
        const string cst_code_conflict_msg = "Tax code conflict: ";
        const string cst_vat_percentage_msg = "Invalid VAT percentage: ";
        const string cst_multiple_errors_msg = "The following entries have invalid tax codes or vat percentages: ";
        const string cst_invalid_code_msg = "Invalid tax code: ";

        /// Access only for demonstration purposes
        internal static SortedList<short, decimal> st_data;

        public static bool Initialize() {
            if (st_data != null)
                return false;

            st_data = new SortedList<short, decimal>(tax_code_cnt);
            return true;
        }

        static void Assert_Not_Null() {
            if (st_data == null)
                true.tift(cst_null_exception_msg);
        }

        static bool Is_Valid(decimal percentage) {

            bool ret = (percentage <= 100.0m && percentage >= 0.0m);
            return ret;
        }

        static bool Is_Valid(short tax_code) {

            /// Sage's tax codes range from 0 to 99
            bool ret = (tax_code < tax_code_cnt && tax_code >= 0);
            return ret;
        }

        static void Assert_Valid(short tax_code) {

            if (Is_Valid(tax_code))
                return;

            true.tift(cst_invalid_code_msg + tax_code.ToString());

        }

        static void Assert_Valid(decimal percentage) {

            if (percentage <= 100.0m && percentage >= 0.0m)
                return;

            true.tift(cst_vat_percentage_msg);

        }

        static void Get_And_Throw(short tax_code, decimal expected) {

            decimal actual;

            if (!st_data.TryGetValue(tax_code, out actual))
                return;

            if (actual == expected)
                return;

            true.tift(cst_code_conflict_msg + tax_code.ToString());

        }

        static void Get_Or_Throw(short tax_code) {

            decimal _;
            Get_Or_Throw(tax_code, out _);

        }

        static void Get_Or_Throw(short tax_code, out decimal ratio) {

            if (!st_data.TryGetValue(tax_code, out ratio)) {
                true.tift(cst_unknown_code_msg + tax_code.ToString());
            }

        }

 

    }


}