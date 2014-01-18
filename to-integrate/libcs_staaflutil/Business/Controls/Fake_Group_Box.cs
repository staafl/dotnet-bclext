using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using Fairweather.Service;

namespace Common.Controls
{
    public partial class Fake_Group_Box : UserControl
    {
        public Fake_Group_Box() {

            InitializeComponent();

            text_label.Location = new Point(cst_text_xx, cst_text_yy);


            text_label.Text = null;

            Adjust_Border();

            text_label.BringToFront();

            this.SetStyle(ControlStyles.Selectable, false);
            this.SetStyle(ControlStyles.ContainerControl, true);
            Border_Pen = Pens.DarkGray;

        }

        public Pen Border_Pen { get; set; }

        const int cst_border_xx = 1;
        const int cst_border_yy = 8;
        const int cst_border_offset = -2;
        const int cst_text_xx = 5;
        const int cst_text_yy = 1;
        Rectangle border_loc;

        public System.Windows.Forms.Label Text_Label {
            get {
                return text_label;
            }
        }

        protected override void OnPaint(PaintEventArgs e) {

            var pen = Border_Pen;
            if (pen != null) {
                var g = e.Graphics;
                g.DrawRectangle(pen, border_loc);

            }

            base.OnPaint(e);

        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text {
            get {
                return text_label.Text;
            }
            set {
                text_label.Text = value;
            }
        }

        void Adjust_Border() {

            border_loc.Size = this.Size.Expand(cst_border_offset, cst_border_offset);
            border_loc.Location = new Point(cst_border_xx, cst_border_yy);

            border_loc.Height -= cst_border_yy;


        }

        protected override void OnResize(EventArgs e) {

            Adjust_Border();

            base.OnResize(e);

        }
    }
}
