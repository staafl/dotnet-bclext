using System.Collections.Generic;
using System.Windows.Forms;
using Common.Controls;
using DTA;
using Fairweather.Service;

namespace Config_Gui
{
    public partial class Ses_Settings_Tab_2: DTA_Tab
    {
        public Ses_Settings_Tab_2() {

            InitializeComponent();


        }

        public override Dictionary<Control, Ini_Field> Get_Fields() {

            return new Dictionary<Control, Ini_Field>
            {
                {tb_receipt, DTA_Fields.ESF_purchase_receipt_details},
                {tb_discount, DTA_Fields.ESF_purchase_discount_details},

            };
        }

        #region designer

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        Label label2;
        TextBox tb_discount;
        Label label1;
        TextBox tb_receipt;

        System.ComponentModel.IContainer components = null;

        void InitializeComponent() {
            this.label2 = new System.Windows.Forms.Label();
            this.tb_discount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_receipt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Default Discount Details:";
            // 
            // tb_discount
            // 
            this.tb_discount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_discount.Location = new System.Drawing.Point(148, 51);
            this.tb_discount.Name = "tb_discount";
            this.tb_discount.Size = new System.Drawing.Size(210, 20);
            this.tb_discount.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Default Receipt Details:";
            // 
            // tb_receipt
            // 
            this.tb_receipt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_receipt.Location = new System.Drawing.Point(148, 25);
            this.tb_receipt.Name = "tb_receipt";
            this.tb_receipt.Size = new System.Drawing.Size(210, 20);
            this.tb_receipt.TabIndex = 5;
            // 
            // Settings_Tab_2
            // 
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_discount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_receipt);
            this.Name = "Settings_Tab_2";
            this.Size = new System.Drawing.Size(570, 547);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


    }
}

