using System;

using System.Windows.Forms;

using Common;

using Fairweather.Service;

namespace Config_Gui
{
    public partial class TE_Advanced_Settings : Common.Dialogs.Form_Base
    {


        public TE_Advanced_Settings(Ini_File p_pman, Company_Number company)
            : base(Form_Kind.Modal_Dialog) {

            InitializeComponent();

            this.ini = p_pman;
            this.company = company;

            tabc_advanced_Settings.Set_Company(company, false);
            tabc_advanced_Settings.Setup(ini);
            tabc_advanced_Settings.Set_Company(company, true, true);

            but_cancel.Click += (_1, _2) => this.Close();
        }


        void but_ok_Click(object sender, EventArgs e) {

            tabc_advanced_Settings.Store_Data();
            Close();

        }


        readonly Ini_File ini;
        readonly Company_Number company;
        Config_Gui.TE_Advanced_Settings_Control tabc_advanced_Settings;
        Button but_ok;
        Button but_cancel;

        #region designer code

        System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        void InitializeComponent() {
            this.but_ok = new System.Windows.Forms.Button();
            this.but_cancel = new System.Windows.Forms.Button();
            this.tabc_advanced_Settings = new Config_Gui.TE_Advanced_Settings_Control();
            this.SuspendLayout();
            // 
            // but_ok
            // 
            this.but_ok.Location = new System.Drawing.Point(413, 396);
            this.but_ok.Name = "but_ok";
            this.but_ok.Size = new System.Drawing.Size(75, 23);
            this.but_ok.TabIndex = 9;
            this.but_ok.Text = "OK";
            this.but_ok.UseVisualStyleBackColor = true;
            this.but_ok.Click += new System.EventHandler(this.but_ok_Click);
            // 
            // but_cancel
            // 
            this.but_cancel.Location = new System.Drawing.Point(492, 396);
            this.but_cancel.Name = "but_cancel";
            this.but_cancel.Size = new System.Drawing.Size(75, 23);
            this.but_cancel.TabIndex = 12;
            this.but_cancel.Text = "Cancel";
            this.but_cancel.UseVisualStyleBackColor = true;
            // 
            // tabc_advanced_Settings
            // 
            this.tabc_advanced_Settings.Active_Tab = 0;
            this.tabc_advanced_Settings.Highlight_Color = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tabc_advanced_Settings.Location = new System.Drawing.Point(4, 2);
            this.tabc_advanced_Settings.Name = "tabc_advanced_Settings";
            this.tabc_advanced_Settings.Normal_Button_Color = System.Drawing.Color.LightGray;
            this.tabc_advanced_Settings.Size = new System.Drawing.Size(563, 388);
            this.tabc_advanced_Settings.TabIndex = 0;
            // 
            // POS_Advanced_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 428);
            this.Controls.Add(this.but_ok);
            this.Controls.Add(this.but_cancel);
            this.Controls.Add(this.tabc_advanced_Settings);
            this.Name = "POS_Advanced_Settings";
            this.Text = "POS - Advanced Settings";
            this.ResumeLayout(false);

        }

        #endregion

    }
}
