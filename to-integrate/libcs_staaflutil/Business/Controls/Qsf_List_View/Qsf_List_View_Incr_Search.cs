using System;
using System.Windows.Forms;


namespace Common.Controls
{
    partial class QSF_ListView
    {
        bool b_no_reset;

        protected override void OnSelectedIndexChanged(EventArgs e) {

            if (!b_no_reset)
                search.ResetSearch();

            if (SelectedIndices.Count > 0) {

                Items[SelectedIndices[0]].Focused = true;

            }

            base.OnSelectedIndexChanged(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e) {

            var text = Char.ToUpperInvariant(e.KeyChar).ToString();

            if (!TryFindAndSelect(text, false, false)) {

                search.ResetSearch();

            }
            e.Handled = true;

            base.OnKeyPress(e);

        }

        public void Set_Selected_Item(ListViewItem lvi, bool ensure_visible, bool make_top) {

            b_no_reset = true;
            try {
                this.SelectedIndices.Clear();
                if (ensure_visible)
                    lvi.EnsureVisible();

                if (make_top)
                    this.TopItem = lvi;

                lvi.Focused = true;
                lvi.Selected = true;
            }
            finally {
                b_no_reset = false;
            }
        }


        protected override void OnSearchForVirtualItem(SearchForVirtualItemEventArgs e) {

            b_no_reset = true;

            try {

                var text = e.Text.ToUpper();

                search.AcceptString(text);

                var real_text = search.CurrentSearchString;


                var null_index = RecordsCursor.GetIndex(real_text, true);

                bool ok = null_index.HasValue;

                if (ok) {
                    e.Index = null_index.Value;
                    Prepare_Cache(e.Index - grace, e.Index + grace);
                }
                else {
                    e.Index = -1;
                }

                base.OnSearchForVirtualItem(e);

                #region commented out legacy code

                //if (ok) {
                //      BeginInvoke((MethodInvoker)(() =>
                //      {
                //            try {
                //                  if (this.SelectedIndices.Count == 0)
                //                        return;

                //                  var ind = this.SelectedIndices[0];
                //                  this.SelectedIndices.Clear();
                //                  this.SelectedIndices.Add(ind);
                //            }
                //            finally {
                //                  b_OnSearchForVirtualItem = false;
                //            }

                //      }));
                //}
                //else  
                #endregion

                {

                    b_no_reset = false;
                }
            }
            catch {
                b_no_reset = false;
                throw;
            }
        }

        public void Reset_Search() {
            search.ResetSearch();

        }

        public ListViewItem TryFind(string text) {
            return TryFind(text, true);
        }

        ListViewItem TryFind(string text, bool reset) {

            if (reset)
                search.ResetSearch();

            var ret = FindItemWithText(text);

            return ret;

        }

        public bool TryFindAndSelect(string text, bool make_top) {
            return TryFindAndSelect(text, make_top, true);
        }

        bool TryFindAndSelect(string text, bool make_top, bool reset) {

            var lvi = TryFind(text, reset);

            if (lvi == null)
                return false;

            Set_Selected_Item(lvi, true, true);


            return true;
        }

    }
}
