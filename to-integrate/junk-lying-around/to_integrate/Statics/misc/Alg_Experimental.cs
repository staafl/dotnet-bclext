


namespace Fairweather.Service
{
    public static partial class Algorithms_Experimental
    {

        #region Bottom-up merge-sort with unsafe buffer
#if UNSAFE

        unsafe static void Fast_Merge(int[] array,
                                      int left,
                                      int middle,
                                      int right,
                                      int* left_buf) {

            int left_cnt = middle - left;
            for (int ii = 0; ii < left_cnt; ++ii) {
                left_buf[ii] = array[ii + left];
            }

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

        unsafe public static void Merge_Sort_Bottom_Up(int[] array) {

            int cnt = array.Length;

            if (cnt == 0 || cnt == 1)
                return;

            int* buffer = stackalloc int[cnt];

            int cnt_2 = cnt / 2;

            int[] queue1 = new int[cnt_2];

            int ii;
            for (ii = 0; ii < cnt_2; ++ii) {

                int ii_2 = 2 * ii;
                if (array[ii_2] > array[ii_2 + 1]) {

                    int temp = array[ii_2];
                    array[ii_2] = array[ii_2 + 1];
                    array[ii_2 + 1] = temp;

                }

                queue1[ii] = 2;
            }

            if (cnt % 2 == 1)
                queue1[ii + 1] = 1;

            int queue_cnt = cnt_2;

            do {

                bool remainder = (queue_cnt & 1) == 1;
                int temp_cnt = queue_cnt;
                int queue_ptr1 = 0;
                int queue_ptr2 = 0;


                int current = 0;
                int merges = queue_cnt / 2;

                for (int jj = 0; jj < merges; ++jj) {

                    var fst = queue1[queue_ptr1++];
                    var sec = queue1[queue_ptr1++];

                    queue1[queue_ptr2++] = fst + sec;
                    --queue_cnt;


                    int middle = current + fst;
                    int next = current + fst + sec;

                    Fast_Merge(array, current, middle, next, buffer);

                    current = next;

                }

                if (remainder) {

                    queue1[queue_ptr2++] = queue1[queue_ptr1];

                }

            }
            while (queue_cnt >= 2);

            return;
        }
#endif // UNSAFE
        #endregion

    }
}