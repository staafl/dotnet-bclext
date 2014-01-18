using System;
using System.IO;
using Common;
using Fairweather.Service;

namespace Sage_Int
{
    public class Company_Settings_Old : Settings_Old
    {


        readonly int ix;
        public Company_Settings_Old(D d)
            : this(d, Record_Type.Undefined) {
        }
        public Company_Settings_Old(D d, Record_Type mode)
            : base(d) {
            if (mode == 0)
                return;
            this.ix = SIT_Engine.Get_Mode_Index(mode);
        }

        protected bool? String_Flag(string key) {

            var str = String(key);
            if (str.IsNullOrEmpty())
                return null;

            if (str[0] == '1')
                return true;
            if (ix >= str.Length)
                return false;
            if (str[ix] == '1')
                return true;

            return false;

        }


        // removeme 0788c1c9-a6a2-4e85-ab99-91ad0b75d7b6
        Lazy<Pair<bool>> Documents_Grouping_Auto_Relative = new Lazy<Pair<bool>>(
            () =>
            {
                var _auto = true;
                var _relative = true;

                var _file = "invoice_grouping.txt";
                if (File.Exists(_file)) {
                    using (var _sr = new StreamReader(_file)) {
                        Func<bool, bool> read = __def =>
                        {
                            if (_sr.EndOfStream)
                                return __def;
                            var __str = _sr.ReadLine().strdef().Trim().ToUpper();
                            if (__str.IsNullOrEmpty())
                                return __def;
                            if (__str.EndsWith("YES"))
                                return true;
                            if (__str.EndsWith("NO"))
                                return false;
                            return __def;
                        };

                        _auto = read(_auto);
                        _relative = _auto ? read(_relative) : false;

                    }

                }

                return new Pair<bool>(_auto, _relative);

            });

        public bool? Documents_Numbering_Auto {
            get {
                return Documents_Grouping_Auto_Relative.Value.First;// String_Flag("DOCUMENTS_NUMBERING_AUTO");
            }
        }
        public bool? Documents_Grouping_Relative {
            get {
                return Documents_Grouping_Auto_Relative.Value.Second;//String_Flag("DOCUMENTS_GROUPING_RELATIVE");
            }
        }
        // C:\Users\Fairweather\Desktop\Generation\sageint_settings.pl
        public bool? Has_Headers {
            get {
                return String_Flag(STR_DYNAMIC_IMPORT_FILE_LAYOUT);
            }
        }
        public bool? Auto_Date {
            get {
                return String_Flag(STR_NEW_ACCOUNT_AUTO_DATE_ENTRY);
            }
        }
        public bool? Output {
            get {
                return String_Flag(STR_OUTPUT_SUCCESS_CSV);
            }
        }
        public bool? Overwrite {
            get {
                return String_Flag(STR_BLANK_FIELDS_OVERWRITE);
            }
        }
        public bool? Group_Transactions_Sales {
            get {
                return String_Flag(STR_GROUP_TRANSACTIONS_SALES);
            }
        }
        public bool? Group_Transactions_Purchase {
            get {
                return String_Flag(STR_GROUP_TRANSACTIONS_PURCHASE);
            }
        }
        public bool? Use_Mappings {
            get {
                return String_Flag(STR_USE_MAPPINGS);
            }
        }
        public bool? Group_Transactions_Any {
            get {
                return Group_Transactions_Sales == true || Group_Transactions_Purchase == true;
            }
        }
        public bool? Check_Type {
            get {
                if (String_Flag(STR_CHANGE_BANK_AUDIT_TYPE) == true)
                    return false;
                return String_Flag(STR_CHECK_BANK_AUDIT_TYPE);
            }
        }
        public bool? Change_Type {
            get {
                if (String_Flag(STR_CHANGE_BANK_AUDIT_TYPE) != true)
                    return false;
                return String_Flag(STR_CHECK_BANK_AUDIT_TYPE);
            }
        }
        public bool? Use_Defaults {
            get {
                return String_Flag(STR_USE_DEFAULTS);
            }
        }
        public string Sage_Usr {
            get {
                return String(STR_SAGEUSR_PATH);
            }
            set {
                Set(STR_SAGEUSR_PATH, value);
            }
        }
        public string CurrentPeriod {
            get {
                return String(STR_CURRENT_PERIOD);
            }
            set {
                Set(STR_CURRENT_PERIOD, value);
            }
        }

        #region ancient, horrible implementation
        //public string OutputSuccess {
        //    get {
        //        return _settings[current_company]["OUTPUT_SUCCESS_CSV"];
        //    }
        //    set {
        //        _settings[current_company]["OUTPUT_SUCCESS_CSV"] = value;
        //        b_flag = false;
        //        for (int i = 0; i < settings_cbs["report"].Length; i++) {
        //            if (settings_cbs["report"][i] != null) {
        //                settings_cbs["report"][i].Checked = (value[i] == '1');
        //            }
        //        }
        //    }
        //}
        //public string GroupTransactionsS {
        //    get {
        //        return _settings[current_company]["GROUP_TRANSACTIONS_SALES"];
        //    }
        //    set {
        //        _settings[current_company]["GROUP_TRANSACTIONS_SALES"] = value;
        //        b_flag = false;
        //        for (int i = 0; i < settings_cbs["group_sales"].Length; i++) {
        //            if (settings_cbs["group_sales"][i] != null) {
        //                settings_cbs["group_sales"][i].Checked = (value[i] == '1');
        //            }
        //        }
        //    }
        //}
        //public string GroupTransactionsP {
        //    get {
        //        return _settings[current_company]["GROUP_TRANSACTIONS_PURCHASE"];
        //    }
        //    set {
        //        _settings[current_company]["GROUP_TRANSACTIONS_PURCHASE"] = value;
        //        b_flag = false;
        //        for (int i = 0; i < settings_cbs["group_purchase"].Length; i++) {
        //            if (settings_cbs["group_purchase"][i] != null) {
        //                settings_cbs["group_purchase"][i].Checked = (value[i] == '1');
        //            }
        //        }
        //    }
        //}
        //public string OverrideBlankFields {
        //    get {
        //        return _settings[current_company]["BLANK_FIELDS_OVERWRITE"];
        //    }
        //    set {
        //        _settings[current_company]["BLANK_FIELDS_OVERWRITE"] = value;
        //        b_flag = false;
        //        for (int i = 0; i < settings_cbs["overr"].Length; i++) {
        //            if (settings_cbs["overr"][i] != null) {
        //                settings_cbs["overr"][i].Checked = (value[i] == '1');
        //            }
        //        }
        //    }
        //}
        //public string AutoDateEntry {
        //    get {
        //        return _settings[current_company]["NEW_ACCOUNT_AUTO_DATE_ENTRY"];
        //    }
        //    set {
        //        _settings[current_company]["NEW_ACCOUNT_AUTO_DATE_ENTRY"] = value;
        //        b_flag = false;
        //        for (int i = 0; i < settings_cbs["auto_date"].Length; i++) {
        //            if (settings_cbs["auto_date"][i] != null) {
        //                settings_cbs["auto_date"][i].Checked = (value[i] == '1');
        //            }
        //        }
        //    }
        //}
        //public string ExchangeRate {
        //    get {
        //        return _settings[current_company]["EXCHANGE_RATE_1_BASE"];
        //    }
        //    set {
        //        _settings[current_company]["EXCHANGE_RATE_1_BASE"] = value;
        //        b_flag = false;
        //        for (int i = 0; i < settings_cbs["exchange_rate"].Length; i++) {
        //            if (settings_cbs["exchange_rate"][i] != null) {
        //                settings_cbs["exchange_rate"][i].Checked = (value[i] == '1');
        //            }
        //        }
        //    }
        //}
        //public string GenerateReval {
        //    get {
        //        return _settings[current_company]["GENERATE_REVAL"];
        //    }
        //    set {
        //        _settings[current_company]["GENERATE_REVAL"] = value;
        //        b_flag = false;
        //        for (int i = 0; i < settings_cbs["reval"].Length; i++) {
        //            if (settings_cbs["reval"][i] != null) {
        //                settings_cbs["reval"][i].Checked = (value[i] == '1');
        //            }
        //        }
        //    }
        //}
        //public string UseDefaults {
        //    get {
        //        return "1111111111";
        //    }
        //    set {
        //        _settings[current_company]["USE_DEFAULTS"] = value;
        //        b_flag = false;
        //        for (int i = 0; i < settings_cbs["defaults"].Length; i++) {
        //            if (settings_cbs["defaults"][i] != null) {
        //                settings_cbs["defaults"][i].Checked = (value[i] == '1');
        //            }
        //        }
        //    }
        //}
        //public string UseMappings {
        //    get {
        //        return _settings[current_company]["USE_MAPPINGS"];
        //    }
        //    set {
        //        _settings[current_company]["USE_MAPPINGS"] = value;
        //        b_flag = false;
        //        for (int i = 0; i < settings_cbs["mappings"].Length; i++) {
        //            if (settings_cbs["mappings"][i] != null) {
        //                settings_cbs["mappings"][i].Checked = (value[i] == '1');
        //            }
        //        }
        //    }
        //}
        #endregion


    }
}
