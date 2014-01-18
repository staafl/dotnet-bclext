
#region Header
using System;
using System.Collections.Generic;

using Common;

using Fairweather.Service;

using IRState = Common.Invoice_Recalling_State;


#endregion
using NM = Common.Named_Message;

namespace Standardization
{
    static public partial class Standard
    {
        static bool
        Get_Message_And_Type(NM named_message, object[] args, out string message, out Message_Type type) {

            message = null;
            type = 0;
            Pair<String_Producer, Message_Type> pair;

            if (!string_producers.TryGetValue(named_message, out pair))
                return false;

            message = pair.First(args);
            type = pair.Second;

            return true;

        }

        static Dictionary<NM, Pair<String_Producer, Message_Type>>
        string_producers = new Dictionary<NM, Pair<String_Producer, Message_Type>>
            {
                {NM.An_Unexpected_Error, 
                 Error(Use_Title(TITLE_AN_UNEXPECTED_ERROR))},

                {NM.Connection_To_The_Sage_Data_Files_Cannot_Be_Established, 
                 Error(spf(spf_CONNECTION_TO_THE_SAGE_DATA)) },

                {NM.The_Folder_You_Have_Selected, 
                Error(As_Is(THE_FOLDER_YOU_HAVE_SELECTED)) },

                {NM.Serialized_Recall_Failed, 
                Error(As_Is(RECALLING_THE_BINARY_FILE)) },
                
                {NM.X_is_unable_to_verify_your_login_data,
                Error(Use_Title(TITLE_IS_UNABLE_TO_VERIFY)) },

                {NM.The_License_Information_You_Have_Entered_Is_Not_Valid,
                Error(As_Is(THE_LICENSE_INFORMATION))},

                {NM.Program_Successfully_Activated,
                Success(As_Is(PROGRAM_SUCCESSFULLY_ACTIVATED))},

                {NM.Sage_Line_50_Is_Not_Installed, 
                  Attention_OK_Cancel(Sage_Line_50_Is_Not_Installed)},

                  {NM.Unable_To_Recall_Invoice,
                      Error(Unable_To_Recall)},
                
            };

        static String_Producer Make<T>(Func<T, object[], string> f) {
            return (args) => f((T)args[0], args);
        }

        static String_Producer Unable_To_Recall {
            get {
                return
            Make<IRState>((state, args) =>
                {
                    var def_message = "An unknown error prevents {0} from recalling the selected invoice.";
                    def_message = def_message.spf(Data.App_Name);

                    var dict = new Dictionary<IRState, string>
                    {
                        {IRState.Invalid_Invoice_Number, 
                         "The selected Document Number ({0}) was not found in the data files.".spf(args[1])},
                        {IRState.Customer_Does_Not_Exist, 
                         "The Customer Account {0} was not found in the data files.".spf(args[2])}
                    };

                    var message = dict.Get_Or_Default(state, () => def_message);

                    return message;
                });

            }
        }



        static string Sage_Line_50_Is_Not_Installed(Object[] _) {

            var ret = SAGE_LINE_50_IS_NOT_INSTALLED.spf(Data.Default_Title,
                  Data.Is_Entry_Screens_Suite ?
                  " and work correctly" :
                  " and interface data");
            return ret;
        }

        delegate string String_Producer(object[] args);

        /*       Helpers        */

        static String_Producer
        As_Is(string format) {

            return new String_Producer((_) => format);

        }

        static String_Producer
        Use_Title(string format) {

            return new String_Producer((_) => format.spf(Data.Default_Title));

        }

        static String_Producer
        spf(string format) {

            return new String_Producer(obj => format.spf(obj));

        }

        static Pair<String_Producer, Message_Type>
        Error(String_Producer str_prod) {

            return Pair.Make(str_prod, Message_Type.Error);

        }

        static Pair<String_Producer, Message_Type>
        Attention_OK_Cancel(String_Producer str_prod) {

            return Pair.Make(str_prod, Message_Type.Attention_OK_Cancel);

        }

        static Pair<String_Producer, Message_Type>
        Warning(String_Producer str_prod) {

            return Pair.Make(str_prod, Message_Type.Warning);

        }

        static Pair<String_Producer, Message_Type>
        Success(String_Producer str_prod) {

            return Pair.Make(str_prod, Message_Type.Success);

        }

        /*       Global        */

        const string PROGRAM_SUCCESSFULLY_ACTIVATED = "Program successfully activated.";

        const string spf_CONNECTION_TO_THE_SAGE_DATA =
            "Connection to the Sage data files cannot not be established.\n"
            + "\n"
            + "Please make sure that the folder you have selected contains\n"
            + "data files of version {0} and the credentials are valid.";

        const string RECALLING_THE_BINARY_FILE = "Recalling the binary file failed. The data is probably corrupted.";

        const string THE_FOLDER_YOU_HAVE_SELECTED =
              "The folder you have selected does not appear to be a valid Sage Company Folder.";

        const string TITLE_IS_UNABLE_TO_VERIFY =
              "{0} is unable to verify your login data, as Sage is currently being used in exclusive mode."
                                       + "\nPlease try again later.";

        const string TITLE_AN_UNEXPECTED_ERROR =
              "An unexpected error prevents {0} from verifying your login data."
                                       + "\nPlease verify your settings and try again.";


        const string THE_LICENSE_INFORMATION = "The license information you have entered is not valid.";

        const string SAGE_LINE_50_IS_NOT_INSTALLED = "Sage Line 50 Accounts is not installed.\n\n"
                                      + "{0} can still run {1}.\n"
                                      + "For this to be possible, the registration of certain DLL files is required.\n\n"
                                      + "If you are using an antivirus, firewall or other security software\n"
                                      + "please make sure that you allow or permit the registration,\n"
                                      + "should your security software generate a prompt.";
    }
}