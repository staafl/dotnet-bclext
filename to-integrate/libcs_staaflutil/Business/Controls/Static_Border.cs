using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;


namespace Common.Controls
{
    partial class Static_Border : Label
    {

        int m_border_width;

        public int Border_Width {
            get {
                var ret = m_border_width;
                return ret;
            }
            set {
                if (m_border_width != value) {
                    m_border_width = value;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e) {

            base.OnPaint(e);

            Region region;

            {
                var rect = this.ClientRectangle;

                region = new Region(rect);

                rect = rect.Expand(-m_border_width);

                region.Exclude(rect);
            }

            {
                var g = e.Graphics;
                g.FillRegion(Brushes.Gray, region);
            }

        }

        public Static_Border() {

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.SupportsTransparentBackColor, true);

            this.SetStyle(ControlStyles.Opaque |
                          ControlStyles.Selectable, false);

            this.BackColor = Color.Transparent;
            this.SendToBack();

        }

    }

}