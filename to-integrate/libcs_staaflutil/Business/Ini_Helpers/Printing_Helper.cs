using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Common;

using Fairweather.Service;
namespace DTA
{
    public class Printing_Helper : Ini_Helper
    {
        public
        Printing_Helper(Ini_File ini, Company_Number number, Printing_Scenario scenario)
            : base(ini, number) {

            this.company = number;
            this.Scenario = scenario;


        }


        public Printing_Scenario Scenario {
            get;
            set;
        }

        readonly Company_Number company;

        public Company_Number Company {
            get { return company; }
        }


        public string Get_Filename() {

            var dir = Data.Get_Printing_Directory(company, Scenario).Value;
            var prefix = Data.Get_Printing_Prefix();
            var ext = Data.Printing_Extension;

            var file = Data.Get_Next_Filename(dir, prefix, ext);

            return file;

        }

        public object OPOS_Printer_Id {
            get {
                return "" + OPOS_Paper_Cutting_Percentage 
                          + OPOS_High_Quality_Letters 
                          + OPOS_Keep_Printer_Open 
                          + OPOS_Image_DPI 
                          + OPOS_Logical_Name;
            }
        }

        public int OPOS_Paper_Cutting_Percentage {
            get { return Int(DTA_Fields.POS_printing_opos_cut_paper_perc); }
        }

        public bool OPOS_High_Quality_Letters {
            get { return True(DTA_Fields.POS_printing_opos_hiq_letters); }
        }

        public bool OPOS_Keep_Printer_Open {
            get { return True(DTA_Fields.POS_printing_opos_keep_open); }
        }

        public int OPOS_Image_DPI {
            get {
                return Int(DTA_Fields.POS_printing_opos_image_dpi);
            }
        }

        public string OPOS_Logical_Name {
            get { return String(DTA_Fields.POS_printing_opos_logical_name); }
        }


        public bool Notepad_Print_Tags {
            get {
                return True(DTA_Fields.POS_printing_txt_print_tags);
            }
        }

        public bool Notepad_Open_File {
            get {
                return True(DTA_Fields.POS_printing_txt_open_file);
            }
        }

        public bool Notepad_Open_Dir {
            get {
                return True(DTA_Fields.POS_printing_txt_open_dir);
            }
        }

        public int GDI_DPI {
            get {
                return Int(DTA_Fields.POS_printing_gdi_dpi);
            }
        }
        public bool GDI_Show_Preview {
            get {
                return True(DTA_Fields.POS_printing_gdi_preview);
            }
        }
        public bool GDI_Show_Print_Dialog {
            get {
                return True(DTA_Fields.POS_printing_gdi_print_dialog);
            }
        }
        public bool GDI_Show_Page_Setup {
            get {
                return True(DTA_Fields.POS_printing_gdi_page_setup);
            }
        }

        public string GDI_Printer_To_Use {
            get {

                var preferred = GDI_Preferred_Printer;

                bool _;
                var ret = H.Printer_Or_Default(preferred, out _);

                return ret;

            }

        }

        public bool GDI_Preferred_Printer_Available {
            get {

                var preferred = GDI_Preferred_Printer;

                // No preferred printer
                if (preferred == null)
                    return true;

                bool available;
                H.Printer_Or_Default(preferred, out available);

                return available;

            }

        }

        public string GDI_Preferred_Printer {
            get {
                var str = String(DTA_Fields.POS_printing_gdi_printer);

                if (str.Safe_Equals(Ini_Main.DEFAULT, true))
                    return null;

                return str;
            }
        }

        public Pos_GDI_Font GDI_Font {
            get {

                var size = Int(DTA_Fields.POS_printing_gdi_font_size);
                var ret = Get_GDI_Font(size, false);
                return ret;
            }
        }

        public Pos_GDI_Font Get_GDI_FontR(int? size, bool bold) {
            size = size ?? 0;
            size += Int(DTA_Fields.POS_printing_gdi_font_size);
            return Get_GDI_Font(size, bold);
        }

        public Pos_GDI_Font Get_GDI_Font(int? size, bool bold) {

            size = size ?? Int(DTA_Fields.POS_printing_gdi_font_size);
            var face = Font(DTA_Fields.POS_printing_gdi_font_face);
            var func = ((Func<Font>)(() => new Font(face, size.Value, bold ? FontStyle.Bold : FontStyle.Regular)));

            var ret = new Pos_GDI_Font(func);

            return ret;
        }


        public int GDI_Page_Width_MM {
            get {
                return Int(DTA_Fields.POS_printing_gdi_page_width);

            }
        }


        public Printing_Provider Provider {
            get {
                var dta = String(DTA_Fields.POS_printing_provider);

                var dict = new Dictionary<string, Printing_Provider>{

                    {Ini_Main.TEXT, Printing_Provider.Notepad},
                    {Ini_Main.WINDOWS, Printing_Provider.Windows},
                    {Ini_Main.BIXOLON, Printing_Provider.Bixolon},
                    {Ini_Main.OPOS, Printing_Provider.Pos_For_Net},

                };

                var ret = dict[dta];

                return ret;

            }
        }

        public int Copies {
            get {
                if (this.Scenario == Printing_Scenario.Preview)
                    return 1;

                var str = new Dictionary<Printing_Scenario, Ini_Field>{
                    {Printing_Scenario.Sales_Receipt, DTA_Fields.POS_printout_count_sale},
                    {Printing_Scenario.Future_Cheque, DTA_Fields.POS_printout_count_receipt},
                    {Printing_Scenario.Receipt_On_Account, DTA_Fields.POS_printout_count_receipt},
                    {Printing_Scenario.End_Of_Shift, DTA_Fields.POS_printout_count_eos},
                }[this.Scenario];


                return Int(str);
            }
        }

        Receipt_Layout_SV Get_Default_Layout() {

            Sage_Logic sdr;

            Sage_Logic.Get(ini, Company, out sdr).tiff();

            var name = sdr.Get_Company_Data().Name;

            var ret = new Receipt_Layout_SV(name, "", "", "logo.bmp", true, false);

            return ret;

        }

        public bool Print_Image {
            get {
                return Layout.Use_Image && H.Validate_Image(Layout.Image);
            }
        }


        public Receipt_Layout_SV Layout {
            set {
                S.Serialize_To_File(Receipt_Layout_File, false, value).tiff();
            }
            get {
                Receipt_Layout_SV ret;
                if (!S.Deserialize_From_File(Receipt_Layout_File, false, out ret)) {
                    Layout = Get_Default_Layout();
                    ret = Layout;
                }


                return ret;
            }
        }

        // Manual serialization
        ///
        //public Printing_Header Header {
        //    set {
        //        using (var sw = new StreamWriter(Current_File, false)) {
        //            sw.WriteLine(value.Image);
        //            sw.WriteLine(value.Use_Image ? "yes" : "no");
        //            sw.WriteLine(value.Name);
        //            sw.WriteLine(value.Text);

        //        }
        //    }
        //    get {
        //        if (!File.Exists(Current_File))
        //            return Get_Default_Header();

        //        using (var sw = new StreamReader(Current_File)) {
        //            var img = sw.ReadLine();
        //            var use_image = sw.ReadLine() != "no";
        //            var title = sw.ReadLine();

        //            var text = sw.ReadToEnd();

        //            return new Printing_Header(title, text, img, use_image);

        //        }


        //    }
        //}

        string Receipt_Layout_File {
            get {
                return Data.Get_Header_Filename(Company);
            }
        }

        string Receipt_Layout_Backup {
            get {
                return Receipt_Layout_File + ".bak";
            }
        }

        public void Backup_Layout() {
            if (File.Exists(Receipt_Layout_File))
                File.Copy(Receipt_Layout_File, Receipt_Layout_Backup, true);
        }

        public void Restore_Layout() {
            File.Delete(Receipt_Layout_File);

            if (File.Exists(Receipt_Layout_Backup))
                File.Move(Receipt_Layout_Backup, Receipt_Layout_File);
        }

        public void Remove_Layout_Backup() {

            File.Delete(Receipt_Layout_Backup);

        }

    }
}