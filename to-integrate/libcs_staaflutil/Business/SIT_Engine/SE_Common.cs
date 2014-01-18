using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Common;
using Common.Posting;
using Common.Sage;
using Fairweather.Service;
using Line = Fairweather.Service.Pair<Common.Posting.Data_Line, Sage_Int.Line_Data>;

namespace Sage_Int
{
    partial class SIT_Engine
    {

        void
        Tell(bool scan, Record_Type mode, object ix, Line_Data line) {
            sit_exe.Tell(scan, mode, ix, line);


        }


        string
        Get_Errors_Path(Company_Number number) {

            var dir = Data.STR_Sit_History_Dir.Path.Cpath(number.As_String);
            var ret = dir + "\\Errors " + Get_Timestamp() + ".log";
            return ret;

        }

        string
        Get_Success_Path(Company_Number number) {

            var dir = Data.STR_Sit_History_Dir.Path.Cpath(number.As_String);
            var ret = dir + "\\Success " + Get_Timestamp() + ".csv";
            return ret;

        }

        string
        Get_Timestamp() {
            return "({0:yyyy-MM-dd} {0:HH.mm.ss})".spf(now);
        }


        // ****************************



        IEnumerable<Line>
       Get_Data_Lines_Iterator(
           Sit_Company_Settings sett_company,
           string file,
           Record_Type mode,
           string[] headers,
           Field_Count_Validator fcv) {

            using (var sr = new StreamReader(file))
            using (var csv = Get_Csv_Reader(sr, true)) {

                var s = sr.BaseStream;
                var len = s.Length;

                var has_headers = sett_company.Has_Headers == true;
                if (has_headers) {

                    var rec = csv.Get_Next_Record();
                    if (rec == null)
                        yield break;

                }

                // ******************************
                /*       Error handling        */
                // ******************************

                // now inlined
                Func<Csv_Error, bool, Line_Data>
                get_line_data = (_error, _read_through) =>
                {
                    // 24edb6b0-be82-4489-b2bb-8b5cb563abc8

                    var _line_no = csv.Line - (sr.EndOfStream ? 0 : _read_through ? 1 : 0);
                    var _rec_no = csv.Record - (has_headers ? 1 : 0);
                    var _perc = (int)((s.Position * 100) / len);

                    return new Line_Data(
                         _line_no,
                         _rec_no,
                         _perc,
                         _error);
                };


                List<string> record;
                while (true) {
                    XCsv csv_ex = null;
                    try {
                        record = csv.Get_Next_Record();
                    }
                    catch (XCsv ex) {
                        csv_ex = ex;
                        record = null;
                    }

                    if (csv_ex != null) {

                        var error = new Csv_Error(csv_ex);
                        // 24edb6b0-be82-4489-b2bb-8b5cb563abc8
                        var line_data = get_line_data(error, false);
                        //                           // inline verified on 03.06.10
                        //                       #region get_line_data(error)
                        //new Line_Data(csv.Line,
                        //              csv.Record - (has_headers ? 1 : 0),
                        //              (int)((s.Position * 100) / len),
                        //              error);
                        //                       #endregion

                        yield return Pair.Make((Data_Line)null, line_data);

                        if (!csv_ex.Fatal) {
                            csv.Skip_To_Next_Line();
                            continue;
                        }

                    }

                    if (record == null)
                        yield break;

                    var cnt = record.Count;
                    var end_on_comma = csv.Last_Char_Was_Comma;

                    string desc;

                    if (fcv.Check(cnt, end_on_comma, out desc)) {
                        var data_line = new Data_Line(record.Count);

                        for (int ii = 0; ii < fcv.Min && ii < headers.Length; ++ii)
                            data_line[headers[ii]] = record[ii];

                        var line_data = get_line_data(null, true);
                        //// inline verified on 03.06.10
                        //#region get_line_data(null)
                        //new Line_Data(csv.Line,
                        //              csv.Record - (has_headers ? 1 : 0),
                        //              (int)((s.Position * 100) / len),
                        //              null);
                        //#endregion

                        yield return Pair.Make(data_line, line_data);

                    }

                    else {
                        var error = new Csv_Error(Csv_Error_Type.Invalid_Field_Count, desc, csv.Column, false, true);
                        // 24edb6b0-be82-4489-b2bb-8b5cb563abc8
                        var line_data = get_line_data(error, true);
                        //                       // inline verified on 03.06.10
                        //                       #region get_line_data(error)
                        //new Line_Data(csv.Line,
                        //              csv.Record - (has_headers ? 1 : 0),
                        //              (int)((s.Position * 100) / len),
                        //              error);
                        //                       #endregion

                        yield return Pair.Make((Data_Line)null, line_data);


                    }

                }


            }
        }



        IEnumerable<Line>
       Get_Data_Lines(Sit_Company_Settings sett_company,
                      string file,
                      Record_Type mode,
                      out string[] headers) {

            headers = null;

            var fcv = new Field_Count_Validator();

            if (sett_company.Has_Headers == true) {

                using (var sr = new StreamReader(file))
                using (var csv = Get_Csv_Reader(sr, false)) {

                    var ok = false;
                    try {
                        var lst = csv.Get_Next_Record();

                        if (lst != null) {

                            if (lst.Count > 0 && lst.Last().Trim().IsNullOrEmpty()) {
                                // let's pretend the trailing comma wasn't there
                                lst.RemoveAt(lst.Count - 1);
                            }

                            // trailing commas have no influence when we're reading the headers
                            // from the file
                            const bool last_is_comma = false;

                            fcv.Check(lst.Count, last_is_comma);



                            if (lst.Count == 0)
                                lst = null;
                        }


                        headers = lst == null ? null : lst.arr();

                        ok = (headers != null);

                    }
                    catch (XCsv) {

                    }

                    if (!ok) {
                        headers = null;
                        return new Line[0];
                    }
                }
            }
            else {

                headers = Get_Static_Headers(mode);

            }

            if (headers != null)
                headers = headers.Select(_h => _h.ToUpper()).arr();

            var seq = Get_Data_Lines_Iterator(sett_company, file, mode, headers, fcv);

            //*/
            return seq;
            /*/
            return new Cached_Enumerable<Line>(seq);
            //*/

        }

        Csv_Parser
       Get_Csv_Reader(TextReader base_reader, bool small_buffer) {

            // http://en.wikipedia.org/wiki/Comma-separated_values#Basic_rules
            /*
                In some CSV implementations, leading and trailing spaces
                or tabs, adjacent to commas, are trimmed. This practice
                is contentious and in fact is specifically prohibited by
                RFC 4180, which states, "Spaces are considered part of a
                field and should not be ignored."

             */

            //const ValueTrimmingOptions trimming = ValueTrimmingOptions.None;

            var ret = new Csv_Parser(base_reader);

            // new CsvReader(base_reader, false, ',', '"', '"', '#', trimming, small_buffer ? 0x3 : 0x1000);
            // ret.SkipEmptyLines = true;
            // ret.SupportsMultiline = true;

            // ret.MissingFieldAction = false;
            // ret.DefaultParseErrorAction = ;

            return ret;
        }

        // string[]
        //Get_Record(CsvReader csv) {
        //    string[] ret;
        //    csv.CopyCurrentRecordTo(ret = new string[csv.FieldCount]);
        //    return ret;
        //}

        //Prepare the  headers
        string[]
        Get_Static_Headers(Record_Type mode) {

            switch (mode) {
                case Record_Type.Sales:
                case Record_Type.Purchase:
                case Record_Type.Bank:
                    return SIT_Engine.record_static;

                case Record_Type.Expense:
                    return SIT_Engine.nominal_static;

                case Record_Type.Stock:
                    return SIT_Engine.stock_static;
            }

            true.tift<ArgumentException>();
            return null;

        }

        void
        Maybe_Enter_Defaults(Data_Line data,
                            Record_Type mode,
                            bool is_new,
                            bool auto_date,
                            Dictionary<string, object> defaults) {

            var dict = data.Dict;

#warning verify
            if (is_new) {
                if (auto_date) {

                    var key = (string)null;

                    switch (mode) {
                        case Record_Type.Sales:
                        case Record_Type.Purchase:
                            key = "ACCOUNT_OPENED"; break;

                        //case Record_Type.Expense: /* not to be filled manually */
                        //key = "RECORD_CREATE_DATE"; break;

                    }

                    if (!key.IsNullOrEmpty())
                        //&&
                        //data[key].strdef().Trim().IsNullOrEmpty())

                        data[key] = today;

                }

                dict.Fill(defaults.Where(_kvp => !dict.ContainsKey(_kvp.Key)), false);

            }
            else {
                dict.Drop(_kvp => _kvp.Value.IsNullOrEmpty());

            }
        }



        IEnumerable<string> Get_Fields_For_Type(Record_Type type) {

            return Sage_Fields.Fields(type).Select(_kvp => _kvp.Key).lst();

        }

        Func<Data_Line, Data_Line, bool>
        Items_Same_Document(Sit_Company_Settings sett_company) {

            var auto = (sett_company.Documents_Numbering_Auto == true);
            var relative = (sett_company.Documents_Grouping_Relative == true);
            var absolute = !auto;

            (relative && !auto).tift();


            return (_dl1, _dl2) =>
            {
                if (relative || absolute) {
                    return _dl1.INVOICE_NUMBER == _dl2.INVOICE_NUMBER;

                }
                if (auto) {
                    return _dl1.ACCOUNT_REF.Clean() == _dl2.ACCOUNT_REF.Clean() &&
                           _dl1.INVOICE_DATE == _dl2.INVOICE_DATE &&
                           _dl1.INVOICE_TYPE == _dl2.INVOICE_TYPE;
                }
                true.tift();
                return false;
            };
        }
    }
}
