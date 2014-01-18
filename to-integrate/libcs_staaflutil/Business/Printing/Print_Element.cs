

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace Fairweather.Service
{
    /// <summary>
    /// Represents a hierarchy of printable objects.
    /// </summary>
    [DebuggerDisplay("Frame: {Frame}, First Element = {Contents.First()},nq")] 
    public class Print_Element : IRenderable
    {
        readonly Print_Frame m_frame;

        public Print_Frame Frame {
            get { return m_frame; }
        }

        readonly SortedList<int, List<IRenderable>> m_contents;

        public IEnumerable<Pair<int, IRenderable>> Contents {
            get {
                return (from kvp in m_contents
                       let z_order = kvp.Key
                       let elems = kvp.Value
                       select from elem in elems
                              select Pair.Make(z_order, elem)).Flatten();
            }
        }


        public Print_Element(Print_Frame frame) : this(frame, null) { }


        public Print_Element(Print_Frame frame, params IRenderable[] contents) : this(frame, contents as IEnumerable<IRenderable>) { }

        public Print_Element(Print_Frame frame, IEnumerable<IRenderable> contents) {

            (frame).tifn<ArgumentNullException>("frame");

            m_frame = frame;

            var forced = contents.OrDefault(en => en.Force(), 
                                            () => null);

            var cnt = forced.OrDefault(a => (int?)a.Count, 
                                        () => null);

            m_contents = new SortedList<int, List<IRenderable>>(cnt ?? 1);

            if (cnt == null)
                return;

            int ii = 0;
            foreach (var elem in forced) {
                m_contents.Add(ii, new List<IRenderable>{elem});
                ii += 10;
            }

        }



        public void Draw(Print_Engine engine, Print_Document doc, Graphics g, int covered, Rectangle parent_bounds) {

            var rectangle = m_frame.Get_Rectangle(engine, covered, parent_bounds).Value;

            foreach (var pair in Contents) {
                var renderable = pair.Second;
                renderable.Draw(engine, doc, g, 0, rectangle);
            }

        }

        public bool Can_Draw(Print_Engine engine, int covered, Rectangle page_bounds) {
            return m_frame.Get_Rectangle(engine, covered, page_bounds).HasValue;
        }

        public void Add_Child(IRenderable renderable) {
            Add_Child(renderable, m_contents.Last().Key + 10);
        }

        public void Add_Child(IRenderable renderable, int z_order) {

            (renderable == this).tift();

            m_contents.Get_Or_New(z_order, true).Add(renderable);
        }

    }

    /*       Ancient code from the tutorial        */
    // http://www.devarticles.com/c/a/C-Sharp/Printing-Using-C-sharp/
    ///
    //public class Print_Composition : IRenderable
    //{
    //    public static Print_Composition Empty {
    //        get {
    //            return new Print_Composition();
    //        }
    //    }

    //    protected Print_Composition() {
    //    }

    //    List<IRenderable> m_primitives = new List<IRenderable>();

    //    public float Calculate_Height(Print_Document doc, Graphics g) {

    //        var ret = (from prim in m_primitives
    //                   select prim.Calculate_Height(doc, g)).Sum();

    //        return ret;

    //    }

    //    public void Draw(Print_Document doc, Graphics g, Rectangle rect) {

    //        var left = rect.Left;
    //        var width = rect.Width;

    //        float yy = rect.Top;

    //        var hh = Calculate_Height(doc, g);

    //        foreach (var primitive in m_primitives) {

    //            primitive.Draw(doc, g, rect.Set_Edge(1, (int)yy));

    //            yy += primitive.Calculate_Height(doc, g);

    //        }


    //    }

    //    protected void Add_Primitive(IRenderable primitive) {
    //        m_primitives.Add(primitive);
    //    }

    //    protected void Add_Text(string text) {

    //        Add_Primitive(new Primitive_Text(text));

    //    }

    //    protected void Add_Data(String dataName, String dataValue) {
    //        Add_Text("{0}: {1}".spf(dataName, dataValue));
    //    }

    //    protected void Add_Horizontal_Rule() {
    //        Add_Primitive(new Primitive_Rule());
    //    }

    //    protected void Add_Blank_Line() {
    //        Add_Text("");
    //    }

    //    protected void Add_Header(String buf) {
    //        Add_Text(buf);
    //        Add_Horizontal_Rule();
    //    }

    //}


}












