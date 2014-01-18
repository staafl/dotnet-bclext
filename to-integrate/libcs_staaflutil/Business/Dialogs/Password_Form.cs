using System;
using System.Windows.Forms;
using Activation;
using Common;
using Fairweather.Service;
using Standardization;

namespace Config_Gui
{
    public partial class Password_Form : Common.Dialogs.Form_Base
    {

        readonly string rd_username;
        readonly string rd_password;
        readonly string rd_path;

        readonly int rd_version;

        Pair<string>? m_result;

        public Pair<string>? Result {
            get {
                if (this.DialogResult != DialogResult.OK)
                    return null;

                var ret = m_result;
                return ret;
            }
        }

        protected override TextBox[] Flat_Text_Boxes {
            get {
                return new TextBox[] { tb_pass1, tb_pass2, tb_new_user };
            }
        }

        public Password_Form(string path, string username, string password, int version)
            : base(Form_Kind.Modal_Dialog) {

            InitializeComponent();

            lab_user.Text = username;
            tb_new_user.Text = username;

            rd_username = username;
            rd_path = path;
            rd_password = password;

            rd_version = version;

            this.AcceptButton = but_ok;
            this.CancelButton = but_cancel;

            Set_Event_Handlers();

        }


        void Set_Event_Handlers() {

            tb_new_user.TextChanged += tb_TextChanged;
            tb_pass1.TextChanged += tb_TextChanged;
            tb_pass2.TextChanged += tb_TextChanged;

        }


        void tb_TextChanged(object sender, EventArgs e) {

            Refresh_OK_Button();

        }
        /*       For the older version of TestUserCredentials which was used in this form,        */
        /*       see Scraps "PWDFORM-TESTUSER"        */


        void okB_Click(object sender, EventArgs e) {

            string pwd1 = tb_pass1.Text;
            string pwd2 = tb_pass2.Text;

            string user = tb_new_user.Text;

            string password;

            if (pwd1 != pwd2) {

                Standard.Show(Message_Type.Error, "The passwords do not match.");
                return;

            }
            else if (pwd1.IsNullOrEmpty()) {

                password = rd_password;

            }
            else {
                password = pwd1;
            }

            var status = Activation_Helpers.Check_User_Creds(rd_version,
                                                                rd_path,
                                                                user,
                                                                password);

            if (status == User_Credentials_Status.Incorrect_Credentials) {
                Standard.Show(Message_Type.Error, "Credentials not valid.");
                return;
            }
            else if (status == User_Credentials_Status.Unknown_Error) {
                Standard.Show(Message_Type.Error, "A connection to the data files could not be established.\n"
                                                        + "\n"
                                                        + "If you are using a network connecton please make sure\n"
                                                        + "it is available, and that the data are stored in the\n"
                                                        + "following folder:\n\nS"
                                                        + rd_path);

                /*       Old message reformatted to fit mbox        */

                /*                                        "A connection to the data files could not be established.\n"
                                                        + "\n"
                                                        + "If you are using a network connecton please make sure it is available,\n"
                                                        + "and that the data are stored in the following folder:\n"        */

                return;
            }

            m_result = new Pair<string>(user, password);
            this.DialogResult = DialogResult.OK;

            Try_Close();
        }

        void Refresh_OK_Button() {

            bool pwd1_empty = tb_pass1.Text.IsNullOrEmpty();
            bool pwd2_empty = tb_pass2.Text.IsNullOrEmpty();
            bool user_empty = tb_new_user.Text.IsNullOrEmpty();

            bool ok = !(user_empty || (pwd1_empty != pwd2_empty));

            but_ok.Enabled = ok;

        }

        //The Cancel button
        void button1_Click(object sender, EventArgs e) {
            Try_Close();
        }

    }
}
/*       The password hiding thing - see scraps "PASS"        */
