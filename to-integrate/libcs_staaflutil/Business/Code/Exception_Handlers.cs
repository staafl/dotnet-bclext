using System;
using System.Windows.Forms;

using DTA;
using Fairweather.Service;
using Standardization;


namespace Common
{
    // moved from screens/code/$exceptionhandling.cs
    // in rev 1017

    public static class Exceptions
    {

        public static void Dispatch_X(Exception ex) {

            if (ex == null)
                return;

            Logging.Notify(ex);

            if (ex is XSage) {

                Handle_Sage_X((XSage)ex, false);
                return;

            }

            if (ex is XIni) {

                Handle_XIni((XIni)ex);
                return;

            }

            if (ex is Exception) {

                Handle_General_X(ex);
                return;

            }


        }


        static bool simulator_error_shown = false;
        static public void Handle_General_X(Exception ex) {

            var msg = "There was an unexpected error.";

            //"\n\nPlease make sure that the configured data path contains " +
            //"data files of the appropriate version of Sage.";

            var details = ex.Msg_Or_Type();
            msg += "\n\nError details: " + details;

            if (ex is InvalidOperationException) {
                if (ex.Message.Contains("Invoke")) {
                    // check if we've run into the BeginInvoke bug with the Pos Printer Simulator

                    if (Data.App == App.Ses_Cfg) {
                        simulator_error_shown = true; // no need to bother
                        if (simulator_error_shown)
                            return;
                        msg = "It appears that the Microsoft Pos Printer Simulator has crashed. You may need to restart the application prior to using OPOS printing again.";
                        simulator_error_shown = true;

                    }
                }
            }


            Standard.Show(Message_Type.Error, msg);


        }

        // ****************************

        static public bool Handle_Sage_X(XSage ex, bool allow_retry) {

            if (ex is XSage_Conn) {
                return Handle_XSage_Conn(ex as XSage_Conn, allow_retry);
            }
            else if (ex is XSage_Logic) {
                Handle_XSage_Logic(ex as XSage_Logic);
                return false;
            }
            else {
                Handle_General_X(ex);
                return false;
            }

        }

        static void Handle_XSage_Logic(XSage_Logic ex) {

            string error_message = "An unexpected error occurred.\n";

            if (!ex.Message.IsNullOrEmpty()) {
                error_message += "\n";
                error_message += "Last Error Message reported by Sage: " + ex.Message;

            }

            if (ex.Last_Transaction.OrDefault_(0) != 0) {
                error_message += "\n";
                error_message += "\nLast Sage transaction number: " + ex.Last_Transaction.Value.ToString();

            }

            Standard.Show(Message_Type.System_Error, error_message);

        }

        static bool Handle_XSage_Conn(XSage_Conn ex, bool allow_retry) {

            if (!Standard.Maybe_Show_Connection_Error(ex, allow_retry).Positive())
                return false;

            if (!allow_retry)
                return false;


            return true;


        }

        // ****************************

        static public void Handle_XIni(XIni ex) {

            string msg;
            var type = ex.Type;

            if (type == XIni.Fault_Type.File_Missing) {
                msg = "The configuration file SES.DTA is missing.\n\n"
                                 + "Program cannot continue.";
            }
            else if (type == XIni.Fault_Type.File_Corrupted) {
                msg = "Configuration error.\n\n"
                    + "Please make sure that the configuration files are in place,\n"
                    + "and have not been modified.";
            }
            else {
                msg = ex.Message;
                return;
            }

            Standard.Show(Message_Type.Error, msg);


        }



    }
}