using System;
using System.Collections;

namespace Fairweather.Service
{
    public class Once : Times
    {
        public Once()
            : base(1) {
        }
    }

    public class Times : IEnumerable, IEnumerator
    {
        readonly int cnt;
        int now;

        static public Times Once {
            get {
                return new Once();
            }
        }
        public Times(int cnt) {
            this.cnt = cnt;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this;
        }

        void IEnumerator.Reset() {
            throw new InvalidOperationException();
        }

        bool IEnumerator.MoveNext() {
            return now++ < cnt;
        }

        object IEnumerator.Current {
            get {
                return now;
            }
        }

    }
}
