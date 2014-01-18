
using System;
using System.Drawing.Printing;


using Printer = System.Drawing.Printing.PrintDocument;

#if WINFORMS
using System.Windows.Forms;
#endif

namespace Fairweather.Service
{


    /// <summary>
    /// This class outputs the contents of a Fairweather.Service.Print_Document to
    /// a printer.
    /// It also covers some auxiliary functionality, such as Show_Print_Preview etc.
    /// </summary>
    public class Print_Engine : Print_Engine_Base
    {

        /*       Consider exposing some of the properties of the Printer        */

        public Print_Engine() :
            this(new Printer(), new Print_Document()) { }

        public Print_Engine(Print_Document document)
            : this(new Printer(), document) { }

        public Print_Engine(string printer_name, Print_Document document)
            : this(Get_Printer(printer_name), document) { }


        public Print_Engine(Printer printer, Print_Document document) {

            document.tifn();
            printer.tifn();

            this.printer = printer;
            this.Document = document;

            printer.BeginPrint += (_, e) => Begin_Print(e);
            printer.PrintPage += (_, e) => Print_Page(e);
            printer.EndPrint += new PrintEventHandler(doc_EndPrint);

        }


        static Printer Get_Printer(string printer_name) {

            var doc = new Printer();

            doc.PrinterSettings.PrinterName = printer_name;

            return doc;

        }



#if WINFORMS
        public Form Dialog_Owner {
            get;
            set;
        }

        public DialogResult Show_Print_Preview() {
            return Show_Print_Preview(null);
        }

        public DialogResult Show_Print_Preview(Action<PrintPreviewDialog> modifier) {


            using (var dialog = new PrintPreviewDialog()) {

                dialog.Document = printer;
                if (modifier != null)
                    modifier(dialog);

                var ret = dialog.ShowDialog(Dialog_Owner);
                return ret;

            }

        }


        /// <summary>
        /// Displays a Print Settings dialog to the user, allowing
        /// the end user to change printing settings and initiate
        /// printing.
        /// The printing settings are persisted after the printing is
        /// complete.
        /// This overload allows the user to pre-select the number of copies
        /// entered in the dialog.
        /// </summary>
        public DialogResult Show_Print_Dialog(short? copies) {
            return Show_Print_Dialog(copies, null);
        }

        public DialogResult Show_Print_Dialog(short? copies, Action<PrintDialog> modifier) {

            using (var dialog = new PrintDialog()) {

                dialog.Document = printer;
                dialog.UseEXDialog = true;

                if (copies.HasValue)
                    dialog.PrinterSettings.Copies = copies.Value;

                if (modifier != null)
                    modifier(dialog);

                var ret = dialog.ShowDialog(Dialog_Owner);

                if (ret == DialogResult.OK) {
                    printer.PrinterSettings = dialog.PrinterSettings;
                    Print();
                }

                return ret;

            }


        }


        /// <summary>
        /// Shows a Page Setup dialog allowing the end user to
        /// modify the page settings to be used in subsequent printing.
        /// The page settings are persisted.
        /// </summary>
        public DialogResult Show_Page_Setup() {
            return Show_Page_Setup(null);
        }

        public DialogResult Show_Page_Setup(Action<PageSetupDialog> modifier) {

            using (var dialog = new PageSetupDialog()) {

                dialog.EnableMetric = true;
                dialog.Document = printer;
                dialog.AllowPrinter = false;

                dialog.PageSettings = printer.DefaultPageSettings;
                if (modifier != null)
                    modifier(dialog);
                var ret = dialog.ShowDialog(Dialog_Owner);

                if (ret == DialogResult.OK) {

                    printer.DefaultPageSettings = dialog.PageSettings;
                    printer.PrinterSettings = dialog.PrinterSettings;

                }

                return ret;
            }

        }

#endif

        public void Print() {

            Reset(true);

            try {

                printer.Print();

            }
            catch (System.ComponentModel.Win32Exception) {

                // "The device is not ready"
                Reset(true);
                throw;

            }

        }

        public void Reset(bool null_producer) {

            Current_Page = null;

            if (null_producer)
                producer = null;

        }

        public int? Current_Page {
            get;
            set;
        }


        public event EventHandler<PrintEventArgs> BeginPrint;

        public event EventHandler<PrintEventArgs> EndPrint;

        public event EventHandler<PrintPageEventArgs> PrintPage;



        public string Default_Printer {
            get { return PrinterSettings.PrinterName; }
            set { PrinterSettings.PrinterName = value; }

        }
        public PageSettings PageSettings {
            get {
                return printer.DefaultPageSettings;
            }
        }
        public PrinterSettings PrinterSettings {
            get {
                return printer.PrinterSettings;
            }
        }
        public Print_Document Document {
            get;
            set;
        }

        public Printer Printing_Provider {
            get { return printer; }
        }


        // ****************************


        void Begin_Print(PrintEventArgs e) {


            producer = Document.Get_Printer();

            this.producer.tifn();

            Reset(false);
            Current_Page = 0;

            BeginPrint.Raise(this, e);

        }

        void Print_Page(PrintPageEventArgs e) {

            PrintPage.Raise(this, e);
            ++Current_Page;

            var rect = e.MarginBounds;
            var g = e.Graphics;


            var pair = producer(rect, this);

            var to_print = pair.First;

            e.HasMorePages = pair.Second;

            foreach (var print in to_print)
                print(g);

        }

        void doc_EndPrint(object sender, PrintEventArgs e) {

            Reset(true);

            EndPrint.Raise(this, e);

        }

        Print_Producer producer;

        readonly Printer printer;




    }




}












