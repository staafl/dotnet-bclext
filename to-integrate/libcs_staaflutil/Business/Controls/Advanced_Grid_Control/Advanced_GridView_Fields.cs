using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

using Common;
using Common.Controls;

using Fairweather.Service;


using Cell = System.Windows.Forms.DataGridViewCell;


namespace Common.Controls
{
    partial class Advanced_Grid_View : DataGridView
    {
        public event EventHandler<EventArgs> Active_Cell_Changed;

        protected Control m_active_control;
        protected const int cst_def_precision = Data.CST_DEF_PRECISION;
        protected const int cst_def_decimal = 2;
        protected const int cst_row_height = 21;

        protected int[] m_column_lengths;
        protected Our_DGV_Column_Type[] m_column_types;

        protected readonly Dictionary<int, Control> m_ctrls;



        protected Dictionary<int, int> m_precisions;


#pragma warning disable

        protected bool bf_scroll;
        protected bool bf_enter;
        protected bool bf_leave;

        protected bool bf_cell_enter;
        protected bool bf_cell_state_changed;
        protected bool bf_active_control_leave;
        protected bool bf_column_edited;
        protected bool bf_column_editing;
        protected bool bf_main_column_value_entered;

        protected bool bi_cell_enter;
        protected bool bi_cell_state_changed;

        protected bool b_commit_value;
        protected bool bf_switch_cell;
#pragma warning restore



        public int[] ColumnLengths {
            [DebuggerStepThrough]
            get { return m_column_lengths; }
            set {
                if (value != m_column_lengths)
                    Set_Column_Lengths(value);
            }
        }
        public Our_DGV_Column_Type[] ColumnTypes {
            [DebuggerStepThrough]
            get { return m_column_types; }
            set {
                if (value != m_column_types) {

                    Set_Column_Types(value);
                }
            }
        }

        protected virtual void On_Active_Cell_Changed(EventArgs e) {
            this.Active_Cell_Changed.Raise(this, e);

        }

        // Not to be confused with CurrentCell
        public Cell Active_Cell {
            get;
            protected set;
        }

        public Control Active_Control {
            [DebuggerStepThrough]
            get { return m_active_control; }
            [DebuggerStepThrough]
            set {
                if (value != m_active_control) {
                    if (m_active_control != null) {
                        m_active_control.Leave -= OnActiveControlLeave;
                    }

                    m_active_control = value;

                    if (m_active_control != null) {
                        m_active_control.Leave += OnActiveControlLeave;
                    }
                }
            }
        }


    }

}
