using System;
using System.Collections.Generic;

namespace Tick42.HotButtons.Utils
{
    /// <summary>
    /// Caches objects of a given type keying in by cache id and construction arguments.
    /// </summary>
    public static class SimpleCache
    {
        /// <summary>
        /// Uses the 'ctor' function and a cache with id 'cacheId' and keys 'args'
        /// </summary>
        public static T GetOrCreate<T>(Func<T> ctor, string cacheId, params object[] keys)
        {
            return SimpleCacheOfT<T>.GetOrCreate(ctor, cacheId, keys);
        }

        /// <summary>
        /// Uses the default constructor with 'ctorArgs' and a cache with id "SimpleCache.constructor" and arguments 'ctorArgs'
        /// </summary>
        public static T GetOrCreate<T>(params object[] ctorArgs)
        {
            return SimpleCacheOfT<T>.GetOrCreate(() => (T) Activator.CreateInstance(typeof(T), ctorArgs), "SimpleCache.constructor", ctorArgs);
        }

        /// <summary>
        /// Uses the default constructor with 'ctorArgs' and a cache with id "SimpleCache.constructor" and arguments 'args'
        /// </summary>
        public static T GetOrCreate<T>(object[] ctorArgs, params object[] keys)
        {
            return SimpleCacheOfT<T>.GetOrCreate(() => (T) Activator.CreateInstance(typeof(T), ctorArgs), "SimpleCache.constructor", keys);
        }

        static class SimpleCacheOfT<T>
        {
            private static readonly Dictionary<object, T> dict_ = new Dictionary<object, T>();

            public static T GetOrCreate(Func<T> ctor, string cacheId, object[] keys)
            {
                T obj;
                var comparer = new TagEqualityWrapper(cacheId,
                    new TagEqualityWrapper(cacheId, new ArrayEqualityWrapper(keys)));
                if (!dict_.TryGetValue(comparer, out obj))
                {
                    dict_[comparer] = obj = ctor();
                }
                return obj;
            }
        }
    }
}