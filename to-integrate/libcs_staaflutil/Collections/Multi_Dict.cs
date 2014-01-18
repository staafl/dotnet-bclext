using System.Collections.Generic;

namespace Fairweather.Service
{
    public class Multi_Dict<TKey, TValue> : Dictionary<TKey, List<TValue>>
    {
        public Multi_Dict() { }
        public Multi_Dict(int capacity) : base(capacity) { }
        public Multi_Dict(IDictionary<TKey, List<TValue>> idict) : base(idict) { }


        public void Add_Range(TKey key, IEnumerable<TValue> values) {

            List<TValue> list;
            if (!TryGetValue(key, out list)) {
                list = new List<TValue>();
                this[key] = list;
            }

            list.AddRange(values);
        }

        public void Add_Value(TKey key, TValue value) {

            List<TValue> list;
            if (!TryGetValue(key, out list)) {
                list = new List<TValue>();
                this[key] = list;
            }

            list.Add(value);
        }

        public bool Remove_Value(TKey key, TValue value) {

            List<TValue> list;

            if (!TryGetValue(key, out list))
                return false;

            return(list.Remove(value));

        }

        public void Add(TKey key, params TValue[] values) {

            base.Add(key, new List<TValue>(values));

        }
    }
}
