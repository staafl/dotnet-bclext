using System;
using System.Diagnostics;
using System.Drawing;

namespace Fairweather.Service
{
    [DebuggerDisplay("Rectangle,nq")]
    public class Primitive_Rectangle : Primitive_Base
    {
        readonly Brush background;

        public Primitive_Rectangle() : this(null) { }

        public Primitive_Rectangle(Rectangle intern)
            : this(intern, null, null) {
        }

        public Primitive_Rectangle(Endo<Rectangle> intern)
            : this(intern, null, null) {
        }

        /// <summary>
        /// Stop trying to write Haskell in C#
        /// </summary>
        public Primitive_Rectangle(Rectangle intern, Brush background, Pen pen)
            : base(rect => intern.Translate(rect.Location, true).Intersection(rect), null, null, pen) {
        }

        public Primitive_Rectangle(Endo<Rectangle> intern, Brush background, Pen pen)
            : base(intern, null, null, pen) {
            this.background = background;
        }



        protected override void Draw(Brush _1, Pen pen, Font _2, Graphics g, Rectangle rect) {


            if (background != null)
                g.FillRectangle(background, rect);

            g.DrawRectangle(pen, rect);

        }

    }
}
