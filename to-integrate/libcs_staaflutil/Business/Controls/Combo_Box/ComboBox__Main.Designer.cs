namespace Common.Controls
{
    partial class Our_Combo_Box
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

        #region Component Designer generated code

        /// <summary>  Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent() {
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // comboBox
            // 
            this.comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox.DropDownHeight = 1;
            this.comboBox.DropDownWidth = 1;
            this.comboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.IntegralHeight = false;
            this.comboBox.Location = new System.Drawing.Point(0, 0);
            this.comboBox.MaxDropDownItems = 1;
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(100, 21);
            this.comboBox.TabIndex = 0;
            // 
            // Our_ComboBoxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.comboBox);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(100, 20);
            this.Name = "Our_ComboBoxControl";
            this.Size = new System.Drawing.Size(100, 20);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.ComboBox comboBox;
    }
}
