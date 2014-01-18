using System;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;


using EventHandler = System.EventHandler<System.EventArgs>;
namespace Common.Controls
{
    public partial class Numeric_Box : UserControl, ICalculatorTextBox
    {
        public event Handler<bool> NumpadClosing;
        public event EventHandler NumpadClosed;

        bool show_button_border;
        public bool ShowButtonBorder {
            get {
                return show_button_border;
            }
            set {
                if (show_button_border != value) {
                    show_button_border = value;

                    if (show_button_border) {
                        but_calc_button.FlatAppearance.BorderColor = Color.Gray;
                        but_calc_button.FlatAppearance.BorderSize = 1;
                    }
                    else {
                        but_calc_button.FlatAppearance.BorderColor = Color.White;
                        but_calc_button.FlatAppearance.BorderSize = 0;
                    }
                }
            }
        }

        string prev_value;
        void button1_Click(object sender, EventArgs e) {

            if (!this.ContainsFocus)
                this.Select();

            if (Numpad.Visible) {
                this.Focus();
                Numpad.Do_Close();
                return;
            }


            (this.TopLevelControl is IControlHost).tiff();


            {
                var ctrl = this.TopLevelControl;
                ctrl.Controls.Add(Numpad);

                var host = (ctrl as IControlHost);
                host.MouseClickedOnScreen += NumericBox_MouseClickedOnScreen;
            }

            Numpad.Initialize();

            {
                Rectangle rect1 = this.Bounds_On_Screen().Expand(1);
                Rectangle rect2 = Numpad.Bounds_On_Screen();

                Rectangle container = Numpad.Parent.Bounds_On_Screen();

                Rectangle? rect3 = rect2.Align_Vertices_In_Container(rect1, container, Pair.Make(1, 2), Pair.Make(2, 1));

                var pt = Numpad.Parent.PointToClient(rect3.Value.Location);
                Numpad.Location = pt.Translate(1, 0);
            }

            prev_value = this.Text;
            Numpad.Appear();

            BeginInvoke((MethodInvoker)(() => this.SimulateEntered()));
        }

        void NumericBox_MouseClickedOnScreen(object sender, EventArgs e) {

            var pt = this.Get_Cursor_Location();

            if (Numpad.Bounds_On_Screen().Contains(Cursor.Position))
                return;

            if (but_calc_button.Bounds.Contains(pt))
                return;

            if (Numpad.Visible) {
                vb_box.Focus();
                Numpad.Do_Close();
                //SimulateLeft();
            }
        }

        void numpad_VisibleChanged(object sender, EventArgs e) {

            if (!Numpad.Visible) {

                if (prev_value != this.Text)
                    this.Has_User_Typed_Text = true;

                if (vb_box.Text.Length > this.MaxLength)
                    vb_box.Text = vb_box.Text
                                          .Remove(MaxLength - 2)
                                          .TrimEnd('.') + ".00";

                if (NumpadClosed != null)
                    NumpadClosed(this, EventArgs.Empty);
            }
        }

        protected virtual void On_Numpad_Closing(Args<bool> e) {
            NumpadClosing.Raise(this, e);
        }

        public bool NumpadVisible { get { return Numpad.Visible; } }
    }
}