
using System;
using System.Windows.Forms;

using Fairweather.Service;
using Standardization;

namespace Common.Controls
{
#if NO_CALCULATOR
    public class Our_TextBox : TextBox
#else
    public class Our_Text_Box : TextBox, ICalculatorTextBox
#endif
    {

        public Our_Text_Box() {

            (this).AutoSize = false;
            this.BorderStyle = BorderStyle.FixedSingle;

        }

        const char space = ' ';

        public override string Text {
            get {
                var value = base.Text;

                if (value != null && Right_Padding != 0) {
                    int desired = value.Length - Right_Padding;
                    if (desired > 0)
                        value = value.Clip_Right(desired);
                }
                return value;
            }
            set {
                if (value != null && Right_Padding != 0)
                    value += space.Repeat(Right_Padding);

                base.Text = value;
            }
        }

        bool m_readonly = false;

        public bool Is_Readonly {
            get {
                var ret = m_readonly;
                return ret;
            }
            set {

                base.ReadOnly = value;

                if (value) {

                    this.BackColor = Colors.TextBoxes.ReadOnlyBackGround;

                }
                else {

                    this.BackColor = Colors.TextBoxes.NormalBackGround;

                }
            }
        }

        public new bool Enabled {
            set {
                Is_Readonly = !value;
            }
            get {
                return !Is_Readonly;
            }
        }

        public new bool ReadOnly {
            set {
                Is_Readonly = value;
            }
            get {
                return Is_Readonly;
            }
        }

        public int Right_Padding {
            get;
            set;
        }

        public bool TabOnEnter {
            get;
            set;
        }

        protected override void
        OnKeyDown(KeyEventArgs e) {

            if (e.KeyCode == Keys.Enter) {
                if (this.TabOnEnter) {
                    this.Select_Next_Safe(true);
                    return;
                }
            }

            base.OnKeyDown(e);

        }

        public bool Auto_Highlight { get; set; }

        protected override void OnGotFocus(EventArgs e) {
            if (Auto_Highlight)
                this.SelectAll();

            base.OnGotFocus(e);
        }
        protected override void OnEnter(EventArgs e) {

            if (Auto_Highlight)
                this.SelectAll();

            base.OnEnter(e);
        }


#if !NO_CALCULATOR

        void ICalculatorTextBox.AcceptChar(char ch) {

            Our_StaticTextBoxBase.Accept_Char(this, ch);
        }

        Control ICalculatorTextBox.Control {
            get {
                return this;
            }
        }
        void ICalculatorTextBox.NotifyChanged() { }
        bool ICalculatorTextBox.IsCalculable { get { return false; } }
#endif
    }
}