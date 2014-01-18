using System;
using System.Collections;

using System.Collections.Generic;

namespace Fairweather.Service
{
    public class
        Synonyms<TK, TV>
        : IDictionary<TK, TV>, IReadWrite<TK, TV>, IRead<TK, TV>
    {

        public Synonyms() {

            syns = new Dictionary<TK, TK>();
            vals = new Dictionary<TK, TV>();

        }


        public Synonyms(int capacity) {

            syns = new Dictionary<TK, TK>(capacity);
            vals = new Dictionary<TK, TV>(capacity);

        }


        public Synonyms(IDictionary<TK, TV> dict) {

            vals = new Dictionary<TK, TV>(dict);
            syns = new Dictionary<TK, TK>(dict.Count);
            syns.rw().Fill(dict.Keys.Pairs<TK, TK>(_=>_));

        }



        // TODO: replace with a real tree structure   
        readonly Dictionary<TK, TK> syns;

        readonly Dictionary<TK, TV> vals;

        object obj = new object();

        public object Lock_Root {
            get {
                return obj;
            }
        }


        // IDictionary<TK,TV>

        // ****************************

        public bool
        ContainsKey(TK key) {

            return syns.ContainsKey(key);
            //if (vals.ContainsKey(key))
            //    return true;


            //if (!Get_Root(key, out key))
            //    return false;

            //if (vals.ContainsKey(key))
            //    return true;

            //return true;
        }

        public bool
        ContainsValue(TV value) {
            return vals.ContainsValue(value);
        }

        public bool
        TryGetValue(TK key, out TV val) {

            // optimize for root
            if (vals.TryGetValue(key, out val))
                return true;

            if (!Get_Root(key, out key))
                return false;

            var ret = vals.TryGetValue(key, out val);

            return ret;


        }

        public TV this[TK key] {
            get {
                try { return vals[Get_Root(key)]; }
                catch (KeyNotFoundException ex) {
                    // erase stack trace
                    throw ex;
                }
            }
            set { vals[Get_Or_Set_Root(key)] = value; }
        }


        public void
        Add(TK key, TV value) {
            try {
                vals.Add(Get_Or_Set_Root(key), value);
            }
            catch (ArgumentException ex) {
                throw ex;
            }

        }

        public void
        Clear() {

            syns.Clear();
            vals.Clear();

        }

        /// <summary>
        /// Not Implemented.
        public bool
        Remove(TK key) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the number of values which have
        /// keys mapped to them.
        /// </summary>
        public int
        Count {
            get { return vals.Count; }
        }


        // ****************************


        /// <summary> Makes all members of "keys" synonyms for "key1" when
        /// retrieving items.
        /// "key1" must be an existing key, otherwise an IOpExc is thrown.
        /// each member of "keys" must be unmapped or IOpExc is thrown
        public void
        Add_Synonyms(TK key1, params TK[] keys) {

            if (keys.Length == 0)
                return;

            TK root;

            if (!Get_Root(key1, out root))
                true.tift<InvalidOperationException>();

            //// If "key1" is not present, add it as a root node
            //if (!Get_Root(key1, out root)) {
            //    syns.Add(key1, key1);
            //    root = key1;
            //}

            foreach (var key2 in keys)

                try {
                    syns.Add(key2, root);
                }
                catch (ArgumentException) {
                    true.tift<InvalidOperationException>(key2.ToString());

                }

        }

        public bool
        Is_Synonym(TK key1, TK key2) {

            if (!Get_Root(key1, out key1))
                return false;
            if (!Get_Root(key2, out key2))
                return false;

            if (!key1.Equals(key2))
                return false;

            return true;
        }


        /// <summary> Reduces the height of each synonym node,
        /// allowing for faster subsequent retrieval.
        /// "max_iter" - optional upper bound for the number of
        /// iterations
        /// </summary>
        public void
        Compress(int? max_iter) {

            bool ok = false;
            int ii = 0;
            Func<bool> cond;

            if (max_iter == null || max_iter.Value < 0)
                cond = () => !ok;
            else
                cond = () => !ok && ++ii <= max_iter.Value;

            while (cond()) {

                ok = true;
                var copy = this.syns.arr();

                foreach (var kvp in copy) {

                    // root node?
                    if (kvp.Key.Equals(kvp.Value))
                        continue;

                    var grandpa = syns[kvp.Value];

                    if (!kvp.Value.Equals(grandpa)) {
                        ok = false;
                        syns[kvp.Key] = grandpa;
                    }

                }

                if (ok || ii > max_iter.OrDefault_(int.MaxValue))
                    break;
            }


        }

        public Dictionary<TK, TV>
        Flatten() {

            //public void
            //Flatten() {
            //    foreach (var key in syns) {
            //        vals[key] = vals[Get_Root(key)];
            //    }
            //    foreach (var key in copy) {
            //        syns[key] = key;
            //    }
            //}

            var dict = new Dictionary<TK, TV>(syns.Count);
            foreach (var kvp in syns) {
                dict[kvp.Key] = vals[Get_Root(kvp)];
            }
            return dict;
        }


        // ****************************


        public ICollection<TK>
        Keys { get { return syns.Keys; } }

        public ICollection<TV>
        Values { get { return vals.Values; } }

        public IEnumerable<TK>
        Roots {
            get {
                foreach (var kvp in syns)
                    if (kvp.Key.Equals(kvp.Value))
                        yield return kvp.Key;
            }
        }


        // ****************************


        TK Get_Root(TK key) {
            return Get_Root(key, syns[key]);

        }

        TK Get_Or_Set_Root(TK key) {
            TK root;
            if (!Get_Root(key, out root)) {
                root = key;
                syns[key] = key;
            }
            return root;
        }
        bool Get_Root(TK key, out TK root) {

            if (!syns.TryGetValue(key, out root))
                return false;

            root = Get_Root(key, root);
            return true;

        }

        TK Get_Root(TK key, TK val) {

            while (!key.Equals(val)) {

                key = syns[val];
                H.Swap(ref key, ref val);

            }

            return key;
        }

        TK Get_Root(KeyValuePair<TK, TK> kvp) {
            return Get_Root(kvp.Key, kvp.Value);
        }

        // IEnumerable

        // ****************************

        IEnumerator
        IEnumerable.GetEnumerator() {
            return (this as IEnumerable<KeyValuePair<TK, TV>>).GetEnumerator();
        }

        // IEnumerable<TK, TV>

        // ****************************


        IEnumerator<KeyValuePair<TK, TV>>
        IEnumerable<KeyValuePair<TK, TV>>.GetEnumerator() {

            foreach (var kvp in syns) {

                yield return new KeyValuePair<TK, TV>(kvp.Key, vals[Get_Root(kvp.Value)]);
            }

        }

        // ICollection<KVP>

        // ****************************

        public bool
        IsReadOnly { get { return false; } }

        public void
        Add(KeyValuePair<TK, TV> kvp) {

            Add(kvp.Key, kvp.Value);
        }

        public void
        CopyTo(KeyValuePair<TK, TV>[] arr, int start_index) {
            this.arr().CopyTo(arr, start_index);
        }

        // abstract
        public bool
        Contains(KeyValuePair<TK, TV> kvp) {

            TV value;
            if (!TryGetValue(kvp.Key, out value))
                return false;
            if (!value.Safe_Equals(kvp.Value))
                return false;
            return true;

        }

        // abstract
        public bool
        Remove(KeyValuePair<TK, TV> kvp) {
            throw new NotImplementedException();
        }


        // IContains<TK>

        // ****************************

        bool IContains<TK>.Contains(TK key) {
            return ContainsKey(key);
        }




#if TRACK_CHILDREN // in case of deletion

		readonly Dictionary<TK, List<TK>> children;

#endif

    }
}
