using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Fairweather.Service;
namespace Common.Controls
{
    // IDEAS:
    // If we need to create a new keyboard, all we're going to need to do is encapsulate the logic 
    // in an abstract class and provide the layout information in the child class
    public partial class Keyboard : Panel
    {
        public Keyboard() {


            this.SetStyle(ControlStyles.ContainerControl |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.Opaque |
                          ControlStyles.UserPaint,
                          true);

            this.SetStyle(ControlStyles.Selectable,
                          false);

            UpdateStyles();

            var row_data = row_lengths.Zip_With(row_locations,
                                          (ii, pt) => new Pair<int, Point>(ii, pt))
                                      .ToArray();

            var st_keys = Extensions.Concat_Arrays(st_keys_0,
                                                   st_keys_1,
                                                   st_keys_2,
                                                   st_keys_3,
                                                   st_keys_4);

            m_layout = KeyboardManager.Create_Keyboard_Layout(st_keys,
                                                                row_data,
                                                                new Dictionary<int, int>(0),
                                                                4,
                                                                Point.Empty);
            Set_Defaults();

            Prepare_Text_Layout();

            b_init = true;
        }

       
        readonly static Set<string> m_toggled = new Set<string>();

        // Represents the keys, their textual representation, values and location
        readonly Keyboard_Key[] m_layout;

#pragma warning disable
        readonly bool b_init;
#pragma warning restore



        bool bf_style_changed;

        protected override void OnStyleChanged(EventArgs e) {

            if (bf_style_changed)
                return;
            try {
                bf_style_changed = false;
                this.SetStyle(ControlStyles.Selectable, false);
            }
            finally {
                bf_style_changed = true;
            }

            base.OnStyleChanged(e);
        }

        protected override void OnEnter(EventArgs e) {
            base.OnEnter(e);
        }


        /// <summary>  To be called upon creation
        /// </summary>
        partial void Set_Defaults();

        /// <summary>  To be called on creation and when the font is changed
        /// </summary>
        partial void Prepare_Text_Layout();

    }

}
