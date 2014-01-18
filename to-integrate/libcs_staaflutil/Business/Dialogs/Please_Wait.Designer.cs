namespace Common.Dialogs
{
    partial class Please_Wait
    {
        /// <summary>  Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

        /// <summary>  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>  Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent() {
            this.progress_bar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progress_bar
            // 
            this.progress_bar.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.progress_bar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.progress_bar.Location = new System.Drawing.Point(12, 43);
            this.progress_bar.Name = "progress_bar";
            this.progress_bar.Size = new System.Drawing.Size(237, 20);
            this.progress_bar.TabIndex = 4;
            this.progress_bar.Value = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Please wait...";
            // 
            // PleaseWait
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 75);
            this.ControlBox = false;
            this.Controls.Add(this.progress_bar);
            this.Controls.Add(this.label1);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(266, 100);
            this.MinimumSize = new System.Drawing.Size(266, 100);
            this.Name = "PleaseWait";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loading product and barcode data";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.progress_bar, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        System.Windows.Forms.ProgressBar progress_bar;

        System.Windows.Forms.Label label1;


    }
}