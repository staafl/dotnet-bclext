namespace Common
{
    using System.Diagnostics;
    using DTA;
    [DebuggerStepThrough]
    public abstract class Printing_Data
    {
        readonly Printing_Helper m_helper;

        public Printing_Helper Helper {
            get { return m_helper; }
        }

        public Company_Number Company {
            get {
                return Helper.Company;
            }
        }


        protected Printing_Data(Printing_Helper helper) {
            this.m_helper = helper;
        }


        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "company = " + this.Company;

            ret = "{Printing_Data: " + ret + "}";
            return ret;

        }

    }
}
