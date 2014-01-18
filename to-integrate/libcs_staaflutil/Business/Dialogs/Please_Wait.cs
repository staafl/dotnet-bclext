using System;
using System.ComponentModel;
using System.Windows.Forms;
using Common;
using Common.Dialogs;
using Fairweather.Service;

namespace Common.Dialogs
{
    public partial class Please_Wait : Form_Base
    {

        public Please_Wait()
            : base(Form_Kind.Modal_Dialog) {

            InitializeComponent();

            this.progress_bar.Value = 0;

        }

        public ProgressBar Bar {
            get {
                return progress_bar;
            }
        }

        bool b_close;

        public bool Closeable {
            get { return b_close; }
            set { b_close = value; }
        }

        protected override void OnClosing(CancelEventArgs e) {

            if (!b_close) {
                e.Cancel = true;
                return;
            }

            base.OnClosing(e);
        }

    }
}
