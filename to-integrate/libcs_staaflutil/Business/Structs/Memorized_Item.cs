using System;
using System.Diagnostics;

namespace Common
{
    [Serializable]
    [DebuggerStepThrough]
    public struct Memorized_Item
    {


        public Memorized_Item(string code,
         string description,
         decimal unit_nv,
         decimal qty,
         decimal total_due,
         bool total_due_is_with_vat)
            : this() {

            this.Stock_Code = code;
            this.Description = description;
            this.Unit_NV = unit_nv;
            this.Qty = qty;
            this.Total_Due = total_due;
            Total_Due_Is_With_Vat = total_due_is_with_vat;
        }

        public Memorized_Item(string code,
                             string description,
                             decimal vatless_price,
                             decimal qty,
                             decimal total_due)
            : this(code, description, vatless_price, qty, total_due, true) {

        }


        public bool Total_Due_Is_With_Vat {
            get;
            set;
        }


        public string Stock_Code {
            get;
            set;

        }

        public string Description {
            get;
            set;
        }

        public decimal Unit_NV {
            get;
            set;
        }

        public decimal Qty {
            get;
            set;
        }

        public decimal Total_Due {
            get;
            set;
        }






    }
}
