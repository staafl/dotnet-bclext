using System;
using System.Collections.Generic;

#if WINFORMS
using System.Drawing;
#endif

namespace Fairweather.Service
{
    /* Various helper methods */
    static partial class Extensions
    {

        static public char ToUpper(this char ch) {

            var ret = Char.ToUpper(ch);
            return ret;
        }

        static public IEnumerable<T>
        OrEmpty<T>(this IEnumerable<T> seq) {
            return seq ?? new T[0];
        }

        // ****************************


        // NO MORE!!!!!!

        // null ? def() : f(obj)
        static public TRet
        OrDefault<T1, TRet>(this T1 obj,
                            Func<T1, TRet> f,
                            Func<TRet> def) {
            if (obj == null)
                return def();
            return f(obj);
        }

        // null ? def() : obj
        static public T
        OrDefault<T>(this T obj, Func<T> def) {

            if (obj == null)
                return def();

            return obj;

        }


        // ****************************


        // null ? def : f(obj)
        static public TRet
        OrDefault_<T1, TRet>(this T1 obj,
                             Func<T1, TRet> f,
                             TRet def) {
            if (obj == null)
                return def;
            return f(obj);
        }


        // null ? def : obj
        static public T
        OrDefault_<T>(this T obj, T def) {

            if (obj == null)
                return def;
            return def;

        }






        // ****************************



        static public T
        OrNew<T>(this T nullable)
            where T : class, new() {

            return nullable ?? new T();

        }

        static public T
        OrNew<T>(this T? nullable)
            where T : struct {

            return nullable ?? new T();

        }



        // ****************************



        static public Font Change_Size(this Font font, float size, bool dispose) {

            using (dispose ? font : On_Dispose.Nothing) {
                var ret = new Font(font.FontFamily, size, font.Style);
                return ret;
            }

        }

        static public Font Underline(this Font font, bool dispose) {

            using (dispose ? font : On_Dispose.Nothing) {
                var ret = new Font(font, font.Style | FontStyle.Underline);
                return ret;
            }

        }

        static public Font Bold(this Font font, bool dispose) {

            using (dispose ? font : On_Dispose.Nothing) {
                var ret = new Font(font, font.Style | FontStyle.Bold);
                return ret;
            }

        }

        static public Font Italic(this Font font, bool dispose) {

            using (dispose ? font : On_Dispose.Nothing) {
                var ret = new Font(font, font.Style | FontStyle.Italic);
                return ret;
            }
        }

        static public Font Regular(this Font font, bool dispose) {

            using (dispose ? font : On_Dispose.Nothing) {
                var ret = new Font(font, FontStyle.Regular);
                return ret;
            }
        }

#if WINFORMS


        public static StringFormat Far_Aligned_V(this StringFormat format, bool dispose) {

            var ret = new StringFormat(format);

            ret.Alignment = StringAlignment.Far;

            if (dispose)
                format.Dispose();

            return ret;

        }

        public static StringFormat Far_Aligned_H(this StringFormat format, bool dispose) {

            var ret = new StringFormat(format);

            ret.LineAlignment = StringAlignment.Far;

            if (dispose)
                format.Dispose();


            return ret;

        }


        public static StringFormat Near_Aligned_V(this StringFormat format, bool dispose) {

            var ret = new StringFormat(format);

            ret.Alignment = StringAlignment.Near;

            if (dispose)
                format.Dispose();


            return ret;

        }

        public static StringFormat Near_Aligned_H(this StringFormat format, bool dispose) {

            var ret = new StringFormat(format);

            ret.LineAlignment = StringAlignment.Near;

            if (dispose)
                format.Dispose();

            return ret;

        }


        public static StringFormat Word_Trimmed(this StringFormat format, bool dispose) {

            var ret = new StringFormat(format);

            ret.Trimming = StringTrimming.Word;

            if (dispose)
                format.Dispose();


            return ret;

        }

#endif
    }
}
