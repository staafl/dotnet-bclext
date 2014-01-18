using System.Collections.Generic;
using System.Diagnostics;

namespace Common
{
    [DebuggerStepThrough]
    public struct Command_Line_Info
    {
        public Command_Line_Info(Sub_App module,
                                int? company_number,
                                string username,
                                string password,
                                List<Command_Line_Switch_Data> switches,
                                object other_data)
            : this() {
            this.Module = module;
            this.Company_Number = company_number;
            this.Username = username;
            this.Password = password;
            this.Switches = switches;
            this.Other = other_data;
        }


        public Sub_App Module {
            get;
            set;

        }

        public int? Company_Number {
            get;
            set;

        }

        public string Username {
            get;
            set;

        }

        public string Password {
            get;
            set;

        }

        public object Other {
            get;
            set;

        }

        public List<Command_Line_Switch_Data> Switches {
            get;
            set;

        }




        public override string ToString() {

            string ret = "";

            ret += "module = " + this.Module;
            ret += ", ";
            ret += "company_number = " + this.Company_Number;
            ret += ", ";
            ret += "username = " + this.Username;
            ret += ", ";
            ret += "password = " + this.Password;
            ret += ", ";
            ret += "other = " + this.Other;
            ret += ", ";
            ret += "switches = " + this.Switches;

            ret = "{Command_Line_Info: " + ret + "}";
            return ret;

        }


    }
}
