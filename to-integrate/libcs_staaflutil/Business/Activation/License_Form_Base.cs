using System;
using System.Windows.Forms;
using Common;
using Fairweather.Service;
using Standardization;

namespace Activation
{
    public class License_Form_Base : Common.Dialogs.Form_Base
    {
        protected License_Form_Base()
            : base(Form_Kind.Modal_Dialog) { }

        protected TextBox tb_pin;
        protected TextBox reqTB;
        protected TextBox tb_activation;
        protected Button but_ok;
        protected Button but_cancel;

        protected override void OnLoad(EventArgs e) {

            base.OnLoad(e);

            reqTB.Text = Activation_Data.Generate_Request(true);

            SetEventHandlers();
        }

        void SetEventHandlers() {

            tb_activation.TextChanged += tb_TextChanged;
            tb_pin.TextChanged += tb_TextChanged;

            H.Select_On_Enter(tb_activation, tb_pin, reqTB);

            //reqTB.DoubleClick += (_1, _2) => reqTB.SelectAll();

            this.but_ok.Click += okBut_Click;
            this.but_cancel.Click += but_cancel_Click;


        }



        void tb_TextChanged(object sender, EventArgs e) {

            Refresh_OK_Button();
        }

        void Refresh_OK_Button() {

            bool ok = !(tb_activation.Text.IsNullOrEmpty() || tb_pin.Text.IsNullOrEmpty());

            but_ok.Enabled = ok;
        }


        void okBut_Click(object sender, EventArgs e) {

            Try_Activate(true);

        }

        void but_cancel_Click(object sender, System.EventArgs e) {
            this.m_result = null;
            base.Cancel_Clicked();
        }

        protected virtual void
        On_Activation_Succeeded(EventArgs e) {

        }

        void Try_Activate(bool Shows) {

            string key = tb_activation.Text;
            string pin = tb_pin.Text;

            var null_license_result = License_Result.Try_Get(key, pin);

            if (!null_license_result.HasValue) {

                if (Shows) {

                    Named_Message.The_License_Information_You_Have_Entered_Is_Not_Valid.Show();

                    return;
                }

            }

            On_Activation_Succeeded(EventArgs.Empty);

            m_result = null_license_result.Value;

            this.DialogResult = DialogResult.OK;
            Try_Close();


        }


        protected override TextBox[]
        Flat_Text_Boxes {
            get {
                return new[] { reqTB, tb_activation, tb_pin };
            }
        }

        License_Result? m_result;

        public License_Result? Result {
            get {
                var ret = m_result;
                return ret;
            }
        }

    }
}
