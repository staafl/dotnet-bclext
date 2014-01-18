using System;
using System.Diagnostics;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    public partial class Our_Date_Time : UserControl
    {
        const string STR_DdMMYYYY = "dd/MM/yyyy";

        DateTime _cached_dt;

        static readonly DateTime def_min_date = new DateTime(1900, 01, 01);
        static readonly DateTime def_max_date = new DateTime(2099, 12, 31);

        public DateTime MinDate {
            get;
            set;
        }
        public DateTime MaxDate {
            get;
            set;
        }

        public int SplitYear {
            get;
            set;
        }



        DateTime value;
        public DateTime Value {
            [DebuggerStepThrough]
            get { return value; }
            set {
                Try_Set(value, false, true);
            }
        }

        bool Try_Set(DateTime value, bool force, bool validate) {

            if (force || value != this.Value) {

                if (validate) {

                    var args = Args.Make(true, (Pair<DateTime>)Pair.Make(value, this.value));

                    Value_Checking.Raise<bool, Pair<DateTime>>(this, args);

                    if (!args.Mut) {

                        Reject_Value(null, value);

                        // ValueRejected.Raise(this, Args.Make(value));

                        return false;

                    }

                    Value_Accepted.Raise(this, Args.Make_RO(value));

                }

                //    |----------------------  |--------|
                //    V                     |  V        |
                var old_value = H.Set(ref this.value, value);

                if (old_value != value)
                    Value_Changed.Raise(this, Args.Make_RO(new Pair<DateTime>(value, old_value)));

            }

            Display_Value();

            _cached_dt = value;

            return true;
        }

        public bool Refresh_Value() {

            return Try_Set(true);

        }


    }
}