using System.Collections;
using System.Collections.Generic;

namespace Fairweather.Service
{
    // ****************************
    /*       Inheritance        */
    // ****************************

    public class RW_Dict<TIx, TValue>
        : Dictionary<TIx, TValue>
        , IReadWrite<TIx, TValue>
        , IRead<TIx, TValue>
        , IEnumerable<KeyValuePair<TIx, TValue>>
        , IEnumerable<Pair<TIx, TValue>>
    {
        public RW_Dict() : base() { }
        public RW_Dict(int capacity) : base(capacity) { }
        public RW_Dict(IDictionary<TIx, TValue> dict) : base(dict) { }

        // ****************************

        IEnumerator<Pair<TIx, TValue>>
        IEnumerable<Pair<TIx, TValue>>.GetEnumerator() {

            foreach (var kvp in this)
                yield return Pair.Make(kvp.Key, kvp.Value);

        }

        // ****************************

        bool IContains<TIx>.Contains(TIx index) {

            return ContainsKey(index);
        }
    }

    public class RW_List<TValue>
        : List<TValue>
        , IReadWrite<int, TValue>
        , IRead<int, TValue>
        , IEnumerable<TValue>
    {
        public RW_List() : base() { }
        public RW_List(int capacity) : base(capacity) { }
        public RW_List(IEnumerable<TValue> collection) : base(collection) { }


        // ****************************

        bool IContains<int>.Contains(int index) {

            return (index >= 0 && this.Count > index);
        }
    }

    // ****************************
    /*       Composition        */
    // ****************************

    public class RW_Array<TValue>
        : IReadWrite<int, TValue>
        , IRead<int, TValue>
        , IEnumerable<TValue>
    {
        public RW_Array(params TValue[] inner) {
            this.inner = inner;
        }

        public static implicit operator
        TValue[](RW_Array<TValue> arr) {

            return arr.inner;
        }

        public static implicit operator
        RW_Array<TValue>(TValue[] arr) {

            return new RW_Array<TValue>(arr);
        }

        readonly TValue[] inner;

        public TValue[] Inner {
            get { return inner; }
        }

        /*       IEnumerable        */

        public IEnumerator<TValue>
        GetEnumerator() {
            return (inner as IEnumerable<TValue>).GetEnumerator();

        }

        IEnumerator
        IEnumerable.GetEnumerator() {
            return inner.GetEnumerator();
        }


        /*       IRead/IReadWrite        */

        public TValue this[int index] {
            get {
                return inner[index];
            }
            set { inner[index] = value; }
        }

        bool IContains<int>.Contains(int index) {

            return (index >= 0 && inner.Length > index);
        }




    }

    public class RW_IList<TValue>
  : IReadWrite<int, TValue>
  , IRead<int, TValue>
  , IEnumerable<TValue>
    //,  IList<TValue>
    {
        public RW_IList(IList<TValue> inner) {
            this.inner = inner;
        }

        readonly IList<TValue> inner;

        public IList<TValue> Inner {
            get { return inner; }
        }

        public static implicit operator
        RW_IList<TValue>(List<TValue> list) {

            return new RW_IList<TValue>(list);
        }


        // ****************************

        public int Count {
            get { return inner.Count; }
        }

        public bool IsReadOnly {
            get {
                return inner.IsReadOnly;
            }
        }

        // ****************************

        IEnumerator
        IEnumerable.GetEnumerator() {
            return inner.GetEnumerator();
        }

        IEnumerator<TValue>
        IEnumerable<TValue>.GetEnumerator() {
            return inner.GetEnumerator();
        }


        // ****************************


        bool IContains<int>.Contains(int index) {

            return (index >= 0 && inner.Count > index);
        }

        public TValue this[int index] {
            get {
                return inner[index];
            }
            set { inner[index] = value; }
        }


    }


    public class RW_IDict<TIx, TValue>
        : IReadWrite<TIx, TValue>
        , IRead<TIx, TValue>
        , IEnumerable<Pair<TIx, TValue>>
        , IEnumerable<KeyValuePair<TIx, TValue>>
    {
        public RW_IDict(IDictionary<TIx, TValue> inner) {
            this.inner = inner;
        }

        readonly IDictionary<TIx, TValue> inner;

        public IDictionary<TIx, TValue> Inner {
            get { return inner; }
        }

        public static implicit operator
        RW_IDict<TIx, TValue>(Dictionary<TIx, TValue> dict) {

            return new RW_IDict<TIx, TValue>(dict);
        }


        /*       IEnumerable        */

        IEnumerator
        IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        IEnumerator<KeyValuePair<TIx, TValue>>
        IEnumerable<KeyValuePair<TIx, TValue>>.GetEnumerator() {
            return inner.GetEnumerator();
        }

        public IEnumerator<Pair<TIx, TValue>>
        GetEnumerator() {
            foreach (var kvp in inner) {
                yield return Pair.Make(kvp.Key, kvp.Value);
            }
        }


        // ****************************


        bool IContains<TIx>.Contains(TIx index) {

            return inner.ContainsKey(index);
        }

        public TValue this[TIx index] {
            get {
                return inner[index];
            }
            set { inner[index] = value; }
        }

    }

    class Proxy<TKey, TValue> : IRead<TKey, TValue>
    {
        readonly IReadWrite<TKey, TValue> rd_inner;

        public TValue this[TKey index] {
            get { return rd_inner[index]; }
        }

        public Proxy(IReadWrite<TKey, TValue> inner) {
            rd_inner = inner;
        }

        public bool Contains(TKey key) {
            return rd_inner.Contains(key);
        }
    }


}
