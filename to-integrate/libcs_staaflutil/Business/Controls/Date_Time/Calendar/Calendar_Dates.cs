using System;
using System.Windows.Forms;


using Fairweather.Service;

namespace Common.Controls
{
    partial class Our_Calendar : UserControl
    {
        partial void Create_Date_Rectangles() {

            int x_min, y_min, x_max, y_max, x_step, y_step;

            Aux_Get_Measurements(out x_min, out y_min,
                                 out x_max, out y_max,
                                 out x_step, out y_step);

            if (x_step < 10 || y_step < 10)
                throw new InvalidOperationException(
                    "Unable to create date rectangles with dimensions less than 10x10");

            float x_run = (float)x_min;
            float y_run = (float)y_min; // +3; // + 3 is a HACK!!!!!


            for (int xx = 0; xx < cst_rows; y_run += y_step, ++xx) {
                //1
                m_date_rectangles[xx] = new DateRectangle[cst_cols];
                x_run = x_min;

                for (int yy = 0; yy < cst_cols; x_run += x_step, ++yy)
                    //2
                    m_date_rectangles[xx][yy] = new DateRectangle(xx, yy,
                                                                  x_run + 1, y_run + 1,
                                                                  x_step - 2, y_step - 2);//2

                //1
            }
        }
        partial void Setup_Dates() {

            for (int ii = 0; ii < cst_rows; ++ii)
                m_dates[ii] = new Pair<DateInfo, DateRectangle>[cst_cols];

            DateTime value = this.Value;

            DateTime first_month_day = value.First_Date_Of_Month();
            DateTime first_displ_day = Aux_Get_First_Displayed_Day(value);

            DateTime last_month_day = value.Last_Date_Of_Month();
            DateTime last_displ_day = Aux_Get_Last_Displayed_Day(value);

            DateType type = default(DateType);

            Action<int, int, int> set = (index, xx, yy) =>
            {
                DateInfo info = new DateInfo(type, first_displ_day.AddDays(index));

                m_dates[xx][yy] = new Pair<DateInfo, DateRectangle>(info,
                                                                    m_date_rectangles[xx][yy]);

            };

            int start = 0;
            int range;

            //***//
            range = (first_month_day - first_displ_day).Days;

            type = (first_displ_day.Year != first_month_day.Year) ?
                                               DateType.LastYear :
                                               DateType.LastMonth;

            start = Aux_Traverse(start, range, set);
            //***//

            //***//
            type = DateType.ThisMonth;
            range = (last_month_day - first_displ_day).Days + 1;

            start = Aux_Traverse(start, range, set);
            //***//

            //***//
            type = (last_displ_day.Year != first_month_day.Year) ?
                                               DateType.NextYear :
                                               DateType.NextMonth;

            range = (last_displ_day - first_displ_day).Days + 1;

            start = Aux_Traverse(start, range, set);
            //***//
        }

        DateTime Aux_Get_First_Displayed_Day(DateTime date) {

            DateTime first_month_day = date.First_Date_Of_Month();
            DateTime ret = first_month_day;
            DayOfWeek dow = first_month_day.DayOfWeek;

            switch (dow) {

            case DayOfWeek.Monday: // 1
                ret = ret.AddDays(-7);
                break;
            case DayOfWeek.Sunday: // 0
                ret = ret.AddDays(-6);
                break;
            default:
                int day = (int)dow;
                ret = ret.AddDays(-day + 1);
                break;
            }

            return ret;
        }
        DateTime Aux_Get_Last_Displayed_Day(DateTime date) {

            DateTime ret = Aux_Get_First_Displayed_Day(date).AddDays(41);

            return ret;
        }

        /// <summary>  End index is not included.
        /// 
        /// Returns the ending index.
        /// </summary>
        int Aux_Traverse(int starting_index,
                         int end_index,
                         Action<int, int, int> action) {

            int ii = starting_index;
            int xx, yy;

            do {
                xx = m_traverse_order[ii].First;
                yy = m_traverse_order[ii].Second;
                action(ii, xx, yy);
            } while (++ii < end_index);

            return end_index;
        }
    }
}