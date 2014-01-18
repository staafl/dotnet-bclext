

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Fairweather.Service;

namespace Config_Gui
{
    class Multi_Combobox_Manager
    {
        static readonly Set<ComboBox> stat_cbxs = new Set<ComboBox>();
        static readonly Set<CheckBox> stat_chbxs = new Set<CheckBox>();

        readonly Twoway<ComboBox, CheckBox> m_pairs;
        readonly string[] m_item_pool;
        readonly Dictionary<ComboBox, string> m_item_usage;
        readonly Dictionary<ComboBox, Set<string>> m_usage_2;

        public Multi_Combobox_Manager(Dictionary<ComboBox, CheckBox> pairs,
                                      string[] items) {

            pairs.tifn();
            items.tifn();

            int pairs_len = pairs.Count;
            int items_len = items.Length;

            (pairs_len > 0).tiff();
            (items_len >= pairs_len).tiff();

            m_pairs = new Twoway<ComboBox, CheckBox>(pairs_len);
            m_item_usage = new Dictionary<ComboBox, string>(pairs_len);
            m_usage_2 = new Dictionary<ComboBox, Set<string>>(pairs_len);

            foreach (var kvp in pairs) {

                var cbx = kvp.Key;
                var chbx = kvp.Value;

                stat_cbxs[cbx].tift("The same combobox is used in more than one Multi Combobox configuration: " + cbx.Name);
                stat_chbxs[chbx].tift("The same checkbox is used in more than one Multi Combobox configuration: " + chbx.Name);

                stat_cbxs[cbx] = true;
                stat_chbxs[chbx] = true;

                m_pairs.Add(cbx, chbx);

            }

            m_item_pool = new string[items_len];
            items.CopyTo(m_item_pool, 0);
            Array.Sort(m_item_pool);


            SetEventHandlers();
            Prepare_ComboBoxes(true);

        }

        public void Refresh() {

            Prepare_ComboBoxes(false);

        }

        void Prepare_ComboBoxes(bool first) {

            foreach (var cbx in m_pairs.Lefts) {

                cbx.Sorted = true;

                cbx.Items.Clear();

                cbx.Items.AddRange(m_item_pool);

                m_usage_2[cbx] = new Set<string>(m_item_pool);

            }

            if (first) {
                foreach (var pair in m_pairs) {

                    var chbx = pair.Second;
                    var cbx = pair.First;

                    if (!cbx.Enabled)
                        chbx.Checked = false;
                    else if (!chbx.Checked)
                        cbx.Enabled = false;

                }
            }

            foreach (var cbx in m_pairs.Lefts)
                Ensure_Distinct_Items(cbx);

        }

        void SetEventHandlers() {

            foreach (var pair in m_pairs) {

                ComboBox cbx = pair.First;

                cbx.EnabledChanged += combo_Box_EnabledChanged;
                cbx.SelectedIndexChanged += combo_Box_SelectedIndexChanged;
                pair.Second.CheckedChanged += check_Box_CheckedChanged;

            }

        }

        bool b_combo_Box_SelectedIndexChanged;

        void combo_Box_SelectedIndexChanged(object sender, EventArgs e) {
            if (b_combo_Box_SelectedIndexChanged)
                return;

            b_combo_Box_SelectedIndexChanged = true;

            try {

                var cbx = (ComboBox)sender;
                var previous = m_item_usage.Get_Or_Null(cbx);
                var _new = cbx.SelectedItem.StringOrDefault("");

                m_item_usage[cbx] = _new;

                // NOTE: To remove
                if (previous.IsNullOrEmpty() && 
                    _new.IsNullOrEmpty())
                    true.tift();

                foreach (var cbx_2 in m_pairs.Lefts) {

                    if (cbx_2 == cbx)
                        continue;

                    var hashset = m_usage_2[cbx_2];

                    if (!previous.IsNullOrEmpty()
                        && !hashset[previous]) {

                        hashset[previous] = true;
                        cbx_2.Items.Add(previous);

                    }

                    if (!_new.IsNullOrEmpty()) {

                        hashset[_new] = false;
                        cbx_2.Items.Remove(_new);

                    }

                }

            }
            finally {
                b_combo_Box_SelectedIndexChanged = false;
            }
        }
        bool b_check_Box_CheckedChanged;

        void check_Box_CheckedChanged(object sender, EventArgs e) {
            if (b_check_Box_CheckedChanged)
                return;

            b_check_Box_CheckedChanged = true;
            try {
                var chbx = (CheckBox)sender;
                var cbx = m_pairs[chbx];
                var is_checked = chbx.Checked;

                cbx.Enabled = is_checked;

            }
            finally {
                b_check_Box_CheckedChanged = false;
            }
        }

        bool b_combo_Box_EnabledChanged;

        void combo_Box_EnabledChanged(object sender, EventArgs e) {
            if (b_combo_Box_EnabledChanged)
                return;

            b_combo_Box_EnabledChanged = true;
            try {
                var cbx = (ComboBox)sender;
                var chbx = m_pairs[cbx];
                var is_enabled = cbx.Enabled;

                chbx.Checked = is_enabled;

                Ensure_Distinct_Items(cbx);

            }
            finally {
                b_combo_Box_EnabledChanged = false;
            }
        }


        void Ensure_Distinct_Items(ComboBox cbx) {

            bool add = cbx.Enabled;
            var value = cbx.SelectedItem.StringOrDefault("");

            if (value.IsNullOrEmpty())
                return;

            var enabled = (from pair in m_pairs as IEnumerable<Pair<ComboBox, CheckBox>>
                           let cbx_2 = pair.First
                           where cbx_2.Enabled
                           select cbx_2).ToList();

            foreach (var cbx_2 in enabled.Except(cbx)) {

                if (add) {
                    if (!m_usage_2[cbx_2][value]) {

                        m_usage_2[cbx_2][value] = true;
                        cbx_2.Items.Add(value);

                    }
                }
                else {

                    m_usage_2[cbx_2][value] = false;
                    cbx_2.Items.Remove(value);

                }

            }

        }



        //void Ensure_Distinct_Items() {

        //    var enabled = (from pair in m_pairs as IEnumerable<Pair<ComboBox, CheckBox>>
        //                   let cbx = pair.First
        //                   where cbx.Enabled
        //                   select cbx).ToList();

        //    var strings = (from cbx in enabled
        //                   let value = cbx.SelectedItem.StringOrDefault("")
        //                   where !value.IsNullOrEmpty()
        //                   select value).ToArray();

        //    var unused = m_item_pool.Except(strings).ToArray();


        //    foreach (var cbx in enabled) {

        //        var value = cbx.SelectedItem.StringOrDefault("");
        //        var items = cbx.Items.
        //        cbx.Items.Clear();
        //        var items = m_item_pool.Ex

        //        foreach(var @string in strings.Except(value)){
        //            cbx.Items.Remove(@string);

        //        }


        //    }



        //}

    }
}