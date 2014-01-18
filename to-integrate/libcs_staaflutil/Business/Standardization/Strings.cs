using System.Collections.Generic;
using Common;
using System.Linq;
namespace Standardization
{


    using Fairweather.Service;

    public static partial class Standard
    {
        public static string
Get_Missing_Default_Code_Message(bool show, IEnumerable<Pair<Record_Type, string>> pairs) {

            var _message = "";

            var dict = new Dictionary<Record_Type, string>{

                {Record_Type.Sales, "default Cash Sales Account"},
                {Record_Type.Bank, "default Sales Bank Account"},
                {Record_Type.Expense, "default Payments Expense Account"},

            };

            foreach (var pair in pairs.Distinct()) {

                var type = pair.First;
                var index = pair.Second;

                var msg = dict[type];

                _message += "Unable to find {0} ({1}).\n".spf(msg, index);

            }

            _message = "The following errors have been detected:\n\n" + _message;
            _message += "\nThe program cannot continue.";
            

            if (show)
                Message_Type.System_Error.Show(_message);

            return _message;
        }
    }



}