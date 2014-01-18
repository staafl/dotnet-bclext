using System;
using System.Runtime.InteropServices;
namespace Fairweather.Service
{
    public class On_Dispose : IDisposable
    {
        /// Possible concerns:
        ///     * finalization
        ///     * multiple calls
        ///     * identity of the wrapper

        public On_Dispose(Action action) {

            action.tifn();
            m_action = action;

        }

        public void Dispose() {

            if (!disposed) {
                disposed = true;
                m_action();
            }

        }

        readonly Action m_action;
        bool disposed;

        // ****************************

        class nothing : IDisposable
        {
            public nothing() { }
            public void Dispose() { }
        }

        /// <summary>
        /// Returns a no-op instance
        /// </summary>
        static public IDisposable Nothing {
            get {
                return new nothing();
            }
        }

        static public IDisposable Compose(IDisposable obj1, IDisposable obj2) {

            return new On_Dispose(
                () =>
                {
                    obj1.Dispose();
                    obj2.Dispose();
                });

        }
        static public IDisposable Compose(IDisposable obj, Action before, Action after) {

            return new On_Dispose(
                () =>
                {
                    if (before != null)
                        before();
                    obj.Dispose();
                    if (after != null)
                        after();
                });

        }

        static public IDisposable Compose(IDisposable obj, Action act, bool before) {

            return new On_Dispose(
                () =>
                {
                    if (before)
                        act();
                    obj.Dispose();
                    if (!before)
                        act();
                });

        }

        static public IDisposable Com_Object(object obj) {

            return new On_Dispose(
                () =>
                {
                    if (obj != null) {
                            Marshal.FinalReleaseComObject(obj);
                        // Marshal.ReleaseComObject(obj);
                    }
                });

        }




    }
}
