using System;
using System.Diagnostics;

namespace Fairweather.Service
{
    /// <summary>
    /// This class encapsulates a Disposable resource,
    /// disposed automatically when the instance is assigned 
    /// a new resource.
    /// </summary>
    [DebuggerStepThrough]
    public class Disposer<T> : IDisposable
    {

        public Disposer(T value) {
            this.value = value;
        }

        public void Dispose() {
            Dispose(true);
        }

        void Dispose(bool disposing) {
            if (disposing)
                value.Try_Dispose();
        }

        T value;

        public T Value {
            get {
                return this.value;
            }
            set {
                if (this.value.Safe_Equals(value))
                    return;

                this.Value.Try_Dispose();
                this.value = value;
            }
        }

        public static implicit operator T(Disposer<T> manager) {
            return manager.Value;
        }

        public static implicit operator Disposer<T>(T value) {
            return new Disposer<T>(value);
        }


    }



}

