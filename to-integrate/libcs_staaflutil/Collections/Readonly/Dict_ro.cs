using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fairweather.Service
{


    public class Dict_ro<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        //void IDictionary<TKey, TValue>.Add(TKey _1, TValue _2) {
        //    throw new InvalidOperationException();
        //}
        //bool IDictionary<TKey, TValue>.Remove(TKey _1) {
        //    throw new InvalidOperationException();
        //}
        //void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> _) {
        //    throw new InvalidOperationException();
        //}
        //void ICollection<KeyValuePair<TKey, TValue>>.Clear() {
        //    throw new InvalidOperationException();
        //}
        //bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> _) {
        //    throw new InvalidOperationException();
        //}


        readonly IDictionary<TKey, TValue> dict;

        public static implicit operator Dict_ro<TKey, TValue>(Dictionary<TKey, TValue> idict) {
            return new Dict_ro<TKey, TValue>(idict);
        }

        public Dictionary<TKey, TValue>
        Copy() {
            return new Dictionary<TKey, TValue>(this.dict);
        }

        public Dict_ro(IDictionary<TKey, TValue> dict) {
            this.dict = dict;
        }

        public Dict_ro(IEnumerable<KeyValuePair<TKey, TValue>> seq) {

            this.dict = new Dictionary<TKey, TValue>();
            this.dict.Fill(seq, false);
        }


        public TValue this[TKey key] {
            get {
                return dict[key];
            }
        }

        public bool TryGetValue(TKey key, out TValue value) {
            return dict.TryGetValue(key, out value);
        }
        public bool ContainsKey(TKey key) {
            return dict.ContainsKey(key);
        }
        public bool ContainsValue(TValue value) {

            var cast = dict as Dictionary<TKey, TValue>;
            if (cast != null)
                return cast.ContainsValue(value);

            return Values.Any(elem => elem.Safe_Equals(value));

        }
        public ICollection<TKey> Keys {
            get {
                return dict.Keys;
            }
        }

        public ICollection<TValue> Values {
            get {
                return dict.Values;
            }
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return dict.GetEnumerator();
        }

        public int Count {
            get {
                return dict.Count;
            }
        }
    }
}