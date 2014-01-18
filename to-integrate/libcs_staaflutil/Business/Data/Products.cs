using System;
using System.Collections.Generic;
using System.IO;
using Fairweather.Service;

namespace Common
{
    static partial class Data
    {
        public const int CST_DEF_PRECISION = 2;

        public const int MINIMUM_CHAR_COUNT = 46;

        /// <summary>  Excluding leading (back)slash </summary>
        public const string MEMO_DIRECTORY = "memo";

        /// <summary>  Including leading dot </summary>
        public const string MEMO_EXTENSION = ".bin";

        public const string DUPLICATES_LOG = "duplicates.log";

        public const string SALES_RECEIPT_DIR = "sales_rec";
        public const string RECEIPT_ON_ACCOUNT_DIR = "receipts";
        public const string END_OF_SHIFT_DIR = "end_of_shift";
        public const string FUTURE_CHEQUES_DIR = "future_cheque";


        static string Get_Pos_Memo_Dir_Strict(Company_Number number) {

            var ret = Data.Get_Company_Directory(number);
            ret = ret.Cpath(Data.MEMO_DIRECTORY);
            H.Create_Directories(ret);

            return ret;

        }


        static public Lazy<string>
        Get_Pos_Memo_Dir(Company_Number number) {

            var dir = Get_Pos_Memo_Dir_Strict(number);

            return new Lazy<string>(() =>
            {
                Directory.CreateDirectory(dir);
                return dir;
            });

        }

        static Lazy<string>
        Get_Memo_Filename(Company_Number number, string name, string dir) {

            // The extension begins with a dot
            var file = "{0}{1}".spf(name, Data.MEMO_EXTENSION);

            var ret = dir.Cpath(file);

            return H.Create_Dir(dir, ret);

        }

        static public Lazy<string>
        Get_Pos_Memo_Filename(Company_Number number, string name) {

            var dir = Get_Pos_Memo_Dir_Strict(number);

            H.Create_Directories(dir);

            return Get_Memo_Filename(number, name, dir);


        }

        static public Lazy<string>
        Get_Printing_Directory(Company_Number number, Printing_Scenario scenario) {

            var ret = Data.Get_Company_Directory(number);

            var dir = new Dictionary<Printing_Scenario, string>
                {
                    {Printing_Scenario.End_Of_Shift, END_OF_SHIFT_DIR},
                    {Printing_Scenario.Sales_Receipt, SALES_RECEIPT_DIR},
                    {Printing_Scenario.Receipt_On_Account, RECEIPT_ON_ACCOUNT_DIR},
                    {Printing_Scenario.Future_Cheque, FUTURE_CHEQUES_DIR},                        
                }[scenario];

            ret = ret.Cpath(dir);

            return H.Create_Dir(ret, ret);

        }

        static public Lazy<string>
        Get_Header_Filename(Company_Number number) {

            var dir = Data.Get_Company_Directory(number);

            var ret = dir.Cpath("layout.dta");

            return H.Create_Dir(dir, ret);

        }




        public static void
        Get_Duplicate_Barcodes_Memorization_Files(Company_Number number,
                                                  out string memo_file,
                                                  out string memo_file_back) {
            var company_dir = Get_Company_Directory(number);
            memo_file = company_dir.Cpath("duplicates.dta");
            memo_file_back = company_dir.Cpath("duplicates.dta.bak");
            H.Create_Directories(company_dir);

        }



        /// <summary>
        /// Includes .
        /// </summary>
        static public string Printing_Extension {
            get {
                return ".txt";
            }
        }


        
                
        static public string Page_Settings_File(Company_Number number) {

            var dir = Data.Get_Company_Directory(number);

            var ret = dir.Cpath("page_setup.dta");

            return ret;

        }
    }
}