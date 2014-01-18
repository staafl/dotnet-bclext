using System;
using System.Threading;

namespace Fairweather.Service
{
    public class Once_True
    {
        const int USED = 1;
        const int UNUSED = 0;

        int used = UNUSED;
        readonly Action m_act;

        public Once_True()
            : this(null) {
        }
        public Once_True(Action act) {
            m_act = act;
        }



        public static implicit operator bool(Once_True once) {

            var seen = Interlocked.Exchange(ref once.used, USED);

            if (seen == USED)
                return false;

            if (seen != UNUSED)
                true.Throw_If_True();

            if (once.m_act != null)
                once.m_act();

            return true;

        }
    }

}