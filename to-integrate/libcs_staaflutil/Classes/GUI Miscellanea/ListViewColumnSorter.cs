using System.Collections;
using System.Windows.Forms;

namespace Fairweather.Service
{
    public class ListViewColumnSorter : IComparer
    {
        int ColumnToSort;
        SortOrder OrderOfSort;
        CaseInsensitiveComparer ObjectCompare;

        public ListViewColumnSorter() {
            ColumnToSort = 0;

            OrderOfSort = SortOrder.None;

            ObjectCompare = new CaseInsensitiveComparer();
        }

        public int Compare(object x, object y) {

            int compareResult;
            ListViewItem listviewX, listviewY;


            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;


            bool none_X = listviewX.SubItems.Count <= ColumnToSort;
            bool none_Y = listviewY.SubItems.Count <= ColumnToSort;


            if (none_X && none_Y)
                compareResult = 0;

            else if (none_X)
                compareResult = -1;

            else if (none_Y)
                compareResult = 1;

            else
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text,
                                                      listviewY.SubItems[ColumnToSort].Text);


            if (OrderOfSort == SortOrder.Ascending)
                return compareResult;

            else if (OrderOfSort == SortOrder.Descending)
                return (-compareResult);

            else
                return 0;
        }

        public int SortColumn {
            set {
                ColumnToSort = value;
            }
            get {
                return ColumnToSort;
            }
        }

        public SortOrder Order {
            set {
                OrderOfSort = value;
            }
            get {
                return OrderOfSort;
            }
        }

    }
}