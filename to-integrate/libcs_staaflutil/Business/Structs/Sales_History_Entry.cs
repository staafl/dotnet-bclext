using System;
using System.Collections.Generic;
using System.Linq;
using Fairweather.Service;
using Versioning;
using System.Diagnostics;
namespace Common
{
    // autogenerated: D:\Scripts\struct_creator2.pl
    [Serializable]
    [DebuggerStepThrough]
    public struct Sales_History_Entry
    {

        public Sales_History_Entry(
                    DateTime date,
                    int invoice_number,
                    bool posted,
                    decimal qty,

                    decimal discounted_price_vat,
                    decimal discounted_price_nvat,
                    decimal full_price_nvat,

                    decimal discount_perc,
                    bool is_credit_note)
            : this() {

            this.Date = date;
            this.Invoice_Number = invoice_number;
            this.Posted = posted;
            this.Qty = qty;

            this.Discounted_Price_Vat = discounted_price_vat;
            this.Discounted_Price_Nvat = discounted_price_nvat;
            this.Full_Price_Nvat = full_price_nvat;

            this.Discount_Perc = discount_perc;
            this.Is_Credit_Note = is_credit_note;
        }

        public decimal Full_Price_Nvat {
            get;
            set;

        }

        public bool Is_Credit_Note {
            get;
            set;
        }

        public DateTime Date {
            get;
            set;
        }

        public int Invoice_Number {
            get;
            set;
        }

        public bool Posted {
            get;
            set;
        }

        public decimal Qty {
            get;
            set;
        }

        public decimal Discounted_Price_Vat {
            get;
            set;
        }

        public decimal Discounted_Price_Nvat {
            get;
            set;
        }

        public decimal Discount_Perc {
            get;
            set;
        }




    }
}
