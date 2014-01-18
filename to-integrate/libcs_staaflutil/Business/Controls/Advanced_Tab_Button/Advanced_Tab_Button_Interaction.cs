using System;
using System.Drawing;

using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
      public partial class Advanced_Tab_Button : Button
      {
            bool m_is_activated;

            bool m_under_mouse;

            Point m_text_location;

            internal const int cst_left_expansion = 4;
            const int cst_top_expansion = 3;
            internal const int cst_right_expansion = 3;

            public static event EventHandler<EventArgs> Button_Activated;


            public Color Highlight_Color {
                  get {
                        return high_brush.Argument;
                  }
                  set {
                        high_brush.Argument = value;
                  }
            }

            public override Color BackColor {
                  get {
                        return back_brush.Argument;
                  }
                  set {
                        back_brush.Argument = value;
                  }
            }

            public Color Border_Color {
                  get {
                        var ret = m_border_color;
                        return ret;
                  }
                  set {
                        if (m_border_color != value) {
                              m_border_color = value;

                              border_pen.Try_Dispose();
                              border_pen = new Pen(value);
                        }
                  }
            }

            public Color Crown_Color {
                  get {
                        var ret = m_crown_color;
                        return ret;
                  }
                  set {
                        if (m_crown_color != value) {
                              m_crown_color = value;

                              crown_pen.Try_Dispose();
                              crown_pen = new Pen(m_crown_color);
                        }
                  }
            }




            public override string Text {
                  get {
                        return base.Text;
                  }
                  set {
                        if (base.Text != value) {


                              base.Text = value;
                              if (base.Text.IsNullOrEmpty())
                                    return;

                              Refresh_Text_Location();

                        }
                  }
            }

            void Refresh_Text_Location() {
                  m_text_location = this.Center_String(base.Text);

            }

            protected override void OnResize(EventArgs e) {

                  Refresh_Text_Location();
                  base.OnResize(e);

            }

            protected override void OnParentChanged(EventArgs e) {

                  (Parent is Advanced_Tab_Control || Parent == null).tiff();

                  this.m_tab_control = this.Parent as Advanced_Tab_Control;

                  base.OnParentChanged(e);

            }

            public void Activate() {
                  Refresh_Activated();
            }

            void Refresh_Activated() {

                  if (m_is_activated)
                        return;

                  m_is_activated = true;

                  var rect = this.Bounds.Expand(cst_left_expansion,
                                                cst_top_expansion,
                                                cst_right_expansion, 0);

                  this.Bounds = rect;
                  // OnResize

                  this.BringToFront();

                  this.Invalidate();

                  On_Button_Activated(EventArgs.Empty);

            }

            void Refresh_Deactivated() {

                  if (!m_is_activated)
                        return;

                  var rect = this.Bounds.Expand(-cst_left_expansion,
                                                -cst_top_expansion,
                                                -cst_right_expansion, 0);

                  this.Bounds = rect;
                  // OnResize

                  this.Invalidate();

            }

            void On_Button_Activated(EventArgs e) {

                  Button_Activated.Raise(this, e);



            }


            protected override void OnMouseEnter(EventArgs e) {

                  base.OnMouseEnter(e);
                  m_under_mouse = true;
                  this.Invalidate();

            }

            protected override void OnMouseLeave(EventArgs e) {

                  base.OnMouseLeave(e);
                  m_under_mouse = false;
                  this.Invalidate();


            }

            protected override void OnClick(EventArgs e) {

                  Refresh_Activated();

                  base.OnClick(e);

            }

            void Advanced_Tab_Button_Button_Activated(object sender, EventArgs e) {

                  var but = sender as Advanced_Tab_Button;

                  if (but.Tab_Control != this.Tab_Control)
                        return;

                  if (this == but)
                        return;

                  if (!m_is_activated)
                        return;

                  Refresh_Deactivated();
                  m_is_activated = false;

                  /*       15th September        */
                  // this.Invalidate();

            }

      }
}