using System;
using System.ComponentModel;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    public class Payments_Grid : DataGridView
    {
        public Payments_Grid()
            : base() {
            this.VerticalScrollBar.VisibleChanged += (_1, _2) =>
            {
                if (VerticalScrollBar.Visible)
                    VerticalScrollBarShown.Raise(this, EventArgs.Empty);
                else
                    VerticalScrollBarHidden.Raise(this, EventArgs.Empty);
            };
        }

        public event EventHandler VerticalScrollBarShown;
        public event EventHandler VerticalScrollBarHidden;

        public event Handler_RO<int> Sorting;

        public void Sort(DataGridViewColumn dataGridViewColumn, bool ascending) {

            var eArgsMake_RO = Args.Make_RO(dataGridViewColumn.Index);

            Sorting(this, eArgsMake_RO);

            var order = ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;

            base.Sort(dataGridViewColumn, order);
        }

    }

}
