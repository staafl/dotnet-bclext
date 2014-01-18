using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fairweather.Service
{
    public static partial class Extensions
    {
        public static IEnumerable<T2>
        OfType<T1, T2>(this IEnumerable<T1> seq) where T2 : class {
            foreach (var elem in seq) {

                var as_T2 = elem as T2;

                if (as_T2 != null)
                    yield return as_T2;

            }

        }

        public static IEnumerable<T2>
        Cast<T1, T2>(this IEnumerable<T1> seq) {
            foreach (var elem in seq) {
                yield return (T2)(object)elem;

            }

        }
        //public static IEnumerable<T>
        //Concat<T>(this IEnumerable<T> seq, params IEnumerable<T>[] other) {

        //    foreach (var subseq in other.Pend(seq, true))
        //        foreach (var elem in subseq)
        //            yield return elem;

        //}


        [Obsolete("Use Explode2")]
        public static IEnumerable<TResult>
        FlatMap<TResult>
        (this IEnumerable source,
              Func<object, bool> is_composite,
              Func<object, IEnumerable> get_composite,
              Func<object, TResult> func) {

            var ret = new List<TResult>();
            var stack = new Stack<IEnumerable>();
            stack.Push(source);

            while (!stack.Is_Empty()) {

                source = stack.Pop();

                foreach (var elem in source) {

                    if (is_composite(elem)) {

                        stack.Push(get_composite(elem));

                    }

                    ret.Add(func(elem));
                }
            }

            return ret;
        }


        [Obsolete("Use Explode2")]
        public static IEnumerable<TResult> FlatMap<TSource, TResult>
        (this IEnumerable<TSource> source,
              Predicate<TSource> is_composite,
              Func<TSource, IEnumerable<TSource>> get_composite,
              Func<TSource, TResult> func) {

            var ret = new List<TResult>();
            var stack = new Stack<IEnumerable<TSource>>();
            stack.Push(source);

            while (!stack.Is_Empty()) {

                source = stack.Pop();

                foreach (var elem in source) {
                    if (is_composite(elem)) {

                        stack.Push(get_composite(elem));

                    }

                    ret.Add(func(elem));
                }
            }

            return ret;
        }


        /*       Untested        */

        static public IEnumerable<KeyValuePair<TKey, TValue>>
        Order_By_Value<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dict) {

            return dict.OrderBy(kvp => new { kvp.Value, kvp.Key });

        }


        /*       Untested        */

        static public IEnumerable<KeyValuePair<TKey, TValue>>
        Order_By_Key<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dict) {

            return dict.OrderBy(kvp => kvp);//new { kvp.Key, kvp.Value });

        }


        public static IEnumerable<T2>
        Flatmap<T1, T2>(this IEnumerable<T1> seq, Func<T1, IEnumerable<T2>> f) {

            foreach (T1 elem1 in seq)
                foreach (T2 elem2 in f(elem1))
                    yield return elem2;

        }


        public static IEnumerable<T>
        Explode<T>(this IEnumerable<T> seq,
                        Func<T, IEnumerable<T>> f,
                        Action<IEnumerable<T>> push,
                        Func<IEnumerable<T>> pop) {

            push(seq);

            while ((seq = pop()) != null) {

                foreach (var elem in seq) {

                    yield return elem;

                    var seq2 = f(elem);

                    if (seq2 != null)
                        push(seq2.lsta());

                }

            }

        }

        public static IEnumerable<T>
        Explode2<T>(this IEnumerable<T> seq, Func<T, IEnumerable<T>> f) {


            var seqs = new Stack<IEnumerable<T>>();

            var ret = seq.Explode(f,
                                  _seq => seqs.Push(_seq),
                                  () => seqs.Count > 0 ? seqs.Pop() : null);

            return ret;

        }

        public static IEnumerable<T>
        Explode<T>(this IEnumerable<T> seq, Func<T, IEnumerable<T>> f) {


            var seqs = new Queue<IEnumerable<T>>();

            var ret = seq.Explode(f,
                                  _seq => seqs.Enqueue(_seq),
                                  () => seqs.Count > 0 ? seqs.Dequeue() : null);

            return ret;

            //seqs.Enqueue(seq);


            //while (seqs.Count > 0) {

            //    var seq_1 = seqs.Dequeue();

            //    foreach (T elem in seq_1) {



            //        yield return elem;

            //        var seq_2 = f(elem);

            //        if(seq_2 != null)
            //            seqs.Enqueue(seq_2);

            //    }


            //}
        }

        static public IEnumerable<T>
        Loop<T>(this IEnumerable<T> seq) {

            while (true)
                foreach (var elem in seq)
                    yield return elem;

        }

        static public IEnumerable<T>
        Repeat<T>(this T head) {
            while (true)
                yield return head;
        }

        static public IEnumerable<T>
        Repeat<T>(this T head,
                       int count) {
            for (int ii = 0; ii < count; ++ii)
                yield return head;
        }

        static public IEnumerable<T>
        Repeat<T>(this T head,
                       Func<T, T> producer,
                       Predicate<T> do_while) where T : struct {

            while (do_while(head)) {

                yield return head;
                head = producer(head);

            }

        }



        // ****************************




        public static IEnumerable<Pair<T[], int>>
        Diff<T>(this IEnumerable<T> seq1,
                params IEnumerable<T>[] seqs) {

            // All_Differences, Array-only version, commented out
            #region
            //public static List<int>
            //All_Differences<T>(this T[] arr1, T[] arr2) {

            //    var ret = new List<int>();
            //    if (arr1 == arr2)
            //        return ret;

            //    int cnt1 = arr1.Length;
            //    int cnt2 = arr2.Length;
            //    int cnt = Math.Min(cnt1, cnt2);

            //    for (int ii = 0; ii < cnt; ++ii) {

            //        if (!EqualityComparer<T>.Default.Equals(arr1[ii], arr2[ii]))
            //            ret.Add(ii);

            //    }

            //    return ret;
            //}
            #endregion
            // First_Difference, Array-only version, commented out
            #region
            // public static int
            //First_Difference<T>(this T[] arr1, T[] arr2) {

            //    if (arr1 == arr2)
            //        return -1;

            //    int cnt1 = arr1.Length;
            //    int cnt2 = arr2.Length;
            //    int cnt = Math.Min(cnt1, cnt2);

            //    for (int ii = 0; ii < cnt; ++ii) {

            //        if (!EqualityComparer<T>.Default.Equals(arr1[ii], arr2[ii]))
            //            return ii;

            //    }

            //    return -1;
            //}
            #endregion

            if (seqs.All(seq => seq1 == seq))
                yield break;

            int ii = -1;

            foreach (var arr in seq1.Combine(seqs)) {
                ++ii;
                if (!arr.Same())
                    yield return Pair.Make(arr, ii);

            }


        }

        static public IEnumerable<T>
        Duplicates<T>(this IEnumerable<T> seq) {

            var forced = seq.Force();

            var groups = from f in forced
                         group f by f into g
                         where g.Count() > 1
                         select g.Key;

            return groups;

        }

        public static bool
        Piecewise_Equal<T>(this IEnumerable<T> seq1,
                                bool check_lengths,
                           params IEnumerable<T>[] seqs) {

            return seqs.All(seq => seq1.Piecewise_Equal(seq, check_lengths));

        }

        public static bool
        Piecewise_Equal<T>(this IEnumerable<T> enum1,
                                IEnumerable<T> enum2,
                                bool check_lengths) {
            // Array-only Piecewise_Equal, commented out
            #region
            //public static bool
            //Piecewise_Equal<T>(this T[] arr1, T[] arr2, bool check_lengths) {

            //    if (arr1 == arr2)
            //        return true;

            //    if ((arr1 == null) || (arr2 == null)) {
            //        return (arr1 == null) == (arr2 == null);
            //    }

            //    int cnt1 = arr1.Length;
            //    int cnt2 = arr2.Length;

            //    if (check_lengths && (cnt1 != cnt2))
            //        return false;

            //    int cnt = Math.Min(cnt1, cnt2);

            //    for (int ii = 0; ii < cnt; ++ii) {

            //        if (!arr1[ii].Safe_Equals(arr2[ii]))
            //            return false;

            //    }

            //    return true;

            //}
            #endregion

            if (enum1 == enum2)
                return true;

            if ((enum1 == null) || (enum2 == null)) {
                return (enum1 == null) == (enum2 == null);
            }

            using (var enumer2 = enum2.GetEnumerator()) {
                foreach (var elem1 in enum1) {

                    if (!enumer2.MoveNext()) {

                        if (check_lengths)
                            return false;

                        break;
                    }

                    if (!elem1.Safe_Equals(enumer2.Current))
                        return false;

                }
                if (check_lengths && enumer2.MoveNext())
                    return false;
            }

            return true;

        }
#if !NET20
        public static bool
        Set_Equal<T>(this IEnumerable<T> seq1,
                          IEnumerable<T> seq2) {

            var set1 = seq1 as Set<T> ?? new Set<T>(seq1);
            var set2 = seq2 as Set<T> ?? new Set<T>(seq2);

            for (int ii = 0; ii < 2; ++ii) {
                foreach (var elem in set1)
                    if (!set2.Contains(elem))
                        return false;
                H.Swap(ref set1, ref set2);
            }

            return true;
        }
#endif

        // ****************************


        /// [a0, a1.. am] -> [(a0, 0), (a1, 1), ..., (am, m)] 
        public static IEnumerable<Pair<int, T>>
        Ixed<T>(this IEnumerable<T> seq) {
            return seq.Ixed(0);
        }

        public static IEnumerable<Pair<int, T>>
        Ixed<T>(this IEnumerable<T> seq, int start) {
            int ii = start - 1;
            foreach (var elem in seq)
                yield return Pair.Make(++ii, elem);

        }

        public static IEnumerable<int>
        Ixs<T>(this IEnumerable<T> seq, T elem) {

            var ret = from pair in seq.Ixed()
                      where elem.Safe_Equals(pair.Second)
                      select pair.First;

            return ret;

        }

        public static IEnumerable<int>
        Ixs<T>(this IEnumerable<T> seq, Func<T, bool> f) {

            var ret = from pair in seq.Ixed()
                      where f(pair.Second)
                      select pair.First;

            return ret;
        }

        static IEnumerable<int>
        Distances(this IEnumerable<int> numbers) {

            int current = 0;

            foreach (var number in numbers) {

                yield return number - current;
                current = number;

            }

        }

        public static IEnumerable<T>
        Extract<T>(this IEnumerable<T> collection,
                        IEnumerable<int> indices) {

            // http://stackoverflow.com/questions/1018407/what-is-the-most-elegant-way-to-get-a-set-of-items-by-index-from-a-collection

            int currentIndex = -1;

            using (var collectionEnum = collection.GetEnumerator()) {

                foreach (int index in indices) {

                    while (collectionEnum.MoveNext()) {

                        ++currentIndex;

                        if (currentIndex == index) {

                            yield return collectionEnum.Current;
                            break;

                        }
                    }
                }
            }
        }

        static public IEnumerable<Pair<T, int>>
        Freq<T>(this IEnumerable<T> seq) {

            //seq = seq.Force();

            var groups = from elem in seq
                         group elem by elem into g
                         select Pair.Make(g.Key, g.Count());

            return groups;

        }

        static public IEnumerable<Pair<T, int>>
        RLE<T>(this IEnumerable<T> seq) {

            var forced = seq.Force();

            var last = forced[0];
            int num = 1;

            for (int ii = 1; ii < forced.Count; ++ii) {

                if (last.Safe_Equals(forced[ii])) {
                    ++num;
                    continue;
                }

                yield return Pair.Make(last, num);
                last = forced[ii];
                num = 1;

            }

            yield return Pair.Make(last, num);

        }

        static public IEnumerable<T>
        From_To<T>(this IEnumerable<T> seq, int from, int to) {
            return seq.Skip(from).Take(to - from + 1);
        }


        // ****************************

        public static IEnumerable<T>
        From_Enumerable<T>(this IEnumerable ienumerable) {
            return From_Enumerator<T>(ienumerable.GetEnumerator());
        }

        public static IEnumerable<T>
        From_Enumerator<T>(this IEnumerator enumer) {

            var dispose = (enumer as IDisposable) ?? On_Dispose.Nothing;
            using (dispose) {
                while (enumer.MoveNext())
                    yield return (T)enumer.Current;
            }

        }

        public static IEnumerable<T>
        From_Enumerator<T>(this IEnumerator<T> enumer) {
            return From_Enumerator<T>(enumer as IEnumerator);
        }

        // ****************************


        public static IEnumerable<T>
        Pend<T>(this IEnumerable<T> to, T elem, bool pre) {

            if (pre)
                yield return elem;

            foreach (var elem1 in to)
                yield return elem1;

            if (pre)
                yield break;

            yield return elem;
        }

        public static IEnumerable<T>
        Mesh<T>(this IEnumerable<T> seq, T elem) {
            return seq.Mesh(true, elem.Repeat());
        }

        // [[a0, a1...], [b0, b1..]] -> [a0, a1, b0, b1, b3, a2, a4 ...]
        public static IEnumerable<T>
        Mesh<T>(this IEnumerable<T> seq1, int from1,
                     IEnumerable<T> seq2, int from2) {

            using (var e1 = seq1.GetEnumerator())
            using (var e2 = seq2.GetEnumerator()) {

                var e_1 = e1;
                var e_2 = e2;
                while (true) {

                    for (int ii = 0; ii < from1; ++ii) {
                        if (!e_1.MoveNext())
                            yield break;
                        yield return e1.Current;
                    }

                    H.Swap(ref e_1, ref e_2);
                    H.Swap(ref from1, ref from2);

                }
            }
        }

        // [[a0, a1...], [b0, b1..], [c0, c1..]] -> [a0, b0, c0 ... a1, b1, c1 ...]
        public static IEnumerable<T>
        Mesh<T>(this IEnumerable<T> head,
                     bool stop_on_first_empty,
                     params IEnumerable<T>[] tail) {

            var etors = (from enumer in tail.Pend<IEnumerable<T>>(head, true)
                         select enumer.GetEnumerator()).Force();

            while (true) {
                bool ran = false;

                foreach (var etor in etors) {
                    if (etor.MoveNext()) {
                        ran = true;
                        yield return etor.Current;
                    }
                    else if (stop_on_first_empty)
                        yield break;
                }

                if (!ran)
                    yield break;
            }

        }



        // Needed b/c of lack of variance support
        public static IEnumerable<T>
        Flatten<T, TIn>(this IEnumerable<TIn> sequence)
            where TIn : IEnumerable<T> {
            foreach (var enumerable in sequence)
                foreach (var elem in enumerable)
                    yield return elem;
        }

        public static IEnumerable<T>
        Flatten<T>(this IEnumerable<IEnumerable<T>> sequence) {

            foreach (var enumerable in sequence)
                foreach (var elem in enumerable)
                    yield return elem;

        }


        public static IEnumerable<IEnumerable<T>>
        Chunk<T>(this IEnumerable<T> seq, int count) {

            var buffer = new List<T>(count);

            using (var enumerator = seq.GetEnumerator())

                while (enumerator.MoveNext()) {

                    for (int ii = 0; ii < count; ++ii) {
                        buffer.Add(enumerator.Current);

                        if (!enumerator.MoveNext())
                            break;
                    }

                    yield return buffer;
                }


        }

        /// <summary>
        /// [a0, a1.. am] -> [a0, an, a2n, ... akn] 
        /// where (how_many .lt. 0 || k .lt. how_many) and (k .le. m/n)
        /// </summary>
        public static IEnumerable<T>
        Every_Nth<T>(this IEnumerable<T> seq, int n, int? how_many) {

            var ret = from pair in seq.Ixed()
                      where pair.First % n == 0
                      select pair.Second;

            if (how_many.HasValue)
                return ret.Take(how_many.Value);
            else
                return ret;

            // Old code, commented out   
            //int current = 0;

            //foreach (var elem in seq) {

            //    if (how_many == 0)
            //        yield break;

            //    if (current == n) {
            //        current = 0;
            //        how_many--;

            //        yield return elem;

            //    }
            //    ++current;
            //}

        }






        // http://stackoverflow.com/questions/41319/checking-if-a-list-is-empty-with-linq

        public static bool
        Is_Empty<T>(this IEnumerable<T> collection) {

            collection.tifn();

            var as_icoll = collection as ICollection<T>;
            var as_icoll1 = collection as ICollection;

            bool ret;

            if (as_icoll != null)
                ret = (as_icoll.Count == 0);

            else if (as_icoll1 != null)
                ret = (as_icoll1.Count == 0);

            else
                ret = !collection.Any();

            return ret;

        }

        public static bool
        Same<T>(this IEnumerable<T> seq) {

            using (var enumer = seq.GetEnumerator()) {
                if (!enumer.MoveNext())
                    return true;

                var first = enumer.Current;

                while (enumer.MoveNext()) {
                    if (!first.Safe_Equals(enumer.Current))
                        return false;
                }

                return true;
            }

        }


        /// <summary>
        /// The last pair in the enumeration will have Second == true
        /// Useful when comma-splicing
        /// </summary>
        public static IEnumerable<Pair<T, bool>>
        Mark_Last<T>(this IEnumerable<T> source) {

            T elm;

            using (var enm = source.GetEnumerator()) {

                if (!enm.MoveNext())
                    yield break;

                elm = enm.Current;

                while (enm.MoveNext()) {

                    var pair = new Pair<T, bool>(elm, false);

                    elm = enm.Current;

                    yield return pair;

                }

                {
                    var pair = new Pair<T, bool>(elm, true);

                    yield return pair;
                }
            }

        }

        public static T
        FirstOrDefault<T>(this IEnumerable<T> seq,
                               T default_value) {

            foreach (var elem in seq) {
                return elem;
            }

            return default_value;
        }

        public static T
        FirstOrDefault<T>(this IEnumerable<T> seq,
                               Func<T, bool> predicate,
                               T default_value) {

            foreach (var elem in seq) {
                if (predicate(elem))
                    return elem;
            }

            return default_value;
        }

        // ****************************

        /// <summary>
        /// Forces
        /// </summary>
        public static IEnumerable<T>
        Pad_Left<T>(this IEnumerable<T> source,
                        int how_many,
                        T pad_with) {

            var ret = source.Force();
            int needed = how_many - ret.Count;

            if (needed > 0)
                ret.InsertRange(0, Enumerable.Repeat<T>(pad_with, needed));

            return ret;
        }

        public static IEnumerable<T>
        Pad_Right<T>(this IEnumerable<T> seq, int how_many, T pad_with) {

            //var ret = seq.Force();

            //int cnt = ret.Count;

            //if (cnt < how_many)
            //    ret = ret.AddRange(pad_with.Repeat(how_many - cnt));
            //return ret;


            int cnt = 0;

            foreach (var elem in seq) {
                yield return elem;
                ++cnt;
            }

            for (; cnt < how_many; ++cnt)
                yield return pad_with;

        }


        public static IEnumerable<T>
        Pad_Trim<T>(this IEnumerable<T> seq, int how_many, T pad_with, bool pad_left) {

            var taken = seq.Take(how_many);

            var ret = pad_left ? seq.Pad_Left(how_many, pad_with)
                               : seq.Pad_Right(how_many, pad_with);

            return ret;

        }


        // ****************************

        /// [a, b, c, d...] -> [(a,b),(b,c),(c,d)...]
        public static IEnumerable<Pair<T>>
        All_Pairs<T>(this IEnumerable<T> seq) {

            foreach (T var in seq) {


                T last = seq.FirstOrDefault();

                foreach (T elem in seq.Skip(1)) {

                    yield return new Pair<T>(last, elem);
                    last = elem;

                }
            }
        }

        // [a, b, c, d...] -> [(a,b),(c,d)...]
        public static IEnumerable<Pair<T>>
        Take_Pairs<T>(this IEnumerable<T> sequence) {

            // Take_Pairs with Action<T,T> argument, commented out
            #region
            //public static void
            //Take_Pairs<T>(this IEnumerable<T> sequence, Action<T, T> action) {

            //    bool second = false;
            //    T first = default(T);

            //    foreach (T var in sequence) {

            //        if (second) {
            //            action(first, var);
            //            second = false;
            //        }
            //        else {
            //            first = var;
            //            second = true;
            //        }
            //    }
            //}
            #endregion
            // Take_Pairs with Func<TSource, TSource, TProduct> argument, commented out
            #region
            //            public static IEnumerable<TProduct>
            //Take_Pairs<TSource, TProduct>(this IEnumerable<TSource> sequence,
            //                                   Func<TSource, TSource, TProduct> producer) {

            //    bool second = false;
            //    TSource first = default(TSource);
            //    foreach (TSource var in sequence) {

            //        if (second) {
            //            yield return producer(first, var);
            //            second = false;
            //        }
            //        else {
            //            first = var;
            //            second = true;
            //        }

            //    }
            //}
            #endregion

            bool second = false;
            T first = default(T);

            foreach (T var in sequence) {

                if (second) {
                    yield return new Pair<T>(first, var);
                    second = false;
                }
                else {
                    first = var;
                    second = true;
                }

            }
        }

        // [a1,a2,a3,a4,a5,a6...] -> [a1,a2,a6,a7,...] with take = 2, skip = 3

        public static IEnumerable<T>
        Take_Skip<T>(this IEnumerable<T> seq,
                       int take,
                       int skip) {

            using (var enumer = seq.GetEnumerator()) {

                while (true) {
                    for (int ii = 0; ii < take; ++ii) {
                        if (!enumer.MoveNext())
                            yield break;
                        yield return enumer.Current;
                    }
                    for (int ii = 0; ii < skip; ++ii) {
                        if (!enumer.MoveNext())
                            yield break;
                    }
                }
            }

        }


        // ****************************


        public static IEnumerable<Pair<T1, T2>>
        Pairs<T1, T2>(this IEnumerable<T1> seq,
                           Func<T1, T2> prod) {

            foreach (var elem in seq)
                yield return Pair.Make(elem, prod(elem));

        }

        public static IEnumerable<Triple<T1, T2, T3>>
        Triples<T1, T2, T3>(this IEnumerable<T1> seq,
                                 Func<T1, T2> prod1,
                                 Func<T1, T3> prod2) {

            foreach (var elem in seq)
                yield return Triple.Make(elem, prod1(elem), prod2(elem));

        }

        public static IEnumerable<Quad<T1, T2, T3, T4>>
        Quads<T1, T2, T3, T4>(this IEnumerable<T1> seq,
                                   Func<T1, T2> prod1,
                                   Func<T1, T3> prod2,
                                   Func<T1, T4> prod3) {

            foreach (var elem in seq)
                yield return Quad.Make(elem, prod1(elem), prod2(elem), prod3(elem));

        }

        // [a1, a2, a3...] -> [[a1, b1, c1, d1],[a2,b2,c2,d2],...]
        public static IEnumerable<IEnumerable<T>>
        Table_ID<T>(this IEnumerable<T> seq,
                         params Func<T, T>[] fs) {

            return seq.Table(fs.Pend(_ => _, true).arr());

        }

        ///[a1, a2, a3...] -> [[b1, c1, d1],[b2,c2,d2],...]
        public static IEnumerable<IEnumerable<TResult>>
        Table<T, TResult>(this IEnumerable<T> seq,
                          params Func<T, TResult>[] fs) {

            foreach (var elem in seq) {
                var temp = elem;
                yield return from f in fs
                             select f(temp);
            }

        }


        // ****************************



        /// [a, a, a, x, a, x, x, a, x] -> [[a,a,a],[x],[a],[x,x],[a],[x]]
        /// Like group based on an inverted predicate
        public static IEnumerable<IEnumerable<T>>
        Break<T>(this IEnumerable<T> source,
                      Func<T, T, bool> to_split) {

            var enumer = source.Force();

            int cnt = enumer.Count();

            if (cnt == 0) {
                yield break;
            }
            else if (cnt == 1) {
                yield return new List<T>(enumer);
                yield break;
            }

            var list = new List<T>();

            T prev = enumer[0];
            list.Add(prev);

            foreach (var elem in enumer.Skip(1)) {

                if (to_split(prev, elem)) {

                    if (list.Count > 0) {
                        yield return list;
                        list.Clear();
                    }

                }

                list.Add(elem);

                prev = elem;
            }

            if (list.Count > 0)
                yield return list;
        }

        public static IEnumerable<IEnumerable<T>>
        Split<T>(this IEnumerable<T> collection,
                      bool return_empties,
                      params T[] break_on) {
            Func<T, bool>
            breaker =
            elem1 =>
            {
                bool equals = break_on.Contains(elem1);
                return equals;
            };

            return Split(collection, breaker, return_empties);

        }

        /// <summary>
        /// Does not return trailing or leading empty enumerations
        /// </summary>
        public static IEnumerable<IEnumerable<T>>
        Split<T>(this IEnumerable<T> collection,
                      Func<T, bool> condition,
                      bool return_empties) {
            var list1 = new List<T>();
            bool initial = true;

            foreach (T elem in collection) {

                var result = condition(elem);

                if (result) {
                    if (!initial)
                        if (return_empties || list1.Count() > 0) {

                            yield return list1;
                            list1 = new List<T>();
                        }
                }
                else
                    list1.Add(elem);

                initial = false;
            }


            if (list1.Count() > 0) {
                yield return list1;
            }
        }






        // ****************************



        public static Dictionary<TKey, TValue>
        Zip_To_Dict<TKey, TValue>(this IEnumerable<TKey> keys,
                                     IEnumerable<TValue> values) {

            var lst_keys = keys.Force();
            var lst_values = values.Force();

            int cnt = lst_keys.Count;

            (cnt == lst_values.Count).tiff();

            if (cnt == 0)
                return new Dictionary<TKey, TValue>(0);

            var ret = new Dictionary<TKey, TValue>(cnt);

            using (var key_enumer = lst_keys.GetEnumerator())
            using (var value_enumer = lst_values.GetEnumerator()) {

                while (key_enumer.MoveNext() && value_enumer.MoveNext()) {

                    ret.Add(key_enumer.Current, value_enumer.Current);

                }

            }

            return ret;


        }


        public static IEnumerable<TResult>
        Zip_With<T1, T2, TResult>(this IEnumerable<T1> seq1,
                                     IEnumerable<T2> seq2,
                                     Func<T1, T2, TResult> selector) {
            seq1.tifn();
            seq2.tifn();
            selector.tifn();

            return Zip_With_Inner(seq1, seq2, selector);

        }

        static IEnumerable<TResult>
        Zip_With_Inner<T1, T2, TResult>(this IEnumerable<T1> first,
                                  IEnumerable<T2> second,
                                  Func<T1, T2, TResult> selector) {

            using (IEnumerator<T1> e1 = first.GetEnumerator()) {

                using (IEnumerator<T2> e2 = second.GetEnumerator())

                    while (e1.MoveNext() && e2.MoveNext())
                        yield return selector(e1.Current, e2.Current);

            }
        }

        public static IEnumerable<TResult>
        Zip_With<T1, T2, T3, TResult>(this IEnumerable<T1> seq1,
                                         IEnumerable<T2> seq2,
                                         IEnumerable<T3> seq3,
                                         Func<T1, T2, T3, TResult> selector) {
            seq1.tifn();
            seq2.tifn();
            seq3.tifn();
            selector.tifn();

            var ret = seq1.Zip(seq2, seq3).Select(t => selector(t.First, t.Second, t.Third));
            return ret;
        }

        public static IEnumerable<TResult>
        Zip_With<T1, T2, T3, T4, TResult>(this IEnumerable<T1> seq1,
                                         IEnumerable<T2> seq2,
                                         IEnumerable<T3> seq3,
                                         IEnumerable<T4> seq4,
                                         Func<T1, T2, T3, T4, TResult> selector) {
            seq1.tifn();
            seq2.tifn();
            seq3.tifn();
            seq4.tifn();
            selector.tifn();

            var ret = seq1.Zip(seq2, seq3, seq4)
                         .Select(q => selector(q.First, q.Second, q.Third, q.Fourth));

            return ret;
        }




        // ****************************


        public static IEnumerable<Pair<T1, T2>>
        Zip<T1, T2>(this IEnumerable<T1> first,
                        IEnumerable<T2> second) {

            return Zip_With<T1, T2, Pair<T1, T2>>(first, second, Pair.Make);

        }




        public static IEnumerable<Triple<T1, T2, T3>>
        Zip<T1, T2, T3>(this IEnumerable<T1> seq1,
                                   IEnumerable<T2> seq2,
                                   IEnumerable<T3> seq3) {


            using (var e1 = seq1.GetEnumerator())
            using (var e2 = seq2.GetEnumerator())
            using (var e3 = seq3.GetEnumerator())

                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
                    yield return Triple.Make(e1.Current, e2.Current, e3.Current);


        }

        public static IEnumerable<Quad<T1, T2, T3, T4>>
        Zip<T1, T2, T3, T4>(this IEnumerable<T1> seq1,
                           IEnumerable<T2> seq2,
                           IEnumerable<T3> seq3,
                           IEnumerable<T4> seq4) {



            using (var e1 = seq1.GetEnumerator())
            using (var e2 = seq2.GetEnumerator())
            using (var e3 = seq3.GetEnumerator())
            using (var e4 = seq4.GetEnumerator())


                while (e1.MoveNext() && e2.MoveNext() &&
                       e3.MoveNext() && e4.MoveNext())
                    yield return Quad.Make(e1.Current, e2.Current, e3.Current, e4.Current);


        }

        // ****************************

        static public Pair<IEnumerable<T1>, IEnumerable<T2>>

        SplitUp<T1, T2>(this IEnumerable<Pair<T1, T2>> seq) {
            return Pair.Make(seq.Select(p => p.First),
                            seq.Select(p => p.Second));
        }

        static public Triple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>
        SplitUp<T1, T2, T3>(this IEnumerable<Triple<T1, T2, T3>> seq) {
            return Triple.Make(seq.Select(p => p.First),
                             seq.Select(p => p.Second),
                             seq.Select(p => p.Third));
        }

        static public Quad<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>
        SplitUp<T1, T2, T3, T4>(this IEnumerable<Quad<T1, T2, T3, T4>> seq) {
            return Quad.Make(seq.Select(p => p.First),
                             seq.Select(p => p.Second),
                             seq.Select(p => p.Third),
                             seq.Select(p => p.Fourth));

        }


        // ****************************


        ///
        // http://stackoverflow.com/questions/523194/parallel-iteration-in-c

        /// Transposes the sequences
        public static IEnumerable<T[]>
        Combine<T>(this IEnumerable<IEnumerable<T>> seqs) {

            //var ints = from seq1 in Integers.From_To(1, 100)
            //                        .Table_ID(ii => ii + 1, 
            //                                  ii => ii * 10, 
            //                                  ii => ii * ii)
            //                        .Combine()

            return seqs.First().Combine(seqs.Skip(1).arr());

        }

        public static IEnumerable<T[]>
        Combine<T>(this IEnumerable<T> seq, params IEnumerable<T>[] seqs) {

            var all = seqs.Pend(seq, true);

            IEnumerator<T>[] enums = null;

            try {

                var temp = from s in all
                           select s.GetEnumerator();

                enums = temp.ToArray();

                while (enums.All(e => e.MoveNext())) {

                    var result = enums.Select(e => e.Current).ToArray();
                    yield return result;

                }

            }
            finally {

                foreach (var @enum in enums)
                    @enum.Try_Dispose();

            }
        }


        /// <summary>Only enumerates until the end of the shorter collection by default
        /// Lazy
        /// </summary>
        public static IEnumerable<Pair<T1, T2>>
        Combine<T1, T2>(this IEnumerable<T1> seq1,
                             IEnumerable<T2> seq2,
                             bool throw_on_unequal) {
            seq1.tifn("First");
            seq2.tifn("Second");

            bool move1 = false;

            using (var e1 = seq1.GetEnumerator())
            using (var e2 = seq2.GetEnumerator()) {

                while ((move1 = e1.MoveNext()) && e2.MoveNext())
                    yield return new Pair<T1, T2>(e1.Current, e2.Current);


                if (throw_on_unequal) {
                    /* if the first one has moved but the other one has not
                       OR 
                       if the first one has not moved but the second can */
                    if (move1 || e2.MoveNext()) {

                        string left = move1 ? "First" : "Second";
                        string right = !move1 ? "first" : "second";

                        true.tift<ApplicationException>
                        (left + " sequence longer than the " + right);

                    }
                }
            }
        }


        // ****************************



        public static IEnumerable<T>
        Force_First<T>(this IEnumerable<T> sequence) {

            T first = sequence.First();

            return sequence;
        }

        public static List<T>
        Force<T>(this IEnumerable<T> sequence) {

            var ret = sequence.ToList();

            return ret;
        }

        public static IEnumerable<T>
        Force2<T>(this IEnumerable<T> sequence) {

            foreach (T elem in sequence) ;

            return sequence;

        }

        public static IEnumerable<T>
        Force<T>(this IEnumerable<T> sequence, int max_elements) {

            var seq1 = sequence.Take(max_elements).ToList();

            return seq1.Concat(sequence.Skip(max_elements));
        }

        public static IEnumerable<T>
        Except<T>(this IEnumerable<T> sequence, params T[] to_skip) {

            int len = to_skip.Length;

            if (len == 0) {
                foreach (var elem in sequence)
                    yield return elem;

                yield break;
            }


            bool use_hs = len > 10;

            var hashset = use_hs ? new Set<T>(sequence) : null;

            foreach (var elem in sequence) {

                if (use_hs) {
                    if (hashset.Contains(elem))
                        continue;
                }
                else if (to_skip.Contains(elem))
                    continue;

                yield return elem;

            }

        }

        /* [fish,talk,car,bright,equal,splice] -->
                {adj => [bright, equal]}
                {verb => [talk, splice]}
                {noun => [fish, car]} */
        public static Dictionary<TKey, List<TValue>>
        Sift<TKey, TValue>(this IEnumerable<TValue> seq,
                                Func<TValue, TKey> f) {

            var ret = new Dictionary<TKey, List<TValue>>();

            foreach (var val in seq) {
                var key = f(val);
                ret.List_Modify(key, val);
            }

            return ret;

        }

        /* [[john,mike,angel],[jimmy,moses,alex],[jeremy,mary,albert]] -->
                [jimmy,mike,angel] */
        public static IEnumerable<T>
        Pick<T>(this IEnumerable<T> seq,
                     Func<T[], int, T> f,
                     params IEnumerable<T>[] seqs) {

            var ret = from pair in seq.Combine(seqs).Ixed()
                      select f(pair.Second, pair.First);

            return ret;
        }


        // ****************************

        public static IEnumerable<Pair<T1, T2>>
        Combos<T1, T2>(this IEnumerable<T1> seq1,
                   IEnumerable<T2> seq2) {
            foreach (var elem1 in seq1)
                foreach (var elem2 in seq2)
                    yield return Pair.Make(elem1, elem2);
        }

        public static IEnumerable<Triple<T1, T2, T3>>
        Combos<T1, T2, T3>(this IEnumerable<T1> seq1,
                               IEnumerable<T2> seq2,
                               IEnumerable<T3> seq3) {
            foreach (var elem1 in seq1)
                foreach (var elem2 in seq2)
                    foreach (var elem3 in seq3)
                        yield return Triple.Make(elem1, elem2, elem3);
        }


        public static IEnumerable<Quad<T1, T2, T3, T4>>
        Combos<T1, T2, T3, T4>(this IEnumerable<T1> seq1,
                                   IEnumerable<T2> seq2,
                                   IEnumerable<T3> seq3,
                                   IEnumerable<T4> seq4) {
            foreach (var elem1 in seq1)
                foreach (var elem2 in seq2)
                    foreach (var elem3 in seq3)
                        foreach (var elem4 in seq4)
                            yield return Quad.Make(elem1, elem2, elem3, elem4);
        }


        // ****************************
    }
}
