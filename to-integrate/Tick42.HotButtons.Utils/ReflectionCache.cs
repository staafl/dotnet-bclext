using System;
using System.Reflection;

namespace Tick42.HotButtons.Utils
{
    /// <summary>
    ///     Caches expensive reflection calls.
    /// </summary>
    public static class ReflectionCache
    {
        public static PropertyInfo GetPropertyCache(this Type type, string name)
        {
            return SimpleCache.GetOrCreate(() => type.GetProperty(name), "ReflectionCache.GetProperty", type, name);
        }

        public static PropertyInfo[] GetPropertiesDeclaredOnlyCache(this Type type)
        {
            const BindingFlags flags = BindingFlags.Public |
                                       BindingFlags.NonPublic |
                                       BindingFlags.Instance |
                                       BindingFlags.DeclaredOnly;

            return SimpleCache.GetOrCreate(() => type.GetProperties(flags), "ReflectionCache.GetPropertiesDeclaredOnlyCache", type);
        }

        public static PropertyInfo[] GetPropertiesCache(this Type type)
        {
            return SimpleCache.GetOrCreate(type.GetProperties, "ReflectionCache.GetPropertiesCache", type);
        }

        public static Type MakeGenericTypeCache(this Type type, params Type[] genericArguments)
        {
            return SimpleCache.GetOrCreate(() => type.MakeGenericType(genericArguments), "ReflectionCache.MakeGenericTypeCache", type, new ArrayEqualityWrapper(genericArguments));
        }
    }
}