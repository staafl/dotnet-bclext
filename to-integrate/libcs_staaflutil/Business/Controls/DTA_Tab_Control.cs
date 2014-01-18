using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DTA;
using Fairweather.Service;
using Standardization;

using Common;

namespace Common.Controls
{
    public class DTA_Tab_Control : Advanced_Tab_Control
    {
        protected Ini_File ini;

        public virtual void Setup(Ini_File p_pman) {

            ini = p_pman;

            foreach (var tab in this.Tabs) {

                var as_DTA_Tab = tab as DTA_Tab;

                if (as_DTA_Tab != null) {

                    var list = DTA_Controls.Make_DTA_Children(tab, ini, as_DTA_Tab.Get_Fields());

                    pairs.AddRange(list);

                }


            }

        }

        protected Company_Number? current_company;

        protected string Current_Company_As_String {
            get {
                return Current_Company.As_String;
            }
        }

        protected Company_Number Current_Company {
            get {
                return current_company.Value;
            }
        }

        public virtual void Set_Company(Company_Number company_number, bool refresh) {
            Set_Company(company_number, refresh, false);
        }

        public virtual void Set_Company(Company_Number company_number, bool refresh, bool force) {

            if (!force &&
                  current_company == company_number)
                return;

            current_company = company_number;
            if (refresh)
                Load_Data();

        }

        protected List<DTA_Control_Pair> pairs = new List<DTA_Control_Pair>(50);


        public void Load_Data() {

            foreach (var pair in pairs) {
                pair.Refresh(Current_Company_As_String);
            }

        }

        public void Store_Data() {

            foreach (var pair in pairs) {
                pair.Store(Current_Company_As_String);
            }

        }

    }
}
