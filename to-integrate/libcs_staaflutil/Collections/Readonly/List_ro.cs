using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fairweather.Service
{
    public class List_ro<T> : IEnumerable<T>
    {
        readonly IList<T> lst;

        public List_ro(IEnumerable<T> seq) {

            lst = (seq as IList<T>) ?? new List<T>(seq);

        }
        public static implicit operator List_ro<T>(List<T> ilist) {
            return new List_ro<T>(ilist);
        }

        public List<T>
        Copy() {
            return this.lst.ToList();
        }

        public T this[int ix] {
            get {
                return lst[ix];
            }
        }

        public int Count {
            get {
                return lst.Count;
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return lst.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return lst.GetEnumerator();
        }
    }
}