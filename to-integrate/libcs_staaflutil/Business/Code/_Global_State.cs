using System.Diagnostics;
using System.Windows.Forms;
using Fairweather.Service;

namespace Common
{
    /// <summary>
    /// This class contains globally useful information, such as string
    /// constants, default values, program information etc.
    /// 
    /// Once a running instance has determined which application and module
    /// to run, it needs to assign the following fields:
    ///     * Application_Suite
    ///     * Application
    ///     * Sub_Application
    ///     * Ini_File
    ///     * (optionally) Sage_Access
    /// Afterwards it needs to call End_Init() to confirm.
    /// 
    /// After the call to End_Init(), all properties on this class are guaranteed
    /// to be idempotent and stateless.
    /// </summary>
    static public partial class Data
    {
        static Data() {

            var proc = Process.GetCurrentProcess();
            Process_ID = proc.Id;
            Process_Name = proc.ProcessName;

        }

        static bool is_init_over;

        public static bool Is_Init_Over {
            get {
                return is_init_over;
            }
        }

        /// <summary>
        /// Finishes with the initialization of the program, committing
        /// all properties.
        /// Subsequent calls to this method throw an exception.
        /// </summary>
        static public void End_Init() {

            is_init_over.tift();
            Check_Suite(App_Suite);
            Check_Application(App_Suite, App);
            Check_Sub_Application(App, Sub_App);

            is_init_over = true;

        }



        static Sage_Logic s_SageDataRecords;

        /// <summary>
        /// Filthy state
        /// </summary>
        static public Sage_Logic SDR {
            get {
                return s_SageDataRecords;
            }
            set {
                s_SageDataRecords = value;
            }
        }



        static App_Suite s_suite;
        static App s_app;
        static Sub_App s_sub_app;
        static Ini_File s_Ini_File;



        static public App_Suite App_Suite {
            get {
                if (H.Is_In_Designer)
                    return App_Suite.Entry_Screens;

                return s_suite;
            }
            set {
                is_init_over.tift();
                Check_Suite(value);

                s_suite = value;
            }
        }

        static public App App {
            get {

                return s_app;
            }
            set {
                is_init_over.tift();
                Check_Application(App_Suite, value);

                s_app = value;
            }
        }

        static public Sub_App Sub_App {
            get {
                return s_sub_app;
            }
            set {
                is_init_over.tift();

                Check_Sub_Application(App, value);

                s_sub_app = value;

            }
        }

        /// <summary>
        /// Returns a (single) Ini_File instance created during
        /// program initialization.
        /// </summary>
        static public Ini_File Ini_File {
            get {
                return s_Ini_File;
            }
            set {
                is_init_over.tift();

                s_Ini_File = value;
            }
        }

        static public string Get_Help_Url(Control host, HelpEventArgs e) {

            return Help_File;

        }

        static public bool Is_Win7update {
            get;
            set;
        }

        static public string Help_File {
            get {
                if (Is_Entry_Screens_Suite)
                    return "ses_help.chm";
                return null;
            }
        }

        static public bool Is_Entry_Screens_Suite {
            get {
                return App_Suite == App_Suite.Entry_Screens;
            }
        }

        static public bool Is_Interface_Tools_Suite {
            get {
                return App_Suite == App_Suite.Interface_Tools;
            }
        }


        static public bool Is_Entry_Screens {
            get {
                return App == App.Entry_Screens;
            }
        }

        static public bool Is_Ses_Cfg {
            get {
                return App == App.Ses_Cfg;
            }
        }

        static public bool Is_Interface_Tools {
            get {
                return App == App.Sit_Exe;
            }
        }

        public static bool Is_Excel_Helper {
            get {
                return App == App.Excel_Helper;
            }
        }

        public static bool Is_Sage_To_Excel {
            get {
                return App == App.Sage_To_Excel;
            }
        }

        public static bool Is_Doc_Trans {
            get {
                return App == App.Doc_Trans;
            }
        }

        // ****************************


        static public int Process_ID {
            get;
            set;
        }

        static public string Process_Name {
            get;
            set;
        }



        // ****************************



        static void Check_Suite(App_Suite suite) {

            (suite == 0).tift();

            suite.Is_Defined().tiff();


        }

        /// <summary>
        /// Asserts whether or not the current application and application suite 
        /// are supported and whether they are compatible
        /// </summary>
        static void Check_Application(App_Suite suite, App app) {

            (app == 0).tift();

            app.Is_Defined().tiff();

            app.Contains((int)suite).tiff();


        }



        static void Check_Sub_Application(App app, Sub_App sub_app) {

            (sub_app == 0).tift();

            sub_app.Is_Defined().tiff();

            sub_app.Contains((int)app).tiff();

        }





    }
}
