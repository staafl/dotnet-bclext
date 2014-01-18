
namespace Versioning
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using Standardization;
    using DTA;
    using Common;
    using Fairweather.Service;
    using ConnError = Versioning.Sage_Connection_Error;

    public class Work_Space : Sage_Object
    {

        public bool Connected {
            get;
            private set;
        }

        readonly SageDataObject110.WorkSpace ws11;
        readonly SageDataObject120.WorkSpace ws12;
        readonly SageDataObject130.WorkSpace ws13;
        readonly SageDataObject140.WorkSpace ws14;
        readonly SageDataObject150.WorkSpace ws15;
        readonly SageDataObject160.WorkSpace ws16;
        readonly SageDataObject170.WorkSpace ws17;


        Work_Space(object a, int version)
            : base(version) {
            switch (m_version) {
                case 11:
                    ws11 = (SageDataObject110.WorkSpace)a;
                    return;
                case 12:
                    ws12 = (SageDataObject120.WorkSpace)a;
                    return;
                case 13:
                    ws13 = (SageDataObject130.WorkSpace)a;
                    return;
                case 14:
                    ws14 = (SageDataObject140.WorkSpace)a;
                    return;
                case 15:
                    ws15 = (SageDataObject150.WorkSpace)a;
                    return;
                case 16:
                    ws16 = (SageDataObject160.WorkSpace)a;
                    return;
                case 17:
                    ws17 = (SageDataObject170.WorkSpace)a;
                    return;
            }
        }


        public Work_Space(object a, SDO_Engine parent)
            : this(a, parent.Version) {
            this.Parent = parent;


        }

        //public WorkSpace(object a)
        //    : this(a, Dispatch.Version) {

        //}

        // ****************************

        // IMPORTANT: SDO has a habit of leave connection requests for invalid
        // folders hanging - use Validate_Path to prevent this.
        static bool Validate_Path(string path, out bool folder_does_not_exist) {

            //if(!path.EndsWith("ACCDATA"))
            //...

            folder_does_not_exist = false;

            if (File.Exists(path.Cpath("setup.dta")))
                return true;

            folder_does_not_exist = !Directory.Exists(path);

            return false;

        }

        static void Validate_And_Throw(string path) {

            bool no_folder;

            if (!Validate_Path(path, out no_folder)) {
                var ex = new XSage_Conn(no_folder ? ConnError.Folder_Does_Not_Exist :
                                                    ConnError.Invalid_Folder);
                throw ex;
            }

        }

        public void
        Test_Connection(string path, string user, string pwd, string name) {

            // this is superfluous
            // 
            //if (!IsValidUser(path, user, pwd, name)) {
            //    var ex = new SageConnectionException(ConnError.Invalid_Credentials);
            //    throw ex;
            //}

            try {
                Connect(path, user, pwd, true);
            }
            finally {
                Disconnect();
            }

        }

        public bool
        Connect(string path, string user, string pwd, bool check_dir) {

            return Connect(path, user, pwd);
        }

        delegate bool qdta_traverser(string username, string machine_name, bool exclusive);

        InvalidOperationException
        Version_Exception() {
            return new
            InvalidOperationException(
                "Unsupported version: " + m_version);
        }

        /// <summary> 
        /// Traverses and interprets the entries in a sage 'queue.dta', allowing
        /// external logic to gather information about the state of the login queue.
        /// 'qdta' must contain the path to the file including the filename
        /// if 'specific_user' is not null or empty, 'traverser' will be invoked
        /// only for entries whit that username (note: usernames are case insensitive)
        /// 'traverser' is invoked for each relevant record with the following information
        ///     username of the record
        ///     machine name of the record
        ///     whether the record signifies exclusive access
        /// if 'traverser' returns true, the program returns 'true' immediately
        /// otherwise it continues with the next (matching) record
        /// if 'traverser' never returns true, the return value of the function is
        /// 'false'
        /// </summary>
        static bool
        Traverse_Queue_Dta(string qdta,
                           int ver,
                           string specific_user,
                           qdta_traverser traverser) {


            if (!File.Exists(qdta))
                return false;

            var need_specific_user = !specific_user.IsNullOrEmpty();
            specific_user = (specific_user ?? "").ToUpper();

            var ascii = Encoding.ASCII;

            using (var fs = new FileStream(qdta, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(fs)) {

                int
                    /*
                    00 00*/
                username_len =
                    /*[username - */32/*chars, padded with 00s]*/,
                    /*00*/
                machine_name_len =
                    /*[first*/15/*chars of machine name, padded with 00s]*/,
                    /*5F
                    [4 bytes between 30 and 39]
                    [10 arbitrary bytes]
                    [bytes determining lock kind]
                    [other bytes]*/
                record_len =
                    /*Total:*/ver >= 15 ? 100 : 83/*bytes per record */;


                var len = fs.Length;


                for (int ii = 0; ; ++ii) {

                    var next_pos = ii * record_len;

                    if (next_pos > len - record_len)
                        break;

                    fs.Seek(next_pos, SeekOrigin.Begin);

                    var header = br.ReadBytes(2);
                    if (header.Length != 2)
                        break;

                    if (header[0] == header[1] &&
                        header[0] == 0xFF)
                        break; /* eof */

                    var user_bytes = br.ReadBytes(username_len);

                    if (user_bytes.Length != username_len)
                        break;

                    var machine_name_of_rec = "";
                    var user_of_rec = "";
                    var excl = false;

                    if (need_specific_user) {

                        var failed = false;
                        int jj = 0;

                        for (; jj < specific_user.Length; ++jj) {
                            if (user_bytes[jj] != specific_user[jj]) {
                                failed = true;
                                break;
                            }
                        }

                        if (failed)
                            continue;

                        for (; jj < username_len; ++jj) {
                            if (user_bytes[jj] != ' ' &&
                                user_of_rec[jj] != '\0') {
                                failed = true;
                                break;
                            }
                        }

                        if (failed)
                            continue;

                    }


                    user_of_rec = ascii.GetString(user_bytes)
                                       .TrimEnd(' ')
                                       .TrimEnd('\0')
                                       .ToUpper();

                    // br.ReadByte();


                    var mn_bytes = br.ReadBytes(machine_name_len + 1);
                    if (mn_bytes.Length != machine_name_len + 1)
                        break;

                    machine_name_of_rec = ascii.GetString(mn_bytes, 1, machine_name_len)
                                               .ToUpper();

                    // helper methods should not determine policy !
                    // if (!need_specific_user) {


                    // check for exclusive mode

                    const int other_bytes_len = 31;
                    const int excl_bytes_len = 10;
                    const int first_excl_byte = other_bytes_len - excl_bytes_len;

                    var other_bytes = br.ReadBytes(other_bytes_len);

                    if (other_bytes.Length != other_bytes_len)
                        break;

                    // check if the last 'excl_bytes_len' bytes of 'other_bytes'
                    // are as follows:
                    // 0x39, 0x00, 0x00 ... 0x00 (0x00 repeated 10 times)

                    if (other_bytes[first_excl_byte] == 0x39) {
                        excl = true;
                        for (var jj = first_excl_byte + 1; jj < other_bytes_len; ++jj) {
                            if (other_bytes[jj] != 0x00) {
                                excl = false;
                                break;
                            }
                        }
                    }

                    if (traverser(user_of_rec, machine_name_of_rec, excl))
                        return true;

                }

                return false;

            }
        }

        //static bool
        //Logged_In_Exclusive_Mode(string qdta, out string user, out string machine) {

        //    string tmp_user = null;
        //    string tmp_machine = null;

        //    qdta_traverser traverser = (_user, _machine, excl) =>
        //    {
        //        if (!excl)
        //            return false;

        //        tmp_user = _user;
        //        tmp_machine_name = _machine;

        //        return true;
        //    };

        //    var ret = Traverse_Queue_Dta(qdta, null, traverser);

        //    if (ret) {
        //        user = tmp_user;
        //        machine = tmp_machine;
        //    }

        //    return ret;

        //}

        static bool
        Is_User_Logged_In_Or_Sage_In_Exclusive_Mode
        (string qdta, int ver, string user, out string exlusive_mode_user, out string machine) {

            string tmp_machine = null;
            string tmp_exlusive_mode_user = null;

            H.assign(out machine, out exlusive_mode_user);

            bool found = false;
            qdta_traverser traverser = (_user, _machine, _excl) =>
            {
                var _same = _user == user;

                if (!_excl && !_same)
                    return false;

                if (_excl) {
                    // override any results so far
                    tmp_exlusive_mode_user = _user;
                    tmp_machine = _machine;

                }

                if (_same && !found) {
                    // skip all but first record
                    tmp_machine = _machine;
                    found = true;

                }

                return _excl;
            };

            var ret = Traverse_Queue_Dta(qdta, ver, null, traverser);
            ret |= found;

            if (ret) {
                machine = tmp_machine;
                exlusive_mode_user = tmp_exlusive_mode_user;
            }

            return ret;

            // old code, now generalized in Traverse_Queue_Dta

            //machine_name = null;

            //if (!File.Exists(qdta))
            //    return false;

            //using (var fs = new FileStream(qdta, FileMode.Open, FileAccess.Read))
            //using (var br = new BinaryReader(fs)) {

            //    const int
            //        /*
            //        00 00*/
            //    username_len =
            //        /*[username - */32/*chars, padded with 00s]*/,
            //        /*00*/
            //    machine_name_len =
            //        /*[first*/15/*chars of machine name, padded with 00s]*/,
            //        /*5F
            //        [4 bytes between 30 and 39]
            //        [28 other bytes]*/
            //    record_len =
            //        /*Total:*/83/*bytes per record */;

            //    var len = fs.Length;
            //    for (int ii = 0; ; ++ii) {

            //        var next_pos = ii * record_len;

            //        if (next_pos > len - record_len)
            //            break;

            //        fs.Seek(next_pos + 2, SeekOrigin.Begin);

            //        var user2 = br.ReadBytes(username_len);

            //        if (user2.Length != username_len)
            //            continue;

            //        var failed = false;
            //        int jj = 0;

            //        for (; jj < user.Length; ++jj) {
            //            if (user2[jj] != user[jj]) {
            //                failed = true;
            //                break;
            //            }
            //        }

            //        if (failed)
            //            continue;

            //        for (; jj < username_len; ++jj) {
            //            if (user2[jj] != ' ' &&
            //                user2[jj] != '\0') {
            //                failed = true;
            //                break;
            //            }
            //        }

            //        if (failed)
            //            continue;

            //        br.ReadByte();

            //        var mn_bytes = br.ReadBytes(machine_name_len);
            //        machine_name = Encoding.ASCII.GetString(mn_bytes);

            //        return true;


            //    }

            //    return false;

            //}
        }

        static string this_machine_name = Environment.MachineName.ToUpper();

        // I wonder whether the sdo methods here are case-sensitive
        public bool
        Connect(string path, string user, string pwd) {

            path = path.ToUpper();
            user = user.ToUpper();
            pwd = pwd.ToUpper();
            var name = this_machine_name;

            // Console.WriteLine("connecting: {0},{1},{2}", path, pwd, name);

            Validate_And_Throw(path);

            var qdta = path.Cpath("queue.dta");

            var machine_name = "";
            var exclusive_mode_user = "";

            if (Is_User_Logged_In_Or_Sage_In_Exclusive_Mode(
                    qdta,
                    m_version,
                    user,
                    out exclusive_mode_user, out machine_name)) {

                var ini = Data.Ini_File;
                var allow_removing_of_users = (ini == null || ini[DTA_Fields.ALLOW_REMOVING_USERS] == Ini_Main.YES);

                if (exclusive_mode_user.IsNullOrEmpty()) {
                    // user is already logged in
                    if (allow_removing_of_users) {
                        // Sage will pop a dialog
                    }
                    else {
                        var msg = "User {0} is logged in on machine {1}.\n\nLogin cannot proceed.".spf(user, machine_name);

                        var ex = new XSage_Conn(msg, Sage_Connection_Error.Username_In_Use_Cant_Remove);

                        ex.User = user;
                        ex.Workstation = machine_name;

                        true.tift(ex);
                    }
                }
                else {
                    // exclusive mode
                    var msg = "User {0} is logged in on machine {1} in exclusive mode.\n\nLogin cannot proceed.".
                        spf(exclusive_mode_user, machine_name);

                    var ex = new XSage_Conn(msg, Sage_Connection_Error.Exclusive_Mode);

                    ex.User = exclusive_mode_user;
                    ex.Workstation = machine_name;

                    true.tift(ex);
                }


            }

            //var txt = File.ReadAllText(qdta);
            //if (txt.Contains(user.ToUpper())) {
            //    var ex = new SageConnectionException(Sage_Connection_Error.Username_In_Use);
            //    true.tift(ex);

            //}

            var ret = false;
            try {
                switch (m_version) {
                    case 11:
                        ret = ws11.Connect(path, user, pwd, name); break;
                    case 12:
                        ret = ws12.Connect(path, user, pwd, name); break;
                    case 13:
                        ret = ws13.Connect(path, user, pwd, name); break;
                    case 14:
                        ret = ws14.Connect(path, user, pwd, name); break;
                    case 15:
                        ret = ws15.Connect(path, user, pwd, name); break;
                    case 16:
                        ret = ws16.Connect(path, user, pwd, name); break;
                    case 17:
                        ret = ws17.Connect(path, user, pwd, name); break;
                }
            }
            catch (AccessViolationException ex) { /* might as well be wrong version */
                var ex1 =
                    new XSage_Conn(
                        ex.Message,
                        Sage_Connection_Error.Unspecified);
                true.tift(ex1);
            }
            catch (COMException ex) {
                var ex1 = new XSage_Conn(ex, Parent.Last_Error);
                true.tift(ex1);
            }
            Connected = ret;
            return ret;

        }

        public void
        Disconnect() {
             if (Connected) {
                switch (m_version) {
                    case 11:
                        ws11.Disconnect();
                        break;
                    case 12:
                        ws12.Disconnect();
                        break;
                    case 13:
                        ws13.Disconnect();
                        break;
                    case 14:
                        ws14.Disconnect();
                        break;
                    case 15:
                        ws15.Disconnect();
                        break;
                    case 16:
                        ws16.Disconnect();
                        break;
                    case 17:
                        ws17.Disconnect();
                        break;
                }
                Connected = false;
            }
        }

        /// <summary>
        /// Throws: SageConnectionException
        /// </summary>
        public bool
        IsValidUser(string path, string user, string pwd) {

            var name = this_machine_name;
            Validate_And_Throw(path);

            try {
                switch (m_version) {
                    case 11:
                        return ws11.IsValidUser(path, user, pwd, name);
                    case 12:
                        return ws12.IsValidUser(path, user, pwd, name);
                    case 13:
                        return ws13.IsValidUser(path, user, pwd, name);
                    case 14:
                        return ws14.IsValidUser(path, user, pwd, name);
                    case 15:
                        return ws15.IsValidUser(path, user, pwd, name);
                    case 16:
                        return ws16.IsValidUser(path, user, pwd, name);
                    case 17:
                        return ws17.IsValidUser(path, user, pwd, name);
                }
            }
            catch (COMException ex) {
                var ex1 = new XSage_Conn(ex, Parent.Last_Error);
                if (ex1.Connection_Error == Sage_Connection_Error.Invalid_Credentials)
                    return false;
                throw;
            }
            return false;
        }

        public SDO_Engine Parent {
            get;
            set;
        }



        // ****************************



        // ****************************


        public object
        Create(string name) {

            object inner = null;
            switch (m_version) {
                case 11:
                    inner = ws11.CreateObject(name); break;
                case 12:
                    inner = ws12.CreateObject(name); break;
                case 13:
                    inner = ws13.CreateObject(name); break;
                case 14:
                    inner = ws14.CreateObject(name); break;
                case 15:
                    inner = ws15.CreateObject(name); break;
                case 16:
                    inner = ws16.CreateObject(name); break;
                case 17:
                    inner = ws17.CreateObject(name); break;
            }

            try {
                var ret = type_data.Ctor(name)(inner, m_version);
                return ret;
            }
            catch (KeyNotFoundException) {
                true.tift("bad ctor string {0}: ".spf(name, m_version));
                return null;

            }

        }

        public object
        Create(Type t) {
            try {
                return Create(type_data.String(t));
            }
            catch (KeyNotFoundException) {
                true.tift("bad type {0}: ".spf(t.Name));
                return null;
            }
        }

        public T
        Create<T>()
            where T : Sage_Object {
            try {
                return (T)Create(type_data.String(typeof(T)));
            }
            catch (KeyNotFoundException) {
                true.tift("bad type {0}: ".spf(typeof(T).Name));
                return null;
            }


            /*       Old code, commented out        */

            #region
            //var pair = data[typeof(TContainer)];

            //string cons_string = pair.First;
            //object inner;

            //switch (m_version) {
            //    case 11:
            //        inner = ws11.Create(cons_string);
            //        break;
            //    case 12:
            //        inner = ws12.Create(cons_string);
            //        break;
            //    case 13:
            //        inner = ws13.Create(cons_string);
            //        break;
            //    case 14:
            //        inner = ws14.Create(cons_string);
            //        break;
            //    case 15:
            //        inner = ws15.Create(cons_string);
            //        break;
            //    case 16:
            //        inner = ws16.Create(cons_string);
            //        break;
            //    case 17:
            //        inner = ws17.Create(cons_string);
            //        break;
            //    default:
            //        throw new InvalidOperationException("Unsupported version: " + m_version.ToString());
            //}

            //var ret = pair.Second(inner);

            // return ret; 
            #endregion
        }


        // ****************************

        /*       Project costing        */

        public Project
        CreateProject() {
            switch (m_version) {
                case 12:
                    return new Project(ws12.CreateProject(), m_version);
                case 13:
                    return new Project(ws13.CreateProject(), m_version);
                case 14:
                    return new Project(ws14.CreateProject(), m_version);
                case 15:
                    return new Project(ws15.CreateProject(), m_version);
                case 16:
                    return new Project(ws16.CreateProject(), m_version);
                case 17:
                    return new Project(ws17.CreateProject(), m_version);
                default:
                    throw Version_Exception();
            }
        }

        public Projects 
        CreateProjects() {
            switch (m_version) {
                case 12:
                    return new Projects(ws12.CreateProjects(), m_version);
                case 13:
                    return new Projects(ws13.CreateProjects(), m_version);
                case 14:
                    return new Projects(ws14.CreateProjects(), m_version);
                case 15:
                    return new Projects(ws15.CreateProjects(), m_version);
                case 16:
                    return new Projects(ws16.CreateProjects(), m_version);
                case 17:
                    return new Projects(ws17.CreateProjects(), m_version);
                default:
                    throw Version_Exception();
            }

        }

        public ProjectCostCode
        CreateProjectCostCode() {
            switch (m_version) {
                case 12:
                    return new ProjectCostCode(ws12.CreateProjectCostCode(), m_version);
                case 13:
                    return new ProjectCostCode(ws13.CreateProjectCostCode(), m_version);
                case 14:
                    return new ProjectCostCode(ws14.CreateProjectCostCode(), m_version);
                case 15:
                    return new ProjectCostCode(ws15.CreateProjectCostCode(), m_version);
                case 16:
                    return new ProjectCostCode(ws16.CreateProjectCostCode(), m_version);
                case 17:
                    return new ProjectCostCode(ws17.CreateProjectCostCode(), m_version);
                default:
                    throw Version_Exception();
            }
        }


        // ****************************

        static readonly Sage_Type_Dict
        type_data = new Sage_Type_Dict
        {
            // {"###", typeof(###), inner => new ###(inner)},
            #region

            #if SCREENS
            {"InvoiceData", typeof(InvoiceData), (inner, version) => new InvoiceData(inner, version)},
            #endif

            {"InvoiceRecord", typeof(InvoiceRecord), (inner, version) => new InvoiceRecord(inner, version)},

            {"SalesRecord", typeof(SalesRecord),  (inner, version) => new SalesRecord(inner, version)},
            {"StockRecord", typeof(StockRecord),  (inner, version) => new StockRecord(inner, version)},
            {"BankRecord", typeof(BankRecord),  (inner, version) => new BankRecord(inner, version)},
            {"PurchaseRecord", typeof(PurchaseRecord),  (inner, version) => new PurchaseRecord(inner, version)},
            {"NominalRecord", typeof(NominalRecord),  (inner, version) => new NominalRecord(inner, version)},
            {"RemittanceRecord", typeof(RemittanceRecord),  (inner, version) => new RemittanceRecord(inner, version)},
            {"CountryCodeRecord", typeof(CountryCodeRecord),  (inner, version) => new CountryCodeRecord(inner, version)},

            {"StockCategory", typeof(StockCategory),  (inner, version) => new StockCategory(inner, version)},

            {"CurrencyData", typeof(CurrencyData),  (inner, version) => new CurrencyData(inner, version)},
            {"DepartmentData", typeof(DepartmentData),  (inner, version) => new DepartmentData(inner, version)},
            {"ControlData", typeof(ControlData),  (inner, version) => new ControlData(inner, version)},
            {"UsageData", typeof(UsageData),  (inner, version) => new UsageData(inner, version)},
            {"SalesData", typeof(SalesData),  (inner, version) => new SalesData(inner, version)},
            {"NominalData", typeof(NominalData),  (inner, version) => new NominalData(inner, version)},
            {"SplitData", typeof(SplitData),  (inner, version) => new SplitData(inner, version)},
            {"StockData", typeof(StockData),  (inner, version) => new StockData(inner, version)},
            {"RemittanceData", typeof(RemittanceData),  (inner, version) => new RemittanceData(inner, version)},
            {"SetupData", typeof(SetupData),  (inner, version) => new SetupData(inner, version)},
            {"HeaderData", typeof(HeaderData),  (inner, version) => new HeaderData(inner, version)},
            {"InvoiceData", typeof(InvoiceData),  (inner, version) => new InvoiceData(inner, version)},

            {"StockIndex", typeof(StockIndex),  (inner, version) => new StockIndex(inner, version)},
            {"RemittanceIndex", typeof(RemittanceIndex),  (inner, version) => new RemittanceIndex(inner, version)},
            {"PurchaseIndex", typeof(PurchaseIndex),  (inner, version) => new PurchaseIndex(inner, version)},
            {"SalesIndex", typeof(SalesIndex),  (inner, version) => new SalesIndex(inner, version)},
            {"InvoiceIndex", typeof(InvoiceIndex),  (inner, version) => new InvoiceIndex(inner, version)},
            {"BankIndex", typeof(BankIndex),  (inner, version) => new BankIndex(inner, version)},
            {"NominalIndex", typeof(NominalIndex),  (inner, version) => new NominalIndex(inner, version)},


            {"InvoicePost", typeof(InvoicePost),  (inner, version) => new InvoicePost(inner, version)},
            {"InvoiceItem", typeof(InvoiceItem),  (inner, version) => new InvoiceItem(inner, version)},

            {"TransactionPost", typeof(TransactionPost),  (inner, version)  => new TransactionPost(inner, version) }, 
	#endregion

        };

        delegate Sage_Object Sage_Object_Producer(object inner, int version);

        class Sage_Type_Dict : IEnumerable, IEnumerable<Triple<string, Type, Sage_Object_Producer>>
        {
            const int capacity = 100;

            public Sage_Type_Dict() { }


            public IEnumerator<Triple<string, Type, Sage_Object_Producer>>
            GetEnumerator() {

                foreach (var kvp in dict1) {
                    yield return Triple.Make(kvp.Key, kvp.Value.First, kvp.Value.Second);
                }

            }

            IEnumerator
            IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

            readonly Dictionary<string, Pair<Type, Sage_Object_Producer>>
            dict1 = new Dictionary<string, Pair<Type, Sage_Object_Producer>>(capacity);

            readonly Dictionary<Type, Pair<string, Sage_Object_Producer>>
            dict2 = new Dictionary<Type, Pair<string, Sage_Object_Producer>>(capacity);

            public void
            Add(string str, Type type, Sage_Object_Producer func) {

                dict1.Add(str, Pair.Make(type, func));
                dict2.Add(type, Pair.Make(str, func));

            }

            public Sage_Object_Producer
            Ctor(string str) {
                return dict1[str].Second;
            }

            public Sage_Object_Producer
            Ctor(Type type) {
                return dict2[type].Second;
            }

            public Type
            Type(string str) {
                return dict1[str].First;
            }

            public string
            String(Type type) {
                return dict2[type].First;
            }


        }
    }
}