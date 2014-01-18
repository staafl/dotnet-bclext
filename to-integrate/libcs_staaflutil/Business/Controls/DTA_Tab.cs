using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;
using Standardization;

namespace Common.Controls
{
    public class DTA_Tab : UserControl
    {

        public DTA_Tab() {

        }

        bool is_set_up;

        public bool Setup() {

            if (H.Set(ref is_set_up, true))
                return false;

            return true;
        }


        void DTA_Tab_Enter(object sender, EventArgs e) {

        }

        protected override void OnLoad(EventArgs e) {

            if (!is_set_up)
                Setup();

            base.OnLoad(e);

        }

        public virtual Dictionary<Control, Ini_Field>
        Get_Fields() {

            return new Dictionary<Control, Ini_Field>();

        }
    }
}
