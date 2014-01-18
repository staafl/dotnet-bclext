
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Common;

using System.Linq;
using Fairweather.Service;

using Conn_Error = Versioning.Sage_Connection_Error;

using mbb = System.Windows.Forms.MessageBoxButtons;
using mbdb = System.Windows.Forms.MessageBoxDefaultButton;
using mbi = System.Windows.Forms.MessageBoxIcon;

/*       All future control standardization efforts go here        */
namespace Standardization
{
    static partial class Standard
    {

        static public bool Dim_Form_Always {
            get;
            set;
        }


        /*       Shorthands        */


        public static void
        Tell(string msg, params object[] args) {
            Standard.Show(Message_Type.Info, msg.Maybe_Format(args));

        }

        public static void
        Warn(string msg, params object[] args) {
            Standard.Show(Message_Type.Warning, msg.Maybe_Format(args));

        }

        public static void
        Error(string msg, params object[] args) {
            Standard.Show(Message_Type.Error, msg.Maybe_Format(args));

        }

        public static bool
        Ask(string msg, params object[] args) {

            return (Show(Message_Type.Question, msg.Maybe_Format(args)) == DialogResult.Yes);

        }


        // ****************************


        public static DialogResult
        Show(this Message_Type type, string message) {

            return Show(type, message, null, null, null, null);
        }
        public static DialogResult
        Show(this Message_Type type, string message, mbb buts) {

            return Show(type, message, null, buts, null, null);
        }
        public static DialogResult Show(this Message_Type type, string message, string title) {
            mbb? buts = null;
            return Show(type, message, title, buts);
        }
        public static DialogResult Show(this Message_Type type, string message, string title, mbb? buts) {
            mbi? ico = null;
            return Show(type, message, title, buts, ico);
        }
        public static DialogResult Show(this Message_Type type, string message, string title, mbb? buts, mbi? ico) {
            mbdb? defbut = null;
            return Show(type, message, title, buts, ico, defbut);
        }
        public static DialogResult
        Show(this Message_Type type, string message, string title, mbb? buts, mbi? ico, mbdb? defbut) {
            return Show(type, null, message, title, buts, ico, defbut);
        }

        // ****************************

        public static DialogResult Show(this Message_Type type, Form form, string message) {
            return Show(type, form, message, null);
        }
        public static DialogResult Show(this Message_Type type, Form form, string message, string title) {
            mbb? buts = null;
            return Show(type, form, message, title, buts);
        }
        public static DialogResult Show(this Message_Type type, Form form, string message, string title, mbb? buts) {
            mbi? ico = null;
            return Show(type, form, message, title, buts, ico);
        }
        public static DialogResult Show(this Message_Type type, Form form, string message, string title, mbb? buts, mbi? ico) {
            mbdb? defbut = null;
            return Show(type, form, message, title, buts, ico, defbut);
        }
        // ****************************

        public static DialogResult
        Show(this Message_Type type, Form form, string message, string title, mbb? buts, mbi? ico, mbdb? defbut) {
            DialogResult ret;

            /*       I hate switches --Velko        */

            var quad = display_info[type];

            var quadFirst = title ?? quad.First();
            var quadSecond = buts ?? quad.Second;
            var quadThird = ico ?? quad.Third;
            var quadFourth = defbut ?? quad.Fourth;

            ret = MessageBox_Show(form, message, quadFirst, quadSecond, quadThird, quadFourth);

            return ret;

        }



        public static DialogResult
        Show(this Named_Message named_message) {
            return Show(named_message, null);
        }

        public static DialogResult
        Show(this Named_Message named_message, params object[] args) {

            string message;
            Message_Type type;

            args = args ?? new object[0];

            bool ok = Get_Message_And_Type(named_message, args, out  message, out  type);

            ok.tiff();

            var ret = Show(type, message);

            return ret;
        }


        internal static string title {
            get {
                string ret = Data.Default_Title;

                return ret;
            }
        }

        internal static string error_title {
            get {
                var ret = "{0} - Error".spf(Data.App_Name);

                return ret;
            }
        }

        internal static string sys_error_title {
            get {
                var ret = "{0} - System Error".spf(Data.App_Name);

                return ret;
            }
        }

        internal static string warning_title {
            get {
                var ret = "{0} - Warning".spf(Data.App_Name);

                return ret;
            }
        }

        internal static string attention_title {
            get {
                var ret = "{0} - Attention".spf(Data.App_Name);

                return ret;
            }
        }

        const string success_title = "Success";


        static public Form Message_Box_Owner {
            get {
                var form = Form.ActiveForm;
                if (form != null && form.IsDisposed)
                    form = null;
                return form;
            }
        }

        class dict : Quad_Dict<Message_Type, Func<string>, mbb, mbi, mbdb>
        {
            public dict() { }

            public void Add(Message_Type type, Func<string> f, mbb b, mbi i) {

                base.Add(type, Quad.Make(f, b, i, mbdb.Button1));

            }
        }



        static DialogResult
        MessageBox_Show(Form form, string message, string title, mbb buts, mbi ico, mbdb defbut) {

            var frm = H.Get_Top_Form();

            using (frm == null ? null : Dim_Form_Always ? frm.Dim_Form() : null) {
                var ret = MessageBox.Show(form ?? Message_Box_Owner, message, title, buts, ico, defbut);

                return ret;
            }

        }



        static dict display_info = new dict
        {
            {Message_Type.Error, () => error_title, mbb.OK, mbi.Error},
            {Message_Type.System_Error, () => sys_error_title, mbb.OK, mbi.Error},
            {Message_Type.Warning, () => warning_title, mbb.OK, mbi.Warning},
            {Message_Type.Warning_Yes_No, () => warning_title, mbb.YesNo, mbi.Warning},
            {Message_Type.Warning_Retry_Cancel, () => warning_title, mbb.RetryCancel, mbi.Warning},

            {Message_Type.Attention_OK_Cancel, () => attention_title, mbb.OKCancel, mbi.Information},
            {Message_Type.Success, () => success_title, mbb.OK, mbi.Asterisk},
            {Message_Type.Question, () => title, mbb.YesNo, mbi.Question, mbdb.Button2},
            {Message_Type.Info, () => title, mbb.OK, mbi.Information},
            {Message_Type.Request, () => title, mbb.OKCancel, mbi.Information},
            {Message_Type.Abort_Retry_Ignore, () => warning_title, mbb.AbortRetryIgnore, mbi.Warning},
        };





        public static DialogResult
        Maybe_Show_Connection_Error(XSage_Conn ex) {
            return Maybe_Show_Connection_Error(ex, Message_Box_Owner, false);
        }

        public static DialogResult
        Maybe_Show_Connection_Error(XSage_Conn ex, bool allow_retry) {
            return Maybe_Show_Connection_Error(ex, Message_Box_Owner, allow_retry);
        }

        public static DialogResult
        Maybe_Show_Connection_Error(
            XSage_Conn ex,
            Form owner,
            bool allow_retry) {

            var error = ex.Connection_Error;

            if (error.Show_Msg()) {

                var def =
                "Connection to the Sage Data Files could not be established.\n\n" + ex.Message;

                string msg;
                bool can_retry = false;
                Func<bool> retry = () => allow_retry && can_retry;

                if (!ex.Friendly_Message(out msg, out can_retry)) {

                    msg = connection_messages.Get_Or_Default_(error, def, false);

                    can_retry = error.Can_Retry();

                    if (retry()) {
                        // msg += "\n\nWould you like to retry?";
                        
                    }
                    else if (error.Try_Again_Later()) {
                        msg += "\n\nPlease try again later.";

                    }
                }

                var type = retry() ?
                           Message_Type.Warning_Retry_Cancel :
                           Message_Type.Error;

                var ret = Show(type, msg);

                if (type == Message_Type.Error)
                    ret = DialogResult.Cancel;

                return ret;
            }
            else {
                return DialogResult.Cancel;
            }

        }

        static readonly Dictionary<Conn_Error, string>
        connection_messages = new Dictionary<Conn_Error, string> 
            {
                #region
		  {Conn_Error.Folder_Does_Not_Exist,
                    "The directory you are using to connect to Sage does not exist."},

                  {Conn_Error.Invalid_Folder,
                    "The directory you are using does not appear to be a valid Sage company folder."},
                        
                  {Conn_Error.Invalid_Credentials, 
                    "Unable to login to Sage." +
                    "\n\nPlease make sure that a user with the selected username exists, " +
                    "and that the selected password is correct."},

                  {Conn_Error.Username_In_Use_Generic, 
                  "The program cannot log you in at this time - the selected username is already in use."},

                  {Conn_Error.Exclusive_Mode, 
                  "Unable to access the Sage data as the program is being used in Exclusive Mode."},

                  {Conn_Error.SDO_Not_Registered,
                  "Unable to login to Sage as 3rd Party Integration is not currently enabled." +
                  "\n\nPlease register Sage Data Objects and try again."}, 
	#endregion
            };



    }
}