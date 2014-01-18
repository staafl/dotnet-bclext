using System;
using System.Collections.Generic;
using Activation;
using Common;
using Fairweather.Service;

namespace DTA
{
    public class General_Helper : Write_Able_Helper
    {

        public IEnumerable<string> Company_Strings {
            get {

                foreach (var company in Companies) {

                    string item = Get_Company_String(company);

                    yield return item;

                }
            }
        }

        public string
        Get_Company_String(Company_Number company) {

            var company_helper = new Company_Helper(ini, company);

            var as_string = company.As_String;

            var company_name = company_helper.Company_Name;

            var item = "{0}  -  {1}".spf(as_string, company_name);

            return item;
        }

        public General_Helper(Ini_File ini)
            : base(ini) {
        }


        public bool IsRegistered {
            get {
                string key, pin;

                bool has_key = ini.Try_Get_Data(DTA_Fields.ACTIVATION_KEY, out key);
                bool has_pin = ini.Try_Get_Data(DTA_Fields.PIN, out pin);

                if (key.IsNullOrEmpty())
                    return false;

                if (pin.IsNullOrEmpty())
                    return false;

                if (!Activation_Data.ValidateKey(key))
                    return false;

                if (!Activation_Data.ValidatePIN(key, pin))
                    return false;

                return true;
            }
        }

        public string Activation_Key {
            get {
                return String(DTA_Fields.ACTIVATION_KEY);
            }
        }

        public string PIN {
            get {
                return String(DTA_Fields.PIN);
            }

        }

        public Ini_File Ini_File {
            get { return ini; }
        }

        public List<Company_Number> Companies {
            get {

                int cnt = Number_Of_Companies;
                var ret = new List<Company_Number>(cnt);

                for (int ii = 1; ii <= cnt; ++ii) {

                    ret.Add(new Company_Number(ii));
                }

                return ret;
            }
        }

        public Company_Number Default_Company {
            get {
                return new Company_Number(Int(DTA_Fields.DEFAULT_COMPANY));
            }
            set {
                Set(DTA_Fields.DEFAULT_COMPANY, value.As_Number);
            }
        }


        public Set<Sub_App> Licensed_Modules {
            get {
                var lst = new List<Sub_App>();

                if (Data.Is_Entry_Screens) {

                    if (Module_Enabled(1)) {
                        lst.Add(Sub_App.Entry_Customers);
                        lst.Add(Sub_App.Entry_Suppliers);
                    }
                    if (Module_Enabled(2)) {
                        lst.Add(Sub_App.Products);
                    }
                    if (Module_Enabled(3)) {
                        lst.Add(Sub_App.Documents_Transfer);
                        lst.Add(Sub_App.Transactions_Entry);

                    }
                }
                if (Data.Is_Ses_Cfg) {

                    lst.Add(Sub_App.Ses_Cfg);

                }

                if (Data.Is_Interface_Tools) {

                    // not needed
                    throw new NotImplementedException();


                }

                var ret = new Set<Sub_App>(lst);

                return ret;

            }
        }

        public bool Module_Enabled(int module_number) {

            (module_number <= 3 && module_number > 0).tiff();

            var strings = new Dictionary<int, Ini_Field>
            {
                {1, DTA_Fields.MODULE_1},
                {2, DTA_Fields.MODULE_2},
                {3, DTA_Fields.MODULE_3},
            };

            return True(strings[module_number]);

        }

        public bool Allow_Other_Companies {
            get {
                return False(DTA_Fields.ONLY_DEFAULT_COMPANY);
            }
            set {
                Set(DTA_Fields.ONLY_DEFAULT_COMPANY, !value);
            }
        }

        public bool Allow_Reset_Of_Active_Users {
            get {
                return True(DTA_Fields.ALLOW_REMOVING_USERS);
            }
            set {
                Set(DTA_Fields.ALLOW_REMOVING_USERS, value);
            }
        }

        public bool InternalCredentials {
            get {
                return String(DTA_Fields.CREDENTIALS_SOURCE) == FROM_DTA;
            }
            set {
                if (value)
                    Set(DTA_Fields.CREDENTIALS_SOURCE, FROM_DTA);
                else
                    CommandLineCredentials = true;
            }
        }

        public bool CommandLineCredentials {
            get {
                return String(DTA_Fields.CREDENTIALS_SOURCE) == PROMPT;
            }
            set {
                if (value)
                    Set(DTA_Fields.CREDENTIALS_SOURCE, PROMPT);
                else
                    InternalCredentials = true;
            }
        }

        public bool Debug {
            get {
                return True(DTA_Fields.DEBUG);
            }
            set {
                Set(DTA_Fields.DEBUG, value);
            }
        }

        public int Max_Companies {
            get {
                return Safe_Int(DTA_Fields.MAX_COMPANIES);
            }
        }

        public bool Any_Registered_Companies {
            get {
                var num = Number_Of_Companies;
                (num >= 0).tiff();
                bool ret = (num > 0);
                return ret;
            }
        }

        protected override int Safe_Int(Ini_Field field) {

            string str;

            if (!ini.Try_Get_Data(field, out str))
                return 0;

            return base.Safe_Int(field);

        }

        public int Number_Of_Companies {
            get {

                return Safe_Int(DTA_Fields.NUMBER_OF_COMPANIES);

            }
            set {
                ini[DTA_Fields.NUMBER_OF_COMPANIES] = value.ToString();
            }

        }

        public int Version {
            get {
                return Int(DTA_Fields.VERSION);
            }
        }




        public Company_Helper Get_Company_Helper(Company_Number number) {

            (Number_Of_Companies < number.As_Number).tift();

            var ret = new Company_Helper(ini, number);

            return ret;

        }


    }
}