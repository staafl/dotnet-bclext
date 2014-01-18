#define SORTEDLIST

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Fairweather.Service;


using ScrollCst = Fairweather.Service.Native_Const.WM_VSCROLL_low;

namespace Common.Controls
{
    partial class QSF_ListView : SafeVirtualListView
    {
        public QSF_ListView() {

            this.Sorting = SortOrder.Ascending;

            this.View = View.Details;

            this.SortedColumn = 0;

            this.VirtualMode = true;

            this.VirtualListSize = 1;


        }


        readonly Incremental_Search_Provider search = new Incremental_Search_Provider();

        string[] temp_buffer = { "", "" };

        const int grace = 18;
        volatile bool bf_is_scroll;

#if SORTEDLIST
        SortedList<int, ListViewItem> cache1 = new SortedList<int, ListViewItem>(100);
        SortedList<int, ListViewItem> cache2 = new SortedList<int, ListViewItem>(100);
#else
            Dictionary<int, ListViewItem> cache = new Dictionary<int, ListViewItem>(2000);
#endif

        public bool SuspendCacheReset { get; set; }

        public void ResetCache() {

            if (SuspendCacheReset)
                return;

            cache1.Clear();
            cache2.Clear();

        }


        public void Refresh_Virtual_Size() {


            this.VirtualListSize = RecordsCursor.Count;

            var col = this.Columns[1];

            col.Width = -2;

        }

        SortedList<int, ListViewItem> Cache {
            get {
                return RecordsCursor.First_Column ? cache1 : cache2;
            }
        }

        int Get_Cache_Index(int index) {

            int ret = index;
            if (!RecordsCursor.Forward)
                ret = this.VirtualListSize - index - 1;
            return ret;
        }

        readonly ListViewItem empty = new ListViewItem(new[] { "", "" });

        ListViewItem Get_Item(int index) {

            if (bf_is_scroll)
                return empty;

            string key, value;

            ListViewItem ret;

            var cache = Cache;

            int cache_index = Get_Cache_Index(index);

            if (cache.TryGetValue(cache_index, out ret))
                return ret;

            var tmp = RecordsCursor.GetAtIndex(index);

            key = tmp.First;
            value = tmp.Second;

            //http://www.developmentnow.com/g/30_2006_8_0_0_803770/listview-virtual-mode--how-to-ensure-visible.htm
            // virtual_item.SubItems[0].Text = key;
            // virtual_item.SubItems[1].Text = value;

            // ret = virtual_item;

            temp_buffer[0] = key;
            temp_buffer[1] = value;

            ret = new ListViewItem(temp_buffer);

            cache[cache_index] = ret;

            return ret;


        }


        void Force_Refresh() {

            int cnt = this.VirtualListSize;

            if (cnt > 0)
                this.RedrawItems(0, cnt - 1, false);

        }


        protected override void OnRetrieveVirtualItem(RetrieveVirtualItemEventArgs e) {

            int index = e.ItemIndex;
            var lvi = Get_Item(index);
            e.Item = lvi;

            base.OnRetrieveVirtualItem(e);
        }

        bool up_arrow, down_arrow;

        protected override void OnKeyDown(KeyEventArgs e) {

            if (e.KeyCode == Keys.PageUp ||
                e.KeyCode == Keys.PageDown) {

                this.Force_Handle();
                BeginInvoke((MethodInvoker)(() =>
                {
                    bf_is_scroll = true;

                }));

                base.OnKeyDown(e);
                base.OnKeyDown(e);

            }

            if (e.KeyCode == Keys.Up) {
                up_arrow = true;
            }
            else if (e.KeyCode == Keys.Down) {
                down_arrow = true;
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e) {

            if (e.KeyCode == Keys.PageUp ||
                e.KeyCode == Keys.PageDown) {

                ResumeCaching();

            }

            if (e.KeyCode == Keys.Up) {
                up_arrow = false;
            }
            else if (e.KeyCode == Keys.Down) {
                down_arrow = false;
            }

            base.OnKeyUp(e);
        }



        void ResumeCaching() {
            bf_is_scroll = false;
            this.Force_Refresh();
        }

        public event Handler_RO<Pair<string>> SearchStringChanged {
            add { search.SearchStringChanged += value; }
            remove { search.SearchStringChanged += value; }
        }
        protected override void WndProc(ref Message m) {

            if (m.Msg == Native_Const.WM_VSCROLL) {

                var low = B.LoWord(m.WParam);

                if (low == ScrollCst.SB_ENDSCROLL ||
                      low == ScrollCst.SB_THUMBPOSITION) {

                    down_arrow = up_arrow = false;
                    ResumeCaching();


                }
                else if (low == ScrollCst.SB_THUMBTRACK ||
                          low == ScrollCst.SB_PAGEDOWN ||
                          low == ScrollCst.SB_PAGEUP) {

                    bf_is_scroll = true;

                }

                if (low == ScrollCst.SB_PAGEDOWN ||
                    low == ScrollCst.SB_PAGEUP) {

                    //12 times the fun
                    for (int ii = 0; ii < 12; ++ii) {
                        base.WndProc(ref m);
                    }

                }
                else if (low == ScrollCst.SB_LINEDOWN) {
                    down_arrow = true;
                }
                else if (low == ScrollCst.SB_LINEUP) {
                    up_arrow = true;
                }
            }
            base.WndProc(ref m);

        }


        void Prepare_Cache(int start_index, int end_index) {

            if (bf_is_scroll)
                return;

            var cache = Cache;

            start_index = Math.Max(0, start_index);
            end_index = Math.Min(end_index, VirtualListSize - 1);


            int length = end_index - start_index + 1;

            var indices = new List<int>(length);

            lock (cache) {

                for (int ii = start_index; ii < end_index; ++ii) {

                    if (!cache.ContainsKey(Get_Cache_Index(ii))) {
                        indices.Add(ii);
                    }


                }

                int missing_count = indices.Count;
                if (missing_count <= 1)
                    return;

                if (missing_count < grace) {
                    int index = -1;
                    var lvi = this.FocusedItem;

                    if (lvi != null) {
                        index = lvi.Index;
                    }
                    else if (this.SelectedIndices.Count != 0) {
                        index = this.SelectedIndices[0];
                    }

                    if (index != -1) {

                        if (down_arrow) {
                            var last_missing = indices.Last();

                            if (last_missing - index >= grace)
                                return;
                        }

                        if (up_arrow) {
                            var first_missing = indices.First();

                            if (index - first_missing >= grace)
                                return;
                        }

                    }
                }

                // GetIndexedRange?
                var list = RecordsCursor.GetAtIndices(indices, RecordsCursor.Forward);

                string[] pair = { "", "" };

                for (int jj = 0; jj < list.Count; ++jj) {

                    int ii = indices[jj];
                    pair[0] = list[jj].First;
                    pair[1] = list[jj].Second;

                    var lvi = new ListViewItem(pair);
                    cache[Get_Cache_Index(ii)] = lvi;
                }



            }



        }

        protected override void OnCacheVirtualItems(CacheVirtualItemsEventArgs e) {

            //int temp;

            //if (temp % 2 == 0)
            //      return;
            //++temp;

            if (!Visible)
                return;

            //if (!this.SelectedIndices.Count == 0) {
            //      try {
            //            var ind = this.SelectedIndices;
            //            var item = this.Items[ind];
            //            if(item.
            //      }
            //      catch (IndexOutOfRangeException) {
            //      }
            //}

            int start_index = e.StartIndex - grace;
            int end_index = e.EndIndex + grace;


            Prepare_Cache(start_index, end_index);


            base.OnCacheVirtualItems(e);


        }

        protected override void OnVisibleChanged(EventArgs e) {

            this.ResetCache();
            if (this.Visible)
                this.Refresh_Virtual_Size();

            base.OnVisibleChanged(e);

        }





        public Records_Cursor RecordsCursor { get; set; }


        public int? HighlightedIndex {
            get {
                if (SelectedIndices.Count != 1)
                    return null;

                // SelectedIndices contains the real indices
                var index = SelectedIndices[0];

                return index;
            }
        }

        public string HighlightedEntry {
            get {
                var pair = HighlightedPair;

                var ret = pair.HasValue ? pair.Value.First : null;
                return ret;
            }
        }

        public Pair<string>? HighlightedPair {
            get {

                var real_index = HighlightedIndex;

                if (real_index == null)
                    return null;

                var lvi = this.Items[real_index.Value];

                var ret = new Pair<string>(lvi.Text, lvi.SubItems[1].Text);
                return ret;
            }
        }



    }

}
