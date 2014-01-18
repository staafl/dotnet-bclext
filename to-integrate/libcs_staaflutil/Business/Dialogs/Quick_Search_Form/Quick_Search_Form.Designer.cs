namespace Common.Dialogs
{
    using System.Windows.Forms;
    using Common.Controls;

    partial class Quick_Search_Form
    {
        /// <summary>  Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

        /// <summary>  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                if (list_view != null)
                    list_view.RecordsCursor.End();

                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>  Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent() {
            this.group_box = new GroupBox();
            this.label1 = new Label();
            this.our_TextBox1 = new Common.Controls.Our_Text_Box();
            this.list_view = new QSF_ListView();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.but_cancel = new Button();
            this.but_ok = new Button();
            this.but_new = new Button();
            this.but_search = new Button();
            this.group_box.SuspendLayout();
            this.SuspendLayout();
            // 
            // group_box
            // 
            this.group_box.BackColor = System.Drawing.Color.Transparent;
            this.group_box.Controls.Add(this.label1);
            this.group_box.Controls.Add(this.our_TextBox1);
            this.group_box.Controls.Add(this.list_view);
            this.group_box.Controls.Add(this.but_cancel);
            this.group_box.Controls.Add(this.but_ok);
            this.group_box.Controls.Add(this.but_new);
            this.group_box.Controls.Add(this.but_search);
            this.group_box.Location = new System.Drawing.Point(-1, -6);
            this.group_box.Name = "group_box";
            this.group_box.Size = new System.Drawing.Size(345, 211);
            this.group_box.TabIndex = 0;
            this.group_box.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Search:";
            this.label1.Visible = false;
            // 
            // our_TextBox1
            // 
            this.our_TextBox1.Auto_Highlight = false;
            this.our_TextBox1.BackColor = System.Drawing.Color.White;
            this.our_TextBox1.BorderStyle = BorderStyle.FixedSingle;
            this.our_TextBox1.Is_Readonly = false;
            this.our_TextBox1.Location = new System.Drawing.Point(74, 9);
            this.our_TextBox1.Name = "our_TextBox1";
            this.our_TextBox1.Size = new System.Drawing.Size(100, 20);
            this.our_TextBox1.TabIndex = 5;
            this.our_TextBox1.Visible = false;
            // 
            // list_view
            // 
            this.list_view.Columns.AddRange(new ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});

            this.list_view.FullRowSelect = true;
            this.list_view.HideSelection = false;
            this.list_view.Location = new System.Drawing.Point(4, 31);
            this.list_view.MultiSelect = false;
            this.list_view.Name = "list_view";
            this.list_view.OwnerDraw = true;
            this.list_view.Size = new System.Drawing.Size(336, 150);
            this.list_view.Sorting = SortOrder.Ascending;
            this.list_view.TabIndex = 4;
            this.list_view.UseCompatibleStateImageBehavior = false;
            this.list_view.View = View.Details;
            this.list_view.ItemActivate += this.listView_ItemActivate;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "A/C";
            this.columnHeader1.Width = 91;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 225;
            // 
            // but_cancel
            // 
            this.but_cancel.DialogResult = DialogResult.Cancel;
            this.but_cancel.FlatStyle = FlatStyle.System;
            this.but_cancel.Location = new System.Drawing.Point(259, 183);
            this.but_cancel.Name = "but_cancel";
            this.but_cancel.Size = new System.Drawing.Size(81, 23);
            this.but_cancel.TabIndex = 3;
            this.but_cancel.Text = "Cancel";
            this.but_cancel.UseVisualStyleBackColor = true;
            this.but_cancel.Click += new System.EventHandler(this.but_cancel_Click);
            // 
            // but_ok
            // 
            this.but_ok.DialogResult = DialogResult.Cancel;
            this.but_ok.FlatStyle = FlatStyle.System;
            this.but_ok.Location = new System.Drawing.Point(177, 183);
            this.but_ok.Name = "but_ok";
            this.but_ok.Size = new System.Drawing.Size(80, 23);
            this.but_ok.TabIndex = 2;
            this.but_ok.Text = "OK";
            this.but_ok.UseVisualStyleBackColor = true;
            this.but_ok.Click += new System.EventHandler(this.but_ok_Click);
            // 
            // but_new
            // 
            this.but_new.DialogResult = DialogResult.Cancel;
            this.but_new.FlatStyle = FlatStyle.System;
            this.but_new.Location = new System.Drawing.Point(97, 183);
            this.but_new.Name = "but_new";
            this.but_new.Size = new System.Drawing.Size(78, 23);
            this.but_new.TabIndex = 1;
            this.but_new.Text = "&New / Edit";
            this.but_new.UseVisualStyleBackColor = true;
            // 
            // but_search
            // 
            this.but_search.DialogResult = DialogResult.Cancel;
            this.but_search.Enabled = false;
            this.but_search.FlatStyle = FlatStyle.System;
            this.but_search.Location = new System.Drawing.Point(4, 183);
            this.but_search.Name = "but_search";
            this.but_search.Size = new System.Drawing.Size(91, 23);
            this.but_search.TabIndex = 0;
            this.but_search.Text = "&Search";
            this.but_search.UseVisualStyleBackColor = true;

            // 
            // QuickSearchForm
            // 
            this.AcceptButton = this.but_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.but_cancel;
            this.ClientSize = new System.Drawing.Size(344, 205);
            this.ControlBox = false;
            this.Controls.Add(this.group_box);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Name = "QuickSearchForm";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Text = "QuickSearchForm";
            this.TopMost = true;
            this.group_box.ResumeLayout(false);
            this.group_box.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        GroupBox group_box;
        Button but_cancel;
        Button but_ok;
        public Button but_new;
        public Button but_search;
        QSF_ListView list_view;
        ColumnHeader columnHeader1;
        ColumnHeader columnHeader2;
        Label label1;
        Our_Text_Box our_TextBox1;
    }
}