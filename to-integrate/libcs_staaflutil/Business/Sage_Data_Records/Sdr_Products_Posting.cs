using System;
using System.Collections.Generic;
using System.Linq;
using Fairweather.Service;
using Standardization;
using Versioning;


namespace Common
{
    /*       Posting of:        */
    /*        * Sales Invoices        */
    /*        * Receipts on Account        */
    /*        * Expense Payments        */



    public partial class Sage_Logic
    {
        /*       SALES        */

        void Fill_Sale_Objects(Posting_Info post_info,
                               decimal paid,
                               Item_Info[] items,

                              out List<string> header_M_sales,
                              out Dictionary<string, object> header_alone,
                              out List<string> invoice_item_M_stock_record,
                              out List<Dictionary<string, object>> invoice_item_alone) {

            Screen_Info screen_info = post_info.Screen_Info;
            Payment_Info pay_info = post_info.Payment_Info;
            Cheque_Info?[] cheques = pay_info.Six_Cheques;

            header_M_sales = new List<string>()
                {
                    #region 16 lines

                    ACCOUNT_REF, 
                    NAME,        
                    CONTACT_NAME,    
                    // DEF_TAX_CODE, // not available in ver 11

                    #endregion
                };

            header_alone = new Dictionary<string, object>()
                {
                    #region 10 lines  

                    {ADDRESS_1,   /* => */ screen_info.Address_Lines[0]},
                    {ADDRESS_2,   /* => */ screen_info.Address_Lines[1]},
                    {ADDRESS_3,   /* => */ screen_info.Address_Lines[2]},
                    {ADDRESS_4,   /* => */ screen_info.Address_Lines[3]},
                    {ADDRESS_5,   /* => */ screen_info.Address_Lines[4]},
                    {CUST_TEL_NUMBER, /* => */ screen_info.Telephone},
                    
                    {INVOICE_DATE, 			 screen_info.Date.Date}, //<-- Required	
                    {INVOICE_TYPE_CODE, 	 screen_info.Invoice_Type}, //<-- Required
                    {ORDER_NUMBER, 			 screen_info.Order_Number}, 	

                    // See Cheque_Info.ToString()
                    {DELIVERY_NAME,   /* => */ (string)cheques[0]},
                    {DEL_ADDRESS_1,   /* => */ (string)cheques[1]},
                    {DEL_ADDRESS_2,   /* => */ (string)cheques[2]},
                    {DEL_ADDRESS_3,   /* => */ (string)cheques[3]},
                    {DEL_ADDRESS_4,   /* => */ (string)cheques[4]},
                    {DEL_ADDRESS_5,   /* => */ (string)cheques[5]},

                    {NOTES_1,         /* => */ pay_info.Cash.ToString(true)},
                    {NOTES_2,         /* => */ pay_info.Credit.ToString(true)},
                    {NOTES_3,         /* => */ pay_info.Gift.ToString(true)},
		
                    // For the receipt
                    {AMOUNT_PREPAID,       /* => */ paid},
                    {BASE_AMOUNT_PAID,     /* => */ paid},
                    {BANK_CODE,            /* => */ Get_Default_Sales_Bank()},
                    {PAYMENT_TYPE,                  TransType.sdoSR},

                    // Not ATM
                    //{"PAYMENT_REF",                   },
                    //{"FOREIGN_GROSS",            screen_info.Gross}, 

                    #endregion
                };

            invoice_item_M_stock_record = new List<string>()
                {
                    #region 4 lines  

                   DEPT_NUMBER,  
                   NOMINAL_CODE, 
                   TAX_CODE,     
                   UNIT_OF_SALE, 

                    #endregion
                };

            invoice_item_alone = new List<Dictionary<string, object>>(items.Length);

            for (int ii = 0; ii < items.Length; ++ii) {

                var item = items[ii];
                var product = item.Product;

                invoice_item_alone.Add(new Dictionary<string, object>()
                {
                    #region 10 lines  
                   //;;;
                   {STOCK_CODE,         product.Code},
                   {DESCRIPTION,        item.Description},
                   
                   {FULL_NET_AMOUNT,	item.Adjusted_Brute_NV}, // before disc
                   {NET_AMOUNT,			item.Total_NV},         // after disc

                   {TAX_AMOUNT,         item.VAT},
                   {QTY_ORDER,			item.Qty},
                   {UNIT_PRICE,		    item.Adjusted_Base_Price_NV}, // BASE_NV or BASE_NV + SURCHARGE_NV

                   {ITEM_NUMBER,        ii},
                   {TAX_RATE,           product.VAT_Percentage},

                   //{DISCOUNT_RATE,      item.Discount_Rate}, // <-- is not populated automatically
                   {DISCOUNT_AMOUNT,    item.Total_Discount_NV}, // <-- is not populated automatically
                   {ADD_DISC_RATE,      item.Discount_Perc},
                   //{DISCOUNT_AMOUNT,    item.Discount_Rate}, // [sic]

                   // hack starts here
                   //{NETVALUE_DISCOUNT, item.Total_Discount_NV},
                   //{BASE_NET, item.Total_NV},
                   //{BASE_NETVALUE_DISCOUNT, item.Total_Discount_NV},
                   //{BASE_FULL_NET, item.Adjusted_Brute_NV},


                    #endregion

                });
            }
        }



        /*       Called by: Process_Sale        */

        public bool Post_Sale(Posting_Info post_info,
                                         decimal paid,
                                         Item_Info[] items,
                                         string pos_id,

                                         Sage_Invoice_Data? sage_invoice_data /* in case of editing */,
                                         out int number) {


            var screen_info = post_info.Screen_Info;
            var payment_info = post_info.Payment_Info;

            (paid == payment_info.Amount_Prepaid).tiff();


            SalesRecord oSalesRecord;
            InvoicePost oInvoicePost;

            Fields oHeader; //
            InvoiceItem oInvoiceItem;
            StockRecord oStockRecord;

            List<string> header_M_sales;
            Dictionary<string, object> header_alone;
            List<string> invoice_item_M_stock_record;
            List<Dictionary<string, object>> invoice_item_alone;

            Fill_Sale_Objects(post_info,
                                  paid,
                                  items,

                                  out header_M_sales,
                                  out header_alone,
                                  out invoice_item_M_stock_record,
                                  out invoice_item_alone);

            // this field is populated from screen_info.Invoice_Type:
            // header_alone.ContainsKey("INVOICE_TYPE_CODE").tiff();

            using (Establish_Connection()) {

                oSalesRecord = WS.Create<SalesRecord>();
                oInvoicePost = WS.Create<InvoicePost>();

                oSalesRecord[ACCOUNT_REF] = screen_info.Account_Ref;

                Try(oSalesRecord.Find(false));

                // Important bit
                oInvoicePost.Type = screen_info.Ledger_Type;

                oHeader = (Fields)oInvoicePost.HDGet();


                {
                    oHeader[TAKEN_BY] = pos_id;

                    // Stuffing oHeader with data
                    oHeader.Fill(oSalesRecord, header_M_sales);

                    oHeader.Fill(header_alone);
                }

                foreach (var item_info in invoice_item_alone) {

                    oInvoiceItem = oInvoicePost.AddItem();

                    oStockRecord = WS.Create<StockRecord>();

                    oStockRecord[STOCK_CODE] = item_info[STOCK_CODE];

                    Try(oStockRecord.Find(false));

                    // Stuffing oInvoiceItem with data

                    oInvoiceItem.Fill(oStockRecord, invoice_item_M_stock_record);

                    oInvoiceItem.Fill(item_info);
                }


                if (sage_invoice_data.HasValue) {

                    /* delete older document */

                    number = sage_invoice_data.Value.Number;

                    var oInvoiceRecord = WS.Create<InvoiceRecord>();
                    oInvoiceRecord[INVOICE_NUMBER] = number;

                    if (oInvoiceRecord.Find(false)) {
                        oHeader[PRINTED_CODE] = oInvoiceRecord[PRINTED_CODE];
                        oInvoiceRecord.Remove().tiff();
                    }
                }
                else
                // Second important bit
                {   // We need to make sure the document numbers remain consecutive
                    // even if there are previous deleted documents, therefore we will
                    // assign the invoice numbers ourselves

                    var oInvoiceRecord = WS.Create<InvoiceRecord>();

                    if (oInvoiceRecord.MoveLast())
                        number = oInvoiceRecord[INVOICE_NUMBER].ToInt32() + 1;
                    else
                        number = oInvoicePost.GetNextNumber;



                }

                oHeader[INVOICE_NUMBER] = number;

                bool ret = oInvoicePost.Update();

                Try(ret, "Posting product invoice.");

                return ret;
            }
        }

        /*       RECEIPTS        */


        /*       Called by: Process_Receipt       */
        public bool Post_Receipt_On_Account(Receipt_Info receipt_info,
                                            string pos_id,

                                            out int number) {

            number = -1;
            int magic = -1;
            int temp_number = magic;
            int? result = null;

            Payment_Info payment_info = receipt_info.Payment_Info;
            string account_ref = receipt_info.Account_Ref;

            // Used to make sure result only gets assigned once
            Action try_set_result = () =>
            {
                if (temp_number == magic)
                    return;

                H.Assign_If_Null(ref result, temp_number);
            };


            var dict = new Dictionary<Payment_Type, decimal>{
                            {Payment_Type.Cash, payment_info.Cash},
                            {Payment_Type.Credit, payment_info.Credit},
                            {Payment_Type.Gift, payment_info.Gift},
            };

            if (dict.Keys.All(key => key == 0.0m) &&
               payment_info.Current_Cheques.Length == 0)

                return false;


            Action<Payment_Type> try_post =
                (_type) =>
                {
                    if (dict[_type] != 0.0m) {
                        Post_Receipt(false,
                                     _type,
                                     account_ref,
                                     payment_info,  // payment info
                                     null,          // cheque
                                     pos_id,

                                     out temp_number);

                        (temp_number == magic).tift();
                    }

                    try_set_result();
                };

            using (Establish_Connection()) {

                foreach (var type in dict.Keys) {

                    try_post(type);

                }

                for (int ii = 0; ii < payment_info.Current_Cheques.Length; ++ii) {

                    var cheque = payment_info.Current_Cheques[ii];

                    Post_Receipt(false, Payment_Type.Cheque, account_ref, payment_info, cheque, pos_id, out temp_number);

                    try_set_result();

                }

                /*       The post-dated cheques are handled elsewhere        */
                /*       See Post_Future_Cheque        */

                (temp_number == magic).tift();

                result.tifn();
                (result.Value == magic).tift(); // <-- should not be possible


                number = result.Value;

                return true;
            }
        }

        /*       30th August        */

        /*       Called by: Post_Receipt_On_Account       */
        /*       Called by: Generate_Future_Cheques       */
        /*       (through Process_Sale and Process_Receipt)       */
        /*       Merge of Post_Receipt_on_Account and Post_Future_Cheque - revision 696        */
        bool Post_Receipt(bool sale,
                          Payment_Type payment_type,
                          string account_ref,
                          Payment_Info? null_payment_info,
                          Cheque_Info? null_cheque,
                          string pos_id,

                          out int number) {

            if (sale) // sale => payment_type == Payment_Type.Post_Dated_Cheque
                (payment_type == Payment_Type.Post_Dated_Cheque).tiff();

            string msg_error = sale ? "Post-dated cheque posting." : "Receipt on account posting.";

            var oTransactionPost = WS.Create<TransactionPost>();
            var oHeader = oTransactionPost.HDGet();

            Dictionary<string, object> header_alone;
            Dictionary<string, object> split_alone;

            Fill_Transaction_Objects(sale,
                                     null_payment_info,
                                     null_cheque,
                                     payment_type,
                                     account_ref,

                                     out header_alone,
                                     out split_alone);

            oHeader.Fill(header_alone);
            oHeader[USER_NAME] = pos_id;

            var oSplitData = oTransactionPost.SDAdd();

            oSplitData.Fill(split_alone);
            oSplitData[USER_NAME] = pos_id;

            var ret = oTransactionPost.Update();

            Try(ret, msg_error);

            number = oTransactionPost.PostingNumber;

            (number == -1).tift();

            return ret;

        }


        /*       COMMON        */

        /*       Duplicate code with Post_Receipt_On_Account        */

        public bool Post_Future_Cheque(bool sales,
                                       Cheque_Info cheque,
                                       string account_ref,
                                       string pos_id,

                                       out int number) {

            bool ret;
            using (Establish_Connection()) {

                ret =
                Post_Receipt(sales,
                             Payment_Type.Post_Dated_Cheque,
                             account_ref,
                             null,      // payment info
                             cheque,
                             pos_id,

                             out number);

            }

            Try(ret);

            return ret;
        }

        /*       Called by: Post_Future_Cheque */
        /*       Called by: Post_Receipt_On_Account */
        void
        Fill_Transaction_Objects(bool sales,
                                Payment_Info? null_payment_info,
                                Cheque_Info? null_cheque,
                                Payment_Type pay_type,
                                string account_ref,

                                out Dictionary<string, object> header_alone,
                                out Dictionary<string, object> split_alone) {

            string details;
            decimal amount;
            string reference;
            DateTime date;

            Get_Reference_Amt_Details(sales,
                              null_payment_info,
                              null_cheque,
                              pay_type,

                              out amount, out reference,
                              out date, out details);

            header_alone = new Dictionary<string, object>
                {{"ACCOUNT_REF",    account_ref},
                 {"BANK_CODE",      Get_Default_Sales_Bank()},
                 {"DATE",           date},
                 {"DETAILS",        details},
                 {"INV_REF",        reference},
                 {"POSTED_DATE",    Get_Posted_Date()},
                 {"TYPE",           TransType.sdoSA}};

            split_alone = new Dictionary<string, object>
                {{"AMOUNT_PAID",    amount},
                 {"DATE",           date},
                 {"DETAILS",        details},
                 {"NET_AMOUNT",     amount},
                 {"NOMINAL_CODE",   Get_Default_Sales_Bank()},
                 {"POSTED_DATE",    Get_Posted_Date()},
                 //{"TAX_CODE",       Get_Tax_Code()},
                 {"TYPE",           TransType.sdoSA}};
            // Details, Nominal code, Tax code

        }


        /*       Called by: Fill_Transaction_Objects        */
        static void
        Get_Reference_Amt_Details(bool sales,
                                    Payment_Info? null_payment_info,
                                    Cheque_Info? null_cheque,
                                    Payment_Type pay_type,

                                    out decimal amount,
                                    out string reference,
                                    out DateTime date,
                                    out string details) {

            date = Get_Receipt_Date();

            details = sales ? SALES_PAYMENT : RECEIPT_ON_ACCOUNT;

            switch (pay_type) {

                case Payment_Type.Cash: {
                        var payment_info = null_payment_info.Value;

                        amount = payment_info.Cash;
                        reference = CASH;

                        break;
                    }

                case Payment_Type.Credit: {
                        var payment_info = null_payment_info.Value;

                        amount = payment_info.Credit;
                        reference = CREDIT;

                        break;

                    }

                case Payment_Type.Gift: {
                        var payment_info = null_payment_info.Value;

                        amount = payment_info.Gift;
                        reference = GIFT;

                        break;
                    }
                case Payment_Type.Cheque:
                case Payment_Type.Post_Dated_Cheque: {

                        var cheque = null_cheque.Value;

                        amount = cheque.Amount;
                        reference = cheque.Number;
                        date = cheque.Date;

                        bool current = (pay_type == Payment_Type.Cheque);
                        details = Get_Cheque_Details(sales, current, cheque.Number);

                        break;
                    }

                default:
                    throw new ArgumentException("Payment_Type pay_type");
            }

        }


        /*      PAYMENTS     */


        /*       Called by: Process_Payment        */

        public bool
        Post_Payment(Expense_Info expense_info, string pos_id) {

            bool ret = false;

            using (Establish_Connection()) {

                var oTransactionPost = WS.Create<TransactionPost>();

                Dictionary<string, object> header_alone, split_alone;
                Fill_Payment_Objects(expense_info, out header_alone, out split_alone);

                var header = oTransactionPost.HDGet();

                header.Fill(header_alone);
                header["USER_NAME"] = pos_id;

                var split = oTransactionPost.SDAdd();

                split.Fill(split_alone);

                ret = oTransactionPost.Update();
            }

            return ret;
        }

        /*       Called by: Post_Payment        */

        public void
        Fill_Payment_Objects(Expense_Info expense_info,

                             out Dictionary<string, object> header_alone,
                             out Dictionary<string, object> split_alone) {

            header_alone = new Dictionary<string, object>
            {
                {"ACCOUNT_REF", Get_Default_Sales_Bank()},
                {"BANK_CODE", Get_Default_Sales_Bank()},  // <--
                {"DATE", expense_info.Date},
                {"DETAILS", expense_info.Details},
                {"INV_REF", expense_info.Reference},   // <--
                {"TYPE", TransType.sdoCP},

                {"NET_AMOUNT", expense_info.Net_Amount},
                {"TAX_AMOUNT", expense_info.Tax_Amount},
            };

            split_alone = new Dictionary<string, object>
            {
                {"DATE", expense_info.Date},
                {"DETAILS", expense_info.Details},
                {"NOMINAL_CODE", expense_info.Expense_Type},
                {"POSTED_DATE", Get_Receipt_Date()},
                {"TAX_CODE", expense_info.Tax_Code.Tax_Code_ID},
                {"TYPE", TransType.sdoCP},

                {"NET_AMOUNT", expense_info.Net_Amount},
                {"TAX_AMOUNT", expense_info.Tax_Amount},

            };

        }


        //****************************


        static DateTime Get_Posted_Date() {

            DateTime ret = DateTime.Today;

            return ret;
        }

        static DateTime Get_Receipt_Date() {

            DateTime ret = DateTime.Today;

            return ret;
        }

    }
}
