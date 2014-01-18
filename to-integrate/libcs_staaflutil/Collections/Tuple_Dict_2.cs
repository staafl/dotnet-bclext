using System.Collections.Generic;
using System.Linq;
namespace Fairweather.Service
{
    public class Pair_Key_Dict<T1, T2, TValue> : Dictionary<Pair<T1, T2>, TValue>
    {
        public Pair_Key_Dict() { }
        public Pair_Key_Dict(int capacity) : base(capacity) { }
        public Pair_Key_Dict(IDictionary<Pair<T1, T2>, TValue> idict) : base(idict) { }

        public void Add(T1 key1, T2 key2, TValue value) {

            var pair = Pair.Make(key1, key2);
            base.Add(pair, value);

        }

        public TValue this[T1 key1, T2 key2] {
            get {
                return this[Pair.Make(key1, key2)];
            }
            set {
                this[Pair.Make(key1, key2)] = value;
            }
        }

        public bool TryGetValue(T1 key1, T2 key2, out TValue value) {

            var ret = TryGetValue(Pair.Make(key1, key2), out value);
            return ret;

        }

        public Dictionary<T2, TValue> Slice(T1 key1) {
            return this.Where(_kvp => _kvp.Key.First.Safe_Equals(key1))
                       .ToDictionary(_kvp => _kvp.Key.Second,
                                     _kvp => _kvp.Value);
        }
    }

    public class Triple_Key_Dict<T1, T2, T3, TValue> : Dictionary<Triple<T1, T2, T3>, TValue>
    {
        public Triple_Key_Dict() { }
        public Triple_Key_Dict(int capacity) : base(capacity) { }
        public Triple_Key_Dict(IDictionary<Triple<T1, T2, T3>, TValue> idict) : base(idict) { }

        public void Add(T1 key1, T2 key2, T3 key3, TValue value) {

            var triple = Triple.Make(key1, key2, key3);
            base.Add(triple, value);

        }

        public TValue this[T1 key1, T2 key2, T3 key3] {
            get {
                return this[Triple.Make(key1, key2, key3)];
            }
            set {
                this[Triple.Make(key1, key2, key3)] = value;
            }
        }

        public bool TryGetValue(T1 key1, T2 key2, T3 key3, out TValue value) {

            var ret = TryGetValue(Triple.Make(key1, key2, key3), out value);
            return ret;

        }
    }
    public class Quad_Key_Dict<T1, T2, T3, T4, TValue> : Dictionary<Quad<T1, T2, T3, T4>, TValue>
    {
        public Quad_Key_Dict() { }
        public Quad_Key_Dict(int capacity) : base(capacity) { }
        public Quad_Key_Dict(IDictionary<Quad<T1, T2, T3, T4>, TValue> idict) : base(idict) { }

        public void Add(T1 key1, T2 key2, T3 key3, T4 key4, TValue value) {

            var quad = Quad.Make(key1, key2, key3, key4);
            base.Add(quad, value);

        }

        public TValue this[T1 key1, T2 key2, T3 key3, T4 key4] {
            get {
                return this[Quad.Make(key1, key2, key3, key4)];
            }
            set {
                this[Quad.Make(key1, key2, key3, key4)] = value;
            }
        }

        public bool TryGetValue(T1 key1, T2 key2, T3 key3, T4 key4, out TValue value) {

            var ret = TryGetValue(Quad.Make(key1, key2, key3, key4), out value);
            return ret;

        }
    }
}
