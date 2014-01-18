
using System.Windows.Forms;
using System.Collections.Generic;
using System;

using Fairweather.Service;

namespace Common.Dialogs
{
    /*       Departments mode eviscerated in revision 719        */


    public partial class Quick_Search_Form : Form
    {


        static Lazy_Dict<Quick_Search_Form_Mode, Pair<string, string>>
        header_texts = new Lazy_Dict<Quick_Search_Form_Mode, Pair<string, string>>
        (_ => Pair.Make("A/C", "Name"),
            // verified 2010/03/23
         new Pair_Dict<Quick_Search_Form_Mode, string, string> {
             {Quick_Search_Form_Mode.Products,"Code","Description"},
             {Quick_Search_Form_Mode.Departments,"Reference","Description"},
             {Quick_Search_Form_Mode.TTs,"Type","Description"},
             {Quick_Search_Form_Mode.Tax_Codes,"Code","Rate"},
             {Quick_Search_Form_Mode.Cost_Codes,"Code","Description"},
         });



        static bool Dynamic(Quick_Search_Form_Mode mode) {

            bool ret = mode.Contains(Quick_Search_Form_Mode.cst_dynamic_mode);
            return ret;

        }

        static bool Second_Column_Sort_Allowed(Quick_Search_Form_Mode mode) {

            if (mode == Quick_Search_Form_Mode.TTs)
                return false;
            if (mode == Quick_Search_Form_Mode.Tax_Codes)
                return false;

            return !Dynamic(mode);

        }

        static bool New_Edit_Allowed(Quick_Search_Form_Mode mode) {

            if (mode == Quick_Search_Form_Mode.Customers)
                return true;

            if (mode == Quick_Search_Form_Mode.Products)
                return true;

            if (mode == Quick_Search_Form_Mode.Suppliers)
                return true;

            return false;

        }

        static Set<Quick_Search_Form_Mode>
        search_alowed = new Set<Quick_Search_Form_Mode>{
            Quick_Search_Form_Mode.Customers,
            Quick_Search_Form_Mode.Customers_new,
            Quick_Search_Form_Mode.Products,
            Quick_Search_Form_Mode.Products_view,
            Quick_Search_Form_Mode.Suppliers,
            Quick_Search_Form_Mode.Suppliers_new,


        };

        static bool Search_Allowed(Quick_Search_Form_Mode mode) {

            return search_alowed[mode];

        }

        static Quick_Search_Form_Mode Aux_Denew(Quick_Search_Form_Mode mode) {

            var ret = mode & ((Quick_Search_Form_Mode)~Quick_Search_Form_Mode.cst_dynamic_mode);

            return ret;

        }

    }
}
