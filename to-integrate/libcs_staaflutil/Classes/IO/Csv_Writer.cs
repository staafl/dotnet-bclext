using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Fairweather.Service
{
    public class Csv_Writer : IDisposable
    {
        readonly TextWriter tr;

        public Csv_Writer(string file)
            : this(new FileStream(file, FileMode.Create, FileAccess.Write)) {

        }
        public Csv_Writer(Stream s)
            : this(new StreamWriter(s), false) {
        }

        public Csv_Writer(TextWriter tr, bool has_headers) {
            this.tr = tr;
        }

        public void Dispose() {
            tr.Dispose();
        }


        public void Write(IEnumerable<object> fields) {

            tr.WriteLine(fields.Select(_f => _f.strdef()).Csv());

        }



    }
}
