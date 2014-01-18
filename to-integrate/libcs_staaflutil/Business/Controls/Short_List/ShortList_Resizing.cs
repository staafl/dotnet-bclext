using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Fairweather.Service;

namespace Common.Controls
{

    public partial class Our_Short_List : System.Windows.Forms.UserControl
    {
        protected override void OnVisibleChanged(EventArgs e) {

            if (is_resizing)
                FinishResizing();

            base.OnVisibleChanged(e);
        }
        protected override void OnParentVisibleChanged(EventArgs e) {

            if (is_resizing)
                FinishResizing();

            base.OnParentVisibleChanged(e);
        }

        bool is_resizing;

        Point dragging_point;

        void rsz_box_MouseDown(object sender, MouseEventArgs e) {

            BeginDragging();
        }

        void rsz_box_MouseMove(object sender, MouseEventArgs e) {

            if (is_resizing) {

                Drag();
            }
        }

        void rsz_box_MouseUp(object sender, MouseEventArgs e) {

            if (is_resizing) {


                FinishDragging();

            }
            is_resizing = false;
        }

        bool bf_resize;
        protected override void OnResize(EventArgs e) {

            if (bf_resize)
                return;

            try {
                bf_resize = true;

                if (b_init) {

                    if (!is_resizing)
                        Adjust_Size();

                    // To be done every time the size is changed
                    Adjust_Border();

                    Prepare_Boxes();
                    // ___

                    // To be done only if the size is final
                    if (!is_resizing) {
                        FinishResizing();
                    }// ___
                }

                base.OnResize(e);
            }
            finally {
                bf_resize = false;
            }
        }

        partial void Prepare_Boxes() {

            int item_size = this.Item_Height;

            int y_pos = this.ClientRectangle.Top + cst_top_offset;

            int right_column_width = this.Width - m_column_width;

            this.boxes.Clear();

            int max = Display_Capacity;

            for (int ii = 0; ii < max; ++ii) {

                var rect1 = new Rectangle(cst_left_offset,
                                          y_pos,
                                          m_column_width,
                                          item_size);

                var rect2 = new Rectangle(
                                         cst_left_offset + m_column_width,
                                         y_pos,
                                         right_column_width,
                                         item_size);

                var rect_tot = Aux_Get_Item_Total_Rect(y_pos);
                y_pos += item_size;

                boxes.Add(new ItemBox(rect1, rect2, rect_tot));
            }
        }

        partial void Adjust_Size() {

            int list_height = this.Aux_Item_Area_Height;
            int rem = list_height % Item_Height;

            if (rem == 0)
                return;

            int height = this.Height + Item_Height - rem;

            if (MaximumSize != Size.Empty && height > MaximumSize.Height)
                height -= Item_Height;

            this.Height = height;
        }

        partial void Adjust_Border() {
            gfx_border_rect = new Rectangle(this.ClientRectangle.Left,
                                this.ClientRectangle.Top,
                                this.ClientRectangle.Width - 1,
                                this.ClientRectangle.Height - 1);
        }

        partial void Refresh_Scrollbar_Settings() {

            v_scroll.Minimum = 0;
            v_scroll.Maximum = Aux_Get_Last_Valid_Top_Index() +
                               v_scroll_large_change  //This is required to make sure the scrollbar can scroll to the end
                               - 1;

            v_scroll.Enabled = this.Items_Count > this.Display_Capacity;

            v_scroll.Refresh();
        }

        partial void Adjust_Scrbar_Height_Location() {
            v_scroll.Location = new Point(this.Width - v_scroll.Width - border_thickness,
                                          border_thickness);

            v_scroll.Height = this.Height - border_thickness - rsz_box_h;
        }

        partial void Adjust_Rszbox_Location() {

            var loc = this.ClientRectangle
                                   .Vertex(2)
                                   .Translate(rsz_box.Size, false)
                                   .Translate(-1, -1);
            rsz_box.Location = loc;
        }


        partial void BeginDragging() {

            Debug.Assert(!is_resizing);

            is_resizing = true;

            rsz_box.Capture = true;
            rsz_box.Location = rsz_box.Location
                                      .Translate(rsz_box_drag_w_offset, 0);
            rsz_box.Width -= rsz_box_drag_w_offset;

            dragging_point = rsz_box.PointToClient(Cursor.Position);

            this.v_scroll.Visible = false;
        }


        void Drag() {

            var preview_point = this.Get_Cursor_Location()
                                    .Translate(dragging_point, false);

            var preview_edge = preview_point.Translate(rsz_box.Size, true);

            Size preview_size = (Size)preview_edge;

            Point parent_edge = this.TopLevelControl
                                    .ClientRectangle
                                    .Vertex(2);

            parent_edge = this.TopLevelControl.Point_On_Control(this, parent_edge);
            int compare = parent_edge.Compare(preview_edge);


            // In case this will result in an invalid size
            // move cursor back and return

            // && has greater precedence than ||

            bool max = MaximumSize != Size.Empty;
            bool min = MinimumSize != Size.Empty;

            int width = preview_size.Width;
            int height = preview_size.Height;

            int min_w = MinimumSize.Width;
            int max_w = MaximumSize.Width;

            int min_h = MinimumSize.Height;
            int max_h = MaximumSize.Height;


            bool xx_ok = (!max || (preview_size.Width >= MinimumSize.Width))
                         &&
                         (!min || (preview_size.Width <= MaximumSize.Width));

            bool yy_ok = (!max || (preview_size.Height <= MaximumSize.Height))
                         &&
                         (!min || (preview_size.Height >= MinimumSize.Height));
            

            if ((!yy_ok && !xx_ok) ||
                parent_edge.Compare(preview_edge) != 1) {

                Adjust_Rszbox_Location();
                Cursor.Position = PointToScreen(rsz_box.Location
                                                       .Translate(dragging_point, true));
                return;
            }


            rsz_box.Location = preview_point;
            rsz_box.Refresh();

            this.Size = (Size)preview_edge;

            var bounds = this.Bounds;
            this.Parent.Invalidate(bounds);
            this.Parent.Update();

            this.Adjust_Border();
            this.Prepare_Boxes();

        }


        partial void FinishDragging() {

            Debug.Assert(is_resizing);
            disable_paint = false;

            bf_resize = false;

            rsz_box.Location = rsz_box.Location.Translate(-rsz_box_drag_w_offset, 0);
            rsz_box.Width += rsz_box_drag_w_offset;
            rsz_box.Capture = false;

            this.v_scroll.Visible = true;

            Adjust_Size();
            FinishResizing();

            if (top_index > Aux_Get_Last_Valid_Top_Index())
                Scroll_To_Index(Aux_Get_Last_Valid_Top_Index(), true);

        }

        partial void FinishResizing() {

            this.Adjust_Size();
            this.Adjust_Rszbox_Location();
            this.Refresh_Scrollbar_Settings();
            this.Adjust_Scrbar_Height_Location();

        }
    }
}