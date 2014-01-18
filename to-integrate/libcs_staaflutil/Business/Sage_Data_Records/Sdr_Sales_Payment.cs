using System;
using System.Collections.Generic;



using Fairweather.Service;
using Versioning;




namespace Common
{
    public partial class Sage_Logic
    {

        void Fill_Sales_Payment_Objects(Screen_Info screen_info,
                                        decimal refund_amount,
                                        string pos_id,

                                        out Dictionary<string, object> header_alone,
                                        out Dictionary<string, object> split_alone) {

            header_alone = new Dictionary<string, object> {
#region data - 10 lines
                    {"DATE", screen_info.Date},
                    {"DETAILS", "Cash Account Refund"},
                    {"INV_REF", REFUND},
                    {"NET_AMOUNT", refund_amount},
                    {"POSTED_DATE", Get_Posted_Date()},
                    {"USER_NAME", pos_id},
                    //{"",}
	#endregion
                };

            split_alone = new Dictionary<string, object> {
#region data - 10 lines

                    {"DATE", screen_info.Date},
                    {"DETAILS", "Cash Account Refund"},
                    {"NET_AMOUNT", refund_amount},
                    {"POSTED_DATE", Get_Posted_Date()},
                    {"TAX_CODE", 9},
                    {"USER_NAME", pos_id}, 
	#endregion
                };

        }

        /// <summary> Called by Post_Sales_Payment 
        /// Info: "invoice against credit note.txt"
        /// </summary>
        int? Post_SP(Screen_Info screen_info,
                     decimal amount,
                     string pos_id) {

            using (Establish_Connection()) {

                var oTransactionPost = WS.Create<TransactionPost>();

                var header = oTransactionPost.HDGet();
                var split = oTransactionPost.SDAdd();

                Dictionary<string, object> header_alone, split_alone;

                Fill_Sales_Payment_Objects(screen_info, amount, pos_id, out header_alone, out split_alone);

                string client_acc_ref = screen_info.Account_Ref;
                string def_sales_bank = Get_Default_Sales_Bank();

                header.Fill(header_alone);
                header["ACCOUNT_REF"] = client_acc_ref;
                header["TYPE"] = Versioning.TransType.sdoSP;
                header["BANK_CODE"] = def_sales_bank;

                split.Fill(split_alone);
                split["TYPE"] = Versioning.TransType.sdoSP;
                split["NOMINAL_CODE"] = def_sales_bank;

                int? ret = null;

                var ok = oTransactionPost.Update();

                if (ok) {
                    ret = oTransactionPost.PostingNumber;
                }

                return ret;

            }


        }

        /// <summary> Returns the posting number of the BP on success, otherwise null.
        /// Info: "invoice against credit note.txt"
        /// </summary>
        Pair<int>? Post_SI_BP_Combo(Screen_Info screen_info,
                     decimal amount,
                     string pos_id) {

            string mispostings = Get_Control_Account(ControlTypes.sdoNtMispostings);
            string client_acc_ref = screen_info.Account_Ref;
            string def_sales_bank = Get_Default_Sales_Bank();

            int first, second;
            Dictionary<string, object> header_alone, split_alone;

            Fields header;
            SplitData split;
            TransactionPost oTransactionPost;

            using (Establish_Connection()) {

                /*       SI        */

                oTransactionPost = WS.Create<TransactionPost>();

                header = oTransactionPost.HDGet();
                split = oTransactionPost.SDAdd();

                Fill_Sales_Payment_Objects(screen_info, amount, pos_id, out header_alone, out split_alone);

                header.Fill(header_alone);
                header["ACCOUNT_REF"] = client_acc_ref;
                header["TYPE"] = Versioning.TransType.sdoSI;
                header["BANK_CODE"] = mispostings;

                split.Fill(split_alone);
                split["TYPE"] = Versioning.TransType.sdoSI;
                split["NOMINAL_CODE"] = mispostings;

                var ok = oTransactionPost.Update();

                if (!ok)
                    return null;

                first = oTransactionPost.PostingNumber;

                /*       BP        */

                oTransactionPost = WS.Create<TransactionPost>();

                header = oTransactionPost.HDGet();
                split = oTransactionPost.SDAdd();

                Fill_Sales_Payment_Objects(screen_info, amount, pos_id, out header_alone, out split_alone);

                header.Fill(header_alone);
                header["CURRENCY"] = 3;
                header["CURRENCY_USED"] = 3;

                header["TYPE"] = Versioning.TransType.sdoBP;
                header["BANK_CODE"] = def_sales_bank;

                split.Fill(split_alone);
                split["TYPE"] = Versioning.TransType.sdoBP;
                split["NOMINAL_CODE"] = mispostings;

                Pair<int>? ret = null;

                ok = oTransactionPost.Update();

                if (ok) {
                    second = oTransactionPost.PostingNumber;
                    ret = new Pair<int>(first, second);
                }

                return ret;

            }

        }

        // Calles by Process_Sale
        public int? Post_Refund(Screen_Info screen_info,
                                       decimal amount,
                                       string pos_id) {

            (amount > 0.0m).tiff();

            if (Version > 12) {
                var ret = Post_SP(screen_info, amount, pos_id);
                return ret;
            }
            else {
                var pair = Post_SI_BP_Combo(screen_info, amount, pos_id);

                return pair.HasValue ? (int?)pair.Value.Second : null;
            }

        }

        public bool Mark_Credit_Note_As_Settled(string user,
                                                int document_number) {
            bool ret = false;

            Find_Document(user, Document_Type.Product_Credit, document_number,
            (record) =>
            {
                record.Edit();
                record[CUST_ORDER_NUMBER] = SETTLED;
                ret = record.Update();
            });

            return ret;

        }

        bool Find_Document(string user,
                           Document_Type doc_type,
                           int document_number,
                           Action<InvoiceRecord> on_found) {

            user.IsNullOrEmpty().tift("You must provide a username to use this function");

            (document_number >= 0).tiff();

            var invoice_type = doc_type.To_Invoice_Type();

            using (Establish_Connection()) {

                var oInvoiceRecord = WS.Create<InvoiceRecord>();
                oInvoiceRecord[INVOICE_NUMBER] = document_number;

                bool ok = oInvoiceRecord.Find(true);

                var tmp_number = Convert.ToInt32(oInvoiceRecord[INVOICE_NUMBER]);

                (tmp_number == document_number).tiff();

                if (ok) {
                    if ((short)oInvoiceRecord[DELETED_FLAG] == 1)
                        ok = false;

                }

                if (ok) {
                    var tmp_user = oInvoiceRecord[TAKEN_BY].ToString();
                    if (tmp_user != user)
                        ok = false;

                }

                if (ok) {
                    var tmp_type = (InvoiceType)(sbyte)oInvoiceRecord[INVOICE_TYPE_CODE];
                    if (tmp_type != invoice_type)
                        ok = false;

                }

                if (ok)
                    on_found(oInvoiceRecord);

                return ok;

            }
        }

        public Document_Info? Get_Posted_Document(string user,
                                          Document_Type doc_type,
                                          int document_number) {

            Document_Info? ret = null;

            Action<InvoiceRecord> on_found =
                (oInvoiceRecord) =>
                    ret = Get_Document_Info(oInvoiceRecord);

            Find_Document(user, doc_type, document_number, on_found);

            return ret;

        }

        Document_Info Get_Document_Info(InvoiceRecord oInvoiceRecord) {

            var account_ref = oInvoiceRecord["ACCOUNT_REF"].ToString();

            var amount = Convert.ToDecimal(oInvoiceRecord["BASE_TOT_NET"]) +
                         Convert.ToDecimal(oInvoiceRecord["BASE_TOT_TAX"]);

            var user = oInvoiceRecord[TAKEN_BY].ToString();

            var date = Convert.ToDateTime(oInvoiceRecord["INVOICE_DATE"]);

            var type = (InvoiceType)(sbyte)oInvoiceRecord["INVOICE_TYPE_CODE"];

            var number = Convert.ToInt32(oInvoiceRecord["INVOICE_NUMBER"]);

            var settled = Is_Settled(oInvoiceRecord);

            var ret = new Document_Info(account_ref, user, amount,
                                        number, date, type.To_Document_Type(),
                                        settled);


            return ret;
        }

        bool Is_Settled(InvoiceRecord record) {
            return record[CUST_ORDER_NUMBER].Equals(SETTLED);
        }

        /// <summary> Returns the latest document of a specified type which was posted by a specified user.
        /// </summary>
        /// <param name="user">The user who has posted the document</param>
        /// <param name="doc_type">The Document_Type of the desired document</param>
        /// <param name="only_last_is_of_interest">If true, the function will return a result only if the
        /// last posted document BY THE REQUIRED USER is of the specified Document_Type</param>
        /// <param name="is_last">Whether or not the last document posted by the specified user was of the
        /// specified type</param>
        /// <returns>Returns null if no document matching the conditions was found</returns>
        public Document_Info? Get_Last_Posted_Document(string user,
                                                       Document_Type doc_type,
                                                       bool only_last_is_of_interest,
                                                       out bool is_last) {

            user.IsNullOrEmpty().tift("You must provide a username to use this function");

            is_last = false;
            var invoice_type = doc_type.To_Invoice_Type();

            using (Establish_Connection()) {

                var oInvoiceRecord = WS.Create<InvoiceRecord>();

                if (!oInvoiceRecord.MoveLast())
                    return null;

                string tmp_user = "";
                InvoiceType? tmp_type = null;
                is_last = true;
                bool found = false;

                do {
                    tmp_user = oInvoiceRecord[TAKEN_BY].ToString();

                    if (tmp_user != user) {
                        continue;
                    }

                    bool ok = true;

                    if (ok) {
                        tmp_type = (InvoiceType)(sbyte)oInvoiceRecord["INVOICE_TYPE_CODE"];

                        if (tmp_type.Value != invoice_type) {
                            ok = false;
                        }
                    }

                    if (ok) {
                        found = true;
                        break;
                    }

                    is_last = false;
                    if (only_last_is_of_interest)
                        break;

                } while (oInvoiceRecord.MovePrev());


                Document_Info? ret = null;

                if (found) {

                    (tmp_user == user).tiff();
                    tmp_type.HasValue.tiff();
                    (tmp_type.Value == invoice_type).tiff();

                    ret = Get_Document_Info(oInvoiceRecord);

                }

                return ret;
            }

        }


        string Get_Control_Account(ControlTypes control_types) {

            using (Establish_Connection()) {

                var oNominalRecord = WS.Create<NominalRecord>();

                if (!oNominalRecord.FindControlAccount(control_types))
                    return null;

                return oNominalRecord["ACCOUNT_REF"].ToString();

            }

        }
    }
}

