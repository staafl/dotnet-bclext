
using System;
using System.Windows.Forms;


namespace Common.Controls
{
#if NO_CALCULATOR
        public class Our_MaskedTextBox : MaskedTextBox 
#else
    public partial class Our_MaskedTextBox : MaskedTextBox, ICalculatorTextBox
#endif
    {


        public Our_MaskedTextBox() {
            (this).AutoSize = false;
        }

        public bool Auto_Highlight { get; set; }



        protected override void OnEnter(EventArgs e) {

            if (Auto_Highlight)
                this.SelectAll();

            base.OnEnter(e);
        }
    }

#if !NO_CALCULATOR
    public partial class Our_MaskedTextBox : MaskedTextBox, ICalculatorTextBox
    {

        void ICalculatorTextBox.AcceptChar(char ch) {

            Our_StaticTextBoxBase.Accept_Char(this, ch);

        }
        Control ICalculatorTextBox.Control {
            get {
                return this;
            }
        }
        void ICalculatorTextBox.NotifyChanged() { }
        bool ICalculatorTextBox.IsCalculable { get { return false; } }
    }
#endif
}