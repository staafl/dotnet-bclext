
using System;
using System.Collections.Generic;
using System.Diagnostics;

class Scraps
{
    /*       Mergesort with new buffer allocated during every pass        */

    [Conditional("NEVER")]
    public static void Merge_Sort_Unbuffered<T>(T[] array) {

        int cnt = array.Length;

        if (cnt == 0 || cnt == 1)
            return;


        Merge_Sort_Inner(array, 0, cnt);
    }

    [Conditional("NEVER")]
    static void Fast_Merge<T>(T[] array, int left, int middle, int right) {

        int left_cnt = middle - left;
        T[] left_buf = new T[left_cnt];
        Array.Copy(array, left, left_buf, 0, left_cnt);

        // We are treating the (middle, right] slice of the original array as 
        // an additional buffer
        int pos = left;
        int left_head = 0;
        int right_head = middle;

        var cmp = Comparer<T>.Default;

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

    /*               */


    [Conditional("NEVER")]
    static void In_Place_Merge<T>(T[] array, int left, int middle, int right) {

        int left_cnt = middle - left;
        int left_head = left;
        int right_head = middle;
        int pos = left;

        var cmp = Comparer<T>.Default;

        while ((left_head < left_cnt) && (right_head < right)) {

            var left_item = array[left_head];
            var right_item = array[right_head];

            if (cmp.Compare(left_item, right_item) != 1) {
                ++left_head;
            }
            else {
                for (int ii = right_head; ii > pos; --ii) {
                    array[ii] = array[ii - 1];
                }
                ++left_head;
                ++left_cnt;
                ++right_head;
                array[pos] = right_item;
            }

            ++pos;
        }

    }

    [Conditional("NEVER")]
    static void Merge_Sort_Inner<T>(T[] array, int left, int right) {

        int cnt = right - left;

        if (cnt == 1) {
            return;
        }
        if (cnt == 2) {

            var cmp = Comparer<T>.Default;

            if (cmp.Compare(array[left], array[left + 1]) == 1) {

                var temp = array[left];
                array[left] = array[left + 1];
                array[left + 1] = temp;

            }
            return;
        }

        var middle = (right + left) / 2;

        Merge_Sort_Inner(array, left, middle);
        Merge_Sort_Inner(array, middle, right);

        Fast_Merge(array, left, middle, right);
    }

    /*       Mergesort with explicit queue        */


    [Conditional("NEVER")]
    public static void Merge_Sort_Bottom_Up_Queue<T>(T[] array) {



        int cnt = array.Length;

        if (cnt == 0 || cnt == 1)
            return;

        T[] buffer = new T[cnt];

        var queue = new Queue<int>(cnt);
        for (int ii = 0; ii < cnt; ++ii)
            queue.Enqueue(1);

        int current;
        int merges;
        int queue_cnt;



        do {
            current = 0;
            queue_cnt = queue.Count;

            merges = queue_cnt / 2;

            for (int ii = 0; ii < merges; ++ii) {

                var fst = queue.Dequeue();
                var sec = queue.Dequeue();
                queue.Enqueue(fst + sec);

                int middle = current + fst;
                int next = current + fst + sec;

                // Fast_Merge(array, current, middle, next, buffer);

                current = next;

            }

            if (queue_cnt % 2 == 1) {

                queue.Enqueue(queue.Dequeue());

            }

        }
        while (queue_cnt > 2);

        return;
    }

}