using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Fairweather.Service;

namespace Fairweather.Service
{

    [DebuggerStepThrough]
    public struct Pos_Columns_Info
    {
        readonly char m_padding_char;

        readonly int m_left_offset;

        readonly bool m_last_column_right_aligned;

        readonly int[] m_column_intervals;

        readonly int[] m_column_widths;

        readonly bool[] m_truncatable;

        int Columns_Count {
            get {
                return m_column_widths.Length;
            }
        }

        public bool[] Truncatable {
            get {
                return m_truncatable;
            }
        }



        public char Padding_Char {
            get { return m_padding_char; }
        }

        public int Left_Offset {
            get {
                return this.m_left_offset;
            }
        }

        public bool Last_Column_Right_Aligned {
            get {
                return this.m_last_column_right_aligned;
            }
        }

        public IEnumerable<int> Column_Intervals {
            get {
                return this.m_column_intervals.Copy();
            }
        }

        public IEnumerable<int> Column_Widths {
            get {
                return this.m_column_widths.Copy();
            }
        }

        public IEnumerable<Pair<int, int>> Widths_And_Intervals {
            get {
                return m_column_widths.Combine(m_column_intervals, true);
            }
        }


        public int Count {
            get {
                return m_column_intervals.Length;
            }
        }

        static public Pos_Columns_Info From_Weighted(Pos_Columns_Info col_info, int line_length) {
            var as_double = (from elem in col_info.m_column_widths
                             select (double)elem).arr();

            return From_Weighted(col_info.m_left_offset, col_info.m_last_column_right_aligned, col_info.m_column_intervals,
                as_double, line_length);
        }

        static public Pos_Columns_Info From_Weighted(int left_offset, bool last_column_to_the_right,
            int[] column_intervals, double[] column_widths, int line_length) {

            var available = line_length - left_offset - column_intervals.Sum();

            var scaled = column_widths.Scale_To_One().Select(dbl => (int)(available * dbl)).arr();

            var remaining = available - scaled.Sum();

            int ii = 0;
            while (remaining > 0) {

                if (last_column_to_the_right) {
                    column_intervals.Index(0, false, jj => jj + 1);
                }
                else {

                    ++scaled[ii];
                    ++ii;

                }

                --remaining;


            }


            return new Pos_Columns_Info(left_offset, last_column_to_the_right, column_intervals, scaled);

        }

        static Func<Pair<int, bool>, bool> Test(bool last_column_right_aligned) {

            return pair => pair.First > 0 || (last_column_right_aligned && pair.Second);

        }
        public Pos_Columns_Info(int left_offset,
                    bool last_column_right_aligned,
                    int[] column_intervals,
                    int[] column_widths)
            : this(left_offset, last_column_right_aligned, ' ', column_intervals, column_widths) { }

        public Pos_Columns_Info(int left_offset,
                                bool last_column_right_aligned,
                                char padding_char,
                                int[] column_intervals,
                                int[] column_widths)
            : this() {

            (column_intervals.Length == column_widths.Length).tiff();

            var test = Test(last_column_right_aligned);

            // Valid sequence?
            (column_intervals.Mark_Last().All(test)).tiff();

            this.m_left_offset = left_offset;
            this.m_last_column_right_aligned = last_column_right_aligned;
            this.m_column_intervals = column_intervals;
            this.m_column_widths = column_widths;
            this.m_padding_char = padding_char;

            m_truncatable = new bool[Columns_Count];
        }

        #region Pos_Columns_Info

        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "left_offset = " + this.m_left_offset;
            ret += ", ";
            ret += "last_column_right_aligned = " + this.m_last_column_right_aligned;
            ret += ", ";
            ret += "column_intervals = " + this.m_column_intervals;
            ret += ", ";
            ret += "column_widths = " + this.m_column_widths;
            ret += ", ";
            ret += "padding_char = " + this.m_padding_char;

            ret = "{Pos_Columns_Info: " + ret + "}";
            return ret;

        }

        public bool Equals(Pos_Columns_Info obj2) {

#pragma warning disable
            if (this.m_left_offset == null) {
                if (obj2.m_left_offset != null)
                    return false;
            }
            else {
                if (!this.m_left_offset.Equals(obj2.m_left_offset))
                    return false;
            }


            if (this.m_last_column_right_aligned == null) {
                if (obj2.m_last_column_right_aligned != null)
                    return false;
            }
            else {
                if (!this.m_last_column_right_aligned.Equals(obj2.m_last_column_right_aligned))
                    return false;
            }


            if (this.m_column_intervals == null) {
                if (obj2.m_column_intervals != null)
                    return false;
            }
            else {
                if (!this.m_column_intervals.Equals(obj2.m_column_intervals))
                    return false;
            }


            if (this.m_column_widths == null) {
                if (obj2.m_column_widths != null)
                    return false;
            }
            else {
                if (!this.m_column_widths.Equals(obj2.m_column_widths))
                    return false;
            }

            if (!this.m_padding_char.Equals(obj2.m_padding_char))
                return false;


#pragma warning restore
            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Pos_Columns_Info);

            if (ret)
                ret = this.Equals((Pos_Columns_Info)obj2);


            return ret;

        }

        public static bool operator ==(Pos_Columns_Info left, Pos_Columns_Info right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Pos_Columns_Info left, Pos_Columns_Info right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

#pragma warning disable
            unchecked {
                int ret = 23;
                int temp;

                if (this.m_left_offset != null) {
                    ret *= 31;
                    temp = this.m_left_offset.GetHashCode();
                    ret += temp;

                }

                if (this.m_last_column_right_aligned != null) {
                    ret *= 31;
                    temp = this.m_last_column_right_aligned.GetHashCode();
                    ret += temp;

                }

                if (this.m_column_intervals != null) {
                    ret *= 31;
                    temp = this.m_column_intervals.GetHashCode();
                    ret += temp;

                }

                if (this.m_column_widths != null) {
                    ret *= 31;
                    temp = this.m_column_widths.GetHashCode();
                    ret += temp;

                }

                ret *= 31;
                ret += m_padding_char.GetHashCode();

                return ret;
            }
#pragma warning restore
        }

        #endregion
    }


}
