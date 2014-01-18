using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Fairweather.Service
{
#if WINFORMS
    using System.Windows.Forms;
#endif

    public static partial class Extensions
    {

        public static string Clean(this string str) {
            if (str.IsNullOrEmpty())
                return str;

            return str.Trim().ToUpper();

        }

        static public bool
        Emptyish(this string str) {
            return (str??"").Trim().IsNullOrEmpty();
        }

        static public string
        Quote(this string str, bool dbl) {
            return dbl ? "\"" + str + "\"" : "'" + str + "'";


        }

        static public string
        Enclose(this string str, string left, string right) {

            return left + str + right;

        }

        static public string
        Paren(this string str, char lp) {

            str = lp + str;
            switch (lp) {
                case '(':
                    str += ")";
                    break;
                case '{':
                    str += "}";
                    break;
                case '[':
                    str += "]";
                    break;
                case '<':
                    str += ">";
                    break;
                default:
                    str += lp;
                    break;
            }

            return str;

        }


        static public string
        Cat(this IEnumerable<string> seq) {
            var sb = new StringBuilder();
            foreach (var s in seq)
                sb.Append(s);
            return sb.ToString();
        }

        static public string
        Csv(this IEnumerable<string> seq) {

            var fst = true;
            var seq2 = seq.Select(_s => ((fst && !(fst = false)) ? "" : ",") + _s.Csv_Escape());
            // fst && !(fst = false)
            // (fst != (fst = false)
            //  seq.Lippert_Print("{0}", ",{0}", ",{0}");
            var ret = seq2.Cat();

            return ret;

        }
        /// <summary>
        /// This call will return a value that will represent the 'input' parameter
        /// in a csv file.
        /// Any leading or trailing whitespace is retained. 
        /// "" and null are returned as-is.
        /// The value is escaped only if it needs to be.
        /// The call is idempotent.
        /// No delimiters are added.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public string
        Csv_Escape(this string input) {

            if (input.IsNullOrEmpty())
                return "";

            var trim = input.Trim();

            if (trim.IsNullOrEmpty())
                return '"' + input + '"';

            var is_escaped = true;

            if (input[0] != '"' ||
               input[input.Length - 1] != '"') {
                is_escaped = false;
            }
            else {
                int qs = 0;
                foreach (var pair in input.Skip(1).Mark_Last()) {
                    if (pair.Second)
                        break;
                    if (pair.First == '"') {
                        ++qs;
                    }
                    else {
                        is_escaped = (qs % 2 == 1);
                        qs = 0;
                        if (!is_escaped)
                            break;

                    }
                }
            }

            if (is_escaped)
                return input;

            var needs_escaping = trim != input || trim[0] == '#';

            if (!needs_escaping) {
                foreach (var ch in input) {
                    switch (ch) {
                        case '"':
                        case ',':
                        case '\r':
                        case '\n':
                            needs_escaping = true;
                            break;
                    }
                    if (needs_escaping)
                        break;
                }
            }

            if (needs_escaping)
                return '"' + input.Replace("\"", "\"\"") + '"';

            return input;

        }

        static public int IntCompare(this string str1, string str2, int nbase) {

            if (str1.IsNullOrEmpty() || str2.IsNullOrEmpty())
                return String.Compare(str1, str2);

            int int1, int2;
            if (nbase == 10) {

                int1 = int.Parse(str1);
                int2 = int.Parse(str2);

            }
            else {

                int1 = Convert.ToInt32(str1, nbase);
                int2 = Convert.ToInt32(str2, nbase);

            }

            return int1.CompareTo(int2);

        }

        public static string Safe_Substr(this string str, int cnt) {
            if (str.Length > cnt)
                str = str.Substring(0, cnt);
            return str;
        }

        public static string
        Remove_Trailing_Comma(this string str, bool trim) {

            if (str.IsNullOrEmpty())
                return str;

            var work_on = str;
            if (trim)
                work_on = str.TrimEnd();

            if (work_on[work_on.Length - 1] != ',')
                return work_on;

            return work_on.Substring(0, work_on.Length - 1);

        }

        public static string
        Maybe_Format(this string format, params object[] args) {

            if (args == null || args.Length == 0)
                return format;

            return string.Format(format, args);

        }
        static public string
        Remove_White(this string str) {

            var ret = str.Replace(" ", "")
                         .Replace("\n", "")
                         .Replace("\r", "")
                         .Replace("\t", "");

            return ret;

        }

        static StringBuilder sb = new StringBuilder();

        // http://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net

        /// <summary>
        /// Non-threadsafe
        /// </summary>
        static public string
        Remove_Diacritics(this string stIn) {

            string stFormD = stIn.Normalize(NormalizationForm.FormD);

            sb.Length = 0;
            sb.Capacity = stFormD.Length;

            for (int ich = 0; ich < stFormD.Length; ich++) {
                var uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark) {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }


        static public string Head(this string str, int length) {

            var len = str.Length;
            length = Math.Min(len, length);

            var ret = str.Substring(0, length);

            return ret;


        }

        static public string Concat(this IEnumerable<string> strs) {

            return strs.Aggregate("", (s1, s2) => s1 + s2);

        }

        public static string
        No_Trailing_Zeroes(this decimal dec) {

            var ret = dec.ToString();
            if (ret.Contains(".")) {
                ret = ret.TrimEnd('0');
                ret = ret.TrimEnd('.');
            }

            return ret;
        }

        public static string
        At_Most_Precision(this decimal dec, int at_most) {

            var ret = dec.ToString();
            if (ret.Contains(".")) {

                var split = ret.Split('.');

                ret = split[0] + "." + split[1].Clip_Right(at_most);

            }

            return ret;

        }

        public static string
        No_Trailing_Zeroes(this double dec) {

            var ret = dec.ToString();
            if (ret.Contains(".")) {
                ret = ret.TrimEnd('0');
                ret = ret.TrimEnd('.');
            }

            return ret;
        }

        public static string
        At_Most_Precision(this double dbl, int at_most) {

            var ret = dbl.ToString();
            if (ret.Contains(".")) {

                var split = ret.Split('.');

                ret = split[0] + "." + split[1].Clip_Right(at_most);
                ret = ret.TrimEnd('.');
            }

            return ret;

        }


        static public string
        Collapse_Zeroes(this string input, bool collapse_end) {

            var ret = input;

            if (ret.StartsWith("0") && ret.Length > 1) {

                if (ret[1] == '0')
                    ret = '0' + ret.TrimStart('0');

                else if (ret[1] != '.')
                    ret = ret.Substring(1);

            }

            var contains_dot = Lazy.Make(() => ret.Contains('.'));


            if (collapse_end && ret.EndsWith("0") && contains_dot) {

                ret = ret.TrimEnd('0');
                ret = ret.TrimEnd('.');

            }

            return ret;

        }


        // ****************************


        public static bool
        IsNullOrEmpty(this string str) {

            bool ret = String.IsNullOrEmpty(str);

            return ret;
        }

        public static int
        Safe_Length(this string str) {
            if (str == null)
                return 0;
            return str.Length;
        }

        public static int
        Safe_Length(this string str, bool minus_one_for_null) {

            if (str == null)
                return minus_one_for_null ? -1 : 0;
            return str.Length;
        }


        public static bool
        Safe_Equals(this string first, string second, bool ignore_case) {

            if (first == null)
                return second == null;

            if (ignore_case)
                return first.Equals(second, StringComparison.InvariantCultureIgnoreCase);

            return first.Equals(second);
        }

        public static List<string>
        Chunk(this string text, int count) {

            var buf = new char[count];
            int len = text.Length;
            var ret = new List<string>(len / count);

            for (int ii = 0; ii < len; ++ii) {

                int jj;
                for (jj = 0; jj < count && ii < len; ++ii, ++jj) {
                    buf[ii] = text[ii];
                }

                ret.Add(new string(buf, 0, jj));

            }

            return ret;
        }

        public static string
        Repeat(this char ch, int count) {
            string ret = new String(ch, count);

            return ret;
        }

        // Benchmark?
        public static string
        Repeat(this string str, int count) {
            string ret;
            int length = str.Length;
            int total = count * length;

            if (total == 0)
                return "";

            if (count < 10) {
                var array_1 = str.ToCharArray();
                var array_2 = new char[total];

                for (int ii = 0; ii < total; ii += length)
                    array_1.CopyTo(array_2, ii);

                ret = new String(array_2);
            }
            else {
                var sb = new StringBuilder(count * str.Length);
                for (int ii = 0; ii < count; ++ii)
                    sb.Append(str);

                ret = sb.ToString();
            }

            return ret;
        }

        public static int
        Compare_To(this string s1, string s2, StringComparison cmp) {

            return String.Compare(s1, s2, cmp);


        }

        public static int
        Int_Compare(this string s1, string s2) {

            int len1 = s1.Safe_Length();
            int len2 = s1.Safe_Length();

            int ret;
            if (len1 != len2)
                ret = len1.CompareTo(len2);
            else
                ret = s1.Compare_To(s2, StringComparison.Ordinal);

            return ret;

        }


        public static string
        spf(this string format, params object[] args) {

            string ret = String.Format(format, args);

            return ret;
        }

        /// <summary>
        /// Combines the two paths without using System.IO.Path.Combine
        /// Specs: If the path1 is empty -> path2
        ///        If path2 is empty -> path1
        ///        If path2.TrimStart('\\') is a global path -> path2
        ///        Otherwise, combine path1 and path2 making sure that
        ///        the resulting value has exactly one directory delimiter
        ///        at the point of junction
        /// </summary>
        public static string
        CpathEx(this string path1, string path2) {

            var ret = path1;

            if (path1.IsNullOrEmpty()) {

                ret = path2;

            }
            else if (path2.IsNullOrEmpty()) {

                ret = path1;

            }
            else if (Path.IsPathRooted(path2.TrimStart('\\'))) {

                ret = path2;
            }
            else {
                bool flag1 = path1.EndsWith("\\");

                if (flag1 == path2.StartsWith("\\")) {
                    ret = flag1 ? path1 + path2.Substring(1)
                                : path1 + "\\" + path2;
                }
                else {
                    ret = path1 + path2;
                }

            }


            return ret;

        }

        /// <summary>
        /// Combines the sequence of paths using System.IO.Path.Combine
        /// </summary>
        public static string
        Cpath(this string leading, params string[] trailing) {

            string ret = leading;

            if (ret == null)
                ret = "";

            ret = ret.TrimStart('"').TrimEnd('"');

            for (int ii = 0; ii < trailing.Length; ++ii) {

                string temp = trailing[ii].TrimStart('"').TrimEnd('"');

                try {
                    ret = System.IO.Path.Combine(ret, temp ?? "");
                }
                catch (ArgumentException) {
                    return "";
                }
            }

            return ret;

        }


        // ****************************

        public static string
        StringOrDefault<TParam>(this TParam obj) {

            var temp = default(TParam);
            string def;

            if (temp == null)
                def = "";
            else
                def = temp.ToString();

            return StringOrDefault<TParam>(obj, def);
        }

        public static string
        StringOrDefault<TParam>(this TParam obj, string def) {

            string ret = def;

            if (obj != null)
                ret = obj.ToString();

            return ret;
        }

        public static string
        StringOrDefault<TParam>(this TParam obj,
                                     string format,
                                     string def)
                                 where TParam : IFormattable {

            string ret = def;
            IFormattable form = (IFormattable)(obj);

            if (form != null)
                ret = form.ToString(format, System.Globalization.NumberFormatInfo.CurrentInfo);

            return ret;
        }



        // ****************************

#if WINFORMS

        public static CharacterCasing
        Get_Casing(this string str) {

            if (str == "")
                return CharacterCasing.Normal;

            if (str == str.ToUpper())
                return CharacterCasing.Upper;

            else if (str == str.ToLower())
                return CharacterCasing.Lower;


            else
                return CharacterCasing.Normal;

        }

        public static string
        To_Casing(this string str, CharacterCasing casing) {

            if (casing == CharacterCasing.Upper)
                return str.ToUpper();

            else if (casing == CharacterCasing.Lower)
                return str.ToLower();

            else
                return str;

        }
#endif

        /// <summary>
        /// All -> uppercase first letter and all letters after whitespace or punctuation \
        /// Otherwise -> uppercase only first letter
        /// </summary>
        /// <param name="input"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        public static string
        ToProper(this string input, bool all) {

            if (String.IsNullOrEmpty(input))
                return input;

            if (all) {

                char[] chs = input.ToCharArray();

                for (int ii = 0; ii < chs.Length; ++ii) {
                    bool upper = ii == 0;

                    if (!upper) {
                        char prior = chs[ii - 1];
                        upper = Char.IsWhiteSpace(prior) ||
                                Char.IsPunctuation(prior);
                    }

                    if (upper)
                        chs[ii] = Char.ToUpper(chs[ii]);
                    else
                        chs[ii] = Char.ToLower(chs[ii]);

                }

                return new string(chs);
            }
            else {
                string ret = "" + Char.ToUpper(input[0]);

                if (input.Length > 1)
                    ret += input.Substring(1).ToLower();

                return ret;
            }

        }

        public static string
        ToProper(this string input) {

            return ToProper(input, false);

        }

        public static string
        ToProperInvariant(this string input) {

            if (String.IsNullOrEmpty(input))
                return "";

            string ret = "" + Char.ToUpperInvariant(input[0]);

            if (input.Length > 1)
                ret += input.Substring(1).ToLowerInvariant();

            return ret;
        }


        // ****************************

        public static bool
        Eat(this string input, string to_eat, bool case_insensitive, out string output) {

            output = input;

            bool ret;

            var options = case_insensitive ? StringComparison.InvariantCulture :
                                             StringComparison.InvariantCultureIgnoreCase;

            ret = output.StartsWith(to_eat, options);

            if (ret)
                output = output.Substring(to_eat.Length);

            return ret;

        }

        public static bool
        Eat(this string input, string to_eat, out string output) {

            return input.Eat(to_eat, false, out output);

        }

        public static bool
        Eat_Until(this string input, string until, out string output) {

            return input.Eat_Until(input, false, out output);

        }

        public static bool
        Eat_Until(this string input, string until, bool case_insensitive, out string output) {

            output = input;

            var options = case_insensitive ? StringComparison.InvariantCulture :
                                   StringComparison.InvariantCultureIgnoreCase;

            int index = input.IndexOf(until, options);

            bool ret = index != -1;

            if (ret)
                output = input.Substring(0, index);

            return ret;

        }



        // ****************************


        /*       Untested        */

        public static string
        Transform_Tabs(this string text, int tab_width) {

            (tab_width < 4).tift();

            int cnt_tabs = text.Count(ch => ch == '\t');

            char[] array = new char[text.Length + cnt_tabs * (tab_width - 1)];

            int pos = 0;
            for (int ii = 0; ii < text.Length;
                ++ii, ++pos) {

                var ch = text[ii];

                if (ch != '\t') {
                    array[pos] = ch;
                    continue;
                }

                int num_spaces = tab_width - pos % tab_width;

                for (int jj = 0; jj < num_spaces; ++jj, ++pos) {

                    array[pos] = ' ';

                }
                --pos;

            }


            string ret = new String(array, 0, pos);

            return ret;
        }

        public static string
        Transform_Tabs(this string text) {

            var ret = text.Transform_Tabs(8);

            return ret;

        }



        // ****************************


        public static string
        Trim_Pad(this string input,
                      char pad_with,
                      int desired_length,
                      bool pad_left) {

            input.tifn();

            int len = input.Length;
            string ret = input;

            if (len < desired_length) {
                ret = pad_left ? input.PadLeft(desired_length, pad_with) :
                                 input.PadRight(desired_length, pad_with);
            }
            else if (len != desired_length) {
                ret = input.Clip_Right(desired_length);
            }

            return ret;

        }

        public static string
        Trim_Left(this string str, int count) {

            int len = str.Length;

            count = Math.Min(count, len);

            var ret = str.Substring(count, len - count);

            return ret;
        }

        /// <summary> 
        /// Throws if count .lt. 0
        /// Does not throw if count .gt. len
        /// </summary>
        public static string
        Trim_Right(this string str, int count) {

            int len = str.Length;

            count = Math.Min(count, len);

            var ret = str.Substring(0, len - count);

            return ret;
        }

        /// <summary> 
        /// Throws if count .lt. 0
        /// Does not throw if count .gt. len
        /// </summary>
        public static string
        Trim_Right(this string str, int count, params char[] chars) {

            int len = str.Length;

            count = Math.Min(count, len);

            (count <= len && 0 <= count).tiff();

            int to_remove = 0;

            int ii = len - 1;
            while (count > 0 && chars.Contains(str[ii])) {
                --count;
                --ii;
                ++to_remove;
            }

            string ret = str.Trim_Right(to_remove);

            return ret;
        }


        public static string
        Clip_Left(this string str, int target_length) {
            int len = str.Length;
            if (len > target_length) {
                str = str.Substring(target_length);
            }
            return str;

        }

        /// <summary> Does not throw if the target length is greater than the current length.
        /// </summary>
        public static string
        Clip_Right(this string str, int target_length) {

            int len = str.Length;

            (target_length < 0).tift();

            if (len < target_length)
                return str;

            string ret = str.Substring(0, target_length);

            return ret;
        }

        /// <summary> Does not throw if the target length is greater than the current length.
        /// </summary>
        public static string
        Clip_Right(this string str, int target_length, params char[] chars) {

            int len = str.Length;

            (target_length < 0).tift();

            if (len < target_length)
                return str;

            int max_remove = len - target_length;
            int to_remove = 0;
            int ii = str.Length - 1;

            while (max_remove > 0 && chars.Contains(str[ii])) {

                --max_remove;
                --ii;
                ++to_remove;

            }

            string ret = str.Trim_Right(to_remove);

            return ret;
        }




        // ****************************



        /// <summary> Replaces all characters in a substring with the specified character. 
        /// The range includes both the start and endpoint.
        /// </summary>
        public static string
        Replace_Chars(this string target, int start, int end, char ch) {

            char[] chars = target.ToCharArray();
            for (int ii = start; ii <= end; ++ii)
                chars[ii] = ch;

            return new String(chars);
        }

        /// <summary> Replaces all characters in a substring with the specified character keeping the specified characters.
        /// The range includes both the start and endpoint.
        /// </summary>
        public static string
        Replace_Chars(this string target,
                           int start,
                           int end,
                           char ch,
                           params char[] keep) {

            char[] chars = target.ToCharArray();

            for (int ii = start; ii <= end; ++ii)
                if (!keep.Contains(chars[ii]))
                    chars[ii] = ch;

            return new String(chars);
        }

        public static string
        Replace_Substring(this string input, string replacement, int start) {

            return Replace_Substring(input, replacement, start, start + replacement.Safe_Length() - 1);

        }

        /// <summary> Replaces all characters in a substring with the specified string
        /// The range includes both the start and endpoint.
        /// </summary>
        public static string
        Replace_Substring(this string input, string replacement, int start, int end) {

            int total = input.Length;
            string left, right;

            left = start == 0 ? "" : input.Remove(start);

            if (end == start - 1)
                right = input.Substring(start);
            else
                right = total <= end ? "" : input.Substring(end + 1);

            string ret = left + replacement + right;

            return ret;
        }




        /// <summary> Returns the concatenation of all the strings with optional "to_insert" inserted between each pair 
        /// (no leading or trailing strings are added)
        /// </summary>
        public static string
        Unlines(this IEnumerable<string> enumerable, bool insert, string to_insert) {

            string ret;

            int cnt = enumerable.Count();

            if (cnt == 0) {
                ret = "";
            }
            else if (cnt < 10) {

                var enumerable1 = enumerable.Force();
                ret = enumerable1[0];

                if (insert) {

                    for (int ii = 1; ii < cnt; ++ii)
                        ret += to_insert + enumerable1[ii];
                }
                else {
                    for (int ii = 1; ii < cnt; ++ii)
                        ret += enumerable1[ii];
                }
            }
            else {
                bool first = true;
                StringBuilder sb = null;

                foreach (string elem in enumerable) {

                    if (first) {

                        sb = new StringBuilder(cnt * elem.Length);
                        sb.Append(elem);
                        first = false;
                        continue;
                    }

                    if (insert)
                        sb.Append(to_insert);

                    sb.Append(elem);
                }
                ret = sb.ToString();
            }

            return ret;
        }

        /// <summary> Returns the concatenation of all the strings with optional Environment.NewLine inserted 
        /// between each pair (no leading or trailing newlines are added)
        /// </summary>
        public static string
        Unlines(this IEnumerable<string> enumerable, bool newlines) {

            string ret;

            ret = enumerable.Unlines(newlines, "\n");

            return ret;
        }

        /// <summary> Ignores CR (\r, 0x0D)
        /// </summary>
        public static List<string>
        Lines(this string str,
                   bool keep_empty_lines,
                   bool keep_leading_empty,
                   bool keep_trailing_empty) {

            var seq = str.Where(ch => ch != '\r').Force();

            if (seq.Count() == 0)
                return new List<string>();

            List<string> ret = seq.Split(keep_empty_lines, '\n')
                                         .Select(chars => chars.str())
                                         .ToList();

            if (keep_empty_lines && keep_leading_empty && seq.First() == '\n')
                ret.Insert(0, "");

            if (keep_empty_lines && keep_trailing_empty && seq.Last() == '\n')
                ret.Add("");

            return ret;

        }

        /// <summary>
        /// Not CR Safe
        public static int
        Line_Count(this string str, bool count_empties, bool count_trailing) {

            if (str.IsNullOrEmpty())
                return 0;

            int ret = str.Count(c => c == '\n');
            ++ret;

            if (!count_trailing && str.EndsWith("\n"))
                --ret;

            if (!count_empties) {

                var empty_lines = str.Break((c1, c2) => (c1 == '\n') != (c2 == '\n'));
                ret -= empty_lines.Count();

            }

            return ret;
        }




        public static string
        Remove_Substring(this string str, int position, int length) {

            string left = position == 0 ? "" : str.Substring(0, position - 1);
            string right = str.Substring(position + length);

            string ret = left + right;

            return ret;
        }

        public static string
        Remove_Substring(this string input, string substr) {

            return input.Remove_Substring(substr, -1);

        }

        public static string
        Remove_Substring(this string input, string substr, int max_replaces) {

            bool all = max_replaces == -1;
            (all || max_replaces >= 0).tiff();
            string temp = input;
            int cnt = substr.Length;

            if (cnt == 0)
                return input;

            while (all || --max_replaces >= 0) {

                int index = temp.IndexOf(substr);

                if (index == -1)
                    break;

                temp = temp.Substring(0, index) + temp.Substring(index + cnt);
            }

            return temp;
        }



        public static string
        Ellipsis(this string input, int max_length) {

            return input.Ellipsis(max_length, "...", false);
        }

        public static string
        Ellipsis(this string input, int max_length, bool left) {

            return Ellipsis(input, max_length, "...", left);

        }

        public static string
        Ellipsis(this string str, int chars, string ellipsis_string, bool left) {

            str = str ?? "";

            Func<int> len = () => str.Length;

            if (len() <= chars)
                return str;

            str = str.Trim();

            if (len() <= chars)
                return str;

            var ell_len = ellipsis_string.Length;

            if (str.Length <= 2 * ell_len) {
                return str;
            }

            var excess = len() + ell_len - chars;

            if (left) {
                str = str.Substring(excess);
                str = "..." + str;
            }
            else {
                str = str.Trim_Right(excess);
                str = str + "...";
            }

            return str;

        }


        // ****************************


        public static Regex
        To_Regex(this string regex, RegexOptions options) {

            var ret = new Regex(regex, options);

            return ret;
        }

        public static Regex
        To_Regex(this string regex) {

            var ret = new Regex(regex);

            return ret;
        }

        public static Match
        Match(this string text, string regex) {

            var ret = text.Match(regex, RegexOptions.None);

            return ret;

        }

        public static Match
        Match(this string text, Regex regex) {

            var ret = regex.Match(text);

            return ret;

        }

        public static Match
        Match(this string text, string regex, RegexOptions options) {

            var rx = new Regex(regex, options);

            var ret = rx.Match(text);

            return ret;
        }








    }
}

