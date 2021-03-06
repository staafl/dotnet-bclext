using System;
using System.Diagnostics;

namespace Common
{
    [DebuggerStepThrough]
    public struct Expense_Info
    {


        public Expense_Info(string expense_type,
            string reference,
            string details,
            DateTime date,
            decimal net_amount,
            decimal tax_amount,
            Tax_Code tax_code)
            : this() {

            this.Expense_Type = expense_type;
            this.Reference = reference;
            this.Details = details;
            this.Date = date;
            this.Net_Amount = net_amount;
            this.Tax_Amount = tax_amount;
            this.Tax_Code = tax_code;
        }


        public string Expense_Type {
            get;
            set;
        }

        public string Reference {
            get;
            set;
        }

        public string Details {
            get;
            set;

        }

        public DateTime Date {
            get;
            set;

        }

        public decimal Net_Amount {
            get;
            set;

        }

        public decimal Tax_Amount {
            get;
            set;

        }

        public Tax_Code Tax_Code {
            get;
            set;
        }





    }
}
