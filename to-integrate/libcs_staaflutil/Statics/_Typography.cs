using System.Drawing;
using System.Linq;

namespace Fairweather.Service
{
      //[DebuggerStepThrough]
      public static class T
      {

            /// <summary>
            /// Returns a string of the form 012345678901234... with the specified length
            /// </summary>
            /// <param name="length"></param>
            /// <returns></returns>
            public static string Char_Counter(int length) {

                  var ret = Magic.range(0, 9).Repeat((length + 10) / 10).Flatten().Take(length).Pretty_Print("{0}");
                  return ret;

            }

            public static int How_Tall(string str, int char_count, Font font) {

                  var ret = T.How_Many_Lines(str, char_count) * font.Height;

                  return ret;

            }

            public static int How_Tall(Graphics g, string str, Font font, float line_width, GraphicsUnit unit) {

                  var ret = T.How_Many_Lines(g, str, font, line_width, unit) * font.Height;

                  return ret;

            }

            public static int How_Many_Lines(string str, int chars_per_line) {

                  if (str.IsNullOrEmpty())
                        return 0;

                  var len = str.Length;

                  var ret = ((len - 1) / chars_per_line) + 1;

                  return ret;


            }
            public static int How_Many_Lines(Graphics g, string str, Font font, float line_width, GraphicsUnit unit) {

                  var chars_per_line = Characters_Per_Line(g, font, line_width, unit);
                  var ret = How_Many_Lines(str, chars_per_line);

                  return ret;


            }

            public static string Ellipsis(Graphics g, string str, Font font, int width, GraphicsUnit unit, bool left) {


                  var chars = Characters_Per_Line(g, font, width, unit);

                  str = str.Ellipsis(chars, "...", left);

                  return str;


            }

            public static float String_Width(Graphics g, string str, Font font, GraphicsUnit unit) {

                  var prev = g.PageUnit;
                  float ret;
                  try {
                        g.PageUnit = unit;

                        ret = g.MeasureString(str, font).Width;

                  }
                  finally {
                        g.PageUnit = prev;
                  }

                  return ret;

            }

            public static int Characters_Per_Line(Graphics g, Font font, float line_width, GraphicsUnit unit) {

                  var width1 = String_Width(g, "x", font, unit);
                  var width2 = String_Width(g, "xx", font, unit);
                  var width3 = String_Width(g, "xxx", font, unit);

                  var diff = width3 - width2; // one char 
                  var over = width1 - diff;   // overhead

                  var fitted = (line_width - 2 * over) / diff;

                  // Test:
                  //var str = 'x'.Repeat((int)fitted);

                  //var length = g.MeasureString(str, font).Width;
                  //var as_mm = C.Pix_to_MM(length, g.DpiX);
                  //Console.WriteLine(as_mm);

                  return (int)fitted;

            }

            /// <summary>
            /// Returns the point at which the string needs to be drawn in order
            /// to be perfectly centered in the rectangle.
            /// </summary>
            public static Point Center_String(Graphics g, Font font, Rectangle rect, string str) {

                  str.tifn();


                  var sizef = g.MeasureString(str, font);

                  sizef = sizef.Scale(2, false);

                  var center = rect.CenterF();

                  var ret = center.Translate(sizef, false);

                  return Point.Round(ret);
            }
      }
}