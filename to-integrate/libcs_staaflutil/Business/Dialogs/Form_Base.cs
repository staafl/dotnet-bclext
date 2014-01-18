using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Standardization;

using Fairweather.Service;
using Common.Controls;

namespace Common.Dialogs
{
    /// <summary> All sorts of helper methods and properties
    /// useful for the deriving forms. Also used to ensure 
    /// consistent appearance. 
    /// Currently implemented:
    /// * Hide_Focus() 
    /// * IPC messaging (see WndProc & OnWoken)
    /// * Global help provision (see OnHelpRequested)
    /// * Initial size stored (see OnLoad)
    /// * Global OSK provision (a tad complicated)
    /// * Hotkeys provison
    /// * Tight control over closing sequence
    /// * Forcing of 'flat' appearance upon all children
    /// * Automatic Refresh() of parent when hidden
    /// </summary>
    public partial class Form_Base : Form
    {
        /*       Construction        */

        public Form_Base()
            : this(Form_Kind.Fixed) {
            // H.IsInDesigner.tiff();
        }

        protected Form_Base(Form_Kind kind) {

            Form_Kind = kind;
            this.Hotkeys = new Dictionary<Keys, Func<bool, bool>>();

            is_fixed = kind.Contains(Form_Kind.Fixed);
            is_main = kind.Contains(Form_Kind.Main_Form);
            is_dialog = kind.Contains(Form_Kind.Modal_Dialog);

            // basic appearance settings

            this.BackColor = Colors.FormBackground;

            if (is_fixed) {

                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.SizeGripStyle = SizeGripStyle.Hide;

            }


            this.ShowIcon = is_main;
            this.ShowInTaskbar = is_main;

            this.MaximizeBox = is_main && !is_fixed;
            this.MinimizeBox = is_main;

            if (is_dialog)
                this.StartPosition = FormStartPosition.CenterParent;

            // ****************************
            
            this.Allow_Normal_Close = true;

            Close_On_Esc = is_dialog;

            Prepare_Focus_Ctrl();



        }

        /*       Hiding focus        */

        void Prepare_Focus_Ctrl() {

            this.Controls.Add(focus_ctrl);

            focus_ctrl.TabIndex = int.MaxValue;
            focus_ctrl.TabStop = false;
            focus_ctrl.Size = new Size(1, 1);
            focus_ctrl.Location = Point.Empty.Translate(-10, -10);
            focus_ctrl.Visible = true;
            focus_ctrl.BorderStyle = BorderStyle.None;
            
        }

        readonly TextBox focus_ctrl = new TextBox();

        protected void Hide_Focus() {

            focus_ctrl.Select_Focus();
            focus_ctrl.Select_Focus();

        }

        // Receiving of IPC messages

        protected override void WndProc(ref Message m) {

            if (m.Msg == Native_Const.WM_USER) {

                var wparam = (int)m.WParam;
                var lparam = (int)m.LParam;

                if (lparam == -1) {

                    if (Data.App == App.Entry_Screens) {
                        Try_Close();
                        Application.Exit();

                    }
                    return;

                }

                var module = (Sub_App)(lparam);
                
                // in case of debugging
                // MessageBox.Show(this, 
                // "Received at {0} : {1}, {2} {3}".spf(this.Handle, m.LParam, module, Global.Sub_Application));

                if (module == Data.Sub_App) {

                    this.Activate(true, true);
                    OnWoken();

                }

                return;

            }

            base.WndProc(ref m);

        }

        protected virtual void OnWoken() {
        }


        /*       Help        */

        protected override void OnHelpRequested(HelpEventArgs hevent) {

            string url = Data.Get_Help_Url(this, hevent);

            if (url == null) {
                base.OnHelpRequested(hevent);
                return;
            }

            try {
                Help.ShowHelp(this, url);
            }
            catch (ArgumentException) {
                Standard.Tell("Unable to find help resource {0}.", url);
            }

        }
        
        // ***

        protected void Try_Refresh_Parent() {

            var bnds = this.Bounds_On_Screen();

            var parent = this.Parent;

            if (parent == null || parent.IsDisposed)
                return;

            parent.InvokeOrNot(true, false, () => parent.Invalidate(this.Bounds_On_Control(parent)));


        }


        /*       To override or set...       */

        protected virtual TextBox[] Flat_Text_Boxes {
            get {
                return new TextBox[] { };
            }
        }

        protected virtual void OSK_Visible_Changed(bool visible) { }

        protected virtual bool Reposition_For_OSK { get { return true; } }


        protected Func<Point> osk_location_producer;

        protected bool Allow_Normal_Close {
            get;
            set;
        }

        protected bool Close_On_Esc {
            get;
            set;
        }

        protected bool Use_OSK {
            get;
            set;
        }

        protected bool Force_Foreground { get; set; }


        /*       Hotkeys        */


        /// <summary>
        /// Each action should take a parameter indicating whether the 
        /// Alt key is currently pressed
        /// Return value indicates whether the pressed key should be filtered.
        /// </summary>
        protected Dictionary<Keys, Func<bool, bool>> Hotkeys {
            get;
            set;
        }

        protected void Set_FX(Keys f_key, Action handler) {

            Hotkeys.Add(f_key, alt =>
            {
                if (!alt)
                    handler();
                return true;
            });

        }

        protected void Set_Handler(Button but, Action handler) {
            Keys? key = null;
            Set_Handler(but, key, handler);
        }



        protected void Set_Handler(Button but, Keys? key, Action handler) {
            bool alt_state = true;
            Set_Handler(but, key, alt_state, handler);
        }

        protected void Set_Handler(Button but, Keys? key, bool alt_state, Action handler) {

            if (but != null)
                but.Click += (_1, _2) => handler();

            if (key != null) {

                Func<bool, bool> func = alt =>
                {
                    if (alt == alt_state) {
                        if (but == null || (but.Visible && but.Enabled)) {
                            handler();

                        }
                    }

                    return true;
                };

                Hotkeys.Add(key.Value, func);
            }
        }


        bool Handle_Key_Down(Keys keys) {

            if (Use_OSK && keys == Keys.F12) {
                but_keyboard_Click(null, EventArgs.Empty);
                return true;
            }

            if (Close_On_Esc && keys == Keys.Escape) {
                this.DialogResult = DialogResult.Cancel;
                Try_Close();
                return true;
            }

            Func<bool, bool> act;

            if (Hotkeys.TryGetValue(keys, out act)) {

                bool alt = H.Is_Alt_Pressed();
                // Native_Methods.GetKeyState(Key_Codes.VK_LMENU) != 0;// ||
                // Native_Methods.GetKeyState(Key_Codes.VK_RMENU) != 0;

                var ret = act(alt);
                return true;
            }
            return false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {


            if (msg.Msg == Native_Const.WM_KEYDOWN ||
                msg.Msg == Native_Const.WM_SYSKEYDOWN) {

                Keys keyCode = keyData & Keys.KeyCode;

                if (Handle_Key_Down(keyCode))
                    return true;

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /*       Closing        */

        protected virtual bool Closing_Allowed() {
            return true;
        }

        protected bool b_try_close;

        public bool Is_Closed {
            get;
            set;
        }

        public bool Is_Closing {
            get;
            set;
        }

        protected virtual void Cancel_Clicked() {

            this.DialogResult = DialogResult.Cancel;
            Try_Close();

        }

        /// <summary>
        /// a) conditionally cancels the shutdown
        /// b) refreshes the parent
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e) {

            if (!Closing_Allowed()) {
                e.Cancel = true;
                return;
            }

            // every deriver and ancestor has had the
            // ability to cancel the operation before this point

            // should we cancel ?
            if (!Allow_Normal_Close && !b_try_close)
                e.Cancel = true;

            Is_Closing = true;
            try {

                base.OnFormClosing(e);

                if (!e.Cancel)
                    Is_Closed = true;

            }
            finally {
                Is_Closing = false;
            }


            Try_Refresh_Parent();

        }


        /// <summary>
        /// Initiates the closing sequence. Nothing really special inside other
        /// than setting a flag.
        /// </summary>
        public void Try_Close() {

            bool was = H.Set(ref b_try_close, true);

            try {
                base.Close();
                if (!Is_Closed)
                    return;
                var owner = this.Owner;
                if (owner != null)
                    owner.Activate();
            }
            finally {
                b_try_close = was;
            }
        }

        /*       Launching        */

        /// 08 Dec 09
        protected override void OnLoad(EventArgs e) {

            this.Force_Handle();

            initial_size.Assign(Size);

            base.OnLoad(e);

        }

        protected override void OnVisibleChanged(EventArgs e) {

            if (this.IsDisposed)
                return;

            base.OnVisibleChanged(e);

            if (!this.Visible)
                Try_Refresh_Parent();

        }

        void Flat_Style_Hack() {
            if (!H.Is_In_Designer) {

                Func<Control, bool> predicate =
                    c =>
                    {

                        Control parent = c.Parent;
                        if (//c.Parent is UserControl ||
                            parent is NumericUpDown ||
                            parent is Address_Box ||
                            parent is DataGridView ||
                            parent is Our_Combo_Box ||
                            parent is Numeric_Box)
                            return false;

                        return true;

                        //                    if (c.Parent == null ||
                        //c.Parent == this ||
                        //c.Parent is GroupBox ||
                        //c.Parent is Panel)
                        //                        return true;
                    };


                var children = this.All_Children().Where(predicate);

                foreach (var tbx in children
                                    .OfType<TextBoxBase>())
                    tbx.Flat_Style();

                foreach (var cb in children
                                   .OfType<ComboBox>())
                    cb.Flat_Style();

                foreach (var nupd in children
                                     .OfType<NumericUpDown>())
                    nupd.BorderStyle = BorderStyle.FixedSingle;

            }
        }

        protected override void OnShown(EventArgs e) {

            base.OnShown(e);

            if (is_main)
                this.Activate(true, true);

            if (is_fixed)
                this.MinimumSize = this.MaximumSize = this.Size;

            Flat_Style_Hack();

        }



        /*       On Screen Keyboard        */

        static bool m_OSK_Active;

        protected static bool OSK_Active {
            get {
                return m_OSK_Active;
            }
            set {
                if (m_OSK_Active == value)
                    return;

                m_OSK_Active = value;
                OSK_Active_Changed.Raise(null, Args.Make_RO(value));
            }
        }

        protected static event Handler_RO<bool> OSK_Active_Changed;


        //static class OSK_Synchronization
        //{
        //    /// <summary>
        //    /// represents the current hierarchy of Forms.
        //    /// At the moment it is assumed that each Form is modal.
        //    /// If the need for generality arises, the List[] can be replaced
        //    /// with a Tree[]
        //    /// </summary>     /* root -> children */
        //    readonly Many_Dict<Form_Base, Form_Base> forms;
        //    /// <summary>
        //    /// The forms that must be excluded from the logic.
        //    /// </summary>
        //    readonly Set<Form_Base> exclusions;
        //    /* Desired behavior:
        //     * When the OSK is enabled / disabled in one form, it will
        //     * automatically be enabled or disabled in any of the other
        //     * forms in the particular form's hierarchy.
        //     */

        //}
        protected override void OnActivated(EventArgs e) {

            base.OnActivated(e);

            if (b_Show_OSK)
                return;

            if (OSK_Active) {

                if (Use_OSK &&
                    !Is_OSK_Visible) {

                    MethodInvoker act = () => this.Show_OSK();
                    
                    this.Force_Handle();
                    BeginInvoke(act);


                }

            }

        }

        protected override void OnDeactivate(EventArgs e) {

            base.OnDeactivate(e);

            if (b_Show_OSK)
                return;

            if (this.Modal)
                return;

            if (!H.App_Active())
                return;

            if (this.Is_OSK_Visible)
                m_kform.Visible = false;

        }


        /*       Viva la composition        */


        protected Keyboard_Form m_kform;

        protected bool Is_OSK_Visible {
            get {
                return m_kform != null && m_kform.Visible;
            }
            set {
                if (value)
                    Show_OSK();
                else
                    Hide_OSK();
            }
        }


        bool b_Show_OSK;
        protected void Show_OSK() {

            if (Is_OSK_Visible)
                return;

            b_Show_OSK = true;
            try {
                if (m_kform == null)
                    m_kform = new Keyboard_Form(this, osk_location_producer);

                if (m_kform.Disposed_Ing())
                    return;

                if (Reposition_For_OSK)
                    Position_For_OSK(true);

                m_kform.Show_Unactivated();
                this.Select_Focus();

                OSK_Visible_Changed(true);
                OSK_Active = true;

            }
            finally {
                b_Show_OSK = false;
            }

        }

        protected void Hide_OSK() {

            if (!Is_OSK_Visible)
                return;

            if (m_kform.Disposed_Ing())
                return;

            OSK_Visible_Changed(false);

            if (Reposition_For_OSK)
                Position_For_OSK(false);

            m_kform.Hide();
            this.Select_Focus();

            OSK_Active = false;

        }




        Rectangle before_keyboard_form;

        void Position_For_OSK(bool show) {

            this.Force_Handle();
            BeginInvoke((MethodInvoker)(() =>
            {
                if (Is_Closing || b_try_close || Is_Closed)
                    return;

                if (show) {
                    before_keyboard_form = this.Bounds;

                    var on_screen = m_kform.Bounds_On_Screen();
                    var screen = H.Screen_Rectangle(true);

                    if (!screen.Contains(on_screen)) {
                        var yy = on_screen.Bottom - screen.Bottom;
                        this.Bounds = this.Bounds.Translate(0, -yy);
                    }
                }
                else {

                    this.Bounds = before_keyboard_form;

                }
            }));

        }



        protected virtual void but_keyboard_Click(object sender, EventArgs e) {

            var ctrl = this.ActiveControl;

            if ((ctrl is Button) &&
                ctrl.Name == "but_keyboard") {

                ctrl = null;

            }

            if (Is_OSK_Visible)
                Hide_OSK();
            else
                Show_OSK();

            this.Activate();

            if (ctrl == null) {
                this.SelectNextControl(null, true, true, true, true);
            }
            else {
                ActiveControl = ctrl;
                ctrl.Select_Focus();
            }




        }

        public Form_Kind Form_Kind { get; set; }

        readonly bool is_dialog;
        readonly bool is_fixed;
        readonly bool is_main;
        readonly Assign_Once<Size> initial_size;



    }

}
