
using System.Drawing;
using System.Windows.Forms;
using Fairweather.Service;

namespace Common.Controls
{
    public partial class Keyboard : Panel
    {
        partial void Set_Defaults() {

            Key_Color = Color.Brown;
            Back_Color = Color.Black;
            Text_Color = Color.Black;
            Border_Color = Color.Black;
            Key_Font = new Font("Tahoma", 32);

        }

        public bool Auto_Revert_Shift {
            get;
            set;
        }

        public Color Key_Color {
            get { return brush_key.Color; }

            set {
                if (brush_key != null && Key_Color == value)
                    return;

                brush_key.Try_Dispose();

                brush_key = new SolidBrush(value);
            }
        }

        public Color Text_Color {
            get { return brush_text.Color; }
            set {
                if (brush_text != null && Text_Color == value)
                    return;

                brush_text.Try_Dispose();

                brush_text = new SolidBrush(value);

            }
        }

        public Color Back_Color {
            get { return brush_back.Color; }
            set {
                if (brush_back != null && Back_Color == value)
                    return;

                brush_back.Try_Dispose();

                brush_back = new SolidBrush(value);
            }
        }

        public Color Border_Color {
            get { return pen_border.Color; }
            set {
                if (pen_border != null && Border_Color == value)
                    return;


                pen_border.Try_Dispose();

                pen_border = new Pen(value, cst_border_width);
            }
        }

        Font keyboard_font;
        public Font Key_Font {
            get { return keyboard_font; }
            set {
                if (Key_Font == value)
                    return;

                
                keyboard_font.Try_Dispose();

                keyboard_font = value;

                Prepare_Text_Layout();
            }
        }
    }
}
