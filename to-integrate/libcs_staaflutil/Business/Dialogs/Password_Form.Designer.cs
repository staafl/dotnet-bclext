namespace Config_Gui
{
    partial class Password_Form
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
            this.tb_pass1 = new System.Windows.Forms.TextBox();
            this.tb_pass2 = new System.Windows.Forms.TextBox();
            this.tb_new_user = new System.Windows.Forms.TextBox();
            this.passwordBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lab_user = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.but_ok = new System.Windows.Forms.Button();
            this.but_cancel = new System.Windows.Forms.Button();
            this.passPan = new System.Windows.Forms.Panel();
            this.passwordBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.passPan.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_pass1
            // 
            this.tb_pass1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_pass1.Location = new System.Drawing.Point(138, 21);
            this.tb_pass1.Name = "tb_pass1";
            this.tb_pass1.PasswordChar = '*';
            this.tb_pass1.Size = new System.Drawing.Size(100, 20);
            this.tb_pass1.TabIndex = 2;
            // 
            // tb_pass2
            // 
            this.tb_pass2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_pass2.Location = new System.Drawing.Point(138, 47);
            this.tb_pass2.Name = "tb_pass2";
            this.tb_pass2.PasswordChar = '*';
            this.tb_pass2.Size = new System.Drawing.Size(100, 20);
            this.tb_pass2.TabIndex = 3;
            // 
            // tb_new_user
            // 
            this.tb_new_user.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_new_user.Location = new System.Drawing.Point(137, 42);
            this.tb_new_user.Name = "tb_new_user";
            this.tb_new_user.Size = new System.Drawing.Size(100, 20);
            this.tb_new_user.TabIndex = 0;
            // 
            // passwordBox
            // 
            this.passwordBox.Controls.Add(this.label2);
            this.passwordBox.Controls.Add(this.label1);
            this.passwordBox.Controls.Add(this.tb_pass2);
            this.passwordBox.Controls.Add(this.tb_pass1);
            this.passwordBox.Location = new System.Drawing.Point(9, 7);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(245, 78);
            this.passwordBox.TabIndex = 1;
            this.passwordBox.TabStop = false;
            this.passwordBox.Text = "Change Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Confirm new Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Type in new Password:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lab_user);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tb_new_user);
            this.groupBox1.Location = new System.Drawing.Point(5, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 76);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Change Username";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "New Username:";
            // 
            // lab_user
            // 
            this.lab_user.Location = new System.Drawing.Point(122, 22);
            this.lab_user.Name = "lab_user";
            this.lab_user.Size = new System.Drawing.Size(116, 13);
            this.lab_user.TabIndex = 5;
            this.lab_user.Text = "INTERFACE";
            this.lab_user.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Current Username:";
            // 
            // but_ok
            // 
            this.but_ok.Enabled = false;
            this.but_ok.Location = new System.Drawing.Point(95, 178);
            this.but_ok.Name = "but_ok";
            this.but_ok.Size = new System.Drawing.Size(75, 23);
            this.but_ok.TabIndex = 4;
            this.but_ok.Text = "OK";
            this.but_ok.UseVisualStyleBackColor = true;
            this.but_ok.Click += new System.EventHandler(this.okB_Click);
            // 
            // but_cancel
            // 
            this.but_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.but_cancel.Location = new System.Drawing.Point(175, 178);
            this.but_cancel.Name = "but_cancel";
            this.but_cancel.Size = new System.Drawing.Size(75, 23);
            this.but_cancel.TabIndex = 5;
            this.but_cancel.Text = "Cancel";
            this.but_cancel.UseVisualStyleBackColor = true;
            this.but_cancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // passPan
            // 
            this.passPan.Controls.Add(this.passwordBox);
            this.passPan.Location = new System.Drawing.Point(-4, 77);
            this.passPan.Name = "passPan";
            this.passPan.Size = new System.Drawing.Size(257, 95);
            this.passPan.TabIndex = 0;
            // 
            // PasswordForm
            // 
            this.AcceptButton = this.but_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.but_cancel;
            this.ClientSize = new System.Drawing.Size(257, 208);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.passPan);
            this.Controls.Add(this.but_cancel);
            this.Controls.Add(this.but_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Access Credentials";
            this.passwordBox.ResumeLayout(false);
            this.passwordBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.passPan.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        System.Windows.Forms.TextBox tb_pass1;
        System.Windows.Forms.TextBox tb_pass2;
        System.Windows.Forms.TextBox tb_new_user;
        System.Windows.Forms.GroupBox passwordBox;
        System.Windows.Forms.Label label2;
        System.Windows.Forms.Label label1;
        System.Windows.Forms.GroupBox groupBox1;
        System.Windows.Forms.Button but_ok;
        System.Windows.Forms.Label label5;
        System.Windows.Forms.Label lab_user;
        System.Windows.Forms.Label label3;
        System.Windows.Forms.Button but_cancel;
        System.Windows.Forms.Panel passPan;
    }
}