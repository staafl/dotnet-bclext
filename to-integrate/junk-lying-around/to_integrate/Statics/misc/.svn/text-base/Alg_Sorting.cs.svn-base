
using System;
using System.Collections.Generic;

using System.Linq;


namespace Fairweather.Service
{
    public static partial class Algorithms
    {
        /*       Mergesort        */

        static void Fast_Merge<T>(T[] array,
                                  int left,
                                  int middle,
                                  int right,
                                  T[] left_buf,
                                  IComparer<T> cmp) {

            int left_cnt = middle - left;
            Array.Copy(array, left, left_buf, 0, left_cnt);

            // We are treating the (middle, right] slice of the original array as 
            // an additional buffer
            int pos = left;
            int left_head = 0;
            int right_head = middle;

            while ((left_head < left_cnt) && (right_head < right)) {

                var left_item = left_buf[left_head];
                var right_item = array[right_head];

                if (cmp.Compare(left_item, right_item) != 1) {
                    array[pos] = left_item;
                    ++left_head;

                }
                else {
                    array[pos] = right_item;
                    ++right_head;
                }

                ++pos;
            }

            while (left_head < left_cnt)
                array[pos++] = left_buf[left_head++];

        }

        static void Merge_Sort_Inner<T>(T[] array,
                                        int left,
                                        int right,
                                        T[] buffer,
                                        IComparer<T> cmp) {

            int cnt = right - left;

            #region trivial cases

            if (cnt == 1) {
                return;
            }
            else if (cnt == 2) {


                if (cmp.Compare(array[left], array[left + 1]) == 1) {

                    var temp = array[left];
                    array[left] = array[left + 1];
                    array[left + 1] = temp;

                }
                return;
            }
            else if (cnt == 3) {

                bool fst = cmp.Compare(array[left], array[left + 1]) != 1;
                bool sec = cmp.Compare(array[left + 1], array[left + 2]) != 1;

                if (fst && sec) //  x <= y <= z
                    return;

                if (!fst && !sec) {      // x > y > z
                    var temp = array[left];
                    array[left] = array[left + 2];
                    array[left + 2] = temp;
                    return;
                }

                if (fst) {     // x <= y > z

                    var temp = array[left + 1];
                    array[left + 1] = array[left + 2];
                    array[left + 2] = temp;
                    return;

                }

                // x > y <= z
                {
                    var temp = array[left];
                    array[left] = array[left + 1];
                    array[left + 1] = temp;
                    return;
                }
            }
            #endregion

            var middle = (right + left) / 2;

            Merge_Sort_Inner(array, left, middle, buffer, cmp);
            Merge_Sort_Inner(array, middle, right, buffer, cmp);

            Fast_Merge(array, left, middle, right, buffer, cmp);
        }

        public static void Merge_Sort<T>(T[] array) {


            var cmp = Comparer<T>.Default;
            Merge_Sort(array, cmp);

        }

        public static void Merge_Sort<T>(T[] array, IComparer<T> cmp) {

            int cnt = array.Length;

            if (cnt == 0 || cnt == 1)
                return;

            if (typeof(T) == typeof(int)) {
#if UNSAFE
                Algorithms_Experimental.Merge_Sort_Bottom_Up(array as int[]);
#else
                Algorithms_Nongeneric.Merge_Sort(array as int[]);
#endif
            }


            T[] buffer = new T[(cnt + 1) / 2];



            Merge_Sort_Inner(array, 0, cnt, buffer, cmp);


        }


        /*       Heapsort        */

        public static void Heap_Sort<T>(T[] array, IComparer<T> cmp) where T : struct{

            int cnt = array.Length;

            if (cnt == 0 || cnt == 1)
                return;

            if (cmp == null)
                cmp = Comparer<T>.Default;

            var heap = new Heap<T>(array, true, (fst, sec) => cmp.Compare(fst, sec));

            for (int ii = 0; ii < cnt; ++ii) {
                array[ii] = heap.Top;
                heap.Remove_Top();
            }



        }


        /*       Insertion sort        */


        static public void Insert_Sort<T>(T[] array, IComparer<T> comparer) {

            int cnt = array.Length;

            if (cnt == 1 || cnt == 0)
                return;

            for (int ii = 1; ii < cnt; ++ii) {

                var elem = array[ii];

                int jj;
                for (jj = ii - 1; jj >= 0; --jj) {

                    if (comparer.Compare(array[jj], elem) != 1)
                        break;

                    array[jj + 1] = array[jj];
                }

                array[jj + 1] = elem;
            }

        }

        static public void Insert_Sort<T>(T[] array) {

            if (typeof(T) == typeof(int))
                Algorithms_Nongeneric.Insert_Sort(array as int[]);

            var cmp = Comparer<T>.Default;

            Insert_Sort(array, cmp);

        }


        /*       Distribution sort        */

        /*       "values" is assumed to be sorted        */

        static public void Dist_Sort<T>(T[] array, List<T> values) {

            int cnt = array.Length;

            int range = values.Count;

            var dict = new Dictionary<T, int>(range);

            for (int ii = 0; ii < cnt; ++ii) {

                (dict.Values.Sum() == ii).Throw_If_False();

                var elem = array[ii];

                int count;
                if (!dict.TryGetValue(elem, out count))
                    count = 0;

                ++count;
                dict[elem] = count;


                /*       Invariant: The sum of all values in dict        */
                /*       and the remaining number of elements in array        */
                /*       (array(ii,cnt)) is constant        */


            }

            int position = 0;

            foreach(var value in values){

                int count;
                if (!dict.TryGetValue(value, out count))
                    continue;

                for (int jj = 0; jj < count; ++jj) {
                    array[position] = value;
                    ++position;
                }

            }


        }


        // Links:
        // http://www.tools-of-computing.com/tc/CS/Sorts/bitonic_sort.htm
        // http://www.csse.monash.edu.au/~lloyd/tildeAlgDS/Sort/Radix/

        public class Comparison_Counter<T> : IComparer<T>, IDisposable
        {
            static public Comparison_Counter<T> Get_Comparer() {
                var ret = new Comparison_Counter<T>(Comparer<T>.Default);
                return ret;
            }
            static public Comparison_Counter<T> Get_Comparer(string format) {

                string temp = format.spf(0);

                var ret = new Comparison_Counter<T>(Comparer<T>.Default, format);
                return ret;
            }

            readonly IComparer<T> rd_base;
            int m_count;
            readonly string m_format = "{0}";

            public int Count {
                get { return m_count; }
            }

            public int Compare(T left, T right) {

                ++m_count;
                return rd_base.Compare(left, right);

            }

            Comparison_Counter(IComparer<T> @base, string format) {
                rd_base = @base;
                m_format = format;

            }
            Comparison_Counter(IComparer<T> @base)
                : this(@base, "{0}") {
            }

            public void Dispose() {

                Console.WriteLine(m_format.spf(m_count));

            }
        }





    }
}
