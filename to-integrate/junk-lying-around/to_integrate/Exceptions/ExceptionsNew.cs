using System;
using System.Collections.Generic;
using System.Linq;

namespace staafl.libc
{
    static class ExceptionHelpers
    {
        // throw if empty
        // throw if not any of
        
        public static void ThrowIfTrue<TE>(this bool condition, string msg = null)
            where TE : Exception
        {
            if (condition)
            {
                throw GetException<TE>(msg ?? "Argument must be nonnegative, got: " + num);
            }
        }
        
        public static void ThrowIfFalse<TE>(this bool condition, string msg = null)
            where TE : Exception
        {
            if (!condition)
            {
                throw GetException<TE>(msg);
            }
        }
        
        
        public static void ThrowIfNegative<TE>(this dynamic num, string msg = null)
            where TE : Exception
        {
            if (num < 0)
            {
                throw GetException<TE>(msg, got: " + num);
            }
        }
        
        public static void ThrowIfNegative(this dynamic num, string msg = null)
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

        static TE GetException<TE>(string msg, Exception innerException = null) where TE : Exception
        {

            var ex = Activator.CreateInstance(typeof(TE), new object[] { msg, innerException });

            return (TE)ex;

        }
    }
}
