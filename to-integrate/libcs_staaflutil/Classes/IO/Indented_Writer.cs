using System;
using System.IO;

namespace Fairweather.Service
{
    public class Indent_Writer : Writer_Decorator
    {
        public Indent_Writer(TextWriter writer)
            : this(writer, def_tab_str) {

        }

        public Indent_Writer(TextWriter writer, string tab_str)
            : base(writer) {
            this.str = tab_str;
        }



        // ****************************


        public int Indent {
            get {
                return this.indent;
            }
            set {
                this.indent = Math.Max(value, 0);
            }
        }

        public string Indent_String {
            get {
                return this.str;
            }
        }


        // ****************************


        const string def_tab_str = "    ";//new string(' ', 4);

        int indent;
        readonly string str;

        static readonly Lazy_Dict<Pair<int, string>, string>
        string_cache = new Lazy_Dict<Pair<int, string>, string>(
            (pair) =>
            {

                var cnt = pair.First;
                var str = pair.Second;

                if (cnt == 0 || str.IsNullOrEmpty())
                    return "";

                int src_len = str.Length;
                int dest_len = cnt * src_len;

                var arr = new char[dest_len];

                for (int ii = 0; ii < dest_len; ii += src_len) {

                    str.CopyTo(0, arr, ii, src_len);

                }

                return new string(arr);
            });

        protected override void Line_Action() {

            writer.Write(string_cache[Pair.Make(indent, str)]);

        }
    }
}
