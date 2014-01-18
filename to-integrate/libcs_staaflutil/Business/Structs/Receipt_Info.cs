using System;

namespace Common
{
    [Serializable]
    public struct Receipt_Info
    {
        public Receipt_Info(string account,
                    string name,
                    Payment_Info payment_info,
                    decimal? old_bal,
                    decimal payment,
                    decimal? new_bal)
            : this() {

            Old_Balance = old_bal;
            Account_Ref = account;
            Name = name;
            Payment_Info = payment_info;
            Payment_Amount = payment;
            New_Balance = new_bal;
        }


        public decimal? Old_Balance {
            get;
            set;
        }


        public decimal Payment_Amount {
            get;
            set;
        }

        public decimal? New_Balance {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }

        public string Account_Ref {
            get;
            set;
        }

        public Payment_Info Payment_Info {
            get;
            set;
        }


    }
}
