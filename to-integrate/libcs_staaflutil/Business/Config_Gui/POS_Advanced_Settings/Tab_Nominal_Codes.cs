using System.Collections.Generic;
using System.Windows.Forms;
using Common.Controls;
using DTA;
using Fairweather.Service;

namespace Config_Gui
{
      public partial class Tab_Nominal_Codes : DTA_Tab
      {

            public Tab_Nominal_Codes() {

                  InitializeComponent();


            }
            public override Dictionary<Control, Ini_Field> Get_Fields() {

                  return new Dictionary<Control, Ini_Field> {
                {tb_def_receipts_bank, DTA_Fields.POS_def_sales_bank},
                {tb_def_cash_acc, DTA_Fields.POS_def_cash_account},
                {tb_def_payments_acc, DTA_Fields.POS_def_payments_account},
                {chb_payments_account_locked, DTA_Fields.POS_cash_payments_expense_account_locked},
                {chb_receipts_bank_locked, DTA_Fields.POS_receipts_account_locked},
                {chb_cash_account_auto, DTA_Fields.POS_select_cash_account_automatically},
                {tb_restricted_access, DTA_Fields.POS_restrict_access_to},

            };
            }

            #region designer

            protected override void Dispose(bool disposing) {
                  if (disposing && (components != null)) {
                        components.Dispose();
                  }
                  base.Dispose(disposing);
            }

            Flat_Check_Box chb_receipts_bank_locked;
            Label label18;
            Flat_Check_Box chb_payments_account_locked;
            Label label35;


            TextBox tb_def_receipts_bank;
            TextBox tb_def_cash_acc;
            TextBox tb_def_payments_acc;

     
            Label label27;
            Label label26;
            Label lab;
            Label label9;
            Flat_Check_Box chb_cash_account_auto;
            public TextBox tb_restricted_access;
            private Label label1;
            private Label label2;

            System.ComponentModel.IContainer components = null;

            void InitializeComponent() {
                this.chb_receipts_bank_locked = new Common.Controls.Flat_Check_Box();
                this.label18 = new System.Windows.Forms.Label();
                this.chb_payments_account_locked = new Common.Controls.Flat_Check_Box();
                this.label35 = new System.Windows.Forms.Label();
                this.tb_def_payments_acc = new System.Windows.Forms.TextBox();
                this.tb_def_receipts_bank = new System.Windows.Forms.TextBox();
                this.tb_def_cash_acc = new System.Windows.Forms.TextBox();
                this.label27 = new System.Windows.Forms.Label();
                this.label26 = new System.Windows.Forms.Label();
                this.lab = new System.Windows.Forms.Label();
                this.label9 = new System.Windows.Forms.Label();
                this.chb_cash_account_auto = new Common.Controls.Flat_Check_Box();
                this.tb_restricted_access = new System.Windows.Forms.TextBox();
                this.label1 = new System.Windows.Forms.Label();
                this.label2 = new System.Windows.Forms.Label();
                this.SuspendLayout();
                // 
                // chb_receipts_bank_locked
                // 
                this.chb_receipts_bank_locked.AutoSize = true;
                this.chb_receipts_bank_locked.BackColor = System.Drawing.Color.White;
                this.chb_receipts_bank_locked.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
                this.chb_receipts_bank_locked.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.chb_receipts_bank_locked.Location = new System.Drawing.Point(309, 99);
                this.chb_receipts_bank_locked.Name = "chb_receipts_bank_locked";
                this.chb_receipts_bank_locked.Size = new System.Drawing.Size(12, 11);
                this.chb_receipts_bank_locked.TabIndex = 12;
                this.chb_receipts_bank_locked.TabStop = false;
                this.chb_receipts_bank_locked.UseVisualStyleBackColor = false;
                // 
                // label18
                // 
                this.label18.AutoSize = true;
                this.label18.Location = new System.Drawing.Point(10, 98);
                this.label18.Name = "label18";
                this.label18.Size = new System.Drawing.Size(158, 13);
                this.label18.TabIndex = 35;
                this.label18.Text = "Receipts Bank Account locked:";
                // 
                // chb_payments_account_locked
                // 
                this.chb_payments_account_locked.AutoSize = true;
                this.chb_payments_account_locked.BackColor = System.Drawing.Color.White;
                this.chb_payments_account_locked.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
                this.chb_payments_account_locked.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.chb_payments_account_locked.Location = new System.Drawing.Point(309, 125);
                this.chb_payments_account_locked.Name = "chb_payments_account_locked";
                this.chb_payments_account_locked.Size = new System.Drawing.Size(12, 11);
                this.chb_payments_account_locked.TabIndex = 20;
                this.chb_payments_account_locked.TabStop = false;
                this.chb_payments_account_locked.UseVisualStyleBackColor = false;
                // 
                // label35
                // 
                this.label35.AutoSize = true;
                this.label35.Location = new System.Drawing.Point(10, 124);
                this.label35.Name = "label35";
                this.label35.Size = new System.Drawing.Size(178, 13);
                this.label35.TabIndex = 34;
                this.label35.Text = "Payments Expense Account locked:";
                // 
                // tb_def_payments_acc
                // 
                this.tb_def_payments_acc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.tb_def_payments_acc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
                this.tb_def_payments_acc.Location = new System.Drawing.Point(211, 68);
                this.tb_def_payments_acc.MaxLength = 8;
                this.tb_def_payments_acc.Name = "tb_def_payments_acc";
                this.tb_def_payments_acc.Size = new System.Drawing.Size(110, 20);
                this.tb_def_payments_acc.TabIndex = 9;
                // 
                // tb_def_receipts_bank
                // 
                this.tb_def_receipts_bank.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.tb_def_receipts_bank.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
                this.tb_def_receipts_bank.Location = new System.Drawing.Point(211, 16);
                this.tb_def_receipts_bank.MaxLength = 8;
                this.tb_def_receipts_bank.Name = "tb_def_receipts_bank";
                this.tb_def_receipts_bank.Size = new System.Drawing.Size(110, 20);
                this.tb_def_receipts_bank.TabIndex = 3;
                // 
                // tb_def_cash_acc
                // 
                this.tb_def_cash_acc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.tb_def_cash_acc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
                this.tb_def_cash_acc.Location = new System.Drawing.Point(211, 42);
                this.tb_def_cash_acc.MaxLength = 8;
                this.tb_def_cash_acc.Name = "tb_def_cash_acc";
                this.tb_def_cash_acc.Size = new System.Drawing.Size(110, 20);
                this.tb_def_cash_acc.TabIndex = 6;
                // 
                // label27
                // 
                this.label27.AutoSize = true;
                this.label27.Location = new System.Drawing.Point(10, 72);
                this.label27.Name = "label27";
                this.label27.Size = new System.Drawing.Size(180, 13);
                this.label27.TabIndex = 25;
                this.label27.Text = "Default Payments Expense Account:";
                // 
                // label26
                // 
                this.label26.AutoSize = true;
                this.label26.Location = new System.Drawing.Point(10, 20);
                this.label26.Name = "label26";
                this.label26.Size = new System.Drawing.Size(160, 13);
                this.label26.TabIndex = 26;
                this.label26.Text = "Default Receipts Bank Account:";
                // 
                // lab
                // 
                this.lab.AutoSize = true;
                this.lab.Location = new System.Drawing.Point(10, 46);
                this.lab.Name = "lab";
                this.lab.Size = new System.Drawing.Size(143, 13);
                this.lab.TabIndex = 27;
                this.lab.Text = "Default Cash Sales Account:";
                // 
                // label9
                // 
                this.label9.AutoSize = true;
                this.label9.Location = new System.Drawing.Point(10, 150);
                this.label9.Name = "label9";
                this.label9.Size = new System.Drawing.Size(184, 13);
                this.label9.TabIndex = 59;
                this.label9.Text = "Cash Account selected automatically:";
                // 
                // chb_cash_account_auto
                // 
                this.chb_cash_account_auto.AutoSize = true;
                this.chb_cash_account_auto.BackColor = System.Drawing.Color.White;
                this.chb_cash_account_auto.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
                this.chb_cash_account_auto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.chb_cash_account_auto.Location = new System.Drawing.Point(309, 151);
                this.chb_cash_account_auto.Name = "chb_cash_account_auto";
                this.chb_cash_account_auto.Size = new System.Drawing.Size(12, 11);
                this.chb_cash_account_auto.TabIndex = 60;
                this.chb_cash_account_auto.TabStop = false;
                this.chb_cash_account_auto.UseVisualStyleBackColor = false;
                // 
                // tb_restricted_access
                // 
                this.tb_restricted_access.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.tb_restricted_access.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
                this.tb_restricted_access.Location = new System.Drawing.Point(211, 172);
                this.tb_restricted_access.Name = "tb_restricted_access";
                this.tb_restricted_access.Size = new System.Drawing.Size(110, 20);
                this.tb_restricted_access.TabIndex = 61;
                // 
                // label1
                // 
                this.label1.AutoSize = true;
                this.label1.Location = new System.Drawing.Point(10, 176);
                this.label1.Name = "label1";
                this.label1.Size = new System.Drawing.Size(173, 13);
                this.label1.TabIndex = 62;
                this.label1.Text = "Restrict Access to Sales Accounts:";
                // 
                // label2
                // 
                this.label2.AutoSize = true;
                this.label2.Location = new System.Drawing.Point(10, 189);
                this.label2.Name = "label2";
                this.label2.Size = new System.Drawing.Size(187, 13);
                this.label2.TabIndex = 63;
                this.label2.Text = "(use ; to separate account references)";
                // 
                // Tab_Nominal_Codes
                // 
                this.Controls.Add(this.label2);
                this.Controls.Add(this.tb_restricted_access);
                this.Controls.Add(this.label1);
                this.Controls.Add(this.chb_cash_account_auto);
                this.Controls.Add(this.label9);
                this.Controls.Add(this.chb_receipts_bank_locked);
                this.Controls.Add(this.label18);
                this.Controls.Add(this.chb_payments_account_locked);
                this.Controls.Add(this.label35);
                this.Controls.Add(this.tb_def_payments_acc);
                this.Controls.Add(this.tb_def_receipts_bank);
                this.Controls.Add(this.tb_def_cash_acc);
                this.Controls.Add(this.label27);
                this.Controls.Add(this.label26);
                this.Controls.Add(this.lab);
                this.Name = "Tab_Nominal_Codes";
                this.Size = new System.Drawing.Size(344, 227);
                this.ResumeLayout(false);
                this.PerformLayout();

            }

            #endregion

            public TextBox Tb_def_payments_acc {
                  get { return tb_def_payments_acc; }
            }

            public TextBox Tb_def_receipts_bank {
                  get { return tb_def_receipts_bank; }
            }

            public TextBox Tb_def_cash_acc {
                  get { return tb_def_cash_acc; }
            }




      }
}

