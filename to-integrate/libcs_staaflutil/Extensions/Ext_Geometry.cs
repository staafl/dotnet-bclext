#if WINFORMS

using System;
using System.Linq;


#if WINFORMS

using System.Drawing;


#endif


namespace Fairweather.Service
{

    static partial class Extensions
    {

        static public PointF CenterF(this Rectangle rect) {

            var ret = new PointF((rect.Right + rect.Left) / 2.0f,
                                  (rect.Bottom + rect.Top) / 2.0f);

            return ret;
        }

        #region RECTANGLE MANIPULATION



        static public Rectangle Intersection(this Rectangle rect, Rectangle with) {
            rect.Intersect(with);
            return rect;
        }
        static public Rectangle No_Location(this Rectangle rect) {
            return new Rectangle(Point.Empty, rect.Size);
        }
        //static public bool Intersects_With(this Rectangle rect1, Rectangle rect2) {
        //    for (int ii = 0; ii < 4; ++ii) {
        //        if(rect1.Contains(rect2.Vertex(x))
        //            return true;
        //    }
        //    return false;
        //}
        ///
        // Does not allow for border overlapping
        // 
        static public bool Contains(this Rectangle outer, Rectangle inner) {

            if (outer.Left > inner.Left || outer.Top > inner.Top ||
                outer.Right < inner.Right || outer.Bottom < inner.Bottom)
                return false;

            return true;
        }

        static public Rectangle Center_In_Rectangle(this Rectangle r1, Rectangle r2) {

            var loc = r2.Center().Translate(-r1.Width / 2, -r1.Height / 2);
            var ret = new Rectangle(loc, r1.Size);

            return ret;

        }

        static public Point Center(this Rectangle rect) {

            Point ret = new Point((rect.Right + rect.Left) / 2,
                                  (rect.Bottom + rect.Top) / 2);

            return ret;
        }

        static public PointF Center(this RectangleF rect) {

            PointF ret = new PointF((rect.Right + rect.Left) / 2,
                                  (rect.Bottom + rect.Top) / 2);

            return ret;
        }

        static public Rectangle Translate(this Rectangle rect, int dx, int dy) {

            var ret = new Rectangle(rect.Left + dx, rect.Top + dy,
                                    rect.Width, rect.Height);

            return ret;
        }

        static public Rectangle Translate(this Rectangle rect, Point pt, bool positive) {

            var ret = Translate(rect,
                                positive ? pt.X : -pt.X,
                                positive ? pt.Y : -pt.Y);

            return ret;
        }

        static public Rectangle Expand(this Rectangle rect, int amount) {
            Rectangle ret;

            ret = new Rectangle(rect.Left - amount,
                                rect.Top - amount,
                                rect.Width + (2 * amount),
                                rect.Height + (2 * amount));

            return ret;
        }

        static public Rectangle Expand(this Rectangle rect,
                                            int amount,
                                            bool left, bool top,
                                            bool right, bool bottom) {


            Rectangle ret;

            int l_left = left ? rect.Left - amount : rect.Left;
            int l_top = top ? rect.Top - amount : rect.Top;

            int l_width = right ? rect.Width + amount : rect.Width;
            int l_height = bottom ? rect.Height + amount : rect.Height;

            if (left)
                l_width += amount;

            if (top)
                l_height += amount;

            ret = new Rectangle(l_left, l_top, l_width, l_height);

            return ret;
        }

        static public Rectangle Expand(this Rectangle rect,
                                            int left, int top,
                                            int right, int bottom) {


            Rectangle ret;

            int l_left = rect.Left - left;
            int l_top = rect.Top - top;

            int l_width = rect.Width + right + left;
            int l_height = rect.Height + bottom + top;

            ret = new Rectangle(l_left, l_top, l_width, l_height);

            return ret;
        }


        static public Point Inner_Point(this Rectangle rect, Point pt) {

            // If pt is a point in the same coordinate system
            // as rect, the output will be the same point
            // in a coordinate system with origin = rect.Location

            var ret = pt.Translate(rect.Location, false);

            return ret;
        }

        static void Vertex_Number_To_Bool_Bool(int number, out bool left, out bool top) {

            number %= 4;

            left = number == 0 || number == 3;
            top = number == 0 || number == 1;

        }

        static public Rectangle
        Align_Vertices(this Rectangle rect1,
                            Rectangle rect2,
                            int vertex1,
                            int vertex2) {


            var temp = new Rectangle(rect2.Vertex(vertex2),
                                     rect1.Size);

            var offset = rect1.Inner_Point(rect1.Vertex(vertex1));

            temp = temp.Translate(offset, false);

            return temp;

        }

        static public Rectangle?
        Align_Vertices_In_Container(this Rectangle rect,
                                         Rectangle align_with,
                                         Rectangle align_in) {

            var ret =
                Align_Vertices_In_Container(rect, align_with, align_in,
                                                            Pair.Make(1, 2),
                                                            Pair.Make(2, 1),
                                                            Pair.Make(3, 0),
                                                            Pair.Make(0, 3),
                                                            Pair.Make(0, 1));

            return ret;
        }

        static public Rectangle?
        Align_Vertices_In_Container(this Rectangle rect,
                                         Rectangle align_with,
                                         Rectangle align_in,
                                         params Pair<int>[] alignments) {


            Rectangle ret;
            foreach (var pair in alignments) {

                ret = Align_Vertices(rect, align_with, pair.First, pair.Second);

                if (align_in.Contains(ret))
                    return ret;

            }

            return null;
        }



        /// <summary>
        /// LTRB
        /// </summary>
        static public int Coord(this Rectangle rect, int coord) {
            switch (coord % 4) {
                case 0:
                    return rect.Left;
                case 1:
                    return rect.Top;
                case 2:
                    return rect.Right;
                case 3:
                    return rect.Bottom;
                default:
                    throw new Exception();

            }
        }

        /// Returns the specified Vertex of the rectangle

        static public Point Vertex(this Rectangle rect, int vert) {

            (vert < 0).tift();

            vert %= 4;

            bool left = (vert == 0 || vert == 3);
            bool top = (vert == 0 || vert == 1);

            Point ret = new Point(left ? rect.Left : rect.Right,
                                  top ? rect.Top : rect.Bottom);

            return ret;
        }

        static public Point Vertex(this Rectangle rect, bool left, bool top) {

            int vert = left ? (top ? 0 : 3) :
                              (top ? 1 : 2);

            return Vertex(rect, vert);

        }

        // The edges are counted clockwise modulo 4, 0 being Left-Top and 3 being Left-Bottom
        static public Point[] Vertices(this Rectangle rect, params int[] verts) {

            // Non-assignment binary operators associate left to right
            Point[] ret = verts.Select(_ii => rect.Vertex(_ii)).ToArray();


            return ret;
        }

        /// <summary>
        /// The returned edge consists of two adjacent vertices in anti-clockwise order
        /// </summary>
        static public Line2D Edge(this Rectangle rect, int edge) {

            int vert1 = edge;
            int vert2 = edge == 0 ? 3 : edge - 1;
            var pt1 = rect.Vertex(vert1);
            var pt2 = rect.Vertex(vert2);

            var ret = new Line2D(pt1, pt2);

            return ret;

        }

        static public Point Middle_Of_Edge(this Rectangle rect, int edge) {

            int xx = 0;
            int yy = 0;
            switch (edge % 4) {
                case 0:
                case 2:
                    xx = (rect.Left + rect.Right) / 2;
                    break;
                case 1:
                    xx = rect.Right; break;
                case 3:
                    xx = rect.Left; break;
            }
            switch (edge % 4) {
                case 1:
                case 3:
                    xx = (rect.Top + rect.Bottom) / 2;
                    break;
                case 0:
                    xx = rect.Top; break;
                case 2:
                    xx = rect.Bottom; break;
            }

            Point ret = new Point(xx, yy);
            return ret;
        }


        // Returns the distance between the specified pair of sides of 
        // the two rectangles relative to the first one's side
        static public int Get_Edge_Distance(this Rectangle rect1,
                                                 Rectangle rect2,
                                                 Direction_LURD edge) {
            return Get_Edge_Distance(rect1, rect2, edge, edge);
        }

        static public int Get_Edge_Distance(this Rectangle rect1,
                                      Rectangle rect2,
                                      Direction_LURD edge1,
                                      Direction_LURD edge2) {

            edge1.Same_Or_Opposite(edge2).tiff();

            var int_1 = edge1.As_Int();
            var int_2 = edge2.As_Int();

            var coord1 = rect1.Coord(int_1);
            var coord2 = rect2.Coord(int_2);

            var ret = coord1 - coord2;

            return ret;
        }

        /*       Untested        */

        /// <summary>
        /// Moves the specified edge to a coordinate without translating the
        /// rectangle
        /// Left = 0, Top = 1, Right = 2, Bottom = 3
        /// </summary>
        static public Rectangle
        Set_Edge(this Rectangle rect, int edge, int coordinate) {

            edge %= 4;
            int left = rect.Left,
                top = rect.Top,
                width = rect.Width,
                height = rect.Height;

            switch (edge) {
                case 0:
                    width += (left - coordinate);
                    left = coordinate;
                    break;
                case 1:
                    height += (top - coordinate);
                    top = coordinate; break;
                case 2:
                    width = coordinate - left; break;
                case 3:
                    height = coordinate - top; break;
            }

            var ret = new Rectangle(left, top, width, height);

            return ret;
        }

        /*       Untested        */

        static public Rectangle Set_Vertex(this Rectangle rect, int vertex, Point pt) {
            //01
            //32

            bool even = (vertex & 1) == 0;
            vertex &= 0x3;

            //bool even = (vertex % 2) == 0;
            //vertex %= 4;

            var ret = Set_Edge(rect, vertex, even ? pt.X : pt.Y);

            ++vertex;
            vertex %= 4;

            ret = Set_Edge(ret, vertex, even ? pt.Y : pt.X);

            return ret;
        }

        // To implement: Set_Center, Set_Point(int xx, int yy, int new_xx, int new_yy)

        static public Rectangle Set_Point(this Rectangle rect, Point pt1, Point pt2) {

            var ret = rect.Translate(pt1.X, pt1.Y);
            ret = ret.Translate(-pt2.X, -pt2.Y);

            return ret;
        }

        static public Rectangle
        Move_Edge(this Rectangle rect, int edge_number, int coordinate) {

            edge_number %= 4;
            // vertex n. xx is the second point of edge xx
            var vertex = rect.Vertex(edge_number);

            bool horizontal = Horizontal_Edge(edge_number);
            var edge_coord = horizontal ? vertex.Y : vertex.X;

            var to_translate = coordinate - edge_coord;

            int xx = 0, yy = 0;

            if (horizontal)
                yy = to_translate;
            else
                xx = to_translate;

            var ret = rect.Translate(xx, yy);

            return ret;

        }

        static public Rectangle Move_Vertex(this Rectangle rect, int vert_number, Point location) {

            vert_number %= 4;
            var vertex = rect.Vertex(vert_number);


            var to_translate = location.Translate(vertex, false);

            var ret = rect.Translate(to_translate.X, to_translate.Y);

            return ret;
        }

        static bool Horizontal_Edge(int edge_number) {

            edge_number %= 4;

            bool ret = (edge_number == 1 || edge_number == 3);

            return ret;

        }
        #endregion

        #region SIZE MANIPULATION

        static public Size Max_Dimensions(this Size sz1, Size sz2) {

            var ret = new Size(
                Math.Max(sz1.Width, sz2.Width),
                Math.Max(sz1.Height, sz2.Height));

            return ret;

        }

        static public Size Min_Dimensions(this Size sz1, Size sz2) {

            var ret = new Size(
                Math.Min(sz1.Width, sz2.Width),
                Math.Min(sz1.Height, sz2.Height));

            return ret;

        }

        public static Size Scale(this Size size, int factor, bool positive) {

            Size ret = new Size(positive ? size.Width * factor : size.Width / factor,
                                positive ? size.Height * factor : size.Height / factor);

            return ret;
        }

        public static Size Difference(this Size size1, Size size2) {

            int new_x = size1.Width - size2.Width;
            int new_y = size1.Height - size2.Height;

            Size ret = new Size(new_x, new_y);

            return ret;
        }

        public static Size Expand(this Size size1, int xx, int yy) {

            int x = size1.Width + xx;
            int y = size1.Height + yy;

            Size ret = new Size(x, y);

            return ret;
        }

        public static Size Expand(this Size size1, Size size2) {

            int x = size2.Width;
            int y = size2.Height;

            Size ret = new Size(size1.Width + x, size1.Height + y);

            return ret;
        }

        #endregion

        #region SIZEF MANIPULATION

        public static SizeF Scale(this SizeF SizeF, float factor, bool positive) {

            SizeF ret = new SizeF(positive ? SizeF.Width * factor : SizeF.Width / factor,
                                positive ? SizeF.Height * factor : SizeF.Height / factor);

            return ret;
        }

        public static SizeF Difference(this SizeF size1, SizeF size2) {

            float new_x = size1.Width - size2.Width;
            float new_y = size1.Height - size2.Height;

            SizeF ret = new SizeF(new_x, new_y);

            return ret;
        }

        public static SizeF Expand(this SizeF size1, float xx, float yy) {

            float x = size1.Width + xx;
            float y = size1.Height + yy;

            SizeF ret = new SizeF(x, y);

            return ret;
        }

        public static SizeF Expand(this SizeF size1, SizeF size2) {

            float x = size2.Width;
            float y = size2.Height;

            SizeF ret = new SizeF(size1.Width + x, size1.Height + y);

            return ret;
        }

        #endregion

        #region POINT MANIPULATION

        static public Int32 Quadrant(this Point pt) {

            int xxn = pt.X;
            int yyn = pt.Y;

            if (xxn >= 0)
                return yyn >= 0 ? 0 : 3;

            return yyn >= 0 ? 1 : 2;
        }

        static public Point SetX(this Point pt, int xx) {
            return new Point(xx, pt.Y);
        }
        static public Point SetY(this Point pt, int yy) {
            return new Point(pt.X, yy);
        }

        public static int Compare(this Point pt1, Point pt2) {

            int cmp_x = pt1.X.CompareTo(pt2.X);
            int cmp_y = pt1.Y.CompareTo(pt2.Y);

            switch (cmp_x + cmp_y) {
                case 2:
                    return 1;
                case -2:
                    return -1;
                default:
                    return 0;
            }
        }

        public static Point Translate(this Point pt, Point pt2, bool positive) {

            Point ret = new Point(positive ? pt.X + pt2.X : pt.X - pt2.X,
                                  positive ? pt.Y + pt2.Y : pt.Y - pt2.Y);

            return ret;
        }
        public static Point Translate(this Point pt, Size size, bool positive) {

            Point ret = new Point(positive ? pt.X + size.Width : pt.X - size.Width,
                                  positive ? pt.Y + size.Height : pt.Y - size.Height);

            return ret;
        }
        public static Point Translate(this Point pt, int dx, int dy) {

            Point ret = new Point(pt.X + dx, pt.Y + dy);

            return ret;
        }



        /// <summary> Returns the Left coordinates of a rectangle with a given center point and width
        /// </summary>
        public static int LeftFromCenter(this Point center, int width) {

            int ret = (center.X - width / 2);

            return ret;
        }

        public static Point Abs(this Point pt) {
            var ret = new Point(Math.Abs(pt.X), Math.Abs(pt.Y));
            return ret;

        }


        public static double Distance(this Point pt1, Point pt2) {

            var tran = pt1.Translate(pt2, false);
            var hsq = (tran.X * tran.X) + (tran.Y * tran.Y);
            var ret = Math.Sqrt(hsq);

            return ret;

        }

        #endregion

        #region POINTF MANIPULATION

        static public PointF SetX(this PointF pt, float xx) {
            return new PointF(xx, pt.Y);
        }
        static public PointF SetY(this PointF pt, float yy) {
            return new PointF(pt.X, yy);
        }

        public static float Compare(this PointF pt1, PointF pt2) {

            int cmp_x = pt1.X.CompareTo(pt2.X);
            int cmp_y = pt1.Y.CompareTo(pt2.Y);

            switch (cmp_x + cmp_y) {
                case 2:
                    return 1;
                case -2:
                    return -1;
                default:
                    return 0;
            }
        }

        //public static PointF Translate(this PointF pt, int dx, int dy) {

        //    PointF ret = new PointF(pt.X + dx,
        //                            pt.Y + dy);

        //    return ret;
        //}
        //public static PointF Translate(this PointF pt, Size size, bool positive) {

        //    PointF ret = new PointF(positive ? pt.X + size.Width : pt.X - size.Width,
        //                            positive ? pt.Y + size.Height : pt.Y - size.Height);

        //    return ret;
        //}
        public static PointF Translate(this PointF pt, PointF pt2, bool positive) {

            PointF ret = new PointF(positive ? pt.X + pt2.X : pt.X - pt2.X,
                                  positive ? pt.Y + pt2.Y : pt.Y - pt2.Y);

            return ret;
        }
        public static PointF Translate(this PointF pt, SizeF size, bool positive) {

            PointF ret = new PointF(positive ? pt.X + size.Width : pt.X - size.Width,
                                  positive ? pt.Y + size.Height : pt.Y - size.Height);

            return ret;
        }

        public static PointF Translate(this PointF pt, Size size, bool positive) {

            PointF ret = new PointF(positive ? pt.X + size.Width : pt.X - size.Width,
                                  positive ? pt.Y + size.Height : pt.Y - size.Height);

            return ret;
        }
        public static PointF Translate(this PointF pt, float dx, float dy) {

            PointF ret = new PointF(pt.X + dx, pt.Y + dy);

            return ret;
        }

        /// <summary> Returns the Left coordinates of a rectangle with a given center PointF and width
        /// </summary>
        public static float LeftFromCenter(this PointF center, float width) {

            float ret = (center.X - width / 2);

            return ret;
        }

        public static PointF Abs(this PointF pt) {
            var ret = new PointF(Math.Abs(pt.X), Math.Abs(pt.Y));
            return ret;

        }


        public static double Distance(this PointF pt1, PointF pt2) {

            var tran = pt1.Translate(pt2, false);
            var hsq = (tran.X * tran.X) + (tran.Y * tran.Y);
            var ret = Math.Sqrt(hsq);

            return ret;

        }

        #endregion


    }

}

#endif