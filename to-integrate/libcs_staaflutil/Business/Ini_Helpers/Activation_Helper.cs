using System;
using Common;
using Fairweather.Service;

namespace DTA
{
    public class Activation_Helper : General_Helper
    {
        public Activation_Helper(Ini_File ini)
            : base(ini) {


        }

        public void Activate_Application(License_Result license_result,
                                         bool first_company) {

            (first_company == !Any_Registered_Companies).tiff();

            if (first_company) {

                Ini_Main.Prepare_Blank_Ini_File(Data.Ini_Filename.Value.Path);

                ini.Read_Data();

                this.Default_Company = new Company_Number(1);

                ini[DTA_Fields.ONLY_DEFAULT_COMPANY] = YES;
                ini[DTA_Fields.DEBUG] = NO;
                ini[DTA_Fields.CREDENTIALS_SOURCE] = FROM_DTA;

            }

            string activation = license_result.Activation;

            ini[DTA_Fields.ACTIVATION_KEY] = activation;
            ini[DTA_Fields.PIN] = license_result.Pin;

            var license_data = license_result.Data;

            Set(DTA_Fields.MODULE_1, license_data.Module_1);
            Set(DTA_Fields.MODULE_2, license_data.Module_2);
            Set(DTA_Fields.MODULE_3, license_data.Module_3);

            string max_companies = license_result.Data.Number_Of_companies.ToString();
            ini[DTA_Fields.MAX_COMPANIES] = max_companies;

            ini.Write_Data();

        }

        /// <summary>
        /// Writes
        /// </summary>
        /// <param name="company_result"></param>
        // [Obsolete("Deal with the default values")]
        public void Register_Company(Company_Registration_Result company_result) {

            int current = Number_Of_Companies;

            var new_company_num = new Company_Number(current + 1);

            Ini_Main.Prepare_Dta_For_New_Company(Data.Ini_Filename.Value.Path, new_company_num);

            ini.Read_Data();

            var company_num = new_company_num;

            ini.Set_Data(DTA_Fields.VERSION, company_result.Version.ToString());

            var control_bank = company_result.Control_Bank_Account;

            {
                var proxy = ini.Group(company_num.As_String);

                proxy[DTA_Fields.COMPANY_NAME] = company_result.Company_Name;
                proxy[DTA_Fields.COMPANY_PATH] = company_result.Path;
                proxy[DTA_Fields.COMPANY_NUMBER] = company_num.As_String;

                proxy[DTA_Fields.USERNAME] = company_result.User_Name;
                proxy[DTA_Fields.PASSWORD] = company_result.Password;


                proxy[DTA_Fields.USR_FILE_PATH] = company_result.Sage_Usr_path;
                proxy[DTA_Fields.CREDENTIALS_SOURCE] = FROM_DTA;

                proxy[DTA_Fields.TE_default_bank_receipts] = control_bank;
                proxy[DTA_Fields.TE_default_bank_payments] = control_bank;

                //DTA_Index.Set_Default_Values(Settings_Kind.PointOfSales, proxy);

                //foreach (var kvp in DTA_Fields.Fields)
                //    proxy[kvp.Key] = kvp.Value.Default_Value;


            }


            Number_Of_Companies = new_company_num.As_Number;


            ini.Write_Data();

        }


    }
}
