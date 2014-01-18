namespace Activation
{
    using System.Diagnostics;

    [DebuggerStepThrough]
    public struct License_Data
    {

        readonly bool m_module1;

        readonly bool m_module2;

        readonly bool m_module3;

        readonly int m_number_of_companies;

        public bool Module_1 {
            get {
                return this.m_module1;
            }
        }

        public bool Module_2 {
            get {
                return this.m_module2;
            }
        }

        public bool Module_3 {
            get {
                return this.m_module3;
            }
        }

        public bool Rec_Module {
            get {
                return Module_1;
            }
        }

        public bool Tran_Module {
            get {
                return Module_2;
            }
        }

        public bool Doc_Module {
            get {
                return Module_3;
            }
        }

        public int Number_Of_companies {
            get {
                return this.m_number_of_companies;
            }
        }


        public License_Data(bool rec_module,
                    bool tran_module,
                    bool doc_module,
                    int number_of_companies,
                    bool _) :this(rec_module, tran_module, doc_module, number_of_companies)
        {

        }
        public License_Data(bool module1,
                            bool module2,
                            bool module3,
                            int number_of_companies) 
        {

            this.m_module1 = module1;
            this.m_module2 = module2;
            this.m_module3 = module3;
            this.m_number_of_companies = number_of_companies;

        }


        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "rec_module = " + this.m_module1;
            ret += ", ";
            ret += "tran_module = " + this.m_module2;
            ret += ", ";
            ret += "doc_module = " + this.m_module3;
            ret += ", ";
            ret += "number_of_companies = " + this.m_number_of_companies;

            return ret;
        }

        public bool Equals(License_Data obj2) {

            if (!this.m_module1.Equals(obj2.m_module1))
                return false;

            if (!this.m_module2.Equals(obj2.m_module2))
                return false;

            if (!this.m_module3.Equals(obj2.m_module3))
                return false;

            if (!this.m_number_of_companies.Equals(obj2.m_number_of_companies))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is License_Data);

            if (ret)
                ret = this.Equals((License_Data)obj2);


            return ret;

        }

        public static bool operator ==(License_Data left, License_Data right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(License_Data left, License_Data right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

            unchecked {
                int ret = 23;
                int temp;

                ret *= 31;
                temp = this.m_module1.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_module2.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_module3.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_number_of_companies.GetHashCode();
                ret += temp;

                return ret;
            }
        }

    }

    public struct SDO_Registration_Result
    {
        readonly string m_activation;

        readonly string m_serial;

        readonly int m_version;


        public string Activation {
            get { return m_activation; }
        }

        public string Serial {
            get { return m_serial; }
        }

        public int Version {
            get { return m_version; }
        }

        public SDO_Registration_Result(string serial,
                                         string activation,
                                         int version) {
            m_serial = serial;
            m_activation = activation;
            m_version = version;
        }

    }


    [DebuggerStepThrough]
    public struct Company_Registration_Result
    {

        readonly string m_company_name;

        readonly string m_path;

        readonly string m_user_name;

        readonly string m_password;

        readonly string m_period;

        readonly string m_sage_usr_path;

        readonly int m_version;


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


        public Company_Registration_Result(string company_name,
                    string path,
                    string user_name,
                    string password,
                    string period,
                    string sage_usr_path,
                    int version) {
            this.m_company_name = company_name;
            this.m_path = path;
            this.m_user_name = user_name;
            this.m_password = password;
            this.m_period = period;
            this.m_sage_usr_path = sage_usr_path;
            this.m_version = version;
        }


        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "company_name = " + this.m_company_name;
            ret += ", ";
            ret += "path = " + this.m_path;
            ret += ", ";
            ret += "user_name = " + this.m_user_name;
            ret += ", ";
            ret += "password = " + this.m_password;
            ret += ", ";
            ret += "period = " + this.m_period;
            ret += ", ";
            ret += "sage_usr_path = " + this.m_sage_usr_path;
            ret += ", ";
            ret += "version = " + this.m_version;

            ret = "{Activation_Result: " + ret + "}";
            return ret;

        }

        public bool Equals(Company_Registration_Result obj2) {

            if (!this.m_company_name.Equals(obj2.m_company_name))
                return false;

            if (!this.m_path.Equals(obj2.m_path))
                return false;

            if (!this.m_user_name.Equals(obj2.m_user_name))
                return false;

            if (!this.m_password.Equals(obj2.m_password))
                return false;

            if (!this.m_period.Equals(obj2.m_period))
                return false;

            if (!this.m_sage_usr_path.Equals(obj2.m_sage_usr_path))
                return false;

            if (!this.m_version.Equals(obj2.m_version))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Company_Registration_Result);

            if (ret)
                ret = this.Equals((Company_Registration_Result)obj2);


            return ret;

        }

        public static bool operator ==(Company_Registration_Result left, Company_Registration_Result right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Company_Registration_Result left, Company_Registration_Result right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

            unchecked {
                int ret = 23;
                int temp;

                ret *= 31;
                temp = this.m_company_name.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_path.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_user_name.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_password.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_period.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_sage_usr_path.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_version.GetHashCode();
                ret += temp;

                return ret;
            }
        }

    }
	


}