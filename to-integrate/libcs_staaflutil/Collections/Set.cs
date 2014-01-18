using System.Collections;
using System.Collections.Generic;

namespace Fairweather.Service
{


#if !NET20
    public sealed class Set<TElem>
        : HashSet<TElem>, IReadWrite<TElem, bool>, IRead<TElem, bool>, IEnumerable<TElem>
    {

        public Set()
            : base() {

        }

        public Set(IEnumerable<TElem> seq)
            : base(seq) {

        }
        
        public Set(params TElem[] elems)
            : base(elems as IEnumerable<TElem>) {

        }
        
        public Set(int capacity)
            : base() {
            
            //this.Capacity = capacity;
	}
        public void AddRange(IEnumerable<TElem> collection) {

            foreach (var elem in collection)
                Add(elem);

        }

        public void AddQuick(TElem elem) {
            base.Add(elem);
        }

        public void RemoveQuick(TElem elem) {
            base.Remove(elem);
        }

        public bool this[TElem index] {
            get {
                var ret = Contains(index);
                return ret;
            }
            set {
                if (value)
                    Add(index);

                else
                    Remove(index);
            }
        }
    }
#else
    public class Set<TElem> : IReadWrite<TElem, bool>, IRead<TElem, bool>, IEnumerable<TElem>
    {
        readonly Dictionary<TElem, bool> dict;

        public void Clear() {
            dict.Clear();
        }
        public Set() {
            dict = new Dictionary<TElem, bool>();
        }
        public Set(int capacity) {
            dict = new Dictionary<TElem, bool>(capacity);
        }
        public Set(IEnumerable<TElem> elems) {
            dict = new Dictionary<TElem, bool>();
            foreach (TElem elem in elems)
                dict[elem] = false;

        }
        public Set(params TElem[] elems)
            : this(elems as IEnumerable<TElem>) {
        }



        public void AddRange(IEnumerable<TElem> collection) {

            foreach (TElem elem in collection)
                dict.Add(elem, false);

        }
        public void RemoveRange(IEnumerable<TElem> collection) {

            foreach (TElem elem in collection)
                dict.Remove(elem);

        }

        public void AddQuick(TElem elem) {
            if (elem == null)
                has_null = true;
            else
                dict[elem] = false;
        }

        public void RemoveQuick(TElem elem) {
            if (elem == null) {
                has_null = false;
            }
            else {
                dict.Remove(elem);
            }
        }

        public bool Add(TElem elem) {
            if (elem == null) {
                return !H.Set(ref has_null, true);
            }
            else {
                if (dict.ContainsKey(elem))
                    return false;
                dict[elem] = false;
                return true;
            }
        }

        public bool Remove(TElem elem) {
            if (elem == null) {
                return H.Set(ref has_null, false);
            }
            else {
                return dict.Remove(elem);
            }
        }


        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public IEnumerator<TElem> GetEnumerator() {
            return dict.Keys.GetEnumerator();
        }

        public int Count {
            get {
                return dict.Count;
            }
        }

        public bool Contains(TElem index) {

            return this[index];

        }
        bool has_null = false;

        public bool this[TElem index] {
            get {
                if (index == null)
                    return has_null;

                bool ret = dict.ContainsKey(index);
                return ret;
            }
            set {
                if (value)
                    Add(index);

                else
                    Remove(index);
            }
        }
    }
#endif
}
