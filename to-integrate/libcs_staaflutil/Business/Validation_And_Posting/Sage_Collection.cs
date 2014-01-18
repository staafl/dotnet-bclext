using System;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using Common.Sage;

namespace Common.Posting
{
    public abstract class Sage_Collection : IReadWrite<string, object>, IRead<string, object>
    {
        protected abstract T Get<T>(string field);
        protected abstract void Set<T>(string field, T value);

        public abstract bool Contains(string field);

        public object this[string field] {
            get {
                return Get<object>(field);
            }
            set {
                Set<object>(field, value);
            }
        }

        public void Add(string field, object obj) {
            this[field] = obj;
        }

        public bool Same_Header(Sage_Collection other) {

            other.tifn();

            return this.ACCOUNT_REF == other.ACCOUNT_REF && !other.ACCOUNT_REF.IsNullOrEmpty()
                && this.TYPE == other.TYPE && !other.TYPE.IsNullOrEmpty()
                && this.DATE == other.DATE && !other.DATE.IsNullOrEmpty()
                && this.INV_REF == other.INV_REF && !other.INV_REF.IsNullOrEmpty();


            //&& this.NOMINAL_CODE == other.NOMINAL_CODE && ! other.NOMINAL_CODE.IsNullOrEmpty() 
            //&& this.BANK_CODE == other.BANK_CODE && ! other.BANK_CODE.IsNullOrEmpty()

        }

        public string Clean(string key) {
            return this[key].strdef().Trim().ToUpper();
        }

        #region CONSTANTS

        const string CST_PROJ_REF = "PROJ_REF";
        const string CST_COST_CODE = "COST_CODE";


        const string CST_DATE = "DATE";

        const string CST_UNIQUE_REF = "UNIQUE_REF";

        const string CST_ACCOUNT_TYPE = "ACCOUNT_TYPE";

        const string CST_ACCOUNT_REF = "ACCOUNT_REF";
        const string CST_NOMINAL_CODE = "NOMINAL_CODE";
        const string CST_STOCK_CODE = "STOCK_CODE";


        const string CST_ADDRESS_1 = "ADDRESS_1";
        const string CST_ADDRESS_2 = "ADDRESS_2";
        const string CST_ADDRESS_3 = "ADDRESS_3";
        const string CST_ADDRESS_4 = "ADDRESS_4";
        const string CST_ADDRESS_5 = "ADDRESS_5";

        const string CST_LAST_CHEQUE = "LAST_CHEQUE";

        const string CST_TELEPHONE = "TELEPHONE";
        const string CST_PRINTED_FLAG = "PRINTED_FLAG";
        const string CST_FAX = "FAX";

        const string CST_AMOUNT_PAID = "AMOUNT_PAID";
        const string CST_AMOUNT_PREPAID = "AMOUNT_PREPAID";
        const string CST_BANK_CODE = "BANK_CODE";
        const string CST_BASE_AMOUNT_PAID = "BASE_AMOUNT_PAID";
        const string CST_BASE_CURRENCY = "BASE_CURRENCY";
        const string CST_BASE_TOT_NET = "BASE_TOT_NET";
        const string CST_BASE_TOT_TAX = "BASE_TOT_TAX";
        const string CST_CONTACT_NAME = "CONTACT_NAME";
        const string CST_CUST_ORDER_NUMBER = "CUST_ORDER_NUMBER";
        const string CST_CUST_TEL_NUMBER = "CUST_TEL_NUMBER";
        const string CST_DEF_TAX_CODE = "DEF_TAX_CODE";

        const string CST_DEL_ADDRESS = "DEL_ADDRESS";
        const string CST_DEL_ADDRESS_1 = "DEL_ADDRESS_1";
        const string CST_DEL_ADDRESS_2 = "DEL_ADDRESS_2";
        const string CST_DEL_ADDRESS_3 = "DEL_ADDRESS_3";
        const string CST_DEL_ADDRESS_4 = "DEL_ADDRESS_4";
        const string CST_DEL_ADDRESS_5 = "DEL_ADDRESS_5";

        const string CST_DELETED_FLAG = "DELETED_FLAG";
        const string CST_DELIVERY_NAME = "DELIVERY_NAME";
        const string CST_DEPT_NUMBER = "DEPT_NUMBER";
        const string CST_DESCRIPTION = "DESCRIPTION";
        const string CST_DETAILS = "DETAILS";
        const string CST_DISCOUNT_AMOUNT = "DISCOUNT_AMOUNT";
        const string CST_DISCOUNT_RATE = "DISCOUNT_RATE";
        const string CST_FULL_NET_AMOUNT = "FULL_NET_AMOUNT";
        const string CST_INV_REF = "INV_REF";
        const string CST_INVOICE_DATE = "INVOICE_DATE";
        const string CST_INVOICE_NUMBER = "INVOICE_NUMBER";
        const string CST_INVOICE_TYPE_CODE = "INVOICE_TYPE_CODE";

        const string CST_ITEMS_NET = "ITEMS_NET";
        const string CST_ITEMS_TAX = "ITEMS_TAX";

        const string CST_LAST_SPLIT = "LAST_SPLIT";
        const string CST_NAME = "NAME";
        const string CST_NET_AMOUNT = "NET_AMOUNT";

        const string CST_NOTES_1 = "NOTES_1";
        const string CST_NOTES_2 = "NOTES_2";
        const string CST_NOTES_3 = "NOTES_3";

        const string CST_ORDER_NUMBER = "ORDER_NUMBER";
        const string CST_PAYMENT_TYPE = "PAYMENT_TYPE";
        const string CST_POSTED_CODE = "POSTED_CODE";
        const string CST_PRINTED_CODE = "PRINTED_CODE";
        const string CST_POSTED_DATE = "POSTED_DATE";
        const string CST_QTY_ORDER = "QTY_ORDER";
        const string CST_QTY = "QTY";

        const string CST_SALES_PRICE = "SALES_PRICE";

        const string CST_SETTLED = "SETTLED";
        const string CST_STOCK_CAT = "STOCK_CAT";

        // Stock Quantity Decimal Precision
        const string CST_STOCK_QTYDP = "STOCK_QTYDP";
        // Stock Unit Decimal Precision
        const string CST_STOCK_UNITDP = "STOCK_UNITDP";

        const string CST_TAKEN_BY = "TAKEN_BY";
        const string CST_TAX_AMOUNT = "TAX_AMOUNT";
        const string CST_TAX_CODE = "TAX_CODE";
        const string CST_TAX_RATE = "TAX_RATE";
        const string CST_INTERNAL_REF = "INTERNAL_REF";

        const string CST_COST_PRICE = "COST_PRICE";
        const string CST_TYPE = "TYPE";
        const string CST_UNIT_OF_SALE = "UNIT_OF_SALE";
        const string CST_UNIT_PRICE = "UNIT_PRICE";
        const string CST_USER_NAME = "USER_NAME";

        const string CST_BANK = "BANK";
        const string CST_GROSS_AMOUNT = "GROSS_AMOUNT";
        const string CST_INDEX = "INDEX";
        const string CST_LASTLINE_FLAG = "LASTLINE_FLAG";
        const string CST_PAYMENT_AMOUNT = "PAYMENT_AMOUNT";
        const string CST_PAYMENT_DATE = "PAYMENT_DATE";
        const string CST_PAYMENT_FLAG = "PAYMENT_FLAG";
        const string CST_PAYMENT_NO = "PAYMENT_NO";
        const string CST_TRAN_TYPE = "TRAN_TYPE";
        const string CST_URN = "URN";
        const string CST_VAT_AMOUNT = "VAT_AMOUNT";

        const string CST_COUNTRY_NAME = "COUNTRY_NAME";
        const string CST_COUNTRY_CODE = "COUNTRY_CODE";
        const string CST_VAT_FORMAT_1 = "VAT_FORMAT_1";
        const string CST_VAT_FORMAT_2 = "VAT_FORMAT_2";
        const string CST_VAT_FORMAT_3 = "VAT_FORMAT_3";
        const string CST_VAT_FORMAT_4 = "VAT_FORMAT_4";
        const string CST_VAT_FORMAT_5 = "VAT_FORMAT_5";

        const string CST_HEADER_NUMBER = "HEADER_NUMBER";
        const string CST_WIZ_START_SPLITS = "WIZ_START_SPLITS";
        const string CST_FINYEAR_MONTH = "FINYEAR_MONTH";
        const string CST_FINYEAR_YEAR = "FINYEAR_YEAR";
        const string CST_EXCHANGE_RATE = "EXCHANGE_RATE";
        const string CST_MINOR_UNIT = "MINOR_UNIT";
        const string CST_MAJOR_UNIT = "MAJOR_UNIT";
        const string CST_CODE = "CODE";
        const string CST_SYMBOL = "SYMBOL";
        const string CST_ACCOUNT_NUMBER = "ACCOUNT_NUMBER";
        const string CST_CURRENCY = "CURRENCY";
        const string CST_STATUS = "STATUS";
        const string CST_FIRST_SPLIT = "FIRST_SPLIT";
        const string CST_NO_OF_SPLIT = "NO_OF_SPLIT";
        const string CST_NO_OF_HEADER = "NO_OF_HEADER";
        const string CST_SDISCOUNT_NO = "SDISCOUNT_NO";
        const string CST_PDISCOUNT_NO = "PDISCOUNT_NO";

        const string CST_RECORD_NUMBER = "RECORD_NUMBER";
        const string CST_LAST_PAY_DATE = "LAST_PAY_DATE";

        const string CST_BASE_FULL_NET = "BASE_FULL_NET";
        const string CST_BASE_NET = "BASE_NET";
        const string CST_BASE_NETVALUE_DISCOUNT = "BASE_NETVALUE_DISCOUNT";
        const string CST_NETVALUE_DISCOUNT = "NETVALUE_DISCOUNT";

        const string CST_ITEM_NUMBER = "ITEM_NUMBER";
        const string CST_ADD_DISC_RATE = "ADD_DISC_RATE";

        const string CST_QTY_IN_STOCK = "QTY_IN_STOCK";
        const string CST_REFERENCE = "REFERENCE";
        const string CST_QUANTITY = "QUANTITY";


        #endregion

        // C:\Users\Fairweather\Desktop\temp.pl

        #region PROPERTIES

        public int ACCOUNT_NUMBER {
            get {
                return Get<int>(CST_ACCOUNT_NUMBER);
            }
            set {
                Set(CST_ACCOUNT_NUMBER, value);
            }
        }

        public string ACCOUNT_REF {
            get {
                return Get<string>(CST_ACCOUNT_REF);
            }
            set {
                Set(CST_ACCOUNT_REF, value);
            }
        }

        public string ACCOUNT_TYPE {
            get {
                return Get<string>(CST_ACCOUNT_TYPE);
            }
            set {
                Set(CST_ACCOUNT_TYPE, value);
            }
        }

        public string ADDRESS_1 {
            get {
                return Get<string>(CST_ADDRESS_1);
            }
            set {
                Set(CST_ADDRESS_1, value);
            }
        }

        public string ADDRESS_2 {
            get {
                return Get<string>(CST_ADDRESS_2);
            }
            set {
                Set(CST_ADDRESS_2, value);
            }
        }

        public string ADDRESS_3 {
            get {
                return Get<string>(CST_ADDRESS_3);
            }
            set {
                Set(CST_ADDRESS_3, value);
            }
        }

        public string ADDRESS_4 {
            get {
                return Get<string>(CST_ADDRESS_4);
            }
            set {
                Set(CST_ADDRESS_4, value);
            }
        }

        public string ADDRESS_5 {
            get {
                return Get<string>(CST_ADDRESS_5);
            }
            set {
                Set(CST_ADDRESS_5, value);
            }
        }

        public decimal ADD_DISC_RATE {
            get {
                return Get<decimal>(CST_ADD_DISC_RATE);
            }
            set {
                Set(CST_ADD_DISC_RATE, value);
            }
        }

        public decimal AMOUNT_PAID {
            get {
                return Get<decimal>(CST_AMOUNT_PAID);
            }
            set {
                Set(CST_AMOUNT_PAID, value);
            }
        }

        public decimal AMOUNT_PREPAID {
            get {
                return Get<decimal>(CST_AMOUNT_PREPAID);
            }
            set {
                Set(CST_AMOUNT_PREPAID, value);
            }
        }

        public string BANK {
            get {
                return Get<string>(CST_BANK);
            }
            set {
                Set(CST_BANK, value);
            }
        }

        public string BANK_CODE {
            get {
                return Get<string>(CST_BANK_CODE);
            }
            set {
                Set(CST_BANK_CODE, value);
            }
        }

        public decimal BASE_AMOUNT_PAID {
            get {
                return Get<decimal>(CST_BASE_AMOUNT_PAID);
            }
            set {
                Set(CST_BASE_AMOUNT_PAID, value);
            }
        }

        public string BASE_CURRENCY {
            get {
                return Get<string>(CST_BASE_CURRENCY);
            }
            set {
                Set(CST_BASE_CURRENCY, value);
            }
        }

        public decimal BASE_FULL_NET {
            get {
                return Get<decimal>(CST_BASE_FULL_NET);
            }
            set {
                Set(CST_BASE_FULL_NET, value);
            }
        }

        public decimal BASE_NET {
            get {
                return Get<decimal>(CST_BASE_NET);
            }
            set {
                Set(CST_BASE_NET, value);
            }
        }

        public decimal BASE_NETVALUE_DISCOUNT {
            get {
                return Get<decimal>(CST_BASE_NETVALUE_DISCOUNT);
            }
            set {
                Set(CST_BASE_NETVALUE_DISCOUNT, value);
            }
        }

        public decimal BASE_TOT_NET {
            get {
                return Get<decimal>(CST_BASE_TOT_NET);
            }
            set {
                Set(CST_BASE_TOT_NET, value);
            }
        }

        public string BASE_TOT_TAX {
            get {
                return Get<string>(CST_BASE_TOT_TAX);
            }
            set {
                Set(CST_BASE_TOT_TAX, value);
            }
        }

        public string CODE {
            get {
                return Get<string>(CST_CODE);
            }
            set {
                Set(CST_CODE, value);
            }
        }

        public string CONTACT_NAME {
            get {
                return Get<string>(CST_CONTACT_NAME);
            }
            set {
                Set(CST_CONTACT_NAME, value);
            }
        }

        public string COST_CODE {
            get {
                return Get<string>(CST_COST_CODE);
            }
            set {
                Set(CST_COST_CODE, value);
            }
        }

        public decimal COST_PRICE {
            get {
                return Get<decimal>(CST_COST_PRICE);
            }
            set {
                Set(CST_COST_PRICE, value);
            }
        }

        public string COUNTRY_CODE {
            get {
                return Get<string>(CST_COUNTRY_CODE);
            }
            set {
                Set(CST_COUNTRY_CODE, value);
            }
        }

        public string COUNTRY_NAME {
            get {
                return Get<string>(CST_COUNTRY_NAME);
            }
            set {
                Set(CST_COUNTRY_NAME, value);
            }
        }

        public string CURRENCY {
            get {
                return Get<string>(CST_CURRENCY);
            }
            set {
                Set(CST_CURRENCY, value);
            }
        }

        public int CUST_ORDER_NUMBER {
            get {
                return Get<int>(CST_CUST_ORDER_NUMBER);
            }
            set {
                Set(CST_CUST_ORDER_NUMBER, value);
            }
        }

        public int CUST_TEL_NUMBER {
            get {
                return Get<int>(CST_CUST_TEL_NUMBER);
            }
            set {
                Set(CST_CUST_TEL_NUMBER, value);
            }
        }

        public DateTime DATE {
            get {
                return Get<DateTime>(CST_DATE);
            }
            set {
                Set(CST_DATE, value);
            }
        }

        public string DEF_TAX_CODE {
            get {
                return Get<string>(CST_DEF_TAX_CODE);
            }
            set {
                Set(CST_DEF_TAX_CODE, value);
            }
        }

        public bool DELETED_FLAG {
            get {
                return Get<bool>(CST_DELETED_FLAG);
            }
            set {
                Set(CST_DELETED_FLAG, value);
            }
        }

        public string DELIVERY_NAME {
            get {
                return Get<string>(CST_DELIVERY_NAME);
            }
            set {
                Set(CST_DELIVERY_NAME, value);
            }
        }

        public string DEL_ADDRESS {
            get {
                return Get<string>(CST_DEL_ADDRESS);
            }
            set {
                Set(CST_DEL_ADDRESS, value);
            }
        }

        public string DEL_ADDRESS_1 {
            get {
                return Get<string>(CST_DEL_ADDRESS_1);
            }
            set {
                Set(CST_DEL_ADDRESS_1, value);
            }
        }

        public string DEL_ADDRESS_2 {
            get {
                return Get<string>(CST_DEL_ADDRESS_2);
            }
            set {
                Set(CST_DEL_ADDRESS_2, value);
            }
        }

        public string DEL_ADDRESS_3 {
            get {
                return Get<string>(CST_DEL_ADDRESS_3);
            }
            set {
                Set(CST_DEL_ADDRESS_3, value);
            }
        }

        public string DEL_ADDRESS_4 {
            get {
                return Get<string>(CST_DEL_ADDRESS_4);
            }
            set {
                Set(CST_DEL_ADDRESS_4, value);
            }
        }

        public string DEL_ADDRESS_5 {
            get {
                return Get<string>(CST_DEL_ADDRESS_5);
            }
            set {
                Set(CST_DEL_ADDRESS_5, value);
            }
        }

        public int DEPT_NUMBER {
            get {
                return Get<int>(CST_DEPT_NUMBER);
            }
            set {
                Set(CST_DEPT_NUMBER, value);
            }
        }

        public string DESCRIPTION {
            get {
                return Get<string>(CST_DESCRIPTION);
            }
            set {
                Set(CST_DESCRIPTION, value);
            }
        }

        public string DETAILS {
            get {
                return Get<string>(CST_DETAILS);
            }
            set {
                Set(CST_DETAILS, value);
            }
        }

        public decimal DISCOUNT_AMOUNT {
            get {
                return Get<decimal>(CST_DISCOUNT_AMOUNT);
            }
            set {
                Set(CST_DISCOUNT_AMOUNT, value);
            }
        }

        public decimal DISCOUNT_RATE {
            get {
                return Get<decimal>(CST_DISCOUNT_RATE);
            }
            set {
                Set(CST_DISCOUNT_RATE, value);
            }
        }

        public decimal EXCHANGE_RATE {
            get {
                return Get<decimal>(CST_EXCHANGE_RATE);
            }
            set {
                Set(CST_EXCHANGE_RATE, value);
            }
        }

        public string FAX {
            get {
                return Get<string>(CST_FAX);
            }
            set {
                Set(CST_FAX, value);
            }
        }

        public int FINYEAR_MONTH {
            get {
                return Get<int>(CST_FINYEAR_MONTH);
            }
            set {
                Set(CST_FINYEAR_MONTH, value);
            }
        }

        public int FINYEAR_YEAR {
            get {
                return Get<int>(CST_FINYEAR_YEAR);
            }
            set {
                Set(CST_FINYEAR_YEAR, value);
            }
        }

        public int FIRST_SPLIT {
            get {
                return Get<int>(CST_FIRST_SPLIT);
            }
            set {
                Set(CST_FIRST_SPLIT, value);
            }
        }

        public decimal FULL_NET_AMOUNT {
            get {
                return Get<decimal>(CST_FULL_NET_AMOUNT);
            }
            set {
                Set(CST_FULL_NET_AMOUNT, value);
            }
        }

        public decimal GROSS_AMOUNT {
            get {
                return Get<decimal>(CST_GROSS_AMOUNT);
            }
            set {
                Set(CST_GROSS_AMOUNT, value);
            }
        }

        public int HEADER_NUMBER {
            get {
                return Get<int>(CST_HEADER_NUMBER);
            }
            set {
                Set(CST_HEADER_NUMBER, value);
            }
        }

        public string INDEX {
            get {
                return Get<string>(CST_INDEX);
            }
            set {
                Set(CST_INDEX, value);
            }
        }

        public string INTERNAL_REF {
            get {
                return Get<string>(CST_INTERNAL_REF);
            }
            set {
                Set(CST_INTERNAL_REF, value);
            }
        }

        public DateTime INVOICE_DATE {
            get {
                return Get<DateTime>(CST_INVOICE_DATE);
            }
            set {
                Set(CST_INVOICE_DATE, value);
            }
        }

        public int INVOICE_NUMBER {
            get {
                return Get<int>(CST_INVOICE_NUMBER);
            }
            set {
                Set(CST_INVOICE_NUMBER, value);
            }
        }

        public string INVOICE_TYPE_CODE {
            get {
                return Get<string>(CST_INVOICE_TYPE_CODE);
            }
            set {
                Set(CST_INVOICE_TYPE_CODE, value);
            }
        }

        public string INV_REF {
            get {
                return Get<string>(CST_INV_REF);
            }
            set {
                Set(CST_INV_REF, value);
            }
        }

        public decimal ITEMS_NET {
            get {
                return Get<decimal>(CST_ITEMS_NET);
            }
            set {
                Set(CST_ITEMS_NET, value);
            }
        }

        public string ITEMS_TAX {
            get {
                return Get<string>(CST_ITEMS_TAX);
            }
            set {
                Set(CST_ITEMS_TAX, value);
            }
        }

        public int ITEM_NUMBER {
            get {
                return Get<int>(CST_ITEM_NUMBER);
            }
            set {
                Set(CST_ITEM_NUMBER, value);
            }
        }

        public bool LASTLINE_FLAG {
            get {
                return Get<bool>(CST_LASTLINE_FLAG);
            }
            set {
                Set(CST_LASTLINE_FLAG, value);
            }
        }

        public string LAST_CHEQUE {
            get {
                return Get<string>(CST_LAST_CHEQUE);
            }
            set {
                Set(CST_LAST_CHEQUE, value);
            }
        }

        public DateTime LAST_PAY_DATE {
            get {
                return Get<DateTime>(CST_LAST_PAY_DATE);
            }
            set {
                Set(CST_LAST_PAY_DATE, value);
            }
        }

        public int LAST_SPLIT {
            get {
                return Get<int>(CST_LAST_SPLIT);
            }
            set {
                Set(CST_LAST_SPLIT, value);
            }
        }

        public string MAJOR_UNIT {
            get {
                return Get<string>(CST_MAJOR_UNIT);
            }
            set {
                Set(CST_MAJOR_UNIT, value);
            }
        }

        public string MINOR_UNIT {
            get {
                return Get<string>(CST_MINOR_UNIT);
            }
            set {
                Set(CST_MINOR_UNIT, value);
            }
        }

        public string NAME {
            get {
                return Get<string>(CST_NAME);
            }
            set {
                Set(CST_NAME, value);
            }
        }

        public decimal NETVALUE_DISCOUNT {
            get {
                return Get<decimal>(CST_NETVALUE_DISCOUNT);
            }
            set {
                Set(CST_NETVALUE_DISCOUNT, value);
            }
        }

        public decimal NET_AMOUNT {
            get {
                return Get<decimal>(CST_NET_AMOUNT);
            }
            set {
                Set(CST_NET_AMOUNT, value);
            }
        }

        public string NOMINAL_CODE {
            get {
                return Get<string>(CST_NOMINAL_CODE);
            }
            set {
                Set(CST_NOMINAL_CODE, value);
            }
        }

        public string NOTES_1 {
            get {
                return Get<string>(CST_NOTES_1);
            }
            set {
                Set(CST_NOTES_1, value);
            }
        }

        public string NOTES_2 {
            get {
                return Get<string>(CST_NOTES_2);
            }
            set {
                Set(CST_NOTES_2, value);
            }
        }

        public string NOTES_3 {
            get {
                return Get<string>(CST_NOTES_3);
            }
            set {
                Set(CST_NOTES_3, value);
            }
        }

        public int NO_OF_HEADER {
            get {
                return Get<int>(CST_NO_OF_HEADER);
            }
            set {
                Set(CST_NO_OF_HEADER, value);
            }
        }

        public int NO_OF_SPLIT {
            get {
                return Get<int>(CST_NO_OF_SPLIT);
            }
            set {
                Set(CST_NO_OF_SPLIT, value);
            }
        }

        public int ORDER_NUMBER {
            get {
                return Get<int>(CST_ORDER_NUMBER);
            }
            set {
                Set(CST_ORDER_NUMBER, value);
            }
        }

        public decimal PAYMENT_AMOUNT {
            get {
                return Get<decimal>(CST_PAYMENT_AMOUNT);
            }
            set {
                Set(CST_PAYMENT_AMOUNT, value);
            }
        }

        public DateTime PAYMENT_DATE {
            get {
                return Get<DateTime>(CST_PAYMENT_DATE);
            }
            set {
                Set(CST_PAYMENT_DATE, value);
            }
        }

        public bool PAYMENT_FLAG {
            get {
                return Get<bool>(CST_PAYMENT_FLAG);
            }
            set {
                Set(CST_PAYMENT_FLAG, value);
            }
        }

        public string PAYMENT_NO {
            get {
                return Get<string>(CST_PAYMENT_NO);
            }
            set {
                Set(CST_PAYMENT_NO, value);
            }
        }

        public Versioning.TransType PAYMENT_TYPE {
            get {
                return Get<Versioning.TransType>(CST_PAYMENT_TYPE);
            }
            set {
                Set(CST_PAYMENT_TYPE, value);
            }
        }

        public string PDISCOUNT_NO {
            get {
                return Get<string>(CST_PDISCOUNT_NO);
            }
            set {
                Set(CST_PDISCOUNT_NO, value);
            }
        }

        public string POSTED_CODE {
            get {
                return Get<string>(CST_POSTED_CODE);
            }
            set {
                Set(CST_POSTED_CODE, value);
            }
        }

        public DateTime POSTED_DATE {
            get {
                return Get<DateTime>(CST_POSTED_DATE);
            }
            set {
                Set(CST_POSTED_DATE, value);
            }
        }

        public string PRINTED_CODE {
            get {
                return Get<string>(CST_PRINTED_CODE);
            }
            set {
                Set(CST_PRINTED_CODE, value);
            }
        }

        public bool PRINTED_FLAG {
            get {
                return Get<bool>(CST_PRINTED_FLAG);
            }
            set {
                Set(CST_PRINTED_FLAG, value);
            }
        }

        public string PROJ_REF {
            get {
                return Get<string>(CST_PROJ_REF);
            }
            set {
                Set(CST_PROJ_REF, value);
            }
        }

        public decimal QTY {
            get {
                return Get<decimal>(CST_QTY);
            }
            set {
                Set(CST_QTY, value);
            }
        }

        public decimal QTY_IN_STOCK {
            get {
                return Get<decimal>(CST_QTY_IN_STOCK);
            }
            set {
                Set(CST_QTY_IN_STOCK, value);
            }
        }

        public decimal QTY_ORDER {
            get {
                return Get<decimal>(CST_QTY_ORDER);
            }
            set {
                Set(CST_QTY_ORDER, value);
            }
        }

        public int QUANTITY {
            get {
                return Get<int>(CST_QUANTITY);
            }
            set {
                Set(CST_QUANTITY, value);
            }
        }

        public int RECORD_NUMBER {
            get {
                return Get<int>(CST_RECORD_NUMBER);
            }
            set {
                Set(CST_RECORD_NUMBER, value);
            }
        }

        public string REFERENCE {
            get {
                return Get<string>(CST_REFERENCE);
            }
            set {
                Set(CST_REFERENCE, value);
            }
        }

        public decimal SALES_PRICE {
            get {
                return Get<decimal>(CST_SALES_PRICE);
            }
            set {
                Set(CST_SALES_PRICE, value);
            }
        }

        public string SDISCOUNT_NO {
            get {
                return Get<string>(CST_SDISCOUNT_NO);
            }
            set {
                Set(CST_SDISCOUNT_NO, value);
            }
        }

        public bool SETTLED {
            get {
                return Get<bool>(CST_SETTLED);
            }
            set {
                Set(CST_SETTLED, value);
            }
        }

        public string STATUS {
            get {
                return Get<string>(CST_STATUS);
            }
            set {
                Set(CST_STATUS, value);
            }
        }

        public string STOCK_CAT {
            get {
                return Get<string>(CST_STOCK_CAT);
            }
            set {
                Set(CST_STOCK_CAT, value);
            }
        }

        public string STOCK_CODE {
            get {
                return Get<string>(CST_STOCK_CODE);
            }
            set {
                Set(CST_STOCK_CODE, value);
            }
        }

        public decimal STOCK_QTYDP {
            get {
                return Get<decimal>(CST_STOCK_QTYDP);
            }
            set {
                Set(CST_STOCK_QTYDP, value);
            }
        }

        public int STOCK_UNITDP {
            get {
                return Get<int>(CST_STOCK_UNITDP);
            }
            set {
                Set(CST_STOCK_UNITDP, value);
            }
        }

        public string SYMBOL {
            get {
                return Get<string>(CST_SYMBOL);
            }
            set {
                Set(CST_SYMBOL, value);
            }
        }

        public string TAKEN_BY {
            get {
                return Get<string>(CST_TAKEN_BY);
            }
            set {
                Set(CST_TAKEN_BY, value);
            }
        }

        public decimal TAX_AMOUNT {
            get {
                return Get<decimal>(CST_TAX_AMOUNT);
            }
            set {
                Set(CST_TAX_AMOUNT, value);
            }
        }

        public short TAX_CODE {
            get {
                return Get<short>(CST_TAX_CODE);
            }
            set {
                Set(CST_TAX_CODE, value);
            }
        }

        public decimal TAX_RATE {
            get {
                return Get<decimal>(CST_TAX_RATE);
            }
            set {
                Set(CST_TAX_RATE, value);
            }
        }

        public string TELEPHONE {
            get {
                return Get<string>(CST_TELEPHONE);
            }
            set {
                Set(CST_TELEPHONE, value);
            }
        }

        public Versioning.StockTransType TRAN_TYPE {
            get {
                return Get<Versioning.StockTransType>(CST_TRAN_TYPE);
            }
            set {
                Set(CST_TRAN_TYPE, value);
            }
        }

        public Versioning.InvoiceType INVOICE_TYPE {
            get {
                return Get<Versioning.InvoiceType>(CST_INVOICE_TYPE_CODE);
            }
            set {
                Set(CST_INVOICE_TYPE_CODE, value);
            }
        }

        public Versioning.TransType TYPE {
            get {
                return Get<Versioning.TransType>(CST_TYPE);
            }
            set {
                Set(CST_TYPE, value);
            }
        }

        public Versioning.StockTransType ST_TYPE {
            get {
                return Get<Versioning.StockTransType>(CST_TYPE);
            }
            set {
                Set(CST_TYPE, value);
            }
        }

        public string UNIQUE_REF {
            get {
                return Get<string>(CST_UNIQUE_REF);
            }
            set {
                Set(CST_UNIQUE_REF, value);
            }
        }

        public string UNIT_OF_SALE {
            get {
                return Get<string>(CST_UNIT_OF_SALE);
            }
            set {
                Set(CST_UNIT_OF_SALE, value);
            }
        }

        public decimal UNIT_PRICE {
            get {
                return Get<decimal>(CST_UNIT_PRICE);
            }
            set {
                Set(CST_UNIT_PRICE, value);
            }
        }

        public string URN {
            get {
                return Get<string>(CST_URN);
            }
            set {
                Set(CST_URN, value);
            }
        }

        public string USER_NAME {
            get {
                return Get<string>(CST_USER_NAME);
            }
            set {
                Set(CST_USER_NAME, value);
            }
        }

        public decimal VAT_AMOUNT {
            get {
                return Get<decimal>(CST_VAT_AMOUNT);
            }
            set {
                Set(CST_VAT_AMOUNT, value);
            }
        }

        public string VAT_FORMAT_1 {
            get {
                return Get<string>(CST_VAT_FORMAT_1);
            }
            set {
                Set(CST_VAT_FORMAT_1, value);
            }
        }

        public string VAT_FORMAT_2 {
            get {
                return Get<string>(CST_VAT_FORMAT_2);
            }
            set {
                Set(CST_VAT_FORMAT_2, value);
            }
        }

        public string VAT_FORMAT_3 {
            get {
                return Get<string>(CST_VAT_FORMAT_3);
            }
            set {
                Set(CST_VAT_FORMAT_3, value);
            }
        }

        public string VAT_FORMAT_4 {
            get {
                return Get<string>(CST_VAT_FORMAT_4);
            }
            set {
                Set(CST_VAT_FORMAT_4, value);
            }
        }

        public string VAT_FORMAT_5 {
            get {
                return Get<string>(CST_VAT_FORMAT_5);
            }
            set {
                Set(CST_VAT_FORMAT_5, value);
            }
        }

        public int WIZ_START_SPLITS {
            get {
                return Get<int>(CST_WIZ_START_SPLITS);
            }
            set {
                Set(CST_WIZ_START_SPLITS, value);
            }
        }



        #endregion
    }
}
