using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Common.Controls;
using DTA;
using Fairweather.Service;
using Common;

namespace Config_Gui
{
    public partial class Tab_3 : UserControl
    {
        public Tab_3() {
            InitializeComponent();
            //var lab = this.tb_serial.Toggle_Readonly_Label(false);
            //lab.TextAlign = ContentAlignment.TopLeft;
            //lab.Padding = new Padding(0, 2, 0, 0);
            lab_title.Text = Data.App_Name;

            
            //but_files_version.Visible = false;
        }

        System.ComponentModel.IContainer components = null;

        public List<Pair<Ini_Field, Label>> Tech_Support() {

            var ret = new List<Pair<Ini_Field, Label>>(8);

            var fields = DTA_Fields.Tech_Support
                                   .OrderBy(fld => fld.Field).arr();

            foreach (Control ctrl in gb_tech_support.Controls) {

                if (!(ctrl is Label))
                    continue;

                var lab = ctrl as Label;
                var ch = lab.Name.Last();
                var line = int.Parse(ch.ToString()) - 1;

                if (fields.Length > line) {
                    var field = fields[line];
                    ret.Add(Pair.Make(field, lab));

                }
            }

            return ret;

        }
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        void InitializeComponent() {
            this.but_change_license = new System.Windows.Forms.Button();
            this.but_files_version = new System.Windows.Forms.Button();
            this.gb_license_info = new System.Windows.Forms.GroupBox();
            this.lab_module_3_enabled = new System.Windows.Forms.Label();
            this.lab_module_2_enabled = new System.Windows.Forms.Label();
            this.lab_module_1_enabled = new System.Windows.Forms.Label();
            this.lab_serial = new Common.Controls.Text_Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lab_num_companies = new System.Windows.Forms.Label();
            this.lab_module_3_name = new System.Windows.Forms.Label();
            this.lab_module_2_name = new System.Windows.Forms.Label();
            this.lab_module_1_name = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gb_tech_support = new System.Windows.Forms.GroupBox();
            this.lab_tech8 = new System.Windows.Forms.Label();
            this.lab_tech7 = new System.Windows.Forms.Label();
            this.lab_tech6 = new System.Windows.Forms.Label();
            this.lab_tech5 = new System.Windows.Forms.Label();
            this.lab_tech4 = new System.Windows.Forms.Label();
            this.lab_tech3 = new System.Windows.Forms.Label();
            this.lab_tech2 = new System.Windows.Forms.Label();
            this.lab_tech1 = new System.Windows.Forms.Label();
            this.lab_title = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lab_version = new System.Windows.Forms.Label();
            this.gb_license_info.SuspendLayout();
            this.gb_tech_support.SuspendLayout();
            this.SuspendLayout();
            // 
            // but_change_license
            // 
            this.but_change_license.Location = new System.Drawing.Point(9, 127);
            this.but_change_license.Name = "but_change_license";
            this.but_change_license.Size = new System.Drawing.Size(111, 23);
            this.but_change_license.TabIndex = 1;
            this.but_change_license.Text = "Change License...";
            this.but_change_license.UseVisualStyleBackColor = true;
            // 
            // but_files_version
            // 
            this.but_files_version.Location = new System.Drawing.Point(7, 46);
            this.but_files_version.Name = "but_files_version";
            this.but_files_version.Size = new System.Drawing.Size(138, 22);
            this.but_files_version.TabIndex = 50;
            this.but_files_version.Text = "Component Files Version";
            this.but_files_version.UseVisualStyleBackColor = true;
            // 
            // gb_license_info
            // 
            this.gb_license_info.Controls.Add(this.lab_module_3_enabled);
            this.gb_license_info.Controls.Add(this.lab_module_2_enabled);
            this.gb_license_info.Controls.Add(this.lab_module_1_enabled);
            this.gb_license_info.Controls.Add(this.lab_serial);
            this.gb_license_info.Controls.Add(this.label1);
            this.gb_license_info.Controls.Add(this.but_change_license);
            this.gb_license_info.Controls.Add(this.lab_num_companies);
            this.gb_license_info.Controls.Add(this.lab_module_3_name);
            this.gb_license_info.Controls.Add(this.lab_module_2_name);
            this.gb_license_info.Controls.Add(this.lab_module_1_name);
            this.gb_license_info.Controls.Add(this.label2);
            this.gb_license_info.Location = new System.Drawing.Point(6, 120);
            this.gb_license_info.Name = "gb_license_info";
            this.gb_license_info.Size = new System.Drawing.Size(384, 158);
            this.gb_license_info.TabIndex = 44;
            this.gb_license_info.TabStop = false;
            this.gb_license_info.Text = "License Information";
            // 
            // lab_module_3_enabled
            // 
            this.lab_module_3_enabled.AutoSize = true;
            this.lab_module_3_enabled.Location = new System.Drawing.Point(193, 103);
            this.lab_module_3_enabled.Name = "lab_module_3_enabled";
            this.lab_module_3_enabled.Size = new System.Drawing.Size(16, 13);
            this.lab_module_3_enabled.TabIndex = 37;
            this.lab_module_3_enabled.Text = "...";
            // 
            // lab_module_2_enabled
            // 
            this.lab_module_2_enabled.AutoSize = true;
            this.lab_module_2_enabled.Location = new System.Drawing.Point(201, 87);
            this.lab_module_2_enabled.Name = "lab_module_2_enabled";
            this.lab_module_2_enabled.Size = new System.Drawing.Size(16, 13);
            this.lab_module_2_enabled.TabIndex = 36;
            this.lab_module_2_enabled.Text = "...";
            // 
            // lab_module_1_enabled
            // 
            this.lab_module_1_enabled.AutoSize = true;
            this.lab_module_1_enabled.Location = new System.Drawing.Point(180, 71);
            this.lab_module_1_enabled.Name = "lab_module_1_enabled";
            this.lab_module_1_enabled.Size = new System.Drawing.Size(16, 13);
            this.lab_module_1_enabled.TabIndex = 35;
            this.lab_module_1_enabled.Text = "...";
            // 
            // lab_serial
            // 
            this.lab_serial.AutoEllipsis = true;
            this.lab_serial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.lab_serial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lab_serial.Location = new System.Drawing.Point(86, 24);
            this.lab_serial.Name = "lab_serial";
            this.lab_serial.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.lab_serial.Size = new System.Drawing.Size(100, 20);
            this.lab_serial.TabIndex = 0;
            this.lab_serial.UseMnemonic = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Serial Number:";
            // 
            // lab_num_companies
            // 
            this.lab_num_companies.AutoSize = true;
            this.lab_num_companies.Location = new System.Drawing.Point(121, 55);
            this.lab_num_companies.Name = "lab_num_companies";
            this.lab_num_companies.Size = new System.Drawing.Size(16, 13);
            this.lab_num_companies.TabIndex = 9;
            this.lab_num_companies.Text = "...";
            // 
            // lab_module_3_name
            // 
            this.lab_module_3_name.AutoSize = true;
            this.lab_module_3_name.Location = new System.Drawing.Point(6, 103);
            this.lab_module_3_name.Name = "lab_module_3_name";
            this.lab_module_3_name.Size = new System.Drawing.Size(189, 13);
            this.lab_module_3_name.TabIndex = 5;
            this.lab_module_3_name.Text = "Documents Interface Module Enabled:";
            // 
            // lab_module_2_name
            // 
            this.lab_module_2_name.AutoSize = true;
            this.lab_module_2_name.Location = new System.Drawing.Point(6, 87);
            this.lab_module_2_name.Name = "lab_module_2_name";
            this.lab_module_2_name.Size = new System.Drawing.Size(196, 13);
            this.lab_module_2_name.TabIndex = 4;
            this.lab_module_2_name.Text = "Transactions Interface Module Enabled:";
            // 
            // lab_module_1_name
            // 
            this.lab_module_1_name.AutoSize = true;
            this.lab_module_1_name.Location = new System.Drawing.Point(6, 71);
            this.lab_module_1_name.Name = "lab_module_1_name";
            this.lab_module_1_name.Size = new System.Drawing.Size(175, 13);
            this.lab_module_1_name.TabIndex = 3;
            this.lab_module_1_name.Text = "Records Interface Module Enabled:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Number of companies:";
            // 
            // gb_tech_support
            // 
            this.gb_tech_support.Controls.Add(this.lab_tech8);
            this.gb_tech_support.Controls.Add(this.lab_tech7);
            this.gb_tech_support.Controls.Add(this.lab_tech6);
            this.gb_tech_support.Controls.Add(this.lab_tech5);
            this.gb_tech_support.Controls.Add(this.lab_tech4);
            this.gb_tech_support.Controls.Add(this.lab_tech3);
            this.gb_tech_support.Controls.Add(this.lab_tech2);
            this.gb_tech_support.Controls.Add(this.lab_tech1);
            this.gb_tech_support.Location = new System.Drawing.Point(6, 283);
            this.gb_tech_support.Name = "gb_tech_support";
            this.gb_tech_support.Size = new System.Drawing.Size(384, 158);
            this.gb_tech_support.TabIndex = 49;
            this.gb_tech_support.TabStop = false;
            this.gb_tech_support.Text = "Technical Support Information";
            // 
            // lab_tech8
            // 
            this.lab_tech8.AutoSize = true;
            this.lab_tech8.Location = new System.Drawing.Point(15, 122);
            this.lab_tech8.Name = "lab_tech8";
            this.lab_tech8.Size = new System.Drawing.Size(166, 13);
            this.lab_tech8.TabIndex = 41;
            this.lab_tech8.Text = "Technical Support Line Number 8";
            // 
            // lab_tech7
            // 
            this.lab_tech7.AutoSize = true;
            this.lab_tech7.Location = new System.Drawing.Point(15, 108);
            this.lab_tech7.Name = "lab_tech7";
            this.lab_tech7.Size = new System.Drawing.Size(166, 13);
            this.lab_tech7.TabIndex = 40;
            this.lab_tech7.Text = "Technical Support Line Number 7";
            // 
            // lab_tech6
            // 
            this.lab_tech6.AutoSize = true;
            this.lab_tech6.Location = new System.Drawing.Point(15, 94);
            this.lab_tech6.Name = "lab_tech6";
            this.lab_tech6.Size = new System.Drawing.Size(166, 13);
            this.lab_tech6.TabIndex = 39;
            this.lab_tech6.Text = "Technical Support Line Number 6";
            // 
            // lab_tech5
            // 
            this.lab_tech5.AutoSize = true;
            this.lab_tech5.Location = new System.Drawing.Point(15, 80);
            this.lab_tech5.Name = "lab_tech5";
            this.lab_tech5.Size = new System.Drawing.Size(166, 13);
            this.lab_tech5.TabIndex = 38;
            this.lab_tech5.Text = "Technical Support Line Number 5";
            // 
            // lab_tech4
            // 
            this.lab_tech4.AutoSize = true;
            this.lab_tech4.Location = new System.Drawing.Point(15, 66);
            this.lab_tech4.Name = "lab_tech4";
            this.lab_tech4.Size = new System.Drawing.Size(166, 13);
            this.lab_tech4.TabIndex = 37;
            this.lab_tech4.Text = "Technical Support Line Number 4";
            // 
            // lab_tech3
            // 
            this.lab_tech3.AutoSize = true;
            this.lab_tech3.Location = new System.Drawing.Point(15, 52);
            this.lab_tech3.Name = "lab_tech3";
            this.lab_tech3.Size = new System.Drawing.Size(166, 13);
            this.lab_tech3.TabIndex = 36;
            this.lab_tech3.Text = "Technical Support Line Number 3";
            // 
            // lab_tech2
            // 
            this.lab_tech2.AutoSize = true;
            this.lab_tech2.Location = new System.Drawing.Point(15, 38);
            this.lab_tech2.Name = "lab_tech2";
            this.lab_tech2.Size = new System.Drawing.Size(166, 13);
            this.lab_tech2.TabIndex = 35;
            this.lab_tech2.Text = "Technical Support Line Number 2";
            // 
            // lab_tech1
            // 
            this.lab_tech1.AutoSize = true;
            this.lab_tech1.Location = new System.Drawing.Point(15, 24);
            this.lab_tech1.Name = "lab_tech1";
            this.lab_tech1.Size = new System.Drawing.Size(166, 13);
            this.lab_tech1.TabIndex = 34;
            this.lab_tech1.Text = "Technical Support Line Number 1";
            // 
            // lab_title
            // 
            this.lab_title.AutoSize = true;
            this.lab_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.lab_title.Location = new System.Drawing.Point(5, 8);
            this.lab_title.Name = "lab_title";
            this.lab_title.Size = new System.Drawing.Size(119, 13);
            this.lab_title.TabIndex = 45;
            this.lab_title.Text = "Sage Entry Screens";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(5, 77);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(165, 13);
            this.label19.TabIndex = 46;
            this.label19.Text = "Copyright (c) 2011 InfoTrends Ltd";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(5, 94);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(93, 13);
            this.label20.TabIndex = 47;
            this.label20.Text = "All rights reserved.";
            // 
            // lab_version
            // 
            this.lab_version.AutoSize = true;
            this.lab_version.Location = new System.Drawing.Point(5, 24);
            this.lab_version.Name = "lab_version";
            this.lab_version.Size = new System.Drawing.Size(126, 13);
            this.lab_version.TabIndex = 48;
            this.lab_version.Text = "Version 1.0.0.101 (Alpha)";
            // 
            // Tab_3
            // 
            this.Controls.Add(this.but_files_version);
            this.Controls.Add(this.gb_tech_support);
            this.Controls.Add(this.lab_version);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.lab_title);
            this.Controls.Add(this.gb_license_info);
            this.Name = "Tab_3";
            this.Size = new System.Drawing.Size(600, 800);
            this.gb_license_info.ResumeLayout(false);
            this.gb_license_info.PerformLayout();
            this.gb_tech_support.ResumeLayout(false);
            this.gb_tech_support.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        public Triple<Label> Labels_module_name {
            get {
                return Triple.Make1(lab_module_1_name,
                                    lab_module_2_name,
                                    lab_module_3_name);
            }
        }
        public Triple<Label> Labels_module_enabled {
            get {
                return Triple.Make1(lab_module_1_enabled,
                                    lab_module_2_enabled,
                                    lab_module_3_enabled);
            }
        }

        #region controls


        internal Button but_change_license;
        internal Button but_files_version;
        GroupBox gb_license_info;
        GroupBox gb_tech_support;
        internal Label lab_num_companies;
        internal Label lab_module_1_enabled;
        internal Label lab_module_2_enabled;
        internal Label lab_module_3_enabled;
        internal Label label1;
        internal Label lab_title;
        internal Label label19;
        internal Label label2;
        internal Label label20;
        internal Label lab_version;
        internal Label lab_module_1_name;
        internal Label lab_module_2_name;
        internal Label lab_module_3_name;
        internal Label lab_tech1;
        internal Label lab_tech2;
        internal Label lab_tech3;
        internal Label lab_tech4;
        internal Label lab_tech5;
        internal Label lab_tech6;
        internal Label lab_tech7;
        internal Label lab_tech8;
        #endregion
        internal Text_Label lab_serial;

    }
}
