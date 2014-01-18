using System;
using System.Threading;

namespace Fairweather.Service
{


    public sealed class Lazy<T>
    {
        public Lazy(T value) {

            Getter = () => value;
            Fresh = true;

        }

        public Lazy(Func<T> producer) {

            producer.tifn<ArgumentNullException>();
            Getter = producer;

        }

        // ****************************


        public bool Fresh {
            get {
                return Thread.VolatileRead(ref fresh) == 1;
            }
            set {
                Interlocked.Exchange(ref fresh, value ? 1 : 0);
            }
        }

        public Func<T> Getter {
            get;
            set;
        }


        public T Value {
            get {
                lock (locker) // Getter)
                    return Fresh ? value : Force();
            }
        }

        public T Force() {

            value = Getter();
            Fresh = true;

            return value;

        }


        // ****************************



        T value;
        int fresh;

        readonly object locker = new object();



        // ****************************


        public static implicit operator Lazy<T>(T value) {

            return new Lazy<T>(value);

        }

        public static implicit operator T(Lazy<T> lazy) {

            var ret = lazy.Value;
            return ret;
        }

        public static implicit operator Lazy<T>(Func<T> f) {

            var ret = Lazy.Make(f);
            return ret;

        }

    }
}
