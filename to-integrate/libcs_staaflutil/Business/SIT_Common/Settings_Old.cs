using System;
using System.Collections.Generic;
using System.Threading;
using Common;
using Fairweather.Service;
using Standardization;
namespace Sage_Int
{
    public class Settings_Old : D
    {
         protected const string STR_CURRENT_PERIOD = "CURRENT_PERIOD";
         protected const string STR_SAGEUSR_PATH = "SAGEUSR_PATH";
         protected const string STR_SERIAL_NUMBER = "SERIAL_NUMBER";
         protected const string STR_SDO_ACTIVATION_KEY = "SDO_ACTIVATION_KEY";
         protected const string STR_SDO_SERIAL_NUMBER = "SDO_SERIAL_NUMBER";
         protected const string STR_MAX_COMPANIES = "MAX_COMPANIES";
        /*       Company        */

         protected const string STR_SAGE_USERNAME = "SAGE_USER_NAME";
         protected const string STR_SAGE_PASSWORD = "SAGE_PASSWORD";
         protected const string STR_COMPANY_PATH = "COMPANY_PATH";
         protected const string STR_COMPANY_NAME = "COMPANY_NAME";


         protected const string STR_DYNAMIC_IMPORT_FILE_LAYOUT = "DYNAMIC_IMPORT_FILE_LAYOUT";
         protected const string STR_NEW_ACCOUNT_AUTO_DATE_ENTRY = "NEW_ACCOUNT_AUTO_DATE_ENTRY";
         protected const string STR_OUTPUT_SUCCESS_CSV = "OUTPUT_SUCCESS_CSV";
         protected const string STR_BLANK_FIELDS_OVERWRITE = "BLANK_FIELDS_OVERWRITE";
         protected const string STR_GROUP_TRANSACTIONS_SALES = "GROUP_TRANSACTIONS_SALES";
         protected const string STR_GROUP_TRANSACTIONS_PURCHASE = "GROUP_TRANSACTIONS_PURCHASE";
         protected const string STR_USE_MAPPINGS = "USE_MAPPINGS";
         protected const string STR_CHECK_BANK_AUDIT_TYPE = "CHECK_BANK_AUDIT_TYPE";
         protected const string STR_CHANGE_BANK_AUDIT_TYPE = "CHANGE_BANK_AUDIT_TYPE";
         protected const string STR_USE_DEFAULTS = "USE_DEFAULTS";

        /*       Global        */

         protected const string STR_ACTIVATION_KEY = "ACTIVATION_KEY";
         protected const string STR_RECORDS_MODULE = "RECORDS_MODULE";
         protected const string STR_TRANS_MODULE = "TRANSACTIONS_MODULE";
         protected const string STR_DOCS_MODULE = "DOCUMENTS_MODULE";
         protected const string STR_DEFAULT_COMPANY = "DEFAULT_COMPANY";
         protected const string STR_CREDENTIALS_SOURCE = "CREDENTIALS_SOURCE";
         protected const string STR_DEBUG = "DEBUG";
         protected const string STR_SAGE_VERSION = "SAGE_VERSION";


         protected const string STR_NUMBER_OF_COMPANIES = "NUMBER_OF_COMPANIES";

         protected const string STR_1 = "1";
         protected const string STR_0 = "0";

        /*
         * int? version SAGE_VERSION
int? default_company DEFAULT_COMPANY
int? number_of_companies NUMBER_OF_COMPANIES
bool? creds_from_prompt CREDENTIALS_SOURCE
bool? records_module RECORDS_MODULE 
bool? trans_module TRANS_MODULE
bool? docs_module DOCS_MODULE
bool? debug DEBUG
string activation_key ACTIVATION_KEY
string username SAGE_USERNAME
string password SAGE_PASSWORD
string company_name COMPANY_NAME
string company_path COMPANY_PATH
string last_company_number LAST_COMPANY_NUMBER
string last_company_date LAST_COMPANY_DATE
string last_company_time LAST_COMPANY_TIME
bool? has_headers 
flag has_headers STR_DYNAMIC_IMPORT_FILE_LAYOUT
flag auto_date STR_NEW_ACCOUNT_AUTO_DATE_ENTRY
flag output STR_OUTPUT_SUCCESS_CSV
flag overwrite STR_BLANK_FIELDS_OVERWRITE
flag group_sales STR_GROUP_TRANSACTIONS_SALES
flag group_purchase STR_GROUP_TRANSACTIONS_PURCHASE
flag mapping STR_USE_MAPPINGS
         */

        public Settings_Old(D d) {
            this.Fill(d, true);
        }

        /*       Global        */

        // C:\Users\Fairweather\Desktop\Generation\sageint_settings.pl
        public int Version {
            get {
                return Int(STR_SAGE_VERSION) ?? 0;
            }
            set {
                Set(STR_SAGE_VERSION, value);
            }
        }
        public int? Default_Company {
            get {
                return Int(STR_DEFAULT_COMPANY);
            }
            set {
                Set(STR_DEFAULT_COMPANY, value);
            }
        }
        public int? Number_Of_Companies {
            get {
                return Int(STR_NUMBER_OF_COMPANIES);
            }
            set {
                Set(STR_NUMBER_OF_COMPANIES, value);
            }
        }
        public bool? Creds_From_Prompt {
            get {
                return True(STR_CREDENTIALS_SOURCE);
            }
            set {
                Set(STR_CREDENTIALS_SOURCE, value);
            }
        }
        public bool Creds_From_Dta {
            get {
                return Creds_From_Prompt != true;
            }
        }
        public bool? Records_Module {
            get {
                return True(STR_RECORDS_MODULE);
            }
            set {
                Set(STR_RECORDS_MODULE, value);
            }
        }
        public bool? Trans_Module {
            get {
                return True(STR_TRANS_MODULE);
            }
            set {
                Set(STR_TRANS_MODULE, value);
            }
        }
        public bool? Docs_Module {
            get {
                return True(STR_DOCS_MODULE);
            }
            set {
                Set(STR_DOCS_MODULE, value);
            }
        }
        public bool? Debug {
            get {
                return True(STR_DEBUG);
            }
            set {
                Set(STR_DEBUG, value);
            }
        }
        public string Username {
            get {
                return String(STR_SAGE_USERNAME);
            }
            set {
                Set(STR_SAGE_USERNAME, value);
            }
        }
        public string Password {
            get {
                return String(STR_SAGE_PASSWORD);
            }
            set {
                Set(STR_SAGE_PASSWORD, value);
            }
        }
        public string Company_Name {
            get {
                return String(STR_COMPANY_NAME);
            }
            set {
                Set(STR_COMPANY_NAME, value);
            }
        }
        public string Activation_Key {
            get {
                return String(STR_ACTIVATION_KEY);
            }
            set {
                Set(STR_ACTIVATION_KEY, value);
            }
        }
        public string Company_Path {
            get {
                return String(STR_COMPANY_PATH);
            }
            set {
                Set(STR_COMPANY_PATH, value);
            }
        }
        public string SDO_Serial {
            get {
                return String(STR_SDO_SERIAL_NUMBER);
            }
            set {
                Set(STR_SDO_SERIAL_NUMBER, value);
            }
        }
        public string SDO_Activation {
            get {
                return String(STR_SDO_ACTIVATION_KEY);
            }
            set {
                Set(STR_SDO_ACTIVATION_KEY, value);
            }
        }
        public string Serial {
            set {
                Set(STR_SERIAL_NUMBER, value);
            }
            get {
                return String(STR_SERIAL_NUMBER);
            }
        }
        public int? Max_Companies {
            get {
                return Int(STR_MAX_COMPANIES);
            }
            set {
                Set(STR_MAX_COMPANIES, value);
            }
        }

        public IEnumerable<Company_Number>
        Companies {
            get {
                for (int ii = 1; ii <= (Number_Of_Companies ?? 0); ++ii) {
                    yield return new Company_Number(ii);
                }
            }
        }



        /*       Helpers        */

        protected void Set(string key, bool? bb) {
            Set(key, bb == true ? STR_1 : STR_0);
        }

        protected void Set(string key, int? ii) {
            Set(key, ii == null ? null : ii + "");
        }

        protected void Set(string key, string str) {
            this[key] = str;
        }

        protected void Set(string key, DateTime? dt) {
            Set(key, dt == null ? null : dt.Value.ToString(true));
        }


        protected bool? True(string key) {
            string str;
            if (!this.TryGetValue(key, out str))
                return null;
            return str == STR_1;
        }

        protected int? Int(string key) {
            var str = this[key];
            int ret;
            if (int.TryParse(str, out ret))
                return ret;
            return null;
        }

        protected DateTime? Date(string key) {

            var str = this[key];
            var ret = DateTime.ParseExact(str, "dd/MM/yyyy", Thread.CurrentThread.CurrentCulture.DateTimeFormat);

            return ret;
        }

        protected string String(string key) {

            return this[key];

        }
    }



}
