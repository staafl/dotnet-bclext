
/*       SCRAP "*DATA vs *RECORD"        */

//public bool Bench_Data_VS_Record() {
//    using (new Connection_State(this)) {
//        {

//            var sw = new Stopwatch();

//            var oStockData = (SalesData)vWS.Create("SalesData");

//            GC.Collect();
//            GC.WaitForPendingFinalizers();

//            int cnt = oStockData.Count();
//            sw.Start();

//            for (int ii = 0; ii < cnt; ++ii) {
//                oStockData.Read(ii);
//            }

//            sw.Stop();

//            var elapsed_time = sw.ElapsedMilliseconds;
//            var elapsed_ticks = sw.ElapsedTicks;

//            string output = "{0} ms, {1} ticks".Printf(elapsed_time, elapsed_ticks);
//            Console.WriteLine(output);
//            sw.Reset();
//        }


//        {
//            var sw = new Stopwatch();

//            GC.Collect();
//            GC.WaitForPendingFinalizers();

//            var oStockRecord = (SalesRecord)vWS.Create("SalesRecord");

//            sw.Start();

//            oStockRecord.MoveFirst();

//            while (oStockRecord.MoveNext()) ;

//            sw.Stop();

//            var elapsed_time = sw.ElapsedMilliseconds;
//            var elapsed_ticks = sw.ElapsedTicks;

//            string output = "{0} ms, {1} ticks".Printf(elapsed_time, elapsed_ticks);
//            Console.WriteLine(output);
//            sw.Reset();
//        }
//    }

//    return true;
//}

///
//#if false
//public bool Some_Test1() {

//            var state = this.EnsureConnectionEstablished();
//            try {
//                var watch = new Stopwatch();

//                GC.Collect(2);

//                GC.WaitForFullGCComplete();
//                GC.WaitForPendingFinalizers();

//                watch.Start();
//                for (int ii = 0; ii < iters; ++ii) {
//                    var record = vWS.CreateContainer<SalesRecord>();
//                }
//                watch.Stop();

//                Console.WriteLine("{0} ms.", watch.ElapsedMilliseconds);
//                Console.WriteLine("{0} ticks.", watch.ElapsedTicks);
//            }
//            finally {
//                EnsureConnectionRestored(state);
//            }

//            return true;
//        }


//        public bool Some_Test2() {

//            var state = this.EnsureConnectionEstablished();
//            try {
//                var watch = new Stopwatch();

//                GC.Collect(2);

//                GC.WaitForFullGCComplete();
//                GC.WaitForPendingFinalizers();

//                watch.Start();
//                for (int ii = 0; ii < iters; ++ii) {
//                    var record = (PurchaseRecord)vWS.Create("PurchaseRecord");
//                }
//                watch.Stop();

//                Console.WriteLine("{0} ms.", watch.ElapsedMilliseconds);
//                Console.WriteLine("{0} ticks.", watch.ElapsedTicks);
//            }
//            finally {
//                EnsureConnectionRestored(state);
//            }

//            return true;
//        }

//        static public void Test1() {
//            throw new Exception("Test disabled.");
//            //var sgr = new SageDataRecords();
//            //for (int jj = 0; jj < 10; ++jj) {
//            //    DateTime dt;
//            //    List<string> temp1;
//            //    List<string> temp2;

//            //    GC.Collect(2);
//            //    GC.WaitForPendingFinalizers();

//            //    dt = DateTime.Now;
//            //    for (int ii = 0; ii < 1; ++ii)
//            //        ; ;// sgr.GetProducts(out temp1, out temp2);

//            //    Console.WriteLine(DateTime.Now - dt);
//            //}
//            //Console.ReadKey();
//            //Console.ReadKey();
//        }
//#if MEMO
//                    static public void Test2() {

//                        int loops = 5;
//                        int cycle_begin = 100;
//                        int capacity = 1000;
//                        int record_cnt = 10000;

//                        Func<int, int> change = ii => ii * 10;

//                        debug.output("Testing:");
//                        debug.newline();

//                        debug.indent();
//                        debug.output("Loops {0}", loops);
//                        debug.output("Record count {0}", record_cnt);
//                        debug.output("Capacity {0}", capacity);
//                        debug.unindent();

//                        debug.macro("separate");

//                        var sgr = new SageDataRecords();
//                        ConnectionStatus state = sgr.EnsureConnectionEstablished();
//                        try {

//                            for (int ii = 0, cycle_num = cycle_begin;
//                                ii < loops;
//                                ++ii, cycle_num = change(cycle_num)) {

//                                debug.output("Iteration {0}", ii);
//                                debug.indent();
//                                debug.output("Cycles {0}", cycle_num);

//                                var Memo = new MemoizedRecordCollection<SalesRecord>(sgr.vWS, capacity);
//                                List<string> accs;

//                                {
//                                    var custs = sgr.GetCustomerAccounts()
//                                                   .Take(record_cnt)
//                                                   .Select(kvp => kvp.Key).ToArray();

//                                    int c_len = custs.Length;

//                                    accs = new List<string>(cycle_num);
//                                    Random rand = new Random();

//                                    for (int jj = 0; jj < cycle_num; ++jj) {

//                                        int num = rand.Next(0, c_len);
//                                        accs.Add(custs[num]);

//                                    }
//                                }
//                                record rec;
//                                record srec = (record)sgr.vWS.Create("SalesRecord");
//                                Stopwatch watch = new Stopwatch();

//                                //***********************//
//                                // Warming up

//                                watch.Start();

//                                foreach (var str in accs) {
//                                    Memo.Get_Record(str, out rec);

//                                    srec.Fields.Item("ACCOUNT_REF").Value = str;
//                                    srec.Find(false);
//                                }
//                                Memo.Clear_Records();

//                                watch.Stop();
//                                watch.Reset();

//                                //***********************//

//                                debug.output("Unmemoized run");
//                                debug.flush();
//                                debug.indent();

//                                GC.Collect(2);
//                                GC.WaitForPendingFinalizers();

//                                watch.Start();
//                                foreach (var str in accs) {

//                                    srec.Fields.Item("ACCOUNT_REF").Value = str;
//                                    srec.Find(false);

//                                    var accref = (string)srec.Fields.Item("ACCOUNT_REF").Value;
//                                }
//                                watch.Stop();
//                                debug.output("MSec {0} : Clocks {1}", watch.ElapsedMilliseconds,
//                                                                      watch.ElapsedTicks);
//                                watch.Reset();
//                                debug.unindent();

//                                //***********************//
                                
//                                GC.Collect(2);
//                                GC.WaitForPendingFinalizers();

//                                debug.output("Memoized run");
//                                debug.flush();
//                                debug.indent();

//                                watch.Start();
//                                foreach (var str in accs) {
//                                    Memo.Get_Record(str, out rec);

//                                    var fields = rec.Fields;
//                                    var accref = (string)fields.Item("ACCOUNT_REF").Value;

//                                }
//                                watch.Stop();
//                                debug.output("MSec {0} : Clocks {1}", watch.ElapsedMilliseconds,
//                                                                      watch.ElapsedTicks);
//                                watch.Reset();
//                                debug.unindent();

//                                //***********************//

//                                GC.Collect(2);
//                                GC.WaitForPendingFinalizers();

//                                debug.output("Memoized run");
//                                debug.flush();
//                                debug.indent();

//                                watch.Start();
//                                foreach (var str in accs) {
//                                    Memo.Get_Record(str, out rec);

//                                    var fields = rec.Fields;
//                                    var accref = (string)fields.Item("ACCOUNT_REF").Value;

//                                }
//                                watch.Stop();
//                                debug.output("MSec {0} : Clocks {1}", watch.ElapsedMilliseconds,
//                                                                      watch.ElapsedTicks);
//                                watch.Reset();
//                                debug.unindent();

//                                //***********************//

//                                debug.unindent();
//                            }
//                            debug.readkey();

//                        }
//                        finally {
//                            sgr.EnsureConnectionRestored(state);
//                        }
//                    }
//                  class MemoizedRecordCollection<T> where T : record
//                {
//                    readonly WorkSpace m_ws;
//                    readonly int m_capacity;
//                    readonly Dictionary<string, MemoizedRecord> m_records;
//                    int m_count;

//                    string m_first_record;
//                    string m_last_record;

//                    public MemoizedRecordCollection(WorkSpace ws, int capacity) {

//                        m_ws = ws;
//                        m_capacity = capacity;
//                        m_records = new Dictionary<string, MemoizedRecordCollection<T>.MemoizedRecord>(capacity);
//                    }
//                    public void Clear_Records() {
//                        lock (m_records) {
//                            m_records.Clear();
//                            m_count = 0;
//                        }
//                    }
//                    public bool Get_Record(string index_string, out record record) {

//                        MemoizedRecord rec;
//                        if (m_records.TryGetValue(index_string, out rec)) {
//                            record = rec;
//                            return true;
//                        }

//                        rec = new MemoizedRecord(this);

//                        if (!rec.Find(index_string, false)) {
//                            record = null;
//                            return false;
//                        }

//                        Add_A_Record(index_string, rec);
//                        record = rec;
//                        return true;
//                    }

//                    void Add_A_Record(string index_string, MemoizedRecord record) {

//                        lock (m_records) {

//                            if (m_count >= m_capacity) {
//                                Remove_A_Record();
//                            }

//                            ++m_count;
//                            m_records.Add(index_string, record);
//                        }
//                    }
//                    void Remove_A_Record() {

//                        lock (m_records) {
//                            m_records.Remove(m_records.First().Key);
//                            --m_count;
//                        }
//                    }
//                    class MemoizedRecord : record
//                    {
//                        static readonly string st_default_index;
//                        static readonly string st_creation_string;
//                        static MemoizedRecord() {

//                            st_default_index = Dispatch_Service.Default_Index<T>();
//                            st_creation_string = Dispatch_Service.Creation_String<T>();
//                        }
//                        readonly T m_record;
//                        readonly MemoizedRecordCollection<T> m_parent;

//                        string m_current_record;
//                        bool m_active;

//                        public MemoizedRecord(MemoizedRecordCollection<T> parent) {

//                            m_parent = parent;
//                            m_record = (T)m_parent.m_ws.Create(st_creation_string);
//                        }

//                        public string Default_Index { get { return m_record.Default_Index; } }
//                        public bool Edit() {
//                            bool ret = false;
//                            return ret;
//                        }
//                        public bool AddNew() {
//                            bool ret = false;
//                            return ret;
//                        }
//                        public bool Update() {
//                            bool ret = false;
//                            return ret;
//                        }

//                        public bool Find(string index_string, bool partial) {

//                            this.Fields.Item(st_default_index).Value = index_string;

//                            bool result = m_record.Find(partial);

//                            if (!result)
//                                return false;

//                            return true;
//                        }
//                        public bool Find(bool partial) {

//                            bool result = m_record.Find(partial);

//                            if (!result)
//                                return false;

//                            return true;
//                        }
//                        public bool MoveFirst() {
//                            bool ret = false;
//                            return ret;
//                        }
//                        public bool MoveNext() {
//                            bool ret = false;
//                            return ret;
//                        }
//                        public bool MovePrev() {
//                            bool ret = false;
//                            return ret;
//                        }
//                        public bool MoveLast() {
//                            bool ret = false;
//                            return ret;
//                        }

//                        //void Refresh_Parent() {

//                        //    if (!m_current_record.IsNullOrEmpty())
//                        //        m_parent.m_records.Remove(m_current_record);

//                        //    m_current_record = (string)m_record.Fields.Item(m_record.Default_Index).Value;
//                        //    m_parent.m_records.Add(m_current_record, this);
//                        //}

//                        public Fields Fields { get { return m_record.Fields; } }
//                    }
//                }
//#endif

//        //Dumping remittance data from sage
//        public void TestRem() {

//            ConnectionStatus state = EnsureConnectionEstablished();
//            try {

//                Console.WriteLine(GetLastTransactionNumber());

//                RemittanceIndex = (RemittanceIndex)vWS.Create("RemittanceIndex");
//                RemittanceIndex.MoveFirst();

//                Dispatch.RemittanceRecord RemittanceRecord = (Dispatch.RemittanceRecord)vWS.Create("RemittanceRecord");
//                do {

//                    Console.WriteLine(RemittanceIndex["URN"]);
//                    Console.WriteLine(RemittanceIndex["RECORD_NUMBER"]);
//                    Console.WriteLine(RemittanceIndex.DataRecordNumber);
//                    Console.WriteLine(RemittanceIndex.IndexRecordNumber);
//                    Console.WriteLine(RemittanceIndex.Key);


//                    Console.WriteLine();
//                } while (RemittanceIndex.MoveNext());

//                return;
//                //RemittanceRecord.MoveFirst();
//                //string[] fields = { "ACCOUNT_REF", "BANK", "URN", "INDEX", "TRAN_TYPE", "GROSS_AMOUNT", "NET_AMOUNT", "PAYMENT_NO", 
//                //                "PAYMENT_AMOUNT", "PAYMENT_FLAG", "LASTLINE_FLAG", "NAME", "DATE", "DETAILS", "TAX_CODE",
//                //                "VAT_AMOUNT", "INV_REF" };
//                //Array.Sort(fields);
//                //do {
//                //    foreach (string str in fields) {
//                //        Console.Write("{0}: ", str.PadRight(20, ' '));
//                //        Console.WriteLine(RemittanceRecord[str]);
//                //    }
//                //    Console.WriteLine();
//                //} while (RemittanceRecord.MoveNext());
//            }
//            finally {
//                EnsureConnectionRestored(state);
//            }
//        }

//        //Benchmark to determine which version TransactionPost (reflection or delegate) performs
//        //better
//        public void TestRefl() {
//            ConnectionStatus state = EnsureConnectionEstablished();
//            try {
//                NativeMethods.AllocConsole();

//                DateTime dt = DateTime.Now;
//                GC.Collect(2);
//                for (int ii = 0; ii < 100; ++ii) {
//                    TransactionPost = (TransactionPost)vWS.Create("TransactionPost");
//                    object aa;

//                    for (int jj = 0; jj < 500; ++jj) {
//                        aa = TransactionPost.HDGet();
//                        aa = TransactionPost.HDGet();
//                        aa = TransactionPost.HDGet();
//                        aa = TransactionPost.HDGet();
//                    }
//                }

//                Console.WriteLine(DateTime.Now - dt);
//                dt = DateTime.Now;

//                Console.ReadKey();
//            }
//            finally {
//                EnsureConnectionRestored(state);
//            }
//        }
//#endif


//    }
//}

//        public bool Test_Products2() {

//            // we always typecast any value passed to an SDO field - we recommend that you follow this 
//            // practice in your own applications.

//            var record = (InvoiceRecord)vWS.Create("InvoiceRecord");
//            var ipost = (InvoicePost)vWS.Create("InvoicePost");
//            var stock_record = (StockRecord)vWS.Create("StockRecord");
//            var header = (Fields)ipost.HDGet();

//            InvoiceItem iitem_read;
//            InvoiceItem iitem_write;

//            stock_record.MoveFirst();
//            record.MoveFirst();

//            ipost.Type = LedgerType.sdoLedgerInvoice;

//            header["INVOICE_NUMBER"] = record["INVOICE_NUMBER"];
//            header["ACCOUNT_REF"] = record["ACCOUNT_REF"];
//            header["INVOICE_TYPE_CODE"] = (InvoiceType)InvoiceType.sdoProductInvoice;
//            header["INVOICE_DATE"] = header["INVOICE_DATE"];
//            header["BANK_CODE"] = "1200";

//            header["ADDRESS_1"] = record["ADDRESS_1"];
//            header["ADDRESS_2"] = record["ADDRESS_2"];
//            header["ADDRESS_3"] = record["ADDRESS_3"];
//            header["ADDRESS_4"] = record["ADDRESS_4"];
//            header["ADDRESS_5"] = record["ADDRESS_5"];
//            header["CONTACT_NAME"] = record["CONTACT_NAME"];
//            header["CURRENCY"] = record["CURRENCY"];
//            header["CURRENCY_USED"] = record["CURRENCY_USED"];
//            header["CUST_ORDER_NUMBER"] = record["CUST_ORDER_NUMBER"];
//            header["CUST_TEL_NUMBER"] = record["CUST_TEL_NUMBER"];
//            header["DEL_ADDRESS_1"] = record["DEL_ADDRESS_1"];
//            header["DEL_ADDRESS_2"] = record["DEL_ADDRESS_2"];
//            header["DEL_ADDRESS_3"] = record["DEL_ADDRESS_3"];
//            header["DEL_ADDRESS_4"] = record["DEL_ADDRESS_4"];
//            header["DEL_ADDRESS_5"] = record["DEL_ADDRESS_5"];
//            header["FOREIGN_RATE"] = header["FOREIGN_RATE"];
//            header["GLOBAL_DETAILS"] = record["GLOBAL_DETAILS"];
//            header["GLOBAL_NOM_CODE"] = record["GLOBAL_NOM_CODE"];
//            header["GLOBAL_TAX_CODE"] = header["GLOBAL_TAX_CODE"];
//            header["NAME"] = record["NAME"];
//            header["NOTES_1"] = record["NOTES_1"];
//            header["NOTES_2"] = record["NOTES_1"];
//            header["NOTES_3"] = record["NOTES_3"];
//            header["ORDER_NUMBER"] = record["ORDER_NUMBER"];
//            header["PAYMENT_REF"] = record["PAYMENT_REF"];
//            header[TAKEN_BY] = record[TAKEN_BY];

//            iitem_read = (InvoiceItem)record.Link;

//            if (iitem_read.MoveFirst()) {

//                do {
//                    iitem_write = ipost.AddItem();

//                    iitem_write["STOCK_CODE"] = iitem_read["STOCK_CODE"];
//                    iitem_write["NOMINAL_CODE"] = iitem_read["NOMINAL_CODE"];
//                    iitem_write["TAX_CODE"] = iitem_read["TAX_CODE"];

//                    iitem_write["COMMENT_1"] = iitem_read["COMMENT_1"];
//                    iitem_write["COMMENT_2"] = iitem_read["COMMENT_2"];
//                    iitem_write["DESCRIPTION"] = iitem_read["DESCRIPTION"];
//                    iitem_write["FULL_NET_AMOUNT"] = iitem_read["FULL_NET_AMOUNT"];
//                    iitem_write["NET_AMOUNT"] = iitem_read["NET_AMOUNT"];
//                    iitem_write["QTY_ORDER"] = iitem_read["QTY_ORDER"];
//                    iitem_write["TAX_AMOUNT"] = iitem_read["TAX_AMOUNT"];
//                    iitem_write["TAX_RATE"] = iitem_read["TAX_RATE"];
//                    iitem_write["UNIT_OF_SALE"] = iitem_read["UNIT_OF_SALE"];
//                    iitem_write["UNIT_PRICE"] = iitem_read["UNIT_PRICE"];


//                } while (iitem_read.MoveNext());

//            }

//            Console.WriteLine("Move next: {0}", iitem_read.MoveNext());

//            iitem_write = ipost.AddItem();

//            iitem_write["STOCK_CODE"] = stock_record["STOCK_CODE"];
//            iitem_write["DESCRIPTION"] = stock_record["DESCRIPTION"];

//            iitem_write["FULL_NET_AMOUNT"] = 100;
//            iitem_write["NET_AMOUNT"] = 100;
//            iitem_write["NOMINAL_CODE"] = stock_record["NOMINAL_CODE"];
//            iitem_write["QTY_ORDER"] = 2;
//            iitem_write["TAX_AMOUNT"] = 17.5;
//            iitem_write["TAX_CODE"] = stock_record["TAX_CODE"];
//            iitem_write["TAX_RATE"] = 17.5;
//            iitem_write["UNIT_PRICE"] = 50;

//            //iitem_write["COMMENT_1"] = "";
//            //iitem_write["COMMENT_2"] = "";
//            //iitem_write["UNIT_OF_SALE"] = "";
//            //
//            bool ret = ipost.Update();
//            Console.WriteLine("Update: {0}", ret);

//            Console.WriteLine("Last sage error: " + sdoEng.LastErrorText);

//            return ret;
//        }

//        public bool Test_Products3() {

//            bool ret = true;

//            var record = (InvoiceRecord)vWS.Create("InvoiceRecord");

//            {
//                ret = record.MoveLast();
//                {
//                    var fields = new List<string> 
//                                {
//                    #region
//                    "ACCOUNT_REF",
//                    "ADDRESS_1",
//                    "ADDRESS_2",
//                    "ADDRESS_3",
//                    "ADDRESS_4",
//                    "ADDRESS_5",
//                    "AMOUNT_PREPAID",
//                    "BANK_CODE",
//                    "BASE_TOT_NET",
//                    "BASE_TOT_TAX",
//                    "BASE_CARR_NET",
//                    "BASE_CARR_TAX",
//                    "BASE_SETT_DISC_RATE",
//                    "BASE_AMOUNT_PAID",
//                    "CARR_DEPT_NUMBER",
//                    "CARR_NET",
//                    "CARR_NOM_CODE",
//                    "CARR_TAX",
//                    "CARR_TAX_CODE",
//                    "CONSIGNMENT_REF",
//                    "CONTACT_NAME",
//                    "COURIER",
//                    "CURRENCY",
//                    "CURRENCY_USED",
//                    "CUST_DISC_RATE",
//                    "CUST_ORDER_NUMBER",
//                    "CUST_TEL_NUMBER",
//                    "DEF_TAX_CODE",
//                    "DEL_ADDRESS_1",
//                    "DEL_ADDRESS_2",
//                    "DEL_ADDRESS_3",
//                    "DEL_ADDRESS_4",
//                    "DEL_ADDRESS_5",
//                    "DELETED_FLAG",
//                    "DELIVERY_NAME",
//                    "DISCOUNT_TYPE",
//                    "EURO_GROSS",
//                    "EURO_RATE",
//                    "EXTERNAL_USAGE",
//                    "FIRST_ITEM",
//                    "FOREIGN_GROSS",
//                    "FOREIGN_RATE",
//                    "GLOBAL_DEPT_NUMBER",
//                    "GLOBAL_DETAILS",
//                    "GLOBAL_NOM_CODE",
//                    "GLOBAL_TAX_CODE",
//                    "INVOICE_DATE",
//                    "INVOICE_NUMBER",
//                    "INVOICE_TYPE_CODE",
//                    "ITEMS_NET",
//                    "ITEMS_TAX",
//                    "LAST_UPDATED",
//                    "NAME",
//                    "NOTES_1",
//                    "NOTES_2",
//                    "NOTES_3",
//                    "ORDER_NUMBER",
//                    "PAYMENT_REF",
//                    "PAYMENT_TYPE",
//                    "POSTED_CODE",
//                    "PRINTED_CODE",
//                    //"QUOTE_EXPIRY",
//                    "QUOTE_STATUS",
//                    "RECURRING_REFERENCE",
//                    "SETTLEMENT_DISC_RATE",
//                    "SETTLEMENT_DUE_DAYS",
//                    "STATUS",
//                    //"TAKEN",
//                    "TOTAL_BYTES",

//                    #endregion
//                                };

//                    var enumer = fields.OrderBy(str => str);

//                    foreach (var str in enumer) {
//                        Console.WriteLine("\t" + str + " = \t" + record[str].ToString());
//                        Console.Out.Flush();
//                    }
//                }
//            }

//            {
//                var fields = new List<string> 
//                                {
//                    #region
//"ADD_DISC_RATE",
//"BASE_FULL_NET",
//"BASE_NET",
//"BASE_TAX",
//"COMMENT_1",
//"COMMENT_2",
////"DELIVERY_DATE",
//"DEPT_NUMBER",
//"DESCRIPTION",
//"DISCOUNT_AMOUNT",
//"DISCOUNT_RATE",
//"EXT_ORDER_REF",
////"EXT_ORDER_REF_LINE",
//"FULL_NET_AMOUNT",
//"INVOICE_NUMBER",
//"ITEM_NUMBER",
//"JOB_REFERENCE",
//"NET_AMOUNT",
//"NEXT_ITEM",
//"NOMINAL_CODE",
//"OFFSET",
//"PREV_ITEM",
//"QTY_ALLOCATED",
//"QTY_DELIVERED",
//"QTY_DESPATCH",
//"QTY_INTRASTAT_CONFIRMED",
//"QTY_ORDER",
//"SERVICE_FILE",
//"SERVICE_FILE_SIZE",
//"SERVICE_FLAG",
//"SERVICE_ITEM_LINES",
//"STOCK_CODE",
//"TAX_AMOUNT",
//"TAX_CODE",
//"TAX_FLAG",
//"TAX_RATE",
//"TEXT",
//"UNIT_OF_SALE",
//"UNIT_PRICE"


//                    #endregion
//                                };
//                var enumer = fields.OrderBy(str => str);
//                var item = record.Link;
//                item.MoveFirst();
//                do {

//                    Console.WriteLine("New Item:");
//                    foreach (var str in enumer)
//                        Console.WriteLine("\t" + str + " = \t" + item[str].ToString());


//                }
//                while (item.MoveNext());
//            }

//            return ret;
//        }

//#endif