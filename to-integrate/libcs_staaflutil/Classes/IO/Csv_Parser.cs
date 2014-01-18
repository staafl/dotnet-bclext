using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fairweather.Service
{
    /// <summary>
    /// This class allows you to read csv records one after another from a stream.
    /// It does the job of parsing the fields, figuring out where each record ends and
    /// throwing meaningful exceptions in case of formatting errors.
    /// 
    /// It does not handle headers, fields per record count restrictions, etc.
    /// </summary>
    public class Csv_Parser : IDisposable
    {
        // Csv format references
        // http://www.csvreader.com/csv_format.php
        // http://en.wikipedia.org/wiki/Comma-separated_values
        // http://tools.ietf.org/html/rfc4180
        public Csv_Parser(TextReader tr) {
            this.lc = tr as Line_Counter ?? new Line_Counter(tr);
            //this.reader = new Reader(lc);

        }

        public void Dispose() {
            lc.Dispose();
        }


        // ****************************

        /// <summary>
        /// After this call, 'next' will contain the
        /// initial character on the line.
        /// </summary>
        public void Skip_To_Next_Line() {

            while (true) {
                read_first = true;
                next = lc.Read();
                if (next == lf || next == eof)
                    return;
            }


        }

        public bool End_Of_File { get { return next == -1; } }

        public bool Last_Char_Was_Comma { get; private set; }

        const char comma = ',';
        const char cr = '\r';
        const char lf = '\n';
        const char qq = '"';
        const char hash = '#';
        const int eof = -1;
        const int max_len = 2000;

        public int Line {
            get {
                return lc.Line;
            }
        }

        public int Column {
            get {
                return lc.Col;
            }
        }

        public int Record {
            get {
                return record;
            }
        }


        // ****************************
        /*           State           */
        // ****************************

        bool read_first;
        int next;
        //bool last_was_comma;

        int record = -1;

        readonly char[] chars = new char[max_len];
        readonly Line_Counter lc;

        // ****************************

        void Runaway_Quote_Error(bool _length_exceeded, int? open_quote_line, int? open_quote_col) {

            var msg = _length_exceeded ? "Maximum field length exceeded" : "Terminating \" not found";

            // open_quote_col.tifn();

            msg += " (unmatched opening \" on line {0}, position {1}?)".spf(open_quote_line + 1, open_quote_col);
            msg += ".";

            Error(msg, Csv_Error_Type.Runaway_Quote, true, true);

        }

        void Error(Csv_Error_Type error_type, bool fatal, bool increment_record) {
            string msg = null;
            Error(msg, error_type, fatal, increment_record);
        }

        void Error(string msg, Csv_Error_Type error_type, bool fatal, bool increment_record) {
            if (increment_record)
                ++record;
            throw new XCsv(msg, error_type, fatal, lc.Line, lc.Col, Record);
        }

        // ****************************

        /// <summary>
        /// Single '\n' or cr-lf pair is the record delimiter.
        /// </summary>
        /// <returns></returns>
        public List<string>
        Get_Next_Record() {
            int line;
            int index;
            return Get_Next_Record(out line, out index);
        }

        const bool are_all_errors_fatal = false;
        /// <summary>
        /// Single '\n' or cr-lf pair is the record delimiter.
        /// return.Count == (number of delimiting commas) + 1
        /// </summary>
        /// <returns></returns>
        public List<string>
        Get_Next_Record(out int line, out int index) {

            line = -1;
            index = -1;

            if (End_Of_File)
                return null;

            var lst = new List<string>(15);

            int cnt = 0;

            var open_quote = false;
            var any_non_ws = false;
            var closing_quote_seen = false;

            // the following two variables are non-null iff the currently
            // examined field is escaped.

            // the line of the current escaped field's opening "
            int? open_quote_line = null;

            // the line position of the current escaped field's opening "
            // open_quote_col == 1 iff the " was the first character on the line
            int? open_quote_col = null;

            #region inlined lambdas
            //Action add_char = () =>
            //{
            //    try {
            //        chars[cnt++]=ch;
            //    }
            //    catch (IndexOutOfRangeException) {
            //        runaway_quote_error(true);
            //    }
            //};

            //Action<bool> add_field = force =>
            //{
            //    if (!force && pos == 0)
            //        return;

            //    var s = new string(chars, pos);

            //    //if (in_quoted)
            //    //    (s == s.Trim()).tiff();

            //    pos = 0;

            //    open_quote = false;
            //    any_non_ws = false;
            //    closing_quote_seen = false;
            //    open_quote_line = null;
            //    open_quote_col = null;

            //    lst.Add(s);
            //}; 
            #endregion


            // [ ] empty fields
            // [ ] trailing characters in the file
            // [ ] quoted fields
            // [ ] multiline records
            // [ ] unquoted fields
            // [ ] unquoted fields with leading/trailing whitespace
            // [ ] blank lines
            // [ ] commented lines

            if (!read_first) {
                read_first = true;
                next = lc.Read();
            }

            while (true) {
            NEXT_LINE:
                do {
                    if (End_Of_File)
                        return null;

                    if (next == hash)
                        Skip_To_Next_Line();
                    else
                        break;

                } while (true);

                line = lc.Line;
                var non_ws_on_line = false;

                while (true) {

                    // inline verified on 03.06.10
                    #region read_next()

                    if (next == eof)
                        return null;

                    var ch = unchecked((char)next);
                    next = lc.Read();
                    var is_last = (next == -1);

                    #endregion



                    if (ch == qq) {

                        if (!open_quote) {
                            if (closing_quote_seen) {
                                Error(Csv_Error_Type.Trailing_Quote, are_all_errors_fatal, true); // alpha,"bravo" ",
                            }
                            else if (any_non_ws) {
                                Error(Csv_Error_Type.Unexpected_Quote, are_all_errors_fatal, true); // al"pha,bravo,
                            }
                            else if (open_quote_col == null) {
                                // entering quoted field
                                // forget preceding whitespace

                                cnt = 0;
                                open_quote_col = lc.Col - 1;
                                open_quote_line = lc.Line;
                            }
                            else {
                                // skip every other quote in an escaped field
                                // this will make sure that the terminating " is ignored
                                any_non_ws = true;
                                //last_was_comma = false;

                                // inline verified on 03.06.10
                                #region add_char();
                                try {
                                    chars[cnt++] = ch;
                                }
                                catch (IndexOutOfRangeException) {
                                    Runaway_Quote_Error(true, open_quote_line, open_quote_col);
                                }
                                #endregion

                            }
                        }

                        open_quote = !open_quote;
                        non_ws_on_line = true;

                    }
                    else {

                        var is_ws = false;

                        if (ch == comma) {
                            non_ws_on_line = true;
                            if (!open_quote || closing_quote_seen) { // sic
                                // delimiter
                                // inline verified on 03.06.10
                                #region add_field(true)
                                var s = new string(chars, 0, cnt);

                                cnt = 0;

                                open_quote = false;
                                any_non_ws = false;
                                closing_quote_seen = false;
                                open_quote_line = null;
                                open_quote_col = null;

                                lst.Add(s);
                                #endregion

                                if (is_last)
                                    goto LAST; // sorry
                                continue;
                            }

                        }
                        else if (ch == cr) {
                            is_ws = true;

                            if (!open_quote && next == lf)
                                continue;
                            if (!open_quote && is_last)
                                ch = lf; // this hack is better than the alternative

                            // if it's in an escaped field or if it's alone,
                            // treat it as an ordinary character

                        }
                        else if (ch == ' ' || ch == '\t') {
                            is_ws = true;

                        }

                        if (ch == lf) {
                            is_ws = true;

                            if (!open_quote || closing_quote_seen) {

                                // inline verified on 03.06.10
                                #region add_field(false)
                                if (non_ws_on_line) {
                                    var s = new string(chars, 0, cnt);

                                    cnt = 0;

                                    open_quote = false;
                                    any_non_ws = false;
                                    closing_quote_seen = false;
                                    open_quote_line = null;
                                    open_quote_col = null;

                                    lst.Add(s);
                                }
                                #endregion

                                // inline verified on 03.06.10
                                #region return value
                                if (!non_ws_on_line) {
                                    if (is_last)
                                        return null;
                                    goto NEXT_LINE;
                                }

                                index = ++record;
                                return lst;
                                #endregion

                            }

                            // otherwise throw error in the final if:
                            // if (is_last) {

                        }

                        non_ws_on_line |= !is_ws;

                        if ((open_quote_col != null) &&
                            !open_quote &&
                            !closing_quote_seen) {

                            closing_quote_seen = true;

                        }

                        if (closing_quote_seen) {
                            if (is_ws) {
                                // ignore trailing ws
                                if (is_last)
                                    goto LAST;
                                continue;
                            }
                            else {
                                Error(Csv_Error_Type.Trailing_Chars, are_all_errors_fatal, true);// alpha,"brav" o,
                            }
                        }

                        // inline verified on 03.06.10
                        #region add_char();
                        try {
                            chars[cnt++] = ch;
                        }
                        catch (IndexOutOfRangeException) {
                            Runaway_Quote_Error(true, open_quote_line, open_quote_col);
                        }
                        #endregion

                        if (!is_ws) {
                            any_non_ws = true;
                            //last_was_comma = ch != ',';
                        }

                    }

                LAST:
                    if (is_last) {
                        // open_quote.tiff();

                        if (open_quote && ch != qq)
                            Runaway_Quote_Error(false, open_quote_line, open_quote_col);

                        // inline verified on 03.06.10
                        #region add_field(false)
                        if (non_ws_on_line) {
                            var s = new string(chars, 0, cnt);

                            cnt = 0;

                            open_quote = false;
                            any_non_ws = false;
                            closing_quote_seen = false;
                            open_quote_line = null;
                            open_quote_col = null;

                            lst.Add(s);
                        }
                        #endregion

                        // inline verified on 03.06.10
                        #region return value
                        if (!non_ws_on_line)
                            return null;

                        index = ++record;
                        return lst;
                        #endregion

                    }

                }
            }

        }

        // ****************************


    }
}
