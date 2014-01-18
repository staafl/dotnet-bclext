
using System;
using System.IO;
using System.Windows.Forms;
using Fairweather.Service;
using Standardization;
namespace Common.Dialogs
{

    public partial class Company_Registration_Form : Form_Base
    {

        public Company_Registration_Form()
            : base(Form_Kind.Modal_Dialog) {

        }

        protected void SetEventHandlers() {

            this.but_browse.Click += this.but_browse_Click;

            this.tb_path.TextChanged += this.textBox_TextChanged;
            this.tb_path.Leave += this.tb_path_Leave;

            this.but_ok.Click += this.but_ok_Click;
            this.but_ok.Enter += this.control_Enter;

            this.but_cancel.Click += this.cancelB_Click;


            this.tb_user.TextChanged += this.textBox_TextChanged;
            this.tb_user.Enter += this.control_Enter;

            this.tb_pass1.TextChanged += this.textBox_TextChanged;
            this.tb_pass1.Enter += this.control_Enter;

            if (tb_pass2 != null) {
                this.tb_pass2.TextChanged += this.textBox_TextChanged;
                this.tb_pass2.Enter += this.control_Enter;
            }

        }



        //void Clear_Pass_TB() {
        //    tb_pass1.Text = tb_pass2.Text = "";
        //}

        void but_ok_Click(object sender, EventArgs e) {


            string pwd1 = tb_pass1.Text;

            if (tb_pass2 != null) {
                string pwd2 = tb_pass2.Text;

                if (pwd1 != pwd2) {

                    Standard.Show(Message_Type.Error, "Passwords do not match.");

                    return;
                }
            }

            Accept(pwd1);


        }

        protected virtual void
        Accept(string password) {
            throw new InvalidOperationException("Must override");
        }

        protected virtual void
        cancelB_Click(object sender, EventArgs e) {

            this.DialogResult = DialogResult.Cancel;
            Try_Close();

        }

        protected virtual void
        but_browse_Click(object sender, EventArgs e) {

            string txt = tb_path.Text.Trim();
            if (Directory.Exists(txt))
                folder_dialog.SelectedPath = txt;

            while (folder_dialog.ShowDialog(this) == DialogResult.OK) {

                this.Refresh(); // because of the dialog
                string path = folder_dialog.SelectedPath;

                if (Test_Path(path, true, true, true, true))
                    break;

            }
        }

        void textBox_TextChanged(object sender, EventArgs e) {

            Refresh_OK_Button();

        }

        protected void control_Enter(object sender, EventArgs e) {

            string path = tb_path.Text;

            if (!Test_Path(path, true, false, false, false)) {

                tb_path.Focus();
                tb_path.SelectAll();

                return;
            }

            var tb = sender as TextBox;

            if (tb == null)
                return;

            if (tb == tb_user) {
                if (tb_user.Text.IsNullOrEmpty())
                    tb_user.Text = Data.Default_User;

                tb_user.SelectAll();
            }


        }

        /// <summary>
        /// A place for deriving classes to hook into
        /// </summary>
        protected virtual bool
        Test_Path(string path, bool show_error) {

            return true;

        }

        bool
        Test_Path(string path,
                   bool show_error,
                   bool set_tb_path,
                   bool focus_tb_user,
                   bool set_default_user
            ) {


            if (path.IsNullOrEmpty()) {
                return false;
            }

            string path_tmp;
            if (!M.Find_Setup_Dta(path, out path_tmp)) {

                if (show_error)
                    Named_Message.The_Folder_You_Have_Selected.Show();

                return false;


            }
            path = path_tmp;


            if (!Test_Path(path, show_error))
                return false;


            if (set_tb_path)
                tb_path.Text = path;

            if (focus_tb_user)
                tb_user.Focus();

            if (tb_user.Text.IsNullOrEmpty() && set_default_user)
                tb_user.Text = Data.Default_User;

            return true;
        }

        void tb_path_Leave(object sender, System.EventArgs e) {

            Test_Path(tb_path.Text, false, true, true, true);

        }

        void Refresh_OK_Button() {

            bool ok = !(tb_user.Text.IsNullOrEmpty() ||
             tb_path.Text.IsNullOrEmpty() ||

             tb_pass1.Text.IsNullOrEmpty() ||
             (tb_pass2 != null && tb_pass2.Text.IsNullOrEmpty()));


            but_ok.Enabled = ok;

        }

        public Company_Registration_Result? Result {
            get;
            protected set;
        }

        #region controls

        protected TextBox tb_path;
        protected TextBox tb_pass1;
        protected TextBox tb_pass2;
        protected TextBox tb_user;
        protected Button but_browse;
        protected FolderBrowserDialog folder_dialog;

        protected Button but_cancel;
        protected Button but_ok;

        //public FolderBrowserDialog Fbd_Browser {
        //    get { return folder_dialog; }
        //}

        //public TextBox Tb_Path {
        //    get { return tb_path; }
        //}
        //public TextBox Tb_Pass1 {
        //    get { return tb_pass1; }
        //}
        //public TextBox Tb_Pass2 {
        //    get { return tb_pass2; }
        //}
        //public TextBox Tb_User {
        //    get { return tb_user; }
        //}
        //public Button But_Browse {
        //    get { return but_browse; }
        //}

        #endregion

    }

}