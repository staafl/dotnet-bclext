namespace Common
{
    using System.Diagnostics;
    using Activation;

    [DebuggerStepThrough]
    public struct License_Result
    {

        readonly string m_pin;

        readonly string m_activation;

        readonly License_Data m_data;


        public string Pin {
            get {
                return this.m_pin;
            }
        }

        public string Activation {
            get {
                return this.m_activation;
            }
        }

        public License_Data Data {
            get {
                return this.m_data;
            }
        }

        public static License_Result?
        Try_Get(string activation,
                string pin) {

            pin = pin.Replace("-", "");
            activation = activation.Replace("-", "");
            License_Data? data;

            if (!Activation_Data.ValidateKey(activation, true, out data))
                return null;

            if (!Activation_Data.ValidatePIN(activation, pin))
                return null;

            return new License_Result(activation, pin, data.Value);

        }
        public License_Result(
                    string activation,
                    string pin,
                    License_Data data) {
            this.m_pin = pin;
            this.m_activation = activation;
            this.m_data = data;
        }


        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "pin = " + this.m_pin;
            ret += ", ";
            ret += "activation = " + this.m_activation;
            ret += ", ";
            ret += "data = " + this.m_data;

            return ret;
        }

        public bool Equals(License_Result obj2) {

            if (!this.m_pin.Equals(obj2.m_pin))
                return false;

            if (!this.m_activation.Equals(obj2.m_activation))
                return false;

            if (!this.m_data.Equals(obj2.m_data))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is License_Result);

            if (ret)
                ret = this.Equals((License_Result)obj2);


            return ret;

        }

        public static bool operator ==(License_Result left, License_Result right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(License_Result left, License_Result right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

            unchecked {
                int ret = 23;
                int temp;

                ret *= 31;
                temp = this.m_pin.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_activation.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_data.GetHashCode();
                ret += temp;

                return ret;
            }
        }

    }
}
