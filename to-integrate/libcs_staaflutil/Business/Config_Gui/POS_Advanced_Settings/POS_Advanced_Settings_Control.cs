using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using Common;
using Common.Controls;

using DTA;
using Fairweather.Service;
using Standardization;

namespace Config_Gui
{

    public class POS_Advanced_Settings_Control : DTA_Tab_Control_2
    {
        public POS_Advanced_Settings_Control() {

            this.SetStyle(ControlStyles.ContainerControl, true);

            tab_General = new Tab_General();
            tab_Barcodes = new Tab_Barcodes();
            tab_Discounts = new Tab_Discounts();
            tab_Nominal_Codes = new Tab_Nominal_Codes();
            tab_Security = new Tab_Security();
            tab_Perf = new Tab_Performance();
            tab_Search = new Tab_Search();
            tab_Printing = new Tab_Printing();
            tab_Quick = new Tab_Quick();


            tab_General.Name = "General";
            tab_Barcodes.Name = "Barcodes";
            tab_Discounts.Name = "Discounts";
            tab_Nominal_Codes.Name = "Account Codes"; // "Nominal Codes";
            tab_Security.Name = "Security";
            tab_Perf.Name = "Performance";
            tab_Search.Name = "Search";
            tab_Printing.Name = "Printing";
            tab_Quick.Name = "Quick Items";


            this.Setup(tab_General,
                            tab_Barcodes,
                            tab_Discounts,
                            tab_Nominal_Codes,
                            tab_Security,
                            tab_Perf,
                            tab_Printing,
                            tab_Search,
                            tab_Quick);

            this.Activate_Tab(0);

            tab_Barcodes.But_change_price.Click += but_change_Click;
            tab_Barcodes.But_change_wt.Click += but_change_Click;

            tab_Printing.Cb_provider.SelectedIndexChanged +=
                (_1, _2) => Refresh_but_configure_Enabled();

            validators = Get_Validators(tab_Nominal_Codes);

            var restricted_access = tab_Nominal_Codes.tb_restricted_access;

            validators2[restricted_access] = () =>
            {
                var _set = Ini_Helper.Set(restricted_access.Text);

                if (!_set[tab_Nominal_Codes.Tb_def_cash_acc.Text])
                    return true;

                Standard.Warn("You cannot restrict access to the default Cash Account.");
                return false;
            };

            restricted_access.Validating += (_1, _2) => Verify_Textbox(restricted_access, true);
            foreach (var box in validators.Keys)
                box.Validating += (_sender, _2) => Verify_Textbox((TextBox)_sender , true);

            tab_Printing.But_configure.Click += (but_configure_Click);
            tab_Printing.But_test.Click += (but_test_Click);
            tab_Printing.But_edit.Click += (but_edit_Click);


        }

        // ****************************

        static Dictionary<TextBox, Pair<Record_Type, string>>
        Get_Validators(Tab_Nominal_Codes tab) {

            return new Dictionary<TextBox, Pair<Record_Type, string>>
            {

            {tab.Tb_def_cash_acc , Pair.Make(Record_Type.Sales, "Account Record")},
            {tab.Tb_def_receipts_bank ,Pair.Make(Record_Type.Bank,"Bank Account") },
            {tab.Tb_def_payments_acc ,Pair.Make(Record_Type.Expense,"Nominal Code") },

            };

        }

        public override void Setup(Ini_File p_pman) {

            base.Setup(p_pman);

            tab_Quick.Setup(Current_Company);

        }

        protected override UserControl Tab_To_Revert_To() {
            return tab_Nominal_Codes;
        }
       
        /*       Barcodes        */


        void but_change_Click(object sender, EventArgs e) {

            bool is_weight = (sender == tab_Barcodes.But_change_wt);

            using (var bform =
                new Barcode_Structure_Dialog(
                    is_weight,
                    ini,
                    Current_Company)) {

                bform.ShowDialog();

            }

        }

        /*       Printing        */

        Printing_Helper Get_Printing_Helper() {

            var proxy = this.ini.Company_Proxy_ro(Current_Company);

            var ret = new Printing_Helper(ini, Current_Company, Printing_Scenario.Sales_Receipt);

            return ret;
        }

        void Refresh_but_configure_Enabled() {

            var value = Get_Cb_provider_Value();

            var ok = !value.Safe_Equals(Ini_Main.BIXOLON, true);

            tab_Printing.But_configure.Enabled = ok;

        }

        string Get_Cb_provider_Value() {

            string[] vals, txt;

            DTA_Controls.Get_Combo_Box_Items(
                DTA_Combo_Box_Type.Print_Provider,
                out vals, out txt);

            var item = tab_Printing.Cb_provider.SelectedItem;

            if (item.IsNullOrEmpty())
                return null;

            var index = txt.IndexOf(item);
            var ret = vals[index];

            return ret;

        }


        void but_edit_Click(object sender, EventArgs e) {

            var helper = Get_Printing_Helper();

            helper.Scenario = Printing_Scenario.Preview;

            using (var rform = new Receipt_Designer(0, helper)) {

                rform.ShowDialog();

            }

        }

        void but_configure_Click(object sender, EventArgs e) {

            var dta_val = Get_Cb_provider_Value();

            var dict = new Dictionary<string, Func<DTA_Dialog>> { 
            { Ini_Main.TEXT, () => new Notepad_Printer_Dialog(ini, Current_Company) },
            { Ini_Main.WINDOWS, () => new GDI_Printer_Dialog(ini, Current_Company) },
            { Ini_Main.OPOS, ()=> new OPOS_Printer_Dialog(ini, Current_Company)},};

            var func = dict[dta_val];

            using (var dialog = func()) {
                dialog.ShowDialog(this.Parent);
            }

            tab_Printing.Cb_provider.Refresh_Border();

        }

        void but_test_Click(object sender, EventArgs e) {

            Store_Data();

            var helper = Get_Printing_Helper();

            Pos_Printing_Utility.Owning_Form = this.FindForm();

            Pos_Printing_Utility.Print_Sample_Receipt(helper);

        }


        /*       Fields        */

        readonly Tab_General tab_General;
        readonly Tab_Barcodes tab_Barcodes;
        readonly Tab_Discounts tab_Discounts;
        readonly Tab_Nominal_Codes tab_Nominal_Codes;
        readonly Tab_Security tab_Security;
        readonly Tab_Search tab_Search;
        readonly Tab_Printing tab_Printing;
        readonly Tab_Performance tab_Perf;
        readonly Tab_Quick tab_Quick;




    }

}
