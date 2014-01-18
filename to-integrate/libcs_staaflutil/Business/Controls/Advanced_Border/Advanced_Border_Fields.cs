using System.Drawing;
using System.Windows.Forms;


using Fairweather.Service;

namespace Common.Controls
{

    using Quad = Quad<int, int, int, int>;

    public partial class Border
    {
        readonly Quad? m_offsets;
        readonly Border_Options m_options;

        public Border_Options Options {
            get { return m_options; }
        }

        Pen pen;

        readonly Control m_host;

        public Color Border_Color {
            get {
                var ret = pen.Color;
                return ret;
            }
            set {
                if (value == Border_Color)
                    return;

                pen = new Pen(value, Border_Width);

            }
        }

        public int Border_Width {
            get { return (int)pen.Width; }
            set {
                if (value == Border_Width)
                    return;

                pen = new Pen(Border_Color, value);


            }
        }

        public Control Host {
            get { return m_host; }
        }

    }
}