using System;



namespace Fairweather.Service
{
    public static partial class Magic
    {
        public static int
        Binary_Search(int lower,
                      int upper,
                      Func<int, int> comparison) {

            (lower <= upper).tiff();


            int middle;

            while (lower < upper) {

                middle = (lower + upper) / 2;

                var cmp = comparison(middle);

                if (cmp == 0)
                    return middle;

                if (cmp > 0)
                    upper = middle;
                else
                    lower = middle;

            }

            return -1;

        }

        /// <summary>
        /// Searches a sorted sequence for the beginning or the end of an interval, as specified by find_start.
        /// Returns -1 if such an interval does not exist.
        /// Comparer should return 0 if a specified index lies within the interval, -1 if it lies below (to
        /// the left of) it and +1 if it lies above (to the right of) it.
        /// </summary>
        public static int
        Binary_Range_Search(int lower, int upper, Func<int, int> comparer, bool find_start) {


            (lower <= upper).tiff();

            int middle = -1;
            int found_index = -1;
            while (lower < upper) {

                middle = (lower + upper) / 2;

                int cmp = comparer(middle);

                if (cmp == 0) {
                    found_index = middle;
                    cmp = find_start ? 1 : -1;
                }

                if (cmp > 0) {
                    if (upper == middle)
                        break;
                    upper = middle;
                }
                else {
                    if (lower == middle)
                        break;
                    lower = middle;
                }

            }

            return found_index;
        }

        /// <summary>
        /// Searches a sorted sequence for the beginning or the end of an interval, as specified by find_start.
        /// Returns -1 if such an interval does not exist.
        /// Comparer should return 0 if a specified index lies within the interval, -1 if it lies below (to
        /// the left of) it and +1 if it lies above (to the right of) it.
        /// </summary>
        public static int
        Binary_Range_Search1(int lower, int upper, Func<int, int> comparer, bool find_start) {

            (lower <= upper).tiff();

            bool found = false;

            int middle;
            Func<int, int> change = find_start ? (Func<int, int>)(ii => ii - 1) : ii => ii + 1;

            while (lower <= upper) {

                middle = (lower + upper) / 2;

                int cmp = comparer(middle);

                if (cmp == 0) {
                    do {
                        found = true;
                        var changed = change(middle);

                        if (changed < lower)
                            break;

                        if (changed > upper)
                            break;

                        if (comparer(changed) != 0)
                            break;

                        middle = changed;

                    } while (true);


                    return middle;
                }


                if (cmp < 0) {
                    if (upper == middle)
                        upper = middle - 1;
                    else
                        upper = middle;
                }
                else {
                    if (lower == middle)
                        lower = middle + 1;
                    else
                        lower = middle;
                }

            }

            if (!found)
                return -1;

            return find_start ? lower - 1 : upper - 1;
        }


    }
}