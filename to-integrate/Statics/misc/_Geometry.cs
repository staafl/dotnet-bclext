
using System;
using System.Drawing;

namespace Fairweather.Service
{
#if WINFORMS
    static public class G
    {
        static public Line2D? Test_Clip() {

            var line2d = Line2D.From_Coords(30, 20, 280, 160);
            var clip_rect = Rectangle.FromLTRB(70, 150, 230, 60);

            var ret = ClipLine2D(line2d, clip_rect);

            return ret;

        }
        static public Rectangle FromLTRB(Point pt1, Point pt2) {
            return Rectangle.FromLTRB(pt1.X, pt1.Y, pt2.X, pt2.Y);
        }


        static void getPQClip(out Func<float, float, bool> pqclip, out Func<float> min, out Func<float> max) {

            float clip_start = 0, clip_end = 1;

            min = () => clip_start;
            max = () => clip_end;

            pqclip = (covered, distance) =>
            {
                // let DIR be the direction we're concerned with, CDIR the opposite
                // let SIDE be the DIR side of the rectangle;
                // SIDE splits the plane into two semi-planes, 
                // P+ (to the CDIR of SIDE) and P- (to the DIR of side)
                // 
                ///
                // we mean to clip LINE so that it lies in P+ completely
                ///
                // let covered be the distance between the start of LINE
                // and the DIR side of the rectangle (aka the distance to P+)
                // with DIR being the positive side and CDIR the negative 
                // (ie travelling DIR-wise)
                ///
                
                // LINE is parallel to SIDE?
                if (covered == 0) {

                    // LINE lies in P-?
                    if (distance < 0)
                        return false;

                    // LINE lies on SIDE or in P+
                    return true;

                }

                else {
                    /// let amount be the ratio of the distance to P+
                    /// and covered.
                    /// By Thales' theorem, it is also the ratio between
                    /// the clipped part of LINE and LINE
                    float amount = distance / covered;

                    // travelling CDIR-wise?
                    if (covered < 0) {

                        // Line is too short?
                        if (amount > clip_end)
                            return false;

                        // Need clipping?
                        else if (amount > clip_start)
                            clip_start = amount;

                    }
                    // travelling DIR-wise
                    else {

                        // Line is too short?
                        if (amount < clip_start)
                            return false;

                        // Need clipping?
                        else if (amount < clip_end)
                            clip_end = amount;
                    }

                    return true;

                }
            };
        }

        // http://www.skytopia.com/project/articles/compsci/clipping.html
        /// <summary>
        /// A straightforward line-clipping algorithm which makes use of Thales' theorem
        /// (Liang-Barsky)
        /// It clips LINE four times, respective to the four pairs of semi-planes defined
        /// by the four sides of CLIP, leaving only the portion which lies in the same
        /// plane as CLIP
        /// </summary>
        public static Line2D? ClipLine2D(Line2D line, Rectangle clip) {

            var x1 = line.X1;
            var y1 = line.Y1;

            var x2 = line.X2;
            var y2 = line.Y2;

            var run = x2 - x1;
            var rise = y2 - y1;

            var clip_Left = clip.Left;
            var clip_Right = clip.Right;
            var clip_Top = clip.Top;
            var clip_Bottom = clip.Bottom;

            Func<float, float, bool> pqClip;
            Func<float> min, max;

            getPQClip(out pqClip, out min, out max);

            PointF result1 = line.First, result2 = line.Second;

            // (x1 - clip_Left) is the xx distance between the start of the line
            // and the left side of the clipping rectangle WHEN GOING LEFT
            // -run is the distance travelled TO THE LEFT

            if (!pqClip(-run, x1 - clip_Left))
                return null;

            if (!pqClip(run, clip_Right - x1))
                return null;

            if (!pqClip(rise, clip_Top - y1))
                return null;

            if (!pqClip(-rise, y1 - clip_Bottom))
                return null;

            if (min() > 0)
                result1 = result1.Translate(run * min(), rise * min());

            if (max() < 1)
                result2 = result2.Translate(-run * max(), -rise * max());

            var ret = new Line2D(Point.Truncate(result1), Point.Truncate(result2));

            return ret;

        }

        // http://en.wikipedia.org/wiki/Liang-Barsky
        /// Doesn't work
        //public static Line2D? ClipLine2D(Line2D line, Rectangle rect) {

        //    //var [a-z]+ = rect.Left
        //    //var \1 = rect.\1

        //    var left = rect.Left;
        //    var right = rect.Right;
        //    var top = rect.Top;
        //    var bottom = rect.Bottom;

        //    var clip_tr = rect.Vertex(1);
        //    var clip_lb = rect.Vertex(3);

        //    PointF result1 = line.First;
        //    PointF result2 = line.Second;

        //    var normal = line.Second.Translate(line.First, false);
        //    Func<float, float, bool> pqClip;
        //    Func<float> min, max;

        //    getPQClip(out pqClip, out min, out max);

        //    if (!pqClip(-normal.X, result1.X - clip_lb.X))
        //        return null;

        //    if (!pqClip(normal.X, clip_tr.X - result1.X))
        //        return null;

        //    if (!pqClip(-normal.Y, result1.Y - clip_lb.Y))
        //        return null;

        //    if (!pqClip(normal.Y, clip_tr.Y - result1.Y))
        //        return null;

        //    if (max() < 1) {
        //        result2 = result2.SetX(result1.X + max() * normal.X);
        //        result2 = result2.SetY(result1.Y + max() * normal.Y);
        //    }

        //    if (min() > 0)
        //        result1 = result1.Translate(min() * normal.X, min() * normal.Y);

        //    return new Line2D(Point.Truncate(result1), Point.Truncate(result2));
        //}




        //static public Line2D ClipLine2D(Line2D line, Rectangle rect) {

        //    Point pt1 = line.First, pt2 = line.Second;

        //    var c1 = rect.Contains(pt1);
        //    var c2 = rect.Contains(pt2);

        //    if (c1 && c2)
        //        return line;

        //    var normal = line.Normal;
        //    var rectt = rect.Translate(line.First, false);
        //    var sin = normal.Sin;



        //    if (!c1) {

        //        var xxl = rectt.Left;

        //        var solvel = (int)(xxl * sin);

        //        var temp = new Point(xxl, solvel);

        //        if (line2D.Contains(temp))
        //            pt1 = temp;

        //    }
        //}
        
        static public Endo<Rectangle> Translator(int xx, int yy) {
            return rect => rect.Translate(xx, yy);
        }
        static public Endo<Rectangle> Translator(Point pt, bool pos) {
            return rect => rect.Translate(pt, pos);
        }
        static public Endo<Rectangle> Intersector(Rectangle rect) {
            return rect1 => rect1.Intersection(rect);
        }
        static public Endo<Rectangle> Expander(int amount) {
            return Expander(amount, amount, amount, amount);
        }
        static public Endo<Rectangle> Expander(int left, int top, int right, int bottom) {
            return rect => rect.Expand(left, top, right, bottom);
        }
        static public Endo<Rectangle> Expander(int amount, bool left, bool top, bool right, bool bottom) {
            return rect => rect.Expand(amount, left, top, right, bottom);
        }


    }
#endif
}