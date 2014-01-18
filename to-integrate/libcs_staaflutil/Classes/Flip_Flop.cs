using System;
using System.Threading;

namespace Fairweather.Service
{
    /* What a mistake this could have been ... */

    public class Flip_Flop
    {
        const int BEFORE = -1;
        const int WHILE = 0;
        const int AFTER = 1;


        int state = BEFORE;
        readonly Func<bool> m_start;
        readonly Func<bool> m_until;

        public Flip_Flop(Func<bool> start, Func<bool> until) {
            m_start = start;
            m_until = until;

        }

        public Flip_Flop Reset() {
            return new Flip_Flop(m_start, m_until);
        }

        public static implicit operator bool(Flip_Flop flip) {

            var seen = flip.state;

            if (seen == AFTER)
                return false;

            if (seen == BEFORE) {
                if (!flip.m_start())
                    return false;

                Interlocked.Exchange(ref flip.state, WHILE);
                return true;
            }

            if (seen == WHILE) {

                if (flip.m_until()) {
                    Interlocked.Exchange(ref flip.state, AFTER);
                    return false;
                }

                return true;
            }

            true.tift();

            return false;

        }
    }
}