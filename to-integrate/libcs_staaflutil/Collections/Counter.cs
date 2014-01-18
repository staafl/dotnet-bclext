using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fairweather.Service
{
    /// <summary>
    /// Can also be implemented as a Lazy_Dict[T,int] with a producer () => 0.
    /// </summary>
    public class Counter<T> : IRead<T, int>, IReadWrite<T, int>, IEnumerable<Pair<T, int>>
    {

        public Counter() {
            dict = new Dictionary<T, int>();
        }

        public Counter(int capacity)
            : this(capacity, false) {
        }

        public Counter(int capacity, bool allow_negative) {
            dict = new Dictionary<T, int>(capacity);
            this.Allow_Negative = allow_negative;
        }

        public Counter(IEnumerable<T> values, bool allow_negative)
            : this(values.Maybe_Count().OrDefault(() => 0).Value) {

            foreach (var val in values)
                Inc(val);
        }



        // ****************************




        public bool Allow_Negative {
            get;
            set;
        }

        public int Total_Count {
            get {
                var ret = dict.Sum(kvp => kvp.Value);
                return ret;
            }
        }




        public int Inc(T key) {
            return Add(key, 1);
        }

        public int Dec(T key) {
            return Add(key, -1);
        }

        public int Add(T key, int count) {
            int cnt;

            if (!dict.TryGetValue(key, out cnt))
                cnt = 0;

            cnt += count;

            if (cnt == 0 || (!Allow_Negative && cnt < 0))
                dict.Remove(key);
            else
                dict[key] = cnt;

            return cnt;
        }

        public int Tran(T from, T to, int count, bool allow_overflow) {

            int old_from = this[from];

            var new_from = old_from - count;

            var to_add = count;

            if (new_from < 0 && !Allow_Negative) {

                if (!allow_overflow)
                    to_add = old_from;

                new_from = 0;

            }

            if (old_from != new_from)
                this[from] = new_from;

            return Add(to, to_add);


        }


        public int this[T key] {
            get {
                int cnt;
                if (!dict.TryGetValue(key, out cnt))
                    cnt = 0;
                return cnt;
            }
            set {
                if (value == 0 || (!Allow_Negative && value < 0))
                    dict.Remove(key);
                else
                    dict[key] = value;
            }
        }


        public void Clear() {

            dict.Clear();
        }

        public int Entry_Count {
            get {
                return dict.Count;
            }
        }

        public bool Contains(T key) {
            return this[key] > 0;
        }

        public T Min() {
            int min_cnt = 0;
            T min_t = default(T);
            foreach (var pair in this) {
                if (pair.Second <= min_cnt) {
                    min_cnt = pair.Second;
                    min_t = pair.First;
                }
            }
            return min_t;
        }


        public IEnumerable<T> Entries {
            get {
                return dict.Keys;
            }
        }

        public IEnumerator<Pair<T, int>> GetEnumerator() {
            return dict.pairs().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }





        // ****************************


        readonly Dictionary<T, int> dict;


    }
}
