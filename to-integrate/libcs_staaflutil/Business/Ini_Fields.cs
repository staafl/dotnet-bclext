namespace DTA
{
    using System;

    using Common;

    using System.Collections.Generic;

    using Fairweather.Service;
    using Common.Sage;

    public static class DTA_Fields
    {
        static DTA_Fields() { }

        /* values */
        const string
        SAME_POS = Ini_Main.SAME_POS;
        #region values
        const string PURCHASES_ONLY = Ini_Main.PURCHASES_ONLY;
        const string ANY_POS = Ini_Main.ANY_POS;
        const string ANY = Ini_Main.ANY;

        const string NUMBER = Ini_Main.NUMBER;
        const string ALL = Ini_Main.ALL;

        const string CASH = Ini_Main.CASH;
        const string SUPER = Ini_Main.SUPER;
        const string NONE = Ini_Main.NONE;
        const string QTY = Ini_Main.QTY;
        const string AMOUNT = Ini_Main.AMOUNT;

        const string NO = Ini_Main.NO;
        const string YES = Ini_Main.YES;

        const string NVAT = Ini_Main.NVAT;
        const string VAT = Ini_Main.VAT;
        const string BOTH = Ini_Main.BOTH;

        const string ONE_HUNDRED = "100";
        const string DATE_FORMAT = Ini_Main.DATE_FORMAT;

        //{[A-Z_]+}.*
        //\1 = DTA_Main.\1;
        const string DATE = Ini_Main.DATE;
        const string UNPOSTED = Ini_Main.UNPOSTED;
        const string ACCOUNT = Ini_Main.ACCOUNT;
        const string POSTED_BY = Ini_Main.POSTED_BY;

        const string TEXT = Ini_Main.TEXT;
        const string WINDOWS = Ini_Main.WINDOWS;
        const string BIXOLON = Ini_Main.BIXOLON;
        const string OPOS = Ini_Main.OPOS;

        const string COURIER = Data.COURIER;
        const string LUCIDA = Data.LUCIDA;
        const string DEFAULT = Ini_Main.DEFAULT;

        const string PROMPT = Ini_Main.PROMPT;
        const string FROM_DTA = Ini_Main.FROM_DTA;

        #endregion



        #region helper methods


        static string Date(DateTime date) {

            return date.ToString(DATE_FORMAT);
        }

        //public static string
        //Get_Default_Value(string field) {

        //    DTA_Field dta_field;
        //    bool ok = rd_fields.TryGetValue(field, out dta_field);

        //    if (!ok)
        //        true.tift<ApplicationException>("Invalid field: " + field);

        //    return dta_field.Default_Value;

        //}

        //public static bool
        //Try_Get_Default_Value(string field, out string value) {

        //    DTA_Field dta_field;

        //    bool ret = rd_fields.TryGetValue(field, out dta_field);

        //    if (ret)
        //        value = dta_field.Default_Value;
        //    else
        //        value = null;

        //    return ret;

        //}

        //internal static int
        //Set_Default_Values(IReadWrite<string, string> destination,
        //   bool overwrite,
        //   bool set_required) {

        //    int ret = 0;

        //    foreach (var kvp in rd_fields) {

        //        string field = kvp.Key;

        //        string def_val = kvp.Value.Default_Value;

        //        if (!set_required) {

        //            bool is_required = kvp.Value.Is_Required;

        //            if (is_required)
        //                continue;

        //        }

        //        if (overwrite ||
        //        !destination.Contains(field) ||
        //        destination[field].IsNullOrEmpty()) {

        //            destination[field] = def_val;
        //            ++ret;

        //        }

        //    }

        //    return ret;

        //}

        ///// <summary>
        ///// Returns all keys which were not present in the source.
        ///// </summary>
        //public static bool
        //Verify(IContains<string> source, out List<string> missing) {

        //    bool ret = true;
        //    missing = new List<string>();
        //    foreach (var field in rd_fields.Keys) {

        //        if (!source.Contains(field)) {
        //            missing.Add(field);
        //            ret = false;
        //        }

        //    }

        //    return ret;

        //}

        ///// <summary>
        ///// 
        ///// All NON REQUIRED keys not present in the source are filled with their default values.
        ///// All REQUIRED keys not present in the source or with the empty string as value are entered
        /////     in "missing"
        /////     
        ///// All NON REQUIRED keys present in the source are left alone.
        ///// All REQUIRED keys with non-empty values are left alone.
        ///// 
        ///// </summary>
        //public static bool
        //Verify_And_Fill(IReadWrite<string, string> source, out List<string> missing) {

        //    bool ret = true;
        //    missing = new List<string>();

        //    foreach (var kvp in rd_fields) {

        //        var field = kvp.Key;
        //        var dta_field = kvp.Value;
        //        bool required = dta_field.Is_Required;

        //        if (!required) {
        //            if (source.Contains(field))
        //                continue;
        //            else
        //                source[field] = dta_field.Default_Value;
        //        }

        //        if (source.Contains(field) &&
        //        !source[field].IsNullOrEmpty())
        //            continue;

        //        missing.Add(field);
        //        ret = false;


        //    }

        //    return ret;

        //}

        //public static Dictionary<string, DTA_Field> Fields {
        //    get {
        //        return new Dictionary<string, DTA_Field>(rd_fields);
        //    }
        //}

        //public static DTA_Field? Field(string field) {

        //    DTA_Field ret;
        //    if (!rd_fields.TryGetValue(field, out ret))
        //        return null;

        //    return ret;
        //}
        #endregion



        /* Definitions */

        /*       SES - General        */


        public static readonly Ini_Field MODULE_1 = new Ini_Field("MODULE_1_ENABLED");
        public static readonly Ini_Field MODULE_2 = new Ini_Field("MODULE_2_ENABLED");
        public static readonly Ini_Field MODULE_3 = new Ini_Field("MODULE_3_ENABLED");


        /*       Transactions entry screens        */

        public static readonly Ini_Field
        TE_default_bank_receipts = new Ini_Field("DEFAULT_BANK_RECEIPTS", "1200"),
        TE_default_bank_payments = new Ini_Field("DEFAULT_BANK_PAYMENTS", "1200"),
        TE_jx_against_debtors_creditors_ctrl = new Ini_Field("JX_AGAINST_DEBTORS_CREDITORS_CTRL", NO),
        TE_sales_ref_check = new Ini_Field("SALES_REF_CHECK", Ini_Main.ONDEMAND),
        TE_purchase_ref_check = new Ini_Field("PURCHASE_REF_CHECK", Ini_Main.AUTO),
        TE_max_details_history_length = new Ini_Field(cst_max_details_history_length, "100"),
        TE_posted_report = new Ini_Field("POSTED_REPORT", YES)
        ;

        /*       Supplier Payments / Customer Receipts screens        */


        public static readonly Ini_Field
        ESF_purchase_discount_details = new Ini_Field("PURCHASE_DISCOUNT_DETAILS", "Purchase Discount"),
        #region
 ESF_purchase_receipt_details = new Ini_Field("PURCHASE_RECEIPT_DETAILS", "Purchase Receipt"),
        ESF_sales_discount_details = new Ini_Field("SALES_DISCOUNT_DETAILS", "Sales Discount"),
        ESF_sales_receipt_details = new Ini_Field("SALES_RECEIPT_DETAILS", "Sales Receipt"),
        ESF_post_remittance_by_number = new Ini_Field("POST_REMITTANCE_BY_NUMBER", NO);
        #endregion

        /*       Point of Sales screen        */



        public static readonly Ini_Field
        POS_end_of_day_security = new Ini_Field("END_OF_DAY_SECURITY", CASH),
        POS_confirm_invoice = new Ini_Field(cst_confirm_invoice, YES),
        POS_confirm_receipt = new Ini_Field(cst_confirm_receipt, YES),
        POS_confirm_payment = new Ini_Field(cst_confirm_payment, YES),
        POS_price_reverse_calculate = new Ini_Field(cst_price_reverse_calculate, YES),
        #region
 POS_avg_cost_calculation_mode = new Ini_Field("AVERAGE_COST_CALCULATION_MODE", PURCHASES_ONLY),
        POS_start_maximized = new Ini_Field(cst_start_maximized, YES),
        POS_pos_id = new Ini_Field(cst_pos_id, "1"),
        POS_z_ser_number = new Ini_Field(cst_z_ser_number, "0"),
        POS_last_date_eos_ran = new Ini_Field(cst_last_date_eos_ran, Date(DateTime.MinValue)),

        POS_select_customer_before_data_entry = new Ini_Field(cst_select_customer_before_data_entry, NO),
        POS_launch_with_duplicates = new Ini_Field(cst_launch_with_duplicates, NO),
        POS_restrict_access_to = new Ini_Field("RESTRICT_ACCESS_TO", ""),

        POS_allow_credit_note_refunds = new Ini_Field(cst_allow_credit_note_refunds, NO),
            //POS_allow_sale_of_duplicate_items = new Ini_Field(cst_allow_sale_of_duplicate_items,  NO),
        POS_allow_surcharge = new Ini_Field(cst_allow_surcharge, NONE),
        POS_allow_zero_priced_items = new Ini_Field(cst_allow_zero_priced_items, NO),
        POS_date_editable = new Ini_Field(cst_date_editable, SUPER),


        POS_search_name = new Ini_Field(cst_search_name, YES),
        POS_search_address = new Ini_Field(cst_search_address, YES),
        POS_search_phone = new Ini_Field(cst_search_phone, YES),
        POS_search_email = new Ini_Field(cst_search_email, NO),
        POS_search_contact = new Ini_Field(cst_search_contact, NO),
        POS_search_country = new Ini_Field(cst_search_country, NO),
        POS_search_case_sensitive = new Ini_Field(cst_search_case_sensitive, NO),

        /*       Weight and price barcodes        */


        POS_use_weight_barcodes = new Ini_Field(cst_use_weight_barcodes, NO),
        POS_use_price_barcodes = new Ini_Field(cst_use_price_barcodes, NO),

        POS_price_prefix_start = new Ini_Field(cst_price_prefix_start, "0"),
        POS_weight_prefix_start = new Ini_Field(cst_weight_prefix_start, "0"),

        // Caution - Modifying these default PW barcode settings 
            // by hand may cause the BarcodeStructureDialog to malfunction.
            // Let the users set them up themselves.
        POS_price_prefix = new Ini_Field(cst_price_prefix, "PRC"),
        POS_weight_prefix = new Ini_Field(cst_weight_prefix, "WGT"),

        POS_price_barcode_length = new Ini_Field(cst_price_barcode_length, "13"),
        POS_weight_barcode_length = new Ini_Field(cst_weight_barcode_length, "13"),

        POS_price_code_start = new Ini_Field(cst_price_code_start, "7"),
        POS_weight_code_start = new Ini_Field(cst_weight_code_start, "7"),

        POS_price_code_length = new Ini_Field(cst_price_code_length, "5"),
        POS_weight_code_length = new Ini_Field(cst_weight_code_length, "5"),

        POS_price_product_start = new Ini_Field(cst_price_product_start, "3"),
        POS_weight_product_start = new Ini_Field(cst_weight_product_start, "3"),

        POS_price_product_length = new Ini_Field(cst_price_product_length, "4"),
        POS_weight_product_length = new Ini_Field(cst_weight_product_length, "4"),

        POS_weight_decimals = new Ini_Field(cst_weight_decimals, "3"),


        /*       Barcodes        */


        POS_barcode1 = new Ini_Field(cst_barcode1, Sage_Fields.StockRecord.Barcode_Fields[0].Name),
        POS_barcode2 = new Ini_Field(cst_barcode2, Sage_Fields.StockRecord.Barcode_Fields[1].Name),
        POS_barcode3 = new Ini_Field(cst_barcode3, Sage_Fields.StockRecord.Barcode_Fields[2].Name),

        POS_use_barcode1 = new Ini_Field(cst_use_barcode1, YES),
        POS_use_barcode2 = new Ini_Field(cst_use_barcode2, NO),
        POS_use_barcode3 = new Ini_Field(cst_use_barcode3, NO),


        POS_cash_payments_expense_account_locked = new Ini_Field(cst_cash_payments_expense_account_locked, YES),
        POS_receipts_account_locked = new Ini_Field(cst_receipts_account_locked, NO),

        POS_def_payments_account = new Ini_Field(cst_def_payments_account, "9999"),
        POS_def_sales_bank = new Ini_Field(cst_def_sales_bank, "1200"),
        POS_def_cash_account = new Ini_Field(cst_def_cash_account, "CASH"),

        POS_lines_cancellation_allowed_to = new Ini_Field(cst_lines_cancellation_allowed_to, CASH),
        POS_invoice_cancellation_allowed_to = new Ini_Field(cst_invoice_cancellation_allowed_to, CASH),
        POS_qty_decrease_allowed_to = new Ini_Field(cst_qty_decrease_allowed_to, CASH),

        POS_discount_distribution_mode = new Ini_Field(cst_discount_distribution_mode, QTY),
        POS_display_prices_with = new Ini_Field(cst_display_prices_with, VAT),

        POS_max_line_disc_perc_cashier = new Ini_Field(cst_max_line_disc_perc_cashier, ONE_HUNDRED),
        POS_max_line_disc_perc_super = new Ini_Field(cst_max_line_disc_perc_super, ONE_HUNDRED),
        POS_max_total_disc_perc_cashier = new Ini_Field(cst_max_total_disc_perc_cashier, ONE_HUNDRED),
        POS_max_total_disc_perc_super = new Ini_Field(cst_max_total_disc_perc_super, ONE_HUNDRED),
        POS_max_rounding_discount_amount = new Ini_Field(cst_max_rounding_discount_amount, "10.00"),

        POS_printout_count_sale = new Ini_Field(cst_printout_count_sale, "3"),
        POS_printout_count_receipt = new Ini_Field(cst_printout_count_receipt, "3"),
        POS_printout_count_eos = new Ini_Field(cst_printout_count_eos, "1"),

        POS_super_pass = new Ini_Field(cst_super_pass, "123321"),

        POS_calculate_days_documents_scanning = new Ini_Field(cst_calculate_days_documents_scanning, DATE),
        POS_calculate_unposted_balance_scanning = new Ini_Field(cst_calculate_unposted_balance_scanning, UNPOSTED),

        POS_no_payment_for_transactions = new Ini_Field(cst_no_payment_for_transactions, NO),
        POS_select_cash_account_automatically = new Ini_Field(cst_select_cash_account_automatically, YES),
        POS_cache_customer_accounts = new Ini_Field(cst_cache_customer_accounts, NO),

        POS_printing_provider = new Ini_Field(cst_printing_provider, TEXT),

        POS_printing_txt_open_dir = new Ini_Field(cst_printing_txt_open_dir, YES),
        POS_printing_txt_open_file = new Ini_Field(cst_printing_txt_open_file, NO),
        POS_printing_txt_print_tags = new Ini_Field(cst_printing_txt_print_tags, NO),

        POS_printing_gdi_print_dialog = new Ini_Field(cst_printing_gdi_print_dialog, YES),
        POS_printing_gdi_page_setup = new Ini_Field(cst_printing_gdi_page_setup, YES),
        POS_printing_gdi_preview = new Ini_Field(cst_printing_gdi_preview, YES),

        POS_printing_gdi_printer = new Ini_Field(cst_printing_gdi_printer, DEFAULT),

        POS_printing_gdi_font_face = new Ini_Field(cst_printing_gdi_font_face, COURIER),
        POS_printing_gdi_font_size = new Ini_Field(cst_printing_gdi_font_size, "8"),
        POS_printing_gdi_page_width = new Ini_Field(cst_printing_gdi_page_width, "80"),
        POS_printing_gdi_dpi = new Ini_Field(cst_printing_gdi_dpi, "96"),

        POS_printing_opos_image_dpi = new Ini_Field("OPOS_IMAGE_DPI", "96"),
        POS_printing_opos_keep_open = new Ini_Field("OPOS_KEEP_OPEN", YES),
        POS_printing_opos_cut_paper_perc = new Ini_Field(cst_printing_opos_cut_paper_perc, "90"),
        POS_printing_opos_hiq_letters = new Ini_Field(cst_printing_opos_hiq_letters, NO),
        POS_printing_opos_logical_name = new Ini_Field(cst_printing_opos_logical_name, NONE),
            // "MICROSOFT POSPRINTER SIMULATOR"),

        POS_calculate_unposted = new Ini_Field(cst_calculate_unposted, NO),

        POS_document_editing_permissions = new Ini_Field(cst_document_editing_permissions, NONE),
        POS_document_viewing_permissions = new Ini_Field(cst_document_viewing_permissions, ANY),

        POS_view_product_details = new Ini_Field(cst_view_product_details, CASH),

        POS_auto_sales_history_display = new Ini_Field(cst_auto_sales_history_display, NONE),
        POS_auto_calculate_unposted_stock = new Ini_Field(cst_auto_calculate_unposted_stock, YES);


        #endregion


        /*       SES Quick Search Form        */


        public static readonly Ini_Field
        QSF_bank_sort = new Ini_Field("QSF_BANK_SORT"),
        QSF_customer_sort = new Ini_Field("QSF_CUSTOMER_SORT"),
        QSF_supplier_sort = new Ini_Field("QSF_SUPPLIER_SORT");

        /*       Interface Tools        */
        [Obsolete]
        public static readonly Ini_Field

        IT_records_module = new Ini_Field("RECORDS_MODULE"),
        IT_trans_module = new Ini_Field("TRANSACTIONS_MODULE"),
        IT_docs_module = new Ini_Field("DOCUMENTS_MODULE");



        public static readonly Ini_Field
        // FIELDS COMMON ACROSS MODULES


        IT_current_period = new Ini_Field("CURRENT_PERIOD"),
        IT_output_success_csv = new Ini_Field("OUTPUT_SUCCESS_CSV", YES),
        IT_check_bank_audit_type = new Ini_Field("CHECK_BANK_AUDIT_TYPE", YES),
        IT_change_bank_audit_type = new Ini_Field("CHANGE_BANK_AUDIT_TYPE", NO),
        IT_group_transactions_sales = new Ini_Field("GROUP_TRANSACTIONS_SALES", YES),
        IT_group_transactions_purchase = new Ini_Field("GROUP_TRANSACTIONS_PURCHASE", YES),
        IT_invoice_numbering_auto = new Ini_Field("DOCUMENTS_AUTO_NUMBERING", YES),
        IT_invoice_items_relative_grouping = new Ini_Field("ITEMS_RELATIVE_GROUPING", YES),

        // IT_sageusr_path = new Ini_Field("sageusr_path"), // USR_FILE_PATH
            // IT_serial_number = new Ini_Field("serial_number"), // PIN, i think
            // IT_sdo_activation_key = new Ini_Field("sdo_activation_key"), // let's quietly ditch these two
            // IT_sdo_serial_number = new Ini_Field("sdo_serial_number"),
            /*       company        */

        //IT_sage_username = new Ini_Field("sage_user_name"), // USERNAME
            //IT_sage_password = new Ini_Field("sage_password"),  // PASSWORD
            //IT_company_path = new Ini_Field("company_path"),    // COMPANY_PATH
            //IT_company_name = new Ini_Field("company_name"),    // COMPANY_NAME

        // FIELDS SPECIFIC TO A MODULE

        IT_dynamic_import_file_layout = new Ini_Field("DYNAMIC_IMPORT_FILE_LAYOUT", YES),
        IT_new_account_auto_date_entry = new Ini_Field("NEW_ACCOUNT_AUTO_DATE_ENTRY", YES),
        IT_blank_fields_overwrite = new Ini_Field("BLANK_FIELDS_OVERWRITE", NO),
        IT_use_mappings = new Ini_Field("USE_MAPPINGS", NO),
        IT_use_defaults = new Ini_Field("USE_DEFAULTS", YES)


        // static string def_exchange_rate = "0000000000";//"1111111111";
            // static string def_reval = "0000000000";//"1111111111";

        ;


        /* static string def_report = "1111111111";
        static string def_dynamic = "1111111111";
        static string def_auto_date = "1111111111";
        static string def_overwrite = "0000000000";
        static string def_group_sales = "1111111111";
        static string def_group_purchase = "1111111111";
        static string def_defaults = "1111111111";
        static string def_mappings = "0000000000";


        // ************************************************
        /*       FIELDS COMMON TO AT LEAST 2 APPS        */
        // ************************************************


        /*       Company Specific        */


        public static readonly Ini_Field COMPANY_PATH = new Ini_Field("COMPANY_PATH");
        // the company name as it was during registration
        public static readonly Ini_Field COMPANY_NAME = new Ini_Field("COMPANY_NAME");
        public static readonly Ini_Field COMPANY_NUMBER = new Ini_Field("COMPANY_NUMBER");
        public static readonly Ini_Field USERNAME = new Ini_Field("USERNAME");
        public static readonly Ini_Field PASSWORD = new Ini_Field("PASSWORD");
        public static readonly Ini_Field USR_FILE_PATH = new Ini_Field("PATH");


        /*       Program-wide        */


        public static readonly Ini_Field CREDENTIALS_SOURCE = new Ini_Field("CREDENTIALS_SOURCE");
        public static readonly Ini_Field ACTIVATION_KEY = new Ini_Field("ACTIVATION_KEY");
        public static readonly Ini_Field PIN = new Ini_Field("PIN");
        public static readonly Ini_Field DEFAULT_COMPANY = new Ini_Field("DEFAULT_COMPANY");


        public static readonly Ini_Field DEBUG = new Ini_Field("DEBUG");
        public static readonly Ini_Field VERSION = new Ini_Field("VERSION");

        public static readonly Ini_Field MAX_COMPANIES = new Ini_Field("MAX_COMPANIES");
        public static readonly Ini_Field NUMBER_OF_COMPANIES = new Ini_Field("NUMBER_OF_COMPANIES");

        public static readonly Ini_Field ONLY_DEFAULT_COMPANY = new Ini_Field("ONLY_DEFAULT_COMPANY", NO);

        public static readonly Ini_Field ALLOW_REMOVING_USERS = new Ini_Field("ALLOW_REMOVING_USERS", NO);


        // {\d} = new Ini_Field("
        // \1 = new Ini_Field(Global.Get_Tech_Support_Key(\1), "
        public static readonly Ini_Field
        TECH_1 = new Ini_Field(Data.Get_Tech_Support_Key(1), "InfoTrends Ltd"),
        TECH_2 = new Ini_Field(Data.Get_Tech_Support_Key(2), "16, Zenas Kanther Street"),
        TECH_3 = new Ini_Field(Data.Get_Tech_Support_Key(3), "1065 Nicosia, Cyprus"),
        TECH_4 = new Ini_Field(Data.Get_Tech_Support_Key(4), "Mail Address:"),
        TECH_5 = new Ini_Field(Data.Get_Tech_Support_Key(5), "P.O. Box 27778"),
        TECH_6 = new Ini_Field(Data.Get_Tech_Support_Key(6), "2433 Nicosia, Cyprus"),
        TECH_7 = new Ini_Field(Data.Get_Tech_Support_Key(7), "Tel: +357 99 677133"),
        TECH_8 = new Ini_Field(Data.Get_Tech_Support_Key(8), "Fax: +357 22 666033");


        /* field name constants (why???) */
        public const string
        #region
 cst_confirm_invoice = "CONFIRM_INVOICE",
        cst_confirm_receipt = "CONFIRM_RECEIPT",
        cst_confirm_payment = "CONFIRM_PAYMENT",
        cst_max_details_history_length = "MAX_DETAILS_HISTORY_LENGTH",

        cst_auto_calculate_unposted_stock = "AUTO_CALCULATE_UNPOSTED_STOCK_BALANCE",

        // NONE, DATE, NUMBER, ALL !!! NOT ANY
        cst_auto_sales_history_display = "AUTO_SALES_HISTORY_DISPLAY",


        cst_view_product_details = "VIEW_PRODUCT_DETAILS",

        cst_document_viewing_permissions = "DOCUMENT_VIEWING_PERMISSIONS",

        cst_document_editing_permissions = "DOCUMENT_EDITING_PERMISSIONS",

        cst_calculate_unposted = "AUTO_CALCULATE_UNPOSTED_BALANCE",

        cst_printing_opos_hiq_letters = "OPOS_PRINTING_HI_QUALITY_LETTERS",
        cst_printing_opos_logical_name = "OPOS_PRINTING_LOGICAL_NAME",
        cst_printing_opos_cut_paper_perc = "OPOS_PAPER_CUT_PERC",


        cst_printing_gdi_dpi = "GDI_PRINTER_RESOLUTION",
        cst_printing_gdi_printer = "GDI_PRINTING_PRINTER",
        cst_printing_gdi_preview = "GDI_PRINTING_SHOW_PREVIEW",
        cst_printing_gdi_page_setup = "GDI_PRINTING_SHOW_PAGE_SETUP",
        cst_printing_gdi_print_dialog = "GDI_PRINTING_SHOW_PRINT_DIALOG",
        cst_printing_gdi_font_face = "GDI_PRINTING_FONT_FACE",
        cst_printing_gdi_font_size = "GDI_PRINTING_FONT_SIZE",
        cst_printing_gdi_page_width = "GDI_PRINTING_PAGE_WIDTH",



        cst_printing_txt_print_tags = "TXT_PRINTING_PRINT_TAGS",
        cst_printing_txt_open_file = "TXT_PRINTING_OPEN_FILE",
        cst_printing_txt_open_dir = "TXT_PRINTING_OPEN_DIR",

        cst_printing_provider = "PRINTING_PROVIDER",

        cst_price_reverse_calculate = "PRICE_BARCODE_REVERSE_CALCULATE_QTY",

        cst_price_prefix_start = "PRICE_PREFIX_START",
        cst_weight_prefix_start = "WEIGHT_PREFIX_START",

        cst_price_barcode_length = "PRICE_BARCODE_LENGTH",
        cst_weight_barcode_length = "WEIGHT_BARCODE_LENGTH",

        cst_weight_decimals = "WEIGHT_DECIMALS",

        cst_weight_code_length = "WEIGHT_CODE_LENGTH",
        cst_price_code_length = "PRICE_CODE_LENGTH",

        cst_weight_code_start = "WEIGHT_CODE_START",
        cst_price_code_start = "PRICE_CODE_START",

        cst_weight_prefix = "WEIGHT_PREFIX",
        cst_price_prefix = "PRICE_PREFIX",

        cst_price_product_length = "PRICE_PRODUCT_LENGTH",
        cst_weight_product_length = "WEIGHT_PRODUCT_LENGTH",

        cst_price_product_start = "PRICE_PRODUCT_START",
        cst_weight_product_start = "WEIGHT_PRODUCT_START",

        cst_use_price_barcodes = "USE_PRICE_BARCODES",
        cst_use_weight_barcodes = "USE_WEIGHT_BARCODES",


        cst_start_maximized = "START_MAXIMIZED",

        cst_cash_payments_expense_account_locked = "CASH_PAYMENTS_EXPENSE_ACCOUNT_LOCKED",

        cst_receipts_account_locked = "RECEIPTS_EXPENSE_ACCOUNT_LOCKED",


        cst_z_ser_number = "Z_SER_NUMBER",

        /// <summary> dd/MM/yyyy </summary>
        cst_last_date_eos_ran = "LAST_DATE_EOS_RAN",

        cst_launch_with_duplicates = "LAUNCH_WITH_DUPLICATES",

        // Cash/Super/None
        cst_date_editable = "DATE_EDITABLE",

        cst_select_customer_before_data_entry = "SELECT_CUSTOMER_BEFORE_DATA_ENTRY",

        cst_search_address = "SEARCH_ADDRESS",
        cst_search_phone = "SEARCH_PHONE",
        cst_search_email = "SEARCH_EMAIL",
        cst_search_contact = "SEARCH_CONTACT",
        cst_search_name = "SEARCH_NAME",
        cst_search_country = "SEARCH_COUNTRY",
        cst_search_case_sensitive = "SEARCH_CASE_SENSITIVE",

        cst_def_cash_account = "DEF_CASH_ACCOUNT",
        cst_def_sales_bank = "DEF_SALES_BANK",
        cst_def_payments_account = "DEF_CASH_PAYMENTS_EXPENSE_ACCOUNT",


        cst_allow_credit_note_refunds = "ALLOW_CREDIT_NOTE_REFUNDS",
        cst_allow_surcharge = "ALLOW_SURCHARGE",

        cst_allow_zero_priced_items = "ALLOW_ZERO_PRICED_ITEMS",


        cst_barcode1 = "BARCODE1",
        cst_barcode2 = "BARCODE2",
        cst_barcode3 = "BARCODE3",

        cst_use_barcode1 = "USE_BARCODE1",
        cst_use_barcode2 = "USE_BARCODE2",
        cst_use_barcode3 = "USE_BARCODE3",

        // QTY or TOTAL
        cst_discount_distribution_mode = "DISCOUNT_DISTRIBUTION_MODE",
            // VAT or NVAT
        cst_display_prices_with = "DISPLAY_PRICES_WITH",

        cst_invoice_cancellation_allowed_to = "INVOICE_CANCELLATION_ALLOWED_TO",
        cst_lines_cancellation_allowed_to = "LINES_CANCELLATION_ALLOWED_TO",

        cst_max_line_disc_perc_cashier = "MAX_LINE_DISC_PERC_CASHIER",
        cst_max_line_disc_perc_super = "MAX_LINE_DISC_PERC_SUPER",
        cst_max_rounding_discount_amount = "MAX_ROUNDING_DISCOUNT_AMOUNT",
        cst_max_total_disc_perc_cashier = "MAX_TOTAL_DISC_PERC_CASHIER",
        cst_max_total_disc_perc_super = "MAX_TOTAL_DISC_PERC_SUPER",

        cst_num_digits_in_prefix = "NUM_DIGITS_IN_PREFIX",


        cst_pos_id = "POS_NUMBER",
        cst_printout_count_sale = "PRINTOUT_COUNT_SALE",
        cst_printout_count_receipt = "PRINTOUT_COUNT_RECEIPT",
        cst_printout_count_eos = "PRINTOUT_COUNT_EOS",

        cst_qty_decrease_allowed_to = "QTY_DECREASE_ALLOWED_TO",


        cst_super_pass = "SUPER_PASS",

        cst_select_cash_account_automatically = "SELECT_CASH_ACCOUNT_AUTOMATICALLY",
        cst_cache_customer_accounts = "CACHE_CUSTOMER_ACCOUNTS",
        cst_no_payment_for_transactions = "NO_PAYMENT_FOR_TRANSACTIONS",

        // ACCOUNT or UNPOSTED
        cst_calculate_unposted_balance_scanning = "CALCULATE_UNPOSTED_BALANCE_SCANNING",

        // POSTED_BY or DATE
        cst_calculate_days_documents_scanning = "CALCULATE_DAYS_DOCUMENTS_SCANNING";



        [Obsolete]
        public const string
        cst_allow_sale_of_duplicate_items = "ALLOW_SALE_OF_DUPLICATE_ITEMS",

        cst_rounding_discounts_allowed_to = "ROUNDING_DISCOUNTS_ALLOWED_TO",

        cst_line_discounts_allowed_to = "LINE_DISCOUNTS_ALLOWED_TO",

        cst_total_discounts_allowed_to = "TOTAL_DISCOUNTS_ALLOWED_TO";

        #endregion



        /*       Indices        */
        // KEEP AT THE BOTTOM!
        

        public static readonly Set<Ini_Field>
        Tech_Support = new Set<Ini_Field> 
        {
            TECH_1 ,
            #region 
            TECH_2 ,
            TECH_3 ,
            TECH_4 ,
            TECH_5 ,
            TECH_6 ,
            TECH_7 ,
            TECH_8 , 
            #endregion
        },

        General_Settings = new Set<Ini_Field>
        {
            ACTIVATION_KEY,
            #region 
            PIN,
            DEFAULT_COMPANY,

            MODULE_1,
            MODULE_2,
            MODULE_3,

            USR_FILE_PATH,
            VERSION,
            NUMBER_OF_COMPANIES,
            ONLY_DEFAULT_COMPANY,
            ALLOW_REMOVING_USERS,
            //{COMPANY_PATH, Required},
            //{COMPANY_NAME, Required},
            //{COMPANY_NUMBER, Required},

            //{SESDTA, Required},

            //{USERNAME, Required},
            //{PASSWORD, Required},
            #endregion
        },

        POS_Fields = new Set<Ini_Field>
        {
            POS_end_of_day_security,
            POS_price_reverse_calculate,
            #region
            POS_restrict_access_to,
            POS_avg_cost_calculation_mode,
            POS_start_maximized,
            POS_pos_id ,
            POS_z_ser_number,
            POS_last_date_eos_ran,

            POS_select_customer_before_data_entry,
            POS_launch_with_duplicates,

            POS_allow_credit_note_refunds,
            //POS_allow_sale_of_duplicate_items,
            POS_allow_surcharge,
            POS_allow_zero_priced_items,
            POS_date_editable,


            POS_search_name,
            POS_search_address,
            POS_search_phone,
            POS_search_email,
            POS_search_contact,
            POS_search_country,
            POS_search_case_sensitive,

            /*       Weight and price barcodes        */


            POS_use_weight_barcodes ,
            POS_use_price_barcodes ,

            POS_price_prefix_start,
            POS_weight_prefix_start,

            // Caution - Modifying these default PW barcode settings 
            // by hand may cause the BarcodeStructureDialog to malfunction.
            // Let the users set them up themselves.
            POS_price_prefix,
            POS_weight_prefix,

            POS_price_barcode_length,
            POS_weight_barcode_length,

            POS_price_code_start,
            POS_weight_code_start,

            POS_price_code_length,
            POS_weight_code_length,

            POS_price_product_start ,
            POS_weight_product_start ,

            POS_price_product_length,
            POS_weight_product_length,

            POS_weight_decimals,


            /*       Barcodes        */


            POS_barcode1,
            POS_barcode2,
            POS_barcode3,

            POS_use_barcode1,
            POS_use_barcode2,
            POS_use_barcode3,


            POS_cash_payments_expense_account_locked,
            POS_receipts_account_locked,

            POS_def_payments_account ,
            POS_def_sales_bank,
            POS_def_cash_account,

            POS_lines_cancellation_allowed_to,
            POS_invoice_cancellation_allowed_to,
            POS_qty_decrease_allowed_to,

            POS_discount_distribution_mode,
            POS_display_prices_with,

            POS_max_line_disc_perc_cashier ,
            POS_max_line_disc_perc_super ,
            POS_max_total_disc_perc_cashier ,
            POS_max_total_disc_perc_super ,
            POS_max_rounding_discount_amount ,

            POS_printout_count_sale ,
            POS_printout_count_receipt ,
            POS_printout_count_eos,

            POS_super_pass ,

            POS_calculate_days_documents_scanning,
            POS_calculate_unposted_balance_scanning,

            POS_no_payment_for_transactions,
            POS_select_cash_account_automatically,
            POS_cache_customer_accounts,

            POS_printing_provider,

            POS_printing_txt_open_dir,
            POS_printing_txt_open_file,
            POS_printing_txt_print_tags,

            POS_printing_gdi_print_dialog,
            POS_printing_gdi_page_setup,
            POS_printing_gdi_preview,

            POS_printing_gdi_printer,

            POS_printing_gdi_font_face,
            POS_printing_gdi_font_size,
            POS_printing_gdi_page_width,
            POS_printing_gdi_dpi,

            POS_printing_opos_cut_paper_perc,
            POS_printing_opos_hiq_letters,
            POS_printing_opos_logical_name,

            POS_calculate_unposted,

            POS_document_editing_permissions,
            POS_document_viewing_permissions,

            POS_view_product_details,

            POS_auto_sales_history_display,
            POS_auto_calculate_unposted_stock,
            
            POS_confirm_invoice,
            POS_confirm_receipt,
            POS_confirm_payment,

            TE_max_details_history_length, 
            #endregion
        },

        Entry_Screens_Fields = new Set<Ini_Field>
        {
            ESF_purchase_discount_details,
            ESF_purchase_receipt_details,
            ESF_sales_discount_details,
            ESF_sales_receipt_details,
            ESF_post_remittance_by_number,
        },

        Interface_Tools_Fields_General = new Set<Ini_Field>
        {
            IT_current_period ,
            IT_output_success_csv ,
            IT_check_bank_audit_type ,
            IT_change_bank_audit_type ,
            IT_group_transactions_sales ,
            IT_group_transactions_purchase ,

           /* IT_records_module,
            IT_trans_module,
            IT_docs_module, */

        },

        Interface_Tools_Fields_Specific = new Set<Ini_Field>
        {
            IT_dynamic_import_file_layout ,
            IT_new_account_auto_date_entry ,
            IT_blank_fields_overwrite ,
            IT_use_mappings ,
            IT_use_defaults ,
        }
        ;

    }
}
