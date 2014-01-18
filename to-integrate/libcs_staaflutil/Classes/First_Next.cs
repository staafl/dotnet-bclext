using System;
using System.Threading;

namespace Fairweather.Service
{
    public class First_Next
    {
        const int BEFORE = -1;
        const int WHILE = 0;
        const int AFTER = 1;


        int state = BEFORE;
        readonly Func<bool> m_first;
        readonly Func<bool> m_next;

        public First_Next(Func<bool> first, Func<bool> next) {
            m_first = first;
            m_next = next;

        }

        public First_Next Reset() {
            return new First_Next(m_first, m_next);
        }

        public static implicit operator bool(First_Next fn) {

            var seen = fn.state;

            if (seen == AFTER)
                return false;

            if (seen == BEFORE) {
                if (!fn.m_first()) {
                    Interlocked.Exchange(ref fn.state, AFTER);
                    return false;
                }

                Interlocked.Exchange(ref fn.state, WHILE);
                return true;
            }

            if (seen == WHILE) {

                if (!fn.m_next()) {
                    Interlocked.Exchange(ref fn.state, AFTER);
                    return false;
                }

                return true;
            }

            true.tift();

            return false;

        }
    }
}
