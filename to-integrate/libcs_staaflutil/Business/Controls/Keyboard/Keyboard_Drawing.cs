using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Fairweather.Service;

namespace Common.Controls
{
    public partial class Keyboard : Panel
    {
        // NOTE: Should be made instance
        // if more than one keyboard is
        // being used
        //
        // Alternatively, we could pool
        // the brushes between all instances
        // of the control
        SolidBrush brush_back;
        SolidBrush brush_key;
        SolidBrush brush_text;
        SolidBrush brush_toggle = new SolidBrush(Color.Gray);

        Pen pen_border;
        Pen pen_wide_border_2 = new Pen(Color.Gray, 2);

        Pen pen_wide_border_1_black = new Pen(Color.Black, 1);
        Pen pen_wide_border_3 = new Pen(Color.Black, 2);

        Dictionary<int, PointF> m_text_locations = new Dictionary<int, PointF>();

        partial void Prepare_Text_Layout() {

            using (var g = this.CreateGraphics()) {
                Size sz;
                for (int ii = 0; ii < m_layout.Length; ++ii) {

                    sz = g.MeasureString(m_layout[ii].Name, this.Key_Font)
                          .ToSize()
                          .Scale(2, false);

                    m_text_locations[ii] = m_layout[ii].Bounds
                                                       .Center()
                                                       .Translate(sz, false);
                }
            }

            Set_Explicit_Text_Locations();
        }
        void Set_Explicit_Text_Locations() {

            int offset;
            Keyboard_Key key;
            for (int ii = 0; ii < m_layout.Length; ++ii) {
                key = m_layout[ii];

                if (m_text_offsets.TryGetValue(key.Name, out offset))
                    m_text_locations[ii] = new PointF((float)(m_layout[ii].Bounds.Left + offset),
                                                       m_text_locations[ii].Y);
            }
        }

        protected override void OnPaint(PaintEventArgs e) {

            e.Graphics.FillRectangle(brush_back, this.ClientRectangle);
            e.Graphics.DrawRectangle(pen_border, this.ClientRectangle.Expand(-1, false, false, true, true));

            bool upper = Aux_Is_Uppercase_On();

            for (int ii = 0; ii < m_layout.Length; ++ii) {

                var key = m_layout[ii];

                var draw_string = key.Name;

                if (draw_string.Safe_Length() == 1 && key.Key.Input_1!= "{UP}") {
                    if (upper)
                        draw_string = key.Key.Input_2;

                }

                var font = this.Key_Font;

                bool depressed = (key_pressed && pressed_key.Position == ii) || Aux_Is_Key_Toggled(key);

                /*       Hack        */

                bool down = (key.Name == "DOWNARROW");

                if (down) {

                    draw_string = "^";

                    m_text_locations[ii] = new PointF(-554.0f, -197.0f);

                    if (depressed)
                        m_text_locations[ii] = m_text_locations[ii].Translate(-2, -2);

                }

                var rect = Rectangle.Round(key.Bounds);
                var bounds = m_text_locations[ii];
                if (depressed)
                    bounds = bounds.Translate(+1, +1);


                e.Graphics.FillRectangle(depressed ? brush_toggle : brush_key,
                                         key.Bounds);

                if (down)
                    e.Graphics.RotateTransform(180.0f);

                e.Graphics.DrawString(draw_string,
                                      font,
                                      brush_text,
                                      bounds);

                if (down)
                    e.Graphics.RotateTransform(180.0f);

                if (depressed) {
                    e.Graphics.DrawRectangle(pen_wide_border_1_black, rect);
                    e.Graphics.DrawCurve(pen_wide_border_3, rect.Translate(+1, +1)
                                                                .Vertices(3, 0, 1), 0.0f);

                }
                else {
                    e.Graphics.DrawRectangle(pen_wide_border_2, rect);

                }
            }

            e.Graphics.Flush(System.Drawing.Drawing2D.FlushIntention.Sync);
            base.OnPaint(e);
        }
    }
}
