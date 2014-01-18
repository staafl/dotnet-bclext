using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;

using Common;


namespace Common.Controls
{
    public partial class Our_Combo_Box : UserControl, ICalculatorTextBox
    {
        void ParentForm_FormClosing(object sender, FormClosingEventArgs e) {

            if (short_list.Visible) {
                short_list.Visible = false;
                Undo();
                e.Cancel = true;
            }
        }

        //<-- ITextBox
        Control ICalculatorTextBox.Control { get { return this.comboBox; } }
        void ICalculatorTextBox.NotifyChanged() { }
        bool ICalculatorTextBox.IsCalculable {
            get { return false; }
        }

        void ICalculatorTextBox.AcceptChar(char ch) {
            Aux_Accept_New_Char(ch);
        }
        // --!>
    }
}
