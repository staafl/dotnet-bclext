using System.Collections;
using System.Collections.Generic;
using System;

// Source:
// http://refactormycode.com/codes/945-cached-ienumerable-t
// Author:
// Ants

namespace Fairweather.Service
{
    /// <summary>
    /// Untested
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Cached_Enm<T> : IEnumerator<T>
    {
        public Cached_Enm(int capacity, IEnumerator<T> enm)
            : this(new List<T>(capacity), enm) {
        }

        public Cached_Enm(IEnumerator<T> enm)
            : this(new List<T>(), enm) {
        }

        /// <summary>
        /// You better not delete entries from 'cache'.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="enm"></param>
        public Cached_Enm(List<T> cache, IEnumerator<T> enm) {

            // enm.tifn();
            // cache.tifn();

            this.cache = cache;
            this.enm = enm;
        }


        /*       IEnumerator members        */

        public void Dispose() {
            enm.Dispose();
        }

        public void Reset() {
            enm.Reset();
            ix = -1;
            cache.Clear();
        }

        bool IEnumerator.MoveNext() {
            return MoveNext();
        }

        object IEnumerator.Current {
            get {
                return cache[ix];
            }
        }

        // ****************************

        public T Current {
            get {
                try {
                    return cache[ix];
                }
                catch (ArgumentOutOfRangeException) {
                    if (ix == -1)
                        throw new InvalidOperationException("Enumeration has not started. You need to call MoveNext() prior to accessing Current.");
                    else
                        throw new InvalidOperationException("State corruption. Cache may have been invalidated.");
                }
            }
        }

        public bool MovePrev() {
            return (ix >= 1 && --ix >= 0);

        }

        public bool MoveNext() {

            if (ix + 1 < this.cache.Count) {
                ++ix;
                return true;
            }

            while (true) {

                if (!this.enm.MoveNext())
                    return false;

                this.cache.Add(this.enm.Current);

                if (ix < this.cache.Count)
                    return true;

            }


        }

        // ****************************

        /// <summary>
        /// Discards the specified number of elements from the cache.
        /// Elements are discarded in FIFO order.
        /// If 'count' == null, then assume 'count' == 'Cache.Count'
        /// If 'keep_after_ix', then the call will leave the current element and
        /// all after it in the cache and only remove elements before them.
        /// </summary>
        public int Discard_Cache(int? count, bool keep_after_ix) {

            if (ix == -1)
                // important
                return 0;

            var prev = cache.Count;

            int cnt = count ?? prev;

            cnt = (int)Math.Min(prev, cnt);

            if (keep_after_ix)
                cnt = Math.Min(ix, cnt);

            if (cnt <= 0)
                // empty cache
                // or no elements selected to remove
                return 0;

            cache.RemoveRange(0, cnt);

            var dropped = prev - cache.Count;

            // (dropped == cnt).tiff();

            ix -= dropped;

            if (ix < 0) {

                // did we throw away the current element?
                // don't forget that enumeration has already started,
                // since 'ix' was >= 0
                cache.Add(enm.Current);
                ix = 0;
            }

            return dropped;

        }
        public List<T> Cache {
            get {
                return cache;
            }

        }

        public int Ix {
            get {
                return ix;
            }
            set {
                ix = value;
            }
        }


        /*       State        */

        int ix = -1;

        readonly List<T> cache;

        readonly IEnumerator<T> enm;


    }
    public class Cached_Seq<T> : IEnumerable<T>
    {
        public Cached_Seq(IEnumerable<T> seq, int capacity) {
            this.enm = seq.GetEnumerator();
            this.cache = new List<T>(capacity);

        }

        public Cached_Seq(IEnumerable<T> seq) {
            this.enm = seq.GetEnumerator();
            this.cache = new List<T>();
        }

        readonly IEnumerator<T> enm;
        readonly List<T> cache;


        public IEnumerator<T>
        GetEnumerator() {

            return new Cached_Enm<T>(cache, enm);


        }

        IEnumerator
        IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public List<T> Cache {
            get {
                return cache;
            }
        }

    }

    class ThreadSafeCachedEnumerable<T> : IEnumerable<T>
    {
        public ThreadSafeCachedEnumerable(IEnumerable<T> enumerable) {
            this.enumer = enumerable.GetEnumerator();
        }

        readonly IEnumerator<T> enumer;
        readonly IList<T> cache = new List<T>();


        bool CacheNextItem(int index) {

            lock (enumer) {
                while (index >= this.cache.Count) {
                    if (!this.enumer.MoveNext())
                        return false;
                    this.cache.Add(this.enumer.Current);
                }
            }
            return true;
        }

        public IEnumerator<T> GetEnumerator() {

            int index = 0;

            while (index < this.cache.Count || CacheNextItem(index)) {
                yield return this.cache[index++];
            }
        }

        IEnumerator
        IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

    }

#if Xunit

    public class ThreadSafeCachedEnumerableFacts
    {
        [Fact]
        public void CanHandleEmpty() {
            var enumerable = new ThreadSafeCachedEnumerable<int>(new int[] { });
            Assert.Empty(enumerable);
        }

        [Fact]
        public void CanHandleOne() {
            var enumerable = new ThreadSafeCachedEnumerable<int>(new int[] { 3 });
            Assert.Equal(1, enumerable.Count());
            Assert.True(enumerable.Contains(3));
        }

        [Fact]
        public void CanHandleTwo() {
            var enumerable = new ThreadSafeCachedEnumerable<int>(new int[] { 3, 5 });
            Assert.Equal(2, enumerable.Count());
            Assert.True(enumerable.Contains(3));
            Assert.True(enumerable.Contains(5));
        }

        [Fact]
        public void UsesCache() {
            var enumerable = new ThreadSafeCachedEnumerable<int>(new RandomNumbers(10));
            var list1 = new List<int>(enumerable);
            var list2 = new List<int>(enumerable);

            Assert.Equal(list1.Count, list2.Count);
            for (int i = 0; i < 10; ++i)
                Assert.Equal(list1[i], list2[i]);
        }

        [Fact]
        public void IEnumerableInterfaceWorks() {
            var enumerable = (System.Collections.IEnumerable)new ThreadSafeCachedEnumerable<int>(new int[] { 3 });
            var list = new List<int>();
            foreach (int i in enumerable)
                list.Add(i);
            Assert.Equal(1, list.Count);
            Assert.True(list.Contains(3));
        }

    
    }
#endif
}
