using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
namespace Fairweather.Service
{
    public class Command_Line_Parser
    {
        readonly bool allow_switches;
        readonly bool switch_parameters;
        readonly bool switch_grouping;
        readonly char switch_leading_char;
        readonly char switch_param_char;

        public Command_Line_Parser(
                bool allow_switches,
                bool switch_parameters,
                bool switch_grouping,
                char switch_leading_char,
                char switch_param_char) {

            this.allow_switches = allow_switches;
            this.switch_parameters = switch_parameters;
            this.switch_grouping = switch_grouping;
            this.switch_leading_char = switch_leading_char;
            this.switch_param_char = switch_param_char;

        }

        public void
        Parse(string[] argv,
              out List<Command_Line_Argument> args,
              out Multi_Dict<string, Switch> switches) {

            args = new List<Command_Line_Argument>(argv.Length / 2);
            switches = new Multi_Dict<string, Switch>(argv.Length / 2);

            var str_sw = @"{0}(\w+)(?:{1}(\w+))?".spf(switch_leading_char, switch_param_char);

            var rx_sw = new Regex(str_sw, RegexOptions.Compiled);

            foreach (var str in argv) {

                if (str.Trim().IsNullOrEmpty())
                    continue;

                bool is_switch = allow_switches;

                if (is_switch) {

                    var m = rx_sw.Match(str);

                    is_switch = m.Success;

                    if (is_switch) {

                        var sw_id = m.Groups[1].Value;
                        var sw_pars = m.Groups[2].Value;

                        if (!switch_parameters) {
                            sw_id += sw_pars;
                            sw_pars = "";
                        }

                        var sws = switch_grouping ? sw_id.Select(_c => _c + "") : new[] { sw_id };

                        var list = switches[sw_id];

                        foreach (var pair in sws.Mark_Last()) {
                            var id = pair.First;
                            var to_add = new Switch(
                                switch_leading_char + id,
                                id,
                                pair.Second ? sw_pars : "");

                            list.Add(to_add);

                        }


                    }
                }

                if (!is_switch) {
                    var to_add = new Command_Line_Argument(str.Trim());
                    args.Add(to_add);
                }

            }

        }


    }
    	


}