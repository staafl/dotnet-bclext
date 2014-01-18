using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Fairweather.Service;

namespace Common.Controls
{
    public partial class Calculator : UserControl
    {
        Font m_Key_Font;

        readonly IActiveControlEnabledForm m_host;

        readonly Calculator_Engine engine = new Calculator_Engine();


        decimal max;
        ICalculatorTextBox m_vb_target;
        Control m_target;
        bool b_type;
        bool b_clear;

        const int cst_but_width = 31;
        const int cst_but_height = 31;
        const int cst_offset_h = 1;
        const int cst_offset_v = 1;



        Button[,] buttons;

        public event EventHandler<EventArgs> ButtonPressed;


        public Calculator_Engine Engine {
            [DebuggerStepThrough]
            get { return engine; }
        }

        public Action<decimal> Output {
            [DebuggerStepThrough]
            get { return engine.Output; }
            [DebuggerStepThrough]
            set { engine.Output = value; }
        }
        public Func<decimal> Input {
            [DebuggerStepThrough]
            get { return engine.Input; }
            [DebuggerStepThrough]
            set { engine.Input = value; }
        }

        static readonly Regex rx_button_val = new Regex("but_(.*)");
        static readonly Regex rx_only_zero = new Regex("^0+$");

      
    }
}