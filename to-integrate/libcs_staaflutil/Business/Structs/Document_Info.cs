using System;
using System.Diagnostics;

namespace Common
{
    [DebuggerStepThrough]
    public struct Document_Info
    {
        public Document_Info(string account_ref,
            string user,
            decimal total_amount,
            int posted_number,
            DateTime date_posted,
            Document_Type document_type,
            bool settled)
            : this() {
            this.Account_Ref = account_ref;
            this.User = user;
            this.Total_Amount = total_amount;
            this.Posted_Number = posted_number;
            this.Date_Posted = date_posted;
            this.Document_Type = document_type;
            this.Settled = settled;
        }


        public string Account_Ref {
            get;
            set;

        }

        public string User {
            get;
            set;

        }

        public decimal Total_Amount {
            get;
            set;

        }

        public int Posted_Number {
            get;
            set;

        }

        public DateTime Date_Posted {
            get;
            set;

        }

        public Document_Type Document_Type {
            get;
            set;

        }

        public bool Settled {
            get;
            set;

        }






    }
}
