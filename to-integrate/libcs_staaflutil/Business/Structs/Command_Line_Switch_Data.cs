using System.Diagnostics;

namespace Common
{
    [DebuggerStepThrough]
    public struct Command_Line_Switch_Data
    {





        public Command_Line_Switch Switch {
            get;
            set;

        }

        public string Data {
            get;
            set;

        }


        public Command_Line_Switch_Data(Command_Line_Switch @switch,
                                        string data)
            : this() {
            this.Switch = @switch;
            this.Data = data;
        }


        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "switch = " + this.Switch;
            ret += ", ";
            ret += "data = " + this.Data;

            ret = "{Command_Line_Switch_Data: " + ret + "}";
            return ret;

        }

        public bool Equals(Command_Line_Switch_Data obj2) {

#pragma warning disable
            if (this.Switch == null) {
                if (obj2.Switch != null)
                    return false;
            }
            else {
                if (!this.Switch.Equals(obj2.Switch))
                    return false;
            }


            if (this.Data == null) {
                if (obj2.Data != null)
                    return false;
            }
            else {
                if (!this.Data.Equals(obj2.Data))
                    return false;
            }


#pragma warning restore
            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Command_Line_Switch_Data);

            if (ret)
                ret = this.Equals((Command_Line_Switch_Data)obj2);


            return ret;

        }

        public static bool operator ==(Command_Line_Switch_Data left, Command_Line_Switch_Data right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Command_Line_Switch_Data left, Command_Line_Switch_Data right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

#pragma warning disable
            unchecked {
                int ret = 23;
                int temp;

                if (this.Switch != null) {
                    ret *= 31;
                    temp = this.Switch.GetHashCode();
                    ret += temp;

                }

                if (this.Data != null) {
                    ret *= 31;
                    temp = this.Data.GetHashCode();
                    ret += temp;

                }

                return ret;
            }
#pragma warning restore
        }

    }
}
