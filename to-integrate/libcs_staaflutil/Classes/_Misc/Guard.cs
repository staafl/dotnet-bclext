using System;

namespace Fairweather.Service
{

    public class Guard<T> : IEquatable<Guard<T>>
    {


        T value;
        readonly Func<T, bool> contract;


        public Guard(Func<T, bool> contract) {

            contract.Throw_If_Null<ArgumentNullException>("contract");
            this.contract = contract;

        }

        public Guard(Func<T, bool> contract, T initial)
            : this(contract) {
            this.Value = initial;
        }

        public T Value {
            get {

                return value;

            }
            set {
                if (!contract(value))
                    true.Throw_If_True();

                this.value = value;

            }
        }


        /// <summary>
        /// Implicit conversion to the type parameter from the encapsulated value.
        /// </summary>
        public static implicit operator T(Guard<T> wrapper) {
            return wrapper.Value;
        }

        /// <summary>
        /// Equality operator, which performs an identity comparison on the encapuslated
        /// references. No exception is thrown even if the references are null.
        /// </summary>
        public static bool operator ==(Guard<T> first, Guard<T> second) {
            if (first.value == null)
                return second.value == null;

            return first.value.Equals(second.value);
        }

        /// <summary>
        /// Inequality operator, which performs an identity comparison on the encapuslated
        /// references. No exception is thrown even if the references are null.
        /// </summary>
        public static bool operator !=(Guard<T> first, Guard<T> second) {
            return !(first == second);
        }

        /// <summary>
        /// Defers to the ToString implementation of the encapsulated reference, or an
        /// empty string if the reference is null.
        /// </summary>
        public override string ToString() {
            return value == null ? "" : value.ToString();
        }


        /// <summary>
        /// Equality is deferred to encapsulated references, but there is no equality
        /// between a NonNullable[T] and a T. This method never throws an exception,
        /// even if a null reference is encapsulated.
        /// </summary>
        public override bool Equals(object obj) {
            if (!(obj is Guard<T>)) {
                return false;
            }
            return Equals((Guard<T>)obj);
        }

        /// <summary>
        /// Type-safe (and non-boxing) equality check.
        /// </summary>
        public bool Equals(Guard<T> other) {
            return object.Equals(this.value, other.value);
        }



        /// <summary>
        /// Type-safe (and non-boxing) static equality check.
        /// </summary>
        public static bool Equals(Guard<T> first, Guard<T> second) {
            return object.Equals(first.value, second.value);
        }



        /// <summary>
        /// Defers to the GetHashCode implementation of the encapsulated reference, or 0 if
        /// the reference is null.
        /// </summary>
        public override int GetHashCode() {
            return value == null ? 0 : value.GetHashCode();
        }

 
    }
}
