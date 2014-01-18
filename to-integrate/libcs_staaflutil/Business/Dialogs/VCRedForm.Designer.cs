namespace SageIntConfig
{
    partial class VCRedForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.but_ok = new System.Windows.Forms.Button();
            this.but_no = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, -4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(274, 94);
            this.label1.TabIndex = 0;
            this.label1.Text = "Microsoft Visual C++ Redistributable package is required\r\nbut has been uninstalle" +
                "d.\r\n\r\nPlease click \'OK\' to automatically reinstall the package.\r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // but_ok
            // 
            this.but_ok.Location = new System.Drawing.Point(109, 83);
            this.but_ok.Name = "but_ok";
            this.but_ok.Size = new System.Drawing.Size(75, 23);
            this.but_ok.TabIndex = 1;
            this.but_ok.Text = "OK";
            this.but_ok.UseVisualStyleBackColor = true;
            this.but_ok.Click += new System.EventHandler(this.but_ok_Click);
            // 
            // but_no
            // 
            this.but_no.Location = new System.Drawing.Point(190, 83);
            this.but_no.Name = "but_no";
            this.but_no.Size = new System.Drawing.Size(75, 23);
            this.but_no.TabIndex = 2;
            this.but_no.Text = "Cancel";
            this.but_no.UseVisualStyleBackColor = true;
            this.but_no.Click += new System.EventHandler(this.but_no_Click);
            // 
            // VCRedForm
            // 
            this.AcceptButton = this.but_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 114);
            this.ControlBox = false;
            this.Controls.Add(this.but_no);
            this.Controls.Add(this.but_ok);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VCRedForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sage Interface Tools - Error";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VCRedForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button but_ok;
        private System.Windows.Forms.Button but_no;
    }
}