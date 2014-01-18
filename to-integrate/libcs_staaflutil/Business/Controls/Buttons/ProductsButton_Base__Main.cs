using System;
using System.Diagnostics;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    public partial class Our_Button : System.Windows.Forms.Button
    {

        public Our_Button() {

            this.SetStyle(ControlStyles.Selectable, false);

            this.UpdateStyles();


        }


        bool toggled;
        bool toggle_able;

        public bool Toggled {
            get { return toggled; }
            set {
                if (value == toggled)
                    return;

                (value && !Toggle_Able).tift
                    ("Unable to set toggled state on button while Toggled_Able is set to false");

                toggled = value;

                if (value)
                    On_Toggled_On(EventArgs.Empty);
                else
                    On_Toggled_Off(EventArgs.Empty);


            }
        }

        public bool Toggle_Able {
            get { return toggle_able; }
            set {
                if (value == toggle_able)
                    return;

                toggle_able = value;

                Refresh();
                if (!value)
                    Toggled = false;
            }
        }

        // Changes the state of the button and
        // returns the new state
        public bool Toggle() {

            bool ret = Toggle(true);

            return ret;
        }

        public bool Toggle(bool throw_on_error) {

            bool ret = Toggled;

            if (throw_on_error || Toggle_Able)
                Toggled = !Toggled;
            else
                ret = false;

            return ret;
        }

        public bool Toggle_On_Click {
            get;
            set;
        }

        [DebuggerStepThrough]
        protected override void WndProc(ref Message m) {

            /*       This is done here instead of        */
            /*       MouseClick etc. to prevent flicker        */

            if (m.Msg == Native_Const.WM_LBUTTONUP) {
                if (this.Is_Under_Mouse()) {
                    if (Enabled && Toggle_On_Click)
                        Toggle(false);
                }

            }

            base.WndProc(ref m);
        }

    }
}
