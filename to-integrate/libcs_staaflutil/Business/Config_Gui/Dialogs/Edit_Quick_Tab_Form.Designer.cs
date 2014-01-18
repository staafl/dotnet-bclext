using System.Windows.Forms;
using Common.Controls;
namespace Config_Gui
{
    partial class Edit_Quick_Tab_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.Label label1;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.but_ok = new System.Windows.Forms.Button();
            this.but_cancel = new System.Windows.Forms.Button();
            this.tb_name = new System.Windows.Forms.TextBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.col_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_img = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.but_up = new Common.Controls.Unselectable_Button();
            this.but_down = new Common.Controls.Unselectable_Button();
            this.but_browse = new Common.Controls.Unselectable_Button();
            this.but_delete = new Common.Controls.Unselectable_Button();
            this.but_preview = new Common.Controls.Unselectable_Button();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(9, 14);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(60, 13);
            label1.TabIndex = 0;
            label1.Text = "Tab Name:";
            // 
            // but_ok
            // 
            this.but_ok.Location = new System.Drawing.Point(364, 265);
            this.but_ok.Name = "but_ok";
            this.but_ok.Size = new System.Drawing.Size(75, 23);
            this.but_ok.TabIndex = 0;
            this.but_ok.Text = "OK";
            this.but_ok.UseVisualStyleBackColor = true;
            // 
            // but_cancel
            // 
            this.but_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.but_cancel.Location = new System.Drawing.Point(445, 265);
            this.but_cancel.Name = "but_cancel";
            this.but_cancel.Size = new System.Drawing.Size(75, 23);
            this.but_cancel.TabIndex = 0;
            this.but_cancel.Text = "Cancel";
            this.but_cancel.UseVisualStyleBackColor = true;
            // 
            // tb_name
            // 
            this.tb_name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_name.Location = new System.Drawing.Point(75, 10);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(204, 20);
            this.tb_name.TabIndex = 1;
            // 
            // dgv
            // 
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_name,
            this.col_barcode,
            this.col_img});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgv.Location = new System.Drawing.Point(12, 38);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv.Size = new System.Drawing.Size(508, 221);
            this.dgv.TabIndex = 0;
            // 
            // col_name
            // 
            this.col_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_name.FillWeight = 180F;
            this.col_name.HeaderText = "Name";
            this.col_name.Name = "col_name";
            // 
            // col_barcode
            // 
            this.col_barcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_barcode.FillWeight = 200F;
            this.col_barcode.HeaderText = "Barcode";
            this.col_barcode.Name = "col_barcode";
            // 
            // col_img
            // 
            this.col_img.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_img.FillWeight = 400F;
            this.col_img.HeaderText = "Image";
            this.col_img.Name = "col_img";
            // 
            // but_up
            // 
            this.but_up.Location = new System.Drawing.Point(526, 67);
            this.but_up.Name = "but_up";
            this.but_up.Size = new System.Drawing.Size(75, 23);
            this.but_up.TabIndex = 0;
            this.but_up.Text = "&Up";
            this.but_up.UseVisualStyleBackColor = true;
            // 
            // but_down
            // 
            this.but_down.Location = new System.Drawing.Point(526, 96);
            this.but_down.Name = "but_down";
            this.but_down.Size = new System.Drawing.Size(75, 23);
            this.but_down.TabIndex = 0;
            this.but_down.Text = "&Down";
            this.but_down.UseVisualStyleBackColor = true;
            // 
            // but_browse
            // 
            this.but_browse.Enabled = false;
            this.but_browse.Location = new System.Drawing.Point(526, 125);
            this.but_browse.Name = "but_browse";
            this.but_browse.Size = new System.Drawing.Size(75, 23);
            this.but_browse.TabIndex = 0;
            this.but_browse.Text = "B&rowse...";
            this.but_browse.UseVisualStyleBackColor = true;
            // 
            // but_delete
            // 
            this.but_delete.Location = new System.Drawing.Point(526, 38);
            this.but_delete.Name = "but_delete";
            this.but_delete.Size = new System.Drawing.Size(75, 23);
            this.but_delete.TabIndex = 0;
            this.but_delete.Text = "De&lete";
            this.but_delete.UseVisualStyleBackColor = true;
            // 
            // but_preview
            // 
            this.but_preview.Location = new System.Drawing.Point(526, 154);
            this.but_preview.Name = "but_preview";
            this.but_preview.Size = new System.Drawing.Size(75, 23);
            this.but_preview.TabIndex = 0;
            this.but_preview.Text = "&Preview...";
            this.but_preview.UseVisualStyleBackColor = true;
            // 
            // Edit_Quick_Tab_Form
            // 
            this.AcceptButton = this.but_ok;
            this.CancelButton = this.but_cancel;
            this.ClientSize = new System.Drawing.Size(613, 300);
            this.Controls.Add(label1);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.but_cancel);
            this.Controls.Add(this.tb_name);
            this.Controls.Add(this.but_delete);
            this.Controls.Add(this.but_preview);
            this.Controls.Add(this.but_down);
            this.Controls.Add(this.but_browse);
            this.Controls.Add(this.but_up);
            this.Controls.Add(this.but_ok);
            this.MaximumSize = new System.Drawing.Size(619, 325);
            this.MinimumSize = new System.Drawing.Size(619, 325);
            this.Name = "Edit_Quick_Tab_Form";
            this.Text = "Edit_Quick_Tab_Form";
            this.Controls.SetChildIndex(this.but_ok, 0);
            this.Controls.SetChildIndex(this.but_up, 0);
            this.Controls.SetChildIndex(this.but_browse, 0);
            this.Controls.SetChildIndex(this.but_down, 0);
            this.Controls.SetChildIndex(this.but_preview, 0);
            this.Controls.SetChildIndex(this.but_delete, 0);
            this.Controls.SetChildIndex(this.tb_name, 0);
            this.Controls.SetChildIndex(this.but_cancel, 0);
            this.Controls.SetChildIndex(this.dgv, 0);
            this.Controls.SetChildIndex(label1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        Button but_ok;
        Button but_cancel;
        TextBox tb_name;
        DataGridView dgv;
        DataGridViewTextBoxColumn col_name;
        DataGridViewTextBoxColumn col_barcode;
        DataGridViewTextBoxColumn col_img;
        private Unselectable_Button but_up;
        private Unselectable_Button but_down;
        private Unselectable_Button but_delete;
        private Unselectable_Button but_preview;
        private Unselectable_Button but_browse;
    }
}