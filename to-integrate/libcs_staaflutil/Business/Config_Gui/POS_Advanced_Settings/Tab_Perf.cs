using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Common.Controls;
using DTA;

using Fairweather.Service;

namespace Config_Gui
{
    public partial class Tab_Performance : DTA_Tab
    {


        public Tab_Performance() {

            InitializeComponent();

            cb_unposted_balance.DropDownClosed += cb_DropDownClosed;

        }

        void cb_DropDownClosed(object sender, EventArgs e) {
            Border.First_Instance(cb_days_documents).Refresh();

        }


        public override Dictionary<Control, Ini_Field> Get_Fields() {
            return new Dictionary<Control, Ini_Field>() 
            {
                {cb_days_documents, DTA_Fields.POS_calculate_days_documents_scanning},
                {cb_unposted_balance, DTA_Fields.POS_calculate_unposted_balance_scanning},
                {chb_cache_accounts, DTA_Fields.POS_cache_customer_accounts},
                {chb_calculate_unposted, DTA_Fields.POS_calculate_unposted},

                {chb_calculate_unposted_stock, DTA_Fields.POS_auto_calculate_unposted_stock},
                {cb_auto_sales_history, DTA_Fields.POS_auto_sales_history_display},

                {chb_confirm_invoice, DTA_Fields.POS_confirm_invoice},
                {chb_confirm_receipt, DTA_Fields.POS_confirm_receipt},
                {chb_confirm_payment, DTA_Fields.POS_confirm_payment},
            };
        }



        #region designer

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        Label label12;
        Flat_Check_Box chb_cache_accounts;
        Label label11;
        Label label10;
        ComboBox cb_days_documents;
        ComboBox cb_unposted_balance;
        Label label1;
        Flat_Check_Box chb_calculate_unposted;
        Label label2;
        Flat_Check_Box chb_calculate_unposted_stock;
        Label label3;
        ComboBox cb_auto_sales_history;
        private Label label4;
        private Flat_Check_Box chb_confirm_invoice;
        private Fake_Group_Box fake_Group_Box2;
        private Flat_Check_Box chb_confirm_receipt;
        private Label label5;
        private Flat_Check_Box chb_confirm_payment;
        private Label label6;


        System.ComponentModel.IContainer components = null;



        #endregion

        void InitializeComponent() {
            this.label12 = new System.Windows.Forms.Label();
            this.chb_cache_accounts = new Common.Controls.Flat_Check_Box();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cb_days_documents = new System.Windows.Forms.ComboBox();
            this.cb_unposted_balance = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chb_calculate_unposted = new Common.Controls.Flat_Check_Box();
            this.label2 = new System.Windows.Forms.Label();
            this.chb_calculate_unposted_stock = new Common.Controls.Flat_Check_Box();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_auto_sales_history = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chb_confirm_invoice = new Common.Controls.Flat_Check_Box();
            this.fake_Group_Box2 = new Common.Controls.Fake_Group_Box();
            this.chb_confirm_receipt = new Common.Controls.Flat_Check_Box();
            this.label5 = new System.Windows.Forms.Label();
            this.chb_confirm_payment = new Common.Controls.Flat_Check_Box();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 77);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(136, 13);
            this.label12.TabIndex = 70;
            this.label12.Text = "Cache Customer Accounts:";
            // 
            // chb_cache_accounts
            // 
            this.chb_cache_accounts.AutoSize = true;
            this.chb_cache_accounts.BackColor = System.Drawing.Color.White;
            this.chb_cache_accounts.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_cache_accounts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_cache_accounts.Location = new System.Drawing.Point(363, 78);
            this.chb_cache_accounts.Name = "chb_cache_accounts";
            this.chb_cache_accounts.Size = new System.Drawing.Size(12, 11);
            this.chb_cache_accounts.TabIndex = 69;
            this.chb_cache_accounts.TabStop = false;
            this.chb_cache_accounts.UseVisualStyleBackColor = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(141, 13);
            this.label11.TabIndex = 68;
            this.label11.Text = "Day\'s Documents Scanning:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(146, 13);
            this.label10.TabIndex = 67;
            this.label10.Text = "Unposted Balance Scanning:";
            // 
            // cb_days_documents
            // 
            this.cb_days_documents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_days_documents.FormattingEnabled = true;
            this.cb_days_documents.Location = new System.Drawing.Point(224, 44);
            this.cb_days_documents.Name = "cb_days_documents";
            this.cb_days_documents.Size = new System.Drawing.Size(151, 21);
            this.cb_days_documents.TabIndex = 1;
            // 
            // cb_unposted_balance
            // 
            this.cb_unposted_balance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_unposted_balance.FormattingEnabled = true;
            this.cb_unposted_balance.Location = new System.Drawing.Point(224, 16);
            this.cb_unposted_balance.Name = "cb_unposted_balance";
            this.cb_unposted_balance.Size = new System.Drawing.Size(151, 21);
            this.cb_unposted_balance.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(257, 13);
            this.label1.TabIndex = 72;
            this.label1.Text = "Calculate Unposted Customer Balance Automatically:";
            // 
            // chb_calculate_unposted
            // 
            this.chb_calculate_unposted.AutoSize = true;
            this.chb_calculate_unposted.BackColor = System.Drawing.Color.White;
            this.chb_calculate_unposted.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_calculate_unposted.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_calculate_unposted.Location = new System.Drawing.Point(363, 104);
            this.chb_calculate_unposted.Name = "chb_calculate_unposted";
            this.chb_calculate_unposted.Size = new System.Drawing.Size(12, 11);
            this.chb_calculate_unposted.TabIndex = 71;
            this.chb_calculate_unposted.TabStop = false;
            this.chb_calculate_unposted.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(241, 13);
            this.label2.TabIndex = 74;
            this.label2.Text = "Calculate Unposted Stock Balance Automatically:";
            // 
            // chb_calculate_unposted_stock
            // 
            this.chb_calculate_unposted_stock.AutoSize = true;
            this.chb_calculate_unposted_stock.BackColor = System.Drawing.Color.White;
            this.chb_calculate_unposted_stock.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_calculate_unposted_stock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_calculate_unposted_stock.Location = new System.Drawing.Point(363, 131);
            this.chb_calculate_unposted_stock.Name = "chb_calculate_unposted_stock";
            this.chb_calculate_unposted_stock.Size = new System.Drawing.Size(12, 11);
            this.chb_calculate_unposted_stock.TabIndex = 73;
            this.chb_calculate_unposted_stock.TabStop = false;
            this.chb_calculate_unposted_stock.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 13);
            this.label3.TabIndex = 76;
            this.label3.Text = "Automatic Display of Sales History:";
            // 
            // cb_auto_sales_history
            // 
            this.cb_auto_sales_history.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_auto_sales_history.FormattingEnabled = true;
            this.cb_auto_sales_history.Location = new System.Drawing.Point(224, 152);
            this.cb_auto_sales_history.Name = "cb_auto_sales_history";
            this.cb_auto_sales_history.Size = new System.Drawing.Size(151, 21);
            this.cb_auto_sales_history.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 13);
            this.label4.TabIndex = 78;
            this.label4.Text = "After \'Complete Transaction\':";
            // 
            // chb_confirm_invoice
            // 
            this.chb_confirm_invoice.AutoSize = true;
            this.chb_confirm_invoice.BackColor = System.Drawing.Color.White;
            this.chb_confirm_invoice.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_confirm_invoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_confirm_invoice.Location = new System.Drawing.Point(363, 213);
            this.chb_confirm_invoice.Name = "chb_confirm_invoice";
            this.chb_confirm_invoice.Size = new System.Drawing.Size(12, 11);
            this.chb_confirm_invoice.TabIndex = 77;
            this.chb_confirm_invoice.TabStop = false;
            this.chb_confirm_invoice.UseVisualStyleBackColor = false;
            // 
            // fake_Group_Box2
            // 
            this.fake_Group_Box2.Location = new System.Drawing.Point(5, 184);
            this.fake_Group_Box2.Name = "fake_Group_Box2";
            this.fake_Group_Box2.Size = new System.Drawing.Size(400, 107);
            this.fake_Group_Box2.TabIndex = 79;
            this.fake_Group_Box2.Text = "Show Confirmation Dialog";
            // 
            // chb_confirm_receipt
            // 
            this.chb_confirm_receipt.AutoSize = true;
            this.chb_confirm_receipt.BackColor = System.Drawing.Color.White;
            this.chb_confirm_receipt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_confirm_receipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_confirm_receipt.Location = new System.Drawing.Point(363, 235);
            this.chb_confirm_receipt.Name = "chb_confirm_receipt";
            this.chb_confirm_receipt.Size = new System.Drawing.Size(12, 11);
            this.chb_confirm_receipt.TabIndex = 77;
            this.chb_confirm_receipt.TabStop = false;
            this.chb_confirm_receipt.UseVisualStyleBackColor = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 234);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 78;
            this.label5.Text = "After \'Receipts\':";
            // 
            // chb_confirm_payment
            // 
            this.chb_confirm_payment.AutoSize = true;
            this.chb_confirm_payment.BackColor = System.Drawing.Color.White;
            this.chb_confirm_payment.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_confirm_payment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_confirm_payment.Location = new System.Drawing.Point(363, 258);
            this.chb_confirm_payment.Name = "chb_confirm_payment";
            this.chb_confirm_payment.Size = new System.Drawing.Size(12, 11);
            this.chb_confirm_payment.TabIndex = 77;
            this.chb_confirm_payment.TabStop = false;
            this.chb_confirm_payment.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 257);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 78;
            this.label6.Text = "After \'Payments\':";
            // 
            // Tab_Performance
            // 
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chb_confirm_payment);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chb_confirm_receipt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chb_confirm_invoice);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chb_calculate_unposted_stock);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chb_calculate_unposted);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.chb_cache_accounts);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cb_auto_sales_history);
            this.Controls.Add(this.cb_days_documents);
            this.Controls.Add(this.cb_unposted_balance);
            this.Controls.Add(this.fake_Group_Box2);
            this.Name = "Tab_Performance";
            this.Size = new System.Drawing.Size(435, 441);
            this.ResumeLayout(false);
            this.PerformLayout();

        }







    }
}

