using System;

namespace Fairweather.Service
{

#if WINFORMS
    using System.Windows.Forms;

    public class ListViewItemComparer : System.Collections.IComparer
    {
        int column;
        SortOrder order;

        public ListViewItemComparer(int col, SortOrder sort) {
            column = col;
            order = sort;
        }

        public int Compare(object a, object b) {
            int ret = -1;

            string lva = ((ListViewItem)a).SubItems[column].Text;
            string lvb = ((ListViewItem)b).SubItems[column].Text;

            ret = String.Compare(lva, lvb);

            if (order == SortOrder.Descending)
                ret *= -1;

            return ret;
        }
    }
#endif
}