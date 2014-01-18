using System;
using System.Windows.Forms;
using System.ComponentModel;

using Fairweather.Service;


namespace Common.Controls
{
    public partial class Our_Date_Time : UserControl
    {
        protected virtual void OnCalendarShown(EventArgs e) {

            Calendar_Shown.Raise(this, e);

        }

        protected virtual void OnCalendarHidden(EventArgs e) {

            if (value != _cached_dt)
                Value_Accepted.Raise(this, EventArgs.Empty);

            _cached_dt = value;

            Calendar_Hidden.Raise(this, e);

        }



        protected override void OnEnter(EventArgs e) {

            if (!cal.Visible)
                _cached_dt = value;

            MethodInvoker act = () =>
            {
                mtbx.Focus();
                mtbx.SelectAll();
            };

            BeginInvoke(act);

            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e) {

            Try_Set(false);

            base.OnLeave(e);

        }


        bool Try_Set(bool force) {

            var text = mtbx.Text;

            DateTime dt_temp;

            var result = Check_Input(text, out dt_temp);

            if (result) {

                return Try_Set(dt_temp, force, true);

            }
            else {

                Reject_Value(text, null);

                return false;

            }

        }

        void Reject_Value(string text, DateTime? null_dt) {

            Value_Rejected.Raise(this, Args.Make_RO(Pair.Make(text, null_dt)));

            if (!null_dt.HasValue)
                Try_Set(DateTime.Today.Date, true, false);

            Display_Value();

        }

        void Display_Value() {
            Display_Value(this.value);
        }

        void Display_Value(DateTime value) {
            Text = value.ToString("ddMMyyyy");
            Display_Changed.Raise(this);
        }


        protected override void OnKeyDown(KeyEventArgs e) {

            if (e.KeyCode == Keys.F4 && !e.Alt) {
                ShowHideCalendar();
            }

            base.OnKeyDown(e);
        }
    }
}