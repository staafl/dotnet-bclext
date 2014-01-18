using System;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    public enum ScrollType
    {
        ScrollUntilOnTop,
        ScrollUntilVisible
    }
    public partial class Our_Short_List : System.Windows.Forms.UserControl
    {
#pragma warning disable

        bool disable_scroll;

#pragma warning restore

        bool Aux_Is_Valid_Index(int index) {

            bool ret = index >= 0 && index < items.Count;

            return ret;
        }

        void Aux_Test_Is_Valid_Index(int index) {

            if (!Aux_Is_Valid_Index(index))
                true.tift<ArgumentOutOfRangeException>("Invalid item index.");

        }

        bool Aux_Is_Valid_Scroll_Index(int index) {

            bool ret = (index >= 0) && (index < items.Count - Display_Capacity);

            return ret;
        }

        int Aux_Get_Last_Valid_Top_Index() {

            int item_count = items.Count;

            if (item_count == 0)
                return -1;

            int max_items = Display_Capacity;

            if (item_count < max_items)
                return 0;

            int ret = item_count - max_items;

            return ret;
        }

        int top_index = 0;
        int Top_Index {
            get {
                return top_index;
            }
            set {
                if (top_index != value) {
                    top_index = value;
                    v_scroll.Value = value;
                    Invoke((MethodInvoker)(() => Refresh()));
                }
            }
        }

        void v_scroll_Scroll(object sender, ScrollEventArgs e) {
            if (e.NewValue != e.OldValue)
                Scroll_To_Index(e.NewValue, true);
        }

        /// <summary>  Scrolls the control in such a way that the given
        /// index will be as near the top of the list as possible
        /// 
        /// If index equals -1, nothing is done.
        /// </summary> 
        void Scroll_To_Index(int index, bool force) {

            if (index == -1)
                return;

            if (!Aux_Is_Valid_Index(index))
                true.tift<ArgumentException>("Invalid index: " + index.ToString());

            if (disable_scroll)
                return;

            if (!force && Aux_Is_Index_Visible(index))
                return;

            // new_top will either be the requested index
            int new_top = Math.Min(index, Aux_Get_Last_Valid_Top_Index());

            Top_Index = new_top;
        }


        public void Select_And_Scroll(int index, ScrollType type) {

            bool is_visible = Aux_Is_Index_Visible(index);

            if (is_visible && index == selected_index)
                return;

            Aux_Test_Is_Valid_Index(index);

            if (!is_visible) {

                int new_top;

                if (type == ScrollType.ScrollUntilVisible) {

                    if (index < top_index)

                        // If the item is above in the shortlist
                        // just make that the top index
                        new_top = index;
                    else

                        // If the item is below in the shortlist
                        // it will be the (index - 1*PAGE) + 1
                        new_top = Math.Min(Aux_Get_Last_Valid_Top_Index(),
                                             index - Display_Capacity + 1);
                }
                else {
                    new_top = index;
                }

                if (new_top != top_index)
                    Scroll_To_Index(new_top, true);

            }

            SelectedIndex = index;

        }

        // Version with additional parameter "bool move_selection" removed in rev 227
        public bool Try_Scroll(bool up, int amount, bool wrap) {
            // Last edited on 20091205
            if (amount == 0)
                return false;

            if (this.Items_Count <= 0)
                true.tift<InvalidOperationException>(
                    "The list is empty.");

            int new_selection = selected_index + (up ? -amount : amount);

            if (new_selection < 0 || new_selection >= Items_Count) {

                new_selection = (up == wrap) ? Items_Count - 1 : 0;

            }

            Select_And_Scroll(new_selection, ScrollType.ScrollUntilVisible);


            return true;
        }

        /// <summary>  Sets the position of the scrollbar and the top item to 0
        /// </summary>
        void Reset_Scroll() {

            if (this.items.Count > 0) {

                top_index = 0;
                v_scroll.Value = 0;
            }
        }

    }


}
