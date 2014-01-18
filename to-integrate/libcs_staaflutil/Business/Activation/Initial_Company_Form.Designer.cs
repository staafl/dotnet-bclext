namespace Activation
{
    using System.Windows.Forms;

    partial class Initial_Company_Form
    {
        System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        void InitializeComponent() {
            this.folder_dialog = new System.Windows.Forms.FolderBrowserDialog();
            this.but_browse = new System.Windows.Forms.Button();
            this.tb_path = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.but_ok = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_user = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_pass1 = new System.Windows.Forms.TextBox();
            this.tb_pass2 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.verCB = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.but_cancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // folder_dialog
            // 
            this.folder_dialog.SelectedPath = "C:\\";
            this.folder_dialog.ShowNewFolderButton = false;
            // 
            // browseB
            // 
            this.but_browse.Location = new System.Drawing.Point(188, 72);
            this.but_browse.Name = "browseB";
            this.but_browse.Size = new System.Drawing.Size(75, 23);
            this.but_browse.TabIndex = 2;
            this.but_browse.Text = "&Browse...";
            this.but_browse.UseVisualStyleBackColor = true;
            // 
            // pathTB
            // 
            this.tb_path.Location = new System.Drawing.Point(12, 46);
            this.tb_path.Name = "pathTB";
            this.tb_path.Size = new System.Drawing.Size(253, 20);
            this.tb_path.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select company folder:";
            // 
            // commB
            // 
            this.but_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.but_ok.Enabled = false;
            this.but_ok.Location = new System.Drawing.Point(112, 324);
            this.but_ok.Name = "commB";
            this.but_ok.Size = new System.Drawing.Size(75, 23);
            this.but_ok.TabIndex = 6;
            this.but_ok.Text = "OK";
            this.but_ok.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Default Username:";
            // 
            // userTB
            // 
            this.tb_user.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_user.Location = new System.Drawing.Point(6, 35);
            this.tb_user.Name = "userTB";
            this.tb_user.Size = new System.Drawing.Size(239, 20);
            this.tb_user.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Default Password:";
            // 
            // pwd1TB
            // 
            this.tb_pass1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_pass1.Location = new System.Drawing.Point(6, 81);
            this.tb_pass1.Name = "pwd1TB";
            this.tb_pass1.PasswordChar = '*';
            this.tb_pass1.Size = new System.Drawing.Size(239, 20);
            this.tb_pass1.TabIndex = 4;
            // 
            // pwd2TB
            // 
            this.tb_pass2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_pass2.Location = new System.Drawing.Point(6, 107);
            this.tb_pass2.Name = "pwd2TB";
            this.tb_pass2.PasswordChar = '*';
            this.tb_pass2.Size = new System.Drawing.Size(239, 20);
            this.tb_pass2.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tb_pass2);
            this.groupBox1.Controls.Add(this.tb_pass1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tb_user);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 107);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(251, 139);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Company Credentials:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Location = new System.Drawing.Point(6, 64);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(253, 68);
            this.panel1.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Sage Version:";
            // 
            // verCB
            // 
            this.verCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.verCB.FormattingEnabled = true;
            this.verCB.Items.AddRange(new object[] {
            "11.0",
            "12.0",
            "13.0 (2007)",
            "14.0 (2008)",
            "15.0 (2009)",
            "16.0 (2010)",
            "17.0 (2011)"});
            this.verCB.Location = new System.Drawing.Point(15, 30);
            this.verCB.Name = "verCB";
            this.verCB.Size = new System.Drawing.Size(110, 21);
            this.verCB.TabIndex = 0;
            this.verCB.SelectedIndexChanged += new System.EventHandler(this.verCB_SelectedIndexChanged);
            this.verCB.SelectionChangeCommitted += new System.EventHandler(this.verCB_SelectedIndexChanged);
            this.verCB.SelectedIndexChanged += new System.EventHandler(this.verCB_SelectedIndexChanged);

            this.verCB.Leave += new System.EventHandler(this.verCB_Leave);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tb_path);
            this.groupBox2.Controls.Add(this.but_browse);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(4, 63);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(271, 253);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Initial Company Setup";
            // 
            // cancelB
            // 
            this.but_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.but_cancel.Location = new System.Drawing.Point(193, 324);
            this.but_cancel.Name = "cancelB";
            this.but_cancel.Size = new System.Drawing.Size(75, 23);
            this.but_cancel.TabIndex = 7;
            this.but_cancel.Text = "Cancel";
            this.but_cancel.UseVisualStyleBackColor = true;
            this.but_cancel.Click += new System.EventHandler(this.cancelB_Click);
            // 
            // InitialForm
            // 
            this.AcceptButton = this.but_ok;
            this.CancelButton = this.but_cancel;
            this.ClientSize = new System.Drawing.Size(292, 353);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.verCB);
            this.Controls.Add(this.but_cancel);
            this.Controls.Add(this.but_ok);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InitialForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Sage Interface Tools Initial Setup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        ComboBox verCB;
        GroupBox groupBox1;
        GroupBox groupBox2;
        Label label1;
        Label label2;
        Label label3;
        Label label4;
        Panel panel1;

    }
}