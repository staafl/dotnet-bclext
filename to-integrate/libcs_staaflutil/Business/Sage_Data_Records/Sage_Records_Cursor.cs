using System;
using System.Collections.Generic;
using System.Linq;
using Fairweather.Service;
using Versioning;

namespace Common
{
    // Functionality for the dynamic records cursor
    public partial class Sage_Logic
    {

        public Sage_Records_Cursor Get_Accounts_Cursor(Record_Type type) {

            return new Sage_Records_Cursor(this, type);

        }


        public class Sage_Records_Cursor : Records_Cursor
        {
            const string char_order = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            static readonly string[] search_strings =
                (from ch in char_order
                 select ch.ToString() + "*").ToArray();

            readonly Record_Type m_type;
            readonly Sage_Logic sdr;

            Sage_Connection connection;

            Work_Space ws {
                get {
                    return sdr.WS;
                }
            }

            public Sage_Records_Cursor(Sage_Logic sdr, Record_Type type) {

                m_type = type;
                this.sdr = sdr;

            }

            public override IDisposable Prepare(out bool ok) {

                ok = Sage_Access.Sage_Guard(() => { sdr.Establish_Connection(); connection = sdr.Conn; });

                return ok ? connection : null;

            }

            public override void End() {

                connection.Try_Dispose();

            }

            public override Records_Cursor Get_Copy() {

                return new Sage_Records_Cursor(this.sdr, m_type);

            }

            bool IsValidPrefix(string prefix) {
                return prefix.Length <= 8;
            }

            int Get_Customer_Records_Count() {

                int ret;

                using (sdr.Establish_Connection()) {

                    var oRecord = sdr.Get_Dynamic_Record(m_type);

                    ret = oRecord.Count;

                    return ret;
                }
            }


            List<Pair<string>>
            Get_Records_Range(Func<ISageRecord, bool> start,
                                       Func<ISageRecord, bool> next,
                                       int expected_count,
                                       Func<Pair<string>, bool> condition) {

                if (expected_count <= 0)
                    expected_count = 100;

                var ret = new List<Pair<string>>(expected_count);

                using (sdr.Establish_Connection()) {

                    var oRecord = sdr.Get_Dynamic_Record(m_type);

                    if (!start(oRecord))
                        return ret;

                    do {

                        var acc_ref = oRecord[ACCOUNT_REF].ToString();
                        var name = oRecord[NAME].ToString();
                        var pair = new Pair<string>(acc_ref, name);


                        if (!condition(pair))
                            break;

                        ret.Add(pair);

                    }
                    while (next(oRecord));

                }

                return ret;

            }

            Pair<string>? Get_Record(
               Func<ISageRecord, bool> start,
               Func<ISageRecord, bool> next,
               Func<ISageRecord, bool> condition) {

                Pair<string>? ret = null;

                using (sdr.Establish_Connection()) {

                    var oRecord = sdr.Get_Dynamic_Record(m_type);

                    if (Get_Record(oRecord, start, next, condition)) {

                        var acc_ref = oRecord[ACCOUNT_REF].ToString();
                        var name = oRecord[NAME].ToString();
                        ret = new Pair<string>(acc_ref, name);

                    }


                }

                return ret;


            }

            bool Get_Record(ISageRecord instance,
                Func<ISageRecord, bool> start,
                Func<ISageRecord, bool> next,
                Func<ISageRecord, bool> condition) {

                if (!start(instance))
                    return false;

                do {

                    if (condition(instance))
                        return true;

                } while (next(instance));

                return false;
            }

            bool Get_Record(IIndexOrRecord instance,
                            Func<IIndexOrRecord, bool> start,
                            Func<IIndexOrRecord, bool> next,
                            Func<IIndexOrRecord, bool> condition) {

                if (!start(instance))
                    return false;

                do {

                    if (condition(instance))
                        return true;

                } while (next(instance));

                return false;
            }

            Func<IIndexOrRecord, bool> Get_Find(
                bool forward, bool first_col, bool partial, string prefix) {

                // Important: SDO's partial search does not include exact entries.
                // Ours does!

                first_col.tiff();

                Func<IIndexOrRecord, bool> ret;

                if (forward) {

                    ret = partial ? (Func<IIndexOrRecord, bool>)(
                                         record => ForwardFindPartial(record, prefix)) :
                                          record => FindNonPartial(record, prefix);

                }
                else {

                    ret = partial ? (Func<IIndexOrRecord, bool>)(
                                         record => ReverseFindPartial(record, prefix)) :
                                         record => FindNonPartial(record, prefix);

                }

                return ret;
            }

            bool FindNonPartial(IIndexOrRecord record, string prefix) {

                // Important: using this method for reverse find assumes that only one record can ever
                // be found while performing non-partial search

                record.Key = prefix;
                var ret = record.Find(false);
                return ret;

            }

            bool ForwardFindPartial(IIndexOrRecord record, string prefix) {

                if (FindNonPartial(record, prefix))
                    return true;

                record.Key = prefix;// +"*";
                var ret = record.Find(true);

                if (ret) {
                    var key = record.Key;
                    ret = key.StartsWith(prefix);
                }

                return ret;

            }

            bool ReverseFindPartial(IIndexOrRecord record, string prefix) {

                record.Key = (prefix /*+ "*"*/);

                if (record.Find(true)) {

                    var acc_ref = record.Key;
                    if (!acc_ref.StartsWith(prefix))
                        return false;

                    while (record.MoveNext()) {

                        string key = record.Key;

                        if (!key.StartsWith(prefix)) {
                            break;
                        }

                    }

                    // There is a sentinel record after the last one:
                    //record.MoveLast()
                    //true
                    //record.MoveNext()
                    //false
                    //record.Index_Value
                    //""

                    if (!record.MovePrev())
                        return false;

                    return true;
                }

                return FindNonPartial(record, prefix);
            }

            void Get_Start_Next(bool forward, bool first_col,
                                out Func<ISageRecord, bool> start,
                                out Func<ISageRecord, bool> next) {


                first_col.tiff();

                if (first_col) {
                    if (forward) {
                        start = record => record.MoveFirst();
                        next = record => record.MoveNext();
                    }
                    else {
                        start = record => record.MoveLast();
                        next = record => record.MovePrev();
                    }
                    return;
                }
                else {
                    throw new InvalidOperationException();
                }

                //Get_Start_Next_Second_Col(forward, out start, out next);

            }

            /// <summary>
            /// "indices" is assumed to be sorted in advance
            /// </summary>
            /// <param name="forward"></param>
            /// <param name="first_col"></param>
            /// <param name="indices"></param>
            /// <param name="start"></param>
            /// <param name="next"></param>
            void Get_Start_Next_Indexed(bool forward, bool first_col, List<int> indices,
                                                         out Func<ISageRecord, bool> start,
                                                         out Func<ISageRecord, bool> next) {

                indices.tifn();
                first_col.tiff();

                int cnt = indices.Count;

                if (cnt == 0) {
                    start = _ => false;
                    next = _ => false;
                    return;
                }

                if (forward) {

                    var current_rec = 0;
                    int next_rec = indices[0];

                    int ind_indices = 0;

                    start = record =>
                          {
                              if (!record.MoveFirst())
                                  return false;

                              for (; current_rec < next_rec; ++current_rec)
                                  if (!record.MoveNext())
                                      return false;

                              return true;
                          };

                    if (cnt > 1) {
                        next = record =>
                        {
                            ++ind_indices;
                            if (ind_indices >= cnt)
                                return false;

                            next_rec = indices[ind_indices];

                            for (; current_rec < next_rec; ++current_rec) {
                                if (!record.MoveNext())
                                    return false;
                            }

                            return true;
                        };
                    }
                    else {
                        next = _ => false;
                    }

                }
                else {
                    int count = Count;
                    var current_rec = Count - 1;
                    int next_rec = count - 1 - indices[0];
                    int ind_indices = 0;

                    start = record =>
                   {
                       if (!record.MoveLast())
                           return false;

                       for (; current_rec > next_rec; --current_rec)
                           if (!record.MovePrev())
                               return false;

                       return true;
                   };

                    if (cnt > 1) {
                        next = record =>
                        {
                            ++ind_indices;
                            if (ind_indices >= cnt)
                                return false;

                            next_rec = count - 1 - indices[ind_indices];

                            for (; current_rec >= next_rec; --current_rec) {
                                if (!record.MovePrev())
                                    return false;
                            }

                            return true;
                        };
                    }
                    else {
                        next = _ => false;
                    }
                }



            }

            Func<ISageRecord, bool> Get_Next(bool forward) {

                Func<ISageRecord, bool> ret;
                if (forward)
                    ret = record => record.MoveNext();
                else
                    ret = record => record.MovePrev();

                return ret;

            }

            Func<Pair<string>, bool> Get_Starts_With(bool first_col, string str, StringComparison comparison) {

                Func<Pair<string>, bool> ret;

                if (first_col)
                    ret = pair => pair.First.StartsWith(str, comparison);
                else
                    ret = pair => pair.Second.StartsWith(str, comparison);

                return ret;

            }


            public Account_Record this[string account_ref] {
                get {
                    return sdr.Get_Account_Record(true, account_ref).Value;
                }
            }

            public override int Count {
                get {
                    return Get_Customer_Records_Count();
                }
            }

            public override bool Is_Empty {
                get {

                    bool ret;

                    using (sdr.Establish_Connection()) {

                        var oRecord = sdr.Get_Dynamic_Record(m_type);

                        ret = !oRecord.MoveFirst();

                    }

                    return ret;

                }
            }

            public override bool Supports_Second_Column {
                get { return false; }
            }

            public override bool ContainsKey(string key) {

                bool ret = false;

                Sage_Access.Sage_Guard(() =>
                {
                    using (sdr.Establish_Connection()) {

                        var oRecord = sdr.Get_Dynamic_Record(m_type);
                        oRecord[ACCOUNT_REF] = key;

                        ret = oRecord.Find(false);

                    }
                });

                return ret;

            }

            public override List<Pair<string>> GetRange(int startIndex, int length) {

                (startIndex >= 0).tiff();

                var forward = Forward;
                var first_col = First_Column;

                Func<Pair<string>, bool> condition = _ => true;
                Func<ISageRecord, bool> start, next, start2, next2;

                Get_Start_Next(forward, first_col, out start, out next);

                start2 =
                record =>
                {

                    if (!start(record))
                        return false;

                    for (int ii = 0; ii < startIndex; ++ii)
                        if (!next(record))
                            return false;

                    return true;
                };


                next2 = next;
                int jj = 0;

                if (length >= 0) {

                    condition = _ => jj < length;
                    next2 = record =>
                    {
                        ++jj;

                        return next(record);
                    };
                }


                var list = Get_Records_Range(start2, next2, length, condition);

                return list;


            }

            public override List<Pair<string>> GetPureRange(string prefix, int length, StringComparison comparison) {

                if (!IsValidPrefix(prefix))
                    return new List<Pair<string>>();

                (comparison == StringComparison.InvariantCultureIgnoreCase).tiff();
                prefix = prefix.ToUpperInvariant();

                var forward = Forward;
                var first_col = First_Column;

                var start = Get_Find(forward, first_col, true, prefix);
                var next = Get_Next(first_col);
                var condition = Get_Starts_With(first_col, prefix, comparison);

                var next2 = next;
                var condition2 = condition;

                if (length >= 0) {
                    int ii = 0;
                    condition2 = record => ii < length && condition(record);
                    next2 = record => { ++ii; return next(record); };
                }

                // Func<ISageRecord, bool> chained = a => start(a);

                var cast = start.Convert_Delegate<Func<ISageRecord, bool>>();

                var ret = Get_Records_Range(cast, next2, length, condition2);

                return ret;

            }

            public override Pair<string> GetAtIndex(int index) {

                (index >= 0).tiff();

                var forward = Forward;
                var first_col = First_Column;

                Func<ISageRecord, bool> start, next;
                Get_Start_Next(forward, first_col, out start, out next);


                int ii = 0;

                Func<ISageRecord, bool> next2 =
                record =>
                {
                    if (ii == index)
                        return false;

                    if (ii < index) {
                        while (++ii <= index) {
                            if (!next(record))
                                return false;
                        }
                        --ii;
                        return true;
                    }

                    true.tift("Missed sought index");
                    return false;
                };

                Func<ISageRecord, bool> condition =
                _ =>
                {
                    return ii == index;
                };

                var null_pair = Get_Record(start, next2, condition);

                var ret = null_pair.Value;

                return ret;
            }

            public override Pair<string>? GetKVP(string key) {

                Func<ISageRecord, bool> start = (record) =>
                {
                    record[ACCOUNT_REF] = key;
                    return record.Find(false);
                };
                Func<ISageRecord, bool> next = (_) => false;

                Func<ISageRecord, bool> condition = _ => true;

                return Get_Record(start, next, condition);

            }

            public override int? GetIndex(string prefix, bool partial, StringComparison comparison) {

                (comparison == StringComparison.InvariantCultureIgnoreCase).tiff();
                prefix = prefix.ToUpperInvariant();

                if (!IsValidPrefix(prefix))
                    return null;

                bool forward = Forward;
                bool first_col = First_Column;

                using (sdr.Establish_Connection()) {

                    var SalesIndex = sdr.Get_Index(m_type);

                    var start = Get_Find(forward, first_col, partial, prefix);

                    Func<IIndexOrRecord, bool> next = _ => false;

                    Func<IIndexOrRecord, bool> condition = _ => true;

                    if (Get_Record(SalesIndex, start, next, condition)) {

                        int ret = SalesIndex.IndexRecordNumber - 1;

                        if (!forward)
                            ret = Count - ret - 1;

                        return ret;
                    }

                    return null;
                }
            }

            public override List<Pair<string>> GetAtIndices(List<int> indices, bool is_presorted) {

                indices.tifn();

                if (!is_presorted)
                    indices.Sort();

                bool forward = Forward;
                bool first_col = First_Column;

                Func<ISageRecord, bool> start, next;

                Get_Start_Next_Indexed(forward, first_col, indices, out start, out next);

                var ret = Get_Records_Range(start, next, indices.Count, _ => true);

                return ret;

            }

            //// Doesn't work - Sage does not allow us to search by name.
            ///
            //void Get_Start_Next_Second_Col(bool forward,
            //                    out Func<ISageIndex, bool> start,
            //                    out Func<ISageIndex, bool> next) {

            //      int ii = 0;
            //      int max_index = char_order.Length - 1;

            //      //Func<IIndex, bool> try_ind = forward : 
            //      //    record =>
            //      //    {   record[NAME] = search_strings[ii];
            //      //            ++ii;
            //      //        return record.Find(true);
            //      //    } : 
            //      //    record =>
            //      //    {   record[NAME] = search_strings[ii];
            //      //            --ii;
            //      //        return record.Find(true);
            //      //    };

            //      if (forward) {
            //            start = record =>
            //            {
            //                  if (!record.MoveFirst())
            //                        return false;

            //                  while (ii <= max_index) {
            //                        record[NAME] = search_strings[ii];
            //                        ++ii;
            //                        if (record.Find(true))
            //                              return true;
            //                  }

            //                  return false;
            //            };

            //            next = record =>
            //            {
            //                  if (record.MoveNext())
            //                        return true;

            //                  while (ii <= max_index) {
            //                        record[NAME] = search_strings[ii];
            //                        ++ii;
            //                        if (record.Find(true))
            //                              return true;
            //                  }

            //                  return false;
            //            };

            //      }

            //      ii = max_index;
            //      start = record =>
            //      {
            //            if (!record.MoveFirst())
            //                  return false;

            //            while (ii >= 0) {
            //                  record[NAME] = search_strings[ii];
            //                  --ii;
            //                  if (record.Find(true)) {
            //                        if (record.MoveLast())
            //                              return true;
            //                  }
            //            }

            //            return false;
            //      };

            //      next = record =>
            //      {
            //            if (record.MovePrev())
            //                  return true;

            //            while (ii >= 0) {
            //                  record[NAME] = search_strings[ii];
            //                  --ii;
            //                  if (record.Find(true)) {
            //                        if (record.MoveLast())
            //                              return true;
            //                  }
            //            }

            //            return false;
            //      };
            //}

            //public override List<Pair<string>> GetRange(string prefix, int length, StringComparison comparison) {

            //    var forward = Forward;
            //    var first_col = First_Column;

            //    Func<Pair<string>, bool> condition = _ => true;
            //    var condition2 = condition;

            //    var to_find = prefix + "*";
            //    var start = Get_Find(forward, first_col, true, to_find);
            //    var next = Get_Next(forward);
            //    var next2 = next;

            //    if (length >= 0) {
            //        int ii = 0;
            //        condition2 = _ => ii < length;
            //        next2 = record => { ++ii; return next(record); };
            //    }

            //    var ret = Get_Customer_Records_Range(start, next2, length, condition2);

            //    return ret;

            //}

        }










    }


}