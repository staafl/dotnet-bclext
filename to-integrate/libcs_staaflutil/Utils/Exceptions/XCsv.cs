using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fairweather.Service
{
    [global::System.Serializable]
    public class XCsv : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public int Line { get; private set; }
        public int Column { get; private set; }
        public int Record { get; private set; }
        public bool Fatal { get; private set; }
        public Csv_Error_Type Error_Type { get; private set; }

        static Dictionary<Csv_Error_Type, string>
        messages = new Dictionary<Csv_Error_Type, string>
		        {
		            {Csv_Error_Type.Trailing_Chars, "Non-whitespace characters after the closing quote."},
		            {Csv_Error_Type.Unexpected_Quote, "Quote found in unquoted field."},
		            {Csv_Error_Type.Trailing_Quote, "Trailing quote after quoted field."},
		        };

        static Set<Csv_Error_Type> fatal = new Set<Csv_Error_Type> { Csv_Error_Type.Runaway_Quote };

        public XCsv() { }
        public XCsv(string message)
            : base(message) {
        }
        public XCsv(Csv_Error_Type error, int line, int col, int rec)
            : this(error, fatal[error], line, col, rec) {
        }
        public XCsv(string msg, Csv_Error_Type error, bool fatal, int line, int col, int rec)
            : base(msg.IsNullOrEmpty() ? messages.Get_Or_Default_(error, "A CSV error occurred.") : msg) {
            this.Fatal = fatal;
            this.Line = line;
            this.Record = rec;
            this.Column = col;
            this.Error_Type = error;
        }

        public XCsv(Csv_Error_Type error, bool fatal, int line, int col, int rec)
            : this(null, error, fatal, line, col, rec) {

        }


        public XCsv(string message, Exception inner) : base(message, inner) { }
        protected XCsv(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
