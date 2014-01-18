using System;
using System.Windows.Forms;

namespace Fairweather.Service
{
    public class Lock_Text : IDisposable
    {
        readonly Control ctrl1, ctrl2;
        readonly bool lock12, lock21;

        public Lock_Text(Control ctrl1, Control ctrl2, bool lock12, bool lock21) {
            this.ctrl1 = ctrl1;
            this.ctrl2 = ctrl2;
            this.lock12 = lock12;
            this.lock21 = lock21;

            Init(true);
        }

        public void Dispose() {
            Init(false);
        }

        void Init(bool arg) {
            if (arg) {
                if (lock12)
                    ctrl1.TextChanged += ctrl1_TextChanged;
                if (lock21)
                    ctrl2.TextChanged += ctrl2_TextChanged;

            }
            else {
                if (lock12)
                    ctrl1.TextChanged -= ctrl1_TextChanged;
                if (lock21)
                    ctrl2.TextChanged -= ctrl2_TextChanged;
            }
        }

        public bool Suspended { get; set; }

        void ctrl1_TextChanged(object sender, EventArgs e) {
            if (!Suspended)
            ctrl2.Text = ctrl1.Text;
        }

        void ctrl2_TextChanged(object sender, EventArgs e) {
            if (!Suspended)
            ctrl1.Text = ctrl2.Text;
        }
    }
}
