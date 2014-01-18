using System;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Controls
{
    public partial class Our_Short_List : System.Windows.Forms.UserControl
    {

        public int Display_Capacity {
            get {
                int ret = Aux_Item_Area_Height / Item_Height;

                return ret;
            }
        }
        public int Item_Height {
            get {
                int ret = this.FontHeight + 1;

                return ret;
            }
        }

        #region GRAPHIC OBJECTS - 6 Lines
        Pen gfx_border_pen = new Pen(Color.Black);

        SolidBrush gfx_back_brush = new SolidBrush(Color.White);
        SolidBrush gfx_text_brush = new SolidBrush(Color.Black);

        SolidBrush gfx_sel_back_brush = new SolidBrush(Color.DarkBlue);
        SolidBrush gfx_sel_text_brush = new SolidBrush(Color.White);
        Rectangle gfx_border_rect;

        #endregion

        void DrawItemText(Graphics g, Item item, bool selected) {

            ItemBox box;

            if (Aux_Get_Item_Box(item.Index, out box)) {

                var brush = selected ? gfx_back_brush : gfx_text_brush;

                g.DrawString(item.Key,
                            this.Font,
                            brush,
                            box.Boundary1,
                            StringFormat.GenericTypographic);

                g.DrawString(item.Value,
                            this.Font,
                            brush,
                            box.Boundary2,
                            StringFormat.GenericTypographic);
            }
        }
        void DrawItemBackground(Graphics g, Item item, bool selected) {

            ItemBox box;
            if (Aux_Get_Item_Box(item.Index, out box)) {

                var brush = selected ? gfx_sel_back_brush : gfx_back_brush;

                var rect = box.TotalBoundary;

                g.FillRectangle(brush, rect);
            }
        }

        void DrawItemText(Graphics g, ItemBox box, Item item, bool selected) {

            var brush = selected ? gfx_back_brush : gfx_text_brush;

            g.DrawString(item.Key,
                        this.Font,
                        brush,
                        box.Boundary1,
                        StringFormat.GenericTypographic);

            g.DrawString(item.Value,
                        this.Font,
                        brush,
                        box.Boundary2,
                        StringFormat.GenericTypographic);
        }
        void DrawItemBackground(Graphics g, ItemBox box, Item item, bool selected) {

            var brush = selected ? gfx_sel_back_brush : gfx_back_brush;

            var rect = box.TotalBoundary;

            g.FillRectangle(brush, rect);
        }

        bool disable_paint;
        protected override void OnPaint(PaintEventArgs e) {

            e.Graphics.FillRectangle(gfx_back_brush, gfx_border_rect);

            if (disable_paint)
                return;

            e.Graphics.DrawRectangle(gfx_border_pen, gfx_border_rect);

            int c_max = Math.Min(Display_Capacity, items.Count - top_index);

            int cnt_box = 0;
            int cnt_item = top_index;

            for (; cnt_box < c_max;
                   ++cnt_box,
                   ++cnt_item) {

                var box = boxes[cnt_box];
                var item = items[cnt_item];

                bool selected = (cnt_item == selected_index);

                if (selected)
                    DrawItemBackground(e.Graphics, box, item, true);

                DrawItemText(e.Graphics,
                             box,
                             item,
                             selected);
            }
            base.OnPaint(e);
        }

        /// <summary>  Fills min and max with the box numbers between which the
        /// Y coordinates lies.
        /// </summary>
        void Aux_Get_Search_Bounds(int y_coord, out int min, out int max) {

            int y_pos = y_coord - cst_top_offset;
            int height = Item_Height;
            min = y_pos / height;
            max = min;

            #region BINARY SEARCH
            //int prec = 3;
            //int left = cst_top_offset;
            //int coord = y_coord - left;
            //int right = this.Aux_Item_Area_Height;

            //min = 0;
            //max = Display_Capacity;

            //while (--prec > 0) {

            //    int middle = (left + right) / 2 + ((left & 1) & (right & 1));
            //    switch (coord.CompareTo(middle)) {

            //    case -1:
            //        max = (min + max - 1) / 2;
            //        right = middle;
            //        break;

            //    case 1:
            //        min = (min + max - 1) / 2;
            //        left = middle;
            //        break;

            //    default:
            //        min = max = left;
            //        return;
            //    }
            //}
            #endregion
        }

        bool Aux_Get_Item_Box(int item_index, out ItemBox box) {

            box = default(ItemBox);

            bool ret = true;

            int ind = item_index - top_index;

            if (ind < 0 || ind >= Display_Capacity)
                return false;

            box = boxes[ind];

            return ret;
        }

        Rectangle Aux_Get_Item_Total_Rect(int y_cord) {

            var ret = new Rectangle(1,
                                    y_cord,
                                    this.Width /*- gutter_width */- 2,
                                    Item_Height);

            return ret;
        }

        bool Aux_Is_Index_Visible(int index) {

            int temp = index - top_index;
            bool ret = temp >= 0 &&
                       temp < Display_Capacity &&
                       temp < items.Count;

            return ret;
        }

        int Aux_Get_Index_From_Box(int box_number) {

            int ret = top_index + box_number;

            return ret;

        }

        int Aux_Item_Area_Height {
            get {
                int ret = this.Height - cst_top_offset - cst_bottom_offset;

                return ret;
            }
        }

    }
}