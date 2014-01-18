
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Common;
using Common.Dialogs;
using Fairweather.Service;
using Standardization;


namespace Config_Gui
{

    public class DTA_Dialog : Form_Base
    {
        // Only used in the designer
        public DTA_Dialog() { }

        /// <summary>
        /// Do not mutate
        /// </summary>
        List<DTA_Control_Pair> m_pairs;

        readonly protected Ini_File ini;

        readonly protected Company_Number m_company_number;

        public DTA_Dialog(Ini_File ini, Company_Number number)
            : base(Form_Kind.Modal_Dialog) {
            this.ini = ini;
            this.m_company_number = number;
        }

        protected virtual Dictionary<Control, DTA_Custom_Behavior_Pair> Custom_Behavior_Pairs {
            get { throw new NotImplementedException(); }
        }

        public virtual Dictionary<Control, Ini_Field> Get_Dictionary {
            get {
                throw new NotImplementedException();
            }
        }

        protected void Prepare_DTA() {

            var dict = Get_Dictionary;

            m_pairs = DTA_Controls.Make_DTA_Children(this, ini, Get_Dictionary, Custom_Behavior_Pairs);

            Refresh_DTA();

        }

        void Refresh_DTA() {
            foreach (var pair in m_pairs) {
                pair.Refresh(m_company_number.As_String);
            }
        }

        void Store_DTA() {
            foreach (var pair in m_pairs) {
                pair.Store((m_company_number.As_String));
            }
        }

        protected void but_ok_Click(object sender, EventArgs e) {

            Store_DTA();
            ini.Write_Data();
            Close();
        }
    }
}