using System;
using System.Threading;

namespace Fairweather.Service
{
    // credit: William Stacey @ http://cid-f4a38e96e598161e.profile.live.com/
    //                          http://staceyw.spaces.live.com/Blog/cns!F4A38E96E598161E!482.entry
    /// <summary>
    /// Condition Variable (CV) class.
    /// </summary>
    public class CV
    {

        public CV(object m) {
            lock (inner) {
                this.outer = m;
            }
        }

        // ****************************


        public void Wait() {
            bool enter = false;
            try {
                lock (inner) {
                    Monitor.Exit(outer);
                    enter = true;
                    Monitor.Wait(inner);
                }
            }
            finally {
                if (enter)
                    Monitor.Enter(outer);
            }
        }

        public void Pulse() {
            lock (inner) {
                Monitor.Pulse(inner);
            }
        }


        public void PulseAll() {
            lock (inner) {
                Monitor.PulseAll(inner);
            }
        }



        // ****************************


        readonly object inner = new object(); // Internal lock.
        readonly object outer;                // The lock associated with this CV.
    }

    public sealed class RW
    {

        public RW() {
            writer_queue = new CV(syncRoot);
        }

        // ****************************

        /// <summary>
        /// Gets a value indicating if a reader lock is held.
        /// </summary>
        public bool IsReaderLockHeld {
            get {
                lock (syncRoot) {
                    if (state > 0)
                        return true;
                    return false;
                }
            }
        }
        /// <summary>
        /// Gets a value indicating if the writer lock is held.
        /// </summary>
        public bool IsWriterLockHeld {
            get {
                lock (syncRoot) {
                    if (state < 0)
                        return true;
                    return false;
                }
            }
        }
        /// <summary>
        /// Aquires the writer lock.
        /// </summary>
        public IDisposable AcquireWriterLock() {
            lock (syncRoot) {
                writeWaiters++;
                while (state != 0)
                    writer_queue.Wait();      // Wait until existing writer frees the lock.
                writeWaiters--;
                state = -1;             // Thread has writer lock.
            }
            return new OnDispose(() => ReleaseWriterLock());
        }
        /// <summary>
        /// Aquires a reader lock.
        /// </summary>
        public IDisposable AcquireReaderLock() {
            lock (syncRoot) {
                readWaiters++;
                // Defer to a writer (one time only) if one is waiting to prevent writer starvation.
                if (writeWaiters > 0) {
                    writer_queue.Pulse();
                    Monitor.Wait(syncRoot);
                }
                while (state < 0)
                    Monitor.Wait(syncRoot);
                readWaiters--;
                state++;
            }
            return new OnDispose(() => ReleaseReaderLock());

        }
        /// <summary>
        /// Releases the writer lock.
        /// </summary>
        public void ReleaseWriterLock() {
            bool doPulse = false;
            lock (syncRoot) {
                state = 0;
                // Decide if we pulse a writer or readers.
                if (readWaiters > 0) {
                    Monitor.PulseAll(syncRoot); // If multiple readers waiting, pulse them all.
                }
                else {
                    doPulse = true;
                }
            }
            if (doPulse)
                writer_queue.Pulse();                     // Pulse one writer if one waiting.
        }
        /// <summary>
        /// Releases a reader lock.
        /// </summary>
        public void ReleaseReaderLock() {
            bool doPulse = false;
            lock (syncRoot) {
                state--;
                if (state == 0)
                    doPulse = true;
            }
            if (doPulse)
                writer_queue.Pulse();                     // Pulse one writer if one waiting.
        }


        // ****************************

        readonly object syncRoot = new object();    // Internal lock.
        readonly CV writer_queue;

        int state = 0;                                  // 0 or greater means readers can pass; -1 is active writer.
        int readWaiters = 0;                        // Readers waiting for writer to exit.
        int writeWaiters = 0;                       // Writers waiting for writer lock.
    }

}

