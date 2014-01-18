using System;
using System.Collections.Generic;

namespace Fairweather.Service
{
    /// <summary>
    /// Not threadsafe
    /// </summary>
    class Quick_Value_Dictionary<TIndex, TValue>
    {
        Dictionary<TIndex, TValue> rd_inner;
        Dictionary<TValue, int> rd_values;

        public Quick_Value_Dictionary() {
            rd_inner = new Dictionary<TIndex, TValue>();
            rd_values = new Dictionary<TValue, int>();
        }

        public Quick_Value_Dictionary(int capacity) {
            rd_inner = new Dictionary<TIndex, TValue>(capacity);
            rd_values = new Dictionary<TValue, int>();
        }

        void Insert(TIndex index, TValue value, bool add) {

            TValue previous;

            bool exists = rd_inner.TryGetValue(index, out previous);

            (exists && add).tiff<InvalidOperationException>("Key is already present: " + index);

            if (exists && value.Equals(previous))
                return;

            rd_inner[index] = value;

            if (exists)
                Remove_Value(previous);

            Add_Value(value);
        }

        void Add_Value(TValue value) {

            int count;
            if (!rd_values.TryGetValue(value, out count))
                count = 0;

            ++count;
            rd_values[value] = count;

        }

        void Remove_Value(TValue value) {

            int count;

            if (!rd_values.TryGetValue(value, out count))
                count = 0;

            (count == 0).tiff();

            if (count == 1) {
                rd_values.Remove(value);
                return;
            }

            --count;
            rd_values[value] = count;
        }

        public void Add(TIndex index, TValue value) {

            Insert(index, value, true);

        }

        public bool Remove(TIndex index) {

            TValue value;

            bool ret = rd_inner.TryGetValue(index, out value);

            if (!ret)
                return false;

            rd_inner.Remove(index);

            Remove_Value(value);

            return true;
        }

        public bool TryGetValue(TIndex index, out TValue value) {
            return rd_inner.TryGetValue(index, out value);

        }

        public TValue this[TIndex index] {
            get { return rd_inner[index]; }
            set { Insert(index, value, false); }
        }

        public IEnumerator<KeyValuePair<TIndex, TValue>> GetEnumerator {
            get {
                return rd_inner.GetEnumerator();
            }
        }

        public IEnumerable<TIndex> Keys {
            get {
                return rd_inner.Keys;
            }
        }

        public IEnumerable<TValue> Values {
            get {

                foreach (var kvp in rd_values) {

                    int count = kvp.Value;
                    while (--count >= 0)
                        yield return kvp.Key;

                }
            }
        }

        public bool ContainsKey(TIndex index) {
            return rd_inner.ContainsKey(index);
        }

        public bool ContainsValue(TValue value) {

            bool ret = Count_Of_Value(value) == 0;

            return ret;

        }

        public int Count_Of_Value(TValue value) {

            int count;
            if (!rd_values.TryGetValue(value, out count))
                count = 0;

            return count;


        }

        public int Count {
            get {
                return rd_inner.Count;
            }
        }

        public void Clear() {

            rd_inner.Clear();
            rd_values.Clear();

        }

    }
}
