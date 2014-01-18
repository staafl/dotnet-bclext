using System;
using System.Collections.Generic;
using DTA;
using Fairweather.Service;
using Versioning;

namespace Common
{
    public partial class Sage_Logic : Sage_Access
    {
        public static bool
        Get(Ini_File ini,
             Company_Number company,
             out Sage_Logic sdr) {


            var company_helper = new Company_Helper(ini, company);

            var creds = company_helper.Credentials;

            var ret = Get(ini, creds, out sdr);

            return ret;

        }

        public static bool
        Get(Ini_File ini,
            Company_Number company,
            string username,
            string password,
            out Sage_Logic sdr) {

            var creds = new Credentials(company, username, password);

            var ret = Get(ini, creds, out sdr);

            return ret;


        }


        public static bool
        Get(Ini_File ini,
            Credentials creds,
            out Sage_Logic sdr) {

            sdr = null;

            if (instances.TryGetValue(creds, out sdr))
                return true;

            bool ret = false;

            try {

                string path;
                IRead<Ini_Field, string> proxy;
                int version;

                var general_helper = new General_Helper(ini);
                var company_helper = new Company_Helper(ini, creds.Company);

                try {
                    version = general_helper.Version;
                    path = company_helper.Company_Path;
                    proxy = ini.Group_ro(creds.Company.As_String);

                }
                catch (KeyNotFoundException ex) {
                    throw new XIni(ex.Msg_Or_Type(), ex, XIni.Fault_Type.File_Corrupted);

                }

                ret = Get(creds, path, version, ini, proxy, out sdr);

            }
            // also throws SageConnectionException
            catch (XIni) {
                ret = false;
            }
            catch (ApplicationException) {
                ret = false;
            }

            return ret;

        }

        public static bool
        Get(Credentials creds,
            string path,
            int version,
            Ini_File ini,
            IRead<Ini_Field, string> proxy,
            out Sage_Logic sdr) {

            var ret = false;

            sdr = new Sage_Logic(path, creds, version, ini, proxy);
            instances[creds] = sdr;
            ret = true;

            return ret;

        }

        Sage_Logic(
            string path,
            Credentials creds,
            int version,
            Ini_File ini,
            IRead<Ini_Field, string> proxy)
            : base(path, creds, version) {

            this.Ini = ini;
            this.Settings = new SDR_DTA_Helper(Company, ini);
            this.Proxy = proxy;

        }

        Sage_Logic(
            string path,
            Credentials creds,
            int version)
            : base(path, creds, version) {
            // I should probably add an inheritance node above the current class
        }

        static readonly Dictionary<Credentials, Sage_Logic>
        instances = new Dictionary<Credentials, Sage_Logic>();

        // ****************************


        public void Try(bool condition) {
            if (!condition) {
                Alert_Logic_Error();
            }
        }

        public void Try(bool condition, string message) {
            if (!condition) {
                Alert_Logic_Error(message);
            }
        }

        public void Alert_Logic_Error() {

            Alert_Logic_Error(null);

        }

        public void Alert_Logic_Error(string message) {

            Sage_Error error_code = 0;
            int? last_tran = null;

            using (Establish_Connection()) {

                message = message ?? "";
                if (!message.IsNullOrEmpty())
                    message += "\nLast sage error: ";

                var text = Sdo.Last_Error_Text;

                message += text;

                error_code = Sdo.Last_Error;

                try {
                    last_tran = Get_Last_Transaction_Number();
                }
                catch (XSage_Logic) {
                    last_tran = null;
                }

            }

            var ex = new XSage_Logic(message, last_tran, error_code);
            true.tift(ex);

        }




        // ****************************        


        public bool Has_Ini {
            get { return Ini != null; }
        }

        public Ini_File Ini {
            get;
            private set;
        }

        public SDR_DTA_Helper Settings {
            get;
            private set;
        }

        public IRead<Ini_Field, string> Proxy {
            get;
            private set;
        }



    }
}