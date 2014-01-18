using System.Collections.Generic;

using Fairweather.Service;

namespace Fairweather.Service
{
      //[DebuggerStepThrough]
      public sealed class Pos_Bixolon_Print_Font : Pos_Print_Font
      {
            readonly bool m_color;
            readonly Pair<int> m_size;

            public Pair<int> Size {
                  get { return m_size; }
            }


            public bool Color {
                  get { return m_color; }
            }





            static Dictionary<Bixolon_Font_Type, string> dict_strings =
                  new Dictionary<Bixolon_Font_Type, string>();

            static string Get_String(Bixolon_Font_Type type) {

                  var ret = dict_strings.Get_Or_Default(type, () => type.Get_String());

                  return ret;
            }

            static void Get_Info(Bixolon_Font_Type type, out Pair<int> size, out bool type_a) {

                  var str = Get_String(type);

                  var substring = str.Substring("Font".Length);

                  var ch_type = substring[0];
                  var ch_width = substring[1];
                  var ch_height = substring[3];
                  type_a = (ch_type == 'A');

                  int horizontal_fact = type_a ? 12 : 9;
                  int vertical_fact = type_a ? 24 : 17;
                  int width = int.Parse(ch_width.ToString()) * horizontal_fact;
                  int height = int.Parse(ch_height.ToString()) * vertical_fact;

                  size = Pair.Make(width, height);

            }

            public Pos_Bixolon_Print_Font(Bixolon_Font_Type type,
                float height)
                  : this(type, height, false, false) { }

            public Pos_Bixolon_Print_Font(Bixolon_Font_Type type,
                        float height,
                        bool bold,
                        bool color)
                  : base(Get_String(type), height, bold, Print_Font_Type.Bixolon_Font) {

                  this.m_color = color;
                  bool _;
                  Get_Info(type, out m_size, out _);

            }


      }


}
