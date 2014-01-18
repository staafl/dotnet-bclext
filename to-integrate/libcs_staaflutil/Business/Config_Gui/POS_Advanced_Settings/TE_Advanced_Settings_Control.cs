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

    public class TE_Advanced_Settings_Control : DTA_Tab_Control_2
    {
        public TE_Advanced_Settings_Control() {

            this.SetStyle(ControlStyles.ContainerControl, true);

            tab_General = new TE_Tab_General();
            tab_Nominal = new TE_Tab_Nominal_Codes();

            tab_General.Name = "General";
            tab_Nominal.Name = "Nominal Codes";

            this.Setup(tab_General, tab_Nominal);

            this.Activate_Tab(0);

            validators = Get_Validators(tab_Nominal);

            foreach (var box in validators.Keys)
                box.Validating += (sender, _2) => Verify_Textbox(sender as TextBox, true);

        }

        // ****************************

        static Dictionary<TextBox, Pair<Record_Type, string>>
        Get_Validators(TE_Tab_Nominal_Codes tab) {

            return new Dictionary<TextBox, Pair<Record_Type, string>>
            {
                {tab.Tb_def_receipts_bank, Pair.Make(Record_Type.Bank,"Bank Account") },
                {tab.Tb_def_payments_bank, Pair.Make(Record_Type.Bank,"Bank Account") },
            };

        }

        protected override UserControl Tab_To_Revert_To() {
            return tab_Nominal;
        }



        /*       Fields        */

        readonly TE_Tab_General tab_General;
        readonly TE_Tab_Nominal_Codes tab_Nominal;



    }

}
