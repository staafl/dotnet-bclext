using System;

namespace Fairweather.Service
{
    /// <summary>
    /// This class encapsulates a T resource, updating it
    /// every time Updater2::Argument is assigned a new value.
    /// </summary>
    public class Updater2<T, TArg> : Updater<T, TArg>
    {
        public Updater2(TArg argument, T initial, Func<TArg, T> producer) {

            this.f_value = producer;
            this.arg = argument;
            value = initial;

        }

        public Updater2(TArg argument, Func<TArg, T> producer) {

            this.f_value = producer;
            this.arg = argument;
            Refresh();

        }

        /// <summary>
        /// The function used to update the stored value.
        /// </summary>
        readonly Func<TArg, T> f_value;
        T value;
        TArg arg;


        void Refresh() {

            if (Auto_Dispose)
                value.Try_Dispose();

            value = f_value(arg);

        }

        public TArg Argument {
            get { return arg; }
            set {
                if (arg.Safe_Equals(value))
                    return;

                arg = value;
                Refresh();
            }
        }


        public override T Value {
            get {
                return value;
            }
        }



        ///// <summary>
        ///// If true, every time the resource is refreshed,
        ///// its old value will be disposed.
        ///// </summary>
        //public bool Auto_Dispose {
        //    get;
        //    set;
        //}


        //public static implicit operator T(Updater2<T, TArg> upd) {
        //    return upd.Value;
        //}

    }
}
