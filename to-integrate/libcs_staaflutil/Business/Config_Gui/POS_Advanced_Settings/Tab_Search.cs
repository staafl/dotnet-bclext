using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Common.Controls;

using DTA;
using Fairweather.Service;

namespace Config_Gui
{
    public partial class Tab_Search : DTA_Tab
    {
        readonly CheckBox[] rdchbxs;

        public Tab_Search() {

            InitializeComponent();
            rdchbxs = new CheckBox[]
            {                
                chb_search_address, 
                chb_search_phone, 
                chb_search_email, 
                chb_search_contact, 
                chb_search_name, 
                chb_search_country
            };

        }
        public override Dictionary<Control, Ini_Field> Get_Fields() {

            return new Dictionary<Control, Ini_Field>{
                {chb_search_address, DTA_Fields.POS_search_address},
                {chb_search_phone, DTA_Fields.POS_search_phone},
                {chb_search_email, DTA_Fields.POS_search_email},
                {chb_search_contact, DTA_Fields.POS_search_contact},
                {chb_search_name, DTA_Fields.POS_search_name},
                {chb_search_country, DTA_Fields.POS_search_country},
                {chb_search_case_sensitive, DTA_Fields.POS_search_case_sensitive},
            };

        }
        #region designer

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        Label label2;
        Label label3;
        Label label4;
        Label label5;
        Label label6;
        Label label7;
        Flat_Check_Box chb_search_name;
        Flat_Check_Box chb_search_phone;
        Flat_Check_Box chb_search_address;
        Flat_Check_Box chb_search_contact;
        Flat_Check_Box chb_search_country;
        Flat_Check_Box chb_search_email;
        Flat_Check_Box chb_search_case_sensitive;
        Label label8;
        Fake_Group_Box fake_Group_Box1;

        System.ComponentModel.IContainer components = null;

        void InitializeComponent() {
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chb_search_name = new Flat_Check_Box();
            this.chb_search_phone = new Flat_Check_Box();
            this.chb_search_address = new Flat_Check_Box();
            this.chb_search_contact = new Flat_Check_Box();
            this.chb_search_country = new Flat_Check_Box();
            this.chb_search_email = new Flat_Check_Box();
            this.chb_search_case_sensitive = new Flat_Check_Box();
            this.label8 = new System.Windows.Forms.Label();
            this.fake_Group_Box1 = new Common.Controls.Fake_Group_Box();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Telephone:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Address:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "eMail:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Contact Name:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 174);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Country:";
            // 
            // chb_search_name
            // 
            this.chb_search_name.AutoSize = true;
            this.chb_search_name.BackColor = System.Drawing.Color.White;
            this.chb_search_name.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_search_name.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_search_name.Location = new System.Drawing.Point(203, 47);
            this.chb_search_name.Name = "chb_search_name";
            this.chb_search_name.Size = new System.Drawing.Size(12, 11);
            this.chb_search_name.TabIndex = 10;
            this.chb_search_name.TabStop = false;
            this.chb_search_name.UseVisualStyleBackColor = true;
            this.chb_search_name.CheckStateChanged += new System.EventHandler(this.chb_CheckStateChanged);
            // 
            // chb_search_phone
            // 
            this.chb_search_phone.AutoSize = true;
            this.chb_search_phone.BackColor = System.Drawing.Color.White;
            this.chb_search_phone.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_search_phone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_search_phone.Location = new System.Drawing.Point(203, 73);
            this.chb_search_phone.Name = "chb_search_phone";
            this.chb_search_phone.Size = new System.Drawing.Size(12, 11);
            this.chb_search_phone.TabIndex = 30;
            this.chb_search_phone.TabStop = false;
            this.chb_search_phone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_search_phone.UseVisualStyleBackColor = true;
            this.chb_search_phone.CheckStateChanged += new System.EventHandler(this.chb_CheckStateChanged);
            // 
            // chb_search_address
            // 
            this.chb_search_address.AutoSize = true;
            this.chb_search_address.BackColor = System.Drawing.Color.White;
            this.chb_search_address.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_search_address.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_search_address.Location = new System.Drawing.Point(203, 99);
            this.chb_search_address.Name = "chb_search_address";
            this.chb_search_address.Size = new System.Drawing.Size(12, 11);
            this.chb_search_address.TabIndex = 40;
            this.chb_search_address.TabStop = false;
            this.chb_search_address.UseVisualStyleBackColor = true;
            this.chb_search_address.CheckStateChanged += new System.EventHandler(this.chb_CheckStateChanged);
            // 
            // chb_search_contact
            // 
            this.chb_search_contact.AutoSize = true;
            this.chb_search_contact.BackColor = System.Drawing.Color.White;
            this.chb_search_contact.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_search_contact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_search_contact.Location = new System.Drawing.Point(203, 151);
            this.chb_search_contact.Name = "chb_search_contact";
            this.chb_search_contact.Size = new System.Drawing.Size(12, 11);
            this.chb_search_contact.TabIndex = 60;
            this.chb_search_contact.TabStop = false;
            this.chb_search_contact.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_search_contact.UseVisualStyleBackColor = true;
            this.chb_search_contact.CheckStateChanged += new System.EventHandler(this.chb_CheckStateChanged);
            // 
            // chb_search_country
            // 
            this.chb_search_country.AutoSize = true;
            this.chb_search_country.BackColor = System.Drawing.Color.White;
            this.chb_search_country.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_search_country.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_search_country.Location = new System.Drawing.Point(203, 177);
            this.chb_search_country.Name = "chb_search_country";
            this.chb_search_country.Size = new System.Drawing.Size(12, 11);
            this.chb_search_country.TabIndex = 70;
            this.chb_search_country.TabStop = false;
            this.chb_search_country.UseVisualStyleBackColor = true;
            this.chb_search_country.CheckStateChanged += new System.EventHandler(this.chb_CheckStateChanged);
            // 
            // chb_search_email
            // 
            this.chb_search_email.AutoSize = true;
            this.chb_search_email.BackColor = System.Drawing.Color.White;
            this.chb_search_email.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_search_email.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_search_email.Location = new System.Drawing.Point(203, 125);
            this.chb_search_email.Name = "chb_search_email";
            this.chb_search_email.Size = new System.Drawing.Size(12, 11);
            this.chb_search_email.TabIndex = 50;
            this.chb_search_email.TabStop = false;
            this.chb_search_email.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_search_email.UseVisualStyleBackColor = true;
            this.chb_search_email.CheckStateChanged += new System.EventHandler(this.chb_CheckStateChanged);
            // 
            // chb_search_case_sensitive
            // 
            this.chb_search_case_sensitive.AutoSize = true;
            this.chb_search_case_sensitive.BackColor = System.Drawing.Color.White;
            this.chb_search_case_sensitive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chb_search_case_sensitive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chb_search_case_sensitive.Location = new System.Drawing.Point(203, 221);
            this.chb_search_case_sensitive.Name = "chb_search_case_sensitive";
            this.chb_search_case_sensitive.Size = new System.Drawing.Size(12, 11);
            this.chb_search_case_sensitive.TabIndex = 0;
            this.chb_search_case_sensitive.TabStop = false;
            this.chb_search_case_sensitive.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 220);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(117, 13);
            this.label8.TabIndex = 81;
            this.label8.Text = "Case Sensitive Search:";
            // 
            // fake_Group_Box1
            // 
            this.fake_Group_Box1.Location = new System.Drawing.Point(5, 19);
            this.fake_Group_Box1.Name = "fake_Group_Box1";
            this.fake_Group_Box1.Size = new System.Drawing.Size(400, 186);
            this.fake_Group_Box1.TabIndex = 83;
            this.fake_Group_Box1.Text = "Quick Customer Search Fields";
            // 
            // Tab_Search
            // 
            this.Controls.Add(this.chb_search_case_sensitive);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chb_search_country);
            this.Controls.Add(this.chb_search_address);
            this.Controls.Add(this.chb_search_name);
            this.Controls.Add(this.chb_search_email);
            this.Controls.Add(this.chb_search_contact);
            this.Controls.Add(this.chb_search_phone);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.fake_Group_Box1);
            this.Name = "Tab_Search";
            this.Size = new System.Drawing.Size(411, 294);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        void chb_CheckStateChanged(object sender, EventArgs e) {

            int @checked = rdchbxs.Sum(chb => chb.Checked ? 1 : 0);

            if (@checked >= 3) {
                foreach (var chb in rdchbxs) {
                    chb.Enabled = chb.Checked;
                }
            }
            else {
                foreach (var chb in rdchbxs) {
                    chb.Enabled = true;
                }
            }

        }



    }
}

