using System.Diagnostics;
using System.Windows.Forms;
namespace Common.Controls
{
    public partial class Our_Combo_Box : UserControl
    {
        public ComboBox.ObjectCollection Items {
            get { return comboBox.Items; }
        }

        public int SelectionStart {
            [DebuggerStepThrough]
            get { return comboBox.SelectionStart; }
            [DebuggerStepThrough]
            set { comboBox.SelectionStart = value; }
        }

        public int SelectionLength {
            [DebuggerStepThrough]
            get { return comboBox.SelectionLength; }
            [DebuggerStepThrough]
            set { comboBox.SelectionLength = value; }
        }

        public void SelectAll() {
            comboBox.SelectAll();
        }

        public string SelectedText {
            get { return comboBox.SelectedText; }
            set { comboBox.SelectedText = value; }
        }

        public CharacterCasing CharacterCasing {
            get;
            set;
        }

        public FlatStyle FlatStyle {
            get { return comboBox.FlatStyle; }
            set { comboBox.FlatStyle = value; }
        }

        public bool Allow_Blank {
            get;
            set;
        }

        public bool Auto_Tab {
            get;
            set;
        }

        public virtual int MaxLength {
            [DebuggerStepThrough]
            get { return this.comboBox.MaxLength; }
            [DebuggerStepThrough]
            set {
                if (value == this.comboBox.MaxLength)
                    return;

                this.comboBox.MaxLength = value;
            }
        }

        public override string Text { // 14th July - Changed new to override
            [DebuggerStepThrough]
            get { return comboBox.Text; }
            [DebuggerStepThrough]
            set {
                comboBox.Text = value;
            }
        }

        /// <summary> Sets the combo box's value without raising any events
        /// </summary>
        public void Set_Safe_Text(string value) {

            var before_text = bf_text;
            var before_examine = bf_examine;

            try {

                bf_text = true;
                bf_examine = true;
                Text = value;

            }
            finally {

                bf_text = before_text;
                bf_examine = before_examine;

            }

        }

    }
}