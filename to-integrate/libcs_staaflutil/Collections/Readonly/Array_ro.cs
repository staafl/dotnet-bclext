using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fairweather.Service
{
    public class Array_ro<T> : IEnumerable<T>
    {


        public Array_ro(IEnumerable<T> seq) {

            arr = (seq as T[]) ?? seq.ToArray();

        }
        public static implicit operator Array_ro<T>(T[] arr) {
            return new Array_ro<T>(arr);
        }

        public T[]
        Copy() {
            return this.arr.Copy();
        }

        readonly T[] arr;

        public T this[int ix] {
            get {
                return arr[ix];
            }
        }

        public int Length {
            get {
                return arr.Length;
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return arr.Cast<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return arr.GetEnumerator();
        }
    }
}
