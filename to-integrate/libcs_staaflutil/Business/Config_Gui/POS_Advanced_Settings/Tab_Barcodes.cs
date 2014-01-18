using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Common;
using Common.Controls;
using DTA;
using Fairweather.Service;
using Standardization;

namespace Config_Gui
{
    public partial class Tab_Barcodes : DTA_Tab
    {

        // Older mechanism for combo-box synchronization removed by me
        // // -- velko, 05/10/2009

        Multi_Combobox_Manager cb_man;
        Panel panel1;


        static readonly string[] rd_combo_items;

        static Tab_Barcodes() {

            var type = DTA_Combo_Box_Type.Barcode;
            string[] _;
            DTA_Controls.Get_Combo_Box_Items(type, out _, out rd_combo_items);

        }

        public Tab_Barcodes() {

            InitializeComponent();

            var cb_to_chb = new Dictionary<ComboBox, CheckBox>() 
            {
                {cb_barcode_1, chb_barcode_1},
                {cb_barcode_2, chb_barcode_2},
                {cb_barcode_3, chb_barcode_3},
            };

            Standard.Flat_Style(cb_barcode_1);
            Standard.Flat_Style(cb_barcode_2);
            Standard.Flat_Style(cb_barcode_3);

            var border1 = Border.Instances(cb_barcode_1).First();
            var border2 = Border.Instances(cb_barcode_2).First();
            var border3 = Border.Instances(cb_barcode_3).First();

            var handler = produce_handler(border1, border2, border3);
            cb_barcode_1.DropDownClosed += handler;
            cb_barcode_2.DropDownClosed += handler;
            cb_barcode_3.DropDownClosed += handler;

            cb_barcode_1.BringToFront();

            cb_man = new Multi_Combobox_Manager(cb_to_chb, rd_combo_items);
        }

        EventHandler produce_handler(params Control[] ctrls) {
            return (_, e) =>
            {
                foreach (var ctrl in ctrls) {
                    ctrl.Refresh();
                };
            };
        }

        public override Dictionary<Control, Ini_Field> Get_Fields() {
            return new Dictionary<Control, Ini_Field>() 
            {
                {chb_barcode_1, DTA_Fields.POS_use_barcode1},
                {chb_barcode_2, DTA_Fields.POS_use_barcode2},
                {chb_barcode_3, DTA_Fields.POS_use_barcode3},
                {cb_barcode_1,  DTA_Fields.POS_barcode1},
                {cb_barcode_2,  DTA_Fields.POS_barcode2},
                {cb_barcode_3,  DTA_Fields.POS_barcode3},
                {chb_wt_barcodes, DTA_Fields.POS_use_weight_barcodes},
                {chb_price_barcodes, DTA_Fields.POS_use_price_barcodes},
            };
        }



        #region designer

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        ComboBox cb_barcode_1;
        ComboBox cb_barcode_2;
        ComboBox cb_barcode_3;
        Label label4;
        Flat_Check_Box chb_wt_barcodes;
        Label label25;
        Label label24;
        Label label12;

        Flat_Check_Box chb_barcode_1;
        Flat_Check_Box chb_barcode_2;
        Flat_Check_Box chb_barcode_3;

        System.ComponentModel.IContainer components = null;

        void InitializeComponent() {
            this.cb_barcode_1 = new System.Windows.Forms.ComboBox();
            this.cb_barcode_2 = new System.Windows.Forms.ComboBox();
            this.cb_barcode_3 = new System.Windows.Forms.ComboBox();
            this.chb_barcode_1 = new Common.Controls.Flat_Check_Box();
            this.chb_barcode_2 = new Common.Controls.Flat_Check_Box();
            this.chb_barcode_3 = new Common.Controls.Flat_Check_Box();
            this.chb_wt_barcodes = new Common.Controls.Flat_Check_Box();
            this.label12 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chb_price_barcodes = new Common.Controls.Flat_Check_Box();
            this.but_change_wt = new System.Windows.Forms.Button();
            this.but_change_price = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.fake_Group_Box1 = new Common.Controls.Fake_Group_Box();
            this.fake_Group_Box2 = new Common.Controls.Fake_Group_Box();
            this.fake_Group_Box3 = new Common.Controls.Fake_Group_Box();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_barcode_1
            // 
            this.cb_barcode_1.Location = new System.Drawing.Point(68, 3);
            this.cb_barcode_1.Name = "cb_barcode_1";
            this.cb_barcode_1.Size = new System.Drawing.Size(163, 21);
            this.cb_barcode_1.TabIndex = 20;
            // 
            // cb_barcode_2
            // 
            this.cb_barcode_2.Location = new System.Drawing.Point(68, 29);
            this.cb_barcode_2.Name = "cb_barcode_2";
            this.cb_barcode_2.Size = new System.Drawing.Size(163, 21);
            this.cb_barcode_2.TabIndex = 40;
            // 
            // cb_barcode_3
            // 
            this.cb_barcode_3.Location = new System.Drawing.Point(68, 55);
            this.cb_barcode_3.Name = "cb_barcode_3";
            this.cb_barcode_3.Size = new System.Drawing.Size(163, 21);
            this.cb_barcode_3.TabIndex = 60;
            // 
            // chb_barcode_1
            // 
            this.chb_barcode_1.AutoSize = true;
            this.chb_barcode_1.BackColor = System.Drawing.Color.White;
            this.chb_barcode_1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_barcode_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_barcode_1.Location = new System.Drawing.Point(50, 9);
            this.chb_barcode_1.Name = "chb_barcode_1";
            this.chb_barcode_1.Size = new System.Drawing.Size(12, 11);
            this.chb_barcode_1.TabIndex = 10;
            this.chb_barcode_1.TabStop = false;
            this.chb_barcode_1.Tag = "";
            this.chb_barcode_1.UseVisualStyleBackColor = false;
            // 
            // chb_barcode_2
            // 
            this.chb_barcode_2.AutoSize = true;
            this.chb_barcode_2.BackColor = System.Drawing.Color.White;
            this.chb_barcode_2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_barcode_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_barcode_2.Location = new System.Drawing.Point(50, 35);
            this.chb_barcode_2.Name = "chb_barcode_2";
            this.chb_barcode_2.Size = new System.Drawing.Size(12, 11);
            this.chb_barcode_2.TabIndex = 30;
            this.chb_barcode_2.TabStop = false;
            this.chb_barcode_2.Tag = "";
            this.chb_barcode_2.UseVisualStyleBackColor = false;
            // 
            // chb_barcode_3
            // 
            this.chb_barcode_3.AutoSize = true;
            this.chb_barcode_3.BackColor = System.Drawing.Color.White;
            this.chb_barcode_3.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_barcode_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_barcode_3.Location = new System.Drawing.Point(50, 60);
            this.chb_barcode_3.Name = "chb_barcode_3";
            this.chb_barcode_3.Size = new System.Drawing.Size(12, 11);
            this.chb_barcode_3.TabIndex = 50;
            this.chb_barcode_3.TabStop = false;
            this.chb_barcode_3.Tag = "";
            this.chb_barcode_3.UseVisualStyleBackColor = false;
            // 
            // chb_wt_barcodes
            // 
            this.chb_wt_barcodes.AutoSize = true;
            this.chb_wt_barcodes.BackColor = System.Drawing.Color.White;
            this.chb_wt_barcodes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_wt_barcodes.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chb_wt_barcodes.FlatAppearance.BorderSize = 2;
            this.chb_wt_barcodes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_wt_barcodes.Location = new System.Drawing.Point(176, 150);
            this.chb_wt_barcodes.Name = "chb_wt_barcodes";
            this.chb_wt_barcodes.Size = new System.Drawing.Size(12, 11);
            this.chb_wt_barcodes.TabIndex = 70;
            this.chb_wt_barcodes.TabStop = false;
            this.chb_wt_barcodes.Tag = "";
            this.chb_wt_barcodes.UseVisualStyleBackColor = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(26, 42);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 13);
            this.label12.TabIndex = 39;
            this.label12.Text = "Field 1:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(26, 68);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(41, 13);
            this.label24.TabIndex = 40;
            this.label24.Text = "Field 2:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(26, 94);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(41, 13);
            this.label25.TabIndex = 36;
            this.label25.Text = "Field 3:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "Use Weight Barcodes:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 61;
            this.label1.Text = "Weight Barcode structure:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 286);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 65;
            this.label2.Text = "Price Barcode structure:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 260);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 62;
            this.label5.Text = "Use Price Barcodes:";
            // 
            // chb_price_barcodes
            // 
            this.chb_price_barcodes.AutoSize = true;
            this.chb_price_barcodes.BackColor = System.Drawing.Color.White;
            this.chb_price_barcodes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_price_barcodes.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chb_price_barcodes.FlatAppearance.BorderSize = 2;
            this.chb_price_barcodes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_price_barcodes.Location = new System.Drawing.Point(176, 261);
            this.chb_price_barcodes.Name = "chb_price_barcodes";
            this.chb_price_barcodes.Size = new System.Drawing.Size(12, 11);
            this.chb_price_barcodes.TabIndex = 100;
            this.chb_price_barcodes.TabStop = false;
            this.chb_price_barcodes.Tag = "";
            this.chb_price_barcodes.UseVisualStyleBackColor = false;
            // 
            // but_change_wt
            // 
            this.but_change_wt.Location = new System.Drawing.Point(176, 170);
            this.but_change_wt.Name = "but_change_wt";
            this.but_change_wt.Size = new System.Drawing.Size(75, 23);
            this.but_change_wt.TabIndex = 90;
            this.but_change_wt.Text = "Edit";
            this.but_change_wt.UseVisualStyleBackColor = true;
            // 
            // but_change_price
            // 
            this.but_change_price.Location = new System.Drawing.Point(176, 281);
            this.but_change_price.Name = "but_change_price";
            this.but_change_price.Size = new System.Drawing.Size(75, 23);
            this.but_change_price.TabIndex = 110;
            this.but_change_price.Text = "Edit";
            this.but_change_price.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(1, 115);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(397, 108);
            this.label8.TabIndex = 0;
            this.label8.Text = "\r\n";
            // 
            // fake_Group_Box1
            // 
            this.fake_Group_Box1.Location = new System.Drawing.Point(5, 124);
            this.fake_Group_Box1.Name = "fake_Group_Box1";
            this.fake_Group_Box1.Size = new System.Drawing.Size(400, 110);
            this.fake_Group_Box1.TabIndex = 71;
            this.fake_Group_Box1.Text = "Weight Barcodes";
            // 
            // fake_Group_Box2
            // 
            this.fake_Group_Box2.Location = new System.Drawing.Point(5, 235);
            this.fake_Group_Box2.Name = "fake_Group_Box2";
            this.fake_Group_Box2.Size = new System.Drawing.Size(400, 110);
            this.fake_Group_Box2.TabIndex = 72;
            this.fake_Group_Box2.Text = "Price Barcodes";
            // 
            // fake_Group_Box3
            // 
            this.fake_Group_Box3.Location = new System.Drawing.Point(5, 19);
            this.fake_Group_Box3.Name = "fake_Group_Box3";
            this.fake_Group_Box3.Size = new System.Drawing.Size(400, 104);
            this.fake_Group_Box3.TabIndex = 73;
            this.fake_Group_Box3.Text = "Barcode Fields";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cb_barcode_1);
            this.panel1.Controls.Add(this.cb_barcode_2);
            this.panel1.Controls.Add(this.cb_barcode_3);
            this.panel1.Controls.Add(this.chb_barcode_1);
            this.panel1.Controls.Add(this.chb_barcode_2);
            this.panel1.Controls.Add(this.chb_barcode_3);
            this.panel1.Location = new System.Drawing.Point(126, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(257, 78);
            this.panel1.TabIndex = 0;
            // 
            // Tab_Barcodes
            // 
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.but_change_wt);
            this.Controls.Add(this.but_change_price);
            this.Controls.Add(this.chb_price_barcodes);
            this.Controls.Add(this.chb_wt_barcodes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.fake_Group_Box3);
            this.Controls.Add(this.fake_Group_Box1);
            this.Controls.Add(this.fake_Group_Box2);
            this.Name = "Tab_Barcodes";
            this.Size = new System.Drawing.Size(424, 370);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        Button but_change_wt;
        Button but_change_price;


        Flat_Check_Box chb_price_barcodes;
        Fake_Group_Box fake_Group_Box1;
        Fake_Group_Box fake_Group_Box2;
        Fake_Group_Box fake_Group_Box3;
        Label label1;
        Label label2;
        Label label5;
        Label label8;


        public Button But_change_wt {
            get { return but_change_wt; }
        }
        public Button But_change_price {
            get { return but_change_price; }
        }



    }
}

