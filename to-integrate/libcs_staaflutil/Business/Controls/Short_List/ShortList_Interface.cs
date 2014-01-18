using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


using Fairweather.Service;


namespace Common.Controls
{
    public partial class Our_Short_List : System.Windows.Forms.UserControl
    {
        /// <summary>  Returns the number of visible boxes
        /// which currently have text.
        /// </summary>
        int Aux_Number_Of_Filled_Boxes() {
            int ret = Math.Min(Display_Capacity, Items_Count - top_index);

            return ret;
        }
        int Items_Count {
            get { return items.Count; }
        }
        void Items_Clear() {
            this.items.Clear();
            Refresh();
        }

        public Item? ItemUnderCursor {
            get {
                int box = FindBoxUnderCursor();

                if (box == -1)
                    return null;

                int index = top_index + box;

                if (!Aux_Is_Valid_Index(index))
                    return null;

                return items[index];
            }
        }

        int FindBoxUnderCursor() {

            int min, max;
            Point pt = this.Get_Cursor_Location();

            if (!this.ClientRectangle.Contains(pt))
                return -1;

            int y_coord = pt.Y;

            Aux_Get_Search_Bounds(y_coord,
                                  out min, out max);

            if (max >= Aux_Number_Of_Filled_Boxes())
                return -1;

            for (int ii = min; ii <= max; ++ii) {

                var bounds = boxes[ii].TotalBoundary;
                if (bounds.Top <= y_coord &&
                    bounds.Bottom >= y_coord)
                    return ii;

            }
            return -1;
        }
        public void Close() {

            this.Visible = false;
            selected_index = -1;
            top_index = 0;
        }

        bool Set_Selected_Index(int value, bool from_mouse) {

            if (selected_index == value)
                return false;

            using (var g = this.CreateGraphics()) {

                if (Aux_Is_Index_Visible(selected_index)) {

                    DrawItemBackground(g, items[selected_index], false);
                    DrawItemText(g, items[selected_index], false);

                }

                selected_index = value;
                OnSelectedIndexChanged(from_mouse);

                if (Aux_Is_Index_Visible(selected_index)) {

                    DrawItemBackground(g, items[selected_index], true);
                    DrawItemText(g, items[selected_index], true);
                }
            }

            return true;
        }

        int selected_index;
        /// <summary>  Internal and External use
        /// </summary>
        public int SelectedIndex {
            set {
                Set_Selected_Index(value, false);

            }
            get {
                return selected_index;
            }
        }
        public bool HasSelectedItem {
            get { return Aux_Is_Valid_Index(selected_index); }
        }
        public Item? SelectedItem {
            get {

                return Aux_Is_Valid_Index(selected_index) ? items[selected_index] 
                                                          : (Item?)null;
            }
        }

        int m_column_width = 60;
        public int First_Column_Width {
            get {
                return m_column_width;
            }
            set {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(
                        "FirstColumnWidth can be only a positive integer: " + value.ToString()
                        );

                if (m_column_width != value) {

                    m_column_width = value;
                    Refresh();
                }
            }
        }


        protected override void OnMouseDown(MouseEventArgs e) {


            if (ItemUnderCursor != null)
                OnAcceptChanges(EventArgs.Empty);

            else
                OnRejectChanges(Rejected_Event_Args.Make("", "No item selected."));
        }
        // SELECTION
        protected override void OnMouseMove(MouseEventArgs e) {

            // BUGFIX N 01: BEGIN
            if (!this.Visible)
                return;
            // END

            var pos = (PointF)this.PointToClient(Cursor.Position);

            int min, max;
            Aux_Get_Search_Bounds((int)pos.Y, out min, out max);
            
            if (max >= Aux_Number_Of_Filled_Boxes())
                return;

            for (int ii = min; ii <= max; ++ii) {

                if (boxes[ii].TotalBoundary.Contains(pos)) {

                    Set_Selected_Index(Aux_Get_Index_From_Box(ii), true);
                    break;
                }
            }

            base.OnMouseMove(e);
        }
        protected override void OnMouseWheel(MouseEventArgs e) {
            base.OnMouseWheel(e);
        }

        readonly ItemsCollection m_items_collection;
        public ItemsCollection Items {
            get { return m_items_collection; }
        }
        public class ItemsCollection : IEnumerable<Item>
        {
            readonly Our_Short_List host;
            internal ItemsCollection(Our_Short_List host_p) {
                this.host = host_p;
            }



            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return host.items.GetEnumerator();
            }
            public IEnumerator<Item> GetEnumerator() {
                return host.items.GetEnumerator();
            }
            public int Count {
                get { return host.items.Count; }
            }
            public void Clear() {
                host.items.Clear();
            }
        }
    }
}