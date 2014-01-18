using System;

namespace Fairweather.Service
{
    using System.Threading;

    /* Credits for this class go to Peter Ritchie at
     * http://msmvps.com/blogs/peterritchie/archive/2006/10/13/
     * _2700_System.Threading.Thread.Suspend_280029002700_-is-obsolete_3A00_-_2700_
     * Thread.Suspend-has-been-deprecated_2E00__2E00__2E00_.aspx */

    abstract class SuspendableThread : IDisposable
    {
        /* IDisposable */
        public void Dispose() {

            Dispose(true);

        }

        void Dispose(bool @explicit) {

            if (@explicit) {

                suspendChangedEvent.Close(); // <-- Calls Dispose
                terminateEvent.Close();
            }

        }
        /* ----------- */

        readonly ManualResetEvent suspendChangedEvent = new ManualResetEvent(false);

        readonly ManualResetEvent terminateEvent = new ManualResetEvent(false);

        long suspended;

        Thread thread;

        ThreadState failsafeThreadState = ThreadState.Unstarted;


        public SuspendableThread() {

        }

        void ThreadEntry() {

            failsafeThreadState = ThreadState.Stopped;

            OnDoWork();

        }

        protected abstract void OnDoWork();

        /* Usage pattern: */
        //protected override void OnDoWork() {

        //    try {

        //        while (false == HasTerminateRequest()) {
        //        
        //            Boolean awokenByTerminate = SuspendIfNeeded();

        //            if (awokenByTerminate)
        //                return;

        //            // TODO: replace the following two lines
        //            Debug.WriteLine("doing some work...");

        //            Thread.Sleep(450);
        //        }

        //    }
        //    finally {   // TODO: Replace the following line with thread

        //        // exit processing.
        //        Debug.WriteLine("Exiting ThreadEntry()...");
        //        
        //    }

        //}

        /// <summary>
        /// Suspends the thread if there are any requests to do so.
        /// 
        /// Returns true if the thread was woken by a call to Terminate
        /// Returns false if there were no requests for suspending.
        /// Returns false if the thread is resumed.
        /// </summary>
        /// <returns></returns>
        protected Boolean SuspendIfNeeded() {

            bool suspendEventChanged;

            do {


                suspendEventChanged = suspendChangedEvent.WaitOne(0, true);

                if (!suspendEventChanged)
                    return false;

                Boolean needToSuspend = Interlocked.Read(ref suspended) != 0;

                suspendChangedEvent.Reset();

                if (!needToSuspend)
                    return false;

                /// Suspending...
                int index = WaitHandle.WaitAny(new WaitHandle[] { suspendChangedEvent, terminateEvent });

                if (index == 1)
                    return true;

                /// ...Waking
                continue;

            } while (false);

            false.Throw_If_False<InvalidOperationException>("SuspendIfNeeded");
            return false; // <-- should never happen
        }

        protected bool HasTerminateRequest() {

            var ret = terminateEvent.WaitOne(0, true);

            return ret;

        }

        public void Start() {

            thread = new Thread(ThreadEntry);


            // make sure this thread won't be automaticaly
            // terminated by the runtime when the
            // application exits

            thread.IsBackground = false;

            thread.Start();

        }

        public void Join() {

            if (thread != null) {

                thread.Join();

            }

        }

        public Boolean Join(Int32 milliseconds) {

            if (thread != null) {

                var ret = thread.Join(milliseconds);

                return ret;

            }

            return true;

        }

        /// <remarks>Not supported in .NET Compact Framework</remarks>
        public Boolean Join(TimeSpan timeSpan) {

            if (thread != null) {

                var ret = thread.Join(timeSpan);

                return ret;

            }

            return true;

        }

        public void Terminate() {

            terminateEvent.Set();

        }

        public void TerminateAndWait() {

            terminateEvent.Set();

            thread.Join();

        }

        public void Suspend() {

            while (1 != Interlocked.Exchange(ref suspended, 1)) {

            }

            suspendChangedEvent.Set();

        }

        public void Resume() {

            while (0 != Interlocked.Exchange(ref suspended, 0)) {

            }

            suspendChangedEvent.Set();

        }

        public ThreadState ThreadState {

            get {

                if (null != thread) {

                    return thread.ThreadState;

                }

                return failsafeThreadState;

            }
        }
    }
}
