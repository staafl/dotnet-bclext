using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common.Controls;
using DTA;
using Fairweather.Service;
using Standardization;
using Versioning;
using Microsoft.Win32;
using Sage_Int;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

using Excel = Microsoft.Office.Interop.Excel;

namespace Common
{
    /// <summary> Miscellaneous functions specific to the application
    /// </summary>
    static public class M
    {
        static public IDisposable
      Get_Excel_App(out Excel.Application xl) {

            xl = null;
            Excel.Application xl_tmp;

            //var procs = Process.GetProcessesByName("excel");

            //if (procs.Length == 0)
            try {
                xl_tmp = new Excel.Application();
            }
            catch (COMException) {

                // 0x80040154
                return On_Dispose.Nothing;

            }

            //else
            //     xl_tmp = (Excel.Application)Marshal.GetActiveObject("Excel.Application");

            var ret = On_Dispose.Compose(On_Dispose.Com_Object(xl_tmp),
                                         () =>
                                         {
                                             if (xl_tmp == null)
                                                 return;
                                             try {
                                                 xl_tmp.Quit();
                                             }
                                             catch (COMException) {
                                                 /* 0x800706BA - the RPC server is unavailable */
                                             }
                                         },
                                         () =>
                                         {
                                             GC.WaitForPendingFinalizers();
                                             GC.Collect();
                                         });

            xl = xl_tmp;
            return ret;

        }


        public static List_Cursor
        Set_Tax_Code_Cursor() {

            if (Records_Access.Cursor_Assigned(Record_Type.Tax_Code))
                return null;

            // "T1 - 15%"
            var ret = new List_Cursor(from _tc in Tax_Code.Instances
                                      select new Pair<string>(_tc.Index_String, _tc.VAT_Percentage + "%"));

            Records_Access.Set_Cursor(Record_Type.Tax_Code, ret);

            return ret;

        }

        public static void
        Prepare_Tax_Code_Combobox(Advanced_Combo_Box acb) {

            acb.Setup(Quick_Search_Form_Mode.Tax_Codes);


            Action<string> try_adjust = (txt) =>
            {
                uint _possible;

                if (uint.TryParse(txt, out _possible) &&
                    _possible < 100)
                    acb.Value = "T" + _possible;
            };

            acb.QSF_Shown += (_1, _2) => try_adjust(acb.Text);

            acb.Reject_Changes += (_1, e) => try_adjust(e.Proposed);


        }

        /*       Various helpers        */

        /// <summary>
        /// abcdefgh --> ab-cd-ef-gh
        /// </summary>
        static public string
        Hyphenate(string str) {
            int length = str.Length;

            for (int ii = length - 2; ii >= 2; ii -= 2)
                str = str.Insert(ii, "-");

            return str;
        }

        static public bool
        Get_Key_String(Keys keyData, bool caps_lock, out string keyString) {

            bool ret = true;
            keyString = "";

            Keys keyCode = keyData & Keys.KeyCode;
            string str = keyCode.ToString().ToLower();

            bool is_letter_or_digit = false;
            bool is_in_dictionary = false;

            if (str.Length == 1) {

                char ch = char.Parse(str);

                if (Char.IsLetterOrDigit(ch)) {

                    is_letter_or_digit = true;
                    keyString = str;
                }
            }

            if (!is_letter_or_digit)
                is_in_dictionary = st_key_to_str.TryGetValue(keyCode, out keyString);

            if (is_in_dictionary || is_letter_or_digit) {

                string modifiers = "";

                if ((keyData & Keys.Alt) == Keys.Alt)
                    modifiers += "%";

                if ((keyData & Keys.Control) == Keys.Control)
                    modifiers += "^";

                if ((keyData & Keys.Shift) == Keys.Shift) {

                    if (is_letter_or_digit)
                        if (!caps_lock)
                            keyString = keyString.ToUpper();

                    modifiers += "+";
                }
                else if (is_letter_or_digit) {
                    if (caps_lock)
                        keyString = keyString.ToUpper();

                }

                keyString = modifiers + keyString;
            }
            else {
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// Formats a decimal number so that all negative values are represented by 0.00m.
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        static public string
        String_Or_Zero(this decimal dec) {

            return Math.Max(dec, 0.00m).ToString(true);

        }


        /// <summary>
        /// Composes a plain-text header useful when writing out a report in txt.
        /// </summary>
        public static string
        Get_Report_Header(string title,
                          string subtitle,
                          string filename,
                          bool copyright,
                          bool add_datestamp) {

            /*"{0}{1}\n{2}{3}{4}{5}{6}".spf(  appname,
                                              maybe_title,
                                              maybe_copyright,
                                              maybe_subtitle,
                                              maybe_datestamp,
                                              maybe_filename);*/

            //var header = nl + "Sage Entry Screens - Point Of Sales System" + nl +
            //        "(c) InfoTrends Ltd." + nl + nl +
            //        "Duplicate Product Barcodes report ({0})" + nl + nl +
            //        "{1}" + nl;

            string nl = Environment.NewLine;

            var ret = nl + Data.App_Name;

            if (!title.IsNullOrEmpty())
                ret += " - " + title;

            ret += nl;

            if (copyright)
                ret += "(c) InfoTrends Ltd" + nl;

            ret += nl;

            var datestamp = DateTime.Today.ToString(true) + " " + DateTime.Now.ToString("HH:mm:ss");

            if (subtitle.IsNullOrEmpty()) {
                if (add_datestamp)
                    ret += datestamp + nl;
            }
            else {
                ret += subtitle;
                if (add_datestamp)
                    ret += " ({0})".spf(datestamp) + nl;
            }

            if (!filename.IsNullOrEmpty())
                ret += nl + Path.GetFullPath(filename) + nl;

            return ret;
        }

        static public void
        DGV_Workaround(Control ctrl) {

            //See http://social.msdn.microsoft.com/forums/en-US/Vsexpressvcs/thread/0d8eae05-90d2-4686-9103-1d2d372f7860/
            Native_Methods.SendMessage(ctrl.Handle, Native_Const.WM_KEYDOWN, 0x20, 0x390001);

            if (ctrl.Parent != null)
                Native_Methods.SendMessage(ctrl.Parent.Handle,
                                           Native_Const.WM_KEYDOWN,
                                           0x20,
                                           0x390001);

        }

        static public string
        Cheque_Amount_To_Words(this decimal amount, string currency, string small) {

            var ret = amount.ToWords(2, currency, small);
            if (!ret.IsNullOrEmpty())
                ret += " Only";

            return ret;

        }

        static public int
        Perform_Backspace(this ITextBox tb) {

            int start_len = tb.Text.Length;
            int ret;

            int sel_len = tb.SelectionLength;
            int pos = tb.SelectionStart;

            if (sel_len == 0) {

                if (pos == 0) {
                    return 0;
                }

                tb.Text = tb.Text.Remove(pos - 1, 1);
                tb.SelectionStart = pos;
            }
            else {
                tb.Text = tb.Text.Remove(pos, sel_len);
                tb.SelectionStart = pos;
            }

            ret = tb.Text.Length - start_len;

            return ret;
        }

        static public void
        Beep() {

            Console.Beep(1000, 18);

            //else if (beep == 1)
            //    Console.Beep(900, 14);

        }

        static Dictionary<Keys, String> st_key_to_str = new Dictionary<Keys, String>()
        {
#region 15 lines
		    {Keys.Space, " "},
            {Keys.Back, "{BKSP}"}, {Keys.Pause, "{BREAK}"}, {Keys.CapsLock, "{CAPSLOCK}"},
            {Keys.Delete, "{DEL}"}, {Keys.Down , "{DOWN}"}, {Keys.End, "{END}"},
            {Keys.Enter, "{ENTER}"}, {Keys.Escape, "{ESC}"}, {Keys.Help, "{HELP}"},
            {Keys.Home, "{HOME}"}, {Keys.Insert, "{INSERT}"}, {Keys.Left, "{LEFT}"},
            {Keys.NumLock, "{NUMLOCK}"}, {Keys.PageDown, "{PGDN}"}, {Keys.PageUp, "{PGUP}"},
            {Keys.PrintScreen, "{PRTSC}"}, {Keys.Right, "{RIGHT}"}, {Keys.Scroll, "{SCROLLLOCK}"},
            {Keys.Tab, "{TAB}"}, {Keys.Up, "{UP}"}, {Keys.F1, "{F1}"},
            {Keys.F2, "{F2}"}, {Keys.F3, "{F3}"}, {Keys.F4, "{F4}"},
            {Keys.F5, "{F5}"}, {Keys.F6, "{F6}"}, {Keys.F7, "{F7}"},
            {Keys.F8, "{F8}"}, {Keys.F9, "{F9}"}, {Keys.F10, "{F10}"},
            {Keys.F11, "{F11}"}, {Keys.F12, "{F12}"}, {Keys.F13, "{F13}"},
            {Keys.F14, "{F14}"}, {Keys.F15, "{F15}"}, {Keys.F16, "{F16}"},
            {Keys.OemOpenBrackets, "{[}"}, {Keys.OemCloseBrackets, "{]}"},
            {Keys.OemBackslash, "\\"}, {Keys.OemSemicolon, ";"}, {Keys.OemQuotes, "'"},
            {Keys.Oemcomma , ","}, {Keys.OemPeriod , "."}, {Keys.HanjaMode, "/"}, 
	#endregion
        };


        /*       SIT ini helpers        */


        static public void
        Read_SIT_Settings(string path, out DD sett_all, out Settings_Old sett_global) {
            sett_all = Read_SIT_Settings(path);

            sett_global = new Settings_Old(sett_all["0000"]);
        }

        static DD Read_SIT_Settings(string path) {

            var text = File.ReadAllText(path);

            var clear = Encryption.Decrypt(text);
            clear = clear.Replace("\r", "");

            var ret = new DD();

            // ^(\d+) ([A-Za-z_0-9]*?)=""?([-0-9A-Za-z_:\\\. ]*?)""?$

            var m = clear.Match(@"^(\d+) ([A-Za-z_0-9]*?)=([^\r\n]+)$", RegexOptions.Multiline);

            for (; m.Success; m = m.NextMatch()) {

                var groups = m.Groups;

                var group1 = groups[1].Value.ToUpper();
                var group2 = groups[2].Value.ToUpper();

                ret[group1][group2] = groups[3].Value;

            }

            return ret;

        }

        static void Save_Settings(DD sett_all) {

            throw new NotImplementedException();
            //var sb = new StringBuilder();

            //foreach (var kvp1 in sett_all) {

            //    foreach (var kvp2 in kvp1.Value) {

            //        string to_print = "{0} {1}={2}\r\n".spf(kvp1.Key, kvp2.Key, kvp2.Value);
            //        sb.Append(to_print);

            //    }

            //    sb.Append("\r\n");

            //}

            //string output = Encrypter.Encrypt(sb.ToString());

            // var dta_file = Get_Dta_File();

            // using (var sw = new StreamWriter(dta_file)) {


            //    sw.Write(output);

            //}

        }

        // ****************************


        public static bool
        Check_Config_Dta(bool delete) {

            const string config_dta = "config.dta";// "config.dta";



            var exists = File.Exists(config_dta);



            var ret = false;

            //MessageBox.Show(exists.strdef(), "exists");


            if (exists) {

                var text = File.ReadAllText(config_dta).Trim();
                var pass = M.Get_Service_Pass();

                ret = (text == pass);

                //MessageBox.Show(text, "text");
                //MessageBox.Show(pass, "pass");

            }

            if (delete)
                File.Delete(config_dta);

            return ret;

        }

        public static string
        Get_Service_Pass() {

            var pass = "Info!";

            var date = DateTime.Now;
            var odd = date.Day % 2 == 1;

            var hh = date.Hour.ToString("00");
            var dd = date.Day.ToString("00");

            return pass + (odd ? dd + hh : hh + dd);

            //if (odd)
            //    ret += hh;

            //ret += dd;

            //if (!odd)
            //    ret += hh;

            //return ret;
            //
        }



        public static bool
        Ensure_Single_Instance(bool broadcast) {

            Mutex mut;
            bool ret = H.Ensure_Single_Instance(Data.Mutex_ID, true, out mut);

            if (!ret && broadcast) {

                // alert all other instances
                H.Broadcast(Native_Const.WM_USER,
                            0,
                            new[] { (int)Data.Sub_App },
                            H.Other_Instances());


            }

            return ret;

        }

        public static Pair<DateTime>
        Get_Financial_Period(Work_Space ws) {

            var sd = ws.Create<SetupData>();

            int year1 = sd["FINYEAR_YEAR"].ToInt32();

            int month1 = sd["FINYEAR_MONTH"].ToInt32() + 1;

            int month2 = month1 == 1 ? 12 : month1 - 1;

            DateTime dt1 = new DateTime(year1, month1, 1);
            DateTime dt2 = new DateTime(year1, month2, 1);

            dt2 = dt2.Last_Date_Of_Month();

            return Pair.Make(dt1, dt2);

        }

        public static string
        Format_Financial_Period(Pair<DateTime> period, bool both) {

            const string fmt = "dd MMMM yyyy";
            var ret = period.First.ToString(fmt);

            if (both) {
                ret += " - " + period.Second.ToString(fmt);
            }

            return ret;
        }

        class Crypt : ICrypt
        {
            public string Encrypt(string clear) {
                return Encryption.Encrypt(clear);
            }
            public string Decrypt(string cipher) {
                return Encryption.Decrypt(cipher);
            }
        }

        public static bool
        Get_Ini_File(bool set_as_def,
                     bool replace_with_empty,
                     out Ini_File ini) {

            var file = Data.Ini_Filename.Value.Path.ToLower();
            return Get_Ini_File(file, set_as_def, replace_with_empty, out ini);

        }

        public static bool
        Get_Ini_File(string file,
                     bool set_as_def,
                     bool replace_with_empty,
                     out Ini_File ini) {

            Ini_File.Add_Crypto_Proxy(file, new Crypt());

            if (!File.Exists(file)) {

                Directory.CreateDirectory(Path.GetDirectoryName(file));

                using (File.Create(file)) { }

                if (!Data.Is_Excel_Helper)
                    Ini_Main.Prepare_Blank_Ini_File(file);

            }

            bool ret = Ini_File.Get(file, out ini);

            if (!ret) {
                if (replace_with_empty) {
                    File.WriteAllText(file, "");
                    ret = Ini_File.Get(file, out ini);
                }
            }

            if (ret && set_as_def)
                Data.Ini_File = ini;

            return ret;

        }

        public static IReadWrite<Ini_Field, string>
        Company_Proxy(this Ini_File file, Company_Number number) {
            return file.Group(number.As_String);
        }

        public static IRead<Ini_Field, string>
        Company_Proxy_ro(this Ini_File file, Company_Number number) {
            return file.Group_ro(number.As_String);
        }

        public static string
        Get_Username() {

            var customer = new SDOAPPLib.Customer10();
            string ret = customer.UserName;
            //return (ret.IsNullOrEmpty() ? "SCREENS" : ret);

            return ret;

        }

        public static string
        File_Stamp() {

            return DateTime.Today.ToString(true).Replace("/", "-");

        }

        public static bool
        Find_Setup_Dta(string path, out string real_path) {

            path = path.Trim(' ', '"');

            real_path = "";

            if (!Directory.Exists(path))
                return false;

            Func<string> setup_dta = () => path.Cpath("SETUP.DTA");
            Func<bool> exists = () => File.Exists(setup_dta());

            if (!File.Exists(setup_dta())) {

                var casing = path.Get_Casing();

                do {
                    var contents = Directory.GetDirectories(path);

                    if (exists())
                        break;
                    if (contents.Length != 1)
                        break;

                    path = path.Cpath(contents[0]);

                } while (true);

                if (!exists()) {

                    path = path.Cpath("Accdata");

                }

                path = path.To_Casing(casing);

            }

            bool ret = exists();

            if (ret)
                real_path = path;

            return ret;

        }

        /// <summary>
        /// Retrieves the two global WaitHandle objects used for synchronization between
        /// a starting SES.EXE process and an existing SES.EXE process in dashboard mode.
        /// Because of the logic involved (see ses_start.txt), the WHs may need to satisfy
        /// certain conditions, in particular they may need to be new (ie created by the current
        /// call), or they may need to be old (existing prior to the call, presumably created
        /// by another process). These requirements are stated using the 'must_be_new' and the
        /// 'must_be_old' parameters, and the call verifies them as it retrieves the handles and
        /// signals the caller using the return value of the call.
        /// 
        /// Note that if both parameters are 'true', the method throws an exception. 
        /// Note as well that the out parameters are populated IFF the return value is TRUE.
        /// If bath parameters are 'false', the method always returns 'true', provided no exceptions
        /// occur.
        /// </summary>
        public static bool
        Get_Event_Handles(bool state,
                          bool must_be_new,
                          bool must_be_old,
                          out EventWaitHandle handle1,
                          out EventWaitHandle handle2) {

            (must_be_new && must_be_old).tift();

            const string name1 = "InfoTrends_____________________________________CreationWaitHandle1";
            const string name2 = "InfoTrends_____________________________________CreationWaitHandle2";

            bool is_new = false;
            EventWaitHandle tmp_handle1, tmp_handle2;

            H.assign(out handle1, out handle2, out tmp_handle1, out tmp_handle2);

            Func<bool> check = () =>
            {
                if ((must_be_new && !is_new) ||
                    (must_be_old && is_new)) {

                    using (tmp_handle1)
                    using (tmp_handle2) { }

                    return false;
                }
                return true;
            };


            try {

                tmp_handle1 = new EventWaitHandle(state, EventResetMode.AutoReset, name1, out is_new);

                if (!check())
                    return false;

                tmp_handle2 = new EventWaitHandle(state, EventResetMode.AutoReset, name2, out is_new);

                if (!check())
                    return false;
            }
            catch {
                using (tmp_handle1)
                using (tmp_handle2) { }
                throw;
            }

            handle1 = tmp_handle1;
            handle2 = tmp_handle2;

            return true;

        }

        /// <summary>
        /// Retrieves a single wait handle used for IP synchronization. See comments@Get_Event_Handles for more
        /// info.
        /// </summary>
        [Obsolete("Not used")]
        public static EventWaitHandle
        Get_Event_Handle(bool state, bool must_be_new, bool must_be_old) {

            (must_be_new && must_be_old).tift();

            const string name = "InfoTrends_____________________________________CreationWaitHandle";

            bool is_new;

            var ret = new EventWaitHandle(state, EventResetMode.AutoReset, name, out is_new);

            if ((must_be_new && !is_new) ||
                (must_be_old && is_new)) {
                ret.Close();
                ret = null;
            }

            return ret;

        }










        /*       Page settings persistence in sescfg        */


        const bool xml = true;

        public static PageSettings Get_Page_Settings(Company_Number company, out bool set) {

            set = false;
            string file = Data.Page_Settings_File(company);

            Func<PageSettings> def = () => new PageSettings();
            if (!File.Exists(file)) {
                return def();
            }

            PageSettings ret;

            if (!S.Deserialize_From_File<PageSettings>(file, false, out ret))
                return def();

            set = true;
            return ret;

        }

        public static bool Save_Page_Settings(PageSettings settings, Company_Number company) {

            // see msdn:mysettings.save thread, answer by Jack2005_MSFT
            var pr_settings = settings.PrinterSettings;
            if (pr_settings.PrintFileName.IsNullOrEmpty())
                pr_settings.PrintFileName = "_";

            string file = Data.Page_Settings_File(company);

            var ret = S.Serialize_To_File(file, false, settings);

            return ret;

        }


        public static void Refresh_Border(this Control ctrl) {

            var border = Border.First_Instance(ctrl);
            if (border != null)
                border.Refresh();

        }

        public static void Refresh_Combobox_Borders(ComboBox closed, ComboBox[] boxes) {

            foreach (var box in boxes) {

                if (box.Location.Y < closed.Location.Y)
                    continue;

                if (box.Location.Y > closed.Bounds.Bottom + closed.DropDownHeight)
                    continue;

                if (box.Location.X > closed.Bounds.Right)
                    continue;

                if (box.Bounds.Right <= closed.Location.X)
                    continue;

                var border = Border.First_Instance(box);

                if (border != null)
                    border.Invalidate();

            }
        }




        /*       Required files validation        */




        public static bool Check_For_Files(bool show) {
            string msg;
            return Check_For_Files(show, out msg);
        }

        public static bool Check_For_Files(bool show, out string msg) {

            msg = "";

            var files = Data.Get_Program_Files().ToList();

            List<Program_File_Info> missing;

            if (H.Check_Files(files, out missing))
                return true;

            msg = Get_Missing_Files_Message(missing);

            if (show)
                Standard.Show(Message_Type.Error, msg);


            return false;

        }

        public static void Show_Service_DLL_Message() {

            MessageBox.Show(
                "Required system file SERVICE.DLL is missing.\n\nThe program cannot continue.",
                Data.App_Name + " - System Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

        }


        public static string Get_Missing_Files_Message(List<Program_File_Info> missing) {

            int cnt = missing.Count;
            var msg = "Required system file{0} {1} missing." +
                "\n\nReinstalling the application may fix the problem.";
            // The program cannot continue.

            var ret = String.Format(msg,
                cnt == 1 ? "" : "s",
                missing.Select(_f => _f.Name).Lippert_Print());

            return ret;
        }

        public static int
        Top_Level_FNF(FileNotFoundException _) {

            if (!File.Exists(Data.STR_SERVICEDLL.Path)) {
                M.Show_Service_DLL_Message();
            }
            else {
                M.Check_For_Files(true);
            }

            return (int)Return_Code.Files_Missing;
        }

        /*       Other early setup        */


        public static void
        Set_Winforms_Exception_Handlers(
            Action<ThreadExceptionEventArgs> Application_ThreadException,
            Action<UnhandledExceptionEventArgs> CurrentDomain_UnhandledException) {

            AppDomain.CurrentDomain.UnhandledException +=
                  (_, args) => CurrentDomain_UnhandledException(args);

            Application.ThreadException += (_, args) => Application_ThreadException(args);

        }

        public static void
        Set_Winforms_Exception_Handlers(Logging logger) {

            Set_Winforms_Exception_Handlers(logger, Common.Exceptions.Dispatch_X);

        }

        static readonly Set<Logging> swf_handlers_set = new Set<Logging>();

        public static void
        Set_Winforms_Exception_Handlers(Logging logger, Action<Exception> act) {
            if (swf_handlers_set[logger])
                return;

            Set_Winforms_Exception_Handlers(
                  args =>
                  {
                      act(args.Exception);
                      logger.Log(args.Exception);
                      Common.Exceptions.Dispatch_X(args.Exception);

                  },
                  args =>
                  {
                      var as_ex = args.ExceptionObject as Exception;
                      if (as_ex != null) {
                          act(as_ex);
                          logger.Log(as_ex);
                          Common.Exceptions.Dispatch_X(as_ex);

                      }
                  }
                  );

            swf_handlers_set[logger] = true;

        }

        public static Logging Get_Logger(bool set_auto_logging, bool set_handlers) {

            if (set_auto_logging)
                Logging.Auto_Logging = true;

            var ret = Logging.Get_Logger(Data.Errorlog.Path,
                                   Data.Errorlog_bak,
                                   Data.MAX_LOG_SIZE_KB,
                                   true);

            if (set_handlers)
                Set_Winforms_Exception_Handlers(ret);

            return ret;
        }

        public static void Set_Culture() {

            M.Set_Culture(
                  ".", ".", "/", DayOfWeek.Monday, "dd/MM/yyyy",
                //"fr-FR"
                  "el-GR");


        }

        public static void Set_Culture(string number_decimal_separator,
                             string currency_decimal_separator,
                             string date_separator,
                             DayOfWeek first_day_of_week,
                             string short_date_pattern,
                             string culture_info) {

            //Culture stuff for the double.parse and date.parse methods
            NumberFormatInfo appNumberFormat = new NumberFormatInfo();
            appNumberFormat.NumberDecimalSeparator = number_decimal_separator;
            appNumberFormat.CurrencyDecimalSeparator = currency_decimal_separator;

            DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();
            dateFormat.DateSeparator = date_separator;
            dateFormat.FirstDayOfWeek = first_day_of_week;
            dateFormat.ShortDatePattern = short_date_pattern;

            //CultureInfo appCultureInfo = new CultureInfo("fr-FR");
            CultureInfo appCultureInfo = new CultureInfo(culture_info);

            appCultureInfo.NumberFormat = appNumberFormat;
            appCultureInfo.DateTimeFormat = dateFormat;

            Thread.CurrentThread.CurrentCulture = appCultureInfo;

        }



        static public void Run_Sescfg(bool ses) {
            try {
                if (ses)
                    Process.Start(Data.STR_SESCFGEXE.Path);
                else
                    Process.Start(Data.STR_SITCFGEXE.Path);
            }
            catch (FileNotFoundException) { }
        }

    }

}
