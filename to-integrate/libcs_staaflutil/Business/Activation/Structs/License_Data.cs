namespace Common
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
                    bool _)
            : this(rec_module, tran_module, doc_module, number_of_companies) {

        }
        public License_Data(bool module1,
                            bool module2,
                            bool module3,
                            int number_of_companies) {

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
}
