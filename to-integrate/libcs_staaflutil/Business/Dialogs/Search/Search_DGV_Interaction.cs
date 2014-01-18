using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Fairweather.Service;
using Standardization;

namespace Common.Controls
{
    partial class Search_DGV
    {



        protected override bool Suppress_Tab_Key {
            get { return true; }

        }



        Control Get_Value_Control(int row) {

            var argtype = Get_Argument_Type(row);
            switch (argtype) {

                case Common.Queries.Argument_Type.String:
                    return tb_string;

                case Common.Queries.Argument_Type.Decimal:
                    return nb_decimal;

                case Common.Queries.Argument_Type.Bool:
                    return cb_yesno;

                case Common.Queries.Argument_Type.Integer:
                    return nb_integer;

                case Common.Queries.Argument_Type.Date:
                    return dt_date;

                //break;
                case Common.Queries.Argument_Type.Place_Holder:
                case Common.Queries.Argument_Type.Entity:
                case Common.Queries.Argument_Type.Clause:
                default:
                    true.tift();
                    throw new ApplicationException();
            }

        }

        protected override void 
        Get_Active_Control_Size_Location(
                out int width, 
                out int height, 
                out Point pt) {

            base.Get_Active_Control_Size_Location(out width, out height, out pt);

            if (m_active_control is Numeric_Box)
                --width;

        }

        int Get_Condition(int row) {

            var argtype = Get_Argument_Type(row);
            var index = rd_arg_types[argtype];

            return index;

        }

        protected override void
        Get_Active_Control(int col, int row, out bool read_only, out Control ctrl) {

            ctrl = null;
            read_only = false;

            if (col == COL_JOIN) {

                ctrl = row == 0 ? cb_where : cb_andor;
                return;

            }

            if (col == COL_CONDITION) {

                int condition = Get_Condition(row);
                ctrl = m_cond_cbxs[condition];
                return;

            }

            if (col == COL_FIELD) {

                ctrl = cb_field;

                return;

            }

            if (col == COL_VALUE) {

                ctrl = Get_Value_Control(row);
                Common.M.DGV_Workaround(ctrl);
                var as_tbx = ctrl as TextBox;
                if (as_tbx != null) {

                    as_tbx.CharacterCasing = this.Case_Insensitive_Contains ? CharacterCasing.Upper : CharacterCasing.Normal;
                }

                return;
            }

            if (col == COL_PRECEDENCE) {

                ctrl = cb_precedence;
                if (row == 0) {
                    cb_precedence.Items.Clear();
                    cb_precedence.Items.Add("Not applicable");
                    return;
                }

                int cnt = this.RowCount;
                var available = new Set<int>(Magic.range(1, 10));

                for (int ii = 1; ii < cnt; ++ii) {
                    if (ii == row)
                        continue;
                    try {
                        var value = Convert.ToInt32(this[COL_PRECEDENCE, ii].Value);
                        available.Remove(value);
                    }
                    catch (InvalidCastException) { }
                }

                cb_precedence.Items.Clear();
                cb_precedence.Items.AddRange(available.Select(_int => _int.ToString()).ToArray());

                return;

            }

            throw new ApplicationException();

        }

        protected override void
        OnCellValueChanged(DataGridViewCellEventArgs e) {

            int icol = e.ColumnIndex;
            int irow = e.RowIndex;

            if (irow == -1)
                return;

            if (icol == COL_JOIN) {

                var val = this[icol, irow].Value;
                int cnt = this.RowCount;

                if (val.IsNullOrEmpty())
                    return;

                if (val.ToString() == NONE) {

                    if (this[COL_JOIN, irow].Value.IsNullOrEmpty())
                        return;

                    this.Rows.RemoveAt(irow);

                    if (irow == cnt - 1 || cnt == 1)
                        this.Rows.Add(1);

                    return;
                }
                else {
                    if (irow == 0)
                        this[COL_PRECEDENCE, 0].Value = "N/A";

                    if (irow == cnt - 1 && cnt < MAX_ROWS) {
                        this.Rows.Insert(cnt, 1);
                    }
                }
            }

            if (icol == COL_FIELD) {

                this[COL_CONDITION, irow].Value = null;
                this[COL_VALUE, irow].Value = null;


            }


            base.OnCellValueChanged(e);

        }

        protected override bool
        Verify_Cell_OK_For_Selecting(ref int col, ref int row) {

            if (!base.Verify_Cell_OK_For_Selecting(ref col, ref row))
                return false;

            var cells = this.Rows[row].Cells;

            if (row == 0 && col == COL_PRECEDENCE) {
                col = COL_FIELD;
                return true;
            }

            if (row > 0) {

                var row_above = this.Rows[row - 1];
                var cells_above = row_above.Cells;

                for (int ii = 0; ii < this.ColumnCount; ++ii) {

                    if (cells_above[ii].Value.IsNullOrEmpty()) {
                        if (row == 0 && ii == COL_PRECEDENCE)
                            continue;

                        --row;
                        col = ii;
                        return true;

                    }
                }
            }

            if (cells[COL_JOIN].Value.IsNullOrEmpty()) {
                col = COL_JOIN;
                return true;
            }

            if (cells[COL_FIELD].Value.IsNullOrEmpty() &&
                (col == COL_VALUE || col == COL_CONDITION)) {
                col = COL_FIELD;
                return true;
            }

            return true;

        }



    }
}
