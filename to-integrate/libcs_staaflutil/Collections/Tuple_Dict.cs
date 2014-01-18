using System.Collections.Generic;

namespace Fairweather.Service
{

    public class Pair_Dict<TKey, T1, T2> : Dictionary<TKey, Pair<T1, T2>>
    {
        public Pair_Dict() { }
        public Pair_Dict(int capacity) : base(capacity) { }
        public Pair_Dict(IDictionary<TKey, Pair<T1, T2>> idict) : base(idict) { }

        public void Add(TKey key, T1 arg1, T2 arg2) {

            var pair = Pair.Make(arg1, arg2);
            base.Add(key, pair);

        }
    }

    public class Triple_Dict<TKey, T1, T2, T3> : Dictionary<TKey, Triple<T1, T2, T3>>
    {
        public Triple_Dict() { }
        public Triple_Dict(int capacity) : base(capacity) { }
        public Triple_Dict(IDictionary<TKey, Triple<T1, T2, T3>> idict) : base(idict) { }

        public void Add(TKey key, T1 arg1, T2 arg2, T3 arg3) {

            var triple = Triple.Make(arg1, arg2, arg3);
            base.Add(key, triple);

        }
    }
    public class Quad_Dict<TKey, T1, T2, T3, T4> : Dictionary<TKey, Quad<T1, T2, T3, T4>>
    {
        public Quad_Dict() { }
        public Quad_Dict(int capacity) : base(capacity) { }
        public Quad_Dict(IDictionary<TKey, Quad<T1, T2, T3, T4>> idict) : base(idict) { }

        public void Add(TKey key, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {

            var quad = Quad.Make(arg1, arg2, arg3, arg4);
            base.Add(key, quad);

        }
    }
}

