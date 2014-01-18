using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Fairweather.Service
{
    /// <summary>
    /// A simple reimplementation of some of the features of System.Diagnostiscs.Debug
    /// </summary>
    [DebuggerStepThrough]
    public static partial class D
    {


        /*       Static        */

        static D() {

            Date_Stamp = "[hh:mm:ss dd/MM/yyyy] ";
            Line_Break_Char = '*';
            Line_Break_Width = 40;
            Prefix = "";// ">>> ";

            main_writer =
            // prefix_writer =
            // new Prefix_Writer(
                indent_writer = new Indent_Writer(
                    multi_writer = new Multi_Writer(Console.Out)
                )
                // , => Prefix + DateTime.Now.ToString(Date_Stamp)
                // )
            ;


        }



        // ****************************

        //const string COND = "TRUE";

        static readonly TextWriter main_writer;
        static readonly Indent_Writer indent_writer;
        // static readonly Prefix_Writer prefix_writer;
        static readonly Multi_Writer multi_writer;

        static readonly string nl = Environment.NewLine;

        static void WriteInner(string str) {

            main_writer.Write(str);

            if (Auto_Flush)
                main_writer.Flush();


        }
        static void WriteLineInner(string str) {

            main_writer.WriteLine(str);

            if (Auto_Flush)
                main_writer.Flush();

        }


        // ****************************


        /*public*/ static void Add_Writer(TextWriter tw) {
            multi_writer.Writers.Add(tw);
        }

        public static char Line_Break_Char {
            get;
            set;
        }

        public static int Line_Break_Width {
            get;
            set;
        }

        public static string Date_Stamp {
            get;
            set;
        }

        public static string Prefix {
            get;
            set;

        }

        /*public*/
        static bool Muted {
            get;
            set;
        }

        public static bool Auto_Flush {
            get;
            set;
        }

        public static int Indent_Level {
            get { return indent_writer.Indent; }
            set {
                indent_writer.Indent = value;
            }
        }

        public static IDisposable Indent() {
            ++Indent_Level;
            return new On_Dispose(() => --Indent_Level);
        }


        static public readonly IDictionary<string, Action>
        Macros = new Dictionary<string, Action>();

        // ****************************


        //[Conditional(COND)]
        public static void w(object obj) {

            w(obj.StringOrDefault());

        }

        //[Conditional(COND)]
        public static void w(string str, params object[] strings) {

            if (Muted)
                return;

            WriteInner(str.Maybe_Format(strings));
        }

        //[Conditional(COND)]
        public static void wl() {
            wl("");
        }

        //[Conditional(COND)]
        public static void wl(object obj) {

            wl(obj.StringOrDefault());

        }

        //[Conditional(COND)]
        public static void wl(string str, params object[] strings) {

            if (Muted)
                return;

            WriteLineInner(str.Maybe_Format(strings));

        }

        public static IDisposable func() {
            return func((Func<object>)null);
        }
        public static IDisposable func(string name) {
            Func<object> return_value = null;
            return func(name, return_value);
        }
        public static IDisposable func(Func<object> return_value) {
            string name = D.Get_Caller(1).Name;
            return func(name, return_value);
        }
        public static IDisposable func(string name,
                                       Func<object> return_value) {
            // {{ is escaped {
            // http://geekswithblogs.net/jonasb/archive/2007/03/05/108023.aspx
            wl("{0} {{", name);
            var indent = Indent();
            var ret = new On_Dispose(() =>
            {
                try {
                    using (indent) {
                        if (return_value == null)
                            return;
                        object val = return_value();
                        Type type = val == null ? null : val.GetType();
                        val = val ?? (object)"null";
                        wl("*** Return value: {0}:{1}", val, type);
                    }
                }
                finally {
                    wl("}");
                }
            });

            return ret;

        }


        // ****************************

        //[Conditional(COND)]
        public static void lbr() {
            lbr(Line_Break_Char);
        }

        //[Conditional(COND)]
        public static void lbr(char ch) {
            wl();
            l(ch);
            wl();
        }

        //[Conditional(COND)]
        public static void l() {
            l(Line_Break_Char);
        }

        //[Conditional(COND)]
        public static void l(char ch) {

            WriteLineInner(new string(ch, Line_Break_Width));

        }


        // ****************************


        //[Conditional(COND)]
        public static void mac(string name) {

            if (Muted)
                return;

            Action act;

            if (!Macros.TryGetValue(name, out act)) {
                wl("macro {0} not found", name);
                return;
            }

            act();

        }

        //[Conditional(COND)]
        public static void rk() {

            if (Muted)
                return;

            try {
                Console.ReadKey();
            }
            catch (System.InvalidOperationException) { }
        }

        //[Conditional(COND)]
        public static void fl() {

            if (Muted)
                return;

            main_writer.Flush();
        }

        //[Conditional(COND)]
        public static void br() {

            if (Muted)
                return;

            Debugger.Break();

        }

    }
}