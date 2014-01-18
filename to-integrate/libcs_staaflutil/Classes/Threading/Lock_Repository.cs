#define RW

using System;
using System.Collections.Generic;
using System.Threading;

#if !RW && !NET20

using ReaderWriterLock = System.Threading.ReaderWriterLockSlim;

#endif

namespace Fairweather.Service
{
    // This class sucks and needs tons of work and is awful...

    /// <summary>
    /// Untested.
    /// The real value of this class is that TId could be a tuple or an ID collection.
    /// It can also be used to lock on strings, types, and other cross-domain objects.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public class Lock_Box<TId>
    {

        public Lock_Box(/* bool auto_clean */) {
            locks = new Dictionary<TId, object>();

        }


#if RW
        readonly RW rw_lock = new RW();
#else
        readonly ReaderWriterLock rw_lock = new ReaderWriterLock();
#endif


        // ****************************

        public int Count_Taken {
            get {
                return locks.Count;
            }
        }
        public void Clean() {

            var ids = new List<TId>();

            using (rw_lock.AcquireReaderLock()) {
                foreach (var kvp in locks) {
                    if (counter[kvp.Value] == 0)
                        ids.Add(kvp.Key);
                }
            }

            using (rw_lock.AcquireWriterLock()) {
                foreach (var id in ids) {
                    locks.Remove(id);
                }
            }

        }


        public IDisposable this[TId id] {
            get {
                return Lock(id);
            }
        }


        public IDisposable Lock(TId id) {

            // Rationale:
            // 
            // Supposing the reader lock contains the writer lock.
            // 
            // t(0)                    t(1)
            // get reader lock     
            //                         get reader lock 
            //                         test dictionary -> false
            //                         get writer lock
            //                         create new value
            //                         call Add
            // test dictionary -> ?
            //                         finish call to add
            // 
            // Obviously, the second test will be performed while the
            // dictionary is in inconsistent state.
            // 
            // Solution:
            // 
            // * Acquire separate reader lock
            // * Test
            // If test is true -> return
            // Otherwise:
            // * release reader lock
            // * acquire writer lock
            // * test again
            // 
            // 
            object ret;
            bool got;
            using (rw_lock.AcquireReaderLock()) {

                got = locks.TryGetValue(id, out ret);
                if (got)
                    return Get(id, ret);

            }

            using (rw_lock.AcquireWriterLock()) {

                // preempted?
                if (!locks.TryGetValue(id, out ret)) {
                    ret = new object();
                    locks[id] = ret;
                }

                return Get(id, ret);

            }


            // simple solution if locks is Lazy_Dict(Make_Lock)
            //lock (locker)
            //    ret = locks[id];



        }


        // ****************************


        IDisposable Get(TId id, object locker) {
            counter.Inc(locker);
            Monitor.Enter(locker);
            return new OnDispose(() =>
            {
                Monitor.Exit(locker);
                Check(id, locker);
            });
        }


        void Check(TId id, object locker) {
            throw new NotImplementedException();
        }




        void Remove(TId id) {

            //using(rw.AcquireWriteLock()) {

            object locker;

            if (!locks.TryGetValue(id, out locker))
                return; // ???

            if (counter[locker] != 0)
                return;

            locks.Remove(id);

            //}

        }


        readonly object locker = new object();

        readonly Dictionary<TId, object> locks;

        readonly Counter<object> counter = new Counter<object>();

    }
}
