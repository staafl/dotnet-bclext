using System;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    public partial class Our_Date_Time
    {


        void Handle_Calendar_Date_Rejected() {
            HideCalendar();
            SelectAll();
        }

        void Handle_Calendar_Date_Confirmed() {

            HideCalendar();
            SelectAll();
            // ProcessTabKey(true);
        }



        void button_Click(object sender, EventArgs e) {

            if (!bf_but_click)
                ShowHideCalendar();
        }

        public bool CalendarVisible {
            get { return cal.Visible; }
            set {
                if (value == cal.Visible)
                    return;

                if (value)
                    ShowCalendar();

                else
                    HideCalendar();
            }
        }

        public void Undo() {

            value = _cached_dt;
            Display_Value();
        }

        Block bf_show_hide_calendar = new Block();
        protected void ShowHideCalendar() {

            if (bf_show_hide_calendar)
                return;

            using (bf_show_hide_calendar.Lock()) {

                if (cal.Visible)
                    HideCalendar();

                else
                    ShowCalendar();
            }
        }
        protected void ShowCalendar() {

            DateTime dt_temp;

            string text = mtbx.Text;

            var check = Check_Input(text, out dt_temp);

            if (check)
                cal.Value = dt_temp;

            if (!b_cal_init) {

                var form = this.FindForm();

                if (form == null)
                    throw new InvalidOperationException("Our_DateTime: Unable to find top level form.");

                form.Controls.Add(cal);

                if (form is IControlHost)
                    (form as IControlHost).MouseClickedOnScreen += HandleMouseClickedOnScreen;

                b_cal_init = true;

            }

            var rect1 = this.Bounds_On_Screen();
            var rect2 = cal.Bounds;
            var rect3 = rect2.Align_Vertices_In_Container(rect1, this.TopLevelControl.Bounds_On_Screen()
                                                                                          .Expand(-10)).Value;

            cal.Location = this.TopLevelControl.PointToClient(rect3.Location);

            cal.Visible = true;
            cal.Focus();
            cal.BringToFront();

            OnCalendarShown(EventArgs.Empty);
        }

        protected void HideCalendar() {

            // a control should never leave focus wander
            
            mtbx.Select_Focus();
            cal.Visible = false;

            OnCalendarHidden(EventArgs.Empty);
        }
    }
}