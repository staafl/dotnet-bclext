using System;
using System.Collections.Generic;
namespace Fairweather.Service
{
    public class Exceptions_Collector
    {

        public Exceptions_Collector(params Type[] to_gather) {

            this.to_gather = new Set<Type>(to_gather);
            this.exceptions = new List<Pair<Exception, object>>();

        }


        // ****************************

        public void Collect(Action act) {

            Collect(act, null);
        }
        public void Collect(Action act, object id) {

            try {
                act();
            }
            catch (Exception ex) {

                if (!to_gather.Contains(ex.GetType()))
                    throw;

                exceptions.Add(Pair.Make(ex, id));

            }
        }

        public T Collect<T>(Func<T> f) {
            object id = null;
            return Collect<T>(f, id);
        }
        public T Collect<T>(Func<T> f, object id) {
            T def = default(T);
            return Collect<T>(f, id, def);
        }
        public T Collect<T>(Func<T> f, object id, T def) {

            try {
                return f();
            }
            catch (Exception ex) {

                if (!to_gather.Contains(ex.GetType()))
                    throw;

                exceptions.Add(Pair.Make(ex, id));

            }

            return def;

        }


        public IEnumerable<Pair<Exception, object>> Exceptions {
            get { return exceptions; }
        }

        public void Clear() { exceptions.Clear(); }
        public int Count { get { return exceptions.Count; } }

        readonly List<Pair<Exception, object>> exceptions;
        readonly Set<Type> to_gather;

    }
}
