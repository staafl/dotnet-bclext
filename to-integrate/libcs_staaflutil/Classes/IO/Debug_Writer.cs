using System.Diagnostics;
using System.IO;
using System.Text;


namespace Fairweather.Service
{
    public class Debug_Writer : TextWriter
    {


        public Debug_Writer()
            : this(false) {
        }
        public Debug_Writer(bool trace) {
            this.Trace = trace;
        }


        public bool Trace { get; set; }


        public override void Write(char ch) {

            if (Trace)
                System.Diagnostics.Trace.Write(pool[ch]);
            else
                Debug.Write(pool[ch]);

        }


        public override void WriteLine(string value) {

            if (Trace)
                System.Diagnostics.Trace.WriteLine(value);
            else
                Debug.WriteLine(value);

        }

        public override void Write(string value) {

            if (Trace)
                System.Diagnostics.Trace.Write(value);
            else
                Debug.Write(value);

        }
        public override Encoding Encoding {
            get { return Encoding.Default; }
        }

        // ****************************


        static readonly Lazy_Dict<char, string>
        pool = new Lazy_Dict<char, string>(ch => ch.ToString());


    }
}
