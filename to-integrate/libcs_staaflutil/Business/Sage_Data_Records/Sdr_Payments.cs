using System;
using System.Collections.Generic;
using System.Linq;
using Fairweather.Service;
using Versioning;
using Common.Posting;

namespace Common
{
    public partial class Sage_Logic
    {
        public bool Commit(Postings postings,
                           bool by_split,
                           Trans_Line manual,
                           Screen_Type type,
                           out int? sr_number) {

            if (manual != null)
                manual.Trans_Ref.tifn();

            using (Establish_Connection()) {

                string user = M.Get_Username();

                if (user.IsNullOrEmpty()) // <--
                    user = Conn.Name;

                bool customer = (type == Screen_Type.Customers);
                bool ret;

                Check_Totals(postings, postings.Cheque_Amount, postings.Total_Discounts);

                bool no_Ref = postings.Invoice_Ref.IsNullOrEmpty();
                bool amount = postings.Cheque_Amount > 0 || postings.Sa_Amount > 0;

                int? payment_no, discount_no, pa_no;

                ret = Commit(postings, customer, user, by_split, manual,
                             out payment_no,
                             out discount_no,
                             out pa_no);

                if (no_Ref && !customer && amount)
                    Remittance(type, postings, payment_no, discount_no, pa_no);

                sr_number = payment_no;


                return ret;
            }
        }

        static void Check_Totals(Postings postings, decimal total_receipts, decimal total_discounts) {

            //var our_total_receipts = 0.0m;
            //var our_total_discounts = 0.0m;

            //var receipts = postings.Receipt_Allocations.Select(cad => cad.Amount);
            //var discounts = postings.Discount_Allocations.Select(cad => cad.Amount);

            //if (!receipts.Is_Empty()) {
            //    our_total_receipts = receipts.Aggregate((d1, d2) => Math.Round(d1 + d2, 2));

            //    our_total_receipts = Math.Round(total_receipts, 2);

            //    our_total_receipts -= postings.Total_Usages;

            //    our_total_receipts = Math.Round(total_receipts, 2);

            //}

            //if (!discounts.Is_Empty()) {
            //    our_total_discounts = discounts.Aggregate((d1, d2) => Math.Round(d1 + d2, 2));
            //    our_total_discounts = Math.Round(total_discounts, 2);
            //}

            //(total_receipts == our_total_receipts).tiff();
            //(total_discounts == our_total_discounts).tiff();

        }


        // void Allocate(bool by_split, IEnumerable<Credit_Allocation_Data> data, DateTime date, int? source) { }

        bool Commit(Postings postings,
                    bool customer,
                    string user,
                    bool by_split,
                    Trans_Line manual,
                    out int? payment_no,
                    out int? discount_no,
                    out int? pa_no) {

            H.assign(out payment_no, out discount_no, out pa_no);

            var cheque = postings.Cheque_Amount;
            var discount = postings.Total_Discounts;
            var date = postings.Date;

            // Allocate the Payments on Accounts and the Credit

            if (by_split)
                Allocate_By_Split(postings.Usage_Allocations, date, null);
            else
                Allocate_By_Header(postings.Usage_Allocations, date, null);

            if (cheque > 0.0m) {

                int receipt_no;
                if (manual == null) {
                    Post_SR(postings, customer, user, cheque, out receipt_no);

                }
                else {
                    receipt_no = manual.Trans_Ref.Value;

                }
                payment_no = receipt_no;

                // Allocate the Receipt
                if (by_split)
                    Allocate_By_Split(postings.Receipt_Allocations, date, receipt_no);

                else
                    Allocate_By_Header(postings.Receipt_Allocations, date, receipt_no);

            }

            if (discount > 0.0m) {

                int discount_no_tmp;


                Post_SD(postings, customer, user, discount, out discount_no_tmp);

                discount_no = discount_no_tmp;

                // Allocate the Discount

                if (by_split)
                    Allocate_By_Split(postings.Receipt_Allocations, date, discount_no_tmp);

                else
                    Allocate_By_Header(postings.Discount_Allocations, date, discount_no_tmp);


            }

            // Payment on account
            if (postings.Sa_Amount > 0.0m) {
                int sa_no;

                Post_SA(postings, user, customer, postings.Sa_Amount, out sa_no);
                pa_no = sa_no;
            }

            // Old functions:
            // Allocate_Credit_By_Header(postings);
            // Allocate_SR_By_Header(postings, receipt_no);
            // Allocate_SD_By_Header(postings, discount_no);
            ///
            // Old functions:
            // Allocate_Credit_By_Split(,)
            // Allocate_SD(postings, tran_no);
            // Allocate_SR(postings, tran_no);

            return true;
        }



        void Allocate_By_Header(IEnumerable<Credit_Allocation_Data> data,
                                DateTime date,
                                int? credit_source) {


            foreach (var cad in data) {

                int source = (credit_source ?? cad.Source).Value; // receipt, discount or credit split
                int target = cad.Target;    // invoice split

                var header = WS.Create<HeaderData>();

                header.FindFirst(FIRST_SPLIT, target).tiff(); //16 May // 15 Jan 10
                //HeaderData.Read(target).tiff();

                //var inv_ref = HeaderData[INV_REF].ToString(); // Where does this go?

                var split_data = header.Link;

                var first_next = split_data.First_Next();

                // returns the outstanding amounts of each of the header's splits
                Func<decimal?> deficits = () =>
                {
                    decimal owed = 0.0m;

                    do {
                        if (!first_next)
                            return null;

                        if (split_data.Deleted)
                            continue;

                        owed = Get_Amount(split_data);
                        if (owed == 0.0m)
                            continue;

                    } while (false);


                    return owed;
                };


                bool asked = false;
                Func<decimal?> amounts = () => { if (asked) return null; asked = true; return cad.Amount; };

                Allocator alloc = (_to_alloc, _1, _2) =>
                {
                    var TransactionPost = WS.Create<TransactionPost>();

                    int invoice = split_data.RecordNumber;
                    Try(TransactionPost.Allocate(invoice, source,
                             (double)_to_alloc, date));
                };


                Magic.distribute(amounts, deficits, alloc);


            }

        }

        void Allocate_By_Split(IEnumerable<Credit_Allocation_Data> data,
                       DateTime date,
                       int? credit) {


            foreach (var cad in data) {

                var TransactionPost = WS.Create<TransactionPost>();

                var amount = cad.Amount;
                int source = (credit ?? cad.Source).Value;
                int target = cad.Target;

                Try(TransactionPost.Allocate(target, source,
                                             (double)amount, date));


            }


        }



        //// ****************************

        /// Receipt

        void Post_SR(Postings postings, bool customer, string user, decimal amount, out int receipt_no) {

            var post = WS.Create<TransactionPost>();

            var header = post.HDGet();
            var split = post.SDAdd();

            var type = customer ? TransType.sdoSR : TransType.sdoPP;
            var details = postings.Receipt_Details;
            var date = postings.Date;

            header[DATE] = date;
            header[POSTED_DATE] = DateTime.Today;
            header[TYPE] = type;
            header[ACCOUNT_REF] = postings.Account_Ref;
            header[BANK_CODE] = postings.Bank_Ref;
            header[INV_REF] = postings.Invoice_Ref;
            header[USER_NAME] = user;
            header[DETAILS] = details;

            split[TYPE] = type;
            split[DATE] = date;
            split[NET_AMOUNT] = amount;
            split[TAX_AMOUNT] = 0;
            split[NOMINAL_CODE] = postings.Bank_Ref;
            split[TAX_CODE] = 9;
            split[DEPT_NUMBER] = postings.Department;
            split[INTERNAL_REF] = postings.Ex_Ref;
            split[USER_NAME] = user;
            split[DETAILS] = details;

            Try(post.Update());

            int tran_no = post.PostingNumber;
            receipt_no = Get_Credit_Split_Number(tran_no);

        }

        /// Discount
        void Post_SD(Postings postings, bool customer, string user, decimal discount, out int discount_no) {

            int discount_code = Get_Discount_Code(customer);

            var type = customer ? TransType.sdoSD : TransType.sdoPD;
            var date = postings.Date;
            var details = postings.Discount_Details;

            var TransactionPost = WS.Create<TransactionPost>();
            var HeaderFields = TransactionPost.HDGet();
            var SplitData = TransactionPost.SDAdd();


            HeaderFields[DATE] = date;
            HeaderFields[POSTED_DATE] = DateTime.Today;
            HeaderFields[TYPE] = type;
            HeaderFields[ACCOUNT_REF] = postings.Account_Ref;
            HeaderFields[BANK_CODE] = postings.Bank_Ref;
            HeaderFields[INV_REF] = postings.Invoice_Ref;
            HeaderFields[USER_NAME] = user;
            HeaderFields[DETAILS] = details;

            SplitData[TYPE] = type;
            SplitData[DATE] = date;
            SplitData[NET_AMOUNT] = discount;
            SplitData[TAX_AMOUNT] = 0;
            SplitData[NOMINAL_CODE] = discount_code;
            SplitData[TAX_CODE] = 9;
            SplitData[USER_NAME] = user;
            SplitData[DEPT_NUMBER] = postings.Department;
            SplitData[DETAILS] = details;

            Try(TransactionPost.Update());

            var tran_no = TransactionPost.PostingNumber;
            discount_no = Get_Credit_Split_Number(tran_no);


        }

        /// Payment on Account
        void Post_SA(Postings postings, string user, bool customer, decimal amount, out int sa_no) {

            var TransactionPost = WS.Create<TransactionPost>();

            var HeaderFields = TransactionPost.HDGet();
            var SplitData = TransactionPost.SDAdd();

            var type = customer ? TransType.sdoSA : TransType.sdoPA;
            var date = DateTime.Today;
            var details = postings.Sa_Details;

            HeaderFields[DATE] = postings.Date;
            HeaderFields[POSTED_DATE] = date;
            HeaderFields[TYPE] = type;
            HeaderFields[ACCOUNT_REF] = postings.Account_Ref;
            HeaderFields[BANK_CODE] = postings.Bank_Ref;
            HeaderFields[INV_REF] = postings.Invoice_Ref;
            HeaderFields[DETAILS] = details;
            HeaderFields[USER_NAME] = user;

            SplitData[TYPE] = type;
            SplitData[DATE] = postings.Date;
            SplitData[NET_AMOUNT] = amount;
            SplitData[TAX_AMOUNT] = 0;
            SplitData[NOMINAL_CODE] = postings.Bank_Ref; //TODO Where do we get the nominal code from?
            SplitData[TAX_CODE] = 9;
            SplitData[INTERNAL_REF] = postings.Ex_Ref;
            SplitData[DETAILS] = details; //TODO:
            SplitData[USER_NAME] = user;

            Try(TransactionPost.Update());

            var tran_no = TransactionPost.PostingNumber;
            sa_no = Get_Credit_Split_Number(tran_no);
        }



        // ****************************



        int Get_Discount_Code(bool customer) {

            var oControlData = WS.Create<ControlData>();

            oControlData.Read().tiff();

            int sdiscount_no = oControlData[customer ? SDISCOUNT_NO : PDISCOUNT_NO].ToInt32();

            var oNominalData = WS.Create<NominalData>();

            oNominalData.Read(sdiscount_no);

            int discount_code = oNominalData[ACCOUNT_REF].ToInt32();
            return discount_code;
        }

        int Get_Credit_Split_Number(int tran_no) {

            var header = WS.Create<HeaderData>();
            header.Read(tran_no).tiff();

            var split = header.Link;

            split.MoveFirst().tiff();

            int receipt = split.RecordNumber;
            return receipt;
        }



        // ****************************


        public List<Transaction_Line>
        Get_Transactions(string account_Ref,
             string bank_Ref,
             bool by_split,
             Screen_Type type) {

            using (Establish_Connection()) {

                bool customer = type == Screen_Type.Customers;

                var ret = Get_Transactions(account_Ref, bank_Ref, customer, by_split);

                return ret;
            }
        }





        List<Transaction_Line>
        Get_Transactions(string account_Ref,
                    string bank_Ref,
                    bool customer,
                    bool by_split) {

            ILinkRecord Record;

            if (customer)
                Record = WS.Create<SalesRecord>();
            else
                Record = WS.Create<PurchaseRecord>();

            Record[ACCOUNT_REF] = account_Ref;

            Try(Record.Find(false));

            var HeaderData = new HeaderData(Record.Link, Version);

            var first_next = HeaderData.First_Next();

            var uninteresting = new Set<TransType> { TransType.sdoPD, TransType.sdoPP, TransType.sdoSD, TransType.sdoSR };

            var ret = new List<Transaction_Line>(30);

            while (first_next) {

                if (HeaderData.Deleted)
                    continue;

                var type = (TransType)HeaderData[TYPE];

                if (uninteresting[type])
                    continue;

                var date = HeaderData[DATE].ToDateTime();
                var reference = HeaderData[INV_REF].ToString();

                int unique_Ref;
                string details, ex_Ref;
                bool disputed;
                decimal amount;
                int? tax_code = null;

                H.assign(out unique_Ref, out details, out ex_Ref, out disputed, out amount);


                Action make = () =>
                {
                    var tr = new Transaction_Line(type, date, unique_Ref, reference, ex_Ref,
                                                  amount, tax_code, details, disputed);

                    ret.Add(tr);

                };

                if (by_split) {

                    var SplitData = HeaderData.Link;

                    var first_next2 = SplitData.First_Next();


                    while (first_next2) {

                        if (SplitData.Deleted)
                            continue;

                        amount = Get_Amount(SplitData);

                        if (amount <= 0)
                            continue;

                        unique_Ref = SplitData.RecordNumber;
                        tax_code = SplitData[TAX_CODE].ToInt32();
                        ex_Ref = SplitData[INTERNAL_REF].ToString();
                        details = SplitData[DETAILS].ToString();
                        disputed = SplitData[STATUS].ToBool();

                        make();

                    }
                }
                else {

                    amount = Get_Amount(HeaderData);

                    if (amount <= 0)
                        continue;

                    unique_Ref = HeaderData[FIRST_SPLIT].ToInt32();
                    details = HeaderData[DETAILS].ToString();
                    disputed = Is_Disputed(HeaderData);

                    make();
                }

            }

            return ret;
        }


        static bool Is_Disputed(HeaderData HeaderData) {

            var SplitData = HeaderData.Link;

            var first_next = SplitData.First_Next();

            while (first_next) {

                if (SplitData.Deleted)
                    continue;

                if (Is_Disputed(SplitData))
                    return true;

            }

            return false;
        }

        static bool Is_Disputed(SplitData SplitData) {

            if (SplitData[STATUS].ToInt32() == 1)
                return true;

            return false;

        }


        static decimal Get_Amount(Sage_Container container) {

            (container is HeaderData || container is SplitData).tiff();

            decimal amount = container[NET_AMOUNT].ToDecimal() +
                             container[TAX_AMOUNT].ToDecimal() -
                             container[AMOUNT_PAID].ToDecimal();

            return amount;
        }

    }


}
