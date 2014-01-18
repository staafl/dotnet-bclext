using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

using Common;
using Fairweather.Service;
using Ionic.Zip;
using Microsoft.Win32;
using Standardization;
using Versioning;

namespace Activation
{
    // Session.Debug\){(:Wh)}+MessageBox\.Show
    // cst_debug)\1Console.WriteLine
    /*  */
    // <summary>(:Wh)+///
    // <summary>


    public static partial class Activation_Helpers
    {
        const string INVALID_OR_INCORRECT = "INVALID OR INCORRECT USERNAME OR PASSWORD";
        const string vcredist5EXE = "vcredist5.exe";
        const string vcr = "vcr";
        const string sgeDTA = "sge.dta";
        const string vcrEXE = "vcr.dta";






        /*       Sage program folders and files        */


        /// <summary> Finds the location of sage.usr for a specified version, using GetSageFolderVersion to
        /// verify </summary>
        public static bool
        Find_sage_usr(int version, out string sageusr) {

            bool ret;
            bool found = false;
            string path = "";


            if (version >= 11 && version <= 13) {
                path = Find_sage_usr_v11_v13(version, out found);

            }
            else {
                /*       Versions 14, 15,...        */
                /* Try default directories */

                path = Get_Sage_Data_Folder(version);
                if (Directory.Exists(path))
                    found = true;

            }


            sageusr = path.Cpath("sage.usr");

            ret = (found && File.Exists(sageusr));

            if (!ret)
                sageusr = "";

            return ret;
        }

        /// <summary>
        /// Only returns the parent directory
        /// </summary>
        /// <param name="version"></param>
        /// <param name="is_ok"></param>
        /// <returns></returns>
        static string
        Find_sage_usr_v11_v13(int ver, out bool is_ok) {

            // Refer to sage_registry_keys.txt and odbc_registry_keys.txt
            // for more info

            (ver >= 11 && ver <= 13).tiff();

            is_ok = false;

            var hklm = Registry.LocalMachine;

            Func<RegistryKey, string, string, string>
            get_value = (_key, _str1, _str2) =>
            {
                var _child = _key.OpenSubKey(_str1);
                if (_child == null)
                    return "";

                var _ret = _child.GetValue(_str2, "").strdef();
                return _ret ?? "";
            };

            Func<string, bool> is_correct_path = _path =>
            {

                int _tmp_ver;
                if (!Get_Sage_Dir_Version(_path, out _tmp_ver))
                    return false;

                if (_tmp_ver != ver)
                    return false;

                return true;
            };


            var hklm_software_sage = hklm.OpenSubKey("software\\sage");

            if (hklm_software_sage != null) {

                var version_keys = from _key in hklm_software_sage.GetSubKeyNames()
                                   where _key.StartsWith("Accounts ")
                                   select _key;

                /* HKEY_LOCAL_MACHINE\SOFTWARE\*
                ...
                [...\Sage\Accounts 11.01]
                [...\Sage\Accounts 11.01\11.00]
                [...\Sage\Accounts 12.00]
                [...\Sage\Accounts 12.00\12.00]
                [...\Sage\Accounts 14.00]
                [...\Sage\Accounts 14.00\14.00]
                [...\Sage\Accounts 2010]
                 ...
                 */

                if (!version_keys.Is_Empty()) {

                    /* HKEY_LOCAL_MACHINE\SOFTWARE\
                     [...\Sage\Line 50]
                     [...\Sage\Line 50\14.00]
                     [...\Sage\Line 50\15.00]
                     [...\Sage\Line 50\16.00]
                     [...\Sage\Line 50\LastInstallPath]
                     @="C:\\Sage16\\"
                     [...\Sage\Line 50\PreviousInstallPath]
                     @="C:\\Sage11\\"
                     */

                    foreach (var suffix in new[] { "lastinstallpath", "previousinstallpath" }) {

                        var path = get_value(hklm_software_sage, "line 50\\" + suffix, "");
                        if (is_correct_path(path)) {
                            is_ok = true;
                            return path;
                        }

                    }
                }

            }


            /* [...\ODBC\ODBC.INI\SageLine50v11]
            "Driver"="C:\\Windows\\system32\\S11DBC32.dll"
            "DataPathname"="C:\\Sage11\\ACCDATA"
            "UseDataPath"="No"*/
            var key = "software\\odbc\\odbc.ini\\SageLine50v" + ver;

            var ret = get_value(hklm, key, "DataPathname").ToUpper();

            if (!ret.Eat_Until("\\ACCDATA", true, out ret))
                return null;

            if (is_correct_path(ret)) {
                is_ok = true;
                return ret;
            }

            return null;

        }

        public static string
        Get_Sage_Data_Folder(int version) {

            string ret = H.Get_Common_AppData_Dir();

            string trail = "sage\\accounts\\{0}".spf(cst_ver_0_year + version);

            ret = ret.Cpath(trail);

            return ret;

        }


        /// <param name="folder">A sage install folder</param>
        public static bool
        Get_Sage_Dir_Version(string folder, out int ver) {

            folder = folder.ToUpper()
                           .TrimEnd('\\');

            /*       Already found?        */
            if (m_sage_dirs.TryGetValue(folder, out ver))
                return true;

            var ret = Get_Sage_Dir_Version_Inner(folder, out ver);

            if (ret)
                m_sage_dirs[folder] = ver;

            return ret;

            /* another option for version id - 12 lines */
            // see scraps -002
        }

        static bool
        Get_Sage_Dir_Version_Inner(string folder, out int ver) {

            H.assign(out ver);

            /*       Sage executable version        */

            string str_exe = folder.Cpath("sage.exe");

            if (File.Exists(str_exe)) {

                var fvi = FileVersionInfo.GetVersionInfo(str_exe);

                ver = fvi.FileMajorPart;

                return true;
            }

            /*       Myver.ini        */

            string str_myver = folder + "\\myver.ini";

            if (File.Exists(str_myver)) {

                string version_str;
                using (var sr = new StreamReader(str_myver)) {

                    sr.ReadLine();
                    version_str = sr.ReadLine();
                }

                version_str = version_str.Replace("VERSION=", "");

                int tmp;
                if (int.TryParse(version_str, out tmp)) {
                    ver = tmp - 1994;
                    return true;
                }

            }

            return false;
        }



        static string
        Get_Sage_Common_Path() {

            var folder = Environment.SpecialFolder.CommonApplicationData;
            var common_data = Environment.GetFolderPath(folder);
            var ret = common_data.Cpath("Sage\\Accounts");

            return ret;
        }



        /*       Company data        */

        public static bool
        Get_Name_Period_Control(Sage_Access access,
                                out string name,
                                out string period,
                                out string bank_control) {
            bool ret = false;
            H.assign(out name, out period, out bank_control);

            try {

                using (access.Establish_Connection())
                    foreach (var _ in new[] { 0 }) {

                        var ws = access.WS;
                        var sd = ws.Create<SetupData>();

                        name = sd["NAME"].strdef();

                        if (access.Version >= 14) {

                            //TODO: Seems to work...
                            var temp = UnicodeEncoding.Unicode.GetBytes(name);

                            for (int ii = 0; ii < temp.Length; ii += 2) {

                                if (temp[ii + 1] == 0)
                                    continue;

                                temp[ii] += 128;
                                temp[ii + 1] -= 1;

                            }

                            name = UnicodeEncoding.Unicode.GetString(temp);
                        }

                        var pair = M.Get_Financial_Period(ws);

                        period = M.Format_Financial_Period(pair, true);

                        var nominal = ws.Create<Versioning.NominalRecord>();

                        if (nominal.FindControlAccount(Versioning.ControlTypes.sdoNtBank)) {

                            // nominal.MajorControl.tiff();

                            bank_control = nominal["ACCOUNT_REF"].strdef();

                        }

                        ret = true;

                    }

            }
            catch (XSage_Conn ex) { // TEMP
                Logging.Notify(ex);

                var trouble = ex.Message.ToUpper();

                if (trouble.Contains("EXCLUSIVE MODE")) {
                    Named_Message.X_is_unable_to_verify_your_login_data.Show();

                }
                else {
                    Named_Message.An_Unexpected_Error.Show();

                }

                ret = false;

            }


            return ret;
        }



        public static bool
        Get_Period(string path, string user, string pwd, int ver,
                   out string period) {
            XSage_Conn _;
            return Get_Period(path, user, pwd, ver, true, out period, out _);
        }



        public static bool
        Get_Period(string path, string user, string pwd, int ver, bool Show,
                   out string period,
                   out XSage_Conn exception) {

            period = null;
            exception = null;
            bool ret = false;


            try {
                var sdo = new SDO_Engine(ver);

                var WS = sdo.WSAdd();
                try {
                    WS.Connect(path, user, pwd);

                    var pair = M.Get_Financial_Period(WS);

                    period = M.Format_Financial_Period(pair, true);

                }
                finally {
                    WS.Try_Disconnect();
                }

                ret = true;

            }
            catch (XSage_Conn ex) {

                Logging.Notify(ex);

                if (Show)
                    Named_Message.An_Unexpected_Error.Show();

                exception = ex;
                ret = false;

            }


            return ret;
        }


        /*       Preparations for installation        */

        /*       Such procedures are better written as pairs: one that does the actual work,        */
        /*       and one which wraps it, handling its result        */


        /* The things that need to be done in order for the installation
         * to succeed:
         * 
         * 1) Ensure the VC++ dependency is covered
         * 2) Ensure the necessary SDO dlls are in place and regsvr32'd
         * 3) Ensure SDO itself is registered (3rd party integration is in place
         */

        static void
        Create_Temp_Dir() {
            var dir_info = Directory.CreateDirectory(temp_dir);

            dir_info.Attributes = FileAttributes.Hidden | FileAttributes.Directory;
        }

        public static bool
        Delete_Temp_Dir() {

            var ret = false;

            if (Directory.Exists(temp_dir)) {

                try {

                    Directory.Delete(temp_dir, true);
                    ret = true;

                }
                catch (IOException ex) {
                    Logging.Notify(ex);

                    ret = false;
                }

            }

            return ret;

        }

        // ****************************

        static void
        Check_Registry(int ver, out bool need_sdo_dll, out bool need_vcred) {

            //If we need the VC++ Redistributable
            need_vcred = !has_vcred[ver] && !VCRed_Registry_Keys_OK(ver);

            if (!need_vcred)
                has_vcred[ver] = true;

            //If we need the SDO dlls
            need_sdo_dll = !has_sdo[ver] && !SDO_Registry_Keys_OK(ver);

            if (!need_sdo_dll)
                has_sdo[ver] = true;

        }

        static bool
        VCRed_Registry_Keys_OK(int ver) {

            var ret = false;

            using (D.func("VCRed_Registry_Keys_OK",
                        () => ret)) {

                if (ver < 13)
                    return ret = true;

                ret = H.Has_VCRed();

            }

            return ret;
        }

        static bool
        SDO_Registry_Keys_OK(int ver) {

            bool ret = false;

            using (D.func("SDO_Dlls_OK", () => ret)) {

                // oleview -> coclass SDOEngine
                var clsids = new Dictionary<int, string>
                    {{11, @"CLSID\{E73A6738-8DA1-4E20-B3F4-F314E3A11883}"},
                     {12, @"CLSID\{C38F8CC3-7C66-49FF-897F-A572FB87231E}"},
                     {13, @"CLSID\{169D0186-5DE7-4DA6-82B1-C9AEB9BE6660}"},
                     {14, @"CLSID\{BBC68D84-1620-4202-A506-EEC43ED4EA9B}"},
                     {15, @"CLSID\{A29354FB-35FA-4CD4-8261-DAD864982348}"},
                     {16, @"CLSID\{81F4150A-2F39-44B8-A4DC-666CD0C1FB67}"},
                     {17, @"CLSID\{0C42A671-A0E4-4326-8AC8-7D451A89F1BC}"},

                    };

                try {

                    var reg = Registry.ClassesRoot;

                    reg = reg.OpenSubKey(clsids[ver]);

                    reg = reg.OpenSubKey("InProcServer32");

                    var tmp = reg.GetValue("", "").strdef("");

                    ret = !tmp.IsNullOrEmpty();

                    D.wl("Registry Key is " + reg);
                    D.wl("Value is " + tmp);

                }
                catch (NullReferenceException ex) {
                    Logging.Notify(ex);

                    //Usually means the key does not exist; 
                    ////i.e. the sdo dlls are not registered
                    ret = false;
                }

            }

            return ret;
        }

        // ****************************


        /// <summary>
        /// Conditionally copies the required SDO dlls to the 'temp' directory
        /// Conditionally extracts the VC++ redist. file to the 'temp' dir.
        /// </summary>
        public static void
        Extract_Files_To_Temp(int ver) {

            if (extracted[ver])
                return;

            using (D.func("Extract_Dlls")) {

                // ..\[sge.dta]\v13.dta --> ..\temp\v13.dta
                // ..\temp\[v13.dta]\*  --> ..\temp\v13\*

                Create_Temp_Dir();

                Extract_SDO_Dlls(ver);

                bool _, need_vcred;

                Check_Registry(ver, out _, out need_vcred);

                if (need_vcred)
                    Extract_VCRed();

                extracted[ver] = true;

            }
        }

        static void Extract_SDO_Dlls(int ver) {

            var v13 = "v" + ver; // v13

            // arc entry
            var v13DTA = v13 + ".dta";         // v13.dta

            using (var zip = new ZipFile(sgeDTA)) {

                ZipEntry e = zip[v13DTA];

                e.ExtractWithPassword(temp_dir, ExtractExistingFileAction.OverwriteSilently, pass);

            }


            // source arc
            var temp_v13DTA = temp_dir.Cpath(v13DTA); // ../temp/v13.dta

            using (var zip = new ZipFile(temp_v13DTA))
                zip.ExtractAll(temp_dir, ExtractExistingFileAction.OverwriteSilently);

        }
        static void
        Extract_VCRed() {

            if (!File.Exists(vcrEXE)) {

                using (var zip = new ZipFile(sgeDTA)) {

                    ZipEntry e = zip[vcrEXE];

                    e.ExtractWithPassword(temp_dir, ExtractExistingFileAction.OverwriteSilently, pass);

                }
            }

            var vcr_exe = temp_dir.Cpath(vcredist5EXE); // temp\vcredist5.exe

            if (!File.Exists(vcr_exe)) {

                string temp_vcrDTA = temp_dir.Cpath(vcrEXE); // temp\vcr.dta

                using (ZipFile zip = new ZipFile(temp_vcrDTA)) {

                    string entry = vcr + "\\" + vcredist5EXE; // vcr\vcredist5.exe
                    zip[entry].Extract(temp_dir, ExtractExistingFileAction.OverwriteSilently);

                }
            }

        }


        // ****************************

        static bool
        Install_VCRed(string vcred_path) {

            var vcredist = new Process();


            vcredist.StartInfo.FileName = vcred_path;
            vcredist.StartInfo.UseShellExecute = false;
            vcredist.StartInfo.Arguments = "/q:a /c:\"msiexec /i vcredist.msi /qn\"";
            vcredist.Start();
            vcredist.WaitForExit();

            return true;

        }

        static void
        Copy_SDO_Dlls_To_Sysdir(string sysdir, string dir, string[] files) {

            foreach (string file in files) {

                var from = dir.Cpath(file);

                var to = sysdir.Cpath(file);

                if (File.Exists(to))
                    continue;

                File.Copy(from, to, false); //TODO: Overwrite?

            }

        }



        // ****************************

        public static SDO_Activation_Status
        Get_SDO_Activation_Status(int ver, string path, string user, string pass) {

            var ret = SDO_Activation_Status.Unknown_Error;

            using (D.func("Get_SDO_Activation_Status",
                          () => ret)) {


                Action<Exception, SDO_Activation_Status?> handle = (ex, res) =>
                {
                    Logging.Notify(ex);

                    D.wl(ex);


                    if (res.HasValue) {
                        ret = res.Value;
                    }
                    else {

                        ret = SDO_Activation_Status.Unknown_Error;

                        var trouble = ex.Message.strdef().ToUpper();

                        var dict = new Dictionary<string, SDO_Activation_Status>
                        {
                            {"NOT REGISTERED", SDO_Activation_Status.Not_Activated},

                            // ex is COMException || ex is SageConnectionException
                            {"RETRIEVING THE COM CLASS FACTORY", SDO_Activation_Status.COM_Exception},

                            // TOCHECK
                            //{"THAT OTHER MEMORY IS CORRUPT", SDO_Activation_Status.Not_Activated},

                            // the error we're getting when the user cancels Sage registration
                            {"USER COUNT EXCEEDED", SDO_Activation_Status.Sage_Not_Registered_User_Refuses},

                        };


                        foreach (var kvp in dict) {
                            if (trouble.Contains(kvp.Key)) {
                                ret = kvp.Value;
                                break;
                            }
                        }

                    }

                    D.wl("Handler: " + ret);
                };


                try {

                    var sdo = new SDO_Engine(ver);

                    var is_registered = false;

                    // SDO docs:
                    // "The SDO and Application can run for 30 days or 
                    // 30 uses before registration is required."

                    for (int ii = 0; ii < 31; ++ii) {

                        is_registered = sdo.IsRegistered;

                        if (!is_registered)
                            break;

                        Work_Space ws = null;
                        try {
                            ws = sdo.WSAdd();
                            ws.Connect(path, user, pass);

                            // Prompt for Sage Password?
                        }
                        finally {
                            ws.Try_Disconnect();
                        }
                    }

                    ret = is_registered ?
                          SDO_Activation_Status.OK :
                          SDO_Activation_Status.Not_Activated;

                }
                catch (NullReferenceException ex) { // <-- This is what we get on version 14 etc if SDO is not present
                    handle(ex, ver >= 14 ?
                               SDO_Activation_Status.Not_Activated :
                               (SDO_Activation_Status?)null);
                }
                catch (COMException ex) {
                    handle(ex, null);
                }
                catch (XSage_Conn ex) {
                    handle(ex, null);
                }

            }

            return ret;
        }

        public static User_Credentials_Status
        Check_User_Creds(int ver, string path, string user, string pwd) {

            User_Credentials_Status ret = 0;

            using (D.func("Check_User_Creds",
                           () => ret)) {

                try {

                    var sdo = new SDO_Engine(ver);

                    var ws = sdo.WSAdd();

                    var valid_user = ws.IsValidUser(path, user, pwd);

                    ret = valid_user ?
                          User_Credentials_Status.Valid_User :
                          User_Credentials_Status.Incorrect_Credentials;

                }
                catch (Exception ex) {
                    if (!(ex is COMException || ex is XSage_Conn))
                        throw;

                    Logging.Notify(ex);

                    D.wl(ex);

                    string trouble = ex.Message.ToUpper();

                    if (trouble.StartsWith(INVALID_OR_INCORRECT))
                        ret = User_Credentials_Status.Incorrect_Credentials;
                    else
                        ret = User_Credentials_Status.Unknown_Error;

                }
            }
            return ret;

        }



        /// <summary> regsvr_result is filled with -1 if regsvr32 is not ran
        /// </summary>
        public static SDO_Installation_Status
        Install_SDO(Form form,
                    int ver,
                    ref bool ask_for_install,
                    out int regsvr_result1,
                    out int regsvr_result2) {

            regsvr_result1 = -1;
            regsvr_result2 = -1;

            var ret = SDO_Installation_Status.Unknown_Error;
            using (D.func("Install_SDO",
                         () => ret))
                try {

                    if (ver < min_ver || ver > max_ver)
                        return ret = SDO_Installation_Status.Wrong_Version;

                    bool install_sdo, install_vcred;

                    Check_Registry(ver, out install_sdo, out install_vcred);

                    //if (!install_sdo) {

                    //    D.wl("Finished with InstallSdo, nothing to do");

                    //    return ret = SDO_Install_Result.All_OK;

                    //}

                    if (!H.Is_User_Admin()) {

                        Show_Admin_Error();

                        return ret = SDO_Installation_Status.Access_Error;
                    }

                    if (install_sdo && ask_for_install) {

                        ask_for_install = false;
                        var show_msg = Named_Message.Sage_Line_50_Is_Not_Installed.Show();

                        if (show_msg != DialogResult.OK)
                            return ret = SDO_Installation_Status.User_Clicked_NO;

                    }

                    using (var wform = new Common.Dialogs.Wait_Form()) {

                        wform.Show(form);
                        wform.Refresh();

                        if (install_vcred)
                            Activation_Helpers.Install_VCRed(vcr_exe_path());

                        if (install_sdo) {

                            Activation_Helpers.Copy_SDO_Dlls_To_Sysdir(sysdir, sdo_dlls_path(ver), files[ver]);

                        } // <-- this clause used to extend to END

                        // register the sdo dlls anyway

                        var to_reg = to_register[ver];
                        int len = to_reg.Length;

                        // (len == 2).tiff();

                        int[] results = new int[len];

                        for (int ii = 0; ii < len; ++ii) {

                            results[ii] = H.Regsvr_32(to_reg[ii], true, true);
                            form.Invalidate();

                        }

                        regsvr_result1 = results[0];

                        regsvr_result2 = results[1];

                        // END

                        form.Invalidate();

                        return ret = SDO_Installation_Status.All_OK;
                    }

                }
                catch (SecurityException ex) {

                    Logging.Notify(ex);

                    string trouble = ex.Message.ToUpper();

                    var msg = "The registration of the required dll files has failed.\n"
                         + "This could be a result of the configuration of your antivirus,\n"
                         + "firewall or other security software.\n\n"
                         + "Please temporarily disable such software and click Retry.";

                    var dresult = Standard.Show(Message_Type.System_Error, msg, MessageBoxButtons.RetryCancel);

                    return ret = dresult == DialogResult.Retry ?
                                 SDO_Installation_Status.Try_Again :
                                 SDO_Installation_Status.Firewall;


                }
        }



        public static void Show_Admin_Error() {

            Standard.Show(Message_Type.Error,
                                                   "Unable to register required software components.\n\n"
                                                + "Please run the application again, using an account with\n"
                                                + "Administrative privileges.");

        }




        public static void
        Remove_sdoeng_usr(int version) {

            string sdoengusr;

            if (version <= 13) {

                sdoengusr = H.Get_Win_Dir().Cpath("sdoeng.usr");

            }
            else {

                string progdir = Get_Sage_Common_Path();
                string ver_str = (version + cst_ver_0_year).ToString();

                sdoengusr = progdir.Cpath(ver_str, "sdoeng.usr");
            }

            if (File.Exists(sdoengusr)) {

                if (cst_debug)
                    Console.WriteLine("Deleting " + sdoengusr);

                File.Delete(sdoengusr);
            }

        }

        public static void
        Unregister_sgregister_dll(int version) {

            if (cst_debug)
                Console.WriteLine("Unregistering the sdodll for version {0}".spf(version));

            H.Regsvr_32("regsvr32", false, true);

        }

        public static void
        Unregister_sdoeng_dll(int version) {

            if (cst_debug)
                Console.WriteLine("Unregistering the sdodll for version {0}".spf(version));

            foreach (var file in to_unregister[version])
                H.Regsvr_32(file, false, true);

        }


    }
}
