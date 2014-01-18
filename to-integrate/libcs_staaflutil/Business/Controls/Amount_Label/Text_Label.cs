using System;
using System.Windows.Forms;
using System.Diagnostics;
using Standardization;
using Fairweather.Service;

namespace Common.Controls
{
    public class Text_Label : Label
    {
        public Text_Label() {

            this.Flat_Style(true);

        }

        protected override void OnDoubleClick(EventArgs e) {

            Clipboard.SetText(this.Text);

            //var bc = this.BackColor;
            //var fc = this.ForeColor;

            //this.BackColor = fc;
            //this.ForeColor = bc;

            //this.Invalidate();
            //this.Refresh();

            //base.OnDoubleClick(e);

            //this.Delay(() =>
            //{
            //    this.BackColor = bc;
            //    this.ForeColor = fc;
            //}, 10);

        }
    }
}
