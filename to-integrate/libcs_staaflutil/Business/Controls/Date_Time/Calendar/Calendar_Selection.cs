using System;
using System.Drawing;
using System.Windows.Forms;
using Fairweather.Service;

using EHandler = System.EventHandler<System.EventArgs>;

namespace Common.Controls
{
    partial class Our_Calendar : UserControl
    {
        public event EHandler Date_Changed;
        public event EHandler Date_DoubleClicked;
        public event EHandler Enter_Pressed;
        public event EHandler Esc_Pressed;

        protected override void OnDoubleClick(EventArgs e) {

            if (Aux_Is_Cursor_Over_Cells()) {

                int x_ind, y_ind;
                Aux_Get_Cell_Under_Cursor(out x_ind, out y_ind);

                if (m_dates[y_ind][x_ind].First.Type == DateType.ThisMonth)
                    Date_DoubleClicked.Raise(this);


            }
            base.OnDoubleClick(e);
        }

        protected virtual void On_Date_Changed(EventArgs e) {

            Date_Changed.Raise(this, e);
        }


        bool Aux_Is_Cursor_Over_Cells() {


            Point pt = this.Get_Cursor_Location();

            if (!this.ClientRectangle.Contains(pt))
                return false;

            Rectangle rect1 = this.ClientRectangle.Expand(-cst_border * 2);

            Rectangle rect2 = rect1.Expand(-cst_top_gutter, false, true, false, false);

            if (!rect2.Contains(pt))
                return false;

            return true;
        }

        void Aux_Get_Cell_Under_Cursor(out int x_ind, out int y_ind) {

            Point pt = this.Get_Cursor_Location();
            int x_coord = pt.X;
            int y_coord = pt.Y;
            x_ind = -1;
            y_ind = -1;

            for (int xx = 0; xx < cst_cols; ++xx) {

                if (m_date_rectangles[0][xx].Rectangle.Right >= x_coord) {
                    x_ind = xx;
                    break;

                }
            }

            for (int yy = 0; yy < cst_rows; ++yy) {

                if (m_date_rectangles[yy][0].Rectangle.Bottom >= y_coord) {
                    y_ind = yy;
                    break;

                }
            }
        }


        protected override void OnMouseDown(MouseEventArgs e) {

            if (Aux_Is_Cursor_Over_Cells()) {

                int x_ind, y_ind;
                Aux_Get_Cell_Under_Cursor(out x_ind, out y_ind);
                Value = m_dates[y_ind][x_ind].First.Date;
            }

            base.OnMouseDown(e);
        }

        protected override void OnEnter(EventArgs e) {

            label.Focus();
            base.OnEnter(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e) {

            if (Math.Abs(e.Delta) >= 120) {
                int offset = e.Delta / -120;
                Value = Value.AddMonths(offset);
            }

            base.OnMouseWheel(e);
        }
    }
}