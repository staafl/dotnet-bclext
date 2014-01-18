using System;
using System.Collections.Generic;

using System.Linq;


namespace Fairweather.Service
{
    public static class Algorithms_Nongeneric
    {
        static void 
        Fast_Merge(int[] array, int left, int middle, int right, int[] left_buf) {

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

                if (left_item <= right_item) {
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

        static void Merge_Sort_Inner(int[] array, int left, int right, int[] buffer) {

            int cnt = right - left;

            #region trivial cases

            if (cnt < 4) {
                if (cnt == 1) {
                    return;
                }
                else if (cnt == 2) {

                    if (array[left] > array[left + 1]) {

                        var temp = array[left];
                        array[left] = array[left + 1];
                        array[left + 1] = temp;

                    }
                    return;
                }

                bool fst = array[left] <= array[left + 1];
                bool sec = array[left + 1] <= array[left + 2];

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

            Merge_Sort_Inner(array, left, middle, buffer);
            Merge_Sort_Inner(array, middle, right, buffer);

            Fast_Merge(array, left, middle, right, buffer);
        }

        public static void Merge_Sort(int[] array) {

            int cnt = array.Length;

            if (cnt == 0 || cnt == 1)
                return;

            var buffer = new int[(cnt + 1) / 2];

            Merge_Sort_Inner(array, 0, cnt, buffer);

        }

        static public void Insert_Sort(int[] array) {

            int cnt = array.Length;

            if (cnt == 1 || cnt == 0)
                return;

            for (int ii = 1; ii < cnt; ++ii) {
                var elem = array[ii];

                int jj;
                for (jj = ii - 1; jj >= 0; --jj) {

                    if (array[jj] < elem)
                        break;

                    array[jj + 1] = array[jj];
                }

                array[jj + 1] = elem;
            }
        }

        // http://www.inf.fh-flensburg.de/lang/algorithmen/sortieren/shell/shellen.htm
        static readonly int[] gaps = {1391376, 463792, 198768, 86961, 33936, 13776, 4592,
                                      1968, 861, 336, 112, 48, 21, 7, 3, 1};


        static int Get_Gap_Index(int cnt) {

            int cnt_2 = cnt / 3;


            //for (int ii = gaps.Length - 1; ii >= 0; --ii) {
            for (int ii = 0; ii <= gaps.Length; ++ii) {
                //
                if (cnt_2 >= gaps[ii])
                    return ii;

            }

            throw new ArgumentOutOfRangeException();

        }

        static public void Shell_Sort(int[] array) {

            int cnt = array.Length;
            int last_index = cnt - 1;

            int gap_index = Get_Gap_Index(cnt);

            //for (int ii = gap_index; ii >= 0; --ii) {
            for (int ii = gap_index; ii < gaps.Length; ++ii) {

                int gap_length = gaps[ii];

                for (int index = gap_length; index < cnt; index += gap_length) {

                    var elem = array[index];

                    int jj;
                    for (jj = index - gap_length;
                         jj >= 0;
                         jj -= gap_length) {

                        if (array[jj] <= elem) {

                            array[jj + gap_length] = elem;
                            break;
                            // elem = array[jj]; // <-- this is useless

                        }
                        else {

                            array[jj + gap_length] = array[jj];

                        }

                    }
                    array[jj + gap_length] = elem;
                }
            }
        }

        /*       Bitonic sort        */

        static public void Bitonic_Sort(int[] array) {

            int cnt = array.Length;
            if (cnt == 0)
                return;

            Sort(array, 0, cnt, true);

        }

        static void Sort(int[] array, int m, int n, bool up) {

            if (n == 1)
                return;

            int n_2 = n >> 1;

            Sort(array, m, n_2, true);
            Sort(array, m + n_2, n_2, false);
            Merge(array, m, n_2, up);

        }

        static void Merge(int[] array, int m, int n, bool up) {

            if (n == 0)
                return;

            for (int ii = 0; ii < n; ++ii) {

                int ind1 = m + ii;
                int ind2 = m + n + ii;
                var elem1 = array[ind1];
                var elem2 = array[ind2];

                if (up ? elem1 > elem2 : elem1 < elem2) {

                    array.Swap(ind1, ind2);

                }

            }
            int n_2 = n >> 1;

            Merge(array, m, n_2, up);
            Merge(array, m + n, n_2, up);


        }

        static public void Bitonic_Sort_2(int[] array) {

            int cnt = array.Length;

            if (cnt == 0 || cnt == 1)
                return;

            /*       For all powers of two kk (<cnt)        */
            // [10, 100, 1000, ...]
            for (int kk = 2; kk < cnt; kk <<= 1) {

                /*       For all powers of two jj (<kk)  [[1], [10, 1], [100, 10, 1], ...] */
                for (int jj = kk >> 1; jj > 0; jj >>= 1) {

                    /*       For every element in the sequence ii        */
                    for (int ii = 0; ii < cnt; ++ii) {

                        int xor = ii ^ jj; // flip the log2(jj)-th bit              
                        if (xor > ii)      // make sure we process a pair once      // <-- flipping this conditional inverts the sorting direction
                            continue;      // xor and ii differ in a single bit,    // it also establishes that xor is a valid index
                        // which is 0 in xor and 1 in ii         // since ii e [0, cnt)

                        (ii - xor == jj).Throw_If_False();

                        var up = (ii & kk) == 0; // <-- the direction we're sorting
                        var elem1 = array[ii];   // depends on the log2(kk)-th bit 
                        var elem2 = array[xor];  // in both ii and xor

                        if (up ? elem1 < elem2 : elem2 < elem1) {

                            array.Swap(ii, xor);

                        }

                    }

                }

            }

        }

        /*       Distribution sort        */

        static public void Dist_Sort(int[] array) {

            int cnt = array.Length;
            int min = array.Min();
            int max = array.Max();

            int range = max - min + 1;
            var dict = new Dictionary<int, int>(range);

            for (int ii = 0; ii < cnt; ++ii) {

                var elem = array[ii];

                int count;
                if (!dict.TryGetValue(elem, out count))
                    count = 0;

                ++count;
                dict[elem] = count;


            }

            int position = 0;
            for (int ii = 0; ii < range; ++ii) {

                int count;
                if (!dict.TryGetValue(ii, out count))
                    continue;

                for (int jj = 0; jj < count; ++jj) {
                    array[position] = ii;
                    ++position;
                }

            }


        }

        /*       Radix sort        */

        static Func<int, int, int> LSD_func = (number, position) =>
            {
                #region
                //*
                ++position;
                var ret = A.Nth_Digit(number, -position, 10);

                return ret;

                /*/
                for (int ii = 0; ii < position; ++ii) {
                    number /= 10;
                }

                return number % 10;
                //*/

                #endregion
            };

        static Func<int, int, int> MSD_func = (number, position) =>
        {
            #region
            //*

            var ret = A.Nth_Digit(number, position, 10);

            return ret;

            /*/
                        int temp_1 = 1;
                        int temp_2 = number;
                        int pow = position.Pow(10);

                        while (temp_2 >= pow) {
                            temp_1 *= 10;
                            temp_2 /= 10;
                        }

                        return (number / temp_1) % 10;
                        //*/

            #endregion
        };
        /*       Assumes array contains positive values        */

        // func(n, i) must return the ith digit in n's decimal representation
        // For LSD the digits are counted from the right, for MSD - from the left
        // if there is no such digit, func should return 10
        static public void Radix_Sort(int[] array,
                                      Func<int, int, int> func) {

            int cnt = array.Length;

            if (cnt == 0 || cnt == 1)
                return;

            if (func == null)
                func = LSD_func;

            int[] temp = new int[cnt];      // scratch space for rearranging the elements
            int[] digits = new int[cnt];    // used for caching results from func
            int[] count = new int[11];      // <-- counts the number of elements with a certain digit 
            // at a specific location

            int max = array.Max();          // get the number of iterations we need to do

            int max_digits = 0;

            while (max > 0) {

                max /= 10;
                ++max_digits;

            }

            /*       For each digit position, pos       */

            for (int pos = 0; pos < max_digits; ++pos) {

                Array.Clear(count, 0, 11);

                for (int ii = 0; ii < cnt; ++ii) {

                    int digit = func(array[ii], pos) + 1;   // get the pos-th digit
                    digits[ii] = digit;
                    ++count[digit];

                }

                /*       Determining the subsequence positions        */

                for (int ii = 1; ii <= 10; ++ii)         // count now represents index ranges in the main
                    count[ii] += count[ii - 1];          // array where each sorted subsequence will be stored
                // [ -1:[0..10], 0:[11..24], 1:[25..50], ... , 9:[143..150] ]

                //*

                for (int ii = cnt - 1; ii >= 0; --ii) { // fill the subsequences
                    int digit = digits[ii];             // digit is the pos-th digit of array[ii]
                    temp[--count[digit]] = array[ii];   // filling from right to left
                }

                /*/ // Alternative : Filling the subsequences from the left to the right

                for (int ii = 10; ii > 0; --ii) {        // shift the counters to the right
                    count[ii] = count[ii - 1];           // to allow us to fill the sequences from the left
                }
                
                count[0] = 0;                            // <-- These come first
                
                for (int ii = 0; ii < cnt; ++ii) {
                    int digit = digits[ii];            
                    temp[count[digit]++] = array[ii];   
                }
                //*/

                Array.Copy(temp, array, cnt);

            }
        }

        static void Swap_If_Less(int[] array, int ind1, int ind2) {

            if (ind1 == ind2)
                return;
            //(ind1 == ind2).Throw_If_True();

            var elem1 = array[ind1];
            var elem2 = array[ind2];

            if (elem1 >= elem2)
                return;

            array[ind1] = elem2;
            array[ind2] = elem1;

        }

        public static void QuickSort(int[] array) {

            int cnt = array.Length;

            if (cnt <= 1)
                return;

            var stack = new Stack<Pair<int>>(A.Log2(cnt));

            stack.Push(new Pair<int>(0, cnt - 1));

            while (!stack.Is_Empty()) {

                var range = stack.Pop();
                int left = range.First;
                int right = range.Second;

                //TODO: Add randomization?
                int median = left + (right - left + 1) / 2;

                Swap_If_Less(array, median, left);   // array[left] <= array[median]
                Swap_If_Less(array, right, left);    // array[left] <= array[right]
                Swap_If_Less(array, right, median);  // array[left] <= array[median] <= array[right]

                int pivot = array[median];

                int low = left;
                int high = right;
                do {
                    // The comparisons must be both non-strict
                    // TODO: investigate
                    // if, say, the first comparison is strict, and the other one is not,
                    // and if the slice has the form xYY..zzzz.., the left pointer will not move
                    while (array[low] < pivot) ++low;
                    // xxxxxxxYYYYzzzzzzz

                    while (array[high] > pivot) --high;

                    //array[low] > pivot,
                    //array[high] < pivot

                    if (low == high) {
                        ++low; --high; // This step is not strictly necessary
                        break;
                    }

                    if (low > high) {
                        break;
                    }

                    // array[high] <= pivot <= array[low]
                    
                    // REMOVE: xor swap - remove
                    array[low] ^= array[high];
                    array[high] ^= array[low];
                    array[low] ^= array[high];
                    // array[low] <= pivot <= array[high]

                    ++low; --high;

                } while (low < high);

                // array[left..high] <= array[low..right]

                // At the end of the above loop low and high will generally fulfill low = high + 2 or low =
                // high + 1. Both cases are fine.

                int l_dist = high - left + 1;
                int r_dist = right - low + 1;

                bool l_smaller = l_dist < r_dist;
                bool l_ins = l_dist < 20;
                bool r_ins = r_dist < 20;

                //l_ins = r_ins = false;
                if(l_ins || r_ins){
               
                     // insertion sort
                     if(l_ins)
                         Insert_Sort_Helper(array, left, high);
                        
                     if(r_ins)
                         Insert_Sort_Helper(array, low, right);
                        
                     if(l_ins && r_ins)
                         continue;

                }

                if (l_smaller && !r_ins)
                    if (r_dist > 1)
                        stack.Push(new Pair<int>(low, right));

                if (!l_ins)
                    if (l_dist > 1)
                        stack.Push(new Pair<int>(left, high));

                if (!l_smaller && !r_ins)
                    if (r_dist > 1)
                        stack.Push(new Pair<int>(low, right));

            }

        }

        static void Insert_Sort_Helper(int[] array, int left, int right) {

            for (int ii = left + 1; ii <= right; ++ii) {

                var elem = array[ii];

                int jj;
                for (jj = ii - 1; jj >= left; --jj) {

                    var elem2 = array[jj];

                    if (elem >= elem2)
                        break;

                    array[jj + 1] = elem2;

                }

                array[jj + 1] = elem;
            }


        }

    }
}




























