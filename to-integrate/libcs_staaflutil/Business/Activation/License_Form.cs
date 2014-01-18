using System.Drawing;

using Common;


namespace Activation
{
    public partial class License_Form : License_Form_Base
    {

        public License_Form(bool license_error)
            : base() {

            InitializeComponent();

            this.Text = Data.Activation_Title;


            if (license_error) {

                label1.Text = "Program license error.";
                label1.TextAlign = ContentAlignment.MiddleCenter;

            }


        }


    }


}
