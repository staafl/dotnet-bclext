using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Common.Controls;
using Fairweather.Service;
using Standardization;

using Cell = System.Windows.Forms.DataGridViewCell;
using ColumnType = Common.Our_DGV_Column_Type;


namespace Common.Controls
{
    partial class Advanced_Grid_View : DataGridView, IGrid
    {




        /*       September 26th        */

        protected override void
        OnCellMouseDown(DataGridViewCellMouseEventArgs e) {

            var row = e.RowIndex;
            var col = e.ColumnIndex;

            if (row < 0 || col < 0) {
                base.OnCellMouseDown(e);
                return;
            }

            var cell = this[col, row];
            var eargs = new DataGridViewCellStateChangedEventArgs(cell, DataGridViewElementStates.Selected);
            OnCellStateChanged(eargs);

        }

        protected override void
        OnCellStateChanged(DataGridViewCellStateChangedEventArgs e) {

            if (bf_cell_state_changed ||
                bi_cell_state_changed)
                return;



            if (e.StateChanged == DataGridViewElementStates.Selected) {

                bi_cell_state_changed = true;

                try {
                    var cell = e.Cell;

                    int col = cell.ColumnIndex;
                    int row = cell.RowIndex;

                    var not_focused = !this.ContainsFocus;
                    var output_cell = !Is_Readonly(col, row);
                    var empty_row = row == this.Empty_Row_Index;

                    var unselect = (not_focused || output_cell || empty_row);

                    if (unselect) {
                        try {
                            cell.Selected = false;
                        }
                        catch (NullReferenceException) {

                            // this exception is not related to "cell" itself
                            // but to an unexpected error thrown by the internal
                            // DataGridView code

                        }
                    }

                    if (not_focused)
                        return;

                    Open_Cell(col, row);
                }
                finally {
                    bi_cell_state_changed = false;
                }
            }

            base.OnCellStateChanged(e);
        }


        protected override void
        OnCellEnter(DataGridViewCellEventArgs e) {
            /* silence this event */
        }




        /// <summary> We need to resize the active control, in case it is visible
        /// Btw, at the moment we do not do anything about the floating box -
        /// since the cell under it has the same color, the effect is not visible
        /// </summary>
        protected override void
        OnColumnWidthChanged(DataGridViewColumnEventArgs e) {

            var ctrl = m_active_control;

            if (ctrl != null && Active_Cell != null) {
                Set_Active_Control_Size_Location();
                ctrl.Refresh();
            }

            base.OnColumnWidthChanged(e);
        }



        public DataGridViewCell
        Last_Active_Cell {
            get {
                var tmp_cell = last_Active_Cell;
                if (tmp_cell == null)
                    return null;
                if (tmp_cell.OwningRow == null ||
                    tmp_cell.OwningRow.DataGridView != this)
                    return null;
                return tmp_cell;
            }
        }

        public int
        Last_Active_Col {
            get {
                var cell = Last_Active_Cell;
                if (cell == null)
                    return 0;

                return cell.ColumnIndex;
            }
        }

        public int
        Last_Active_Row {
            get {
                var cell = Last_Active_Cell;
                if (cell == null)
                    return 0;

                return cell.RowIndex;
            }
        }

        public virtual int
        Empty_Row_Index {
            get {
                return this.RowCount - 1;
            }
        }



        protected void
        OnActiveControlLeave(object sender, EventArgs e) {
            if (bf_active_control_leave)
                return;

            Close_Cell();

        }


        protected virtual
        Grid_Key_Processor Grid_Key_Processor {
            get {
                return new Grid_Key_Processor(this);
            }
        }

        protected virtual bool
        ProcessTabKey(bool fwd) {

            if (Suppress_Tab_Key)
                return true;

            if (new Grid_Navigator(this).ProcessTabKey(fwd))
                return true;

            return true;
        }

        protected virtual bool
        Suppress_Tab_Key {
            get { return false; }

        }

    }
}
