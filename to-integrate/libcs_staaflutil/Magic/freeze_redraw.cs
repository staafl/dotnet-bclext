using System;
using System.Windows.Forms;

namespace Fairweather.Service
{
    static partial class Magic
    {
        public static Freeze_Redraw Freeze_Redraw(this Control ctrl) {

            var ret = new Freeze_Redraw(ctrl);
            ret.Active = true;
            return ret;

        }
    }

    public class Freeze_Redraw : IDisposable
    {
        readonly Control ctrl;
        public Freeze_Redraw(Control ctrl) {
            this.ctrl = ctrl;
        }

        public void Dispose() {
            this.Active = false;
        }

        bool active;

        // http://www.syncfusion.com/FAQ/windowsforms/faq_c95c.aspx#q637q
        public bool Active {
            get { return active; }
            set {

                if (value == Active)
                    return;

                const int setredraw = Native_Const.WM_SETREDRAW;
                ctrl.Force_Handle();

                var handle = ctrl.Handle;

                if (!ctrl.IsHandleCreated) // is this possible?
                    /* do not set field */
                    return;

                if (value) {
                    Native_Methods.SendMessage(handle, setredraw, 0, 0);
                }
                else {
                    Native_Methods.SendMessage(handle, setredraw, 1, 0);
                    ctrl.Invalidate(true);

                }

                active = value;
            }
        }


    }
}
