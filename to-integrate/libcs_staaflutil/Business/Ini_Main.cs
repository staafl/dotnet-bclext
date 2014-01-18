namespace DTA
{
    using System;
    using System.IO;

    using Common;
    using Fairweather.Service;

    public static partial class Ini_Main
    {
        public const string PURCHASES_ONLY = "PURCHASES_ONLY";

        public const string NUMBER = "NUMBER";
        public const string ALL = "ALL";

        public const string SAME_POS = "SAME_POS";
        public const string ANY_POS = "ANY_POS";
        public const string ANY = "ANY";

        public const string CASH = "CASH";
        public const string SUPER = "SUPER";
        public const string NONE = "NONE";
        public const string QTY = "QTY";
        public const string AMOUNT = "AMOUNT";

        public const string NO = "0";
        public const string YES = "1";

        public const string NVAT = "NVAT";
        public const string VAT = "VAT";
        public const string BOTH = "BOTH"; // VAT & NVAT

        public const string PROMPT = "PROMPT";
        public const string FROM_DTA = "FROM_DTA";
        public const string UNKNOWN = "UNKNOWN";

        public const string DATE = "DATE";
        public const string UNPOSTED = "UNPOSTED";
        public const string ACCOUNT = "ACCOUNT";
        public const string POSTED_BY = "POSTED_BY";

        public const string TEXT = "TEXT";
        public const string WINDOWS = "WINDOWS";
        public const string BIXOLON = "BIXOLON";
        public const string OPOS = "OPOS";


        public const string COURIER = Data.COURIER;
        public const string LUCIDA = Data.LUCIDA;
        public const string DEFAULT = "DEFAULT";

        public const string ONDEMAND = "ONDEMAND";
        public const string AUTO = "AUTO";

        public const string DATE_FORMAT = "dd/MM/yyyy";

        public static void
        Prepare_Blank_Ini_File(string file) {

            var cipher = Common.Encryption.Encrypt(cst_activation_template);

            File.WriteAllText(file, cipher);

        }

        public static void
        Prepare_Dta_For_New_Company(string file, Company_Number number) {

            (number.As_Number > 0).tiff();

            string newline = Environment.NewLine;
            var clear = Common.Encryption.Decrypt(File.ReadAllText(file));

            clear += "{0}[{1}]{0}".spf(newline, number.As_String);
            clear += cst_new_company_template_sit;

            var cipher = Common.Encryption.Encrypt(clear);

            File.WriteAllText(file, cipher);

        }

        public static string Get_Version(int ver) {
            var ret = ver.ToString() + ".0";
            return ret;
        }

        public static int Get_Version(string str) {
            var ret = int.Parse(str);
            return ret;
        }




        const string cst_activation_template =
@"ACTIVATION_KEY=
PIN=
MODULE_1_ENABLED=
MODULE_2_ENABLED=
MODULE_3_ENABLED=
DEFAULT_COMPANY=
ONLY_DEFAULT_COMPANY=
MAX_COMPANIES=
NUMBER_OF_COMPANIES=
VERSION=
#PROMPT or FROM_DTA
CREDENTIALS_SOURCE="
;

        const string cst_new_company_template_sit =
        #region
 @"COMPANY_NAME=
COMPANY_NUMBER=
COMPANY_PATH=
##
##
PASSWORD=
USERNAME=
PATH=
";
        #endregion
        [Obsolete]
        const string cst_new_company_template =
        #region
 @"COMPANY_NAME=
COMPANY_NUMBER=
COMPANY_PATH=
##
##
PASSWORD=
USERNAME=
PATH=
##
##
POS_NUMBER=
Z_SER_NUMBER=
LAST_DATE_EOS_RAN=
LAUNCH_WITH_DUPLICATES=
##
##
ALLOW_CREDIT_NOTES_FOR_CASH_ACC=
ALLOW_CREDIT_NOTE_REFUNDS=
ALLOW_SALE_OF_DUPLICATE_ITEMS=
ALLOW_SURCHARGE=
ALLOW_ZERO_PRICED_ITEMS=
##
##
BARCODE1=
BARCODE2=
BARCODE3=
USE_BARCODE1=
USE_BARCODE2=
USE_BARCODE3=
##
##
BASE_CULTURE=
CURRENCY_DECIMAL_SEPARATOR=
SHORT_DATE_STRING=
FIRST_DAY_OF_WEEK=
NUMBER_DECIMAL_SEPARATOR=
##
##
DEF_CASH_ACCOUNT=
DEF_CASH_PAYMENTS_EXPENSE_ACCOUNT=
DEF_RECEIPTS_BANK=
DEF_SALES_BANK=
##
##
DEFAULT_DETAILS_P=
DEFAULT_DETAILS_S=
DEFAULT_DISCOUNT_DETAILS_P=
DEFAULT_DISCOUNT_DETAILS_S=
POST_REMITTANCE_BY=
##
##
SELECT_CUSTOMER_BEFORE_DATA_ENTRY=
DISCOUNT_DISTRIBUTION_MODE=
DISPLAY_PRICES_WITH=
##
##
SUPER_PASS=
QTY_DECREASE_ALLOWED_TO=
ROUNDING_DISCOUNTS_ALLOWED_TO=
CASH_PAYMENTS_EXPENSE_ACCOUNT_LOCKED=
INVOICE_CANCELLATION_ALLOWED_TO=
LINES_CANCELLATION_ALLOWED_TO=
DATE_EDITABLE=SUPER
#
# the following two parameters are obsolete
#
LINE_DISCOUNTS_ALLOWED_TO=
TOTAL_DISCOUNTS_ALLOWED_TO=
##
##
MAX_LINE_DISC_PERC_CASHIER=
MAX_LINE_DISC_PERC_SUPER=
MAX_ROUNDING_DISCOUNT_AMOUNT=
MAX_TOTAL_DISC_PERC_CASHIER=
MAX_TOTAL_DISC_PERC_SUPER=
##
##
PRINTOUT_COUNT_RECEIPT=
PRINTOUT_COUNT_SALE=
PRINTOUT_COUNT_EOS=
##
##
USE_PRICE_BARCODES=
PRICE_PREFIX_START=
PRICE_PREFIX=
PRICE_BARCODE_LENGTH=
##
PRICE_CODE_START=
PRICE_CODE_LENGTH=
##
PRICE_PRODUCT_START=
PRICE_PRODUCT_LENGTH=
##
# All indices are zero-based
USE_WEIGHT_BARCODES=
WEIGHT_PREFIX_START=
WEIGHT_PREFIX=
WEIGHT_BARCODE_LENGTH=
##
WEIGHT_CODE_START=
WEIGHT_CODE_LENGTH=
##
WEIGHT_PRODUCT_START=
WEIGHT_PRODUCT_LENGTH=
WEIGHT_DECIMALS=
##
##
QSF_BANK_SORT=
QSF_SUPPLIER_SORT=
##
##
SEARCH_ADDRESS=
SEARCH_PHONE=
SEARCH_EMAIL=
SEARCH_CONTACT=
SEARCH_NAME=
SEARCH_COUNTRY=
SEARCH_CASE_SENSITIVE=
##
POST_REMITTANCE_BY=
";
        #endregion



    }

}
