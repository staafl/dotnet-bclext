using System;
using System.Windows.Forms;
using Common;
using Common.Controls;

namespace Common.Dialogs
{
    partial class Quick_Items_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;




        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tab_control = new Common.Controls.Advanced_Tab_Control();
            this.SuspendLayout();
            // 
            // tab_control
            // 
            this.tab_control.Active_Tab = -1;
            this.tab_control.BackColor = System.Drawing.Color.LightGray;
            this.tab_control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_control.Highlight_Color = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tab_control.Location = new System.Drawing.Point(2, 0);
            this.tab_control.Name = "tab_control";
            this.tab_control.Normal_Button_Color = System.Drawing.Color.LightGray;
            this.tab_control.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.tab_control.Size = new System.Drawing.Size(916, 375);
            this.tab_control.TabIndex = 0;
            // 
            // Quick_Items_Form
            // 
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(918, 375);
            this.ControlBox = false;
            this.Controls.Add(this.tab_control);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(874, 323);
            this.MinimumSize = new System.Drawing.Size(874, 323);
            this.Name = "Quick_Items_Form";
            this.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.Text = " Quick Items";
            this.Controls.SetChildIndex(this.tab_control, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public Advanced_Tab_Control tab_control;


    }
}