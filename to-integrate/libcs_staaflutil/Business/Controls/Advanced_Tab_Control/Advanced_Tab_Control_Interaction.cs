using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Fairweather.Service;
using Standardization;
namespace Common.Controls
{
    public partial class Advanced_Tab_Control : UserControl
    {
        protected virtual void
        Activate_Tab(int tab_index) {

            Activate_Tab(tab_index, false);

        }

        public event Handler_RO<int> Tab_Activated;

        protected virtual void
        Activate_Tab(int tab_index, bool force) {

            if (!force && tab_index == m_active_index) 
                return;
            
            m_active_index = tab_index;

            foreach (var panel in m_tabs)
                panel.Visible = false;

            var button = m_buttons[m_active_index];

            if (!button.Is_Activated)
                button.Activate();

            var tab = m_tabs[m_active_index];
            tab.Visible = true;

            if(!Do_Not_Focus_Tabs)
                tab.Focus();

            Refresh_Colors();

            Tab_Activated.Raise(this, Args.Make_RO(tab_index));

            bool ok = true;

            do {
                ok = true;

                if (tab_index == button_under_arrows &&
                    button_under_arrows != 0) {
                    ++first_visible_button;
                    ok = false;

                }
                else if (tab_index < first_visible_button) {
                    first_visible_button = tab_index;
                    ok = false;

                }

                Position_Buttons();
            } while (!ok);
        }

        protected virtual void
        Refresh_Colors() {

            foreach (var triple in m_pairs) {

                triple.First.BackColor = BackColor;
                triple.Second.BackColor = Normal_Button_Color;
                triple.Second.Highlight_Color = Highlight_Color;

                triple.Second.FlatAppearance.MouseOverBackColor = m_selected_color;
                triple.Second.FlatAppearance.MouseDownBackColor = m_selected_color;

            }

            m_tabs[m_active_index].BackColor = m_selected_color;

            this.Invalidate(new Rectangle(0, 0, this.Width, tab_yy));


        }

        int m_active_index = -1;

        public int Active_Tab {

            get {
                var ret = m_active_index;
                return ret;
            }
            set {
                if (m_active_index == value)
                    return;

                Activate_Tab(value);
            }
        }

        Color m_selected_color = Colors.Tab_Control.Default_Highlight_Color;
        Color m_normal_button_color = Colors.Tab_Control.Default_Button_Back_Color;


        public Color Highlight_Color {
            get {
                var ret = m_selected_color;
                return ret;
            }
            set {
                if (m_selected_color != value) {
                    m_selected_color = value;

                    foreach (var button in this.m_buttons)
                        button.Highlight_Color = value;
                }
            }
        }
        public Color Normal_Button_Color {
            get {
                return m_normal_button_color;
            }
            set {
                if (m_normal_button_color == value)
                    return;

                m_normal_button_color = value;

                foreach (var button in this.m_buttons)
                    button.BackColor = value;
            }
        }

        public override Color BackColor {
            get {
                return base.BackColor;
            }
            set {
                base.BackColor = value;
            }
        }


        public void Set_Alternate_Highlight_Color() {
            Highlight_Color = Colors.Tab_Control.Alternate_Highlight_Color;
        }

        public void Set_Default_Highlight_Color() {
            Highlight_Color = Colors.Tab_Control.Default_Highlight_Color;
        }

        public void Set_Default_BackColor() {
            this.BackColor = Colors.Tab_Control.Default_Back_Color;
        }

        public void Set_Default_Normal_Button_Color() {
            this.BackColor = Colors.Tab_Control.Default_Button_Back_Color;
        }

        public void Set_Alternate_Normal_Button_Color() {
            this.BackColor = Colors.Tab_Control.Alternate_Button_Back_Color;

        }


        protected override void OnSizeChanged(EventArgs e) {

            if (!b_init) {
                return;
            }

            if (b_init) {
                foreach (var panel in m_tabs) {
                    Resize_Panel(panel);
                }
            }


            base.OnSizeChanged(e);

        }
    }
}