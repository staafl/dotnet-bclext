namespace Common.Dialogs
{
    using Common.Controls;


    partial class Search_Form
    {
        /// <summary>  Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

        /// <summary>  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>  Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent() {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv_1 = new Common.Controls.Search_DGV();
            this.col_join = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_precedence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_field = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_cond = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_predefined_1 = new Common.Controls.Our_Text_Box();
            this.lab_predefined_1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lab_predefined_3 = new System.Windows.Forms.Label();
            this.lab_predefined_2 = new System.Windows.Forms.Label();
            this.tb_predefined_3 = new Common.Controls.Our_Text_Box();
            this.tb_predefined_2 = new Common.Controls.Our_Text_Box();
            this.dgv_results = new System.Windows.Forms.DataGridView();
            this.col_account = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_total_balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lab_results = new System.Windows.Forms.Label();
            this.but_ok = new System.Windows.Forms.Button();
            this.but_cancel = new System.Windows.Forms.Button();
            this.but_apply = new System.Windows.Forms.Button();
            this.bur_keyboard = new System.Windows.Forms.Button();
            this.but_calculate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_results)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_1
            // 
            this.dgv_1.Active_Control = null;
            this.dgv_1.AllowUserToAddRows = false;
            this.dgv_1.AllowUserToDeleteRows = false;
            this.dgv_1.AllowUserToResizeRows = false;
            this.dgv_1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_1.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dgv_1.Case_Insensitive_Contains = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_1.ColumnLengths = null;
            this.dgv_1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_join,
            this.col_precedence,
            this.col_field,
            this.col_cond,
            this.col_value});
            this.dgv_1.ColumnTypes = null;
            this.dgv_1.Location = new System.Drawing.Point(12, 34);
            this.dgv_1.Name = "dgv_1";
            this.dgv_1.Record_Type = Common.Record_Type.Undefined;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_1.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_1.RowHeadersWidth = 16;
            this.dgv_1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv_1.RowTemplate.Height = 21;
            this.dgv_1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgv_1.ShowCellErrors = false;
            this.dgv_1.ShowCellToolTips = false;
            this.dgv_1.ShowEditingIcon = false;
            this.dgv_1.Size = new System.Drawing.Size(626, 123);
            this.dgv_1.StandardTab = true;
            this.dgv_1.TabIndex = 10;
            // 
            // col_join
            // 
            this.col_join.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_join.FillWeight = 50F;
            this.col_join.HeaderText = "Join";
            this.col_join.Name = "col_join";
            this.col_join.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_join.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // col_precedence
            // 
            this.col_precedence.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_precedence.FillWeight = 48F;
            this.col_precedence.HeaderText = "Precedence";
            this.col_precedence.Name = "col_precedence";
            this.col_precedence.ReadOnly = true;
            // 
            // col_field
            // 
            this.col_field.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_field.FillWeight = 110F;
            this.col_field.HeaderText = "Field";
            this.col_field.Name = "col_field";
            this.col_field.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_field.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // col_cond
            // 
            this.col_cond.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_cond.FillWeight = 70F;
            this.col_cond.HeaderText = "Condition";
            this.col_cond.Name = "col_cond";
            this.col_cond.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_cond.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // col_value
            // 
            this.col_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_value.HeaderText = "Value";
            this.col_value.Name = "col_value";
            this.col_value.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 1;
            // 
            // tb_predefined_1
            // 
            this.tb_predefined_1.Auto_Highlight = false;
            this.tb_predefined_1.BackColor = System.Drawing.Color.White;
            this.tb_predefined_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_predefined_1.Is_Readonly = false;
            this.tb_predefined_1.Location = new System.Drawing.Point(385, 20);
            this.tb_predefined_1.Name = "tb_predefined_1";
            this.tb_predefined_1.Right_Padding = 0;
            this.tb_predefined_1.Size = new System.Drawing.Size(226, 20);
            this.tb_predefined_1.TabIndex = 20;
            this.tb_predefined_1.TabOnEnter = false;
            this.tb_predefined_1.TextChanged += new System.EventHandler(this.tb_predefined_TextChanged);
            // 
            // lab_predefined_1
            // 
            this.lab_predefined_1.AutoSize = true;
            this.lab_predefined_1.Location = new System.Drawing.Point(16, 20);
            this.lab_predefined_1.Name = "lab_predefined_1";
            this.lab_predefined_1.Size = new System.Drawing.Size(16, 13);
            this.lab_predefined_1.TabIndex = 3;
            this.lab_predefined_1.Text = "...";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lab_predefined_3);
            this.groupBox1.Controls.Add(this.lab_predefined_2);
            this.groupBox1.Controls.Add(this.lab_predefined_1);
            this.groupBox1.Controls.Add(this.tb_predefined_3);
            this.groupBox1.Controls.Add(this.tb_predefined_2);
            this.groupBox1.Controls.Add(this.tb_predefined_1);
            this.groupBox1.Location = new System.Drawing.Point(12, 163);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(626, 109);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search on predefined fields:";
            // 
            // lab_predefined_3
            // 
            this.lab_predefined_3.AutoSize = true;
            this.lab_predefined_3.Location = new System.Drawing.Point(16, 73);
            this.lab_predefined_3.Name = "lab_predefined_3";
            this.lab_predefined_3.Size = new System.Drawing.Size(16, 13);
            this.lab_predefined_3.TabIndex = 5;
            this.lab_predefined_3.Text = "...";
            // 
            // lab_predefined_2
            // 
            this.lab_predefined_2.AutoSize = true;
            this.lab_predefined_2.Location = new System.Drawing.Point(16, 47);
            this.lab_predefined_2.Name = "lab_predefined_2";
            this.lab_predefined_2.Size = new System.Drawing.Size(16, 13);
            this.lab_predefined_2.TabIndex = 4;
            this.lab_predefined_2.Text = "...";
            // 
            // tb_predefined_3
            // 
            this.tb_predefined_3.Auto_Highlight = false;
            this.tb_predefined_3.BackColor = System.Drawing.Color.White;
            this.tb_predefined_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_predefined_3.Is_Readonly = false;
            this.tb_predefined_3.Location = new System.Drawing.Point(385, 75);
            this.tb_predefined_3.Name = "tb_predefined_3";
            this.tb_predefined_3.Right_Padding = 0;
            this.tb_predefined_3.Size = new System.Drawing.Size(226, 20);
            this.tb_predefined_3.TabIndex = 40;
            this.tb_predefined_3.TabOnEnter = false;
            this.tb_predefined_3.TextChanged += new System.EventHandler(this.tb_predefined_TextChanged);
            // 
            // tb_predefined_2
            // 
            this.tb_predefined_2.Auto_Highlight = false;
            this.tb_predefined_2.BackColor = System.Drawing.Color.White;
            this.tb_predefined_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_predefined_2.Is_Readonly = false;
            this.tb_predefined_2.Location = new System.Drawing.Point(385, 47);
            this.tb_predefined_2.Name = "tb_predefined_2";
            this.tb_predefined_2.Right_Padding = 0;
            this.tb_predefined_2.Size = new System.Drawing.Size(226, 20);
            this.tb_predefined_2.TabIndex = 30;
            this.tb_predefined_2.TabOnEnter = false;
            this.tb_predefined_2.TextChanged += new System.EventHandler(this.tb_predefined_TextChanged);
            // 
            // dgv_results
            // 
            this.dgv_results.AllowUserToAddRows = false;
            this.dgv_results.AllowUserToDeleteRows = false;
            this.dgv_results.AllowUserToResizeRows = false;
            this.dgv_results.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_results.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_results.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_results.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_results.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_account,
            this.col_name,
            this.col_phone,
            this.col_balance,
            this.col_total_balance});
            this.dgv_results.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgv_results.Enabled = false;
            this.dgv_results.Location = new System.Drawing.Point(12, 296);
            this.dgv_results.Name = "dgv_results";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_results.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_results.RowHeadersWidth = 16;
            this.dgv_results.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv_results.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_results.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_results.ShowEditingIcon = false;
            this.dgv_results.Size = new System.Drawing.Size(626, 195);
            this.dgv_results.StandardTab = true;
            this.dgv_results.TabIndex = 30;
            // 
            // col_account
            // 
            this.col_account.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_account.HeaderText = "A/C Ref";
            this.col_account.Name = "col_account";
            this.col_account.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_account.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // col_name
            // 
            this.col_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_name.HeaderText = "Name";
            this.col_name.Name = "col_name";
            this.col_name.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // col_phone
            // 
            this.col_phone.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_phone.HeaderText = "Telephone";
            this.col_phone.Name = "col_phone";
            this.col_phone.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_phone.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // col_balance
            // 
            this.col_balance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_balance.HeaderText = "Balance";
            this.col_balance.Name = "col_balance";
            this.col_balance.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_balance.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // col_total_balance
            // 
            this.col_total_balance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_total_balance.HeaderText = "Total Balance";
            this.col_total_balance.Name = "col_total_balance";
            this.col_total_balance.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.col_total_balance.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // lab_results
            // 
            this.lab_results.AutoSize = true;
            this.lab_results.Location = new System.Drawing.Point(11, 278);
            this.lab_results.Name = "lab_results";
            this.lab_results.Size = new System.Drawing.Size(42, 13);
            this.lab_results.TabIndex = 5;
            this.lab_results.Text = "Results";
            // 
            // but_ok
            // 
            this.but_ok.Location = new System.Drawing.Point(482, 499);
            this.but_ok.Name = "but_ok";
            this.but_ok.Size = new System.Drawing.Size(75, 23);
            this.but_ok.TabIndex = 50;
            this.but_ok.Text = "OK";
            this.but_ok.UseVisualStyleBackColor = true;
            // 
            // but_cancel
            // 
            this.but_cancel.Location = new System.Drawing.Point(563, 499);
            this.but_cancel.Name = "but_cancel";
            this.but_cancel.Size = new System.Drawing.Size(75, 23);
            this.but_cancel.TabIndex = 60;
            this.but_cancel.Text = "Cancel";
            this.but_cancel.UseVisualStyleBackColor = true;
            // 
            // but_apply
            // 
            this.but_apply.Location = new System.Drawing.Point(401, 499);
            this.but_apply.Name = "but_apply";
            this.but_apply.Size = new System.Drawing.Size(75, 23);
            this.but_apply.TabIndex = 40;
            this.but_apply.Text = "&Apply";
            this.but_apply.UseVisualStyleBackColor = true;
            // 
            // bur_keyboard
            // 
            this.bur_keyboard.Location = new System.Drawing.Point(12, 499);
            this.bur_keyboard.Name = "bur_keyboard";
            this.bur_keyboard.Size = new System.Drawing.Size(75, 23);
            this.bur_keyboard.TabIndex = 61;
            this.bur_keyboard.Text = "Keyboard";
            this.bur_keyboard.UseVisualStyleBackColor = true;
            // 
            // but_calculate
            // 
            this.but_calculate.Location = new System.Drawing.Point(93, 499);
            this.but_calculate.Name = "but_calculate";
            this.but_calculate.Size = new System.Drawing.Size(175, 23);
            this.but_calculate.TabIndex = 62;
            this.but_calculate.Text = "Calculate Total Stock Balance...";
            this.but_calculate.UseVisualStyleBackColor = true;
            this.but_calculate.Visible = false;
            this.but_calculate.Click += new System.EventHandler(this.but_calculate_Click);
            // 
            // Search_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 530);
            this.Controls.Add(this.but_calculate);
            this.Controls.Add(this.bur_keyboard);
            this.Controls.Add(this.but_cancel);
            this.Controls.Add(this.but_apply);
            this.Controls.Add(this.but_ok);
            this.Controls.Add(this.lab_results);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgv_results);
            this.Controls.Add(this.dgv_1);
            this.MaximumSize = new System.Drawing.Size(656, 555);
            this.MinimumSize = new System.Drawing.Size(656, 555);
            this.Name = "Search_Form";
            this.Text = "Custom Search";
            this.Controls.SetChildIndex(this.dgv_1, 0);
            this.Controls.SetChildIndex(this.dgv_results, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.lab_results, 0);
            this.Controls.SetChildIndex(this.but_ok, 0);
            this.Controls.SetChildIndex(this.but_apply, 0);
            this.Controls.SetChildIndex(this.but_cancel, 0);
            this.Controls.SetChildIndex(this.bur_keyboard, 0);
            this.Controls.SetChildIndex(this.but_calculate, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_results)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        Search_DGV dgv_1;
        System.Windows.Forms.Label label1;
        Our_Text_Box tb_predefined_1;
        System.Windows.Forms.Label lab_predefined_1;
        System.Windows.Forms.GroupBox groupBox1;
        System.Windows.Forms.Label lab_predefined_3;
        System.Windows.Forms.Label lab_predefined_2;
        Our_Text_Box tb_predefined_3;
        Our_Text_Box tb_predefined_2;
        System.Windows.Forms.DataGridView dgv_results;
        System.Windows.Forms.DataGridViewTextBoxColumn col_account;
        System.Windows.Forms.DataGridViewTextBoxColumn col_name;
        System.Windows.Forms.DataGridViewTextBoxColumn col_phone;
        System.Windows.Forms.DataGridViewTextBoxColumn col_balance;
        System.Windows.Forms.DataGridViewTextBoxColumn col_total_balance;

        System.Windows.Forms.Label lab_results;
        System.Windows.Forms.Button but_ok;
        System.Windows.Forms.Button but_cancel;
        System.Windows.Forms.Button but_apply;
        System.Windows.Forms.DataGridViewTextBoxColumn col_join;
        System.Windows.Forms.DataGridViewTextBoxColumn col_precedence;
        System.Windows.Forms.DataGridViewTextBoxColumn col_field;
        System.Windows.Forms.DataGridViewTextBoxColumn col_cond;
        System.Windows.Forms.DataGridViewTextBoxColumn col_value;
        System.Windows.Forms.Button bur_keyboard;
        System.Windows.Forms.Button but_calculate;
    }
}