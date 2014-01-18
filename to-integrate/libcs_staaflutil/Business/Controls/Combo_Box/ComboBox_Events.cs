using System;
using System.Diagnostics;
using System.Windows.Forms;
using Fairweather.Service;
using EventHandler = System.EventHandler<System.EventArgs>;

namespace Common.Controls
{
    public partial class Our_Combo_Box : UserControl
    {

        bool bi_listbox;
        bool bf_examine;
        bool bf_text;
        bool bf_del;
        bool bf_sel;
        bool bf_focus;
        bool bf_show;

        public event EventHandler Accept_Changes;

        /// <summary> Occurs when focus leaves the combo box while it contains a text which is not a recognized
        /// entry </summary>
        public event EventHandler New_Account_Event;

        public event EventHandler<Rejected_Event_Args<string>> Reject_Changes;

        public event EventHandler Selected_Index_Changed;
        public event EventHandler List_View_Shown;
        public event EventHandler List_View_Hidden;

        public event EventHandler Text_Changed;

        //[DebuggerStepThrough]
        protected virtual void OnAcceptChanges(EventArgs e) {

            //SelectionLength = 0;
            SelectAll();
            
            value = Text;

            Accept_Changes.Raise(this, e);

            if (this.Auto_Tab) 
                this.Select_Next_Safe(true);

        }

        void On_NewAccount() {

            New_Account_Event.Raise(this, EventArgs.Empty);

        }

        [DebuggerStepThrough]
        protected virtual void OnRejectChanges(Rejected_Event_Args<string> e) {

            Undo();

            Reject_Changes.Raise(this, e);

        }

        protected virtual void Handle_Blank() {

            this.Select_Focus();

        }

        protected override void OnLeave(EventArgs e) {

            if (short_list.Focused)
                return;

            if (this.ContainsFocus)
                return; 

            try {
                var result = Handle_Changes(true, false, false);

                bf_show = true;     

                if (this.Value.IsNullOrEmpty() &&
                    this.m_new_account.IsNullOrEmpty()) {

                    if (!this.Allow_Blank) {

                        Handle_Blank();
                        return;

                    }

                }

                this.SelectionLength = 0;
                short_list.Close();

                if (comboBox.Text != this.Value) {


                    if (result == Selection_Result.Empty_Account_OK || 
                       !result.Contains(Selection_Result.cst_ok)) {

                        Set_Safe_Text(this.Value);

                    }
                    else {

                        if (result == Selection_Result.Invalid_Account)
                            this.value = null;

                    }

                }

                base.OnLeave(e);
            }
            finally {
                bf_show = false;
            }
        }

        [DebuggerStepThrough]
        protected override void OnEnter(EventArgs e) {

            comboBox.Focus();
            comboBox.Select();
            comboBox.SelectAll();
        }

        [DebuggerStepThrough]
        protected virtual void OnListViewVisibleChanged(EventArgs e) {
            if (short_list.Visible == true) {
                if (bf_show)
                    return;
                if (short_list.Items.Count > 0)
                    short_list.SelectedIndex = 0;
                OnListViewShown(e);
            }
            else {
                OnListViewHidden(e);
            }
        }

        [DebuggerStepThrough]
        protected virtual void OnListViewShown(EventArgs e) {
            if (List_View_Shown != null)
                List_View_Shown(this, e);
        }

        [DebuggerStepThrough]
        protected virtual void OnListViewHidden(EventArgs e) {
            if (List_View_Hidden != null)
                List_View_Hidden(this, e);
        }

    }
}