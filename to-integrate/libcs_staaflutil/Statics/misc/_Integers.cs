using System.Collections.Generic;

namespace Fairweather.Service
{
    public static class Integers
    {
        // Primes...
        // 
        

        static public IEnumerable<int> From(int first) {

            return first.Repeat(ii => ++ii, (ii) => true);

        }

        /// <summary>
        /// Inclusive of 'last'!
        /// </summary>
        /// <param name="first"></param>
        /// <returns></returns>
        static public IEnumerable<int> 
        From_To(int first, int last) {

            for (int ii = first; ii <= last; ++ii)
                yield return ii;

        }

    }
}
