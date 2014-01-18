
using Common.Controls;
namespace Common.Dialogs
{
    partial class Edit_Product_Form
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent() {
            this.tab_control = new Common.Controls.Advanced_Tab_Control();
            this.but_close = new System.Windows.Forms.Button();
            this.but_save = new System.Windows.Forms.Button();
            this.but_discard = new System.Windows.Forms.Button();
            this.but_delete = new System.Windows.Forms.Button();
            this.but_prev = new System.Windows.Forms.Button();
            this.but_next = new System.Windows.Forms.Button();
            this.but_keyboard = new Unselectable_Button();
            this.SuspendLayout();
            // 
            // tab_control
            // 
            this.tab_control.Active_Tab = -1;
            this.tab_control.Highlight_Color = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tab_control.Location = new System.Drawing.Point(10, 7);
            this.tab_control.Name = "tab_control";
            this.tab_control.Normal_Button_Color = System.Drawing.Color.LightGray;
            this.tab_control.Size = new System.Drawing.Size(764, 405);
            this.tab_control.TabIndex = 0;
            // 
            // but_close
            // 
            this.but_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.but_close.Location = new System.Drawing.Point(697, 425);
            this.but_close.Name = "but_close";
            this.but_close.Size = new System.Drawing.Size(75, 23);
            this.but_close.TabIndex = 60;
            this.but_close.Text = "Close";
            this.but_close.UseVisualStyleBackColor = true;
            // 
            // but_save
            // 
            this.but_save.Enabled = false;
            this.but_save.Location = new System.Drawing.Point(10, 425);
            this.but_save.Name = "but_save";
            this.but_save.Size = new System.Drawing.Size(75, 23);
            this.but_save.TabIndex = 0;
            this.but_save.Text = "&Save";
            this.but_save.UseVisualStyleBackColor = true;
            // 
            // but_discard
            // 
            this.but_discard.Location = new System.Drawing.Point(91, 425);
            this.but_discard.Name = "but_discard";
            this.but_discard.Size = new System.Drawing.Size(75, 23);
            this.but_discard.TabIndex = 2;
            this.but_discard.Text = "&Discard";
            this.but_discard.UseVisualStyleBackColor = true;
            this.but_discard.Click += new System.EventHandler(this.but_discard_Click);
            // 
            // but_delete
            // 
            this.but_delete.Enabled = false;
            this.but_delete.Location = new System.Drawing.Point(172, 425);
            this.but_delete.Name = "but_delete";
            this.but_delete.Size = new System.Drawing.Size(75, 23);
            this.but_delete.TabIndex = 4;
            this.but_delete.Text = "Dele&te";
            this.but_delete.UseVisualStyleBackColor = true;
            // 
            // but_prev
            // 
            this.but_prev.Location = new System.Drawing.Point(253, 425);
            this.but_prev.Name = "but_prev";
            this.but_prev.Size = new System.Drawing.Size(75, 23);
            this.but_prev.TabIndex = 6;
            this.but_prev.Text = "&Previous";
            this.but_prev.UseVisualStyleBackColor = true;
            // 
            // but_next
            // 
            this.but_next.Location = new System.Drawing.Point(334, 425);
            this.but_next.Name = "but_next";
            this.but_next.Size = new System.Drawing.Size(75, 23);
            this.but_next.TabIndex = 8;
            this.but_next.Text = "&Next";
            this.but_next.UseVisualStyleBackColor = true;
            // 
            // but_keyboard
            // 
            this.but_keyboard.Location = new System.Drawing.Point(415, 425);
            this.but_keyboard.Name = "but_keyboard";
            this.but_keyboard.Size = new System.Drawing.Size(85, 23);
            this.but_keyboard.TabIndex = 50;
            this.but_keyboard.Text = "Keyboard";
            this.but_keyboard.UseVisualStyleBackColor = true;
            // 
            // Edit_Product_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.but_close;
            this.ClientSize = new System.Drawing.Size(782, 455);
            this.Controls.Add(this.tab_control);
            this.Controls.Add(this.but_keyboard);
            this.Controls.Add(this.but_next);
            this.Controls.Add(this.but_prev);
            this.Controls.Add(this.but_delete);
            this.Controls.Add(this.but_discard);
            this.Controls.Add(this.but_save);
            this.Controls.Add(this.but_close);
            this.MaximumSize = new System.Drawing.Size(788, 480);
            this.MinimumSize = new System.Drawing.Size(788, 480);
            this.Name = "Edit_Product_Form";
            this.Text = "Product Record -";
            this.Controls.SetChildIndex(this.but_close, 0);
            this.Controls.SetChildIndex(this.but_save, 0);
            this.Controls.SetChildIndex(this.but_discard, 0);
            this.Controls.SetChildIndex(this.but_delete, 0);
            this.Controls.SetChildIndex(this.but_prev, 0);
            this.Controls.SetChildIndex(this.but_next, 0);
            this.Controls.SetChildIndex(this.but_keyboard, 0);
            this.Controls.SetChildIndex(this.tab_control, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        Common.Controls.Advanced_Tab_Control tab_control;
        System.Windows.Forms.Button but_close;
        System.Windows.Forms.Button but_save;
        System.Windows.Forms.Button but_discard;
        System.Windows.Forms.Button but_delete;
        System.Windows.Forms.Button but_prev;
        System.Windows.Forms.Button but_next;
        System.Windows.Forms.Button but_keyboard;
    }
}