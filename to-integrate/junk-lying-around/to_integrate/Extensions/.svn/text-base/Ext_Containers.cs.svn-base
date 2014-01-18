using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fairweather.Service
{
    static partial class Extensions
    {
        public static void 
        Add_Range<T, S>(this ICollection<T> list, params S[] values) where S : T {
            foreach (S value in values)
                list.Add(value);
        }


        public static KeyValuePair<TK, TV>
        KVP<TK, TV>(this IDictionary<TK, TV> dict, TK key) {
            return new KeyValuePair<TK, TV>(key, dict[key]);
        }

        public static int?
        Maybe_Count<T>(this IEnumerable<T> seq) {

            var cast = seq as ICollection<T>;
            if (cast == null)
                return null;

            return cast.Count;

        }

        public static int
        Maybe_Count<T>(this IEnumerable<T> seq, int def) {

            var cast = seq as ICollection<T>;
            if (cast == null)
                return def;

            return cast.Count;

        }

        public static void
        AddRange<T>(this ICollection<T> col, IEnumerable<T> enumerable) {
            foreach (var cur in enumerable)
                col.Add(cur);
        }

        public static Dictionary<T, TValue>
        ToDictionary<T, TValue>(this IEnumerable<T> seq, Func<T, TValue> value_func) {

            var ret = new Dictionary<T, TValue>();

            foreach (T elem in seq)
                ret.Add(elem, value_func(elem));

            return ret;
        }

        public static void
        Format<T>(this List<T> list, int capacity) {
            list.Clear();
            list.Capacity = capacity;
        }

        public static void
        Format<T>(this List<T> list, int capacity, IEnumerable<T> seq) {
            list.Clear();
            list.Capacity = capacity;
            list.AddRange(seq);
        }

        public static void
        Format<T1, T2>(this IDictionary<T1, T2> idict,
                       IEnumerable<KeyValuePair<T1, T2>> seq) {
            idict.Clear();
            idict.Fill(seq, false);
        }


        public static int
        Replace<T>(this IList<T> ilist, T old_elem, T new_elem) {

            int replacements = 0;
            for (int ii = 0; ii < ilist.Count; ++ii) {
                if (object.Equals(old_elem, ilist[ii])) {
                    ilist[ii] = new_elem;
                    ++replacements;
                }
            }

            return replacements;
        }

        public static Dictionary<TKey, TValue>
        Copy<TKey, TValue>(this IDictionary<TKey, TValue> idict) {

            return new Dictionary<TKey, TValue>(idict);

        }

        public static List<T>
        Copy<T>(this IList<T> ilist) {

            return new List<T>(ilist);

        }

        public static int
        Safe_Length<T>(this T[] arr, bool minus_one) {
            if (arr != null)
                return arr.Length;
            if (minus_one)
                return -1;

            return 0;
        }


        public static int
        Safe_Length<T>(this IEnumerable<T> seq, bool minus_one) {
            if (seq != null)
                return seq.Count();

            if (minus_one)
                return -1;

            return 0;
        }

        // ****************************

        static public T
        Index<T>(this List<T> list, int index, bool forward) {
            return list[forward ? index : list.Count - 1 - index];
        }

        public static T
        Index<T>(this List<T> list, int index, bool forward, T value) {

            var ret = list.Index(index, forward);

            list[forward ? index : list.Count - 1 - index] = value;

            return ret;
        }

        public static T
        Index<T>(this T[] array, int index, bool forward, Endo<T> mutator) {

            return array.Index(index, forward, mutator(array.Index(index, forward)));

        }

        public static T
        Index<T>(this T[] array, int index, bool forward) {
            return array[forward ? index : array.Length - 1 - index];
        }

        public static T
        Index<T>(this T[] array, int index, bool forward, T value) {

            var ret = array.Index(index, forward);

            array[forward ? index : array.Length - 1 - index] = value;

            return ret;
        }




        // ****************************


        public static int
        Insert_Into_Sorted<T>(this List<T> list, T value, Comparison<T> cmp) {

            int ii;
            // Todo: Binary search?
            for (ii = 0; ii < list.Count; ++ii) {

                if (cmp(value, list[ii]) < 0) {
                    break;
                }
            }

            list.Insert(ii, value);
            return ii;

        }


        public static T
        Chop_First<T>(this List<T> list) {

            var ret = list[0];
            list.RemoveAt(0);
            return ret;

        }

        public static T
        Chop_Last<T>(this List<T> list) {

            int cnt = list.Count;
            var ret = list[cnt - 1];
            list.RemoveAt(cnt - 1);
            return ret;

        }

        public static void
        Remove_At<T>(this List<T> list, params int[] indices) {

            list.tifn();

            if (indices.Is_Empty())
                return;

            Array.Sort(indices);

            int ind_cnt = indices.Length;

            int list_cnt = list.Count;

            int jj = 0;

            for (int ii = 0; ii < list_cnt; ++ii) {

                if (indices[jj] == ii) {
                    list.RemoveAt(ii);
                    --ii;
                    --list_cnt;
                    ++jj;

                    if (jj == ind_cnt)
                        break;
                }

            }

        }

        /// <summary>
        /// Performs a Shwartzian transform on the list and sorts it.
        /// </summary>
        public static void
        Sort<T1, T2>(this List<T1> list, Func<T1, T2> func)
            where T2 : IComparable<T2> {

            var arr1 = list.ToArray();
            var arr2 = list.Select(elem => func(elem)).ToArray();

            Array.Sort(arr2, arr1);

            list.Clear();

            list.AddRange(arr1);

            return;


        }

        public static void
        Add_Range<T>(this List<T> list, T head, params T[] tail) {

            int cnt = tail.Length;
            var array = new T[cnt + 1];
            array[0] = head;

            Array.Copy(tail, 0, array, 1, cnt);

            list.AddRange(array);

        }

        public static int
        Remove_All<T>(this T[] array, Predicate<T> predicate, bool resize) {

            array.tifn();
            predicate.tifn();

            int cnt = array.Length;

            if (cnt == 0)
                return 0;

            int front = 0, back = 0;

            while (front < cnt) {

                while (front < cnt && predicate(array[front])) {
                    ++front;

                }
                while (front < cnt && !predicate(array[front])) {
                    array[back++] = array[front++];
                }

            }

            int ret = cnt - back;
            if (resize)
                Array.Resize(ref array, back);
            else
                Array.Clear(array, back, cnt - back);

            return ret;

        }


        // ****************************



        public static bool
        Piecewise_Equal<TIndex, TValue>(this Dictionary<TIndex, TValue> dict1,
                                             Dictionary<TIndex, TValue> dict2,
                                             bool check_all) {

            if (dict1 == dict2)
                return true;

            if ((dict1 == null) || (dict2 == null)) {
                return (dict1 == null) == (dict2 == null);
            }

            for (int ii = 0; ii <= 1; ++ii) {

                foreach (var kvp in dict1) {

                    TValue value;

                    if (!dict2.TryGetValue(kvp.Key, out value)) {
                        if (check_all)
                            return false;
                    }

                    if (!value.Safe_Equals(kvp.Value))
                        return false;

                }

                H.Swap(ref dict1, ref dict2);

            }

            return true;

        }


        /*       Untested        */
        /// <summary>
        /// Returns false if the two collections are of different types
        /// or lengths
        /// </summary>
        public static bool
        Deep_Equals<T>(this IEnumerable<T> left,
                            IEnumerable<T> right) {

            if (left.GetType() != right.GetType())
                return false;

            if (left == null || right == null) {

                if (left == null)
                    return right == null;

                return left == null;
            }

            var arr_left = left as T[];
            var arr_right = right as T[];

            if (arr_left != null) {

                if (arr_right == null)
                    return false;

                if (arr_left.Length != arr_right.Length) {
                    return false;
                }

                for (int ii = 0; ii < arr_left.Length; ++ii)
                    if (!arr_left[ii].Equals(arr_right[ii]))
                        return false;

                return true;

            }

            var left_forced = left.Force();
            var right_forced = right.Force();

            if (left_forced.Count != right_forced.Count)
                return false;

            using (var enumer1 = left_forced.GetEnumerator())
            using (var enumer2 = left_forced.GetEnumerator()) {

                while (enumer1.MoveNext()) {
                    enumer2.MoveNext();

                    if (!enumer1.Current.Equals(enumer2.Current))
                        return false;
                }

                return true;
            }


        }






        // ****************************





        static public Pair<TKey[], TValue[]>
        ToArrays<TKey, TValue>(this IDictionary<TKey, TValue> dict) {



            TKey[] keys;
            TValue[] values;

            var pairs = dict.ToList();

            keys = (from pair in pairs
                    select pair.Key).ToArray();

            values = (from pair in pairs
                      select pair.Value).ToArray();

            return Pair.Make(keys, values);

        }

        static public int
        Drop<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                Func<KeyValuePair<TKey, TValue>, bool> f) {

            int ret = 0;
            var list = dict.Where(f).Select(kvp => kvp.Key).lst();
            foreach (var key in list)
                if (dict.Remove(key))
                    ++ret;

            return ret;


        }

        static public int
        Drop<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                IEnumerable<TKey> keys) {

            int ret = 0;

            foreach (var key in keys)
                if (dict.Remove(key))
                    ++ret;

            return ret;

        }

        static public void
        Rekey<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                 Func<TKey, Pair<bool, TKey>> f) {

            var list = new List<Triple<TKey, TKey, TValue>>();
            var new_keys = new Set<TKey>();

            foreach (var kvp in dict) {

                var maybe = f(kvp.Key);

                if (maybe.First) {
                    list.Add(Triple.Make(kvp.Key, maybe.Second, kvp.Value));
                    new_keys[maybe.Second] = true;
                }

            }


            foreach (var triple in list) {
                if (!new_keys[triple.First])
                    dict.Remove(triple.First);
                dict[triple.Second] = triple.Third;
            }

        }

        static public bool
        Rekey<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                 IEnumerable<Pair<TKey>> keys,
                                 bool strict) {

            bool ret = true;
            foreach (var pair in keys) {
                if (!dict.Replace_Key(pair.First, pair.Second)) {
                    ret = false;
                    if (strict)
                        break;
                }
            }

            return ret;

        }


        static public Dictionary<TNew, TValue>
        Transform_Keys<TOld, TNew, TValue>(this IDictionary<TOld, TValue> dict,
                                                Func<TOld, TNew> f) {

            var ret = dict.ToDictionary(kvp => f(kvp.Key),
                                        kvp => kvp.Value);

            return ret;

        }




        static public Dictionary<TKey, TNew>
        Transform_Values<TKey, TOld, TNew>(this IDictionary<TKey, TOld> dict,
                                           Func<TKey, TOld, TNew> f) {

            var ret = dict.ToDictionary(kvp => kvp.Key,
                                        kvp => f(kvp.Key, kvp.Value));

            return ret;

        }
        static public Dictionary<TKey, TNew>
        Transform_Values<TKey, TOld, TNew>(this IDictionary<TKey, TOld> dict,
                                                Func<TOld, TNew> f) {

            var ret = dict.ToDictionary(kvp => kvp.Key,
                                        kvp => f(kvp.Value));

            return ret;

        }





        static public bool
        Replace_Key<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                       TKey old_key,
                                       TKey new_key) {

            TValue value;
            if (!dict.TryGetValue(old_key, out value))
                return false;

            if (!dict.Remove(old_key))
                return false;

            dict[new_key] = value;

            return true;

        }




        // ****************************


        static public List<KeyValuePair<TIndex, string>>
        Sort_By_Value<TIndex>(this Dictionary<TIndex, string> dict) {

            var list = dict.ToList();
            list.Sort((kvp1, kvp2) => kvp1.Value.CompareTo(kvp2.Value));

            return list;
        }

        static public List<KeyValuePair<TIndex, int>>
        Sort_By_Value<TIndex>(this Dictionary<TIndex, int> dict) {

            var list = dict.ToList();
            list.Sort((kvp1, kvp2) => kvp1.Value.CompareTo(kvp2.Value));

            return list;
        }

        static public List<KeyValuePair<TIndex, TValue>>
        Sort_By_Value<TIndex, TValue>(this Dictionary<TIndex, TValue> dict) {

            var list = dict.ToList();
            list.Sort((kvp1, kvp2) => Comparer<TValue>.Default.Compare(kvp1.Value, kvp2.Value));

            return list;
        }




        static public List<KeyValuePair<string, TValue>>
        Sort_By_Key<TValue>(this Dictionary<string, TValue> dict) {

            var list = dict.ToList();
            list.Sort((kvp1, kvp2) => kvp1.Key.CompareTo(kvp2.Key));

            return list;
        }

        static public List<KeyValuePair<int, TValue>>
        Sort_By_Key<TValue>(this Dictionary<int, TValue> dict) {

            var list = dict.ToList();
            list.Sort((kvp1, kvp2) => kvp1.Key.CompareTo(kvp2.Key));

            return list;
        }

        static public List<KeyValuePair<TKey, TValue>>
        Sort_By_Key<TKey, TValue>(this Dictionary<TKey, TValue> dict) {

            var list = dict.ToList();
            list.Sort((kvp1, kvp2) => Comparer<TKey>.Default.Compare(kvp1.Key, kvp2.Key));

            return list;
        }


        // ****************************


        static public void
        Fill<T>(this T[] array, IEnumerable<T> seq, int start_index, bool exact) {

            int ii = start_index;
            int cnt = array.Length;

            foreach (var elem in seq) {
                array[ii] = elem;
                ++ii;
            }

            if (exact && ii != cnt)
                true.tift();

        }

        static public void
        Fill<T>(this List<T> list, IEnumerable<T> seq, int start_index, bool exact) {

            int ii = start_index;
            int cnt = list.Count;

            foreach (var elem in seq) {
                list[ii] = elem;
                ++ii;
            }

            if (exact && ii != cnt)
                true.tift();

        }




        static public void
        Fill<TIx, TValue>(this IReadWrite<TIx, TValue> left,
                               IReadWrite<TIx, TValue> right,
                               IEnumerable<TIx> ixs) {

            ixs.tifn();

            foreach (var ix in ixs)
                left[ix] = right[ix];

        }


        static public void
        Fill<TIx, TValue>(this IDictionary<TIx, TValue> left,
                               IReadWrite<TIx, TValue> right,
                               IEnumerable<TIx> values) {

            values.tifn();

            foreach (var ix in values)
                left[ix] = right[ix];

        }

        static public void
        Fill<TIx, TValue>(this IReadWrite<TIx, TValue> fill,
                               IDictionary<TIx, TValue> from) {

            foreach (var kvp in from)
                fill[kvp.Key] = kvp.Value;

        }

        /*       Return the parameter to allow use in field initialization        */

        static public IDictionary<TIx, TValue>
        Fill<TIx, TValue>(this IDictionary<TIx, TValue> fill,
                               IEnumerable<KeyValuePair<TIx, TValue>> from,
                               bool allow_duplicates) {

            if (allow_duplicates) {
                foreach (var kvp in from)
                    fill[kvp.Key] = kvp.Value;
            }
            else {
                foreach (var kvp in from)
                    fill.Add(kvp.Key, kvp.Value);
            }

            return fill;
        }

        static public IReadWrite<TIx, TValue>
        Fill<TIx, TValue>(this IReadWrite<TIx, TValue> fill,
                               IEnumerable<Pair<TIx, TValue>> values) {

            foreach (var pair in values)
                fill[pair.First] = pair.Second;

            return fill;

        }

        static public IReadWrite<TIx1, TValue>
        Fill<TIx1, TIx2, TValue>(this IReadWrite<TIx1, TValue> left,
                                      IReadWrite<TIx2, TValue> right,
                                      IEnumerable<Pair<TIx1, TIx2>> ixs) {

            foreach (var pair in ixs)
                left[pair.First] = right[pair.Second];

            return left;

        }

        static public IReadWrite<TIx1, TValue>
        Fill<TIx1, TIx2, TValue>(this IReadWrite<TIx1, TValue> left,
                                      IReadWrite<TIx2, TValue> right,
                                      IDictionary<TIx1, TIx2> ixs) {

            foreach (var kvp in ixs)
                left[kvp.Key] = right[kvp.Value];

            return left;

        }



        // ****************************

        static public bool
        Try_Modify<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                 TKey key,
                                  Action<TValue> modifier) {

            TValue temp;

            bool ret = dict.TryGetValue(key, out temp);

            if (ret)
                modifier(temp);

            return ret;
        }

        static public bool
        List_Modify<TKey, TValue>(this IDictionary<TKey, List<TValue>> dict,
                                  TKey key,
                                  TValue value) {

            return dict.Try_Modify(key, lst => lst.Add(value),
                     () => new List<TValue> { value });
        }

        static public bool
        Try_Modify<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                              TKey key,
                              Action<TValue> modifier,
                              Func<TValue> def) {

            TValue temp;

            bool ret = dict.TryGetValue(key, out temp);

            if (ret)
                modifier(temp);
            else
                dict[key] = def();

            return ret;
        }

        static public bool
        Try_Modify<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                      TKey key,
                                      Func<TValue, TValue> modifier) {

            TValue temp;

            bool ret = dict.TryGetValue(key, out temp);

            if (ret)
                dict[key] = modifier(temp);

            return ret;
        }

        static public bool
        Try_Modify<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                              TKey key,
                              Func<TValue, TValue> modifier,
                              Func<TValue> def) {

            TValue temp;

            bool ret = dict.TryGetValue(key, out temp);

            if (ret)
                dict[key] = modifier(temp);
            else
                dict[key] = def();

            return ret;
        }

        /// <summary>
        /// If the specified key is present, manipulate the value using the specified function.
        /// Otherwise, add it to the dictionary with the given value
        /// Return whether or not the key was there.
        /// </summary>
        static public bool
        Try_Modify<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                      TKey key,
                                      Func<TValue, TValue> modifier,
                                      TValue default_value,
                                      bool manipulate_default) {

            if (manipulate_default)
                return Try_Modify(dict, key,
                                  modifier,

                                  () => default_value);
            else
                return Try_Modify(dict, key,
                  modifier,
                  () => modifier(default_value));

            //TValue temp;

            //bool ret = dict.TryGetValue(key, out temp);

            //if (!ret) {
            //    if (manipulate_default)
            //        temp = modifier(default_value);
            //    else
            //        temp = default_value;

            //}

            //dict[key] = temp;

            //return ret;
        }

        // ****************************


        static public bool
        FindByValue<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                     TValue val,
                                     out TKey key) {

            key = default(TKey);

            foreach (var kvp in dict) {
                if (kvp.Value.Equals(val)) {
                    key = kvp.Key;
                    return true;
                }
            }

            return false;

        }

        /// <summary>
        /// Left-biased
        /// </summary>
        static public Dictionary<TKey, TValue>
        Merge<TKey, TValue>(this Dictionary<TKey, TValue> dict1,
                                 Dictionary<TKey, TValue> dict2,
                                 bool throw_on_duplicate) {

            var ret = new Dictionary<TKey, TValue>(dict2);

            foreach (var kvp in dict1) {

                if (throw_on_duplicate) {

                    try {
                        ret.Add(kvp.Key, kvp.Value);
                    }
                    catch (ArgumentException ex) {

                        string message = "An element with the key " + kvp.Key + "exists in both dictionaries.";
                        var new_ex = new ArgumentException(message, ex);
                        true.tift(new_ex);

                    }

                }
                else {

                    ret[kvp.Key] = kvp.Value;

                }
            }

            return ret;
        }



        // ****************************


        static public T
        Get_Or_Default<T>(this T[] array, int index, T def) {

            if (index < array.Length)
                return array[index];

            return def;

        }

        static public T
        Get_Or_Default<T>(this T[] array, int index, Func<T> def) {

            if (index < array.Length)
                return array[index];

            return def();

        }

        // ****************************


        static public TValue
        Get_Or_New<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                      TKey key,
                                      bool place_if_not_found)
            where TValue : new() {

            return Get_Or_Default(dict, key, () => new TValue(), place_if_not_found);
        }

        static public TValue
        Get_Or_Null<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                       TKey key)
            where TValue : class {

            TValue ret;

            if (!dict.TryGetValue(key, out ret))
                ret = null;

            return ret;


        }

        //static public Pair<bool, TValue>
        //Try_Get_Value<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) {

        //    TValue val;
        //    bool ok = dict.TryGetValue(key, out val);
        //    var ret = Pair.Make(ok, val);
        //    return ret;

        //}

        static public TValue
        Get_Or_Default_<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                          TKey key,
                                          TValue default_value) {

            return Get_Or_Default_(dict, key, default_value, true);

        }


        static public TValue
        Get_Or_Default<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                          TKey key,
                                          Func<TValue> default_value) {

            return Get_Or_Default(dict, key, default_value, true);
        }

        static public TValue
        Get_Or_Default_<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                          TKey key,
                                          TValue default_value,
                                          bool place_if_not_found) {

            TValue ret;

            if (!dict.TryGetValue(key, out ret)) {
                ret = default_value;
                if (place_if_not_found)
                    dict[key] = default_value;
            }

            return ret;

        }

        static public TValue
        Get_Or_Default<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                          TKey key,
                                          Func<TValue> default_value,
                                          bool place_if_not_found) {

            TValue ret;

            if (!dict.TryGetValue(key, out ret)) {
                ret = default_value();
                if (place_if_not_found)
                    dict[key] = ret;
            }

            return ret;

        }


        // ****************************


        static public TResult[]
        Transform<TResult>(this Array array) {

            var temp = array.Cast<TResult>();

            var ret = temp.ToArray();

            return ret;

        }

        static public TResult[]
        Transform<TValue, TResult>(this TValue[] array) {

            var temp = array.Cast<TResult>();

            var ret = temp.ToArray();

            return ret;

        }

        static public TResult[]
        Transform<TValue, TResult>(this TValue[] array, Func<TValue, TResult> func) {

            var temp = array.Select(elem => func(elem));

            var ret = temp.ToArray();

            return ret;

        }


        // ****************************

        static public string
        Tuple_Print<TKey, TValue>(this IEnumerable<ITuple> seq, string fmt) {

            return seq.Pretty_Print(tuple => fmt.spf(tuple.ToArray()));

        }

        static public string
        Pair_Print<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dict) {

            return dict.Pretty_Print(kvp => "{0},{1}".spf(kvp.Key, kvp.Value) + Environment.NewLine);


        }

        static public string
        Pair_Print<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dict, string fmt) {

            return dict.Pretty_Print(kvp => fmt.spf(kvp.Key, kvp.Value));


        }


        // ****************************


        /// <summary>
        /// Default format is "{0}, "
        /// </summary>
        static public string
        Pretty_Print<T>(this IEnumerable<T> seq) {
            return Pretty_Print(seq, null as string);
        }

        /// <summary>
        /// "" as format is "{0}, "
        /// </summary>
        static public string
        Pretty_Print<T>(this IEnumerable<T> seq, Func<T, string> format) {

            var sb = new StringBuilder();

            Pretty_Print(seq, format, str => sb.Append(str));

            return sb.ToString();

        }

        /// <summary>
        /// "" as format is "{0}, "
        /// </summary>
        static public string
        Pretty_Print<T>(this IEnumerable<T> seq, string format) {

            var sb = new StringBuilder();

            if (format.IsNullOrEmpty())
                format = "{0}, ";

            Pretty_Print(seq, format, str => sb.Append(str));

            return sb.ToString();

            /// Rolled-out code
            //if (format.IsNullOrEmpty())
            //    format = "{0}, ";


            //var sb = new StringBuilder();

            //foreach (var elem in seq) {


            //    object obj = elem;
            //    if (obj == null)
            //        obj = "[null]";

            //    sb.AppendFormat(format, obj);
            //}

            //return sb.ToString();

        }



        /// <summary>
        /// Default format is "{0}, "
        /// </summary>
        static public void
        Pretty_Print<T>(this IEnumerable<T> seq, string format, Action<string> display) {

            Pretty_Print(seq, obj => format.spf(obj), display);
        }

        /// <summary>
        /// Default format is "{0}, "
        /// </summary>
        static public void
        Pretty_Print<T>(this IEnumerable<T> seq,
                        Func<T, string> format,
                        Action<string> display) {

            format = format ?? (obj => obj.ToString() + ", ");

            foreach (var elem in seq) {

                var str = elem == null ? "[null]" : format(elem);

                display(str);

            }

        }


        public static string
        Simple_Print<T>(this IEnumerable<T> seq) {
            return Lippert_Print<T>(seq, "{0}", ", {0}", ", {0}");

        }


        static public string
        Lippert_Print<T>(this IEnumerable<T> seq) {

            return seq.Lippert_Print("{0}", ", {0}", " and {0}");

        }

        public static string
        Lippert_Print<T>(this IEnumerable<T> seq, string fmt_last) {

            return Lippert_Print<T>(seq, "{0}", ", {0}", fmt_last);

        }

        // example: "{0}", ", {0}", " and {0}"
        // example: "{0}", ", {0}", " or {0}"
        static public string
        Lippert_Print<T>(this IEnumerable<T> seq,
                              string fmt_first,
                              string fmt_middle,
                              string fmt_last) {

            var sb = new StringBuilder();

            using (var enm = seq.GetEnumerator()) {

                if (!enm.MoveNext())
                    return "";

                // first element
                object elem = enm.Current;

                var is_last = !enm.MoveNext();

                var format = fmt_first;

                while (true) {

                    var obj = elem ?? "[null]";

                    sb.AppendFormat(format, obj);

                    if (is_last)
                        break;

                    // next element
                    elem = enm.Current;

                    is_last = !enm.MoveNext();

                    format = is_last ? fmt_last : fmt_middle;

                }
            }

            return sb.ToString();

        }


    }
}
