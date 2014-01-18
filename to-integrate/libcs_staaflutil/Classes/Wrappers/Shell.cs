
namespace Fairweather.Service
{
    /// <summary>
    /// Usefull for putting nullable values in dictionary, etc.
    /// Differs from Box[T] in its Equals and Hashcode semantics.
    /// </summary>
    public class Shell<T>
    {

        public Shell(T value) {
            this.Value = value;
        }

        public static explicit operator T(Shell<T> shell) {
            return shell.Value;
        }
        public static implicit operator Shell<T>(T value) {
            return new Shell<T>(value);
        }

        public T Value { get; private set; }

        public override bool Equals(object obj) {
            return Value.Safe_Equals(obj);
        }

        public override int GetHashCode() {
            return Value == null ? 0 : Value.GetHashCode();
        }

        public override string ToString() {
            return Value.StringOrDefault("[null]");
        }
        //public static bool operator ==(Shell<T> s1, Shell<T> s2) {
        //    return s1.Safe_Equals(s2);
        //}
        //public static bool operator !=(Shell<T> s1, Shell<T> s2) {
        //    return !s1.Safe_Equals(s2);
        //}
    }
}
