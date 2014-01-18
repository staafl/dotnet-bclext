using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using DTA;
using Fairweather.Service;
using Versioning;
using Common.Posting;

namespace Common
{


    partial class Sage_Logic
    {

        public string Get_Company_Name() {
            bool _;
            return Get_Company_Name(out _);
        }
        public string Get_Company_Name(out bool ok) {
            return Get_Company_Name(true, out ok);

        }
        public string Get_Company_Name(bool do_throw, out bool ok) {
            return Get_Company_Name("UNABLE TO CONNECT TO DATA", do_throw, out ok);

        }

        public string Get_Company_Name(string def, bool do_throw, out bool ok) {

            ok = false;
            string ret;
            try {
                using (Establish_Connection()) {

                    var data = WS.Create<Versioning.SetupData>();

                    if (data.Read(1)) {
                        // do_throw.tift<XSage_Logic>();
                        ret = data[NAME].strdef();
                        ok = true;
                    }
                    else {
                        ret = def;

                    }
                }
            }
            catch (XSage_Conn) {
                if (do_throw)
                    throw;

                ret = def;

            }

            return ret;


        }




        public int
        Post_FF_Transaction(List<Trans_Line> header) {

            var fst = header.First();

            using (Establish_Connection()) {

                var tp = WS.Create<TransactionPost>();
                var hd = tp.HDGet();

                var tt = (TransType)fst[TYPE];

                //(tt == TransType.ourXR || tt == TransType.ourXP).tift();

                var for_both_1 = new Dictionary<string, object>
                {
                    {USER_NAME, Username},
                    {TYPE, tt},
                };

                var for_both_2 = new List<string> {
                    DATE, DETAILS,
                };

                var for_header = new List<string> {
                    INV_REF, ACCOUNT_REF
                };

                var for_split = new List<string> {
                    NOMINAL_CODE,TAX_CODE,INTERNAL_REF,NET_AMOUNT,TAX_AMOUNT,DATE,DETAILS
                };

                var mode = tt.Get_TT_Mode();

                hd[TYPE] = tt;

                hd.Fill(for_both_1);
                hd.Fill(fst, for_both_2);
                hd.Fill(fst, for_header);

                if (mode == Record_Type.Sales ||
                    mode == Record_Type.Purchase) {
                    if (tt.Should_Be_Bank())
                        hd[BANK_CODE] = fst.NOMINAL_CODE;
                }

                else if (mode == Record_Type.Bank)
                    hd[BANK_CODE] = fst.ACCOUNT_REF;

                foreach (var split in header) {

                    var sd = tp.SDAdd();

                    sd[TYPE] = tt;
                    sd.Fill(for_both_1);
                    sd.Fill(fst, for_both_2);
                    sd.Fill(split, for_split);

                    var pr = split[PROJ_REF];
                    var cc = split[COST_CODE];

                    if (!pr.IsNullOrEmpty()) {
                        var pr_obj = WS.CreateProject();
                        pr_obj.Load(pr).tiff();
                        sd.Project = pr_obj;

                    }
                    if (!cc.IsNullOrEmpty()) {
                        var cc_obj = WS.CreateProjectCostCode();
                        cc_obj.Load(cc).tiff();
                        sd.CostCode = cc_obj;

                    }
                }

                tp.Update().tiff<XSage_Logic>();

                return tp.PostingNumber;
            }


        }

        public SortedList<string, string>
        Get_Projects() {

            using (Establish_Connection()) {

                var projs = WS.CreateProjects();
                var proj = WS.CreateProject();
                var cnt = projs.Count;

                var ret = new SortedList<string, string>(cnt);

                for (int ii = 1; ii <= cnt; ++ii) {
                    if (!proj.Load(ii))
                        continue;

                    ret.Add(proj.Reference, proj.Name);

                }

                return ret;
            }
        }

        public SortedList<string, string>
        Get_Cost_Codes() {

            var refs = new Set<string>();

            var file = Path.Cpath("pccostix.dta");

            using (var s = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Read))
            using (var br = new BinaryReader(s)) {

                long pos = 0;
                long len = s.Length;
                var ascii = Encoding.ASCII;
                const int ref_len = 9; // including null byte
                const int rec_len = 13;
                while (pos + rec_len <= len) {

                    var bytes = br.ReadBytes(ref_len); // null-terminated
                    pos += ref_len;

                    var str = ascii.GetString(bytes).TrimEnd(' ', '\0');
                    br.ReadBytes(rec_len - ref_len);
                    pos += rec_len - ref_len;

                    refs[str] = true;
                }

            }

            using (Establish_Connection()) {

                var cc = WS.CreateProjectCostCode();

                var ret = new SortedList<string, string>(refs.Count);

                foreach (var str in refs) {

                    if (!cc.Load(str))
                        continue;

                    ret.Add(cc.Reference, cc.Description);

                }

                return ret;
            }
        }

        public Company_Record
        Get_Company_Data() {

            using (Establish_Connection()) {
                var oSetupData = WS.Create<SetupData>();

                string name = oSetupData[NAME].ToString();
                int currency = Convert.ToInt32(oSetupData[BASE_CURRENCY]);

                int year = Convert.ToInt32(oSetupData[FINYEAR_YEAR]);
                int month = Convert.ToInt32(oSetupData[FINYEAR_MONTH]) + 1;
                DateTime period_start = new DateTime(year, month, 1);

                bool by_split = (Convert.ToInt32(oSetupData[WIZ_START_SPLITS]) == 1);
                string country = oSetupData[COUNTRY_CODE].ToString();

                oSetupData = null;

                return new Company_Record(name, currency, period_start, by_split, country);
            }
        }

        public Dictionary<string, Country_Record>
        Get_Countries() {

            var ret = new Dictionary<string, Country_Record>();

            using (Establish_Connection()) {

                var oCountryCodeRecord = WS.Create<CountryCodeRecord>();

                if (oCountryCodeRecord.MoveFirst()) {

                    do {
                        // ^{[A-Z_0-9]*}.*
                        // var \1 = oCountryCodeRecord["\1"].ToString();

                        var country_code = oCountryCodeRecord[COUNTRY_CODE].ToString();
                        var country_name = oCountryCodeRecord[COUNTRY_NAME].ToString();
                        var vat_format_1 = oCountryCodeRecord[VAT_FORMAT_1].ToString();
                        var vat_format_2 = oCountryCodeRecord[VAT_FORMAT_2].ToString();
                        var vat_format_3 = oCountryCodeRecord[VAT_FORMAT_3].ToString();
                        var vat_format_4 = oCountryCodeRecord[VAT_FORMAT_4].ToString();
                        var vat_format_5 = oCountryCodeRecord[VAT_FORMAT_5].ToString();

                        Country_Record record = new Country_Record(country_code,
                                                                    country_name,
                                                                    vat_format_1,
                                                                    vat_format_2,
                                                                    vat_format_3,
                                                                    vat_format_4,
                                                                    vat_format_5);

                        ret.Add(country_code, record);

                    } while (oCountryCodeRecord.MoveNext());

                }


            }

            return ret;
        }

        public string
        Get_Default_Country() {

            var ret = Get_Data_Field(1, Data_Type.Setup, COUNTRY_CODE);

            return (string)ret;
        }

        public Currency_Record[]
        Get_Currencies() {

            using (Establish_Connection()) {

                var currencies = new List<Currency_Record>();

                var oCurrencyData = WS.Create<CurrencyData>();

                int cCurrencies = oCurrencyData.Count;

                for (int ii = 1; ii <= cCurrencies; ++ii) //Note the indexing
{
                    oCurrencyData.Read(ii);

                    string symbol = oCurrencyData[SYMBOL].ToString();
                    string name = oCurrencyData[NAME].ToString();
                    string code = oCurrencyData[CODE].ToString();

                    var rate = oCurrencyData[EXCHANGE_RATE].ToDecimal();

                    string major = oCurrencyData[MAJOR_UNIT].ToString().ToProper();
                    string minor = oCurrencyData[MINOR_UNIT].ToString().ToProper();

                    currencies.Add(new Currency_Record(ii, symbol, name, code, rate, major, minor));
                }

                return currencies.ToArray();
            }

        }

        public string[]
        Get_Departments() {

            using (Establish_Connection()) {

                var departments = new List<string>(1000);
                var oDepartmentData = WS.Create<DepartmentData>();
                int cDepartments = oDepartmentData.Count;
                for (int ii = 1; ii <= cDepartments; ii++) {
                    oDepartmentData.Read(ii);
                    departments.Add(oDepartmentData[NAME].ToString());
                }
                oDepartmentData = null;

                return departments.ToArray();
            }
        }

        // ****************************



        public Bank_Record?
        Get_Bank_Record(string id) {

            Dictionary<string, object> data1, data2;

            bool ok = Try_Get_Record_Fields(id, Record_Type.Bank, out data1,
               ACCOUNT_REF, ACCOUNT_NUMBER, NAME, CURRENCY);
            if (!ok)
                return null;

            ok = Try_Get_Record_Fields(id, Record_Type.Expense, out data2,
                            NAME);
            if (!ok)
                return null;

            var name = data2[NAME].ToString();
            int currency = data1[CURRENCY].ToInt32();
            var bank_name = data1[NAME].ToString();
            var bank_account = data1[ACCOUNT_NUMBER].ToString();

            if (bank_name.IsNullOrEmpty())
                bank_name = name;

            var ret = new Bank_Record(id, name, currency, bank_name, bank_account);

            return ret;

        }

        public Account_Record?
        Get_Account_Record(bool customer, string id) {

            if (id.IsNullOrEmpty())
                return null;

            var type = customer ? Record_Type.Sales
                     : Record_Type.Purchase;

            Account_Record? ret;
            using (Establish_Connection()) {

                var record = Get_Record(type);

                record[ACCOUNT_REF] = id;

                if (record.Find(false))
                    ret = Get_Account_Record(record);
                else
                    ret = null;

            }

            return ret;

        }

        public void
        Execute(Record_Type type, Action<ISageRecord> act) {
            using (Establish_Connection()) {

                var record = Get_Record(type);
                act(record);

            }
        }

        Account_Record
        Get_Account_Record(IFields record) {

            /*       31 August        */

            string account = record[ACCOUNT_REF].ToString();
            string name = record[NAME].ToString();
            int currency = record[CURRENCY].ToInt32();

            var ret = new Account_Record(account, name, currency);

            return ret;

        }

        Dictionary<string, object>
        Get_Fields(IFields record,
        params string[] fields) {

            using (Establish_Connection()) {

                int field_cnt = fields.Length;

                var ret = new Dictionary<string, object>(field_cnt);

                ret.Fill(record, fields);

                return ret;
            }
        }

        public bool
        Try_Get_Record_Field(string record, Record_Type type, string field,
             out object value) {
            value = null;
            using (Establish_Connection()) {

                var data = Get_Record(type);
                data.Set_Index_Value(record);

                if (!data.Find(false))
                    return false;

                value = data[field];
                return true;

            }

        }

        public object
        Get_Record_Field(string record, Record_Type type, string field) {
            object value;
            Try_Get_Record_Field(record, type, field, out value).tiff();
            return value;

        }


        public bool
        Try_Get_Record_Fields(string index,
            Record_Type type,
            out Dictionary<string, object> data,
            params string[] fields) {

            data = null;
            using (Establish_Connection()) {

                var irecord = Get_Record(type);

                irecord[irecord.Index_String] = index;

                if (!irecord.Find(false))
                    return false;

                data = Get_Fields(irecord, fields);

                return true;
            }

        }


        public Dictionary<string, object>
        Get_Record_Fields(string index,
        Record_Type record_type,
        params string[] fields) {

            using (Establish_Connection()) {

                var irecord = Get_Record(record_type);

                irecord.Set_Index_Value(index);

                if (!irecord.Find(false)) {

                    string format = "Account name does not exist ({0}) for record type {1}";

                    string type = record_type.Get_String();
                    true.tift(format.spf(index, type));

                };

                var ret = Get_Fields(irecord, fields);

                return ret;
            }

        }





        public bool
        Try_Get_Data_Field(int record, Data_Type type, string field,
        out object value) {

            value = null;

            using (Establish_Connection()) {

                var data = Get_Data(type);
                if (!data.Read(record))
                    return false;

                value = data[field];
                return true;

            }

        }

        public object
        Get_Data_Field(int index, Data_Type type, string field) {
            object value;
            Try_Get_Data_Field(index, type, field, out value).tiff();
            return value;

        }

        public bool
        Try_Get_Data_Fields(int index,
        Data_Type type,
        out Dictionary<string, object> data,
        params string[] fields) {

            data = null;
            using (Establish_Connection()) {

                var idata = Get_Data(type);

                if (!idata.Read(index))
                    return false;

                data = Get_Fields(idata, fields);

                return true;
            }

        }

        public Dictionary<string, object>
        Get_Data_Fields(int index,
        Data_Type type,
        params string[] fields) {

            Dictionary<string, object> data;

            if (Try_Get_Data_Fields(index, type, out data, fields))
                return data;

            true.tift("Data record not found: {0}".spf(index));
            return null;
        }






        public bool
        Verify_Record_Exists(Record_Type type, string index) {

            bool ret;
            using (Establish_Connection()) {

                var record = Get_Record(type);
                record.Key = index;

                ret = record.Find(false);

            }

            return ret;

        }

        // ****************************


        //[Obsolete]
        //Account_Record
        //    Get_Account_Record(SalesData record) {

        //    /*       05th September        */

        //    (record is SalesData).tiff();

        //    string account = record[ACCOUNT_REF].ToString();
        //    string name = record[NAME].ToString();
        //    int currency = Convert.ToInt32(record[CURRENCY]);
        //    DateTime last_pay = Convert.ToDateTime(record[LAST_PAY_DATE]);

        //    var ret = new Account_Record(account, name, currency, last_pay);

        //    return ret;

        //}

        public Dictionary<string, T>
        Scan_Customer_Accounts<T>(Func<SalesData, T> producer) {


            using (Establish_Connection()) {

                var oSalesData = WS.Create<SalesData>();

                int cnt = oSalesData.Count;

                var _customerAccounts = new Dictionary<string, T>(cnt);

                /*       Deleted records are removed when running compress data        */

                int deleted_cnt = 0;
                for (int ii = 1; ii <= cnt; ++ii) {

                    if (!oSalesData.Read(ii))
                        true.tift("read");

                    if (oSalesData.Deleted) {

                        ++deleted_cnt;
                        continue;

                    }

                    var acc_ref = oSalesData[ACCOUNT_REF].ToString();
                    _customerAccounts[acc_ref] = producer(oSalesData);

                }

                oSalesData = null;

                return _customerAccounts;

            }

        }

        public Dictionary<string, string>
        Get_Customer_Accounts_Names() {

            return Scan_Customer_Accounts(data => data[NAME].ToString());

        }

        public Dictionary<string, Account_Record>
        Get_Customer_Accounts() {

            return Scan_Customer_Accounts(data => Get_Account_Record(data));

        }

        [Obsolete]
        public Dictionary<string, Account_Record>
        Get_Supplier_Accounts() {

            using (Establish_Connection()) {

                var _supplierAccounts = new Dictionary<string, Account_Record>();

                var oPurchaseRecord = WS.Create<PurchaseRecord>();

                if (oPurchaseRecord.MoveFirst()) {

                    do {
                        string account = oPurchaseRecord[ACCOUNT_REF].ToString();
                        var name = oPurchaseRecord[NAME].ToString();

                        int currency = Convert.ToInt32(oPurchaseRecord[CURRENCY]);
                        var last_pay = Convert.ToDateTime(oPurchaseRecord[LAST_PAY_DATE]);

                        _supplierAccounts[account] = new Account_Record(account, name, currency);

                    } while (oPurchaseRecord.MoveNext());
                }

                oPurchaseRecord = null;
                return _supplierAccounts;

            }
        }

        [Obsolete]
        public Dictionary<string, Bank_Record>
        Get_Bank_Accounts() {

            using (Establish_Connection()) {

                var _bankAccounts = new Dictionary<string, Bank_Record>();
                var oNominalRecord = WS.Create<NominalRecord>();
                var oBankRecord = WS.Create<BankRecord>();

                if (oBankRecord.MoveFirst()) {

                    do {
                        string account_ref = oBankRecord[ACCOUNT_REF].ToString();
                        oNominalRecord[ACCOUNT_REF] = account_ref;

                        oNominalRecord.Find(false).tiff();

                        var name = oNominalRecord[NAME].ToString();
                        int currency = oBankRecord[CURRENCY].ToInt32();
                        var bank_name = oBankRecord[NAME].ToString();
                        var bank_account = oBankRecord[ACCOUNT_NUMBER].ToString();
                        // var type = (BankType)oBankRecord[ACCOUNT_TYPE].ToInt32();

                        if (bank_name.IsNullOrEmpty())
                            bank_name = name;

                        _bankAccounts[account_ref] = new Bank_Record(account_ref, name, currency, bank_name, bank_account);

                    } while (oBankRecord.MoveNext());
                }

                oBankRecord = null;
                oNominalRecord = null;
                return _bankAccounts;

            }
        }


        // ****************************

        public Account_Record
        Update_Account_Record(
               bool new_account,
               string index,
               Record_Type record_type,
               Dictionary<string, object> fields) {


            using (Establish_Connection()) {

                var record = Get_Record(record_type);

                if (new_account) {

                    record.AddNew();
                    record.Set_Index_Value(index);

                }
                else {

                    record.Set_Index_Value(index);

                    if (!record.Find(false)) {

                        string err_format = "Account name does not exist ({0}) for record type {1}";
                        string type = record_type.Get_String();

                        true.tift(err_format.spf(index, type));

                    };

                    record.Edit().tiff();

                }

                record.Fill(fields);

                record.Update().tiff();

                var ret = Get_Account_Record(record);


                return ret;
            }

        }

        public bool
        Delete_Customer(string account_ref, out string error_msg, out Account_Record account) {

            H.assign(out error_msg, out account);

            var ret = false;

            using (Establish_Connection()) {

                var record = WS.Create<SalesRecord>();

                record[ACCOUNT_REF] = account_ref;

                record.Find(false).tiff();

                ret = record.CanRemove();

                if (ret) {

                    account = Get_Account_Record(record);

                    ret = record.Remove();

                }


                if (!ret) {

                    // var code = sdo.LastErrorCode;

                    // see SDO: Exceptions/Error Codes 
                    //         
                    // 28 The record cannot be deleted as the account has transactions associated with it. 
                    // 29 The record ... for a reason other than having transactions associated i.e. 
                    //     there is a balance on the account. 

                    error_msg = "Account cannot be deleted - it may have outstanding transactions.";

                }

            }

            return ret;

        }


        string[]
        Get_Address_Lines(string address) {

            string[] ret = address.Lines(false, false, false)
                      .Pad_Right(cst_address_lines, "")
                      .ToArray();

            return ret;
        }

        public string
        Get_Address(string account_ref) {

            string[] lines = new string[cst_address_lines];

            for (int ii = 1; ii <= cst_address_lines; ++ii)
                lines[ii - 1] = "ADDRESS_" + ii.ToString();

            var address_lines = Get_Record_Fields(account_ref, Record_Type.Sales, lines);

            string ret = address_lines.OrderBy(kvp => kvp.Key)
                          .Select(kvp => kvp.Value.ToString())
                          .Unlines(true);

            return ret;
        }



        public static string
        Get_Record_Code_Prefix(string surname, string name) {

            var ret = (surname + name).Trim_Pad('0', 4, false);
            ret = ret.ToUpper();

            return ret;
        }




        public bool
        Fill_Tax_Codes(bool force) {

            if (!Tax_Code.Initialize()) {

                if (!force)
                    return false;
                // they have already been filled

            }

            int tax_code_cnt = Tax_Code.Tax_Code_Count;

            using (Establish_Connection()) {

                var oControlData = WS.Create<ControlData>();


                /// Sage's tax codes range from 0 to 99
                for (int ii = 0; ii < tax_code_cnt; ++ii) {

                    // It doesn't seem as though oControlData can be deleted
                    // -- Velko, 08/10/09
                    //if (oControlData.Deleted)
                    //    continue;

                    string index = Tax_Code.Get_Index_String(ii);
                    string str_rate = "{0}_RATE".spf(index);

                    decimal rate = Convert.ToDecimal(oControlData[str_rate]);

                    var tax_code = new Tax_Code((short)ii, rate);

                }

            }

            return true;
        }

        public Dictionary<string, Nominal_Record>
        Get_Nominal_Data() {

            var ret = new Dictionary<string, Nominal_Record>();

            using (Establish_Connection()) {

                var oNominalRecord = WS.Create<NominalRecord>();
                oNominalRecord.MoveFirst();

                do {

                    string description = oNominalRecord[NAME].ToString();
                    string reference = oNominalRecord[ACCOUNT_REF].ToString();

                    var temp = new Nominal_Record(description, reference);
                    ret.Add(reference, temp);

                } while (oNominalRecord.MoveNext());

            }

            return ret;

        }



        public int
        Get_Last_Transaction_Number() {

            using (Establish_Connection()) {

                var HeaderData = WS.Create<HeaderData>();

                if (!HeaderData.MoveLast())
                    return 0;

                int ret = HeaderData[LAST_SPLIT].ToInt32();

                return ret;

            }

        }



        public Pair<DateTime>
        Get_Financial_Period() {


            Pair<DateTime> ret;

            using (Establish_Connection()) {

                ret = M.Get_Financial_Period(Conn.ws);

            }

            return ret;


        }




        readonly Dictionary<Data_Type, string> datas = new Dictionary<Data_Type, string>
{
{Data_Type.Setup, "SetupData"},

};

        readonly Dictionary<Record_Type, string> records = new Dictionary<Record_Type, string>
{
{Record_Type.Sales, "SalesRecord"},
{Record_Type.Purchase, "PurchaseRecord"},
{Record_Type.Bank, "BankRecord"},
{Record_Type.Expense, "NominalRecord"},
{Record_Type.Stock, "StockRecord"},
// more to be added on demand
};

        readonly Dictionary<Record_Type, string> ixes = new Dictionary<Record_Type, string>
{
{Record_Type.Sales, "SalesIndex"},
{Record_Type.Purchase, "PurchaseIndex"},
{Record_Type.Bank, "BankIndex"},
{Record_Type.Expense, "NominalIndex"},
{Record_Type.Stock, "StockIndex"},
// more to be added on demand
};



        IData Get_Data(Data_Type type) {

            var str = datas[type];
            var ret = (IData)WS.Create(str);
            return ret;

        }

        ISageRecord Get_Dynamic_Record(Record_Type type) {

            var ret = (ISageRecord)Get_Record(type);

            if (type == Record_Type.Bank)
                ret = new Dynamic_Bank((BankRecord)ret, WS.Create<NominalRecord>());

            return ret;

        }



        ISageRecord Get_Record(Record_Type type) {

            string str = records[type];

            var ret = (ISageRecord)WS.Create(str);


            return ret;
        }

        IIndex Get_Index(Record_Type type) {

            string str = ixes[type];

            var ret = (IIndex)WS.Create(str);


            return ret;
        }


        string Get_Default_Sales_Bank() {

            string ret = Proxy[DTA_Fields.POS_def_sales_bank];
            ret = ret.Trim();
            return ret;

        }

        string Get_Default_Payments_Account() {

            string ret = Proxy[DTA_Fields.POS_def_payments_account];
            ret = ret.Trim();
            return ret;

        }

        string Get_Default_Cash_Account() {

            string ret = Proxy[DTA_Fields.POS_def_cash_account];
            ret = ret.Trim();
            return ret;

        }

        string Get_Default_Receipts_Bank() {

            // 23rd september
            return Get_Default_Sales_Bank();

        }
    }
}
