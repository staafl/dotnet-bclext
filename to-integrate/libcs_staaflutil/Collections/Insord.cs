using System;
using System.Collections.Generic;
using System.Linq;

namespace Fairweather.Service
{
    /* A dictionary that keeps the inserted key-value pairs in the order of
     * insertion.
     * Naturally, the advertised property is only valid if the user keeps to the new
     * or shadowing methods.        */

    public class Insord<TK, TV> : Dictionary<TK, TV>, IRead<TK, TV>, IReadWrite<TK, TV>
    {

        readonly List<TK> lst = new List<TK>();
        /*       invariants:        */
        // lst.Count == this.Count
        // lst.Duplicates.Count == 0
        // this.Remove(key) == lst.Remove(key)

        public Insord()
            : base(0) {


        }

        public Insord(IDictionary<TK, TV> seq)
            : base(seq) {



        }

        public Insord(IEnumerable<Pair<TK, TV>> seq)
            : base(seq.Maybe_Count(0)) {

            foreach (var pair in seq)
                this.Add(pair.First, pair.Second);

        }



        public Insord(int capacity)
            : base(capacity) {


        }



        // ****************************

        public TK
        Key_By_Order(int ix) {
            return lst[ix];
        }

        public KeyValuePair<TK, TV>
        By_Order(int ix) {
            var key = lst[ix];
            return this.KVP(key);
        }

        public IEnumerable<TK>
        Keys_Order {
            get {
                return lst;
            }
        }

        public IEnumerable<KeyValuePair<TK, TV>>
        Order {
            get {
                return lst.Select(key => this.KVP(key));
            }
        }


        public int?
        Where(TK key) {

            for (int ii = 0; ii < lst.Count; ++ii) {
                if (lst[ii].Safe_Equals(key))
                    return ii;
            }

            return null;

        }


        public new void Add(TK key, TV value) {
            base.Add(key, value);
            lst.Add(key);
        }

        public new bool Remove(TK key) {

            var ret = base.Remove(key);

            if (ret)
                lst.Remove(key).tiff();

            return ret;

        }

        public new TV this[TK key] {
            get {
                return base[key];
            }
            set {
                Remove(key);
                Add(key, value);
            }
        }

        public bool Contains(TK key) {
            return ContainsKey(key);
        }

        public int? Move(TK key, int how_much) {

            var null_where = Where(key);

            if (null_where == null || how_much == 0)
                return null_where;

            int where = null_where.Value;

            int to = where + how_much;

            lst.RemoveAt(where);
            lst.Insert(to, key);
            return to;

        }

        // ****************************


    }
}
