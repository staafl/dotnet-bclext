using System;
using System.Windows.Forms;
using Common.Controls;

namespace Common
{
    public class CalculatorTextBoxWrapper : ICalculatorTextBox
    {
        Control Inner { get; set; }

        Func<int?, int> sel_start;
        Func<int?, int> sel_len;
        Action select_all;
        Func<int?, int> max_len;

        public CalculatorTextBoxWrapper(TextBoxBase tb)
            : this(tb,
            ni =>
            {
                if (ni.HasValue)
                    tb.SelectionStart = ni.Value;
                return tb.SelectionStart;
            },
            ni =>
            {
                if (ni.HasValue)
                    tb.SelectionLength = ni.Value;
                return tb.SelectionLength;
            },
            () => tb.SelectAll(),
            ni =>
            {
                if (ni.HasValue)
                    tb.MaxLength = ni.Value;
                return tb.MaxLength;
            }) { }


        public CalculatorTextBoxWrapper(Control inner,
                                        Func<int?, int> sel_start,
                                        Func<int?, int> sel_len,
                                        Action select_all,
                                        Func<int?, int> max_len) {
            this.sel_start = sel_start;
            this.sel_len = sel_len;
            this.max_len = max_len;

            this.select_all = select_all;

            this.Inner = inner;
        }

        public bool IsCalculable { get { return false; } }
        public void NotifyChanged() {
        }
        public void AcceptChar(char ch) {
            Our_StaticTextBoxBase.Accept_Char(this, ch);

        }


        public string Text { get { return Inner.Text; } set { Inner.Text = value; } }

        public int SelectionStart { get { return sel_start(null); } set { sel_start(value); } }

        public int SelectionLength { get { return sel_len(null); } set { sel_len(value); } }

        public void SelectAll() {
            select_all();
        }

        public int MaxLength { get { return max_len(null); } set { max_len(value); } }

        public Control Control { get { return Inner; } }

    }
}
