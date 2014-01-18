using System;
using System.Diagnostics;

namespace Common
{
    [Serializable]
    [DebuggerStepThrough]
    public struct Posting_Info
    {

        public Posting_Info(Payment_Info payment_info,
                            Screen_Info screen_info,
                            Grid_Info grid_info,
            decimal outstanding)
            : this() {

            Payment_Info = payment_info;
            Screen_Info = screen_info;
            Totals_Info = grid_info;
            Outstanding = outstanding;
        }

        public decimal Outstanding {

            get;
            set;
        }


        public Grid_Info Totals_Info {
            get;
            set;
        }

        public Payment_Info Payment_Info {
            get;
            set;
        }

        public Screen_Info Screen_Info {
            get;
            set;
        }

    }
}
