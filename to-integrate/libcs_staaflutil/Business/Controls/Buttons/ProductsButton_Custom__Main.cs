using System;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{

    // Custom-drawn button for those times when the text just doesn't
    // fit into a normal one
    // VERY INCOMPLETE
    public class Our_Button_Custom : Our_Button
    {
        protected override void OnPaint(PaintEventArgs e) {

            // if(b_Paint)
            //   return;

            base.OnPaint(e);

            var g = e.Graphics;

            using(var fore = new SolidBrush(this.ForeColor))
            using(var back = new SolidBrush(this.BackColor))
            {
                var rect = this.ClientRectangle;

                g.FillRectangle(back, rect.Expand(-1));

                Point point = rect.Center();
                SizeF size = g.MeasureString(this.Text, this.Font);

                point = point.Translate(size.ToSize().Scale(2, false), false);

                g.DrawString(this.Text, this.Font, fore, point);

            }


        }
        public Our_Button_Custom() {

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | 
                          ControlStyles.UserPaint, true);


        }

        protected override void On_Toggled_On(EventArgs e) {

            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 2;
            base.On_Toggled_On(e);
        }

        protected override void On_Toggled_Off(EventArgs e) {

            this.FlatStyle = FlatStyle.Standard;
            this.FlatAppearance.BorderSize = 1;
            base.On_Toggled_Off(e);
        }
    }
}