using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;
using Standardization;

namespace Common.Controls
{
    public partial class Our_Date_Time : UserControl, ICalculatorTextBox
    {


        public Our_Date_Time() {

            this.SuspendLayout();

            Prepare_Button();

            Prepare_Calendar();

            Prepare_TextBox();

            Size = new Size(100, 20);

            ResumeLayout(false);

            PerformLayout();

            Set_Event_Handlers();


        }



        Our_Button but;
        Our_Calendar cal;
        MaskedTextBox mtbx;

        #region flags - 10 Lines
#pragma warning disable

        bool b_flag;
        bool b_cal_init;

        bool bf_mtb_kd;
        bool bf_mtb_kp;
        bool bf_mtb_val;

        bool bf_dte_vc;
        bool bf_dte_vr;
        bool bf_dte_mcos;

        bool bf_but_click;

        bool bf_mcal_dsel;

#pragma warnings restore
        #endregion


        void Prepare_Button() {

            but = new Our_Button();
            Aux_Set_Button_Icon();

            but.Cursor = Cursors.Default;
            but.FlatStyle = FlatStyle.Flat;
            but.Location = new Point(82, 0);
            but.Name = "button";
            but.Size = new Size(18, 20);
            but.TabIndex = 2;
            but.TabStop = false;
            but.UseVisualStyleBackColor = true;//false;

            Controls.Add(but);

        }

        void Aux_Set_Button_Icon() {

            var bmp = global::Fairweather.Service.Properties.Resources.img_calendar;
            bmp.MakeTransparent(bmp.GetPixel(1, 1));

            but.BackgroundImage = bmp;
            but.BackgroundImageLayout = ImageLayout.Stretch;
            but.BackColor = Colors.NumericBox.ButtonBackColor;

        }

        void Prepare_Calendar() {

            cal = new Our_Calendar();

            cal.Location = new Point(0, 0);
            cal.Name = "calendar";
            cal.Size = new Size(280, 260);
            cal.TabIndex = 3;
            cal.TabStop = false;
            cal.Visible = false;


            Controls.Add(cal);
        }

        void Prepare_TextBox() {

            mtbx = new MaskedTextBox();

            mtbx.BorderStyle = BorderStyle.FixedSingle;
            mtbx.Dock = DockStyle.Fill; // 22nd June
            mtbx.Location = new Point(0, 0);
            mtbx.Mask = "00/00/0000";
            mtbx.Name = "maskedTextBox";
            mtbx.PromptChar = ' ';
            mtbx.Size = new Size(83, 20);
            mtbx.TabIndex = 0;
            mtbx.TabStop = false;
            mtbx.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;

            Controls.Add(mtbx);

        }

        void Set_Event_Handlers() {

            mtbx.KeyDown += maskedTextBox_KeyDown;
            mtbx.KeyDown += (sender, e) => OnKeyDown(e);
            mtbx.KeyPress += maskedTextBox_KeyPress;

            but.MouseClick += button_Click;
            but.Leave += (sender, e) => { if (!Focused) OnLeave(e); };

            cal.Leave += (sender, e) => { if (!Focused) OnLeave(e); };
            cal.Esc_Pressed += (_1, _2) => Handle_Calendar_Date_Rejected();
            cal.Enter_Pressed += (_1, _2) => Handle_Calendar_Date_Confirmed();
            cal.Date_DoubleClicked += (_, __) => Handle_Calendar_Date_Confirmed();
            cal.Date_Changed += (_1, _2) => { Display_Value(cal.Value); };
        }

        void HandleMouseClickedOnScreen(object sender, EventArgs e) {

            if (cal.Visible) {

                var pt1 = cal.Get_Cursor_Location();

                if (!cal.ClientRectangle
                             .Contains(pt1)) {

                    bf_but_click = true;

                    HideCalendar();

                    this.Begin_Invoke(() => mtbx.Focus());
                }
            }
            else {

                bf_but_click = false;

            }
        }

        Color before;
        protected override void OnEnabledChanged(EventArgs e) {

            // if(b_EnabledChanged)
            //   return;
            if (!this.Enabled) {

                // IDEA: Follow suit with other controls
                var bmp = global::Fairweather.Service.Properties.Resources.img_calendar_disabled1;
                bmp.MakeTransparent(bmp.GetPixel(1, 1));

                but.BackgroundImage = bmp;

                but.BackgroundImageLayout = ImageLayout.Stretch;
                but.BackColor = Colors.Date_Time_Control.ButtonBackColor;
                but.BackColor = Colors.Date_Time_Control.ButtonDisabledBackColor;
                before = mtbx.BackColor;
                this.BackColor = Colors.TextBoxes.ReadOnlyBackGround;
                mtbx.BackColor = Colors.TextBoxes.ReadOnlyBackGround;
                mtbx.ForeColor = Color.Black;

            }
            else {
                this.BackColor = before;
                mtbx.BackColor = before;

                Aux_Set_Button_Icon();
            }

            base.OnEnabledChanged(e);

        }


        public MaskedTextBox MaskedTextBox {
            get { return mtbx; }
        }


        // ****************************

        public bool IsCalculable { get { return false; } }

        public void AcceptChar(char ch) {

            Our_StaticTextBoxBase.Accept_Char(this, ch);

            if (SelectionLength == 0) {
                if (SelectionStart == 2 || SelectionStart == 5)
                    SelectionStart += 1;
            }

        }


        public Control Control {
            get { return MaskedTextBox; }
        }

        public void NotifyChanged() { }

        // ****************************


        public int SelectionStart {
            get { return MaskedTextBox.SelectionStart; }
            set { MaskedTextBox.SelectionStart = value; }

        }
        public int SelectionLength {
            get { return MaskedTextBox.SelectionLength; }
            set { MaskedTextBox.SelectionLength = value; }

        }
        public int MaxLength {
            get { return MaskedTextBox.MaxLength; }
            set { MaskedTextBox.MaxLength = value; }

        }
        public override string Text {
            get {
                return MaskedTextBox.Text;
            }
            set {
                MaskedTextBox.Text = value;
            }
        }
        public int TextLength {
            get { return MaskedTextBox.TextLength; }

        }


        // ****************************

        IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
