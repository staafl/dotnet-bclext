using System.Collections.Generic;
using System.Windows.Forms;
using Common;
using Common.Controls;
using DTA;
using Fairweather.Service;
using Standardization;

namespace Config_Gui
{

    public class OPOS_Printer_Dialog : DTA_Dialog
    {


        public OPOS_Printer_Dialog(Ini_File ini, Company_Number number)
            : base(ini, number) {

            InitializeComponent();

            foreach (var info in Pos_For_Net.Get_OPOS_Logical_Names(this))
                cb_opos_device.Items.Add(info);

            Prepare_DTA();

            if (cb_opos_device.SelectedIndex == -1 && cb_opos_device.Items.Count > 0)
                cb_opos_device.SelectedIndex = 0;

            but_ok.Click += (but_ok_Click);
            but_cancel.Click += (_1, _2) => Close();

            this.Text = "Printing Configuration - OPOS";



        }

        public override Dictionary<Control, Ini_Field> Get_Dictionary {
            get {
                return new Dictionary<Control, Ini_Field>
                {  {chb_hiq_letters, DTA_Fields.POS_printing_opos_hiq_letters},
                    {cb_opos_device, DTA_Fields.POS_printing_opos_logical_name},
                    {chb_keep_open,  DTA_Fields.POS_printing_opos_keep_open},
                    {nupd_paper_cut, DTA_Fields.POS_printing_opos_cut_paper_perc},
                    {nupd_scale, DTA_Fields.POS_printing_opos_image_dpi},

                };
            }
        }

        protected override Dictionary<Control, DTA_Custom_Behavior_Pair> Custom_Behavior_Pairs {
            get {
                return new Dictionary<Control, DTA_Custom_Behavior_Pair> { };
            }
        }

        #region designer

        Label label1;
        Label label2;
        Label label3;
        Button but_ok;
        Button but_cancel;
        NumericUpDown nupd_paper_cut;
        ComboBox cb_opos_device;
        private Flat_Check_Box chb_keep_open;
        private Label label4;
        private Label label5;
        private NumericUpDown nupd_scale;
        Flat_Check_Box chb_hiq_letters;

        void InitializeComponent() {
            this.chb_hiq_letters = new Common.Controls.Flat_Check_Box();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.but_ok = new System.Windows.Forms.Button();
            this.but_cancel = new System.Windows.Forms.Button();
            this.nupd_paper_cut = new System.Windows.Forms.NumericUpDown();
            this.cb_opos_device = new System.Windows.Forms.ComboBox();
            this.chb_keep_open = new Common.Controls.Flat_Check_Box();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nupd_scale = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_paper_cut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_scale)).BeginInit();
            this.SuspendLayout();
            // 
            // chb_hiq_letters
            // 
            this.chb_hiq_letters.AutoSize = true;
            this.chb_hiq_letters.BackColor = System.Drawing.Color.White;
            this.chb_hiq_letters.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_hiq_letters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_hiq_letters.Location = new System.Drawing.Point(269, 46);
            this.chb_hiq_letters.Name = "chb_hiq_letters";
            this.chb_hiq_letters.Size = new System.Drawing.Size(12, 11);
            this.chb_hiq_letters.TabIndex = 1;
            this.chb_hiq_letters.TabStop = false;
            this.chb_hiq_letters.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "High-quality Letters:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "OPOS Device:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Paper cutting % (0 for none):";
            // 
            // but_ok
            // 
            this.but_ok.Location = new System.Drawing.Point(126, 173);
            this.but_ok.Name = "but_ok";
            this.but_ok.Size = new System.Drawing.Size(75, 23);
            this.but_ok.TabIndex = 5;
            this.but_ok.Text = "OK";
            this.but_ok.UseVisualStyleBackColor = true;
            // 
            // but_cancel
            // 
            this.but_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.but_cancel.Location = new System.Drawing.Point(206, 173);
            this.but_cancel.Name = "but_cancel";
            this.but_cancel.Size = new System.Drawing.Size(75, 23);
            this.but_cancel.TabIndex = 6;
            this.but_cancel.Text = "Cancel";
            this.but_cancel.UseVisualStyleBackColor = true;
            // 
            // nupd_paper_cut
            // 
            this.nupd_paper_cut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_paper_cut.Location = new System.Drawing.Point(240, 99);
            this.nupd_paper_cut.Name = "nupd_paper_cut";
            this.nupd_paper_cut.Size = new System.Drawing.Size(41, 20);
            this.nupd_paper_cut.TabIndex = 3;
            // 
            // cb_opos_device
            // 
            this.cb_opos_device.FormattingEnabled = true;
            this.cb_opos_device.Location = new System.Drawing.Point(138, 12);
            this.cb_opos_device.Name = "cb_opos_device";
            this.cb_opos_device.Size = new System.Drawing.Size(143, 21);
            this.cb_opos_device.TabIndex = 0;
            // 
            // chb_keep_open
            // 
            this.chb_keep_open.AutoSize = true;
            this.chb_keep_open.BackColor = System.Drawing.Color.White;
            this.chb_keep_open.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_keep_open.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_keep_open.Location = new System.Drawing.Point(269, 75);
            this.chb_keep_open.Name = "chb_keep_open";
            this.chb_keep_open.Size = new System.Drawing.Size(12, 11);
            this.chb_keep_open.TabIndex = 2;
            this.chb_keep_open.TabStop = false;
            this.chb_keep_open.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Keep Printer open:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(218, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Image Scaling in DPI (0 to let Printer decide):";
            // 
            // nupd_scale
            // 
            this.nupd_scale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_scale.Location = new System.Drawing.Point(240, 128);
            this.nupd_scale.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.nupd_scale.Name = "nupd_scale";
            this.nupd_scale.Size = new System.Drawing.Size(41, 20);
            this.nupd_scale.TabIndex = 4;
            // 
            // OPOS_Printer_Dialog
            // 
            this.AcceptButton = this.but_ok;
            this.CancelButton = this.but_cancel;
            this.ClientSize = new System.Drawing.Size(291, 203);
            this.Controls.Add(this.cb_opos_device);
            this.Controls.Add(this.nupd_scale);
            this.Controls.Add(this.nupd_paper_cut);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.but_cancel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.but_ok);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chb_keep_open);
            this.Controls.Add(this.chb_hiq_letters);
            this.MaximumSize = new System.Drawing.Size(297, 228);
            this.MinimumSize = new System.Drawing.Size(297, 228);
            this.Name = "OPOS_Printer_Dialog";
            this.Text = "Printing Configuration";
            this.Controls.SetChildIndex(this.chb_hiq_letters, 0);
            this.Controls.SetChildIndex(this.chb_keep_open, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.but_ok, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.but_cancel, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.nupd_paper_cut, 0);
            this.Controls.SetChildIndex(this.nupd_scale, 0);
            this.Controls.SetChildIndex(this.cb_opos_device, 0);
            ((System.ComponentModel.ISupportInitialize)(this.nupd_paper_cut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_scale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }

}