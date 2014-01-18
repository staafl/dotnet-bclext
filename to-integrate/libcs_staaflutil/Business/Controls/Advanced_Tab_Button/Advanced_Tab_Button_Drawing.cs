using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;




using Common;
using Common.Controls;
using Common.Dialogs;
using DTA;
using Standardization;
using Fairweather.Service;


namespace Common.Controls
{
    public partial class Advanced_Tab_Button : Button
    {
        const int cst_corner = 2;

        public static readonly
        Color def_crown_color = Colors.Tab_Control.Default_Button_Crown;

        public static readonly
        Color def_border_color = Colors.Tab_Control.Default_Border_Color;

        public static readonly
        Color def_highlit_color = Colors.Tab_Control.Default_Highlight_Color;

        public static readonly
        Color def_back_color = Colors.Tab_Control.Default_Button_Back_Color;

        Pen border_pen = new Pen(def_border_color, 1);
        Pen crown_pen = new Pen(def_crown_color, 1);


        Updater2<Brush, Color>
        high_brush = Updater.Make2(def_highlit_color, color => (Brush)new SolidBrush(color), true);

        Updater2<Brush, Color>
        back_brush = Updater.Make2(def_back_color, color => (Brush)new SolidBrush(color));

        Color m_crown_color = def_crown_color;
        Color m_border_color = def_border_color;



        protected override void OnPaint(PaintEventArgs e) {

            var g = e.Graphics;
            g.Clear(Color.LightGray);

            var lt = new Point(cst_corner, 0);                          //   lt---------rt
            var rt = new Point(this.Width - cst_corner - 1, 0);         // lm             rm
            var lm = new Point(0, cst_corner);                          // |               |
            var rm = new Point(this.Width - 1, cst_corner);             // |               |
            var ld = this.ClientRectangle.Vertex(3).Translate(0, -1);   // |               |
            var rd = this.ClientRectangle.Vertex(2).Translate(-1, -1);  // ld-------------rd

            var client = this.ClientRectangle;

            if (m_is_activated || m_under_mouse) {

                g.FillRectangle(high_brush, client.Expand(0, -2, 0, 0));

                g.DrawLine(crown_pen, lt,
                                      rt);

                g.DrawLine(crown_pen, lt.Translate(-1, 1),
                                      rt.Translate(1, 1));

                g.DrawLine(crown_pen, lm,
                                      rm);

                g.DrawLine(border_pen, rm.Translate(0, 1),
                                       rd);

                g.DrawLine(border_pen, ld,
                                       lm.Translate(0, 1));

                if (!m_is_activated)
                    g.DrawLine(border_pen, ld, rd);



            }
            else {

                g.FillRectangle(back_brush,
                                this.ClientRectangle);

                var path = new GraphicsPath();
                path.AddLines(new Point[] { lt, rt, rm, rd, ld, lm, lt });

                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawPath(border_pen, path);
                g.SmoothingMode = SmoothingMode.Default;

            }

            if (this.Text.IsNullOrEmpty())
                return;

            g.DrawString(base.Text, this.Font, Brushes.Black, m_text_location);


        }
    }
}