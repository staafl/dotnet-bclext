using System;
using System.Diagnostics;

namespace Common
{
    [DebuggerStepThrough]
    public struct Invoice_Breakdown_Data : IBreakdown_Data
    {

        public Invoice_Breakdown_Data(int number,
                  string account_ref,
                  DateTime date,
                  Payment_Info info,
                  decimal total_owed,
                  decimal total_paid,
                  decimal discount,
                  decimal brute)
            : this() {

            this.Number = number;
            this.Account_Ref = account_ref;
            this.Date = date;
            this.Payment_Info = info;
            this.Total_Owed = total_owed;
            this.Total_Paid = total_paid;
            this.Discount = discount;
            this.Brute = brute;
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

        public Payment_Info Payment_Info {
            get;
            set;
        }

        public Decimal Payment_Amount(Payment_Type type) {
            return Payment_Info.Payment_Amount(type);
        }


        public decimal Total_Owed {
            get;
            set;
        }

        public decimal Total_Paid {
            get;
            set;
        }

        public decimal Discount {
            get;
            set;
        }

        public decimal Brute { get; set; }


    }
}