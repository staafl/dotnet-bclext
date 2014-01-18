using System;
using System.Collections.Generic;
using System.Linq;

static class Helpers
{
    public static bool SameContents<T>(this IEnumerable<T> seq1, IEnumerable<T> seq2, Func<T, int> f)
    {
        return seq1.OrderBy(f).SequenceEqual(
               seq2.OrderBy(f));
    }
    
    public static void ThrowIfNegative(this int num, string msg = null)
    {
        if (num < 0)
        {
            throw GetException<ArgumentOutOfRangeException>(msg ?? "Argument must be nonnegative, got: " + num);
        }
    }

    public static void ThrowIfNull<T>(this T obj, string msg = null)
    {
        if (obj == null)
        {
            throw GetException<ArgumentNullException>(msg ?? "Argument cannot be null.");
        }
    }

    public static void ThrowIfNull<T, TE>(this T obj, string msg = null) where TE : Exception
    {
        if (obj == null)
        {
            throw GetException<TE>(msg ?? "Value cannot be null.");
        }
    }

    public static void ThrowIfNotDefined<TEnum>(this TEnum enumValue, string msg = null)
    {
        if (!Enum.IsDefined(typeof(TEnum), enumValue))
        {
            throw GetException<ArgumentException>(msg ?? "Invalid enum value: " + enumValue);
        }
    }

    public static void ThrowIfNotDefined<TEnum, TE>(this TEnum enumValue, string msg = null)
        where TE : Exception
    {
        if (!Enum.IsDefined(typeof(TEnum), enumValue))
        {
            throw GetException<TE>(msg ?? "Invalid enum value: " + enumValue);
        }
    }

    public static TE GetException<TE>(string msg, Exception innerException = null) where TE : Exception
    {
        var ex = Activator.CreateInstance(typeof(TE), new object[] { msg, innerException });

        return (TE)ex;
    }
    
    public static V GetOrDefault<K,V>(this IDictionary<K,V> dict, K key, V defValue) {
        V ret;
        if (dict.TryGetValue(key, out ret))
            return ret;
        return defValue;
    }
}

















// 2013-06-13
// 2013-06-13
// 2013-06-13
