using System;
using System.Threading;

namespace Fairweather.Service
{
    // Credit: Jared Parsons' BCL Helpers
    [Immutable]
    public sealed class Thread_Affinity
    {

        public Thread_Affinity() {
            m_threadId = Thread.CurrentThread.ManagedThreadId;
        }

        public void Check() {
            if (Thread.CurrentThread.ManagedThreadId != m_threadId) {
                var msg = String.Format(
                    "Call to class with affinity to thread {0} detected from thread {1}.",
                    m_threadId,
                    Thread.CurrentThread.ManagedThreadId);
                throw new InvalidOperationException(msg);
            }
        }

        readonly int m_threadId;

    }
}