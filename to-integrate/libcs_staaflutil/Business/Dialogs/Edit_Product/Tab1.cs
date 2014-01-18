using System.Windows.Forms;
using Common.Controls;
using System.Drawing;
using System;
using Fairweather.Service;
using Standardization;

namespace Common.Dialogs
{
    public partial class Product_Tab1 : UserControl
    {


        public Product_Tab1() {

            InitializeComponent();

            var dp6 = new Set<Control> { numericBox5, numericBox6 };

            foreach (var ctrl in this.All_Children()) {

                var as_Amount_Label = ctrl as Amount_Label;

                if (as_Amount_Label != null) {

                    as_Amount_Label.Decimal_Places = 
                        dp6[as_Amount_Label] ? 6 : 2;

                    as_Amount_Label.ForeColor = Color.Black;
                    as_Amount_Label.BackColor = Colors.TextBoxes.ReadOnlyBackGround;

                }


                var as_Text_Label = ctrl as Text_Label;

                if (as_Text_Label != null) {

                    as_Text_Label.ForeColor = Color.Black;
                    as_Text_Label.BackColor = Colors.TextBoxes.ReadOnlyBackGround;

                }


            }
        }

    }
}
