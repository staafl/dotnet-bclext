namespace Common.Dialogs
{
    using System.Windows.Forms;

    using Common.Controls;

    partial class Product_Tab2
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

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
        void InitializeComponent() {
            this.dgv_history = new System.Windows.Forms.DataGridView();
            this.col_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_unit_vat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_unit_nvat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_full_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_disc_perc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_posted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.but_get_by_number1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.but_get_all1 = new System.Windows.Forms.Button();
            this.lab_please_select = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.but_get_by_date1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.but_clear = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.but_get_by_number = new System.Windows.Forms.RadioButton();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alab_last_cost = new Common.Controls.Amount_Label();
            this.alab_sales_price = new Common.Controls.Amount_Label();
            this.lab_product = new Common.Controls.Text_Label();
            this.lab_customer = new Common.Controls.Text_Label();
            this.but_get_by_date = new System.Windows.Forms.RadioButton();
            this.but_get_all = new System.Windows.Forms.RadioButton();
            this.lab_please_use_one = new System.Windows.Forms.Label();
            this.lab_no_matching = new System.Windows.Forms.Label();
            this.lab_please_select_customer = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_history)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_history
            // 
            this.dgv_history.AllowUserToAddRows = false;
            this.dgv_history.AllowUserToDeleteRows = false;
            this.dgv_history.AllowUserToResizeRows = false;
            this.dgv_history.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_history.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_number,
            this.col_type,
            this.col_date,
            this.col_unit_vat,
            this.col_unit_nvat,
            this.col_full_price,
            this.col_disc_perc,
            this.col_qty,
            this.col_posted});
            this.dgv_history.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgv_history.Location = new System.Drawing.Point(12, 39);
            this.dgv_history.Name = "dgv_history";
            this.dgv_history.RowHeadersVisible = false;
            this.dgv_history.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_history.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv_history.Size = new System.Drawing.Size(732, 295);
            this.dgv_history.TabIndex = 0;
            // 
            // col_number
            // 
            this.col_number.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.col_number.FillWeight = 102F;
            this.col_number.Frozen = true;
            this.col_number.HeaderText = "Document No.";
            this.col_number.Name = "col_number";
            this.col_number.ReadOnly = true;
            this.col_number.Width = 101;
            // 
            // col_type
            // 
            this.col_type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.col_type.FillWeight = 42F;
            this.col_type.Frozen = true;
            this.col_type.HeaderText = "Type";
            this.col_type.Name = "col_type";
            this.col_type.ReadOnly = true;
            this.col_type.Width = 41;
            // 
            // col_date
            // 
            this.col_date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.col_date.FillWeight = 67F;
            this.col_date.Frozen = true;
            this.col_date.HeaderText = "Date";
            this.col_date.Name = "col_date";
            this.col_date.ReadOnly = true;
            this.col_date.Width = 66;
            // 
            // col_unit_vat
            // 
            this.col_unit_vat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_unit_vat.FillWeight = 113F;
            this.col_unit_vat.HeaderText = "Doc. Sales Price\nVAT Inclusive";
            this.col_unit_vat.Name = "col_unit_vat";
            this.col_unit_vat.ReadOnly = true;
            // 
            // col_unit_nvat
            // 
            this.col_unit_nvat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_unit_nvat.FillWeight = 113F;
            this.col_unit_nvat.HeaderText = "Doc. Sales Price\nVAT Exclusive";
            this.col_unit_nvat.Name = "col_unit_nvat";
            this.col_unit_nvat.ReadOnly = true;
            // 
            // col_full_price
            // 
            this.col_full_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_full_price.FillWeight = 136F;
            this.col_full_price.HeaderText = "Doc. Full Sales Price VAT Exclusive";
            this.col_full_price.Name = "col_full_price";
            // 
            // col_disc_perc
            // 
            this.col_disc_perc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_disc_perc.FillWeight = 68F;
            this.col_disc_perc.HeaderText = "Disc. %";
            this.col_disc_perc.Name = "col_disc_perc";
            this.col_disc_perc.ReadOnly = true;
            // 
            // col_qty
            // 
            this.col_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_qty.FillWeight = 51F;
            this.col_qty.HeaderText = "Doc. Quantity";
            this.col_qty.Name = "col_qty";
            this.col_qty.ReadOnly = true;
            // 
            // col_posted
            // 
            this.col_posted.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_posted.FillWeight = 44F;
            this.col_posted.HeaderText = "Posted";
            this.col_posted.Name = "col_posted";
            this.col_posted.ReadOnly = true;
            // 
            // but_get_by_number1
            // 
            this.but_get_by_number1.Location = new System.Drawing.Point(436, 369);
            this.but_get_by_number1.Name = "but_get_by_number1";
            this.but_get_by_number1.Size = new System.Drawing.Size(151, 23);
            this.but_get_by_number1.TabIndex = 30;
            this.but_get_by_number1.Text = "Last Document (by num&ber)";
            this.but_get_by_number1.UseVisualStyleBackColor = true;
            this.but_get_by_number1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(194, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Customer:";
            // 
            // but_get_all1
            // 
            this.but_get_all1.Location = new System.Drawing.Point(654, 340);
            this.but_get_all1.Name = "but_get_all1";
            this.but_get_all1.Size = new System.Drawing.Size(90, 23);
            this.but_get_all1.TabIndex = 40;
            this.but_get_all1.Text = "&All Documents";
            this.but_get_all1.UseVisualStyleBackColor = true;
            this.but_get_all1.Visible = false;
            // 
            // lab_please_select
            // 
            this.lab_please_select.AutoSize = true;
            this.lab_please_select.Location = new System.Drawing.Point(187, 190);
            this.lab_please_select.Name = "lab_please_select";
            this.lab_please_select.Size = new System.Drawing.Size(366, 13);
            this.lab_please_select.TabIndex = 4;
            this.lab_please_select.Text = "Please select a product from the \'Details\' tab in order to see its Sales History." +
                "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Product Code:";
            // 
            // but_get_by_date1
            // 
            this.but_get_by_date1.Location = new System.Drawing.Point(593, 369);
            this.but_get_by_date1.Name = "but_get_by_date1";
            this.but_get_by_date1.Size = new System.Drawing.Size(151, 23);
            this.but_get_by_date1.TabIndex = 33;
            this.but_get_by_date1.Text = "Last Document (by da&te)";
            this.but_get_by_date1.UseVisualStyleBackColor = true;
            this.but_get_by_date1.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(355, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 26);
            this.label3.TabIndex = 34;
            this.label3.Text = "Current Sales Price\r\n(VAT Exclusive):";
            // 
            // but_clear
            // 
            this.but_clear.Location = new System.Drawing.Point(395, 340);
            this.but_clear.Name = "but_clear";
            this.but_clear.Size = new System.Drawing.Size(75, 23);
            this.but_clear.TabIndex = 59;
            this.but_clear.Text = "&Clear";
            this.but_clear.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(565, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "Last Cost Price:";
            // 
            // but_get_by_number
            // 
            this.but_get_by_number.Appearance = System.Windows.Forms.Appearance.Button;
            this.but_get_by_number.AutoSize = true;
            this.but_get_by_number.Location = new System.Drawing.Point(12, 340);
            this.but_get_by_number.Name = "but_get_by_number";
            this.but_get_by_number.Size = new System.Drawing.Size(147, 23);
            this.but_get_by_number.TabIndex = 60;
            this.but_get_by_number.TabStop = true;
            this.but_get_by_number.Text = "Last Document (by num&ber)";
            this.but_get_by_number.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.FillWeight = 80F;
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "Document No.";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.FillWeight = 32F;
            this.dataGridViewTextBoxColumn2.Frozen = true;
            this.dataGridViewTextBoxColumn2.HeaderText = "Type";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.FillWeight = 53F;
            this.dataGridViewTextBoxColumn3.Frozen = true;
            this.dataGridViewTextBoxColumn3.HeaderText = "Date";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.FillWeight = 110F;
            this.dataGridViewTextBoxColumn4.HeaderText = "Stock Unit Price\nVAT Exclusive";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.FillWeight = 110F;
            this.dataGridViewTextBoxColumn5.HeaderText = "Document Sales Price\nVAT Exclusive";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.FillWeight = 136F;
            this.dataGridViewTextBoxColumn6.HeaderText = "Discounted Price\nVAT Inclusive";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn7.FillWeight = 60F;
            this.dataGridViewTextBoxColumn7.HeaderText = "Discount %";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn8.FillWeight = 45F;
            this.dataGridViewTextBoxColumn8.HeaderText = "Document Quantity";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn9.FillWeight = 38F;
            this.dataGridViewTextBoxColumn9.HeaderText = "Posted";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // alab_last_cost
            // 
            this.alab_last_cost.AutoEllipsis = true;
            this.alab_last_cost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.alab_last_cost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.alab_last_cost.Decimal_Places = 2;
            this.alab_last_cost.Default_Format = "F2";
            this.alab_last_cost.Location = new System.Drawing.Point(652, 7);
            this.alab_last_cost.Name = "alab_last_cost";
            this.alab_last_cost.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.alab_last_cost.Size = new System.Drawing.Size(92, 20);
            this.alab_last_cost.TabIndex = 35;
            this.alab_last_cost.Text = "0.00";
            this.alab_last_cost.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.alab_last_cost.UseMnemonic = false;
            this.alab_last_cost.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // alab_sales_price
            // 
            this.alab_sales_price.AutoEllipsis = true;
            this.alab_sales_price.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.alab_sales_price.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.alab_sales_price.Decimal_Places = 2;
            this.alab_sales_price.Default_Format = "F2";
            this.alab_sales_price.Location = new System.Drawing.Point(464, 7);
            this.alab_sales_price.Name = "alab_sales_price";
            this.alab_sales_price.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.alab_sales_price.Size = new System.Drawing.Size(92, 20);
            this.alab_sales_price.TabIndex = 35;
            this.alab_sales_price.Text = "0.00";
            this.alab_sales_price.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.alab_sales_price.UseMnemonic = false;
            this.alab_sales_price.Value = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            // 
            // lab_product
            // 
            this.lab_product.AutoEllipsis = true;
            this.lab_product.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.lab_product.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lab_product.Location = new System.Drawing.Point(90, 7);
            this.lab_product.Name = "lab_product";
            this.lab_product.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.lab_product.Size = new System.Drawing.Size(92, 20);
            this.lab_product.TabIndex = 32;
            this.lab_product.Text = "...";
            this.lab_product.UseMnemonic = false;
            // 
            // lab_customer
            // 
            this.lab_customer.AutoEllipsis = true;
            this.lab_customer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.lab_customer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lab_customer.Location = new System.Drawing.Point(252, 7);
            this.lab_customer.Name = "lab_customer";
            this.lab_customer.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.lab_customer.Size = new System.Drawing.Size(92, 20);
            this.lab_customer.TabIndex = 3;
            this.lab_customer.Text = "...";
            this.lab_customer.UseMnemonic = false;
            // 
            // but_get_by_date
            // 
            this.but_get_by_date.Appearance = System.Windows.Forms.Appearance.Button;
            this.but_get_by_date.AutoSize = true;
            this.but_get_by_date.Location = new System.Drawing.Point(165, 340);
            this.but_get_by_date.Name = "but_get_by_date";
            this.but_get_by_date.Size = new System.Drawing.Size(133, 23);
            this.but_get_by_date.TabIndex = 60;
            this.but_get_by_date.TabStop = true;
            this.but_get_by_date.Text = "Last Document (by da&te)";
            this.but_get_by_date.UseVisualStyleBackColor = true;
            // 
            // but_get_all
            // 
            this.but_get_all.Appearance = System.Windows.Forms.Appearance.Button;
            this.but_get_all.AutoSize = true;
            this.but_get_all.Location = new System.Drawing.Point(304, 340);
            this.but_get_all.Name = "but_get_all";
            this.but_get_all.Size = new System.Drawing.Size(85, 23);
            this.but_get_all.TabIndex = 60;
            this.but_get_all.TabStop = true;
            this.but_get_all.Text = "&All Documents";
            this.but_get_all.UseVisualStyleBackColor = true;
            // 
            // lab_please_use_one
            // 
            this.lab_please_use_one.AutoSize = true;
            this.lab_please_use_one.Location = new System.Drawing.Point(242, 190);
            this.lab_please_use_one.Name = "lab_please_use_one";
            this.lab_please_use_one.Size = new System.Drawing.Size(256, 13);
            this.lab_please_use_one.TabIndex = 61;
            this.lab_please_use_one.Text = "Please use one of the buttons below to retrieve data.";
            // 
            // lab_no_matching
            // 
            this.lab_no_matching.AutoSize = true;
            this.lab_no_matching.Location = new System.Drawing.Point(312, 190);
            this.lab_no_matching.Name = "lab_no_matching";
            this.lab_no_matching.Size = new System.Drawing.Size(125, 13);
            this.lab_no_matching.TabIndex = 62;
            this.lab_no_matching.Text = "No matching documents.";
            // 
            // lab_please_select_customer
            // 
            this.lab_please_select_customer.AutoSize = true;
            this.lab_please_select_customer.Location = new System.Drawing.Point(171, 190);
            this.lab_please_select_customer.Name = "lab_please_select_customer";
            this.lab_please_select_customer.Size = new System.Drawing.Size(396, 13);
            this.lab_please_select_customer.TabIndex = 4;
            this.lab_please_select_customer.Text = "Please select a customer from the main screen in order to see his/her Sales Histo" +
                "ry";
            // 
            // Product_Tab2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lab_no_matching);
            this.Controls.Add(this.lab_please_use_one);
            this.Controls.Add(this.but_get_all);
            this.Controls.Add(this.but_get_by_date);
            this.Controls.Add(this.but_get_by_number);
            this.Controls.Add(this.alab_last_cost);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.alab_sales_price);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.but_get_by_date1);
            this.Controls.Add(this.lab_product);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lab_please_select);
            this.Controls.Add(this.lab_customer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.but_clear);
            this.Controls.Add(this.but_get_all1);
            this.Controls.Add(this.but_get_by_number1);
            this.Controls.Add(this.lab_please_select_customer);
            this.Controls.Add(this.dgv_history);
            this.Name = "Product_Tab2";
            this.Size = new System.Drawing.Size(764, 405);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_history)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        Button but_get_by_date1;
        Button but_get_all1;
        Button but_get_by_number1;

        internal DataGridView dgv_history;
        internal Label label1;

        internal Label lab_please_select;
        internal Label lab_please_use_one;
        internal Label lab_no_matching;
        internal Label lab_please_select_customer;

        internal Text_Label lab_product;
        internal Text_Label lab_customer;
        internal Label label2;
        DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        internal Label label3;
        internal Amount_Label alab_sales_price;
        internal Button but_clear;
        internal Label label4;
        internal Amount_Label alab_last_cost;
        DataGridViewTextBoxColumn col_number;
        DataGridViewTextBoxColumn col_type;
        DataGridViewTextBoxColumn col_date;
        DataGridViewTextBoxColumn col_unit_vat;
        DataGridViewTextBoxColumn col_unit_nvat;
        DataGridViewTextBoxColumn col_full_price;
        DataGridViewTextBoxColumn col_disc_perc;
        DataGridViewTextBoxColumn col_qty;
        DataGridViewTextBoxColumn col_posted;
        internal RadioButton but_get_by_number;
        internal RadioButton but_get_by_date;
        internal RadioButton but_get_all;

    }
}
