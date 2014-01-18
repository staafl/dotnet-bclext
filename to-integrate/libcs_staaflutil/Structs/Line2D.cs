using System;
using System.Diagnostics;



namespace Fairweather.Service
{
#if WINFORMS
    using Point2D = System.Drawing.Point;
    using Point2DF = System.Drawing.PointF;
    using Rectangle2D = System.Drawing.Rectangle;


    [DebuggerStepThrough]
    public struct Line2D
    {

        public bool Parallel(Line2D line) {

            if (Horizontal)
                return line.Horizontal;

            return Tg == line.Tg;

        }

        public bool Collinear(Line2D line) {

            if (First == line.First) {
                if (!Parallel(new Line2D(Second, line.Second)))
                    return false;
            }
            else {
                if (!Parallel(new Line2D(First, line.First)))
                    return false;
            }

            return true;//Parallel(line)


        }

        public Point2D? Intersection(Line2D line) {

            if (Horizontal) {

                if (line.Horizontal)
                    return null;

                return line.Intersection(this);
            }
            throw new NotImplementedException();
        }
        public static Line2D From_Coords(int x1, int y1, int x2, int y2) {

            return new Line2D(new Point2D(x1, y1), new Point2D(x2, y2));

        }

        readonly Point2D m_first;

        readonly Point2D m_second;

        readonly bool m_directed;


        public Point2D First {
            get {
                return this.m_first;
            }
        }

        public Point2D Second {
            get {
                return this.m_second;
            }
        }

        public bool Directed {
            get {
                return this.m_directed;
            }
        }

        public Line2D(Point2D pt)
            : this(Point2D.Empty, pt) {
        }

        public Line2D(Pair<Point2D> pair) : this(pair.First, pair.Second, false) { }

        public Line2D(Pair<Point2D> pair, bool directed) : this(pair.First, pair.Second, directed) { }

        public Line2D(Point2D first,
                      Point2D second)
            : this(first, second, false) {
        }

        public Line2D(Point2D first,
                      Point2D second,
                      bool directed) {

            directed.tift("not implemented");

            this.m_first = first;
            this.m_second = second;
            this.m_directed = directed;
        }

        public Double Length {
            get {
                var xx = m_first.X - m_second.X;
                var yy = m_first.Y - m_second.Y;

                var ret = Math.Sqrt(xx * xx + yy * yy);

                return ret;
            }
        }

        public Line2D Flip {
            get {
                var ret = new Line2D(m_second, m_first, m_directed);

                return ret;
            }
        }

        float Avg(bool xx) {

            float ret = xx ? m_first.X + m_second.X :
                             m_first.Y + m_second.Y;

            ret /= 2.0f;

            return ret;

        }

        public Point2DF Middle {
            get {
                var ret = new Point2DF(Avg(true), Avg(false));

                return ret;
            }
        }

        public static explicit operator Pair<Point2D>(Line2D line) {

            return new Pair<Point2D>(line.m_first, line.m_second);

        }

        public Line2D Normal {
            get {
                return new Line2D(Point2D.Empty,
                                  new Point2D(m_second.X - m_first.X,
                                              m_second.Y - m_first.Y));

            }
        }

        public Line2D Scale(float scale) {

            var normal = this.Normal;

            var pt = new Point2DF(normal.m_second.X * scale,
                                  normal.m_second.Y * scale);

            var ret = new Line2D(m_first, Point2D.Truncate(pt), m_directed);

            return ret;

        }

        public Line2D Translate(int xx, int yy) {

            var first = m_first.Translate(xx, yy);
            var second = m_second.Translate(xx, yy);

            var ret = new Line2D(first, second, m_directed);

            return ret;

        }


        public static Line2D operator -(Line2D first, Line2D second) {
            var norm1 = first.Normal;
            var norm2 = second.Normal;

            return new Line2D(norm1.Second, norm2.Second);
        }

        public int Rise {
            get {
                return m_second.Y - m_first.Y;
            }
        }
        public int Run {
            get {
                return m_second.X - m_first.X;
            }
        }
        public bool Horizontal {
            get {
                return this.Second.Y == this.First.Y;
            }
        }

        public bool IsPoint {
            get {
                return this.First == this.Second;
            }
        }


        /// <summary>
        /// The origin is 0
        /// </summary>
        public int Quadrant {
            get {
                return this.Normal.Second.Quadrant();
            }
        }


        //public Rational CotgR {
        //    get {
        //        var normal = this.Normal;
        //        return new Rational(normal.m_second.X, normal.m_second.Y);
        //    }
        //}
        //public Rational TgR {
        //    get {
        //        var normal = this.Normal;
        //        return new Rational(normal.m_second.Y, normal.m_second.X);
        //    }
        //}

        //public Rational CosR {
        //    get {
        //        var normal = this.Normal;
        //        return new Rational(normal.m_second.X/normal.Length);
        //    }
        //}

        //public Rational SinR {
        //    get {
        //        var normal = this.Normal;
        //        return new Rational(normal.m_second.Y, normal.Length);
        //    }
        //}


        public double Cotg {
            get {
                var normal = this.Normal;
                return normal.m_second.X / (double)normal.m_second.Y;
            }
        }

        public double Tg {
            get {
                var normal = this.Normal;
                return normal.m_second.Y / (double)normal.m_second.X;
            }
        }

        public double Cos {
            get {
                var normal = this.Normal;
                return normal.m_second.X / (double)normal.Length;
            }
        }

        public double Sin {
            get {
                var normal = this.Normal;
                return normal.m_second.Y / (double)normal.Length;
            }
        }

        public double Angle {
            get {
                var ret = Math.Asin(this.Sin);

                return ret;
            }
        }

        public int X1 {
            get {
                return First.X;
            }
        }
        public int Y1 {
            get {
                return First.Y;
            }
        }
        public int X2 {
            get {
                return Second.X;
            }
        }
        public int Y2 {
            get {
                return Second.Y;
            }
        }

        Rectangle2D From_Diagonal() {
            return Rectangle2D.FromLTRB(First.X, First.Y, Second.X, Second.Y);

        }


        public bool Contains(Point2D pt) {

            var norm2 = new Line2D(pt);
            var norm1 = this.Normal;

            if (!this.From_Diagonal().Contains(pt))
                return false;

            if (this.IsPoint)
                return this.First == pt;

            if (norm2.Quadrant != this.Quadrant)
                return false;

            if (norm2.Sin != norm1.Sin ||
                norm2.Cos != norm1.Cos)
                return false;

            return true;

        }
        #region Line2D

        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "first = " + this.m_first;
            ret += ", ";
            ret += "second = " + this.m_second;
            ret += ", ";
            ret += "directed = " + this.m_directed;

            ret = "{Line2D: " + ret + "}";
            return ret;

        }

        public bool Equals(Line2D obj2) {

#pragma warning disable
            if (this.m_first == null) {
                if (obj2.m_first != null)
                    return false;
            }
            else {
                if (!this.m_first.Equals(obj2.m_first))
                    return false;
            }


            if (this.m_second == null) {
                if (obj2.m_second != null)
                    return false;
            }
            else {
                if (!this.m_second.Equals(obj2.m_second))
                    return false;
            }


            if (this.m_directed == null) {
                if (obj2.m_directed != null)
                    return false;
            }
            else {
                if (!this.m_directed.Equals(obj2.m_directed))
                    return false;
            }


#pragma warning restore
            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Line2D);

            if (ret)
                ret = this.Equals((Line2D)obj2);


            return ret;

        }

        public static bool operator ==(Line2D left, Line2D right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Line2D left, Line2D right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

#pragma warning disable
            unchecked {
                int ret = 23;
                int temp;

                if (this.m_first != null) {
                    ret *= 31;
                    temp = this.m_first.GetHashCode();
                    ret += temp;

                }

                if (this.m_second != null) {
                    ret *= 31;
                    temp = this.m_second.GetHashCode();
                    ret += temp;

                }

                if (this.m_directed != null) {
                    ret *= 31;
                    temp = this.m_directed.GetHashCode();
                    ret += temp;

                }

                return ret;
            }
#pragma warning restore
        }

        #endregion
    }
#endif
}