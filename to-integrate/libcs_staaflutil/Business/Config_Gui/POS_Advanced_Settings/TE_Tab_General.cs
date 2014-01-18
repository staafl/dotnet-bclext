using System.Collections.Generic;
using System.Windows.Forms;

using Common.Controls;
using DTA;
using Fairweather.Service;

namespace Config_Gui
{
    public partial class TE_Tab_General : DTA_Tab
    {
        public TE_Tab_General() {

            InitializeComponent();


        }


        public override Dictionary<Control, Ini_Field> Get_Fields() {
            return new Dictionary<Control, Ini_Field>{

                {cb_sales_ref_check, DTA_Fields.TE_sales_ref_check},
                {cb_purchase_ref_check, DTA_Fields.TE_purchase_ref_check},
                {chb_allow_jx_against_ctrl, DTA_Fields.TE_jx_against_debtors_creditors_ctrl},
                {chb_posted_report, DTA_Fields.TE_posted_report},
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
        private ComboBox cb_sales_ref_check;
        private ComboBox cb_purchase_ref_check;
        private Label label1;
        private CheckBox chb_allow_jx_against_ctrl;
        private Label label4;
        private CheckBox chb_posted_report;

        System.ComponentModel.IContainer components = null;

        void InitializeComponent() {
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_sales_ref_check = new System.Windows.Forms.ComboBox();
            this.cb_purchase_ref_check = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chb_allow_jx_against_ctrl = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chb_posted_report = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(243, 13);
            this.label3.TabIndex = 17;
            this.label3.Tag = "";
            this.label3.Text = "Check document references for Purchase Ledger:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Check document references for Sales Ledger:";
            // 
            // cb_sales_ref_check
            // 
            this.cb_sales_ref_check.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_sales_ref_check.FormattingEnabled = true;
            this.cb_sales_ref_check.Items.AddRange(new object[] {
            "No",
            "On Demand",
            "Auto"});
            this.cb_sales_ref_check.Location = new System.Drawing.Point(358, 16);
            this.cb_sales_ref_check.Name = "cb_sales_ref_check";
            this.cb_sales_ref_check.Size = new System.Drawing.Size(121, 21);
            this.cb_sales_ref_check.TabIndex = 69;
            // 
            // cb_purchase_ref_check
            // 
            this.cb_purchase_ref_check.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_purchase_ref_check.FormattingEnabled = true;
            this.cb_purchase_ref_check.Items.AddRange(new object[] {
            "No",
            "On Demand",
            "Auto"});
            this.cb_purchase_ref_check.Location = new System.Drawing.Point(358, 42);
            this.cb_purchase_ref_check.Name = "cb_purchase_ref_check";
            this.cb_purchase_ref_check.Size = new System.Drawing.Size(121, 21);
            this.cb_purchase_ref_check.TabIndex = 69;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 13);
            this.label1.TabIndex = 17;
            this.label1.Tag = "";
            this.label1.Text = "Allow JC/JD against Creditors/Debtors control:";
            // 
            // chb_allow_jx_against_ctrl
            // 
            this.chb_allow_jx_against_ctrl.AutoSize = true;
            this.chb_allow_jx_against_ctrl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_allow_jx_against_ctrl.Location = new System.Drawing.Point(468, 73);
            this.chb_allow_jx_against_ctrl.Name = "chb_allow_jx_against_ctrl";
            this.chb_allow_jx_against_ctrl.Size = new System.Drawing.Size(12, 11);
            this.chb_allow_jx_against_ctrl.TabIndex = 70;
            this.chb_allow_jx_against_ctrl.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 13);
            this.label4.TabIndex = 71;
            this.label4.Tag = "";
            this.label4.Text = "Posted Transactions Report:";
            // 
            // chb_posted_report
            // 
            this.chb_posted_report.AutoSize = true;
            this.chb_posted_report.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_posted_report.Location = new System.Drawing.Point(468, 99);
            this.chb_posted_report.Name = "chb_posted_report";
            this.chb_posted_report.Size = new System.Drawing.Size(12, 11);
            this.chb_posted_report.TabIndex = 70;
            this.chb_posted_report.UseVisualStyleBackColor = true;
            // 
            // TE_Tab_General
            // 
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chb_posted_report);
            this.Controls.Add(this.chb_allow_jx_against_ctrl);
            this.Controls.Add(this.cb_purchase_ref_check);
            this.Controls.Add(this.cb_sales_ref_check);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "TE_Tab_General";
            this.Size = new System.Drawing.Size(570, 154);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


    }
}

