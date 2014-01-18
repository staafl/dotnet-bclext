namespace Common.Controls
{
    partial class Numpad
    {
        /// <summary>  Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

        /// <summary>  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>  Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
            this.but_bsp = new System.Windows.Forms.Button();
            this.but_c = new System.Windows.Forms.Button();
            this.but_ce = new System.Windows.Forms.Button();
            this.but_div = new System.Windows.Forms.Button();
            this.but_mul = new System.Windows.Forms.Button();
            this.but_9 = new System.Windows.Forms.Button();
            this.but_8 = new System.Windows.Forms.Button();
            this.but_7 = new System.Windows.Forms.Button();
            this.but_sub = new System.Windows.Forms.Button();
            this.but_6 = new System.Windows.Forms.Button();
            this.but_5 = new System.Windows.Forms.Button();
            this.but_4 = new System.Windows.Forms.Button();
            this.but_add = new System.Windows.Forms.Button();
            this.but_3 = new System.Windows.Forms.Button();
            this.but_2 = new System.Windows.Forms.Button();
            this.but_1 = new System.Windows.Forms.Button();
            this.but_eq = new System.Windows.Forms.Button();
            this.but_dot = new System.Windows.Forms.Button();
            this.but_0 = new System.Windows.Forms.Button();
            this.but_cc = new System.Windows.Forms.Button();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // but_bsp
            // 
            this.but_bsp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_bsp.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_bsp.Location = new System.Drawing.Point(4, 9);
            this.but_bsp.Name = "but_bsp";
            this.but_bsp.Size = new System.Drawing.Size(25, 24);
            this.but_bsp.TabIndex = 0;
            this.but_bsp.TabStop = false;
            this.but_bsp.Tag = "00";
            this.but_bsp.Text = "<-";
            this.but_bsp.UseVisualStyleBackColor = true;
            this.but_bsp.Click += new System.EventHandler(this.but_bsp_Click);
            // 
            // but_c
            // 
            this.but_c.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_c.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_c.Location = new System.Drawing.Point(30, 9);
            this.but_c.Name = "but_c";
            this.but_c.Size = new System.Drawing.Size(25, 24);
            this.but_c.TabIndex = 1;
            this.but_c.TabStop = false;
            this.but_c.Tag = "10";
            this.but_c.Text = "C";
            this.but_c.UseVisualStyleBackColor = true;
            this.but_c.Click += new System.EventHandler(this.but_c_Click);
            // 
            // but_ce
            // 
            this.but_ce.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_ce.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_ce.Location = new System.Drawing.Point(56, 9);
            this.but_ce.Name = "but_ce";
            this.but_ce.Size = new System.Drawing.Size(25, 24);
            this.but_ce.TabIndex = 2;
            this.but_ce.TabStop = false;
            this.but_ce.Tag = "20";
            this.but_ce.Text = "Ce";
            this.but_ce.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.but_ce.UseVisualStyleBackColor = true;
            this.but_ce.Click += new System.EventHandler(this.but_ce_Click);
            // 
            // but_div
            // 
            this.but_div.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_div.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_div.Location = new System.Drawing.Point(82, 9);
            this.but_div.Name = "but_div";
            this.but_div.Size = new System.Drawing.Size(25, 24);
            this.but_div.TabIndex = 3;
            this.but_div.TabStop = false;
            this.but_div.Tag = "30";
            this.but_div.Text = "/";
            this.but_div.UseVisualStyleBackColor = true;
            this.but_div.Click += new System.EventHandler(this.but_div_Click);
            // 
            // but_mul
            // 
            this.but_mul.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_mul.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_mul.Location = new System.Drawing.Point(82, 34);
            this.but_mul.Name = "but_mul";
            this.but_mul.Size = new System.Drawing.Size(25, 24);
            this.but_mul.TabIndex = 7;
            this.but_mul.TabStop = false;
            this.but_mul.Tag = "31";
            this.but_mul.Text = "*";
            this.but_mul.UseVisualStyleBackColor = true;
            this.but_mul.Click += new System.EventHandler(this.but_mul_Click);
            // 
            // but_9
            // 
            this.but_9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_9.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_9.Location = new System.Drawing.Point(56, 34);
            this.but_9.Name = "but_9";
            this.but_9.Size = new System.Drawing.Size(25, 24);
            this.but_9.TabIndex = 6;
            this.but_9.TabStop = false;
            this.but_9.Tag = "21";
            this.but_9.Text = "9";
            this.but_9.UseVisualStyleBackColor = true;
            this.but_9.Click += new System.EventHandler(this.but_Click);
            // 
            // but_8
            // 
            this.but_8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_8.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_8.Location = new System.Drawing.Point(30, 34);
            this.but_8.Name = "but_8";
            this.but_8.Size = new System.Drawing.Size(25, 24);
            this.but_8.TabIndex = 5;
            this.but_8.TabStop = false;
            this.but_8.Tag = "11";
            this.but_8.Text = "8";
            this.but_8.UseVisualStyleBackColor = true;
            this.but_8.Click += new System.EventHandler(this.but_Click);
            // 
            // but_7
            // 
            this.but_7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_7.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_7.Location = new System.Drawing.Point(4, 34);
            this.but_7.Name = "but_7";
            this.but_7.Size = new System.Drawing.Size(25, 24);
            this.but_7.TabIndex = 4;
            this.but_7.TabStop = false;
            this.but_7.Tag = "01";
            this.but_7.Text = "7";
            this.but_7.UseVisualStyleBackColor = true;
            this.but_7.Click += new System.EventHandler(this.but_Click);
            // 
            // but_sub
            // 
            this.but_sub.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_sub.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_sub.Location = new System.Drawing.Point(82, 59);
            this.but_sub.Name = "but_sub";
            this.but_sub.Size = new System.Drawing.Size(25, 24);
            this.but_sub.TabIndex = 11;
            this.but_sub.TabStop = false;
            this.but_sub.Tag = "32";
            this.but_sub.Text = "-";
            this.but_sub.UseVisualStyleBackColor = true;
            this.but_sub.Click += new System.EventHandler(this.but_sub_Click);
            // 
            // but_6
            // 
            this.but_6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_6.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_6.Location = new System.Drawing.Point(56, 59);
            this.but_6.Name = "but_6";
            this.but_6.Size = new System.Drawing.Size(25, 24);
            this.but_6.TabIndex = 10;
            this.but_6.TabStop = false;
            this.but_6.Tag = "22";
            this.but_6.Text = "6";
            this.but_6.UseVisualStyleBackColor = true;
            this.but_6.Click += new System.EventHandler(this.but_Click);
            // 
            // but_5
            // 
            this.but_5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_5.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_5.Location = new System.Drawing.Point(30, 59);
            this.but_5.Name = "but_5";
            this.but_5.Size = new System.Drawing.Size(25, 24);
            this.but_5.TabIndex = 9;
            this.but_5.TabStop = false;
            this.but_5.Tag = "12";
            this.but_5.Text = "5";
            this.but_5.UseVisualStyleBackColor = true;
            this.but_5.Click += new System.EventHandler(this.but_Click);
            // 
            // but_4
            // 
            this.but_4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_4.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_4.Location = new System.Drawing.Point(4, 59);
            this.but_4.Name = "but_4";
            this.but_4.Size = new System.Drawing.Size(25, 24);
            this.but_4.TabIndex = 8;
            this.but_4.TabStop = false;
            this.but_4.Tag = "02";
            this.but_4.Text = "4";
            this.but_4.UseVisualStyleBackColor = true;
            this.but_4.Click += new System.EventHandler(this.but_Click);
            // 
            // but_add
            // 
            this.but_add.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_add.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_add.Location = new System.Drawing.Point(82, 84);
            this.but_add.Name = "but_add";
            this.but_add.Size = new System.Drawing.Size(25, 24);
            this.but_add.TabIndex = 15;
            this.but_add.TabStop = false;
            this.but_add.Tag = "33";
            this.but_add.Text = "+";
            this.but_add.UseVisualStyleBackColor = true;
            this.but_add.Click += new System.EventHandler(this.but_add_Click);
            // 
            // but_3
            // 
            this.but_3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_3.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_3.Location = new System.Drawing.Point(56, 84);
            this.but_3.Name = "but_3";
            this.but_3.Size = new System.Drawing.Size(25, 24);
            this.but_3.TabIndex = 14;
            this.but_3.TabStop = false;
            this.but_3.Tag = "23";
            this.but_3.Text = "3";
            this.but_3.UseVisualStyleBackColor = true;
            this.but_3.Click += new System.EventHandler(this.but_Click);
            // 
            // but_2
            // 
            this.but_2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_2.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_2.Location = new System.Drawing.Point(30, 84);
            this.but_2.Name = "but_2";
            this.but_2.Size = new System.Drawing.Size(25, 24);
            this.but_2.TabIndex = 13;
            this.but_2.TabStop = false;
            this.but_2.Tag = "13";
            this.but_2.Text = "2";
            this.but_2.UseVisualStyleBackColor = true;
            this.but_2.Click += new System.EventHandler(this.but_Click);
            // 
            // but_1
            // 
            this.but_1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_1.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_1.Location = new System.Drawing.Point(4, 84);
            this.but_1.Name = "but_1";
            this.but_1.Size = new System.Drawing.Size(25, 24);
            this.but_1.TabIndex = 12;
            this.but_1.TabStop = false;
            this.but_1.Tag = "03";
            this.but_1.Text = "1";
            this.but_1.UseVisualStyleBackColor = true;
            this.but_1.Click += new System.EventHandler(this.but_Click);
            // 
            // but_eq
            // 
            this.but_eq.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_eq.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_eq.Location = new System.Drawing.Point(82, 109);
            this.but_eq.Name = "but_eq";
            this.but_eq.Size = new System.Drawing.Size(25, 24);
            this.but_eq.TabIndex = 19;
            this.but_eq.TabStop = false;
            this.but_eq.Tag = "34";
            this.but_eq.Text = "=";
            this.but_eq.UseVisualStyleBackColor = true;
            this.but_eq.Click += new System.EventHandler(this.but_eq_Click);
            // 
            // but_dot
            // 
            this.but_dot.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_dot.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_dot.Location = new System.Drawing.Point(56, 109);
            this.but_dot.Name = "but_dot";
            this.but_dot.Size = new System.Drawing.Size(25, 24);
            this.but_dot.TabIndex = 18;
            this.but_dot.TabStop = false;
            this.but_dot.Tag = "24";
            this.but_dot.Text = ".";
            this.but_dot.UseVisualStyleBackColor = true;
            this.but_dot.Click += new System.EventHandler(this.but_dot_Click);
            // 
            // but_0
            // 
            this.but_0.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_0.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_0.Location = new System.Drawing.Point(30, 109);
            this.but_0.Name = "but_0";
            this.but_0.Size = new System.Drawing.Size(25, 24);
            this.but_0.TabIndex = 17;
            this.but_0.TabStop = false;
            this.but_0.Tag = "14";
            this.but_0.Text = "0";
            this.but_0.UseVisualStyleBackColor = true;
            this.but_0.Click += new System.EventHandler(this.but_Click);
            // 
            // but_cc
            // 
            this.but_cc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_cc.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_cc.Location = new System.Drawing.Point(4, 109);
            this.but_cc.Name = "but_cc";
            this.but_cc.Size = new System.Drawing.Size(25, 24);
            this.but_cc.TabIndex = 16;
            this.but_cc.TabStop = false;
            this.but_cc.Tag = "04";
            this.but_cc.Text = "CC";
            this.but_cc.UseVisualStyleBackColor = true;
            // 
            // groupBox
            // 
            this.groupBox.BackColor = System.Drawing.Color.Silver;
            this.groupBox.Controls.Add(this.but_bsp);
            this.groupBox.Controls.Add(this.but_eq);
            this.groupBox.Controls.Add(this.but_c);
            this.groupBox.Controls.Add(this.but_dot);
            this.groupBox.Controls.Add(this.but_ce);
            this.groupBox.Controls.Add(this.but_0);
            this.groupBox.Controls.Add(this.but_div);
            this.groupBox.Controls.Add(this.but_cc);
            this.groupBox.Controls.Add(this.but_7);
            this.groupBox.Controls.Add(this.but_add);
            this.groupBox.Controls.Add(this.but_8);
            this.groupBox.Controls.Add(this.but_3);
            this.groupBox.Controls.Add(this.but_9);
            this.groupBox.Controls.Add(this.but_2);
            this.groupBox.Controls.Add(this.but_mul);
            this.groupBox.Controls.Add(this.but_1);
            this.groupBox.Controls.Add(this.but_4);
            this.groupBox.Controls.Add(this.but_sub);
            this.groupBox.Controls.Add(this.but_5);
            this.groupBox.Controls.Add(this.but_6);
            this.groupBox.Location = new System.Drawing.Point(0, -5);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(112, 138);
            this.groupBox.TabIndex = 20;
            this.groupBox.TabStop = false;
            // 
            // Numpad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "Numpad";
            this.Size = new System.Drawing.Size(112, 133);
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        System.Windows.Forms.Button but_bsp;
        System.Windows.Forms.Button but_c;
        System.Windows.Forms.Button but_ce;
        System.Windows.Forms.Button but_div;
        System.Windows.Forms.Button but_mul;
        System.Windows.Forms.Button but_9;
        System.Windows.Forms.Button but_8;
        System.Windows.Forms.Button but_7;
        System.Windows.Forms.Button but_sub;
        System.Windows.Forms.Button but_6;
        System.Windows.Forms.Button but_5;
        System.Windows.Forms.Button but_4;
        System.Windows.Forms.Button but_add;
        System.Windows.Forms.Button but_3;
        System.Windows.Forms.Button but_2;
        System.Windows.Forms.Button but_1;
        System.Windows.Forms.Button but_eq;
        System.Windows.Forms.Button but_dot;
        System.Windows.Forms.Button but_0;
        System.Windows.Forms.Button but_cc;
        System.Windows.Forms.GroupBox groupBox;
    }
}
