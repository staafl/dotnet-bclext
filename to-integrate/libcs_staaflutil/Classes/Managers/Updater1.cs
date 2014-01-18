using System;

namespace Fairweather.Service
{
    /// <summary>
    /// This class encapsulates a T resource.
    /// Every time Updater::Value is fetched, Updater1::Argument
    /// is compared to the return value of Updater1::F_Argument and,
    /// if those two are different, the held resource and argument are
    /// changed to F_Argument() and Updater1::F_Value(F_Argument())
    /// </summary>
    /// <typeparam name="T">The type of resource to encapsulate.</typeparam>
    /// <typeparam name="TArg">The type of the argument.</typeparam>
    public class Updater1<T, TArg> : Updater<T, TArg>
    {
        public Updater1(Func<TArg> arg, Func<TArg, T> producer) {
            this.f_arg = arg;
            this.f_value = producer;
            Refresh(f_arg());
        }

        readonly Func<TArg> f_arg;
        readonly Func<TArg, T> f_value;

        public Func<TArg> F_Argument {
            get { return f_arg; }
        }
        public Func<TArg, T> F_Value {
            get { return f_value; }
        }


        T value;
        TArg arg;


        void Refresh(TArg new_arg) {

            arg = new_arg;

            if (Auto_Dispose)
                value.Try_Dispose();

            value = f_value(new_arg);

        }

        public bool Is_Fresh {
            get {
                TArg new_arg;
                return Check_Fresh(out new_arg);
            }
        }

        public bool Check_Fresh(out TArg new_arg) {

            new_arg = f_arg();

            return arg.Safe_Equals(new_arg);

        }

        void Maybe_Refresh() {

            TArg new_arg;

            if (!Check_Fresh(out new_arg))
                Refresh(new_arg);

        }

        public override T Value {
            get {
                Maybe_Refresh();
                return value;
            }
        }

        public TArg Argument {
            get { return arg; }
        }


        ///// <summary>
        ///// If true, every time the resource is refreshed,
        ///// its old value will be disposed.
        ///// </summary>
        //public bool Auto_Dispose {
        //    get;
        //    set;
        //}


        //public static implicit operator T(Updater1<T, TArg> upd) {
        //    return upd.Value;
        //}

    }
}
