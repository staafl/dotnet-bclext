using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


using Fairweather.Service;

namespace Common.Controls
{

    public partial class Border
    {
        protected override void OnPaint(PaintEventArgs e) {

            base.OnPaint(e);

            if (no_edges)
                return;

            var parent = this.Parent;
            var bnds = this.Bounds.Expand(1, 1, 0, 0);

            using (var g_parent = parent.CreateGraphics()) {

                Draw(g_parent, bnds);

            }

            if (m_host != null) {

                using (var g_host = m_host.CreateGraphics()) {

                    var to_draw = parent.Rectangle_On_Control(m_host, bnds);

                    Draw(g_host, to_draw);


                }
                // 20th September
                // m_host.Invalidate();
            }
        }

        void Draw(Graphics g, Rectangle bnds) {

            if (all_edges) {
                g.DrawRectangle(pen, bnds);

                return;
            }

            for (int ii = 0; ii < 4; ++ii) {

                if (!m_edges_to_draw[ii])
                    continue;

                var edge = bnds.Edge(ii);
                g.DrawLine(pen, edge.First, edge.Second);

            }


        }
        protected override void OnEnabledChanged(EventArgs e) {

            if (!this.Enabled)
                this.BackColor = Color.DarkGray;
            else
                this.BackColor = Color.Transparent;

            base.OnEnabledChanged(e);

        }

        void Refresh_Border() {

            (this.m_mode == Mode.Host).tiff();

            var offsets = m_offsets.Value;

            var bounds = m_host.Bounds;

            var original = this.Bounds;

            var new_bounds = bounds.Expand(offsets.First,
                                                    offsets.Second,
                                                    offsets.Third,
                                                    offsets.Fourth);

            if (original == new_bounds)
                return;

            this.Bounds = new_bounds;

            //this.Force_Handle();
            //BeginInvoke((MethodInvoker)(() =>
            //{
            //    this.Refresh();
            //    this.Parent.Invalidate(original);
            //    this.Parent.Invalidate(this.Bounds);
            //    this.Parent.Update();
            //}));

        }

        void Mimic_Color() {

            Verify_Mode(Mode.Host);

            this.BackColor = m_host.BackColor;

        }

        void host_BoundsChanged(object sender, EventArgs e) {

            Refresh_Border();

        }

        void host_ParentChanged(object sender, EventArgs e) {

            Refresh_Parent(true);


        }

        void Refresh_Parent(bool refresh_border) {

            (m_mode == Mode.Host).tiff();


            var parent = m_host.Parent;

            // We are probably called from some controls's
            // constructor
            if (parent == null)
                return;

            parent.Controls.Add(this);
            this.Parent = parent;

            if (refresh_border)
                Refresh_Border();

        }

        Quad<bool> m_edges = new Quad<bool>(true, true, true, true);
        bool[] m_edges_to_draw;
        bool all_edges = true;
        bool no_edges = false;

        public Quad<bool> Border_Edges {
            get {
                return m_edges;
            }
            set {
                if (m_edges == value)
                    return;

                m_edges = value;

                m_edges_to_draw = value.To_Array();

                if (m_edges_to_draw.All(b => b)) {

                    all_edges = true;
                    no_edges = false;
                    return;
                }

                all_edges = false;


                if (m_edges_to_draw.All(b => !b)) {

                    no_edges = true;

                    return;
                }

            }
        }
    }
}