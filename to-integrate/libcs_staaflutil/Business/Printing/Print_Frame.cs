

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Fairweather.Service
{

    /// <summary>
    /// This class encapsulates the layout information needed
    /// to print IRenderable instances
    /// </summary>
    public abstract class Print_Frame
    {

        protected Print_Frame() {


        }

        protected Rectangle? Intersect_Or_Null(Rectangle page_bounds, Rectangle rect) {

            //if (!page_bounds.IntersectsWith(rect))
            //    return null;

            /*       In case of partial elements        */

            var page_top = page_bounds.Top;
            var rect_top = rect.Top;

            if (page_top > rect_top)
                rect = rect.Translate(0, page_top - rect_top);

            // ****************************

            if (!page_bounds.IntersectsWith(rect))
                return null;

            return page_bounds.Intersection(rect);
        }
        /// <summary>
        /// This method returns the bounds within which this frame will draw
        /// the IRenderable it contains.
        /// "covered" = the length of the document that is already printed, i.e.
        /// the total height of all printed pages so far excluding the current one
        /// "page_bounds" = the location and size of the allowed printing bounds.
        /// </summary>
        public abstract Rectangle? Get_Rectangle(Print_Engine engine, int covered, Rectangle page_bounds);


        public abstract Rectangle? Get_Absolute_Rectangle(int covered, Rectangle page_bounds);
    }

    /// <summary>
    /// This frame has an absolute location in the document. As the document is printed and covered increases,
    /// the return values of Get_Rectangle 
    /// </summary>
    public class Absolute_Frame : Print_Frame
    {
        public static Absolute_Frame Default(int x_size, int y_size) {
            return new Absolute_Frame(0, 0, x_size, y_size);
        }
        readonly int x_offset;
        readonly int y_offset;
        readonly Size sz;

        public override Rectangle? Get_Rectangle(Print_Engine _, int covered, Rectangle page_bounds) {
            return Intersect_Or_Null(page_bounds, Get_Absolute_Rectangle(covered, page_bounds).Value);
        }


        public override Rectangle? Get_Absolute_Rectangle(int covered, Rectangle page_bounds) {

            var loc = page_bounds.Location.Translate(x_offset, y_offset - covered);

            var ret = new Rectangle(loc, sz);

            return ret;

        }


        public Absolute_Frame(int x_offset, int y_offset, int x_size, int y_size)
            : this(x_offset, y_offset, new Size(x_size, y_size)) {
        }

        public Absolute_Frame(int x_offset, int y_offset, Size sz) {

            this.x_offset = x_offset;
            this.y_offset = y_offset;
            this.sz = sz;

        }
    }

    public class Relative_Frame : Print_Frame
    {
        readonly Func<Rectangle, Rectangle, Rectangle> m_transform;
        readonly Print_Frame m_relative_to;

        readonly Dictionary<Pair<int, Rectangle>, Rectangle?>
        memo;



        /// <summary>
        /// Candidate for optimization - the translations only need to be calculated once.
        /// </summary>
        public override Rectangle? Get_Rectangle(Print_Engine engine, int covered, Rectangle page_bounds) {

            var ret = Get_Absolute_Rectangle(covered, page_bounds);

            if (ret != null)
                ret = Intersect_Or_Null(page_bounds, ret.Value);

            return ret;

        }

        public Relative_Frame(Print_Frame relative_to, Func<Rectangle, Rectangle, Rectangle> transform) {

            m_relative_to = relative_to;
            m_transform = transform;
            memo = new Dictionary<Pair<int, Rectangle>, Rectangle?>(10);
        }



        public Relative_Frame(Print_Frame relative_to, Endo<Rectangle> transform)
            : this(relative_to, (relative_rectangle, _) => transform(relative_rectangle)) {
        }

        public override Rectangle? Get_Absolute_Rectangle(int covered, Rectangle page_bounds) {

            var pair = Pair.Make(covered, page_bounds);
            var ret = memo.Get_Or_Default(pair, () => m_relative_to.Get_Absolute_Rectangle(covered, page_bounds));

            ret = ret.OrDefault(_rect => m_transform(_rect.Value, page_bounds), null);

            if (ret == null)
                memo.Remove(pair);

            return ret;

        }


    }

    public class Page_Bound_Frame : Print_Frame
    {
        readonly Func<int, bool> m_predicate;
        readonly Endo<Rectangle> m_transform;

        public Page_Bound_Frame(Func<int, bool> predicate, Rectangle location_on_page)
            : this(predicate, page_bounds => location_on_page.Translate(page_bounds.Location, true)) {

        }

        public Page_Bound_Frame(Func<int, bool> predicate, Endo<Rectangle> transform) {

            transform.tifn<ArgumentNullException>("transform");
            predicate.tifn<ArgumentNullException>("predicate");

            m_transform = transform;
            m_predicate = predicate;
        }

        public override Rectangle? Get_Absolute_Rectangle(int covered, Rectangle page_bounds) {

            var ret = Get_Rectangle(covered, page_bounds);

            if (ret != null)
                ret = ret.Value.Translate(0, -covered);

            return ret;

        }

        public override Rectangle? Get_Rectangle(Print_Engine engine, int covered, Rectangle page_bounds) {

            if (!m_predicate(engine.Current_Page.Value))
                return null;

            return Get_Rectangle(covered, page_bounds);
        }

        Rectangle? Get_Rectangle(int covered, Rectangle page_bounds) {

            var rect = m_transform(page_bounds);
            var ret = Intersect_Or_Null(page_bounds, rect);

            return ret;

        }
    }

}












