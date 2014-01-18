namespace Common
{
    using System.Diagnostics;
    [DebuggerStepThrough]
    public struct Company_Registration_Result
    {
        public Company_Registration_Result(
            string company_name,
            string path,
            string user_name,
            string password,
            string period,
            string sage_usr_path,
            int version,
            string control_bank_acc) {
            this.m_company_name = company_name;
            this.m_path = path;
            this.m_user_name = user_name;
            this.m_password = password;
            this.m_period = period;
            this.m_sage_usr_path = sage_usr_path;
            this.m_version = version;
            this.m_control_bank_acc = control_bank_acc;
        }

        public string Company_Name {
            get {
                return this.m_company_name;
            }
        }

        public string Path {
            get {
                return this.m_path;
            }
        }

        public string User_Name {
            get {
                return this.m_user_name;
            }
        }

        public string Password {
            get {
                return this.m_password;
            }
        }

        public string Period {
            get {
                return this.m_period;
            }
        }

        public string Sage_Usr_path {
            get {
                return this.m_sage_usr_path;
            }
        }

        public int Version {
            get {
                return this.m_version;
            }
        }

        public string Control_Bank_Account {
            get { return this.m_control_bank_acc; }
        }


        readonly string m_company_name;

        readonly string m_path;

        readonly string m_user_name;

        readonly string m_password;

        readonly string m_period;

        readonly string m_sage_usr_path;

        readonly int m_version;

        readonly string m_control_bank_acc;

    }
}
