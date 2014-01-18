using System;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    partial class Our_Calendar : UserControl
    {


        static readonly Pen pen_border = Pens.Black;

        Action<Graphics, int, int> m_paint_rectangle;
        void Prepare_Painter_Methods() {

            m_paint_rectangle =
            (grp, xx, yy) =>
            {
                Rectangle rect = m_date_rectangles[xx][yy].Rectangle;

                DateInfo info = m_dates[xx][yy].First;

                bool is_selected = (info.Date == this.Value);

                grp.FillRectangle(is_selected ? brush_cell_selected : brush_cell,
                                  rect);

                int day = info.Date.Day;
                bool single = day <= 9;

                string str = day.ToString();

                SolidBrush brush = (info.Type == DateType.ThisMonth) ?
                                               (yy < 5) ?
                                            brush_month :
                                            brush_wkend :
                                                         brush_trail;

                grp.DrawString(str, this.Cell_Font,
                               brush, rect.Center()
                                                .Translate(single ? m_date_font_x1 :
                                                                    m_date_font_x2,
                                                           m_date_font_y));
            };
        }

        void Draw_Date_String(Graphics g) {

            string str = this.Value_String;

            g.DrawString(str, Strip_Font, brush_strip_text,
                        (PointF)strip_rectangle.Center()
                                               .Translate(m_strip_x, m_strip_y));

        }
        void Draw_Day_Titles(Graphics g) {

            for (int ii = 0; ii < 7; ++ii) {

                Point pt = m_day_title_drawing_pts[ii];

                g.DrawString((ii + 1).Get_Weekday_Abbr(false),

                             this.Day_Title_Font,

                             brush_day_title,

                             pt);
            }

        }

        protected override void OnPaint(PaintEventArgs e) {

            Action<int, int, int> painter =
                (index, xx, yy) => m_paint_rectangle(e.Graphics, xx, yy);

            e.Graphics.FillRectangle(brush_back,
                                     this.ClientRectangle);

            e.Graphics.DrawRectangle(pen_border,
                                     this.ClientRectangle.Expand(-1, false, false,
                                                                     true, true));

            e.Graphics.FillRectangle(brush_strip, strip_rectangle);

            Draw_Date_String(e.Graphics);

            Aux_Traverse(0, cst_total_cells, painter);

            Draw_Day_Titles(e.Graphics);


            base.OnPaint(e);
        }


    }
}