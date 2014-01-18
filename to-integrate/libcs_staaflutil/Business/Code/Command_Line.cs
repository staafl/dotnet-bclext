using System;
using System.Collections.Generic;

using Fairweather.Service;

namespace Common
{
    public static class Command_Line_Processor
    {
        public static Command_Line_Info?
        Process_Command_Line(string[] argv, out Command_Line_Status status, out object misc_data) {

            status = 0;
            misc_data = null;

            var ret = default(Command_Line_Info);

            Sub_App application = 0;
            int? company_number = null;
            string username = null, password = null;

            List<Command_Line_Switch_Data> switches = null;
            object other_data = null;

            // Compiles
            other_data = username = null;
            // Does not compile
            // username = other_data = null; 

            if (argv.Is_Empty()) {

                status = Command_Line_Status.Command_Line_Is_Empty;
                return new Command_Line_Info(
                    Sub_App.Dashboard,
                    null, null, null, null, null);

            }

            var remaining = new List<string>(argv);


            // once
            for (var _ = 0; _ < 1; ++_) {

                if (!Get_Module(remaining, out application, out status, out misc_data))
                    return null;

                if (!Get_Switches(remaining, out switches, out status, out misc_data))
                    return null;

                if (!Get_Company_Username_Password(
                    remaining,
                    out company_number, out username, out password,
                    out status, out misc_data))

                    return null;


                if (!remaining.Is_Empty()) {
                    status = Command_Line_Status.Wrong_Number_Of_Arguments;
                    misc_data = remaining;
                }



            }

            ret = new Command_Line_Info(
                application,
                company_number,
                username,
                password,
                switches,
                other_data);

            return ret;
        }

        static Dictionary<int, Sub_App>
        SES_Modules {
            get {
                return new Dictionary<int, Sub_App>{
                    {0, Sub_App.Entry_Customers},
                    {1, Sub_App.Entry_Suppliers},
                    {2, Sub_App.Products},
                    {3, Sub_App.Transactions_Entry},
                    {4, Sub_App.Documents_Transfer},
                };
            }
        }

        /// TBI
        static Dictionary<int, Sub_App>
        SIT_Modules {
            get {
                return new Dictionary<int, Sub_App>
                {
                };
            }
        }

        static Dictionary<string, Command_Line_Switch>
        SES_Switches {
            get {
                return new Dictionary<string, Command_Line_Switch>
                {
                };
            }
        }

        static Dictionary<string, Command_Line_Switch>
        SIT_Switches {
            get {
                return new Dictionary<string, Command_Line_Switch>
                {
                };
            }
        }

#if NEVER
            static bool Is_SES {
                  get {
                        return true;
                  }
            }
#else
        static bool
            Is_SES {
            get {
                return Data.Is_Entry_Screens;
            }
        }
#endif



        static bool
            Get_Module(List<string> command_line,
                               out Sub_App application,
                               out Command_Line_Status status,
                               out object misc_data) {

            application = 0;
            status = 0;
            misc_data = null;

            int app_index;
            bool ok = false;

            var application_string = command_line.Chop_First();

            // once
            for (var _ = 0; _ < 1; ++_) {

                if (!int.TryParse(application_string, out app_index)) {
                    ok = false;
                    break;


                }


                var dict = Is_SES ? SES_Modules : SIT_Modules;


                if (!dict.TryGetValue(app_index, out application)) {
                    ok = false;
                    break;


                }

                ok = true;
            }


            if (!ok) {

                application = 0;
                status = Command_Line_Status.Invalid_Module;
                misc_data = application_string;

            }
            else {
                status = Command_Line_Status.OK_No_Company_Username_Password;
            }

            return ok;



        }


        static bool
            Get_Switches(List<string> command_line,
                                 out List<Command_Line_Switch_Data> switches,
                                 out Command_Line_Status status,
                                 out object misc_data) {


            misc_data = null;
            status = 0;

            switches = new List<Command_Line_Switch_Data>();

            Predicate<string> is_switch = (str) => str.StartsWith("/");

            var set = Is_SES ? SES_Switches : SIT_Switches;

            string current = null;
            bool ok = true;

            var visited = new List<int>();
            int ii = -1;

            // once
            for (var _ = 0; _ < 1; ++_)
                using (var enumerator = command_line.GetEnumerator()) {

                    if (!enumerator.MoveNext())
                        break;

                    ++ii;
                    current = enumerator.Current;

                    if (!is_switch(current))
                        continue;

                    Command_Line_Switch sw;
                    string data = null;

                    if (!set.TryGetValue(current, out sw)) {

                        status = Command_Line_Status.Invalid_Switch;
                        ok = false;
                        break;


                    }

                    visited.Add(ii);

                    if (sw.Takes_Parameter()) {

                        if (!enumerator.MoveNext() || is_switch(enumerator.Current)) {

                            status = Command_Line_Status.Missing_Switch_Parameter;
                            ok = false;
                            break;


                        }

                        visited.Add(ii);

                        data = enumerator.Current;
                    }

                    var sw_data = new Command_Line_Switch_Data(sw, data);
                    switches.Add(sw_data);

                }

            if (!ok) {
                misc_data = current;
            }
            else {

                command_line.Remove_At(visited.ToArray());

                status = Command_Line_Status.OK_No_Company_Username_Password;

            }

            return ok;

        }

        static bool
            Get_Company_Username_Password(
                                List<string> command_line,
                                out int? number,
                                out string username,
                                out string password,
                                out Command_Line_Status status,
                                out object misc_data) {

            status = 0;
            misc_data = null;
            number = null;
            username = password = null;
            int len = command_line.Count;

            if (len == 0) {
                status = Command_Line_Status.OK_No_Company_Username_Password;
                return true;
            }

            if (len == 2) {

                status = Command_Line_Status.Wrong_Number_Of_Arguments;
                return false;

            }


            var company_string = command_line.Chop_First();

            int temp_number;

            if (!int.TryParse(company_string, out temp_number)) {

                status = Command_Line_Status.Invalid_Company_String;
                misc_data = company_string;
                return false;

            }

            number = temp_number;

            if (len == 1) {

                status = Command_Line_Status.OK_Has_Company_No_Username_Password;
                return true;

            }

            username = command_line.Chop_First();
            password = command_line.Chop_First();

            status = Command_Line_Status.OK_Has_Company_Username_Password;

            return true;

        }


    }
}