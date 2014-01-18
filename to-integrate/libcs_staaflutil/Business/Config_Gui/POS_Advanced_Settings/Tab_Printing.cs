using System.Collections.Generic;

using System.Windows.Forms;

using Common.Controls;
using DTA;
using Fairweather.Service;

namespace Config_Gui
{
    public partial class Tab_Printing : DTA_Tab
    {
        public Tab_Printing() {

            InitializeComponent();

            //Standard.Flat_Style(cb_provider);
            //Standard.Flat_Style(cb_layout);
            cb_layout.SelectedIndex = 0;
        }


        public ComboBox Cb_provider {
            get { return cb_provider; }
        }
        public Button But_configure {
            get { return but_configure; }
        }
        public Button But_test {
            get { return but_test; }
        }
        public Button But_edit {
              get { return but_edit; }
        }

        public override Dictionary<Control, Ini_Field> Get_Fields() {

            return new Dictionary<Control, Ini_Field>{
                {cb_provider, DTA_Fields.POS_printing_provider},
                        {nupd_copies_sales, DTA_Fields.POS_printout_count_sale},
                        {nupd_copies_receipts, DTA_Fields.POS_printout_count_receipt},
                        {nupd_copies_eos, DTA_Fields.POS_printout_count_eos},

            };

        }


        #region designer

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        Button but_configure;


        Fake_Group_Box fake_Group_Box2;
        Label label1;
        ComboBox cb_provider;
        Panel panel1;
        Button but_test;
        Button but_edit;
        Fake_Group_Box fake_Group_Box3;
        ComboBox cb_layout;
        Panel panel2;
        Label label28;
        Label label30;
        Label label31;
        NumericUpDown nupd_copies_sales;
        NumericUpDown nupd_copies_receipts;
        NumericUpDown nupd_copies_eos;
        Fake_Group_Box fake_Group_Box1;





        System.ComponentModel.IContainer components = null;

        void InitializeComponent() {
            this.but_configure = new System.Windows.Forms.Button();
            this.fake_Group_Box2 = new Common.Controls.Fake_Group_Box();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_provider = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.but_test = new System.Windows.Forms.Button();
            this.but_edit = new System.Windows.Forms.Button();
            this.fake_Group_Box3 = new Common.Controls.Fake_Group_Box();
            this.cb_layout = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label28 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.nupd_copies_sales = new System.Windows.Forms.NumericUpDown();
            this.nupd_copies_receipts = new System.Windows.Forms.NumericUpDown();
            this.nupd_copies_eos = new System.Windows.Forms.NumericUpDown();
            this.fake_Group_Box1 = new Common.Controls.Fake_Group_Box();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_copies_sales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_copies_receipts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_copies_eos)).BeginInit();
            this.SuspendLayout();
            // 
            // but_configure
            // 
            this.but_configure.Location = new System.Drawing.Point(158, 86);
            this.but_configure.Name = "but_configure";
            this.but_configure.Size = new System.Drawing.Size(75, 23);
            this.but_configure.TabIndex = 10;
            this.but_configure.Text = "Configure";
            this.but_configure.UseVisualStyleBackColor = true;
            // 
            // fake_Group_Box2
            // 
            this.fake_Group_Box2.Location = new System.Drawing.Point(5, 19);
            this.fake_Group_Box2.Name = "fake_Group_Box2";
            this.fake_Group_Box2.Size = new System.Drawing.Size(400, 107);
            this.fake_Group_Box2.TabIndex = 0;
            this.fake_Group_Box2.Text = "Printing Provider";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 58;
            this.label1.Text = "Name:";
            // 
            // cb_provider
            // 
            this.cb_provider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_provider.FormattingEnabled = true;
            this.cb_provider.Location = new System.Drawing.Point(23, 4);
            this.cb_provider.Name = "cb_provider";
            this.cb_provider.Size = new System.Drawing.Size(154, 21);
            this.cb_provider.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cb_provider);
            this.panel1.Location = new System.Drawing.Point(135, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(204, 41);
            this.panel1.TabIndex = 0;
            // 
            // but_test
            // 
            this.but_test.Location = new System.Drawing.Point(239, 86);
            this.but_test.Name = "but_test";
            this.but_test.Size = new System.Drawing.Size(75, 23);
            this.but_test.TabIndex = 20;
            this.but_test.Text = "Test";
            this.but_test.UseVisualStyleBackColor = true;
            // 
            // but_edit
            // 
            this.but_edit.Location = new System.Drawing.Point(206, 192);
            this.but_edit.Name = "but_edit";
            this.but_edit.Size = new System.Drawing.Size(75, 23);
            this.but_edit.TabIndex = 40;
            this.but_edit.TabStop = false;
            this.but_edit.Text = "Edit";
            this.but_edit.UseVisualStyleBackColor = true;
            // 
            // fake_Group_Box3
            // 
            this.fake_Group_Box3.Location = new System.Drawing.Point(5, 125);
            this.fake_Group_Box3.Name = "fake_Group_Box3";
            this.fake_Group_Box3.Size = new System.Drawing.Size(400, 107);
            this.fake_Group_Box3.TabIndex = 6;
            this.fake_Group_Box3.Text = "Layout";
            // 
            // cb_layout
            // 
            this.cb_layout.FormattingEnabled = true;
            this.cb_layout.Items.AddRange(new object[] {
            "Standard Vertical"});
            this.cb_layout.Location = new System.Drawing.Point(26, 10);
            this.cb_layout.Name = "cb_layout";
            this.cb_layout.Size = new System.Drawing.Size(121, 21);
            this.cb_layout.TabIndex = 30;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cb_layout);
            this.panel2.Location = new System.Drawing.Point(134, 144);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(205, 42);
            this.panel2.TabIndex = 30;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(26, 256);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(81, 13);
            this.label28.TabIndex = 66;
            this.label28.Text = "Sales Receipts:";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(26, 282);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(110, 13);
            this.label30.TabIndex = 68;
            this.label30.Text = "Receipts on Account:";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(26, 308);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(95, 13);
            this.label31.TabIndex = 67;
            this.label31.Text = "End of Shift report:";
            // 
            // nupd_copies_sales
            // 
            this.nupd_copies_sales.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_copies_sales.Location = new System.Drawing.Point(232, 252);
            this.nupd_copies_sales.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nupd_copies_sales.Name = "nupd_copies_sales";
            this.nupd_copies_sales.Size = new System.Drawing.Size(48, 20);
            this.nupd_copies_sales.TabIndex = 50;
            this.nupd_copies_sales.Tag = "";
            // 
            // nupd_copies_receipts
            // 
            this.nupd_copies_receipts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_copies_receipts.Location = new System.Drawing.Point(232, 278);
            this.nupd_copies_receipts.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nupd_copies_receipts.Name = "nupd_copies_receipts";
            this.nupd_copies_receipts.Size = new System.Drawing.Size(48, 20);
            this.nupd_copies_receipts.TabIndex = 60;
            this.nupd_copies_receipts.Tag = "";
            // 
            // nupd_copies_eos
            // 
            this.nupd_copies_eos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_copies_eos.Location = new System.Drawing.Point(232, 304);
            this.nupd_copies_eos.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nupd_copies_eos.Name = "nupd_copies_eos";
            this.nupd_copies_eos.Size = new System.Drawing.Size(48, 20);
            this.nupd_copies_eos.TabIndex = 70;
            this.nupd_copies_eos.Tag = "";
            // 
            // fake_Group_Box1
            // 
            this.fake_Group_Box1.Location = new System.Drawing.Point(5, 231);
            this.fake_Group_Box1.Name = "fake_Group_Box1";
            this.fake_Group_Box1.Size = new System.Drawing.Size(400, 107);
            this.fake_Group_Box1.TabIndex = 72;
            this.fake_Group_Box1.Text = "Printout Copies";
            // 
            // Tab_Printing
            // 
            this.Controls.Add(this.label28);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.nupd_copies_sales);
            this.Controls.Add(this.nupd_copies_receipts);
            this.Controls.Add(this.nupd_copies_eos);
            this.Controls.Add(this.fake_Group_Box1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.but_edit);
            this.Controls.Add(this.but_test);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.but_configure);
            this.Controls.Add(this.fake_Group_Box2);
            this.Controls.Add(this.fake_Group_Box3);
            this.Name = "Tab_Printing";
            this.Size = new System.Drawing.Size(570, 547);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nupd_copies_sales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_copies_receipts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_copies_eos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion




    }
}

