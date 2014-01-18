using System;
using System.Collections.Generic;
using Fairweather.Service;

namespace Common
{
    public abstract class Any_List_Cursor : Records_Cursor
    {
        protected Any_List_Cursor() : base() { }

        public override bool Is_Empty {
            get {
                return (Count == 0);
            }
        }


        protected int GetIndex(bool forward, int index) {

            return GetIndex(forward, index, 0);

        }

        protected int GetIndex(bool forward, int index, int offset) {

            return forward ? index + offset :
                             Count - 1 - index - offset;

        }

        protected bool IsValidIndex(int index) {

            if (index < 0)
                return false;

            if (index >= Count)
                return false;

            return true;

        }

        protected string GetValue(bool first_col, int real_index) {

            var pair = GetPair(first_col, real_index);

            var ret = first_col ? pair.First : pair.Second;

            return ret;

        }

        public override Pair<string> GetAtIndex(int index) {

            bool forward = Forward;
            bool first_col = First_Column;

            var real_index = GetIndex(forward, index);

            IsValidIndex(real_index).tiff();

            var ret = GetPair(first_col, real_index);

            return ret;

        }

        public override List<Pair<string>> GetAtIndices(List<int> indices, bool is_presorted) {

            bool forward = Forward;
            bool first_col = First_Column;

            var ret = new List<Pair<string>>(indices.Count);

            foreach (var index in indices) {

                var real_index = GetIndex(forward, index);
                var pair = GetPair(first_col, real_index);
                ret.Add(pair);

            }

            return ret;

        }


        protected abstract Pair<string> GetPair(bool first_col, int real_index);



        /// Returns a continuous range of elements starting from the specified
        /// index and with the specified maximum length.
        /// Length of -1 signifies no upper bound.
        /// The direction of enumeration is affected by the SortedListCursor::Forward
        /// property 
        /// Forward => [startIndex, startIndex+1, ..., startIndex+length-1]
        /// Forward => [startIndex, startIndex-1, ..., startIndex-length+1]
        public override List<Pair<string>>
        GetRange(int startIndex, int length) {

            bool forward = this.Forward;
            bool first_col = this.First_Column;

            if (length == -1)
                length = Count - startIndex;

            IsValidIndex(startIndex).tiff();
            (length >= 0).tiff();

            var ret = new List<Pair<string>>();

            for (int ii = 0; ii < length; ++ii) {

                int index = GetIndex(forward, startIndex, ii);

                if (!IsValidIndex(index))
                    break;

                var pair = GetPair(first_col, index);

                ret.Add(pair);

            }

            return ret;
        }











    }
}
