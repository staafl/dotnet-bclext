namespace Common
{
    public interface ITextBox
    {
        string Text { get; set; }
        int SelectionStart { get; set; }
        int SelectionLength { get; set; }

        void SelectAll();
        int MaxLength { get; set; }
    }
}
