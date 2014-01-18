using System;
using System.Diagnostics;
using System.Drawing;

namespace Fairweather.Service
{
    public abstract class Primitive_Base : IRenderable
    {


        protected Primitive_Base(Endo<Rectangle> intern, Font font, Brush brush, Pen pen) {

            this.Font = font;
            this.Brush = brush;
            this.Pen = pen;
            this.intern = intern.OrDefault(() => _ => _);

        }


        public void Draw(Print_Engine _1, Print_Document document, Graphics g, int _2, Rectangle rect) {

            var to_draw = intern(rect).Intersection(rect);

            Draw(Brush ?? document.Default_Brush,
                 Pen ?? document.Default_Pen,
                 Font ?? document.Default_Font,
                 g, to_draw);

        }

        public Font Font {
            get;
            private set;
        }

        public Brush Brush {
            get;
            private set;
        }

        public Pen Pen {
            get;
            private set;
        }




        protected abstract void Draw(Brush brush, Pen pen, Font font, Graphics g, Rectangle rect);



        readonly Endo<Rectangle> intern;



    }
}
