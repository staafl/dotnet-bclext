using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Common;
using Versioning;

using Standardization;
using Common.Posting;
using Fairweather.Service;
using D = System.Collections.Generic.Dictionary<string, string>;
using Line = Fairweather.Service.Pair<Common.Posting.Data_Line, Sage_Int.Line_Data>;

namespace Sage_Int
{
    // 21 csv\jcjd.csv 0001 screens screens
    public partial class SIT_Engine
    {
        public SIT_Engine(I_SIT_Exe sit_exe) {
            this.sit_exe = sit_exe;
            sit_exe.Prepare(this);
            now = DateTime.Now;
            today = now.Date;
        }

        public event Action Initial_Setup;
        public event Action<Quad<string, string, string, bool>> Scan_Started;
        public event Action<Quad<Sit_General_Settings, bool, string, string>> Scan_Failed;
        public event Action Interface_Started;
        public event Action<Triple<int>, string> Interface_Over;
        public event Action Connecting;
        public event Action Connected;
        public event Action Disconnecting;
        public event Action Scan_Only_Success;
        public event Action Byed;

        readonly DateTime now, today;
        readonly I_SIT_Exe sit_exe;



        public const string STR_0000 = "0000";

        static public int?
        Try_Start_SIT(
            Func<I_SIT_Exe> sit_exe,
            out SIT_Engine engine,
            out Logging logger,
            out Ini_File ini,
            out Sit_General_Settings sett_global) {

            ini = null;
            sett_global = null;



            logger = M.Get_Logger(true, false);

            M.Set_Culture();
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            engine = new SIT_Engine(sit_exe());

            engine.Exception += _ex => Logging.Notify(_ex);

            var null_rc = engine.Initial_Check();
            if (null_rc != null)
                return null_rc;

            if (!M.Get_Ini_File(true, false, out ini)) {

                M.Run_Sescfg(false);

                return (int)Return_Code.Not_Registered;

            }

            sett_global = new Sit_General_Settings(ini);

            return null;
        }


        public int
        Run(Sit_General_Settings sett_global,
            Sit_Company_Settings sett_company,
            Record_Type mode,
            bool scan_only,
            Credentials creds,
            string file) {


            try {
                int? return_code;

                // this here because of 'sett_company' and 'mode'
                if (!Validate(out return_code, sett_global, sett_company, mode)) {

                    M.Run_Sescfg(false);

                    return_code = return_code ?? (int)Return_Code.Unknown_Error;

                    return return_code.Value;


                }

                // * read settings
                // * handle any of the special program modes:
                //     - help
                //     - registration key



                /************* ******* ****************/

                // stuff we should have by now:
                // * invoked program mode
                // * company number
                // * username
                // * password
                // * used program switches

                if (!Directory.Exists(Data.STR_Sit_History_Dir.Path))
                    Directory.CreateDirectory(Data.STR_Sit_History_Dir.Path);

                if (!Connect(sett_global, sett_company, creds, out sdo, out ws)) {
                    return Data.CONNECTION_ERROR;
                }

                var context = new Sage_Context(ws);
                var start = DateTime.Now;

                // make sure nobody else touches the file
                // until we're done with it
                FileStream __;
                if (!Open_File(file, out __)) {
                    return Data.OTHER_ERROR;
                }

                using (__) {
                    try {

                        if (!Check_Empty_File(file, sett_company.Has_Headers == true)) {

                            Warn(@"Empty files are not allowed.");

                            return Data.LOGIC_ERROR;

                        }


                        string[] headers;
                        var lines = Get_Data_Lines(sett_company, file, mode, out headers);

                        Dictionary<string, object> defaults;
                        Get_Defaults(mode, out defaults);

                        // Phase 2:
                        // * verify csv structure
                        // * verify records consistency
                        Phase_2(out return_code,
                                context,
                                sett_global, sett_company,
                                creds,
                                mode, scan_only,
                                defaults,
                                file,
                                headers,
                                lines);

                        if (return_code != null) {
                            return return_code.Value;
                        }

                        // Phase 3:
                        // * insert records

                        Insert(out return_code,
                               context,
                               sett_company,
                               creds,
                               mode,
                               defaults,
                               headers,
                               lines);

                    }
                    finally {

                        Try_Disconnect();

                        // debugging
                        Console.WriteLine((DateTime.Now - start));

                        Bye();

                    }
                }

                return return_code.GetValueOrDefault(0);
            }
            catch (Exception ex) {

                return Handle_Exception(ex);

            }
            finally {

                Bye();

            }

        }

        public int? Initial_Check() {
            string msg;
            if (!M.Check_For_Files(false, out msg)) {
                Warn(msg +
@"
Program cannot continue.");
                return Data.LOGIC_ERROR;
            }

            if (!Check_Instance())
                return Data.LOGIC_ERROR;

            return null;
        }

        /// <summary>
        /// when you need to parse some arguments before interface
        /// </summary>
        public int
        Run(string[] args, Ini_File ini, Sit_General_Settings sett_global) {

            //#if bench
            //Benchmark();
            //return 0;
            //#endif

            try {
                int? return_code;
                List<string> arguments;
                Set<string> switches;

                Initial_Setup.Raise();

                // 21 20090730.CSV 0001 USER PASSWORD

                Sift_Args(args, out arguments, out switches);

                Handle_Switches(out return_code, arguments, switches, sett_global);

                if (return_code != null)
                    return return_code.Value;


                /************* SETTINGS ****************/

                if (switches["/?"]) {

                    sit_exe.Help(ini, sett_global);
                    return Data.ALL_OK;

                }

                Sit_Company_Settings sett_company;
                Record_Type mode;
                bool scan_only;
                Credentials creds;
                string file;

                /* sadly, we need to know the company number to know
                 * exactly what the arguments mean */
                Read_Arguments(
                    out return_code,
                    arguments,

                    ini,
                    sett_global,

                    out sett_company,
                    out mode,
                    out scan_only,
                    out creds,
                    out file);

                if (return_code != null)
                    return return_code.Value;

                (arguments.Count >= 2).tiff();

                return Run(sett_global, sett_company, mode, scan_only, creds, file);


            }
            catch (Exception ex) {

                return Handle_Exception(ex);

            }
            finally {

                Bye();

            }
        }

        /// <summary>
        /// Loads the default new account settings from Sage's database into _defaults[].
        /// </summary>
        /// <param name="a">1 means load Sales defaults, 2 means Purchase defaults</param>
        /// <returns>True on success</returns>
        bool
        Get_Defaults(Record_Type mode, out Dictionary<string, object> defaults) {

            defaults = new Dictionary<string, object>();

            Dictionary<string, string> pairs;

            if (mode == Record_Type.Sales)
                pairs = new Dictionary<string, string>
{ 
{"COUNTRY_CODE","CUSTOMER_COUNTRY_CODE"}, 
{"CURRENCY","SALES_CURRENCY"}, 
{"DEPT_NUMBER","SALES_DEPARTMENT"}, 
{"DISCOUNT_TYPE","SALES_DISC_TYPE"}, 
{"SETTLEMENT_DUE_DAYS","SALES_DUEDAYS"}, 
{"DEF_NOM_CODE","SALES_NOMINAL"}, 
{"PAYMENT_DUE_DAYS","SALES_PAY_DUE_DAYS"}, 
{"TERMS","SALES_TERMS"}
//{"DEF_TAX_CODE","SALES_DEF_TAX_CODE"},*/ 
//{,"SALES_DEF_CALLRATE",""},
};
            else if (mode == Record_Type.Purchase)
                pairs = new Dictionary<string, string>
{ 
{"COUNTRY_CODE","SUPPLIER_COUNTRY_CODE"},
{"CURRENCY","PURCHASE_CURRENCY"},
{"CREDIT_LIMIT","PURCHASE_CREDIT_LIMIT"},
{"DEPT_NUMBER","PURCHASE_DEPARTMENT"},
{"DEF_NOM_CODE","PURCHASE_NOMINAL"},
{"SETTLEMENT_DISC_RATE","PURCHASE_SETT_DISCOUNT"},
{"DEF_TAX_CODE","PURCHASE_TAXCODE"},
{"TERMS","PURCHASE_TERMS" }
//{"","DEF_PUR_ADDRESS"},
};
            else
                return true;

            var sd = ws.Create<SetupData>();

            bool ignored;
            if (!Try(() => sd.Open(OpenMode.sdoRead),
            "Unable to read details from Sage",
            out ignored))
                return false;

            try {
                defaults.Fill(pairs.Transform_Values(_s => sd[_s]), false);
            }
            finally {
                sd.Close();
            }

            return true;
        }


        // ****************************

        bool
        Sift_Args(
            string[] argv,
            out List<string> args,
            out Set<string> switches) {

            //var parser = new Command_Line_Parser(
            //                true,
            //                true,
            //                false,
            //                '/',
            //                '=');
            //parser.Parse(
            // args_in.Select(_a => _a.ToUpper()),
            // out t_args,
            // out t_sws);

            args = new List<string>();
            switches = new Set<string>();

            for (int ii = 0; ii < argv.Length; ii++) {

                var arg = argv[ii];

                arg = arg.ToUpper();
                arg = arg.Replace("\"", "");

                if (arg[0] == '/') {

                    switches[arg] = true;

                    continue;
                }

                if (arg == "?") {

                    switches["/?"] = true;
                    continue;

                }

                args.Add(arg);

            }

            return true;
        }

        bool Check_VCRed(Sit_General_Settings sett_global) {

            if (sett_global.Version <= 12)
                return true;

            if (H.Has_VCRed())
                return true;

            Warn(
@"Microsoft Visual C++ Redistributable package is required,
but has been uninstalled.

Prior to using this software, you will have to run 'SITCFG.exe' in order to install it.");

            return false;

        }


        bool Check_Instance() {

            var ret = !H.Other_Instances().Any();

            // string prod_id = "InfoTrends.Interface_Tools";

            // H.Ensure_Single_Instance(prod_id, false, out mut);

            if (!ret) {

                Warn(
@"Another instance of Sage Interface Tools is running.

Program cannot continue.");

            }

            return ret;
        }


        // ****************************


        void
        Handle_Switches(out int? return_code,
                        List<string> arguments,
                        Set<string> switches,
                        Sit_General_Settings sett_global) {

            H.assign(out return_code);
            int cnt = arguments.Count;

            if (cnt == 0) {
                if (!switches["/R"] && !switches["/?"]) {

                    Warn(@"Invalid set of parameters.");
                    Show_Syntax(sett_global);

                    return_code = Data.LOGIC_ERROR;
                    return;
                }
            }

            if (cnt <= 1 && !switches["/?"]) {

                Warn(@"Invalid set of parameters.");
                Show_Syntax(sett_global);

                return_code = Data.LOGIC_ERROR;

                return;
            }


            if (switches["/R"]) {

                string req = Activation.Activation_Data.Generate_Request(true);

                using (var sw = new StreamWriter("RequestCode.txt"))
                    sw.Write(req);

                return_code = Data.ALL_OK;

                return;

            }

            if (switches["/S"]) { // silent mode

                var p = Process.GetCurrentProcess();
                var hWnd = p.MainWindowHandle;

                if (hWnd != IntPtr.Zero)
                    Native_Methods.ShowWindow(hWnd, 0);

                Console.SetOut(new StringWriter());
            }

            return_code = null;

        }



        string
        Get_Long_Description(Record_Type mode, bool scan_only) {
            var ret = "Type of Interface: {0} - {1}".spf(Get_Module_String(mode, scan_only).ToLower(), mode_descriptions[mode]);

            if (scan_only)
                ret += ", scan only";

            return ret;
        }






        // ****************************

        void
        Read_Arguments(
            out int? return_code,
            List<string> arguments,
            Ini_File ini,
            Sit_General_Settings sett_global,

            out Sit_Company_Settings sett_company,
            out Record_Type mode,
            out bool scan_only,
            out Credentials creds,
            out string file) {

            H.assign(out return_code, out mode, out scan_only);
            H.assign(out sett_company, out creds, out file);


            int cnt = arguments.Count;

            if (cnt < 2) {
                // too few arguments
                Warn(@"Too few parameters provided.");
                Show_Syntax(sett_global);
                return_code = Data.WRONG_PARAMETERS;
                return;
            }

            if (cnt == 4) {
                // wrong number of arguments
                Warn(@"Wrong number of arguments.");
                Show_Syntax(sett_global);
                return_code = Data.WRONG_PARAMETERS;
                return;

            }

            if (cnt > 5) {
                // too many arguments
                Warn(@"Too many parameters provided.");
                Show_Syntax(sett_global);
                return_code = Data.WRONG_PARAMETERS;
                return;
            }

            if (sett_global.CommandLineCredentials == true && cnt < 5) {

                // no username/password
                Show_Credentials_Message(false);
                return_code = Data.WRONG_PARAMETERS;
                return;

            }

            (arguments.Count >= 2).tiff();

            if (!Get_Module(arguments, sett_global, out mode, out scan_only)) {
                return_code = Data.WRONG_PARAMETERS;
                return;

            }

            Get_Company_Config(
                out return_code,
                arguments,
                ini,
                sett_global,
                mode,

                out creds,
                out sett_company);

            if (return_code != null)
                return;

            if (!Get_File(arguments, out file)) {
                return_code = Data.LOGIC_ERROR;
                return;

            }

        }

        void
        Get_Company_Config(
            out int? return_code,
            List<string> arguments,
            Ini_File ini,
            Sit_General_Settings sett_global,
            Record_Type mode,
            out Credentials creds,
            out Sit_Company_Settings sett_company) {

            Company_Number number;
            string username;
            string password;

            H.assign(out return_code, out creds, out sett_company, out number);

            return_code = null;

            int cnt = arguments.Count;

            if (sett_global.InternalCredentials) {
                (cnt == 2 || cnt == 3 || cnt == 5).tiff();
            }
            else {
                (cnt == 5).tiff();
            }

            string company_string;

            if (cnt == 2) {

                var def_company = sett_global.Default_Company;

                if (def_company == null) {
                    Warn(
@"There are no registered companies.

Please run sitcfg.exe and register a company,
in order to interface data.");

                    return_code = Data.LOGIC_ERROR;
                    return;
                }

                company_string = def_company.As_String;

            }
            else {

                company_string = arguments[2];


            }


            int company_int;
            var ok = int.TryParse(company_string, out company_int) && company_int > 0;

            if (ok) {
                number = new Company_Number(company_int);
            }
            else {

                Warn(
@"Company number '{0}' does not exist in the record.

Program cannot continue.", company_string);

                return_code = Data.LOGIC_ERROR;

                return;

            }

            sett_company = new Sit_Company_Settings(ini, number, mode);

            if (cnt == 5) {

                username = arguments[3];
                password = arguments[4];


            }
            else {
                username = sett_company.Username;
                password = sett_company.Password;

            }

            if (username.IsNullOrEmpty() ||
                password.IsNullOrEmpty()) {

                Show_Credentials_Message(sett_global.InternalCredentials);

                return_code = Data.LOGIC_ERROR;
                return;

            }

            return_code = null;

            creds = new Credentials(number, username, password);

        }

        bool
        Get_File(List<string> arguments, out string file) {

            file = Path.GetFullPath(arguments[1].Trim());

            if (!File.Exists(file)) {

                Warn(@"File '{0}' does not exist.", file);
                return false;

            }

            var name = Path.GetFileName(file);
            var ext = Path.GetExtension(file).ToUpper();

            if (ext != ".CSV") {

                // wl(extension);
                Warn(@"Invalid file type. '{0}' is not a CSV file.", name);
                return false;

            }

            return true;

        }

        string
        Get_Module_String(Record_Type mode, bool scan_only) {
            return sit_modes[mode] + (scan_only ? "V" : "");
        }

        bool
        Get_Module(List<string> arguments,
            Sit_General_Settings sett_global,
            out Record_Type mode,
            out bool scan_only) {

            H.assign(out mode, out scan_only);

            var mode_as_string = arguments[0];

            var match = mode_as_string.Match(@"^(\d+)(V?)$", RegexOptions.IgnoreCase);

            var ret = match.Success;

            int mode_as_int = 0;
            if (ret) {

                if (match.Groups[2].Value.ToUpper() == "V") {

                    scan_only = true;
                    mode_as_string = match.Groups[1].Value;

                }

                ret = int.TryParse(mode_as_string, out mode_as_int);

            }

            if (ret) {
                ret = sit_modes.TryGetValue(mode_as_int, out mode);
                if (!ret) {
                    Warn(@"Invalid module parameter: ""{0}"".", mode_as_int);
                    // Warn(@"For a list of valid module parameters, type SIT /? and choose option 1.");
                    Show_Syntax(sett_global);
                }

                if (sit_tbi_modes[mode]) {
                    Warn(@"Module ""{0}"" ({1}) is not yet implemented.", mode_as_int, mode_descriptions[mode]);

                }



            }

            return ret;

        }


        // ****************************

        bool
        Validate(
            out int? return_code,
            Sit_General_Settings sett_global,
            Sit_Company_Settings sett_company,
            Record_Type mode) {

            if (!Check_License(sett_global)) {
                M.Run_Sescfg(false);
                return_code = Data.LICENSE_ERROR;
                return false;

            }

            if (!Check_Companies(sett_global)) {

                return_code = Data.LOGIC_ERROR;

                return false;

            }



            if (!Check_Version(sett_global.Version)) {

                return_code = Data.LOGIC_ERROR;

                return false;

            }

            if (!Check_VCRed(sett_global)) {
                return_code = Data.LOGIC_ERROR;

                return false;

            }


            // Check _company_ settings
            if (!Check_Company_Settings(sett_company)) {

                return_code = Data.LOGIC_ERROR;

                return false;


            }


            if (!Check_Module_Enabled(sett_global, mode)) {

                return_code = Data.LICENSE_ERROR;

                return false;


            }

            return_code = null;

            return true;

        }

        bool Check_License(Sit_General_Settings sett_global) {

            var key = sett_global.Activation_Key;

            if (!Activation.Activation_Data.ValidateKey(key))
                return false;

            return true;

        }


        bool Check_Company_Settings(Sit_Company_Settings sett_company) {

            // old story

            /*
        foreach (var kvp in sett_company) {
            if (kvp.Value.IsNullOrEmpty()) {

                Warn(
@"Invalid {0} supplied.

Program cannot continue.", kvp.Key);

                return false;


        }
            }*/
            return true;
        }

        bool Check_Version(int ver) {

            if (ver >= Activation.Activation_Helpers.min_ver && ver <= Activation.Activation_Helpers.max_ver)
                return true;

            Warn(
@"Invalid Sage version, should be {0} to {1}.

Program cannot continue.".spf(Activation.Activation_Helpers.min_ver, Activation.Activation_Helpers.max_ver));

            return false;

        }


        bool Check_Module_Enabled(Sit_General_Settings sett_global, Record_Type mode) {

            if (record_modes[mode] && sett_global.Records_Module != true) {

                Warn(
@"License error.

Your license does not allow you to use the Records Interface.");

                return false;
            }

            if (trans_modes[mode] && sett_global.Trans_Module != true) {

                Warn(
@"License error.

Your license does not allow you to use the Transactions Interface.");

                return false;

            }

            if (docs_modes[mode] && sett_global.Docs_Module != true) {

                Warn(
@"License error.

Your license does not allow you to use the Documents Interface.");

                return false;

            }

            return true;

        }

        bool Check_Companies(Sit_General_Settings sett_global) {

            var companies = sett_global.Number_Of_Companies;

            (companies < -1).tift();

            if (companies <= 0) {

                Warn(
@"There are no registered companies.

Please run sitcfg.exe and register a company,
in order to interface data.");
                return false;

            }

            return true;
        }




        void
        Phase_2(out int? return_code,
                Sage_Context context,
                Sit_General_Settings sett_global,
                Sit_Company_Settings sett_company,
                Credentials creds,
                Record_Type mode,
                bool scan_only,
                Dictionary<string, object> defaults,
                string file,
                string[] headers,
                IEnumerable<Line> lines) {

            Clear();

            Scan_Started.Raise(Quad.Make(mode_descriptions[mode],
                   file,
                   sett_company.Company_Name,
                   scan_only));

            var scan_result = false;

            var errors_file_1 = Data.STR_Sit_Error_Log.Path;

            File.Delete(Data.STR_Sit_Success_Log.Path);
            File.Delete(Data.STR_Sit_Error_Log.Path);

            using (var sw = new StreamWriter(errors_file_1, false)) {

                Action<string> report = sw.WriteLine;

                var errors_file_2 = Get_Errors_Path(creds.Company);

                try {

                    sw.WriteLine("Validation of file: {0}".spf(file));

                    sw.WriteLine("Date: {0:dd-MM-yyyy} Time: {1:HH:mm:ss}".spf(today, now));

                    sw.WriteLine("Company Name: " + sett_company.Company_Name);
                    sw.WriteLine(Get_Long_Description(mode, scan_only));
                    sw.WriteLine("");

                    scan_result = Scan(context,
                                         sett_global, sett_company,
                                         creds, mode,
                                         defaults,
                                         file,
                                         headers,
                                         lines,
                                         report);





                    if (scan_result) {

                        sw.WriteLine("Validation of file completed successfully.");

                        if (!scan_only)
                            sw.WriteLine("Data entered in Sage 50 Accounts.");

                    }
                    else {
                        sw.WriteLine("Validation of file completed with errors.");
                        sw.WriteLine("No data entered in Sage 50 Accounts.");

                    }
                }
                finally {
                    sw.Try_Dispose();
                    Try_Copy_File(errors_file_1, errors_file_2);
                }
            }



            if (scan_result) {

                if (scan_only) {

                    //No insertion required

                    Scan_Only_Success.Raise();

                    Bye();

                    return_code = Data.ALL_OK;

                    return;

                }


            }
            else {

                Scan_Failed.Raise(Quad.Make(sett_global, scan_only, file, Data.STR_Sit_Error_Log.Path));

                Bye();

                return_code = Data.FILE_FAILED_TO_VALIDATE;
                return;


            }
            return_code = null;

        }


        bool
        Connect(Sit_General_Settings sett_global,
                Sit_Company_Settings sett_company,
                Credentials creds,
                out SDO_Engine sdo,
                out Work_Space ws) {

            sdo = new SDO_Engine(sett_global.Version);
            ws = sdo.WSAdd();

            Connecting.Raise();

            ws.Connect(
                sett_company.Company_Path,
                creds.Username,
                creds.Password,
                true);

            Connected.Raise();

            return true;



        }


        void
        Try_Disconnect() {
            var tmp_vWS = ws;
            if (tmp_vWS == null)
                return;
            if (tmp_vWS.Connected) {
                Disconnecting.Raise();
                tmp_vWS.Disconnect();
            }
        }




        /*       Misc        */


        void
        Show_Syntax(Sit_General_Settings sett_global) {

            var source = "Program is set to obtain credentials ";

            source += sett_global.InternalCredentials ? "from its data file." :
            "from the command prompt.";

            string syntax = "Syntax: sit <MODULE> <FILENAME> ";

            syntax += sett_global.InternalCredentials
            ? "<company>"
            : "<USERNAME> <PASSWORD> <company>";

            Warn(source);
            Warn(syntax);

        }


        void
        Show_Credentials_Message(bool from_dta) {
            Warn(@"Invalid set of credentials (Sage 50 Accounts Username and Password).");

            Warn(@"Program is set to obtain credentials from {0}.", from_dta ? "its data file" : "the command-line");
            Warn(@"No credentials {0}.",
            from_dta
            ? "for the specified company are on record"
            : "have been entered.");
            Warn(@"Use SITCFG.exe to:");
            Warn(@"1. Define credentials for the specific company, or");
            Warn(@"2. Set the program to obtain credentials from the command-lines");
        }


        public event Action<Exception> Exception;

        int
        Handle_Exception(Exception ex) {

            Exception.Raise(ex);

            int? ret = null;
            var as_sce = ex as XSage_Conn;

            if (as_sce != null) {
                switch (as_sce.Connection_Error) {
                    case Sage_Connection_Error.Exclusive_Mode: {
                            Warn(
@"Cannot login to Sage, as it is being used in exclusive mode.

Please wait until the program is released and try again.");
                            return Data.CONNECTION_ERROR;
                        }
                    case Sage_Connection_Error.Invalid_Credentials: {
                            Warn(
@"Invalid or incorrect username or password
                        
Please verify your authentication and try again.");
                            return Data.CONNECTION_ERROR;
                        }
                    case Sage_Connection_Error.Unsupported_Version:
                    case Sage_Connection_Error.Invalid_Version: {
                            Warn(
@"Version mismatch occurred.

Please verify that you are using the correct version of Sage.");
                            return Data.CONNECTION_ERROR;
                        }
                    case Sage_Connection_Error.Folder_Does_Not_Exist:
                    case Sage_Connection_Error.Invalid_Folder: {
                            Warn(
@"Invalid data path.

Please verify your settings and try again.");
                            return Data.DATAPATH_ERROR;
                        }
                    case Sage_Connection_Error.SDO_Not_Registered: {
                            Warn(
@"Third party integration not enabled in Sage.

Program cannot continue.");
                            return Data.CONNECTION_ERROR;
                        }
                    case Sage_Connection_Error.Logon_Count_Exceeded: {
                            Warn(
@"The program cannot log on to Sage at the moment.
                            
Logon count exceeded."
                            );
                            return Data.CONNECTION_ERROR;
                        }
                    case Sage_Connection_Error.Username_In_Use_Generic:
                    case Sage_Connection_Error.Username_In_Use_Cant_Remove:
                        Warn(
@"Cannot login to Sage, as the username provided is already in use.

Please verify your authentication or wait until the username is freed.");
                        return Data.CONNECTION_ERROR;
                    case Sage_Connection_Error.Unspecified:
                    default:
                        ret = Data.CONNECTION_ERROR;
                        break;
                }



            }

            Warn(
@"An unexpected error occurred: {0}

Please make sure that you have the correct version of Sage installed, and are using the appropriate settings.", ex.Msg_Or_Type());

            return ret ?? Data.OTHER_ERROR;

        }





        bool is_byed = false;

        void Bye() {
            if (H.Set(ref is_byed, true))
                return;

            Try_Disconnect();
            Byed.Raise();
        }

    }
}
