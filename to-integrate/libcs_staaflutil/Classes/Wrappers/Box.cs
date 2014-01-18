namespace Fairweather.Service
{
    public class Box<T> where T : struct
    {

        public Box(T value) {
            this.Value = value;
        }

        public static explicit operator T(Box<T> box) {
            return box.Value;
        }
        public static implicit operator Box<T>(T value) {
            return new Box<T>(value);
        }

        //public static implicit operator Box<T>(T? nullable) {
        //    if (nullable.HasValue)
        //        return new Box<T>(nullable.Value);

        //    return null;
        //}


        public T Value { get; private set; }

        public override string ToString() {
            return Value.ToString();
        }
        // override tostring() but not hashcode and equals

    }
}
