using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Colors = Standardization.Colors;

using Fairweather.Service;


using Common;


namespace Screens
{
    class New_Customer_Tab_Control : Common.Controls.Advanced_Tab_Control
    {
        Details_Tab details_tab = new Details_Tab();

        public New_Customer_Tab_Control() {

            details_tab.Name = "Details";
            this.Initialize(details_tab);
            this.Activate_Tab(0);
            details_tab.Visible = true;
        }

        protected override void OnPaint(PaintEventArgs e) {


            base.OnPaint(e);

        }

        string new_account;

        void Refresh_Buttons() {
        }


        void Accept_Selected_User() {

            new_account = null;
            lab_new_acc.Visible = false;


            var array = m_fields.Lefts.ToArray();

            var dict = m_sgr.Get_Field_Data(cb_account.Value, RecordType.Customer, array);

            m_old_values.Clear();

            foreach (var kvp in dict) {

                string key = kvp.Key;
                object val = kvp.Value;

                m_fields[key].Input(val);
                m_old_values[key] = val;

            }

            tb_name.Focus();
            tb_name.SelectAll();

        }

        public bool Try_Save() {

            if (cb_account.Value.IsNullOrEmpty()
                && cb_account.New_Account.IsNullOrEmpty())
                return false;

            Save_Changes();
            return true;
        }

        public bool Try_Delete() {
            if (cb_account.Value.IsNullOrEmpty())
                return;

            Handle_Delete();
            return;
        }

        public void Next_User() {

            var sr_list = cb_account.AutoCompleteCustomSource;
            if (cb_account.Value == null) {
                cb_account.Value = sr_list.ElementAt(0).Key;
                Accept_Selected_User();

                return
                    ;
            }

            int index = sr_list.IndexOfKey(cb_account.Value);
            ++index;

            int cnt = sr_list.Count;

            cb_account.Value = sr_list.ElementAt(index % cnt).Key;
            Accept_Selected_User();

        }

        void Handle_Delete() {

            var value = cb_account.Value;

            if (value.IsNullOrEmpty())
                return;

            if (!Standard.Show_Yes_No("Delete customer?"))
                return;

            var either = m_sgr.Delete_Customer(value);

            if (either.Is_Right) {

                var account = either.Right.Value;
                m_on_deleted(account);
                cb_account.AutoCompleteCustomSource.Remove(value);

                Clear_Controls(true);
            }
            else {

                Standard.Show_Error_Message(either.Left, false);

            }
        }

        public void Prev_User() {

            var sr_list = cb_account.AutoCompleteCustomSource;

            if (cb_account.Value == null) {
                cb_account.Value = sr_list.Last().Key;
                Accept_Selected_User();

                return
                    ;
            }

            int index = sr_list.IndexOfKey(cb_account.Value);
            --index;

            int cnt = sr_list.Count;

            if (index <= -1)
                index = cnt - 1;

            cb_account.Value = sr_list.ElementAt(index % cnt).Key;
            Accept_Selected_User();
        }

        public void Clear_Controls(bool clear_cv) {

            lab_new_acc.Visible = false;

            m_old_values.Clear();

            foreach (var pair in m_fields.Rights) {

                pair.Input(String.Empty);

            }

            if (clear_cv) {

                cb_account.Value = String.Empty;
                cb_account.Text = String.Empty;
                cb_account.Focus();

            }
        }


        void Save_Changes() {

            var changes = new Dictionary<string, object>();

            bool is_new_account = !new_account.IsNullOrEmpty();
            foreach (var kvp in this.m_fields) {

                var field = kvp.First;

                var text = kvp.Second.Output();

                if (is_new_account || text != m_old_values[field])
                    changes.Add(field, text);


            }

            if (changes.Is_Empty())
                return;

            string acc_ref = is_new_account ? cb_account.Text : cb_account.Value;

            var account = m_sgr.Update_Account_Record(is_new_account, acc_ref, RecordType.Customer, changes);

            if (is_new_account) {


                m_on_creation(account);
                QuickSearchForm.Refresh_Accounts(QuickSearchFormMode.CustomerAccounts);
                QuickSearchForm.Refresh_Accounts(QuickSearchFormMode.CustomerAccounts_new);


                Account_Added(account);
            }

            Clear_Controls(true);
        }

        public void Account_Removed(Account_Record account) {

            cb_account.AutoCompleteCustomSource.Remove(account.Account);
        }

        public void Account_Added(Account_Record account) {

            cb_account.AutoCompleteCustomSource.Add(account.Account, account.Name);

        }



    }
}