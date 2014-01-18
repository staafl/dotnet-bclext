using System.Diagnostics;

namespace Common
{
    [DebuggerStepThrough]
    public class ColumnEditedEventArgs : ColumnEventArgs
    {
        public ColumnEditedEventArgs(int col_index, int row_index)
            : base(col_index, row_index, 0.0, 0.0) { }

        public ColumnEditedEventArgs(int col_index, int row_index, object old_value, object new_value)
            : base(col_index, row_index, old_value, new_value) { }

    }
}
