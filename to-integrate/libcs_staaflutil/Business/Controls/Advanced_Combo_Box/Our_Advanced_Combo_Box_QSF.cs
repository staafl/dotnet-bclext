using System;
using System.Drawing;
using Fairweather.Service;
using System.Windows.Forms;

using Common.Dialogs;
using Standardization;
namespace Common.Controls
{
    public partial class Advanced_Combo_Box
    {
        public bool Auto_Show_QSF {
            get;
            set;
        }

        //protected override void Handle_Blank() {

        //    if (this.QSF_Visible)
        //        return;

        //    if (Auto_Show_QSF) {

        //        this.Select_Focus();
        //        Show_QSF();
        //        return;

        //    }


        //    base.Handle_Blank();

        //}

        bool b_setup;

        public event Handler<Quick_Search_Form> QSF_Shown;
        public event Handler<Quick_Search_Form> QSF_Hidden;


        Point Get_QSF_Location() {

            int xx = (this.QSF_Orientation.Horizontal == Direction_LR.Right) ?
                     -qsf.Width + this.Width + 2 :
                     -1;

            int yy = (this.QSF_Orientation.Vertical == Direction_UD.Down) ?
                     this.Height + 1 :
                     -qsf.Height;

            var pt1 = this.Location.Translate(xx, yy);
            var ret = this.Parent.PointToScreen(pt1);

            return ret;
        }

        public void Show_QSF() {

            b_setup.tiff();

            qsf.Location = Get_QSF_Location();

            this.Hide_Shortlist();

            QSF_Shown.Raise(this, Args.Make(qsf));

            var initial_text = this.Text;
            int start = this.SelectionStart;

            if (!this.SelectedText.IsNullOrEmpty() && start != 0)
                initial_text = initial_text.Substring(0, start);

            qsf.Show(initial_text);
            qsf.Activate();

        }

        public void Hide_QSF() {

            qsf.Hide();

        }

        public bool QSF_Visible {
            get {
                return qsf != null && qsf.Visible;
            }
            set {
                if (qsf == null)
                    return;

                if (QSF_Visible == value)
                    return;

                if (value)
                    Show_QSF();
                else
                    Hide_QSF();
            }
        }


        public void Setup(Quick_Search_Form_Mode mode) {

            short_list.Close();

            if (qsf != null) {
                qsf.Close();
                qsf.Try_Dispose();
            }


            qsf = new Quick_Search_Form(this, mode, "");

            qsf.Deactivate += (_1, _2) => QSF_Hidden.Raise(this, Args.Make(qsf));
            qsf.Owner = this.FindForm();

            var null_type = Quick_Search_Form.Get_Record_Type(mode);

            this.Record_Type = null_type.Value;

            this.MaxLength = this.Record_Type.MaxLength();

            this.m_mode = mode;

            b_setup = true;

        }

        void Notify_Search_Form_Result(string result, string name) {

            var as_SortedListCursor = (RecordsCursor) as Sorted_List_Cursor;

            if (as_SortedListCursor == null)
                return;
            if (as_SortedListCursor.ContainsKey(result))
                return;

            //string name = Global.SageDataRecords
            //                          .Get_Record_Data(result, Record_Type.Customer_Account, "NAME")["NAME"].ToString();
            as_SortedListCursor.Add_New(result, name);

        }

        protected override bool Account_Valid(string txt, bool allow_gui) {
            return base.Account_Valid(txt, allow_gui) && Check_Not_Restricted(txt, allow_gui);
        }

        bool Check_Not_Restricted(string item) {
            bool allow_gui = true;
            return Check_Not_Restricted(item, allow_gui);
        }
        bool Check_Not_Restricted(string item, bool allow_gui) {

            var pair = Pair.Make(this.Record_Type, item);
            if (!dict_restricted_accs[pair])
                return true;

            if (allow_gui) {
                ((IQuick_Search_Form_Host)this).Refresh();
                Standard.Warn("Your current security settings do not allow you to access account '{0}'.", item);
            }

            return false;

        }


        void IQuick_Search_Form_Host.Collect_QSF_Result(Pair<string> pair, Quick_Search_Form_Mode mode1) {

            var result = pair.First;

            if (!Check_Not_Restricted(result))
                return;

            if (!result.IsNullOrEmpty()) {

                Notify_Search_Form_Result(result, pair.Second);

                Set_Safe_Text(result);
                Refresh();
            }


            if (!b_OnAcceptChanges) {

                Handle_Changes(false, true, false);

            }
            //if (!result.IsNullOrEmpty()) {
            //      this.Value = result;
            //}

        }

        void IQuick_Search_Form_Host.Refresh() {

            var form = this.FindForm();

            form.Invalidate(qsf.Bounds);

            //var bounds = qsf.Bounds;
            //Set<Control> refreshed = new Set<Control>();

            //for (int ii = 0; ii < 4; ++ii) {

            //    var vert = bounds.Vertex(ii);
            //    var child = form.GetChildAtPoint(vert);

            //    if (child != null && !refreshed[child]) {
            //        refreshed[child] = true;
            //        child.Refresh();
            //    }
            //}

            form.Update();
            //form.Refresh();

        }

        bool b_OnAcceptChanges;
        protected override void OnAcceptChanges(EventArgs e) {

            if (b_OnAcceptChanges)
                return;

            b_OnAcceptChanges = true;
            try {
                //(this as IQuickSearchFormHost).CollectQuickSearchResult(this.Text, this.m_mode);
            }
            finally {
                b_OnAcceptChanges = false;
            }

            /*       Note that OnAcceptChanges comes after CollectQuickSearchResult       */
            /*       In the future I might remove the m_receiver sink and pump all         */
            /*       messages through OnAcceptChanges        */


            base.OnAcceptChanges(e);


        }


        Quick_Search_Form qsf;

        public Quick_Search_Form Qsf {
            get {
                return qsf;
            }
        }

        Quick_Search_Form_Mode m_mode;

        Moving_Border m_border;

        public Moving_Border Border {
            get { return m_border; }
        }



        public bool Is_Customer {
            get {
                return this.Record_Type == Record_Type.Sales;

            }
        }

        public bool Is_Supplier {
            get {
                return this.Record_Type == Record_Type.Purchase;
            }
        }

        public void Handle_QSF_Event(Quick_Search_Form_Event event_type) {

            if (event_type == Quick_Search_Form_Event.New_Button_Clicked) {

                var item = qsf.Highlighted_Item;

                if (item.IsNullOrEmpty())
                    item = this.Text;

                if (item.IsNullOrEmpty())
                    item = this.Value;

                if (Is_Customer || Is_Supplier) {

                    Show_Edit_Dialog(item);

                }
                else {
                    New_Button_Clicked.Raise(this, Args.Make_RO(item));

                }

            }
            else if (event_type == Quick_Search_Form_Event.Search_Button_Clicked) {

                Show_Search_Dialog();

            }
            else {
                true.tift(event_type.Get_String());
            }

        }

        public bool Show_Edit_Dialog(string item) {

            var frm = this.FindForm();

            if (!Check_Not_Restricted(item)) {
                frm.Activate();
                return false;
            }

            QSF_Visible = false;
            frm.Refresh();

            var sdr = Data.SDR;
            bool ok;

            using (var disp = sdr.Attempt_Transaction(true, out ok)) {

                if (!ok)
                    return false;

                using (frm.Dim_Form())
                using (var dialog = new New_Customer(item, sdr, this.Is_Customer)) {

                    disp.Try_Dispose();


                    var result = dialog.ShowDialog(this.FindForm());

                    if (result == DialogResult.OK) {
                        this.Text = dialog.Last_Accessed_Record;
                        Handle_Changes(false, true, true);
                        return true;
                    }

                    return false;

                }

            }
        }

        /// <summary>
        /// To which vertex of the control the QSF is aligned.
        /// </summary>
        [System.ComponentModel.DesignerSerializationVisibility(
         System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public Rectangle_Vertex QSF_Orientation {
            get;
            set;
        }
    }
}
