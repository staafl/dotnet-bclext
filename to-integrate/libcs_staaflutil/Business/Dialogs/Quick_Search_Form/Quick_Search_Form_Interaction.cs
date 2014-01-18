using System;

using System.Windows.Forms;

namespace Common.Dialogs
{
    /*       Departments mode eviscerated in revision 719        */

    public partial class Quick_Search_Form : Form
    {

        protected override void OnVisibleChanged(EventArgs e) {

            if (this.Visible) {
                shown = true;

                bool ok;
                list_view.RecordsCursor.Prepare(out ok);
                if (!ok)
                    this.Visible = false;
                // this.DialogResult = DialogResult.Cancel;
            }
            else {
                //this.m_host.Refresh();
                list_view.RecordsCursor.End();
            }

            base.OnVisibleChanged(e);
        }

        protected override void OnDeactivate(EventArgs e) {
            //return;
#pragma warning disable

            if (shown && this.Owner != null && !this.Owner.ContainsFocus)
                this.Owner.Activate();

            this.Hide();

            base.OnDeactivate(e);
#pragma warning restore

        }

        void AcceptSelection() {

            Hide();

            this.m_host.Refresh();

            b_ok = true;
            this.DialogResult = DialogResult.OK;

            var result = list_view.HighlightedPair;

            m_host.Collect_QSF_Result(result.Value, m_mode);

            list_view.SelectedIndices.Clear();
        }


        void listView_ItemActivate(object sender, EventArgs e) {
            AcceptSelection();
        }

        void but_ok_Click(object sender, EventArgs e) {
            AcceptSelection();
        }

        void but_cancel_Click(object sender, EventArgs e) {
            Hide();
        }






    }

}
