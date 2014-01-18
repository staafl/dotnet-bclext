using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Common;
using Fairweather.Service;
using Standardization;


namespace Common.Dialogs
{
    public partial class New_Customer
    {
        void but_Click(object sender, EventArgs e) {

            if (sender == but_save) {

                if (cb_account.Value.IsNullOrEmpty()
                    && cb_account.New_Account.IsNullOrEmpty())
                    return;

                Save_Changes();
                return;
            }

            if (sender == but_delete) {

                if (cb_account.Value.IsNullOrEmpty())
                    return;

                Handle_Delete();
                return;
            }
        }




        void but_previous_Click(object sender, EventArgs e) {

            int cnt = RecordsCursor.Count;
            bool ok;

            var old = current_index;
            try {
                using (var disp = RecordsCursor.Prepare(out ok)) {

                    if (!ok)
                        return;
                    if (current_index >= 1)
                        --current_index;
                    else
                        current_index = cnt - 1;

                    var account = RecordsCursor.GetAtIndex(current_index);

                    Select_Customer(account.First);
                }
            }
            catch {
                current_index = old;
                throw;
            }

        }

        void Select_Customer(string customer) {

            cb_account.Value = customer;

            Accept_Selected_User();

        }

        void but_next_Click(object sender, EventArgs e) {

            int cnt = RecordsCursor.Count;

            if (current_index >= cnt)
                current_index = 0;
            else
                ++current_index;

            var account = RecordsCursor.GetAtIndex(current_index);

            Select_Customer(account.First);

        }

        void but_close_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            Try_Close();
        }

        void but_keyboard_Click_1(object sender, EventArgs e) {
            base.but_keyboard_Click(sender, e);
        }





        void but_discard_Click(object sender, EventArgs e) {

            Clear_Controls(true);

        }




        void Refresh_Buttons() {
        }



        Record_Type Record_Type {
            get {
                return customer ? Record_Type.Sales : Record_Type.Purchase;
            }
        }

        void Accept_Selected_User() {

            new_account = null;
            lab_new_acc.Visible = false;
            string value = cb_account.Value;
            Last_Accessed_Record = value;

            binding.Load(value, Record_Type);

            tb_name.Focus();
            tb_name.SelectAll();

            current_index = RecordsCursor.GetIndex(value, false).Value;

        }

        void Handle_New_Account(bool tb_name_focus) {

            current_index = -1;

            /*       Hack        */

            this.Clear_Controls(false);

            lab_new_acc.Visible = true;

            new_account = cb_account.Text;


            if (tb_name_focus)
                tb_name.Focus();

        }


        void Handle_Create(Account_Record record) {

            RecordsCursor.Add_New(record.Account_Ref, record.Name);
            Last_Accessed_Record = record.Account_Ref;

        }

        void Handle_Change(Account_Record record) {

            RecordsCursor.Change_Value(record.Account_Ref, record.Name);
            Last_Accessed_Record = record.Account_Ref;

        }

        void Handle_Delete() {

            var value = cb_account.Value;

            if (value.IsNullOrEmpty())
                return;

            if (!Standard.Ask("Delete customer?"))
                return;


            Account_Record _;
            string error_msg;

            bool ok;

            H.assign(out ok, out _, out error_msg);

            if (!Sage_Access.Sage_Guard(() => ok = m_sdr.Delete_Customer(value, out error_msg, out _)))
                return;

            if (ok) {

                Clear_Controls(true);

            }
            else {

                Standard.Show(Message_Type.Error, error_msg);

            }

            RecordsCursor.Remove(value);
        }


        void Save_Changes() {

            var changes = new Dictionary<string, object>();

            bool is_new_account = !new_account.IsNullOrEmpty();

            string acc_ref = is_new_account ? cb_account.New_Account : cb_account.Value;

            var null_record = binding.Store(is_new_account, acc_ref, Record_Type);

            if (!null_record.HasValue)
                return;

            var record = null_record.Value;

            //foreach (var kvp in this.m_fields) {

            //    var field = kvp.First;

            //    var text = kvp.Second.Output();

            //    if (is_new_account || text != m_old_values[field])
            //        changes.Add(field, text);


            //}

            //if (!is_new_account && changes.Is_Empty())
            //    return;


            //var record = Sage_Access.Get_Value(
            //    () => m_sgr.Update_Account_Record(is_new_account, acc_ref, Record_Type, changes));

            if (!Sage_Access.Sage_Guard(() =>
            {
                if (is_new_account)
                    Handle_Create(record);
                else
                    Handle_Change(record);
            }))
                return;

            this.DialogResult = DialogResult.OK;
            Try_Close();

            //Clear_Controls(true);
        }

        public string Last_Accessed_Record {
            get;
            set;
        }

        void Set_Default_Country() {
            cb_country.SelectedIndex = default_country;
        }

        void Clear_Controls(bool clear_cb) {

            lab_new_acc.Visible = false;

            binding.Clear("");

            Set_Default_Country();

            if (clear_cb) {

                cb_account.Value = "";
                cb_account.Text = "";
                cb_account.Focus();

            }
        }
    }
}
