using System;
using System.Collections.Generic;

namespace Fairweather.Service
{
    /// <summary>
    /// Collections of predicates, mappings etc
    /// </summary>
    public static class F
    {
        public static Func<T> Delay<T>(this T obj) {
            return () => obj;
        }

        ///// <summary>
        ///// Memoize a particular function.  Function can only be called from this thread
        ///// </summary>
        //public static Func<TSource, TResult> 
        //Memoize<TSource, TResult>(this Func<TSource, TResult> func) {
        //    var affinity = new Thread_Affinity();
        //    var map = new Dictionary<TSource, TResult>();
        //    return x =>
        //    {
        //        affinity.Check();
        //        TResult result;
        //        if (!map.TryGetValue(x, out result)) {
        //            result = func(x);
        //            map[x] = result;
        //        }

        //        return result;
        //    };
        //}

        /// <summary>
        /// Return a function which will negate the result of the original function
        /// </summary>
        public static Func<bool>
        Not<TArg1>(this Func<bool> func) {
            return () => !func();
        }

        /// <summary>
        /// Return a function which will negate the result of the original function
        /// </summary>
        public static Func<TArg1, bool>
        Not<TArg1>(this Func<TArg1, bool> func) {
            return (arg1) => !func(arg1);
        }

        ///// <summary>
        ///// Return a function which will negate the result of the original function
        ///// </summary>
        //public static Func<TArg1, TArg2, bool> 
        //Negate<TArg1, TArg2>(this Func<TArg1, TArg2, bool> func) {
        //    return (arg1, arg2) => !func(arg1, arg2);
        //}

        //public static Func<TArg1, TArg2, TArg3, bool> 
        //Negate<TArg1, TArg2, TArg3>(this Func<TArg1, TArg2, TArg3, bool> func) {
        //    return (arg1, arg2, arg3) => !func(arg1, arg2, arg3);
        //}



        public static Endo<T> Compose<T>(Endo<T> f, Endo<T> g) {

            return (arg) => f(g(arg));

        }

        public static void Void() { }
        public static void Void<T>(T _) { }


        public static bool True() { return true; }
        public static bool False() { return false; }

        public static bool IsNull(object obj) { return obj == null; }

        // Is
        // Not
        // Gt
        // Lt
        // Eq
        // Ge
        // Le
        // Ne
    }

}