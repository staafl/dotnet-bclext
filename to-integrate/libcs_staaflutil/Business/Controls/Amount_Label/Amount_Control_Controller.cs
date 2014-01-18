using System;
using System.Windows.Forms;
using System.Diagnostics;
using Standardization;
using Fairweather.Service;

namespace Common.Controls
{
    class Amount_Control_Controller
    {
        readonly IAmountControl ctrl;

        public Amount_Control_Controller(IAmountControl ctrl) {
            this.ctrl = ctrl;
            this.DecimalPlaces = 2;
        }

        public int DecimalPlaces {
            get {
                if (places == -1)
                    return 2;

                return places;
            }
            set {
                if (value < 0)
                    throw new ArgumentException("Decimal places must be a nonnegative integer.");

                if (places == value)
                    return;

                places = value;

                Default_Format = "F" + places.ToString();

                Text = Default_Text;

            }
        }

        string Text {
            get {
                return ctrl.Control.Text;
            }
            set {
                ctrl.Control.Text = value;
            }
        }


        public bool SetValue(decimal value) {
            var text = value.ToString(Default_Format);
            var prev = this.GetValue();
            if (this.Text == text)
                return false;
            this.Text = text;
            if (value == prev)
                return false;
            return true;
        }

        public decimal GetValue() {
            return Text.DecimalOrZero(DecimalPlaces);
        }

        public string Default_Text {
            get {
                var ret = 0.0m.ToString(Default_Format);
                return ret;
            }
        }




        public string Default_Format {
            get;
            set;
        }


        // ****************************

        int places = -1;


    }
}
