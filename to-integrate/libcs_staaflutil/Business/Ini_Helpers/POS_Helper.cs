using System;
using System.Collections.Generic;
using Common;

using Fairweather.Service;

namespace DTA
{
    public class POS_Helper : Ini_Helper
    {
        public POS_Helper(Ini_File ini, 
                          Company_Number company,
                          ICurrent_User_Mode form)
            : base(ini, company) {

            this.m_company = company;
            this.m_form = form;

        }

        readonly ICurrent_User_Mode m_form;
        readonly Company_Number m_company;

        public Company_Number Company_Number {
            get { return m_company; }
        }
        public Printing_Helper Printing_Helper(Printing_Scenario scenario) {
            return new Printing_Helper(Data.Ini_File, m_company, scenario);
        }

        static readonly Dictionary<string, Document_Permissions>
        str_to_dp = new Dictionary<string, Document_Permissions>
        {
            // {DTA_Main.NONE, Document_Editing_Permissions.None},
            {Ini_Main.ANY, Document_Permissions.Any},
            {Ini_Main.ANY_POS, Document_Permissions.Any_Pos},
            {Ini_Main.SAME_POS, Document_Permissions.Same_Pos},
        };

        public Document_Permissions Document_Viewing_Permissions {
            get {
                var str = String(DTA_Fields.POS_document_viewing_permissions);

                var ret = str_to_dp.Get_Or_Default(str, () => Document_Permissions.None);

                return ret;
            }
        }

        public Document_Permissions Document_Editing_Permissions {
            get {
                var str = String(DTA_Fields.POS_document_editing_permissions);

                var ret = str_to_dp.Get_Or_Default(str, () => Document_Permissions.None);

                return ret;
            }
        }

        public Set<string>
        Restricted_Sales_Accounts {
            get {
                return Set(DTA_Fields.POS_restrict_access_to);
            }
        }

        public bool
        Auto_Calculate_Unposted_Stock {
            get {
                return True(DTA_Fields.POS_auto_calculate_unposted_stock);
            }
        }

        public Auto_Sales_History_Mode
        Auto_Sales_History_Display {
            get {
                string in_ini = String(DTA_Fields.POS_auto_sales_history_display);

                var ret =
                new Dictionary<string, Auto_Sales_History_Mode>
                {
                    {Ini_Main.NONE,Auto_Sales_History_Mode.None},
                    {Ini_Main.NUMBER,Auto_Sales_History_Mode.By_Num},
                    {Ini_Main.DATE,Auto_Sales_History_Mode.By_Date},
                    {Ini_Main.ALL,Auto_Sales_History_Mode.All},
                }.Get_Or_Default_(in_ini, Auto_Sales_History_Mode.None);

                return ret;
            }
        }


        public Average_Cost_Type Average_Cost_Type {
            get {
                return String(DTA_Fields.POS_avg_cost_calculation_mode) == Ini_Main.ALL
                                                                        ? Average_Cost_Type.All
                                                                        : Average_Cost_Type.Purchases_Only;
            }
        }

        public bool Calculate_Unposted {
            get {
                return True(DTA_Fields.POS_calculate_unposted);
            }
        }

        public bool Cache_Customer_Accounts {
            get {
                return True(DTA_Fields.POS_cache_customer_accounts);
            }
        }

        public bool Allow_Zero_Priced_Items {
            get {
                return True(DTA_Fields.POS_allow_zero_priced_items);
            }
        }

        public bool Allow_Credit_Note_Refunds {
            get {
                return True(DTA_Fields.POS_allow_credit_note_refunds);
            }
        }

        public bool Receipts_Account_Locked {
            get {
                return True(DTA_Fields.POS_receipts_account_locked);
            }
        }

        public bool No_Payment_For_Transactions {
            get {
                return True(DTA_Fields.POS_no_payment_for_transactions);
            }
        }

        public string Pos_ID {
            get {
                return String(DTA_Fields.POS_pos_id);
            }
        }

        public bool Cash_Account_Selected_Automatically {
            get {
                return True(DTA_Fields.POS_select_cash_account_automatically);
            }
        }

        public string Cash_Account {
            get {
                var ret = m_proxy[DTA_Fields.POS_def_cash_account];

                return ret;
            }
        }

        public string Default_Expense_Cash_Account {
            get {
                var ret = m_proxy[DTA_Fields.POS_def_payments_account];
                return ret;
            }
        }

        public bool Expense_Account_Locked {
            get {
                var ret = True(DTA_Fields.POS_cash_payments_expense_account_locked);

                return ret;
            }
        }

        public bool Discard_Invoice_Allowed {
            get {
                var ret = Is_Permitted(DTA_Fields.POS_invoice_cancellation_allowed_to);
                return ret;
            }
        }

        public bool End_Of_Day_Allowed {
            get {
                var ret = Is_Permitted(DTA_Fields.POS_end_of_day_security);
                return ret;
            }
        }

        public bool End_Of_Day_Allowed_to_Super {
            get {
                var ret = Is_Permitted(DTA_Fields.POS_end_of_day_security, User_Level.Super);
                return ret;
            }
        }

        public bool Remove_Row_Allowed {
            get {
                var ret = Is_Permitted(DTA_Fields.POS_lines_cancellation_allowed_to);
                return ret;
            }
        }

        public bool Date_Editable_for_Supervisor {
            get {

                var ret = Is_Permitted(DTA_Fields.POS_date_editable, User_Level.Super);

                return ret;
            }
        }

        public bool Date_Editable {
            get {

                var ret = Is_Permitted(DTA_Fields.POS_date_editable);

                return ret;
            }
        }

        public bool Qty_Decrease_Allowed {
            get {
                var ret = Is_Permitted(DTA_Fields.POS_qty_decrease_allowed_to);

                return ret;
            }
        }

        public bool View_Product_Details_Allowed {
            get {
                return Is_Permitted(DTA_Fields.POS_view_product_details);
            }
        }

        public bool Surcharge_Allowed {
            get {
                return Is_Permitted(DTA_Fields.POS_allow_surcharge);
            }
        }

        public bool Launch_With_Duplicates {
            get {
                return True(DTA_Fields.POS_launch_with_duplicates);
            }
        }

        public string Our_Company_Directory {
            get {
                var ret = Data.Get_Company_Directory(m_company);
                return ret;
            }
        }

        public string Duplicates_Log_Path {
            get {
                var ret = Our_Company_Directory.Cpath(Data.DUPLICATES_LOG);
                return ret;
            }
        }

        bool Is_Permitted(Ini_Field field) {

            var current = m_form.Current_User_Mode;

            bool ret = Is_Permitted(field, current);

            return ret;


        }

        bool Is_Permitted(Ini_Field field, User_Level current) {

            var str = m_proxy[field];

            var required = String_To_Privilege(str);

            bool ret = Is_Permitted(required, current);

            return ret;


        }


        bool Is_Permitted(User_Level required, User_Level current) {


            if (required == User_Level.None)
                return false;

            if (required == User_Level.Cash)
                return true;

            if (required == User_Level.Super) {

                return current == User_Level.Super;

            }

            true.tift();
            throw new InvalidOperationException();

        }

        User_Level String_To_Privilege(string str) {

            User_Level ret = 0;

            if (str == CASH) {
                ret = User_Level.Cash;
            }

            if (str == SUPER) {
                ret = User_Level.Super;
            }

            if (str == NONE) {
                ret = User_Level.None;
            }
            (ret == 0).tift();

            return ret;

        }

        public int Printout_Count_Sale {
            get {
                var str = m_proxy[DTA_Fields.POS_printout_count_sale];

                var ret = int.Parse(str);

                return ret;
            }
        }

        public int Printout_Count_Receipt {
            get {
                var str = m_proxy[DTA_Fields.POS_printout_count_receipt];

                var ret = int.Parse(str);

                return ret;
            }
        }

        public int Printout_Count_End_Of_Shift {
            get {
                var str = m_proxy[DTA_Fields.POS_printout_count_eos];

                var ret = int.Parse(str);

                return ret;
            }
        }

        public bool Select_Customer_Before_Products {
            get {
                var ret = m_proxy[DTA_Fields.POS_select_customer_before_data_entry] == YES;

                return ret;
            }
        }

        public bool Display_Prices_VAT {
            get {
                string value = m_proxy[DTA_Fields.POS_display_prices_with];

                bool ret = (value == VAT || value == BOTH);

                return ret;
            }
        }

        public bool Display_Prices_NVAT {
            get {
                string value = m_proxy[DTA_Fields.POS_display_prices_with];

                bool ret = (value == NVAT || value == BOTH);

                return ret;
            }
        }

        public bool Start_Maximized {
            get {
                return True(DTA_Fields.POS_start_maximized);
            }
        }

        public bool Reverse_Calculate_QTY {
            get {
                return True(DTA_Fields.POS_price_reverse_calculate);
            }
        }

        public bool Do_Not_Confirm_Invoice {
            get { return True(DTA_Fields.POS_confirm_invoice); }
        }

        public bool Do_Not_Confirm_Receipt {
            get { return True(DTA_Fields.POS_confirm_receipt); }
        }

        public bool Do_Not_Confirm_Payment {
            get { return True(DTA_Fields.POS_confirm_payment); }
        }


        /*
        readonly DiscountsClass m_discounts = null;

        public DiscountsClass Discounts {
            get { return m_discounts; }
        }


        public class DiscountsClass
        {
            readonly POS_Helper m_outer;
            public DiscountsClass(POS_Helper outer) {
                m_outer = outer;
            }


        }*/
    }
}
