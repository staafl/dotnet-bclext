using System;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    partial class Our_Calendar : UserControl
    {
        bool bf_resize;
        protected override void OnResize(EventArgs e) {

            if (bf_resize)
                return;

            try {

                bf_resize = true;
                if (b_init) {
                    Adjust_Size();
                    Finish_Resize();

                }

                base.OnResize(e);
            }
            finally {
                bf_resize = false;
            }
        }
        void Finish_Resize() {

            Create_Date_Rectangles();
            Setup_Dates();

            Set_Strip_Location();
            Set_Button_Locations();
            Refresh_Date_Title_Font_Settings();
        }

        Rectangle strip_rectangle;
        void Set_Strip_Location() {

            var tmp = new Rectangle(0, 0, this.Width, cst_top_gutter + 8);
            strip_rectangle = tmp.Expand(-cst_border - 1, true, true, true, true);

        }
        partial void Adjust_Size() {

            int x_step, y_step;
            Aux_Get_Measurements(out x_step, out y_step);


            int x_new = x_step * cst_cols + cst_border * 2;
            int y_new = y_step * cst_rows + cst_top_gutter + cst_border * 2;

            if (x_new != this.Width || y_new != this.Height)
                this.Size = new Size(x_new, y_new);

        }

        void Aux_Get_Measurements(out int x_min, out int y_min,
                          out int x_max, out int y_max,
                          out int x_step, out int y_step) {

            Aux_Get_Measurements(out x_min, out y_min,
                                 out x_max, out y_max);

            Aux_Get_Measurements(x_min, y_min,
                                 x_max, y_max,
                                 out x_step, out y_step);
        }

        void Aux_Get_Measurements(out int x_step, out int y_step) {

            int x_min, y_min, x_max, y_max;

            Aux_Get_Measurements(out x_min, out y_min,
                                 out x_max, out y_max,
                                 out x_step, out y_step);
        }

        void Aux_Get_Measurements(out int x_min, out int y_min,
                                  out int x_max, out int y_max) {

            x_min = cst_border;
            y_min = cst_top_gutter + cst_border + cst_day_title;

            x_max = this.Width - cst_border;
            y_max = this.Height - cst_border;
        }

        void Aux_Get_Measurements(int x_min, int y_min,
                                  int x_max, int y_max,
                                  out int x_step, out int y_step) {

            x_step = (x_max - x_min) / cst_cols;
            y_step = (y_max - y_min) / cst_rows;
        }
    }
}