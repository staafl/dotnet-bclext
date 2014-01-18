using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace Fairweather.Service
{
    //Legacy
    //Represents a sequence of boolean values
    [DebuggerStepThrough]
    public class Sequence
    {
        readonly StringBuilder str_b;

        public Sequence() : this(20) { }
        public Sequence(int initial_capacity) {

            str_b = new StringBuilder(initial_capacity);
        }

        Sequence(StringBuilder sb) {

            this.str_b = new StringBuilder(sb.ToString());
        }

        Sequence(string str) {

            this.str_b = new StringBuilder(str);
        }



        // ****************************


        ///  Inserts an element at the specified location.
        ///  All elements to the right of the new element 
        ///  are shifted one position to the right, i.e. are
        ///  incremented
        public void Insert(int element) {

            Insert_Range(element, 1);
        }

        ///  Inserts count elements starting at the specified 
        ///  location. All elements to the right of the inserted
        ///  range are shifted to the right
        public void Insert_Range(int first_element, int count) {

            (first_element < 0).tift();

            (count < 1).tift();

            lock (str_b) {

                int last_elem = first_element + count - 1;

                int trailing1 = first_element - str_b.Length;

                if (trailing1 > 0)
                    str_b.Append('0', trailing1);

                // This is correct
                int trailing = last_elem - str_b.Length;

                if (trailing > 0) {

                    str_b.Append('1', trailing);
                    count -= trailing;
                }

                str_b.Insert(first_element, "1", count);
                // is_fresh = false;
            }
        }

        /// Erases the specified element of values. All values to the 
        /// right of the deleted element are shifted left, i.e. decremented.
        public void Remove(int element) {

            (element < 0).tift("Only non-negative values allowed.");

            lock (str_b) {

                if (element >= str_b.Length)
                    return;

                str_b.Remove(element, 1);

                // is_fresh = false;
            }
        }

        /// Erases the specified range of values. All values to the 
        /// right of the deleted range are shifted left.
        public void Remove_Range(int start, int count) {

            (start < 0).tift();
            (count < 1).tift();

            lock (str_b) {

                // This is correct
                count = Math.Min(str_b.Length - start, count);

                str_b.Remove(start, count);

                // is_fresh = false;
            }
        }

        // ****************************

        /// Either adds or removes the selected element
        /// from the sequence without shifting any of the
        /// other elements.
        public void Set(int element, bool present) {

            (element < 0).tift("Only non-negative values allowed.");

            char ch = present ? '1' : '0';
            lock (str_b) {

                // This is correct
                int trail = element - str_b.Length;

                // Element is beyond the end of the string
                if (trail >= 0) {

                    if (!present)
                        return;

                    str_b.Append('0', trail);
                    str_b.Append('1');
                    // is_fresh = false;

                    return;
                }

                if (str_b[element] == ch)
                    return;

                str_b[element] = ch;

                // is_fresh = false;
            }
        }

        public void Set_Range(int first_element, int count, bool value) {

            (count < 1).tift();
            (first_element < 0).tift();

            char ch = value ? '1' : '0';

            int last_elem = first_element + count - 1;

            lock (str_b) {

                // This is correct
                // [] [] [] ... ... ... []
                int trailing = last_elem - str_b.Length + 1;

                if (trailing > 0) {

                    if (value)
                        str_b.Append(ch, trailing);

                    last_elem -= trailing;
                }

                for (int ii = first_element; ii <= last_elem; ++ii)
                    str_b[ii] = ch;

                // is_fresh = false;
            }
        }

        // ****************************


        public void Clear() {
            lock (str_b) {

                str_b.Length = 0;
                // is_fresh = false;
            }
        }

        public Sequence Invert() {

            var sb = new StringBuilder(this.str_b.ToString().TrimEnd('0'));
            var new_sb = sb.Replace('0', 'x')
                           .Replace('1', '0')
                           .Replace('x', '1');

            var ret = new Sequence(new_sb);

            return ret;
        }

        // ****************************


        public bool Contains(int element) {

            (element < 0).tift("Only non-negative values allowed.");

            bool ret;

            lock (str_b) {

                ret = element < str_b.Length &&
                           str_b[element] == '1';
            }

            return ret;
        }

        public IEnumerable<int> Elements() {

            IEnumerable<int> result;
            lock (str_b) {

                //if (!is_fresh) {

                result = str_b.ToString()
                             .Select((c, i) => (c == '1' ? i : -1))
                             .Where(i => i != -1);

                // is_fresh = true;
                //}
            }

            var ret = result;

            return ret;
        }

        public int Count() {

            int ret = str_b.ToString()
                           .Count(ch => ch == '1');

            return ret;
        }

        public override string ToString() {

            return str_b.ToString();
        }

        static bool Are_Same_Sequence(Sequence seq1, Sequence seq2) {

            string str1 = seq1.ToString().TrimEnd('0');
            string str2 = seq2.ToString().TrimEnd('0');

            bool ret = str1.Length == str2.Length && str1 == str2;

            return ret;
        }
    }
}