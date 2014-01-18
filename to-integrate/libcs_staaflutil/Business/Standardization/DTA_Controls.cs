
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

using System.Linq;
using System.Windows.Forms;


using DTA;

using Common;
using Common.Sage;
using Common.Controls;

using Colors = Standardization.Colors;


using Fairweather.Service;


/*       All future control standardization efforts go here        */
namespace Standardization
{


    static public class DTA_Controls
    {

        const string YES = Ini_Main.YES;
        const string NO = Ini_Main.NO;

        /// <summary>
        /// Assigns the tags inside the "tags" collection to the appropriate
        /// child controls of "parent"
        /// </summary>
        /// Throws IOpExc if a child control already has a tag and it does not match the one
        /// in the collection
        static public void
        Assign_Tags(Control parent, Dictionary<Control, string> tags) {

            var all_children = parent.All_Children();

            foreach (Control ctrl in all_children) {

                string tag1 = ctrl.Tag.StringOrDefault("");
                string tag2;

                bool is_tag1_null = tag1.IsNullOrEmpty();

                if (!tags.TryGetValue(ctrl, out tag2)) {

                    if (is_tag1_null)
                        // the control will be invisible to us
                        continue;

                    tags[ctrl] = tag1.ToString();
                    continue;
                }

                if (!is_tag1_null) {

                    (tag1.ToString() == tag2).tiff();
                    continue;

                }

                ctrl.Tag = tag2;

            }

        }

        static public void
        Get_Combo_Box_Items(DTA_Combo_Box_Type type,
                                                   out string[] dta_values,
                                                   out string[] cb_items) {

            switch (type) {
                case DTA_Combo_Box_Type.Super_Both_None:
                    dta_values = new[] { Ini_Main.CASH, Ini_Main.SUPER, Ini_Main.NONE };
                    cb_items = new[] { "Cashier", "Supervisor", "Disabled" };

                    break;
                case DTA_Combo_Box_Type.Vat_Nvat_Both:
                    dta_values = new[] { Ini_Main.BOTH, Ini_Main.VAT, Ini_Main.NVAT };
                    cb_items = new[] { "VAT Inclusive & Exclusive", "VAT Inclusive", "VAT Exclusive" };

                    break;
                case DTA_Combo_Box_Type.Qty_Amount:
                    dta_values = new[] { Ini_Main.QTY, Ini_Main.AMOUNT };
                    cb_items = new[] { "Quantity", "Amount" };

                    break;
                case DTA_Combo_Box_Type.Barcode: {

                        var fields = Sage_Fields.StockRecord.Barcode_Fields;

                        dta_values = fields.Select(f => f.Name).arr();
                        cb_items = fields.Select(f => "{0} ({1})".spf(f.Description,
                                                                      f.Length)).arr();


                    }
                    break;
                case DTA_Combo_Box_Type.Account_Unposted: {
                        dta_values = new[] { Ini_Main.ACCOUNT, Ini_Main.UNPOSTED };
                        cb_items = new[] { "By Account", "By Unposted" };
                        break;
                    }
                case DTA_Combo_Box_Type.Date_Posted_By: {
                        dta_values = new[] { Ini_Main.DATE, Ini_Main.POSTED_BY };
                        cb_items = new[] { "By Date", "By POS Terminal" };
                        break;
                    }
                case DTA_Combo_Box_Type.Print_Provider: {

                        var lst1 = new List<string> {
                            Ini_Main.TEXT, Ini_Main.WINDOWS, Ini_Main.BIXOLON };

                        var lst2 = new List<string>{ 
                            "Text File Generator", "Windows Printer Drivers", "Bixolon SRP-350" };

                        if (Pos_For_Net.Pos_For_Net_Present) {
                            lst1.Add(Ini_Main.OPOS);
                            lst2.Add("OPOS");
                        }

                        dta_values = lst1.arr();
                        cb_items = lst2.arr();

                        break;
                    }
                case DTA_Combo_Box_Type.OPOS_Printer: {

                        var lst1 = Pos_For_Net.Get_OPOS_Logical_Names(null);

                        if (lst1.Is_Empty())
                            lst1 = new List<string> { "NONE" };

                        dta_values = lst1.arr();
                        cb_items = lst1.arr();

                        break;

                    }
                case DTA_Combo_Box_Type.Font: {

                        var fonts = Data.Fonts;

                        dta_values = new string[fonts.Count];
                        cb_items = new string[fonts.Count];

                        int ii = 0;

                        foreach (var kvp in Data.Fonts) {
                            dta_values[ii] = kvp.Key;
                            cb_items[ii] = kvp.Value;
                            ++ii;
                        }

                        break;
                    }
                case DTA_Combo_Box_Type.Preferred_Printer: {

                        var list1 = H.Printers().lst();
                        var list2 = new List<string>(list1);
                        list1.Insert(0, Ini_Main.DEFAULT);
                        list2.Insert(0, "Use System Default");

                        dta_values = list1.arr();
                        cb_items = list2.arr();
                        break;

                    }
                case DTA_Combo_Box_Type.Document_Permissions: {
                        dta_values = new[] { Ini_Main.NONE, Ini_Main.SAME_POS, Ini_Main.ANY_POS, Ini_Main.ANY };
                        cb_items = new[] { "None", "Same Terminal documents", "Any Terminal documents", "All documents" };
                        break;
                    }
                case DTA_Combo_Box_Type.Automatic_Sales_History_Display: {
                        dta_values = new[] { Ini_Main.NONE, Ini_Main.NUMBER, Ini_Main.DATE, Ini_Main.ALL };
                        cb_items = new[] { "No", "Last Document (by number)", "Last Document (by date)", "All documents" };
                        break;
                    }
                case DTA_Combo_Box_Type.Average_Cost_Calculation_Type: {
                        dta_values = new[] { Ini_Main.PURCHASES_ONLY, Ini_Main.ALL };
                        cb_items = new[] { "Weighted (only Purchases)", "Weighted (all Transactions)" };
                        break;
                    }
                case DTA_Combo_Box_Type.Check_Ref_Mode: {
                        dta_values = new[] { Ini_Main.NO, Ini_Main.ONDEMAND, Ini_Main.AUTO };
                        cb_items = new[] { "No", "On Demand", "Automatically" };
                        break;
                    }
                default:
                    true.tift(type.Get_String());
                    throw new ApplicationException();
            }

        }

        static public string Default_Category { get; set; }


        static DTA_Conversion_Pair
        Make_DTA_Validating_Box(this Ini_File ini,
                                Validating_Box vb) {

            // Standard.Style_Validating_Box(vb, true);

            Action<string> set = (value) =>
            {
                if (value.IsNullOrEmpty())
                    vb.Value = 0.0m;
                else
                    vb.Value = decimal.Parse(value);
            };

            Func<string> get = () =>
                {
                    var value = vb.Value;
                    return value.ToString(true);
                };

            return new DTA_Conversion_Pair(set, get);

        }

        static DTA_Conversion_Pair
        Make_DTA_Textbox(this Ini_File ini, TextBox tb) {

            Action<string> set = value => tb.Text = value;

            Func<string> get = () =>
                {
                    if (!tb.Enabled)
                        return null;

                    return tb.Text;

                };

            return new DTA_Conversion_Pair(set, get);

        }


        static DTA_Conversion_Pair
        Make_DTA_Checkbox(this Ini_File ini, CheckBox chb) {

            Action<string> set = (value) =>
            {

                (value == YES || value == NO).tiff();

                chb.Checked = (value == YES);

            };

            Func<string> get = () =>
            {
                bool ok = chb.Checked && chb.Enabled;

                return ok ? YES : NO;
            };

            return new DTA_Conversion_Pair(set, get);

        }

        static DTA_Conversion_Pair
        Make_DTA_NumericUpDown(this Ini_File ini,
                                             NumericUpDown nupd) {

            Action<string> set = (value) =>
            {
                int number = int.Parse(value);

                nupd.Value = ((decimal)number).Min_Max(nupd.Minimum, nupd.Maximum);

            };

            Func<string> get = () =>
            {
                if (!nupd.Enabled)
                    return null;

                return nupd.Value.ToString();
            };

            return new DTA_Conversion_Pair(set, get);

        }



        static DTA_Conversion_Pair
        Make_DTA_Combobox(this Ini_File ini, ComboBox cb,
                                             DTA_Combo_Box_Type type) {
            string[] dta_values;
            string[] cb_items;

            Get_Combo_Box_Items(type, out dta_values, out cb_items);

            var ret = Make_DTA_Combobox(ini, cb, dta_values, cb_items);

            return ret;

        }

        /// <summary>
        /// This procedure will only work if the values of the respective items in the
        /// combo box are not altered after it is ran. Items can, however, be added and removed;
        /// </summary>
        static DTA_Conversion_Pair
        Make_DTA_Combobox(this Ini_File ini, ComboBox cb,
                                             string[] values,
                                             string[] items) {

            cb.Items.Clear();
            cb.Items.AddRange(items);

            values = values.Select(str => str.ToUpper()).ToArray();
            Action<string> set = (value) =>
            {
                int index = values.IndexOf(value);

                if (index < 0)
                    return;

                //(index >= 0).tiff();

                string item = items[index];

                cb.SelectedItem = item;

                /*       Old method        */

                //cb.SelectedIndex = index;

            };

            Func<string> get = () =>
            {
                if (!cb.Enabled)
                    return null;

                string item = cb.SelectedItem.ToString();
                int index = items.IndexOf(item);

                (index == -1).tift();

                string value = values[index];
                return value;

                /*       Old method        */

                //string value = values[cb.SelectedIndex];

            };

            return new DTA_Conversion_Pair(set, get);

        }

        //static public
        //List<DTA_Control_Pair> Make_DTA_Children(Dictionary<Control, string> controls,
        //                                         Ini_File ini) {


        //    var ret = new List<DTA_Control_Pair>(controls.Count);

        //    foreach (Control ctrl in controls) {

        //        DTA_Control_Pair pair;

        //        if (ctrl is Panel) {   // Flatmap
        //            ret.AddRange(Make_DTA_Children(ctrl, ini));
        //            continue;
        //        }
        //        if (!Try_Make_DTA_Child(ini, ctrl, out pair))
        //            continue;

        //        ret.Add(pair);
        //        //child.Tag = counter;
        //        ++counter;

        //    }

        //    return ret;

        //}
        // To effect additional formatting (i.e. 01 -> POS01) the caller might pass a dictionary<string, IO_Pair<string>>
        // containing the mapping functions for each field
        public static List<DTA_Control_Pair>
        Make_DTA_Children(Control parent,
                          Ini_File ini,
                          Dictionary<Control, Ini_Field> fields) {

            var custom_behaviors = new Dictionary<Control, DTA_Custom_Behavior_Pair>();

            return Make_DTA_Children(parent, ini, fields, custom_behaviors);

        }

        static public List<DTA_Control_Pair>
        Make_DTA_Children(Control parent,
                          Ini_File ini,
                          Dictionary<Control, Ini_Field> fields,
                          Dictionary<Control, DTA_Custom_Behavior_Pair> custom_behaviors) {

            int counter = 0;

            var children = parent.Controls.Cast<Control>().ToList();

            if (children.Count == 0)
                return new List<DTA_Control_Pair>();

            var ret = new List<DTA_Control_Pair>(children.Count);

            foreach (Control child in children) {

                DTA_Conversion_Pair pair;

                if (child is Panel ||
                    child is GroupBox) {   // Flatmap

                    ret.AddRange(Make_DTA_Children(child, ini, fields, custom_behaviors));
                    continue;

                }

                Ini_Field field;

                if (!fields.TryGetValue(child, out field))
                    continue;

                if (!Try_Make_DTA_Child(ini, child, out pair))
                    continue;

                var get = pair.Get_Value;
                var set = pair.Set_Value;

                if (custom_behaviors != null) {

                    DTA_Custom_Behavior_Pair pair2;

                    if (custom_behaviors.TryGetValue(child, out pair2)) {

                        get = () => pair2.After_Get(pair.Get_Value());
                        set = (value) => pair.Set_Value(pair2.Before_Set(value));

                    }

                }

                Action<string> refresh, store;

                refresh = category =>
                {
                    set(ini[category, field]);
                };
                store = category =>
                {
                    var value = get();
                    ini[category, field] = value;
                };

                var control_pair = new DTA_Control_Pair(refresh, store);

                ret.Add(control_pair);
                //child.Tag = counter;
                ++counter;

            }

            return ret;

        }


        static bool
        Try_Make_DTA_Child(Ini_File ini,
                           Control child,
                           out  DTA_Conversion_Pair result) {

            result = default(DTA_Conversion_Pair);

            string name = child.Name;

            if (child is ComboBox) {

                // Standard.Flat_Style(child as ComboBox);

                DTA_Combo_Box_Type type;

                if (name.EndsWith("vat"))
                    type = DTA_Combo_Box_Type.Vat_Nvat_Both;

                else if (name.EndsWith("qty"))
                    type = DTA_Combo_Box_Type.Qty_Amount;

                else if (name.Contains("barcode_"))
                    type = DTA_Combo_Box_Type.Barcode;

                else {

                    var dict = new Dictionary<string, DTA_Combo_Box_Type>
                    {
                        {"cb_unposted_balance", DTA_Combo_Box_Type.Account_Unposted},
                        {"cb_days_documents",DTA_Combo_Box_Type.Date_Posted_By},
                        {"cb_provider", DTA_Combo_Box_Type.Print_Provider},
                        {"cb_font", DTA_Combo_Box_Type.Font},
                        {"cb_printer", DTA_Combo_Box_Type.Preferred_Printer},
                        {"cb_opos_device", DTA_Combo_Box_Type.OPOS_Printer},
                        {"cb_document_edit_permissions", DTA_Combo_Box_Type.Document_Permissions},
                        {"cb_document_view_permissions", DTA_Combo_Box_Type.Document_Permissions},
                        {"cb_auto_sales_history", DTA_Combo_Box_Type.Automatic_Sales_History_Display},
                        {"cb_avg_cost_calc", DTA_Combo_Box_Type.Average_Cost_Calculation_Type},
                        {"cb_sales_ref_check", DTA_Combo_Box_Type.Check_Ref_Mode},
                        {"cb_purchase_ref_check", DTA_Combo_Box_Type.Check_Ref_Mode},

                    };

                    type = dict.Get_Or_Default_(name.ToLower(), DTA_Combo_Box_Type.Super_Both_None);
                }

                result = Make_DTA_Combobox(ini, child as ComboBox, type);

                return true;

            }

            if (child is TextBox) {

                result = Make_DTA_Textbox(ini, child as TextBox);
                return true;

            }

            if (child is NumericUpDown) {

                result = Make_DTA_NumericUpDown(ini, child as NumericUpDown);
                return true;

            }

            if (child is CheckBox) {

                result = Make_DTA_Checkbox(ini, child as CheckBox);
                return true;

            }

            if (child is Validating_Box) {

                result = Make_DTA_Validating_Box(ini, child as Validating_Box);
                return true;

            }

            return false;
        }
    }

    [DebuggerStepThrough]
    public struct DTA_Control_Pair
    {

        readonly Action<string> m_refresh;

        readonly Action<string> m_store;


        public Action<string> Refresh {
            get {
                return this.m_refresh;
            }
        }

        public Action<string> Store {
            get {
                return this.m_store;
            }
        }


        public DTA_Control_Pair(Action<string> refresh,
                                Action<string> store) {

            this.m_refresh = refresh;
            this.m_store = store;
        }

        #region DTA_Control_Pair

        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "refresh = " + this.m_refresh;
            ret += ", ";
            ret += "store = " + this.m_store;

            ret = "{DTA_Control_Pair<T>: " + ret + "}";
            return ret;

        }

        public bool Equals(DTA_Control_Pair obj2) {

            if (!this.m_refresh.Equals(obj2.m_refresh))
                return false;

            if (!this.m_store.Equals(obj2.m_store))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is DTA_Control_Pair);

            if (ret)
                ret = this.Equals((DTA_Control_Pair)obj2);


            return ret;

        }

        public static bool operator ==(DTA_Control_Pair left, DTA_Control_Pair right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(DTA_Control_Pair left, DTA_Control_Pair right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

            unchecked {
                int ret = 23;
                int temp;

                ret *= 31;
                temp = this.m_refresh.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_store.GetHashCode();
                ret += temp;

                return ret;
            }
        }

        #endregion

    }

    [DebuggerStepThrough]
    public struct DTA_Conversion_Pair
    {

        readonly Action<string> m_set_value;

        readonly Func<string> m_get_value;


        public Action<string> Set_Value {
            get {
                return this.m_set_value;
            }
        }

        public Func<string> Get_Value {
            get {
                return this.m_get_value;
            }
        }


        public DTA_Conversion_Pair(Action<string> set,
                                                   Func<string> get) {

            this.m_set_value = set;
            this.m_get_value = get;
        }


        #region DTA_Conversion_Pair

        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "refresh = " + this.m_set_value;
            ret += ", ";
            ret += "store = " + this.m_get_value;

            ret = "{DTA_Conversion_Pair<T>: " + ret + "}";
            return ret;

        }

        public bool Equals(DTA_Conversion_Pair
obj2) {

            if (!this.m_set_value.Equals(obj2.m_set_value))
                return false;

            if (!this.m_get_value.Equals(obj2.m_get_value))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is DTA_Conversion_Pair);

            if (ret)
                ret = this.Equals((DTA_Conversion_Pair)obj2);


            return ret;

        }

        public static bool operator ==(DTA_Conversion_Pair
left, DTA_Conversion_Pair
right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(DTA_Conversion_Pair
left, DTA_Conversion_Pair
right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

            unchecked {
                int ret = 23;
                int temp;

                ret *= 31;
                temp = this.m_set_value.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_get_value.GetHashCode();
                ret += temp;

                return ret;
            }
        }

        #endregion
    }

    [DebuggerStepThrough]
    public struct DTA_Custom_Behavior_Pair
    {

        readonly Func<string, string> m_before_set;

        readonly Func<string, string> m_after_get;


        public Func<string, string> Before_Set {
            get {
                return this.m_before_set;
            }
        }

        public Func<string, string> After_Get {
            get {
                return this.m_after_get;
            }
        }


        public DTA_Custom_Behavior_Pair(Func<string, string> before_set,
                                                             Func<string, string> after_get) {

            this.m_before_set = before_set;
            this.m_after_get = after_get;

        }


        #region DTA_Custom_Behavior_Pair

        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "refresh = " + this.m_before_set;
            ret += ", ";
            ret += "store = " + this.m_after_get;

            ret = "{DTA_Custom_Behavior_Pair<T>: " + ret + "}";
            return ret;

        }

        public bool Equals(DTA_Custom_Behavior_Pair obj2) {

            if (!this.m_before_set.Equals(obj2.m_before_set))
                return false;

            if (!this.m_after_get.Equals(obj2.m_after_get))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is DTA_Custom_Behavior_Pair);

            if (ret)
                ret = this.Equals((DTA_Custom_Behavior_Pair)obj2);


            return ret;

        }

        public static bool operator ==(DTA_Custom_Behavior_Pair left, DTA_Custom_Behavior_Pair right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(DTA_Custom_Behavior_Pair left, DTA_Custom_Behavior_Pair right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

            unchecked {
                int ret = 23;
                int temp;

                ret *= 31;
                temp = this.m_before_set.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_after_get.GetHashCode();
                ret += temp;

                return ret;
            }
        }

        #endregion
    }

}