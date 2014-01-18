using System;
using System.Collections.Generic;
using Common;
using Fairweather.Service;
using Common.Sage;
using Versioning;

namespace Common.Posting
{
    public class Sage_Context
    {
        const string STR_ACCOUNT_REF = "ACCOUNT_REF";
        const string STR_ACCOUNT_TYPE = "ACCOUNT_TYPE";
        const string STR_INVOICE_NUMBER = "INVOICE_NUMBER";
        const string STR_STOCK_CODE = "STOCK_CODE";


        public Sage_Context(Func<IDisposable> connect,
                            Func<Work_Space> get_ws) {
            this.connect = connect;
            this.get_ws = get_ws;
        }
        public Sage_Context(Work_Space ws) {
            get_ws = () => ws;
            connect = () => null;
        }

        public bool
        Exists(Record_Type mode, object key) {

            bool ret;
            if (exists.TryGetValue(Pair.Make(mode, key), out ret))
                return ret;

            // in the special cases, let's fetch all data in one go
            if (mode == Record_Type.Bank) {
                BankType type;
                ret = Check_Bank(key.ToString(), out type);
            }
            else if (mode == Record_Type.Expense) {
                NominalType type;
                bool major;
                ret = Check_Nominal(key.ToString(), out major, out type);
            }
            else {

                using (connect()) {

                    var record = mode == Record_Type.Sales ? (ILinkRecord)WS.Create<SalesRecord>()
                               : mode == Record_Type.Purchase ? (ILinkRecord)WS.Create<PurchaseRecord>()
                               : mode == Record_Type.Document ? (ILinkRecord)WS.Create<InvoiceRecord>()
                               : mode == Record_Type.Stock ? (ILinkRecord)WS.Create<StockRecord>()
                               : null;

                    record.tifn();

                    if (mode == Record_Type.Document)
                        record[STR_INVOICE_NUMBER] = key.ToInt32();
                    else if (mode == Record_Type.Stock)
                        record[STR_STOCK_CODE] = key.ToString();
                    else
                        record[STR_ACCOUNT_REF] = key.ToString();

                    ret = record.Find(false);
                }
            }

            exists[mode, key] = ret;

            return ret;

        }


        // ****************************

        readonly Func<IDisposable> connect;
        readonly Func<Work_Space> get_ws;

        public Work_Space WS {
            get { return get_ws(); }
        }

        public int Version {
            get {
                return WS.Version;
            }
        }

        public bool
        Check_Bank(string acc_ref,
                   out Versioning.BankType bank_type) {

            H.assign(out bank_type);

            if (acc_ref.IsNullOrEmpty() ||
                acc_ref.Length > 8)
                return false;

            Versioning.BankType? null_type;

            if (bank.TryGetValue(acc_ref, out null_type)) {

                if (null_type == null)
                    return false;

                bank_type = null_type.Value;
                return true;

            }
            else {
                bool ok;
                BankRecord record;
                using (connect()) {
                    record = WS.Create<BankRecord>();
                    record[STR_ACCOUNT_REF] = acc_ref;
                    ok = record.Find(false);
                }

                if (ok) {

                    bank_type = (Versioning.BankType)record[STR_ACCOUNT_TYPE].ToInt32();
                    bank[acc_ref] = bank_type;

                    return true;

                }
                else {

                    nom[acc_ref] = null;
                    return false;
                }

            }

        }
        public bool
        Check_Nominal(string acc_ref,
                      out bool major, out Versioning.NominalType type) {

            H.assign(out major, out type);

            if (acc_ref.IsNullOrEmpty() ||
                acc_ref.Length > 8)
                return false;

            Pair<bool, Versioning.NominalType>? null_pair;

            if (nom.TryGetValue(acc_ref, out null_pair)) {

                if (null_pair == null)
                    return false;

                major = null_pair.Value.First;
                type = null_pair.Value.Second;
                return true;

            }
            else {
                bool ok;
                NominalRecord record;
                using (connect()) {
                    record = WS.Create<NominalRecord>();
                    record[STR_ACCOUNT_REF] = acc_ref;
                    ok = record.Find(false);
                }
                if (ok) {

                    major = record.MajorControl;
                    type = (Versioning.NominalType)record[STR_ACCOUNT_TYPE].ToInt32();
                    nom[acc_ref] = Pair.Make(major, type);

                    return true;

                }
                else {

                    nom[acc_ref] = null;
                    return false;
                }

            }

        }


        public bool
        Project_Exists(string cc) {

            bool ret;

            if (projs.TryGetValue(cc, out ret))
                return ret;

            using (connect()) {
                var proj_obj = WS.CreateProject();
                ret = proj_obj.Load(cc) && proj_obj.Reference == cc;
                projs[cc] = ret;

                return ret;
            }
        }

        public bool
        Cost_Code_Exists(string cc) {

            bool ret;

            if (ccs.TryGetValue(cc, out ret))
                return ret;
            using (connect()) {
                var cc_obj = WS.CreateProjectCostCode();
                ret = cc_obj.Load(cc) && cc_obj.Reference == cc;
                ccs[cc] = ret;

                return ret;
            }
        }

        public bool
        Document_Exists(string inv_ref, TransType type) {
            bool ret;
            if (docs.TryGetValue(inv_ref, out ret))
                return ret;
            using (connect()) {
                var inv_obj = WS.Create<InvoiceRecord>();
                inv_obj[STR_INVOICE_NUMBER] = inv_ref;
                ret = inv_obj.Find(false) && inv_obj[STR_INVOICE_NUMBER].strdef() == inv_ref;
                docs[inv_ref] = ret;

                return ret;
            }
        }

        public void Clear() {
            nom.Clear();
            bank.Clear();
            ccs.Clear();
            projs.Clear();
            exists.Clear();
        }
        // ****************************

        readonly Dictionary<string, Pair<bool, Versioning.NominalType>?>
        nom = new Dictionary<string, Pair<bool, Versioning.NominalType>?>();

        readonly Dictionary<string, Versioning.BankType?>
        bank = new Dictionary<string, Versioning.BankType?>();

        readonly Dictionary<string, bool> ccs = new Dictionary<string, bool>();
        readonly Dictionary<string, bool> projs = new Dictionary<string, bool>();
        readonly Dictionary<string, bool> docs = new Dictionary<string, bool>();

        readonly Pair_Key_Dict<Record_Type, object, bool>
        exists = new Pair_Key_Dict<Record_Type, object, bool>();

    }
}
