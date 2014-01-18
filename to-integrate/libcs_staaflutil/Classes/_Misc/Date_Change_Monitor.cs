using System;
using System.Threading;

using Microsoft.Win32;

using DateEventHandler = System.
EventHandler<Fairweather.Service.Date_Change_Monitor.Date_Changed_Event_Args>;

namespace Fairweather.Service
{
    /*       Untested & probably buggy        */
    //http://msdn.microsoft.com/en-us/library/microsoft.win32.systemevents.timechanged.aspx
    //http://stackoverflow.com/questions/372309/is-there-a-way-to-get-notification-of-date-change-in-c
    public static class Date_Change_Monitor
    {
        const int cst_wakeup_allowance = 1000;
        const int cst_wakeup_resolution = 250;

        static bool observing;

        public static bool Observing {
            get { return Date_Change_Monitor.observing; }
            set {

                if (observing == value)
                    return;


                if (value == false) {

                    timer.Try_Dispose();

                }
                else {

                    Start_Timer();

                }

                observing = value;

            }
        }

        static Timer timer;

        static DateTime last_seen_date;

        static bool b_superseded;
        static bool b_timer_elapsed;

        static public event DateEventHandler Date_Changed;


        static Date_Change_Monitor() {

            SystemEvents.TimeChanged += TimeChanged;

        }


        static void On_Date_Changed(Date_Change_Reason reason) {


            var eargs = new Date_Changed_Event_Args(last_seen_date,
                                                    DateTime.Now,
                                                    reason);

            last_seen_date = DateTime.Now;

            Date_Changed.Raise(null, eargs);


            if (observing)
                Start_Timer();

        }

        static void TimerElapsed(object unused) {

            if (b_timer_elapsed)
                return;

            try {
                b_timer_elapsed = true;

                while (!Is_Date_Changed())
                    Thread.Sleep(cst_wakeup_resolution);

                if (b_superseded)
                    return;

                On_Date_Changed(Date_Change_Reason.Normal_Date_Change);

            }
            finally {

                b_superseded = false;
                b_timer_elapsed = false;

            }

        }

        static void TimeChanged(object sender, EventArgs e) {

            if (!observing)
                return;

            if (!Is_Date_Changed())
                return;

            if (b_timer_elapsed)
                b_superseded = true;

            On_Date_Changed(Date_Change_Reason.User_Changed_Date);

        }

        static public void Start_Timer() {

            last_seen_date = DateTime.Now;

            int due_time = Get_RemainingMS();

            timer.Try_Dispose();

            timer = new Timer(TimerElapsed,
                              null,
                              due_time,
                              Timeout.Infinite);

            observing = true;


        }


        static int Get_RemainingMS() {

            var interval = (DateTime.Now.Next_Day() - DateTime.Now);
            var ret = (int)interval.TotalMilliseconds;
            ret -= cst_wakeup_allowance;

            return ret;

        }
        static bool Is_Date_Changed() {

            int last = last_seen_date.Comparable_Day();
            int today = DateTime.Now.Comparable_Day();

            var ret = last != today;

            return ret;

        }

        public class Date_Changed_Event_Args : EventArgs
        {

            readonly DateTime m_before;
            readonly DateTime m_now;
            readonly Date_Change_Reason m_reason;

            public Date_Change_Reason Reason {
                get { return m_reason; }
            }

            public DateTime Before {
                get { return m_before; }
            }

            public DateTime Now {
                get { return m_now; }
            }

            public Date_Changed_Event_Args(DateTime before,
                                           DateTime now,
                                           Date_Change_Reason reason) {

                m_before = before;
                m_now = now;
                m_reason = reason;
            }

        }

        public enum Date_Change_Reason
        {
            User_Changed_Date = 1,
            Normal_Date_Change = 2,

        }
    }
}
