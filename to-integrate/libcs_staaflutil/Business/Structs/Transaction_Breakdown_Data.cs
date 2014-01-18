using System;
using System.Diagnostics;

namespace Common
{
    [DebuggerStepThrough]
    public struct Transaction_Breakdown_Data : IBreakdown_Data
    {

        public Transaction_Breakdown_Data(int number,
                  string account_ref,
                  DateTime date,
                  string details,
                  Payment_Type type,
                  decimal net,
                  decimal tax)
            : this() {

            this.Number = number;
            this.Account_Ref = account_ref;
            this.Date = date;
            this.Details = details;

            this.Payment_Type = type;

            this.Net = net;
            this.Tax = tax;
        }


        public Decimal Payment_Amount(Payment_Type type) {
            if (type == this.Payment_Type)
                return Net + Tax;
            return 0.0m;
        }


        public int Number {
            get;
            set;
        }

        public string Account_Ref {
            get;
            set;
        }

        public DateTime Date {
            get;
            set;
        }

        public string Details { get; set; }

        public Payment_Type Payment_Type { get; set; }


        public decimal Tax { get; set; }
        public decimal Net { get; set; }


    }
}
