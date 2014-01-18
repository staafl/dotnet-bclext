using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Fairweather.Service;

namespace Common.Controls
{

    using Quad = Quad<int, int, int, int>;

    public partial class Border : UserControl
    {
        static readonly List<Border> list_of_null = new
        List<Border> { null };
        static readonly Dictionary<Control, List<Border>>
              instances = new Dictionary<Control, List<Border>>();

        static public List<Border> Instances(Control host) {
            return instances.Get_Or_Default(host, () => new List<Border>());
        }
        static public Border First_Instance(Control host) {
            return instances.Get_Or_Default(host, () => list_of_null)[0];
        }



        /*       Main constructor        */
        /*       Don't forget to set the m_mode field        */

        Border(Mode mode, Border_Options options) {

            Verify_Mode(mode, options);

            m_mode = mode;

            m_options = options;

            pen = default_pen;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.SupportsTransparentBackColor,
                          true);

            this.SetStyle(ControlStyles.Selectable,
                          false);

            this.Force_Handle();

            if (m_options.Contains(Border_Options.Transparent_Color))
                this.BackColor = Color.Transparent;



        }



        /*       Call the main constructor        */

        public Border()
            : this(Mode.Free, def_free_options) { }

        public Border(Border_Options options) : this(Mode.Free, options) { }


        /*       Calls ctor(control, quad, options)        */

        public Border(Control host)
            : this(host, default_offsets, def_host_options) { }

        /*       Calls main constructor        */

        public Border(Control host,
                               Quad offsets,
                               Border_Options options)
            : this(Mode.Host, options) {

            m_host = host;
            m_offsets = offsets;
            m_options = options;

            host.ParentChanged += host_ParentChanged;
            host.SizeChanged += host_BoundsChanged;
            host.Move += host_BoundsChanged;
            host.Paint += new PaintEventHandler(host_Paint);

            instances.Get_Or_Default(host, () => new List<Border>()).Add(this);

            var as_combobox = host as ComboBox;
            if (as_combobox != null) {
                as_combobox.DropDownClosed += (as_combobox_DropDownClosed);
            }


            EventHandler host_disposed = (sender, e) =>
            {
                if (!this.IsDisposed)
                    Dispose();
            };


            if (options.Contains(Border_Options.Mimic_Host_Back_Color)) {

                EventHandler host_back_color_changed = (sender, e) =>
                {
                    Mimic_Color();
                };

                host.BackColorChanged += host_back_color_changed;
                Mimic_Color();

            }

            if (options.Contains(Border_Options.Mimic_Host_Visibility)) {

                EventHandler host_visible_changed = (sender, e) =>
                {
                    Mimic_Visibility();
                };

                host.VisibleChanged += host_visible_changed;
                Mimic_Visibility();

            }

            Refresh_Parent(false);
            Refresh_Border();

        }


        void host_Paint(object sender, PaintEventArgs e) {
            this.Invalidate();
        }

        void as_combobox_DropDownClosed(object sender, EventArgs e) {
            this.Refresh();
        }

        void Mimic_Visibility() {

            Verify_Mode(Mode.Host);

            if (this.Visible != m_host.Visible) {
                this.Visible = m_host.Visible;

                this.Invalidate();

                if (this.Visible)
                    m_host.BringToFront();
            }


        }

        void Verify_Mode(Mode mode) {

            (mode != this.m_mode).tift();

        }


    }
}
