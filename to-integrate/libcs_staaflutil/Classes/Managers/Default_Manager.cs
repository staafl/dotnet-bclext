using System;

namespace Fairweather.Service
{
    public static class Default_Manager
    {
        public static Default_Manager<T> Make<T>(T default_value, Func<T> producer) where T : class, new() {

            return Default_Manager.Make(default_value, _ => producer());

        }

        public static Default_Manager<T> Make<T>(T default_value, Endo<T> producer) where T : class, new() {

            return new Default_Manager<T>(default_value, producer);
        }

        public static Default_Manager<T> Make_With_New<T>(T default_value) where T : class, new() {
            return new Default_Manager<T>(default_value, _ => new T());
        }
    }

    /// <summary>
    /// The goal of this class is to allow the same object to be shared between many users.
    /// By calling Split() the wrapper class will create its own separate instance so that
    /// it can modify its properties without affecting anyone else. Useful for Pens, Brushes,
    /// Handles and similar resources.
    /// </summary>
    public class Default_Manager<T> where T : class
    {
        public Default_Manager(T default_value, Endo<T> producer) {

            (producer).Throw_If_Null<ArgumentNullException>("producer");

            this.default_value = default_value;
            this.value = default_value;
            this.producer = producer;
        }

        readonly T default_value;
        readonly Endo<T> producer;

        T value;

        public T Default_Value {
            get { return default_value; }
        }

        public bool Is_Default {
            get {
                return this.value == default_value;
            }
        }

        public T Value {
            get {
                return this.value;
            }
        }

        /// <summary>
        /// Instructs the wrapper class to create its own personal instance of the 
        /// resource if it is currently using the default value.
        /// </summary>
        public void Split() {

            if (Is_Default)
                this.value = producer(default_value);

        }

        public static implicit operator T(Default_Manager<T> manager) {
            return manager.Value;
        }
    }

}
