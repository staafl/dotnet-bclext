#if false
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fairweather.Service.Collections
{
    class Quick_Value_Dictionary<TIndex, TValue>
    {
        Dictionary<TIndex, TValue> rd_inner;
        Dictionary<TValue, int> rd_values;

        public Quick_Value_Dictionary() {

        }

        public void Add(TIndex index, TValue value) {
        }

        public bool Remove(TIndex index) {
        }

        public bool TryGetValue(TIndex index, out TValue value) {

        }

        public TValue this[TIndex index] {
            get { /* return the specified index here */ }
            set { /* set the specified index to value here */ }
        }

        public IEnumerable<TIndex> Keys {
            get {
            }
        }

        public IEnumerable<TValue> Values {
            get {
            }
        }

        public bool ContainsKey(TIndex index) { }

        public bool ContainsValue(TValue value) { }
        public int Count_Of_Value(TValue value) { }


    }
}

#endif