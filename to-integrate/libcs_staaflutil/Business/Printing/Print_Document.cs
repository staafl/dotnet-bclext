
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Fairweather.Service
{
    public delegate IRenderable Header_Maker(Print_Engine engine, Print_Document doc);

    /// <summary>
    /// Call this delegate with the printing information to receive
    /// the printing delegates for the next page.
    /// </summary>
    public delegate Pair<List<Action<Graphics>>, bool>
    Print_Producer(Rectangle page_margins, Print_Engine engine);



    /// <summary>
    /// The topmost element in the document hierarchy.
    /// Add elements using Add_Element(Print_Element) and
    /// then get a Print_Producer using Get_Printer();
    /// </summary>
    public class Print_Document
    {
        const int cst_grace = 5;


        public static Print_Frame Default_Frame {
            get {
                return new Absolute_Frame(0, 0, Size.Empty);
            }
        }
        static readonly IRenderable empty = new Empty_IRenderable();

        public Print_Document() {

            Header = (_1, _2) => empty;
            Footer = (_1, _2) => empty;
            Name = "";

            Default_Font = new Font("Arial", 10);
            Default_Brush = new SolidBrush(Color.Black);
            Default_Pen = Pens.Black;

            //header.Add_Text("Report");
            //header.Add_Text("Page: [pagenum]");
            //header.Add_Horizontal_Rule();
            //header.Add_Blank_Line();

            //// create the footer...
            //footer = new Print_Element();
            //footer.Add_Blank_Line();
            //footer.Add_Horizontal_Rule();
            //footer.Add_Text("Confidential");

        }


        readonly List<Print_Element>
        m_elements = new List<Print_Element>();


        /// <summary>
        /// Retrieves a delegate to use when printing.
        /// Every successive time it is called it returns a list
        /// of printer methods relevant to the page.
        /// </summary>
        /// <returns></returns>
        public Print_Producer Get_Printer() {

            var elems = new List<Print_Element>(m_elements);

            int covered = 0;

            return
                (page_margins, engine) =>
                {

                    elems.Is_Empty().tift();

                    var list = new List<Action<Graphics>>();

                    var footer = Footer(engine, this);
                    var header = Header(engine, this);

                    int head_height = Header_Height;
                    int foot_height = Footer_Height;

                    var rectangle = page_margins;

                    var head_bottom = (int)(rectangle.Top + head_height);
                    var foot_top = (int)(rectangle.Bottom - foot_height);

                    var head_rect = rectangle.Set_Edge(3, head_bottom);
                    var foot_rect = rectangle.Set_Edge(1, foot_top);


                    // ****************************

                    list.Add(g1 => header.Draw(engine, this, g1, 0, head_rect));
                    list.Add(g1 => footer.Draw(engine, this, g1, 0, foot_rect));

                    // ****************************


                    rectangle = rectangle.Set_Edge(1, head_bottom).Set_Edge(3, foot_top);

                    elems.Sort(element =>
                    {
                        var null_rect = element.Frame.Get_Rectangle(engine, covered, rectangle);
                        if (null_rect.HasValue)
                            return null_rect.Value.Top;
                        return Int32.MaxValue;
                    });


                    var page_bottom = rectangle.Bottom;

                    var local_covered = covered;

                    int index = 0;
                    while (index < elems.Count) {

                        var elem = elems[index];

                        var null_rect = elem.Frame.Get_Rectangle(engine, local_covered, rectangle);

                        if (!null_rect.HasValue) {
                            ++index;
                            continue;
                        }

                        var print_in = null_rect.Value;
                        var print_in_top = print_in.Top;
                        var print_in_bottom = print_in.Bottom;

                        // Run out of space?
                        if (page_bottom - print_in_top < 1) {
                            ++index;
                            continue;
                        }

                        //// Enough space to print the entire thing (or close)?
                        //if (page_bottom - print_in_bottom <= -cst_grace) {
                        elems.RemoveAt(0);
                        //}
                        //// Skip to the next one
                        //else {
                        //++index;
                        //}

                        list.Add(g1 =>
                        {
                            elem.Draw(engine, this, g1, local_covered, rectangle);
                        });

                    }

                    covered += rectangle.Height;

                    return Pair.Make(list, !elems.Is_Empty());
                };
        }

        /// <summary>
        /// Removes all visual elements from the document.
        /// </summary>
        public void Clear() {
            m_elements.Clear();
        }


        /// <summary>
        /// Adds a visual element to the document.
        /// </summary>
        public void Add_Element(Print_Element elem) {
            m_elements.Add(elem);
        }


        public int Header_Height {
            get;
            set;
        }

        public int Footer_Height {
            get;
            set;
        }


        public Header_Maker Header { get; set; }

        public Header_Maker Footer { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the default pen to use when printing.
        /// </summary>
        public Pen Default_Pen {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default font to use when printing.
        /// </summary>
        public Font Default_Font {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default brush to use when printing.
        /// </summary>
        public Brush Default_Brush {
            get;
            set;
        }



    }




}












