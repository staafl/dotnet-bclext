using System;
using System.IO;
using Common;
using Fairweather.Service;
using DTA;

namespace Sage_Int
{
    public class Sit_Company_Settings : Company_Helper
    {
        // from old implementation using clipe | perl -e "while(<>){s/STR_(\w+)/DTA_Fields.IT_\L\1/; print; }" | clip

        readonly Record_Type? mode;

        public Sit_Company_Settings(Ini_File ini, Company_Number company)
            : this(ini, company, null) {
        }
        public Sit_Company_Settings(Ini_File ini, Company_Number company, Record_Type? mode)
            : base(ini, company) {
            this.mode = mode;
            if (mode == 0)
                return;
        }

        protected bool? String_Flag(Ini_Field field) {

            /*
            // global setting
            if (True(field))
                return true;
            if (False(field))
                return false;
            */  
            var str = Get_Module_Field(field, mode.Value);
            return True(str);


        }


        static public Ini_Field Get_Module_Field(Ini_Field field, Record_Type mode) {
            var str = Get_Module_Field_Name(field.Field, mode);
            return new Ini_Field(str, field.Default_Value);
        }

        static public string Get_Module_Field_Name(string field_prefix, Record_Type mode) {
            return field_prefix + (mode == 0 ? "" : "_" + Sage_Int.SIT_Engine.sit_modes[mode]);

        }
        
        public bool? Documents_Numbering_Auto {
            get {
                return True(DTA_Fields.IT_invoice_numbering_auto);
            }
        }
        public bool? Documents_Grouping_Relative {
            get {
                return True(DTA_Fields.IT_invoice_items_relative_grouping);
            }
        }

        public bool? Has_Headers {
            get {
                return String_Flag(DTA_Fields.IT_dynamic_import_file_layout);
            }
        }
        public bool? Auto_Date {
            get {
                return String_Flag(DTA_Fields.IT_new_account_auto_date_entry);
            }
        }
        public bool? Output {
            get {
                return String_Flag(DTA_Fields.IT_output_success_csv);
            }
        }
        public bool? Overwrite {
            get {
                return String_Flag(DTA_Fields.IT_blank_fields_overwrite);
            }
        }
        public bool? Group_Transactions_Sales {
            get {
                return True(DTA_Fields.IT_group_transactions_sales);
            }
        }
        public bool? Group_Transactions_Purchase {
            get {
                return True(DTA_Fields.IT_group_transactions_purchase);
            }
        }
        public bool? Use_Mappings {
            get {
                return String_Flag(DTA_Fields.IT_use_mappings);
            }
        }
        public bool? Group_Transactions_Any {
            get {
                return Group_Transactions_Sales == true || Group_Transactions_Purchase == true;
            }
        }
        public bool? Check_Type {
            get {
                if (String_Flag(DTA_Fields.IT_change_bank_audit_type) == true)
                    return false;
                return String_Flag(DTA_Fields.IT_check_bank_audit_type);
            }
        }
        public bool? Change_Type {
            get {
                if (String_Flag(DTA_Fields.IT_change_bank_audit_type) != true)
                    return false;
                return String_Flag(DTA_Fields.IT_check_bank_audit_type);
            }
        }
        public bool? Use_Defaults {
            get {
                return String_Flag(DTA_Fields.IT_use_defaults);
            }
        }
        /*public string Sage_Usr {
            get {
                return String(DTA_Fields.IT_sageusr_path);
            }
            set {
                Set(DTA_Fields.IT_sageusr_path, value);
            }
        }*/
        public string CurrentPeriod {
            get {
                return String(DTA_Fields.IT_current_period);
            }
            /*set {
                Set(DTA_Fields.IT_current_period, value);
            }*/
        }

    }
}