using System;
using System.Diagnostics;
using System.Drawing;

namespace Fairweather.Service
{
    public class Primitive_Line : Primitive_Base
    {

        public static Primitive_Line Line_Break(Pen pen, int yy) {


            Func<Rectangle, Point> start = rect => rect.Vertex(0).Translate(0, yy);
            Func<Rectangle, Point> end = rect => rect.Vertex(1).Translate(0, yy);

            return new Primitive_Line(pen, start, end);

        }

        readonly Func<Rectangle, Point> start;
        readonly Func<Rectangle, Point> end;

        protected override void Draw(Brush _1, Pen pen, Font _2, Graphics g, Rectangle rect) {

            var pt1 = start(rect);
            var pt2 = end(rect);

            //*

            var clip = g.Clip;
            g.Clip = new Region(rect);

            g.DrawLine(pen, pt1, pt2);
            g.Clip = clip;

            /*/
            var line = new Line2D(pt1, pt2);
            var null_line = G.ClipLine2D(line, rect);

            if (null_line.HasValue) {
                var draw1 = null_line.Value.First;
                var draw2 = null_line.Value.Second;

                g.DrawLine(pen, draw1, draw2);
            }
            //*/
        }

        public Primitive_Line(Pen pen, int length) : this(pen, Point.Empty, length, 0) { }

        public Primitive_Line(Pen pen, Point relative_start, Point relative_end)
            : base(null, null, null, pen) {


            this.start = rect => rect.Location.Translate(relative_start, true);
            this.end = rect => rect.Location.Translate(relative_end, true);

        }

        public Primitive_Line(Pen pen, Func<Rectangle, Point> start, Func<Rectangle, Point> end)
            : base(null, null, null, pen) {

            this.start = start;
            this.end = end;

        }

        public Primitive_Line(Pen pen, Point relative_start, int length, int slope)
            : this(pen, relative_start, Get_Rel_End(relative_start, length, slope)) {

        }

        static Point Get_Rel_End(Point relative_start, int length, int slope) {

            var sin = Math.Sin(slope);
            var cos = Math.Cos(slope);

            double xx = cos * length;
            double yy = sin * length;

            var ret = relative_start.Translate((int)xx, (int)yy);

            return ret;

        }
    }
}
