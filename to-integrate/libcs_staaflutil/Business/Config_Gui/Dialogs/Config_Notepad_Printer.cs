using System.Collections.Generic;
using System.Windows.Forms;
using Common;
using Common.Controls;
using DTA;
using Fairweather.Service;
using Standardization;

namespace Config_Gui
{

    public class Notepad_Printer_Dialog
        //*
        : DTA_Dialog
        /*/
        : Form_Base
        //*/
    {


        public Notepad_Printer_Dialog(Ini_File ini, Company_Number number)
            : base(ini, number) {


            InitializeComponent();

            Prepare_DTA();

            but_ok.Click += (but_ok_Click);
            but_cancel.Click += (_1, _2) => Close();

            this.Text = "Printing Configuration - Text Files";

        }

        public override Dictionary<Control, Ini_Field> Get_Dictionary {
            get {
                return new Dictionary<Control, Ini_Field>
                {
                    {chb_open_directory, DTA_Fields.POS_printing_txt_open_dir},
                    {chb_open_file, DTA_Fields.POS_printing_txt_open_file},
                    {chb_print_tags, DTA_Fields.POS_printing_txt_print_tags},

                };
            }
        }

        protected override Dictionary<Control, DTA_Custom_Behavior_Pair> Custom_Behavior_Pairs {
            get {
                return new Dictionary<Control, DTA_Custom_Behavior_Pair> { };
            }
        }

        #region designer
        Flat_Check_Box chb_open_file;
        Flat_Check_Box chb_open_directory;
        Label label1;
        Label label2;
        Label label3;
        Button but_ok;
        Button but_cancel;
        Flat_Check_Box chb_print_tags;

        void InitializeComponent() {
            this.chb_print_tags = new Common.Controls.Flat_Check_Box();
            this.chb_open_file = new Common.Controls.Flat_Check_Box();
            this.chb_open_directory = new Common.Controls.Flat_Check_Box();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.but_ok = new System.Windows.Forms.Button();
            this.but_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chb_print_tags
            // 
            this.chb_print_tags.AutoSize = true;
            this.chb_print_tags.BackColor = System.Drawing.Color.White;
            this.chb_print_tags.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_print_tags.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_print_tags.Location = new System.Drawing.Point(253, 14);
            this.chb_print_tags.Name = "chb_print_tags";
            this.chb_print_tags.Size = new System.Drawing.Size(12, 11);
            this.chb_print_tags.TabIndex = 0;
            this.chb_print_tags.TabStop = false;
            this.chb_print_tags.UseVisualStyleBackColor = true;
            // 
            // chb_open_file
            // 
            this.chb_open_file.AutoSize = true;
            this.chb_open_file.BackColor = System.Drawing.Color.White;
            this.chb_open_file.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_open_file.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_open_file.Location = new System.Drawing.Point(253, 64);
            this.chb_open_file.Name = "chb_open_file";
            this.chb_open_file.Size = new System.Drawing.Size(12, 11);
            this.chb_open_file.TabIndex = 1;
            this.chb_open_file.TabStop = false;
            this.chb_open_file.UseVisualStyleBackColor = true;
            // 
            // chb_open_directory
            // 
            this.chb_open_directory.AutoSize = true;
            this.chb_open_directory.BackColor = System.Drawing.Color.White;
            this.chb_open_directory.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_open_directory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_open_directory.Location = new System.Drawing.Point(253, 39);
            this.chb_open_directory.Name = "chb_open_directory";
            this.chb_open_directory.Size = new System.Drawing.Size(12, 11);
            this.chb_open_directory.TabIndex = 2;
            this.chb_open_directory.TabStop = false;
            this.chb_open_directory.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Replace non-text elements with tags:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(187, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Open containing directory when done:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Open output file when done:";
            // 
            // but_ok
            // 
            this.but_ok.Location = new System.Drawing.Point(109, 93);
            this.but_ok.Name = "but_ok";
            this.but_ok.Size = new System.Drawing.Size(75, 23);
            this.but_ok.TabIndex = 6;
            this.but_ok.Text = "OK";
            this.but_ok.UseVisualStyleBackColor = true;
            // 
            // but_cancel
            // 
            this.but_cancel.Location = new System.Drawing.Point(190, 93);
            this.but_cancel.Name = "but_cancel";
            this.but_cancel.Size = new System.Drawing.Size(75, 23);
            this.but_cancel.TabIndex = 7;
            this.but_cancel.Text = "Cancel";
            this.but_cancel.UseVisualStyleBackColor = true;
            // 
            // Notepad_Printer_Dialog
            // 
            this.ClientSize = new System.Drawing.Size(277, 124);
            this.Controls.Add(this.but_cancel);
            this.Controls.Add(this.but_ok);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chb_open_directory);
            this.Controls.Add(this.chb_open_file);
            this.Controls.Add(this.chb_print_tags);
            this.Name = "Notepad_Printer_Dialog";
            this.Text = "Printing Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }

}