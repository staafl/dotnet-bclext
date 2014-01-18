using System.Collections.Generic;
using System.Windows.Forms;

using Common.Controls;
using DTA;
using Fairweather.Service;

namespace Config_Gui
{
    public partial class Tab_General : DTA_Tab
    {
        public Tab_General() {

            InitializeComponent();


        }


        public override Dictionary<Control, Ini_Field> Get_Fields() {
            return new Dictionary<Control, Ini_Field>{

                {chb_duplicate_items, DTA_Fields.POS_launch_with_duplicates},
                {chb_zero_priced_items, DTA_Fields.POS_allow_zero_priced_items},
                {nupd_day_index, DTA_Fields.POS_z_ser_number},

                {chb_select_client_before_data, DTA_Fields.POS_select_customer_before_data_entry},
                {chb_no_payment, DTA_Fields.POS_no_payment_for_transactions},
                {chb_allow_credit_refunds, DTA_Fields.POS_allow_credit_note_refunds},
                {cb_avg_cost_calc, DTA_Fields.POS_avg_cost_calculation_mode},

            };

        }


        #region designer

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        Label label3;
        Label label2;
        Flat_Check_Box chb_zero_priced_items;
        Flat_Check_Box chb_duplicate_items;
        NumericUpDown nupd_day_index;
        Label label1;
        Label label4;
        Label label5;
        Label label6;
        Label label7;
        Label label8;
        Flat_Check_Box chb_select_client_before_data;
        Label label13;
        Flat_Check_Box chb_no_payment;
        Label label10;
        Flat_Check_Box chb_allow_credit_refunds;
        ComboBox cb_avg_cost_calc;
        Label label9;

        System.ComponentModel.IContainer components = null;

        void InitializeComponent() {
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chb_zero_priced_items = new Common.Controls.Flat_Check_Box();
            this.chb_duplicate_items = new Common.Controls.Flat_Check_Box();
            this.nupd_day_index = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.chb_select_client_before_data = new Common.Controls.Flat_Check_Box();
            this.label13 = new System.Windows.Forms.Label();
            this.chb_no_payment = new Common.Controls.Flat_Check_Box();
            this.label10 = new System.Windows.Forms.Label();
            this.chb_allow_credit_refunds = new Common.Controls.Flat_Check_Box();
            this.cb_avg_cost_calc = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_day_index)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 13);
            this.label3.TabIndex = 17;
            this.label3.Tag = "";
            this.label3.Text = "Allow Zero-Priced Items:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(229, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Launch while Items with Duplicate Codes Exist:";
            // 
            // chb_zero_priced_items
            // 
            this.chb_zero_priced_items.AutoSize = true;
            this.chb_zero_priced_items.BackColor = System.Drawing.Color.White;
            this.chb_zero_priced_items.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_zero_priced_items.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_zero_priced_items.Location = new System.Drawing.Point(331, 47);
            this.chb_zero_priced_items.Name = "chb_zero_priced_items";
            this.chb_zero_priced_items.Size = new System.Drawing.Size(12, 11);
            this.chb_zero_priced_items.TabIndex = 20;
            this.chb_zero_priced_items.TabStop = false;
            this.chb_zero_priced_items.UseVisualStyleBackColor = false;
            // 
            // chb_duplicate_items
            // 
            this.chb_duplicate_items.AutoSize = true;
            this.chb_duplicate_items.BackColor = System.Drawing.Color.White;
            this.chb_duplicate_items.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_duplicate_items.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_duplicate_items.Location = new System.Drawing.Point(331, 22);
            this.chb_duplicate_items.Name = "chb_duplicate_items";
            this.chb_duplicate_items.Size = new System.Drawing.Size(12, 11);
            this.chb_duplicate_items.TabIndex = 10;
            this.chb_duplicate_items.TabStop = false;
            this.chb_duplicate_items.UseVisualStyleBackColor = false;
            // 
            // nupd_day_index
            // 
            this.nupd_day_index.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_day_index.Location = new System.Drawing.Point(295, 69);
            this.nupd_day_index.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nupd_day_index.Name = "nupd_day_index";
            this.nupd_day_index.Size = new System.Drawing.Size(48, 20);
            this.nupd_day_index.TabIndex = 24;
            this.nupd_day_index.Tag = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 51;
            this.label1.Text = "Current Day Index:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 461);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 13);
            this.label4.TabIndex = 53;
            this.label4.Text = "Default vertical distance: 26";
            this.label4.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 480);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 54;
            this.label5.Text = "Default indent: 16";
            this.label5.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 502);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(187, 13);
            this.label6.TabIndex = 55;
            this.label6.Text = "Default left offset: 10 (26 in group box)";
            this.label6.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 524);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(185, 13);
            this.label7.TabIndex = 56;
            this.label7.Text = "Default up offset: 20 (25 in group box)";
            this.label7.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 97);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(173, 13);
            this.label8.TabIndex = 57;
            this.label8.Text = "Select Customer before Data Entry:";
            // 
            // chb_select_client_before_data
            // 
            this.chb_select_client_before_data.AutoSize = true;
            this.chb_select_client_before_data.BackColor = System.Drawing.Color.White;
            this.chb_select_client_before_data.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_select_client_before_data.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_select_client_before_data.Location = new System.Drawing.Point(331, 98);
            this.chb_select_client_before_data.Name = "chb_select_client_before_data";
            this.chb_select_client_before_data.Size = new System.Drawing.Size(12, 11);
            this.chb_select_client_before_data.TabIndex = 28;
            this.chb_select_client_before_data.TabStop = false;
            this.chb_select_client_before_data.Tag = "";
            this.chb_select_client_before_data.UseVisualStyleBackColor = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 121);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(126, 13);
            this.label13.TabIndex = 66;
            this.label13.Text = "No Payment for Invoices:";
            // 
            // chb_no_payment
            // 
            this.chb_no_payment.AutoSize = true;
            this.chb_no_payment.BackColor = System.Drawing.Color.White;
            this.chb_no_payment.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_no_payment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_no_payment.Location = new System.Drawing.Point(331, 122);
            this.chb_no_payment.Name = "chb_no_payment";
            this.chb_no_payment.Size = new System.Drawing.Size(12, 11);
            this.chb_no_payment.TabIndex = 65;
            this.chb_no_payment.TabStop = false;
            this.chb_no_payment.UseVisualStyleBackColor = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 146);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(134, 13);
            this.label10.TabIndex = 57;
            this.label10.Text = "Allow Credit Note Refunds:";
            // 
            // chb_allow_credit_refunds
            // 
            this.chb_allow_credit_refunds.AutoSize = true;
            this.chb_allow_credit_refunds.BackColor = System.Drawing.Color.White;
            this.chb_allow_credit_refunds.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_allow_credit_refunds.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_allow_credit_refunds.Location = new System.Drawing.Point(331, 147);
            this.chb_allow_credit_refunds.Name = "chb_allow_credit_refunds";
            this.chb_allow_credit_refunds.Size = new System.Drawing.Size(12, 11);
            this.chb_allow_credit_refunds.TabIndex = 28;
            this.chb_allow_credit_refunds.TabStop = false;
            this.chb_allow_credit_refunds.Tag = "";
            this.chb_allow_credit_refunds.UseVisualStyleBackColor = false;
            // 
            // cb_avg_cost_calc
            // 
            this.cb_avg_cost_calc.FormattingEnabled = true;
            this.cb_avg_cost_calc.Location = new System.Drawing.Point(184, 171);
            this.cb_avg_cost_calc.Name = "cb_avg_cost_calc";
            this.cb_avg_cost_calc.Size = new System.Drawing.Size(159, 21);
            this.cb_avg_cost_calc.TabIndex = 67;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 175);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(156, 13);
            this.label9.TabIndex = 68;
            this.label9.Text = "Average Cost Price Calculation:";
            // 
            // Tab_General
            // 
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cb_avg_cost_calc);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.chb_no_payment);
            this.Controls.Add(this.chb_allow_credit_refunds);
            this.Controls.Add(this.chb_select_client_before_data);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chb_zero_priced_items);
            this.Controls.Add(this.chb_duplicate_items);
            this.Controls.Add(this.nupd_day_index);
            this.Name = "Tab_General";
            this.Size = new System.Drawing.Size(570, 547);
            ((System.ComponentModel.ISupportInitialize)(this.nupd_day_index)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


    }
}

