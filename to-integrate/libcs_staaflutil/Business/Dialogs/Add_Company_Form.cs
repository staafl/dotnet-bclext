using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Activation;
using Common;
using Fairweather.Service;
using Standardization;


namespace Config_Gui
{
    public partial class Add_Company_Form : Common.Dialogs.Company_Registration_Form
    {
        readonly string sageusr_path;

        bool bf_folder_browser;
        readonly int m_version;

        readonly Set<string> rd_registered_paths;

        public Add_Company_Form(int version,
                              List<string> registered_paths) {

            InitializeComponent();

            rd_registered_paths = new Set<string>(from path in registered_paths
                                                  select path.ToUpper());

            m_version = version;

            Activation_Helpers.Find_sage_usr(version, out sageusr_path);

            SetEventHandlers();

            this.groupBox1.Align_Rights(this.but_cancel);
            this.but_ok.Location = but_cancel.Location.Translate(-but_cancel.Width - 4, 0);

        }


        protected override void
        OnFormClosing(FormClosingEventArgs e) {

            if (bf_folder_browser)
                e.Cancel = true;

            base.OnFormClosing(e);

        }


        protected override void
        but_browse_Click(object sender, EventArgs e) {

            // this flag is set to prevent the modal folder browser
            // from taking the whole dialog with itself when it's
            // closed
            bf_folder_browser = true;

            try {

                base.but_browse_Click(sender, e);

            }
            finally {

                this.Force_Handle();
                BeginInvoke((MethodInvoker)(() => bf_folder_browser = false));

            }
        }


        protected override void
        Accept(string password) {

            string path = tb_path.Text;
            string user = tb_user.Text;
            bool close;

            Company_Registration_Result? result;
            Activation_Helpers.Accept(
                               path,
                               user,
                               password,
                               m_version,
                               true,
                               true,
                               () => { throw new NotImplementedException(); },
                               _str => { },
                               _int => { throw new NotImplementedException(); },
                               out close, out result);

            if (result.HasValue) {

                Result = result.Value;
                Try_Close();

            }

        }

        protected override bool
        Test_Path(string path,
                  bool show_error) {

            if (rd_registered_paths[path.ToUpper()]) {

                if (show_error) {
                    var message = "The following company is already registered:\n " + path;

                    Standard.Show(Message_Type.Error, message);
                }

                return false;
            }

            return true;

        }

    }
}

/*       Old folder browse stuff - see scraps FLD_BRWS        */
