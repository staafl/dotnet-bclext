using System;
using System.Windows.Forms;

namespace Fairweather.Service
{
    public static class Boolean_Column_Static
    {
        public static Boolean_Column
        Boolean_Column(this DataGridViewColumn col) {

            return new Boolean_Column(col);

        }
    }


    public class Boolean_Column : IDisposable
    {
        readonly DataGridViewColumn col;
        DataGridView dgv { get { return col.DataGridView; } }

        public Boolean_Column(DataGridViewColumn col) {


            this.col = col;

            // ****************************

            dgv.CellFormatting += handler;
            col.Set_Value_Type(typeof(bool));

        }

        void handler(object sender, DataGridViewCellFormattingEventArgs e) {

            if (col.Index == e.ColumnIndex) {
                /* It's a bit sad that .NET does not have a more elegant way of
                 * handling this issue. */

                e.Value = ((bool)e.Value) ? "Yes" : "No";
                e.FormattingApplied = true;
            }

        }

        public void Dispose() {
            dgv.CellFormatting -= handler;
        }




    }
}
