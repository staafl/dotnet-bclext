using System;
using System.Collections.Generic;

using Fairweather.Service;

namespace Fairweather.Service
{
    //public static class Shell
    //{
    //    public static Shell<T>
    //    Make<T>(T obj) {
    //        return new Shell<T>(obj);
    //    }
    //}

    public static class Array_ro
    {
        public static Array_ro<T>
        Make<T>(this IEnumerable<T> seq) {
            return new Array_ro<T>(seq);
        }
    }
    public static class List_ro
    {
        public static List_ro<T>
        Make<T>(this IEnumerable<T> seq) {
            return new List_ro<T>(seq);
        }
        public static List_ro<T>
        Make<T>(this IList<T> ilist) {
            return new List_ro<T>(ilist);
        }
    }
    public static class Dict_ro
    {
        public static Dict_ro<TKey, TValue>
        Make<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> seq) {
            return new Dict_ro<TKey, TValue>(seq);
        }
        public static Dict_ro<TKey, TValue>
        Make<TKey, TValue>(this IDictionary<TKey, TValue> idict) {
            return new Dict_ro<TKey, TValue>(idict);
        }
    }


    public static class KVP
    {
        public static KeyValuePair<TKey, TValue>
        Make<TKey, TValue>(TKey key, TValue value) {
            return new KeyValuePair<TKey, TValue>(key, value);
        }
    }




    //public static class Option
    //{
    //    static public bool
    //    Has_Value<T>(this Option<T> opt) {
    //        return opt != null;
    //    }

    //    static public T Value_Or_Default<T>(this Option<T> opt) {
    //        if (opt.Has_Value())
    //            return opt.Value;

    //        return default(T);
    //    }

    //    public static Option<T> Make<T>(T value) {
    //        return Option<T>.Make(value);
    //    }

    //    public static Option<T> Empty<T>() {
    //        return null;
    //    }
    //}

    public static class Updater
    {
        public static Updater1<T, TArg>
        Make1<T, TArg>(Func<TArg> arg, Func<TArg, T> producer) {
            bool dispose = false;
            return Make1<T, TArg>(arg, producer, dispose);
        }

        public static Updater1<T, TArg>
        Make1<T, TArg>(Func<TArg> arg, Func<TArg, T> producer, bool dispose) {
            var ret = new Updater1<T, TArg>(arg, producer);
            ret.Auto_Dispose = dispose;
            return ret;
        }

        public static Updater2<T, TArg>
        Make2<T, TArg>(TArg arg, Func<TArg, T> producer) {
            bool dispose = false;
            return Make2<T, TArg>(arg, producer, dispose);
        }

        public static Updater2<T, TArg>
        Make2<T, TArg>(TArg arg, Func<TArg, T> producer, bool dispose) {
            var ret = new Updater2<T, TArg>(arg, producer);
            ret.Auto_Dispose = dispose;
            return ret;
        }


    }

    //public static class Guard
    //{
    //    public static Guard<T> Make<T>(Func<T, bool> contract) {
    //        return new Guard<T>(contract);
    //    }

    //    public static Guard<T> Make<T>(Func<T, bool> contract, T initial) {
    //        return new Guard<T>(contract, initial);
    //    }


    //    public class Positive : Guard<int>
    //    {
    //        public Positive() : base(ii => ii > 0) { }

    //        public Positive(int start) : base(ii => ii > 0, start) { }

    //        public static implicit operator int(Positive wrapper) {
    //            return wrapper.Value;
    //        }

    //        public static implicit operator Positive(int integer) {
    //            return new Positive(integer);
    //        }

    //    }

    //    public class Non_Neg : Guard<int>
    //    {

    //        public Non_Neg() : base(ii => ii >= 0) { }

    //        public Non_Neg(int start) : base(ii => ii >= 0, start) { }

    //        public static implicit operator int(Non_Neg wrapper) {
    //            return wrapper.Value;
    //        }

    //        public static implicit operator Non_Neg(int integer) {
    //            return new Non_Neg(integer);
    //        }

    //    }

    //}

    public static class Disposer
    {

        public static Disposer<TDisposable>
        Make1<TDisposable>(TDisposable disposable)
            where TDisposable : IDisposable {
            return new Disposer<TDisposable>(disposable);
        }

        //public static Disposer_2<TDisposable, TArg>
        //Make2<TDisposable, TArg>(Func<TArg, TDisposable> func, TArg arg) {
        //    return new Disposer_2<TDisposable, TArg>(func, arg);
        //}

    }

    public static class Lazy
    {
        public static Lazy<T>
        Make<T>(Func<T> producer) {
            return new Lazy<T>(producer);
        }

        public static T?
        Fresh_Or_Null<T>(this Lazy<T> man) where T : struct {

            if (man.Fresh)
                return man;

            return null;

        }

    }

    //public static class Not_Null
    //{
    //    public static Not_Null<T> Make<T>(T value) where T : class {
    //        return new Not_Null<T>(value);
    //    }
    //}


}