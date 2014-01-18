
using System;
using System.Windows.Forms;
using DTA;

using Fairweather.Service;
using Standardization;


namespace Common
{
    static public partial class Pos_Printing_Utility
    {

        public static Form Owning_Form {
            get;
            set;
        }



        static readonly Pos_Columns_Info
        Left_Right = new Pos_Columns_Info(0, true, new int[] { 2, 0 }, new int[] { 20, 1000 });

        static readonly Pos_Columns_Info
        Left_Right_Dot = new Pos_Columns_Info(0, true, '.', new int[] { 2, 0 }, new int[] { 20, 1000 });


        public static Pos_Print_Font[]
        Footer_Text_Font(Printing_Helper helper) {
            return new Pos_Print_Font[] { A2x2_7f(false, false), helper.Get_GDI_FontR(2, false) };

        }

        public static Pos_Print_Font[]
        Header_Text_Font(Printing_Helper helper) {

            return new Pos_Print_Font[] { A1x1_7f(false, false), helper.Get_GDI_Font(null, false) };

        }

        public static Pos_Print_Font[]
        Header_Name_Font(Printing_Helper helper) {

            return new Pos_Print_Font[] { B2x1_7f(false, true), helper.Get_GDI_FontR(4, false) };

        }

        public static Pos_Print_Font[]
        Header_Title_Font(Printing_Helper helper) {

            return new Pos_Print_Font[] { A2x2_7f(false, false), helper.Get_GDI_FontR(4, true) };

        }

        public static Pos_Print_Font[]
        Receipt_Number_Font(Printing_Helper helper) {

            return new Pos_Print_Font[] {A1x1_9_5f(false, false), helper.Get_GDI_Font(10, true)};
        }


        public static void
        Print_Header(this Pos_Printer_Engine printer, Printing_Helper helper, string title) {


            var layout = helper.Layout;


            if (helper.Print_Image) {
                printer.Print_Image(layout.Image, HAlignment.Center);
            }

            // 04-12-2009
            //printer.Print_Empty_Line();


            if (!layout.Name.IsNullOrEmpty()) {

                printer.Set_Any_Of(Header_Name_Font(helper));

                printer.Print_Single_Line(layout.Name, HAlignment.Center);

            }

            printer.Print_Empty_Line();

            if (!layout.Header.IsNullOrEmpty()) {

                printer.Set_Any_Of(Header_Text_Font(helper));

                printer.Print_Empty_Line();

                printer.Print_Several_Lines(layout.Header, HAlignment.Left);

                printer.Print_Empty_Line();


            }

            printer.Print_Empty_Line();


            printer.Set_Any_Of(Header_Title_Font(helper));

            printer.Print_Single_Line(title, HAlignment.Center);

        }


        static void
        Print_Trailing_White(this Pos_Printer_Engine printer) {

            printer.Print_Empty_Line();
            printer.Print_Empty_Line();
            printer.Print_Empty_Line();

            if (printer is Pos_Bixolon_350_Printer) {

                // 20091205
                for (int ii = 0; ii < 5; ++ii)
                    printer.Raw_Command("f");

            }

        }


        static void
        Print_Item(this Pos_Printer_Engine printer, string name, decimal price) {

            var price_string = price.ToString(true);

            printer.Print_Item(name, price_string);
        }

        static void
        Print_Item(this Pos_Printer_Engine printer, string left, string right) {

            printer.Print_Columns(Left_Right, left, right);

        }

        static public void
        Print_Date(this Pos_Printer_Engine printer) {

            Print_Date(printer, DateTime.Now, true);

        }

        static public void
        Print_Date(this Pos_Printer_Engine printer,
                    DateTime date,
                    bool print_hrs) {

            string date_string = date.ToString(true);

            if (print_hrs) {

                var time_string = date.ToString("HH:mm");

                printer.Print_Columns(Left_Right, date_string, time_string);

            }
            else {
                printer.Print_Text(date_string);
            }
        }

        static public void
        Print_Days_Data(this Pos_Printer_Engine printer, Day_Totals data, string title) {

            printer.Print_Text(title);

            printer.Print_Empty_Line();

            printer.Print_Item("Cash:", data.Cash);
            printer.Print_Item("Credit Cards:", data.Credit);
            printer.Print_Item("Gift Vouchers:", data.Gift);
            printer.Print_Item("Cheques:", data.Cheques);
            printer.Print_Item("Post-dated Cheques:", data.Post_Dated);

            printer.Print_Empty_Line();

            printer.Print_Item("Total:", data.Total);

        }

        /*       FONTS        */

        public static Pos_Bixolon_Print_Font
        B2x1_7f(bool bold, bool color) {
            return new Pos_Bixolon_Print_Font(Bixolon_Font_Type.FontB2x1, 7f, bold, color);
        }

        public static Pos_Bixolon_Print_Font
        A1x1_9_5f(bool bold, bool color) {
            return new Pos_Bixolon_Print_Font(Bixolon_Font_Type.FontA1x1, 9.5f, bold, color);
        }

        public static Pos_Bixolon_Print_Font
        A1x1_7f(bool bold, bool color) {
            return new Pos_Bixolon_Print_Font(Bixolon_Font_Type.FontA1x1, 7f, bold, color);
        }

        public static Pos_Bixolon_Print_Font
        A2x2_7f(bool bold, bool color) {
            return new Pos_Bixolon_Print_Font(Bixolon_Font_Type.FontA2x2, 7f, bold, color);
        }

        public static Pos_Bixolon_Print_Font
        A2x2_9_5f(bool bold, bool color) {
            return new Pos_Bixolon_Print_Font(Bixolon_Font_Type.FontA2x2, 5f, bold, color);
        }

    }
}
