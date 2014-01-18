using System;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
namespace Fairweather.Service
{
    public class Csv_Formatter<T> : IFormatter
    {
        static FileStream Get_Stream(string file) {
            return new FileStream(file, FileMode.OpenOrCreate);
        }

        static bool Test() {

            var csv_ser = new Csv_Formatter<Rectangle>(
            _rect => new object[] { _rect.X, _rect.Y, _rect.Width, _rect.Height },
            _vals =>
            {
                var _arr = _vals.Select(_v => _v.ToInt32()).arr();
                return new Rectangle(_arr[0], _arr[1], _arr[2], _arr[3]);
            });

            const int length = 10;
            var seq = Magic.range(0, length - 1);
            var before = seq.Combos(seq, seq, seq).Select(_q => new Rectangle(_q.First, _q.Second, _q.Third, _q.Fourth)).lst();
            var file = "test.txt";
            File.Delete(file);
            using (var fs = Get_Stream(file)) {
                csv_ser.Serialize(fs, before);
            }
            H.Run_Notepad(file, true, true);
            using (var fs = Get_Stream(file)) {
                var after = csv_ser.Deserialize(fs, before.Count).lst();
                if (!before.Deep_Equals(after))
                    return false;
            }
            return true;
        }



        public Csv_Formatter(ICsvFormatter<T> formatter) :
            this(formatter.Serialize, formatter.Deserialize) {

        }

        /// <summary>
        /// Creates a Csv_Serializer() instance that can map
        /// between csv file headers and properties on T
        /// </summary>
        public Csv_Formatter(Func<string, string, object> f) {
            throw new NotImplementedException();
            //this.has_headers = false;
        }

        /// <summary>
        /// Creates a Csv_Serializer
        /// </summary>
        /// <param name="f2"></param>
        /// <param name="f1"></param>
        public Csv_Formatter(Func<T, IEnumerable<object>> ser, Func<IEnumerable<string>, T> deser) {

            this.deser = deser;
            this.ser = ser;

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
            var sr = new StreamReader(s);
            try {
                var ret = Deserialize(sr, cnt);
                sr.DiscardBufferedData();
                return ret;

            }
            catch {
                sr.Try_Dispose();
                throw;
            }
        }

        public IEnumerable<T>
        Deserialize(TextReader tr, int? cnt) {
            //using (
            var reader = new Csv_Parser(tr);
            //) {
            for (int ii = 0; !(ii >= cnt); ++ii) {
                var lst = reader.Get_Next_Record();
                if (lst == null)
                    yield break;
                var obj = deser(lst);
                yield return obj;
            }
            // }
        }

        public T
        Deserialize(TextReader tr) {
            return Deserialize(tr, 1).First();
        }

        public T
        Deserialize(Stream s) {
            return Deserialize(s, 1).First();
        }

        public void
        Serialize(Stream s, params T[] objs) {
            Serialize(s, objs as IEnumerable<T>);
        }

        public void
        Serialize(Stream s, IEnumerable<T> objs) {
            //using (
            var sw = new StreamWriter(s);
            try {

                //) {
                foreach (var obj in objs) {
                    var seq = ser(obj);
                    var fields = seq.Select(_obj => _obj.strdef());
                    var str = fields.Csv();
                    sw.WriteLine(str);

                }
            }
            finally {
                if (sw != null)
                    sw.Flush();
            }
            //}
        }


        readonly Func<IEnumerable<string>, T> deser;
        readonly Func<T, IEnumerable<object>> ser;
    }
}
