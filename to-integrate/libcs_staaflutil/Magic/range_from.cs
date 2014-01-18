using System;
using System.Collections.Generic;

namespace Fairweather.Service
{
    static partial class Magic {

        static public IEnumerable<int> 
        from(int first) {

            return first.Repeat(ii => ++ii, (ii) => true);

        }

        /// <summary>
        /// Inclusive of 'last'!
        /// </summary>
        /// <param name="first"></param>
        /// <returns></returns>
        static public IEnumerable<int>
        range(int first, int last) {

            for (int ii = first; ii <= last; ++ii)
                yield return ii;

        }
    }
}
