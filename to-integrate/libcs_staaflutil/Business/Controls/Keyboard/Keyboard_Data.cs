using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Common.Controls
{
    public partial class Keyboard
    {
        #region KEY DEFINITIONS - 4 rows

        static readonly Key[] st_keys_0 = { new Key("Esc", "{ESC}", new Size(key_def_width + 11, key_def_height)),

                                            new Key("!", "!","~"),
                                            new Key("@", "@","`"),
                                            new Key("#", "#","€"),
                                            new Key("$", "$","¢"),
                                            new Key("%", "%","№"),
                                            new Key("^", "^","£"),
                                            new Key("&", "&","¥"),
                                            new Key("*", "*","¤"),
                                            new Key("(", "(","™"),
                                            new Key(")", ")","©"),

                                            new Key("-", "-","_"),
                                            new Key("=", "=","+"),

                                            new Key("<-","{BKSP}"),

                                            new Key(null, new Size(4, key_def_height)),

                                            // new Key("", 0, new Size(272, key_def_height)),
                                            new Key("Del", "{DEL}", new Size(key_def_width + 11, key_def_height)),                             

                                          };
        static readonly Key[] st_keys_1 = { new Key("Tab", "{TAB}", new Size(key_def_width + 11, key_def_height)), 
                                            new Key("q"), 
                                            new Key("w"), 
                                            new Key("e"), 
                                            new Key("r"), 
                                            new Key("t"), 
                                            new Key("y"), 
                                            new Key("u"), 
                                            new Key("i"), 
                                            new Key("o"), 
                                            new Key("p"), 
                                            new Key("[","[","{"),
                                            new Key("]","]","}"),  
                                            new Key("\\", "\\", "|"),

                                            new Key(null, new Size(4, key_def_height)),
                                            new Key("Home", "{HOME}", new Size(key_def_width + 11, key_def_height)),
                                          };

        static readonly Key[] st_keys_2 = { new Key("Caps", "{CAPSLOCK}", new Size(key_def_width + 20, key_def_height), true),
                                            new Key("a"), 
                                            new Key("s"), 
                                            new Key("d"), 
                                            new Key("f"), 
                                            new Key("g"), 
                                            new Key("h"), 
                                            new Key("j"), 
                                            new Key("k"), 
                                            new Key("l"), 
                                            new Key(";", ";", ":"),  
                                            new Key("'", "'",  "\""),   
                                            new Key("Enter", "{ENTER}", new Size(key_def_width + 35, key_def_height)), 

                                            new Key(null, new Size(4, key_def_height)),
                                            new Key("PgUp","{PGUP}", new Size(key_def_width + 11, key_def_height)),

                                          };

        static readonly Key[] st_keys_3 = { new Key("Shift", "", new Size(key_def_width + 30, key_def_height), true),
                                            new Key("z"), 
                                            new Key("x"), 
                                            new Key("c"), 
                                            new Key("v"), 
                                            new Key("b"), 
                                            new Key("n"), 
                                            new Key("m"), 
                                            new Key(",",",", "<"), 
                                            new Key(".",".", ">"), 
                                            new Key("/", "/", "?"), 
                                            new Key("^", "{UP}"), 
                                            new Key("Shift", "", new Size(key_def_width + 25, key_def_height), true),
                                            new Key(null,  new Size(4, key_def_height)),
                                            new Key("PgDn", "{PGDN}", new Size(key_def_width + 11, key_def_height)),

                                          };

        static readonly Key[] st_keys_4 = { new Key("Ctrl", "", new Size(key_def_width + 11, key_def_height), true), 
                                            new Key(null, new Size(key_def_width - 15, key_def_height)),
                                            new Key("Alt", "", new Size(key_def_width + 11, key_def_height), true),
                                            new Key(" ", new Size(key_def_width * 8 + 7, key_def_height)),
                                            new Key("<=", "{LEFT}"), 
                                            new Key("DOWNARROW", "{DOWN}"), 
                                            new Key("=>", "{RIGHT}"),
                                            new Key(null,  new Size(29, key_def_height)),
                                            new Key("End", "{END}", new Size(key_def_width + 11, key_def_height)),

                                          };
        #endregion

        #region ADDITIONAL DEFINITIONS AND CONSTANTS - 6 Lines
        static readonly Dictionary<string, int> m_text_offsets = new Dictionary<string, int>() {
            {"Tab", 6},
            {"Caps",6},
            {"Shift", 6},
            {"Ctrl", 6},
        };

        static readonly int[] row_lengths = { st_keys_0.Length, 
                                              st_keys_1.Length, 
                                              st_keys_2.Length, 
                                              st_keys_3.Length,
                                              st_keys_4.Length, };

        static readonly Point[] row_locations = { new Point(cst_row_left, cst_row_top), 
                                                  new Point(cst_row_left, cst_row_top + 40), 
                                                  new Point(cst_row_left, cst_row_top + 40*2), 
                                                  new Point(cst_row_left, cst_row_top + 40*3),
                                                  new Point(cst_row_left, cst_row_top + 40*4),};

        const int key_def_width = 40;
        const int key_def_height = 36;
        const int cst_row_top = 11;
        const int cst_row_left = 11;
        const int cst_border_width = 1;

        #endregion

    }
}
