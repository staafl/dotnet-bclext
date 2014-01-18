using System;
using System.Diagnostics;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    public partial class Calculator : UserControl
    {
        [DebuggerStepThrough]
        bool IsMyControl(Control ctrl) {

            if (ctrl == null)
                return false;

            bool ret = ctrl == this;

            while (!ret && ctrl != null) {

                ret |= this.Controls.Contains(ctrl);
                ctrl = ctrl.Parent;
            }

            return ret;
        }

        static bool IsArrowKey(Keys key) {


            return key == Keys.Up || key == Keys.Down || key == Keys.Left || key == Keys.Right;
        }

        static Pair<int> ArrowKeyValue(Keys key) {


            int horz = 0;
            int vert = 0;

            if (key == Keys.Up)
                vert = -1;
            else if (key == Keys.Down)
                vert = 1;

            if (key == Keys.Left)
                horz = -1;
            else if (key == Keys.Right)
                horz = +1;

            var ret = new Pair<int>(horz, vert);

            return ret;
        }

        [DebuggerStepThrough]
        static bool Aux_Is_Numeric_Key(Keys key, out int value) {

            value = default(int);

            bool ret = key >= Keys.D0 && key <= Keys.D9;

            if (ret) {
                value = key - Keys.D0;
                return true;
            }

            ret = key >= Keys.NumPad0 && key <= Keys.NumPad9;

            if (ret) {
                value = key - Keys.NumPad0;
                return true;
            }

            return ret;
        }

        [DebuggerStepThrough]
        static bool Aux_Is_Typeable_String(string str, out string typeable_str) {

            if (str.Length == 1 && Char.IsNumber(str[0])) {
                typeable_str = str;
                return true;
            }

            if (str == "dot") {
                typeable_str = ".";
                return true;
            }

            typeable_str = null;
            return false;
        }

        [DebuggerStepThrough]
        static string Aux_Get_Button_Value(string name) {

            var mm = rx_button_val.Match(name);

            if (!mm.Success)
                throw new InvalidOperationException();

            string ret = mm.Groups[1].Value;

            return ret;
        }

        void groupBox_MouseEnter(object sender, EventArgs e) {

            OnMouseEnter(EventArgs.Empty);
        }


        protected override void
        OnTabStopChanged(EventArgs e) {
            if (this.TabStop)
                this.TabStop = false;

        }



        #region designer
        void InitializeComponent() {
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.but_bsp = new Our_Button();
            this.but_eq = new Our_Button();
            this.but_c = new Our_Button();
            this.but_dot = new Our_Button();
            this.but_ce = new Our_Button();
            this.but_0 = new Our_Button();
            this.but_div = new Our_Button();
            this.but_cc = new Our_Button();
            this.but_7 = new Our_Button();
            this.but_add = new Our_Button();
            this.but_8 = new Our_Button();
            this.but_3 = new Our_Button();
            this.but_9 = new Our_Button();
            this.but_2 = new Our_Button();
            this.but_mul = new Our_Button();
            this.but_1 = new Our_Button();
            this.but_4 = new Our_Button();
            this.but_sub = new Our_Button();
            this.but_5 = new Our_Button();
            this.but_6 = new Our_Button();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
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
            this.groupBox.Size = new System.Drawing.Size(120, 158);
            this.groupBox.TabIndex = 21;
            this.groupBox.TabStop = false;
            // 
            // but_bsp
            // 
            this.but_bsp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_bsp.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_bsp.Location = new System.Drawing.Point(2, 7);
            this.but_bsp.Name = "but_bsp";
            this.but_bsp.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_bsp.TabIndex = 0;
            this.but_bsp.TabStop = false;
            this.but_bsp.Tag = "00";
            this.but_bsp.Text = "<-";
            this.but_bsp.UseVisualStyleBackColor = true;
            this.but_bsp.Click += new System.EventHandler(this.but_Click);
            // 
            // but_eq
            // 
            this.but_eq.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_eq.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_eq.Location = new System.Drawing.Point(89, 127);
            this.but_eq.Name = "but_eq";
            this.but_eq.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_eq.TabIndex = 19;
            this.but_eq.TabStop = false;
            this.but_eq.Tag = "34";
            this.but_eq.Text = "=";
            this.but_eq.UseVisualStyleBackColor = true;
            this.but_eq.Click += new System.EventHandler(this.but_Click);
            // 
            // but_c
            // 
            this.but_c.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_c.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_c.Location = new System.Drawing.Point(31, 7);
            this.but_c.Name = "but_c";
            this.but_c.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_c.TabIndex = 1;
            this.but_c.TabStop = false;
            this.but_c.Tag = "10";
            this.but_c.Text = "C";
            this.but_c.UseVisualStyleBackColor = true;
            this.but_c.Click += new System.EventHandler(this.but_Click);
            // 
            // but_dot
            // 
            this.but_dot.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_dot.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_dot.Location = new System.Drawing.Point(60, 127);
            this.but_dot.Name = "but_dot";
            this.but_dot.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_dot.TabIndex = 18;
            this.but_dot.TabStop = false;
            this.but_dot.Tag = "24";
            this.but_dot.Text = ".";
            this.but_dot.UseVisualStyleBackColor = true;
            this.but_dot.Click += new System.EventHandler(this.but_Click);
            // 
            // but_ce
            // 
            this.but_ce.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_ce.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_ce.Location = new System.Drawing.Point(60, 7);
            this.but_ce.Name = "but_ce";
            this.but_ce.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_ce.TabIndex = 2;
            this.but_ce.TabStop = false;
            this.but_ce.Tag = "20";
            this.but_ce.Text = "Ce";
            this.but_ce.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.but_ce.UseVisualStyleBackColor = true;
            this.but_ce.Click += new System.EventHandler(this.but_Click);
            // 
            // but_0
            // 
            this.but_0.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_0.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_0.Location = new System.Drawing.Point(31, 127);
            this.but_0.Name = "but_0";
            this.but_0.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_0.TabIndex = 17;
            this.but_0.TabStop = false;
            this.but_0.Tag = "14";
            this.but_0.Text = "0";
            this.but_0.UseVisualStyleBackColor = true;
            this.but_0.Click += new System.EventHandler(this.but_Click);
            // 
            // but_div
            // 
            this.but_div.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_div.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_div.Location = new System.Drawing.Point(89, 7);
            this.but_div.Name = "but_div";
            this.but_div.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_div.TabIndex = 3;
            this.but_div.TabStop = false;
            this.but_div.Tag = "30";
            this.but_div.Text = "/";
            this.but_div.UseVisualStyleBackColor = true;
            this.but_div.Click += new System.EventHandler(this.but_Click);
            // 
            // but_cc
            // 
            this.but_cc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_cc.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_cc.Location = new System.Drawing.Point(2, 127);
            this.but_cc.Name = "but_cc";
            this.but_cc.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_cc.TabIndex = 16;
            this.but_cc.TabStop = false;
            this.but_cc.Tag = "04";
            this.but_cc.Text = "CC";
            this.but_cc.UseVisualStyleBackColor = true;
            this.but_cc.Click += new System.EventHandler(this.but_Click);
            // 
            // but_7
            // 
            this.but_7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_7.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_7.Location = new System.Drawing.Point(2, 37);
            this.but_7.Name = "but_7";
            this.but_7.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_7.TabIndex = 4;
            this.but_7.TabStop = false;
            this.but_7.Tag = "01";
            this.but_7.Text = "7";
            this.but_7.UseVisualStyleBackColor = true;
            this.but_7.Click += new System.EventHandler(this.but_Click);
            // 
            // but_add
            // 
            this.but_add.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_add.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_add.Location = new System.Drawing.Point(89, 97);
            this.but_add.Name = "but_add";
            this.but_add.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_add.TabIndex = 15;
            this.but_add.TabStop = false;
            this.but_add.Tag = "33";
            this.but_add.Text = "+";
            this.but_add.UseVisualStyleBackColor = true;
            this.but_add.Click += new System.EventHandler(this.but_Click);
            // 
            // but_8
            // 
            this.but_8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_8.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_8.Location = new System.Drawing.Point(31, 37);
            this.but_8.Name = "but_8";
            this.but_8.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_8.TabIndex = 5;
            this.but_8.TabStop = false;
            this.but_8.Tag = "11";
            this.but_8.Text = "8";
            this.but_8.UseVisualStyleBackColor = true;
            this.but_8.Click += new System.EventHandler(this.but_Click);
            // 
            // but_3
            // 
            this.but_3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_3.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_3.Location = new System.Drawing.Point(60, 97);
            this.but_3.Name = "but_3";
            this.but_3.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_3.TabIndex = 14;
            this.but_3.TabStop = false;
            this.but_3.Tag = "23";
            this.but_3.Text = "3";
            this.but_3.UseVisualStyleBackColor = true;
            this.but_3.Click += new System.EventHandler(this.but_Click);
            // 
            // but_9
            // 
            this.but_9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_9.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_9.Location = new System.Drawing.Point(60, 37);
            this.but_9.Name = "but_9";
            this.but_9.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_9.TabIndex = 6;
            this.but_9.TabStop = false;
            this.but_9.Tag = "21";
            this.but_9.Text = "9";
            this.but_9.UseVisualStyleBackColor = true;
            this.but_9.Click += new System.EventHandler(this.but_Click);
            // 
            // but_2
            // 
            this.but_2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_2.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_2.Location = new System.Drawing.Point(31, 97);
            this.but_2.Name = "but_2";
            this.but_2.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_2.TabIndex = 13;
            this.but_2.TabStop = false;
            this.but_2.Tag = "13";
            this.but_2.Text = "2";
            this.but_2.UseVisualStyleBackColor = true;
            this.but_2.Click += new System.EventHandler(this.but_Click);
            // 
            // but_mul
            // 
            this.but_mul.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_mul.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_mul.Location = new System.Drawing.Point(89, 37);
            this.but_mul.Name = "but_mul";
            this.but_mul.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_mul.TabIndex = 7;
            this.but_mul.TabStop = false;
            this.but_mul.Tag = "31";
            this.but_mul.Text = "*";
            this.but_mul.UseVisualStyleBackColor = true;
            this.but_mul.Click += new System.EventHandler(this.but_Click);
            // 
            // but_1
            // 
            this.but_1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_1.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_1.Location = new System.Drawing.Point(2, 97);
            this.but_1.Name = "but_1";
            this.but_1.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_1.TabIndex = 12;
            this.but_1.TabStop = false;
            this.but_1.Tag = "03";
            this.but_1.Text = "1";
            this.but_1.UseVisualStyleBackColor = true;
            this.but_1.Click += new System.EventHandler(this.but_Click);
            // 
            // but_4
            // 
            this.but_4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_4.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_4.Location = new System.Drawing.Point(2, 67);
            this.but_4.Name = "but_4";
            this.but_4.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_4.TabIndex = 8;
            this.but_4.TabStop = false;
            this.but_4.Tag = "02";
            this.but_4.Text = "4";
            this.but_4.UseVisualStyleBackColor = true;
            this.but_4.Click += new System.EventHandler(this.but_Click);
            // 
            // but_sub
            // 
            this.but_sub.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_sub.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_sub.Location = new System.Drawing.Point(89, 67);
            this.but_sub.Name = "but_sub";
            this.but_sub.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_sub.TabIndex = 11;
            this.but_sub.TabStop = false;
            this.but_sub.Tag = "32";
            this.but_sub.Text = "-";
            this.but_sub.UseVisualStyleBackColor = true;
            this.but_sub.Click += new System.EventHandler(this.but_Click);
            // 
            // but_5
            // 
            this.but_5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_5.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_5.Location = new System.Drawing.Point(31, 67);
            this.but_5.Name = "but_5";
            this.but_5.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_5.TabIndex = 9;
            this.but_5.TabStop = false;
            this.but_5.Tag = "12";
            this.but_5.Text = "5";
            this.but_5.UseVisualStyleBackColor = true;
            this.but_5.Click += new System.EventHandler(this.but_Click);
            // 
            // but_6
            // 
            this.but_6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.but_6.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.but_6.Location = new System.Drawing.Point(60, 67);
            this.but_6.Name = "but_6";
            this.but_6.Size = new System.Drawing.Size(cst_but_width, cst_but_height);
            this.but_6.TabIndex = 10;
            this.but_6.TabStop = false;
            this.but_6.Tag = "22";
            this.but_6.Text = "6";
            this.but_6.UseVisualStyleBackColor = true;
            this.but_6.Click += new System.EventHandler(this.but_Click);
            // 
            // Calculator
            // 
            this.Controls.Add(this.groupBox);
            this.Name = "Calculator";
            this.Size = new System.Drawing.Size(120, 152);
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region controls
        protected Our_Button but_bsp;
        protected Our_Button but_eq;
        protected Our_Button but_c;
        protected Our_Button but_dot;
        protected Our_Button but_ce;
        protected Our_Button but_0;
        protected Our_Button but_div;
        protected Our_Button but_cc;
        protected Our_Button but_7;
        protected Our_Button but_add;
        protected Our_Button but_8;
        protected Our_Button but_3;
        protected Our_Button but_9;
        protected Our_Button but_2;
        protected Our_Button but_mul;
        protected Our_Button but_1;
        protected Our_Button but_4;
        protected Our_Button but_sub;
        protected Our_Button but_5;
        protected Our_Button but_6;
        GroupBox groupBox;
        #endregion

    }
}