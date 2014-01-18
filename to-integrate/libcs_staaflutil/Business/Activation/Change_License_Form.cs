using System;

using Common;

using Fairweather.Service;
using Standardization;

namespace Activation
{
    public partial class Change_License_Form : License_Form_Base
    {
        public Change_License_Form() {

            InitializeComponent();

            tb_pin.Align_Lefts(but_ok);
            tb_pin.Align_Rights(but_cancel);

        }

        protected override void On_Activation_Succeeded(EventArgs e) {

            Named_Message.Program_Successfully_Activated.Show();

            base.On_Activation_Succeeded(e);
        }






    }
}
