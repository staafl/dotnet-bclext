using System;
using System.Collections.Generic;

namespace Fairweather.Service
{
    /// <summary>
    /// Basically, a list with efficient 'Contains' (and sometimes 'Count') operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="THash"></typeparam>
    public class Hashlist<T, THash> : List<T>, IRead<int, T>, IReadWrite<int, T>
    {

        public Hashlist()
            : this(Convert) {

        }

        public Hashlist(int capacity)
            : this(Convert, capacity) {

        }

        public Hashlist(IEnumerable<T> collection)
            : base(collection) {
            counter = new Counter<THash>();

        }



        public Hashlist(Func<T, THash> f)
            : this(f, 0) {
            counter = new Counter<THash>();

        }


        public Hashlist(Func<T, THash> f, int capacity)
            : base(capacity) {

            f.tifn();
            this.f = f;
            counter = new Counter<THash>(capacity);
        }

        public Hashlist(Func<T, THash> f, IEnumerable<T> collection)
            : base(collection) {
            f.tifn();
            this.f = f;
            counter = new Counter<THash>();

        }






        static THash Convert(T arg) {
            return (THash)(object)arg;
        }

        // ****************************


        public new void Clear() {
            base.Clear();
            counter.Clear();
        }

        public bool Contains_Item(T key) {
            return counter[f(key)] > 0;
        }

        /// <summary>
        /// Only of real value if the hashing function is injective.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int Counter_State(T key) {
            return counter[f(key)];
        }

        public bool Contains(int ix) {
            return ix >= 0 && ix < Count;
        }

        public new bool Remove(T key) {

            counter.Dec(f(key));

            base.Remove(key);

            return true;

        }

        public new T this[int ix] {
            get { return base[ix]; }
            set {
                var old = base[ix];
                if (old.Safe_Equals(value))
                    return;

                counter.Dec(f(old));
                counter.Inc(f(value));

                base[ix] = value;
            }
        }

        // IEnumerable


        // ****************************


        readonly Func<T, THash> f;
        readonly Counter<THash> counter;



    }
}
