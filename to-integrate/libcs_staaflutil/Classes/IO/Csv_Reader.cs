using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fairweather.Service
{
    public class Csv_Reader : IDisposable
    {
        readonly Csv_Parser parser;


        public Csv_Reader(string file, bool has_headers)
            : this(new FileStream(file, FileMode.Open, FileAccess.Read), has_headers) {

        }

        public Csv_Reader(Stream s, bool has_headers)
            : this(new StreamReader(s), has_headers) {
        }

        public Csv_Reader(TextReader tr, bool has_headers) {
            parser = new Csv_Parser(tr);
            if (has_headers)
                headers = new List<string>();
        }

        public void Dispose() { parser.Dispose(); }

        bool Maybe_Read_Headers() {

            if (read_first)
                return true;

            read_first = true;

            if (headers == null)
                return true;

            var rec = parser.Get_Next_Record();

            if (rec == null)
                return false;

#warning todo - check headers consistency

            headers.AddRange(rec);

            return true;

        }

        public List<string> Read_List() {
            if (!Maybe_Read_Headers())
                return null;

            var rec = parser.Get_Next_Record();
            return rec;

        }

        public Insord<string, string> Read_Dict() {

            if (!Maybe_Read_Headers())
                return null;

            if (headers == null)
                headers.tifn();

            var rec = parser.Get_Next_Record();

            int cnt = Math.Min(rec.Count, headers.Count);

            var ret = new Insord<string, string>(cnt);

            for (int ii = 0; ii < cnt; ++ii) {

                ret.Add(headers[ii], rec[ii]);


            }

            return ret;

        }

        bool read_first;
        readonly List<string> headers;

        public Csv_Parser Parser { get { return parser; } }



    }
}
