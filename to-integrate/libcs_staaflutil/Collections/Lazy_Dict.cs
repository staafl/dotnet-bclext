using System;
using System.Collections.Generic;
using System.Linq;

namespace Fairweather.Service
{
    /// <summary>
    /// Indexing concurrently without synchronization may cause the producer to
    /// be invoked more than once.
    /// </summary>
    public class Lazy_Dict<TKey, TValue> : Dictionary<TKey, TValue>
    {

        public Lazy_Dict(Func<TKey, TValue> producer, IDictionary<TKey, TValue> seq)
            : base(seq) {

            (producer).tifn<ArgumentNullException>("producer");

            this.Producer = producer;


        }

        public Lazy_Dict(Func<TKey, TValue> producer, IEnumerable<Pair<TKey, TValue>> seq)
            : base(seq.Count()) {

            (producer).tifn<ArgumentNullException>("producer");

            this.Producer = producer;
            foreach (var pair in seq)
                this.Add(pair.First, pair.Second);

        }

        public Lazy_Dict(Func<TKey, TValue> producer)
            : base(0) {

            (producer).tifn<ArgumentNullException>("producer");
            this.Producer = producer;

        }

        public Lazy_Dict(Func<TKey, TValue> producer, int capacity)
            : base(capacity) {

            (producer).tifn<ArgumentNullException>("producer");
            this.Producer = producer;

        }



        // ****************************


        public Func<TKey, TValue> Producer {
            get;
            set;
        }


        public new TValue this[TKey key] {
            get {
#if SERIAL
                lock (locker)
#endif
                return this.Get_Or_Default(key, () => Producer(key));
            }
            set {
#if SERIAL
                lock (locker)
#endif
                base[key] = value;
            }
        }


        // ****************************


#if SERIAL
        readonly object locker = new object();
#endif
    }
}