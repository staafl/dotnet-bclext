using System;
using Fairweather.Service;

namespace Common
{
    [Serializable]
    public struct Grid_Info
    {

        public Grid_Info(decimal surcharge, 
                         decimal total_disc_v,
                         decimal brute_v,
                         decimal total_v,
                         decimal total_nv,
                         decimal vat)
            : this() {

            (surcharge >= 0.00m).tiff();

            Surcharge = surcharge;
            Brute_V = brute_v;
            Total_Disc_V = total_disc_v;
            Total_V = total_v;
            Total_NV = total_nv;
            VAT = vat;
        }

        public decimal Surcharge {
            get;
            set;

        }

        public decimal Brute_V {
            get;
            set;

        }

        public decimal Total_Disc_V {
            get;
            set;

        }

        public decimal Total_V {
            get;
            set;
        }

        public decimal Total_NV {
            get;
            set;

        }

        public decimal VAT {
            get;
            set;

        }

        // ****************************

        public decimal Adjusted_Brute_V {
            get {
                return Brute_V + Surcharge;
            }
        }

        public decimal Adjusted_Total_Disc_V {
            get {
                return Total_Disc_V + Surcharge;
            }
        }



    }
}
