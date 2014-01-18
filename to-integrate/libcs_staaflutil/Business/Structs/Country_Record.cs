using System.Diagnostics;

namespace Common
{
    [DebuggerStepThrough]
    public struct Country_Record
    {
        #region Country_Info

        readonly string m_country_code;

        readonly string m_country_name;

        readonly string m_vat_format_1;

        readonly string m_vat_format_2;

        readonly string m_vat_format_3;

        readonly string m_vat_format_4;

        readonly string m_vat_format_5;


        public string Country_Code {
            get {
                return this.m_country_code;
            }
        }

        public string Country_Name {
            get {
                return this.m_country_name;
            }
        }

        public string Vat_Format_1 {
            get {
                return this.m_vat_format_1;
            }
        }

        public string Vat_Format_2 {
            get {
                return this.m_vat_format_2;
            }
        }

        public string Vat_Format_3 {
            get {
                return this.m_vat_format_3;
            }
        }

        public string Vat_Format_4 {
            get {
                return this.m_vat_format_4;
            }
        }

        public string Vat_Format_5 {
            get {
                return this.m_vat_format_5;
            }
        }


        public Country_Record(string country_code,
                    string country_name,
                    string vat_format_1,
                    string vat_format_2,
                    string vat_format_3,
                    string vat_format_4,
                    string vat_format_5) {
            this.m_country_code = country_code;
            this.m_country_name = country_name;
            this.m_vat_format_1 = vat_format_1;
            this.m_vat_format_2 = vat_format_2;
            this.m_vat_format_3 = vat_format_3;
            this.m_vat_format_4 = vat_format_4;
            this.m_vat_format_5 = vat_format_5;
        }


        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "country_code = " + this.m_country_code;
            ret += ", ";
            ret += "country_name = " + this.m_country_name;
            ret += ", ";
            ret += "vat_format_1 = " + this.m_vat_format_1;
            ret += ", ";
            ret += "vat_format_2 = " + this.m_vat_format_2;
            ret += ", ";
            ret += "vat_format_3 = " + this.m_vat_format_3;
            ret += ", ";
            ret += "vat_format_4 = " + this.m_vat_format_4;
            ret += ", ";
            ret += "vat_format_5 = " + this.m_vat_format_5;

            ret = "{Country_Info: " + ret + "}";
            return ret;

        }

        public bool Equals(Country_Record obj2) {

            if (!this.m_country_code.Equals(obj2.m_country_code))
                return false;

            if (!this.m_country_name.Equals(obj2.m_country_name))
                return false;

            if (!this.m_vat_format_1.Equals(obj2.m_vat_format_1))
                return false;

            if (!this.m_vat_format_2.Equals(obj2.m_vat_format_2))
                return false;

            if (!this.m_vat_format_3.Equals(obj2.m_vat_format_3))
                return false;

            if (!this.m_vat_format_4.Equals(obj2.m_vat_format_4))
                return false;

            if (!this.m_vat_format_5.Equals(obj2.m_vat_format_5))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Country_Record);

            if (ret)
                ret = this.Equals((Country_Record)obj2);


            return ret;

        }

        public static bool operator ==(Country_Record left, Country_Record right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Country_Record left, Country_Record right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

            unchecked {
                int ret = 23;
                int temp;

                ret *= 31;
                temp = this.m_country_code.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_country_name.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_vat_format_1.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_vat_format_2.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_vat_format_3.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_vat_format_4.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_vat_format_5.GetHashCode();
                ret += temp;

                return ret;
            }
        }

        #endregion
    }
}
