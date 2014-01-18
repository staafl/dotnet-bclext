using System;
using System.Collections.Generic;
using Fairweather.Service;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
namespace Common
{
    static partial class Data
    {

        public static Program_File_Info? Ini_Filename {
            get {
                if (Is_Excel_Helper)
                    return STR_XLDTA;

                if (Is_Entry_Screens_Suite)
                    return STR_SESDTA;

                if (Is_Interface_Tools_Suite)
                    return STR_SITDTA;

                return null;
            }
        }

        public const int MAX_LOG_SIZE_KB = 100;


        const string STR_Bak = ".bak";


        // ****************************


        /// "\Local" "\Global" - see http://msdn.microsoft.com/en-us/library/system.threading.mutex.aspx
        const string MUTEX_PREFIX = "421b02c3-0d64-4d0e-9762-27ce9f08f6fd.InfoTrends.SageEntryScreens.";


        const string MUTEX_NAME_SIT = MUTEX_PREFIX + "SageInt";
        const string MUTEX_NAME_SITGUI = MUTEX_PREFIX + "SageIntGui";
        const string MUTEX_NAME_SITCFG = MUTEX_PREFIX + "SageIntConfig";

        const string Mutex_ID_XL = MUTEX_PREFIX + "XL_Helper";
        const string MUTEX_NAME_CUSTOMER = MUTEX_PREFIX + "CustomerReceipts";
        const string MUTEX_NAME_SUPPLIER = MUTEX_PREFIX + "SupplierPayments";
        const string MUTEX_NAME_PRODUCTS = MUTEX_PREFIX + "Products";
        const string MUTEX_NAME_TRANSFER = MUTEX_PREFIX + "Transfer";
        const string MUTEX_NAME_TRANS = MUTEX_PREFIX + "TransactionsEntry";

        const string MUTEX_NAME_SESCFG = MUTEX_PREFIX + "SESCFG";
        public const string MUTEX_NAME_DASH = MUTEX_PREFIX + "DASHBOARD";





        // ****************************


        public static Program_File_Info[]
        Get_Program_Files() {

            if (Data.Is_Excel_Helper)
                return new[] {
                    STR_MicrosoftOfficeInteropExceldll,
                    STR_MicrosoftVbeInteropdll,
                    STR_Officedll,
                    STR_SDODLL,
                    STR_SERVICEDLL,};

            if (Data.Is_Interface_Tools_Suite)
                return new[] {   STR_SITEXE,
                                     STR_SITCFGEXE,
                                     STR_SERVICEDLL, // <--
                                     STR_SITDTA,
                                     //STR_NETDTA,
                                     STR_SGEDTA,
                                     STR_SDODLL,
                                     STR_ARCDLL };

            if (Data.Is_Entry_Screens_Suite)
                return new[] { STR_SESEXE, 
                                   STR_SESCFGEXE, 
                                   STR_SERVICEDLL,
                                   STR_PRINTDLL,
                                   STR_ARCDLL,
                                   STR_SAMDLL,
                                   STR_SDODLL,
                                   //STR_NETDTA,
                                   STR_SGEDTA };

            return new Program_File_Info[0];
        }

        public static Program_File_Info[]
        Get_Versioned_Files() {

            if (Data.Is_Interface_Tools_Suite)
                return new[] {  STR_SITEXE, 
                                STR_SITGUIEXE,
                                STR_SITCFGEXE,
                                STR_SERVICEDLL,
                                // STR_CSVDLL, 
                                STR_SDODLL,
                                // STR_CODEGENEXE,
                                // STR_CODEGENGUIEXE,
                                // STR_CRYPTEXE,
                                // STR_SITDTA,
                                STR_SGEDTA
                };

            if (Data.Is_Entry_Screens_Suite)
                return new[] { STR_SESEXE, 
                               STR_SESCFGEXE, 
                               STR_SERVICEDLL,
                               STR_PRINTDLL,
                               STR_SAMDLL,
                               STR_SDODLL,
                               STR_SGEDTA };

            return new Program_File_Info[0];
        }

        public static Dictionary<Program_File_Info, string>
        Get_Custom_Versions() {

            return new Dictionary<Program_File_Info, string>
                {
                    {STR_SDODLL, "1.0.0.101"},
                    {STR_SAMDLL, "1.0.0.0"},
                    {STR_SGEDTA, "1.0.0.1"},

                };

        }


        public static string App_Name {
            get {
                if (Is_Win7update)
                    return "Update Utility";

                if (Is_Entry_Screens)
                    return "Sage Entry Screens";

                if (Is_Ses_Cfg)
                    return "Sage Entry Screens";

                if (Is_Interface_Tools_Suite)
                    return "Sage Interface Tools";

                if (Is_Excel_Helper)
                    return "Excel To Sage";// "Excel Invoice Helper";

                if (Is_Doc_Trans)
                    return "Sage Documents Transfer";

                if (Is_Sage_To_Excel)
                    return "Sage To Excel";


                return H.Get_Asm_Title();
            }
        }


        public static string Default_Machine_Name {
            get {
                return App_Name;
            }
        }

        public static string Workspace {
            get {
                return App_Name;
            }
        }

        static public Credentials Default_Creds {
            get;
            set;
        }



        public static string Mutex_ID {
            get {

                if (Is_Excel_Helper)
                    return Mutex_ID_XL;

                return new Dictionary<App, Func<string>>
                          {
                                {App.Entry_Screens, Mutex_ID_SES},
                                {App.Ses_Cfg, Mutex_ID_SES},
                                {App.Sit_Cfg, Mutex_ID_SIT},
                          }[Data.App]();
            }
        }


        static string Mutex_ID_SES() {
            return new Dictionary<Sub_App, string>
                               {
                                     {Sub_App.Products, MUTEX_NAME_PRODUCTS},
                                     {Sub_App.Entry_Customers, MUTEX_NAME_CUSTOMER},
                                     {Sub_App.Entry_Suppliers, MUTEX_NAME_SUPPLIER},
                                     {Sub_App.Documents_Transfer, MUTEX_NAME_TRANSFER},
                                     {Sub_App.Ses_Cfg, MUTEX_NAME_SESCFG},
                                     {Sub_App.Transactions_Entry, MUTEX_NAME_TRANS},
                                     {Sub_App.Dashboard, MUTEX_NAME_DASH},
                               }[Data.Sub_App];
        }

        static string Mutex_ID_SIT() {
            return new Dictionary<Sub_App, string>
                               {
                                     {Sub_App.Sit_Exe, MUTEX_NAME_SIT},
                                     {Sub_App.Sit_Cfg, MUTEX_NAME_SITCFG},
                                     {Sub_App.Sit_Gui, MUTEX_NAME_SITGUI},
                               }[Data.Sub_App];
        }


        public static Program_File_Info Errorlog {
            get {


                if (Is_Sage_To_Excel)
                    return STR_S2xllog;

                if (Is_Excel_Helper)
                    return STR_Xllog;

                if (Is_Ses_Cfg)
                    return STR_Cfglog;


                Dictionary<Sub_App, Program_File_Info> dict = null;

                if (Is_Entry_Screens) {

                    dict = new Dictionary<Sub_App, Program_File_Info>
                        {
                          {Sub_App.Products, STR_Productslog},
                          {Sub_App.Entry_Customers, STR_Entrylog},
                          {Sub_App.Entry_Suppliers, STR_Entrylog},
                          {Sub_App.Documents_Transfer, STR_Transferlog},
                          {Sub_App.Dashboard, STR_Dashboardlog},
                          {Sub_App.Transactions_Entry, STR_Translog},
                          {Sub_App.Startup, STR_Startuplog},
                        };
                }
                else if (Is_Interface_Tools_Suite) {
                    dict = null; // see below
                }

                Program_File_Info ret;
                if (dict != null && dict.TryGetValue(Data.Sub_App, out ret)) {
                    return ret;

                }
                else // below
                {
                    var exe_name = (Get_App_Short_Name() + ".log").ToLower();
                    return new Program_File_Info(exe_name, def_subfolder, ProgramData, false);

                }

            }
        }

        public static string Errorlog_bak {
            get {
                return Path.GetFileNameWithoutExtension(Errorlog.Path) + STR_Bak;
            }
        }



        public static string Get_Company_Directory(Company_Number number) {
            //?? 
            var ret = Data.AppDataFolder.Cpath(number.As_String);
            return ret;
        }

        public static string Get_App_Short_Name() {
            string sub;

            if (Data.Is_Interface_Tools_Suite &&
                Data.App != App.Sage_To_Excel) {
                sub = "sit";

            }
            else if (Data.Is_Entry_Screens_Suite)
                sub = "ses";
            else
                sub = H.Get_Exe_Name();

            return sub;
        }

        public static string Get_App_Data_Subfolder() {

            var ret = "infotrends";

            var sub = Get_App_Short_Name();

            ret = ret.Cpath(sub);

            return ret;

        }

        public static Lazy<string>
        Get_Quick_Tabs_Dir(Company_Number company) {
            return H.Create_Dir(Get_Company_Directory(company).Cpath("quick_tabs"));

        }

        public static string Serial_Number_Prefix {
            get {
                if (Data.Is_Entry_Screens_Suite)
                    return "SES";

                else if (Data.Is_Interface_Tools_Suite)
                    return "SIT";

                true.tift();
                return null;
            }
        }


        // ****************************


        public const string LUCIDA = "LUCIDA";
        public const string COURIER = "COURIER";

        public static Dictionary<string, string> Fonts {
            get {
                return new Dictionary<string, string>{
                         { LUCIDA, "Lucida Console" },
                         { COURIER, "Courier New" },};
            }
        }


        // ****************************


        public static string Discard_Prompt(bool credit_note) {

            var ret = "Are you sure you want to discard this {0}?".spf(credit_note ? "Credit Note" : "POS Invoice");
            return ret;
        }

        public static string Default_User {

            get {
                if (Is_Entry_Screens_Suite)
                    return "SCREENS";

                return "INTERFACE";
            }
        }

        public static string Default_Title {
            get {
                return Data.App_Name;
            }
        }

        public static string QSF_Title {
            get {
                return "{0} - Search".spf(Data.App_Name);
            }
        }

        public static string Activation_Title {
            get {
                return "{0} Activation".spf(Data.App_Name);
            }
        }

        public static string Initial_Setup_Title {

            get {
                return "{0} Initial Setup".spf(Data.App_Name);
            }
        }

        public const int MIN_SAGE_VER = 11;
        public const int MAX_SAGE_VER = 17;

        public static string[] Get_Sage_Versions() {
            return new[] { "11.0", "12.0", "13.0 (2007)", "14.0 (2008) ", "15.0 (2009)", "16.0 (2010)", "17.0 (2011)" };
        }


        public static string Get_Tech_Support_Key(int line_num) {

            var ret = "TECHNICAL_SUPPORT_" + line_num.ToString();

            return ret;

        }





        /// <summary> The user-friendly names of the three licensing modules </summary>
        public static Triple<string> Module_Names {
            get {
                if (Data.Is_Entry_Screens_Suite)
                    return Triple.Make(
                        "Payments Module",
                        "Point of Sales Module",
                        "Transactions Entry Module");

                return Triple.Make("Records Interface Module",
                                   "Transactions Interface Module",
                                   "Documents Interface Module");

            }

        }


        // ****************************


        static public string Get_Printing_Prefix() {
            return DateTime.Today.To_Sortable(false);
        }

        /// <summary>
        /// In cases where series of consecutive reports (printouts, etc) 
        /// are stored using in a directory using filenames that are each
        /// a combination of prefix, date, order (per day) and extension,
        /// this CALLwill return the report filename representing TODAY's
        /// DATE and the NEXT AVAILABLE ORDER NUMBER for TODAY'S DATE.
        /// Example: 
        ///     reports are stored as 'prefix'[date][order].'ext'
        ///     'dir' contains 'prefix'[today][001].'ext',
        ///                    'prefix'[today][002].'ext'
        ///                and 'prefix'[today][003].'ext'
        ///     Return value is 'prefix'[today][004].'ext'
        /// </summary>
        public static string
        Get_Next_Filename(string dir, string prefix, string ext) {
            var next = H.Get_Next_Free_Number(dir, prefix, ext);

            var ret = dir.Cpath("{0} {1:0000}{2}".spf(prefix, next, ext));

            return ret;

        }



    }

}