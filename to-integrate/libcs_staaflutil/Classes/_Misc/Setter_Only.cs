using System;

namespace Fairweather.Service
{
    // By creating a readonly field, you can make sure a value is never
    // modified outside of its setter
    public class Setter_Only<T>
    {
        T value;

        public T Value {
            get {
                return this.value;
            }
            set {
                m_setter(value, m_setter_inner);
            }
        }

        readonly Action<T, Action<T>> m_setter;
        readonly Action<T> m_setter_inner;

        /*       Is this a good idea?        */

        public static Setter_Only<T> operator +(Setter_Only<T> setter_only, T value) {
            setter_only.Value = value;
            return setter_only;
        }
        public static implicit operator T(Setter_Only<T> setter_only) {
            return setter_only.Value;
        }

        public Setter_Only(T field, Action<T, Action<T>> setter) {

            setter.Throw_If_Null();

            this.value = field;
            this.m_setter = setter;
            m_setter_inner = (_value) => value = _value;

        }
    }
}
