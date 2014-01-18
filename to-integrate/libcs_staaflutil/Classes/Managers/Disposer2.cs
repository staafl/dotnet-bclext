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
    public class Disposer_2<TDisposable, TArg> : IDisposable
    {
        static bool Try_Dispose(TDisposable disposable) {
            return (disposable as IDisposable).Try_Dispose();
        }

        public void Dispose() {
            Dispose(true);
        }

        void Dispose(bool disposing) {
            if (disposing) {
                Try_Dispose(m_disposable);
            }
        }

        readonly bool constructed;



        readonly Func<TArg, TDisposable> func;

        TDisposable m_disposable;

        TArg m_argument;



        public Func<TArg, TDisposable> Func {
            get { return func; }
        }

        public TDisposable Disposable {
            get {
                return this.m_disposable;
            }
        }

        public TArg Argument {
            get {
                return this.m_argument;
            }
            set {
                if (constructed && m_argument.Safe_Equals(value))
                    return;

                Try_Dispose(m_disposable);

                m_argument = value;
                m_disposable = func(m_argument);
            }
        }



        public Disposer_2(Func<TArg, TDisposable> func, TArg argument) {
            func.Throw_If_Null();
            this.func = func;
            Argument = argument;
            constructed = true;
        }

        public static implicit operator TDisposable(Disposer_2<TDisposable, TArg> manager) {
            return manager.Disposable;
        }

        //public static implicit operator TArg(Disposable_Manager<TDisposable, TArg> manager) {
        //      return manager.Argument;
        //}

        //public Disposable_Manager(TDisposable disposable,
        //                  TArg argument) {
        //      this.m_disposable = disposable;
        //      this.m_argument = argument;
        //}


    }

}

