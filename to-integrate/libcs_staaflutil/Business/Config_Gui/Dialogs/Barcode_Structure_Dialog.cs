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

    public partial class Barcode_Structure_Dialog : DTA_Dialog
    {
        const string cst_Weight = "Weight";
        const string cst_Price = "Price";
        const string cst_weight = "weight";
        const string cst_price = "price";

        readonly bool m_is_weight;
        Flat_Check_Box cb_reverse_calculate;
        Label lab_reverse_calculate;


        public Barcode_Structure_Dialog(bool is_weight,
                                        Ini_File ini,
                                        Company_Number company)
            : base(ini, company) {

            m_is_weight = is_weight;

            InitializeComponent();


            Set_Texts(is_weight);

            Set_Layout(is_weight);


            // Standard.Make_Readonly_Label(lab_visualize);

            Prepare_DTA();

            Ensure_Limits();

            Refresh_Controls();

            Set_Event_Handlers();

        }

        void Set_Event_Handlers() {

            tb_prefix.TextChanged += tb_prefix_TextChanged;
            nupd_price_weight_length.ValueChanged += nupd_ValueChanged;
            nupd_price_weight_start.ValueChanged += nupd_ValueChanged;
            nupd_product_code_length.ValueChanged += nupd_ValueChanged;
            nupd_product_code_start.ValueChanged += nupd_ValueChanged;
            nupd_prefix_start.ValueChanged += nupd_ValueChanged;
            nupd_barcode_length.ValueChanged += nupd_ValueChanged;
            but_ok.Click += but_ok_Click;
            but_cancel.Click += but_cancel_Click;

        }

        void Set_Texts(bool is_weight) {

            string proper_case = is_weight ? cst_Weight : cst_Price;

            this.Text = "Barcode Structure - {0} Barcodes".spf(proper_case);
            gb_prefix.Text = "{0} prefix".spf(proper_case);
            gb_code.Text = "{0} code".spf(proper_case);

            foreach (Control child in this.All_Children()) {

                var tag = child.Tag.StringOrDefault();

                if (tag.IsNullOrEmpty())
                    continue;

                var text = child.Text.Replace("...", proper_case);

                child.Text = text;

            }
        }

        void Set_Layout(bool is_weight) {

            if (is_weight) {

                lab_reverse_calculate.Visible = false;
                cb_reverse_calculate.Visible = false;
            }
            else {

                nupd_decimal_places.Minimum =
                nupd_decimal_places.Maximum =
                nupd_decimal_places.Value = 2;

                nupd_decimal_places.Enabled = false;
                nupd_decimal_places.Visible = false;
                lab_decimal_places.Visible = false;

                // The next time I need to lock a control to
                // the border of another control I should write 
                // a helper routine and place it in Service.Layout
                // together with the Lock_To_Parent routines

                int diff = gb_prefix.Height - gb_code.Height;

                var pt = lab_reverse_calculate.Location;
                lab_reverse_calculate.Location = pt.Translate(0, diff);

                pt = cb_reverse_calculate.Location;
                cb_reverse_calculate.Location = pt.Translate(0, diff);

                gb_code.Height = gb_prefix.Height;
            }
        }


        protected override Dictionary<Control, DTA_Custom_Behavior_Pair> Custom_Behavior_Pairs {
            get {

                Func<string, string> before_set, after_get;

                before_set = str =>
                {
                    var as_int = int.Parse(str);
                    ++as_int;
                    return as_int.ToString();
                };
                after_get = str =>
                {
                    var as_int = int.Parse(str);
                    --as_int;
                    return as_int.ToString();
                };

                var pair = new DTA_Custom_Behavior_Pair(before_set, after_get);

                var ret = Index_Upds.ToDictionary(ctrl => ctrl, _ => pair);

                return ret;
            }
        }

        public override Dictionary<Control, Ini_Field> Get_Dictionary {
            get {
                var ret = m_is_weight ? Weight_Dictionary : Price_Dictionary;
                return ret;
            }
        }

        Dictionary<Control, Ini_Field> Price_Dictionary {
            get {
                return new Dictionary<Control, Ini_Field>
            {

                {nupd_barcode_length, DTA_Fields.POS_price_barcode_length},
                {nupd_prefix_start, DTA_Fields.POS_price_prefix_start},
                {tb_prefix, DTA_Fields.POS_price_prefix},
                {nupd_price_weight_start, DTA_Fields.POS_price_code_start},
                {nupd_price_weight_length, DTA_Fields.POS_price_code_length},
                {nupd_product_code_start, DTA_Fields.POS_price_product_start},
                {nupd_product_code_length, DTA_Fields.POS_price_product_length},
                {cb_reverse_calculate, DTA_Fields.POS_price_reverse_calculate},

            };
            }
        }

        Dictionary<Control, Ini_Field> Weight_Dictionary {
            get {
                return new Dictionary<Control, Ini_Field>
            {
                {nupd_barcode_length, DTA_Fields.POS_weight_barcode_length},
                {nupd_prefix_start, DTA_Fields.POS_weight_prefix_start},
                {tb_prefix, DTA_Fields.POS_weight_prefix},
                {nupd_price_weight_start, DTA_Fields.POS_weight_code_start},
                {nupd_price_weight_length, DTA_Fields.POS_weight_code_length},
                {nupd_product_code_start, DTA_Fields.POS_weight_product_start},
                {nupd_product_code_length, DTA_Fields.POS_weight_product_length},
                {nupd_decimal_places, DTA_Fields.POS_weight_decimals},
            };
            }
        }


        /*       Helpers        */

        List<Control> Index_Upds {
            get {
                return new List<Control>{                    
                              nupd_prefix_start,
                              nupd_product_code_start,
                              nupd_price_weight_start,};
            }
        }

        bool Is_Index_Nupd(NumericUpDown nupd) {

            var set = new Set<Control>(Index_Upds);

            var ret = set[nupd];

            return ret;

        }

        int Value(NumericUpDown nupd) {

            var value = (int)nupd.Value;

            if (Is_Index_Nupd(nupd))
                --value;

            return value;

        }

        void Set_Value(NumericUpDown nupd, int value) {

            if (Is_Index_Nupd(nupd))
                ++value;

            nupd.Value = value;

        }

        void Set_Max(NumericUpDown nupd, int value) {
            Set_Limit(nupd, value, false);
        }

        void Set_Min(NumericUpDown nupd, int value) {
            Set_Limit(nupd, value, true);
        }

        void Set_Limit(NumericUpDown nupd, int value, bool min) {

            if (Is_Index_Nupd(nupd))
                ++value;

            //b_nupd_ValueChanged = true;
            //try {
            if (min)
                nupd.Minimum = value;
            else
                nupd.Maximum = value;
            //                  }
            //               finally {
            //               b_nupd_ValueChanged = false;
            //            }

        }


        static string
        Visualize(int total_digits, int prefix_start, string prefix,
                       Pair<int> product_code,
                       Pair<int> price_wt_code,
                       char code_char) {

            var ret = '_'.Repeat(total_digits);

            ret = ret.Replace_Substring(prefix, prefix_start);

            var sss = 's'.Repeat(product_code.Second);
            ret = ret.Replace_Substring(sss, product_code.First);

            var www_or_ppp = code_char.Repeat(price_wt_code.Second);
            ret = ret.Replace_Substring(www_or_ppp, price_wt_code.First);

            return ret;

        }

        /*       Main Logic        */

        void Refresh_Controls() {

            this.Force_Handle();
            BeginInvoke((MethodInvoker)(() =>
            {
                Visualize();
                Ensure_Limits();
            }));

            but_ok.Enabled = !tb_prefix.Text.IsNullOrEmpty();
        }


        void Visualize() {

            int total_digits = Value(nupd_barcode_length);
            int prefix_start = Value(nupd_prefix_start);

            string prefix = tb_prefix.Text;

            var product_code = Pair.Make(Value(nupd_product_code_start),
                                                          Value(nupd_product_code_length));

            var price_wt_code = Pair.Make(Value(nupd_price_weight_start),
                                                           Value(nupd_price_weight_length));

            char code_char = m_is_weight ? 'w' : 'p';

            var code = Visualize(total_digits, prefix_start, prefix, product_code, price_wt_code, code_char);

            lab_visualize.Text = code;

        }


        void Ensure_Limits() {

            int total_digits = Value(nupd_barcode_length);

            int prefix_start = Value(nupd_prefix_start);
            int prefix_len = tb_prefix.Text.Length;
            var prefix_end = prefix_start + prefix_len;

            var until_product = Value(nupd_product_code_start);
            var product_len = Value(nupd_product_code_length);
            var product_end = until_product + product_len;

            var until_code = Value(nupd_price_weight_start);
            var code_len = Value(nupd_price_weight_length);
            var code_end = until_code + code_len;

            var max_code_len = total_digits - until_code;
            var last_code_start = total_digits - code_len;

            var max_product_len = until_code - until_product;
            var last_product_start = until_code - product_len;

            var max_prefix_len = until_product - prefix_start;
            var last_prefix_start = until_product - prefix_len;

            //{nupd_[^\.]+}\.Maximum\ +=\ +{[^;]+};
            //\1.Set_Limit(\2, false);
            //{nupd_[^\.]+}\.Minimum\ +=\ +{[^;]+};
            //\1.Set_Limit(\2, true);

            //{nupd_[^\.]+}.Set_Limit\({[^,]+}, false\)
            // Set_Max(\1, \2)
            //{nupd_[^\.]+}.Set_Limit\({[^,]+}, true\)
            // Set_Min(\1, \2)

            Set_Max(nupd_price_weight_length, max_code_len);
            Set_Min(nupd_price_weight_start, product_end);
            Set_Max(nupd_price_weight_start, last_code_start);

            Set_Max(nupd_product_code_length, max_product_len);
            Set_Min(nupd_product_code_start, prefix_end);
            Set_Max(nupd_product_code_start, last_product_start);

            Set_Min(nupd_barcode_length, code_end);

            tb_prefix.MaxLength = max_prefix_len;
            Set_Max(nupd_prefix_start, last_prefix_start);
            Set_Min(nupd_prefix_start, 0);

            if (m_is_weight)
                Set_Max(nupd_decimal_places, code_len);

        }


        /*       Misc        */

        void but_cancel_Click(object sender, EventArgs e) {

            Close();

        }

#pragma warning disable
        bool b_nupd_ValueChanged;
#pragma warning restore

        void nupd_ValueChanged(object sender, EventArgs e) {

            if (b_nupd_ValueChanged)
                return;

            Refresh_Controls();

        }

        void tb_prefix_TextChanged(object sender, EventArgs e) {

            Refresh_Controls();


        }






        #region designer


        System.ComponentModel.IContainer components = null;


        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }



        void InitializeComponent() {
            this.lab_prefix = new System.Windows.Forms.Label();
            this.tb_prefix = new Common.Controls.Our_Text_Box();
            this.la_prefix_start = new System.Windows.Forms.Label();
            this.lab_product_code = new System.Windows.Forms.Label();
            this.lab_price_weight_start = new System.Windows.Forms.Label();
            this.lab_product_code_length = new System.Windows.Forms.Label();
            this.lab_price_weight_length = new System.Windows.Forms.Label();
            this.lab_decimal_places = new System.Windows.Forms.Label();
            this.nupd_decimal_places = new System.Windows.Forms.NumericUpDown();
            this.nupd_price_weight_length = new System.Windows.Forms.NumericUpDown();
            this.nupd_price_weight_start = new System.Windows.Forms.NumericUpDown();
            this.nupd_product_code_length = new System.Windows.Forms.NumericUpDown();
            this.nupd_product_code_start = new System.Windows.Forms.NumericUpDown();
            this.nupd_prefix_start = new System.Windows.Forms.NumericUpDown();
            this.nupd_barcode_length = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.but_ok = new System.Windows.Forms.Button();
            this.but_cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lab_visualize = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gb_prefix = new System.Windows.Forms.GroupBox();
            this.gb_code = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cb_reverse_calculate = new Common.Controls.Flat_Check_Box();
            this.lab_reverse_calculate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_decimal_places)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_price_weight_length)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_price_weight_start)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_product_code_length)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_product_code_start)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_prefix_start)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_barcode_length)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.gb_prefix.SuspendLayout();
            this.gb_code.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // lab_prefix
            // 
            this.lab_prefix.AutoSize = true;
            this.lab_prefix.Location = new System.Drawing.Point(9, 26);
            this.lab_prefix.Name = "lab_prefix";
            this.lab_prefix.Size = new System.Drawing.Size(36, 13);
            this.lab_prefix.TabIndex = 1;
            this.lab_prefix.Tag = "PW";
            this.lab_prefix.Text = "Prefix:";
            // 
            // tb_prefix
            // 
            this.tb_prefix.Auto_Highlight = false;
            this.tb_prefix.BackColor = System.Drawing.Color.White;
            this.tb_prefix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_prefix.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tb_prefix.Is_Readonly = false;
            this.tb_prefix.Location = new System.Drawing.Point(122, 22);
            this.tb_prefix.Name = "tb_prefix";
            this.tb_prefix.Right_Padding = 0;
            this.tb_prefix.Size = new System.Drawing.Size(100, 20);
            this.tb_prefix.TabIndex = 20;
            this.tb_prefix.TabOnEnter = false;
            this.tb_prefix.Text = "WGT";
            // 
            // la_prefix_start
            // 
            this.la_prefix_start.AutoSize = true;
            this.la_prefix_start.Location = new System.Drawing.Point(10, 54);
            this.la_prefix_start.Name = "la_prefix_start";
            this.la_prefix_start.Size = new System.Drawing.Size(59, 13);
            this.la_prefix_start.TabIndex = 3;
            this.la_prefix_start.Tag = "PW";
            this.la_prefix_start.Text = "Prefix start:";
            // 
            // lab_product_code
            // 
            this.lab_product_code.AutoSize = true;
            this.lab_product_code.Location = new System.Drawing.Point(7, 26);
            this.lab_product_code.Name = "lab_product_code";
            this.lab_product_code.Size = new System.Drawing.Size(88, 13);
            this.lab_product_code.TabIndex = 4;
            this.lab_product_code.Text = "Stock code start:";
            // 
            // lab_price_weight_start
            // 
            this.lab_price_weight_start.AutoSize = true;
            this.lab_price_weight_start.Location = new System.Drawing.Point(4, 26);
            this.lab_price_weight_start.Name = "lab_price_weight_start";
            this.lab_price_weight_start.Size = new System.Drawing.Size(69, 13);
            this.lab_price_weight_start.TabIndex = 5;
            this.lab_price_weight_start.Tag = "PW";
            this.lab_price_weight_start.Text = "... code start:";
            // 
            // lab_product_code_length
            // 
            this.lab_product_code_length.AutoSize = true;
            this.lab_product_code_length.Location = new System.Drawing.Point(7, 54);
            this.lab_product_code_length.Name = "lab_product_code_length";
            this.lab_product_code_length.Size = new System.Drawing.Size(43, 13);
            this.lab_product_code_length.TabIndex = 6;
            this.lab_product_code_length.Text = "Length:";
            // 
            // lab_price_weight_length
            // 
            this.lab_price_weight_length.AutoSize = true;
            this.lab_price_weight_length.Location = new System.Drawing.Point(4, 54);
            this.lab_price_weight_length.Name = "lab_price_weight_length";
            this.lab_price_weight_length.Size = new System.Drawing.Size(43, 13);
            this.lab_price_weight_length.TabIndex = 7;
            this.lab_price_weight_length.Tag = "PW";
            this.lab_price_weight_length.Text = "Length:";
            // 
            // lab_decimal_places
            // 
            this.lab_decimal_places.AutoSize = true;
            this.lab_decimal_places.Location = new System.Drawing.Point(4, 82);
            this.lab_decimal_places.Name = "lab_decimal_places";
            this.lab_decimal_places.Size = new System.Drawing.Size(82, 13);
            this.lab_decimal_places.TabIndex = 8;
            this.lab_decimal_places.Tag = "PW";
            this.lab_decimal_places.Text = "Decimal places:";
            // 
            // nupd_decimal_places
            // 
            this.nupd_decimal_places.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_decimal_places.Location = new System.Drawing.Point(165, 78);
            this.nupd_decimal_places.Name = "nupd_decimal_places";
            this.nupd_decimal_places.Size = new System.Drawing.Size(48, 20);
            this.nupd_decimal_places.TabIndex = 70;
            this.nupd_decimal_places.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nupd_price_weight_length
            // 
            this.nupd_price_weight_length.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_price_weight_length.Location = new System.Drawing.Point(165, 50);
            this.nupd_price_weight_length.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nupd_price_weight_length.Name = "nupd_price_weight_length";
            this.nupd_price_weight_length.Size = new System.Drawing.Size(48, 20);
            this.nupd_price_weight_length.TabIndex = 60;
            this.nupd_price_weight_length.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // nupd_price_weight_start
            // 
            this.nupd_price_weight_start.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_price_weight_start.Location = new System.Drawing.Point(165, 22);
            this.nupd_price_weight_start.Name = "nupd_price_weight_start";
            this.nupd_price_weight_start.Size = new System.Drawing.Size(48, 20);
            this.nupd_price_weight_start.TabIndex = 50;
            this.nupd_price_weight_start.Value = new decimal(new int[] {
            11,
            0,
            0,
            0});
            // 
            // nupd_product_code_length
            // 
            this.nupd_product_code_length.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_product_code_length.Location = new System.Drawing.Point(174, 50);
            this.nupd_product_code_length.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nupd_product_code_length.Name = "nupd_product_code_length";
            this.nupd_product_code_length.Size = new System.Drawing.Size(48, 20);
            this.nupd_product_code_length.TabIndex = 50;
            this.nupd_product_code_length.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // nupd_product_code_start
            // 
            this.nupd_product_code_start.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_product_code_start.Location = new System.Drawing.Point(174, 22);
            this.nupd_product_code_start.Name = "nupd_product_code_start";
            this.nupd_product_code_start.Size = new System.Drawing.Size(48, 20);
            this.nupd_product_code_start.TabIndex = 40;
            this.nupd_product_code_start.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // nupd_prefix_start
            // 
            this.nupd_prefix_start.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_prefix_start.Location = new System.Drawing.Point(174, 50);
            this.nupd_prefix_start.Name = "nupd_prefix_start";
            this.nupd_prefix_start.Size = new System.Drawing.Size(48, 20);
            this.nupd_prefix_start.TabIndex = 30;
            this.nupd_prefix_start.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // nupd_barcode_length
            // 
            this.nupd_barcode_length.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_barcode_length.Location = new System.Drawing.Point(174, 28);
            this.nupd_barcode_length.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.nupd_barcode_length.Minimum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nupd_barcode_length.Name = "nupd_barcode_length";
            this.nupd_barcode_length.Size = new System.Drawing.Size(48, 20);
            this.nupd_barcode_length.TabIndex = 10;
            this.nupd_barcode_length.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Total barcode length:";
            // 
            // but_ok
            // 
            this.but_ok.Location = new System.Drawing.Point(308, 494);
            this.but_ok.Name = "but_ok";
            this.but_ok.Size = new System.Drawing.Size(75, 23);
            this.but_ok.TabIndex = 40;
            this.but_ok.Text = "OK";
            this.but_ok.UseVisualStyleBackColor = true;
            // 
            // but_cancel
            // 
            this.but_cancel.Location = new System.Drawing.Point(388, 494);
            this.but_cancel.Name = "but_cancel";
            this.but_cancel.Size = new System.Drawing.Size(75, 23);
            this.but_cancel.TabIndex = 50;
            this.but_cancel.Text = "Cancel";
            this.but_cancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Location = new System.Drawing.Point(4, 291);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(459, 190);
            this.groupBox1.TabIndex = 91;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Barcode preview";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lab_visualize);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Location = new System.Drawing.Point(76, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 55);
            this.panel1.TabIndex = 107;
            // 
            // lab_visualize
            // 
            this.lab_visualize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lab_visualize.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.lab_visualize.Location = new System.Drawing.Point(18, 22);
            this.lab_visualize.Name = "lab_visualize";
            this.lab_visualize.Size = new System.Drawing.Size(242, 23);
            this.lab_visualize.TabIndex = 100;
            this.lab_visualize.Text = "WGT_ssss_wwww_";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Courier New", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(23, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 12);
            this.label6.TabIndex = 96;
            this.label6.Text = "1 2 3 4 5 6 7 8 9 0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Courier New", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(224, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 12);
            this.label8.TabIndex = 98;
            this.label8.Text = "1 2 3";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Courier New", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(123, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 12);
            this.label7.TabIndex = 97;
            this.label7.Text = "1 2 3 4 5 6 7 8 9 0 ";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Location = new System.Drawing.Point(76, 80);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(284, 102);
            this.groupBox5.TabIndex = 106;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Legend";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(15, 71);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(98, 18);
            this.label15.TabIndex = 92;
            this.label15.Text = "UPPERCASE";
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label14.Location = new System.Drawing.Point(148, 45);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(15, 26);
            this.label14.TabIndex = 105;
            this.label14.Text = " ";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.label13.Location = new System.Drawing.Point(148, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(15, 26);
            this.label13.TabIndex = 104;
            this.label13.Text = "s";
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(15, 48);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(15, 26);
            this.label12.TabIndex = 103;
            this.label12.Text = "p";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(15, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(15, 26);
            this.label11.TabIndex = 102;
            this.label11.Text = "w:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(110, 73);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(152, 13);
            this.label9.TabIndex = 99;
            this.label9.Text = "characters denote themselves.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(159, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 92;
            this.label1.Text = ": digits of no interest";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(159, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 93;
            this.label2.Text = ": stock code segment";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 95;
            this.label3.Text = ": weight code segment";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 94;
            this.label4.Text = ": price code segment";
            // 
            // gb_prefix
            // 
            this.gb_prefix.Controls.Add(this.nupd_prefix_start);
            this.gb_prefix.Controls.Add(this.la_prefix_start);
            this.gb_prefix.Controls.Add(this.tb_prefix);
            this.gb_prefix.Controls.Add(this.lab_prefix);
            this.gb_prefix.Location = new System.Drawing.Point(4, 66);
            this.gb_prefix.Name = "gb_prefix";
            this.gb_prefix.Size = new System.Drawing.Size(233, 101);
            this.gb_prefix.TabIndex = 15;
            this.gb_prefix.TabStop = false;
            this.gb_prefix.Text = "Price prefix";
            // 
            // gb_code
            // 
            this.gb_code.Controls.Add(this.nupd_price_weight_start);
            this.gb_code.Controls.Add(this.nupd_price_weight_length);
            this.gb_code.Controls.Add(this.nupd_decimal_places);
            this.gb_code.Controls.Add(this.lab_decimal_places);
            this.gb_code.Controls.Add(this.lab_price_weight_length);
            this.gb_code.Controls.Add(this.lab_price_weight_start);
            this.gb_code.Location = new System.Drawing.Point(241, 66);
            this.gb_code.Name = "gb_code";
            this.gb_code.Size = new System.Drawing.Size(222, 128);
            this.gb_code.TabIndex = 30;
            this.gb_code.TabStop = false;
            this.gb_code.Text = "Price code";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.nupd_product_code_start);
            this.groupBox4.Controls.Add(this.nupd_product_code_length);
            this.groupBox4.Controls.Add(this.lab_product_code_length);
            this.groupBox4.Controls.Add(this.lab_product_code);
            this.groupBox4.Location = new System.Drawing.Point(4, 168);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(233, 101);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Stock code";
            // 
            // cb_reverse_calculate
            // 
            this.cb_reverse_calculate.AutoSize = true;
            this.cb_reverse_calculate.BackColor = System.Drawing.Color.White;
            this.cb_reverse_calculate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cb_reverse_calculate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_reverse_calculate.Location = new System.Drawing.Point(443, 201);
            this.cb_reverse_calculate.Name = "cb_reverse_calculate";
            this.cb_reverse_calculate.Size = new System.Drawing.Size(12, 11);
            this.cb_reverse_calculate.TabIndex = 35;
            this.cb_reverse_calculate.TabStop = false;
            this.cb_reverse_calculate.UseVisualStyleBackColor = true;
            // 
            // lab_reverse_calculate
            // 
            this.lab_reverse_calculate.AutoSize = true;
            this.lab_reverse_calculate.Location = new System.Drawing.Point(245, 200);
            this.lab_reverse_calculate.Name = "lab_reverse_calculate";
            this.lab_reverse_calculate.Size = new System.Drawing.Size(167, 13);
            this.lab_reverse_calculate.TabIndex = 93;
            this.lab_reverse_calculate.Text = "Calculate quantity based on price:";
            // 
            // BarcodeStructureDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 521);
            this.Controls.Add(this.lab_reverse_calculate);
            this.Controls.Add(this.cb_reverse_calculate);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.gb_code);
            this.Controls.Add(this.gb_prefix);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.but_cancel);
            this.Controls.Add(this.but_ok);
            this.Controls.Add(this.nupd_barcode_length);
            this.Controls.Add(this.label5);
            this.Name = "BarcodeStructureDialog";
            this.Text = "Barcode Structure";
            ((System.ComponentModel.ISupportInitialize)(this.nupd_decimal_places)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_price_weight_length)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_price_weight_start)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_product_code_length)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_product_code_start)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_prefix_start)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_barcode_length)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.gb_prefix.ResumeLayout(false);
            this.gb_prefix.PerformLayout();
            this.gb_code.ResumeLayout(false);
            this.gb_code.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region controls
        Common.Controls.Our_Text_Box tb_prefix;
        Button but_ok;
        Button but_cancel;

        GroupBox groupBox1;
        Label label13;
        Label label12;
        Label label11;
        Label label6;
        Label label9;
        Label label1;
        Label label8;
        Label label2;
        Label label7;
        Label label3;
        Label label4;
        Label label15;
        Label label14;
        GroupBox groupBox5;
        GroupBox gb_prefix;
        GroupBox gb_code;
        GroupBox groupBox4;
        Panel panel1;
        Label lab_visualize;
        Label la_prefix_start;
        Label lab_decimal_places;
        Label lab_prefix;
        Label lab_price_weight_length;
        Label lab_price_weight_start;
        Label lab_product_code;
        Label lab_product_code_length;
        Label label5;
        NumericUpDown nupd_barcode_length;
        NumericUpDown nupd_decimal_places;
        NumericUpDown nupd_prefix_start;
        NumericUpDown nupd_price_weight_length;
        NumericUpDown nupd_price_weight_start;
        NumericUpDown nupd_product_code_length;
        NumericUpDown nupd_product_code_start;
        #endregion




    }
}
