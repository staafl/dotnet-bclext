using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using Common.Posting;
using Fairweather.Service;
using Standardization;
using Versioning;
using Line = Fairweather.Service.Pair<Common.Posting.Data_Line, Sage_Int.Line_Data>;
using Saved = Fairweather.Service.Triple<Common.Posting.Data_Line, Sage_Int.Line_Data, object>;

namespace Sage_Int
{
    partial class SIT_Engine
    {


        void
        Insert(out int? return_code,
              Sage_Context context,
              Sit_Company_Settings sett_company,
              Credentials creds,
              Record_Type mode,
              Dictionary<string, object> defaults,
              string[] headers,
              IEnumerable<Line> lines) {

            return_code = Data.OTHER_ERROR;

            Interface_Started.Raise();



            // ****************************


            Insert_Delegate del = null;

            if (accounts[mode] || mode == Record_Type.Stock)
                del = Insert_Accounts(sett_company, mode, defaults);

            else if (docs_modes[mode])
                del = Insert_Documents(sett_company, mode, defaults);

            else if (mode == Record_Type.Audit_Trail)
                del = Insert_Audit_Trail(sett_company);


            del.tifn();


            // ****************************


            object null_ix = null;
            Line_Data? null_line = null;

            Action tell = () =>
            {
                if (null_ix == null ||
                    null_line == null)
                    return;
                Tell(false, mode, null_ix, null_line.Value);
            };

            var ticker = new Ticker(TimeSpan.FromSeconds(0.3), tell);

            var success_log_1 = Data.STR_Sit_Success_Log.Path;
            var success_log_2 = Get_Success_Path(creds.Company);

            bool trans = (mode == Record_Type.Audit_Trail);
            bool docs = docs_modes[mode];
            var rules = new List<IRule> { new General_Rules(mode) };

            if (mode == Record_Type.Audit_Trail)
                rules.Add(Get_TT_Rules(context, sett_company));


            // ****************************
            int cnt_saved = 0, cnt_skipped = 0, cnt_seen = 0;
            using (var sw = new StreamWriter(success_log_1)) {

                var auto = docs && (sett_company.Documents_Numbering_Auto == true) &&
                                   (sett_company.Documents_Grouping_Relative == false);

                // HACK
                if (docs && auto)
                    sw.Write("INVOICE_NUMBER,");

                sw.Write(headers.Csv());
                if (trans)
                    sw.Write(",    ,SPLIT_NUMBER");
                sw.WriteLine();

                foreach (var pair in lines.Mark_Last()) {

                    ++cnt_seen;

                    var is_last = pair.Second;
                    var data = pair.First.First;

                    foreach (var rule in rules) {
                        int _1 = default(int);
                        bool _2;
                        IEnumerable<Validation_Error> _3;

                        rule.Inspect_Line(data, _1, is_last, out _2, out _3).tiff();
                    }

                    var line = pair.First.Second;
                    null_line = line;

                    bool failed;
                    IEnumerable<Saved> saved, skipped;

                    // ****************************

                    del(data, line, is_last, out saved, out skipped, out failed);

                    // ****************************

                    if (failed) {
                        return_code = Data.FILE_FAILED_TO_INTERFACE;
                        return;
                    }

                    var saved_cnt = saved.Count();
                    var skipped_cnt = skipped.Count();

                    cnt_saved += saved_cnt;
                    cnt_skipped += skipped_cnt;

                    if (!trans && !docs) {
                        ((saved_cnt >= 1) ^ (skipped_cnt >= 1)).tiff();
                    }

                    foreach (var triple in saved) {

                        var saved_data = triple.First;
                        var saved_line = triple.Second;
                        var obj_ix = triple.Third;

                        null_ix = obj_ix ?? null_ix;

                        // HACK
                        if (docs && auto) {
                            sw.Write(saved_data.INVOICE_NUMBER + ",");
                        }

                        sw.Write(headers.Select(_h =>
                        {
                            var _saved_field = saved_data[_h];
                            return _saved_field is DateTime
                                   ? ((DateTime)_saved_field).ToString(true)
                                   : _saved_field.strdef();
                        }).Csv());

                        if (trans) {
                            obj_ix.tifn();
                            sw.Write(",    ," + (obj_ix ?? "??"));
                        }

                        sw.WriteLine();

                    }


                    foreach (var triple in skipped) {

                        var skipped_data = triple.First;
                        var skipped_line = triple.Second;
                        var obj_ix = triple.Third;


                        var tmp = trans ? "transaction" :
                                  docs ? "document" :
                                  "record '" + obj_ix + "'";

                        tmp += " at line " + skipped_line.User_Line;

                        sw.WriteLine("# " + tmp + " skipped due to error.");
                    }



                    ticker.Tick(is_last);


                }


            }


            Interface_Over.Raise(
                Triple.Make(cnt_saved, cnt_skipped, cnt_seen), success_log_1);

            // ****************************

            Try_Copy_File(success_log_1, success_log_2);

            // ****************************


            return_code = null;

        }


        delegate void
        Insert_Delegate(Data_Line data, Line_Data line, bool is_last,
                        out IEnumerable<Saved> saved, out IEnumerable<Saved> skipped, out bool failed);

        ILinkRecord
        Get_Record(Record_Type mode) {

            return (ILinkRecord)ws.Create(
                new Dictionary<Record_Type, Type>
                {
                    {Record_Type.Sales, typeof(Versioning.SalesRecord)},
                    {Record_Type.Purchase, typeof(Versioning.PurchaseRecord)},
                    {Record_Type.Expense, typeof(Versioning.NominalRecord)},
                    {Record_Type.Bank, typeof(Versioning.BankRecord)},

                }[mode]);
        }


        Insert_Delegate
        Insert_Accounts(Sit_Company_Settings sett_company,
                       Record_Type mode,
                       Dictionary<string, object> defaults) {

            bool use_defaults = ((mode == Record_Type.Purchase) ||
                                 (mode == Record_Type.Sales)) &&
                                (sett_company.Use_Defaults == true);

            bool auto_date = (sett_company.Auto_Date == true);
            bool overwrite = (sett_company.Overwrite == true);
            //bool edit = !overwrite;

            var record = Get_Record(mode);

            return delegate(Data_Line _data, Line_Data _line, bool _is_last,
                            out IEnumerable<Saved> _saved,
                            out IEnumerable<Saved> _skipped,
                            out bool _failed) {

                _failed = false;
                _saved = new Saved[0];
                _skipped = new Saved[0];
                var _any_skipped = false;

                var acc_ref = _data.ACCOUNT_REF;

                var triple = new Saved[] { Triple.Make(_data, _line, (object)acc_ref) };

                var _is_new = true;

                //if (edit) {
                record["ACCOUNT_REF"] = acc_ref;
                if (record.Find(false)) {

                    if (_failed = !Try(
                    () => record.Edit(),
                    "Unable to access record '" + acc_ref + "' in edit mode",
                    out _any_skipped))
                        return;

                    if (_any_skipped) {
                        _skipped = triple;
                        return;
                    }

                    _is_new = false;

                }
                //}

                if (_is_new) {
                    if (_failed = !Try(() => record.AddNew(),
                                       "Sage access error"))
                        return;

                    if (_any_skipped) {
                        _skipped = triple;
                        return;
                    }
                }

                if (_is_new) {
                    if (use_defaults)
                        Maybe_Enter_Defaults(_data, mode, _is_new, auto_date, defaults);
                }

                if (!_is_new && !overwrite)
                    _data.Dict.Drop(_kvp => _kvp.Value.strdef().Trim().IsNullOrEmpty());

                record.Fill(_data.Dict);

                if (_failed = !Try(
                    () => record.Update(),
                    "Unable to save record " + acc_ref.Quote(false),
                    out _any_skipped))
                    return;

                if (_any_skipped) {
                    _skipped = triple;
                    return;
                }

                _saved = triple;
            };
        }


   


        Insert_Delegate
        Insert_Documents(Sit_Company_Settings sett_company,
                         Record_Type mode,
                         Dictionary<string, object> defaults) {

            var auto = (sett_company.Documents_Numbering_Auto == true);
            var relative = (sett_company.Documents_Grouping_Relative == true);
            var absolute = !auto;

            (relative && !auto).tift();


            var invoice_post = (InvoicePost)null;
            var last_line = (Data_Line)null;

            var invoice_record_fields = Get_Fields_For_Type(Record_Type.Invoice);
            var invoice_item_fields = Get_Fields_For_Type(Record_Type.Invoice_Item);

            var same_header = Items_Same_Document(sett_company);

            var header = new List<Pair<Data_Line, Line_Data>>();
            Fields header_obj = null;


            return delegate(Data_Line _data, Line_Data _line, bool _is_last,
                out IEnumerable<Saved> _saved,
                out IEnumerable<Saved> _skipped,
                out bool _failed) {

                // mostly copy-paste from Insert_Audit_Trail's delegate

                _failed = false;

                var _lst_saved = new List<Saved>();
                _saved = _lst_saved;
                _skipped = new Saved[] { };




                // if this call returns false, state is corrupted - 
                // don't process any more records
                Func<bool> _maybe_update = () =>
                {
                    int num;

                    if (auto || relative)
                        num = invoice_post.GetNextNumber;
                    else
                        num = _data.INVOICE_NUMBER;

                    header_obj["INVOICE_NUMBER"] = num;

                    if (!Try(() => invoice_post.Update(),
                        "Unable to interface document at line " + _line.User_Line)) {


                        return false;
                    }


                    invoice_post = null;


                    // avoiding a subtle bug where the newly entered INVOICE_NUMBER coincides with that
                    // of the next item in the file (this could mess up relative mode)
                    // _last_line.INVOICE_NUMBER = -1;
                    last_line = null;




                    foreach (var _t2 in header)
                        _t2.First.INVOICE_NUMBER = num;

                    _lst_saved.AddRange(header.Select(__t2 => new Saved(__t2.First, __t2.Second, num))
                                       .lst());
                    header.Clear();

                    // ??? invoice_post.Clear();
                    return true;

                };



                if (invoice_post != null) {

                    if (!same_header(_data, last_line)) {

                        if (_failed = !_maybe_update())
                            return;


                    }

                }

                if (invoice_post == null) {
                    invoice_post = ws.Create<InvoicePost>();
                    invoice_post.Type = _data.INVOICE_TYPE.Ledger_Type().Value;
                    header_obj = invoice_post.HDGet();
                    header_obj.Fill(_data, invoice_record_fields);
                }

                var _item = invoice_post.AddItem();

                _item.Fill(_data, invoice_item_fields);

                header.Add(Pair.Make(_data, _line));

                last_line = _data;

                if (_is_last) {
                    if (_failed = !_maybe_update())
                        return;

                }


            };

        }


        Insert_Delegate
        Insert_Audit_Trail(Sit_Company_Settings sett_company) {

            var groupable = new Set<TransType>();

            if (sett_company.Group_Transactions_Any == true) {

                if (sett_company.Group_Transactions_Sales == true) {
                    groupable[TransType.sdoSC] = true;
                    groupable[TransType.sdoSI] = true;
                }

                if (sett_company.Group_Transactions_Purchase == true) {
                    groupable[TransType.sdoPC] = true;
                    groupable[TransType.sdoPI] = true;
                }

            }

            Data_Line last_seen = null;
            TransactionPost tran_post = null;
            var header = new List<Pair<Data_Line, Line_Data>>();

            var header_fields = Get_Fields_For_Type(Record_Type.Header_Data);
            var split_fields = Get_Fields_For_Type(Record_Type.Split_Data);


            return delegate(Data_Line _data, Line_Data _line, bool _is_last,
                            out IEnumerable<Saved> _saved,
                            out IEnumerable<Saved> _skipped,
                            out bool _failed) {

                _failed = false;


                var _saved_lst = new List<Saved>();
                _saved = _saved_lst;

                _skipped = new Saved[0];

                var _type = _data.TYPE;

                /* invariant: tran_post == null == last_seen == null */




                // if this call returns false, state is corrupted - 
                // don't process any more transactions
                Func<bool> _maybe_update = () =>
                {
                    if (!Try(() => tran_post.Update(),
                        "Unable to post transaction at line " + _line.User_Line)) {
                        return false;
                    }

                    var __post_no = tran_post.PostingNumber;


                    tran_post = null;
                    last_seen = null;


                    var __header = ws.Create<HeaderData>();

                    if (!Try(() => __header.Read(__post_no),
                           "Unable to access posted transaction at line " + _line.User_Line))
                        return false;





                    var __split = __header.Link;
                    var __fn = __split.First_Next();

                    int? __no = __header["FIRST_SPLIT"].ToInt32();

                    bool __fail = false;

                    using (var __enm = header.GetEnumerator()) {
                        /*
                        do {
                        /*/
                        while (__fn) {
                            //*/

                            // here __enm is one record behind __fn/__split

                            if (__fail = !__enm.MoveNext())
                                // too many splits
                                break;

                            // here __enm is on the same record as __fn/__split

                            var __triple = Triple.Make(__enm.Current.First, __enm.Current.Second, (object)(int?)__no);
                            _saved_lst.Add(__triple);


                            __no = __split["NEXT_SPLIT"].ToInt32();

                        }
                        //while (__split.MoveNext());

                        if (!__fail) {

                            while (__enm.MoveNext()) {

                                // too few splits
                                __fail = true;

                                var __triple = Triple.Make(__enm.Current.First, __enm.Current.Second, (object)(int?)null);
                                _saved_lst.Add(__triple);

                            }

                        }


                    }

                    header.Clear();



                    if (!__fail)
                        return true;

                    return YesNo(() =>
                       "Unable to compose interface log for transaction at line " + _line.User_Line + "."
                   + "\nContinue anyway?");
                };


                if (tran_post != null) {

                    var _same_header = last_seen.Same_Header(_data) &&
                                       groupable[_type];

                    if (!_same_header) {

                        if (_failed = !_maybe_update())
                            return;



                    }

                }

                if (tran_post == null) {
                    tran_post = ws.Create<TransactionPost>();
                    tran_post.HDGet().Fill(_data, header_fields);


                }

                var _split = tran_post.SDAdd();

                _split.Fill(_data, split_fields);

                var _proj = _data["PROJECT_REF"];
                var _cc = _data["COST_CODE"];

                if (!_proj.IsNullOrEmpty()) {
                    var _proj_obj = ws.CreateProject();
                    _proj_obj.Load(_proj);
                    _split.Project = _proj_obj;
                }
                if (!_cc.IsNullOrEmpty()) {
                    var _cc_obj = ws.CreateProjectCostCode();
                    _cc_obj.Load(_cc);
                    _split.CostCode = _cc_obj;
                }

                header.Add(Pair.Make(_data, _line));

                last_seen = _data;

                if (_is_last) {
                    if (_failed = !_maybe_update())
                        return;

                }















                /* END */















                /*       Old crap that's not relevant anymore        */

                //var tmp2 = header["NO_OF_SPLIT"].ToInt32();
                //var tmp1 = header["FIRST_SPLIT"].ToInt32();

                //num_splits.Add(tmp2);
                //first_splits.Add(tmp2);

                //if (first_splits.Count > 1) {
                //    bool shown = false;
                //    line = (b_dynamic ? 2 : 1);

                //    for (int cnt = 1; cnt < first_splits.Count; ++cnt) {
                //        ++line;
                //        if (first_splits[cnt - 1] + num_splits[cnt - 1] != first_splits[cnt]) {
                //            if (!H.Set(ref shown, true))
                //                Console.WriteLine("There was a Transaction Error. Sage Accounts data integrity may be compromised.");
                //            Console.WriteLine("Unexpected error occurred at line {0}.", line);
                //        }
                //    }
                //}

            };

        }



        // ****************************



    }
}