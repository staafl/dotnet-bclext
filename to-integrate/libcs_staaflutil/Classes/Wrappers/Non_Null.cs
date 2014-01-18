using System;

namespace Fairweather.Service
{

    /// Copied from MiscUtil r285
    // http://www.yoda.arachsys.com/csharp/miscutil/
    ///Changed struct to class
    // Changed NonNullable<> to Not_Null<> for the sake of consistency
    /// <summary>
    /// Encapsulates a reference compatible with the type parameter. The reference
    /// is guaranteed to be non-null unless the value has been created with the
    /// parameterless constructor (e.g. as the default value of a field or array).
    /// Implicit conversions are available to and from the type parameter. The
    /// conversion to the non-nullable type will throw ArgumentNullException
    /// when presented with a null reference. The conversion from the non-nullable
    /// type will throw NullReferenceException if it contains a null reference.
    /// This type is a value type (to avoid taking any extra space) and as the CLR
    /// unfortunately has no knowledge of it, it will be boxed as any other value
    /// type. The conversions are also available through the Value property and the
    /// parameterised constructor.
    /// </summary>
    /// <typeparam name="T">Type of non-nullable reference to encapsulate</typeparam>
    public class Not_Null<T> : IEquatable<Not_Null<T>> where T : class
    {
        readonly T value;

        /// <summary>
        /// Creates a non-nullable value encapsulating the specified reference.
        /// </summary>
        public Not_Null(T value) {
            value.Throw_If_Null<ArgumentException>("value");
            this.value = value;
        }

        /// <summary>
        /// Retrieves the encapsulated value, or throws a NullReferenceException if
        /// this instance was created by default.
        /// </summary>
        public T Value {
            get {

                if (value == null)
                    true.Throw_If_True("value is null");

                return value;
            }
        }

        /// <summary>
        /// Implicit conversion from the specified reference.
        /// </summary>
        public static implicit operator Not_Null<T>(T value) {
            return new Not_Null<T>(value);
        }

        /// <summary>
        /// Implicit conversion to the type parameter from the encapsulated value.
        /// </summary>
        public static implicit operator T(Not_Null<T> wrapper) {
            return wrapper.Value;
        }

        /// <summary>
        /// Equality operator, which performs an identity comparison on the encapuslated
        /// references. No exception is thrown even if the references are null.
        /// </summary>
        public static bool operator ==(Not_Null<T> first, Not_Null<T> second) {
            return first.value == second.value;
        }

        /// <summary>
        /// Inequality operator, which performs an identity comparison on the encapuslated
        /// references. No exception is thrown even if the references are null.
        /// </summary>
        public static bool operator !=(Not_Null<T> first, Not_Null<T> second) {
            return first.value != second.value;
        }

        /// <summary>
        /// Equality is deferred to encapsulated references, but there is no equality
        /// between a NonNullable[T] and a T. This method never throws an exception,
        /// even if a null reference is encapsulated.
        /// </summary>
        public override bool Equals(object obj) {
            if (!(obj is Not_Null<T>)) {
                return false;
            }
            return Equals((Not_Null<T>)obj);
        }

        /// <summary>
        /// Type-safe (and non-boxing) equality check.
        /// </summary>
        public bool Equals(Not_Null<T> other) {
            return object.Equals(this.value, other.value);
        }

        /// <summary>
        /// Type-safe (and non-boxing) static equality check.
        /// </summary>
        public static bool Equals(Not_Null<T> first, Not_Null<T> second) {
            return object.Equals(first.value, second.value);
        }

        /// <summary>
        /// Defers to the GetHashCode implementation of the encapsulated reference, or 0 if
        /// the reference is null.
        /// </summary>
        public override int GetHashCode() {
            return value == null ? 0 : value.GetHashCode();
        }

        /// <summary>
        /// Defers to the ToString implementation of the encapsulated reference, or an
        /// empty string if the reference is null.
        /// </summary>
        public override string ToString() {
            return value == null ? "" : value.ToString();
        }
    }
}
