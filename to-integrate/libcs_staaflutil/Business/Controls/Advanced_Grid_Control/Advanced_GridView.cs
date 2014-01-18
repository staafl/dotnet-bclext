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
    public partial class Advanced_Grid_View : DataGridView
    {
        public Advanced_Grid_View() {

            m_ctrls = new Dictionary<int, Control>();
            m_precisions = new Dictionary<int, int>();

            this.RowTemplate.Height = cst_row_height;

            Application.AddMessageFilter(new WM_MOUSEWHEEL_Filter(this));

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {

            if (keyData == Keys.PageUp ||
                keyData == Keys.PageDown) {

                int displayed_rowcount = DisplayedRowCount(false);

                var row = Last_Active_Row;
                var col = Last_Active_Col;
                
                bool up = keyData == Keys.PageUp;
                int new_row = up ? Math.Max(row - displayed_rowcount, 0)
                    : Math.Min(row + displayed_rowcount, RowCount - 1);

                Open_Cell(col, new_row);

                return true;

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        protected override void WndProc(ref Message m) {
            if (m.Msg == Native_Const.WM_MOUSEWHEEL) {
                var tran = m.Translate_Mouse_Wheel();
                var times_rolled = Math.Abs(tran) / 40;
                var key = tran < 0 ? "{UP}" : "{DOWN}";
                //for (int ii = 0; ii < times_rolled; ++ii) {
                SendKeys.SendWait(key.Repeat(times_rolled));
                //}

                // mousewheel is disabled!
                return;
            }
            base.WndProc(ref m);
        }


        public class WM_MOUSEWHEEL_Filter : IMessageFilter
        {
            readonly Advanced_Grid_View grid;

            public WM_MOUSEWHEEL_Filter(Advanced_Grid_View grid) {
                this.grid = grid;
            }

            public bool PreFilterMessage(ref Message m) {

                if (m.Msg == Native_Const.WM_MOUSEWHEEL) {

                    if (grid.ContainsFocus &&
                        grid.Active_Control != null) {

                        grid.WndProc(ref m);
                        return true;

                    }

                }

                return false;
            }
        }

        // ****************************



        protected override void OnLeave(EventArgs e) {

            if (Clean_On_Leave)
                this.CurrentCell = null;

            base.OnLeave(e);

        }

        protected override void OnScroll(ScrollEventArgs e) {

            if (Kill_Scroll)
                e.NewValue = e.OldValue;

            base.OnScroll(e);
        }

        protected virtual bool Clean_On_Leave {
            get {
                return false;
            }
        }

        protected virtual bool Kill_Scroll {
            get {
                return false;
            }
        }

        /// <summary>
        /// Should probably be overridden when column sorting
        /// is required.
        /// </summary>
        protected virtual bool Hijack_Column_Header_Click {
            get {
                return true;
            }
        }

        /// <summary>
        /// Represents the direction in which focus should travel
        /// as the user enters values in cells.
        /// </summary>
        protected virtual Grid_Workflow_Direction Workflow_Direction {
            get {
                return Grid_Workflow_Direction.Top_Bottom;
            }
        }

        bool Is_Right_Workflow {
            get {
                return Workflow_Direction == Grid_Workflow_Direction.Left_Right;
            }
        }

        // ****************************



        // what needs to / can be overridden ?
        // Verify_Cell_OK_For_Selecting
        // OnCellValueChanged
        // Get_Active_Control
        // Setup_Column
        // 

        protected virtual void Set_Column_Types(ColumnType[] column_types) {

            this.Rows.Clear();

            (m_column_lengths == null).tift();

            (column_types.Length == this.Columns.Count)
                .tiff("You need to call SetColumnLength before using this function.");


            m_precisions.Clear();

            m_column_types = column_types;

            foreach (var ctrl in this.m_ctrls.Values)
                ctrl.Try_Dispose();


            for (int ii = 0; ii < this.Columns.Count; ++ii)
                Setup_Column(ii);

            foreach (var col in this.Columns.Cast<DataGridViewColumn>())
                col.MinimumWidth = Math.Max(col.MinimumWidth, 25);

        }

        // This function is meant to be the counterpart of Get_Active_Control.
        // If advanced control management is required, override both to create/
        // use controls in any way you require.
        protected virtual bool
        Setup_Column(int col) {


            var type = m_column_types[col];

            if (type == ColumnType.Readonly_Decimal) {
                Prepare_Output_Only_Column(col, true);
                return true;
            }

            if (type == ColumnType.NumericBox) {
                Prepare_Numeric_Box(col);
                return true;

            }

            if (type == ColumnType.TextBox) {
                Prepare_Text_Box(col);
                return true;

            }

            if (type == ColumnType.AdvancedComboBox) {
                Prepare_Advanced_Combo_Box(col);
                return true;

            }

            if (type == ColumnType.DateTime) {
                Prepare_Date_Time(col);
                return true;
            }



            if (type == ColumnType.ComboBox) {
                Prepare_Combo_Box(col);
                return true;

            }


            return false;
        }

        /// <summary> Only the entries for NumBxs are used </summary>
        protected void
        Set_Column_Lengths(int[] column_lengths) {

            if (column_lengths.Count() != this.Columns.Count)
                throw new ArgumentException();

            m_column_lengths = column_lengths;

        }

        public void
        Set_Column_Precisions(IDictionary<int, int> precision) {

            foreach (var pair in precision) {

                int key = pair.Key;
                int value = pair.Value;

                m_precisions[key] = value;

                this.Columns[key].CellTemplate.Style.Format = "F" + value.ToString();

                Control ctrl;
                if (m_ctrls.TryGetValue(key, out ctrl))
                    ((Numeric_Box)ctrl).DecimalPlaces = value;
            }

        }





        class DGVComboBox : ComboBox
        {
            readonly Advanced_Grid_View dgv;
            public DGVComboBox(Advanced_Grid_View dgv) {
                this.dgv = dgv;
                this.SetStyle(
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.AllPaintingInWmPaint,
                    true);

            }

            public event Handler<bool> EnterPressed;

            protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {

                if (keyData == Keys.Enter) {

                    var args = Args.Make(false);

                    EnterPressed.Raise(this, args);

                    if (args.Mut)
                        return true;

                }

                return base.ProcessCmdKey(ref msg, keyData);
            }
        }




        /*       Helper funcs        */

        public bool Is_Visible(int row_index) {
            int first = this.FirstDisplayedScrollingRowIndex;
            int last = first + this.Maximum_Displayed_Rows(Rows[0].Height) - 1;

            bool inside = row_index >= first &&
                          row_index < last;
            return inside;
        }

        public void Select_Last_Row() {

            this.Open_Cell(0, this.Empty_Row_Index);

        }

        /// <summary>
        /// Selects the next editable cell on the row or, recursively,
        /// on the next row if none are available on this.
        /// </summary>
        public void Select_Next_Editable_Cell() {
            ProcessTabKey(true);
        }

        protected virtual bool Normal_F6 { get { return false; } }
        public virtual bool Use_F6() {

            if (Normal_F6) {
                var cell = Active_Cell;

                if (cell == null || cell.RowIndex <= 0)
                    return false;

                int col = cell.ColumnIndex;
                int row = cell.RowIndex;

                object value = this[col, row - 1].Value;

                Close_Cell();

                cell.Value = value;

                Open_Cell(col, row);

                Reopen_Active_Cell();

                return true;
            }

            return false;
        }

        public void Reopen_Active_Cell() {

            var cell = this.Active_Cell;
            if (cell == null)
                return;

            var row = cell.RowIndex;
            var col = cell.ColumnIndex;
            Open_Cell(col, row);

        }


        public virtual DataGridViewCell Scroll_Cell {
            get {
                return null;
            }
        }

        protected Cell New_Entry_Cell {
            [DebuggerStepThrough]
            get {

                var ret = this[0, Empty_Row_Index];

                return ret;

            }
        }


        /*       Misc        */


        /// <summary>
        /// The higher this is, the lower the Qsf can go without
        /// having to change its orientation.
        /// </summary>
        protected virtual int Qsf_Lower_Margin {
            get {
                return 150;
            }
        }

        protected void Align_Quick_Search_Form(Quick_Search_Form form) {

            var ctrl = m_active_control;

            if (ctrl == null)
                return;

            var align_with = ctrl.Bounds_On_Screen();

            var container1 = this.Bounds_On_Screen()
                                 .Expand(10, 100, 0, Qsf_Lower_Margin);

            var rect1 = form.ClientRectangle;
            var pair = Pair.Make(0, 3);
            var rect2 = rect1.Align_Vertices_In_Container(align_with, container1, pair, pair.Flip());

            var pt = rect2.Value.Location;

            form.Location = pt.Translate(-1, 0);

        }


        protected override void OnRowPrePaint(DataGridViewRowPrePaintEventArgs e) {
            //http://devnotebook.spaces.live.com/blog/cns!3CEE4F280F8CEA92!106.entry
            //
            e.PaintParts &= ~DataGridViewPaintParts.Focus;
            //e.PaintParts &= ~DataGridViewPaintParts.SelectionBackground;
            base.OnRowPrePaint(e);

        }


        // ****************************

        void Maybe_Hijack_Column_Header_Click(int ix) {
            if (!Hijack_Column_Header_Click)
                return;
            Do_Hijack_Column_Header_Click(ix);
        }

        void Do_Hijack_Column_Header_Click(int col) {

            /* Desired behavior:
             * 
             * Open the cell on the last used row that is just beneath the column,
             * if possible. */

            var row = Last_Active_Row;

            if (row == Empty_Row_Index)
                col = 0;

            this.Open_Cell(col, row);
        }

        protected override void
        OnColumnDividerDoubleClick(DataGridViewColumnDividerDoubleClickEventArgs e) {

            Maybe_Hijack_Column_Header_Click(e.ColumnIndex);

            base.OnColumnDividerDoubleClick(e);
        }

        protected override void
        OnColumnDividerWidthChanged(DataGridViewColumnEventArgs e) {

            Maybe_Hijack_Column_Header_Click(e.Column.Index);

            base.OnColumnDividerWidthChanged(e);
        }


        protected override void
        OnColumnHeaderMouseDoubleClick(DataGridViewCellMouseEventArgs e) {

            Maybe_Hijack_Column_Header_Click(e.ColumnIndex);

            base.OnColumnHeaderMouseDoubleClick(e);
        }

        protected override void
        OnColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e) {


            Maybe_Hijack_Column_Header_Click(e.ColumnIndex);


            base.OnColumnHeaderMouseClick(e);


        }

        // ****************************

    }
}