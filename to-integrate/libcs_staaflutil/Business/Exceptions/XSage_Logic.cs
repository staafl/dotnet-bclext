using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Fairweather.Service;
using Versioning;

namespace Common
{
    [DebuggerStepThrough]
    public class XSage_Logic : XSage
    {
        static string Get_Default_Message() {
            var sdr = Common.Data.SDR;
            if (sdr != null) {
                var sdo = sdr.Sdo;
                if (sdo != null)
                    return sdo.Last_Error_Text;
            }
            return "";
        }
        public XSage_Logic()
            : this(Get_Default_Message()) {
        }

        public XSage_Logic(string message)
            : base(message) {
        }

        public XSage_Logic(string message, Exception inner)
            : base(message, inner) {
        }


        //public XSage_Logic(string message, int? lastTransaction)
        //    : base(message) {

        //    Last_Transaction = lastTransaction;
        //}

        //public XSage_Logic(string message, Sage_Error error)
        //    : base(message, error) {

        //}

        public XSage_Logic(string message, int? lastTransaction, Sage_Error error)
            : base(message, error) {

            Last_Transaction = lastTransaction;

        }

        //public XSage_Logic(COMException comex, Sage_Error error)
        //    : base(comex, error) {
        //}

        public XSage_Logic(COMException comex, int? lastTransaction, Sage_Error error)
            : base(comex, error) {

            Last_Transaction = lastTransaction;
        }




        public int? Last_Transaction {
            get;
            set;
        }

        public override string ToString() {

            var ret = base.ToString() + "\r\n" + "Last Transaction Number: " + Last_Transaction.StringOrDefault("null");

            return ret;

        }
    }
}
