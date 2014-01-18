using System;
using System.Threading;
namespace Fairweather.Service
{
    public class Counted_Lock
    {
        public Counted_Lock() {

        }

        public IDisposable Lock() {

            Monitor.Enter(this);

            try {
                if (Interlocked.Increment(ref taken) == 1)
                    NewAcquired.Raise(this);

                Acquired.Raise(this);
            }
            catch {
                Interlocked.Decrement(ref taken);
                Monitor.Exit(this);
            }

            return new OnDispose(() => this.Unlock());

        }

        public void Unlock() {

            try {

                if(Monitor.TryEnter(this, 0))
                    true.Throw_If_True<InvalidOperationException>("lock not held");

                int state = Interlocked.Decrement(ref taken);

                if (state < 0) {
                    taken = 0;
                    true.Throw_If_True<InvalidOperationException>("state corruption: " + state);
                }

                Released.Raise(this);

                if (state == 0)
                    FullReleased.Raise(this);



            }
            finally {
                Monitor.Exit(this);
            }

        }

        int taken;

        /// <summary>
        /// Raised immediately before the lock is released.
        /// </summary>
        public event EventHandler Released;
        /// <summary>
        /// Raised immediately before the lock is released if the lock is the
        /// will become free as result of the release.
        /// </summary>
        public event EventHandler FullReleased;

        /// <summary>
        /// Occurs immediately after the lock is acquired.
        /// </summary>
        public event EventHandler Acquired;
        /// <summary>
        /// Occurs immediately after the lock is acquired if the lock was
        /// free before the acquisition.
        /// </summary>
        public event EventHandler NewAcquired;

        /// <summary>
        /// The number of times the lock needs to be released to become free.
        /// </summary>
        public int Taken { get { return Thread.VolatileRead(ref taken); } }
    }

}
