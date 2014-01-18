using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Fairweather.Service;
using Common;
using Common.Dialogs;

namespace Common
{
    public partial class Quick_Links_Form_Concept : Form_Base
    {
        public Quick_Links_Form_Concept()
            : base(Form_Kind.Modal_Dialog) {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m) {

            if (m.Msg == Native_Const.WM_MOUSEWHEEL) {

                int scroll = m.Translate_Mouse_Wheel();

                flowLayoutPanel1.Do_Scroll(scroll);
                return;
            }

            base.WndProc(ref m);

        }
    }
}
