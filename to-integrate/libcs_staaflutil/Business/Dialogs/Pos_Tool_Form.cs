using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Common.Controls;
using Fairweather.Service;
using Colors = Standardization.Colors.Keyboard;
using Fonts = Standardization.Fonts.Keyboard;

namespace Common.Dialogs
{
    public class Pos_Tool_Form : Hook_Enabled_Form
    {

        public Pos_Tool_Form(Form host)
            : this(host, null) { }

        public Pos_Tool_Form(Form host, Func<Point> location_producer)
            : base(true, true, Form_Kind.Modal_Dialog) {

            this.StartPosition = FormStartPosition.Manual;

            this.Allow_Normal_Close = true;

            this.Owner = host;
            Host = host;
            m_location_producer = location_producer;
            host.Move += (_1, _2) => Maybe_Adjust_Position();
            host.SizeChanged += (_1, _2) => Maybe_Adjust_Position();

            Maybe_Adjust_Position();

            this.SetStyle(ControlStyles.Selectable, false);


        }

        public Form Host {
            get;
            set;
        }

        public void Adjust_Position() {

            if (m_location_producer == null) {
                var bnds = Host.Bounds;
                bnds = bnds.Translate(bnds.Width / 2, 0);
                bnds = bnds.Translate(-this.Width / 2, 0);

                this.Location = bnds.Vertex(3);
            }
            else {
                this.Location = m_location_producer();
            }

        }

        public void Show_Unactivated() {

            this.Owner = Host;
            Native_Methods.ShowWindow(this.Handle,
                                      Native_Const.SW_SHOWNOACTIVATE);

            Maybe_Adjust_Position();

        }


        /*       Positioning        */


        void Maybe_Adjust_Position() {

            if (!m_follow_host)
                return;

            Adjust_Position();

        }

        bool m_follow_host = true;
        protected Func<Point> m_location_producer;



        // Thanks to jmCilHinny

        /*       TOOL WINDOW IMPLEMENTATION        */

        protected override CreateParams CreateParams {
            get {
                var value = base.CreateParams;

                value.ExStyle |= Native_Const.WS_EX_NOACTIVATE;
                value.ExStyle |= Native_Const.WS_EX_TOOLWINDOW;
                value.Parent = IntPtr.Zero;

                return value;
            }
        }

        protected override void WndProc(ref Message m) {



            if (m.Msg == Native_Const.WM_MOUSEACTIVATE ||
                m.Msg == Native_Const.WM_ACTIVATE) {

                m.Result = (IntPtr)Native_Const.MA_NOACTIVATE;
            }
            else
                base.WndProc(ref m);
        }

        protected override void OnVisibleChanged(EventArgs e) {

            //if (this.Visible)
           //     this.BringToFront();

            base.OnVisibleChanged(e);

        }

    }
}
