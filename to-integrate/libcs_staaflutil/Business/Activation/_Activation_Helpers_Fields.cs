using System;
using System.Collections.Generic;
using System.IO;
using Fairweather.Service;
using Versioning;
using Common;
namespace Activation
{
    public static partial class Activation_Helpers
    {
        const string STR_Sdoeng110dll = "sdoeng110.dll"; // SdoEng111.dll is also available...
        const string STR_Sdoeng120dll = "sdoeng120.dll";
        const string STR_Sg50SDO_Engine130dll = "sg50SDOEngine130.dll";
        const string STR_Sg50SDOEngine140dll = "sg50SDOEngine140.dll";
        const string STR_Sg50SDOEngine150dll = "sg50SDOEngine150.dll";
        const string STR_Sg50SDOEngine160dll = "sg50SDOEngine160.dll";
        const string STR_Sg50SDOEngine170dll = "sg50SDOEngine170.dll";

        const string STR_Sgregisterdll = "sgregister.dll";
        const string STR_Sgdt32dll = "sgdt32.dll";
        const string STR_Sgintl32dll = "sgintl32.dll";
        const string STR_SGLMInterfacedll = "SGLMInterface.dll";

        static readonly Dictionary<string, int> m_sage_dirs = new Dictionary<string, int>();
        static readonly Set<int> extracted = new Set<int>();
        static readonly Set<int> has_vcred = new Set<int>();

        static readonly Set<int> has_sdo = new Set<int>();

        // -> causes NRE File.Exists(curr_dir.Cpath("debug.dta"));
        public static readonly bool cst_debug = File.Exists(Environment.CurrentDirectory.Cpath("debug.dta"));


        public const int cst_ver_0_year = 1994;
        public const int min_ver = 11;
        public const int max_ver = 17;

        const string pass = "Info!0000";

        // VERSION_SPECIFIC

        static readonly string[] files_11 = { STR_Sgdt32dll, STR_Sgregisterdll, STR_Sgintl32dll, "MFC42.dll", "MSVCRT.DLL", STR_Sdoeng110dll };
        static readonly string[] files_12 = { STR_Sgdt32dll, STR_Sgregisterdll, STR_Sgintl32dll, "MFC71.dll", "MSVCR71.dll", "MSVCP71.dll", STR_Sdoeng120dll };
        static readonly string[] files_13 = { STR_Sgdt32dll, STR_Sgregisterdll, STR_Sgintl32dll, "MFC80.dll", "MSVCR80.dll", "MSVCP80.dll", STR_Sg50SDO_Engine130dll };
        static readonly string[] files_14 = { STR_Sgdt32dll, STR_Sgregisterdll, STR_Sgintl32dll, STR_Sg50SDOEngine140dll };
        static readonly string[] files_15 = { STR_Sgdt32dll, STR_Sgregisterdll, STR_Sgintl32dll, STR_Sg50SDOEngine150dll };
        static readonly string[] files_16 = { STR_Sgdt32dll, STR_Sgregisterdll, STR_Sgintl32dll, STR_Sg50SDOEngine160dll, STR_SGLMInterfacedll };
        static readonly string[] files_17 = { STR_Sgdt32dll, STR_Sgregisterdll, STR_Sgintl32dll, STR_Sg50SDOEngine170dll, STR_SGLMInterfacedll };

        static readonly string[] to_reg_11 = { STR_Sgregisterdll, STR_Sdoeng110dll };
        static readonly string[] to_reg_12 = { STR_Sgregisterdll, STR_Sdoeng120dll };
        static readonly string[] to_reg_13 = { STR_Sgregisterdll, STR_Sg50SDO_Engine130dll };
        static readonly string[] to_reg_14 = { STR_Sgregisterdll, STR_Sg50SDOEngine140dll };
        static readonly string[] to_reg_15 = { STR_Sgregisterdll, STR_Sg50SDOEngine150dll };
        static readonly string[] to_reg_16 = { STR_Sgregisterdll, STR_Sg50SDOEngine160dll };
        static readonly string[] to_reg_17 = { STR_Sgregisterdll, STR_Sg50SDOEngine170dll };



        static readonly string[] to_unreg_11 = { STR_Sdoeng110dll };
        static readonly string[] to_unreg_12 = { STR_Sdoeng120dll };
        static readonly string[] to_unreg_13 = { STR_Sg50SDO_Engine130dll };
        static readonly string[] to_unreg_14 = { STR_Sg50SDOEngine140dll };
        static readonly string[] to_unreg_15 = { STR_Sg50SDOEngine150dll };
        static readonly string[] to_unreg_16 = { STR_Sg50SDOEngine160dll };
        static readonly string[] to_unreg_17 = { STR_Sg50SDOEngine170dll };


        static readonly Dictionary<int, string[]> files = new Dictionary<int, string[]>{
                                                                          {11, files_11}, {12, files_12},
                                                                          {13, files_13}, {14, files_14},
                                                                          {15, files_15}, {16, files_16},
                                                                          {17, files_17}, 
        };

        static readonly Dictionary<int, string[]> to_register = new Dictionary<int, string[]>{
                                                                                {11, to_reg_11}, {12, to_reg_12},
                                                                                {13, to_reg_13}, {14, to_reg_14},
                                                                                {15, to_reg_15}, {16, to_reg_16},
                                                                                {17, to_reg_17},
        };


        static readonly Dictionary<int, string[]> to_unregister = new Dictionary<int, string[]>{
                                                                                {11, to_unreg_11}, {12, to_unreg_12},
                                                                                {13, to_unreg_13}, {14, to_unreg_14},
                                                                                {15, to_unreg_15}, {16, to_unreg_16},
                                                                                {17, to_unreg_17},
        };

        static readonly Func<int, string> sdo_dlls_path = ver => temp_dir.Cpath("v" + ver);
        // \temp\vcr\vcredist5.exe
        static readonly Func<string> vcr_exe_path = () => temp_dir.Cpath(vcr).Cpath(vcredist5EXE);

        public static readonly string temp_dir = Data.STR_Activation_Temp_Dir.Path;

        public static readonly string sysdir = H.Get_Sys_Dir();


    }
}