namespace Common.Dialogs
{
    using Common.Controls;

    using Fairweather.Service;

    partial class Product_Tab1
    {

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        internal void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Product_Tab1));
            this.acb_product = new Common.Controls.Advanced_Combo_Box();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.numericBox8 = new Common.Controls.Amount_Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.comboBox2 = new Common.Controls.Text_Label();
            this.comboBox1 = new Common.Controls.Text_Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.comboBox6 = new Common.Controls.Text_Label();
            this.comboBox8 = new Common.Controls.Text_Label();
            this.comboBox5 = new Common.Controls.Text_Label();
            this.comboBox4 = new Common.Controls.Text_Label();
            this.comboBox3 = new Common.Controls.Text_Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.numericBox9 = new Common.Controls.Amount_Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.but_update_avg_cost = new Common.Controls.Update_Button();
            this.but_update_sb = new Common.Controls.Update_Button();
            this.label23 = new System.Windows.Forms.Label();
            this.amount_Label2 = new Common.Controls.Amount_Label();
            this.amount_Label1 = new Common.Controls.Amount_Label();
            this.label29 = new System.Windows.Forms.Label();
            this.textBox11 = new Common.Controls.Amount_Label();
            this.textBox10 = new Common.Controls.Amount_Label();
            this.numericBox4 = new Common.Controls.Amount_Label();
            this.numericBox3 = new Common.Controls.Amount_Label();
            this.numericBox1 = new Common.Controls.Amount_Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.dateTime1 = new Common.Controls.Our_Date_Time();
            this.numericBox5 = new Common.Controls.Amount_Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.amount_Label3 = new Common.Controls.Amount_Label();
            this.label30 = new System.Windows.Forms.Label();
            this.dateTime2 = new Common.Controls.Our_Date_Time();
            this.textBox6 = new Common.Controls.Amount_Label();
            this.numericBox7 = new Common.Controls.Amount_Label();
            this.numericBox6 = new Common.Controls.Amount_Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // acb_product
            // 
            this.acb_product.Allow_Blank = false;
            this.acb_product.Allow_New_Account = false;
            this.acb_product.Auto_Show_QSF = false;
            this.acb_product.Auto_Tab = false;
            this.acb_product.BackColor = System.Drawing.Color.Silver;
            this.acb_product.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.acb_product.Control_Host = null;
            this.acb_product.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.acb_product.Location = new System.Drawing.Point(145, 26);
            this.acb_product.Max_Shortlist_Size = new System.Drawing.Size(0, 0);
            this.acb_product.MaxLength = 30;
            this.acb_product.MinimumSize = new System.Drawing.Size(80, 20);
            this.acb_product.Name = "acb_product";
            this.acb_product.QSF_Visible = false;
            this.acb_product.SelectedIndex = -1;
            this.acb_product.SelectedText = "";
            this.acb_product.SelectionLength = 0;
            this.acb_product.SelectionStart = 0;
            this.acb_product.Size = new System.Drawing.Size(100, 21);
            this.acb_product.TabIndex = 0;
            this.acb_product.Value = null;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.numericBox8);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.comboBox2);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(11, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(492, 158);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Product Details";
            // 
            // textBox5
            // 
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox5.Location = new System.Drawing.Point(133, 117);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 29;
            // 
            // textBox4
            // 
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox4.Location = new System.Drawing.Point(133, 93);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            this.textBox4.TabIndex = 28;
            // 
            // numericBox8
            // 
            this.numericBox8.AutoEllipsis = true;
            this.numericBox8.BackColor = System.Drawing.SystemColors.Window;
            this.numericBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericBox8.Decimal_Places = 2;
            this.numericBox8.Default_Format = "F2";
            this.numericBox8.Location = new System.Drawing.Point(328, 117);
            this.numericBox8.Margin = new System.Windows.Forms.Padding(0);
            this.numericBox8.MinimumSize = new System.Drawing.Size(20, 20);
            this.numericBox8.Name = "numericBox8";
            this.numericBox8.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.numericBox8.Size = new System.Drawing.Size(121, 20);
            this.numericBox8.TabIndex = 27;
            this.numericBox8.Text = "0.00";
            this.numericBox8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.numericBox8.UseMnemonic = false;
            this.numericBox8.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox3.Location = new System.Drawing.Point(328, 93);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(121, 20);
            this.textBox3.TabIndex = 26;
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.Location = new System.Drawing.Point(133, 44);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(316, 20);
            this.textBox2.TabIndex = 25;
            // 
            // comboBox2
            // 
            this.comboBox2.AutoEllipsis = true;
            this.comboBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.comboBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.comboBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox2.Location = new System.Drawing.Point(133, 68);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.comboBox2.Size = new System.Drawing.Size(316, 21);
            this.comboBox2.TabIndex = 9;
            this.comboBox2.UseMnemonic = false;
            // 
            // comboBox1
            // 
            this.comboBox1.AutoEllipsis = true;
            this.comboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.comboBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.Location = new System.Drawing.Point(330, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.comboBox1.Size = new System.Drawing.Size(118, 21);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.UseMnemonic = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(263, 121);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Weight (kg)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(263, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Location";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(261, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Item Type";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Com. Code Description";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "IntraStat Com. Code";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Category";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Description";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Product Code";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox8);
            this.groupBox3.Controls.Add(this.comboBox6);
            this.groupBox3.Controls.Add(this.comboBox8);
            this.groupBox3.Controls.Add(this.comboBox5);
            this.groupBox3.Controls.Add(this.comboBox4);
            this.groupBox3.Controls.Add(this.comboBox3);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Location = new System.Drawing.Point(11, 165);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(492, 98);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Defaults";
            // 
            // textBox8
            // 
            this.textBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox8.Location = new System.Drawing.Point(289, 40);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(93, 20);
            this.textBox8.TabIndex = 32;
            // 
            // comboBox6
            // 
            this.comboBox6.AutoEllipsis = true;
            this.comboBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.comboBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.comboBox6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox6.Location = new System.Drawing.Point(289, 65);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.comboBox6.Size = new System.Drawing.Size(93, 21);
            this.comboBox6.TabIndex = 18;
            this.comboBox6.UseMnemonic = false;
            // 
            // comboBox8
            // 
            this.comboBox8.AutoEllipsis = true;
            this.comboBox8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.comboBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.comboBox8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox8.Location = new System.Drawing.Point(289, 15);
            this.comboBox8.Name = "comboBox8";
            this.comboBox8.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.comboBox8.Size = new System.Drawing.Size(93, 21);
            this.comboBox8.TabIndex = 16;
            this.comboBox8.UseMnemonic = false;
            // 
            // comboBox5
            // 
            this.comboBox5.AutoEllipsis = true;
            this.comboBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.comboBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.comboBox5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox5.Location = new System.Drawing.Point(95, 65);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.comboBox5.Size = new System.Drawing.Size(93, 21);
            this.comboBox5.TabIndex = 15;
            this.comboBox5.UseMnemonic = false;
            // 
            // comboBox4
            // 
            this.comboBox4.AutoEllipsis = true;
            this.comboBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.comboBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.comboBox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox4.Location = new System.Drawing.Point(95, 40);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.comboBox4.Size = new System.Drawing.Size(93, 21);
            this.comboBox4.TabIndex = 14;
            this.comboBox4.UseMnemonic = false;
            // 
            // comboBox3
            // 
            this.comboBox3.AutoEllipsis = true;
            this.comboBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.comboBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.comboBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox3.Location = new System.Drawing.Point(95, 15);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.comboBox3.Size = new System.Drawing.Size(93, 21);
            this.comboBox3.TabIndex = 10;
            this.comboBox3.UseMnemonic = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(219, 69);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(62, 13);
            this.label18.TabIndex = 13;
            this.label18.Text = "Department";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(219, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Part No.";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(12, 19);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(56, 13);
            this.label17.TabIndex = 8;
            this.label17.Text = "Sales N/C";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(219, 19);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 13);
            this.label14.TabIndex = 11;
            this.label14.Text = "Tax Code";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 44);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(75, 13);
            this.label16.TabIndex = 9;
            this.label16.Text = "Purchase N/C";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 69);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 13);
            this.label15.TabIndex = 10;
            this.label15.Text = "Supplier A/C";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox9);
            this.groupBox4.Controls.Add(this.numericBox9);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Location = new System.Drawing.Point(507, 165);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(238, 98);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Sales Price";
            // 
            // textBox9
            // 
            this.textBox9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox9.Location = new System.Drawing.Point(133, 40);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(88, 20);
            this.textBox9.TabIndex = 33;
            // 
            // numericBox9
            // 
            this.numericBox9.AutoEllipsis = true;
            this.numericBox9.BackColor = System.Drawing.SystemColors.Window;
            this.numericBox9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericBox9.Decimal_Places = 2;
            this.numericBox9.Default_Format = "F2";
            this.numericBox9.Location = new System.Drawing.Point(133, 15);
            this.numericBox9.Margin = new System.Windows.Forms.Padding(0);
            this.numericBox9.MinimumSize = new System.Drawing.Size(20, 20);
            this.numericBox9.Name = "numericBox9";
            this.numericBox9.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.numericBox9.Size = new System.Drawing.Size(88, 20);
            this.numericBox9.TabIndex = 32;
            this.numericBox9.Text = "0.00";
            this.numericBox9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.numericBox9.UseMnemonic = false;
            this.numericBox9.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(18, 44);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(62, 13);
            this.label19.TabIndex = 15;
            this.label19.Text = "Unit of Sale";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(18, 19);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(31, 13);
            this.label20.TabIndex = 14;
            this.label20.Text = "Price";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label31);
            this.groupBox5.Controls.Add(this.but_update_avg_cost);
            this.groupBox5.Controls.Add(this.but_update_sb);
            this.groupBox5.Controls.Add(this.label23);
            this.groupBox5.Controls.Add(this.amount_Label2);
            this.groupBox5.Controls.Add(this.amount_Label1);
            this.groupBox5.Controls.Add(this.label29);
            this.groupBox5.Controls.Add(this.textBox11);
            this.groupBox5.Controls.Add(this.textBox10);
            this.groupBox5.Controls.Add(this.numericBox4);
            this.groupBox5.Controls.Add(this.numericBox3);
            this.groupBox5.Controls.Add(this.numericBox1);
            this.groupBox5.Controls.Add(this.label26);
            this.groupBox5.Controls.Add(this.label25);
            this.groupBox5.Controls.Add(this.label21);
            this.groupBox5.Controls.Add(this.label22);
            this.groupBox5.Controls.Add(this.label24);
            this.groupBox5.Location = new System.Drawing.Point(11, 261);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(492, 105);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Status";
            // 
            // but_update_avg_cost
            // 
            this.but_update_avg_cost.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.but_update_avg_cost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.but_update_avg_cost.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_update_avg_cost.ForeColor = System.Drawing.Color.Black;
            this.but_update_avg_cost.Fresh = false;
            this.but_update_avg_cost.Location = new System.Drawing.Point(188, 58);
            this.but_update_avg_cost.Name = "but_update_avg_cost";
            this.but_update_avg_cost.Normal_Color = System.Drawing.Color.Empty;
            this.but_update_avg_cost.Size = new System.Drawing.Size(20, 20);
            this.but_update_avg_cost.TabIndex = 37;
            this.but_update_avg_cost.Text = "!";
            this.but_update_avg_cost.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.but_update_avg_cost.UseVisualStyleBackColor = true;
            this.but_update_avg_cost.Visible = false;
            // 
            // but_update_sb
            // 
            this.but_update_sb.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.but_update_sb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.but_update_sb.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_update_sb.ForeColor = System.Drawing.Color.Black;
            this.but_update_sb.Fresh = false;
            this.but_update_sb.Location = new System.Drawing.Point(161, 59);
            this.but_update_sb.Name = "but_update_sb";
            this.but_update_sb.Normal_Color = System.Drawing.Color.Empty;
            this.but_update_sb.Size = new System.Drawing.Size(20, 20);
            this.but_update_sb.TabIndex = 36;
            this.but_update_sb.Text = "!";
            this.but_update_sb.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.but_update_sb.UseVisualStyleBackColor = true;
            this.but_update_sb.Visible = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(158, 17);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(59, 13);
            this.label23.TabIndex = 35;
            this.label23.Text = "Free Stock";
            // 
            // amount_Label2
            // 
            this.amount_Label2.AutoEllipsis = true;
            this.amount_Label2.BackColor = System.Drawing.SystemColors.Window;
            this.amount_Label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.amount_Label2.Decimal_Places = 2;
            this.amount_Label2.Default_Format = "F2";
            this.amount_Label2.Location = new System.Drawing.Point(222, 12);
            this.amount_Label2.Margin = new System.Windows.Forms.Padding(0);
            this.amount_Label2.MinimumSize = new System.Drawing.Size(20, 20);
            this.amount_Label2.Name = "amount_Label2";
            this.amount_Label2.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.amount_Label2.Size = new System.Drawing.Size(85, 20);
            this.amount_Label2.TabIndex = 34;
            this.amount_Label2.Text = "0.00";
            this.amount_Label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.amount_Label2.UseMnemonic = false;
            this.amount_Label2.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // amount_Label1
            // 
            this.amount_Label1.AutoEllipsis = true;
            this.amount_Label1.BackColor = System.Drawing.SystemColors.Window;
            this.amount_Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.amount_Label1.Decimal_Places = 2;
            this.amount_Label1.Default_Format = "F2";
            this.amount_Label1.Location = new System.Drawing.Point(69, 58);
            this.amount_Label1.Margin = new System.Windows.Forms.Padding(0);
            this.amount_Label1.MinimumSize = new System.Drawing.Size(20, 20);
            this.amount_Label1.Name = "amount_Label1";
            this.amount_Label1.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.amount_Label1.Size = new System.Drawing.Size(85, 20);
            this.amount_Label1.TabIndex = 33;
            this.amount_Label1.Text = "0.00";
            this.amount_Label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.amount_Label1.UseMnemonic = false;
            this.amount_Label1.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(12, 62);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(53, 13);
            this.label29.TabIndex = 32;
            this.label29.Text = "Stock Bal\r\n";
            // 
            // textBox11
            // 
            this.textBox11.AutoEllipsis = true;
            this.textBox11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.textBox11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox11.Decimal_Places = 2;
            this.textBox11.Default_Format = "F2";
            this.textBox11.Location = new System.Drawing.Point(222, 35);
            this.textBox11.Name = "textBox11";
            this.textBox11.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.textBox11.Size = new System.Drawing.Size(85, 20);
            this.textBox11.TabIndex = 31;
            this.textBox11.Text = "0.00";
            this.textBox11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.textBox11.UseMnemonic = false;
            this.textBox11.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // textBox10
            // 
            this.textBox10.AutoEllipsis = true;
            this.textBox10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.textBox10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox10.Decimal_Places = 2;
            this.textBox10.Default_Format = "F2";
            this.textBox10.Location = new System.Drawing.Point(69, 35);
            this.textBox10.Name = "textBox10";
            this.textBox10.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.textBox10.Size = new System.Drawing.Size(85, 20);
            this.textBox10.TabIndex = 30;
            this.textBox10.Text = "0.00";
            this.textBox10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.textBox10.UseMnemonic = false;
            this.textBox10.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // numericBox4
            // 
            this.numericBox4.AutoEllipsis = true;
            this.numericBox4.BackColor = System.Drawing.SystemColors.Window;
            this.numericBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericBox4.Decimal_Places = 2;
            this.numericBox4.Default_Format = "F2";
            this.numericBox4.Location = new System.Drawing.Point(394, 35);
            this.numericBox4.Margin = new System.Windows.Forms.Padding(0);
            this.numericBox4.MinimumSize = new System.Drawing.Size(20, 20);
            this.numericBox4.Name = "numericBox4";
            this.numericBox4.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.numericBox4.Size = new System.Drawing.Size(85, 20);
            this.numericBox4.TabIndex = 23;
            this.numericBox4.Text = "0.00";
            this.numericBox4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.numericBox4.UseMnemonic = false;
            this.numericBox4.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // numericBox3
            // 
            this.numericBox3.AutoEllipsis = true;
            this.numericBox3.BackColor = System.Drawing.SystemColors.Window;
            this.numericBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericBox3.Decimal_Places = 2;
            this.numericBox3.Default_Format = "F2";
            this.numericBox3.Location = new System.Drawing.Point(394, 12);
            this.numericBox3.Margin = new System.Windows.Forms.Padding(0);
            this.numericBox3.MinimumSize = new System.Drawing.Size(20, 20);
            this.numericBox3.Name = "numericBox3";
            this.numericBox3.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.numericBox3.Size = new System.Drawing.Size(85, 20);
            this.numericBox3.TabIndex = 22;
            this.numericBox3.Text = "0.00";
            this.numericBox3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.numericBox3.UseMnemonic = false;
            this.numericBox3.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // numericBox1
            // 
            this.numericBox1.AutoEllipsis = true;
            this.numericBox1.BackColor = System.Drawing.SystemColors.Window;
            this.numericBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericBox1.Decimal_Places = 2;
            this.numericBox1.Default_Format = "F2";
            this.numericBox1.Location = new System.Drawing.Point(69, 12);
            this.numericBox1.Margin = new System.Windows.Forms.Padding(0);
            this.numericBox1.MinimumSize = new System.Drawing.Size(20, 20);
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.numericBox1.Size = new System.Drawing.Size(85, 20);
            this.numericBox1.TabIndex = 20;
            this.numericBox1.Text = "0.00";
            this.numericBox1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.numericBox1.UseMnemonic = false;
            this.numericBox1.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(314, 40);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(67, 13);
            this.label26.TabIndex = 19;
            this.label26.Text = "Re-order Qty";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(314, 17);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(77, 13);
            this.label25.TabIndex = 18;
            this.label25.Text = "Re-order Level";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(158, 40);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(50, 13);
            this.label21.TabIndex = 17;
            this.label21.Text = "On Order";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(12, 17);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(53, 13);
            this.label22.TabIndex = 14;
            this.label22.Text = "Stock Bal";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(12, 40);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(51, 13);
            this.label24.TabIndex = 15;
            this.label24.Text = "Allocated";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.dateTime1);
            this.groupBox6.Controls.Add(this.numericBox5);
            this.groupBox6.Controls.Add(this.label27);
            this.groupBox6.Controls.Add(this.label28);
            this.groupBox6.Location = new System.Drawing.Point(507, 261);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(238, 105);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Stock Take";
            // 
            // dateTime1
            // 
            this.dateTime1.CalendarVisible = false;
            this.dateTime1.Location = new System.Drawing.Point(133, 12);
            this.dateTime1.MaxDate = new System.DateTime(((long)(0)));
            this.dateTime1.MaxLength = 32767;
            this.dateTime1.MinDate = new System.DateTime(((long)(0)));
            this.dateTime1.Name = "dateTime1";
            this.dateTime1.SelectionLength = 0;
            this.dateTime1.SelectionStart = 10;
            this.dateTime1.Size = new System.Drawing.Size(88, 20);
            this.dateTime1.SplitYear = 0;
            this.dateTime1.TabIndex = 25;
            this.dateTime1.Value = new System.DateTime(((long)(0)));
            // 
            // numericBox5
            // 
            this.numericBox5.AutoEllipsis = true;
            this.numericBox5.BackColor = System.Drawing.SystemColors.Window;
            this.numericBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericBox5.Decimal_Places = 2;
            this.numericBox5.Default_Format = "F2";
            this.numericBox5.Location = new System.Drawing.Point(133, 34);
            this.numericBox5.Margin = new System.Windows.Forms.Padding(0);
            this.numericBox5.MinimumSize = new System.Drawing.Size(20, 20);
            this.numericBox5.Name = "numericBox5";
            this.numericBox5.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.numericBox5.Size = new System.Drawing.Size(88, 20);
            this.numericBox5.TabIndex = 24;
            this.numericBox5.Text = "0.00";
            this.numericBox5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.numericBox5.UseMnemonic = false;
            this.numericBox5.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(18, 38);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(46, 13);
            this.label27.TabIndex = 21;
            this.label27.Text = "Quantity";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(18, 16);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(30, 13);
            this.label28.TabIndex = 20;
            this.label28.Text = "Date";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.amount_Label3);
            this.groupBox2.Controls.Add(this.label30);
            this.groupBox2.Controls.Add(this.dateTime2);
            this.groupBox2.Controls.Add(this.textBox6);
            this.groupBox2.Controls.Add(this.numericBox7);
            this.groupBox2.Controls.Add(this.numericBox6);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Location = new System.Drawing.Point(507, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(238, 158);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ordering";
            // 
            // amount_Label3
            // 
            this.amount_Label3.AutoEllipsis = true;
            this.amount_Label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.amount_Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.amount_Label3.Decimal_Places = 2;
            this.amount_Label3.Default_Format = "F2";
            this.amount_Label3.Location = new System.Drawing.Point(133, 130);
            this.amount_Label3.Name = "amount_Label3";
            this.amount_Label3.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.amount_Label3.Size = new System.Drawing.Size(88, 20);
            this.amount_Label3.TabIndex = 32;
            this.amount_Label3.Text = "0.00";
            this.amount_Label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.amount_Label3.UseMnemonic = false;
            this.amount_Label3.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(18, 134);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(98, 13);
            this.label30.TabIndex = 31;
            this.label30.Text = "Average Cost Price";
            // 
            // dateTime2
            // 
            this.dateTime2.CalendarVisible = false;
            this.dateTime2.Location = new System.Drawing.Point(133, 105);
            this.dateTime2.MaxDate = new System.DateTime(((long)(0)));
            this.dateTime2.MaxLength = 32767;
            this.dateTime2.MinDate = new System.DateTime(((long)(0)));
            this.dateTime2.Name = "dateTime2";
            this.dateTime2.SelectionLength = 0;
            this.dateTime2.SelectionStart = 10;
            this.dateTime2.Size = new System.Drawing.Size(88, 20);
            this.dateTime2.SplitYear = 0;
            this.dateTime2.TabIndex = 25;
            this.dateTime2.Value = new System.DateTime(((long)(0)));
            // 
            // textBox6
            // 
            this.textBox6.AutoEllipsis = true;
            this.textBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox6.Decimal_Places = 2;
            this.textBox6.Default_Format = "F2";
            this.textBox6.Location = new System.Drawing.Point(133, 80);
            this.textBox6.Name = "textBox6";
            this.textBox6.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.textBox6.Size = new System.Drawing.Size(88, 20);
            this.textBox6.TabIndex = 30;
            this.textBox6.Text = "0.00";
            this.textBox6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.textBox6.UseMnemonic = false;
            this.textBox6.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // numericBox7
            // 
            this.numericBox7.AutoEllipsis = true;
            this.numericBox7.BackColor = System.Drawing.SystemColors.Window;
            this.numericBox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericBox7.Decimal_Places = 2;
            this.numericBox7.Default_Format = "F2";
            this.numericBox7.Location = new System.Drawing.Point(133, 55);
            this.numericBox7.Margin = new System.Windows.Forms.Padding(0);
            this.numericBox7.MinimumSize = new System.Drawing.Size(20, 20);
            this.numericBox7.Name = "numericBox7";
            this.numericBox7.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.numericBox7.Size = new System.Drawing.Size(88, 20);
            this.numericBox7.TabIndex = 26;
            this.numericBox7.Text = "0.00";
            this.numericBox7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.numericBox7.UseMnemonic = false;
            this.numericBox7.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // numericBox6
            // 
            this.numericBox6.AutoEllipsis = true;
            this.numericBox6.BackColor = System.Drawing.SystemColors.Window;
            this.numericBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericBox6.Decimal_Places = 2;
            this.numericBox6.Default_Format = "F2";
            this.numericBox6.Location = new System.Drawing.Point(133, 18);
            this.numericBox6.Margin = new System.Windows.Forms.Padding(0);
            this.numericBox6.MinimumSize = new System.Drawing.Size(20, 20);
            this.numericBox6.Name = "numericBox6";
            this.numericBox6.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.numericBox6.Size = new System.Drawing.Size(88, 20);
            this.numericBox6.TabIndex = 25;
            this.numericBox6.Text = "0.00";
            this.numericBox6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.numericBox6.UseMnemonic = false;
            this.numericBox6.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(18, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 26);
            this.label13.TabIndex = 8;
            this.label13.Text = "Last Cost Price\r\n(Standard)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 109);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Last Ord Date";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 52);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 26);
            this.label12.TabIndex = 9;
            this.label12.Text = "Last Cost Price\r\n(Discounted)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 84);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Last Ord Qty";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(12, 78);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(102, 13);
            this.label31.TabIndex = 38;
            this.label31.Text = "(including unposted)";
            // 
            // Product_Tab1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.acb_product);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox6);
            this.Name = "Product_Tab1";
            this.Size = new System.Drawing.Size(885, 569);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        internal Advanced_Combo_Box acb_product;

        internal Amount_Label textBox10;
        internal Amount_Label textBox11;
        internal Amount_Label textBox6;
        internal Common.Controls.Amount_Label numericBox1;
        internal Common.Controls.Amount_Label numericBox3;
        internal Common.Controls.Amount_Label numericBox4;
        internal Common.Controls.Amount_Label numericBox5;
        internal Common.Controls.Amount_Label numericBox6;
        internal Common.Controls.Amount_Label numericBox7;
        internal Common.Controls.Amount_Label numericBox8;
        internal Common.Controls.Amount_Label numericBox9;
        internal Common.Controls.Our_Date_Time dateTime1;
        internal Our_Date_Time dateTime2;
        internal Text_Label comboBox1;
        internal Text_Label comboBox2;
        internal Text_Label comboBox3;
        internal Text_Label comboBox4;
        internal Text_Label comboBox5;
        internal Text_Label comboBox6;
        internal Text_Label comboBox8;
        internal System.Windows.Forms.GroupBox groupBox2;
        internal System.Windows.Forms.GroupBox groupBox3;
        internal System.Windows.Forms.GroupBox groupBox4;
        internal System.Windows.Forms.GroupBox groupBox5;
        internal System.Windows.Forms.GroupBox groupBox6;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Label label11;
        internal System.Windows.Forms.Label label12;
        internal System.Windows.Forms.Label label13;
        internal System.Windows.Forms.Label label14;
        internal System.Windows.Forms.Label label15;
        internal System.Windows.Forms.Label label16;
        internal System.Windows.Forms.Label label17;
        internal System.Windows.Forms.Label label18;
        internal System.Windows.Forms.Label label19;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Label label20;
        internal System.Windows.Forms.Label label21;
        internal System.Windows.Forms.Label label22;
        internal System.Windows.Forms.Label label24;
        internal System.Windows.Forms.Label label25;
        internal System.Windows.Forms.Label label26;
        internal System.Windows.Forms.Label label27;
        internal System.Windows.Forms.Label label28;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Label label9;
        internal System.Windows.Forms.TextBox textBox2;
        internal System.Windows.Forms.TextBox textBox3;
        internal System.Windows.Forms.TextBox textBox4;
        internal System.Windows.Forms.TextBox textBox5;
        internal System.Windows.Forms.TextBox textBox8;
        internal System.Windows.Forms.TextBox textBox9;
        internal System.Windows.Forms.GroupBox groupBox1;

#pragma warning disable
        System.ComponentModel.IContainer components;
#pragma warning restore
        internal Amount_Label amount_Label1;
        internal System.Windows.Forms.Label label29;
        internal Amount_Label amount_Label2;
        internal System.Windows.Forms.Label label23;
        internal Update_Button but_update_sb;
        System.Windows.Forms.Label label30;
        internal Amount_Label amount_Label3;
        internal Update_Button but_update_avg_cost;
         System.Windows.Forms.Label label31;
    }
}
