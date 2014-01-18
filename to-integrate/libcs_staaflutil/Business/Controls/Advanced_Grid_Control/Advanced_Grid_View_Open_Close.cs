using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Fairweather.Service;
using Standardization;
using Cell = System.Windows.Forms.DataGridViewCell;
using ColumnType = Common.Our_DGV_Column_Type;

namespace Common.Controls
{
    /// <summary>
    /// The logic in this file handles the actual opening and closing of
    /// the grid cells - it is not concerned with the way a cells is
    /// chosen for opening.
    /// </summary>
    partial class Advanced_Grid_View
    {
        public virtual void Open_Cell(DataGridViewCell cell) {
            if (cell != null)
                Open_Cell(cell.ColumnIndex, cell.RowIndex);
        }

        /// Sets the active cell to the one with the given coordinates
        /// Uses FinishField and ShowControl, disables OnActiveControlLeave
        /// Raises Active_Cell_Changed
        public virtual void
        Open_Cell(int col, int row) {

            if (row == -1) {// 29th August, 15 December
                Close_Cell();
                return;
            }

            if (bf_switch_cell)
                return;

            bf_switch_cell = true;
            bf_active_control_leave = true;

            try {

                if (!Verify_Cell_OK_For_Selecting(ref col, ref row))
                    return;

                var selected = this[col, row];

                last_Active_Cell = selected;

                Close_Cell();

                //17th June
                // 12 Oct - velko
                // 06 Nov
                // 08 Dec
                //CurrentCell = this[col, row];

                var ac = Active_Cell;

                if (ac != null)
                    ac.Selected = false;

                bool read_only;
                Control ctrl;
                Get_Active_Control(col, row, out read_only, out ctrl);

                (read_only || (ctrl != null)).tiff();

                Active_Cell = selected;

                if (read_only) {

                    // Make_Readonly_Cell(selected);

                    selected.Selected = row == this.Empty_Row_Index ? false : true;

                }
                else {

                    selected.Selected = false;
                    Active_Control = ctrl;
                    Show_Control();

                }

                On_Active_Cell_Changed(EventArgs.Empty);

                base.OnCellEnter(new DataGridViewCellEventArgs(col, row));
            }
            finally {
                bf_switch_cell = false;
                bf_active_control_leave = false;
            }
        }

        public virtual void
        Show_Control() {

            var cell = Active_Cell;
            var ctrl = m_active_control;

            if (cell == null || ctrl == null)
                return;

            int col = cell.ColumnIndex;
            int row = cell.RowIndex;

            Prepare_Control_For_Display(col, row);

            //if (!Is_Visible(row))
            //    this.FirstDisplayedScrollingRowIndex = row;

            bf_enter = true;
            try {
                ctrl.Visible = true;

                ctrl.BringToFront();

                ctrl.Select_Focus();

                ctrl.Refresh();

            }
            finally {
                bf_enter = false;
            }

        }


        protected virtual void
        Prepare_Control_For_Display(int col, int row) {

            var p1 = this.Get_Cell_Coords(col, row, true);

            Set_Control_Value();

            Set_Active_Control_Size_Location();

            if (m_active_control is TextBox)
                ((TextBox)m_active_control).SelectAll();

            if (m_active_control is Numeric_Box)
                ((Numeric_Box)m_active_control).Has_User_Typed_Text = false;

            // m_active_control.BringToFront();

        }

        public void
        Set_Active_Control_Size_Location() {

            var ctrl = m_active_control;

            if (ctrl == null)
                return;

            int width, height;
            Point pt;

            Get_Active_Control_Size_Location(out width, out height, out pt);

            ctrl.Size = new Size(width, height);
            ctrl.Location = pt;

        }

        /// <summary> 
        /// Does not throw if _active_control == null
        /// Throws on _active_cell == null
        /// Override for custom positioning.
        /// </summary>
        protected virtual void
        Get_Active_Control_Size_Location(out int width, out int height, out Point pt) {

            H.assign(out width, out height, out pt);

            var ctrl = m_active_control;
            if (ctrl == null)
                return;

            var cell = Active_Cell;

            if (cell == null)
                return;

            int col = Active_Cell.ColumnIndex;
            int row = Active_Cell.RowIndex;

            width = Active_Cell.Size.Width;
            height = cst_row_height;

            pt = this.Get_Cell_Coords(col, row, false, true).Value;

            // former crap...

            //switch (m_column_types[col]) {

            //    case ColumnType.NumericBox: {

            //            m_active_control.Size = new Size(Active_Cell.Size.Width - 1,
            //                                             m_active_control.Size.Height);
            //            break;
            //        }

            //    case ColumnType.ComboBox: {
            //            m_active_control.Size = new Size(Active_Cell.Size.Width,
            //                                             m_active_control.Size.Height);
            //            break;
            //        }

            //    case ColumnType.Normal: {

            //            m_active_control.Size = new Size(Active_Cell.Size.Width - 7,
            //                                            m_active_control.Size.Height);
            //            break;
            //        }

            //    default:
            //        return;
            //}


            if (ctrl is TextBox) {

                // cell_w - 7, ctrl_h

                width -= 6;
                height -= 5;
                (ctrl as TextBox).SelectAll();
                // pt = pt.Translate(2, 4);
                pt = pt.Translate(2, 3);

            }
            else if (ctrl is Advanced_Combo_Box) {

                height -= 1;

            }
            else if (ctrl is Numeric_Box) {

                width -= 1;
                height -= 1;

                // pt = pt.Translate(-1, 0);

            }
            else if (ctrl is Our_Date_Time) {

                height += 1;

                pt = pt.Translate(-1, 0);

            }
            else {
                width -= 1;
                height = ctrl.Height;


            }

        }


        /*       Helpers        */

        public virtual bool
        Is_Readonly(int col, int row) {

            var type = m_column_types[col];
            return type == ColumnType.Readonly_Decimal;

        }

        protected virtual void
        Get_Active_Control(int col,
                           int row,
                           out bool read_only,
                           out Control ctrl) {

            read_only = false;
            ctrl = null;

            if (Is_Readonly(col, row)) {

                read_only = true; // 2nd June
                return;

            }

            if (m_ctrls.TryGetValue(col, out ctrl))
                return;


        }

        /// <summary>
        /// Allows you to prevent a cell from being selected
        /// or to choose another cell
        /// </summary>
        protected virtual bool
        Verify_Cell_OK_For_Selecting(ref int col, ref int row) {
            return true;
        }

        // ****************************

        public void
        Commit_Value(bool close, bool force_update) {

            if (b_commit_value)
                return;

            b_commit_value = true;
            var tmp_active_cell = this.Active_Cell;
            var tmp_active_control = this.Active_Control;

            try {
                if (tmp_active_cell == null)
                    return;

                var row = tmp_active_cell.RowIndex;
                var col = tmp_active_cell.ColumnIndex;

                if (row < 0)
                    return;

                if (Is_Readonly(col, row)) {
                    if (close) {
                        this.Focus();
                        this.CurrentCell = null;
                    }
                    return;
                }

                if (tmp_active_control == null)
                    return;

                if (row < 0) {

                    if (tmp_active_cell.DataGridView != null)
                        return;

                }

                if (close) {
                    bf_active_control_leave = true;
                    bf_main_column_value_entered = true;
                    bf_enter = true;
                    bf_leave = true;
                    try {

                        // This is to make sure the form does not
                        // automatically switch to the next control
                        // when the dummy box is hidden

                        if (this.ContainsFocus)
                            this.Focus();

                        // END

                        tmp_active_control.Visible = false;
                        this.Invalidate(tmp_active_control.Bounds);
                        this.Force_Handle();
                        BeginInvoke((MethodInvoker)(() => { this.Update(); }));

                    }
                    finally {
                        bf_leave = false;
                        bf_enter = false;
                        bf_active_control_leave = false;
                        bf_main_column_value_entered = false;

                    }
                }

                col = tmp_active_cell.ColumnIndex;
                row = tmp_active_cell.RowIndex;

                decimal dbl_value;
                string str_value;

                bool is_dbl;
                bool change;

                Get_Control_Value(tmp_active_cell, tmp_active_control, col, close,

                                  out dbl_value, out str_value, out is_dbl, out change);

                if (close) {

                    bi_cell_state_changed = true;
                    try {
                        tmp_active_cell.Selected = false;
                    }
                    finally {
                        bi_cell_state_changed = false;
                    }

                }

                change |= force_update;

                bool allowed = Test_Value(col, row, str_value);

                change &= allowed;
                change &= row >= 0;

                var prev_value = tmp_active_cell.Value ?? ""; // <--


                if (change) {

                    bool handled = Handle_Set_Value_Virt(col, row, is_dbl, str_value, dbl_value, prev_value);

                    if (!handled) {

                        bool temp = bf_switch_cell;

                        bf_switch_cell = true;
                        try {
                            var value = is_dbl ? dbl_value : (object)str_value;
                            this[col, row].Value = value;
                        }

                        finally {
                            bf_switch_cell = temp;
                        }
                    }

                }


                if (close) {
                    // ****************************

                    // 15/12/2009 - This change was prompted by
                    // a glitch in the scrolling behavior, where
                    // if the user scrolled while a readonly cell
                    // was selected, the cell would lose focus
                    // and the last active normal cell would be selected
                    // It is nevertheless proper.
                    Active_Cell = null;

                    tmp_active_control.Hide();
                    Active_Control = null;


                    // ****************************


                    tmp_active_control.ResetText();

                }

            }
            finally {
                b_commit_value = false;
            }

        }

        public virtual void
        Close_Cell() {
            Commit_Value(true, false);
          
        }


        /*       Helpers        */

        protected virtual void
        Set_Control_Value() {


            var cell = Active_Cell;


            Control ctrl = m_active_control;

            if (ctrl == null || cell == null)
                return;

            if (ctrl is Numeric_Box) {

                var nb = ctrl as Numeric_Box;

                nb.Text = cell.FormattedValue.ToString();

            }
            else if (ctrl is Our_Combo_Box) {

                var cb = (ctrl as Our_Combo_Box);

                cb.Text = (string)(cell.Value ?? "");
                cb.Value = (string)(cell.Value ?? "");

            }
            else if (ctrl is TextBox) {

                var tb = (ctrl as TextBox);

                ctrl.Text = (string)(cell.Value ?? "");


            }
            else if (ctrl is ComboBox) {

                var cb = ctrl as ComboBox;

                int ix = cb.Items.IndexOf(cell.Value.StringOrDefault());

                cb.SelectedIndex = ix;

                if (cb.SelectedIndex == -1)
                    cb.SelectedIndex = 0;

            }
            else if (ctrl is Our_Date_Time) {

                var dt = ctrl as Our_Date_Time;
                var text = cell.Value.StringOrDefault();

                if (text.IsNullOrEmpty())
                    dt.Value = DateTime.Today;
                else
                    dt.Value = DateTime.Parse(text);

            }
            else {
                ctrl.Text = cell.Value.StringOrDefault();
            }
        }


        protected virtual void
        Get_Control_Value(DataGridViewCell tmp_active_cell,
                          Control tmp_active_control,
                          int col,
                          bool close,
                          out decimal dbl_value,
                          out string str_value,
                          out bool is_dbl,
                          out bool change) {

            H.assign(out dbl_value, out str_value, out is_dbl, out change);

            change = true;

            var type = m_column_types[col];

            if (type == ColumnType.AdvancedComboBox) {

                is_dbl = false;

                var cb = (Our_Combo_Box)tmp_active_control;
                str_value = cb.Value;

                change = (!str_value.IsNullOrEmpty());
                if (change)
                    change &= (tmp_active_cell == null || !str_value.Equals(tmp_active_cell.Value));

                if (close) {
                    // Closing the shortlist might trigger an event
                    bf_enter = true;
                    try {
                        cb.Clear();
                        cb.Refresh();
                    }
                    finally {
                        bf_enter = false;
                    }
                }

            }
            else if (type == ColumnType.NumericBox) {

                var nb = (Numeric_Box)tmp_active_control;
                dbl_value = A.DecimalOrZero(nb.Text, m_precisions[col]);
                is_dbl = true;

                change &= nb.Has_User_Typed_Text;

                if (close) {
                    nb.Has_User_Typed_Text = false;
                    nb.Clear();

                }

            }
            else if (tmp_active_control is Our_Date_Time) {

                is_dbl = false;
                str_value = (tmp_active_control as Our_Date_Time).Value.ToString(true);

                if (close)
                    tmp_active_control.Text = "";
            }
            else {

                is_dbl = false;
                str_value = tmp_active_control.Text;

                if (close)
                    tmp_active_control.Text = "";

            }

        }


        protected virtual bool
        Handle_Set_Value_Virt(int col,
                              int row,
                              bool is_dec,
                              string str_value,
                              decimal dec_value,
                              object prev_value) {

            return false;

        }

        protected virtual bool
        Test_Value(int col, int row, string str_value) {
            return true;
        }

        // ****************************

    }
}
