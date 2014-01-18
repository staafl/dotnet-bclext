using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Fairweather.Service;
using Standardization;

namespace Common.Controls
{
    public partial class Numeric_Box : UserControl, ICalculatorTextBox, IValue<decimal>
    {
        //Construction
        public Numeric_Box() {

            InitializeComponent();

            SetControlSettings();
            SetEventHandlers();
            vb_box.ChangeAlignmentOnEnter = true;

            this.Force_Handle();

        }

        public bool Do_Not_Tab {
            get {
                return Numpad.Do_Not_Tab;
            }
            set {
                Numpad.Do_Not_Tab = value;
            }
        }

        public bool Enter_Not_Tab { get; set; }

        //<-- ITextBox Implementation


        void ICalculatorTextBox.AcceptChar(char ch) {

            Our_StaticTextBoxBase.Accept_Char(this.vb_box, ch);
        }

        void ICalculatorTextBox.NotifyChanged() {

            (this.vb_box as ICalculatorTextBox).NotifyChanged();

        }


        bool ICalculatorTextBox.IsCalculable {
            get { return true; }
        }

        Control ICalculatorTextBox.Control { get { return this.vb_box; /* 12th July */ } }
        //--!>

        #region flags - 1 Line
#pragma warning disable
        bool b_flag;
#pragma warning restore
        #endregion


        void SetControlSettings() {

            // this
            this.BackColor = vb_box.BackColor;
            vb_box.BackColorChanged += (sender, e) => this.BackColor = vb_box.BackColor;

            // Button
            but_calc_button.Cursor = Cursors.Default;

            var bmp = global::Fairweather.Service.Properties.Resources.img_calculator;
            bmp.MakeTransparent(bmp.GetPixel(1, 1));

            but_calc_button.BackgroundImage = bmp;
            but_calc_button.BackgroundImageLayout = ImageLayout.Stretch;
            but_calc_button.BackColor = Colors.NumericBox.ButtonBackColor;

            but_calc_button.Anchor = AnchorStyles.Right;

            ShowButtonBorder = true;

            //Numpad
            Numpad = new Numpad(this.vb_box);
            Numpad.Closing += (sender, e) => this.On_Numpad_Closing(e);
            Numpad.Size = new System.Drawing.Size(112, 133);
            Numpad.TabIndex = 32;
            Numpad.Visible = false;

            //TextBox
            vb_box.Location = new Point(2, 3);
            vb_box.MinimumSize = new Size(10, 10);
            vb_box.Size = new Size(Width - c_TextBoxWidthOffset, c_TextBoxHeight);

            vb_box.Name = Name + "_textBox";

            // These properties used to be set on the
            // numeric box itself when it still :'ed TextBox
            vb_box.Multiline = true;
            vb_box.TextAlign = HorizontalAlignment.Right;

        }

        void SetEventHandlers() {

            vb_box.MouseWheel += ((object sender, MouseEventArgs e) => OnMouseWheel(e));
            vb_box.Validating += ((Object sender, CancelEventArgs e) => OnValidating(e));

            //???
            GotFocus += new EventHandler(textBox_GotFocus);

            Numpad.VisibleChanged += new EventHandler(numpad_VisibleChanged);
        }
        //

        //[DebuggerStepThrough]
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (keyData == Keys.F4) {
                button1_Click(null, EventArgs.Empty);
                return true;
            }
            else if (keyData == Keys.Enter && !Enter_Not_Tab) {
                SendKeys.SendWait("{Tab}");
                return true; // ProcessCmdKey(ref msg, Keys.Tab);
            }
            else {
                return base.ProcessCmdKey(ref msg, keyData);

            }
        }

        Control _parent;
        public Control ParentControl {
            get {
                return _parent;
            }
            set {
                _parent = value;

                if (_parent != null)
                    _parent.Controls.Add(Numpad);
            }
        }

        public Numpad Numpad {
            get;
            set;
        }


        protected override void OnResize(EventArgs e) {
            vb_box.Size = new Size(Width - c_TextBoxWidthOffset, c_TextBoxHeight);
            but_calc_button.Location = new Point(Width - but_calc_button.Width - 1, -1);
            base.OnResize(e);
        }

        protected override void OnGotFocus(EventArgs e) {
            b_flag = true;

            try {
                if (!vb_box.Focused) {
                    vb_box.Focus();
                    vb_box.SelectAll();

                    base.OnGotFocus(e);
                }
            }
            finally {
                b_flag = false;
            }
        }

        protected override void OnLeave(EventArgs e) {

            if (!Numpad.Visible) {
                base.OnLeave(e);
            }
        }

        protected override void OnEnabledChanged(EventArgs e) {



            foreach (Control ctrl in this.Controls) {

                ctrl.Visible = this.Enabled;

            }


            base.OnEnabledChanged(e);

        }


        System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.but_calc_button = new Our_Button();
            this.vb_box = new Validating_Box(this.components);
            this.SuspendLayout();
            // 
            // button1
            // 
            this.but_calc_button.BackColor = System.Drawing.Color.Azure;
            this.but_calc_button.Cursor = System.Windows.Forms.Cursors.Default;
            this.but_calc_button.FlatAppearance.BorderSize = 0;
            this.but_calc_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.but_calc_button.Location = new System.Drawing.Point(81, -1);
            this.but_calc_button.Name = "button1";
            this.but_calc_button.Size = new System.Drawing.Size(18, 20);
            this.but_calc_button.TabIndex = 1;
            this.but_calc_button.TabStop = false;
            this.but_calc_button.UseVisualStyleBackColor = false;
            this.but_calc_button.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox
            // 
            this.vb_box.AutoHighlight = true;
            this.vb_box.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.vb_box.ChangeAlignmentOnEnter = false;
            this.vb_box.Decimal_Places = 2;
            this.vb_box.Read_Only_Mode = false;
            this.vb_box.Location = new System.Drawing.Point(0, 0);
            this.vb_box.MaxLength = 11;
            this.vb_box.Multiline = true;
            this.vb_box.Name = "textBox";
            this.vb_box.Size = new System.Drawing.Size(83, 20);
            this.vb_box.TabIndex = 0;
            this.vb_box.Text = "0.00";
            this.vb_box.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // NumericBox
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.but_calc_button);
            this.Controls.Add(this.vb_box);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(20, 20);
            this.Size = new System.Drawing.Size(111, 20);
            this.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        Validating_Box vb_box;
        Our_Button but_calc_button;
    }

}

