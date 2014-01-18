using System;
using System.Drawing;


using Fairweather.Service;

#if WINFORMS
using System.Windows.Forms;


#endif

using Printer = System.Drawing.Printing.PrintDocument;

namespace Fairweather.Service
{

    public class Pos_GDI_Printer : Pos_Printer_Engine
    {
        readonly Lazy_Dict<Font, byte> chars_per_line;
        readonly Print_Engine engine;



        readonly int page_width;
        readonly int dpi;


        Graphics g;

        /// <summary>
        /// Immutable
        /// </summary>
        public Print_Document Document { get; set; }

        public Print_Engine Engine {
            get { return engine; }
        }

        Print_Frame anchor;
        Print_Frame last;



        const GraphicsUnit mm = GraphicsUnit.Millimeter;
        const GraphicsUnit pix = GraphicsUnit.Pixel;

        static readonly Font Default_Font = new Font("Courier New", 12);


#if WINFORMS
        public bool Show_Page_Setup { get; set; }
        public bool Show_Print_Dialog { get; set; }
        public bool Show_Preview { get; set; }

        void OnPageSetup(PageSetupDialog dialog) {
            PageSetup.Raise(this, Args.Make(dialog));
        }
        void OnPrintPreview(PrintPreviewDialog dialog) {
            PrintPreviewDialog.Raise(this, Args.Make(dialog));
        }
        void OnPrintDialog(PrintDialog dialog) {
            PrintDialog.Raise(this, Args.Make(dialog));
        }

        public event Handler<PageSetupDialog> PageSetup;
        public event Handler<PrintPreviewDialog> PrintPreviewDialog;
        public event Handler<PrintDialog> PrintDialog;

#endif


        static Print_Engine Get_Engine() {
            return new Print_Engine(new Printer(), new Print_Document());
        }
        static Print_Engine Get_Engine(Printer printer) {
            return new Print_Engine(printer, new Print_Document());
        }

        public Pos_GDI_Printer(int page_width_in_mm, int dpi)
            : this(page_width_in_mm, dpi, Get_Engine()) {
        }

        public Pos_GDI_Printer(int page_width_in_mm, int dpi, Printer printer)
            : this(page_width_in_mm, dpi, Get_Engine(printer)) {

        }

        byte Get_Chars(Font font) {

            var ret = (byte)T.Characters_Per_Line(g, font, page_width, pix);

            return ret;

        }

        public Pos_GDI_Printer(int page_width_in_mm, int dpi, Print_Engine engine)
            : base() {


            this.engine = engine;
            this.Document = engine.Document;

            this.dpi = dpi;
            engine.PageSettings.PrinterResolution.X = dpi;
            engine.PageSettings.PrinterResolution.Y = dpi;


            this.page_width = (int)C.MM_to_Pix(page_width_in_mm, dpi);


            chars_per_line = new Lazy_Dict<Font, byte>(Get_Chars, 4);

            Current_Font = Default_Font;

            Printout_Count = 1;


        }


        /*       Helpers        */


        Endo<Rectangle> Get_Translator(HAlignment align, Size sz) {

            int width = sz.Width;
            var hz_transl = 0;

            if (align != HAlignment.Left)
                hz_transl = page_width - width;

            if (align == HAlignment.Center)
                hz_transl /= 2;


            return rect =>
            {
                var ret = new Rectangle(rect.Location.Translate(hz_transl, 0), sz);
                return ret;
            };

        }


        Print_Frame Prepare_Frame(HAlignment alignment, Size size, GraphicsUnit _) {

            var frame = new Relative_Frame(last, Get_Translator(alignment, size));

            last = new Relative_Frame(last, rect => rect.Translate(0, size.Height));

            return frame;
        }


        /*       Overrides        */

        Bitmap bmp;
        internal override bool Initialize_Inner() {

            this.bmp = new Bitmap(1, 1);
            bmp.SetResolution(dpi, dpi);
            this.g = Graphics.FromImage(bmp);

            Document.Clear();
            Document.Default_Font = Current_Font;

            anchor = new Absolute_Frame(0, 0, Size.Empty);
            last = anchor;

            return true;

        }



        internal override bool Leave_Inner() {




            if (Preview_Mode) {
                this.engine.Document = Document;
                return true;
            }

            var count = Printout_Count;
            try {
#if WINFORMS
                if (Show_Page_Setup)
                    engine.Show_Page_Setup(OnPageSetup);

                if (Show_Preview)
                    engine.Show_Print_Preview(OnPrintPreview);

                if (Show_Print_Dialog) {

                    var result = engine.Show_Print_Dialog((short)count, OnPrintDialog);
                    if (result != DialogResult.OK) {
                        return true;
                    }

                }
                else
#endif
                    for (int ii = 0; ii < count; ++ii)
                        engine.Print();

            }
            catch (System.ComponentModel.Win32Exception ex) {

                //if (ErrorMode == ErrorMode.Throw_Original)
                //      throw;
                //else
                On_Printing_X(ex);

            }

            this.g.Try_Dispose();
            this.bmp.Try_Dispose();

            return true;
        }


        public bool Print_Graphical_Line_Breaks {
            get;
            set;
        }

        static readonly Lazy<Pen>
        Line_Pen = new Lazy<Pen>(() => new Pen(Color.Black, 1.5f));

        public override void Print_Line_Break() {

            if (!Print_Graphical_Line_Breaks) {
                base.Print_Line_Break();
                return;
            }

            var text = 'x'.Repeat(Characters_Per_Line);

            var width = (int)T.String_Width(g, text, Current_Font, pix);

            var size = new Size(page_width, T.How_Tall(text, Characters_Per_Line, Current_Font));

            var frame = Prepare_Frame(HAlignment.Left, size, pix);

            var pt = new Point(0, Current_Font.Height / 2);

            //pt = pt.Translate(1, 0);
            //width--;
            //width--;

            var line1 = new Primitive_Line(Line_Pen, pt, width, 0);


            var elem = new Print_Element(frame, line1);

            //var line2 = new Primitive_Line(Pens.Black, pt.Translate(0, 1), width, 0);
            //var elem = new Print_Element(frame, line1, line2);

            Document.Add_Element(elem);

        }

        public override void Print_Text(string text) {

            var size = new Size(page_width, T.How_Tall(text, Characters_Per_Line, Current_Font));

            var frame = Prepare_Frame(HAlignment.Left, size, pix);

            var primitive = new Primitive_Text(null, Current_Font, null, text);

            // in case of debugging:
            // var rect = new Primitive_Rectangle(null, Brushes.Green, null);
            // var elem = new Print_Element(frame, rect, primitive);

            var elem = new Print_Element(frame, primitive);

            Document.Add_Element(elem);

        }

        protected override void Print_Image_Inner(string filename, HAlignment alignment) {

            var img = H.Get_Bitmap(filename);

            var frame = Prepare_Frame(alignment, img.Size, pix);

            var primitive = new Primitive_Image(img);

            var elem = new Print_Element(frame, primitive);

            Document.Add_Element(elem);

        }

        public override void Print_Barcode(string barcode_string) {
            throw new NotImplementedException();
        }





        public override bool Supports_Font(Pos_Print_Font font) {
            return font is Pos_GDI_Font;

        }

        public override void Set_Font(Pos_Print_Font font) {

            if (!Supports_Font(font))
                return;

            Current_Font = ((Pos_GDI_Font)font).Font;


            Document.Default_Font = Current_Font;

        }



        /*       Properties        */


        public override bool Initialized {
            get {
                return Document != null;
            }
        }

        // Indicates that no actual printing will be done after the document
        // has been composed
        public bool Preview_Mode { get; set; }

        public Font Current_Font { get; set; }

        public int Printout_Count { get; set; }

        protected override byte Characters_Per_Line {
            get {

                return chars_per_line[Current_Font];
            }
        }

        public override bool Is_Async { get { return false; } }


        /*       Misc        */


        public override void Open_Cash_Drawer() {
        }

        public override void New_Page() {
        }

        public override object Printer_Object { get { return engine; } }




    }
}
