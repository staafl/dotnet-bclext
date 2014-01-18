using System;

using Fairweather.Service;

using EventHandler = System.EventHandler<System.EventArgs>;
namespace Common.Controls
{
    public partial class Our_Button : System.Windows.Forms.Button
    {

        EventHandler hdl_toggled_on;
        EventHandler hdl_toggled_off;

        public event EventHandler Toggled_On {
            add {
                lock (hdl_toggled_on)
                    hdl_toggled_on += value;
            }
            remove {
                lock (hdl_toggled_on)
                    hdl_toggled_on -= value;
            }
        }

        public event EventHandler Toggled_Off {
            add {
                lock (hdl_toggled_off)
                    hdl_toggled_off += value;
            }
            remove {
                lock (hdl_toggled_off)
                    hdl_toggled_off -= value;
            }
        }

        protected virtual void On_Toggled_On(EventArgs e) {

            this.hdl_toggled_on.Raise(this, e);
        }

        protected virtual void On_Toggled_Off(EventArgs e) {

            this.hdl_toggled_off.Raise(this, e);
        }
    }
}