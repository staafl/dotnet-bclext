using System;
using System.Windows.Forms;
using System.Diagnostics;
using Standardization;
using Fairweather.Service;

namespace Common.Controls
{
    public class Amount_Label : Label, IAmountControl
    {
        public Amount_Label() {
            ctrler = new Amount_Control_Controller(this);

            this.Flat_Style(false);

            this.Padding = new Padding(0, 2, 2, 0);

        }

        // C:\Users\Fairweather\Desktop\ctrler.pl
        readonly Amount_Control_Controller ctrler; //
        public Control Control { get { return this; } }
        public int Decimal_Places { get { return ctrler.DecimalPlaces; } set { ctrler.DecimalPlaces = value; } }
        public string Default_Text { get { return ctrler.Default_Text; } }
        public string Default_Format { get { return ctrler.Default_Format; } set { ctrler.Default_Format = value; } }

        public decimal Value { get { return ctrler.GetValue(); } set { ctrler.SetValue(value); } }

    }
}
