using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Fairweather.Service;
using Versioning;

namespace Common
{
    [global::System.Serializable]
    public class XSage_Conn : XSage
    {
        const string STR_User = "user";
        const string STR_Workstation = "workstation";
        const string STR_dataset = "dataset";

        public XSage_Conn() { }
        public XSage_Conn(string message) : base(message) { }

        public XSage_Conn(Sage_Connection_Error error)
            : base() {
            this.Connection_Error = error;
        }

        public XSage_Conn(string message, Sage_Connection_Error error)
            : base(message) {
            this.Connection_Error = error;
        }

        public XSage_Conn(COMException comex, Sage_Error error)
            : base(comex, error) {

            Connection_Error = errors.Get_Or_Default_(error, (Sage_Connection_Error)0);

            if (Connection_Error == 0)
                Connection_Error = Get_From_Message(Message.ToUpper());

        }

        static Sage_Connection_Error
        Get_From_Message(string trouble) {

            return (from kvp in messages
                    where trouble.Contains(kvp.Key)
                    select kvp.Value).FirstOrDefault(Sage_Connection_Error.Unspecified);

        }

        protected XSage_Conn(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        // ****************************

        public bool
        Friendly_Message(out string msg, out bool retry) {

            H.assign(out msg, out retry);

            var user = User;

            if (user.IsNullOrEmpty())
                return false;

            var workstation = Workstation;
            var on_machine = ""; // if not nullorempty - has leading space and no trailing space    

            if (!workstation.IsNullOrEmpty()) {
                var same = workstation.ToUpper() == Environment.MachineName.ToUpper();
                on_machine = same ?
                             " on your machine" :
                             " on machine '" + Workstation + "'";
            }

            if (this.Connection_Error == Sage_Connection_Error.Username_In_Use_Cant_Remove) {

                msg =
                    //*
                "Sage indicates that the username which {1} uses"
                + " to connect to the data is currently active{2}."
                + "\n\nPlease make sure that the user '{0}' is logged out before proceeding.";

                msg = msg.spf(user, Common.Data.App_Name, on_machine);

                /*/
                     
                "User '{0}' is currently logged in{1}.\n\nLogin cannot proceed.";
                    
                msg = msg.spf(user, on_machine);
                    
                //*/

            }
            else if (this.Connection_Error == Sage_Connection_Error.Exclusive_Mode) {

                msg =
"Sage indicates that the data set is currently being used in exclusive mode{0}." +
"\n\nPlease resolve this before proceeding.";

                msg = msg.spf(on_machine);


            }
            else {

                return false;

            }

            retry = true;

            return true;

        }

        public Sage_Connection_Error Connection_Error { get; set; }

        string Maybe_Get(string key) {
            var data = Data;
            if (data == null)
                return null;
            if (!data.Contains(key))
                return null;
            return data[key].strdef(null);
        }
        public string Dataset {
            get {
                return Maybe_Get(STR_dataset);
            }
            set {
                Data[STR_dataset] = value;
            }
        }
        public string User {
            get {
                return Maybe_Get(STR_User);
            }
            set {
                Data[STR_User] = value;
            }
        }

        public string Workstation {
            get {
                return Maybe_Get(STR_Workstation);
            }
            set {
                Data[STR_Workstation] = value;
            }
        }


        // ****************************

        static readonly Dictionary<Sage_Error, Sage_Connection_Error>
        errors = new Dictionary<Sage_Error, Sage_Connection_Error> 
		        {
		        #region
		        {Sage_Error.sdoLogonPassword, Sage_Connection_Error.Invalid_Credentials},

		        {Sage_Error.sdoBadDataPath, Sage_Connection_Error.Invalid_Folder},

		        {Sage_Error.sdoLogonNameInUse, Sage_Connection_Error.Username_In_Use_Generic},

		        {Sage_Error.sdoLogonExclusive, Sage_Connection_Error.Exclusive_Mode},

		        {Sage_Error.sdoNotRegistered, Sage_Connection_Error.SDO_Not_Registered},
		        #endregion
		        };

        /// TODO: Have a look at the actual error codes
        static readonly Dictionary<string, Sage_Connection_Error>
        messages = new Dictionary<string, Sage_Connection_Error> 
		        {
		        #region
		        {"INVALID OR INCORRECT USERNAME OR PASSWORD",
		        Sage_Connection_Error.Invalid_Credentials},

		        {"THE CREDENTIALS YOU HAVE ENTERED APPEAR TO BE INVALID",
		        Sage_Connection_Error.Invalid_Credentials},

		        {"USERNAME IS ALREADY IN USE", 
		        Sage_Connection_Error.Username_In_Use_Generic},

		        {"EXCLUSIVE MODE", 
		        Sage_Connection_Error.Exclusive_Mode},

		        {"SAGE DATA OBJECTS IS NOT REGISTERED.", 
		        Sage_Connection_Error.SDO_Not_Registered},
		        #endregion
		        };

        public override string ToString() {

            var ret = base.ToString() + "\r\n" + "Connection_Error: " + Connection_Error.ToString();
            return ret;

        }

    }
}
