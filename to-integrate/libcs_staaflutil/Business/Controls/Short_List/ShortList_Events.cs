using System;


using Fairweather.Service;

namespace Common.Controls
{
    using EventHandler = System.EventHandler<System.EventArgs>;

    public partial class Our_Short_List : System.Windows.Forms.UserControl
    {
        public event EventHandler AcceptChanges;
        public event EventHandler<Rejected_Event_Args<string>> RejectChanges;
        public event Handler_RO<bool> SelectedIndexChanged;

        protected virtual void OnAcceptChanges(EventArgs e) {
            AcceptChanges.Raise(this, e);
        }

        protected virtual void OnRejectChanges(Rejected_Event_Args<string> e) {
            RejectChanges.Raise(this, e);
        }

        protected virtual void OnSelectedIndexChanged(bool from_mouse) {
            SelectedIndexChanged.Raise(this, Args.Make_RO(from_mouse));
        }


        // OWN
        public event EventHandler ItemsChanged;
        protected virtual void OnItemsChanged(EventArgs e) {

            if (b_init) {
                this.Invalidate();
                this.Refresh();
                this.Refresh_Scrollbar_Settings();
            }

            ItemsChanged.Raise(this, e);
        }
        // ___




        // BASE

        // ___
    }
}