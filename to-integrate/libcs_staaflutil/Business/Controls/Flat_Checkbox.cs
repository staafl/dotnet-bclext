using System;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;

using Standardization;

namespace Common.Controls
{

    public class Flat_Check_Box : CheckBox
    {
        public Flat_Check_Box() {

            Refresh_Back_Color();
            this.FlatStyle = FlatStyle.Flat;
            this.CheckAlign = ContentAlignment.MiddleRight;
            this.TabStop = false;



        }

        void Refresh_Back_Color() {

            var color = this.Enabled ? Colors.CheckBoxes.Normal_Flat_Color :
                                       Colors.CheckBoxes.Disabled_Flat_Color;

            this.BackColor = color;

        }

        public override string Text {
            get {
                return "";
            }
            set {
            }
        }


        protected override void
        OnTextChanged(EventArgs e) {

            if (!base.Text.IsNullOrEmpty())
                base.Text = null;

        }


        protected override void
        OnTabStopChanged(EventArgs e) {

            if (this.TabStop)
                this.TabStop = false;
        }

        protected override void
        OnEnabledChanged(EventArgs e) {

            Refresh_Back_Color();
            base.OnEnabledChanged(e);

        }

        protected override void
        OnPaint(PaintEventArgs e) {

            base.OnPaint(e);

            var g = e.Graphics;

            var rect = this.ClientRectangle;
            var line = rect.Edge(0);


            using (var pen = new Pen(this.Parent.BackColor)) {

                g.DrawLine(pen, line.First, line.Second);
                line = line.Translate(1, 0);
                pen.Color = Color.Black;
                g.DrawLine(pen, line.First, line.Second);


            }

            //if (this.Focused) {
            //    using (var pen = new Pen(Brushes.Black)) {
            //        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    
            //        g.DrawRectangle(pen, rect.Expand(-2, -1, -2, -2));
            //    }
            //}


        }

    }
}
