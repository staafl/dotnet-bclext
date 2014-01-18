using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

using Fairweather.Service;


namespace Common.Controls
{
    public partial class Keyboard : Panel
    {
        public event Action<Key_Pressed_Args> Key_Pressed;

        bool Aux_Is_Capslock_On() {

            bool ret = m_toggled.Contains("Caps");

            return ret;
        }

        bool Aux_Is_Shift_On() {

            bool ret = m_toggled.Contains("Shift");

            return ret;

        }

        bool Aux_Is_Uppercase_On() {

            bool ret = Aux_Is_Capslock_On() != Aux_Is_Shift_On();

            return ret;
        }

        public bool CapsLock_Toggled {
            get { return Aux_Is_Capslock_On(); }
        }

        bool Aux_Is_Key_Toggled(Keyboard_Key key) {

            if (key.Toggleable == false)
                return false;

            bool ret = m_toggled[key.Name];

            return ret;

        }

        void Aux_Set_Toggled(Keyboard_Key key, bool state) {

            m_toggled[key.Name] = state;


        }

        void Handle_Key_Pressed(Keyboard_Key key) {

            if (key.Toggleable) {

                Aux_Set_Toggled(key, !Aux_Is_Key_Toggled(key));
                return;

            }

            var toggled = m_toggled.Where(_t => _t != "Caps").lst();

            var e = new Key_Pressed_Args(key, toggled, Aux_Is_Capslock_On());

            Key_Pressed.Raise(e);

            if (Auto_Revert_Shift) {
                string[] arr = new[] { "Alt", "Ctrl", "Shift" };
                foreach (var name in arr) {

                    m_toggled[name] = false;

                }

            }

        }

        Keyboard_Key pressed_key;
        bool key_pressed;
        protected override void OnMouseDown(MouseEventArgs e) {

            var pt = this.Get_Cursor_Location();

            key_pressed = false;

            foreach (var key in m_layout) {

                if (key.Bounds.Contains(pt)) {

                    if (key.Key.Input_1 == null)
                        break;

                    pressed_key = key;
                    key_pressed = true;
                    Refresh();

                    break;
                }
            }

            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e) {

            if (key_pressed) {

                Handle_Key_Pressed(pressed_key);
                BeginInvoke((MethodInvoker)(() =>
                {
                    key_pressed = false;
                    Refresh();
                }));
            }

            base.OnMouseUp(e);
        }

        public class Key_Pressed_Args
        {
            public Key_Pressed_Args(Keyboard_Key key, List<string> toggled, bool pr_capslock) {
                this.Toggled = toggled;
                this.Key = key;
                this.Caps_Lock = pr_capslock;
            }


            public List<string> Toggled { get; private set; }
            public bool Caps_Lock { get; private set; }
            public Keyboard_Key Key { get; private set; }

        }
    }
}
