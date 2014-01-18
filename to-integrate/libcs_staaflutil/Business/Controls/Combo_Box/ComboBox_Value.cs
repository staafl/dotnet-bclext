using System;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{

    public partial class Our_Combo_Box : UserControl
    {
        protected override bool
        ProcessCmdKey(ref Message msg, Keys keyData) {

            try {

                bool shortlist_needs_input =
                                            comboBox.Focused
                                         && sh_list_move.ContainsKey(keyData)
                                         && short_list.Visible
                                         && short_list.Items.Count > 0;

                if (shortlist_needs_input) {

                    if (short_list.ItemUnderCursor != null)
                        return true;

                    Handle_Shortlist_Input(keyData);
                    return true;

                }

                if (keyData == Keys.Enter)
                    Console.WriteLine();

                if (Handle_Keys(keyData))
                    return true;

                return base.ProcessCmdKey(ref msg, keyData);

            }
            // Anomaly - 12 0ct --velko
            catch (IndexOutOfRangeException) {
                return true;
            }
        }



        protected override void OnKeyPress(KeyPressEventArgs e) {

            if (CharacterCasing == CharacterCasing.Upper)
                e.KeyChar = Char.ToUpper(e.KeyChar);

            else if (CharacterCasing == CharacterCasing.Lower)
                e.KeyChar = Char.ToLower(e.KeyChar);

            int sel = comboBox.SelectionStart;

            if (short_list.Focused) {

                if (!Char.IsLetterOrDigit(e.KeyChar))
                    return;

                Aux_Accept_New_Char(e.KeyChar);

                bf_focus = true;
                return;
            }

            base.OnKeyPress(e);
        }

        void Aux_Accept_New_Char(char ch) {

            bf_text = true;

            int sel = comboBox.SelectionStart;

            if (comboBox.SelectionLength > 0)
                Text = comboBox.Text
                               .Remove(comboBox.SelectionStart,
                                       comboBox.SelectionLength);

            bf_text = false;

            comboBox.SelectionStart = sel;

            this.Text = this.Text.Insert(comboBox.SelectionStart, ch.ToString());

            comboBox.SelectionStart = sel + ch.ToString().Length;
        }


        public bool Allow_New_Account {
            get;
            set;
        }

        string m_new_account;

        public string New_Account {
            get { return Allow_New_Account ? m_new_account : null; }
        }

        protected Selection_Result Handle_Changes(bool on_leave, bool enter_key, bool tab_key) {

            // If this structure makes me uncomfortable, I can always override
            // the control and put the entire Handle_Changes method inside the guard
            Selection_Result result = 0;


            if (!Sage_Access.Sage_Guard(() => result = Examine_Changes(false))) {
                this.Undo();
                return Selection_Result.Unable_To_Verify;
            }

            do {

                if (result.Contains(Selection_Result.cst_not_ok)) {

                    OnRejectChanges(Rejected_Event_Args.Make(this.Text));

                    break;

                }

                if (result == Selection_Result.Existing_Account ||
                     (enter_key && result == Selection_Result.Already_Selected)) {

                    cached = this.Text;
                    this.Value = this.Text;
                    OnAcceptChanges(EventArgs.Empty);
                    break;

                }

                if (result == Selection_Result.New_Account) {

                    // if there used to be an actual value
                    if (this.Value != null) {

                        (m_new_account.IsNullOrEmpty()).tiff();
                        cached = this.Value;
                        this.value = null; // <-- field
                    }

                    m_new_account = this.Text;

                    On_NewAccount();

                    break;

                }

                if (result == Selection_Result.Already_Selected) {

                    break;

                }

            } while (false);

            return result;

        }
        public virtual Record_Type Record_Type {
            get {
                return 0;
            }
            protected set {
            }
        }

        public Records_Cursor RecordsCursor {
            get {
                return Records_Access.Get_Cursor(this.Record_Type);
            }
        }

        protected virtual bool Account_Valid(string txt, bool allow_gui) {
            return true;
        }

        /// <summary>
        /// Only call under Connection Guard
        /// </summary>
        Selection_Result Examine_Changes(bool force) {

            var txt = this.Text;

            if (!force && txt == this.value)
                return Selection_Result.Already_Selected;

            if (RecordsCursor.ContainsKey(txt)) {

                if (!Account_Valid(txt, true))
                    return Selection_Result.Invalid_Account;

                return Selection_Result.Existing_Account;

            }

            if (txt.IsNullOrEmpty()) {

                return Allow_Blank ? Selection_Result.Empty_Account_OK :
                                     Selection_Result.Empty_Account;

            }

            return Allow_New_Account ? Selection_Result.New_Account :
                                       Selection_Result.Account_Does_Not_Exist;
        }

        bool Commit_Selection() {

            if (!short_list.HasSelectedItem)
                return false;

            bf_text = true;
            try {
                int sel_start = comboBox.SelectionStart;
                comboBox.Text = short_list.SelectedItem.ToString();
                comboBox.SelectionStart = sel_start;

                comboBox.SelectionLength = 200;
                return true;
            }
            finally {
                bf_text = false;
            }
        }
        protected void OnSelectedIndexChanged() {

            if (bf_sel)
                return;

            Commit_Selection();


            if (Selected_Index_Changed != null)
                Selected_Index_Changed(this, EventArgs.Empty);
        }

        string value;
        string cached;

        // This method needs review
        public String Value {
            set {
                if (value != null) {

                    if (!RecordsCursor.ContainsKey(value))
                        value = null;

                }

                cached = value;
                this.value = value;
                m_new_account = null;

                Set_Safe_Text(value);

            }
            get {
                return this.value;
            }
        }

        public void Undo() {

            bf_text = true;

            try {
                comboBox.Text = cached;
                try {
                    // comboBox.SelectionStart = cached.Safe_Length();
                    comboBox.SelectAll();
                }
                catch (ArgumentException) {
                    /* ??? */
                }
            }
            finally {
                bf_text = false;
            }
        }

        public void Clear() {

            bf_text = true;

            cached = "";
            value = "";
            this.Text = "";

            bf_text = false;

        }
    }
}