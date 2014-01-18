using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Fairweather.Service;

namespace Common.Controls
{
    public partial class Our_Combo_Box : UserControl
    {
        readonly Size def_min_shlist_size = new Size(260, 130);

        Size min_shlist_size = new Size(260, 130);

        protected readonly Our_Short_List short_list = new Our_Short_List();

        public Our_Short_List Short_List {
            get {
                return short_list;
            }
        }

        //  States to which side of the combo-box the shortlist should be
        /// aligned. Default is Left.
        //  HorizontalAlignment.Center is the same as Left.
        /// HorizontalAlignment.Right depends on the Shortlist's width.
        [System.ComponentModel.DesignerSerializationVisibility(
         System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public Rectangle_Vertex Short_List_Alignment {
            get;
            set;
        }

        static readonly Dictionary<Rectangle_Vertex, Quad<int>>
        shortlist_position = new Dictionary<Rectangle_Vertex, Quad<int>>
        {
            {new Rectangle_Vertex(0), Quad.Make(3, 0, -1, 0)},
            {new Rectangle_Vertex(1), Quad.Make(2, 1, 1, 0)},
            {new Rectangle_Vertex(2), Quad.Make(1, 2, 1, 0)},
            {new Rectangle_Vertex(3), Quad.Make(0, 3, -1, 0)},
        };

        Point Get_Location_For_Shortlist() {


            var bounds = this.Bounds_On_Screen();
            var shlist_crect = this.short_list.ClientRectangle;
            var tlc = this.TopLevelControl;

            var quad = shortlist_position[Short_List_Alignment];

            int v1 = quad.First,
                v2 = quad.Second,
                xx = quad.Third,
                yy = quad.Fourth;

            var rect3 = shlist_crect.Align_Vertices(bounds, v1, v2);

            var ret = tlc.PointToClient(rect3.Location);

            ret = ret.Translate(xx, yy);

            return ret;
        }

        void Maybe_Init_Shortlist() {

            if (bi_listbox) {
                short_list.Items.Clear();
                return;
            }

            //Should probably be in OnParentChanged

            short_list.Visible = false;

            short_list.MinimumSize = min_shlist_size;

            //Controls.Add(short_list);
            ParentForm.Controls.Add(short_list);
            ParentForm.FormClosing += ParentForm_FormClosing;

            bi_listbox = true;


        }


        protected override void OnTextChanged(EventArgs e) {

            bool bf_flag = false;
            try {
                if (!this.Visible)
                    return;

                if (suspend_shortlist)
                    return;

                string text = Text;

                if (text.IsNullOrEmpty()) {

                    short_list.Close();
                    return;

                }

                if (bf_text)
                    return;

                bf_text = true;
                bf_flag = true;
                List<Pair<string>> list = null;

                bool rc_prepare;
                using (RecordsCursor.Prepare(out rc_prepare)) {

                    if (!rc_prepare)
                        return;

                    if (RecordsCursor.Is_Empty)
                        return;

                    Maybe_Init_Shortlist();

                    list = RecordsCursor.GetPureRange(text, -1, StringComparison.InvariantCultureIgnoreCase);

                }

                short_list.SetItems(list);

                if (!list.Is_Empty() && !bf_del) {

                    bf_text = true;
                    try {

                        int sel_start = comboBox.Text.Length;
                        comboBox.Text = list[0].First;
                        comboBox.SelectionStart = sel_start;
                        comboBox.SelectionLength = 200;

                    }
                    finally {
                        bf_text = false;
                    }
                }



                if (short_list.Items.Count < Minimum_Shortlist_Items) {

                    short_list.Close();
                    return;

                }

                //We need to display it to the user

                //HACK


                int k = list.Max(pair => pair.Second.Length);

                k += 16;

                k *= 7;

                k = Math.Max(k, def_min_shlist_size.Width);
                k = Math.Max(k, short_list.Width);

                min_shlist_size = new Size(k, def_min_shlist_size.Height);

                short_list.MinimumSize = min_shlist_size;
                short_list.Size = min_shlist_size;

                if (!short_list.Visible) {

                    short_list.Location = Get_Location_For_Shortlist();
                    short_list.Invalidate();

                    bf_show = true;

                    try {
                        short_list.Show();
                    }
                    finally {
                        bf_show = false;
                    }

                }

                short_list.BringToFront();

                if (!bf_focus && !short_list.Items.Is_Empty()) {

                    bf_sel = true;
                    try {
                        short_list.SelectedIndex = 0;
                    }
                    finally {
                        bf_sel = false;
                    }
                }

            }
            finally {

                bf_focus = false;
                bf_del = false;
                if (bf_flag)
                    bf_text = false;

                Text_Changed.Raise(e);

                base.OnTextChanged(e);
            }
        }

        void Handle_Delete(bool backspace) {

            if (this.Text.IsNullOrEmpty())
                return;

            bf_del = true;
            bf_focus = true;

            int sel = comboBox.SelectionStart;

            do {
                if (comboBox.SelectionLength > 0) {
                    /*       Selected text        */


                    Text = comboBox.Text.Remove(sel, comboBox.SelectionLength);
                    break;

                }


                if (backspace) {

                    /*       Backspace key        */

                    if (sel > 0) {

                        Text = Text.Remove(sel - 1, 1);
                        --sel;

                    }
                    break;
                }

                /*       Delete key        */

                if (sel <= comboBox.Text.Length - 1) {

                    Text = Text.Remove(sel, 1);

                    break;
                }

            } while (false);


            comboBox.SelectionStart = sel;
        }

        bool Handle_Keys(Keys k) {

            if (k == Keys.Delete || k == Keys.Back) {

                Handle_Delete(k == Keys.Back);

                return true;
            }

            if (k == Keys.Escape) {

                if (short_list.Visible) {

                    short_list.Close();
                    return true;

                }
                else {
                    if ((this.Text == this.cached) ||
                         (this.Text.IsNullOrEmpty() && this.cached.IsNullOrEmpty())) {
                        return false;
                    }
                    else {

                        Undo();
                        SelectAll();

                        return true;

                    }
                }
            }

            if (k == Keys.Enter) {

                if (SelectedIndex != -1 && short_list.Visible) {

                    bf_text = true;
                    comboBox.Text = SelectedItem;
                    short_list.Close();

                    bf_text = false;
                }

                Handle_Changes(false, true, false);

                return true;

            }

            return false;
        }

        void listView_KeyDown(object sender, KeyEventArgs e) {

            Handle_Keys(e.KeyData);

        }

        public Size Max_Shortlist_Size {
            get { return short_list.MaximumSize; }
            set { short_list.MaximumSize = value; }
        }

        static readonly Dictionary<Keys, Pair<bool, int>>
        sh_list_move = new Dictionary<Keys, Pair<bool, int>>
        {
            {Keys.Up, Pair.Make(true, 1)},
            {Keys.Down, Pair.Make(false, 1)},
            {Keys.PageUp, Pair.Make(true, 10)},
            {Keys.PageDown, Pair.Make(false, 10)},
        };

        /// <summary>  Scrolls the listview up and down, depending on the 
        /// key that was pressed
        /// </summary>
        void Handle_Shortlist_Input(Keys keyData) {

            try {
                var pair = sh_list_move[keyData];

                short_list.Try_Scroll(pair.First, pair.Second, false);//, true);
            }
            catch (KeyNotFoundException) {

                true.tift<ArgumentException>(
                    "Invalid key: " + keyData.Get_String());

            }

        }

        public IDisposable Suspend_Shortlist() {
            this.suspend_shortlist = true;
            return new On_Dispose(() => suspend_shortlist = false);
        }

        public bool Shortlist_Visible {
            get {
                return this.short_list.Visible;
            }
        }

        public void Hide_Shortlist() {

            this.short_list.Close();
            this.Parent.Invalidate(this.Bounds.Expand(short_list.Width));
            this.Parent.Update();
        }

        [DefaultValue(1)]
        public int Minimum_Shortlist_Items {
            get;
            set;
        }

        [DefaultValue(100)]
        public int First_Shortlist_Column_Width {
            get;
            set;
        }

        public int SelectedIndex {
            get { return short_list.SelectedIndex; }
            set { short_list.SelectedIndex = value; }
        }

        public string SelectedItem {
            get {
                return (string)(short_list.SelectedItem);
            }
        }

        void shortList_SelectedIndexChanged(object sender, Args_RO<bool> e) {

            if (e.Im)
                return;

            OnSelectedIndexChanged();

        }

        void shortList_AcceptChanges(object sender, EventArgs e) {

            OnSelectedIndexChanged();
            Hide_Shortlist();
            Handle_Changes(false, false, false);

        }

        IControlHost control_host;
        bool suspend_shortlist;

        public IControlHost Control_Host {
            get {
                return control_host;
            }
            set {
                if (control_host != value) {

                    if (control_host != null)
                        control_host.MouseClickedOnScreen -= control_host_MouseClickedOnScreen;

                    control_host = value;
                    control_host.MouseClickedOnScreen += control_host_MouseClickedOnScreen;
                }
            }
        }

        void control_host_MouseClickedOnScreen(object sender, EventArgs e) {

            if (this.Is_Under_Mouse())
                return;

            if (short_list.Is_Under_Mouse())
                return;

            short_list.Close();
        }


    }
}