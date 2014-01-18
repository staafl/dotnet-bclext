using System.Collections.Generic;

namespace Fairweather.Service
{
    // HACK!
    public static class R
    {

        static System.Random m_rand;

        static R() {

            Reseed();

        }

        // ****************************



        public static IEnumerable<int>
            Numbers() {

            while (true)
                yield return m_rand.Next();

        }

        public static IEnumerable<int>
            Numbers(int count) {

            for (int ii = 0; ii < count; ++ii)
                yield return m_rand.Next();

        }


        // ****************************


        //public static int Random_In(Range<int> interval) {

        //    var ret = m_rand.Next(interval.Lower, interval.Upper + 1);

        //    return ret;

        //}

        //public static int Random_In(params Range<int>[] intervals) {

        //    intervals.tifn();
        //    intervals.Is_Empty().tift();

        //    int cnt = intervals.Length;

        //    int num = m_rand.Next(cnt);

        //    var interval = intervals[num];

        //    int ret = Random_In(interval);

        //    return ret;

        //}

        public static void Reseed(int seed) {

            m_rand = new System.Random(seed);

        }

        /// <summary> Automatically called the first time the class is used.
        /// </summary>
        public static void Reseed() {

            m_rand = new System.Random();

        }

        /// <summary> 0 .le. return .lt Int32.MaxValue
        /// </summary>
        /// <returns></returns>
        public static int Random_Int() {

            var ret = m_rand.Next();

            return ret;

        }

        /// <summary> 0 .le. return .lt. max
        /// </summary>
        public static int Random_Int(int max) {

            var ret = m_rand.Next(max);

            return ret;

        }


        /// <summary> min .le. return .lt. max
        /// </summary>
        public static int Random_Int(int min, int max) {

            var ret = m_rand.Next(min, max);

            return ret;

        }


        public static int[] Permutation(int start, int end, bool cyclic) {

            // http://en.wikipedia.org/wiki/Cyclic_permutation

            (start <= end).tiff();

            if (start == end)
                return new int[] { start };

            int cnt = end - start + 1;
            int[] array = new int[cnt];
            for (int ii = start, jj = 0; ii < end; ++ii, ++jj) {
                array[jj] = ii;
            }

            Shuffle(array, cyclic);

            return array;
        }

        public static void Shuffle<T>(T[] array, bool cylic) {

            array.tifn();
            int cnt = array.Length;

            if (cnt <= 1)
                return;

            //  http://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
            //  cylic ->  Sattolo shuffle
            //  !cyclic -> Fisher–Yates shuffle
            int n = cylic ? cnt - 2 : cnt - 1;

            for (; n > 1; --n) {

                int swap_with = Random_Int(0, n + 1);

                if (n != swap_with) {
                    var temp = array[swap_with];
                    array[swap_with] = array[n];
                    array[n] = temp;
                }


            }


        }

        public static int[] Random_Array(int length, bool allow_negs) {

            (length >= 0).tiff();

            if (length == 0)
                return new int[0];

            int[] ret = new int[length];
            for (int ii = 0; ii < length; ++ii) {

                int rand = Random_Int();
                if (allow_negs && Random_Bool()) {
                    rand *= -1;
                    --rand;
                }

                ret[ii] = rand;

            }

            return ret;

        }

        public static bool Random_Bool() {

            // Make sure it's even
            int end = int.MaxValue - (int.MaxValue & 1);

            int rand = Random_Int(end);

            bool ret = ((rand & 1) == 1);

            return ret;

        }

        ///*       Random Global        */

        //const int def_min = 1;
        //const int def_max = 3;
        //static readonly Range<int>[] def_interval = { Interval.ASCII_Lowercase, Interval.ASCII_Uppercase };

        ///// <summary> Returns a pseudorandom string of ASCII letters </summary>
        //public static string Random_String(int len) {

        //    var ret = Random_String(len, def_interval);

        //    return ret;
        //}

        //public static string Random_String(params Range<int>[] classes) {

        //    var ret = Random_String(def_min, def_max, classes);

        //    return ret;

        //}

        //public static string Random_String() {

        //    var ret = Random_String(def_min, def_max, def_interval);

        //    return ret;

        //}

        ///// <summary> Returns a pseudorandom string of ASCII characters with a length between min_len and max_len </summary>
        //public static string Random_String(int min_len, int max_len, params Range<int>[] classes) {

        //    (min_len < 1).tift();
        //    (max_len < min_len).tift();

        //    int len;

        //    if (min_len == max_len) {

        //        len = min_len;

        //    }
        //    else {

        //        len = Get_Randomized_String_Length(min_len, max_len, classes);

        //    }

        //    var ret = Random_String(len, classes);

        //    return ret;

        //}

        ///// <summary> Returns a pseudorandom string of ASCII characters </summary>
        //public static string Random_String(int len, params Range<int>[] classes) {

        //    classes.Is_Empty().tift();

        //    var chars = new char[len];

        //    int[] distribution = classes.Select(_class => _class.Length())
        //                                .ToArray();

        //    int total_length = distribution.Sum();


        //    for (int ii = 0; ii < len; ++ii) {

        //        // the intervals should be weighed
        //        var index = Random_Weighed_(distribution, total_length);

        //        var interval = classes[index];

        //        chars[ii] = (char)Random_In(interval);

        //    }

        //    string ret = new string(chars);

        //    return ret;
        //}

        ///*       Test        */


        //static int Get_Randomized_String_Length(int min_len, int max_len, Range<int>[] classes) {

        //    // We are selecting a member of the sequence S1, S2, ... , where SX is the set of
        //    // X-character strings composed of characters inside from the given intervals.
        //    // 
        //    // We are assuming that the intervals are non-overlapping.

        //    int total_length = classes.Sum(_class => _class.Length());

        //    int strings_with_min = total_length.Pow(min_len);
        //    int strings_with_max = total_length.Pow(max_len);

        //    int seed = m_rand.Next(strings_with_min,    // a representative string from SX
        //                           strings_with_max);


        //    int ii = 0;

        //    /*       Sum of Geometric Progression        */

        //    int sum = 1;
        //    int power = 1;
        //    while (true) {

        //        if (sum >= seed)
        //            break;

        //        sum += power;
        //        power *= total_length;

        //        ++ii;

        //    }

        //    return ii - 1;

        //}

        ////**************************

        //public static T Random_Element<T>(this T[] array) {

        //    var index = m_rand.Next(0, array.Length);

        //    var ret = array[index];

        //    return ret;

        //}

        //public static int Random_Weighed(this int[] distribution) {

        //    int total_length = distribution.Sum();

        //    var ret = Random_Weighed_(distribution, total_length);

        //    return ret;


        //}

        //static int Random_Weighed_(this int[] distribution, int total_length) {

        //    checked {

        //        int seed = m_rand.Next(total_length);

        //        int ii;

        //        for (ii = 0; ii < distribution.Length; ++ii) {

        //            var item = distribution[ii];
        //            if (seed < item)
        //                break;

        //            seed -= item;

        //        }

        //        return ii;
        //    }

        //}

        //public static T Random<T>() {
        //    throw new NotImplementedException();
        //}

    }
}