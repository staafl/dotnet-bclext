using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;


namespace Common.Controls
{
    public partial class Numeric_Box : UserControl, ICalculatorTextBox
    {

        public decimal Value {
            get { return vb_box.Value; }
            set { vb_box.Value = value; }
        }

        public new event EventHandler TextChanged {
            add {
                vb_box.TextChanged += value;
            }
            remove {
                vb_box.TextChanged -= value;
            }
        }

        public event EventHandler ValueChanged {
            add {
                vb_box.ValueChanged += value;
            }
            remove {
                vb_box.ValueChanged -= value;
            }
        }

        public static implicit operator Validating_Box(Numeric_Box nb) {

            nb.vb_box.Name = nb.Name;

            return nb.vb_box;
        }

        // TextBox positioning
        const int c_TextLocationX = 2;
        const int c_TextLocationY = 3;

        const int c_TextBoxWidthOffset = 24;
        const int c_TextBoxHeight = 14;

        int _txt_Y;
        public int TextLowered {
            get {
                return _txt_Y;
            }
            set {
                if (value != _txt_Y) {
                    _txt_Y = value;
                    vb_box.Location = new Point(c_TextLocationX, c_TextLocationY + _txt_Y);
                }
            }
        }
        //

        public void SimulateEntered() {
            b_flag = true;

            vb_box.TextAlign = HorizontalAlignment.Left;

            b_flag = false;
        }

        public void SimulateLeft() {
            b_flag = true;

            BeginInvoke((MethodInvoker)(() =>
            {
                vb_box.TextAlign = HorizontalAlignment.Right;
            }));

            b_flag = false;
        }


        [DebuggerStepThrough]
        public void Clear() {
            vb_box.ResetText();
        }

        // Reimplementations
        [DefaultValue(true)]
        public bool AutoHighlight {
            get {
                return this.vb_box.AutoHighlight;

            }
            set {
                this.vb_box.AutoHighlight = value;
            }
        }
        public int SelectionStart {
            get { return vb_box.SelectionStart; }
            set { vb_box.SelectionStart = value; }
        }
        public int SelectionLength {
            get { return vb_box.SelectionLength; }
            set { vb_box.SelectionLength = value; }
        }
        public void SelectAll() {
            vb_box.SelectAll();
        }
        public HorizontalAlignment TextAlign {
            get { return vb_box.TextAlign; }
            set { vb_box.TextAlign = value; }
        }

        public int DecimalPlaces {
            get { return vb_box.Decimal_Places; }
            set { vb_box.Decimal_Places = value; }
        }

        public int MaxLength {
            get { return vb_box.MaxLength; }
            set { vb_box.MaxLength = value; }
        }
        public override string Text {
            [DebuggerStepThrough]
            get { return vb_box.Text.Trim(); }
            set {
                vb_box.Text = value;
            }
        }
        /// <summary>
        /// Returns whether the box contains the same
        /// text as it did when it was last selected.
        /// </summary>
        public bool Has_Text_Changed_Since_Entered {
            get { return vb_box.Has_Text_Changed_Since_Entered; }

        }

        public bool Check_Typed_Text() {
            bool ret = vb_box.Check_Typed_Text();
            return ret;

        }

        /// <summary>
        /// This property returns whether the user has typed
        /// or pasted any text using the keyboard or mouse
        /// since it was last reset.
        /// This property is only reset when calling ResetText();
        /// The caller can set it to false himself if he wants to 
        /// indicate that the change has been handled.
        /// </summary>
        public bool Has_User_Typed_Text {
            get { return vb_box.Has_User_Typed_Text; }
            set { vb_box.Has_User_Typed_Text = value; }

        }

        /// <summary>
        /// Returns whether the box contains the
        /// default text (0, 0.0, 0.00 etc)
        /// </summary>
        public bool Has_Default_Text {
            get { return vb_box.Has_Default_Text; }
        }
        //


        void textBox_GotFocus(object sender, EventArgs e) {

            b_flag = true;

            vb_box.Text = vb_box.Text.Trim();
            vb_box.SelectAll();

            b_flag = false;
        }
    }
}