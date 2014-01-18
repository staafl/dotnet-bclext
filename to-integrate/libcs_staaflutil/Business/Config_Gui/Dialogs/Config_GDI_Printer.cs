using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common;
using Common.Controls;
using DTA;
using Fairweather.Service;
using Standardization;

namespace Config_Gui
{

    public class GDI_Printer_Dialog : DTA_Dialog
    {
        const int cst_min_page_width = 51;
        const int cst_min_font_size = 5;
        Label label7;
        Button but_change;
        Label lab_page_settings;
        Label label8;
        NumericUpDown nupd_scale;

        readonly bool init;

        public GDI_Printer_Dialog(Ini_File ini, Company_Number number)
            : base(ini, number) {

            InitializeComponent();
            //Standard.Flat_Style(this.cb_font);
            //Standard.Flat_Style(this.cb_printer);

            nupd_page_width.Minimum = cst_min_page_width;
            nupd_font_size.Minimum = cst_min_font_size;

            Prepare_DTA();


            but_ok.Click += (but_ok_Click);
            but_cancel.Click += (_1, _2) => Close();

            this.Text = "Printing Configuration - Generic Windows Drivers";

            init = true;

            Verify_Page_Size();

            // Standard.Make_Readonly_Label(lab_page_settings);
            lab_page_settings.TextAlign = ContentAlignment.TopCenter;
            Set_Page_Settings_Label();


        }
        void Set_Page_Settings_Label() {



            bool is_set;
            var settings = M.Get_Page_Settings(m_company_number, out is_set);

            string text = "Not Set";

            if (is_set) {
                var m = settings.Margins;
                var en1 = new[] { m.Left, m.Top, m.Right, m.Bottom }
                            .Select(fl => (C.In_to_MM(fl) / 100.0).At_Most_Precision(2));

                var en2 = new[] { "L=", " T=", " R=", " B=" };

                var en3 = en2.Zip_With(en1, (s1, s2) => s1 + s2);
                var margins = "[" + en3.Pretty_Print("{0}") + "]";

                text = "{0} {1} {2}".spf(settings.PaperSize.PaperName, settings.Landscape ? "L" : "P", margins);
            }

            lab_page_settings.Text = text;
        }


        public override Dictionary<Control, Ini_Field> Get_Dictionary {
            get {
                return new Dictionary<Control, Ini_Field>
                {
                    {cb_printer,DTA_Fields.POS_printing_gdi_printer },
                    {cb_font,DTA_Fields.POS_printing_gdi_font_face },

                    {chb_preview,DTA_Fields.POS_printing_gdi_preview },
                    {chb_page_setup,DTA_Fields.POS_printing_gdi_page_setup },
                    {chb_print_dialog,DTA_Fields.POS_printing_gdi_print_dialog },
                    {nupd_font_size,DTA_Fields.POS_printing_gdi_font_size },
                    {nupd_page_width,DTA_Fields.POS_printing_gdi_page_width },
                    {nupd_scale,DTA_Fields.POS_printing_gdi_dpi },

                };
            }
        }

        protected override Dictionary<Control, DTA_Custom_Behavior_Pair> Custom_Behavior_Pairs {
            get {
                return new Dictionary<Control, DTA_Custom_Behavior_Pair> { };
            }
        }

        void Verify_Page_Size() {

            if (!init)
                return;

            int font_size = (int)nupd_font_size.Value;
            int page_size = (int)nupd_page_width.Value;

            string font_face;
            Data.Fonts.FindByValue(cb_font.SelectedItem.ToString(), out font_face).tiff();

            var font = new Font(font_face, font_size);

            var check = 'x'.Repeat(Data.MINIMUM_CHAR_COUNT);

            using (var g = this.CreateGraphics()) {

                var needed = T.String_Width(g, check, font, GraphicsUnit.Millimeter);

                nupd_page_width.Minimum = Math.Max(cst_min_page_width, (int)(needed + 0.5f));

            }

        }

        void cb_font_SelectedIndexChanged(object sender, EventArgs e) {

            Verify_Page_Size();

        }

        void nupd_font_size_ValueChanged(object sender, EventArgs e) {
            Verify_Page_Size();

        }

        void nupd_page_width_ValueChanged(object sender, EventArgs e) {
            Verify_Page_Size();
        }

        #region designer
        Flat_Check_Box chb_page_setup;
        Flat_Check_Box chb_print_dialog;
        Label label1;
        Label label2;
        Label label3;
        Button but_ok;
        Button but_cancel;
        ComboBox cb_printer;
        Label label4;
        Label label5;
        ComboBox cb_font;
        NumericUpDown nupd_font_size;
        NumericUpDown nupd_page_width;
        Label label6;
        Flat_Check_Box chb_preview;

        void InitializeComponent() {
            this.chb_preview = new Common.Controls.Flat_Check_Box();
            this.chb_page_setup = new Common.Controls.Flat_Check_Box();
            this.chb_print_dialog = new Common.Controls.Flat_Check_Box();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.but_ok = new System.Windows.Forms.Button();
            this.but_cancel = new System.Windows.Forms.Button();
            this.cb_printer = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_font = new System.Windows.Forms.ComboBox();
            this.nupd_font_size = new System.Windows.Forms.NumericUpDown();
            this.nupd_page_width = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.but_change = new System.Windows.Forms.Button();
            this.lab_page_settings = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.nupd_scale = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_font_size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_page_width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_scale)).BeginInit();
            this.SuspendLayout();
            // 
            // chb_preview
            // 
            this.chb_preview.AutoSize = true;
            this.chb_preview.BackColor = System.Drawing.Color.White;
            this.chb_preview.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_preview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_preview.Location = new System.Drawing.Point(255, 199);
            this.chb_preview.Name = "chb_preview";
            this.chb_preview.Size = new System.Drawing.Size(12, 11);
            this.chb_preview.TabIndex = 40;
            this.chb_preview.TabStop = false;
            this.chb_preview.UseVisualStyleBackColor = true;
            // 
            // chb_page_setup
            // 
            this.chb_page_setup.AutoSize = true;
            this.chb_page_setup.BackColor = System.Drawing.Color.White;
            this.chb_page_setup.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_page_setup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_page_setup.Location = new System.Drawing.Point(255, 249);
            this.chb_page_setup.Name = "chb_page_setup";
            this.chb_page_setup.Size = new System.Drawing.Size(12, 11);
            this.chb_page_setup.TabIndex = 80;
            this.chb_page_setup.TabStop = false;
            this.chb_page_setup.UseVisualStyleBackColor = true;
            // 
            // chb_print_dialog
            // 
            this.chb_print_dialog.AutoSize = true;
            this.chb_print_dialog.BackColor = System.Drawing.Color.White;
            this.chb_print_dialog.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_print_dialog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_print_dialog.Location = new System.Drawing.Point(255, 224);
            this.chb_print_dialog.Name = "chb_print_dialog";
            this.chb_print_dialog.Size = new System.Drawing.Size(12, 11);
            this.chb_print_dialog.TabIndex = 60;
            this.chb_print_dialog.TabStop = false;
            this.chb_print_dialog.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 197);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Show Print Preview Dialog:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 222);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Show Print Dialog:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 247);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Show Page Setup Dialog:";
            // 
            // but_ok
            // 
            this.but_ok.Location = new System.Drawing.Point(114, 277);
            this.but_ok.Name = "but_ok";
            this.but_ok.Size = new System.Drawing.Size(75, 23);
            this.but_ok.TabIndex = 6;
            this.but_ok.Text = "OK";
            this.but_ok.UseVisualStyleBackColor = true;
            // 
            // but_cancel
            // 
            this.but_cancel.Location = new System.Drawing.Point(194, 277);
            this.but_cancel.Name = "but_cancel";
            this.but_cancel.Size = new System.Drawing.Size(75, 23);
            this.but_cancel.TabIndex = 7;
            this.but_cancel.Text = "Cancel";
            this.but_cancel.UseVisualStyleBackColor = true;
            // 
            // cb_printer
            // 
            this.cb_printer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_printer.FormattingEnabled = true;
            this.cb_printer.Location = new System.Drawing.Point(101, 12);
            this.cb_printer.Name = "cb_printer";
            this.cb_printer.Size = new System.Drawing.Size(168, 21);
            this.cb_printer.TabIndex = 8;
            this.cb_printer.Text = "Use System Default";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Preferred Printer:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Default Font:";
            // 
            // cb_font
            // 
            this.cb_font.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_font.FormattingEnabled = true;
            this.cb_font.Location = new System.Drawing.Point(101, 41);
            this.cb_font.Name = "cb_font";
            this.cb_font.Size = new System.Drawing.Size(116, 21);
            this.cb_font.TabIndex = 11;
            this.cb_font.Text = "Courier";
            this.cb_font.SelectedIndexChanged += new System.EventHandler(this.cb_font_SelectedIndexChanged);
            // 
            // nupd_font_size
            // 
            this.nupd_font_size.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_font_size.Location = new System.Drawing.Point(224, 41);
            this.nupd_font_size.Maximum = new decimal(new int[] {
            14,
            0,
            0,
            0});
            this.nupd_font_size.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nupd_font_size.Name = "nupd_font_size";
            this.nupd_font_size.Size = new System.Drawing.Size(45, 20);
            this.nupd_font_size.TabIndex = 12;
            this.nupd_font_size.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nupd_font_size.ValueChanged += new System.EventHandler(this.nupd_font_size_ValueChanged);
            // 
            // nupd_page_width
            // 
            this.nupd_page_width.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_page_width.Location = new System.Drawing.Point(224, 70);
            this.nupd_page_width.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nupd_page_width.Minimum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.nupd_page_width.Name = "nupd_page_width";
            this.nupd_page_width.Size = new System.Drawing.Size(45, 20);
            this.nupd_page_width.TabIndex = 13;
            this.nupd_page_width.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.nupd_page_width.ValueChanged += new System.EventHandler(this.nupd_page_width_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Receipt Width (mm):";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 136);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Default Page Settings:";
            // 
            // but_change
            // 
            this.but_change.Location = new System.Drawing.Point(193, 131);
            this.but_change.Name = "but_change";
            this.but_change.Size = new System.Drawing.Size(75, 23);
            this.but_change.TabIndex = 16;
            this.but_change.Text = "Change";
            this.but_change.UseVisualStyleBackColor = true;
            this.but_change.Click += new System.EventHandler(this.but_change_Click);
            // 
            // lab_page_settings
            // 
            this.lab_page_settings.AutoEllipsis = true;
            this.lab_page_settings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lab_page_settings.Location = new System.Drawing.Point(9, 162);
            this.lab_page_settings.Name = "lab_page_settings";
            this.lab_page_settings.Size = new System.Drawing.Size(260, 20);
            this.lab_page_settings.TabIndex = 17;
            this.lab_page_settings.Text = "Not Set";
            this.lab_page_settings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 100);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Printer Scaling (DPI):";
            // 
            // nupd_scale
            // 
            this.nupd_scale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nupd_scale.Location = new System.Drawing.Point(224, 96);
            this.nupd_scale.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nupd_scale.Minimum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.nupd_scale.Name = "nupd_scale";
            this.nupd_scale.Size = new System.Drawing.Size(45, 20);
            this.nupd_scale.TabIndex = 15;
            this.nupd_scale.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // GDI_Printer_Dialog
            // 
            this.ClientSize = new System.Drawing.Size(280, 307);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.nupd_scale);
            this.Controls.Add(this.lab_page_settings);
            this.Controls.Add(this.but_change);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nupd_page_width);
            this.Controls.Add(this.nupd_font_size);
            this.Controls.Add(this.cb_font);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cb_printer);
            this.Controls.Add(this.but_cancel);
            this.Controls.Add(this.but_ok);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chb_print_dialog);
            this.Controls.Add(this.chb_page_setup);
            this.Controls.Add(this.chb_preview);
            this.MaximumSize = new System.Drawing.Size(286, 332);
            this.MinimumSize = new System.Drawing.Size(286, 332);
            this.Name = "GDI_Printer_Dialog";
            this.Text = "Printing Configuration";
            this.Controls.SetChildIndex(this.chb_preview, 0);
            this.Controls.SetChildIndex(this.chb_page_setup, 0);
            this.Controls.SetChildIndex(this.chb_print_dialog, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.but_ok, 0);
            this.Controls.SetChildIndex(this.but_cancel, 0);
            this.Controls.SetChildIndex(this.cb_printer, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.cb_font, 0);
            this.Controls.SetChildIndex(this.nupd_font_size, 0);
            this.Controls.SetChildIndex(this.nupd_page_width, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.but_change, 0);
            this.Controls.SetChildIndex(this.lab_page_settings, 0);
            this.Controls.SetChildIndex(this.nupd_scale, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            ((System.ComponentModel.ISupportInitialize)(this.nupd_font_size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_page_width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupd_scale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        void but_change_Click(object sender, EventArgs e) {

            var engine = new Print_Engine();
            engine.Dialog_Owner = this.Parent as Form ?? this;
            var printer = engine.Printing_Provider;

            bool _;
            var settings1 = M.Get_Page_Settings(m_company_number, out _);

            printer.DefaultPageSettings = settings1;

            if (engine.Show_Page_Setup() == DialogResult.OK) {

                var settings = printer.DefaultPageSettings;
                M.Save_Page_Settings(settings, m_company_number);

            }

            this.Refresh();
            Set_Page_Settings_Label();
        }


    }

}