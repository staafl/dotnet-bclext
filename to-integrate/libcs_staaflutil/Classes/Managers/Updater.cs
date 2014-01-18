using System;

namespace Fairweather.Service
{

    /// <summary>
    /// This class encapsulates a T resource, whose value depends on a TArg
    /// source and may be refreshed. The particular rules for refreshing are
    /// determined by the deriving classes.
    /// </summary>
    /// <typeparam name="T">The type of resource to encapsulate.</typeparam>
    /// <typeparam name="TArg">The type of the argument.</typeparam>
    public abstract class Updater<T, TArg> : IDisposable
    {
        ///// <summary>
        ///// If true, every time the resource is refreshed,
        ///// its old value will be disposed.
        ///// </summary>
        public bool
        Auto_Dispose { get; set; }

        public abstract T Value { get; }

        public void Dispose() {
            Value.Try_Dispose();
        }

        public static implicit operator T(Updater<T, TArg> upd) {
            return upd.Value;
        }
    }

}
