using System;
using System.Runtime.InteropServices;
using System.Threading;
namespace Fairweather.Service
{
    public static class Interlocked2
    {
        public static T
        Mutate<T>(ref T value, Func<T, T> f)
            where T : class {

            T new_val;
            T old;

            do {
                old = value;
                new_val = f(value);
            } while (Interlocked.CompareExchange(ref value, new_val, old) != old);

            return old;
        }

        //public static XXX Mutate(ref XXX value, Func<XXX, XXX> f) where T : class {

        //    XXX new_val;
        //    XXX old;

        //    do {
        //        old = value;
        //        new_val = f(value);
        //    } while (Interlocked.CompareExchange(ref value, new_val, old) != old);

        //    return old;
        //}

        const string kernel32dll = "kernel32.dll";

        const string InterlockedIncrementAcquire = "InterlockedIncrementAcquire";
        const string InterlockedIncrementRelease = "InterlockedIncrementRelease";

        const string InterlockedIncrementAcquire64 = "InterlockedIncrementAcquire64";
        const string InterlockedIncrementRelease64 = "InterlockedIncrementRelease64";


        // http://msdn.microsoft.com/en-us/library/ms683614(VS.85).aspx

        [DllImport(kernel32dll, EntryPoint = InterlockedIncrementAcquire)]
        public static extern int IncrementAcquire(ref int value);

        [DllImport(kernel32dll, EntryPoint = InterlockedIncrementRelease)]
        public static extern int IncrementRelease(ref int value);

        [DllImport(kernel32dll, EntryPoint = InterlockedIncrementAcquire64)]
        public static extern long IncrementAcquire(ref long value);

        [DllImport(kernel32dll, EntryPoint = InterlockedIncrementRelease64)]
        public static extern long IncrementRelease(ref long value);



    }
}
