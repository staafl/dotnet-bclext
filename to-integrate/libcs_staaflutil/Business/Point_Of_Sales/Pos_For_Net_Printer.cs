using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

using Fairweather.Service;
using Microsoft.PointOfService;
using System.Text;
using System.Windows.Forms;

namespace Fairweather.Service
{
    public class Pos_Net_Printer : Pos_Printer_Engine
    {
        public static bool
        Get_Instance(ISynchronizeInvoke obj,
                     string logical_name,
                     bool high_quality_letters,
                     int image_dpi,
                     out Pos_Net_Printer printer) {

            logical_name.IsNullOrEmpty().tift();
            
            var pos_explorer = new PosExplorer(obj);

            PosPrinter pos_printer = null;
            DeviceInfo device_info = null;

            try {
                device_info = pos_explorer.GetDevice(DeviceType.PosPrinter, logical_name);
                if (device_info == null) {
                    var devices = pos_explorer.GetDevices(DeviceType.PosPrinter);

                    foreach (DeviceInfo info in devices) {
                        //if (info.Names.Any(str => str.Safe_Equals(logical_name, true))) {
                        if (info.ServiceObjectName.Safe_Equals(logical_name, true)) {
                            device_info = info;
                            break;

                        }
                    }

                }

                device_info.tifn<Pos_Printing_Exception>("device_info.tifn");

            }
            catch (PosControlException pex) {
                On_Printing_X_Static("Device data not valid.", pex);

            }

            try {
                pos_printer = (PosPrinter)pos_explorer.CreateInstance(device_info);
                pos_printer.tifn<Pos_Printing_Exception>("m_printer.tifn");

            }
            catch (PosControlException pex) {
                On_Printing_X_Static("Unable to access printer.", pex);

            }


            printer = new Pos_Net_Printer(logical_name, pos_printer, device_info);
            printer.Hiq_Letters = high_quality_letters;
            printer.Image_DPI = image_dpi;

            return true;

        }

        Pos_Net_Printer(string logical_name, PosPrinter printer, DeviceInfo device_info) {

            this.m_printer = printer;
            this.m_logical_name = logical_name;
            this.m_device_info = device_info;

        }



        /*       Settings        */

        public void Set_Characters_Per_Line() {

            if (Is_Simulator)
                return;

            var available = m_printer.RecLineCharsList;
            available.Sort();

            var valid = from width in available
                        where width >= cst_def_chars_per_line
                        select width;

            m_printer.RecLineChars = valid.First();

        }

        public int? Paper_Cutting {
            get;
            set;
        }

        public bool Keep_Open {
            get;
            set;
        }

        bool Do_Keep_Open {
            get {
                return Keep_Open
                    // && !Is_Simulator
                ;
            }
        }

        /*       Status        */

        public DeviceInfo DeviceInfo {
            get {
                return m_device_info;
            }
        }

        public bool Is_Simulator {
            get {
                var type = m_printer.GetType().ToString();
                return type == "Microsoft.PointOfService.DeviceSimulators.PosPrinterSimulator";
            }
        }

        public bool Hiq_Letters {
            get;
            private set;
        }

        public int Image_DPI {
            get;
            private set;
        }

        public override bool Initialized {
            get { return b_init; }
        }

        public override bool Is_Async {
            get { return m_printer.AsyncMode; }
        }

        /*       Methods        */


        protected override void Print_Image_Inner(string file, HAlignment align) {

            Ensure_Init();


            var alignment_as_int = Bmp_Alignment_To_Int(align);

            if (!m_printer.CapRecBitmap)
                return;

            Maybe_Print();

            file = System.IO.Path.GetFullPath(file);
            file = file.ToUpper();
            var pair = Pair.Make(file, align);


            try {
                Set_And_Print(file, alignment_as_int, pair);

            }
            catch (XPrinting ex) {
                Notify(ex);
                if (Muddle_On_Through)
                    return;
                throw;
            }

        }

        void Set_And_Print(string file, int alignment_as_int, Pair<string, HAlignment> pair) {
            var dpi = set_dpi ?? 0;

            int no;
            while (!pic_to_no.TryGetValue(pair, out no)) {

                var img = H.Try_Get_Image(file);
                if (img == null) {
                    var ex = new XPrinting("Image file {0} is invalid or corrupted.".spf(file));
                    Notify(ex);
                    if (Muddle_On_Through)
                        return;
                    throw ex;
                }

                int units = PosPrinter.PrinterBitmapAsIs;

                if (dpi != 0) {
                    var ww_pix = (double)img.Width;
                    var inches = ww_pix / dpi;
                    units = (int)(inches * 1000);

                }

                for (int attempt = 0; ; ++attempt) {
                    try {
                        while (pic_to_no.Count >= cst_max_pics) {
                            var min_used = used.Min();
                            pic_to_no.Remove(min_used);
                        }

                        no = 1;
                        for (; no < /* sic */ cst_max_pics; ++no) {
                            if (!pic_to_no.Contains(no))
                                break;
                        }


                        m_printer.SetBitmap(no,
                                            cst_station,
                                            file,
                                            units,
                                            alignment_as_int);

                        pic_to_no[no] = pair;

                        break;
                    }
                    catch (PosControlException pex) {

                        if (attempt >= cst_set_bitmap_attempts)
                            OnPrintingException(pex);

                        // See Epson ADK sample #3
                        if (pex.ErrorCode != ErrorCode.Failure || pex.ErrorCodeExtended != 0)
                            OnPrintingException(pex);

                        Thread.Sleep(cst_bitmap_attempt_delay);

                    }

                }

            }

            Print_Bitmap_No(no);

        }

        public override void Print_Text(string text) {

            Ensure_Init();

            var to_print = Linefeed(text, 1);

            sb.Append(to_print);

            // m_printer.PrintNormal(cst_station, to_print);

        }

        public override void Print_Barcode(string barcode_string) {
            if (!Muddle_On_Through)
                throw new NotImplementedException();
        }

        public string Linefeed(string text, int lines) {

            var ret = text + ESC(lines + "lF");
            return ret;

        }


        public override bool Supports_Font(Pos_Print_Font font) {

            font.tifn();

            var as_for_net_font = (font as Pos_For_Net_Font);

            if (font == null)
                return false;

            var name = font.Name;
            var supported = m_printer.FontTypefaceList;

            bool ret = supported.Contains(name);

            return ret;

        }

        /// <summary> No-Op 
        /// </summary>
        public override void Set_Font(Pos_Print_Font font) {
        }


        /*       Misc        */


        public void Cut_Paper(int percentage) {

            Ensure_Init();

            if (percentage > 0)
                m_printer.CutPaper(percentage);

        }

        public override void New_Page() {
            if (!Muddle_On_Through)
                throw new NotImplementedException();
        }

        public override void Open_Cash_Drawer() {
            if (!Muddle_On_Through)
                throw new NotImplementedException();
        }

        public override object Printer_Object {
            get { return m_printer; }
        }



        // ****************************

        void Maybe_Print() {
            if (sb.Length <= 0)
                return;

            m_printer.PrintNormal(cst_station, sb.ToString());
            sb.Length = 0;

        }

        protected override byte Characters_Per_Line {
            get {
                if (Is_Simulator)
                    return cst_def_chars_per_line;

                return (byte)m_printer.RecLineChars;
            }
        }

        bool Try(Action act) {
            try {
                act();
                return true;
            }
            catch (PosControlException pex) {
                Notify(pex);

                if (!Muddle_On_Through)
                    OnPrintingException(pex);

                return false;
            }
        }


        internal override bool Initialize_Inner() {

            if (Do_Keep_Open) {
                if (b_init)
                    return true;
            }

            const string unable = "Unable to communicate with printer.";

            try {
                m_printer.Open();

            }
            catch (PosControlException pex) {

                Safe_Close();

                OnPrintingException(unable, pex);

                //var errorcode = pex.ErrorCode;

                //if (errorcode != ErrorCode.Illegal)
                //      OnPrintingException(pex);

                //return false;
            }


            try {
                m_printer.Claim(cst_claim_delay);

            }
            catch (PosControlException pex) {

                Safe_Close();

                OnPrintingException(unable, pex);

                //var errorcode = pex.ErrorCode;
                //if (errorcode == ErrorCode.Timeout)
                //      return false;

                //// "The device cannot currently be claimed for exclusive access"
                //if (errorcode == ErrorCode.Illegal)
                //      return false;

                //OnPrintingException(pex);

            }

            try {
                m_printer.DeviceEnabled = true;
            }
            catch (PosControlException pex) {

                Safe_Close();

                OnPrintingException(unable, pex);


            }

            if (Hiq_Letters)
                Try(() => m_printer.RecLetterQuality = true);

            if (set_dpi == null) {
                int tmp = Image_DPI;
                if (tmp != 0)
                    if (!Try(() => m_printer.MapMode = MapMode.English))
                        tmp = 0;
                set_dpi = tmp;
            }

            Set_Characters_Per_Line();

            b_init = true;


            return true;

        }

        internal override bool Leave_Inner() {

            Maybe_Print();

            var cut_snap = Paper_Cutting;
            if (cut_snap.HasValue)
                Cut_Paper(cut_snap.Value);

            if (Do_Keep_Open)
                return true;

            /*if (Is_Simulator) {
                foreach (Form form in Application.OpenForms) {
                    if (form.GetType().Name.Contains("Simulator")) {
                        form.Invoke(
                            ((MethodInvoker)(() =>
                            {
                                Console.WriteLine(form.Handle);
                                form.Closing += (_1, _2) =>
                                {
                                    Console.WriteLine("aaa");
                                };
                            })));
                    }
                }
            }*/
            set_dpi = null;

            bool ret = Safe_Close();

            b_init = false;

            return ret;

        }

        string Get_Error_String(bool paren) {

            var printer = m_printer;

            var ret = (string)null;

            if (printer != null)
                ret = printer.ErrorString;

            if (ret.Emptyish())
                ret = null;

            if (ret != null) {
                if (ret.ToUpper() == "[ERROR]")
                    ret = null;
                else if (paren)
                    ret = " (" + ret + ")";

            }

            return ret;


        }

        void OnPrintingException(Exception ex) {

            base.On_Printing_X(Get_Error_String(false), ex);

        }

        void OnPrintingException(string msg, Exception ex) {

            msg += Get_Error_String(true);

            base.On_Printing_X(msg, ex);

        }

        bool Safe_Close() {

            if (Is_Simulator) {
                m_printer.Release();
                return true;
            }

            var printer = m_printer;

            if (printer == null)
                return false;

            try {
                /* "If DeviceEnabled, POS for .NET disables the device. 
                    If Claimed, POS for .NET releases the device."*/
                printer.Close();
            }
            catch (PosControlException pex) {
                if (pex.ErrorCode == ErrorCode.Closed)
                    // The device is already closed.
                    return true;

                try {
                    printer.DeviceEnabled = false;

                    if (printer.Claimed)
                        printer.Release();

                    printer.Close();

                    OnPrintingException(pex);

                }
                catch (PosControlException pex1) {
                    OnPrintingException(pex);
                    if (pex1.ErrorCode == ErrorCode.Closed)
                        // The device is already closed.
                        return true;
                    OnPrintingException(pex1);

                }

            }

            return true;
        }

        int Bmp_Alignment_To_Int(HAlignment alignment) {

            int ret = align_to_int[alignment];
            return ret;
        }

        bool Print_Bitmap_No(int no) {
            try {
                m_printer.PrintNormal(cst_station, ESC(no + "B"));

            }
            catch (PosControlException ex) {
                Notify(ex);
                if (Muddle_On_Through)
                    return false;
                OnPrintingException(ex);
            }

            try {
                used.Inc(pic_to_no[no]);
            }
            catch (KeyNotFoundException ex) {
                Notify(ex);
            }

            return true;

        }

        static string ESC(string sequence) {

            return esc + "|" + sequence;

        }


        int? set_dpi;
        bool b_init;
        readonly string m_logical_name;
        readonly DeviceInfo m_device_info;
        readonly PosPrinter m_printer;
        readonly StringBuilder sb = new StringBuilder();

        readonly Twoway<Pair<string, HAlignment>, int>
        pic_to_no = new Twoway<Pair<string, HAlignment>, int>();

        readonly Counter<Pair<string, HAlignment>>
        used = new Counter<Pair<string, HAlignment>>();

        readonly Dictionary<HAlignment, int>
        align_to_int = new Dictionary<HAlignment, int>
        {
            {HAlignment.Left, PosPrinter.PrinterBitmapLeft},
            {HAlignment.Right, PosPrinter.PrinterBitmapRight},
            {HAlignment.Center, PosPrinter.PrinterBitmapCenter},
        };

        #region constants
        /*       Complete list of escape codes is here        */
        // http://msdn.microsoft.com/en-us/library/microsoft.pointofservice.posprinter(WinEmbedded.11).aspx

        static readonly PrinterStation cst_station = PrinterStation.Receipt;

        const int cst_max_pics = 8;

        const char nl = '\n';
        const char cr = (char)13;
        const char esc = (char)27;


        static readonly int cst_set_bitmap_attempts = 3;

        // milliseconds
        static readonly int cst_claim_delay = 1000;

        // milliseconds
        static readonly int cst_bitmap_attempt_delay = 100;

        static readonly string cst_str_superscript = ESC("tpC");
        static readonly string cst_str_subscript = ESC("tbC");

        static readonly string cst_str_normal = ESC("N");
        static readonly string cst_str_bold = ESC("bC");
        static readonly string cst_str_italic = ESC("iC");
        static readonly string cst_str_underline = ESC("uC");
        static readonly string cst_str_std = ESC("1C");
        static readonly string cst_str_wide = ESC("2C");
        static readonly string cst_str_tall = ESC("3C");
        static readonly string cst_str_quad = ESC("4C");
        static readonly string cst_str_linefeed_cut = ESC("fP");

        static readonly string cst_str_left_align = ESC("lA");
        static readonly string cst_str_right_align = ESC("rA");
        static readonly string cst_str_center_align = ESC("cA");

        // Epson ADK sample #7
        static readonly string cst_str_2mm_spaces = ESC("200uF");
        static readonly string cst_str_5mm_spaces = ESC("500uF");

        #endregion





    }



}