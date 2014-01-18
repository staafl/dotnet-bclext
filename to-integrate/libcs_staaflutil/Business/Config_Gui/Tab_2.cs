
using System.Windows.Forms;

using Common.Controls;

namespace Config_Gui
{
    public partial class Tab_2 : UserControl
    {
        public Tab_2() {

            InitializeComponent();

            //Standard.Flat_Style(cb_def_company);

            //Standard.Make_Readonly_Label(this.lab_ver);

        }

        private Label label4;



        #region designer

        System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        void InitializeComponent() {
            this.rb_auth_src_internal = new System.Windows.Forms.RadioButton();
            this.rb_auth_src_prompt = new System.Windows.Forms.RadioButton();
            this.but_upgrade = new System.Windows.Forms.Button();
            this.cb_def_company = new System.Windows.Forms.ComboBox();
            this.chb_debug = new Common.Controls.Flat_Check_Box();
            this.lab_ver = new Text_Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel_cred_src = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chb_allow_other_companies = new Common.Controls.Flat_Check_Box();
            this.label4 = new System.Windows.Forms.Label();
            this.chb_reset_of_active_users = new Common.Controls.Flat_Check_Box();
            this.panel_cred_src.SuspendLayout();
            this.SuspendLayout();
            // 
            // rb_auth_src_internal
            // 
            this.rb_auth_src_internal.AutoSize = true;
            this.rb_auth_src_internal.Location = new System.Drawing.Point(16, 16);
            this.rb_auth_src_internal.Name = "rb_auth_src_internal";
            this.rb_auth_src_internal.Size = new System.Drawing.Size(125, 17);
            this.rb_auth_src_internal.TabIndex = 24;
            this.rb_auth_src_internal.Text = "Internal Configuration";
            this.rb_auth_src_internal.UseVisualStyleBackColor = true;
            // 
            // rb_auth_src_prompt
            // 
            this.rb_auth_src_prompt.AutoSize = true;
            this.rb_auth_src_prompt.Location = new System.Drawing.Point(16, 35);
            this.rb_auth_src_prompt.Name = "rb_auth_src_prompt";
            this.rb_auth_src_prompt.Size = new System.Drawing.Size(178, 17);
            this.rb_auth_src_prompt.TabIndex = 25;
            this.rb_auth_src_prompt.Text = "At Run-Time / Command Prompt";
            this.rb_auth_src_prompt.UseVisualStyleBackColor = true;
            // 
            // but_upgrade
            // 
            this.but_upgrade.Location = new System.Drawing.Point(7, 217);
            this.but_upgrade.Name = "but_upgrade";
            this.but_upgrade.Size = new System.Drawing.Size(160, 23);
            this.but_upgrade.TabIndex = 31;
            this.but_upgrade.Text = "Upgrade Sage 50 Accounts";
            this.but_upgrade.UseVisualStyleBackColor = true;
            // 
            // cb_def_company
            // 
            this.cb_def_company.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_def_company.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_def_company.FormatString = "{s:10}";
            this.cb_def_company.FormattingEnabled = true;
            this.cb_def_company.Location = new System.Drawing.Point(7, 141);
            this.cb_def_company.Name = "cb_def_company";
            this.cb_def_company.Size = new System.Drawing.Size(302, 21);
            this.cb_def_company.TabIndex = 26;
            // 
            // chb_debug
            // 
            this.chb_debug.AutoSize = true;
            this.chb_debug.BackColor = System.Drawing.Color.White;
            this.chb_debug.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_debug.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_debug.Location = new System.Drawing.Point(300, 316);
            this.chb_debug.Name = "chb_debug";
            this.chb_debug.Size = new System.Drawing.Size(12, 11);
            this.chb_debug.TabIndex = 27;
            this.chb_debug.TabStop = false;
            this.chb_debug.UseVisualStyleBackColor = true;
            this.chb_debug.Visible = false;
            // 
            // lab_ver
            // 
            this.lab_ver.Location = new System.Drawing.Point(7, 28);
            this.lab_ver.Name = "lab_ver";
            this.lab_ver.Size = new System.Drawing.Size(54, 20);
            this.lab_ver.TabIndex = 23;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(99, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "Credentials Source:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(4, 123);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(91, 13);
            this.label17.TabIndex = 30;
            this.label17.Text = "Default Company:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Sage Version:";
            // 
            // panel_cred_src
            // 
            this.panel_cred_src.Controls.Add(this.label12);
            this.panel_cred_src.Controls.Add(this.rb_auth_src_prompt);
            this.panel_cred_src.Controls.Add(this.rb_auth_src_internal);
            this.panel_cred_src.Location = new System.Drawing.Point(-9, 62);
            this.panel_cred_src.Name = "panel_cred_src";
            this.panel_cred_src.Size = new System.Drawing.Size(244, 61);
            this.panel_cred_src.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "Allow Users to select other Companies:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 314);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Debug Mode:";
            this.label2.Visible = false;
            // 
            // chb_allow_other_companies
            // 
            this.chb_allow_other_companies.AutoSize = true;
            this.chb_allow_other_companies.BackColor = System.Drawing.Color.White;
            this.chb_allow_other_companies.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_allow_other_companies.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_allow_other_companies.Location = new System.Drawing.Point(250, 172);
            this.chb_allow_other_companies.Name = "chb_allow_other_companies";
            this.chb_allow_other_companies.Size = new System.Drawing.Size(12, 11);
            this.chb_allow_other_companies.TabIndex = 27;
            this.chb_allow_other_companies.TabStop = false;
            this.chb_allow_other_companies.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 192);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(169, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "Allow Reset of Active Sage Users:";
            // 
            // chb_reset_of_active_users
            // 
            this.chb_reset_of_active_users.AutoSize = true;
            this.chb_reset_of_active_users.BackColor = System.Drawing.Color.White;
            this.chb_reset_of_active_users.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_reset_of_active_users.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_reset_of_active_users.Location = new System.Drawing.Point(250, 194);
            this.chb_reset_of_active_users.Name = "chb_reset_of_active_users";
            this.chb_reset_of_active_users.Size = new System.Drawing.Size(12, 11);
            this.chb_reset_of_active_users.TabIndex = 27;
            this.chb_reset_of_active_users.TabStop = false;
            this.chb_reset_of_active_users.UseVisualStyleBackColor = true;
            // 
            // Tab_2
            // 
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel_cred_src);
            this.Controls.Add(this.but_upgrade);
            this.Controls.Add(this.chb_reset_of_active_users);
            this.Controls.Add(this.chb_allow_other_companies);
            this.Controls.Add(this.chb_debug);
            this.Controls.Add(this.lab_ver);
            this.Controls.Add(this.cb_def_company);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label17);
            this.Name = "Tab_2";
            this.Size = new System.Drawing.Size(1000, 1000);
            this.panel_cred_src.ResumeLayout(false);
            this.panel_cred_src.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal Button but_upgrade;
        internal ComboBox cb_def_company;
        internal Flat_Check_Box chb_allow_other_companies;
        internal Flat_Check_Box chb_reset_of_active_users;
        internal Flat_Check_Box chb_debug;
        internal Text_Label lab_ver;
        internal Label label1;
        internal Label label12;
        internal Label label17;
        internal Label label2;
        internal Label label3;
        internal Panel panel_cred_src;
        internal RadioButton rb_auth_src_internal;
        internal RadioButton rb_auth_src_prompt;



    }
}
