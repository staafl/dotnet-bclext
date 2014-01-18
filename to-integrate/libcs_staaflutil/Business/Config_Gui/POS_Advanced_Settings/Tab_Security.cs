using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Common;
using Common.Controls;
using DTA;
using Fairweather.Service;
using Standardization;
namespace Config_Gui
{
    public partial class Tab_Security : DTA_Tab
    {
        public Tab_Security() {

            InitializeComponent();


        }

        public override Dictionary<Control, Ini_Field> Get_Fields() {
            return new Dictionary<Control, Ini_Field> {
                {tb_super_pass, DTA_Fields.POS_super_pass},
                {cb_inv_cancel_allowed_to, DTA_Fields.POS_invoice_cancellation_allowed_to},
                {cb_line_cancel_allowed_to, DTA_Fields.POS_lines_cancellation_allowed_to},
                {cb_qty_decrease_allowed_to, DTA_Fields.POS_qty_decrease_allowed_to},
                {cb_date_changing, DTA_Fields.POS_date_editable},
                {cb_document_edit_permissions, DTA_Fields.POS_document_editing_permissions},
                {cb_document_view_permissions, DTA_Fields.POS_document_viewing_permissions},
                {cb_product_details, DTA_Fields.POS_view_product_details},
                {cb_end_of_day, DTA_Fields.POS_end_of_day_security},

            };
        }

        #region designer

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        ComboBox cb_line_cancel_allowed_to;
        ComboBox cb_qty_decrease_allowed_to;
        ComboBox cb_inv_cancel_allowed_to;
        Label label32;
        Label label15;
        Label label14;
        Label label1;
        ComboBox cb_date_changing;
        Label label2;
        Our_Text_Box tb_super_pass;
        Label label3;
        ComboBox cb_document_edit_permissions;
        Label label4;
        ComboBox cb_document_view_permissions;
        Fake_Group_Box fake_Group_Box1;
        Fake_Group_Box fake_Group_Box2;
        Panel panel1;
        Panel panel2;
        ComboBox cb_product_details;
        Label label5;
        private ComboBox cb_end_of_day;
        private Label label6;

        System.ComponentModel.IContainer components = null;

        void InitializeComponent() {
            this.cb_line_cancel_allowed_to = new System.Windows.Forms.ComboBox();
            this.cb_qty_decrease_allowed_to = new System.Windows.Forms.ComboBox();
            this.cb_inv_cancel_allowed_to = new System.Windows.Forms.ComboBox();
            this.label32 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_date_changing = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_super_pass = new Common.Controls.Our_Text_Box();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_document_edit_permissions = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_document_view_permissions = new System.Windows.Forms.ComboBox();
            this.fake_Group_Box1 = new Common.Controls.Fake_Group_Box();
            this.fake_Group_Box2 = new Common.Controls.Fake_Group_Box();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_product_details = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_end_of_day = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_line_cancel_allowed_to
            // 
            this.cb_line_cancel_allowed_to.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_line_cancel_allowed_to.FormattingEnabled = true;
            this.cb_line_cancel_allowed_to.Location = new System.Drawing.Point(47, 29);
            this.cb_line_cancel_allowed_to.Name = "cb_line_cancel_allowed_to";
            this.cb_line_cancel_allowed_to.Size = new System.Drawing.Size(116, 21);
            this.cb_line_cancel_allowed_to.TabIndex = 20;
            this.cb_line_cancel_allowed_to.DropDownClosed += new System.EventHandler(this.cb_DropDownClosed);
            // 
            // cb_qty_decrease_allowed_to
            // 
            this.cb_qty_decrease_allowed_to.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_qty_decrease_allowed_to.FormattingEnabled = true;
            this.cb_qty_decrease_allowed_to.Location = new System.Drawing.Point(47, 81);
            this.cb_qty_decrease_allowed_to.Name = "cb_qty_decrease_allowed_to";
            this.cb_qty_decrease_allowed_to.Size = new System.Drawing.Size(116, 21);
            this.cb_qty_decrease_allowed_to.TabIndex = 40;
            this.cb_qty_decrease_allowed_to.DropDownClosed += new System.EventHandler(this.cb_DropDownClosed);
            // 
            // cb_inv_cancel_allowed_to
            // 
            this.cb_inv_cancel_allowed_to.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_inv_cancel_allowed_to.FormattingEnabled = true;
            this.cb_inv_cancel_allowed_to.Location = new System.Drawing.Point(47, 55);
            this.cb_inv_cancel_allowed_to.Name = "cb_inv_cancel_allowed_to";
            this.cb_inv_cancel_allowed_to.Size = new System.Drawing.Size(116, 21);
            this.cb_inv_cancel_allowed_to.TabIndex = 30;
            this.cb_inv_cancel_allowed_to.DropDownClosed += new System.EventHandler(this.cb_DropDownClosed);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(26, 40);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(108, 13);
            this.label32.TabIndex = 26;
            this.label32.Text = "Supervisor password:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(26, 118);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(96, 13);
            this.label15.TabIndex = 23;
            this.label15.Text = "Quantity decrease:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(26, 92);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(105, 13);
            this.label14.TabIndex = 24;
            this.label14.Text = "Invoice cancellation:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Product line deletion:";
            // 
            // cb_date_changing
            // 
            this.cb_date_changing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_date_changing.FormattingEnabled = true;
            this.cb_date_changing.Location = new System.Drawing.Point(47, 107);
            this.cb_date_changing.Name = "cb_date_changing";
            this.cb_date_changing.Size = new System.Drawing.Size(116, 21);
            this.cb_date_changing.TabIndex = 42;
            this.cb_date_changing.DropDownClosed += new System.EventHandler(this.cb_DropDownClosed);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 41;
            this.label2.Text = "Date selection:";
            // 
            // tb_super_pass
            // 
            this.tb_super_pass.Auto_Highlight = false;
            this.tb_super_pass.BackColor = System.Drawing.Color.White;
            this.tb_super_pass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_super_pass.Is_Readonly = false;
            this.tb_super_pass.Location = new System.Drawing.Point(45, 4);
            this.tb_super_pass.Name = "tb_super_pass";
            this.tb_super_pass.Right_Padding = 0;
            this.tb_super_pass.Size = new System.Drawing.Size(118, 20);
            this.tb_super_pass.TabIndex = 10;
            this.tb_super_pass.TabOnEnter = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 266);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "Documents visible by supervisor:";
            // 
            // cb_document_edit_permissions
            // 
            this.cb_document_edit_permissions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_document_edit_permissions.FormattingEnabled = true;
            this.cb_document_edit_permissions.Location = new System.Drawing.Point(12, 32);
            this.cb_document_edit_permissions.Name = "cb_document_edit_permissions";
            this.cb_document_edit_permissions.Size = new System.Drawing.Size(147, 21);
            this.cb_document_edit_permissions.TabIndex = 44;
            this.cb_document_edit_permissions.SelectedIndexChanged += new System.EventHandler(this.cb_document_permissions_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 293);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(169, 13);
            this.label4.TabIndex = 43;
            this.label4.Text = "Documents editable by supervisor:";
            // 
            // cb_document_view_permissions
            // 
            this.cb_document_view_permissions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_document_view_permissions.FormattingEnabled = true;
            this.cb_document_view_permissions.Location = new System.Drawing.Point(12, 5);
            this.cb_document_view_permissions.Name = "cb_document_view_permissions";
            this.cb_document_view_permissions.Size = new System.Drawing.Size(147, 21);
            this.cb_document_view_permissions.TabIndex = 44;
            this.cb_document_view_permissions.SelectedIndexChanged += new System.EventHandler(this.cb_document_permissions_SelectedIndexChanged);
            // 
            // fake_Group_Box1
            // 
            this.fake_Group_Box1.Location = new System.Drawing.Point(5, 242);
            this.fake_Group_Box1.Name = "fake_Group_Box1";
            this.fake_Group_Box1.Size = new System.Drawing.Size(400, 88);
            this.fake_Group_Box1.TabIndex = 45;
            this.fake_Group_Box1.Text = "Special Supervisor Rights";
            // 
            // fake_Group_Box2
            // 
            this.fake_Group_Box2.Location = new System.Drawing.Point(5, 19);
            this.fake_Group_Box2.Name = "fake_Group_Box2";
            this.fake_Group_Box2.Size = new System.Drawing.Size(400, 217);
            this.fake_Group_Box2.TabIndex = 46;
            this.fake_Group_Box2.Text = "General";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tb_super_pass);
            this.panel1.Controls.Add(this.cb_end_of_day);
            this.panel1.Controls.Add(this.cb_product_details);
            this.panel1.Controls.Add(this.cb_date_changing);
            this.panel1.Controls.Add(this.cb_line_cancel_allowed_to);
            this.panel1.Controls.Add(this.cb_qty_decrease_allowed_to);
            this.panel1.Controls.Add(this.cb_inv_cancel_allowed_to);
            this.panel1.Location = new System.Drawing.Point(200, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(184, 184);
            this.panel1.TabIndex = 0;
            // 
            // cb_product_details
            // 
            this.cb_product_details.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_product_details.FormattingEnabled = true;
            this.cb_product_details.Location = new System.Drawing.Point(47, 133);
            this.cb_product_details.Name = "cb_product_details";
            this.cb_product_details.Size = new System.Drawing.Size(116, 21);
            this.cb_product_details.TabIndex = 42;
            this.cb_product_details.DropDownClosed += new System.EventHandler(this.cb_DropDownClosed);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cb_document_view_permissions);
            this.panel2.Controls.Add(this.cb_document_edit_permissions);
            this.panel2.Location = new System.Drawing.Point(204, 257);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(179, 62);
            this.panel2.TabIndex = 47;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 171);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(193, 13);
            this.label5.TabIndex = 41;
            this.label5.Text = "View Product Details and Sales History:";
            // 
            // cb_end_of_day
            // 
            this.cb_end_of_day.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_end_of_day.FormattingEnabled = true;
            this.cb_end_of_day.Location = new System.Drawing.Point(47, 159);
            this.cb_end_of_day.Name = "cb_end_of_day";
            this.cb_end_of_day.Size = new System.Drawing.Size(116, 21);
            this.cb_end_of_day.TabIndex = 42;
            this.cb_end_of_day.DropDownClosed += new System.EventHandler(this.cb_DropDownClosed);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 198);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 41;
            this.label6.Text = "End of Day:";
            // 
            // Tab_Security
            // 
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.fake_Group_Box1);
            this.Controls.Add(this.fake_Group_Box2);
            this.Name = "Tab_Security";
            this.Size = new System.Drawing.Size(416, 337);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        void cb_DropDownClosed(object sender, EventArgs e) {
            M.Refresh_Combobox_Borders(sender as ComboBox, new ComboBox[]{
                        cb_line_cancel_allowed_to, cb_qty_decrease_allowed_to, cb_inv_cancel_allowed_to, cb_date_changing});

        }

        void cb_document_permissions_SelectedIndexChanged(object sender, EventArgs e) {

            var view_ix = cb_document_view_permissions.SelectedIndex;
            var edit_ix = cb_document_edit_permissions.SelectedIndex;

            if (view_ix >= edit_ix)
                return;

            Standard.Warn("The 'Documents visible to supervisor' setting needs to be \n"
                        + "at least as permissive as 'Documents editable by supervisor'.");

            var cb = (ComboBox)sender;

            cb.SelectedIndex = (cb == cb_document_view_permissions ? edit_ix : view_ix);
        }


    }
}

