using System;
using System.Collections.Generic;

using Fairweather.Service;

namespace Common
{
    public abstract class Records_Cursor
    {

        public abstract Records_Cursor Get_Copy();

        protected Records_Cursor() {
            Forward = true;
            First_Column = true;
        }
        public abstract bool Is_Empty {
            get;
        }

        public abstract int Count {
            get;
        }

        /// <summary>
        /// Setting this property to false transparently causes all indexing to be
        /// reversed
        /// </summary>
        public bool Forward { get; set; }
        /// <summary>
        /// Setting this property to false transparently causes most other methods
        /// to return values based on the second column of data
        /// </summary>
        public bool First_Column { get; set; }


        public abstract bool Supports_Second_Column { get; }

        public virtual bool Add_New(string key, string value) { return false; }
        public virtual bool Remove(string key) { return false; }


        /// Returns a continuous range of elements starting from the specified
        /// index and with the specified maximum length.
        /// Length of -1 signifies no upper bound.
        /// The direction of enumeration is affected by the SortedListCursor::Forward
        /// property 
        /// Forward => [startIndex, startIndex+1, ..., startIndex+length-1]
        /// Forward => [startIndex, startIndex-1, ..., startIndex-length+1]
        public abstract List<Pair<string>> GetRange(int startIndex, int length);

        /// Returns a continuous range of elements, beginning with the first element
        /// whose key or value (specified by "key") start with the specified prefix.
        /// index and with the specified maximum length. "length" denotes the maximum
        /// number of returned elements.
        /// "length" of -1 signifies no upper bound.
        /// The direction of enumeration is affected by the SortedListCursor::Forward
        /// property.
        /// It may also return elements which do not satisfy the condition (at the end of 
        /// the returned collection.
        //public abstract List<Pair<string>> GetRange(string prefix, int length, StringComparison comparison);

        public abstract List<Pair<string>> GetPureRange(string prefix, int length, StringComparison comparison);

        //public abstract bool TryGetValue(string key, out string value);

        public abstract bool ContainsKey(string key);

        public abstract Pair<string> GetAtIndex(int index);

        public abstract Pair<string>? GetKVP(string key);

        public int? GetIndex(string value, bool partial) {
            return GetIndex(value, partial, StringComparison.InvariantCultureIgnoreCase);
        }
        public abstract int? GetIndex(string prefix, bool partial, StringComparison comparison);

        public abstract List<Pair<string>> GetAtIndices(List<int> indices, bool is_presorted);

        // Prepares the cursor for a surge of activity
        public virtual IDisposable Prepare(out bool ok) {
            ok = true;
            return On_Dispose.Nothing;
        }


        // Signals to the cursor it is not going to be used often for the time being
        public virtual void End() { }

        public bool Change_Value(string key, string new_value) {

            if (!Remove(key))
                return false;

            if (!Add_New(key, new_value))
                return false;

            return true;
        }


    }
}