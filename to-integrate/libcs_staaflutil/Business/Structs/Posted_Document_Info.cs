using System;
using Versioning;

namespace Common
{
    public struct Posted_Document_Info
    {
        public Posted_Document_Info(decimal amount,
                            DateTime time_posted,
                            int post_number,
                            InvoiceType type)
            : this() {
            Amount = amount;
            Time_Posted = time_posted;
            Post_Number = post_number;
            Type = type;
        }

        public decimal Amount {
            get;
            set;
        }

        public DateTime Time_Posted {
            get;
            set;
        }

        public int Post_Number {
            get;
            set;
        }

        public bool Is_Credit {
            get {
                bool ret = Type == InvoiceType.sdoProductCredit;
                return ret;
            }
        }

        public InvoiceType Type {
            get;
            set;
        }




    }
}
