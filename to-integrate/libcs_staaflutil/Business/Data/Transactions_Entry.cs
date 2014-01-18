using System;
using System.Collections.Generic;
using System.IO;
using Fairweather.Service;

namespace Common
{
    public static partial class Data
    {

        const string TE_DETAILS = "details.txt";
        const string STR_Tememo = "te_memo";
        const string STR_Tereports = "te_reports";

        static public string Get_TE_Report_Dir(Company_Number number) {
            var ret = Get_Company_Directory(number);
            ret = ret.Cpath(STR_Tereports);
            Directory.CreateDirectory(ret);
            return ret;
        }

        static public string Details_History_File(Company_Number number) {
            var ret = Get_Company_Directory(number);
            ret = ret.Cpath(TE_DETAILS);
            return ret;
        }

        static public string Get_TE_Memo_Dir(Company_Number number) {
            var ret = Get_Company_Directory(number);
            ret = ret.Cpath(STR_Tememo);
            return ret;
        }

        static public Lazy<string>
        Get_TE_Memo_Filename(Company_Number number, string name) {

            var dir = Get_TE_Memo_Dir(number);
            return Get_Memo_Filename(number, name, dir);


        }
    }
}
