
using System.Windows.Forms;

    using Common.Controls;


namespace Config_Gui
    {

        public partial class Tab_1 : UserControl
        {
            public Tab_1() {

                InitializeComponent();

                // Standard.Flat_Style(cb_company);

                cb_company.BringToFront();
            }


            #region designer

            protected override void Dispose(bool disposing) {
                if (disposing && (components != null)) {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            System.ComponentModel.IContainer components = null;

            void InitializeComponent() {

                components = new System.ComponentModel.Container();
                this.but_add_company = new System.Windows.Forms.Button();
                this.but_change_cred = new System.Windows.Forms.Button();
                this.cb_company = new ComboBox();
                this.gb_access_cred = new System.Windows.Forms.GroupBox();
                this.gb_company_details = new System.Windows.Forms.GroupBox();
                this.label10 = new System.Windows.Forms.Label();
                this.label11 = new System.Windows.Forms.Label();
                this.label13 = new System.Windows.Forms.Label();
                this.label14 = new System.Windows.Forms.Label();
                this.label16 = new System.Windows.Forms.Label();
                this.label4 = new System.Windows.Forms.Label();
                this.lab_company_name = new Text_Label();
                this.lab_company_number = new Text_Label();
                this.lab_company_path = new Text_Label();
                this.lab_username = new Text_Label();
                this.lab_period = new Text_Label();

                this.tabc_settings = new Settings_Control();
                this.gb_settings = new System.Windows.Forms.GroupBox();

                this.gb_access_cred.SuspendLayout();
                this.gb_settings.SuspendLayout();
                this.gb_company_details.SuspendLayout();
                this.SuspendLayout();
                // 
                // but_add_company
                // 
                this.but_add_company.Location = new System.Drawing.Point(313, 23);
                this.but_add_company.Name = "but_add_company";
                this.but_add_company.Size = new System.Drawing.Size(75, 23);
                this.but_add_company.TabIndex = 33;
                this.but_add_company.Text = "Add...";
                this.but_add_company.UseVisualStyleBackColor = true;
                // 
                // gb_access_cred
                // 
                this.gb_access_cred.Controls.Add(this.lab_username);
                this.gb_access_cred.Controls.Add(this.but_change_cred);
                this.gb_access_cred.Controls.Add(this.label4);
                this.gb_access_cred.Location = new System.Drawing.Point(6, 168);
                this.gb_access_cred.Name = "gb_access_cred";
                this.gb_access_cred.Size = new System.Drawing.Size(385, 74);
                this.gb_access_cred.TabIndex = 36;
                this.gb_access_cred.TabStop = false;
                this.gb_access_cred.Text = "Access Credentials";
                // 
                // lab_username
                // 
                this.lab_username.Location = new System.Drawing.Point(9, 43);
                this.lab_username.Name = "lab_username";
                this.lab_username.Size = new System.Drawing.Size(243, 20);
                this.lab_username.TabIndex = 5;
                // 
                // but_change_cred
                // 
                this.but_change_cred.Location = new System.Drawing.Point(256, 41);
                this.but_change_cred.Name = "but_change_cred";
                this.but_change_cred.Size = new System.Drawing.Size(122, 23);
                this.but_change_cred.TabIndex = 6;
                this.but_change_cred.Text = "Change Credentials...";
                this.but_change_cred.UseVisualStyleBackColor = true;
                // 
                // label4
                // 
                this.label4.AutoSize = true;
                this.label4.Location = new System.Drawing.Point(6, 22);
                this.label4.Name = "label4";
                this.label4.Size = new System.Drawing.Size(95, 13);
                this.label4.TabIndex = 13;
                this.label4.Text = "Current Username:";
                // 
                // gb_company_details
                // 
                this.gb_company_details.Controls.Add(this.lab_period);
                this.gb_company_details.Controls.Add(this.lab_company_number);
                this.gb_company_details.Controls.Add(this.label16);
                this.gb_company_details.Controls.Add(this.lab_company_path);
                this.gb_company_details.Controls.Add(this.lab_company_name);
                this.gb_company_details.Controls.Add(this.label14);
                this.gb_company_details.Controls.Add(this.label13);
                this.gb_company_details.Controls.Add(this.label10);
                this.gb_company_details.Location = new System.Drawing.Point(5, 50);
                this.gb_company_details.Name = "gb_company_details";
                this.gb_company_details.Size = new System.Drawing.Size(385, 119);
                this.gb_company_details.TabIndex = 35;
                this.gb_company_details.TabStop = false;
                this.gb_company_details.Text = "Company Details";
                // 
                // lab_period
                // 
                this.lab_period.Location = new System.Drawing.Point(115, 89);
                this.lab_period.Name = "lab_period";
                this.lab_period.TabIndex = 8;
                this.lab_period.Text = "...";
                this.lab_period.Size = new System.Drawing.Size(264, 20);
                // 
                // lab_company_number
                // 
                this.lab_company_number.Location = new System.Drawing.Point(115, 23);
                this.lab_company_number.Name = "lab_company_number";
                this.lab_company_number.Size = new System.Drawing.Size(264, 20);
                this.lab_company_number.TabIndex = 2;

                // 
                // label16
                // 
                this.label16.AutoSize = true;
                this.label16.Location = new System.Drawing.Point(4, 24);
                this.label16.Name = "label16";
                this.label16.Size = new System.Drawing.Size(94, 13);
                this.label16.TabIndex = 7;
                this.label16.Text = "Company Number:";
                // 
                // lab_company_path
                // 
                this.lab_company_path.Location = new System.Drawing.Point(115, 67);
                this.lab_company_path.Name = "lab_company_path";
                this.lab_company_path.Size = new System.Drawing.Size(264, 20);
                this.lab_company_path.TabIndex = 4;

                // 
                // lab_company_name
                // 
                this.lab_company_name.AccessibleDescription = "";
                this.lab_company_name.Location = new System.Drawing.Point(115, 45);
                this.lab_company_name.Name = "lab_company_name";
                this.lab_company_name.Size = new System.Drawing.Size(264, 20);
                this.lab_company_name.TabIndex = 3;

                // 
                // label14
                // 
                this.label14.AutoSize = true;
                this.label14.Location = new System.Drawing.Point(4, 93);
                this.label14.Name = "label14";
                this.label14.Size = new System.Drawing.Size(77, 13);
                this.label14.TabIndex = 2;
                this.label14.Text = "Current Period:";
                // 
                // label13
                // 
                this.label13.AutoSize = true;
                this.label13.Location = new System.Drawing.Point(4, 70);
                this.label13.Name = "label13";
                this.label13.Size = new System.Drawing.Size(105, 13);
                this.label13.TabIndex = 1;
                this.label13.Text = "Company Data Path:";
                // 
                // label10
                // 
                this.label10.AutoSize = true;
                this.label10.Location = new System.Drawing.Point(4, 47);
                this.label10.Name = "label10";
                this.label10.Size = new System.Drawing.Size(85, 13);
                this.label10.TabIndex = 0;
                this.label10.Text = "Company Name:";
                // 
                // label11
                // 
                this.label11.AutoSize = true;
                this.label11.Location = new System.Drawing.Point(4, 8);
                this.label11.Name = "label11";
                this.label11.Size = new System.Drawing.Size(87, 13);
                this.label11.TabIndex = 34;
                this.label11.Text = "Select Company:";
                // 
                // cb_company
                // 
                this.cb_company.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                this.cb_company.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.cb_company.FormatString = "{s:10}";
                this.cb_company.FormattingEnabled = true;
                this.cb_company.Location = new System.Drawing.Point(4, 24);
                this.cb_company.Name = "cb_company";
                this.cb_company.Size = new System.Drawing.Size(300, 21);
                this.cb_company.TabIndex = 32;
                // 
                // gb_settings
                // 
                this.gb_settings.Controls.Add(this.tabc_settings);
                this.gb_settings.Location = new System.Drawing.Point(4, 246);
                this.gb_settings.Name = "gb_settings";
                this.gb_settings.Size = new System.Drawing.Size(385, 205);
                this.gb_settings.TabIndex = 38;
                this.gb_settings.TabStop = false;
                this.gb_settings.Text = "Company Settings";
                // 
                // tabc_settings
                // 
                this.tabc_settings.Active_Tab = 0;
                this.tabc_settings.Location = new System.Drawing.Point(5, 16);
                this.tabc_settings.Name = "tabc_settings";
                this.tabc_settings.Size = new System.Drawing.Size(374, 183);
                this.tabc_settings.TabIndex = 37;
                this.tabc_settings.BringToFront();
                //
                this.but_add_company.Location = new System.Drawing.Point(313, 23);
                this.gb_access_cred.Location = new System.Drawing.Point(4, 172);
                this.lab_username.Location = new System.Drawing.Point(9, 43);
                this.but_change_cred.Location = new System.Drawing.Point(256, 41);
                this.label4.Location = new System.Drawing.Point(6, 22);
                this.gb_company_details.Location = new System.Drawing.Point(4, 54);
                this.lab_company_number.Location = new System.Drawing.Point(115, 23);
                this.label16.Location = new System.Drawing.Point(4, 24);
                this.lab_company_path.Location = new System.Drawing.Point(115, 67);
                this.lab_company_name.Location = new System.Drawing.Point(115, 45);
                this.label14.Location = new System.Drawing.Point(4, 93);
                this.label13.Location = new System.Drawing.Point(4, 70);
                this.label10.Location = new System.Drawing.Point(4, 47);
                this.label11.Location = new System.Drawing.Point(4, 8);
                this.cb_company.Location = new System.Drawing.Point(6, 24);


                // 
                // Test_1
                // 
                this.ClientSize = new System.Drawing.Size(570, 547);
                this.Controls.Add(this.but_add_company);
                this.Controls.Add(this.gb_access_cred);
                this.Controls.Add(this.gb_company_details);
                this.Controls.Add(this.gb_settings);
                this.Controls.Add(this.label11);
                this.Controls.Add(this.cb_company);

                this.Name = "Test_1";
                this.Text = "Test_1";
                this.gb_access_cred.ResumeLayout(false);
                this.gb_access_cred.PerformLayout();
                this.gb_company_details.ResumeLayout(false);
                this.gb_company_details.PerformLayout();
                this.gb_settings.ResumeLayout(false);
                this.gb_settings.PerformLayout();
                this.ResumeLayout(false);

            }

            #endregion

            internal Button but_add_company;
            internal Button but_change_cred;

            internal ComboBox cb_company;

            GroupBox gb_access_cred;
            GroupBox gb_company_details;

            internal Label lab_period;

            internal Label label10;
            internal Label label11;
            internal Label label13;
            internal Label label14;
            internal Label label16;
            internal Label label4;

            internal Label lab_company_name;
            internal Label lab_company_number;
            internal Label lab_company_path;
            internal Label lab_username;

            GroupBox gb_settings;
            internal Settings_Control tabc_settings;


        }
    }

