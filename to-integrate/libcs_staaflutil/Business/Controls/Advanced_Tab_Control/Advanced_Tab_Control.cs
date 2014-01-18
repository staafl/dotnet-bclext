using System;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    public partial class Advanced_Tab_Control : UserControl
    {
        private const int INT_butHeight = 22;

        public Advanced_Tab_Control() {

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint, true);

            but_left = new Unselectable_Button();
            but_right = new Unselectable_Button();
            Prepare_Arrow_Button(but_left, true);
            Prepare_Arrow_Button(but_right, false);



        }

        protected override void WndProc(ref Message m) {

            if (Suppress_Mouse_Buttons)
                switch (m.Msg) {
                    case Native_Const.WM_LBUTTONDOWN:
                    case Native_Const.WM_LBUTTONDBLCLK:
                    case Native_Const.WM_LBUTTONUP:

                    case Native_Const.WM_MBUTTONDOWN:
                    case Native_Const.WM_MBUTTONDBLCLK:
                    case Native_Const.WM_MBUTTONUP:

                    case Native_Const.WM_RBUTTONDOWN:
                    case Native_Const.WM_RBUTTONDBLCLK:
                    case Native_Const.WM_RBUTTONUP:
                        return;
                }


            base.WndProc(ref m);
        }
        public void Clear() {
            b_init = false;
            m_tabcount = -1;
            foreach (var arr in new Control[][] { m_tabs, m_buttons, m_borders })
                if (arr != null)
                    foreach (var ctrl in arr)
                        using (ctrl) { }
            this.Controls.Clear();

        }

        public void
        Setup(UserControl main_panel,
              params UserControl[] panels) {

            b_init.tift();
            b_init = true;
            main_panel.tifn();

            // panels may be "new UserControl[0]" but not null

            int len = panels.Length;

            m_tabcount = len + 1;

            m_tabs = new UserControl[m_tabcount];
            m_buttons = new Advanced_Tab_Button[m_tabcount];

            m_tabs[0] = main_panel;

            Array.Copy(panels, 0, m_tabs, 1, len);

            using (var g = this.CreateGraphics()) {

                for (int ii = 0; ii < len + 1; ++ii) {

                    var panel = m_tabs[ii];
                    var button = new Advanced_Tab_Button();

                    m_buttons[ii] = button;
                    this.Controls.Add(panel);
                    this.Controls.Add(button);

                    Prepare_Panel(panel);

                    Prepare_Button(button, panel, ii, g);




                }

            }


            m_pairs = m_tabs.Zip(m_buttons).arr();

            main_panel.Visible = true;
            this.Force_Handle();

        }

        protected override void OnResize(EventArgs e) {

            Position_Buttons();
            base.OnResize(e);
        }




        void Prepare_Arrow_Button(Button but, bool left) {

            but.FlatStyle = FlatStyle.Flat;
            but.Height = INT_butHeight;
            but.Width = INT_butHeight;
            but.Text = left ? "⇦" : "⇨";
            but.FlatAppearance.MouseOverBackColor = Advanced_Tab_Button.def_highlit_color;
            but.FlatAppearance.BorderSize = 1;

            but.BackColor = Advanced_Tab_Button.def_back_color;

            but.Click += (_1, _2) =>
            {
                int? to_activate = null;

                if (left) {
                    if (Active_Tab > 0) {
                        to_activate = Active_Tab - 1;

                    }
                }
                else {
                    if (Active_Tab < m_tabs.Length - 1) {
                        to_activate = Active_Tab + 1;

                    }
                }

                if (to_activate != null)
                    Active_Tab = to_activate.Value;
            };


            but.Visible = false;
            this.Controls.Add(but);

        }


        void Prepare_Button(Button button, UserControl panel, int index, Graphics g) {

            button.TabStop = false;
            button.Text = panel.Name;
            button.TextAlign = ContentAlignment.MiddleCenter;

            Resize_Button(g, button);

            Set_Button_Handler(button, index);

        }

        void Prepare_Panel(UserControl panel) {

            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Location = new Point(tab_xx, tab_yy);
            panel.Visible = true;
            Resize_Panel(panel);
        }

        void Resize_Panel(UserControl panel) {

            int ww = this.Width - 2 * tab_xx;
            int hh = this.Height - tab_yy;
            panel.Size = new Size(ww, hh);

        }


        void Resize_Button(Graphics g, Button button) {

            var sizef = g.MeasureString(button.Text, this.Font);
            var size = sizef.ToSize();

            int width = size.Width.Min_Max(button_min_width, button_max_width) + button_text_width_tolerance;
            int height = but_height;

            button.Size = new Size(width, height);


        }


        void Position_Buttons() {

            var gone_under_arrows = 0;

            button_under_arrows  = null;

            var width_less_buttons = this.Width - but_right.Width * 2 - 2;


            foreach (var button in m_buttons) {
                button.Visible = false;
            }

            for (int ii = first_visible_button; ii < m_buttons.Length; ++ii) {

                var button = m_buttons[ii];

                Position_Button(button, ii - first_visible_button);

     
                if (button.Right > width_less_buttons) {

                    if (gone_under_arrows == 0) {
                        button_under_arrows = ii;
                        button.Visible = true;
                    }

                    ++gone_under_arrows;
                }
                else {

                    button.Visible = true;

                }
            }


            if (gone_under_arrows > 1 || first_visible_button != 0) {
                but_right.Bounds = but_right.Bounds.Move_Edge(2, this.Width - 2);
                but_right.Bounds = but_right.Bounds.Move_Edge(1, 9);
                but_left.Bounds = but_right.Bounds.Move_Edge(2, this.Width - but_right.Width - 1 /* 3 */);

                but_left.Visible = but_right.Visible = true;

                but_left.BringToFront();
                but_right.BringToFront();

            }
            else {
                but_left.Visible = but_right.Visible = false;
                button_under_arrows = 0;
            }


        }


        void Position_Button(Button button, int number) {

            Point pt;

            int tab_number = first_visible_button + number;

            if (number == 0) {

                pt = new Point(button_xx, button_yy);

            }
            else {

                var to_the_left = m_buttons[tab_number - 1];
                pt = to_the_left.Location;
                pt = pt.Translate(to_the_left.Width, 0);
                pt = pt.Translate(button_gap_x, 0);

            }

            button.Location = pt;
            button.Height = but_height;

        }


        void Set_Button_Handler(Button button, int index) {

            button.Click += (sender, e) =>
            {
                Activate_Tab(index);
            };

        }

        //Old values: 15, 30;
        const int button_text_width_tolerance = 13;
        const int button_min_width = 30;
        const int button_max_width = 150; //100;

        //Old values: 22;
        const int but_height = 20;

        // Old values: 2
        const int button_gap_x = 1;
        const int button_xx = tab_xx + Advanced_Tab_Button.cst_left_expansion;

        //const int button_yy = 7;
        const int button_yy = tab_yy - but_height + 1;

        const int tab_xx = 2;

        // const int tab_yy = but_height + button_yy - 1;
        const int tab_yy = 31;

        readonly Button but_left;
        readonly Button but_right;

        int first_visible_button;
        int? button_under_arrows;

    }
}