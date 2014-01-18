using Fairweather.Service;

namespace Common
{
    public struct Company_Number
    {
        public static bool Is_Valid(int num) {

            return num >= 1;

        }

        public static string To_Company_Number(int num) {

            (num < 1).tift();

            string ret = num.ToString().PadLeft(4, '0');

            return ret;

        }

        public static int To_Company_Number(string str) {
            int ret = int.Parse(str);

            return ret;
        }


        public static explicit operator Company_Number(int number) {
            return new Company_Number(number);
        }

        public static explicit operator Company_Number(string str) {
            return new Company_Number(str);
        }

        readonly string as_string;

        public string As_String {
            get { return as_string; }
        }

        readonly int as_number;

        public int As_Number {
            get { return as_number; }
        }


        public Company_Number(int number) {

            as_string = To_Company_Number(number);
            as_number = number;
            Verify();
        }

        public Company_Number(string str) {

            as_string = str;
            as_number = To_Company_Number(str);
            Verify();
        }


        void Verify() {
            (as_number >= 0).tiff();

            int temp;
            int.TryParse(as_string, out temp).tiff();

            (temp == as_number).tiff();

        }


        /* Boilerplate */

        public override string ToString() {

            return as_string;

        }

        public bool Equals(Company_Number obj2) {

#pragma warning disable
            if (this.as_string == null) {
                if (obj2.as_string != null)
                    return false;
            }
            else {
                if (!this.as_string.Equals(obj2.as_string))
                    return false;
            }


            if (!this.as_number.Equals(obj2.as_number))
                return false;


#pragma warning restore
            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Company_Number);

            if (ret)
                ret = this.Equals((Company_Number)obj2);


            return ret;

        }

        public static bool operator ==(Company_Number left, Company_Number right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Company_Number left, Company_Number right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

#pragma warning disable
            unchecked {
                int ret = 23;
                int temp;

                if (this.as_string != null) {
                    ret *= 31;
                    temp = this.as_string.GetHashCode();
                    ret += temp;

                }

                ret *= 31;
                temp = this.as_number.GetHashCode();
                ret += temp;

                return ret;
            }
#pragma warning restore
        }


    }
}
