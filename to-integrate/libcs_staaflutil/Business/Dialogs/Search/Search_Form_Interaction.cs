using System;
using System.Windows.Forms;
using Common.Controls;
using Fairweather.Service;


namespace Common.Dialogs
{
    public partial class Search_Form : Hook_Enabled_Form
    {
        void Refresh_dgv_results_Enabled() {

            dgv_results.Enabled = dgv_results.RowCount > 0;

        }

        void Accept() {

            if (this.dgv_results.SelectedRows.Count > 0) {

                var row = dgv_results.SelectedRows[0];

                Func<int, string> get = col => row.Cells[col].Value.ToString();
                m_result = Pair.Make(get(0), get(1));


                this.DialogResult = DialogResult.OK;

            }
            else {
                this.DialogResult = DialogResult.Cancel;
                m_result = null;
            }


            Try_Close();

        }




    }
}