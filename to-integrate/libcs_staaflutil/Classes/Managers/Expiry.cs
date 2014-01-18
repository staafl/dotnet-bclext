using System;

namespace Fairweather.Service
{
    public class Expiry<T>
    {

        public Expiry(Func<T> getter)
            : this(getter, null) {
        }
        public Expiry(Func<T> getter, TimeSpan? expiry) {

            // getter.tifn();

            this.getter = getter;
            this.expiry = expiry;

        }

        readonly Func<T> getter;
        readonly TimeSpan? expiry;
        DateTime? last_updated;
        T value;

        public T Get_Value() {

            Refresh(false);

            return value;

        }

        public T Set(T value) {
            this.value = value;
            last_updated = DateTime.Now;
            return value;

        }

        public T Refresh() {

            return Refresh(false);

        }


        public T Refresh(bool force) {

            if (force ||
                last_updated == null ||
                (DateTime.Now - last_updated.Value) > expiry) {

                Refresh_Inner();

            }

            return value;

        }


        protected virtual T Refresh_Inner() {
            return Set(getter());
        }



    }
}
