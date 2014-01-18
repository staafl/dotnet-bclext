using System;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace Fairweather.Service
{
    /// <summary>
    /// Allows you to store objects as sequences of lines.
    /// They better not contain embedded newlines...
    /// </summary>
    public class Simple_Serializer<T> : IFormatter
    {

        public Simple_Serializer(
            int length,
            string prefix,
            Func<T, IEnumerable<string>> ser,
            Func<IEnumerable<string>, T> deser) {
            this.length = length;
            this.prefix = prefix;
            this.ser = ser;
            this.deser = deser;

        }

        #region IFormatter

        public SerializationBinder Binder { get; set; }
        public StreamingContext Context { get; set; }
        public ISurrogateSelector SurrogateSelector { get; set; }

        object IFormatter.Deserialize(Stream s) {
            return Deserialize(s);
        }

        void IFormatter.Serialize(Stream s, object obj) {
            Serialize(s, (T)obj);
        }

        #endregion

        public IEnumerable<T>
        Deserialize(Stream s, int cnt) {

            T temp;
            var sr = new StreamReader(s);
            while (--cnt >= 0 && Deserialize(sr, out temp))
                yield return temp;

        }

        public T
        Deserialize(Stream s) {
            return Deserialize(s, 1).First();
        }

        public bool
        Deserialize(Stream s, out T obj) {
            var sr = new StreamReader(s);

            return Deserialize(sr, out obj);

        }

        public bool
        Deserialize(TextReader sr, out T obj) {

            obj = default(T);
            var lst = new List<string>(length);

            for (int jj = 0; jj < length; ++jj) {
                var line = sr.ReadLine();

                if (line == null)
                    return false;

                if (prefix != null) {
                    if (line.StartsWith(prefix))
                        line = line.Substring(prefix.Length);
                    else
                        return false;
                }

                lst.Add(line);

            }

            obj = deser(lst);
            return true;

        }

        // ****************************

        public void
        Serialize(Stream s, params T[] objs) {
            Serialize(s, objs as IEnumerable<T>);
        }

        public void
        Serialize(Stream s, IEnumerable<T> objs) {

            var sw = new StreamWriter(s);

            try {

                foreach (var obj in objs) {

                    var seq = ser(obj);
                    int ii = 0;

                    foreach (var fld in seq) {
                        ++ii;
                        sw.WriteLine(prefix + fld);
                    }
                    while (ii < length) {
                        ++ii;
                        sw.WriteLine(prefix);
                    }


                }
            }
            finally {
                if (sw != null)
                    sw.Flush();
            }

        }

        // ****************************

        readonly Func<T, IEnumerable<string>> ser;
        readonly Func<IEnumerable<string>, T> deser;
        readonly int length;
        readonly string prefix;


    }
}
