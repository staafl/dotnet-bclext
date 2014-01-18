namespace Config_Gui
{
    using Common.Controls;
      partial class Receipt_Designer
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
                this.pp_control = new System.Windows.Forms.PrintPreviewControl();
                this.but_refresh = new System.Windows.Forms.Button();
                this.cb_zoom = new System.Windows.Forms.ComboBox();
                this.label1 = new System.Windows.Forms.Label();
                this.tb_name = new System.Windows.Forms.TextBox();
                this.but_file = new System.Windows.Forms.Button();
                this.chb_use_image = new Common.Controls.Flat_Check_Box();
                this.label2 = new System.Windows.Forms.Label();
                this.lab_logo = new System.Windows.Forms.Label();
                this.label5 = new System.Windows.Forms.Label();
                this.label7 = new System.Windows.Forms.Label();
                this.but_save = new System.Windows.Forms.Button();
                this.but_cancel = new System.Windows.Forms.Button();
                this.tb_header = new System.Windows.Forms.TextBox();
                this.tb_footer = new System.Windows.Forms.TextBox();
                this.label3 = new System.Windows.Forms.Label();
                this.chb_use_category = new Common.Controls.Flat_Check_Box();
                this.label4 = new System.Windows.Forms.Label();
                this.SuspendLayout();
                // 
                // pp_control
                // 
                this.pp_control.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.pp_control.Location = new System.Drawing.Point(250, 5);
                this.pp_control.Name = "pp_control";
                this.pp_control.Size = new System.Drawing.Size(519, 576);
                this.pp_control.TabIndex = 0;
                this.pp_control.UseAntiAlias = true;
                // 
                // but_refresh
                // 
                this.but_refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
                this.but_refresh.Location = new System.Drawing.Point(6, 552);
                this.but_refresh.Name = "but_refresh";
                this.but_refresh.Size = new System.Drawing.Size(75, 23);
                this.but_refresh.TabIndex = 140;
                this.but_refresh.Text = "&Preview";
                this.but_refresh.UseVisualStyleBackColor = true;
                this.but_refresh.Click += new System.EventHandler(this.but_refresh_Click);
                // 
                // cb_zoom
                // 
                this.cb_zoom.FormattingEnabled = true;
                this.cb_zoom.Items.AddRange(new object[] {
            "10%",
            "25%",
            "50%",
            "75%",
            "100%",
            "150%",
            "250%",
            "500%"});
                this.cb_zoom.Location = new System.Drawing.Point(50, 503);
                this.cb_zoom.Name = "cb_zoom";
                this.cb_zoom.Size = new System.Drawing.Size(121, 21);
                this.cb_zoom.TabIndex = 120;
                // 
                // label1
                // 
                this.label1.AutoSize = true;
                this.label1.Location = new System.Drawing.Point(7, 506);
                this.label1.Name = "label1";
                this.label1.Size = new System.Drawing.Size(37, 13);
                this.label1.TabIndex = 4;
                this.label1.Text = "Zoom:";
                // 
                // tb_name
                // 
                this.tb_name.BackColor = System.Drawing.Color.White;
                this.tb_name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.tb_name.Location = new System.Drawing.Point(9, 149);
                this.tb_name.MaxLength = 26;
                this.tb_name.Name = "tb_name";
                this.tb_name.Size = new System.Drawing.Size(230, 20);
                this.tb_name.TabIndex = 80;
                // 
                // but_file
                // 
                this.but_file.Location = new System.Drawing.Point(9, 92);
                this.but_file.Name = "but_file";
                this.but_file.Size = new System.Drawing.Size(75, 23);
                this.but_file.TabIndex = 60;
                this.but_file.Text = "Select File...";
                this.but_file.UseVisualStyleBackColor = true;
                this.but_file.Click += new System.EventHandler(this.but_file_Click);
                // 
                // chb_use_image
                // 
                this.chb_use_image.AutoSize = true;
                this.chb_use_image.BackColor = System.Drawing.Color.White;
                this.chb_use_image.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
                this.chb_use_image.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.chb_use_image.Location = new System.Drawing.Point(79, 28);
                this.chb_use_image.Name = "chb_use_image";
                this.chb_use_image.Size = new System.Drawing.Size(12, 11);
                this.chb_use_image.TabIndex = 20;
                this.chb_use_image.TabStop = false;
                this.chb_use_image.UseVisualStyleBackColor = true;
                this.chb_use_image.CheckedChanged += new System.EventHandler(this.chb_use_image_CheckedChanged);
                // 
                // label2
                // 
                this.label2.AutoSize = true;
                this.label2.Location = new System.Drawing.Point(9, 27);
                this.label2.Name = "label2";
                this.label2.Size = new System.Drawing.Size(58, 13);
                this.label2.TabIndex = 9;
                this.label2.Text = "Print Logo:";
                // 
                // lab_logo
                // 
                this.lab_logo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.lab_logo.Location = new System.Drawing.Point(9, 55);
                this.lab_logo.Name = "lab_logo";
                this.lab_logo.Size = new System.Drawing.Size(217, 23);
                this.lab_logo.TabIndex = 40;
                // 
                // label5
                // 
                this.label5.AutoSize = true;
                this.label5.Location = new System.Drawing.Point(6, 179);
                this.label5.Name = "label5";
                this.label5.Size = new System.Drawing.Size(45, 13);
                this.label5.TabIndex = 9;
                this.label5.Text = "Header:";
                // 
                // label7
                // 
                this.label7.AutoSize = true;
                this.label7.Location = new System.Drawing.Point(6, 132);
                this.label7.Name = "label7";
                this.label7.Size = new System.Drawing.Size(85, 13);
                this.label7.TabIndex = 9;
                this.label7.Text = "Company Name:";
                // 
                // but_save
                // 
                this.but_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
                this.but_save.Location = new System.Drawing.Point(87, 552);
                this.but_save.Name = "but_save";
                this.but_save.Size = new System.Drawing.Size(75, 23);
                this.but_save.TabIndex = 160;
                this.but_save.Text = "&Save";
                this.but_save.UseVisualStyleBackColor = true;
                this.but_save.Click += new System.EventHandler(this.but_save_Click);
                // 
                // but_cancel
                // 
                this.but_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
                this.but_cancel.Location = new System.Drawing.Point(167, 552);
                this.but_cancel.Name = "but_cancel";
                this.but_cancel.Size = new System.Drawing.Size(75, 23);
                this.but_cancel.TabIndex = 180;
                this.but_cancel.Text = "Cancel";
                this.but_cancel.UseVisualStyleBackColor = true;
                this.but_cancel.Click += new System.EventHandler(this.but_cancel_Click);
                // 
                // tb_header
                // 
                this.tb_header.BackColor = System.Drawing.Color.White;
                this.tb_header.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.tb_header.Location = new System.Drawing.Point(9, 195);
                this.tb_header.Multiline = true;
                this.tb_header.Name = "tb_header";
                this.tb_header.Size = new System.Drawing.Size(230, 141);
                this.tb_header.TabIndex = 90;
                this.tb_header.WordWrap = false;
                // 
                // tb_footer
                // 
                this.tb_footer.BackColor = System.Drawing.Color.White;
                this.tb_footer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.tb_footer.Location = new System.Drawing.Point(9, 359);
                this.tb_footer.Multiline = true;
                this.tb_footer.Name = "tb_footer";
                this.tb_footer.Size = new System.Drawing.Size(230, 91);
                this.tb_footer.TabIndex = 182;
                this.tb_footer.WordWrap = false;
                // 
                // label3
                // 
                this.label3.AutoSize = true;
                this.label3.Location = new System.Drawing.Point(6, 343);
                this.label3.Name = "label3";
                this.label3.Size = new System.Drawing.Size(40, 13);
                this.label3.TabIndex = 181;
                this.label3.Text = "Footer:";
                // 
                // chb_use_category
                // 
                this.chb_use_category.AutoSize = true;
                this.chb_use_category.BackColor = System.Drawing.Color.White;
                this.chb_use_category.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
                this.chb_use_category.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.chb_use_category.Location = new System.Drawing.Point(9, 461);
                this.chb_use_category.Name = "chb_use_category";
                this.chb_use_category.Size = new System.Drawing.Size(12, 11);
                this.chb_use_category.TabIndex = 183;
                this.chb_use_category.TabStop = false;
                this.chb_use_category.UseVisualStyleBackColor = true;
                this.chb_use_category.CheckedChanged += new System.EventHandler(this.chb_use_category_CheckedChanged);
                // 
                // label4
                // 
                this.label4.AutoSize = true;
                this.label4.Location = new System.Drawing.Point(31, 460);
                this.label4.Name = "label4";
                this.label4.Size = new System.Drawing.Size(178, 13);
                this.label4.TabIndex = 0;
                this.label4.Text = "Print Category instead of Description";
                // 
                // Receipt_Designer
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(774, 585);
                this.Controls.Add(this.label4);
                this.Controls.Add(this.tb_footer);
                this.Controls.Add(this.chb_use_category);
                this.Controls.Add(this.label3);
                this.Controls.Add(this.tb_header);
                this.Controls.Add(this.but_cancel);
                this.Controls.Add(this.but_save);
                this.Controls.Add(this.lab_logo);
                this.Controls.Add(this.label7);
                this.Controls.Add(this.label5);
                this.Controls.Add(this.label2);
                this.Controls.Add(this.chb_use_image);
                this.Controls.Add(this.but_file);
                this.Controls.Add(this.tb_name);
                this.Controls.Add(this.label1);
                this.Controls.Add(this.cb_zoom);
                this.Controls.Add(this.but_refresh);
                this.Controls.Add(this.pp_control);
                this.MaximumSize = new System.Drawing.Size(782, 619);
                this.Name = "Receipt_Designer";
                this.Text = "Receipt_Designer";
                this.ResumeLayout(false);
                this.PerformLayout();

            }

            #endregion

            System.Windows.Forms.PrintPreviewControl pp_control;
            System.Windows.Forms.Button but_refresh;
            System.Windows.Forms.ComboBox cb_zoom;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.TextBox tb_name;
            System.Windows.Forms.Button but_file;
            Flat_Check_Box chb_use_image;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label lab_logo;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Button but_save;
            System.Windows.Forms.Button but_cancel;
            System.Windows.Forms.TextBox tb_header;
            System.Windows.Forms.TextBox tb_footer;
            System.Windows.Forms.Label label3;
            Flat_Check_Box chb_use_category;
            System.Windows.Forms.Label label4;
      }
}