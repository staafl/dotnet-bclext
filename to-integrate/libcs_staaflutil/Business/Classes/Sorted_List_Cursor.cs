using System;
using System.Collections.Generic;

using Fairweather.Service;

namespace Common
{
    public class Sorted_List_Cursor : Any_List_Cursor
    {
        public Sorted_List_Cursor(IDictionary<string, string> idict) {

            cnt = idict.Count;

            list1 = new List<Pair<string>>(cnt);
            list2 = new List<Pair<string>>(cnt);
            key_indices = new Dictionary<string, Pair<int>>(cnt);


            Load_Data_Inner(idict);
        }


        public void Load_Data(IDictionary<string, string> idict) {

            cnt = idict.Count;

            list1.Clear();
            list2.Clear();
            key_indices.Clear();

            list1.Capacity = cnt;
            list2.Capacity = cnt;

            Load_Data_Inner(idict);


        }

        void Load_Data_Inner(IDictionary<string, string> idict) {

            var temp_dict1 = new Dictionary<string, int>(cnt);
            var temp_dict2 = new Dictionary<string, int>(cnt);


            foreach (var kvp in idict) {

                var pair1 = new Pair<string>(kvp.Key, kvp.Value);

                list1.Add(pair1);
                list2.Add(pair1);

            }



            bool do_not_sort = idict is SortedDictionary<string, string> ||
                               idict is SortedList<string, string>;


            if (!do_not_sort) {
                list1.Sort(Pair.Compare_First<string>);
            }

            list2.Sort(Pair.Compare_Second<string>);

            for (int ii = 0; ii < cnt; ++ii) {

                var key1 = list1[ii].First;
                var key2 = list2[ii].First;

                temp_dict1[key1] = ii;
                temp_dict2[key2] = ii;

            }

            foreach (var kvp in temp_dict1) {

                var key = kvp.Key;
                var pos1 = kvp.Value;
                var pos2 = temp_dict2[key];

                key_indices[key] = new Pair<int>(pos1, pos2);

            }
        }

        Sorted_List_Cursor(List<Pair<string>> list1,
                         List<Pair<string>> list2,
                         Dictionary<string, Pair<int>> index) {

            cnt = list1.Count;

            (cnt == list2.Count).tiff();
            (cnt == index.Count).tiff();

            this.list1 = list1;
            this.list2 = list2;
            this.key_indices = index;

        }

        /// <summary>
        /// Adding a new key-value pair is required to take effect in all instances constructed
        /// from this one using GetCopy()
        /// </summary>
        public override Records_Cursor Get_Copy() {

            var ret = new Sorted_List_Cursor(list1, list2, key_indices);

            return ret;

        }

        int cnt;

        /// <summary>
        /// List of (key, value) ordered by key
        /// </summary>
        readonly List<Pair<string>> list1;
        /// <summary>
        /// List of (key, value) ordered by value
        /// </summary>
        readonly List<Pair<string>> list2;

        /// <summary>
        /// Dictionary of key -> (index of key in list1, index of key in list2)
        /// </summary>
        readonly Dictionary<string, Pair<int>> key_indices;

        public override int Count {
            get {
                return cnt;
            }
        }


        Func<int, int> GetComparer(string prefix,
            bool first_col, StringComparison comparison) {

            Func<int, int> ret = (int index) =>
            {
                if (index == -1)
                    return -1;

                if (!IsValidIndex(index))
                    true.tift();

                var to_compare = GetValue(first_col, index);

                if (to_compare.StartsWith(prefix, comparison))
                    return 0;

                int cmp = String.Compare(prefix, to_compare, comparison);

                bool before = (cmp < 0);

                return before ? -1 : +1;


            };

            return ret;
        }

        


        /// Returns a continuous range of elements, beginning with the first element
        /// whose key or value (specified by "key") start with the specified prefix.
        /// index and with the specified maximum length. "length" denotes the maximum
        /// number of returned elements.
        /// "length" of -1 signifies no upper bound.
        /// The direction of enumeration is affected by the SortedListCursor::Forward
        /// property.
        /// It may also return elements which do not satisfy the condition (at the end of 
        /// the returned collection.
        //public override List<Pair<string>> GetRange(string prefix, int length, StringComparison comparison) {

        //    bool forward = Forward;
        //    bool first_col = First_Column;

        //    var comparer = GetComparer(prefix, forward, first_col, comparison);

        //    var startIndex = Algorithms.Binary_Range_Search1(0, Count - 1, comparer, forward);

        //    if (startIndex == -1)
        //        return new List<Pair<string>>();

        //    var ret = GetRange(startIndex, length);

        //    return ret;
        //}
        //
        //

        /// Returns a continuous range of elements, beginning with the first element
        /// whose key or value (specified by "key") start with the specified prefix.
        /// index and with the specified maximum length. "length" denotes the maximum
        /// number of returned elements.
        /// "length" of -1 signifies no upper bound.
        /// The direction of enumeration is affected by the SortedListCursor::Forward
        /// property.
        /// It may not return elements which do not satisfy the condition (at the end of 
        /// the returned collection.
        public override List<Pair<string>>
        GetPureRange(string prefix, int length, StringComparison comparison) {

            bool forward = Forward;
            bool first_col = First_Column;

            var comparer = GetComparer(prefix, first_col, comparison);

            var startIndex = Magic.Binary_Range_Search1(0, Count - 1, comparer, forward);
            var endIndex = Magic.Binary_Range_Search1(0, Count - 1, comparer, !forward);

            if (startIndex == -1)
                return new List<Pair<string>>();

            if (endIndex == -1)
                endIndex = forward ? Count - 1 : 0;

            var length1 = endIndex - startIndex + 1;

            if (length != -1)
                length1 = Math.Min(length, length1);

            var ret = GetRange(startIndex, length1);

            return ret;
        }

        protected override Pair<string> GetPair(bool first_col, int real_index) {

            var list = first_col ? list1 : list2;

            var ret = list[real_index];

            return ret;

        }

        public override bool ContainsKey(string key) {
            return key_indices.ContainsKey(key);
        }


        public override Pair<string>? GetKVP(string key) {

            Pair<int> tmp;
            if (!key_indices.TryGetValue(key, out tmp))
                return null;

            var pair = list1[tmp.First];
            return pair;

        }

        public override int? GetIndex(string prefix, bool partial, StringComparison comparison) {

            bool forward = Forward;
            bool first_col = First_Column;

            var comparer = GetComparer(prefix, first_col, comparison);

            var startIndex = Magic.Binary_Range_Search1(0, Count - 1, comparer, forward);

            if (startIndex == -1)
                return null;



            var result = GetIndex(forward, startIndex);

            if (!partial) {
                var found = GetValue(first_col, result);
                if (!found.Equals(prefix, comparison))
                    return null;
            }

            return result;

        }


        /// <summary>
        /// Adding a new key-value pair is required to take effect in all instances constructed
        /// from this one using GetCopy()
        /// </summary>
        public override bool Add_New(string key, string value) {

            if (key_indices.ContainsKey(key))
                return false;

            var pair = new Pair<string>(key, value);

            int index1 = list1.Insert_Into_Sorted(pair, Pair.Compare_First<string>);
            int index2 = list2.Insert_Into_Sorted(pair, Pair.Compare_Second<string>);

            key_indices[key] = new Pair<int>(index1, index2);
            ++cnt;
            return true;
        }

        public override bool Remove(string key) {

            Pair<int> pair;

            if (!key_indices.TryGetValue(key, out pair))
                return false;

            key_indices.Remove(key);

            list1.RemoveAt(pair.First);
            list2.RemoveAt(pair.Second);

            --cnt;

            return true;

        }

        public override bool Supports_Second_Column {
            get { return true; }
        }


    }


}
