namespace Config_Gui
{
    partial class Add_Company_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.but_cancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // folderBrowserDialog1
            // 
            this.folder_dialog.SelectedPath = "C:\\";
            this.folder_dialog.ShowNewFolderButton = false;
            // 
            // browseB
            // 
            this.but_browse.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.but_browse.Location = new System.Drawing.Point(142, 42);
            this.but_browse.Name = "browseB";
            this.but_browse.Size = new System.Drawing.Size(75, 23);
            this.but_browse.TabIndex = 1;
            this.but_browse.Text = "Browse...";
            this.but_browse.UseVisualStyleBackColor = true;
            // 
            // tb_path
            // 
            this.tb_path.Location = new System.Drawing.Point(8, 16);
            this.tb_path.Name = "tb_path";
            this.tb_path.Size = new System.Drawing.Size(209, 20);
            this.tb_path.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select company folder:";
            // 
            // commB
            // 
            this.but_ok.Enabled = false;
            this.but_ok.Location = new System.Drawing.Point(42, 235);
            this.but_ok.Name = "commB";
            this.but_ok.Size = new System.Drawing.Size(75, 23);
            this.but_ok.TabIndex = 5;
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
            // tb_user
            // 
            this.tb_user.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_user.Location = new System.Drawing.Point(8, 35);
            this.tb_user.Name = "tb_user";
            this.tb_user.Size = new System.Drawing.Size(117, 20);
            this.tb_user.TabIndex = 2;
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
            // tb_pwd1
            // 
            this.tb_pass1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_pass1.Location = new System.Drawing.Point(2, 17);
            this.tb_pass1.Name = "tb_pwd1";
            this.tb_pass1.PasswordChar = '*';
            this.tb_pass1.Size = new System.Drawing.Size(117, 20);
            this.tb_pass1.TabIndex = 3;
            // 
            // tb_pwd2
            // 
            this.tb_pass2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_pass2.Location = new System.Drawing.Point(2, 43);
            this.tb_pass2.Name = "tb_pwd2";
            this.tb_pass2.PasswordChar = '*';
            this.tb_pass2.Size = new System.Drawing.Size(117, 20);
            this.tb_pass2.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tb_user);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(7, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(221, 139);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Company Credentials:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tb_pass1);
            this.panel1.Controls.Add(this.tb_pass2);
            this.panel1.Location = new System.Drawing.Point(6, 64);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(188, 68);
            this.panel1.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.tb_path);
            this.panel2.Controls.Add(this.but_browse);
            this.panel2.Location = new System.Drawing.Point(7, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(220, 68);
            this.panel2.TabIndex = 0;
            // 
            // cancelB
            // 
            this.but_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.but_cancel.Location = new System.Drawing.Point(123, 235);
            this.but_cancel.Name = "cancelB";
            this.but_cancel.Size = new System.Drawing.Size(75, 23);
            this.but_cancel.TabIndex = 6;
            this.but_cancel.Text = "Cancel";
            this.but_cancel.UseVisualStyleBackColor = true;
            this.but_cancel.Click += new System.EventHandler(this.cancelB_Click);
            // 
            // AddCompanyForm
            // 
            this.AcceptButton = this.but_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.but_cancel;
            this.ClientSize = new System.Drawing.Size(237, 269);
            this.ControlBox = false;
            this.Controls.Add(this.but_cancel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.but_ok);
            this.Controls.Add(this.groupBox1);
            this.Name = "AddCompanyForm";
            this.Text = "Add a new company";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        System.Windows.Forms.Label label1;
        System.Windows.Forms.Label label3;
        System.Windows.Forms.Label label4;
        System.Windows.Forms.GroupBox groupBox1;
        System.Windows.Forms.Panel panel1;
        System.Windows.Forms.Panel panel2;

    }
}