using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fairweather.Service
{
    class ReadWriteCollection<TKey, TValue> : IReadWrite<TKey, TValue>, IRead<TKey, TValue>
        //, IEnumerable<KeyValuePair<TKey,TValue>>
    {
        readonly Dictionary<TKey, TValue> m_inner_collection = new Dictionary<TKey, TValue>();


        TValue IReadWrite<TKey, TValue>.this[TKey index] { 
            set { m_inner_collection[index] = value; }
            get { return m_inner_collection[index]; }
        }

        public virtual TValue this[TKey index] { 
            get { return m_inner_collection[index]; } 
        }

        public ReadWriteCollection(Dictionary<TKey, TValue> dict) {

            this.m_inner_collection = new Dictionary<TKey, TValue>(dict);
        }

        public ReadWriteCollection(int capacity) {

            this.m_inner_collection = new Dictionary<TKey, TValue>(capacity);
        }

        public ReadWriteCollection() {

            this.m_inner_collection = new Dictionary<TKey, TValue>();
        }
    }
}
