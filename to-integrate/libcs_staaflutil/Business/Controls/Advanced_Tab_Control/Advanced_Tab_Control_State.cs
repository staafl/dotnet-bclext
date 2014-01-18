using System;
using System.Drawing;
using System.Windows.Forms;
using Fairweather.Service;

namespace Common.Controls
{
    public partial class Advanced_Tab_Control
    {
        public bool Suppress_Mouse_Buttons {
            get;
            set;
        }

        public bool Do_Not_Focus_Tabs {
            get; set;
        }
        
        protected UserControl[] m_tabs;

        public UserControl[] Tabs {
            get { return m_tabs; }
        }

        protected Advanced_Tab_Button[] m_buttons;
        protected Border[] m_borders;

        Pair<UserControl, Advanced_Tab_Button>[] m_pairs;

        bool b_init;
        int m_tabcount;


    }
}
