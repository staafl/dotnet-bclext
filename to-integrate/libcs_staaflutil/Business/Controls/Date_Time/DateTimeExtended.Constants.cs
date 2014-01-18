using System;
using System.Diagnostics;

using EventHandler = System.EventHandler<System.EventArgs>;

using Fairweather.Service;

namespace Common.Controls
{
    public partial class Our_Date_Time
    {
        public event Handler_RO<Pair<string, DateTime?>> Value_Rejected;
        public event Handler<bool, Pair<DateTime>> Value_Checking;
        public event Handler_RO<Pair<DateTime>> Value_Accepted;

        public event Handler_RO<Pair<DateTime>> Value_Changed;

        public event EventHandler Calendar_Shown;
        public event EventHandler Calendar_Hidden;
        public event EventHandler Display_Changed;
    }
}
