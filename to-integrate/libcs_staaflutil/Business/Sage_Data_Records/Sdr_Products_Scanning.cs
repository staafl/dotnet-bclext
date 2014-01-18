using System;
using System.Collections.Generic;
using System.Linq;
using Fairweather.Service;
using Versioning;



namespace Common
{

    public partial class Sage_Logic
    {

        /// <summary>
        /// A return value of 'null' means 'Not Applicable' (see specs)
        /// </summary>
        /// <param name="stock_code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public decimal? Calculate_Average_Cost(
            string stock_code,
            Average_Cost_Type type) {

            // type, amount, qty
            var lst = new List<Triple<StockTransType, decimal, decimal>>();

            using (Establish_Connection()) {

                // ws.Create<([^>]+)>();
                // new $1();

                var record = WS.Create<StockRecord>();

                record[STOCK_CODE] = stock_code;

                record.Find(false).tiff();

                bool purchases_only = (type == Average_Cost_Type.Purchases_Only);

                //if (purchases_only) {
                var qty_in_stock = record[QTY_IN_STOCK].ToDecimal();
                if (qty_in_stock < 0.00m)
                    return null;
                else if (qty_in_stock == 0.00m)
                    return 0.00m;
                //}


                var running_amount = 0.0m;
                var running_qty = 0.0m;

                var stock_tran = record.Link;

                var seen = new Set<int>();

                // we're caching the STs while connected to the database,
                // so we're using SortedList, even though the subsequent
                // iteration will be slower.
                // 

                if (!stock_tran.MoveFirst())
                    return 0.0m;

                Func<string> get_ref;
                Func<StockTransType> get_tt;
                Func<bool> move_next, move_prev;
                Func<int> get_id;
                Func<decimal> get_qty, get_cost;

                get_tt = () => (StockTransType)stock_tran[TYPE];

                get_qty = () => stock_tran[QUANTITY].ToDecimal();

                get_cost = () => stock_tran[COST_PRICE].ToDecimal();

                get_ref = () => stock_tran[REFERENCE].ToString();

                // let's hope no reindexing happens while we're in
                // the data ;-)
                get_id = () => stock_tran.RecordNumber;

                move_next = stock_tran.MoveNext;

                move_prev = stock_tran.MovePrev;


                //* // --> if date order

                const bool by_date = true;

                if (by_date) {

                    var quads = new List<Pair<DateTime, Quad<string,
                                                              StockTransType,
                                                              decimal, // qty
                                                              decimal  // cost
                                                            >>>(stock_tran.Count + 10);


                    do {

                        var quad = Quad.Make(get_ref(),
                                             get_tt(),
                                             get_qty(),
                                             get_cost());

                        var adjusted_date = stock_tran[DATE].ToDateTime()
                                                            .AddTicks(stock_tran.RecordNumber);

                        quads.Add(Pair.Make(adjusted_date, quad));

                    } while (move_next());

                    quads.Sort(_p => _p.First);
                    int ix = 0;

                    // let go of the connection if we
                    // initiated it ourselves
                    Conn.Try_Dispose();


                    get_ref = () => quads[ix].Second.First;

                    get_tt = () => quads[ix].Second.Second;

                    get_qty = () => quads[ix].Second.Third;

                    get_cost = () => quads[ix].Second.Fourth;

                    get_id = () => ix;

                    move_next = () => ++ix < quads.Count;

                    move_prev = () => --ix >= 0;




                }
                //*/// <-- if date order


                // both cases

                Func<bool, int?> move = _fwd =>
                {
                    int _ret = 0;
                    do {
                        if (_fwd) {
                            if (!move_next())
                                return null;
                        }
                        else {
                            if (!move_prev())
                                return null;
                        }
                        ++_ret;
                    } while (seen[get_id()]);
                    return _ret;

                };


                Func<decimal> running_average = () => running_amount.Safe_Divide(running_qty);

                Func<decimal> get_qty_delta = () =>
                {

                    var qty = get_qty();

                    switch (get_tt()) {
                        case StockTransType.sdoAO:
                        case StockTransType.sdoDO:
                        case StockTransType.sdoGO:
                        case StockTransType.sdoMO:
                        case StockTransType.sdoWO: {
                                qty *= -1;
                                break;
                            }
                    }

                    return qty;
                };


                Action handle_as_usual = () =>
                {

                    var delta = get_qty_delta();

                    decimal current_cost_price;

                    switch (get_tt()) {
                        case StockTransType.sdoGI:
                        case StockTransType.sdoMI:
                        case StockTransType.sdoAI: {
                                current_cost_price = get_cost();
                                break;
                            }
                        default: {
                                if (purchases_only)
                                    return;

                                current_cost_price = running_average();
                                break;

                            }
                    }

                    var total = delta * current_cost_price;

                    running_qty += delta;
                    running_amount += total;

                    seen[get_id()] = true;

                };





                do {

                    var delta = get_qty_delta();

                    // if the running qty becomes less than 0,
                    // we will have a nonsensical average cost.
                    // 
                    // This is only possible if we're summing using the 'All Transactions'
                    // method.
                    if (running_qty + delta < 0) {

                        purchases_only.tift();

                        var start = get_ref();

                        var negative_qty = running_qty + delta;

                        // how many records we have travelled from
                        // this point
                        // it is important to keep track of the number
                        // of moves the cursor has made, since we may
                        // traverse some transaction records more than
                        // once. 
                        var distance = (int?)0;

                        // ---> x <---
                        // define: trivialize
                        while (negative_qty <= 0) {

                            distance += move(true);

                            if (distance == null) {
                                // ran out of transactions while the balance
                                // was negative
                                running_qty = negative_qty;
                                break; /* do something later */

                            }

                            var new_delta = get_qty_delta();

                            if (new_delta < 0)
                                continue;

                            negative_qty += new_delta;

                            handle_as_usual();

                        }

                        if (distance == null)
                            break;

                        // backtrack to the transaction which would have caused
                        // the balance to drop below zero
                        for (int ii = 0; ii < distance; ++ii)
                            move_prev().tiff();

                        (get_ref() == start).tiff();
                    }

                    handle_as_usual(); // purchases-only logic is inside this call

                } while (move(true) != null);


                var ret = running_average();

                if (ret < 0.0m)
                    return null;

                return ret;

                //do {
                //    decimal current_cost_price;

                //    var current_qty = stock_tran[QTY].ToInt32();

                //    if (purchase_only) {

                //        switch (tran_type) {
                //            case StockTransType.sdoAO:
                //            case StockTransType.sdoMI:
                //            case StockTransType.sdoAI: {
                //                    current_cost_price = stock_tran[COST_PRICE].ToDecimal();
                //                    break;
                //                }
                //            default: {
                //                    if (purchase_only)
                //                        continue;
                //                    current_cost_price = running_average();
                //                }
                //        }

                //    }


                //} while (stock_tran.MoveNext());

            }



        }

        /* Specs:
         * This function returns a representation of a product's sales history
         *  regarding a particular client (in human, information regarding the
         *  past sales of the item to that client), in the form of 'Sales_History_Entry'
         *  objects.
         * 
         * The 'product' and 'acc_ref' parameters have an obvious meaning.
         * 
         * The 'only_one' parameter controls the number of entries returned. If it is set to
         *  true, only the entries (NB: ENTRIES, pl.) corresponding to the LAST sale will be retrieved. For a 
         *  definition of LAST, see below.
         *  
         * 'by_date' only matters if 'only_one', in which case it determines which entries will be 
         *  returned: the ones from the document with the last INVOICE_DATE or the from the document with the 
         *  highest INVOICE_NUMBER.
         *  
         * NB: 'by_date' is currently inactive
         * NB: at the moment, contrary to the above specs, only a SINGLE ENTRY is returned if 'only_one'
         */

        public List<Sales_History_Entry>
        Get_Sales_History(
            Product product,
            string acc_ref,
            bool only_one,
            bool by_date) {

            string stock_code = product.Code;

            var ret = new List<Sales_History_Entry>();

            Func<InvoiceItem, InvoiceRecord, bool, Sales_History_Entry>
            get_entry = (item, record, is_credit_note) =>
            {
                var date = record[INVOICE_DATE].ToDateTime();
                int number = record[INVOICE_NUMBER].ToInt32();
                var posted = record[POSTED_CODE].ToBool();

                var item_info = Read_Invoice_Item(item, product);

                var qty = item_info.Qty;

                var unit_vat = item_info.Unit_Price_V;
                var unit_nvat = item_info.Unit_Price_NV;
                var disc_perc = item_info.Discount_Perc;
                var base_nvat = item_info.Base_Price_NV;

                var entry = new Sales_History_Entry(date, number, posted, qty,
                                                    unit_vat, unit_nvat, base_nvat,
                                                    disc_perc, is_credit_note);

                return entry;
            };


            using (Establish_Connection()) {


                // ****************************

                var stock = WS.Create<StockRecord>();

                stock[STOCK_CODE] = stock_code;
                stock.Find(false).tiff();


                // ****************************
                /*       Posted        */

                {
                    var invoice = WS.Create<InvoiceRecord>();
                    var last_seen = new Lazy_Dict<Pair<string, int>, int>(p => -1);
                    var stock_tran = stock.Link;

                    var fn = stock_tran.First_Next(!only_one);

                    const int goods_out = (int)StockTransType.sdoGO;
                    const int goods_returned = (int)StockTransType.sdoGR;


                    while (fn) {

                        int my_type = stock_tran[TYPE].ToInt32();
                        if (goods_out != my_type &&
                            goods_returned != my_type)
                            continue;

                        //var date = stock_tran[DATE].ToDateTime();
                        //var qty = stock_tran[QUANTITY].ToInt32();
                        //var sales_price = stock_tran[SALES_PRICE].ToDecimal();

                        var invoice_num = stock_tran[REFERENCE].ToInt32();

                        invoice[INVOICE_NUMBER] = invoice_num;

                        if (!invoice.Find(false))
                            // deleted 
                            continue;

                        if (!invoice[ACCOUNT_REF].Equals(acc_ref)) // ?
                            continue;

                        var is_credit_note = invoice.Is_Product_Credit;

                        // ((goods_returned == my_type) == is_credit_note).tift();

                        var item = invoice.Link;

                        if (!item.MoveFirst())
                            continue;

                        int ii = 0; /* this represents the index of the current line
                                     * among all lines containing the relevant product */
                        do {

                            if (!item[STOCK_CODE].Equals(stock_code))
                                continue;

                            var pair = Pair.Make(stock_code, invoice_num);

                            /* this is in order to not mix up stock transactions
                             * arising from the same invoice */
                            if (last_seen[pair] == ii - 1) {

                                last_seen[pair] = ii;

                                ret.Add(get_entry(item, invoice, is_credit_note));

                                if (only_one)
                                    break;
                            }

                            ++ii;

                        } while (item.MoveNext());

                        if (only_one)
                            break;

                    }

                }

                // ****************************
                /*       Unposted        */

                var walk_unposted = Walk_Unposted().GetEnumerator();

                Func<InvoiceRecord> producer =
                    (only_one && !by_date) ?
                    Get_Last_Unposted() :
                    (Func<InvoiceRecord>)(() => walk_unposted.MoveNext() ? walk_unposted.Current : null);

                while (true) {

                    var invoice = producer();

                    if (invoice == null)
                        break;

                    if (!invoice[ACCOUNT_REF].Equals(acc_ref))
                        continue;

                    var item = invoice.Link;

                    var ff2 = item.First_Next();

                    var is_credit_note = invoice.Is_Product_Credit;

                    if (!is_credit_note && !invoice.Is_Product_Invoice)
                        continue;

                    while (ff2) {

                        if (item.Deleted_Safe)
                            continue;

                        var code = item[STOCK_CODE].ToString();

                        if (code != stock_code)
                            continue;

                        ret.Add(get_entry(item, invoice, is_credit_note));

                    }

                }

                if (only_one) {
                    if (ret.Count > 1)
                        ret =

                            (from she in ret
                             group she by she.Invoice_Number into g
                             orderby by_date ? g.First().Date.Date.Ticks
                                             : g.Key
                             select g)
                            .ThenBy(g => g.Key) /* make sure that the invoices from the same date are sorted
                                                 * according to the invoice number */
                            .Last()
                            .lst();
                }

                // ****************************

                return ret;
            }
        }

        /*       Called by Get_Sales_History        */

        public Func<InvoiceRecord>
        Get_Last_Unposted() {

            var record = WS.Create<InvoiceRecord>();

            bool first = true;
            return () =>
            {
                if (first) {
                    if (!record.MoveLast())
                        return null;
                }
                else {
                    if (!record.MovePrev())
                        return null;
                }

                first = false;

                while (record[POSTED_CODE].ToBool())
                    if (!record.MovePrev())
                        return null;

                return record;

            };


        }


        public Dictionary<string, decimal>
        Get_Stock_Balance_For_Stock_Records(
            IEnumerable<string> stock_codes,
            bool include_unposted) {

            bool include_posted = false;

            return Get_Stock_Balance_For_Stock_Records(stock_codes, include_unposted, include_posted);

        }

        public Dictionary<string, decimal>
        Get_Stock_Balance_For_Stock_Records(
                IEnumerable<string> stock_codes,
                bool include_unposted,
                bool include_posted) {


            var ret = new Dictionary<string, decimal>();

            using (Establish_Connection()) {

                var record = WS.Create<StockRecord>();

                if (include_posted)
                    foreach (var stock_code in stock_codes) {

                        record[STOCK_CODE] = stock_code;
                        record.Find(false).tiff();

                        ret[stock_code] = record[QTY_IN_STOCK].ToDecimal();

                    }


                if (include_unposted) {

                    var unposted = Get_Unposted_Sales_For_Stock_Records(stock_codes);

                    foreach (var kvp in unposted) {
                        decimal value;
                        if (!ret.TryGetValue(kvp.Key, out value))
                            value = 0;
                        ret[kvp.Key] = value + kvp.Value;
                    }
                }
            }

            return ret;

        }

        // Guaranteed to return only non-deleted records.
        // No guarantee for the document type.
        IEnumerable<InvoiceRecord>
        Walk_Unposted() {

            var data = WS.Create<InvoiceData>();
            var record = WS.Create<InvoiceRecord>();

            var ff = Get_First_Next(data, POSTED_CODE, (byte)0);

            while (ff) {
                record[INVOICE_NUMBER] = data[INVOICE_NUMBER];
                record.Find(false).tiff();
                yield return record;
            }
        }

        /// <summary>
        /// Sales are represented as negative decimals, 
        /// returns and refunds - as positive. 
        /// </summary>
        public Dictionary<string, decimal>
        Get_Unposted_Sales_For_Stock_Records(IEnumerable<string> stock_codes) {

            var set = new Set<string>(stock_codes);

            var ret = new Dictionary<string, decimal>(set.Count); /* stock code to qty in stock */

            foreach (var stock_code in set)
                ret[stock_code] = 0;

            const int type_invoice = (int)InvoiceType.sdoProductInvoice;
            const int type_credit = (int)InvoiceType.sdoProductCredit;

            using (Establish_Connection()) {

                foreach (var record in Walk_Unposted()) {

                    bool credit = false;

                    var item = record.Link;

                    int type = record[INVOICE_TYPE_CODE].ToInt32();

                    if (type == type_credit) {
                        credit = true;

                    }
                    else if (type != type_invoice) {
                        continue;

                    }

                    var ff = item.First_Next();

                    while (ff) {

                        if (item.Deleted_Safe)
                            continue;

                        var code = item[STOCK_CODE].ToString();
                        if (set[code]) { /* do we care? */
                            Console.WriteLine(record[INVOICE_NUMBER]);

                            var qty = item[QTY_ORDER].ToDecimal();

                            if (credit)
                                qty *= -1;

                            ret[code] -= qty;


                        }

                    }

                }

                return ret;
            }
        }

        /// <summary>
        /// Parses the cheque details string from a Sage invoice.
        /// "sales" -> whether the cheque is given as part of a sales payment as opposed to 
        ///    a receipt on account
        /// "current" -> whether the cheque is current as opposed to future-dated
        /// "number" -> the actual cheque number
        /// </summary>
        /// <param name="details"></param>
        /// <param name="sales"></param>
        /// <param name="current"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        static bool
        Read_Cheque_Details(string details,
                            out bool sales,
                            out bool current,
                            out string number) {

            number = "";

            sales = details.Eat(SALES_PAYMENT, out details);

            if (!sales) {

                bool temp = details.Eat(RECEIPT_ON_ACCOUNT, out details);
                if (!temp)
                    true.tift();

            }

            details.Eat(", ", out details).tiff();

            current = details.Eat(CHEQUE, out details);

            if (!current) {

                bool temp = details.Eat(POST_DATED_CHEQUE, out details);
                if (!temp)
                    true.tift();

            }

            if (details.Eat(CHEQUE_NUM_PREFIX, out details))
                number = details;

            return true;

        }

        static string
        Get_Cheque_Details(bool sales, bool current, string number) {

            // Such cheques are posted from the invoice document and not by us
            (sales && current).tift();

            string ret = "{0}, {1}".spf(sales ? SALES_PAYMENT : RECEIPT_ON_ACCOUNT,
                   current ? CHEQUE : POST_DATED_CHEQUE);

            if (!number.IsNullOrEmpty())
                ret += CHEQUE_NUM_PREFIX + number;

            return ret;

        }


        /// <summary> Gets the total of the posted and unposted balance for an account </summary>
        /// <summary> Credit is negative, Debit is positive </summary>
        public decimal
        Get_Total_Balance(string account_ref) {

            var posted = this.Get_Posted_Account_Balance(account_ref).Value;
            var unposted = this.Get_Unposted_Account_Balance(account_ref);

            var ret = posted + unposted;

            return ret;
        }


        public decimal
        Get_Unposted_Account_Balance(string account_ref) {

            return Get_Unposted_Account_Balance(account_ref, Settings.Calculate_Unposted_Balance_Scanning_Account);

        }

        /// <summary> Credit is negative, Debit is positive </summary>
        public decimal
        Get_Unposted_Account_Balance(string account_ref, bool by_account_ref) {

            using (Establish_Connection()) {

                var InvoiceData = WS.Create<InvoiceData>();
                var ret = 0.0m;

                var first_next = Get_Unposted_First_Next(InvoiceData, by_account_ref, account_ref);

                while (first_next) {

                    if (InvoiceData.Deleted)
                        continue;


                    //if (by_account_ref) {
                    // skip already posted
                    var posted = (sbyte)InvoiceData[POSTED_CODE];
                    if (posted != 0)
                        continue;
                    //} else {
                    if (InvoiceData[ACCOUNT_REF].ToString() != account_ref)
                        continue;
                    //}



                    double net = (double)InvoiceData[BASE_TOT_NET];
                    double tax = (double)InvoiceData[BASE_TOT_TAX];
                    double paid = (double)InvoiceData[BASE_AMOUNT_PAID];

                    double amount = net + tax;
                    amount -= paid;

                    var type = (sbyte)InvoiceData[INVOICE_TYPE_CODE];

                    if (type == (sbyte)InvoiceType.sdoProductCredit) {

                        /*       Credit        */

                        amount *= -1;
                    }
                    else {
                        /*       Debit        */

                        var enum_invoice = (sbyte)InvoiceType.sdoProductInvoice;

                        if (type != enum_invoice)
                            true.tift("Unexpected invoice type.");

                    }

                    ret += (decimal)amount;

                }

                return ret;
            }
        }


        /// <summary> Credit is negative, Debit is positive </summary>
        public decimal?
        Get_Posted_Account_Balance(string account_ref) {

            using (Establish_Connection()) {

                var oSalesRecord = WS.Create<SalesRecord>();

                oSalesRecord["ACCOUNT_REF"] = account_ref;

                Try(oSalesRecord.Find(false));

                var ret = Convert.ToDecimal(oSalesRecord["BALANCE"]);

                oSalesRecord = null;

                return ret;
            }

        }


        /*       End of Day        */

        // we need: 
        // Sales breakdown
        // Receipts breakdown
        // Total payments
        // Total invoices
        // Total credit notes
        public void
        Get_Days_Info(string pos_id,
                        DateTime today,

                        out Day_Totals sales_data,
                        out Day_Totals receipts_data,
                        out decimal payments,
                        out decimal invoices,
                        out decimal credit_notes) {
            // credit notes

            decimal sales_postdated;
            decimal sales_payments;

            using (Establish_Connection()) {



                Get_Documents_Data(pos_id,
                       today,

                       out sales_data,
                       out invoices,
                       out credit_notes);

                Get_Transaction_Data(pos_id,
                         today,

                         out sales_postdated,
                         out receipts_data,
                         out payments,
                         out sales_payments);

                sales_data = new Day_Totals(today,
                    sales_data.Cash - sales_payments,
                                      sales_data.Credit,
                                      sales_data.Gift,
                                      sales_data.Cheques,
                                      sales_data.Post_Dated);

                // (sales_postdated == sales_data.Post_Dated).tiff();

            }

        }


        // Documents

        /// <summary>
        /// Calls "action" on an InvoiceData instance for each Document 
        /// in the database relevant to this POS terminal.
        /// The traversal order is determined by "by_date"
        /// "pos_id" and "today" are used to identify relevant documents.
        /// </summary>  
        void Walk_Documents_Data(string pos_id,
                                 DateTime today,
                                 bool by_date,
                                 Action<InvoiceData> action) {

            var oInvoiceData = WS.Create<InvoiceData>();

            var first_next =
                Days_Documents_First_Next(oInvoiceData, by_date, today, pos_id);

            while (first_next)
                action(oInvoiceData);


        }

        /// <summary>
        /// 
        /// </summary>
        void Get_Documents_Data(string pos_id,
                                DateTime today,
                                out Day_Totals sales_data,
                                out decimal invoices,
                                out decimal credit_notes) {

            bool by_date = Settings.Calculate_Days_Documents_Scanning_Date;

            Get_Documents_Data(pos_id,
                               today,
                               by_date,

                               out sales_data,
                               out invoices,
                               out credit_notes);

        }



        void
        Get_Documents_Data(string pos_id,
                            DateTime today,
                            bool by_date,
                            out Day_Totals sales_data,
                            out decimal invoices,
                            out decimal credit_notes) {

            Action<InvoiceData> summer;
            Func<Day_Totals> final;
            Func<decimal> invoices_summer, credit_notes_summer;

            Get_Documents_Summer(today, pos_id,
                  out summer, out final, out invoices_summer, out credit_notes_summer);

            Walk_Documents_Data(pos_id, today, by_date, summer);

            sales_data = final();
            invoices = invoices_summer();
            credit_notes = credit_notes_summer();



        }

        void
        Get_Documents_Summer(DateTime today,
                                string pos_id,
                                out Action<InvoiceData> summer,
                                out Func<Day_Totals> final,
                                out Func<decimal> invoices_summer,
                                out Func<decimal> credit_notes_summer) {

            decimal sales_cash = 0.0m;
            decimal sales_credit = 0.0m;
            decimal sales_gift = 0.0m;
            decimal sales_cheques = 0.0m;
            decimal sales_postdated = 0.0m; // The same amount is calculated in the Get_Transactions_Data routine
            // !! they may differ in case of pre-dated cheques

            decimal invoices = 0.0m;
            decimal credit_notes = 0.0m;
            invoices_summer = () => invoices;
            credit_notes_summer = () => credit_notes;

            final = () => new Day_Totals(today,
            sales_cash,
            sales_credit,
            sales_gift,
            sales_cheques,
            sales_postdated);

            summer = (oInvoiceData) =>
            {
                if (oInvoiceData.Deleted)
                    return;

                decimal amt = Convert.ToDecimal(oInvoiceData[BASE_TOT_NET]);
                decimal tax = Convert.ToDecimal(oInvoiceData[BASE_TOT_TAX]);
                decimal paid = Convert.ToDecimal(oInvoiceData[AMOUNT_PREPAID]);

                bool credit;
                decimal total = amt + tax;

                {
                    var number = (int)oInvoiceData[INVOICE_NUMBER];
                    var type = (InvoiceType)(sbyte)oInvoiceData[INVOICE_TYPE_CODE];

                    if (type == InvoiceType.sdoProductCredit) {
                        credit = true;
                    }
                    else {
                        if (type != InvoiceType.sdoProductInvoice)
                            true.tift();
                        credit = false;
                    }


                    if (credit) {
                        credit_notes += amt;
                        credit_notes += tax;

                        return;
                    }

                    invoices += amt;
                    invoices += tax;

                    Payment_Info info;

                    Get_Payment_Info(oInvoiceData, out info);

                    sales_cash += info.Cash;
                    sales_credit += info.Credit;
                    sales_gift += info.Gift;
                    sales_cheques += info.Current_Cheques.Sum(cheque => cheque.Amount);
                    sales_postdated += info.Post_Dated_Cheques.Sum(cheque => cheque.Amount);
                }
            };



        }


        /*       Called by Get_Documents_Data        */

        /// <summary>
        /// Parses an invoice's Delivery_Address and Notes fields to get the payment
        /// information.
        /// </summary>
        bool
        Get_Payment_Info(IReadWrite<string, object> invoice,
                         out Payment_Info info) {

            H.assign(out info);

            var cheques = new List<Cheque_Info>();
            var postdated = new List<Cheque_Info>();

            var date = invoice[INVOICE_DATE].ToDateTime();
            int date_day = date.Comparable_Day();

            // See Fill_Sale_Objects for information on
            // how the fields are populated
            for (int ii = 0; ii <= 5; ++ii) {

                decimal amount;
                DateTime cheque_date;
                string number;

                string temp;
                if (ii == 0)
                    temp = DELIVERY_NAME;
                else
                    temp = DEL_ADDRESS + "_" + ii.ToString();

                var cheque_str = invoice[temp].ToString();

                if (!Read_Cheque_Fields(cheque_str, out amount, out cheque_date, out number))
                    continue;

                var cheque = new Cheque_Info(amount, cheque_date, number);

                int cheque_day = cheque_date.Comparable_Day();

                if (cheque_day > date_day)
                    postdated.Add(cheque);
                else
                    cheques.Add(cheque);

            }

            /* they are strings, after all */

            string cash_str = invoice[NOTES_1].ToString();
            string credit_str = invoice[NOTES_2].ToString();
            string gift_str = invoice[NOTES_3].ToString();

            decimal cash, credit, gift;

            if (!decimal.TryParse(cash_str, out cash))
                return false;

            if (!decimal.TryParse(credit_str, out credit))
                return false;

            if (!decimal.TryParse(gift_str, out gift))
                return false;


            info = new Payment_Info(postdated.ToArray(), cheques.ToArray(), cash, credit, gift);

            return true;

        }

        /*       Called by Get_Payment_Info        */

        /// <summary>
        /// Parses a line from an Invoice's DeliveryAddress field to get
        /// the information about a particular cheque.
        /// </summary>
        bool Read_Cheque_Fields(string cheque_str,
                                out decimal amount,
                                out DateTime date,
                                out string number) {

            amount = default(decimal);
            date = default(DateTime);
            number = null;

            if (cheque_str.IsNullOrEmpty())
                return false;

            string[] fields = cheque_str.Split(',');

            if (fields.Length != 3)
                return false;

            if (!decimal.TryParse(fields[0], out amount))
                return false;

            if (!DateTime.TryParse(fields[1], out date)) // TODO: formatting
                return false;

            number = fields[2];

            return true;

        }

        // Transactions

        /*       Called by Get_Days_Info        */

        void Get_Transaction_Data(string pos_id,
                                    DateTime today,

                                    out decimal sales_postdated,
                                    out Day_Totals receipts_data,
                                    out decimal payments,
                                    out decimal sales_payments) {



            var by_date = Settings.Calculate_Days_Documents_Scanning_Date;
            receipts_data = Get_Transaction_Data(pos_id,
                                        today,
                                        by_date,

                                        out sales_postdated,
                                        out payments,
                                        out sales_payments);


            // ASSERTION

            //decimal sales_postdated1, payments1, sales_payments1;

            //var receipts_data1 = Get_Transaction_Data_Ex(pos_id,
            //                today,
            //                by_date,

            //                out sales_postdated1,
            //                out payments1,
            //                out sales_payments1);

            //(receipts_data1 == receipts_data).tiff();
            //(sales_postdated1 == sales_postdated).tiff();
            //(payments1 == payments).tiff();
            //(sales_payments1 == sales_payments).tiff();

        }


        /*       Called by Get_Transaction_Data        */

        First_Next Get_First_Next<TObj, T1>(TObj obj,
                           string field, T1 value)
            where TObj : Sage_Container, IFindFirstNext {

            Func<bool> find_first = null;
            Func<bool> find_next = null;
            First_Next ret = null;

            find_first = () =>
            {
                if (!obj.FindFirst(field, value))
                    return false;
                if (!obj.Deleted)
                    return true;
                return find_next();
            };

            find_next = () =>
            {
                while (true) {
                    if (!obj.FindNext(field, value))
                        return false;
                    if (!obj.Deleted)
                        return true;
                }
            };
            ret = new First_Next(find_first, find_next);

            return ret;

        }


        First_Next Get_First_Next<TObj, T1, T2>(TObj obj,
                                   string field, T1 value,
                                   string field2, T2 value2)
            where TObj : Sage_Container, IFindFirstNext {

            Func<bool> find_first = null;
            Func<bool> find_next = null;
            First_Next ret = null;

            find_first = () =>
            {
                if (!obj.FindFirst(field, value))
                    return false;
                if (!obj.Deleted && obj[field2].Safe_Equals(value2))
                    return true;
                return find_next();
            };

            find_next = () =>
            {
                while (true) {
                    if (!obj.FindNext(field, value))
                        return false;
                    if (!obj.Deleted && obj[field2].Safe_Equals(value2))
                        return true;
                }
            };
            ret = new First_Next(find_first, find_next);

            return ret;

        }

        /// <summary>
        /// Returns a flip-flop which traverses all invoice documents
        /// with the specified INVOICE_DATE ("today") and TAKEN_BY ("pos_id")
        /// values. Only matching invoice documents are examined.
        /// "by_date" determines which of the fields is used in the database query.
        /// The invoice date is only compared for using the year/month/day components,
        /// i.e. the precise time of day is not significant.
        /// </summary>
        First_Next
        Days_Documents_First_Next(InvoiceData oInvoiceData,
                                 bool by_date,
                                 DateTime today,
                                 string pos_id) {

            First_Next first_next;
            if (by_date) {
                first_next = Get_First_Next(oInvoiceData, INVOICE_DATE, today, TAKEN_BY, pos_id);
            }
            else {
                var padded = pos_id.PadRight(60);
                first_next = Get_First_Next(oInvoiceData, TAKEN_BY, padded, INVOICE_DATE, today);
            }

            return first_next;
            //    = new First_Next(
            //    () =>
            //    {
            //        if (!oInvoiceData.FindFirst(INVOICE_DATE, today))
            //            return false;
            //        if (oInvoiceData[TAKEN_BY].ToString() == pos_id)
            //            return true;
            //        return first_next;
            //    },
            //    () =>
            //    {
            //        while (true) {
            //            if (!oInvoiceData.FindNext(INVOICE_DATE, today))
            //                return false;
            //            if (oInvoiceData[TAKEN_BY].ToString() == pos_id)
            //                return true;
            //        }
            //    });
            //}
            //else {
            //    var padded = pos_id.PadRight(60);
            //    first_next = new First_Next(
            //    () =>
            //    {
            //        if (!oInvoiceData.FindFirst(TAKEN_BY, padded))
            //            return false;
            //        if (oInvoiceData[INVOICE_DATE].ToDateTime().Same_Day(today))
            //            return true;
            //        return first_next;
            //    },
            //    () =>
            //    {
            //        while (true) {
            //            if (!oInvoiceData.FindNext(TAKEN_BY, padded))
            //                return false;
            //            if (oInvoiceData[INVOICE_DATE].ToDateTime().Same_Day(today))
            //                return true;
            //        }
            //    });
            //}

        }

        /// <summary>
        /// Returns a flip-flop which traverses all invoice documents
        /// that have the specified ACCOUNT_REF ("account_ref") *OR* that are unposted.
        /// Posted invoices with the right ACCOUNT_REF and unposted documents with
        /// the wrong ACCOUNT_REF *may also be examined*.
        /// "by_account_ref" determines which of the fields is used in the database query.
        /// </summary>
        First_Next
        Get_Unposted_First_Next(InvoiceData oInvoiceData,
                               bool by_account_ref,
                               string account_ref) {

            First_Next first_next;
            if (by_account_ref) {
                string padded = account_ref.PadRight(8, ' ');
                first_next = Get_First_Next(oInvoiceData, ACCOUNT_REF, padded, POSTED_CODE, (sbyte)0);
            }
            else {
                first_next = Get_First_Next(oInvoiceData, POSTED_CODE, (sbyte)0, ACCOUNT_REF, account_ref);
            }

            return first_next;

            //if (by_account_ref) {
            //    // SDO documentation:
            //    /* "If it's a string you are attempting to find then 
            //    To get an exact match you will need to pad the 
            //    field with spaces to the correct length */
            //    string padded = account_ref.PadRight(8, ' ');
            //    first_next = new First_Next(
            //    () => oInvoiceData.FindFirst(ACCOUNT_REF, padded),
            //    () => oInvoiceData.FindNext(ACCOUNT_REF, padded));
            //}
            //else {
            //    first_next = new First_Next(
            //    () => oInvoiceData.FindFirst(POSTED_CODE, (sbyte)0),
            //    () => oInvoiceData.FindNext(POSTED_CODE, (sbyte)0));

            //}
            //return first_next;
        }

        First_Next
        Days_Transactions_First_Next(HeaderData oHeaderData,
                                    bool by_date,
                                    DateTime today,
                                    string pos_id) {

            First_Next first_next;
            if (by_date) {
                first_next = Get_First_Next(oHeaderData, POSTED_DATE, today, USER_NAME, pos_id);
            }
            else {
                string padded = pos_id.PadRight(32, ' ');
                first_next = Get_First_Next(oHeaderData, USER_NAME, padded, POSTED_DATE, today);
            }

            return first_next;

            //First_Next first_next;

            //if (by_date) {
            //    first_next = new First_Next(
            //    () => oHeaderData.FindFirst(POSTED_DATE, today),
            //    () => oHeaderData.FindNext(POSTED_DATE, today));
            //}
            //else {
            //    first_next = new First_Next(
            //    () => oHeaderData.FindFirst(USER_NAME, pos_id),
            //    () => oHeaderData.FindNext(USER_NAME, pos_id));
            //}

        }


        void Walk_Days_Transactions(string pos_id,
                                    DateTime today,
                                    bool by_date,
                                    Action<Payment_Type, HeaderData> act_sales,   // <-- post-dated cheque transaction
                                    Action<Payment_Type, HeaderData> act_receipt, // <-- receipt on account
                                    Action<HeaderData> act_payment,
                                    Action<HeaderData> act_refund) {

            var oHeaderData = WS.Create<HeaderData>();

            var first_next = Days_Transactions_First_Next(oHeaderData, by_date, today, pos_id);

            while (first_next) {

                var type = (TransType)(sbyte)oHeaderData[TYPE];

                var inv_ref = oHeaderData[INV_REF].ToString();
                var details = oHeaderData[DETAILS].ToString();

                var is_some_sort_of_receipt = (type == TransType.sdoSA);

                if (is_some_sort_of_receipt) {

                    bool sales;
                    Payment_Type pay_type;

                    bool ok = Get_Payment_Method_And_Context(inv_ref, details,
                                                            out sales, out pay_type);

                    Try(ok);

                    if (sales) {

                        act_sales(pay_type, oHeaderData);

                        continue;

                    }
                    else {

                        act_receipt(pay_type, oHeaderData);

                        continue;
                    }


                }

                var is_payment = (type == TransType.sdoCP);

                if (is_payment) {

                    act_payment(oHeaderData);

                    continue;

                }

                bool is_refund = (inv_ref == REFUND);

                if (is_refund) {

                    if (Version > 12)
                        is_refund = (type == TransType.sdoSP);
                    else
                        is_refund = (type == TransType.sdoBP);

                }


                if (is_refund) {

                    act_refund(oHeaderData);

                    continue;

                }

            }


        }

        Day_Totals
        Get_Transaction_Data_Ex(string pos_id,
                                DateTime today,
                                bool by_date,

                     out decimal sales_postdated,
                     out decimal payments,
                     out decimal refunds) {


            decimal tmp_sales_postdated, tmp_payments, tmp_refunds;

            sales_postdated = payments = refunds = 0.0m;
            tmp_sales_postdated = tmp_payments = tmp_refunds = 0.0m;

            var receipts_cash = 0.0m;
            var receipts_credit = 0.0m;
            var receipts_gift = 0.0m;
            var receipts_postdated = 0.0m;
            var receipts_cheques = 0.0m;

            Func<HeaderData, decimal> get_total =
            data =>
            {
                decimal amt, tax;
                amt = Convert.ToDecimal(data[NET_AMOUNT]);
                tax = Convert.ToDecimal(data[TAX_AMOUNT]);
                return amt + tax;
            };

            Action<Payment_Type, HeaderData> act_sales =
            (type, data) =>
            {

                decimal amt, tax, total;
                amt = Convert.ToDecimal(data[NET_AMOUNT]);
                tax = Convert.ToDecimal(data[TAX_AMOUNT]);
                total = amt + tax;

                if (type != Payment_Type.Post_Dated_Cheque)
                    true.tift();

                if (tax != 0.0m)
                    true.tift();

                tmp_sales_postdated += total;

            };

            Action<Payment_Type, HeaderData> act_receipt =
            (pay_type, data) =>
            {
                var total = get_total(data);
                switch (pay_type) {

                    case Payment_Type.Cash:
                        receipts_cash += total; break;

                    case Payment_Type.Credit:
                        receipts_credit += total; break;

                    case Payment_Type.Gift:
                        receipts_gift += total; break;

                    case Payment_Type.Cheque:
                        receipts_cheques += total; break;

                    case Payment_Type.Post_Dated_Cheque:
                        receipts_postdated += total; break;

                    default:
                        true.tift(); break;

                }
            };

            Action<HeaderData> act_payment =
            (data) =>
            {
                tmp_payments += get_total(data);
            };


            Action<HeaderData> act_refund =
            (data) =>
            {
                tmp_refunds += get_total(data);
            };


            Walk_Days_Transactions(pos_id, today, by_date,
                                    act_sales,
                                    act_receipt,
                                    act_payment,
                                    act_refund);


            sales_postdated = tmp_sales_postdated;
            payments = tmp_payments;
            refunds = tmp_refunds;

            // TODO: Write a script that makes sure that these strings (cash, credit...)
            // always appear in the same order

            var ret = new Day_Totals(today, receipts_cash,
                                       receipts_credit,
                                       receipts_gift,
                                       receipts_cheques,
                                       receipts_postdated);



            return ret;
        }



        Day_Totals
        Get_Transaction_Data(string pos_id,
                             DateTime today,
                             bool by_date,

                             out decimal sales_postdated,
                             out decimal payments,
                             out decimal refunds) {


            var oHeaderData = WS.Create<HeaderData>();

            sales_postdated = payments = refunds = 0.0m;

            decimal receipts_cash = 0.0m;
            decimal receipts_credit = 0.0m;
            decimal receipts_gift = 0.0m;
            decimal receipts_postdated = 0.0m;
            decimal receipts_cheques = 0.0m;

            bool is_payment;
            bool is_some_sort_of_receipt;
            bool is_refund;

            string user;
            decimal total;

            TransType type;
            Payment_Type pay_type;

            bool sales;

            var first_next = Days_Transactions_First_Next(oHeaderData, by_date, today, pos_id);

            while (first_next) {

                /*       Our Transaction?        */

                user = oHeaderData[USER_NAME].ToString();

                if (user != pos_id)
                    continue;

                /*       Valid transaction        */


                type = (TransType)(sbyte)oHeaderData[TYPE];

                is_some_sort_of_receipt = (type == TransType.sdoSA);
                is_payment = (type == TransType.sdoCP);

                var inv_ref = oHeaderData[INV_REF].ToString();

                if (Version > 12) {
                    is_refund = (type == TransType.sdoSP &&
                                 inv_ref == REFUND);

                }
                else {
                    is_refund = (type == TransType.sdoBP &&
                                 inv_ref == REFUND);

                }

                if (!(is_some_sort_of_receipt || is_payment || is_refund))
                    continue;



                {
                    decimal amt, tax;
                    amt = Convert.ToDecimal(oHeaderData[NET_AMOUNT]);
                    tax = Convert.ToDecimal(oHeaderData[TAX_AMOUNT]);
                    total = amt + tax;

                    if (!is_payment)
                        if (tax != 0.0m)
                            true.tift();
                }

                /*       Expense Payment        */

                if (is_payment) {
                    payments += total;
                    continue;

                }

                /*       Refund        */

                if (is_refund) {
                    refunds += total;
                    continue;
                }

                /*       Sales Payment or Receipt on Account        */

                string details = oHeaderData[DETAILS].ToString();

                Try(Get_Payment_Method_And_Context(inv_ref, details,

                                       out sales, out pay_type));

                if (sales) {

                    /*       Sales Payment      */

                    // Only postdated cheques are examined in this routine
                    // or indeed CAN be examined here

                    if (pay_type != Payment_Type.Post_Dated_Cheque) true.tift();

                    /*       Post-dated Cheque        */


                    sales_postdated += total;


                    continue;

                }



                /*       Receipt on Account        */

                switch (pay_type) {

                    case Payment_Type.Cash:
                        receipts_cash += total; break;

                    case Payment_Type.Credit:
                        receipts_credit += total; break;

                    case Payment_Type.Gift:
                        receipts_gift += total; break;

                    case Payment_Type.Cheque:
                        receipts_cheques += total; break;

                    case Payment_Type.Post_Dated_Cheque:
                        receipts_postdated += total; break;

                    default:
                        true.tift(); break;

                }


            }

            // TODO: Write a script that makes sure that these strings (cash, credit...)
            // always appear in the same order
            var ret = new Day_Totals(today, receipts_cash,
                       receipts_credit,
                       receipts_gift,
                       receipts_cheques,
                       receipts_postdated);

            return ret;
        }

        /*       Called by Get_Transaction_Data_       */

        // Given an SA's reference and details, retrieves the payment type and the 
        // type of the document which caused this transaction to be posted
        bool Get_Payment_Method_And_Context(string reference,
                                            string details,

                                            out bool sales,
                                            out Payment_Type type) {

            /*       31 August 09        */


            bool exact = (details == SALES_PAYMENT ||
                          details == RECEIPT_ON_ACCOUNT);


            sales = details.StartsWith(SALES_PAYMENT);

            if (exact) {

                /*       31 August 09        */

                if (reference == CASH) {
                    type = Payment_Type.Cash;
                }
                else if (reference == CREDIT) {
                    type = Payment_Type.Credit;
                }
                else if (reference == GIFT) {
                    type = Payment_Type.Gift;
                }
                else {
                    true.tift("Unrecognized transaction reference field: " + reference);
                    throw new ApplicationException();
                }
            }
            else {

                if (!sales) {
                    if (!details.StartsWith(RECEIPT_ON_ACCOUNT))
                        true.tift();
                }

                bool current;
                string number;

                Read_Cheque_Details(details, out sales, out current, out number);

                if (number != reference)
                    true.tift();

                type = current ? Payment_Type.Cheque : Payment_Type.Post_Dated_Cheque;

                return true;

            }


            return true;

        }


        public List<IBreakdown_Data>
        Get_Breakdown(string pos_id,
                      DateTime today,
                      EOS_Breakdown_Type breakdown_type,
                      Payment_Type payment_type) {

            if (breakdown_type == EOS_Breakdown_Type.Sales_Receipts) {
                return Get_Invoice_Breakdown(pos_id, today, payment_type);
            }

            if (breakdown_type == EOS_Breakdown_Type.Receipts_On_Account) {
                return Get_Transaction_Breakdown(pos_id, today, payment_type);
            }

            true.tift();
            return null;

        }

        public List<IBreakdown_Data>
        Get_Invoice_Breakdown(string pos_id,
                              DateTime today,
                              Payment_Type payment_type) {

            var by_date = Settings.Calculate_Days_Documents_Scanning_Date;

            using (Establish_Connection()) {

                List<IBreakdown_Data> ret = null;

                Action<InvoiceData> act = (oInvoiceData) =>
                {
                    if (ret == null)
                        ret = new List<IBreakdown_Data>(oInvoiceData.Count);

                    Payment_Info payment_info;

                    if (!Get_Payment_Info(oInvoiceData, out payment_info))
                        true.tift();

                    if (payment_info.Payment_Amount(payment_type) == 0.0m)
                        return;

                    Func<string, decimal> dec = field => Convert.ToDecimal(oInvoiceData[field]);
                    Func<string, int> int32 = field => Convert.ToInt32(oInvoiceData[field]);
                    Func<string, DateTime> date = field => Convert.ToDateTime(oInvoiceData[field]);
                    Func<string, string> str = field => oInvoiceData[field].ToString();

                    var TOTAL_V = dec(ITEMS_NET) + dec(ITEMS_TAX);
                    var PAID = dec(AMOUNT_PREPAID);
                    var acc_ref = str(ACCOUNT_REF);

                    var number = int32(INVOICE_NUMBER);
                    var invoice_date = date(INVOICE_DATE);

                    //decimal TOTAL_DISC_V, BRUTE_V;

                    //Get_Invoice_Brute(number, out BRUTE_V);


                    //TOTAL_DISC_V = BRUTE_V - TOTAL_V;

                    // ASSERTION
                    // (BRUTE_V_1 == BRUTE_V).tiff();

                    var data =
                    new Invoice_Breakdown_Data(
                                                number,
                                                acc_ref,
                                                invoice_date,
                                                payment_info,
                                                TOTAL_V,
                                                PAID,
                                                0.00m,//(TOTAL_DISC_V * 100.0m) / BRUTE_V, //TOTAL_DISC_V,
                                                0.00m
                        //BRUTE_V
                                              );

                    ret.Add(data);
                };

                Walk_Documents_Data(pos_id, today, Settings.Calculate_Days_Documents_Scanning_Date, act);

                return ret ?? new List<IBreakdown_Data>();
            }
        }

        public List<IBreakdown_Data>
        Get_Transaction_Breakdown(string pos_id,
                                  DateTime today,
                                  Payment_Type payment_type) {

            var ret = new List<IBreakdown_Data>(100);

            Action<Payment_Type, HeaderData> _1 = (__1, __2) => { };
            Action<HeaderData> _2 = _ => { };


            Action<Payment_Type, HeaderData> act_receipt = (type, data) =>
            {
                if (payment_type != type)
                    return;

                var number = data[FIRST_SPLIT /*sic*/].ToInt32();
                var details = data[DETAILS].ToString();
                var account = data[ACCOUNT_REF].ToString();
                var date = data[DATE].ToDateTime();
                var net = data[NET_AMOUNT].ToDecimal();
                var tax = data[TAX_AMOUNT].ToDecimal();

                var breakdown = new Transaction_Breakdown_Data(number, account, date, details, type, net, tax);

                ret.Add(breakdown);

            };

            bool by_date = Settings.Calculate_Days_Documents_Scanning_Date;

            Walk_Days_Transactions(pos_id, today, by_date,
                                   _1, act_receipt, _2, _2);

            return ret;
        }

        [Obsolete("Broken in every way")]
        bool Get_Invoice_Brute(int invoice_number,
                               out decimal brute_v) {

            var record = WS.Create<InvoiceRecord>();

            brute_v = 0.0m;

            record[INVOICE_NUMBER] = invoice_number;

            if (!record.Find(false))
                return false;


            var item = (InvoiceItem)record.Link;

            if (!item.MoveFirst())
                return false;

            Func<string, decimal> dec = field => Convert.ToDecimal(item[field]);


            do {
                var BRUTE_NV_HP = dec(FULL_NET_AMOUNT);
                var BRUTE_TAX_HP = BRUTE_NV_HP * dec(TAX_RATE) / 100.0m;


                var BRUTE_V = BRUTE_NV_HP.RoundDown(ROUNDING_PRECISION) +
                              BRUTE_TAX_HP.RoundDown(ROUNDING_PRECISION);

                brute_v += BRUTE_V;

            } while (item.MoveNext());


            return true;
        }

    }
}
