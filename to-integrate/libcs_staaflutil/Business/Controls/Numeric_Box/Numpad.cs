using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    public partial class Numpad : UserControl
    {
        public Numpad() {

            this.SetStyle(ControlStyles.Selectable, false);

        }

        public Numpad(Validating_Box target)
            : this() {
            InitializeComponent();
            _target = target;

            foreach (Control ctrl in Controls) {
                ctrl.KeyPress += new KeyPressEventHandler(ctrl_KeyPress);
                foreach (Control ctrl1 in ctrl.Controls) {
                    ctrl1.KeyPress += new KeyPressEventHandler(ctrl_KeyPress);
                }
            }
        }

        public event Handler<bool> Enter_Pressed;
        public event Handler<bool, bool> Tab_Pressed;
        public event Handler<bool> Closing;

        protected override bool ProcessTabKey(bool fwd) {

            var args = Args.Make(!Do_Not_Tab, fwd);

            Tab_Pressed.Raise(this, args);

            Handle_Eq(args.Mut, fwd);

            return true;

        }

        

        [DebuggerStepThrough]
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {

            if ((msg.Msg == Native_Const.WM_KEYDOWN) ||
                (msg.Msg == Native_Const.WM_SYSKEYDOWN)) {

                if (keyData == Keys.Enter) {

                    var args = Args.Make(true);

                    Enter_Pressed.Raise(this, args);

                    Handle_Eq(args.Mut, true);

                    return true;
                }

                if (keyData == Keys.Back) {
                    but_bsp_Click(null, EventArgs.Empty);
                    return true;
                }

                if (keyData == Keys.Escape) {
                    Do_Close();
                    return true;
                }

                if (keyData == Keys.Left || keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Right) {
                    string tag = but_selected.Tag.ToString();
                    int x = int.Parse(tag.Substring(0, 1));
                    int y = int.Parse(tag.Substring(1, 1));
                    KeyboardMove(this, new KeyboardMoveEventArgs(x, y, keyData));
                    return true;
                }

                bool b1 = (keyData >= Keys.D0 && keyData <= Keys.D9);
                bool b2 = (keyData >= Keys.NumPad0 && keyData <= Keys.NumPad9);

                if (b1 || b2) {

                    string name = "but_" + (keyData - (b1 ? Keys.D0 : Keys.NumPad0)).ToString();

                    but_Click(groupBox.Controls[name], EventArgs.Empty);
                    groupBox.Controls[name].Select();
                    return true;
                }
            }

            return false;
        }

        bool b_start, b_init,
             b_load, b_clear;
        string str_old;

        double register1, register2;

        Operations operation = Operations.None;

        Button[,] keyboard;

        Button but_selected;

        /*       CalculationPerformed event - see scraps "CalculationPerformed"        */


        public delegate void KeyboardMoveEventHandler(object sender, KeyboardMoveEventArgs e);
        public event KeyboardMoveEventHandler KeyboardMove;
        public class KeyboardMoveEventArgs : EventArgs
        {
            public int x;
            public int y;
            public Keys key;
            public KeyboardMoveEventArgs(int x, int y, Keys key) {
                this.x = x;
                this.y = y;
                this.key = key;
            }
        }



        public void Initialize() {

            if (!b_init) {

                keyboard = new Button[4, 5];

                foreach (Control but in groupBox.Controls) {

                    if (but is Button) {

                        string tag = but.Tag.ToString();
                        int x = int.Parse(tag.Substring(0, 1));
                        int y = int.Parse(tag.Substring(1, 1));
                        keyboard[x, y] = (Button)but;
                        but.Enter += new EventHandler(but_Enter);
                    }
                }

                KeyboardMove += new KeyboardMoveEventHandler(Numpad_KeyboardMove);

                b_init = true;
            }
        }

        Validating_Box _target;
        public Validating_Box Target {
            get { return _target; }
            set { _target = value; }
        }
        void Type(char ch) {

            _target.Has_User_Typed_Text = true;

            if (!b_start) {

                str_old = _target.Text;
                _target.Clear();
                b_start = true;

            }

            if (_target.TextLength == _target.MaxLength)
                return;

            _target.Text = _target.Text.Trim() + ch;

            if (_target.Text.StartsWith("00"))
                _target.Text = _target.Text.Substring(1);

            _target.Has_User_Typed_Text = true;


            _target.Parent.Refresh();
        }

        void Calculate() {

            _target.Has_User_Typed_Text = true;

            switch (operation) {
                case Operations.Add: {
                        register1 += register2;
                        break;
                    }
                case Operations.Sub: {
                        register1 -= register2;
                        if (register1 < 0)
                            register1 = 0;
                        break;
                    }
                case Operations.Mul: {
                        register1 *= register2;
                        break;
                    }
                case Operations.Div: {
                        if (register2 > 0)
                            register1 /= register2;
                        else
                            register1 = 0;
                        break;
                    }
            }

            var _new = register1.ToString();



            _target.Text = _new;
            _target.Normalize_Input();
        }

        public void Appear() {

            Visible = true;
            BringToFront();

            but_5.Select();
            but_selected = but_5;

            operation = Operations.None;

            register1 =
                register2 = 0;

            b_start =
                b_load =
                b_clear = false;


        }

        public void Do_Close() {

            if (!Visible)
                return;

            Visible = false;

            b_start = false;
            _target.Normalize_Input();

        }

        void ctrl_KeyPress(object sender, KeyPressEventArgs e) {

            if (e.KeyChar == '+') {
                but_add_Click(null, EventArgs.Empty);
                but_add.Select();
                return;

            }
            if (e.KeyChar == '-') {
                but_sub_Click(null, EventArgs.Empty);
                but_sub.Select();
                return;

            }
            if (e.KeyChar == '=') {
                Handle_Eq(true, true);
                return;

            }
            if (e.KeyChar == '*') {
                but_mul_Click(null, EventArgs.Empty);
                but_mul.Select();
                return;
            }
            if (e.KeyChar == '/') {
                but_div_Click(null, EventArgs.Empty);
                but_div.Select();
                return;
            }
            if (e.KeyChar == '.') {
                but_dot_Click(null, EventArgs.Empty);
                but_dot.Select();
                return;
            }

        }

        void Numpad_KeyboardMove(object sender, Numpad.KeyboardMoveEventArgs e) {
            int end_x = e.x;
            int end_y = e.y;

            if (e.key == Keys.Left) {
                --end_x;
            }
            if (e.key == Keys.Right) {
                ++end_x;
            }
            if (e.key == Keys.Up) {
                --end_y;
            }
            if (e.key == Keys.Down) {
                ++end_y;
            }
            end_x %= keyboard.GetLength(0);
            end_y %= keyboard.GetLength(1);
            if (end_x < 0) end_x = keyboard.GetLength(0) - 1;
            if (end_y < 0) end_y = keyboard.GetLength(1) - 1;
            keyboard[end_x, end_y].Select();
        }

        void but_Enter(object sender, EventArgs e) {
            if (sender is Button)
                but_selected = (Button)sender;
        }

        void but_Click(object sender, EventArgs e) {

            if (b_clear) {
                b_clear = false;
                _target.Clear();
            }
            _target.Has_User_Typed_Text = true;

            char ch;
            ch = ((Control)sender).Name.Remove(0, 4).ToCharArray()[0];
            Type(ch);
        }
        void but_dot_Click(object sender, EventArgs e) {
            if (_target.Text.Contains('.'))
                return;
            Type('.');
        }
        void but_bsp_Click(object sender, EventArgs e) {
            if (_target.Text.Length >= 1)
                _target.Text = _target.Text.Remove(_target.Text.Length - 1);
        }
        void but_c_Click(object sender, EventArgs e) {
            _target.ResetText();
        }
        void but_ce_Click(object sender, EventArgs e) {
            _target.Clear();
        }

        void but_add_Click(object sender, EventArgs e) {
            if (b_load) {
                if (String.IsNullOrEmpty(_target.Text))
                    register2 = 0;
                else
                    register2 = double.Parse(_target.Text);

                Calculate();
                b_load = false;
            }

            operation = Operations.Add;

            if (String.IsNullOrEmpty(_target.Text))
                register1 = 0;
            else
                register1 = double.Parse(_target.Text);

            b_load = true;
            b_clear = true;
        }
        void but_sub_Click(object sender, EventArgs e) {
            if (b_load) {
                if (String.IsNullOrEmpty(_target.Text))
                    register2 = 0;
                else
                    register2 = double.Parse(_target.Text);

                Calculate();
                b_load = false;
            }

            operation = Operations.Sub;

            if (String.IsNullOrEmpty(_target.Text))
                register1 = 0;
            else
                register1 = double.Parse(_target.Text);

            b_load = true;
            b_clear = true;
        }
        void but_mul_Click(object sender, EventArgs e) {
            if (b_load) {
                if (String.IsNullOrEmpty(_target.Text))
                    register2 = 0;
                else
                    register2 = double.Parse(_target.Text);

                Calculate();
                b_load = false;
            }

            operation = Operations.Mul;

            if (String.IsNullOrEmpty(_target.Text))
                register1 = 0;
            else
                register1 = double.Parse(_target.Text);

            b_load = true;
            b_clear = true;
        }
        void but_div_Click(object sender, EventArgs e) {
            if (b_load) {
                if (String.IsNullOrEmpty(_target.Text))
                    register2 = 0;
                else
                    register2 = double.Parse(_target.Text);

                Calculate();
                b_load = false;
            }

            operation = Operations.Div;

            if (String.IsNullOrEmpty(_target.Text))
                register1 = 0;
            else
                register1 = double.Parse(_target.Text);

            b_load = true;
            b_clear = true;
        }

        void but_eq_Click(object sender, EventArgs e) {

            Handle_Eq(true, false);
        }

        public bool Do_Not_Tab {
            get;
            set;
        }

        void Handle_Eq(bool maybe_tab, bool direction) {

            if (b_load) {
                if (_target.Text.IsNullOrEmpty())
                    register2 = 0;
                else
                    register2 = double.Parse(_target.Text);

                Calculate();
            }

            var eargs = Args.Make(false);

            Closing.Raise(this, eargs);

            if (eargs.Mut) {
                Do_Close();

                return;
            }

            if (maybe_tab) {
                _target.Focus();
                if (!Do_Not_Tab)
                    _target.Select_Next_Safe(direction);

            }

            Do_Close();
        }

    }
}
