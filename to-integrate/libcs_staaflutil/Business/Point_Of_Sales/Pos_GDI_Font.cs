using System;
using System.Drawing;


using Fairweather.Service;

namespace Fairweather.Service
{
      public class Pos_GDI_Font : Pos_Print_Font
      {

            readonly Lazy<Font> font;

            public Font Font {
                  get { return font.Value; }
            }

            public Pos_GDI_Font(Func<Font> font)
                  : base("delayed", 0, false, Print_Font_Type.GDI) {
                  this.font = new Lazy<Font>(font);
            }

            //public Pos_GDI_Font(Lazy<Font> font)
            //      : base("delayed", 0, false, Print_Font_Type.GDI) {
            //      this.font = () => font.Force();
            //}

            public Pos_GDI_Font(Font font)
                  : base(font.Name, font.Height, font.Bold, Print_Font_Type.GDI) {
                  this.font = font;
            }

            public static explicit operator Pos_GDI_Font(Font font) {
                  return new Pos_GDI_Font(font);
            }
      }
}
