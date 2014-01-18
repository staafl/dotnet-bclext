using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTA;
using Fairweather.Service;
using Standardization;
using Common;
using System.Windows.Forms;
using System.ComponentModel;

namespace Common.Controls
{
    public abstract class DTA_Tab_Control_2 : DTA_Tab_Control
    {
        public override void Setup(Ini_File p_pman) {

            base.Setup(p_pman);

            helper = new Company_Helper(p_pman, Current_Company);

            Sage_Logic.Get(p_pman, helper.Credentials, out sdr).tiff();

            var form = this.FindForm();
            if (form != null)
                form.Closing += form_Closing;

        }


        protected Dictionary<TextBox, Pair<Record_Type, string>>
        validators;

        protected readonly Dictionary<TextBox, Func<bool>>
        validators2 = new Dictionary<TextBox, Func<bool>>();

        protected Sage_Logic sdr;
        protected Company_Helper helper;

        void form_Closing(object sender, CancelEventArgs e) {

            foreach (var tb in validators.Keys) {
                if (!Verify_Textbox(tb, false)) {
                    e.Cancel = true;
                    return;

                }
            }
        }

        protected abstract UserControl Tab_To_Revert_To();

        protected bool Verify_Textbox(TextBox tb, bool show_msg) {

            bool ret = true;

            Pair<Record_Type, string> pair;

            if (validators.TryGetValue(tb, out pair)) {

                var type = pair.First;
                var message = pair.Second;

                string text = tb.Text;

                if (text.IsNullOrEmpty()) {
                    var msg = "You need to enter a valid {0}.".spf(message);
                    if (show_msg)
                        Message_Type.Error.Show(msg);
                    ret = false;

                }
                else if (!sdr.Verify_Record_Exists(type, text)) {
                    var msg = "The selected {0} ({1}) does not exist in the data files."
                              .spf(message, text);
                    if (show_msg)
                        Message_Type.Error.Show(msg);
                    ret = false;

                }
            }

            if (ret) {
                var fun = validators2.Get_Or_Default_(tb, () => true);
                if (fun())
                    return true;
            }

            this.Activate_Tab(this.m_tabs.IndexOf(Tab_To_Revert_To()));

            tb.Select_Focus(true);

            return false;

        }
    }
}