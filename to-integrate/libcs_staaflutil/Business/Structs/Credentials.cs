using System.Diagnostics;
using System;

namespace Common
{
    [DebuggerStepThrough]
    public struct Credentials
    {
        public Credentials(string user, string pass)
            : this(null, user, pass) {
        }
        public Credentials(
                  Company_Number? company,
                  string user,
                  string pass)
            : this() {
            this.Company_ = company;
            this.Username = user;
            this.Password = pass;
        }

        public Company_Number? Company_ {
            get;
            set;
        }
        public Company_Number Company {
            get { return Company_.Value; }
            set { Company_ = value; }
        }

        public string Username {
            get;
            set;
        }

        public string Password {
            get;
            set;
        }




        #region Credentials

        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "number = " + this.Company_;
            ret += ", ";
            ret += "user = " + this.Username;
            ret += ", ";
            ret += "pass = " + this.Password;

            ret = "{Credentials: " + ret + "}";
            return ret;

        }

        public bool Equals(Credentials obj2) {

#pragma warning disable
            if (!this.Company_.Equals(obj2.Company_))
                return false;


            if (this.Username == null) {
                if (obj2.Username != null)
                    return false;
            }
            else {
                if (!this.Username.Equals(obj2.Username))
                    return false;
            }


            if (this.Password == null) {
                if (obj2.Password != null)
                    return false;
            }
            else {
                if (!this.Password.Equals(obj2.Password))
                    return false;
            }


#pragma warning restore
            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Credentials);

            if (ret)
                ret = this.Equals((Credentials)obj2);


            return ret;

        }

        public static bool operator ==(Credentials left, Credentials right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Credentials left, Credentials right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

#pragma warning disable
            unchecked {
                int ret = 23;
                int temp;

                ret *= 31;
                temp = this.Company.GetHashCode();
                ret += temp;


                if (this.Username != null) {
                    ret *= 31;
                    temp = this.Username.GetHashCode();
                    ret += temp;

                }

                if (this.Password != null) {
                    ret *= 31;
                    temp = this.Password.GetHashCode();
                    ret += temp;

                }

                return ret;
            }
#pragma warning restore
        }

        #endregion
    }
}
