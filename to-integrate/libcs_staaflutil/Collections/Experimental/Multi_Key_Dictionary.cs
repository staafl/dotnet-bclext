using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Fairweather.Service.Collections
{
    public class Multi_Key_Dictionary<TKey, TValue>
    {
        [DebuggerStepThrough]
        public struct KVP
        {
            #region KVP

            readonly TValue m_val;

            readonly Dictionary<TKey, KVP> m_dict;

            readonly bool m_empty;


            public TValue Val {
                get {
                    return this.m_val;
                }
            }

            public Dictionary<TKey, KVP> Dict {
                get {
                    return this.m_dict;
                }
            }

            public bool Empty {
                get {
                    return this.m_empty;
                }
            }


            public KVP(TValue val,
                        Dictionary<TKey, KVP> dict,
                        bool empty) {
                this.m_val = val;
                this.m_dict = dict;
                this.m_empty = empty;
            }


            /* Boilerplate */

            public override string ToString() {

                string ret = string.Empty;

                ret += "val = " + this.m_val;
                ret += ", ";
                ret += "dict = " + this.m_dict;
                ret += ", ";
                ret += "empty = " + this.m_empty;

                ret = "{KVP: " + ret + "}";
                return ret;

            }

            public bool Equals(KVP obj2) {

                if (!this.m_val.Equals(obj2.m_val))
                    return false;

                if (!this.m_dict.Equals(obj2.m_dict))
                    return false;

                if (!this.m_empty.Equals(obj2.m_empty))
                    return false;

                return true;
            }

            public override bool Equals(object obj2) {

                var ret = (obj2 != null && obj2 is KVP);

                if (ret)
                    ret = this.Equals((KVP)obj2);


                return ret;

            }

            public static bool operator ==(KVP left, KVP right) {

                var ret = left.Equals(right);
                return ret;

            }

            public static bool operator !=(KVP left, KVP right) {

                var ret = !left.Equals(right);
                return ret;

            }

            public override int GetHashCode() {

                unchecked {
                    int ret = 23;
                    int temp;

                    ret *= 31;
                    temp = this.m_val.GetHashCode();
                    ret += temp;

                    ret *= 31;
                    temp = this.m_dict.GetHashCode();
                    ret += temp;

                    ret *= 31;
                    temp = this.m_empty.GetHashCode();
                    ret += temp;

                    return ret;
                }
            }

            #endregion
        }
	
        Dictionary<TKey, KVP> m_spine;

        public Multi_Key_Dictionary() {
            m_spine = new Dictionary<TKey, KVP>();

        }

        bool
        Find(ref TValue value, bool get, bool throw_on_not_found, TKey key1, params TKey[] keys) {

            var dict1 = m_spine;
            var dict2 = dict1;
            KVP temp = new KVP();

            TKey temp_key_1 = default(TKey);
            TKey temp_key_2;

            for (int ii = -1; ii < keys.Length; ++ii) {

                if (ii == -1) {
                    temp_key_2 = default(TKey);
                    temp_key_1 = key1;
                }
                else {
                    temp_key_2 = temp_key_1;
                    temp_key_1 = keys[ii];
                }


                if (dict1 == null || 
                    !dict1.TryGetValue(temp_key_1, out temp) ||
                    (ii == keys.Length - 1 && temp.Empty) ) {

                    if (get) {
                        if (throw_on_not_found)
                            throw new KeyNotFoundException(temp_key_1.ToString());
                        else
                            return false;
                    }

                    if (dict1 == null) {

                        dict1 = new Dictionary<TKey, Multi_Key_Dictionary<TKey, TValue>.KVP>();
                        dict2[temp_key_2] = new KVP(default(TValue), dict1, true);
                    }

                }

                dict2 = dict1;
                dict1 = temp.Dict;

            }

            if (get)
                value = temp.Val;
            else
                dict2[temp_key_1] = new KVP(value, dict1, false);

            return true;


        }

        public bool TryGetValue(out TValue value, TKey key1, params TKey[] keys) {

            value = default(TValue);
            var ret = Find(ref value, true, false, key1, keys);
            return ret;

        }

        public TValue this[TKey key1, params TKey[] keys] {
            get {
                TValue value = default(TValue);
                Find(ref value, true, true, key1, keys);

                return value;
            }
            set {
                Find(ref value, false, false, key1, keys);
            }
        }
    }
}
