using System;

//using Timer = System.Timers.Timer;
using System.Timers;

using Fairweather.Service;

namespace Common
{
    public class Incremental_Search_Provider : IDisposable
    {
        const double default_interval = 4000;
        const double default_step = 2500;

        readonly Timer timer = new Timer(default_interval);
        double interval = default_interval;
        double step = default_step;

        public Incremental_Search_Provider() {

            timer.Elapsed += timer_Elapsed;

        }

        void timer_Elapsed(object sender, ElapsedEventArgs e) {

            ResetSearch();

        }

        public void Dispose() {
            timer.Close();
        }

        ~Incremental_Search_Provider() {
            Dispose(false);
        }

        void Dispose(bool disposing) {

            if (disposing) {
                GC.SuppressFinalize(this);
            }

            timer.Close();


        }

        public void AcceptString(string value) {

            CurrentSearchString += value;

            SignalActivity();

        }

        void SignalActivity() {

            timer.Stop();
            timer.Interval = IsSearchRunning ? step : interval;
            timer.Start();
            IsSearchRunning = true;

        }

        public double SearchInterval {
            get {
                return interval;
            }
            set {
                interval = value;
            }
        }

        public double SearchIntervalStep {
            get {
                return step;
            }
            set {
                step = value;
            }
        }

        public void ResetSearch() {

            timer.Stop();
            IsSearchRunning = false;
            CurrentSearchString = "";

        }

        public bool IsSearchRunning {
            get;
            protected set;
        }

        public event Handler_RO<Pair<string>> SearchStringChanged;
        public string CurrentSearchString {
            get { return css; }
            set {
                if (css == value)
                    return;

                SearchStringChanged.Raise(this, Args.Make_RO((Pair<string>)Pair.Make(value, css)));
                css = value;
            }
        }

        string css;
        /*       Legacy code        */


        //int? last_item = null;
        //string incr_search = "";

        //protected override void OnKeyPress(KeyPressEventArgs e) {

        //      Begin_Incr_Search(e);

        //      base.OnKeyPress(e);
        //}

        //void Begin_Incr_Search(KeyPressEventArgs e) {

        //      char new_ch = Char.ToUpper(e.KeyChar);

        //      // This is in case I want it to cycle
        //      // on every letter
        //      // bool cycle = !incr_search.IsNullOrEmpty() &&
        //      //               incr_search.Last().ToUpper() == new_ch;


        //      // This is in case it should only cycle
        //      // on the first letter          
        //      bool cycle = incr_search.Length == 1 &&
        //                   incr_search[0].ToUpper() == new_ch;


        //      incr_search += new_ch;

        //      // We start our search from the highlighted item
        //      // if any...
        //      if (SelectedIndices.Count != 0)
        //            last_item = SelectedIndices[0];

        //      // ...otherwise start from before the 
        //      // first item
        //      else
        //            last_item = -1;

        //      // if we've passed through the entire listview,
        //      // start over
        //      if (Items.Count <= last_item + 1)
        //            last_item = -1;

        //      //// else if the next item
        //      //else{
        //      //    string tmp = Items[last_item + 1].SubItems[col]
        //      //                                     .Text
        //      //                                     .ToUpper();
        //      //    if (!tmp.StartsWith(str))
        //      //        last_item = -1;
        //      //}

        //      timer.Stop();

        //      if (!last_item.HasValue)
        //            last_item = -1;


        //      int? item = null;

        //      // IF a matching item is already highlighted
        //      if (SelectedItems.Count > 0) {

        //            string tmp = SelectedItems[0].SubItems[SortedColumn]
        //                                         .Text
        //                                         .ToUpper();

        //            if (tmp.StartsWith(incr_search))
        //                  item = SelectedItems[0].Index;
        //      }

        //      if (!item.HasValue)
        //            item = PerformIncrementalSearch(incr_search, last_item.Value + 1, SortedColumn);

        //      // Try to cycle the rest of the items
        //      if (!item.HasValue && cycle) {

        //            string temp = incr_search.Remove_From_Right(1);

        //            item = PerformIncrementalSearch(temp, last_item.Value + 2, SortedColumn);

        //            if (item.HasValue)
        //                  incr_search = temp;
        //      }

        //      // Try the character alone
        //      if (!item.HasValue) {

        //            string temp = new_ch.ToString();

        //            item = PerformIncrementalSearch(temp, last_item.Value + 1, SortedColumn);

        //            if (item.HasValue)
        //                  incr_search = temp;
        //      }

        //      if (item.HasValue) {

        //            last_item = item.Value;

        //            timer.Start();
        //            Items[item.Value].Selected = true;
        //            Items[item.Value].Focused = true;
        //            TopItem = Items[item.Value];

        //      }
        //      else
        //            Reset_Search();

        //      e.Handled = true;
        //}

        ///// <summary>  Side-effect free
        ///// </summary>
        //int? PerformIncrementalSearch(string str, int start_item, int col) {

        //      int index = -1;

        //      foreach (ListViewItem lvi in Items) {

        //            index = lvi.Index;

        //            // Skip the first part
        //            if (index < start_item)
        //                  continue;

        //            string tmp = lvi.SubItems[col]
        //                            .Text
        //                            .ToUpper();

        //            // Success
        //            if (tmp.StartsWith(str))
        //                  return index;
        //      }

        //      foreach (ListViewItem lvi in Items) {

        //            index = lvi.Index;

        //            if (index >= start_item)
        //                  break;

        //            string tmp = lvi.SubItems[col]
        //            .Text
        //            .ToUpper();

        //            // Success
        //            if (tmp.StartsWith(str))
        //                  return index;
        //      }

        //      // Failure
        //      return null;
        //}

        //void Reset_Search() {
        //      incr_search = "";
        //      last_item = -1;
        //      timer.Stop();

        //}
        //void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {

        //      Reset_Search();

        //}
        //protected override void OnSelectedIndexChanged(EventArgs e) {

        //      if (SelectedIndices.Count > 0) {

        //            string tmp = SelectedItems[0].SubItems[SortedColumn]
        //                         .Text
        //                         .ToUpper();

        //            if (!tmp.StartsWith(incr_search)) {
        //                  Reset_Search();
        //            }
        //      }

        //      base.OnSelectedIndexChanged(e);
        //}

    }
}