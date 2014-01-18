using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Fairweather.Service;

namespace Common.Controls
{
    public partial class Validating_Box
#if NO_CALCULATOR
    : Our_TextBox
#else
 : Our_Text_Box, ICalculatorTextBox, IValue<decimal>
#endif
, IAmountControl
    {

        public Validating_Box() {

            InitializeComponent();
            this.Force_Handle();
            ctrler = new Amount_Control_Controller(this);

        }

        public Validating_Box(IContainer container)
            : this() {

            container.Add(this);

        }

#if !NO_CALCULATOR

        //<--- ITextBox implenentation

        void ICalculatorTextBox.NotifyChanged() {

            this.Has_User_Typed_Text = true;

            if (!this.Focused) {

                this.OnLeave(EventArgs.Empty);
                this.OnValidating(new CancelEventArgs());

            }

        }
        bool ICalculatorTextBox.IsCalculable {
            get { return true; }
        }

        // Some members are implemented by parents

#endif

        [DebuggerStepThrough]
        protected override void
        OnLeave(EventArgs e) {

            if (this.Read_Only_Mode)
                return;

            if (b_leave)
                return;

            b_leave = true;

            try {

                this.Force_Handle();

                if (ChangeAlignmentOnEnter) {
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        if (!IsDisposed)
                            TextAlign = HorizontalAlignment.Right;
                    }));

                }

                Normalize_Input();


            }
            finally {
                b_leave = false;
            }


            base.OnLeave(e);


        }




        protected override void
        OnEnter(EventArgs e) {

            this.Force_Handle();

            if (this.Read_Only_Mode)
                return;

            if (ChangeAlignmentOnEnter) {

                BeginInvoke((MethodInvoker)(() =>
                {
                    if (!IsDisposed)
                        TextAlign = HorizontalAlignment.Left;
                }));

            }

            if (!Read_Only_Mode && AutoHighlight) {
                BeginInvoke((MethodInvoker)(() =>
                {
                    Focus();
                    SelectAll();
                }));
            }


            Text = Text.Trim();

            last_valid_text = Text;
            text_on_enter = Text;

            base.OnEnter(e);

        }




        [DebuggerStepThrough]
        protected override void
        WndProc(ref Message m) {

            bool editing = (m.Msg == Native_Const.WM_CHAR ||
                            m.Msg == Native_Const.WM_PASTE);
            if (editing)
                b_user_is_editing = true;

            try {
                base.WndProc(ref m);
            }
            finally {
                if (editing)
                    b_user_is_editing = false;
            }

            if (b_wnd_proc)
                return;

            b_wnd_proc = true;
            try {


                if (Read_Only_Mode) {

                    Native_Methods.HideCaret(this.Handle);

                    if (base.SelectionLength != 0) {
                        base.SelectionLength = 0;
                        base.SelectedText = "";
                    }

                }

            }
            finally {
                b_wnd_proc = false;
            }

        }


        // ****************************

        // C:\Users\Fairweather\Desktop\ctrler.pl
        readonly Amount_Control_Controller ctrler; // ctrler = new Amount_Control_Controller(this);
        public Control Control { get { return this; } }
        public int Decimal_Places { get { return ctrler.DecimalPlaces; } set { ctrler.DecimalPlaces = value; } }
        public string Default_Text { get { return ctrler.Default_Text; } }
        public string Default_Format { get { return ctrler.Default_Format; } set { ctrler.Default_Format = value; } }

        /// <summary>
        /// The Value is Real Time - what the user sees in the box, the user gets as Value!
        /// (compare this to other controls such as Advanced_Combo or Date_Time_Picker)
        /// Changing this will be a disaster!
        /// </summary>
        public decimal Value {
            get { return ctrler.GetValue(); }
            set {
                if (ctrler.SetValue(value))
                    ValueChanged.Raise(this, EventArgs.Empty);
            }
        }

    }
}