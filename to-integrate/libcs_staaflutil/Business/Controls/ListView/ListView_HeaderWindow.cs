using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;


using Fairweather.Service;

namespace Screens
{
      /// <summary>  A ListView with customizable header color and 
      /// nonclickable headers
      /// </summary>
      public partial class Our_ListView : ListView
      {
            HeaderWindow mHeader;

            public bool Prevent_Click { get; set; }
            public bool Prevent_Double_Click { get; set; }
            public bool Prevent_Cursor_Change { get; set; }


            class HeaderWindow : NativeWindow
            {

                  readonly Action m_on_mouse_down = () => { };
                  readonly Action m_on_mouse_up = () => { };

                  readonly Func<bool> prevent_cursor_change;
                  readonly Func<bool> prevent_double_click;
                  readonly Func<bool> prevent_click;

                  //Our_ListView m_host;

                  public HeaderWindow(Our_ListView listview) {

                        // m_host = listview;
                        prevent_cursor_change = () => listview.Prevent_Cursor_Change;
                        prevent_double_click = () => listview.Prevent_Double_Click;
                        prevent_click = () => listview.Prevent_Click;
                  }

                  // Instead of a specific action for a particular event,
                  // future versions of this class could take a generic
                  // action which forwards the message in question to the
                  // host control
                  public HeaderWindow(Our_ListView listview,
                                      Action on_mouse_down,
                                      Action on_mouse_up)
                        : this(listview) {

                        m_on_mouse_down = on_mouse_down;
                        m_on_mouse_up = on_mouse_up;

                  }
                  protected override void WndProc(ref Message m) {

                        if (m.Msg == Native_Const.WM_LBUTTONDOWN) {

                              if (prevent_click())
                                    return;

                              m_on_mouse_down();
                        }

                        if (m.Msg == Native_Const.WM_LBUTTONUP) {

                              if (prevent_click())
                                    return;

                              m_on_mouse_up();
                        }

                        if (m.Msg == WM_LBUTTONDBLCLICK) {

                              if (prevent_double_click()) {

                                    // Prevent double-click from changing column
                                    return;
                              }
                        }

                        if (m.Msg == WM_SETCURSOR) {

                              if (prevent_cursor_change()) {
                                    // Prevent cu
                                    // rsor from changing
                                    m.Result = (IntPtr)1;
                                    return;
                              }
                        }

                        base.WndProc(ref m);
                  }
            }

            // P/Invoke declarations

            //{WM_[A-Z]+} =[^;]+;
            // \1 = Native_Constants.\1;     
            const int WM_CREATE = Native_Const.WM_CREATE;
            const int WM_DESTROY = Native_Const.WM_DESTROY;
            const int WM_SETCURSOR = Native_Const.WM_SETCURSOR;
            const int WM_MOUSEWHEEL = Native_Const.WM_MOUSEWHEEL;
            const int WM_NOTIFY = Native_Const.WM_NOTIFY;
            const int WM_LBUTTONDBLCLICK = Native_Const.WM_LBUTTONDBLCLK;

            const int LVM_GETHEADER = 0x101f;
            const int HDN_FIRST = -300;
            const int HDN_BEGINTRACK = HDN_FIRST - 26;

            [StructLayout(LayoutKind.Sequential)]
            struct NMHDR
            {
                  public int hWndFrom;
                  public int idFrom;
                  public int code;
            }
      }
}
