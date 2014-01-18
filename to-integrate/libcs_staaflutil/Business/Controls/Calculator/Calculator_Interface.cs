using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    public partial class Calculator : UserControl
    {
        public Font Key_Font {
            get {

                return m_Key_Font;
            }
            set {
                if (m_Key_Font == value)
                    return;

                m_Key_Font.Try_Dispose();
                m_Key_Font = value;

                foreach (Control ctrl in buttons)
                    ctrl.Font = m_Key_Font;

            }
        }

    }
}