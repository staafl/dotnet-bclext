using System;
using System.Collections.Generic;
using Fairweather.Service;
using print;

namespace Fairweather.Service
{
    /// The sample from Bixolon's driver manual "Windows Driver Manual.pdf"
    //public void Sample_Print() {
    //    Win32PrintClass w32prn = new Win32PrintClass();

    //    w32prn.SetPrinterName("BIXOLON SAMSUNG SRP-350");

    //    // Open CashDrawer
    //    w32prn.OpenCashdrawer(2);	// 2 pin cashdrawer
    //    w32prn.OpenCashdrawer(5);	// 5 pin cashdrawer			

    //    // Print Image
    //    Command_Mode(w32prn);
    //    Print_Text(w32prn,"x");
    //    Print_Text(w32prn,"G");		// 1st NV Image
    //    Print_Text(w32prn,"H");		// 2nd NV Image

    //    // Print Receipt
    //    Set_Font(w32prn,9.5f, "FontA2x2", false, true);
    //    Print_Text(w32prn,"KPS's SHOPPING MALL");
    //    Set_Font(w32prn,9.5f, "FontA1x1", false, false);
    //    Print_Text(w32prn,"Buy Online or call");
    //    Print_Text(w32prn,"");
    //    Set_Font(w32prn,7f, "FontB2x1", false, true);
    //    Print_Text(w32prn,"1-800-915-3355");

    //    Command_Mode(w32prn);
    //    Print_Text(w32prn,comm_left_align);
    //    Set_Font(w32prn,9.5f, "FontA1x1", false, false);

    //    Print_Text(w32prn,"------------------------------------------");
    //    Print_Text(w32prn,"SAMSUNG SRP-350 Printer               $999");
    //    Print_Text(w32prn,"SRP-770 Label Printer                 $749");
    //    Print_Text(w32prn,"SRP-370 Thermal Receipt Printer     $1,299");
    //    Print_Text(w32prn,"SRP-270 Impact Receipt Printer      $1,299");
    //    Print_Text(w32prn,"RIF-BT10                              $949");
    //    Print_Text(w32prn,"SMP600                                $349");
    //    Print_Text(w32prn,"SRP-500 Inkjet Receipt Printer        $249");
    //    Print_Text(w32prn,"------------------------------------------");
    //    Set_Font(w32prn,9.5f, "FontA1x1", false, true);
    //    Print_Text(w32prn,"Total purchase :                    $5,893");
    //    Print_Text(w32prn,"Visa :                              $5,893");
    //    Set_Font(w32prn,9.5f, "FontA1x1", false, false);
    //    Print_Text(w32prn,"------------------------------------------");

    //    Command_Mode(w32prn);

    //    Print_Text(w32prn,"x");
    //    Print_Text(w32prn,"r");

    //    Set_Font(w32prn,20f, "Code128", false, false);
    //    Print_Text(w32prn,"{A{S12235884584645");
    //    Set_Font(w32prn,9.5f, "FontA1x1", false, false);
    //    Print_Text(w32prn,"Date : 04/08/2003     Time : 09:32");
    //    Print_Text(w32prn,"No : 00018857302");
    //    Print_Text(w32prn,"");
    //    Print_Text(w32prn,"");
    //    Print_Text(w32prn,"FREE 2nd Bay Interface ");
    //    Print_Text(w32prn,"With Purchase of ANY Dimension Desktop");
    //    Print_Text(w32prn,"Ends Today !");

    //    // Print Image
    //    Print_Image(w32prn, "sample.bmp");
    //    Print_Image(w32prn, "sample2.bmp");

    //    // Cut Receipt
    //    Command_Mode(w32prn);
    //    Print_Text(w32prn,comm_cut_partial);

    //    // Print
    //    w32prn.EndDoc();
    //}

    /// See the following address for info about PCL and proprietary POS printer languages
    // http://www.tek-tips.com/viewthread.cfm?qid=1192980&page=4

    /// Other control codes (source: "Windows Driver Manual.pdf")
    /*  
    5 - HT is output
    6 - LF is output
    7 - CR is output
    a - Open Drawer 2 (50ms drive pulse width)
    b - Open Drawer 2 (100ms drive pulse width)
    c - Open Drawer 2 (150ms drive pulse width)
    d - Open Drawer 2 (200ms drive pulse width)
    e - Open Drawer 2 (250ms drive pulse width)
    g - Cut Receipt (partial cut) without paper feeding
    i - Print the NV graphics data defined by the key code 0 and 0 in the Double Height Double Width mode
    j - Print the NV graphics data defined by the key code 0 and 1 in the Double Height Double Width mode
    k - Print the NV graphics data defined by the key code 0 and 2 in the Double Height Double Width mode
    l - Print the NV graphics data defined by the key code 0 and 3 in the Double Height Double Width mode
    m - Print the NV graphics data defined by the key code 0 and 4 in the Double Height Double Width mode
    p - HRI characters are not added to the bar code
    q - HRI characters are added at the top of the bar code using Font A
    r - HRI characters are added at the bottom of the bar code using Font A
    s - HRI characters are added at the top of the bar code using Font B
    t - HRI characters are added at the bottom of the bar code using Font B
    w - Text is aligned left
    x - Text is centered
    y - Text is aligned right
    A - Open Drawer 1 (50ms drive pulse width)
    B - Open Drawer 1 (100ms drive pulse width)
    C - Open Drawer 1 (150ms drive pulse width)
    D - Open Drawer 1 (200ms drive pulse width)
    E - Open Drawer 1 (250ms drive pulse width)
    G -  NV bit image No 1 is printed in the Normal mode
    H - NV bit image No 2 is printed in the Normal mode
    I - NV bit image No 3 is printed in the Normal mode
    J - NV bit image No 4 is printed in the Normal mode
    K - NV bit image No 5 is printed in the Normal mode
    P - Cut Receipt (partial cut)
    R - Print the NV graphics data defined by the key code 0 and 0 in Normal mode
    S - Print the NV graphics data defined by the key code 0 and 1 in Normal mode
    T - Print the NV graphics data defined by the key code 0 and 2 in Normal mode
    U - Print the NV graphics data defined by the key code 0 and 3 in Normal mode
    V - Print the NV graphics data defined by the key code 0 and 4 in Normal mode
    [ - NV bit image No 1 is printed in the Double Height Double Width mode
    ] - NV bit image No 2 is printed in the Double Height Double Width mode
    ^ - NV bit image No 3 is printed in the Double Height Double Width mode
    _ -  NV bit image No 4 is printed in the Double Height Double Width mode
    ` - NV bit image No 5 is printed in the Double Height Double Width mode
     */

    public class Pos_Bixolon_350_Printer : Pos_Printer_Engine
    {
        const string cst_printer_name = "BIXOLON SAMSUNG SRP-350";

        const int cashdrawer_2pin = 2;
        const int cashdrawer_5pin = 5;

        // Todo: make this font-specific
        const int cst_printer_cols = 42;

        const int cst_max_left_col = 35;

        const string comm_left_align = "w";
        const string comm_right_align = "y";
        const string comm_center = "x";

        const string comm_cut_partial_nolf = "g";
        const string comm_cut_partial_lf = "P";

        const string comm_barcode_no_hri = "p";
        const string comm_barcode_hri_top_A = "q";
        const string comm_barcode_hri_bottom_A = "r";

        const string comm_barcode_hri_top_B = "s";
        const string comm_barcode_hri_bottom_B = "t";

        const string comm_tab = "5";
        const string comm_linefeed = "6";
        const string comm_carriage_return = "7";



        const string cst_barcode_codabar = "Codabar";

        const string cst_barcode_code39 = "Code39";
        const string cst_barcode_code93 = "Code93";

        /* When choosing printer font Code128, the code set selection character 
        (“{A”, “{B”, or “{C”) must always be specified at the head of the text, e.g. 
        “{B1234”, when printing “1234”.
         */
        const string cst_barcode_code128 = "Code128";
        const string cst_code128_codeset_a = "{A";
        const string cst_code128_codeset_b = "{B";
        const string cst_code128_codeset_c = "{C";


        const string cst_barcode_jan8 = "JAN8(EAN)";
        const string cst_barcode_jan13 = "JAN13(EAN)";
        const string cst_barcode_itf = "ITF";
        const string cst_barcode_upc_a = "UPC-A";
        const string cst_barcode_upc_e = "UPC-E";

        /* Two-dimensional codes:
         * PDF417
            QR Code
       */


        /*
        Open Drawer 1/2
        Cut receipt
        Cut receipt(without paper feeding)
        Justification(Left/Center/Right)
        Output HT,Output LF, Output CR
        Barcode printing
        Print NV bit image*/
        const string cst_font_control = "FontControl";

        Win32PrintClass w32prn;

        public Pos_Bixolon_350_Printer()
            : base() {

        }



        public override bool Initialized {
            get { return w32prn != null; }
        }

        protected override void Print_Image_Inner(string filename, HAlignment alignment) {


            using (Set_Alignment(alignment)) {

                // Workaround - 2009 12 04
                w32prn.SetDeviceFont(7f, "FontB2x1", false, true);
                w32prn.PrintText(" ");

                w32prn.SetDeviceFont(7f, "FontA1x1", false, true);
                w32prn.PrintImage(filename);


            }


        }

        public override void Print_Single_Line(string line, HAlignment alignment) {
            using (Set_Alignment(alignment))
                w32prn.PrintText(line);

        }

        public override void Print_Barcode(string barcode_string) {
            throw new NotImplementedException();
        }

        public override void Print_Empty_Line(int count) {
            using (Command_Mode()) {
                w32prn.PrintText("f");
            }
        }

        public override void Print_Text(string text) {

            w32prn.PrintText(text);

        }



        internal override bool Initialize_Inner() {

            w32prn = new Win32PrintClass();

            if (w32prn == null)
                return false;

            w32prn.SetPrinterName(cst_printer_name);

            return true;


        }

        internal override bool Leave_Inner() {

            w32prn.EndDoc();
            w32prn = null;

            // 23 Nov 09
            // Hoping to get rid of the "String Binding is Invalid" bug
            GC.Collect(2);
            GC.WaitForPendingFinalizers();

            return true;
        }



        public override void Open_Cash_Drawer() {

            w32prn.OpenCashdrawer(cashdrawer_2pin);
            w32prn.OpenCashdrawer(cashdrawer_5pin);

        }

        public override void New_Page() {
            w32prn.NewPage();
        }

        public void Cut_Paper(bool linefeed) {
            Raw_Command(linefeed ? comm_cut_partial_lf : comm_cut_partial_nolf);
        }




        public override bool Supports_Font(Pos_Print_Font font) {
            return (font is Pos_Bixolon_Print_Font) &&
                   (font.Type == Print_Font_Type.Bixolon_Font);
        }

        public override void Set_Font(Pos_Print_Font font) {

            if (!Supports_Font(font))//
                return;//.tiff();

            var bix_font = (Pos_Bixolon_Print_Font)font;

            w32prn.SetDeviceFont(font.Height, font.Name, font.Bold, bix_font.Color);


            this.Font = font;

        }


        public override bool Is_Async { get { return false; } }



        public override void Raw_Command(string command) {

            using (Command_Mode()) {
                w32prn.PrintText(command);
            }

        }

        public override object Printer_Object { get { return w32prn; } }

        protected override byte Characters_Per_Line { get { return cst_printer_cols; } }



        IDisposable Set_Alignment(HAlignment alignment) {

            if (alignment == def_alignment)
                return On_Dispose.Nothing;

            var dict = new Dictionary<HAlignment, string>
            {
                {HAlignment.Left,comm_left_align},
                {HAlignment.Right, comm_right_align},
                {HAlignment.Center, comm_center},
            };

            using (Command_Mode()) {
                Raw_Command(dict[alignment]);
            }

            return new On_Dispose(
                () =>
                {
                    if (alignment != def_alignment)
                        using (Command_Mode()) {
                            Raw_Command(dict[def_alignment]);
                        }
                });

        }

        const HAlignment def_alignment = HAlignment.Left;

        IDisposable Command_Mode() {

            var font = Font;
            w32prn.SetDeviceFont(9.5f, cst_font_control, false, false);
            return new On_Dispose(() => Set_Font(font));

        }

        IDisposable Code128() {

            var font = Font;
            w32prn.SetDeviceFont(9.5f, cst_barcode_code128, false, false);
            return new On_Dispose(() => Set_Font(font));

        }





    }
}
