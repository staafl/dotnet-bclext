using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Common.Dialogs;
using Fairweather.Service;
using Standardization;
using Cell = System.Windows.Forms.DataGridViewCell;
using Colors = Standardization.Colors.GridView;
using ColumnType = Common.Our_DGV_Column_Type;

namespace Common.Controls
{
    /// <summary>
    /// Control creation logic
    /// </summary>
    partial class Advanced_Grid_View
    {
        protected void
        Prepare_Output_Only_Column(int col, bool is_decimal) {

            var obj_col = this.Columns[col];

            if (is_decimal) {
                obj_col.Set_Value_Type(typeof(decimal), true);
            }

            obj_col.Set_Style(style =>
            {
                style.SelectionBackColor = Colors.OutputOnlyCell.SelectedBackground;
                style.SelectionForeColor = Colors.OutputOnlyCell.SelectedForeground;
            });

        }

        protected Our_Date_Time
        Prepare_Date_Time(int col) {

            var dt = Make_Date_Time();
            m_ctrls[col] = dt;
            return dt;

        }

        protected ComboBox
        Prepare_Combo_Box(int col) {

            var cb = Make_Combo_Box();
            m_ctrls[col] = cb;
            return cb;

        }

        protected Numeric_Box
        Prepare_Numeric_Box(int index) {

            var col = this.Columns[index];

            var nb = Make_Numeric_Box();

            var numpad = nb.Numpad;

            Action<bool> handle_numpad_event = (tab_or_enter) =>
            {

                if (!Is_Right_Workflow)
                    return;
                if (!tab_or_enter)
                    return;
                this.Force_Handle();
                BeginInvoke((MethodInvoker)(() =>
                {
                    // forward
                    Select_Next_Editable_Cell();
                }));
            };

            numpad.Enter_Pressed += (_1, _e) =>
            {
                _e.Mut = false;
                handle_numpad_event(true);
            };

            numpad.Tab_Pressed += (_, _e) =>
            {
                _e.Mut = false; // handle ourselves
                handle_numpad_event(_e.Im);
            };




            nb.Do_Not_Tab = true;

            col.Set_Value_Type(typeof(decimal));
            col.Set_Format("F2");

            nb.MaxLength = m_column_lengths[index];
            m_precisions[index] = cst_def_precision;
            m_ctrls[index] = nb;

            return nb;

        }

        protected Advanced_Combo_Box
        Prepare_Advanced_Combo_Box(int index) {

            var acb =
Make_Advanced_Combo_Box();

            acb.MaxLength = m_column_lengths[index];

            this.Columns[index].MinimumWidth = Our_Combo_Box.def_min_size.Width;

            m_ctrls[index] = acb;

            return acb;

        }

        protected TextBox
        Prepare_Text_Box(int index) {

            var tb = new Our_Text_Box();

            tb.Auto_Highlight = true;
            tb.Visible = false;
            tb.BorderStyle = BorderStyle.None;
            tb.MaxLength = 12;
            tb.Multiline = false;
            tb.AcceptsReturn = false;
            tb.Size = new Size(10, cst_row_height - 5);

            this.Controls.Add(tb);

            tb.MaxLength = m_column_lengths[index];
            m_ctrls[index] = tb;
            return tb;

        }


        // ****************************


        protected virtual ComboBox
        Make_Combo_Box() {

            var cb = new DGVComboBox(this);

            cb.FlatStyle = FlatStyle.Flat;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;

            this.Controls.Add(cb);

            cb.Visible = false;

            cb.EnterPressed += (_, args) =>
            {

                /* Logic:
                 * 
                 * 1. Close the cell
                 * 2. IF after a brief interval there is no new cell selected:
                 *      3(a). IF a cell exists BELOW, open that
                 *      3(b). ELSE open the old cell (simulate commit of data)
                 */

                var cell = Active_Cell;

                if (cell == null)
                    return;

                args.Mut = true;

                int cix = cell.ColumnIndex;
                int rix = cell.RowIndex;

                Close_Cell();

                this.Force_Handle();
                BeginInvoke((MethodInvoker)(() =>
                {
                    /* Was row deleted as a result of the action? */
                    if (cell.OwningRow.DataGridView == null)
                        --rix;

                    if (rix == -1)
                        rix = 0;

                    if (Active_Cell == null) {
                        cb.Visible = false;

                        if (rix + 1 < RowCount) {
                            Open_Cell(cix, rix + 1);

                        }
                        else if (rix + 1 == RowCount) {
                            Open_Cell(cix, rix);

                        }
                    }

                }));


            };

            // See ProcessCmdKey for some logic relating to this control

            //var quad = Quad.Make(0, 0, -1, -1);
            //var options = Border.Border_Options.Mimic_Host_Visibility;

            //var border = Border.Create(cb, quad, options);
            //border.Border_Color = Color.Gray;
            //border.Border_Edges = Quad.Make(false, false, true, true);

            return cb;
        }

        protected virtual Numeric_Box
        Make_Numeric_Box() {

            var nb = new Numeric_Box();

            //nb.NumpadClosing += (_1, e) =>
            //{
            //    e.Handled = true;
            //    bf_enter = true;
            //    ProcessTabKey(true);
            //    //SwitchCell(_active_cell.ColumnIndex, _active_cell.RowIndex);
            //};

            nb.NumpadClosed += (_1, _2) =>
            {
                bf_enter = true;
                try {
                    var tmp_active_cell = Last_Active_Cell;
                    Open_Cell(tmp_active_cell.ColumnIndex, tmp_active_cell.RowIndex);
                }
                finally {
                    bf_enter = false;
                }
            };

            nb.DecimalPlaces = cst_def_decimal;

            nb.ShowButtonBorder = false;
            nb.Visible = false;
            nb.BorderStyle = BorderStyle.None;
            nb.Enter_Not_Tab = true;

            this.Controls.Add(nb);

            return nb;

        }

        protected virtual Numeric_Box
        Make_Integer_Box() {

            var nb =
Make_Numeric_Box();

            nb.DecimalPlaces = 0;

            return nb;

        }

        protected virtual Our_Date_Time
        Make_Date_Time() {

            var dt = new Our_Date_Time();

            dt.Visible = false;

            this.Controls.Add(dt);


            dt.Calendar_Hidden += (_1, _2) =>
            {
                var cell = this.Last_Active_Cell;

                cell.Value = dt.Value.ToString(true);

                Open_Cell(cell.ColumnIndex, cell.RowIndex);

                //if (Is_Right_Workflow)
                //    Select_Next_Editable_Cell();
            };

            return dt;

        }

        protected virtual Advanced_Combo_Box
        Make_Advanced_Combo_Box() {

            var acb = new Advanced_Combo_Box();

            acb.First_Shortlist_Column_Width = 115;
            acb.Visible = false;
            acb.FlatStyle = FlatStyle.Flat;
            acb.CharacterCasing = CharacterCasing.Upper;
            acb.Allow_Blank = true;
            acb.Minimum_Shortlist_Items = 2;
            acb.Short_List_Alignment = Rectangle_Vertex.LD;

            this.Controls.Add(acb);

            // here !
            acb.Border.Try_Dispose();


            M.DGV_Workaround(acb);

            acb.Accept_Changes += (_1, _2) =>
            {
                if (this.Is_Right_Workflow) {
                    this.Force_Handle();
                    BeginInvoke((MethodInvoker)(Select_Next_Editable_Cell));
                }
            };

            return acb;
        }

    }
}
