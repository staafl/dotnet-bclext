using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{

    using Quad = Quad<int, int, int, int>;

    public partial class Border
    {
        static readonly Quad default_offsets = new Quad(1, 1, 1, 1);
        static readonly Color default_color = Color.Black;
        static readonly int default_width = 1;
        static readonly Pen default_pen = new Pen(default_color, default_width);

        static public Border Create(Control owner) {

            var border = new Border(owner);
            return border;


        }

        static public Border Create(Control owner,
                                  Quad offsets) {

            var border = new Border(owner, offsets, def_host_options);
            return border;


        }

        static public Border Create(Control owner,
                                  Quad offsets,
                                  Border_Options options) {

            var border = new Border(owner, offsets, options);
            return border;

        }


        const int either_mode = 0x40000;
        const int host_mode = 0x10000;
        const int free_mode = 0x20000;

        enum Mode
        {
            Free = 1,
            Host = 2,
        }
        readonly Mode m_mode;

        const Border_Options def_host_options = Border_Options.Mimic_Host_Visibility;
        const Border_Options def_free_options = Border_Options.Transparent_Color;

        public enum Border_Options
        {
            None = 0,
            Transparent_Color = 0x1 | either_mode,

            Mimic_Host_Back_Color = 0x2 | host_mode,
            Mimic_Host_Visibility = 0x3 | host_mode,

            //Grey_Out_With_Host = 0x4 | host_mode,
            //Disappear_When_Host_Disabled = 0x5 | host_mode,
        }

        static void Verify_Mode(Mode mode, Border_Options options) {

            if (mode == Mode.Free)
                options.Contains(host_mode).tift("host");

            else if (mode == Mode.Host)
                options.Contains(free_mode).tift("free");

            else
                throw new InvalidEnumArgumentException("Mode");
        }
    }
}