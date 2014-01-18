using System;
using Fairweather.Service;

namespace Common
{
    static partial class Data
    {


        /*       Program files        */

        public static readonly string
        ProgramData = H.Get_Common_AppData_Dir(),
        ExeDir = H.Get_Exe_Dir();

        public static readonly Func<object, string>
        def_subfolder = _ => Get_App_Data_Subfolder();

        public static string
        AppDataFolder {
            get { return ProgramData.Cpath(def_subfolder(null)); }
        }

        public static readonly Program_File_Info
        STR_SITEXE = new Program_File_Info("SIT.EXE", ExeDir, true),
        STR_SITCFGEXE = new Program_File_Info("SITCFG.EXE", ExeDir, true),
        STR_SITGUIEXE = new Program_File_Info("SITGUI.EXE", ExeDir, true),
        STR_SITDTA = new Program_File_Info("sit.dta", def_subfolder, ProgramData, true),

            //STR_NETDTA = new Program_File_Info("NET.DTA", def_subfolder,ProgramData, true),
        STR_SGEDTA = new Program_File_Info("sge.dta", ExeDir, true),

        STR_SDODLL = new Program_File_Info("SDO.DLL", ExeDir, true),
        STR_ARCDLL = new Program_File_Info("ARC.DLL", ExeDir, true),
        STR_CSVDLL = new Program_File_Info("CSV.DLL", ExeDir, true),

        STR_SESEXE = new Program_File_Info("SES.EXE", ExeDir, true),
        STR_SESCFGEXE = new Program_File_Info("SESCFG.EXE", ExeDir, true),
        STR_PRINTDLL = new Program_File_Info("PRINT.DLL", ExeDir, true),
        STR_SERVICEDLL = new Program_File_Info("SERVICE.DLL", ExeDir, true),
        STR_SAMDLL = new Program_File_Info("SAM.DLL", ExeDir, true),
        STR_CRYPTEXE = new Program_File_Info("CRYPT.EXE", ExeDir, true),
        STR_CODEGENEXE = new Program_File_Info("CODEGEN.EXE", ExeDir, true),
        STR_CODEGENGUIEXE = new Program_File_Info("CODEGENGUI.EXE", ExeDir, true),

        STR_SESDTA = new Program_File_Info("ses.dta", def_subfolder, ProgramData, true),
        STR_XLDTA = new Program_File_Info("xl.dta", def_subfolder, ProgramData, true),
        STR_SAGE2XLDTA = new Program_File_Info("sage2xl.dta", def_subfolder, ProgramData, true),

        STR_SESDTA_LC = new Program_File_Info("ses.dta", def_subfolder, ProgramData, true),

        STR_Officedll = new Program_File_Info("Office.dll", ExeDir, true),
        STR_MicrosoftVbeInteropdll = new Program_File_Info("Microsoft.Vbe.Interop.dll", ExeDir, true),
        STR_MicrosoftOfficeInteropExceldll = new Program_File_Info("Microsoft.Office.Interop.Excel.dll", ExeDir, true);


        // ****************************



        public static readonly Program_File_Info
        STR_Sitcfglog = new Program_File_Info("sitcfg.log", def_subfolder, ProgramData, false),
        STR_Sitlog = new Program_File_Info("sit.log", def_subfolder, ProgramData, false),

        STR_Cfglog = new Program_File_Info("cfg.log", def_subfolder, ProgramData, false),
        STR_Dashboardlog = new Program_File_Info("dashboard.log", def_subfolder, ProgramData, false),
        STR_Translog = new Program_File_Info("trans.log", def_subfolder, ProgramData, false),
        STR_Startuplog = new Program_File_Info("startup.log", def_subfolder, ProgramData, false),
        STR_Productslog = new Program_File_Info("products.log", def_subfolder, ProgramData, false),
        STR_Entrylog = new Program_File_Info("entry.log", def_subfolder, ProgramData, false),

        STR_Transferlog = new Program_File_Info("transfer.log", def_subfolder, ProgramData, false),

        STR_Xllog = new Program_File_Info("xl.log", def_subfolder, ProgramData, false),
        STR_S2xllog = new Program_File_Info("sage2xl.log", def_subfolder, ProgramData, false)
        ;

        public static readonly Program_File_Info
        STR_Sit_Error_Log = new Program_File_Info("Errors.log", def_subfolder, ProgramData, false),
        STR_Sit_Success_Log = new Program_File_Info("Success.csv", def_subfolder, ProgramData, false);

        public static readonly Program_File_Info
        STR_Sit_History_Dir = new Program_File_Info("History", true, def_subfolder, ProgramData, false),
        STR_Activation_Temp_Dir = new Program_File_Info("temp", true, def_subfolder, ProgramData, false);

    }
}
