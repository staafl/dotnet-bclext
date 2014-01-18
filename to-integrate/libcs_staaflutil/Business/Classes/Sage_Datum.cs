using System;
using System.Collections.Generic;
using Fairweather.Service;

namespace Common
{
    public class Sage_Datum<T> : Expiry<T>
    {

        public Sage_Datum(Func<T> getter)
            : base(getter) {
        }

        public Sage_Datum(Func<T> getter, TimeSpan? expiry) :
            base(getter, expiry) {

        }

        protected override T 
        Refresh_Inner() {
            //using (Establish_Connection())

                return base.Refresh();
        }

        

    }
}
