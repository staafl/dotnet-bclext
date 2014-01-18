using System.Collections.Generic;

namespace Fairweather.Service
{
    public class Cache_Map<TK, TV> : Dictionary<TK, TV>
    {
        public Cache_Map(GetItemDelegate<TK,TV> next) {
            this.next = next;
        }
        public Cache_Map(GetItemDelegate<TK,TV> next, int cnt)
            : base(cnt) {
            this.next = next;
        }

        readonly GetItemDelegate<TK, TV> next;

        public new TV this[TK key] {
            get {
                TV value;

                if (!TryGetValue(key, out value))
                    true.tift<KeyNotFoundException>();

                return value;
            }
            set {
                base[key] = value;
            }
        }

        public new bool TryGetValue(TK key, out TV value) {

            if (base.TryGetValue(key, out value))
                return true;

            bool stop;

            while (true) 
                if(next(key, out value, out stop))
                    return true;
                else if(stop)
                    return false;


        }

    }
}
