using System.Collections.Generic;

namespace Fairweather.Service
{

    /*       Untested        */
    public class NaiveHashTable<TKey, TValue>
    {
        // yield foreach for IEnumerable<KVP>?
        public TValue this[TKey index] {
            get { var ret = this.Get(index); return ret; }
            set { this.Add(index, value); }
        }

        const int hash_size = 104729;
        List<KeyValuePair<TKey, TValue>>[] spine = new List<KeyValuePair<TKey, TValue>>[hash_size];

        public void Clear() {

            spine = new List<KeyValuePair<TKey, TValue>>[hash_size];

        }

        public NaiveHashTable() {
        }

        public void Add(TKey key, TValue value) {

            int hash = key.GetHashCode() % hash_size;

            if (spine[hash] == null)
                spine[hash] = new List<KeyValuePair<TKey, TValue>>();

            var rib = spine[hash];

            foreach (var item in rib) {

                if (item.Key.Equals(key))
                    return;
                //throw new InvalidOperationException();

            }

            rib.Add(new KeyValuePair<TKey, TValue>(key, value));

        }

        public TValue Get(TKey key) {

            int hash = key.GetHashCode() % hash_size;

            if (spine[hash] == null)
                throw new KeyNotFoundException();

            var rib = spine[hash];

            foreach (var item in rib) {

                if (item.Key.Equals(key))
                    return item.Value;

            }


            throw new KeyNotFoundException();

        }

    }

}