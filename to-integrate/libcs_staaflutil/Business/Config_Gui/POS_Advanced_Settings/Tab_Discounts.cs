
using System.Collections.Generic;
using System.Windows.Forms;
using Common.Controls;
using DTA;
using Fairweather.Service;
namespace Config_Gui
{
    public partial class Tab_Discounts : DTA_Tab
    {
        public Tab_Discounts() {

            InitializeComponent();
            base.Setup();

        }

        public override Dictionary<Control, Ini_Field> Get_Fields() {
            return new Dictionary<Control, Ini_Field>
            {
                {this.cb_surcharge,DTA_Fields.POS_allow_surcharge},

                {this.nupd_line_disc_cash,DTA_Fields.POS_max_line_disc_perc_cashier},
                {this.nupd_line_disc_super,DTA_Fields.POS_max_line_disc_perc_super},

                {this.nupd_total_disc_cash,DTA_Fields.POS_max_total_disc_perc_cashier},
                {this.nupd_total_disc_super,DTA_Fields.POS_max_total_disc_perc_super},
                
                //{this.cb_disc_distribution_qty,DTA_Fields.POS_discount_distribution_mode},

                {this.cb_disc_distribution_qty, DTA_Fields.POS_discount_distribution_mode},
                {this.vb_rounding_disc, DTA_Fields.POS_max_rounding_discount_amount},

            };
        }

        #region designer

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        Label label1;
        Label label42;
        ComboBox cb_disc_distribution_qty;
        Label label9;
        ComboBox cb_surcharge;
        NumericUpDown nupd_total_disc_super;
        NumericUpDown nupd_line_disc_super;
        NumericUpDown nupd_total_disc_cash;
        NumericUpDown nupd_line_disc_cash;
        Label label21;
        Label label22;
        Label label23;
        Label label20;
        Label label16;
        Label label19;
        Validating_Box vb_rounding_disc;

        System.ComponentModel.IContainer components = null;

        void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.cb_disc_distribution_qty = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cb_surcharge = new System.Windows.Forms.ComboBox();
            this.nupd_total_disc_super = new System.Windows.Forms.NumericUpDown();
            this.nupd_line_disc_super = new System.Windows.Forms.NumericUpDown();
            this.nupd_total_disc_cash = new System.Windows.Forms.NumericUpDown();
            this.nupd_line_disc_cash = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.vb_rounding_disc = new Common.Controls.Validating_Box(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nupd_total_disc_super)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_line_disc_super)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_total_disc_cash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_line_disc_cash)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 202);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 13);
            this.label1.TabIndex = 44;
            this.label1.Text = "Maximum Rounding Discount:";
            this.label1.Visible = false;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(10, 20);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(161, 13);
            this.label42.TabIndex = 43;
            this.label42.Text = "Negative Discounts (Surcharge):";
            // 
            // cb_disc_distribution_qty
            // 
            this.cb_disc_distribution_qty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_disc_distribution_qty.FormattingEnabled = true;
            this.cb_disc_distribution_qty.Location = new System.Drawing.Point(217, 232);
            this.cb_disc_distribution_qty.Name = "cb_disc_distribution_qty";
            this.cb_disc_distribution_qty.Size = new System.Drawing.Size(88, 21);
            this.cb_disc_distribution_qty.TabIndex = 10;
            this.cb_disc_distribution_qty.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 236);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(134, 13);
            this.label9.TabIndex = 31;
            this.label9.Text = "Discount distribution mode:";
            this.label9.Visible = false;
            // 
            // cb_surcharge
            // 
            this.cb_surcharge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_surcharge.FormattingEnabled = true;
            this.cb_surcharge.Location = new System.Drawing.Point(217, 16);
            this.cb_surcharge.Name = "cb_surcharge";
            this.cb_surcharge.Size = new System.Drawing.Size(88, 21);
            this.cb_surcharge.TabIndex = 15;
            this.cb_surcharge.Tag = "";
            // 
            // nupd_total_disc_super
            // 
            this.nupd_total_disc_super.Location = new System.Drawing.Point(259, 172);
            this.nupd_total_disc_super.Name = "nupd_total_disc_super";
            this.nupd_total_disc_super.Size = new System.Drawing.Size(48, 20);
            this.nupd_total_disc_super.TabIndex = 50;
            this.nupd_total_disc_super.Tag = "";
            // 
            // nupd_line_disc_super
            // 
            this.nupd_line_disc_super.Location = new System.Drawing.Point(259, 94);
            this.nupd_line_disc_super.Name = "nupd_line_disc_super";
            this.nupd_line_disc_super.Size = new System.Drawing.Size(48, 20);
            this.nupd_line_disc_super.TabIndex = 30;
            this.nupd_line_disc_super.Tag = "";
            // 
            // nupd_total_disc_cash
            // 
            this.nupd_total_disc_cash.Location = new System.Drawing.Point(259, 146);
            this.nupd_total_disc_cash.Name = "nupd_total_disc_cash";
            this.nupd_total_disc_cash.Size = new System.Drawing.Size(48, 20);
            this.nupd_total_disc_cash.TabIndex = 40;
            this.nupd_total_disc_cash.Tag = "";
            // 
            // nupd_line_disc_cash
            // 
            this.nupd_line_disc_cash.Location = new System.Drawing.Point(259, 68);
            this.nupd_line_disc_cash.Name = "nupd_line_disc_cash";
            this.nupd_line_disc_cash.Size = new System.Drawing.Size(48, 20);
            this.nupd_line_disc_cash.TabIndex = 20;
            this.nupd_line_disc_cash.Tag = "";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(26, 176);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(60, 13);
            this.label21.TabIndex = 36;
            this.label21.Text = "Supervisor:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(26, 150);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(45, 13);
            this.label22.TabIndex = 35;
            this.label22.Text = "Cashier:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(10, 124);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(144, 13);
            this.label23.TabIndex = 34;
            this.label23.Text = "Maximum overall Discount %:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(26, 98);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(60, 13);
            this.label20.TabIndex = 30;
            this.label20.Text = "Supervisor:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(10, 46);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(129, 13);
            this.label16.TabIndex = 32;
            this.label16.Text = "Maximum line Discount %:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(26, 72);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(45, 13);
            this.label19.TabIndex = 33;
            this.label19.Text = "Cashier:";
            // 
            // vb_rounding_disc
            // 
            this.vb_rounding_disc.Auto_Highlight = false;
            this.vb_rounding_disc.AutoHighlight = false;
            this.vb_rounding_disc.BackColor = System.Drawing.Color.White;
            this.vb_rounding_disc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vb_rounding_disc.ChangeAlignmentOnEnter = false;
            this.vb_rounding_disc.Decimal_Places = 0;
            this.vb_rounding_disc.Default_Format = "F0";
            this.vb_rounding_disc.Has_User_Typed_Text = false;
            this.vb_rounding_disc.Is_Readonly = false;
            this.vb_rounding_disc.Location = new System.Drawing.Point(207, 200);
            this.vb_rounding_disc.MaxLength = 11;
            this.vb_rounding_disc.Name = "vb_rounding_disc";
            this.vb_rounding_disc.Read_Only_Mode = false;
            this.vb_rounding_disc.Right_Padding = 0;
            this.vb_rounding_disc.Size = new System.Drawing.Size(100, 20);
            this.vb_rounding_disc.TabIndex = 51;
            this.vb_rounding_disc.TabOnEnter = false;
            this.vb_rounding_disc.Text = "0";
            this.vb_rounding_disc.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.vb_rounding_disc.Visible = false;
            // 
            // Tab_Discounts
            // 
            this.Controls.Add(this.vb_rounding_disc);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label42);
            this.Controls.Add(this.cb_disc_distribution_qty);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cb_surcharge);
            this.Controls.Add(this.nupd_total_disc_super);
            this.Controls.Add(this.nupd_line_disc_super);
            this.Controls.Add(this.nupd_total_disc_cash);
            this.Controls.Add(this.nupd_line_disc_cash);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label16);
            this.Name = "Tab_Discounts";
            this.Size = new System.Drawing.Size(570, 547);
            ((System.ComponentModel.ISupportInitialize)(this.nupd_total_disc_super)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_line_disc_super)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_total_disc_cash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_line_disc_cash)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


    }
}

