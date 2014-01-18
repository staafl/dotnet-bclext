using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fairweather.Service
{
    /// <summary>
    /// Instances of this collection are considered equal iff
    /// they contain exactly the same elements in the exact same
    /// order.
    /// </summary>
    class ID_Collection<T> : IEnumerable<T>
    {
        public ID_Collection(IEnumerable<T> inner) {

            inner.tifn();

            this.inner = inner;
            hash = new Lazy<int>(() =>
            {

                int ret = 0;
                foreach (var elem in inner.Take(7)) {
                    ret *= 23;
                    ret += elem.GetHashCode();
                }
                return ret;
            });

        }

        public ID_Collection(IEnumerable<T> inner, int hash) {

            inner.tifn();

            this.inner = inner;
            this.hash = new Lazy<int>(hash);

        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return inner.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return inner.GetEnumerator();
        }

        readonly Lazy<int> hash;
        readonly IEnumerable<T> inner;

        public override bool Equals(object obj) {
            if (obj == null)
                return false;

            var as_me = obj as ID_Collection<T>;

            if (as_me == null)
                return false;

            return Equals(this, obj);
        }

        public override int GetHashCode() {
            return hash.Value;
        }

        static public bool operator ==(ID_Collection<T> id1, ID_Collection<T> id2) {
            return Equals(id1, id2);
        }
        static public bool operator !=(ID_Collection<T> id1, ID_Collection<T> id2) {
            return !Equals(id1, id2);
        }

        static bool Equals(ID_Collection<T> id1, ID_Collection<T> id2) {

            if ((id1 == null) != (id2 == null))
                return false;

            if (id1 == null)
                return true;

            return id1.Piecewise_Equal(true, id2);

        }
    }
}
