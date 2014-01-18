using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Versioning;

namespace Common
{

    [global::System.Serializable]
    abstract public class XSage : Exception
    {

        //For guidelines regarding the creation of new exception types, see
        //   http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        //and
        //   http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp


        public XSage() { }
        public XSage(string message) : base(message) { }
        public XSage(string message, Exception inner) : base(message, inner) { }
        protected XSage(SerializationInfo info, StreamingContext context)
            : base(info, context) { }


        public XSage(string message, Sage_Error error)
            : base(message) {

            this.Sage_Error = error;

        }

        public XSage(COMException comex, Sage_Error error)
            : this(comex.Message, comex, error) { }

        public XSage(string message, COMException comex, Sage_Error error)
            : base(message, comex) {

            this.Sage_Error = error;

        }


        public Sage_Error Sage_Error { get; set; }

        public override string ToString() {

            return base.ToString() + "\r\n" + "Error: " + Sage_Error.ToString();

        }

    }
}
