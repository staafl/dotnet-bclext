//#define NOHOOKS


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

using Fairweather.Service;


namespace Common.Dialogs
{
    /// <summary>
    /// Extends Form_Base, providing the ability to capture
    /// events using "hooks"
    /// </summary>
    public class Hook_Enabled_Form : Form_Base, // 19th September
                                     IControlHost,
                                     IActiveControlEnabledForm // 17 October
    {
        static object locker = new object();

        readonly bool is_IControlHost;
        readonly bool is_IActiveControlEnabledForm;

        static bool is_mousedown_set;
        static bool is_setfocus_set;



        static readonly List<Hook_Enabled_Form> mousedown_instances = new List<Hook_Enabled_Form>();
        static readonly List<Hook_Enabled_Form> setfocus_instances = new List<Hook_Enabled_Form>();




#if NOHOOKS
        static readonly Func<Message, bool> hook_mouse_down;
        static readonly Func<Message, bool> hook_set_focus;

        static Hook_Enabled_Form() {



            hook_mouse_down = (Message msg) =>
{

    foreach (var form in mousedown_instances) {

        if (form != Form.ActiveForm)
            continue;

        if (form.Visible && form.Is_Under_Mouse())
            form.OnMouseClickedOnScreen(EventArgs.Empty);

        return false;
    }

    return false;

};

            hook_set_focus = (Message msg) =>
            {
                foreach (var form in setfocus_instances) {

                    if (form != Form.ActiveForm)
                        continue;

                    if (form.Visible && form.Enabled && form.ContainsFocus) {
                        form.HandleControlChange(msg.WParam);
                    }

                    return false;

                }

                return false;

            };
        }

#else

        static readonly HookProc hook_mouse_down;
        static readonly HookProc hook_set_focus;

        static Hook_Enabled_Form() {



            hook_mouse_down = (int nCode, IntPtr wParam, IntPtr lParam) =>
          {

              foreach (var form in mousedown_instances) {

                  if (form != Form.ActiveForm)
                      continue;

                  if (form.Visible && form.Is_Under_Mouse())
                      form.OnMouseClickedOnScreen(EventArgs.Empty);

                  return 0;
              }

              return 0;
          };

            hook_set_focus = (int nCode, IntPtr wParam, IntPtr lParam) =>
            {
                foreach (var form in setfocus_instances) {

                    if (form != Form.ActiveForm)
                        continue;

                    if (form.Visible && form.Enabled && form.ContainsFocus) {
                        form.HandleControlChange(wParam);
                    }

                    return 0;
                }

                return 0;
            };
        }
#endif




        protected Hook_Enabled_Form()
            : this(Form_Kind.Modal_Dialog) {

        }

        protected Hook_Enabled_Form(Form_Kind kind)
            : this(true, true, kind) {

        }

        protected Hook_Enabled_Form(bool is_IControlHost,
                                    bool is_IActiveControlEnabledForm,
              Form_Kind kind)
            : base(kind) {

            this.is_IControlHost = is_IControlHost;
            this.is_IActiveControlEnabledForm = is_IActiveControlEnabledForm;

            bool do_setfocus, do_mousedown;

            lock (locker) {

                do_setfocus = is_IActiveControlEnabledForm && !is_setfocus_set;
                do_mousedown = is_IControlHost && !is_mousedown_set;

                if (is_IActiveControlEnabledForm) {
                    setfocus_instances.Add(this);
                    is_setfocus_set = true;
                }

                if (is_IControlHost) {
                    mousedown_instances.Add(this);
                    is_mousedown_set = true;
                }

            }

            if (do_setfocus) {
                try {
#if NOHOOKS
                    Filter.Register(Native_Constants.WM_SETFOCUS, hook_set_focus);
#else
                    Hook.SetHook(Hook_Type.SetFocus, hook_set_focus);
#endif
                }
                catch {
                    is_setfocus_set = false;
                    throw;
                }
            }

            if (do_mousedown) {
                try {
#if NOHOOKS
                    Filter.Register(Native_Constants.WM_LBUTTONDOWN, hook_mouse_down);
#else
                    Hook.SetHook(Hook_Type.MouseDown, hook_mouse_down);
#endif
                }
                catch {
                    is_mousedown_set = false;
                    throw;
                }
            }


        }



        public event EventHandler<EventArgs> MouseClickedOnScreen;

        bool b_OnMouseClickedOnScreen;
        [DebuggerStepThrough]
        protected virtual void OnMouseClickedOnScreen(EventArgs e) {

            if (b_OnMouseClickedOnScreen)
                return;

            b_OnMouseClickedOnScreen = true;
            try {
                MouseClickedOnScreen.Raise(this, e);

                ActiveControlChanged.Raise(this, EventArgs.Empty);


            }
            finally { b_OnMouseClickedOnScreen = false; }
        }


        /// IActiveControlEnabledForm

        public event EventHandler<EventArgs> ActiveControlChanged;

        bool b_HandleControlChange;
        Control last;
        void HandleControlChange(IntPtr handle) {

            if (b_HandleControlChange)
                return;

            b_HandleControlChange = true;
            try {
                var ctrl = Control.FromChildHandle(handle);

                // This piece of voodoo ensures consistency
                // when the selection is done with the mouse.
                if (ctrl == null)
                    return;

                if (ctrl == last)
                    return;

                last = ctrl;


                if (ctrl != this && !this.Controls.Contains(ctrl)) {
                    try {

                        if (ctrl.Is_Selectable())
                            if (ActiveControl != ctrl) {
                                ActiveControl = ctrl;
                            }
                    }
                    catch (System.ArgumentException) { }

                }


                ActiveControlChanged.Raise(this, EventArgs.Empty);
            }
            finally {
                b_HandleControlChange = false;
            }
        }

    }
}