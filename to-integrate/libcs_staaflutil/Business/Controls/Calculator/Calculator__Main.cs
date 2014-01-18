using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Fairweather.Service;
using Fonts = Standardization.Fonts;

namespace Common.Controls
{
    public partial class Calculator : UserControl
    {

        public Calculator(IActiveControlEnabledForm host) {


            this.SetStyle(ControlStyles.Selectable, false);

            this.m_host = (host as IActiveControlEnabledForm);
            m_host.tifn();


            InitializeComponent();

            Set_Event_Handlers();

            Prepare_Buttons();

            // Set this here
            this.Key_Font = Fonts.Calculator.Key_Font;

            Prepare_Locations_And_Size();

            Refresh_Host_Active_Control();




        }


        void Set_Event_Handlers() {

            this.m_host.ActiveControlChanged += (_1, _2) => { Refresh_Host_Active_Control(); };
            this.groupBox.MouseEnter += groupBox_MouseEnter;

        }

        void Prepare_Buttons() {

            buttons = new Button[4, 5];

            foreach (Control ctrl in groupBox.Controls) {

                if (!(ctrl is Button))
                    continue;

                var but = (Button)ctrl;
                var tag = but.Tag.StringOrDefault("");

                if (tag.Length == 2) {

                    // The array is column-major
                    int x = int.Parse(tag.Substring(0, 1));
                    int y = int.Parse(tag.Substring(1, 1));
                    buttons[x, y] = but;
                }

            }

            foreach (var but in buttons) {
                but.TabStop = false;
                //but.Size = new Size(cst_but_width, cst_but_height);
            }
        }

        void Prepare_Locations_And_Size() {

            int xx = cst_offset_h;
            int yy = cst_offset_v + 6;

            foreach (var row in buttons.Iterate(false)) {

                xx = cst_offset_h + 1;

                foreach (var but in row) {
                    but.Location = new Point(xx, yy);
                    xx += cst_but_width + cst_offset_h;

                }

                yy += cst_but_height + cst_offset_v;
            }

            this.groupBox.Size =
                this.Size = new Size(xx + 2, yy + 2);
        }

        // ****************************


        bool is_eq_pressed = false;

        void Set_Target_Box(ICalculatorTextBox value) {

            if (m_vb_target == value)
                return;

            if (m_vb_target != null) {
                if (!is_eq_pressed && m_vb_target.IsCalculable) {

                    Operate_Eq();
                    m_vb_target.NotifyChanged();

                }
            }

            // Assume we're starting out fresh
            is_eq_pressed = true;

            m_vb_target = value;


            if (value == null) {

                Aux_Refresh_Buttons(false);

                engine.Disconnect();

                b_type = false;

                return;

            }

            b_type = true;

            var calculable = value.IsCalculable;

            if (calculable) {

                Aux_Refresh_Buttons(true);

                b_clear = true;


                int max_len = Math.Min(10, m_vb_target.MaxLength);
                max = A.Pow(10.0M, max_len + 1) - 1; // 99999...

                engine.Output = dec => m_vb_target.Set_Text(dec > max ? 0.0m : dec);

                engine.Input = () => m_vb_target.Text.DecimalOrZero(null);

            }

            else {

                Aux_Refresh_Buttons(false);

                b_clear = false;

            }
        }



        ICalculatorTextBox Target_Box {
            [DebuggerStepThrough]
            get { return m_vb_target; }

            set {
                Set_Target_Box(value);

            }
        }

        ICalculatorTextBox Get_Target(Control parent) {

            ICalculatorTextBox ret = null;

            if (parent is ICalculatorTextBox) {
                ret = (ICalculatorTextBox)parent;
                return ret;
            }

            var children = new Stack<Control>();

            if (parent is ContainerControl) {

                var child = ((ContainerControl)parent).Active_Child();
                while (child != null) {
                    children.Push(child);
                    child = child.Parent;
                }

            }

            ret = Sieve(children);

            if (ret == null) {

                var parents = new Queue<Control>();

                while (parent != null) {
                    parents.Enqueue(parent);
                    parent = parent.Parent;
                }

                ret = Sieve(parents);

            }

            return ret;

        }

        ICalculatorTextBox Sieve(IEnumerable<Control> en) {

            foreach (var ctrl in en) {

                if (ctrl is ICalculatorTextBox) {
                    return (ICalculatorTextBox)ctrl;
                }

                if (ctrl is TextBoxBase) {
                    return new CalculatorTextBoxWrapper((TextBoxBase)ctrl);
                }
            }

            return null;
        }

        void Set_Target(Control target) {

            m_target = target;

            Target_Box = Get_Target(target);

        }

        public Control Target {
            [DebuggerStepThrough]
            get { return m_target; }

            set {
                Set_Target(value);
            }
        }






        void Refresh_Host_Active_Control() {

            var temp = m_host.ActiveControl;

            if (!IsMyControl(temp))
                Target = temp;

        }

        void Aux_Refresh_Buttons(bool is_calculable) {

            int ii, jj;

            // X******
            // xxxxxxx
            // xxxxxxx
            // xxxxxxx
            for (ii = 1; ii < buttons.GetLength(0); ++ii)
                buttons[ii, 0].Enabled = is_calculable;


            // 
            --ii;

            // xxxxxxx 
            // xxxxxx*
            // xxxxxx*
            // xxxxxx*
            for (jj = 1; jj < buttons.GetLength(1); ++jj)
                buttons[ii, jj].Enabled = is_calculable;

        }

    }
}
