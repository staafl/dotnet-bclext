using System;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using Common.Sage;

namespace Common.Posting
{
    /// <summary>
    /// A bunch of sage field/value pairs as they
    /// might appear in a CSV file or a similar representation.
    /// </summary>
    public class Data_Line : Sage_Collection
    {
        //Lazy_Dict<Sage_Field, object> dict = new Lazy_Dict<Sage_Field, object>(_ => null);

        public Data_Line()
            : base() {
            dict = new Dictionary<string, object>();
        }

        public Data_Line(int capacity)
            : base() {
            dict = new Dictionary<string, object>(capacity);
        }

        public Data_Line(IDictionary<string, object> values)
            : base() {
            dict = new Dictionary<string, object>(values);
        }

        readonly Dictionary<string, object> dict;

        public Dictionary<string, object> Dict {
            get {
                return dict;
            }
        }

        public override bool Contains(string field) {
            return dict.ContainsKey(field);
        }

        public bool Contains_Nonempty(string field) {
            object val;
            if (!dict.TryGetValue(field, out val))
                return false;

            return !val.strdef().Clean().IsNullOrEmpty();
        }

        protected override T Get<T>(string field) {
            object ret;
            var def = default(T);
            var type = typeof(T);

            if (type == typeof(string))
                def = (T)(object)"";

            if (!dict.TryGetValue(field, out ret))
                dict[field] = ret = def;

            if (type == typeof(DateTime))
                return (T)(object)((DateTime)(object)ret).Date;
            else if (type.IsEnum)
                return (T)(object)Enum.Parse(type, ret.ToString(), true);
            else
                return (T)(object)Convert.ChangeType(ret, type);

        }

        protected override void Set<T>(string field, T value) {

            if (value == null) {
                if (typeof(T) == typeof(string))
                    value = (T)(object)"";
                else
                    value = default(T);
            }

            if (typeof(T) == typeof(DateTime))
                dict[field] = ((DateTime)(object)value).Date;
            else
                dict[field] = value;
        }


    }
}