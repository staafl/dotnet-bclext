using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using DTA;

using Fairweather.Service;
using Standardization;
using Tag_Levels = Fairweather.Service.Pos_Notepad_Printer.Tag_Levels;
using System.IO;
namespace Common
{
    static partial class Pos_Printing_Utility
    {

        delegate Pos_Printer_Engine
        Engine_Maker(Printing_Helper helper, int copy_count, Company_Number company);

        static readonly Dictionary<Printing_Provider, Engine_Maker>
        Engine_Makers = new Dictionary<Printing_Provider, Engine_Maker>
            {   {Printing_Provider.Notepad, Get_Printer_Notepad},
                {Printing_Provider.Windows, Get_Printer_GDI},
                {Printing_Provider.Bixolon, Get_Printer_Bixolon}, 
                {Printing_Provider.Pos_For_Net, Get_Printer_Pos_For_Net},
            };


        static Pos_Printer_Engine
        Get_Printer(Printing_Helper helper, int copies) {

            var company = helper.Company;
            var provider = helper.Provider;

            Engine_Maker del;

            var preview = (helper.Scenario == Printing_Scenario.Preview);

            if (preview) {
                del = Get_Printer_GDI;
            }
            else {
                del = Engine_Makers[provider];
            }

            var ret = del(helper, copies, company);

            ret.Muddle_On_Through = !preview;
            ret.Error_Mode = Error_Mode.Throw;

            //ret.ErrorMode = ErrorMode.Throw_Original;

            //ret.ErrorMode = ErrorMode.LogAndRaise;
            //ret.Printing_Exception += (sender, args) => Printing_Exception_Handler(sender, args.Data);

            return ret;

        }

        static bool
        Printing_Exception_Handler(object sender, Exception ex) {

            Logging.Notify(ex);

            var msg = ex.Message;

            var show = "An unexpected error occurred while trying to print the document:\n\n";

            show += ex.Msg_Or_Type();

            Standard.Show(Message_Type.Error, show);

            return Standard.Ask("Would you like to try printing again?");

        }



        static Pos_Printer_Engine
        Get_Printer_Notepad(Printing_Helper helper, int copies, Company_Number _3) {

            if (copies == 0)
                return new Pos_Void_Printer();

            var filename = helper.Get_Filename();

            var ret = new Pos_Notepad_Printer(Owning_Form, 2000, filename);

            ret.Print_Tags = helper.Notepad_Print_Tags ?
                             Tag_Levels.Images | Tag_Levels.Misc :
                             Tag_Levels.None;

            ret.Open_File = helper.Notepad_Open_File;
            ret.Open_Dir = helper.Notepad_Open_Dir;

            return ret;
        }

        static Pos_Printer_Engine
        Get_Printer_Bixolon(Printing_Helper helper, int copies, Company_Number _) {

            Try_Close_Net_Printer(false);

            if (helper.Copies == 0)
                return new Pos_Void_Printer();

            var inner = new Pos_Bixolon_350_Printer();
            var ret = new Pos_Printing_Recorder(inner, copies);

            return ret;

        }

        static Pos_Net_Printer net_printer;
        static object net_printer_id;
        static bool disposer_attached;

        static void Try_Close_Net_Printer(bool proc_exit) {
            var snap = net_printer;
            if (snap != null &&
                snap.Initialized &&
                snap.Keep_Open) {
                snap.Keep_Open = false;
                snap.Leave();

            }

            net_printer = null;
            net_printer_id = null;
        }


        static public Pos_Printer_Engine
        Get_Printer_Pos_For_Net(Printing_Helper helper, int copies, Company_Number _) {

            bool keep_open = helper.OPOS_Keep_Printer_Open;

            var net_printer_snap = net_printer;

            if (!keep_open || 
                (net_printer_id != null && net_printer_id != helper.OPOS_Printer_Id)) {
                Try_Close_Net_Printer(false);
                net_printer_snap = null;

            }


            if (net_printer_snap == null) {

                var form = Owning_Form;

                if (form != null)
                    form.Force_Handle();

                Pos_Net_Printer.Get_Instance(
                      form,
                      helper.OPOS_Logical_Name,
                      helper.OPOS_High_Quality_Letters,
                      helper.OPOS_Image_DPI,
                      out net_printer_snap);



            }

            Pos_Printer_Engine ret = net_printer_snap;

            net_printer_snap.Paper_Cutting = helper.OPOS_Paper_Cutting_Percentage;

            net_printer_snap.Keep_Open = keep_open;

            if (!net_printer_snap.Is_Simulator && copies > 1) {
                var recorder = new Pos_Printing_Recorder(net_printer_snap, copies);
                recorder.Economize_Actions = true;
                ret = recorder;

            }

            if (keep_open) {
                net_printer = net_printer_snap;
                net_printer_id = helper.OPOS_Printer_Id;

                if (!disposer_attached) {
                    // DomainUnload is never called on the default appdomain
                    AppDomain.CurrentDomain.ProcessExit += (_1, _2) =>
                    {
                        Try_Close_Net_Printer(true);

                    };
                }
                disposer_attached = true;
            }


            return ret;
        }

        static Pos_Printer_Engine
        Get_Printer_GDI(Printing_Helper helper, int copies, Company_Number company) {

            Try_Close_Net_Printer(false);
            
            var scenario = helper.Scenario;

            var doc_name = scenario.Get_String();
            var printer_name = helper.GDI_Printer_To_Use;
            int width = helper.GDI_Page_Width_MM;


            var doc = new PrintDocument();

            doc.DocumentName = doc_name;

            bool _;
            int dpi = helper.GDI_DPI;

            doc.DefaultPageSettings = M.Get_Page_Settings(company, out _);
            doc.PrinterSettings.PrinterName = printer_name;

            var ret = new Pos_GDI_Printer(width, dpi, doc);

            ret.Engine.Default_Printer = helper.GDI_Printer_To_Use;
            ret.Set_Font(helper.GDI_Font);
            ret.Printout_Count = copies;
            ret.Print_Graphical_Line_Breaks = true;
            ret.Show_Page_Setup = helper.GDI_Show_Page_Setup;
            ret.Show_Preview = helper.GDI_Show_Preview;
            ret.Show_Print_Dialog = helper.GDI_Show_Print_Dialog;

            if (scenario == Printing_Scenario.Preview) {
                ret.Preview_Mode = true;
            }

            return ret;

        }






        static public void
        Print_Sample_Receipt(Printing_Helper helper) {
            Print_Sample_Receipt(helper, null);

        }

        static public void
        Print_Sample_Receipt(Printing_Helper helper, Action<Pos_Printer_Engine> callback) {

            var pair = Prepare_Sample_Receipt(helper);
            var printing_data = pair.First;
            var printer = pair.Second;

            Print_Sales_Receipt(printing_data, helper.Copies, printer);

        }

        static public Pair<Printing_Data_Sale, Pos_Printer_Engine>
        Prepare_Sample_Receipt(Printing_Helper helper) {

            decimal total = 11.5m;
            decimal vat = 1.5m;
            decimal vatless = total - vat;
            decimal payment = 9.0m;
            decimal outstanding = total - payment;

            var payment_info =
                  new Payment_Info(new Cheque_Info[0], new Cheque_Info[0], payment, 0.0m, 0.0m);

            var account = new Account_Record("KIN001", "Kinghorn & French", 3);

            var screen_info =
                  new Screen_Info(account, "", "", "", DateTime.Now, 11, "", Versioning.InvoiceType.sdoProductInvoice);

            var grid_info = new Grid_Info(0m, 0.0m, total, total, vatless, vat);

            var data = Tax_Code.st_data;
            try {
                Tax_Code.st_data = new SortedList<short, decimal> { { 1, 0.15m } };

                var product = new Product("ENV001", "", "", "", "", "Envelopes and Mailing", "Stationery", 10, 1);

                var item = new Item_Info(product,
                    1.0m, "",
                    total, total,
                    total, total,
                    0.0m, 0.0m,
                    0.0m, 0.0m,
                    0.0m, 0.0m,
                    0.0m,
                    0.0m);

                var items = new[] { item };

                var posting_info = new Posting_Info(payment_info, screen_info, grid_info, outstanding);

                var printing_data = new Printing_Data_Sale(helper, posting_info, 21, items, outstanding, null);

                var printer = Get_Printer(helper, helper.Copies);


                return Pair.Make(printing_data, printer);


            }
            finally {
                Tax_Code.st_data = data;
            }

        }



        static public void
        Print_Generic<TData>(TData data, int copies, Pos_Printer_Engine printer)
              where TData : Printing_Data {

            printer = printer ?? Get_Printer(data.Helper, copies);

            var dict = new Dictionary<Type, Action>{
                {typeof(Printing_Data_Sale), 
                      () => Compose_Sales_Receipt(printer, data as Printing_Data_Sale)},
                {typeof(Printing_Data_Receipt), 
                      () => Compose_Receipt(printer, data as Printing_Data_Receipt)},
                {typeof(Printing_Data_EndOfShift), 
                      () => Compose_End_Of_Shift(printer, data as Printing_Data_EndOfShift)},

            };
            var act = dict[typeof(TData)];

            while (true)
                try {
                    using (printer.Use()) {
                        act();
                        break;

                    }
                }
                catch (Exception ex) {/*used to be XPrinting*/
                    if (ex is OutOfMemoryException)
                        throw;

                    if (Printing_Exception_Handler(printer, ex))
                        // retry
                        continue;

                    if (printer is Pos_Notepad_Printer)
                        break;

                    Pos_Notepad_Printer notepad;

                    var dir = Data.Get_Company_Directory(Data.Default_Creds.Company)
                                  .Cpath("failed printouts");

                    var file = "error - " + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".txt";
                    Directory.CreateDirectory(dir);

                    printer = notepad = new Pos_Notepad_Printer(
                        Owning_Form,
                        2000,
                        dir.Cpath(file));

                    notepad.Print_Tags = Tag_Levels.Images | Tag_Levels.Misc;// Tag_Levels.None;

                    notepad.Open_File = true;
                    notepad.Open_Dir = true;

                }
        }





        static public void
        Print_Sales_Receipt(Printing_Data_Sale data, int copies) {
            Print_Generic(data, copies, null);
        }


        static public void
        Print_Sales_Receipt(Printing_Data_Sale data, int copies, Pos_Printer_Engine predefined) {
            Print_Generic(data, copies, predefined);
        }





        static public void
        Print_Receipt(Printing_Helper helper, Receipt_Info receipt_info, int number, int copies) {

            var payment_info = receipt_info.Payment_Info;

            var old_bal = receipt_info.Old_Balance;

            var prepaid = payment_info.Amount_Prepaid;
            var new_bal = old_bal - prepaid;

            string name = receipt_info.Name;

            if (name.IsNullOrEmpty())
                name = receipt_info.Account_Ref;

            var date = DateTime.Now;

            var data = new
            Printing_Data_Receipt(helper, old_bal, prepaid, new_bal,
                                  name, number, date, true);

            Print_Receipt(data, copies);

        }

        static public void
        Print_Future_Cheque(Printing_Helper helper, string name, Future_Cheque_Info future, int count) {

            var cheque = future.Cheque;
            var date = cheque.Date;

            var number = future.Number;
            var amount = future.Amount;

            var new_bal = future.New_Balance;
            var old_bal = future.Old_Balance;


            var footer = new Pair<string>("Cheque No.", cheque.Number);

            var data = new
            Printing_Data_Receipt(helper, old_bal, amount, new_bal,
                                  name, number, date, false, footer);

            Print_Receipt(data, count);
        }


        static public void
        Print_Receipt(Printing_Data_Receipt data, int copies) {
            Print_Generic(data, copies, null);
        }




        static public void
        Print_End_Of_Shift(Printing_Data_EndOfShift data, int copies) {
            Print_Generic(data, copies, null);
        }



        static Func<int, Pos_Columns_Info>
        Sale_Items_Columns() {

            // Switched from absolute to relative column widths - 25 Nov                
            // Modifying these settings is risky

            return (int characters_per_line) =>
                {
                    var _ret = Pos_Columns_Info.From_Weighted(0, true,
                                    new int[] { 1, 1, 1, 0 },
                                    new double[] { 14, 14, 4, 5 }, characters_per_line);

                    _ret.Truncatable[1] = true; // description
                    return _ret;
                };

            ;


        }



        static public void
        Compose_Sales_Receipt(Pos_Printer_Engine printer, Printing_Data_Sale data) {

            var company = data.Company;
            var posting_info = data.Posting_Info;
            var number = data.Number;
            var items = data.Items;
            var remaining_balance = data.Remaining_Balance;
            var null_credit_note = data.Null_Credit_Note;

            var screen_info = posting_info.Screen_Info;
            var payment_info = posting_info.Payment_Info;
            var grid_info = posting_info.Totals_Info;
            var account = screen_info.Account.Value;

            var helper = data.Helper;
            var layout = helper.Layout;

            bool is_refund = null_credit_note.HasValue;


            string title = is_refund ? "Sales Refund" :
                           screen_info.Is_Product_Invoice ? "Sales Receipt" :
                           "Credit Note";

            printer.Print_Header(helper, title);

            printer.Set_Any_Of(Receipt_Number_Font(helper));

            printer.Print_Single_Line("No." + number.ToString(), HAlignment.Left);

            if (is_refund)
                printer.Print_Single_Line(
                    "Against Credit Note No." + null_credit_note.Value.Posted_Number,
                    HAlignment.Left);


            printer.Print_Empty_Line();

            printer.Print_Text(account.Name);

            printer.Print_Line_Break();

            var col_info = Sale_Items_Columns();

            var surcharge = 0.00m;

            foreach (var item in items) {

                var product = item.Product;

                /*       Printing changed from total payable to brute total        */
                /*       10th September        */

                var brute = item.Adjusted_Brute_V;
                var product_code = item.Product.Code;
                var qty = item.Qty;
                var disc = item.Unit_Discount_V;

                var description = product.Category_Or_Description(layout.Use_Category);

                surcharge += item.Total_Surcharge_V;

                printer.Print_Columns_Weighted(col_info,
                          product_code,
                          description,
                          qty.No_Trailing_Zeroes(),
                          brute.ToString(true));

            }

            printer.Print_Line_Break();


            decimal brute_adjusted = grid_info.Adjusted_Brute_V;
            decimal discount_adjusted = grid_info.Adjusted_Total_Disc_V;

            (brute_adjusted == (grid_info.Brute_V + surcharge)).tiff();
            (discount_adjusted == (grid_info.Total_Disc_V + surcharge)).tiff();


            printer.Print_Item("Total price:", brute_adjusted);
            printer.Print_Item("Discount:", discount_adjusted);

            printer.Print_Line_Break();

            printer.Print_Item("Total:", grid_info.Total_V);
            printer.Print_Item("Total payment:", payment_info.Amount_Prepaid);

            if (is_refund)
                printer.Print_Item("Credit note value:", null_credit_note.Value.Total_Amount);

            string outstanding_str = "Outstanding:";
            decimal outstanding = posting_info.Outstanding;

            if (outstanding <= 0.0m) {
                outstanding *= -1;
                outstanding_str = "Change:";
            }

            printer.Print_Item(outstanding_str, outstanding);

            printer.Print_Line_Break();

            if (remaining_balance.HasValue)
                printer.Print_Item("Customer Balance:", remaining_balance.Value);

            printer.Print_Item("VAT Amount Included:", grid_info.VAT);

            printer.Print_Empty_Line();

            printer.Print_Date(screen_info.Date, true);

            printer.Print_Empty_Line();

            if (posting_info.Outstanding > 0.0m) {

                printer.Print_Empty_Line();
                printer.Print_Empty_Line();

                printer.Print_Text("Signature:  ____________________");
                printer.Print_Text("            {0}".spf(account.Name));


            }


            if (!layout.Footer.IsNullOrEmpty()) {

                printer.Print_Empty_Line();

                //printer.Set_Any_Of(Footer_Text_Font(helper));

                printer.Print_Several_Lines(layout.Footer, HAlignment.Center);

                printer.Print_Empty_Line();


            }

            printer.Print_Trailing_White();

        }

        static public void
        Compose_Receipt(Pos_Printer_Engine printer, Printing_Data_Receipt data) {

            var company = data.Company;
            var old_balance = data.Old_Balance;
            var amount_prepaid = data.Amount_Prepaid;
            var new_balance = data.New_Balance;
            var name = data.Name;
            var number = data.Number;
            var date = data.Date;
            var print_hrs = data.Print_Hrs;
            var footer = data.Footer;

            printer.Print_Header(data.Helper, "Receipt");

            printer.Set_Font(A1x1_9_5f(false, false));

            printer.Print_Text("No." + number.ToString());

            printer.Print_Empty_Line();

            var payment_str = amount_prepaid.ToString(true);

            var block1 = "We have received from {0} the amount of €{1}."
                .spf(name, payment_str);

            printer.Set_Font(A1x1_7f(false, false));

            printer.Print_Block(block1, true);

            var payment_words = M.Cheque_Amount_To_Words(amount_prepaid, "Euro", "cent");

            var block2 = "(" + payment_words + ")";

            printer.Print_Block(block2, true);



            printer.Set_Font(A1x1_9_5f(false, false));

            printer.Print_Line_Break();


            if (old_balance.HasValue)
                printer.Print_Item("Previous Balance:", old_balance.Value);

            printer.Print_Item("Receipt Balance:", amount_prepaid);

            if (new_balance.HasValue)
                printer.Print_Item("New Balance:", new_balance.Value);

            printer.Print_Line_Break();

            if (!footer.Is_Empty()) {
                foreach (var pair in footer)
                    printer.Print_Item(pair.First, pair.Second);

                printer.Print_Empty_Line();

            }


            printer.Print_Date(date, print_hrs);

            printer.Print_Trailing_White();

        }

        static public void
        Compose_End_Of_Shift(Pos_Printer_Engine printer, Printing_Data_EndOfShift data) {

            var sales_data = data.Sales_Data;
            var receipts_data = data.Receipts_Data;
            var grand_data = data.Grand_Data;

            var docs = data.Docs;
            var payments = data.Payments;
            var final_total = data.Final_Total;
            var date = data.Date;
            var day_index = data.Day_Index;

            decimal invoices = docs.First;
            decimal credit_notes = docs.Second;

            (grand_data == (sales_data + receipts_data)).tiff();


            Action em_br_em = () => { printer.Print_Empty_Line(); printer.Print_Line_Break(); printer.Print_Empty_Line(); };

            printer.Print_Header(data.Helper, "End Of Day");


            printer.Set_Font(A1x1_9_5f(false, false));

            printer.Print_Empty_Line();


            printer.Print_Text("Date: " + date);
            printer.Print_Text("Day Index: " + day_index);

            printer.Print_Empty_Line();

            em_br_em();

            printer.Print_Text("Documents Raised");

            printer.Print_Empty_Line();

            printer.Print_Item("Invoices:", invoices);
            printer.Print_Item("Credit Notes:", credit_notes);

            em_br_em();

            printer.Print_Days_Data(sales_data, "Sales Payments");

            em_br_em();

            printer.Print_Days_Data(receipts_data, "Receipts on Account");

            em_br_em();

            printer.Print_Days_Data(grand_data, "Grand Total");

            em_br_em();

            printer.Print_Item("Payments:", payments);

            em_br_em();

            printer.Print_Item("Final Total:", final_total);

            printer.Print_Trailing_White();

        }


        /* Retired PrintEndOfDay - see repository revision 688 */




    }

}
