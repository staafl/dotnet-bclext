using System;
using System.Collections.Generic;
using DTA;
using Fairweather.Service;
using Versioning;

namespace Common
{
    public class Sage_Access
    {
        public Sage_Access(string path, Credentials creds, int version)
            : this(path, creds, version, Data.Default_Machine_Name) {

        }

        public Sage_Access(string path, Credentials creds, int version, string machine) {

            this.Creds = creds;
            this.Version = version;
            this.Path = path;
            this.Machine = machine;

        }

        public string Path {
            get;
            private set;
        }

        public Credentials Creds {
            get;
            private set;
        }

        public string Username {
            get { return Creds.Username; }
        }

        public string Password {
            get { return Creds.Password; }
        }

        public Company_Number Company {
            get { return Creds.Company; }
        }

        public int Version {
            get;
            private set;
        }

        public string Machine {
            get;
            set;
        }

        public Sage_Connection Conn {
            get;
            private set;
        }

        public SDO_Engine Sdo {
            get {
                return Conn.sdo;
            }
        }

        public Work_Space WS {
            get {
                return Conn.ws;
            }
        }

        public IDisposable
        Establish_Connection() {

            var new_conn = new Sage_Connection(this.Username, this.Password, this.Path, this.Version, this.Machine);

            var old_conn = Conn;

            Conn = new_conn;

            return new On_Dispose(() =>
            {
                Conn.Try_Dispose();
                Conn = old_conn ?? new_conn;
            });

        }


        public void
        Test_Connection() {

            if (Connected)
                return;

            var sdo = new SDO_Engine(Version);

            var ws = sdo.WSAdd(Data.Workspace);

            ws.Test_Connection(Path, Username, Password, Machine);

        }

        public bool Connected {
            get {
                var tmp = Conn;
                return tmp != null && tmp.Connected;
            }
        }

        public string
        Get_Last_Sage_Error() {
            using (Establish_Connection()) {
                return Sdo.Last_Error_Text;
            }
        }

        /*       Sage Access        */

        public IDisposable
        Attempt_Transaction() {
            bool _;
            return Attempt_Transaction(out _);
        }

        public IDisposable
        Attempt_Transaction(out bool ok) {
            return Attempt_Transaction(true, out ok);
        }

        public IDisposable
        Attempt_Transaction(bool attempt_retry, out bool ok) {

            IDisposable ret = null;

            ok = Sage_Guard(() => ret = Establish_Connection(),
                            attempt_retry);

            return ok ? ret : null;

        }

        public bool
        Test_And_Prompt() {
            return Sage_Guard(this.Test_Connection, cst_sage_guard_def_allow_retry);
        }

        public bool
        Test(bool allow_retry) {
            return Sage_Guard(this.Test_Connection, allow_retry);
        }


        // ****************************

        public static T
        Get_Value<T>(Func<T> f, T def, int? retries) {

            T ret = default(T);

            if (!Repeat_Until_Success(() => ret = f(), retries))
                ret = def;

            return ret;

        }

        public static T
        Get_Value<T>(Func<T> f) {

            var ret = default(T);

            if (!Repeat_Until_Success(() => ret = f()))
                ret = default(T);

            return ret;


        }

        public static bool
        Repeat_Until_Success(Action act) {
            return Repeat_Until_Success(act, null);
        }

        public static bool
        Repeat_Until_Success(Action act, int? max_iters) {

            int ii = 0;
            while (true) {
                if (Sage_Guard(act))
                    return true;

                if (max_iters.HasValue && max_iters.Value > ii)
                    return false;
                ++ii;
            }
        }


        const bool cst_sage_guard_def_allow_retry = true;

        public static bool
        Sage_Guard(Action act) {
            return Sage_Guard(act, cst_sage_guard_def_allow_retry);
        }

        public static bool
        Sage_Guard(Action act, bool allow_retry) {
            return Sage_Guard(act, ex => Exceptions.Handle_Sage_X(ex, allow_retry));
        }

        public static bool
        Sage_Guard(Action act, Func<XSage, bool> handler) {

            while (true) {
                try {
                    act();
                    return true;
                }
                catch (XSage ex) {
                    if (handler != null && handler(ex))
                        continue;
                    return false;
                }
            }
        }


    }
}
