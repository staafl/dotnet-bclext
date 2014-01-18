using System;
using System.Collections.Generic;
using System.Threading;
using Common;
using DTA;
using Fairweather.Service;
using Standardization;
namespace Sage_Int
{
    public class Sit_General_Settings : General_Helper
    {
        public Sit_General_Settings(Ini_File ini) : base(ini) { }
        public bool Records_Module {
            get {
                return Module_Enabled(1); // True(DTA_Fields.IT_records_module);
            }
        }

        public bool Docs_Module {
            get {
                return Module_Enabled(2); //True(DTA_Fields.IT_docs_module);

            }
        }

        public bool Trans_Module {
            get {
                return Module_Enabled(3); //True(DTA_Fields.IT_trans_module);

            }
        }
        /*public int Version {
            get {
                return Int(DTA_Fields.IT_sage_version) ?? 0;
            }
            set {
                Set(DTA_Fields.IT_sage_version, value);
            }
        }
        public int? Default_Company {
            get {
                return Int(DTA_Fields.IT_default_company);
            }
            set {
                Set(DTA_Fields.IT_default_company, value);
            }
        }
        public int? Number_Of_Companies {
            get {
                return Int(DTA_Fields.IT_number_of_companies);
            }
            set {
                Set(DTA_Fields.IT_number_of_companies, value);
            }
        }
        public bool? Creds_From_Prompt {
            get {
                return True(DTA_Fields.IT_credentials_source);
            }
            set {
                Set(DTA_Fields.IT_credentials_source, value);
            }
        }
        public bool Creds_From_Dta {
            get {
                return Creds_From_Prompt != true;
            }
        }
        public bool? Records_Module {
            get {
                return True(DTA_Fields.IT_records_module);
            }
            set {
                Set(DTA_Fields.IT_records_module, value);
            }
        }
        public bool? Trans_Module {
            get {
                return True(DTA_Fields.IT_trans_module);
            }
            set {
                Set(DTA_Fields.IT_trans_module, value);
            }
        }
        public bool? Docs_Module {
            get {
                return True(DTA_Fields.IT_docs_module);
            }
            set {
                Set(DTA_Fields.IT_docs_module, value);
            }
        }
        public bool? Debug {
            get {
                return True(DTA_Fields.IT_debug);
            }
            set {
                Set(DTA_Fields.IT_debug, value);
            }
        }
        public string Username {
            get {
                return String(DTA_Fields.IT_sage_username);
            }
            set {
                Set(DTA_Fields.IT_sage_username, value);
            }
        }
        public string Password {
            get {
                return String(DTA_Fields.IT_sage_password);
            }
            set {
                Set(DTA_Fields.IT_sage_password, value);
            }
        }
        public string Company_Name {
            get {
                return String(DTA_Fields.IT_company_name);
            }
            set {
                Set(DTA_Fields.IT_company_name, value);
            }
        }
        public string Activation_Key {
            get {
                return String(DTA_Fields.IT_activation_key);
            }
            set {
                Set(DTA_Fields.IT_activation_key, value);
            }
        }
        public string Company_Path {
            get {
                return String(DTA_Fields.IT_company_path);
            }
            set {
                Set(DTA_Fields.IT_company_path, value);
            }
        }
        public string SDO_Serial {
            get {
                return String(DTA_Fields.IT_sdo_serial_number);
            }
            set {
                Set(DTA_Fields.IT_sdo_serial_number, value);
            }
        }
        public string SDO_Activation {
            get {
                return String(DTA_Fields.IT_sdo_activation_key);
            }
            set {
                Set(DTA_Fields.IT_sdo_activation_key, value);
            }
        }
        public string Serial {
            set {
                Set(DTA_Fields.IT_serial_number, value);
            }
            get {
                return String(DTA_Fields.IT_serial_number);
            }
        }
        public int? Max_Companies {
            get {
                return Int(DTA_Fields.IT_max_companies);
            }
            set {
                Set(DTA_Fields.IT_max_companies, value);
            }
        }

        public IEnumerable<Company_Number>
        Companies {
            get {
                for (int ii = 1; ii <= (Number_Of_Companies ?? 0); ++ii) {
                    yield return new Company_Number(ii);
                }
            }
        }*/
    }

}
