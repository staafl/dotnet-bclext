using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Text;
using DTA;
using Fairweather.Service;
using Versioning;

namespace Common
{
    public partial class Sage_Logic
    {
        public bool 
        Verify_Default_Codes(out List<Pair<Record_Type, string>> missing) {

            bool ret = true;

            var tmp_missing = new List<Pair<Record_Type, string>>();
            using (Establish_Connection()) {


                Action<Record_Type, string> check = (type, index) =>
                {
                    if (Verify_Record_Exists(type, index))
                        return;

                    ret = false;
                    tmp_missing.Add(Pair.Make(type, index));
                };


                var cash = Get_Default_Cash_Account();
                check(Record_Type.Sales, cash);

                var sales = Get_Default_Sales_Bank();
                check(Record_Type.Bank, sales);

                var payments = Get_Default_Payments_Account();
                check(Record_Type.Expense, payments);

            }

            missing = tmp_missing;
            return ret;
        }

        [Obsolete("Sage 16 Barcode fields support not verified")]
        public string[] Used_Barcode_Fields {
            get {

                string[] fields;
                Index_Type[] index_types;

                Get_Used_Barcode_Fields(this.Version, out fields, out index_types);

                return fields;

            }

        }

        public void
        Get_Used_Barcode_Fields(int version,
                                out string[] fields,
                                out Index_Type[] index_types) {


            var types = H.Get_Enum_Values<Index_Type>().arr();

            // (types.Length == 4).tiff();

            var fields_dict = new Dictionary<Ini_Field, Pair<Index_Type, Ini_Field>>
            {
                {DTA_Fields.POS_use_barcode1, Pair.Make(Index_Type.Bar1, DTA_Fields.POS_barcode1)},
                {DTA_Fields.POS_use_barcode2, Pair.Make(Index_Type.Bar2, DTA_Fields.POS_barcode2)},
                {DTA_Fields.POS_use_barcode3, Pair.Make(Index_Type.Bar3, DTA_Fields.POS_barcode3)},
            };

            // adjust these filters to pad the resultant array
            // i.e. let value = proxy[use] == 1 ? proxy[field] : ""
            
            var chosen = from kvp in fields_dict
                         where Proxy[kvp.Key /* use barcode 1 ? */] == "1" 
                         select kvp.Value;

            /* (Index_Type.Bar1, "POS_BARCODE_1") --> (Index_Type.Bar1, "INTERSTAT_COMM_CODE") */

            var pairs = chosen.Select(_t2 => Pair.Make(_t2.First, Proxy[_t2.Second]))
                              .Where(_t2 => !_t2.Second.IsNullOrEmpty());

            if (version >= 16)
                pairs = pairs.Pend(Pair.Make(Index_Type.Bar16, "BARCODE"), false);

            var bar_arr = pairs.Select(pair => pair.Second).ToArray();
            var ind_arr = pairs.Select(pair => pair.First).ToArray();

            int cnt = bar_arr.Length;

            fields = new string[cnt + 1];
            fields[0] = STOCK_CODE;

            index_types = new Index_Type[cnt + 1];
            index_types[0] = Index_Type.Stock;

            Array.Copy(bar_arr, 0, fields, 1, cnt);
            Array.Copy(ind_arr, 0, index_types, 1, cnt);



        }

        public Dictionary<string, List<Product_Index>>
        Get_Products(out List<string> duplicates1,
                     out List<string> duplicates2,

                     Progress_Reporter reporter,
                     int work_report_interval) {

            Fill_Tax_Codes(true);

            /*       ATTENTION        */
            #region comments - 20 lines
            // USING this routine the barcodes will be pulled to the beginning,
            // i.e. if barcode2 is not in use, but barcode3 is -> barcode3 will
            // be considered barcode2 FOR ALL ITEMS
            // IF this turns out to be a problem, pad bar_fields with empty strings
            // and insert an empty string check here
            // for (int ii = 0; ii < barcodes_cnt; ++ii) {

            //   var item = bar_fields[ii];
            //   --> if(item.IsNullOrEmpty()) continue;
            //    fields[ii] = oStockData[item].ToString();

            // }
            // 
            // the subsequent check for field.IsNullOrEmpty() will
            // make sure that the unused fields are skipped
            // 19th September 
            #endregion


            var seen_categories = new Set<short>();
            var ret = new Dictionary<string, List<Product_Index>>();
            var categories = new Dictionary<int, string>();

            // List of codes which cause conflicts
            var _duplicates1 = new Set<string>();

            // List of keys for items which are part of conflict,
            // i.e. the product codes
            var _duplicates2 = new Set<string>();

            string[] bar_fields;
            Index_Type[] index_types;


            Get_Used_Barcode_Fields(this.Version, out bar_fields, out index_types);

            var bar_fields_length = bar_fields.Length; // ! usually not equal to max_barcodes
            var conflict_count = 0;

            const int max_barcodes = 5;
            var fields = new string[max_barcodes]; // <--


            using (Establish_Connection()) {

                var oStockCat = WS.Create<StockCategory>();
                var oStockData = WS.Create<StockData>();
                var oStockIx = WS.Create<StockIndex>();

                int work = 0;


                bool conflicted = false;

                string description, category;
                short cat_num, tax_code;
                decimal price;

                Product prod;
                Product_Index index;

                int prod_cnt = oStockData.Count;


                reporter.Notify_Total(prod_cnt);


                /*       Main loop        */

                const string BARCODE = "BARCODE";
                using (FileStream fs = bar_fields.IndexOf(BARCODE) == -1 ? null :
                                        new FileStream(
                                            Conn.Path.Cpath("STOCK.DTA"),
                                            FileMode.Open,
                                            FileAccess.Read))

                    for (int record_number = 1; record_number <= prod_cnt; ++record_number) {

                        oStockData.Read(record_number);

                        if (oStockData.Deleted)
                            continue;

                        Array.Clear(fields, 0, max_barcodes);

                        for (int ii = 0; ii < bar_fields_length; ++ii) {

                            var item = bar_fields[ii];
                            if (item == BARCODE) {

                                oStockIx.Key = oStockData[STOCK_CODE].str();

                                oStockIx.Find(false).tiff();

                                var record = oStockIx.DataRecordNumber - 1;

                                fs.Position = 5401 * record + 5247;

                                const int bar_len = 60;

                                var bytes = new byte[bar_len];

                                (fs.Read(bytes, 0, bar_len) == bar_len).tiff();

                                fields[ii] = Encoding.ASCII.GetString(bytes).TrimEnd(' ');

                            }
                            else {
                                fields[ii] = oStockData[item].StringOrDefault(null);
                            }
                        }

                        conflicted = false;

                        description = Convert.ToString(oStockData[DESCRIPTION]);

                        price = Convert.ToDecimal(oStockData[SALES_PRICE]);

                        cat_num = Convert.ToInt16(oStockData[STOCK_CAT]);

                        tax_code = Convert.ToInt16(oStockData[TAX_CODE]);

                        if (!categories.TryGetValue(cat_num, out category)) {

                            oStockCat.Read(cat_num);

                            category = oStockCat[NAME].ToString();
                            categories[cat_num] = category;

                        }

                        for (int ii = 0; ii <= 4; ++ii) {
                            var var = fields[ii];
                            if (var != null)
                                fields[ii] = var.ToUpperInvariant();
                        }

                        prod = new Product(fields[0],
                                           fields[1],
                                           fields[2],
                                           fields[3],
                                           fields[4],
                                           description,
                                           category,
                                           price,
                                           tax_code);

                        for (int ii = 0; ii < bar_fields_length; ++ii) {

                            string field = fields[ii];

                            if (field.Trim().IsNullOrEmpty())
                                continue;

                            if (field == "0")
                                continue;


                            List<Product_Index> list;
                            bool old = ret.TryGetValue(field, out list);

                            if (!old) {

                                list = new List<Product_Index>();
                                ret[field] = list;

                            }
                            else {
                                /*       Duplicate        */

                                _duplicates1.Add(field);

                                /*       TAG 1        */

                                if (!conflicted)
                                    _duplicates2.Add(fields[0]);

                                /*               */

                                conflicted = true;

                            }

                            index = new Product_Index(prod, index_types[ii]);
                            list.Add(index);
                        }

                        if (conflicted)
                            ++conflict_count;

                        /*       Report progress        */

                        ++work;
                        if (work % work_report_interval == 0) {

                            reporter.Notify(work);

                        }

                    }


                reporter.Notify(prod_cnt);

                duplicates1 = _duplicates1.ToList();

                /*       Older version of _duplicates2 -> duplicates2 removed - see revision 690        */
                /*       Replaced with TAG1        */

                duplicates2 = _duplicates2.ToList();

                (duplicates2.Count == conflict_count).tiff();

                return ret;
            }
        }





        /*       Recalling of Sage Invoices        */

        public bool
        Get_Invoice_Data(int invoice_number,
                         Dictionary<string, Product_Index> products,
                          out string account_ref,
                          out Screen_Info screen_info,
                          out List<Item_Info> items,
                          out List<string> deleted_stock_records,
                          out Sage_Invoice_Data sage_invoice_data,
                          out Invoice_Recalling_State recalling_state) {

            H.assign(out screen_info, out sage_invoice_data);

            account_ref = "";
            recalling_state = 0;

            const int default_capacity = 31;

            items = new List<Item_Info>(default_capacity);
            deleted_stock_records = new List<string>();


            using (Establish_Connection()) {

                var oInvoiceRecord = WS.Create<InvoiceRecord>();

                oInvoiceRecord[INVOICE_NUMBER] = invoice_number;

                if (!oInvoiceRecord.Find(false)) {
                    recalling_state = Invoice_Recalling_State.Invalid_Invoice_Number;
                    return false;
                }

                //var debug = Sage_Fields.InvoiceRecord.Fields.Copy().Transform_Values(fld => oInvoiceRecord[fld.Name]);

                //File.WriteAllText("c:\\spatium_test (header) {0}.txt".spf(invoice_number), debug.Pretty_Print());

                account_ref = oInvoiceRecord[ACCOUNT_REF].ToString();

                Account_Record? opt_account = null;

                if (!account_ref.IsNullOrEmpty()) {

                    opt_account = Get_Account_Record(true, account_ref);

                    if (opt_account == null) {
                        recalling_state = Invoice_Recalling_State.Customer_Does_Not_Exist;
                        return false;
                    }

                }
                var is_credit = oInvoiceRecord.Is_Product_Credit;

                var is_settled = is_credit && Is_Settled(oInvoiceRecord);

                var date = oInvoiceRecord[INVOICE_DATE].ToDateTime().Date;
                var order_number = oInvoiceRecord[ORDER_NUMBER].ToString();
                var invoice_type = oInvoiceRecord.Invoice_Type;
                var posted = oInvoiceRecord[POSTED_CODE].ToBool();
                var printed = oInvoiceRecord[PRINTED_CODE].ToBool();

                var taken_by = oInvoiceRecord[TAKEN_BY].ToString();

                Payment_Info payment_info;

                if (Get_Payment_Info(oInvoiceRecord, out payment_info)) {

                    sage_invoice_data = new Sage_Invoice_Data(is_credit, is_settled, invoice_number, posted,
                                                              printed, taken_by, payment_info);

                }
                else {

                    var amount_paid = oInvoiceRecord[AMOUNT_PREPAID].ToDecimal();

                    sage_invoice_data = new Sage_Invoice_Data(is_credit, is_settled, invoice_number, posted,
                                                              printed, taken_by, amount_paid);


                }

                //var address = oInvoiceRecord.Address;

                screen_info = new Screen_Info(
                     opt_account,
                     "", "", "",
                     date, invoice_number,
                     order_number, invoice_type);

                var oInvoiceItem = (InvoiceItem)oInvoiceRecord.Link;

                if (!oInvoiceItem.MoveFirst())
                    return false;

                do {

                    string stock_code = oInvoiceItem[STOCK_CODE].ToString();

                    Product_Index ix;

                    if (!products.TryGetValue(stock_code, out ix)) {

                        deleted_stock_records.Add(stock_code);
                        continue;

                    }


                    var item = Read_Invoice_Item(oInvoiceItem, ix.Product);

                    items.Add(item);


                    //oStockRecord[STOCK_CODE] = stock_code;


                    //if (!oStockRecord.Find(false)) {
                    //    deleted_stock_records.Add(stock_code);
                    //    break;
                    //}

                    //var debug = Sage_Fields.InvoiceItem.Fields.Copy().Transform_Values(fld => oInvoiceItem[fld.Name]);  

                    //File.WriteAllText("c:\\spatium_test {0} (item).txt".spf(invoice_number), debug.Pretty_Print());


                } while (oInvoiceItem.MoveNext());

            }

            return true;

        }

        public Item_Info
        Read_Invoice_Item(InvoiceItem item,
                          Product product) {

            string description = item[DESCRIPTION].ToString();
            var qty = item[QTY_ORDER].ToDecimal();
            var total_nv = item[NET_AMOUNT].ToDecimal();
            var brute_nv = item[FULL_NET_AMOUNT].ToDecimal();
            var total_disc_nv = item[DISCOUNT_AMOUNT].ToDecimal();
            var rate = item[ADD_DISC_RATE].ToDecimal();
            var base_nv = item[UNIT_PRICE].ToDecimal();
            var vat = item[TAX_AMOUNT].ToDecimal();
            var vat_rate = item[TAX_RATE].ToDecimal() / 100.0m;

            var ret = new Item_Info(product, qty, description,
                                    brute_nv,
                                    total_nv,
                                    total_disc_nv,
                                    base_nv,
                                    rate,
                                    vat);


            ret.Unit_Price_V = ret.Unit_Price_NV + item[TAX_AMOUNT].ToDecimal();
            return ret;

        }

        /*       Miscellaneous */

        public void
        Get_Stock_Decimal_Precision(out int qty_decimal_precision, out int unit_price_decimal_precision) {

            using (Establish_Connection()) {

                var oSetupData = WS.Create<SetupData>();

                qty_decimal_precision = Convert.ToInt32(oSetupData[STOCK_QTYDP]);
                unit_price_decimal_precision = Convert.ToInt32(oSetupData[STOCK_UNITDP]);


            }

        }


        /// <summary> To Test </summary>
        int? First_Invoice_Data_Number(string account_ref) {

            using (Establish_Connection()) {

                var InvoiceRecord = WS.Create<InvoiceRecord>();
                var InvoiceIndex = WS.Create<InvoiceIndex>();

                InvoiceRecord[ACCOUNT_REF] = account_ref;
                if (!InvoiceRecord.Find(false))
                    return null;

                var invoice_number = Convert.ToInt32(InvoiceRecord[INVOICE_NUMBER]);
                InvoiceIndex.ObjKey = invoice_number;
                if (!InvoiceIndex.Find(false))
                    return null;

                var ret = InvoiceIndex.DataRecordNumber;
                return ret;
            }
        }

    }
}
