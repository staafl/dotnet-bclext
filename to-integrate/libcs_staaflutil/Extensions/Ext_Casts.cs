using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Fairweather.Service
{

    static partial class Extensions
    {

        public static string str(this object obj) {
            return obj.ToString();
        }

        public static string strdef(this object obj) {
            return (obj ?? (object)"").ToString();
        }

        public static string strdef(this object obj, string def) {
            return (obj ?? (object)def).ToString();
        }

        public static T To<T>(this object obj) {
            return ((T)obj);
        }

        // http://stackoverflow.com/questions/271398/post-your-extension-goodies-for-c-net-codeplex-com-extensionoverflow?answer=274652#274652
        public static T To2<T>(this IConvertible obj) {
            return (T)Convert.ChangeType(obj, typeof(T));
        }



        // http://jacobcarpenters.blogspot.com/2006/06/c-30-and-delegate-conversion.html

        public static T Convert_Delegate<T>(this Delegate source) where T : class {

            if (source.GetInvocationList().Length > 1)
                throw new ArgumentException("Cannot safely convert MulticastDelegate");

            var ret = Delegate.CreateDelegate(typeof(T), source.Target, source.Method) as T;

            return ret;


        }
        /*       Casts        */

        public static T
        To_Enum<T>(this string value, bool ignore_case) where T : struct {

            return (T)Enum.Parse(typeof(T), value, ignore_case);

        }

        public static T
        To_Enum<T>(this object value) where T : struct {

            return (T)Enum.ToObject(typeof(T), value);

        }

        public static T?
        To_Enum_<T>(this object value) where T : struct {

            if (Enum.IsDefined(typeof(T), value))
                return (T?)Enum.ToObject(typeof(T), value);

            return null;

        }

        public static T
        To_Enum<T>(this int value) where T : struct {

            return (T)Enum.ToObject(typeof(T), value);

        }

        public static T?
        To_Enum_<T>(this int value) where T : struct {

            if (Enum.IsDefined(typeof(T), value))
                return (T?)Enum.ToObject(typeof(T), value);

            return null;

        }

        public static Int64?
        ToInt64_(this object obj) {

            if (obj.IsNullOrEmpty())
                return null;
            try {
                return Convert.ToInt64(obj);
            }
            catch (FormatException) {
                return null;
            }
            catch (InvalidCastException) {
                return null;
            }

        }

        public static Int64
        ToInt64(this object obj) {

            return Convert.ToInt64(obj);

        }


        public static Decimal?
        ToDecimal_(this object obj) {

            if (obj.IsNullOrEmpty())
                return null;
            try {
                return Convert.ToDecimal(obj);
            }
            catch (FormatException) {
                return null;
            }
            catch (InvalidCastException) {
                return null;
            }

        }

        public static Decimal
        ToDecimal(this object obj) {

            return Convert.ToDecimal(obj);

        }

        public static Double?
        ToDouble_(this object obj) {

            if (obj.IsNullOrEmpty())
                return null;
            try {
                return Convert.ToDouble(obj);
            }
            catch (FormatException) {
                return null;
            }
            catch (InvalidCastException) {
                return null;
            }

        }

        public static Double
        ToDouble(this object obj) {

            return Convert.ToDouble(obj);

        }



        public static Single?
        ToSingle_(this object obj) {

            if (obj.IsNullOrEmpty())
                return null;
            try {
                return Convert.ToSingle(obj);
            }
            catch (FormatException) {
                return null;
            }
            catch (InvalidCastException) {
                return null;
            }

        }

        public static Single
        ToSingle(this object obj) {

            return Convert.ToSingle(obj);

        }


        public static short
        ToInt16(this object obj) {

            return Convert.ToInt16(obj);

        }

        public static short?
        ToInt16_(this object obj) {

            if (obj.IsNullOrEmpty())
                return null;
            try {
                return Convert.ToInt16(obj);
            }
            catch (FormatException) {
                return null;
            }
            catch (InvalidCastException) {
                return null;
            }

        }

        public static int
        ToInt32(this object obj) {

            return Convert.ToInt32(obj);

        }

        public static int?
        ToInt32_(this object obj) {

            if (obj.IsNullOrEmpty())
                return null;
            try {
                return Convert.ToInt32(obj);
            }
            catch (FormatException) {
                return null;
            }
            catch (InvalidCastException) {
                return null;
            }

        }


        public static UInt16?
        ToUInt16_(this object obj) {

            if (obj.IsNullOrEmpty())
                return null;
            try {
                return Convert.ToUInt16(obj);
            }
            catch (FormatException) {
                return null;
            }
            catch (InvalidCastException) {
                return null;
            }

        }

        public static UInt16
        ToUInt16(this object obj) {

            return Convert.ToUInt16(obj);

        }



        public static UInt64?
        ToUInt64_(this object obj) {

            if (obj.IsNullOrEmpty())
                return null;
            try {
                return Convert.ToUInt64(obj);
            }
            catch (FormatException) {
                return null;
            }
            catch (InvalidCastException) {
                return null;
            }

        }

        public static UInt64
        ToUInt64(this object obj) {

            return Convert.ToUInt64(obj);

        }


        public static DateTime?
        ToDateTime_(this object obj) {

            if (obj.IsNullOrEmpty())
                return null;
            try {
                return Convert.ToDateTime(obj);
            }
            catch (FormatException) {
                return null;
            }
            catch (InvalidCastException) {
                return null;
            }

        }

        public static DateTime
        ToDateTime(this object obj) {

            return Convert.ToDateTime(obj);

        }


        public static Char?
        ToChar_(this object obj) {

            if (obj.IsNullOrEmpty())
                return null;
            try {
                return Convert.ToChar(obj);
            }
            catch (FormatException) {
                return null;
            }
            catch (InvalidCastException) {
                return null;
            }

        }

        public static Char
        ToChar(this object obj) {

            return Convert.ToChar(obj);

        }



        public static UInt32?
        ToUInt32_(this object obj) {

            if (obj.IsNullOrEmpty())
                return null;
            try {
                return Convert.ToUInt32(obj);
            }
            catch (FormatException) {
                return null;
            }
            catch (InvalidCastException) {
                return null;
            }

        }

        public static UInt32
        ToUInt32(this object obj) {

            return Convert.ToUInt32(obj);

        }


        public static SByte?
        ToSByte_(this object obj) {

            if (obj.IsNullOrEmpty())
                return null;
            try {
                return Convert.ToSByte(obj);
            }
            catch (FormatException) {
                return null;
            }
            catch (InvalidCastException) {
                return null;
            }

        }



        public static SByte
        ToSByte(this object obj) {

            return Convert.ToSByte(obj);

        }

        public static Byte?
        ToByte_(this object obj) {

            if (obj.IsNullOrEmpty())
                return null;
            try {
                return Convert.ToByte(obj);
            }
            catch (FormatException) {
                return null;
            }
            catch (InvalidCastException) {
                return null;
            }

        }

        public static Byte
        ToByte(this object obj) {

            return Convert.ToByte(obj);

        }



        public static bool?
        ToBool_(this object obj) {

            if (obj.IsNullOrEmpty())
                return null;
            try {
                return Convert.ToBoolean(obj);
            }
            catch (FormatException) {
                return null;
            }
            catch (InvalidCastException) {
                return null;
            }

        }

        public static bool
        ToBool(this object obj) {

            return Convert.ToBoolean(obj);

        }




        // ****************************
#if !LITE


        public static IRead<TKey, TValue>
        ro<TKey, TValue>(this IReadWrite<TKey, TValue> irw) {

            return new Proxy<TKey, TValue>(irw);

        }

        public static ReadOnlyCollection<T>
        ro<T>(this T[] array) {
            return Array.AsReadOnly(array);
        }


        static public Dictionary<T1, T2>
        dict<T1, T2>(this IEnumerable<Pair<T1, T2>> seq) {
            return seq.ToDictionary(pair => pair.First, pair => pair.Second);
        }

        static public IEnumerable<Pair<T1, T2>>
        pairs<T1, T2>(this IEnumerable<KeyValuePair<T1, T2>> seq) {
            foreach (var kvp in seq)
                yield return (Pair<T1, T2>)kvp;

            //return seq.Cast<Pair<T1, T2>>();
        }


        public static IEnumerable<KeyValuePair<T1, T2>>
        kvps<T1, T2>(this IEnumerable<Pair<T1, T2>> seq) {
            return seq.Cast<KeyValuePair<T1, T2>>();
        }

        static public RW_Dict<TIx, TValue>
        rw<TIx, TValue>(this Dictionary<TIx, TValue> dict) {
            return new RW_Dict<TIx, TValue>(dict);
        }

        static public RW_IDict<TIx, TValue>
        rwi<TIx, TValue>(this IDictionary<TIx, TValue> dict) {
            return new RW_IDict<TIx, TValue>(dict);
        }

        static public RW_List<TValue>
        rw<TIx, TValue>(this List<TValue> list) {
            return new RW_List<TValue>(list);
        }

        static public RW_IList<TValue>
        rwi<TIx, TValue>(this IList<TValue> list) {
            return new RW_IList<TValue>(list);
        }

        static public RW_Array<TValue>
        rw<TIx, TValue>(this TValue[] arr) {
            return new RW_Array<TValue>(arr);
        }

        public static string
        str(this IEnumerable<char> chars) {

            string ret;

            int cnt = chars.Count();

            if (cnt < 40) {

                ret = new String(chars.ToArray());

            }
            else {

                var sb = new StringBuilder(cnt);
                sb.Append(chars.ToArray());
                ret = sb.ToString();

            }

            return ret;

        }

        public static T[]
        arr<T>(this IEnumerable<T> seq) {
            return seq.ToArray();
        }

        public static List<T>
        lst<T>(this IEnumerable<T> seq) {
            return seq.ToList();
        }

        public static Set<T>
        set<T>(this IEnumerable<T> seq) {
            return new Set<T>(seq);
        }

        public static Stack<T>
        stack<T>(this IEnumerable<T> seq) {
            return new Stack<T>(seq);
        }

        public static Queue<T>
        queue<T>(this IEnumerable<T> seq) {
            return new Queue<T>(seq);
        }



        // ****************************


        public static List<T>
        lsta<T>(this IEnumerable<T> seq) {
            return seq as List<T> ?? seq.ToList();
        }

        public static Set<T>
        seta<T>(this IEnumerable<T> seq) {
            return seq as Set<T> ?? seq.set();

        }

        public static Stack<T>
        stacka<T>(this IEnumerable<T> seq) {
            return seq as Stack<T> ?? new Stack<T>(seq);
        }

        public static Queue<T>
        queuea<T>(this IEnumerable<T> seq) {
            return seq as Queue<T> ?? new Queue<T>(seq);
        }
#endif
    }
}
