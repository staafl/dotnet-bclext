using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Management;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;
using System.Threading;



#if WINFORMS
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
#endif

namespace Fairweather.Service
{

    /* Various utility methods kept here to avoid extension methods bloat */
    public static class H
    {



        // http://www.camaswood.com/tech/get-unc-path-from-mapped-drive-or-local-share/

        static public bool
        Try_Get_UNC_or_Local_Path(string path, out string ret) {


            //if path is rooted, check if the drive letter is local

            if (!Path.IsPathRooted(path))
                path = Path.GetFullPath(path);

            var root = Path.GetPathRoot(path);

            try {
                var dinfo = new DriveInfo(root);
                if (dinfo.DriveType == DriveType.Network)
                    return Try_Get_Universal_Name(path, out ret);

            }
            catch (ArgumentException) {
            }

            
            ret = path;
            return true;



        }

        static public bool
        Try_Get_Universal_Name(string path, out string ret) {

            ret = null;

            // Already a UNC, no need to convert
            if (path.StartsWith(@"\\") || path.StartsWith(@"//")) {
                ret = path;
                return true;

            }

            // WNetGetUniversalName does not allow a null buffer
            IntPtr buf = Marshal.AllocCoTaskMem(5);
            int size = 0;

            try {
                // First call to WNetGetUniversalName to get the buffer size
                int code = Native_Methods.WNetGetUniversalName(path, Native_Const.UNIVERSAL_NAME_INFO_LEVEL, buf, ref size);

                // Local Drive
                if (code == Native_Const.ERROR_NOT_CONNECTED || code == Native_Const.ERROR_NO_NETWORK) {

                    return Try_Get_Local_Universal_Name(path, out ret);

                }

                // If the return is not ERROR_MORE_DATA then something went wrong and the
                // conversion didn't work so return the original path
                if (code != Native_Const.ERROR_MORE_DATA) {
                    ret = path;
                    return false;

                }
            }
            finally {
                // Release the temp buffer
                Marshal.FreeCoTaskMem(buf);

            }

            buf = Marshal.AllocCoTaskMem(size);
            try {
                // Look up the name of the share for the mapped drive
                int code = Native_Methods.WNetGetUniversalName(path, Native_Const.UNIVERSAL_NAME_INFO_LEVEL, buf, ref size);

                // If it didn't convert just return the original path
                if (code != Native_Const.NOERROR) {
                    ret = path;
                    return false;

                }

                // Now convert the result to a string we can actually use
                // It's all in the same buffer, but the pointer is first,
                // so offset the pointer by IntPtr.Size and pass to PtrToStringAnsi.
                ret = Marshal.PtrToStringAnsi(new IntPtr(buf.ToInt64() + IntPtr.Size), size);

                // Clean up stuff at the end of UNCPath
                int actualLength = ret.IndexOf('\0');
                if (actualLength >= 0) {
                    ret = ret.Substring(0, actualLength);
                }
                else {
                    // ?
                }

                return true;
            }
            finally {
                // Release the temp buffer
                Marshal.FreeCoTaskMem(buf);

            }
        }

        // Get a UNC for a local share
        static public bool
        Try_Get_Local_Universal_Name(string path, out string ret) {

            ret = null;

            // Already a UNC, no need to convert
            if (path.StartsWith(@"\\") || path.StartsWith(@"//")) {
                ret = path;
                return true;

            }

            var exportedShares = new ManagementClass("Win32_Share");
            ManagementObjectCollection shares = exportedShares.GetInstances();

            foreach (ManagementObject share in shares) {
                var share_name = share["Name"].ToString();
                var share_path = share["Path"].ToString();
                // var caption = share["Caption"].ToString();

                // TOCHECK: case-sensitivity?
                if (!share_name.Contains("$") && path.StartsWith(share_path)) {
                    var rest = share_path.Substring(share_path.Length);
                    ret = String.Format(@"file://\\{0}\{1}\{2}", Environment.MachineName, share_name, rest);
                    return true;

                }

            }


            return false;
        }

        public static void Copy_Directory(string from, string to, bool over) {
            // http://stackoverflow.com/questions/58744/best-way-to-copy-the-entire-contents-of-a-directory-in-c
            new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyDirectory(from, to, over);

            /* other option, same source
             * Process proc = new Process();
proc.StartInfo.UseShellExecute = true;
proc.StartInfo.FileName = @"C:\WINDOWS\system32\xcopy.exe";
proc.StartInfo.Arguments = @"C:\source C:\destination /E /I";
proc.Start();
             // wait for process termination
*/
        }

        public static string To_Excel_Column(int number) {
            (number > 0).tiff();

            // http://stackoverflow.com/questions/181596/how-to-convert-a-column-number-eg-127-into-an-excel-column-eg-aa

            int dividend = number;
            var columnName = "";
            int modulo;

            while (dividend > 0) {
                modulo = (dividend - 1) % 26;
                columnName = (char)(65 + modulo) + columnName;
                dividend = ((dividend - modulo) / 26);
            }

            return columnName;

        }

        public static bool Is_File_Empty(string path) {
            var finfo = new FileInfo(path);
            return (finfo.Exists && finfo.Length == 0);
        }

        static public bool Check_For_Other_Instances() {
            // http://stackoverflow.com/questions/206323/how-to-execute-command-line-in-c-get-std-out-results
            var p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "tasklist.exe";
            p.StartInfo.Arguments = "/FI \"USERNAME eq " + Environment.UserName + "\"";
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            var me = Process.GetCurrentProcess();
            var me_name = me.ProcessName.ToUpper();

            var seen_once = false;
            string str;
            while ((str = p.StandardOutput.ReadLine()) != null) {
                str = str.ToUpper();
                if (str.StartsWith(me_name + ".EXE ")) {
                    if (seen_once)
                        return false;
                    seen_once = true;
                }
            }

            return true;

        }
        // Doesn't handle same file accessed through different network shares.
        static public bool
        Path_Compare(string file1, string file2) {

            file1 = file1.Clean();
            file2 = file2.Clean();

            var abs1 = Path.GetFullPath(file1).Clean();
            var abs2 = Path.GetFullPath(file2).Clean();

            return (abs1 == abs2);

        }

        // if one or both of the files doesn't exist, returns false
        // 
        public static bool
        File_Compare(string file1, string file2, int? depth) {

            int b1, b2;
            FileStream fs1, fs2;


            if (!File.Exists(file1) || !File.Exists(file2))
                return false;

            if (Path_Compare(file1, file2))
                return true;

            using (fs1 = new FileStream(file1, FileMode.Open))
            using (fs2 = new FileStream(file2, FileMode.Open)) {

                if (fs1.Length != fs2.Length)
                    return false;

                do {
                    b1 = fs1.ReadByte();
                    b2 = fs2.ReadByte();
                }
                while ((b1 == b2) &&
                       (b1 != -1) &&
                       (depth -= 1) >= 0);


                return (b1 == b2);

            }
        }


        public static string
        Get_Md5_File_Hash(string path) {
            // http://www.nonhostile.com/howto-calculate-md5-hash-file-vb-net.asp
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var md5 = new MD5CryptoServiceProvider()) {

                var hash = md5.ComputeHash(fs);

                return hash.Pretty_Print("x2");

            }

        }

        public static string
        Get_Md5_Hash(string input) {
            // http://blog.brezovsky.net/en-text-2.html
            using (var md5 = new MD5CryptoServiceProvider()) {

                var bs = Encoding.UTF8.GetBytes(input);
                bs = md5.ComputeHash(bs);
                var sb = new StringBuilder();
                foreach (var b in bs)
                    sb.AppendFormat("x2", b);

                return sb.ToString();

            }
        }

        public static string
        Get_Password_From_Prompt() {

            var lst = new LinkedList<char>();
            const string prompt = "Password: \n";

            Console.Clear();
            Console.Write(prompt);

            while (true) {

                var ckey = Console.ReadKey();
                var key = ckey.Key;
                var ch = ckey.KeyChar;

                Console.Clear();

                if (key == ConsoleKey.Backspace) {
                    if (lst.Count > 0)
                        lst.RemoveLast();
                }
                else {
                    if (Char.IsLetterOrDigit(ch) ||
                        Char.IsPunctuation(ch) ||
                        Char.IsSeparator(ch) ||
                        Char.IsWhiteSpace(ch)) {
                        lst.AddLast(ch);
                    }
                }


                if (key == ConsoleKey.Enter) {
                    Console.Clear();
                    break;
                }

                Console.Write(prompt);
                Console.Write(new string('*', lst.Count));

            }

            if (lst.Count <= 0)
                return "";

            for (int ii = 0; ii < 2; ++ii) {
                var value = lst.Last.Value;
                if (value == '\r' || value == '\n')
                    lst.RemoveLast();
                else
                    break;
            }
            var str = new string(lst.arr());

            return str;

        }

        /// <summary>
        /// Deletes all but the last "max" lines in the file
        /// </summary>
        public static void
        Trim_File_To_Last(string file, int max) {

            var queue = new Queue<string>();

            int ii = 0;

            using (var sr = new StreamReader(file)) {

                string str;

                while ((str = sr.ReadLine()) != null) {

                    queue.Enqueue(str);
                    if (ii++ > max)
                        queue.Dequeue();
                }
            }

            File.WriteAllLines(file, queue.arr());

        }
#if WINFORMS

        public static bool App_Active() {
            var fwh = Native_Methods.GetForegroundWindow();
            int pid;
            var handle = Native_Methods.GetWindowThreadProcessId(fwh, out pid);
            var ret = pid == Process.GetCurrentProcess().Id;
            return ret;
        }

        public static Form Get_Top_Form() {

            return Form.ActiveForm;
            /*
            var open = Application.OpenForms;
            if(open.Count == 0)
                return null;

            var frm = (Form)open[0];
            foreach (var child in frm.OwnedForms)
                if (child.Modal && child.Visible)
                    return child;

            return frm;//*/

        }

        public static IDisposable Change_Cursor(Cursor cursor) {

            Cursor before = Cursor.Current;
            if (before == cursor)
                return null;
            Cursor.Current = cursor;
            return new On_Dispose(() => Cursor.Current = before);

        }
#endif
        public static bool Maybe_Backup(string file, int? recurse) {

            if (!File.Exists(file))
                return false;

            string back = file + ".bak";

            if (recurse == null || recurse > 0) {
                Maybe_Backup(back, recurse == null ? (int?)null : recurse.Value - 1);
            }
            else {
                File.Delete(back);
            }

            File.Move(file, back);

            return true;

        }

        public static void
        Swap_Files(string path1, string path2) {

            for (int ii = 0; ii < 2; ++ii) {

                if (!File.Exists(path1)) {

                    if (File.Exists(path2))
                        File.Move(path2, path1);

                    return;
                }

                H.Swap(ref path1, ref path2);
            }


            string temp = "";

            do {
                temp = Path.GetRandomFileName();
            } while (File.Exists(temp));

            File.Move(path1, temp);
            File.Move(path2, path1);
            File.Move(temp, path2);

        }


        static public bool
        Valid_Path(string path, bool must_be_dir) {

            if (path.IsNullOrEmpty())
                return false;


            path = path.ToUpper();

            string file, dir;

            // check 1
            try {
                file = Path.GetFileName(path);
            }
            catch (ArgumentException) {
                return false;
            }

            // check 2
            try {
                dir = Path.GetDirectoryName(path);
                if (dir == null)
                    dir = path;
                dir = dir.ToUpper().Trim(Path.PathSeparator);
            }
            catch (ArgumentException) {
                return false;
            }


            // is directory name equal to whole path
            // alternatively, we could check if extension is ""
            if (must_be_dir)
                return (dir == path.Trim(Path.PathSeparator));

            return true;

        }

        static public int
        CompareArrays<T>(T[] arr1,
                         T[] arr2,
                         int start1,
                         int start2,
                         int length,
                         int if_first_too_short,
                         int if_second_too_short)
            where T : IComparable<T> {
            int len1 = arr1.Length;
            int len2 = arr2.Length;

            for (int ii = 0; ii < length; ++ii) {

                int ix1 = start1 + ii;
                int ix2 = start2 + ii;

                bool ts1 = ix1 > len1;
                bool ts2 = ix2 > len2;

                if (ts1 || ts2) {
                    if (ts1 == ts2)
                        return 0;

                    return ts1 ? if_first_too_short : if_second_too_short;
                }

                var cmp = Comparer<T>.Default.Compare(arr1[ix1], arr2[ix2]);

                if (cmp != 0)
                    return cmp;

            }

            return 0;

        }


        static public IEnumerable<TEnum>
        Get_Enum_Values<TEnum>()
        where TEnum : struct {

            var type = typeof(TEnum);

            // type.IsEnum.tiff();

            var ret = Enum.GetValues(type);

            // return (TEnum[])ret;
            return ret.Cast<TEnum>();


        }


        static public void
        Prepend_All_Text(string file, string text) {

            if (!File.Exists(file)) {
                File.WriteAllText(file, "");
                return;
            }

            var temp = Path.GetTempFileName();

            using (var sw = new StreamWriter(temp)) {
                sw.Write(text);
                using (var sr = new StreamReader(file)) {

                    var buf = new char[100];
                    int read = 0;
                    while ((read = sr.Read(buf, 0, 100)) != 0)
                        sw.Write(buf, 0, read);
                }
            }

            File.Replace(temp, file, null);

        }

        static public void
        Create_Directories(params string[] files) {

            var dirs = files.Select(_f => Path.GetDirectoryName(Path.GetFullPath(_f))).lst();

            foreach (var dir in dirs) {
                Directory.CreateDirectory(dir);

            }
        }

        static public int?
        Run_Notepad(string file, bool wait, bool maximized) {

            Create_Directories(file);

            if (!File.Exists(file))
                File.WriteAllText(file, "");
            //    return null;

            Process process = new Process();

            process.StartInfo.FileName = "notepad.exe";
            process.StartInfo.Arguments = file;
            process.StartInfo.WindowStyle = maximized ? ProcessWindowStyle.Maximized : ProcessWindowStyle.Normal;
            process.Start();

            if (!wait)
                return 0;

            process.WaitForExit();

            int ret = process.ExitCode;

            return ret;
        }

        public static IEnumerable<Process> Other_Instances() {

            var proc = Process.GetCurrentProcess();

            int pid = proc.Id;
            var pname = proc.ProcessName;
            // proc.MainModule.ModuleName.Replace(".exe", "")

            var procs = from proc2 in Process.GetProcessesByName(pname)
                        where proc2.Id != pid
                        select proc2;

            return procs;
        }
#if WINFORMS
        public static void Select_On_Enter(params TextBoxBase[] tbxs) {
            foreach (var tb in tbxs) {
                var close = tb;
                close.Enter += (_1, _2) => close.SelectAll();
            }
        }
#endif


        // What will be called when calling write("", 0)?
        public static void Write(object obj, int indent) {

            obj = new string('\t', indent) + obj;
            Console.Write(obj);

        }
        public static void WriteLine(object obj, int indent) {

            obj = new string('\t', indent) + obj;
            Console.WriteLine(obj);

        }
        public static void Write(string format, int indent, params object[] args) {

            format = new string('\t', indent) + format;

            Console.Write(format, args);

        }
        public static void WriteLine(string format, int indent, params object[] args) {

            format = new string('\t', indent) + format;

            Console.WriteLine(format, args);

        }


        public static void assign<T1>(out T1 t1) {

            t1 = default(T1);

        }
        public static void assign<T1, T2>(out T1 t1, out T2 t2) {

            t1 = default(T1);
            t2 = default(T2);

        }
        public static void assign<T1, T2, T3>(out T1 t1, out T2 t2, out T3 t3) {

            t1 = default(T1);
            t2 = default(T2);
            t3 = default(T3);

        }
        public static void assign<T1, T2, T3, T4>(out T1 t1, out T2 t2, out T3 t3, out T4 t4) {

            t1 = default(T1);
            t2 = default(T2);
            t3 = default(T3);
            t4 = default(T4);

        }
        public static void assign<T1, T2, T3, T4, T5>(out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5) {

            t1 = default(T1);
            t2 = default(T2);
            t3 = default(T3);
            t4 = default(T4);
            t5 = default(T5);

        }


        static long Directory_Size(DirectoryInfo dir) {

            var path = Path.GetFullPath(dir.FullName).ToUpper();
            long ret;

            //if (!sizes.TryGetValue(path, out ret)) {

            ret = 0;

            foreach (var file in dir.GetFiles())
                ret += file.Length;

            foreach (var sub_dir in dir.GetDirectories())
                ret += Directory_Size(sub_dir);

            //sizes[path] = ret;

            // }

            return ret;

        }

        // For escaping xml:
        // http://msdn.microsoft.com/en-us/library/system.security.securityelement.escape(VS.80).aspx
        ///
        // For escaping urls:
        // http://msdn.microsoft.com/en-us/library/system.web.httputility.aspx

        public static bool Run_Browser(string url, bool check, out string error_message) {



            error_message = null;

            if (check && !Url_Is_Valid(url)) {
                error_message = "URL not recognized as valid:\n" + url;
                return false;
            }

            Process proc;
            try {
                // http://msdn.microsoft.com/en-us/library/0w4h05yb.aspx
                proc = Process.Start(url);
            }
            catch (Win32Exception exc) {

                const int error = -2147467259;

                if (exc.ErrorCode == error) {
                    error_message = exc.Message;
                    return false;
                }

                throw;
            }

            if (proc != null) // started a new process
                H.Bring_To_Fore(proc, true);

            return true;

        }

        public static T Set<T>(ref T variable, T value) {

            T old = variable;
            variable = value;
            return old;

        }

        public static bool Url_Is_Valid(string smtpHost) {
            bool br = false;
            try {
                var ipHost = Dns.GetHostEntry(smtpHost);
                br = true;
            }
            catch (SocketException) {
                br = false;
            }
            return br;
        }

        static public bool Is_Same_File(string file1, string file2) {

            var path1 = Path.GetFullPath(file1).ToUpper();
            var path2 = Path.GetFullPath(file2).ToUpper();

            var ret = (path1 == path2);

            return ret;
        }



        public static string Current_Dir() {

            return AppDomain.CurrentDomain.BaseDirectory;

        }

        /// <summary>
        /// Moves file at 'from' to 'to', overwriting the file at 'to' if it exists.
        /// </summary>
        public static void Overwrite(string from, string to) {
            File.Delete(to);
            File.Move(from, to);

        }

#if WINFORMS
        public static T First_Open<T>() where T : Form {
            var ret = (T)Application.OpenForms.From_Enumerable<Form>().FirstOrDefault(form => form is T);
            return ret;
        }

        public static bool Is_Alt_Pressed() {
            var Control_ModifierKeys = Control.ModifierKeys;

            return Control_ModifierKeys.Contains(Keys.Alt);

            //return Native_Methods.GetKeyState(Key_Codes.VK_LMENU) != 0 ||
            //       Native_Methods.GetKeyState(Key_Codes.VK_RMENU) != 0;

        }

#endif
        public static bool Is_LCtrl_Pressed() {
            var Control_ModifierKeys = Control.ModifierKeys;

            return Control_ModifierKeys.Contains(Keys.Control) ||
                   Control_ModifierKeys.Contains(Keys.LControlKey) ||
                   Control_ModifierKeys.Contains(Keys.RControlKey) ||
                   Control_ModifierKeys.Contains(Keys.ControlKey);

        }

        public static bool Is_Shift_Pressed() {
            var Control_ModifierKeys = Control.ModifierKeys;

            return Control_ModifierKeys.Contains(Keys.Shift) ||
                   Control_ModifierKeys.Contains(Keys.RShiftKey) ||
                   Control_ModifierKeys.Contains(Keys.LShiftKey) ||
                   Control_ModifierKeys.Contains(Keys.ShiftKey);


        }

        public static Rectangle Screen_Rectangle(bool only_working_area) {

            var screen = Screen.PrimaryScreen;

            Rectangle ret;
            if (only_working_area)
                ret = screen.WorkingArea;
            else
                ret = screen.Bounds;

            return ret;

        }

        public static bool
        Test_Assembly(string assembly_name) {

            if (!File.Exists(assembly_name))
                return false;
            try {
                Assembly.LoadFrom(assembly_name);
            }
            catch (IOException) {
                return false;
            }
            catch (ArgumentException) {
                return false;
            }
            catch (BadImageFormatException) {
                return false;
            }

            return true;
        }





        public static int
        Compare_Versions(string str1, string str2) {

            Func<char, bool> dot_or_digit = ch => Char.IsDigit(ch) || ch == '.';


            (str1.All(dot_or_digit) && str2.All(dot_or_digit)).tiff();

            var en1 = str1.Split('.'); // 1.0.101 ->  ["1", "0", "101"]
            var en2 = str2.Split('.'); // 1.0.11.5 -> ["1", "0", "11", "5"]

            int len1 = en1.Length;     // 3
            int len2 = en2.Length;     // 4

            int len = Math.Max(len1, len2); // 4

            for (int ii = 0; ii < len; ++ii) {

                var s1 = en1.Get_Or_Default(ii, "0");
                var s2 = en2.Get_Or_Default(ii, "0");

                var cmp = s1.Int_Compare(s2);

                if (cmp != 0)
                    return cmp;


            }

            return 0;


        }

        /// <summary>
        /// BMP,GIF,JPEG,PNG,TIFF
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool
        Validate_Image(string filename) {

            using (var img = Try_Get_Image(filename))
                return img != null;

        }

        public static Image
        Try_Get_Image(string filename) {

            if (!File.Exists(filename))
                return null;
            // http://msdn.microsoft.com/en-us/library/stf701f5.aspx
            try {
                return Image.FromFile(filename);
            }
            catch (OutOfMemoryException) {
                return null;
            }

        }



        /// <summary>
        /// Test
        /// </summary>
        public static int?
        Replace_In_File(string filename, string to_replace, string replace_with) {//, int? max_replaces) {

            if (to_replace.IsNullOrEmpty())
                return null;

            replace_with = replace_with ?? "";

            string backup = filename;

            do {

                backup += ".bak";
                //backup += ".txt";

            } while (File.Exists(backup));

            int replaces = 0;
            bool do_delete = true;
            try {
                int length = to_replace.Length;
                int len_diff = replace_with.Length - length;

                // The size of the chunks we will be dealing with
                int to_read = length * 2 - 1;


                var buffer = new char[to_read];

                using (var sr = new StreamReader(filename))
                using (var sw = new StreamWriter(backup)) {

                    //Func<bool> do_replace = max_replaces.HasValue ? F.True : () => max_replaces.Value < replaces;
                    //
                    // Start with a full buffer
                    sr.Read(buffer, 0, to_read);

                    while (true) {

                        // How much we are going to save from this read
                        int to_save = length;

                        // How much we are going to discard
                        int to_chop = length;

                        var str = new string(buffer);


                        int index = str.IndexOf(to_replace);

                        if (index != -1) {

                            str = str.Replace(to_replace, replace_with);

                            ++replaces;

                            // this will put the cursor right after the original string's occurrence
                            to_save += index;
                            to_chop += index;

                            // this will put the cursor right after the new string
                            to_save += len_diff;

                        }

                        var result = str.Substring(0, to_save);

                        sw.Write(result);

                        // discard to_chop characters
                        Array.Copy(buffer, to_chop, buffer, 0, to_read - to_chop);

                        if (sr.EndOfStream) {
                            // Write the last batch
                            sw.Write(buffer, 0, to_read - to_chop);
                            break;
                        }
                        else {
                            sr.Read(buffer, to_read - to_chop, to_chop);
                        }
                    }


                }

                File.Delete(filename);

                try {
                    File.Move(backup, filename);
                }
                catch {
                    do_delete = false;
                    throw;
                }

                return replaces;
            }
            finally {
                if (do_delete)
                    File.Delete(backup);
            }

        }

        public static bool
        Safe_Delete(string filename) {

            if (!File.Exists(filename))
                return false;

            try {
                File.Delete(filename);
            }
            catch (IOException) { return false; }

            return true;

        }






        public static Lazy<string> Create_Dir(string dir) {
            return Create_Dir(dir, dir);
        }
        public static Lazy<string>
        Create_Dir(string dir, string filename) {

            return new Lazy<string>(() =>
            {
                Directory.CreateDirectory(dir);
                return filename;
            });

        }

        public static Image
        Get_Bitmap(string filename) {

            File.Exists(filename).tiff();

            //using (var fs = new FileStream(filename, FileMode.Open)) {

            var bmp = new Bitmap(filename);

            //var bmp = new Bitmap(fs);


            return bmp;
            // }

        }

        /// <summary>
        /// Returns true if all required files are present
        /// </summary>
        public static bool
        Check_Files(List<Program_File_Info> files, out List<Program_File_Info> missing) {

            missing = new List<Program_File_Info>(files.Count);

            foreach (var file in files) {

                if (!File.Exists(file.Path))
                    missing.Add(file);


            }

            return missing.Is_Empty();

        }

        /// <summary>
        /// Searches for Microsoft Visual C++ 2005 Redistributable 8.0.56336
        /// </summary>
        /// <returns></returns>
        public static bool
        Has_VCRed() {

            string tmp = "";
            string vc_clsid = @"{7299052b-02a4-4627-81f2-1818da5d550d}";
            // use {837b34e3-7c30-493c-8f6a-2b0f04e2912c} for version 8.0.59193

            Microsoft.Win32.RegistryKey reg;

            try {
                reg = Microsoft.Win32.Registry.LocalMachine;
                reg = reg.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + vc_clsid);
                tmp = reg.GetValue("DisplayName", "").ToString();
            }
            catch (NullReferenceException) {
                tmp = "";
            }

            return !tmp.IsNullOrEmpty();
        }


#if WINFORMS
        public static IEnumerable<String>
        Printers() {


            var settings = new PrinterSettings();

            foreach (string printer in PrinterSettings.InstalledPrinters) {

                settings.PrinterName = printer;

                if (settings.IsValid)
                    yield return printer;

            }


        }

        public static string
        Printer_Or_Default(string printer_name, out bool found) {

            found = false;
            var settings = new PrinterSettings();

            string def = settings.PrinterName;

            if (printer_name.IsNullOrEmpty())
                return def;

            string ret = null;

            string upper = printer_name.ToUpper();

            foreach (string name in PrinterSettings.InstalledPrinters) {

                settings.PrinterName = name;

                if (!settings.IsValid)
                    continue;

                if (name.ToUpper() == upper) {
                    found = true;
                    ret = name;
                    break;
                }

                if (settings.IsDefaultPrinter)
                    def = name;

            }

            ret = ret ?? def;

            return ret;

        }


        static public bool
        Is_In_Designer {
            get { return (Assembly.GetEntryAssembly() == null); }
        }


#endif

        static public char[]
        Get_Filename_Characters() {
            return new char[] { ':', '/', '\\', '*', '?', '"', '<', '>', '|' };
        }

        static public char[]
        Filter_Filename_Characters(string str) {

            str.tifn();
            var ret = new List<char>(str.Length);
            var set = new Set<Char>(Get_Filename_Characters());
            foreach (var ch in str) {
                if (set[ch])
                    ret.Add(ch);
            }

            return ret.ToArray();
        }


        /// <summary>
        /// Supposing that you have a directory with files like
        /// "aaa 001.bbb"
        /// "aaa 002.bbb"
        /// "aaa 004.bbb"
        /// etc., where aaa and bbb are fixed strings.
        /// This function will return the next available number (5).
        /// No discrimination is made between e.g. "0005" and "005".
        /// In case no files were found in the directory, the return value is 1.
        /// </summary>
        public static int
        Get_Next_Free_Number(string directory, string file_prefix, string ext) {

            string prefix = file_prefix, dir = directory;

            var files_arr =
                    Directory.GetFiles(dir, prefix + "*" + ext, SearchOption.TopDirectoryOnly);

            files_arr.Sort();

            var files = from file in files_arr.Reverse()
                        select Path.GetFileNameWithoutExtension(file);

            int ret = 1;

            foreach (var file in files) {

                string str_number;
                int number;

                if (!file.Eat(prefix, out str_number))
                    continue;

                str_number = str_number.Trim();

                if (!int.TryParse(str_number, out number))
                    continue;

                ret = number + 1;

                break;
            }

            return ret;

        }

        public static void
        Backup_Copy(string file) {

            string back = file + ".bak";
            File.Copy(file, back, true);
            File.SetAttributes(back, FileAttributes.Hidden);

        }


        // http://support.microsoft.com/kb/314853 for 'select'
        public static int?
        Open_In_Explorer(string dir, bool wait, bool maximized, string select) {

            if (!Directory.Exists(dir))
                return null;

            var process = new Process();

            process.StartInfo.FileName = "explorer.exe";

            var args = dir;

            if (!select.IsNullOrEmpty())
                args = "/select," + dir.Cpath(select);



            process.StartInfo.Arguments = args;


            process.StartInfo.WindowStyle = maximized ? ProcessWindowStyle.Maximized : ProcessWindowStyle.Normal;
            process.Start();

            if (!wait)
                return 0;

            process.WaitForExit();

            var ret = process.ExitCode;

            return ret;


        }



        public static void
        Either_Order(bool first_second, Action first, Action second) {

            if (first_second)
                first();

            second();

            if (!first_second)
                first();

        }

        public static void
        Throw() {
            true.tift();
        }

        public static void
        Throw<T>() where T : Exception, new() {
            true.tift<T>();

        }


        // ****************************


        public static void
        Swap<T>(ref T first, ref T second) {

            var temp = first;
            first = second;
            second = temp;

        }

        static public T
        Swap<T>(ref T location, T value) {
            var temp = location;
            location = value;
            return temp;
        }


        // ****************************



        public static bool
        Assign_If_Null<T>(ref T nullable, T value) where T : class {

            bool ret = (nullable == null);

            if (ret)
                nullable = value;

            return ret;

        }

        public static bool
        Assign_If_Null<T>(ref Nullable<T> nullable, T value) where T : struct {

            bool ret = !nullable.HasValue;

            if (ret)
                nullable = value;

            return ret;

        }


        ///<summary>Used to make sure something does not get JIT-optimized away
        ///</summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void
        Void(params object[] obj) {

            { }

        }


        public static bool
        Ensure_Single_Instance(string mutex_id, bool close_mutex, out Mutex mutex) {


            bool ret;

            mutex = new Mutex(true, mutex_id, out ret);

            if (ret) {

                GC.KeepAlive(mutex);

            }
            else {

                if (close_mutex)
                    mutex.Close();

            }

            return ret;

        }

        public static bool
        Bring_To_Fore(Process process, bool maximize) {


            bool ok = true;

            var handle = Get_Main_Window_Handle(process);

            if (Native_Methods.IsIconic(handle))
                Native_Methods.ShowWindow(handle, Native_Const.ShowWindow.SW_RESTORE);

            int flag = Native_Methods.SetForegroundWindow(handle);

            Native_Methods.ShowWindow(handle, Native_Const.ShowWindow.SW_SHOW);

            if (maximize && ok)
                Native_Methods.ShowWindow(handle, Native_Const.ShowWindow.SW_MAXIMIZE);

            bool ret = (flag != 0);

            Native_Methods.ForceForegroundWindow(handle);
            Native_Methods.SendMessage(handle, Native_Const.WM_ACTIVATE, 0, 0);

            return ret;

        }



        static public void
        Broadcast(uint message_id, uint wParam, int[] message_data, IEnumerable<Process> targets) {



            if (message_data == null)
                throw new ArgumentException("message_data is null", "message_data");

            if (message_data.Length == 0)
                return;

            if (targets == null)
                throw new ArgumentException("targets is null", "targets");

            var pairs = targets.Pairs<Process, IntPtr>(Get_Main_Window_Handle).Where(pair => pair.Second != IntPtr.Zero);

            foreach (var pair in pairs) {
                var handle = pair.Second;
                var proc = pair.First;

                foreach (var to_send in message_data.Pend(0, false)) {

                    proc.WaitForInputIdle();

                    // MessageBox.Show("Sending to {0} : {1}".spf(handle, to_send));

                    //Native_Methods.PostMessage(handle, message_id, wParam, to_send);
                    //
                    Native_Methods.SendMessage(handle, message_id, wParam, (uint)to_send);

                }

            }


        }

        static public IntPtr
        Get_Main_Window_Handle(Process process) {

            var handle = process.MainWindowHandle;

            if (handle == IntPtr.Zero) {
                // Search through all top level windows
                // Compare process IDs with the ID of our process
                handle = IntPtr.Zero;

                EnumWindowsProc proc = (h, _) =>
                {
                    int pid;
                    if ((Native_Methods.GetWindowLong(h, Native_Const.GWL_STYLE) & Native_Const.WS_VISIBLE) == 0)
                        return true;

                    var tid = Native_Methods.GetWindowThreadProcessId(h, out pid);
                    if (pid == process.Id) {
                        handle = h;
                        return false;
                    }
                    return true;
                };



                Native_Methods.EnumWindows(proc, IntPtr.Zero);
            }


            return handle;

        }

        public static bool
        Is_User_Admin() {

            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            bool ret = wp.IsInRole(WindowsBuiltInRole.Administrator);
            return ret;

        }


        /*       Directories        */

        // ****************************

        // http://www.codekeep.net/snippets/170dc91f-1077-4c7f-ab05-8f82b9d1b682.aspx
        static public string
        Get_Asm_Title() {

            var asm = Assembly.GetExecutingAssembly();
            var attributes = asm.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

            if (attributes.Length > 0) {

                var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                if (titleAttribute.Title != "")
                    return titleAttribute.Title;
            }

            return Path.GetFileNameWithoutExtension(asm.CodeBase);


        }

        static public string
        Get_Exe_File() {
            return Path.GetFileName(Application.ExecutablePath);
        }

        /// <summary>
        /// without extension
        /// </summary>
        /// <returns></returns>
        static public string
        Get_Exe_Name() {
            return Path.GetFileNameWithoutExtension(Application.ExecutablePath);
        }


        static public string
        Get_Exe_Dir() {
            return Path.GetDirectoryName(Application.ExecutablePath);
        }

        public static string
        Get_Win_Dir() {

            var target = EnvironmentVariableTarget.Process;
            string ret = Environment.GetEnvironmentVariable("windir", target);

            return ret;

        }

        public static string
        Get_Sys_Dir() {

            var ret = Environment.GetFolderPath(Environment.SpecialFolder.System);

            return ret;
        }

        public static string
            Get_User_AppData_Dir() {

            string ret = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return ret;
        }

        public static string
        Get_Common_AppData_Dir() {

            string ret = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            return ret;

        }

        // ****************************


        public static int
        Regsvr_32(string dll, bool register, bool sysdir) {

            string path = dll;

            if (sysdir) {

                string tmp = Get_Sys_Dir();

                path = tmp.Cpath(path);

            }

            var arg1 = register ? "" : "/u";

            string reg_str = "{0} {1} /s".spf(arg1, path);

            var regsvr32 = new Process();

            regsvr32.StartInfo.FileName = "regsvr32";
            regsvr32.StartInfo.Arguments = reg_str;
            regsvr32.StartInfo.UseShellExecute = false;
            regsvr32.Start();

            regsvr32.WaitForExit();

            int ret = regsvr32.ExitCode;

            return ret;

        }

        public static T
        Deep_Copy<T>(T obj) where T : ISerializable {

            IFormatter formatter = new BinaryFormatter();
            T ret;
            using (var m_ss = new MemoryStream()) {

                formatter.Serialize(m_ss, obj);
                m_ss.Position = 0;
                ret = (T)formatter.Deserialize(m_ss);
            }

            return ret;
        }

        public static bool
        Compare_And_Swap<T>(T right, ref T left)
                           where T : IEquatable<T> {

            if (EqualityComparer<T>.Default.Equals(left, right))
                return false;

            left = right;

            return true;
        }



    }
}
