using System.Collections.Generic;

using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Dialogs
{
    /*       Departments mode eviscerated in revision 719        */

    public partial class Quick_Search_Form : Form
    {
        /*       Ctor        */

        public Quick_Search_Form(IQuick_Search_Form_Host form,
                               Quick_Search_Form_Mode mode,
                               string sort) {

            InitializeComponent();
            but_new.Click += (_1, _2) => m_host.Handle_QSF_Event(Quick_Search_Form_Event.New_Button_Clicked);
            but_search.Click += (_1, _2) => m_host.Handle_QSF_Event(Quick_Search_Form_Event.Search_Button_Clicked);

            this.Force_Handle();

            //list_view.SearchStringChanged += (_1, pair) =>
            //    {
            //        this.Invoke((MethodInvoker)(() => { this.Text = pair.Data.First; }));
            //    };

            m_host = form;
            m_mode = mode;

            var texts = header_texts[mode];

            this.columnHeader1.Text = texts.First;
            this.columnHeader2.Text = texts.Second;

            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Name = Data.QSF_Title;

            //int sorting;

            //if (int.TryParse(sort, out sorting)) {

            //    var sort_order = (sorting > 0 ?
            //        SortOrder.Ascending :
            //        SortOrder.Descending);

            //    int column = Math.Abs(sorting) - 1;
            //    list_view.Sort(column, sort_order);

            //}

            list_view.RecordsCursor = Get_Records_Cursor();

            list_view.HeaderStyle = ColumnHeaderStyle.Clickable;

            list_view.Select();

            but_new.Enabled = (New_Edit_Allowed(mode));

            but_search.Enabled = (Search_Allowed(mode));


        }


        Pair<string>? result;

        bool b_ok;
        bool shown;

        readonly IQuick_Search_Form_Host m_host;
        readonly Quick_Search_Form_Mode m_mode;

        public Pair<string>? Result {
            get {
                if (!b_ok || this.DialogResult != DialogResult.OK)
                    return null;

                return result;
            }
        }

        public int SortingState() {

            var ret = (list_view.SortedColumn + 1);

            if (list_view.Sorting == SortOrder.Descending)
                ret *= -1;

            return ret;

        }



        public new void Show() {

            this.Show(null, null);

        }



        public void Show(Form owner) {

            this.Show(null, owner);

        }

        public void Show(string initial) {
            Form owner = null;
            Show(initial, owner);
        }

        public void Show(string initial, Form owner) {

            this.result = null;
            ListViewItem lvi = null;

            list_view.Refresh_Virtual_Size();
            list_view.ResetCache();
            list_view.SuspendCacheReset = true;

            try {

                if (!initial.IsNullOrEmpty()) {

                    lvi = list_view.TryFind(initial);

                }

                if (lvi == null) {
                    if (list_view.Items.Count > 0)
                        lvi = list_view.Items[0];
                }

                if (lvi != null)
                    list_view.Set_Selected_Item(lvi, true, true);

                list_view.Reset_Search();

                base.Show(owner);
                Activate();

                list_view.Select_Focus();
                /*       Old code removed in rev 908      */

            }
            finally {
                list_view.SuspendCacheReset = false;

            }

        }

        public static Record_Type? Get_Record_Type(Quick_Search_Form_Mode mode) {

            var dict = new Dictionary<Quick_Search_Form_Mode, Record_Type>
            {
                {Quick_Search_Form_Mode.Customers, Record_Type.Sales},
                {Quick_Search_Form_Mode.Customers_new, Record_Type.Sales},

                {Quick_Search_Form_Mode.Suppliers, Record_Type.Purchase},
                {Quick_Search_Form_Mode.Suppliers_new, Record_Type.Purchase},

                {Quick_Search_Form_Mode.Banks, Record_Type.Bank},

                {Quick_Search_Form_Mode.Products, Record_Type.Stock},
                {Quick_Search_Form_Mode.Products_view, Record_Type.Stock},

                {Quick_Search_Form_Mode.Expenses, Record_Type.Expense},
                {Quick_Search_Form_Mode.Departments, Record_Type.Department},

                {Quick_Search_Form_Mode.TTs, Record_Type.TT},
                {Quick_Search_Form_Mode.Tax_Codes, Record_Type.Tax_Code},

                {Quick_Search_Form_Mode.Projects, Record_Type.Project},
                {Quick_Search_Form_Mode.Cost_Codes, Record_Type.Cost_Code},



            };

            Record_Type type;

            bool ok = dict.TryGetValue(mode, out type);

            if (!ok)
                ok = dict.TryGetValue(Aux_Denew(mode), out type);

            Record_Type? ret = ok ? type : (Record_Type?)null;

            return ret;

        }

        public Records_Cursor Get_Records_Cursor() {

            var type = Get_Record_Type(m_mode);

            type.HasValue.tiff(new XEnum("m_mode", m_mode));

            var ret = Records_Access.Get_Cursor(type.Value);

            return ret.Get_Copy();

        }

        public string Highlighted_Item {
            get {
                return this.list_view.FocusedItem.OrDefault_(item => item.SubItems[0].Text, "");
            }
        }


    }
}
