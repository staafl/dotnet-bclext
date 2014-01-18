using System;
using Versioning;

namespace Common
{

    public partial class Sage_Logic
    {
        const TransType TT_SA = TransType.sdoSA;
        const TransType TT_SI = TransType.sdoSI;
        const TransType TT_SR = TransType.sdoSR;
        const TransType TT_SD = TransType.sdoSD;
        const TransType TT_SC = TransType.sdoSC;
        const TransType TT_PA = TransType.sdoPA;
        const TransType TT_PI = TransType.sdoPI;
        const TransType TT_PP = TransType.sdoPP;
        const TransType TT_PD = TransType.sdoPD;
        const TransType TT_PC = TransType.sdoPC;



        const string PROJ_REF = "PROJ_REF";
        const string COST_CODE = "COST_CODE";

        const int ROUNDING_PRECISION = 2;

        const int cst_address_lines = 5;

        const string CASH = "CASH";
        const string CREDIT = "CC";
        const string GIFT = "GIFT";

        // e.g. "Sales Payment, Post-dated Cheque n. XXXXX"

        const string SALES_PAYMENT = "Sales Payment";
        const string RECEIPT_ON_ACCOUNT = "Receipt on Account";
        const string CHEQUE = "Cheque";
        const string POST_DATED_CHEQUE = "Post-dated Cheque";

        const string CHEQUE_NUM_PREFIX = " n. ";

        const string REFUND = "Refund";


        // ****************************


        const string DATE = "DATE";

        const string UNIQUE_REF = "UNIQUE_REF";

        const string ACCOUNT_TYPE = "ACCOUNT_TYPE";

        const string ACCOUNT_REF = "ACCOUNT_REF";
        const string NOMINAL_CODE = "NOMINAL_CODE";
        const string STOCK_CODE = "STOCK_CODE";


        const string ADDRESS_1 = "ADDRESS_1";
        const string ADDRESS_2 = "ADDRESS_2";
        const string ADDRESS_3 = "ADDRESS_3";
        const string ADDRESS_4 = "ADDRESS_4";
        const string ADDRESS_5 = "ADDRESS_5";

        const string LAST_CHEQUE = "LAST_CHEQUE";

        const string TELEPHONE = "TELEPHONE";
        const string PRINTED_FLAG = "PRINTED_FLAG";
        const string FAX = "FAX";

        const string AMOUNT_PAID = "AMOUNT_PAID";
        const string AMOUNT_PREPAID = "AMOUNT_PREPAID";
        const string BANK_CODE = "BANK_CODE";
        const string BASE_AMOUNT_PAID = "BASE_AMOUNT_PAID";
        const string BASE_CURRENCY = "BASE_CURRENCY";
        const string BASE_TOT_NET = "BASE_TOT_NET";
        const string BASE_TOT_TAX = "BASE_TOT_TAX";
        const string CONTACT_NAME = "CONTACT_NAME";
        const string CUST_ORDER_NUMBER = "CUST_ORDER_NUMBER";
        const string CUST_TEL_NUMBER = "CUST_TEL_NUMBER";
        const string DEF_TAX_CODE = "DEF_TAX_CODE";

        const string DEL_ADDRESS = "DEL_ADDRESS";
        const string DEL_ADDRESS_1 = "DEL_ADDRESS_1";
        const string DEL_ADDRESS_2 = "DEL_ADDRESS_2";
        const string DEL_ADDRESS_3 = "DEL_ADDRESS_3";
        const string DEL_ADDRESS_4 = "DEL_ADDRESS_4";
        const string DEL_ADDRESS_5 = "DEL_ADDRESS_5";

        const string DELETED_FLAG = "DELETED_FLAG";
        const string DELIVERY_NAME = "DELIVERY_NAME";
        const string DEPT_NUMBER = "DEPT_NUMBER";
        const string DESCRIPTION = "DESCRIPTION";
        const string DETAILS = "DETAILS";
        const string DISCOUNT_AMOUNT = "DISCOUNT_AMOUNT";
        [Obsolete("Use ADD_DISC_RATE instead")]
        const string DISCOUNT_RATE = "DISCOUNT_RATE";
        const string FULL_NET_AMOUNT = "FULL_NET_AMOUNT";
        const string INV_REF = "INV_REF";
        const string INVOICE_DATE = "INVOICE_DATE";
        const string INVOICE_NUMBER = "INVOICE_NUMBER";
        const string INVOICE_TYPE_CODE = "INVOICE_TYPE_CODE";

        const string ITEMS_NET = "ITEMS_NET";
        const string ITEMS_TAX = "ITEMS_TAX";

        const string LAST_SPLIT = "LAST_SPLIT";
        const string NAME = "NAME";
        const string NET_AMOUNT = "NET_AMOUNT";

        const string NOTES_1 = "NOTES_1";
        const string NOTES_2 = "NOTES_2";
        const string NOTES_3 = "NOTES_3";

        const string ORDER_NUMBER = "ORDER_NUMBER";
        const string PAYMENT_TYPE = "PAYMENT_TYPE";
        const string POSTED_CODE = "POSTED_CODE";
        const string PRINTED_CODE = "PRINTED_CODE";
        const string POSTED_DATE = "POSTED_DATE";
        const string QTY_ORDER = "QTY_ORDER";
        const string QTY = "QTY";

        const string SALES_PRICE = "SALES_PRICE";

        const string SETTLED = "SETTLED";
        const string STOCK_CAT = "STOCK_CAT";

        // Stock Quantity Decimal Precision
        const string STOCK_QTYDP = "STOCK_QTYDP";
        // Stock Unit Decimal Precision
        const string STOCK_UNITDP = "STOCK_UNITDP";

        const string TAKEN_BY = "TAKEN_BY";
        const string TAX_AMOUNT = "TAX_AMOUNT";
        const string TAX_CODE = "TAX_CODE";
        const string TAX_RATE = "TAX_RATE";
        const string INTERNAL_REF = "INTERNAL_REF";

        const string COST_PRICE = "COST_PRICE";
        const string TYPE = "TYPE";
        const string UNIT_OF_SALE = "UNIT_OF_SALE";
        const string UNIT_PRICE = "UNIT_PRICE";
        const string USER_NAME = "USER_NAME";

        const string BANK = "BANK";
        const string GROSS_AMOUNT = "GROSS_AMOUNT";
        const string INDEX = "INDEX";
        const string LASTLINE_FLAG = "LASTLINE_FLAG";
        const string PAYMENT_AMOUNT = "PAYMENT_AMOUNT";
        const string PAYMENT_DATE = "PAYMENT_DATE";
        const string PAYMENT_FLAG = "PAYMENT_FLAG";
        const string PAYMENT_NO = "PAYMENT_NO";
        const string TRAN_TYPE = "TRAN_TYPE";
        const string URN = "URN";
        const string VAT_AMOUNT = "VAT_AMOUNT";

        const string COUNTRY_NAME = "COUNTRY_NAME";
        const string COUNTRY_CODE = "COUNTRY_CODE";
        const string VAT_FORMAT_1 = "VAT_FORMAT_1";
        const string VAT_FORMAT_2 = "VAT_FORMAT_2";
        const string VAT_FORMAT_3 = "VAT_FORMAT_3";
        const string VAT_FORMAT_4 = "VAT_FORMAT_4";
        const string VAT_FORMAT_5 = "VAT_FORMAT_5";

        const string HEADER_NUMBER = "HEADER_NUMBER";
        const string WIZ_START_SPLITS = "WIZ_START_SPLITS";
        const string FINYEAR_MONTH = "FINYEAR_MONTH";
        const string FINYEAR_YEAR = "FINYEAR_YEAR";
        const string EXCHANGE_RATE = "EXCHANGE_RATE";
        const string MINOR_UNIT = "MINOR_UNIT";
        const string MAJOR_UNIT = "MAJOR_UNIT";
        const string CODE = "CODE";
        const string SYMBOL = "SYMBOL";
        const string ACCOUNT_NUMBER = "ACCOUNT_NUMBER";
        const string CURRENCY = "CURRENCY";
        const string STATUS = "STATUS";
        const string FIRST_SPLIT = "FIRST_SPLIT";
        const string NO_OF_SPLIT = "NO_OF_SPLIT";
        const string NO_OF_HEADER = "NO_OF_HEADER";
        const string SDISCOUNT_NO = "SDISCOUNT_NO";
        const string PDISCOUNT_NO = "PDISCOUNT_NO";

        const string RECORD_NUMBER = "RECORD_NUMBER";
        const string LAST_PAY_DATE = "LAST_PAY_DATE";

        const string BASE_FULL_NET = "BASE_FULL_NET";
        const string BASE_NET = "BASE_NET";
        const string BASE_NETVALUE_DISCOUNT = "BASE_NETVALUE_DISCOUNT";
        const string NETVALUE_DISCOUNT = "NETVALUE_DISCOUNT";

        const string ITEM_NUMBER = "ITEM_NUMBER";
        const string ADD_DISC_RATE = "ADD_DISC_RATE";

        const string QTY_IN_STOCK = "QTY_IN_STOCK";
        const string REFERENCE = "REFERENCE";
        const string QUANTITY = "QUANTITY";

    }
}