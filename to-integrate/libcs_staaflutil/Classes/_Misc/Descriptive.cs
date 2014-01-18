using System;

namespace Fairweather.Service
{

    public static class Descriptive
    {
        public static Descriptive<T> Make<T>(T value) //where T : struct 
        {
            return new Descriptive<T>(value);
        }
    }


    public class Descriptive<T>// where T : struct
    {
        public Descriptive(T value) {
            this.Value = value;
        }
        //public Descriptive(T value, object id) {
        //    this.Value = value;
        //    this.ID = id;
        //}
        //public object ID { get; set; }
        //public static Func<T, object, string> Getter;



        public T Value { get; set; }

        public static implicit operator T(Descriptive<T> opt) {
            return opt.Value;
        }

        public static explicit operator Descriptive<T>(T value) {
            return new Descriptive<T>(value);
        }

        // careful with the implicit conversions

        public static Func<T, string> Getter;

        public override string ToString() {

            return Getter(Value);

        }

    }
}
