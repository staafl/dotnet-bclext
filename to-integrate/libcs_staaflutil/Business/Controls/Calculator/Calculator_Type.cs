using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    public partial class Calculator
    {

        public event Action Operation;

        void Type(string input) {

            bool calculable = m_vb_target.IsCalculable;

            var typed = m_vb_target.Text;
            var pos = m_vb_target.SelectionStart;
            var len = m_vb_target.SelectionLength;
            var max = m_vb_target.MaxLength;

            if (max <= typed.Length &&
                max != 0 &&
                len == 0)
                return;

            if (calculable) {
                Handle_Input(input);
                return;
            }
            else {

                foreach (char ch in input)
                    //SendKeys.Send(ch);
                    m_vb_target.AcceptChar(ch);

            }

        }

        void Handle_Input(string input) {

            string text = m_vb_target.Text;
            Func<string> f_text = () => m_vb_target.Text;


            if (b_clear) {

                m_vb_target.Text = input;
                m_vb_target.SelectionStart = input.Length;
                b_clear = false;
                engine.FreshInput = true;

                return;
            }

            if (input.Contains('.') &&
                text.Contains('.'))

                return;



            foreach (char ch in input)
                m_vb_target.AcceptChar(ch);



            if (rx_only_zero.Match(m_vb_target.Text).Success) {

                m_vb_target.Text = input;
                m_vb_target.SelectionStart = input.Length;

                return;
            }

            var collapsed = m_vb_target.Text.Collapse_Zeroes(false);

            if (collapsed != f_text()) {

                int pos = m_vb_target.SelectionStart;

                m_vb_target.Text = collapsed;

                m_vb_target.SelectionStart = pos;
            }

            if (f_text().StartsWith(".")) {
                int pos = m_vb_target.SelectionStart;

                m_vb_target.Text = "0" + f_text();

                m_vb_target.SelectionStart = pos + 1;

            }
        }

        void Handle_Button(Button button) {

            bool ignored = false;
            is_eq_pressed = false;

            try {

                string value = Aux_Get_Button_Value(button.Name);

                string typeable;

                if (Aux_Is_Typeable_String(value, out typeable)) {

                    if (b_type)
                        Type(typeable);

                    return;
                }

                var op = new Dictionary<string, CalculatorActions> 
                 {{"add", CalculatorActions.Add},
                  {"sub", CalculatorActions.Sub},
                  {"mul", CalculatorActions.Mul},
                  {"div", CalculatorActions.Div},}.Get_Or_Default_(value, (CalculatorActions)0);


                if (op != 0) {
                    b_clear = true;
                    engine.Operate(op);
                    Operation.Raise();
                    return;
                }


                var act = new Dictionary<string, Action>
                {{"eq", Operate_Eq},
                {"c", Operate_C},
                {"ce", Operate_CE},
                {"bsp", () => m_vb_target.Perform_Backspace()}}.Get_Or_Null(value);

                if (act != null) {
                    act();

                    return;
                }


                ignored = true;
            }
            finally {
                if (!ignored)
                    if (m_vb_target != null)
                        m_vb_target.NotifyChanged();
            }
        }


        void but_Click(object sender, EventArgs e) {

            ButtonPressed.Raise(this, e);


            Handle_Button((Button)sender);

        }



        void Operate_CE() {
            m_vb_target.Text = "";

            Operation.Raise();

        }
        void Operate_C() {
            m_vb_target.Text = "0.00";
            m_vb_target.SelectAll();

            Operation.Raise();
        }
        void Operate_Eq() {


            b_clear = true;

            engine.Operate(CalculatorActions.Eq);

            if (m_vb_target != null)
                m_vb_target.SelectAll();

            is_eq_pressed = true;

            Operation.Raise();


        }
    }
}
