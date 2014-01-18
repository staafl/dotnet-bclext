//#define SES_BENCH

using System;
using System.Collections.Generic;
using DTA;
using Fairweather.Service;
using Versioning;
namespace Common
{
    // Fields, Ctor and Helper functions
    public partial class Sage_Data_Records
    {



        public static bool
        Make(Ini_File ini,
             Company_Number number,
             out Sage_Data_Records sdr) {

            var proxy = ini.Group_ro(number.As_String);

            var username = proxy[DTA_Fields.USERNAME];
            var password = proxy[DTA_Fields.PASSWORD];

            SageConnectionException _;

            var credentials = new Credentials(number, username, password);

            var ret = Make(ini, credentials, out sdr, out _);

            return ret;

        }

        public static bool
        Make(Ini_File ini,
             Credentials credentials,
             out Sage_Data_Records sdr,
             out SageConnectionException exception) {



            sdr = null;
            exception = null;

            bool ret = true;

            try {
                sdr = new Sage_Data_Records(ini, credentials);
                ret = true;
            }
            catch (SageConnectionException ex) {
                exception = ex;
                ret = false;
            }
            // also throws SageConnectionException
            catch (DTAException) {
                ret = false;
            }
            catch (ApplicationException) {
                ret = false;
            }

            return ret;

        }

        public static bool
        Make(Ini_File ini,
             Company_Number number,
             string username,
             string password,
             out Sage_Data_Records sdr,
             out SageConnectionException exception) {

            var credentials = new Credentials(number, username, password);
            var ret = Make(ini, credentials, out sdr, out exception);

            return ret;


        }


        Sage_Data_Records(Ini_File ini,
                        Credentials credentials) {

            var username = credentials.Username;
            var password = credentials.Password;
            var number = credentials.Company;

            try {
                m_version = int.Parse(ini[DTA_Fields.VERSION]);

                m_company_number = number.As_String;

                company_proxy = ini.Group_ro(m_company_number);

                m_path = company_proxy[DTA_Fields.COMPANY_PATH];
                m_username = username;
                m_password = password;
                Settings = new SDR_DTA_Helper(number, ini);

                #region commented out - from the other two screens

                //_default_sales_details = proxy["DEFAULT_DETAILS_S"];
                //_default_sales_discount_details = proxy["DEFAULT_DISCOUNT_DETAILS_S"];
                //_default_purchase_details = proxy["DEFAULT_DETAILS_P"];
                //_default_purchase_discount_details = proxy["DEFAULT_DISCOUNT_DETAILS_P"];

                //if (proxy["POST_REMITTANCE_BY"] == "TYPE") {
                //    _remittance_by_number = false;
                //}
                //else if (proxy["POST_REMITTANCE_BY"] == "NUMBER") {
                //    _remittance_by_number = true;
                //}
                //else {
                //    throw new DtaException("INVALID POST_REMITTANCE_BY PARAMETER",
                //                            DtaException.FaultType.FileCorrupted);
                //} 
                #endregion


            }
            catch (MissingFieldException ex) {
                throw new DTAException(ex.ToString(), DTAException.FaultType.FileCorrupted);
            }

            // TestConnection();



        }

        IDisposable Establish_Connection() {

            Sage_Connection new_conn = new Sage_Connection(this.m_username, this.m_password, this.m_path, this.m_version, this.m_name);
            Sage_Connection old_conn = conn;
            conn = new_conn;

            return new On_Dispose(() =>
            {
                conn.Try_Dispose();
                conn = old_conn ?? new_conn;
            });

        }

        public bool Verify_Default_Codes(out List<Pair<Record_Type, string>> missing) {

            bool ret = true;

            var tmp_missing = new List<Pair<Record_Type, string>>();
            using (Establish_Connection()) {


                Action<Record_Type, string> check = (type, index) =>
                {
                    if (Verify_Record_Exists(type, index))
                        return;

                    ret = false;
                    tmp_missing.Add(Pair.Make(type, index));
                };


                var cash = Get_Default_Cash_Account();
                check(Record_Type.Sales, cash);

                var sales = Get_Default_Sales_Bank();
                check(Record_Type.Bank, sales);

                var payments = Get_Default_Payments_Account();
                check(Record_Type.Expense, payments);

            }

            missing = tmp_missing;
            return ret;
        }



        public void TestConnection() {

            var sdo = new SDOEngine(m_version);

            var ws = sdo.WSAdd(Global.Workspace);

            ws.Test_Connection(m_path, m_username, m_password, m_name);

        }

        public IDisposable Transaction() {

            return Establish_Connection();
        }



        // ****************************


        void Try(bool condition) {
            if (!condition) {
                AlertLogicError();
            }
        }

        void Try(bool condition, string message) {
            if (!condition) {
                AlertLogicError(message);
            }
        }

        void AlertLogicError() {

            AlertLogicError(null);

        }

        void AlertLogicError(string message) {

            Sage_Error error_code = 0;
            int? last_tran = null;

            using (Establish_Connection()) {

                message = message ?? "";
                if (!message.IsNullOrEmpty())
                    message += "\nLast sage error: ";

                var text = Sdo.Last_Error_Text;

                message += text;

                error_code = Sdo.Last_Error;

                try {
                    last_tran = Get_Last_Transaction_Number();
                }
                catch (XSage_Logic) {
                    last_tran = null;
                }

            }

            var ex = new XSage_Logic(message, last_tran, error_code);
            true.Throw_If_True(ex);

        }

        // ****************************


        public string
        Get_Last_Sage_Error() {
            using (Establish_Connection()) {
                return Sdo.Last_Error_Text;
            }
        }

        public bool
        Connected {
            get {
                var tmp = conn;
                return tmp != null && tmp.Connected;
            }
        }
        public string Company_Number {
            get { return m_company_number; }
        }

        public IRead<Ini_Field, string> Company_Proxy {
            get { return company_proxy; }
        }

        public string Machine_Name {
            get {
                return m_name;
            }
            set {
                value.IsNullOrEmpty().Throw_If_True();
                m_name = value;
            }
        }


        SDR_DTA_Helper Settings { get; set; }

        // ****************************

        string m_path;
        string m_username;
        string m_password;
        string m_name = Global.Default_Machine_Name;
        int m_version;

        readonly string m_company_number;

        string[] types = new string[]{"", 
                                      "SI", "SC", "SR", "SA", "SD", 
                                      "PI", "PC", "PP","PA", "PD", 
                                      "BP", "BR", "CP", "CR", 
                                      "JD", "JC", "VP", "VR", "CC", "CD", 
                                      "PAI", "PAO", "CO", "SP", "PR"};



        Sage_Connection conn;

        public Sage_Connection Conn {
            get {
                return conn;
            }
        }

        public SDOEngine Sdo {
            get {
                return conn.sdo;
            }
        }

        public WorkSpace WS {
            get {
                return conn.ws;
            }
        }

        readonly IRead<Ini_Field, string> company_proxy;

#pragma warning disable

        static bool _remittance_by_number = false;

        const int IN_UNIQUE_REF = 0;
        const int IN_TYPE = 1;
        const int IN_DATE = 2;
        const int IN_INV_REF = 3;
        const int IN_EXT_REF = 4;
        const int IN_DETAILS = 5;
        const int IN_TAX_CODE = 6;
        const int IN_AMOUNT = 7;
        const int IN_DISPUTED = 8;
        const int IN_RECEIPT = 9;
        const int IN_DISCOUNT = 10;
#pragma warning restore





    }
}