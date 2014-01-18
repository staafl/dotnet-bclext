using System;
using System.Collections.Generic;
using Common.Queries;
using Fairweather.Service;
using Fairweather.Service.Syntax;
using Versioning;

namespace Common
{
    partial class Sage_Logic
    {


        public void 
        Run_Record_Query(Record_Type type,
                         Clause clause,
                         Action<IRead<string, object>> every_record,
                         Action<IRead<string, object>> matching_only,
                         out int total_scanned) {

            var predicate = Query.Compile(clause);

            total_scanned = 0;
            using (Establish_Connection()) {

                var record = Get_Record(type);

                if (!record.MoveFirst())
                    return;


                do {
                    ++total_scanned;
                    var ro = record.ro();

                    every_record(ro);

                    if (!predicate((IEntity)record))
                        continue;

                    matching_only(ro);

                } while (record.MoveNext());


            }

        }

        public void
        Run_Contains_Query(Record_Type type,
                           Dictionary<string, string> strings,
                           Action<IRead<string, object>> every,
                           Action<IRead<string, object>> output,
                           bool caseless,
                           out int total_scanned) {

            total_scanned = 0;

            using (Establish_Connection()) {

                var record = Get_Record(type);

                if (!record.MoveFirst())
                    return;

                var rd = record.ro();

                do {
                    if (record.Deleted)
                        continue;

                    bool ok = true;
                    ++total_scanned;

                    foreach (var kvp in strings) {

                        var key = kvp.Key;
                        string value;

                        if (key == "ADDRESS")
                            value = ((Sage_Container)record).Address;
                        else
                            value = record[key].ToString();

                        var s1 = value;
                        var s2 = kvp.Value;

                        if (caseless) {
                            s1 = s1.ToUpper();
                            s2 = s2.ToUpper();
                        }

                        if (!s1.Contains(s2)) {
                            ok = false;
                            break;
                        }

                    }

                    every(rd);

                    if (!ok)
                        continue;

                    output(rd);

                } while (record.MoveNext());


            }

        }
    }

}
