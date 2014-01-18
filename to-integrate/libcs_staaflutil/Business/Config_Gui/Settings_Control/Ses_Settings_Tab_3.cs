
using System.Collections.Generic;
using System.Windows.Forms;
using Common.Controls;
using DTA;
using Fairweather.Service;

namespace Config_Gui
{
    public partial class Ses_Settings_Tab_3 : DTA_Tab
    {
        public Ses_Settings_Tab_3() {

            InitializeComponent();


        }

        public override Dictionary<Control, Ini_Field> Get_Fields() {
            return new Dictionary<Control, Ini_Field>
            {
                {chb_start_maximized, DTA_Fields.POS_start_maximized},
                {cb_prices_with_vat, DTA_Fields.POS_display_prices_with},
                {nupd_id, DTA_Fields.POS_pos_id},
            };
        }

        public Button But_advanced {
            get { return but_advanced; }
        }
        public Panel Panel1 {
            get { return panel1; }
        }

        #region designer

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        Panel panel1;
        Button but_advanced;

   
        ComboBox cb_prices_with_vat;
        Flat_Check_Box chb_start_maximized;
        Label label8;
        Label label1;
        NumericUpDown nupd_id;
        Label label33;


        System.ComponentModel.IContainer components = null;

        void InitializeComponent() {
            this.panel1 = new System.Windows.Forms.Panel();
            this.but_advanced = new System.Windows.Forms.Button();
            this.cb_prices_with_vat = new System.Windows.Forms.ComboBox();
            this.chb_start_maximized = new Common.Controls.Flat_Check_Box();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nupd_id = new System.Windows.Forms.NumericUpDown();
            this.label33 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_id)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.but_advanced);
            this.panel1.Controls.Add(this.cb_prices_with_vat);
            this.panel1.Controls.Add(this.chb_start_maximized);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.nupd_id);
            this.panel1.Controls.Add(this.label33);
            this.panel1.Location = new System.Drawing.Point(10, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(351, 140);
            this.panel1.TabIndex = 34;
            // 
            // but_advanced
            // 
            this.but_advanced.Location = new System.Drawing.Point(265, 106);
            this.but_advanced.Name = "but_advanced";
            this.but_advanced.Size = new System.Drawing.Size(75, 23);
            this.but_advanced.TabIndex = 50;
            this.but_advanced.Text = "Advanced...";
            this.but_advanced.UseVisualStyleBackColor = true;
            // 
            // cb_prices_with_vat
            // 
            this.cb_prices_with_vat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_prices_with_vat.FormattingEnabled = true;
            this.cb_prices_with_vat.Location = new System.Drawing.Point(193, 32);
            this.cb_prices_with_vat.Name = "cb_prices_with_vat";
            this.cb_prices_with_vat.Size = new System.Drawing.Size(147, 21);
            this.cb_prices_with_vat.TabIndex = 20;
            // 
            // chb_start_maximized
            // 
            this.chb_start_maximized.AutoSize = true;
            this.chb_start_maximized.BackColor = System.Drawing.Color.White;
            this.chb_start_maximized.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_start_maximized.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_start_maximized.Location = new System.Drawing.Point(327, 64);
            this.chb_start_maximized.Name = "chb_start_maximized";
            this.chb_start_maximized.Size = new System.Drawing.Size(12, 11);
            this.chb_start_maximized.TabIndex = 30;
            this.chb_start_maximized.TabStop = false;
            this.chb_start_maximized.UseVisualStyleBackColor = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Display prices with:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Start Maximized:";
            // 
            // nupd_id
            // 
            this.nupd_id.Location = new System.Drawing.Point(293, 6);
            this.nupd_id.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nupd_id.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupd_id.Name = "nupd_id";
            this.nupd_id.Size = new System.Drawing.Size(48, 20);
            this.nupd_id.TabIndex = 10;
            this.nupd_id.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label33
            // 
            this.label33.Location = new System.Drawing.Point(3, 10);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(170, 12);
            this.label33.TabIndex = 30;
            this.label33.Text = "POS Id:";
            // 
            // Settings_Tab_3
            // 
            this.Controls.Add(this.panel1);
            this.Name = "Settings_Tab_3";
            this.Size = new System.Drawing.Size(376, 156);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_id)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

  



    }
}

