using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Common;
using Common.Controls;
using DTA;
using Fairweather.Service;
using Standardization;


namespace Common.Dialogs
{
    public partial class Search_Form : Hook_Enabled_Form
    {

        public Search_Form(Record_Type record_type, Sage_Logic sdr)
            : base(true, true, Form_Kind.Modal_Dialog) {

            m_sgr = sdr;

            this.record_type = record_type;



            List<string> lst;



            titles.TryGetValue(record_type, out lst)
                  .tiff();// invalid type?


            InitializeComponent();



            base.Allow_Normal_Close = true;
            base.Use_OSK = true;

            var company_proxy = sdr.Proxy;

            m_caseless_contains = (company_proxy[DTA_Fields.POS_search_case_sensitive] == Ini_Main.NO);

            rd_predefined_tbxs = new Dictionary<Our_Text_Box, Pair<string, Label>>(3);

            Prepare_Predefined_Search(company_proxy);

            Prepare_Grid();

            but_calculate.Visible = (record_type == Record_Type.Stock);
            col_total_balance.Visible = (record_type == Record_Type.Stock);
            but_calculate.Enabled = false;

            //Prepare_Calculator();

            Set_Event_Handlers();

            this.Text = lst[0];

            this.label1.Text = "Only select records from \'{0}\'".spf(lst[1]);

            for (int ii = 2, jj = 0;
                 ii < lst.Count && jj < dgv_results.ColumnCount;
                 ++ii, ++jj) {

                dgv_results.Columns[jj].HeaderText = lst[ii];

            }

            dgv_results.RowsAdded += (_1, _2) => but_calculate.Enabled = (dgv_results.RowCount > 0);
            dgv_results.RowsRemoved += (_1, _2) => but_calculate.Enabled = (dgv_results.RowCount > 0);

            dgv_results.CellMouseDoubleClick += (_1, _2) => { Accept(); };
            dgv_results.RowsAdded += (_1, _2) => { Refresh_dgv_results_Enabled(); };
            dgv_results.RowsRemoved += (_1, _2) => { Refresh_dgv_results_Enabled(); };
            this.but_ok.Click += (_1, _2) => { Accept(); };
            this.but_apply.Click += (_1, _2) => { Apply(); };
            this.but_cancel.Click += (_1, _2) =>
            {
                this.m_result = null;

                Cancel_Clicked();
            };

        }

        void Prepare_For_Work() {
            tb_predefined_1.Select_Focus();
        }

        protected override void
        OnShown(EventArgs e) {

            tb_predefined_1.Select_Focus();
            base.OnShown(e);

        }


        void Prepare_Predefined_Search(IRead<Ini_Field, string> company_proxy) {

            var predefined_fields = Get_Predefined_Fields(company_proxy);

            rd_predefined_tbxs.Clear();
            rd_predefined_tbxs.Add(tb_predefined_1, Pair.Make(predefined_fields.First, lab_predefined_1));
            rd_predefined_tbxs.Add(tb_predefined_2, Pair.Make(predefined_fields.Second, lab_predefined_2));
            rd_predefined_tbxs.Add(tb_predefined_3, Pair.Make(predefined_fields.Third, lab_predefined_3));


            foreach (var kvp in rd_predefined_tbxs) {

                var tbx = kvp.Key;
                var pair = kvp.Value;

                var field = pair.First;
                var label = pair.Second;

                if (field.IsNullOrEmpty()) {
                    tbx.Enabled = false;
                    label.Text = "...";
                }
                else {
                    if (m_caseless_contains)
                        tbx.CharacterCasing = CharacterCasing.Upper;
                    else
                        tbx.CharacterCasing = CharacterCasing.Normal;

                    Set_Label_Text(label, field);
                    tbx.Enabled = true;
                }

            }


        }

        void Prepare_Grid() {

            dgv_1.Record_Type = record_type;
            dgv_1.BackColor = Colors.GridView.Background;

            dgv_1.ColumnLengths = new int[] { 30, 30, 30, 30, 30 };
            dgv_1.ColumnTypes = new Our_DGV_Column_Type[]{Our_DGV_Column_Type.Custom, 
                                                          Our_DGV_Column_Type.Custom,
                                                          Our_DGV_Column_Type.Custom,
                                                          Our_DGV_Column_Type.Custom,
                                                          Our_DGV_Column_Type.Custom};
            dgv_1.Rows.Add(1);
            dgv_1.Case_Insensitive_Contains = m_caseless_contains;

        }

        void Prepare_Calculator() {

            var calculator = new Calculator(this);

            calculator.Location = dgv_results.Bounds.Vertex(1).Translate(10, 0);

            this.Controls.Add(calculator);

        }


        void Set_Event_Handlers() {
            this.bur_keyboard.Click += but_keyboard_Click;
        }

        void Set_Label_Text(Label label, string field) {

            var dict = new Dictionary<string, string> { { "E_MAIL", "E-mail" } };

            string value;

            if (!dict.TryGetValue(field, out value))
                value = field.Replace("_", " ").ToProper(true);

            label.Text = value;

        }


        public Pair<string>? Result {
            get {

                if (this.DialogResult != DialogResult.OK)
                    return null;

                return m_result;
            }

        }

        void but_calculate_Click(object sender, EventArgs e) {

            var lst = new List<string>(dgv_results.RowCount);
            var rows = new Dictionary<string, int>();

            int ii = 0;

            foreach (DataGridViewRow row in dgv_results.Rows) {
                var code = row.Cells[0].Value.StringOrDefault();
                lst.Add(code);
                rows[code] = ii;
                ++ii;
            }

            var dict = m_sgr.Get_Stock_Balance_For_Stock_Records(
                lst,
                true /* include unposted */);

            foreach (var kvp in dict) {

                var stock_code = kvp.Key;
                var balance = kvp.Value;

                var row_ix = rows[stock_code];
                var cells = dgv_results.Rows[row_ix].Cells;
                cells[RESULT_TOTAL_BALANCE].Value = balance;

            }


        }






    }
}