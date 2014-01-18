using System;
using System.Collections.Generic;

namespace Fairweather.Service
{
    public static class Write_Buffer
    {
        public static Write_Buffer<IReadWrite<TK, TV>, TK, TV>
        Make<TC, TK, TV>(TC inner)
        where TC : IReadWrite<TK, TV> {
            return Make<TC, TK, TV>(inner);
        }

        public static Write_Buffer<TC, TK, TV>
        Make<TC, TK, TV>(TC inner, int buffer_capacity)
            where TC : IReadWrite<TK, TV> {

            return new Write_Buffer<TC, TK, TV>(inner, buffer_capacity);

        }

        public static Write_Buffer<TC2, TK, TV>
        Copy<TC, TC2, TK, TV>(Write_Buffer<TC, TK, TV> original,
                              TC2 new_inner)
            where TC2 : TC, IReadWrite<TK, TV>
            where TC : IReadWrite<TK, TV> {
            return original.Copy(new_inner);
        }
    }

    /// <summary>
    /// This class will replay its index assignments with respect
    /// to the inner class, preserving their order.
    /// Not threadsafe.
    /// Duplicate assignments will be coalesced (assigning [1]=2 and [1]=3 is equiv to [1]=3).
    /// Good for uint.MaxValue assignments.
    /// </summary>
    /// <typeparam name="TC"></typeparam>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public class Write_Buffer<TC, TK, TV> : IReadWrite<TK, TV>
        where TC : IReadWrite<TK, TV>
    {
        public Write_Buffer(TC inner)
            : this(inner, 0) {
        }
        public Write_Buffer(TC inner,
                            int buffer_capacity) {

            buffer = new Dictionary<TK, TV>(buffer_capacity);
            order = new Twoway<TK, int>(buffer_capacity);
            this.inner = inner;
            counter = int.MinValue;
        }

        Write_Buffer(TC inner,
                     Dictionary<TK, TV> buffer,
                     Twoway<TK, int> order,
                     int counter) {

            this.inner = inner;
            this.buffer = buffer;
            this.order = order;
            this.counter = counter;

        }

        public Write_Buffer<TC2, TK, TV>
        Copy<TC2>(TC2 new_inner)
            where TC2 : TC {
            return new Write_Buffer<TC2, TK, TV>(new_inner, buffer, order, counter);
        }

        public TV this[TK ix] {
            get {
                return buffer[ix];
            }
            set {
                buffer[ix] = value;
                //if (coalesce)
                    order[ix] = counter++; // Interlocked.Increment(ref counter);
                //else
                //    order.Add(Pair.Make(ix, value)); // order :: List<Pair<TKey,TValue>>();
            }
        }

        public bool TryGetValue(TK ix, out TV value) {

            if (buffer.TryGetValue(ix, out value))
                return true;

            if (inner.Contains(ix)) {
                value = inner[ix];
                return true;
            }

            return false;

        }

        public bool Remove(TK ix) {
            return buffer.Remove(ix);
        }

        public int Replay() {

            int ret = 0;

            var lst = new List<Pair<TK, TV>>(counter);

            for (int ii = 0; ii < counter; ++ii) {

                TK key;
                if (!order.TryGetValue(ii, out key))
                    continue;

                TV value;
                if (!buffer.TryGetValue(key, out value))
                    throw new InvalidOperationException();

                lst.Add(Pair.Make(key, value));
                ++ret;
            }

            foreach (var pair in lst)
                inner[pair.First] = pair.Second;

            return ret;

        }

        public bool Contains(TK ix) {
            return buffer.ContainsKey(ix);
        }

        public void Reset() {
            buffer.Clear();
            order.Clear();
            counter = 0;
        }

        public int Count {
            get {
                return buffer.Count;
            }
        }

        // ****************************


        readonly Dictionary<TK, TV> buffer;
        readonly Twoway<TK, int> order;
        readonly TC inner;

        int counter;

    }
}
