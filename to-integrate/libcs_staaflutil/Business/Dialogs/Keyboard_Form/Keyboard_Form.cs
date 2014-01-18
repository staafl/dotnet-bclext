using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Common.Controls;
using Fairweather.Service;
using Colors = Standardization.Colors.Keyboard;
using Fonts = Standardization.Fonts.Keyboard;
using System.Collections.Generic;
using System.Linq;

namespace Common.Dialogs
{
    public partial class Keyboard_Form : Pos_Tool_Form
    {
        public Keyboard_Form(Form host)
            : this(host, null) { }

        public Keyboard_Form(Form host, Func<Point> location_producer)
            : base(host, location_producer) {

            InitializeComponent();

            Prepare_Keyboard();
            Prepare_Calculator();

        }


        protected override void Dispose(bool disposing) {

            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);

        }

        void Prepare_Calculator() {


            var as_ = Host as IActiveControlEnabledForm;
            if (as_ == null)
                return;

            this.calc = new Calculator(as_);
            calc.Scale(new SizeF(1.2f, 1.2f));

            var rt = this.ClientRectangle.Vertex(1).Translate(-8, 9);
            rt = rt.Translate(0, 3);
            var bounds = calc.Bounds
                             .Move_Vertex(1, rt)
                             .Expand(0, 0, 0, -4);

            calc.Bounds = bounds;
            calc.Anchor = AnchorStyles.Right | AnchorStyles.Top; /* doesn't seem to work */
            this.Controls.Add(calc);
            calc.Visible = true;
            calc.BringToFront();
            //calc.BorderStyle = BorderStyle.FixedSingle;


        }

        // Note: To be called on construction
        void Prepare_Keyboard() {

            keyboard1 = new Keyboard();

            keyboard1.Dock = DockStyle.Fill;
            keyboard1.Margin = new Padding(0, 3, 0, 0);
            keyboard1.Location = Point.Empty;

            keyboard1.Name = "keyboard1";
            keyboard1.Size = new System.Drawing.Size(932, 218);
            keyboard1.TabIndex = 0;

            keyboard1.Back_Color = Colors.Back_Color;
            keyboard1.Border_Color = Colors.Border_Color;
            keyboard1.Key_Color = Colors.Key_Color;
            keyboard1.Text_Color = Colors.Text_Color;

            keyboard1.Key_Font = Fonts.Key_Font;
            keyboard1.Auto_Revert_Shift = true;

            keyboard1.MouseDown += keyboard1_MouseDown;
            keyboard1.Key_Pressed += keyboard1_Key_Pressed;

            Controls.Add(keyboard1);

        }

        void keyboard1_MouseDown(object sender, EventArgs e) {

            if (!Host.ContainsFocus)
                Host.Focus();

        }

        void keyboard1_Key_Pressed(Keyboard.Key_Pressed_Args e) {

            // http://msdn.microsoft.com/en-us/library/8c6yea83(VS.85).aspx

            var dict = new Twoway<string, string>
            {
                {"Alt", "%"},
                {"Ctrl", "^"},
                {"Shift", "+"},
            };


            var in1 = e.Key.Key.Input_1;
            var in2 = e.Key.Key.Input_2;

            var str = in1;

            if (e.Toggled.Contains("Shift") != e.Caps_Lock) {
                str = in2;

            }

            var to_escape = dict.Rights.Concat(new[] { "(", ")", "~" });

            if (to_escape.Any(_s => str.Contains(_s)) ||
                str == "{" || 
                str == "}") { // sic

                str = "{" + str + "}";
            }

            if (in1 != in2)
                dict.Remove_L("Shift");

            var mod = dict.Where(_kvp => e.Toggled.Contains(_kvp.First))
                          .Aggregate("", (_s, _p) => _s + _p.Second);

            str = mod + str;


            try {
                SendKeys.Send(str);

                // SendKeys.SendWait(str);
                Thread.Sleep(10);
            }
            catch (NullReferenceException) {

            }
            /*       Older methods for sending keystrokes removed in rev 719        */

            Cursor.Show();
        }

        public Calculator Calc {
            get {
                return calc;
            }
        }


    }
}
