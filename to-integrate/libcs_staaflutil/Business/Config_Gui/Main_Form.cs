using System;

using System.Windows.Forms;

using Common;
using Config_Gui;

namespace Config_Gui
{
    public partial class Main_Form : Common.Dialogs.Form_Base
    {
        public Main_Form()
            : base(Form_Kind.Fixed_Main_Form) {

            InitializeComponent();

            this.MaximizeBox = false;

            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
            //              ControlStyles.AllPaintingInWmPaint, true);

            this.tab_control.Setup();

            this.Text = Data.Is_Entry_Screens_Suite ? "Sage Entry Screens - Configuration" :
                 "Sage Interface Tools - Configuration";


        }

        Config_Control tab_control;
        Button but_ok;
        Button but_close;

        #region designer

        System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        void InitializeComponent() {
            this.but_ok = new System.Windows.Forms.Button();
            this.but_close = new System.Windows.Forms.Button();
            this.tab_control = new Config_Gui.Config_Control();
            this.SuspendLayout();
            // 
            // but_ok
            // 
            this.but_ok.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.but_ok.Location = new System.Drawing.Point(243, 493);
            this.but_ok.Name = "but_ok";
            this.but_ok.Size = new System.Drawing.Size(75, 23);
            this.but_ok.TabIndex = 10;
            this.but_ok.Text = "OK";
            this.but_ok.UseVisualStyleBackColor = true;
            this.but_ok.Click += new System.EventHandler(this.but_ok_Click);
            // 
            // but_close
            // 
            this.but_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.but_close.Location = new System.Drawing.Point(324, 493);
            this.but_close.Name = "but_close";
            this.but_close.Size = new System.Drawing.Size(75, 23);
            this.but_close.TabIndex = 20;
            this.but_close.Text = "Close";
            this.but_close.UseVisualStyleBackColor = true;
            this.but_close.Click += new System.EventHandler(this.but_close_Click);
            // 
            // tab_control
            // 
            this.tab_control.Active_Tab = 0;
            this.tab_control.Highlight_Color = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tab_control.Location = new System.Drawing.Point(2, 0);
            this.tab_control.Name = "tab_control";
            this.tab_control.Size = new System.Drawing.Size(399, 487);
            this.tab_control.TabIndex = 0;
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.CancelButton = this.but_close;
            this.ClientSize = new System.Drawing.Size(403, 521);
            this.Controls.Add(this.but_close);
            this.Controls.Add(this.but_ok);
            this.Controls.Add(this.tab_control);
            this.Name = "Main_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.LimeGreen;
            this.ResumeLayout(false);

        }

        #endregion

        void but_close_Click(object sender, EventArgs e) {

            this.Close();
        }

        void but_ok_Click(object sender, EventArgs e) {

            tab_control.Save_Settings();
            this.Close();

        }
    }
}
