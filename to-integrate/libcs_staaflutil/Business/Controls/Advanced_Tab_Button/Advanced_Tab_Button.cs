using System;

using System.Windows.Forms;


namespace Common.Controls
{
      public partial class Advanced_Tab_Button : Button
      {

            public Advanced_Tab_Button() {

                  this.SetStyle(ControlStyles.UserPaint |
                                 ControlStyles.AllPaintingInWmPaint |
                                 ControlStyles.SupportsTransparentBackColor,
                                 true);

                  this.SetStyle(ControlStyles.Selectable,
                                false);

                  Button_Activated += new EventHandler<EventArgs>(Advanced_Tab_Button_Button_Activated);

                  this.BackColor = def_back_color;
            }


            Advanced_Tab_Control m_tab_control;


            public Advanced_Tab_Control Tab_Control {
                  get { return m_tab_control; }
            }

            public bool Is_Activated {
                  get { return m_is_activated; }
                  set { m_is_activated = value; }
            }






      }
}