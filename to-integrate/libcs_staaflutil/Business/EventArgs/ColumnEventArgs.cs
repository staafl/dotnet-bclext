using System.Diagnostics;

namespace Common
{
    /// <summary>  These include ColumnEventArgs, its two children, as well as EditingCancelledEventArgs and
    /// AggregateChang_ED_EventArgs
    /// </summary>
    [DebuggerDisplay(debugger_display)]
    [DebuggerStepThrough]
    public abstract class ColumnEventArgs : System.EventArgs
    {
        const string debugger_display = "Column {debug.column_names[columnIndex],nq}; " +
            // ({columnIndex}); " +
                                        "Row " + "{rowIndex}; " +
                                        "Old " + "{oldValue}; " +
                                        "New " + "{newValue}; ";
        readonly int columnIndex;

        public int ColumnIndex {
            get { return columnIndex; }
        }

        readonly int rowIndex;
        public int RowIndex {
            get { return rowIndex; }
        }

        public ColumnEventArgs(int col_index, int row_index, object old_value, object new_value) {

            columnIndex = col_index;
            rowIndex = row_index;

            newValue = new_value;
            oldValue = old_value;

        }

        readonly object oldValue;
        public object OldValue {
            get { return oldValue; }
        }

        object newValue;
        public object NewValue {
            get { return newValue; }
            set { newValue = value; }
        }

    }
}
