using System;
// http://www.devarticles.com/c/a/C-Sharp/Printing-Using-C-sharp/
using System.Diagnostics;
using System.Drawing;

namespace Fairweather.Service
{
    [DebuggerDisplay("Image: {m_producer()},nq")]
    public class Primitive_Image : Primitive_Base
    {
        readonly Func<Image> m_producer;
        readonly Pen m_border;


        public Primitive_Image(Image image)
            : this(null, () => image, null) { }

        public Primitive_Image(Image image, Pen border)
            : this(null, () => image, border) { }

        public Primitive_Image(Endo<Rectangle> intern, Image image, Pen border)
            : this(intern, () => image, border) { }

        protected override void Draw(Brush _0, Pen _1, Font _2, Graphics g, Rectangle rect) {

            g.DrawImage(m_producer(), rect);
            if (m_border != null)
                g.DrawRectangle(m_border, rect);

        }

        public Primitive_Image(Endo<Rectangle> intern, Func<Image> producer, Pen border)
            : base(intern, null, null, null) {

            (producer).tifn<ArgumentNullException>("producer");

            m_producer = producer;
            m_border = border;
        }
    }





}












