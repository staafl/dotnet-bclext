// http://www.devarticles.com/c/a/C-Sharp/Printing-Using-C-sharp/
using System.Drawing;

namespace Fairweather.Service
{

    public interface IRenderable
    {

        /// <summary>
        /// Instructs the IRenderable instance to draw itself using the Graphics object.
        /// Print_Element instances will subsequently draw their child elements.
        /// 
        /// "covered" -> the part of the element that has already been printed expressed as height in pixels
        /// "parent_bounds" -> the margins of the page or the bounds of the parent element
        /// </summary>
        void Draw(Print_Engine engine, Print_Document document, Graphics g, int covered, Rectangle parent_bounds);


    }

    class Empty_IRenderable : IRenderable
    {
        public Empty_IRenderable() { }

        public void Draw(Print_Engine __1, Print_Document _1, Graphics _2, int __2, Rectangle _3) { }

    }


}












