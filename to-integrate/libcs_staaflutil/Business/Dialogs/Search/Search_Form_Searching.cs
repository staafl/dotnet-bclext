using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Common;
using Common.Controls;
using Common.Queries;
using DTA;
using Fairweather.Service;
using Fairweather.Service.Syntax;
using Standardization;

namespace Common.Dialogs
{
    public partial class Search_Form
    {
        const string STR_BARCODE = "BARCODE";

        /*       Searching        */

        void Apply() {

            Message_Box mbox = null;

            Clause clause;
            Action act;

            int total_records = 0;

            string current_record = "";

            Action<IRead<string, object>> action, every;

            bool is_stock = record_type == Record_Type.Stock;
            every = rd => current_record = rd[is_stock ? "STOCK_CODE" : "ACCOUNT_REF"].str();

            action = rd =>
            {
                int row_ind = dgv_results.Rows.Add(1);

                var row = dgv_results.Rows[row_ind];
                var cells = row.Cells;


                if (is_stock) {

                    var record = rd["STOCK_CODE"].str();

                    cells[RESULT_AC].Value = record;
                    cells[RESULT_NAME].Value = rd["DESCRIPTION"];

                    decimal ratio = new Tax_Code((short)rd["TAX_CODE"]).VAT_Ratio;

                    cells[RESULT_PHONE].Value = rd["SALES_PRICE"].ToDecimal() * (1.0m + ratio);
                    cells[RESULT_BALANCE].Value = rd["QTY_IN_STOCK"];
                }
                else {

                    var record = rd["ACCOUNT_REF"].str();

                    cells[RESULT_AC].Value = record;
                    cells[RESULT_NAME].Value = rd["NAME"];
                    cells[RESULT_PHONE].Value = rd["TELEPHONE"];
                    cells[RESULT_BALANCE].Value = rd["BALANCE"];
                }


            };

            IToken[] tokens;
            Dictionary<string, string> strings;


            if (Try_Predefined_Fields(out strings)) {

                // Premature optimization!!!

                act = () => m_sgr.Run_Contains_Query(
                    record_type,
                    strings,
                    every,
                    action,
                    m_caseless_contains,
                    out total_records);

            }
            else {
                if (!Try_Predefined_Fields(out tokens)) {

                    var barcode_data = tb_predefined_2.Text.Trim();

                    if (is_stock &&
                        !barcode_data.IsNullOrEmpty()) {

                    }

                    if (!dgv_1.Get_Tokens(out tokens)) {
                        Standard.Show(Message_Type.Warning, "Please fill all required fields in the grid.");
                        return;
                    }

                }

                if (tokens.Is_Empty()) {
                    Standard.Show(Message_Type.Warning, "Please enter information for the query.");
                    return;
                }

                clause = Query.Create_Structured_Query(tokens);
                act = () => m_sgr.Run_Record_Query(record_type, clause, every, action, out total_records);

            }




            dgv_results.Rows.Clear();
            dgv_results.SuspendLayout();



            lab_results.Text = "";

            bool ok;

            int stop = 0;
            var thr = new Thread(() =>
            {
                using (mbox = new Message_Box()) {

                    mbox.Size = new Size(202, 100);

                    mbox.Text = "Please wait...";

                    var label = mbox.label5;
                    label.Visible = true;
                    // "Searching for matching records";
                    // mbox.Label5.move(-38, 0);

                    /* stock codes can be much longer than account refs */
                    mbox.Label5.move(is_stock ? -54 : -32, 3);

                    mbox.TopMost = true;
                    mbox.StartPosition = FormStartPosition.Manual;
                    mbox.Location = this.Bounds.Center().Translate(-101, -50);


                    mbox.Show();
                    mbox.Refresh();

                    while (stop == 0) {

                        label.Text = "Current record: " + current_record;
                        mbox.Refresh();
                        Thread.Sleep(100);

                    }

                    mbox.Hide();
                }

            });


            ok = Sage_Access.Sage_Guard(act);

            if (ok) {

                thr.Start();

                Interlocked.Exchange(ref stop, 1);

                thr.Join();

            }

            dgv_results.ResumeLayout();


            // result count dialog removed in revision 780
            // Set_Result_Label();


            if (!ok) {
                dgv_results.Rows.Clear();
                lab_results.Text = "Query Failed";
                Prepare_For_Work();
                return;
            }

            int results = dgv_results.RowCount;

            lab_results.Text = "{0} of {1} Records Matched".spf(results, total_records);


            dgv_results.Select_Focus();

        }

        Triple<string> Get_Predefined_Fields(IRead<Ini_Field, string> company_proxy) {

            if (record_type == Record_Type.Stock)
                return Triple.Make("STOCK_CODE", STR_BARCODE, "DESCRIPTION");

            Triple<string> ret;

            var fields = new Dictionary<Ini_Field, string> 
            {
                {DTA_Fields.POS_search_address,ADDRESS},
                {DTA_Fields.POS_search_phone,  TELEPHONE},
                {DTA_Fields.POS_search_email,  "E_MAIL"},
                {DTA_Fields.POS_search_contact,"CONTACT_NAME"},
                {DTA_Fields.POS_search_name,   "NAME"},
                //{DTA_Fields.POS_search_country,SRFields.COUNTRY_CODE},
		    };

            var list = new List<string>(3);

            int ii = 0;

            foreach (var kvp in fields) {

                if (company_proxy[kvp.Key] == Ini_Main.YES) {
                    list.Add(kvp.Value);
                    ++ii;
                }

                if (ii == 3) {
                    break;
                }

            }

            list = list.Take(3).Pad_Left(3, null).lst();

            ret = new Triple<string>(list[0], list[1], list[2]);

            return ret;

        }


        bool Try_Predefined_Fields(out Dictionary<string, string> strings) {

            bool ret = false;

            strings = new Dictionary<string, string>();

            foreach (var kvp in rd_predefined_tbxs) {

                var text = kvp.Key.Text.Trim();

                if (text.IsNullOrEmpty())
                    continue;

                ret = true;

                var pair = kvp.Value;
                // pair.First is the sage field

                var sage_field = pair.First;

                if (sage_field == STR_BARCODE)
                    return false;

                strings[sage_field] = text;

            }

            return ret;

        }

        bool Try_Predefined_Fields(out IToken[] tokens) {

            bool ret = false;

            var tokens_lst = new List<IToken>();

            Operation op = m_caseless_contains ? Operation.Contains
                                    : Operation.Contains_Caseless;

            bool first = false;

            foreach (var pair1 in rd_predefined_tbxs.Mark_Last()) {



                var tbx = pair1.First.Key;

                var text = tbx.Text.Trim();

                if (text.IsNullOrEmpty())
                    continue;

                if (H.Set(ref first, true))
                    tokens_lst.Add(new Binary_Token(2, Operation.And));

                ret = true;

                var sage_field = pair1.First.Value.First;

                var fields = (sage_field == STR_BARCODE) ? m_sgr.Used_Barcode_Fields : new[] { sage_field };

                foreach (var pair2 in fields.Mark_Last()) {

                    var field = pair2.First;

                    Clause clause = new Clause(new Argument(Argument_Type.Entity, field),
                                                new Argument(Argument_Type.String, text),
                                                op);

                    tokens_lst.Add(new Argument_Token(clause));

                    if (!pair2.Second)
                        tokens_lst.Add(new Binary_Token(10, Operation.Or));

                }





            }

            tokens = tokens_lst.arr();

            return ret;

        }

        Clause Get_Predefined_Clause(string field, string value) {

            var op = m_caseless_contains ? Operation.Contains_Caseless : Operation.Contains;

            if (field == ADDRESS) {

                var list = from _field in new string[]{"ADDRESS_1",
		                                                        "ADDRESS_2",
		                                                        "ADDRESS_3",
		                                                        "ADDRESS_4",
		                                                        "ADDRESS_5"}
                           select new Pair<object>(_field, value);


                var clause = Query.Make_Chained_Clause(Argument_Type.Entity, Argument_Type.String,
                                                       op, false,
                                                       list.ToList());

                return clause;


            }
            else if (field == TELEPHONE) {

                //we'll only search the first field

                //*
                var arg1 = new Argument(Argument_Type.Entity, "TELEPHONE");
                var arg2 = new Argument(Argument_Type.String, value);

                var main_clause = new Clause(arg1, arg2, op);

                /*/
                Clause clause1, clause2, main_clause;
                {
                    var arg1 = new Argument(Argument_Type.Entity, "TELEPHONE");
                    var arg2 = new Argument(Argument_Type.String, value);

                    clause1 = new Clause(arg1, arg2, Operation.Contains);
                }
                {
                    var arg1 = new Argument(Argument_Type.Entity, "TELEPHONE_2");
                    var arg2 = new Argument(Argument_Type.String, value);

                    clause2 = new Clause(arg1, arg2, Operation.Contains);
                }
                {
                    var arg1 = new Argument(Argument_Type.Clause, clause1);
                    var arg2 = new Argument(Argument_Type.Clause, clause2);

                    main_clause = new Clause(arg1, arg2, Operation.Or);
                }
                //*/



                return main_clause;

            }

            else {
                var arg1 = new Argument(Argument_Type.Entity, field);
                var arg2 = new Argument(Argument_Type.String, value);

                var temp_clause = new Clause(arg1, arg2, op);
                return temp_clause;
            }

        }



        void tb_predefined_TextChanged(object sender, EventArgs e) {

            var txt = tb_predefined_1.Text +
                      tb_predefined_2.Text +
                      tb_predefined_3.Text;

            bool empty = txt.IsNullOrEmpty();

            bool enabled = dgv_1.Enabled;

            if (empty != enabled) {
                dgv_1.BackgroundColor = empty ? Color.White : Color.LightGray;
                foreach (DataGridViewRow row in dgv_1.Rows)
                    row.Visible = empty;
            }

            dgv_1.Enabled = empty;



        }
    }
}
