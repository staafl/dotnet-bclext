using System;
using System.Collections.Generic;
using Fairweather.Service;
using System.Linq;

namespace Common
{
    public class List_Cursor : Any_List_Cursor
    {
        public List_Cursor(IEnumerable<Pair<string>> pairs) {

            this.pairs = pairs.lst();
            this.cnt = this.pairs.Count();
            this.set = pairs.Select(p => p.First).set();
        }


        //public void Load_Data(IDictionary<string, string> idict) {

        //    cnt = idict.Count;

        //    list1.Clear();
        //    list2.Clear();
        //    key_indices.Clear();

        //    list1.Capacity = cnt;
        //    list2.Capacity = cnt;

        //    Load_Data_Inner(idict);


        //}

        //void Load_Data_Inner(IDictionary<string, string> idict) {

        //    var temp_dict1 = new Dictionary<string, int>(cnt);
        //    var temp_dict2 = new Dictionary<string, int>(cnt);


        //    foreach (var kvp in idict) {

        //        var pair1 = new Pair<string>(kvp.Key, kvp.Value);

        //        list1.Add(pair1);
        //        list2.Add(pair1);

        //    }



        //    bool do_not_sort = idict is SortedDictionary<string, string> ||
        //                       idict is SortedList<string, string>;


        //    if (!do_not_sort) {
        //        list1.Sort(Pair.Compare_First<string>);
        //    }

        //    list2.Sort(Pair.Compare_Second<string>);

        //    for (int ii = 0; ii < cnt; ++ii) {

        //        var key1 = list1[ii].First;
        //        var key2 = list2[ii].First;

        //        temp_dict1[key1] = ii;
        //        temp_dict2[key2] = ii;

        //    }

        //    foreach (var kvp in temp_dict1) {

        //        var key = kvp.Key;
        //        var pos1 = kvp.Value;
        //        var pos2 = temp_dict2[key];

        //        key_indices[key] = new Pair<int>(pos1, pos2);

        //    }
        //}



        /// <summary>
        /// Adding a new key-value pair is required to take effect in all instances constructed
        /// from this one using GetCopy()
        /// </summary>
        public override Records_Cursor Get_Copy() {

            var ret = new List_Cursor(pairs);

            return ret;

        }

        int cnt;

        readonly Set<string> set;
        readonly List<Pair<string>> pairs;


        public override int Count {
            get {
                return cnt;
            }
        }


        protected override Pair<string> GetPair(bool first_col, int real_index) {

            var ret = pairs[real_index];

            return ret;

        }

        Func<string, Pair<string>, bool> GetPredicate(bool first_col, bool partial) {

            Func<string, string, bool> inner;
            Func<string, Pair<string>, bool> ret;

            if (partial)
                inner = (key, s) => s.StartsWith(key);
            else
                inner = (key, s) => key == s;


            if (first_col)
                ret = (key, pair) => inner(key, pair.First);
            else
                ret = (key, pair) => inner(key, pair.Second);

            return ret;
        }


        /// Returns a continuous range of elements starting from the specified
        /// index and with the specified maximum length.
        /// Length of -1 signifies no upper bound.
        /// The direction of enumeration is affected by the SortedListCursor::Forward
        /// property 
        /// Forward => [startIndex, startIndex+1, ..., startIndex+length-1]
        /// Forward => [startIndex, startIndex-1, ..., startIndex-length+1]
        public override List<Pair<string>>
        GetRange(int startIndex, int length) {

            bool forward = this.Forward;
            bool first_col = this.First_Column;

            if (length == -1)
                length = Count - startIndex;

            IsValidIndex(startIndex).tiff();
            (length >= 0).tiff();

            var ret = new List<Pair<string>>();

            for (int ii = 0; ii < length; ++ii) {

                int index = GetIndex(forward, startIndex, ii);

                if (!IsValidIndex(index))
                    break;

                var pair = GetPair(first_col, index);

                ret.Add(pair);

            }

            return ret;
        }

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

            int ii = -1;
            int start = 0;
            int end = 0;

            var pred = GetPredicate(forward, true);

            bool seen = false;
            bool seeing = false;

            foreach (var kvp in pairs) {

                ++ii;

                bool check = pred(prefix, kvp);

                if (seen == check)
                    continue;

                if (seen /* && !check */) {
                    seeing = false;
                    end = ii - 1;
                    break;
                }

                start = ii;

                seen = true;
                seeing = true;

            }

            if (seeing)
                end = ii;

            if (!seen)
                end = -1;

            var ret = GetRange(start, end - start + 1);

            return ret;
        }



        public override bool ContainsKey(string key) {
            return set[key];
        }

        public override Pair<string> GetAtIndex(int index) {

            bool forward = Forward;
            bool first_col = First_Column;

            var real_index = GetIndex(forward, index);

            IsValidIndex(real_index).tiff();

            var ret = GetPair(first_col, real_index);

            return ret;

        }

        public override Pair<string>? GetKVP(string key) {

            foreach (var kvp in pairs)
                if (kvp.First == key)
                    return kvp;

            return null;

        }

        public override int? GetIndex(string prefix, bool partial, StringComparison comparison) {

            bool forward = Forward;
            bool first_col = First_Column;

            int? result = null;

            var pred = GetPredicate(first_col, partial);

            for (int ii = 0; ii < Count; ++ii) {

                var ix = GetIndex(forward, ii);

                var kvp = GetPair(first_col, ix);

                if (pred(prefix, kvp)) {
                    result = ii;
                    break;
                }

            }

            return result;

        }


        public override bool Supports_Second_Column {
            get { return false; }
        }


    }
}
