using System;
using System.Collections.Generic;
using System.Linq;

namespace Fairweather.Service
{
    static partial class Extensions
    {

        // ****************************

        static public void
        Clear<T>(this T[] array) {
            Array.Clear(array, 0, array.Length);
        }

   
        // ****************************


        public static T[]
        Concat_Arrays<T>(params T[][] arrays) {

            T[] ret = new T[arrays.Sum(_arr => _arr.Length)];

            int pos = 0;
            foreach (var arr in arrays) {

                Array.Copy(arr, 0, ret, pos, arr.Length);
                pos += arr.Length;
            }

            return ret;
        }

        public static IEnumerable<T[]>
        Iterate<T>(this T[,] array, bool row_major) {

            int cnt = row_major ? array.GetLength(0) : // represents the number of rows
                                  array.GetLength(1); // in the array

            for (int row = 0; row < cnt; ++row)
                yield return Array_Row(array, row, row_major);

        }

        public static T[][]
        To_Jagged<T>(this T[,] array, bool invert) {

            int cnt_1 = array.GetLength(invert ? 1 : 0);

            int cnt_2 = array.GetLength(invert ? 0 : 1);


            T[][] ret = new T[cnt_1][];
            for (int ii = 0; ii < cnt_1; ++ii)
                ret[ii] = Array_Row<T>(array, ii, invert);

            return ret;
        }

        public static T[]
        Array_Column<T>(this T[,] array, int col_index, bool row_major) {

            T[] ret = array.Array_Row(col_index, !row_major);

            return ret;
        }

        public static T[]
        Array_Row<T>(this T[,] array, int row_index, bool row_major) {

            int cnt = row_major ? array.GetLength(1) : // represents the number of columns
                                  array.GetLength(0);  // in the array

            T[] ret = new T[cnt];

            if (row_major)
                for (int col = 0; col < cnt; ++col)
                    ret[col] = array[row_index, col];
            else
                for (int col = 0; col < cnt; ++col)
                    ret[col] = array[col, row_index];

            return ret;
        }

        /* TODO */
        /* Benchmark against the LINQ method */
        /* Will this even be called? */
        public static bool
        Contains<T>(this T[] array, T value) {

            int index = Array.IndexOf(array, value);

            bool ret = index != -1;

            return ret;

        }

        public static void
        Swap<T>(this T[] array, int ind1, int ind2) {

            if (ind1 == ind2)
                return;

            var tmp = array[ind1];
            array[ind1] = array[ind2];
            array[ind2] = tmp;

        }

        // Thorougly untested


        static public T[]
        Slice<T>(this T[] arr) {

            int cnt = arr.Length;
            var ret = new T[cnt];
            Array.Copy(arr, ret, cnt);

            return ret;

        }

        public static T[]
        Slice<T>(this T[] array, int start) {

            return array.Slice(start, array.Length - 1);

        }

        public static T[]
        Slice<T>(this T[] array, int start, int end) {

            array.tifn();

            (start >= 0).tiff();
            (start <= end).tiff();

            (end < array.Length).tiff();

            int cnt = end - start + 1;
            var ret = new T[cnt];

            Array.Copy(array, start, ret, 0, cnt);

            return ret;
        }

        static public void
        Insert<T>(this T[] array, T elem) {
            Insert(array, elem, array.Length);
        }

        public static void
        Insert<T>(this T[] array, T elem, int index) {

            var cnt = array.Length;

            (index <= cnt).tiff();

            Array.Resize(ref array, cnt + 1);

            if (index < cnt)
                array.Copy(true, array, index, index + 1);

            array[index] = elem;

        }


        public static T[]
        Copy<T>(this T[] array) {

            if (array == null)
                return null;
            var ret = new T[array.Length];
            array.CopyTo(ret, 0);
            return ret;
        }

        public static void
        Copy<T>(this T[] arr1, bool from_right, T[] arr2, int start1, int start2) {

            if (from_right) {
                for (int ii = arr1.Length - 1, jj = arr2.Length - 1;
                     ii >= start1; --ii, --jj) {
                    arr2[jj] = arr1[ii];
                }
            }
            else {
                Array.Copy(arr1, start1, arr2, start2, arr1.Length - start1);
            }

        }

        // Static Array Methods

        public static int
        BinarySearch<T>(this T[] array, T value) {
            return Array.BinarySearch(array, value);

        }

        public static int
        BinarySearch<T>(this T[] array, T value, IComparer<T> comparer) {
            return Array.BinarySearch(array, value, comparer);

        }

        public static int
        BinarySearch<T>(this T[] array, int index, int length, object value) {
            return Array.BinarySearch(array, index, length, value);

        }

        public static int
        BinarySearch<T>(this T[] array, int index, int length, T value) {
            return Array.BinarySearch(array, index, length, value);

        }

        public static int
        BinarySearch<T>(this T[] array, int index, int length, T value, IComparer<T> comparer) {
            return Array.BinarySearch(array, index, length, value, comparer);
        }

        public static void
        Clear<T>(this T[] array, int index, int length) {
            Array.Clear(array, index, length);

        }

        public static void
        CCopy<T>(this T[] array,
            int sourceIndex,
            T[] destinationArray,
            int destinationIndex,
            int length) {
            Array.ConstrainedCopy(array, sourceIndex, destinationArray, destinationIndex, length);

        }

        public static TOutput[]
        ConvertAll<TInput, TOutput>(this TInput[] array, Converter<TInput, TOutput> converter) {
            return Array.ConvertAll(array, converter);

        }

        public static void
        Copy<T>(this T[] array,
            T[] destinationArray,
            int length) {
            Array.Copy(array, destinationArray, length);

        }

        public static void
        Copy<T>(this T[] array,
            T[] destinationArray,
            long length) {
            Array.Copy(array, destinationArray, length);

        }

        public static void
        Copy<T>(this T[] array,
            int sourceIndex,
            T[] destinationArray,
            int destinationIndex,
            int length) {
            Array.Copy(array, sourceIndex, destinationArray, destinationIndex, length);
        }

        public static void
        Copy<T>(this T[] array,
                    long sourceIndex,
                    T[] destinationArray,
                    long destinationIndex,
                    long length) {
            Array.Copy(array, sourceIndex, destinationArray, destinationIndex, length);

        }

        public static bool
        Exists<T>(this T[] array, Predicate<T> match) {
            return Array.Exists(array, match);

        }

        public static T
        Find<T>(this T[] array, Predicate<T> match) {
            return Array.Find(array, match);

        }

        public static T[]
        FindAll<T>(this T[] array, Predicate<T> match) {
            return Array.FindAll(array, match);

        }

        public static int
        FindIndex<T>(this T[] array, Predicate<T> match) {
            return Array.FindIndex(array, match);

        }

        public static int
        FindIndex<T>(this T[] array, int startIndex, Predicate<T> match) {
            return Array.FindIndex(array, startIndex, match);

        }

        public static int
        FindIndex<T>(this T[] array, int startIndex, int count, Predicate<T> match) {
            return Array.FindIndex(array, startIndex, count, match);
        }

        public static T
        FindLast<T>(this T[] array, Predicate<T> match) {
            return Array.FindLast(array, match);

        }

        public static int
        FindLastIndex<T>(this T[] array, Predicate<T> match) {
            return Array.FindLastIndex(array, match);

        }

        public static int
        FindLastIndex<T>(this T[] array, int startIndex, Predicate<T> match) {
            return Array.FindLastIndex(array, startIndex, match);

        }

        public static int
        FindLastIndex<T>(this T[] array, int startIndex, int count, Predicate<T> match) {
            return Array.FindLastIndex(array, startIndex, count, match);

        }

        public static void
        ForEach<T>(this T[] array, Action<T> action) {
            Array.ForEach(array, action);

        }

        public static int
        IndexOf<T>(this T[] array, object value) {
            return Array.IndexOf(array, value);

        }

        public static int
        IndexOf<T>(this T[] array, T value) {
            return Array.IndexOf(array, value);
        }

        public static int
        IndexOf<T>(this T[] array, object value, int startIndex) {
            return Array.IndexOf(array, value, startIndex);

        }

        public static int
        IndexOf<T>(this T[] array, T value, int startIndex) {
            return Array.IndexOf(array, value, startIndex);

        }

        public static int
        IndexOf<T>(this T[] array, object value, int startIndex, int count) {
            return Array.IndexOf(array, value, startIndex, count);

        }

        public static int
        IndexOf<T>(this T[] array, T value, int startIndex, int count) {
            return Array.IndexOf(array, value, startIndex, count);

        }

        public static int
        LastIndexOf<T>(this T[] array, object value) {
            return Array.LastIndexOf(array, value);

        }

        public static int
        LastIndexOf<T>(this T[] array, T value) {
            return Array.LastIndexOf(array, value);

        }

        public static int
        LastIndexOf<T>(this T[] array, object value, int startIndex) {
            return Array.LastIndexOf(array, value, startIndex);

        }

        public static int
        LastIndexOf<T>(this T[] array, T value, int startIndex) {
            return Array.LastIndexOf(array, value, startIndex);

        }

        public static int
        LastIndexOf<T>(this T[] array, object value, int startIndex, int count) {
            return Array.LastIndexOf(array, value, startIndex, count);

        }

        public static int
        LastIndexOf<T>(this T[] array, T value, int startIndex, int count) {
            return Array.LastIndexOf(array, value, startIndex, count);

        }

        public static void
        Resize<T>(this T[] array, int newSize) {
            Array.Resize(ref array, newSize);

        }

        public static void
        Reverse_<T>(this T[] array) {
            Array.Reverse(array);

        }

        public static void
        Reverse_<T>(this T[] array, int index, int length) {
            Array.Reverse(array, index, length);

        }

        public static void
        Sort<T>(this T[] array) {
            Array.Sort(array);
        }

        public static void
        Sort<T>(this T[] array, IComparer<T> comparer) {
            Array.Sort(array, comparer);

        }

        public static void
        Sort<T>(this T[] array, Comparison<T> comparison) {
            Array.Sort(array, comparison);

        }

        public static void
        Sort<TKey, TValue>(this TKey[] array, TValue[] items) {
            Array.Sort(array, items);

        }

        public static void
        Sort<T>(this T[] array, int index, int length) {
            Array.Sort(array, index, length);

        }

        public static void
        Sort<TKey, TValue>(this TKey[] array, TValue[] items, IComparer<TKey> comparer) {
            Array.Sort(array, items, comparer);

        }

        public static void
        Sort<T>(this T[] array, Array items, int index, int length) {
            Array.Sort(array, items, index, length);

        }

        public static void
        Sort<T>(this T[] array, int index, int length, IComparer<T> comparer) {
            Array.Sort(array, index, length, comparer);

        }

        public static void
        Sort<TKey, TValue>(this TKey[] array, TValue[] items, int index, int length) {
            Array.Sort(array, items, index, length);

        }

        public static void
        Sort<TKey, TValue>(this TKey[] array, TValue[] items, int index, int length, IComparer<TKey> comparer) {
            Array.Sort(array, items, index, length, comparer);

        }

        public static bool
        TrueForAll<T>(this T[] array, Predicate<T> match) {
            return Array.TrueForAll(array, match);

        }



    }
}