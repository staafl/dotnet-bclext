using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

using Fairweather.Service;
using Versioning;

using Common;
using Common.Controls;
using Common.Dialogs;
using DTA;
using Standardization;


namespace Sage_Int
{
    public partial class SIT_Engine
    {


        void Clear() {
            sit_exe.Clear();
        }

        // ****************************


        bool YesNo(Func<string> msg) {
            return sit_exe.YesNo(msg);
        }



        void Warn(object obj) {
            sit_exe.Warn(obj.strdef());
        }

        void Warn(string str, params object[] objs) {
            sit_exe.Warn(str.spf(objs));
        }

        // ****************************

        bool
        RetryIgnoreFail(Func<bool> f, Func<string> msg) {

            bool _;
            return sit_exe.RetryIgnoreFail(f, msg, false, out _);
        }

        bool
        RetryIgnoreFail(Func<bool> f, Func<string> msg, bool can_ignore) {

            bool _;
            return sit_exe.RetryIgnoreFail(f, msg, can_ignore, out _);

        }

        bool
        RetryIgnoreFail(Func<bool> f, Func<string> msg, out bool ignore) {
            return sit_exe.RetryIgnoreFail(f, msg, true, out ignore);
        }

        bool
        RetryIgnoreFail(Func<bool> f, Func<string> msg, bool can_ignore, out bool ignore) {
            return sit_exe.RetryIgnoreFail(f, msg, can_ignore, out ignore);
        }

        /* TBI */
        public Dictionary<string, string>
       Generate_Mappings(string company, Record_Type type) {

            var ret = new Dictionary<string, string>();
            throw new NotImplementedException();

            //string file = "Mappings\\{0}\\{1}.dta".spf(company, mapping_files[mode]);

            //if (!File.Exists(file))
            //    return ret;

            //Regex regex = new Regex(@"([A-Z0-9_]+) (.+) --> ([^\r\n])");//TODO
            ////([-.'""!#$%&()_/A-Za-z0-9]+) --> ([-.'""!#$%&()_/A-Za-z0-9]*)");
            //Match m;

            //StreamReader sr = new StreamReader(file);
            //string contents = sr.ReadToEnd();
            //sr.Close();

            //m = regex.Match(contents);
            //while (m.Success) {

            //    string header = m.Groups[1].Value.ToUpper();
            //    string from = m.Groups[2].Value;
            //    string to = m.Groups[3].Value;

            //    if (!ret.Keys.Contains(header)) {
            //        ret[header] = new Dictionary<string, string>();
            //    }

            //    ret[header][from] = to;
            //    m = m.NextMatch();

            //}

            //return ret;
        }


        /// <summary>
        /// Gets the index number of the specified module number into the
        /// binary settings string, to be used when reading the settings.
        /// </summary>
        public static int
        Get_Mode_Index(Record_Type type) {

            var ret = mode_ixes.IndexOf(type);

            (ret < 0).tift<ArgumentOutOfRangeException>("mode");

            ++ret;
            return ret;
        }


        bool
       Open_File(string file, out FileStream fs) {

            var fs_tmp = fs = null;
            var err_msg = "";

            var ret = RetryIgnoreFail(
                   () =>
                   {
                       try {
                           fs_tmp = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Read);
                           return true;
                       }
                       catch (Exception _ex) {
                           err_msg = " (" + _ex.Msg_Or_Type() + ")";
                           return false;
                       }
                   },
                   () => "Unable to access file '" + file + "'" + err_msg + ".",
                   false);

            fs = fs_tmp;

            return ret;
        }

        public bool
       Check_Empty_File(string file, bool dynamic) {

            try {
                using (var sr = new StreamReader(file))
                using (var csv = Get_Csv_Reader(sr, false)) {

                    int needed_lines = dynamic ? 2 : 1;

                    for (int ii = 0; ii < needed_lines; ++ii) {
                        if (csv.Get_Next_Record() == null)
                            return false;
                    }

                    return true;

                }
            }
            catch (XCsv) {
                return true;
            }
        }

        // ****************************

        bool
        Try_Copy_File(string from, string to) {

            bool ret = false;
            var ex_str = "";
            var dir = Path.GetDirectoryName(to);
            Directory.CreateDirectory(dir);
            RetryIgnoreFail(() =>
            {
                try {
                    File.Copy(from, to);
                    ret = true;
                    return true;
                }
                catch (Exception _ex) {

                    ex_str = " (" + _ex.Msg_Or_Type() + ")";

                    return false;

                }
            },
            () => "Unable to copy file '" + from + "' to '" + to + ":\n\n'" + ex_str + ".",
            true);

            return ret;
        }

        bool
        Try(Func<bool> f, string base_msg) {
            bool skipped;
            return Try(f, base_msg, false, out skipped);
        }

        bool
        Try(Func<bool> f, string base_msg, out bool skipped) {
            return Try(f, base_msg, true, out skipped);
        }

        // int times = 0;
        bool
        Try(Func<bool> f, string base_msg, bool can_ignore, out bool skipped) {
            var err_msg = "";
            return RetryIgnoreFail(
             () =>
             {
                 if (f()) {
                     //if (++times < 10)
                     //    if (new Random().Next() % 3 == 1) {
                     //        err_msg = " (TEST)";
                     //        return false;
                     //    }
                     return true;
                 }

                 var _last_error = sdo.Last_Error_Text;
                 if (!_last_error.IsNullOrEmpty())
                     err_msg = " (" + _last_error + ")";

                 return false;
             },
             () => base_msg + err_msg + ".",
             can_ignore,
             out skipped);

        }






    }

}