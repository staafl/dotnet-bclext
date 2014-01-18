using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common;
using Common.Controls;
using DTA;
using Fairweather.Service;

namespace Config_Gui
{
    public partial class Sit_Settings_Tab : DTA_Tab
    {
        public Sit_Settings_Tab(Record_Type? module) {

            InitializeComponent();
            this.module = module;
            Fill_Checkboxes();

        }

        static Lazy_Dict<Ini_Field, int>
        chb_order = new Lazy_Dict<Ini_Field, int>(
            _f => int.MaxValue,
            new Dictionary<Ini_Field, int>
            {
                {DTA_Fields.IT_dynamic_import_file_layout, 0},
                {DTA_Fields.IT_blank_fields_overwrite, 1},
                {DTA_Fields.IT_new_account_auto_date_entry, 2},
                {DTA_Fields.IT_use_defaults, 3},
                {DTA_Fields.IT_check_bank_audit_type, 2},
                {DTA_Fields.IT_change_bank_audit_type, 3},                
                {DTA_Fields.IT_group_transactions_sales, 2},
                {DTA_Fields.IT_group_transactions_purchase, 3},

            });

        static Dictionary<Ini_Field, string>
        chb_text = new Dictionary<Ini_Field, string>
        {
            {DTA_Fields.IT_output_success_csv, "Generate Success Report"},
            {DTA_Fields.IT_dynamic_import_file_layout, "Dynamic File Layout"},
            {DTA_Fields.IT_blank_fields_overwrite , "Overwrite Blank Fields"},
            {DTA_Fields.IT_new_account_auto_date_entry , "Fill Date on New Account"},
            {DTA_Fields.IT_use_defaults, "Use Default Values"},
            {DTA_Fields.IT_check_bank_audit_type , "Check Account and Transaction Types"},
            {DTA_Fields.IT_change_bank_audit_type , "Adjust Transaction Type"},
            {DTA_Fields.IT_group_transactions_sales, "Group Transactions (SI, SC - Multiple Splits per Header)" },
            {DTA_Fields.IT_group_transactions_purchase, "Group Transactions (PI, PC - Multiple Splits per Header)"},
            {DTA_Fields.IT_invoice_numbering_auto, "Document Auto Numbering"},
            {DTA_Fields.IT_invoice_items_relative_grouping, "Relative Item Grouping"},
        };

        static Sit_Settings_Tab() {

            general_chb[0] = new List<Ini_Field> { DTA_Fields.IT_output_success_csv };

            module_chb[Record_Type.Sales] = module_chb[Record_Type.Purchase] = new List<Ini_Field> 
            { 
                DTA_Fields.IT_dynamic_import_file_layout,
                DTA_Fields.IT_blank_fields_overwrite ,
                DTA_Fields.IT_new_account_auto_date_entry ,
                DTA_Fields.IT_use_defaults 
            };

            module_chb[Record_Type.Expense] = new List<Ini_Field> 
            { 
                DTA_Fields.IT_dynamic_import_file_layout,
                DTA_Fields.IT_blank_fields_overwrite ,
            };

            general_chb[Record_Type.Bank] = new List<Ini_Field> 
            { 
                DTA_Fields.IT_check_bank_audit_type ,
                DTA_Fields.IT_change_bank_audit_type ,
            };

            module_chb[Record_Type.Bank] = new List<Ini_Field> 
            { 
                DTA_Fields.IT_dynamic_import_file_layout,
                DTA_Fields.IT_blank_fields_overwrite ,
            };

            module_chb[Record_Type.Stock] = new List<Ini_Field> 
            { 
                DTA_Fields.IT_dynamic_import_file_layout,
                DTA_Fields.IT_blank_fields_overwrite ,
            };

            module_chb[Record_Type.Audit_Trail] = new List<Ini_Field> 
            { 
                DTA_Fields.IT_group_transactions_sales,
                DTA_Fields.IT_group_transactions_purchase 
                // exchange rate of 1/1 means base currency
                // generate revaluation entries
            };

            general_chb[Record_Type.Audit_Trail] = new List<Ini_Field> 
            { 
                DTA_Fields.IT_dynamic_import_file_layout,
            };

            module_chb[Record_Type.Stock_Tran] = new List<Ini_Field> 
            { 
                DTA_Fields.IT_dynamic_import_file_layout,

            };

            //defensive programming here
            module_chb[Record_Type.Document] = module_chb[Record_Type.Invoice_Or_Credit] = new List<Ini_Field>
        {
            DTA_Fields.IT_invoice_numbering_auto,
            DTA_Fields.IT_invoice_items_relative_grouping,
            // ...
        };

        }

        static readonly Lazy_Dict<Record_Type, List<Ini_Field>>
        general_chb = new Lazy_Dict<Record_Type, List<Ini_Field>>(_ => new List<Ini_Field>()),
        module_chb = new Lazy_Dict<Record_Type, List<Ini_Field>>(_ => new List<Ini_Field>());

        readonly Twoway<Ini_Field, CheckBox> field_to_chb = new Twoway<Ini_Field, CheckBox>();
        readonly Record_Type? module;

        void Fill_Checkboxes() {
            int top_margin = 19;
            int left_margin = 21;
            int in_between = 7;
            var last = top_margin;

            var chbs = general_chb[module ?? 0].Concat(module_chb[module ?? 0]).OrderBy(_f => chb_order[_f]);

            foreach (var field in chbs) {
                var chb = new CheckBox();
                chb.FlatStyle = FlatStyle.Flat;
                chb.AutoSize = true;
                chb.Text = chb_text[field];
                this.Controls.Add(chb);
                chb.Location = new Point(left_margin, last + in_between);
                last = chb.Top + chb.Height;
                chb.Visible = true;
                field_to_chb[chb] = field;

                var close = chb;
                if(field == DTA_Fields.IT_check_bank_audit_type) {
                    close.CheckedChanged += (_1, _2) => { 
                        var _other = field_to_chb[DTA_Fields.IT_change_bank_audit_type];
                        var _checked = close.Checked;
                        _other.Enabled = _checked;
                        if(!_checked)
                            _other.Checked = false;
                    };
                }

            }

        }

        public override Dictionary<Control, Ini_Field> Get_Fields() {

            var general_flds = general_chb[module ?? 0];
            var module_flds = module_chb[module ?? 0];

            var dict = new Dictionary<Control, Ini_Field>();

            foreach (var field in general_flds) {
                dict[field_to_chb[field]] = field;

            }


            foreach (var field in module_flds) {
                module.tifn();
                dict[field_to_chb[field]] = Sage_Int.Sit_Company_Settings.Get_Module_Field(field, module.Value);

            }


            return dict;

        }


        #region designer

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        System.ComponentModel.IContainer components = null;

        void InitializeComponent() {
            this.SuspendLayout();
            // 
            // Sit_Settings_Tab
            // 
            this.Name = "Sit_Settings_Tab";
            this.Size = new System.Drawing.Size(570, 547);
            this.ResumeLayout(false);

        }

        #endregion


    }
}
