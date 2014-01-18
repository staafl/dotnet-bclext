using System;
using System.Drawing;
using System.Windows.Forms;


namespace Common.Controls
{
    public class Our_Button_Flat : Our_Button
    {

        public Our_Button_Flat() {

            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.CheckedBackColor = Color.Red;
        }

        protected override void OnStyleChanged(EventArgs e) {
            base.OnStyleChanged(e);
        }
        protected override void On_Toggled_On(EventArgs e) {

            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 2;
            base.On_Toggled_On(e);
        }

        protected override void On_Toggled_Off(EventArgs e) {

            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 1;
            base.On_Toggled_Off(e);
        }
    }
}