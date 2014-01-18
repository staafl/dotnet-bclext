using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    partial class QSF_ListView
    {
        public event EventHandler<Args<bool, Pair<int, SortOrder?>>> BeforeSorting;

        void On_BeforeSorting(Args<bool, Pair<int, SortOrder?>> e) {

            BeforeSorting.Raise(this, e);

        }

        Incremental_Search_Provider incr_search = new Incremental_Search_Provider();
        int sortedColumn;

        public int SortedColumn {
            [DebuggerStepThrough]
            get {
                return sortedColumn;
            }
            set {
                if (value != sortedColumn) {
                    sortedColumn = value;
                }
            }
        }

        public void Sort(int column, SortOrder? sort_order) {

            var is_second_col = (column == 1);

            if (is_second_col &&
                !RecordsCursor.Supports_Second_Column)
                return;

            var pair = new Pair<int, SortOrder?>(column, sort_order);

            var args = Args.Make(false, pair);

            On_BeforeSorting(args);

            if (args.Mut)
                return;

            incr_search.ResetSearch();
            SortOrder order;

            if (sort_order.HasValue) {
                order = sort_order.Value;

            }
            else if (column != SortedColumn) {
                order = SortOrder.Ascending;

            }
            else if (Sorting == SortOrder.Ascending) {
                order = SortOrder.Descending;

            }
            else {
                order = SortOrder.Ascending;

            }


            Sorting = order;
            SortedColumn = column;

            if (order == SortOrder.Ascending)
                this.RecordsCursor.Forward = true;
            else
                this.RecordsCursor.Forward = false;

            if (column == 0)
                this.RecordsCursor.First_Column = true;
            else
                this.RecordsCursor.First_Column = false;

            Force_Refresh();


            Invalidate(new Rectangle(new Point(0, 0), new Size(Width, Font.Height)),
                       true);
        }



        protected override void OnColumnClick(ColumnClickEventArgs e) {

            this.Sort(e.Column, null);

            base.OnColumnClick(e);
        }

    }
}
