using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    /// <summary>  This class is not thread-safe
    /// </summary>
    /// IDEAS:
    /// * Hide the selected item while scrolling
    public partial class Our_Short_List : System.Windows.Forms.UserControl
    {
        #region CONSTANTS - 12 Lines
        const int v_scroll_large_change = 5;
        const int v_scroll_small_change = 1;

        const int gutter_width = 18;
        const int v_scroll_width = gutter_width;

        const int rsz_box_w = gutter_width + 1;
        const int rsz_box_h = gutter_width - 6;
        const int rsz_box_drag_w_offset = 6;

        const int border_thickness = 1;

        const int cst_left_offset = 5;
        const int cst_top_offset = 4;
        const int cst_bottom_offset = 3;

        readonly Size cst_min_size = new Size(100, 40);
        #endregion

        List<Item> items = new List<Item>();
        List<ItemBox> boxes = new List<ItemBox>();

        PictureBox rsz_box;
        VScrollBar v_scroll;

        readonly bool b_init;

        //CONSTRUCTOR
        public Our_Short_List() {
            disable_paint = true;

            m_items_collection = new ItemsCollection(this);

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          //ControlStyles.UserMouse |
                          ControlStyles.UserPaint |
                          ControlStyles.Opaque |
                          ControlStyles.ResizeRedraw,
                          true);

            this.SetStyle(ControlStyles.Selectable, false);
            this.MinimumSize = cst_min_size;
            var hndl = this.Handle;

            this.Size = new Size(150, 495);
            this.TabStop = false;

            Adjust_Size();
            Prepare_Boxes();
            Adjust_Border();

            Create_Scroll_Bar();
            Create_Resizing_Box();

            Set_Event_Handlers();

            b_init = true;
            disable_paint = false;
        }

        void Set_Event_Handlers() {

            v_scroll.Scroll += v_scroll_Scroll;
            rsz_box.MouseDown += rsz_box_MouseDown;
            rsz_box.MouseUp += rsz_box_MouseUp;
            rsz_box.MouseMove += rsz_box_MouseMove;
        }

        void Create_Resizing_Box() {

            rsz_box = new PictureBox();
            (rsz_box as ISupportInitialize).BeginInit();

            rsz_box.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            rsz_box.BackgroundImage = global::Fairweather.Service.Properties.Resources.img_resize_grip;

            rsz_box.BackgroundImageLayout = ImageLayout.Zoom;
            rsz_box.Cursor = Cursors.SizeNWSE;

            // TODO
            rsz_box.Size = new Size(12, rsz_box_h);//rsz_box_w, rsz_box_h);

            this.Controls.Add(rsz_box);

            Adjust_Rszbox_Location();
            (rsz_box as ISupportInitialize).EndInit();

            rsz_box.Visible = true;
        }

        void Create_Scroll_Bar() {

            // The scrollbar's maximum property should be
            // adjusted every time items are added or removed

            v_scroll = new VScrollBar();

            v_scroll.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            v_scroll.LargeChange = v_scroll_large_change;
            v_scroll.SmallChange = v_scroll_small_change;
            v_scroll.Width = v_scroll_width;

            this.Controls.Add(v_scroll);


            Refresh_Scrollbar_Settings();
            Adjust_Scrbar_Height_Location();

            v_scroll.Visible = true;
        }

        public void SetItems(IEnumerable<Pair<string>> items) {

            this.disable_paint = true;

            try {
                this.items.Clear();
                Reset_Scroll();
                int capacity = this.Display_Capacity;

                this.items.AddRange(
                                    items.Select((pair, _ind) =>
                                        new Item(pair.First, pair.Second, _ind))
                                   );


                disable_paint = false;

                OnItemsChanged(EventArgs.Empty);
            }
            finally {
                disable_paint = false;
            }
        }

        protected override void OnTabStopChanged(EventArgs e) {
            this.TabStop = false;
            base.OnTabStopChanged(e);
        }
        protected override void OnGotFocus(EventArgs e) {
            base.OnGotFocus(e);
        }

        IContainer components = null;
        protected override void Dispose(bool disposing) {

            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        /*       Test removed - 07th September        */

    }
}