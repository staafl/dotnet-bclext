using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Tick42.HotButtons.Utils
{
    /// <summary>
    /// Various helper methods.
    /// </summary>
    public static class Utils
    {
        public static string ToSequenceString<T>(this IEnumerable<T> seq, Func<T, object> func = null, string nullReplacement = null)
        {
            func = func ?? (a => a);
            nullReplacement = nullReplacement ?? "NULL";
            return string.Join(
                ",",
                // ReSharper disable once CompareNonConstrainedGenericWithNull
                seq.OrEmpty().Select(a => a == null ? nullReplacement : func(a)));
        }

        public static IEnumerable<Tuple<string, object>> ReflectProperties(this object obj)
        {
            foreach (var prop in obj.GetType().GetPropertiesCache())
            {
                yield return Tuple.Create(prop.Name, prop.GetValue(obj, null));
            }
        }

        public static string GetTypeName(this object obj)
        {
            if (obj == null)
            {
                return "NULL";
            }
            else
            {
                return obj.GetType().Name;
            }
        }


        public static string Join(this IEnumerable objects, string separator)
        {
            return string.Join(separator, objects);
        }

        public static string Fmt(this string formatString, params object[] arguments)
        {
            return string.Format(formatString, arguments);
        }

        public static IEnumerable<Tuple<string, object>> ReflectDeclaredProperties(this object obj)
        {
            foreach (var prop in obj.GetType().GetPropertiesDeclaredOnlyCache())
            {
                yield return Tuple.Create(prop.Name, prop.GetValue(obj, null));
            }
        }

        /// <summary>
        /// Throws ArgumentNullException if any property of anonymous object is null. Useful for checking arguments in a concise manner.
        /// </summary>
        /// <param name="obj"></param>
        public static void NotNull(this object obj)
        {
            foreach (var prop in obj.ReflectDeclaredProperties())
            {
                if (prop.Item2 == null)
                {
                    throw new ArgumentNullException(prop.Item1);
                }
            }
        }

        /// <summary>
        /// Throws ArgumentNullException if any property of anonymous object is null. Useful for checking arguments in a concise manner.
        /// </summary>
        public static void NotNull(this object obj, ExceptionStreamHelper exceptionStream)
        {
            foreach (var prop in obj.ReflectDeclaredProperties())
            {
                if (prop.Item2 == null)
                {
                    throw exceptionStream.Throw(new ArgumentNullException(prop.Item1));
                }
            }
        }

        private static readonly Dictionary<string, int> getIdDict_ = new Dictionary<string, int>();

        public static string GetId(object obj, bool prefixWithNameOrKey = true, string name = null)
        {
            return GetId(obj.GetTypeName(), prefixWithNameOrKey, name);
        }

        public static string GetId(string key, bool prefixWithNameOrKey = true, string name = null)
        {
            int count;
            lock (getIdDict_)
            {
                if (getIdDict_.TryGetValue(key, out count))
                {
                    getIdDict_[key] = count = count + 1;
                }
                else
                {
                    getIdDict_[key] = count = 1;
                }
            }
            if (prefixWithNameOrKey)
            {
                return (name ?? key) + "." + count;
            }
            else
            {
                return count + "";
            }
        }

        /// <summary>
        ///     Converts an object to a debugging friendly string.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToFriendlyString(this object obj)
        {
            if (obj == null)
            {
                return "[NULL]";
            }
            var enumerable = obj as IEnumerable;
            if (enumerable != null && !(obj is string))
            {
                return "[" + string.Join(", ", enumerable.OfType<object>().Select(ToFriendlyString)) + "]";
            }
            return obj + "";
        }

        /// <summary>
        /// Converts a sequence of tuples to a dictionary.
        /// </summary>
        public static IDictionary<TK, TV> ToDictionary<TK, TV>(this IEnumerable<Tuple<TK, TV>> seq)
        {
            return seq.ToDictionary(t => t.Item1, t => t.Item2);
        }

        /// <summary>
        ///     Converts Ctrl+Alt+A to Keys.Ctrl | Keys.Alt | Keys.A
        /// </summary>
        public static bool TryParseEnum(Type enumType, string asString, out object value)
        {
            value = null;
            if (asString == "")
            {
                value = Enum.ToObject(enumType, 0);
                return true;
            }
            string[] splits = asString.Split('+');
            int intValue = 0;
            foreach (string split in splits)
            {
                object splitValue = SimpleCache.GetOrCreate(() =>
                {
                    try
                    {
                        return Enum.Parse(enumType, split, false);
                    }
                    catch (ArgumentException)
                    {
                        return null;
                    }
                },
                    "TryParseEnum",
                    enumType, split);
                if (splitValue == null)
                {
                    return false;
                }
                intValue |= Convert.ToInt32(splitValue);
            }
            value = intValue;
            return true;
        }

        /// <summary>
        /// Creates a new array with the same elements as the input array and a given size.
        /// </summary>
        public static Array Resize(this Array array, int size)
        {
            if (array.Length == size)
            {
                return array;
            }
            Array newArray = Array.CreateInstance(
                array.GetType().GetElementType(),
                size);
            Array.ConstrainedCopy(array, 0, newArray, 0, size);
            return newArray;
        }

        /// <summary>
        /// Calls the appropriate TryParse method for the given type.
        /// </summary>
        public static bool TryParse(this string str, Type type, out object value)
        {
            value = null;
            if (type == typeof(string))
            {
                value = str;
                return true;
            }

            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            MethodInfo parseMethod = type.GetMethod(
                "TryParse",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new[] { typeof(string), type.MakeByRefType() },
                null);

            if (parseMethod == null)
            {
                return false;
            }
            var arr = new object[] { str, null };
            var ret = (bool) parseMethod.Invoke(null, arr);
            if (ret)
            {
                value = arr[1];
            }
            return ret;
        }

        /// <summary>
        /// Checks if a type implements a generic interface, and returns the generic arguments if so.
        /// </summary>
        /// <returns>If type implements genericInterface, the generic arguments. Else, null.</returns>
        public static Type[] ImplementsGenericInterface(this Type type, Type genericInterface)
        {
            if (type.IsInterface &&
                type.IsGenericType &&
                type.GetGenericTypeDefinition() == genericInterface)
            {
                return type.GetGenericArguments();
            }

            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == genericInterface)
                {
                    return interfaceType.GetGenericArguments();
                }
            }

            return null;
        }

        /// <summary>
        /// Casts an array sequence to an array type, or creates a new one if it is not.
        /// </summary>
        public static T[] AsArray<T>(this IEnumerable<T> seq)
        {
            if (seq == null)
            {
                return null;
            }
            return seq as T[] ?? seq.ToArray();
        }


        /// <summary>
        /// Casts an array sequence to a list type, or creates a new one if it is not.
        /// </summary>
        public static IList<T> AsList<T>(this IEnumerable<T> seq)
        {
            if (seq == null)
            {
                return null;
            }
            return seq as IList<T> ?? seq.ToList();
        }

        /// <summary>
        /// Safely raises an event.
        /// </summary>
        public static void Raise<TArgs>(this Delegate handler, object sender, TArgs args) where TArgs : EventArgs
        {
            if (handler != null)
            {
                handler.DynamicInvoke(sender, args);
            }
        }

        /// <summary>
        /// Safely raises an event.
        /// </summary>
        public static void Raise<TArgs>(this EventHandler<TArgs> handler, object sender, TArgs args)
            where TArgs : EventArgs
        {
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        /// <summary>
        ///     Checks an event for non-nullness and invokes it safely.
        /// </summary>
        public static void Raise(this EventHandler handler, object sender, EventArgs e)
        {
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Substitutes an empty sequence for null.
        /// </summary>
        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> seq)
        {
            return seq ?? new T[0];
        }

        /// <summary>
        /// Creates a grayscale color based on a source color.
        /// </summary>
        public static Color Grayscale(this Color color, float coef)
        {
            Func<float, float, float, float> scale = (a, b, c) =>
                coef * (a + 0.5f * (b + c));
            return Color.FromScRgb(
                color.ScA,
                scale(color.ScR, color.ScG, color.ScB),
                scale(color.ScG, color.ScB, color.ScR),
                scale(color.ScB, color.ScG, color.ScR));
        }

        /// <summary>
        /// Changes the brightness of a color.
        /// </summary>
        public static Color ChangeBrightness(this Color color, float coef)
        {
            return Color.FromScRgb(
                color.ScA,
                color.ScR * coef,
                color.ScG * coef,
                color.ScB * coef);
        }

        /// <summary>
        /// Orders a sequence using the ordering of another sequence. Elements not found in seq2 are left at the end.
        /// </summary>
        public static IEnumerable<T> OrderBy<T, T2>(this IEnumerable<T> seq, IEnumerable<T2> seq2, Func<T, T2> action)
        {
            var dict = new Dictionary<T2, int>();
            int counter = 0;
            foreach (T2 elem in seq2)
            {
                dict[elem] = counter;
                counter += 1;
            }
            return seq.OrderBy(
                elem =>
                {
                    int order;
                    return dict.TryGetValue(action(elem), out order) ? order : int.MaxValue;
                });
        }

        /// <summary>
        /// Tags elements from a sequence using a tuple.
        /// </summary>
        public static IEnumerable<Tuple<T, TTag>> Tag<T, TTag>(this IEnumerable<T> seq, TTag tag)
        {
            return seq.Select(elem => Tuple.Create(elem, tag));
        }
    }
}