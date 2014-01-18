using System;
using System.Collections.Generic;
using Fairweather.Service;

using Versioning;

namespace Common
{
    public enum Command_Line_Switch { }
    public static class Enum_Service
    {
        public static string Friendly_String(this Payment_Type type) {

            var ret = type.Get_String().Replace("_", " ").Replace("que", "ques").Replace("ost ", "ost-");

            return ret;
        }
        public static string Friendly_String(this EOS_Breakdown_Type type) {

            var ret = type.Get_String().Replace("_", " ");

            return ret;
        }

        static Dictionary<Sub_App, string>
        Sub_App_Friendly = new Dictionary<Sub_App, string>
        {
            {Sub_App.Products, "Point Of Sales"},
            {Sub_App.Ses_Cfg, "Sage Entry Screens Configuration"},
            {Sub_App.Entry_Customers, "Customer Receipts"},
            {Sub_App.Entry_Suppliers, "Supplier Payments"},
            //{Sub_App.Documents_Transfer, "Documents Transfer"},

        };

        public static string Friendly_String(this Sub_App module) {

            var ret = Sub_App_Friendly.Get_Or_Default(module, () => module.Get_String().Replace("_", " "));

            return ret;

        }


        const int _flag1 = 0x1;
        const int _flag2 = 2 * _flag1;
        const int _flag3 = 2 * _flag2;
        const int _flag4 = 2 * _flag3;
        const int _flag5 = 2 * _flag4;
        const int _flag6 = 2 * _flag5;

        const int flag1 = 0x00100000;
        const int flag2 = 2 * flag1;
        const int flag3 = 2 * flag2;
        const int flag4 = 2 * flag3;
        const int flag5 = 2 * flag4;
        const int flag6 = 2 * flag5;

        public const int
      Command_Line_Status_Valid = flag1,
      Command_Line_Status_Has_Company = flag2 | Command_Line_Status_Valid,
      Command_Line_Status_Has_Company_Username_Password = flag3 | Command_Line_Status_Has_Company;

        public const int SES_suite = flag1;
        public const int SIT_suite = flag2;

        public const int SES_app = flag1;
        public const int SESCFG_app = flag2;

        public const int SIT_app = flag1;

        public const int Switch_Takes_Param = flag1;



        public static bool Is_Valid(this Command_Line_Status status) {

            return status.Contains(Command_Line_Status_Valid);

        }

        public static App Application(this Sub_App status) {

            if (status.Contains(App.Entry_Screens))
                return App.Entry_Screens;

            if (status.Contains(App.Ses_Cfg))
                return App.Ses_Cfg;

            if (status.Contains(App.Sit_Exe))
                return App.Sit_Exe;

            true.tift();
            throw new InvalidOperationException();

        }

        public static bool Takes_Parameter(this Command_Line_Switch cswitch) {

            return cswitch.Contains(Switch_Takes_Param);

        }


        public static int Return(this Return_Code code) {

            return (int)code;

        }

        public static int MaxLength(this Record_Type type) {

            var dict = new Dictionary<Record_Type, int>
            {
                {Record_Type.Sales, 8},
                {Record_Type.Purchase, 8},
                {Record_Type.Bank, 8},
                {Record_Type.Stock, 30},
                {Record_Type.Department, 4},
                {Record_Type.Expense, 8},
                {Record_Type.TT, 2},
                {Record_Type.Tax_Code, 3},
                {Record_Type.Project, 8},
                {Record_Type.Cost_Code, 8},
            };

            return dict[type];

        }

        static public string Friendly_String(this Receipt_Layout layout) {

            return new Dictionary<Receipt_Layout, string>{
                 {Receipt_Layout.Standard_Vertical, "Standard Vertical"}}[layout];

        }

        static readonly Twoway<Document_Type, InvoiceType>
        doc_to_invoice = new Twoway<Document_Type, InvoiceType> 
        {
            {Document_Type.Product_Invoice, InvoiceType.sdoProductInvoice},
            {Document_Type.Product_Credit, InvoiceType.sdoProductCredit},

        };

        static readonly Twoway<Document_Type, int>
        doc_to_combo_box_index = new Twoway<Document_Type, int> 
        {
            {Document_Type.Product_Invoice, 0},
            {Document_Type.Product_Credit, 1},

        };

        static readonly Twoway<Document_Type, Grid_Mode>
        doc_to_grid = new Twoway<Document_Type, Grid_Mode> 
        {
            {Document_Type.Product_Invoice, Grid_Mode.Invoice },
            {Document_Type.Product_Credit, Grid_Mode.Credit_Note},
        };

        public static Document_Type To_Document_Type(this InvoiceType invoice_type) {
            return doc_to_invoice[invoice_type];
        }

        public static Grid_Mode To_Grid_Mode(this Document_Type document_type) {
            return doc_to_grid[document_type];
        }

        public static Document_Type To_Document_Type(this Grid_Mode grid_mode) {

            Document_Type ret;

            if (grid_mode == Grid_Mode.Invoice_Against_Credit_Note)
                ret = Document_Type.Product_Invoice;

            else
                ret = doc_to_grid[grid_mode];

            return ret;
        }

        public static InvoiceType To_Invoice_Type(this Document_Type document_type) {
            return doc_to_invoice[document_type];
        }

        public static int To_Combobox_Index(this Document_Type document_type) {
            return doc_to_combo_box_index[document_type];

        }

        public static Document_Type From_Combo_Box_Index(int index) {
            return doc_to_combo_box_index[index];

        }

        public static bool Own_Msg(this Sage_Connection_Error error) {
            return false;
        }
        public static bool Show_Msg(this Sage_Connection_Error error) {
            return true;
        }

        public static bool Try_Again_Later(this Sage_Connection_Error error) {
            return error != Sage_Connection_Error.Invalid_Credentials;
        }

        public static bool Can_Retry(this Sage_Connection_Error error) {
            //static readonly Set<Sage_Connection_Error>
            //can_retry = new Set<Sage_Connection_Error> { Sage_Connection_Error.Exclusive_Mode };
            return error == Sage_Connection_Error.Exclusive_Mode
                || error == Sage_Connection_Error.Username_In_Use_Generic
                || error == Sage_Connection_Error.Logon_Count_Exceeded
                || error == Sage_Connection_Error.Unspecified
                ;
        }

    }
}