using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
namespace Fairweather.Service
{
    /* Various helper methods */
    static partial class Extensions
    {
        // http://stackoverflow.com/questions/271398/post-your-extension-goodies-for-c-net-codeplex-com-extensionoverflow?answer=274652#274652
        public static bool 
        In<T>(this T what, params T[] where) {
            if (null == what) throw new ArgumentNullException("source");
            return where.Contains(what);
        }


        static public string 
        ToWords(this decimal amount, int decimal_places, string currency, string small) {

            var as_str = amount.ToString("F" + decimal_places);

            currency = currency ?? "";
            small = small ?? "";

            if (small.ToUpper() == "CENTS")
                small = "Cent";

            string ret = "";
            string before_dot;
            string after_dot;

            if (as_str.Contains('.')) {

                var split = as_str.Split('.');

                before_dot = split[0].ToInt64().ToWords();

                after_dot = split[1].ToInt64().ToWords();

            }
            else {

                before_dot = as_str.ToInt64().ToWords();

                after_dot = null;

            }

            if (!before_dot.IsNullOrEmpty()) 
                ret = Combine_Amount_Unit(before_dot, currency);

            if (!after_dot.IsNullOrEmpty()) {

                if (!ret.IsNullOrEmpty())
                    ret += " and ";

                ret += Combine_Amount_Unit(after_dot, small);
            }



            return ret;
        }

        static string Combine_Amount_Unit(string amount, string unit) {

            if (amount.IsNullOrEmpty())
                return "";

            if (amount.ToUpper() == "ONE")
                unit = unit.TrimEnd('s');

            return amount + " " + unit;
        }

        static string 
        ToWords(this long amount) {
            bool and_bfr_units = false;
            bool and_bfr_last_elem = false;
            return amount.ToWords(and_bfr_units, and_bfr_last_elem);
        }

        static string
        ToWords(this long amount,
                bool and_bfr_units,
                bool and_bfr_last_elem) {

            if (amount == 0)
                return "";

            var magnitude = Math.Log(amount, 1000).ToInt32();
            string ret = "";

            // 123,456,789 --> [123,000,000 , 456,000 , 789]
            for (var ii = magnitude; ii >= 0; --ii) {

                var thousands = Math.Pow(1000, ii).ToInt64();  // 1,000,000 || 1,000 || 1
                var t = Math.Floor((decimal)(amount / thousands)).ToInt64() % 1000; //  123 || 456 || 789
                var t1 = ((t / 100) % 10).ToInt32(); // 1 || 4 || 7 
                var t2 = ((t / 10) % 10).ToInt32();  // 2 || 5 || 8
                var t3 = (t % 10).ToInt32();         // 3 || 6 || 9

                if (t1 + t2 + t3 == 0)
                    continue;

                if (ii == 0 && and_bfr_last_elem) {

                    // last iteration
                    ret = ret.TrimEnd(',', ' ');
                    ret += " and ";

                }

                if (t1 > 0)
                    ret += numbers[t1] + " Hundred ";


                if (t2 > 0) {
                    string temp;
                    if (numbers.TryGetValue(t2 * 10 + t3, out temp)) {
                        // eleven, twelve, ... , nineteen
                        ret += temp;
                        t3 = 0;
                    }
                    else {
                        ret += numbers[t2 * 10];
                    }

                }
                else if (and_bfr_units) {

                    if (t1 > 0 && t3 > 0)
                        // "One Hundred and "
                        ret += " and ";


                }

                if (t3 > 0)
                    ret = ret.TrimEnd(' ') + " " + numbers[t3];

                // thousand, million, etc
                ret += " " + words[ii] + " ";



            }
            ret = ret.Trim();
            return ret;

        }


        #region data for GetWords && GetWordsFromAmount - 40 lines

        static string[] words =
new string[] { "", "Thousand", "Million", "Billion", "Trillion", "Quadrillion" };

        static Dictionary<int, string> numbers = new Dictionary<int, string>
        { 
#region 8 lines
		            {0, ""}, {1, "One"}, {2, "Two"}, {3, "Three"}, 
            {4, "Four"}, {5, "Five"}, {6, "Six"}, {7, "Seven"}, 
            {8, "Eight"}, {9, "Nine"}, {10, "Ten"}, {11, "Eleven"}, 
            {12, "Twelve"}, {13, "Thirteen"}, {14, "Fourteen"}, 
            {15, "Fifteen"}, {16, "Sixteen"}, {17, "Seventeen"}, 
            {18, "Eighteen"}, {19, "Nineteen"}, {20, "Twenty"}, 
            {30, "Thirty"}, {40, "Forty"}, {50, "Fifty"}, {60, "Sixty"}, 
            {70, "Seventy"}, {80, "Eighty"}, {90, "Ninety"} 
	#endregion
        };


        #endregion


        public static void Raise(this Action act) {
            if (act != null)
                act();
        }
        public static void Raise<T>(this Action<T> act, T arg1) {
            if (act != null)
                act(arg1);
        }
        public static void Raise<T1, T2>(this Action<T1, T2> act, T1 arg1, T2 arg2) {
            if (act != null)
                act(arg1, arg2);
        }
        public static void
        Line_Break(this TextWriter tw) {
            Line_Break(tw, '=');
        }

        public static void
        Line_Break(this TextWriter tw, char ch) {
            Line_Break(tw, ch, 40);
        }

        public static void
        Line_Break(this TextWriter tw, char ch, int len) {
            tw.WriteLine(new string(ch, len));
        }

        public static IDisposable
        AcquireReaderLock(this ReaderWriterLock rwl) {
            rwl.AcquireReaderLock(-1);
            return new On_Dispose(() => rwl.ReleaseReaderLock());
        }
        public static IDisposable
        AcquireWriterLock(this ReaderWriterLock rwl) {
            rwl.AcquireReaderLock(-1);
            return new On_Dispose(() => rwl.ReleaseWriterLock());
        }

        public static IDisposable
        Try_Lock(this object obj, int timeout, out bool ok) {

            ok = Monitor.TryEnter(obj, timeout);

            return ok ? new On_Dispose(() => Monitor.Exit(obj))
                      : On_Dispose.Nothing;


        }

        static public string Remove_System_Prefix(this Type type) {

            //string declared_type = "#" + type.Name;
            //declared_type = declared_type.Replace("#System.", "#");
            //declared_type = declared_type.Substring(1);

            string declared_type = type.Name;
            declared_type = Regex.Replace(declared_type, "^System.", "");

            return declared_type;

        }

        static public string Normalize_Name(this Type type) {

            if (type == null)
                return "NULL";

            string name = Remove_System_Prefix(type);

            if (!type.IsGenericType)
                return name;

            int index = name.IndexOf('`');

            if (index != -1) // everything before the backtick
                name = name.Substring(0, index);

            name += "<";
            foreach (Type t in type.GetGenericArguments())
                name += Normalize_Name(t) + ", ";
            name = name.TrimEnd(' ', ',');
            name += ">";

            return name;

        }

        static public bool
        Maybe_Tell(this ITellAsk obj, string txt) {

            if (obj == null)
                return false;

            obj.Tell(txt);
            return true;
        }

        static public bool
        Maybe_Ask(this ITellAsk obj, string txt, bool def) {

            if (obj == null)
                return def;

            return obj.Ask(txt);
        }

        static public bool
        Maybe_Tell<T>(this ITellAsk<T> obj, string txt, T data) {

            if (obj == null)
                return false;

            obj.Tell(txt, data);
            return true;
        }

        static public bool
        Maybe_Ask<T>(this ITellAsk<T> obj, string txt, T data, bool def) {

            if (obj == null)
                return def;

            return obj.Ask(txt, data);
        }

        static public IEnumerable<FileInfo>
        Get_Files(this DirectoryInfo dir, string rx, FileAttributes attribs) {



            return Get_Children(dir, true, rx, attribs).Cast<FileInfo>();


        }

        static public IEnumerable<DirectoryInfo>
        Get_Dirs(this DirectoryInfo dir, string rx, FileAttributes attribs) {



            return Get_Children(dir, false, rx, attribs).Cast<DirectoryInfo>();


        }

        public static IEnumerable<FileSystemInfo>
        Get_Children(DirectoryInfo dir, bool files, string rx, FileAttributes attribs) {

            var infos = files ? (FileSystemInfo[])dir.GetFiles()
                              : (FileSystemInfo[])dir.GetDirectories();

            var rx_obj = new Regex(rx, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var ret = from info in infos
                      where rx_obj.IsMatch(info.Name)
                      select info;

            if (attribs != 0)
                ret = from info in ret
                      where info.Attributes.Contains(attribs)
                      select info;

            return ret;
        }




        // ****************************


        public static bool
        Generic_Equals<T>(this T first, T second) {

            return EqualityComparer<T>.Default.Equals(first, second);

        }

        public static bool
        Safe_Equals<T>(this T first, T second) {

            if (first == null) {
                return second == null;
            }

            return first.Equals(second);

        }

        // ****************************


        public static bool
        Is<TType>(this Object obj) {
            return obj is TType;
        }

        public static bool
        IsNullOrEmpty<T>(this T[] arr) {

            bool ret = false;

            if (arr == null)
                ret = true;

            else if (arr.Is_Empty())
                ret = true;

            return ret;
        }

        public static bool
        IsNull(this object obj) {

            bool ret = false;

            if (obj == null)
                ret = true;

            return ret;
        }

        public static bool
        IsNullOrEmpty(this object obj) {

            bool ret = false;

            if (obj == null)
                ret = true;
            else if (obj.ToString().IsNullOrEmpty())
                ret = true;

            return ret;
        }


        // ****************************


        public static bool
        Compare_Secure_String(this SecureString sstring, string string2) {

            bool ret = false;
            int len = string2.Length;
            do {
                if (sstring.Length != len)
                    break;

                var pass_1 = Marshal.SecureStringToBSTR(sstring);
                var pass_2 = Marshal.StringToBSTR(string2);

                try {
                    ret = pass_1.Compare_Unmanaged_Memory(pass_2);
                }
                finally {
                    Marshal.ZeroFreeBSTR(pass_1);
                    Marshal.ZeroFreeBSTR(pass_2);
                }
            } while (false);

            return ret;
        }

        public static bool
        Compare_Secure_String(this SecureString sstring1, SecureString sstring2) {

            bool ret = false;

            do {
                if (sstring1.Length != sstring2.Length)
                    break;

                var pass_1 = Marshal.SecureStringToBSTR(sstring1);
                var pass_2 = Marshal.SecureStringToBSTR(sstring2);

                try {
                    ret = pass_1.Compare_Unmanaged_Memory(pass_2);
                }
                finally {
                    Marshal.ZeroFreeBSTR(pass_1);
                    Marshal.ZeroFreeBSTR(pass_2);
                }
            } while (false);

            return ret;

        }


        public static bool
        Compare_Unmanaged_Memory(this IntPtr ptr1, IntPtr ptr2) {

            var len1 = Marshal.SizeOf(ptr1);
            var len2 = Marshal.SizeOf(ptr2);

            bool ret = false;
            do {
                if (len1 != len2)
                    break;

                ret = true;
                for (int ii = 0; ii < len1; ++ii) {

                    if (Marshal.ReadByte(ptr1, ii) != Marshal.ReadByte(ptr2, ii)) {
                        ret = false;
                        break;
                    }
                }
            } while (false);

            return ret;
        }


        // ****************************



        public static bool
        False(this bool b1) {

            bool ret = !b1;

            return ret;
        }

        public static bool
        XOR(this bool b1, bool b2) {

            bool ret = (b1 || b2) && !(b1 && b2);

            return ret;
        }

        public static void
        Raise<TArgs>(this Delegate eh, object sender, TArgs e)
            where TArgs : EventArgs {

            if (eh == null)
                return;

            var tmp = _Delegates.Cast<EventHandler<TArgs>>(eh);

            tmp.Raise(sender, e);


        }

        public static void
        Raise(this EventHandler eh, object sender) {

            if (eh != null)
                eh(sender, EventArgs.Empty);

        }

        public static void
        Raise(this EventHandler<EventArgs> eh, object sender) {

            if (eh != null)
                eh(sender, EventArgs.Empty);

        }

        public static void
        Raise<T>(this EventHandler<T> eh, object sender, T e)
            where T : EventArgs {

            if (eh != null)
                eh(sender, e);

        }


        // ****************************


        public static void
        Raise<T>(this Handler<T> eh, object sender, Args<T> e) {

            if (eh != null)
                eh(sender, e);

        }

        public static void
        Raise<TMut, TImm>(this Handler<TMut, TImm> eh, object sender, Args<TMut, TImm> e) {

            if (eh != null)
                eh(sender, e);

        }

        public static void
        Raise<T>(this Handler_RO<T> eh, object sender, Args_RO<T> e) {

            if (eh != null)
                eh(sender, e);

        }


        // ****************************




        public static bool
        Try_Dispose(this IDisposable disp) {

            if (disp == null)
                return false;

            disp.Dispose();
            return true;

        }

        public static bool
        Try_Dispose(this object obj) {

            var cast = obj as IDisposable;
            if (cast == null)
                return false;

            cast.Dispose();
            return true;

        }


        // ************* ENUM MANIPULATION **************
        // **********************************************

        public static bool
        Is_Defined(this Enum value) {
            return Enum.IsDefined(value.GetType(), value);
        }

        public static bool
        Is_0(this Enum value) {
            return Convert.ToInt32(value) == 0;
        }

        public static bool
        Is_Single(this Enum value) {
            var ret = B.Hamming_Weight(Convert.ToInt32(value)) == 1;
            return ret;
        }

        public static string
        Get_String(this Enum value) {

            string ret = Enum.GetName(value.GetType(), value);
            return ret;

        }

        /*       Untested        */

        public static bool
        Contains(this Enum _enum, int value) {

            (Enum.GetUnderlyingType(_enum.GetType()) == typeof(int)).tiff();

            var _enum_as_int = Convert.ToInt32(_enum);
            bool ret = B.Contains(_enum_as_int, value);

            return ret;

        }

        /*       Untested        */

        public static bool
        Contains(this Enum _enum1, Enum _enum2) {

            (_enum1.GetType() != _enum2.GetType()).tift();

            bool ret = B.Contains(Convert.ToInt32(_enum1),
                                                 Convert.ToInt32(_enum2));

            return ret;

        }



        public static IEnumerable<TEnum>
        Get_Contained_Values<TEnum>(this TEnum flags)
            where TEnum : struct {

            var possible_values = Enum.GetValues(typeof(TEnum));
            var cast = Convert.ToInt32(flags);

            foreach (TEnum val in possible_values)
                if (B.Contains(cast, Convert.ToInt32(val)))
                    yield return val;

        }




    }
}
