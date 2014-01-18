using System;
using System.Diagnostics;
using System.Drawing;

namespace Fairweather.Service
{
    [DebuggerDisplay("Text: {Text},nq")]
    public class Primitive_Text : Primitive_Base
    {
        public Primitive_Text(Endo<Rectangle> intern, Font font, Brush brush, Func<string> text)
            : base(intern, font, brush, null) {
            m_text = text;
            Format = new StringFormat(base_format);
        }

        public Primitive_Text(Endo<Rectangle> intern, Font font, Brush brush, string text)
            : this(intern, font, brush, () => text) {
        }

        public Primitive_Text(string text)
            : this(null, null, null, () => text) {
        }

        public Primitive_Text(Func<string> text)
            : this(null, null, null, text) {
        }


        /// <summary>
        /// A new instance is returned every time
        /// </summary>
        public static StringFormat Base_Format {
            get { return new StringFormat(Primitive_Text.base_format); }
        }




        public StringFormat Format {
            get;
            set;
        }

        public string Text {
            get {
                return m_text();
            }
        }

        readonly Func<string> m_text;


        static readonly StringFormat base_format = StringFormat.GenericTypographic.Near_Aligned_H(true).Near_Aligned_V(true);


        protected override void Draw(Brush brush, Pen _, Font font, Graphics g, Rectangle rect) {

            g.DrawString(Text, font, brush, rect, Format);


        }

    }
}
