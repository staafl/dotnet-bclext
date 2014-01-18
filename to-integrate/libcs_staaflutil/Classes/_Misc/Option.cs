using System;
using System.Diagnostics;

namespace Fairweather.Service
{
    // Credit: Jared Parsons' BCL Helpers
    [Serializable]
    [DebuggerDisplay("{Value}")]
    public class Option<T> : IEquatable<Option<T>>
    {
        public static Option<T>
        Make(T value) {
            if (value == null)
                return null;
            return new Option<T>(value);
        }

        Option(T value) {
            this.value = value;
        }

        readonly T value;

        public T Value {
            get {
                var ret = value;
                return ret;
            }
        }


        
        public static implicit operator Option<T>(T value) {
            return Make(value);
        }

        
        public static explicit operator T(Option<T> opt) {
            return opt.Value;
        }

        public static bool operator ==(Option<T> left, Option<T> right) {
            return left.Safe_Equals(right);
        }
        public static bool operator !=(Option<T> left, Option<T> right) {
            return !left.Safe_Equals(right);
        }


        // IEquatable<T> Members

        public bool Equals(Option<T> other) {
            if (other == null)
                return false;

            return value.Equals(other.Value);
        }


        // Overrides

        public override bool Equals(object obj) {
            if (obj is Option<T>) {
                return Equals((Option<T>)obj);
            }

            return false;
        }

        public override int GetHashCode() {

            return value.GetHashCode();

        }



    }
}