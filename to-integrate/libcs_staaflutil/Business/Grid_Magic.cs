using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Fairweather.Service;

namespace Common.Controls
{
    public interface IGrid
    {
        DataGridViewCell Active_Cell { get; }

        Control Active_Control { get; }

        Control TopLevelControl { get; }

        bool Is_Readonly(int col, int row);

        int Empty_Row_Index { get; }

        void Open_Cell(int col, int row);

        void Close_Cell();

        DataGridViewColumnCollection Columns { get; }

        DataGridViewCell this[int col, int row] { get; }

        int ColumnCount { get; }

        bool Use_F6();

        DataGridViewCell Scroll_Cell { get; }
    }



    public class Grid_Key_Processor
    {
        public Set<int> Enter_Means_Tab {
            get;
            private set;
        }
        public Set<int> Ignore_Enter {
            get;
            private set;
        }

        readonly IGrid grid;
        readonly Grid_Navigator navigator;

        public Grid_Key_Processor(IGrid grid) {
            this.grid = grid;
            this.navigator = new Grid_Navigator(grid);

            Enter_Means_Tab = new Set<int>();
            Ignore_Enter = new Set<int>();
        }

        public bool
        Process_Key_Down(Keys keyData) {

            if (keyData == Keys.F6) {

                if (grid.Use_F6())
                    return true;

            }

            var cell = grid.Active_Cell;

            if (cell == null) {

                var scroll_cell = grid.Scroll_Cell;

                if (scroll_cell != null)
                    grid.Open_Cell(scroll_cell.ColumnIndex, scroll_cell.RowIndex);

                return true;

            }

            if (keyData.Contains(Keys.Tab)) {

                if (keyData == Keys.Tab)
                    return navigator.ProcessTabKey(true);

                if (keyData == (Keys.Shift | Keys.Tab))
                    return navigator.ProcessTabKey(false);

            }

            if (keyData == Keys.Enter) {

                if (cell == null)
                    return true;

                int row = cell.RowIndex;
                if (grid.Empty_Row_Index <= row)
                    return true;

                int col = cell.ColumnIndex;

                // 19.06 - change the order of the branches
                if (Enter_Means_Tab[col])
                    navigator.ProcessTabKey(true);
                else if (Ignore_Enter[col])
                    return true;
                else
                    grid.Open_Cell(col, row + 1);

                return true;
            }

            var keyCode = keyData & Keys.KeyCode;

            if (hs_arrow_keys[keyCode]) {

                bool has_modifier = (Native_Methods.GetAsyncKeyState(NavModifier) != 0);
                bool handled = navigator.ProcessNavigationKey(keyCode, has_modifier);

                return handled;

            }

            return false;

        }

        static readonly Set<Keys> hs_arrow_keys = new Set<Keys> { Keys.Left, Keys.Right, Keys.Down, Keys.Up };

        public int NavModifier = Native_Const.VK_CONTROL;
    }


    public class Grid_Navigator
    {
        readonly IGrid grid;
        public Grid_Navigator(IGrid grid) {
            this.grid = grid;
        }

        public bool
        ProcessNavigationKey(Keys key, bool has_modifier) {

            var tmp_active_cell = grid.Active_Cell;

            if (tmp_active_cell == null)
                return true;

            int col = tmp_active_cell.ColumnIndex;
            int row = tmp_active_cell.RowIndex;

            if (row == -1)
                return true;

            bool is_first_row = (row == 0);
            bool is_last_row = (row == grid.Empty_Row_Index);

            bool has_main_col_value = !grid[0, row].Value.IsNullOrEmpty();

            int last_col = grid.ColumnCount - 1;
            int last_row = grid.Empty_Row_Index;

            bool is_main_col = (col == 0);

            if (key == Keys.Down || key == Keys.Up) {

                if (key == Keys.Down) {
                    if (!is_last_row)
                        ++row;

                    if (row == grid.Empty_Row_Index)
                        col = 0;
                }
                else {
                    if (!is_first_row)
                        --row;
                }

                grid.Open_Cell(col, row);

                return true;

            }
            else if (key == Keys.Left || key == Keys.Right) {

                // Prevent leaving the main column

                if (is_main_col) {

                    if (has_modifier)
                        return false;

                    if (!has_main_col_value)
                        return false;

                }

                // Scan to the left until you find a
                // visible cell or hit the border
                while (true) {
                    if (key == Keys.Left) {

                        if (col <= 0)
                            break;

                        --col;

                    }
                    else {

                        if (col >= last_col)
                            break;

                        ++col;

                    }

                    if (!grid.Columns[col].Visible)
                        continue;

                    grid.Open_Cell(col, row);

                    return true;

                }

            }


            return true;

        }

        public bool ProcessTabKey(bool fwd) {

            var cell = grid.Active_Cell ?? grid[0, 0];

            int col = cell.ColumnIndex;
            int row = cell.RowIndex;

            int count = grid.ColumnCount;
            int dec_count = count - 1;

            int result_row = row;
            int result_col = col;

            Action<bool> select_other_control = _fwd =>
            {

                //Select the DGV



                // temp hack
                if (grid.GetType().Name.Contains("Products")) {

                    grid.Close_Cell();

                    Form form = ((Control)grid).FindForm();

                    form.SelectNextControl((Control)grid, _fwd, true, true, true);

                }
                else {

                    grid.TopLevelControl
                        .SelectNextControl(((Form)grid.TopLevelControl).ActiveControl,
                                           true, true, true, true);

                    // a bit of a hack, no?
                    grid.Close_Cell();

                    //Move to the next or the previous control
                    grid.TopLevelControl.SelectNextControl((Control)grid, _fwd, true, false, true);

                }
            };

            while (true) {

                if (fwd) {

                    var last_cell = (result_row == grid.Empty_Row_Index);
                    var last_on_row = (result_col == dec_count);

                    if (last_cell) {
                        select_other_control(true);
                        return true;

                    }

                    if (last_on_row) {
                        result_col = 0;
                        ++result_row;
                    }
                    else {
                        ++result_col;

                    }

                }
                else {

                    var first_cell = (result_row == 0 && result_col == 0);
                    var first_on_row = (result_col == 0);

                    if (first_cell) {
                        select_other_control(false);
                        return true;

                    }

                    if (first_on_row) {
                        result_col = dec_count;
                        --result_row;

                    }
                    else {
                        --result_col;

                    }


                }

                //if (m_column_types[result_col] == ColumnType.Readonly)
                //    continue;

                if (grid.Is_Readonly(result_col, result_row))
                    continue;

                //var result = grid[result_col, result_row];

                break;

            }

            grid.Open_Cell(result_col, result_row);
            return true;
        }
    }
}
