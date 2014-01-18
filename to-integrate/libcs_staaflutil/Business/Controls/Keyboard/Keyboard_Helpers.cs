using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Fairweather.Service;

namespace Common.Controls
{
    public partial class Keyboard : Panel
    {

        public struct Key
        {
            public Key(string  input) : this(input, input) { }

            public Key(string  input, Size size) : this(input, input, size) { }

            public Key(string name, string  input) : this(name, input, input.ToUpper(), st_default_size, false) { }


            public Key(string name, string  input, Size size) : this(name, input, input, size) { }

            public Key(string name, string  input, Size size, bool toggleable) : this(name, input, input, size, toggleable) { }



            public Key(string name, string  input1, string  input2) : this(name, input1, input2, st_default_size) { }

            public Key(string name, string  input1, string  input2, Size size)
                : this(name, input1, input2, size, false) {
            }
            public Key(string name, string  input1, string  input2, Size size, bool toggleable)
                : this() {

                this.Name = name;
                this.Input_1 = input1;
                this.Input_2 = input2;
                this.Size = size;
                this.Toggleable = toggleable;


            }

            static readonly Size st_default_size = new Size(key_def_width, key_def_height);

            public bool Toggleable { get; private set; }
            public Size Size { get; private set; }
            public string Name { get; private set; }
            public string  Input_1 { get; private set; }
            public string  Input_2 { get; private set; }



        }

        public struct Keyboard_Key
        {
            public override int GetHashCode() {

                int ret = this.Name.GetHashCode() ;

                return ret;
            }
            public Keyboard_Key(Key key, Point location, int position)
                : this() {

                Position = position;
                Name = key.Name;
                Bounds = new Rectangle(location, key.Size);
                Key = key;
   

            }


            public bool Toggleable {
                get{ return Key.Toggleable ;}
            }
            public Key Key {
                get;
                private set;
            }
            public String Name {
                get;
                private set;
            }

            public RectangleF Bounds {
                get;
                private set;
            }

            public int Position {
                get;
                private set;
            }
        }

        public static class KeyboardManager
        {
            // Processes a list of keys and returns an array of Keyboard keys
            // with the specified layout
            // Key.None means an empty space with the specified dimensions
            public static Keyboard_Key[]
                Create_Keyboard_Layout(Key[] keys,

                                       Pair<int, Point>[] row_data,     // Length and location;
                // Length of zero means unlimited number
                                       Dictionary<int, int> key_distances, // Index and offset from left key or row border
                                       int def_key_distance,
                                       Point location
                                       ) {

                var ret = new List<Keyboard_Key>(keys.Count());

                bool zero_row_seen = false;
                int current_row = 0;

                int x_loc;
                int y_loc;
                int x_step;
                int row_keys;
                int dummies = 0;
                {
                    Pair<int, Point> row = row_data[0];

                    row_keys = row.First;
                    zero_row_seen = row.First == 0;
                    x_loc = row.Second.X;
                    y_loc = row.Second.Y;
                }
                int max_x = x_loc;
                for (int ii = 0, col = 0; ii < keys.Count(); ++ii, ++col) {

                    if (!zero_row_seen && col == row_keys) {

                        var new_row = row_data[++current_row];

                        x_loc = new_row.Second.X;
                        y_loc = new_row.Second.Y;

                        // Last row
                        if (new_row.First == 0) {

                            zero_row_seen = true;
                            continue;
                        }

                        // Next row
                        col = 0;
                        row_keys = new_row.First;
                    }

                    var key = keys[ii];
                    if (key.Input_1 == null)
                        ++dummies;
                    else
                        ret.Add(new Keyboard_Key(key, new Point(x_loc, y_loc), ii - dummies));

                    if (!key_distances.TryGetValue(ii, out x_step))
                        x_step = def_key_distance;

                    x_loc += key.Size.Width;
                    x_loc += x_step;
                    max_x = Math.Max(x_loc, max_x);
                }

                return ret.ToArray();
            }


        }
    }
}