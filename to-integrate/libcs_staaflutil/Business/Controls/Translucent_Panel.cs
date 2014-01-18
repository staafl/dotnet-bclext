using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Fairweather.Service;

namespace Common.Controls
{
    public interface IPanel
    {
        void Done();
    }

    public class Translucent_Panel : Panel, IPanel
    {
        public Translucent_Panel() {

            // no double-buffering
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);


        }

        protected override void Dispose(bool disposing) {
            if (disposing)
                using (Maybe()) { }

            base.Dispose(disposing);
        }

        static readonly SolidBrush def_brush = Get(0x80, Color.Gray.Darken(0xB0));

        static SolidBrush Get(int alpha, Color tint) {
            var color = Color.FromArgb(alpha, tint);
            return new SolidBrush(color);
        }

        SolidBrush br = def_brush;

        IDisposable Maybe() {
            var ret = br;
            if (ret == def_brush)
                return null;
            return ret;
        }

        public int Alpha {
            get {
                lock (def_brush)
                    return br.Color.A;
            }
            set {
                lock (def_brush)
                    using (Maybe()) {
                        br = Get(value, Tint);
                    }
            }
        }

        public Color Tint {
            get {
                lock (def_brush)
                    return Color.FromArgb(0, br.Color);
            }
            set {
                lock (def_brush)
                    using (Maybe()) {
                        br = Get(Alpha, value);
                    }
            }
        }


        bool done = false;

        public void Done() {
            this.Alpha = 0;
            done = true;
            this.Refresh();
            this.Hide();

        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            lock (def_brush) {
                e.Graphics.FillRectangle(br, this.ClientRectangle);
                if (done)
                    e.Graphics.Flush();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent) {

            // pevent.Graphics.FillRectangle(br, this.ClientRectangle);

            //do not allow the background to be painted 

        }

        /*
        protected override CreateParams CreateParams {

            get {

                var cp = base.CreateParams;

                cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT

                return cp;

            }

        }

        protected void InvalidateEx() {

            if (Parent == null)

                return;

            var rc = this.Bounds; // new Rectangle(this.Location, this.Size);

            Parent.Invalidate(rc, true);

        }


*/

    }
}
