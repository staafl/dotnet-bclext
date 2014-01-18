using System;
using System.Collections.Generic;
using Fairweather.Service;
using Versioning;

namespace Common
{


    public class Sage_Connection : IDisposable
    {

        static Dictionary<Connection_Data, SDO_Engine> engines = new Dictionary<Connection_Data, SDO_Engine>();
        static Dictionary<Connection_Data, Work_Space> workspaces = new Dictionary<Connection_Data, Work_Space>();

        public SDO_Engine sdo {
            get {
                return engines.Get_Or_Default(Data, () => null);
            }
            private set {
                engines[Data] = value;
            }
        }

        public Work_Space ws {
            get {
                return workspaces.Get_Or_Default(Data, () => null);
            }
            private set {
                workspaces[Data] = value;
            }
        }

        static public string Workspace_Name = "Sage Data Objects";

        int EnsureConnectionEstablished() {

            var ret = Ok;

            if (sdo == null) {
                sdo = new SDO_Engine(Data.Version);
                ret |= EngineNotInstantiated;

            }

            if (ws == null) {
                ws = sdo.WSAdd(Workspace_Name);
                ret |= WorkspaceNotInstantiated;

            }

            if (!ws.Connected) {
                ws.Connect(Data.Path, Data.Username, m_pass);
                ret |= ConnectionNotEstablished;

            }

            connected = true;

            return ret;

        }

        void EnsureConnectionRestored(int state) {

            if ((state & ConnectionNotEstablished) == ConnectionNotEstablished) {
                if (ws != null)
                    ws.Disconnect();
            }

            if ((state & WorkspaceNotInstantiated) == WorkspaceNotInstantiated) {
                ws = null;
            }

            if ((state & EngineNotInstantiated) == EngineNotInstantiated) {
                // Last_Seen_Error = sdo.Last_Error_Text;
                sdo = null;
            }

            connected = false;
        }

        const int Ok = 0;
        const int EngineNotInstantiated = 1;
        const int WorkspaceNotInstantiated = 2;
        const int ConnectionNotEstablished = 4;


        //public string Last_Seen_Error {
        //    get;
        //    private set;
        //}

        bool connected;

        public bool Connected {
            get {
                return connected;
            }
        }

        readonly int m_state;

        /// <summary>
        /// Immutable;
        /// </summary>
        public Connection_Data Data {
            get;
            set;
        }

        readonly string m_pass;
        readonly string m_name;

        public string Name {
            get {
                return m_name;
            }
        }

        public Sage_Connection(string username, string password, string path, int version, string name) {

            this.Data = new Connection_Data(username, path, version);

            this.m_pass = password;
            this.m_name = name;

            this.m_state = EnsureConnectionEstablished();


        }

        public string Path {
            get {
                return Data.Path;
            }
        }

        public void Dispose() {

            EnsureConnectionRestored(m_state);

            //GC.SuppressFinalize(this);
        }

        //~Sage_Connection() {

        //    //EnsureConnectionRestored(m_state);

        //}

    }
}