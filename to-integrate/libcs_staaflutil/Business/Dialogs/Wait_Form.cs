
using System.Windows.Forms;
using Fairweather.Service;

namespace Common.Dialogs
{
    public partial class Wait_Form : Form_Base
    {
        public Wait_Form()
            : base(Form_Kind.Modal_Dialog) {
            InitializeComponent();
            this.Text = Data.Default_Title;




        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
                using (timer) { }
            }
            base.Dispose(disposing);
        }

        Timer timer;
        protected override void OnShown(System.EventArgs e) {

            timer = new Timer();
            timer.Tick += (_1, _2) =>
            {
                this.Force_Handle();
                BeginInvoke((MethodInvoker)(() =>
                {
                    this.Refresh();

                }));
            };

            base.OnShown(e);
        }
        protected override void OnFormClosing(FormClosingEventArgs e) {
            base.OnFormClosing(e);
        }
    }
}
