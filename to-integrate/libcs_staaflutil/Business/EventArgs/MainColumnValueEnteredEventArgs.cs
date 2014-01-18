
namespace Common
{
    public class MainColumnValueEnteredEventArgs : ColumnEventArgs
    {
        public MainColumnValueEnteredEventArgs(int column_index, int row_index, object old_value, object new_value) : base(column_index, row_index, old_value, new_value) { }
        public bool Cancel { get; set; }
    }
}
