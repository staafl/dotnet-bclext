using System;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;
using Standardization;

namespace Common.Controls
{
    public class Our_Button_Normal : Our_Button
    {
        readonly Border border;
        public Our_Button_Normal() {

            var offsets = Quad.Make(0, 0, 0, 0);

            border = Border.Create(this, offsets, Border.Border_Options.None);

            border.BackColor = Color.Transparent;
            border.Border_Width = 2;
            border.Visible = false;

        }

        protected override void On_Toggled_On(EventArgs e) {

            this.BackColor = Colors.Buttons.Toggled_Background;
            var bnds = this.Bounds;

            this.Bounds = bnds.Translate(2, 2).Expand(0, 0, -3, -3);
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.MouseOverBackColor = Colors.Buttons.Toggled_Background;
            border.BackColor = Colors.Buttons.Toggled_Background;
            this.FlatAppearance.BorderSize = 0;

            border.Visible = true;
            base.On_Toggled_On(e);

            // Workaround to make the button refresh its appearance
            Native_Methods.SendMessage(this.Handle, Native_Const.WM_MOUSEMOVE, 0, 0);
            this.Parent.Invalidate(bnds.Expand(4));
            this.Parent.Update();

        }

        protected override void On_Toggled_Off(EventArgs e) {

            this.BackColor = Colors.Buttons.Background; 
            this.FlatStyle = FlatStyle.Standard;

            var bnds = this.Bounds;
            this.Bounds = bnds.Translate(-2, -2).Expand(0, 0, 3, 3);

            
            border.Hide();
            border.Visible = false;
            base.On_Toggled_Off(e);

            this.Parent.Invalidate(bnds.Expand(4));
            this.Parent.Update();
        }
    }
}
