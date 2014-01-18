using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

using Common;

using DTA;
using Fairweather.Service;
using Standardization;

namespace Activation
{
    /// <summary>
    /// Abstract chassis for activation programs
    /// </summary>
    public abstract class Program
    {
        static readonly string temp_dir = Activation_Helpers.temp_dir;
        static readonly bool cst_debug = Activation_Helpers.cst_debug;

        const bool __debug = false;

        static protected Program instance;


        // TODO: Any .NET version detection stuff goes here
        public static int Run_Activation_1() {

            try {

                return (int)Run_Activation_2();

            }
            finally {

                Activation_Helpers.Delete_Temp_Dir();


            }
        }

        static Return_Code Run_Activation_2() {

            Mutex mut;

            if (!H.Ensure_Single_Instance(Data.Mutex_ID, true, out mut)) {

                H.Broadcast(Native_Const.WM_USER, 0, new[] { (int)Sub_App.Ses_Cfg }, H.Other_Instances());

                return Return_Code.Multiple_Instances;
            }




            try {
                var ini = Data.Ini_File;
                string key;
                string pin;
                bool has_key = ini.Try_Get_Data(DTA_Fields.ACTIVATION_KEY, out key);
                bool has_pin = ini.Try_Get_Data(DTA_Fields.PIN, out pin);

                bool error;
                bool registered;


                if ((!has_key || key.IsNullOrEmpty()) ||
                    (!has_pin || pin.IsNullOrEmpty())) {

                    error = false;
                    registered = false;

                }
                else {
                    bool valid_key = Activation_Data.ValidateKey(key);
                    bool valid_pin = Activation_Data.ValidatePIN(key, pin);

                    if (valid_key && valid_pin) {

                        error = false;
                        registered = true;

                    }
                    else {

                        error = true;
                        registered = false;

                    }

                }

                bool tmp = true;

                if (!registered) {

                    tmp = Run_Activation_Proc(error);
                    if (!tmp)
                        return Return_Code.Registration_Failed;

                }

                return instance.Accept();

            }
            catch (Exception ex) {
                instance.Handle_Exception(ex);
                return Return_Code.Other_Error;


            }
        }


#if NO_ACTIVATE
        [Obsolete]
#endif
        static bool Run_Activation_Proc(bool error) {

            var ini = Data.Ini_File;
            bool ret;

            do {

                Point location = Point.Empty;
                License_Result license_result;
                Company_Registration_Result? company_result = null;
#if NO_ACTIVATE
                license_result = default(License_Result);
                bool first_company = true;

#else
                if (!__debug) {

                    License_Result? null_result;

                    using (var form = new License_Form(error)) {

                        Application.Run(form);

                        location = form.Location;

                        null_result = form.Result;

                    }

                    if (!null_result.HasValue) {

                        ret = false;
                        break;

                    }

                    license_result = null_result.Value;

                }

                string number_of_companies;
                bool first_company = true;

                if (ini.Try_Get_Data(DTA_Fields.NUMBER_OF_COMPANIES, out number_of_companies)) {

                    if (number_of_companies.IsNullOrEmpty())
                        number_of_companies = "0";

                    var number = Company_Number.To_Company_Number(number_of_companies);
                    first_company = (number == 0);

                }
#endif

                if (first_company) {

                    Company_Registration_Result? null_result;

                    using (var form = new Initial_Company_Form()) {

                        form.Location = location;

                        Application.Run(form);

                        null_result = form.Result;

                    }

                    if (!null_result.HasValue) {

                        ret = false;
                        break;

                    }

                    company_result = null_result.Value;

                }

                if (cst_debug) {
                    Console.WriteLine();
                    Console.WriteLine("Registration result:  {0}", license_result.ToString());
                    Console.WriteLine("Activation result:  {0}", company_result.ToString());
                }

                ret = instance.Perform_Activation(license_result, company_result);

            } while (false);

#if NO_ACTIVATE
            return false;
#else
            return ret;
#endif
        }

        protected abstract void Handle_Exception(Exception ex);

        /// <summary>
        /// The method to call if the registration process has completed successfully or if the program was
        /// already registered. Generally, this method will create and run the application's main form
        /// </summary>
        protected abstract Return_Code Accept();
        protected abstract bool Perform_Activation(License_Result license_result, Company_Registration_Result? company_result);
    }
}
