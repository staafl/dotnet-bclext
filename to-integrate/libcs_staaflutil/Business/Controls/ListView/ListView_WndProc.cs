using System;
using System.Windows.Forms;


using Fairweather.Service;

namespace Screens
{
      public partial class Our_ListView : ListView
      {
            const int fit_to_header = -2;
            const int stretch_last = -2;
            const int fit_to_longest_string = -1;

            // "how to disable listview's column resize cursor image"
            // http://social.msdn.microsoft.com/Forums/en-US/winforms/thread/4d3280ed-0848-4151-8d6e-295576023406
            ///
            // Autosize the last column in a ListView control using WndProc
            // http://www.codeproject.com/KB/list/listviewautosize.aspx

            protected override void WndProc(ref Message m) {

                  var msg = m.Msg;

                  if (msg == Native_Const.WM_PAINT) {
                        if (Autosize_Last_Column)
                              if (this.View == View.Details && this.Columns.Count >= 1) {
                                    this.Columns[this.Columns.Count - 1].Width = stretch_last;
                              }
                  }

                  if (m.Msg == WM_DESTROY) {

                        if (mHeader != null)
                              mHeader.ReleaseHandle();

                        mHeader = null;
                  }

                  if (m.Msg == WM_NOTIFY) {

                        // Prevent dragging
                        NMHDR nm = (NMHDR)m.GetLParam(typeof(NMHDR));

                        if (nm.code == HDN_BEGINTRACK) {

                              m.Result = (IntPtr)1;
                              return;
                        }

                  }

                  if (m.Msg == WM_MOUSEWHEEL) {
                        Native_Methods.SendMessage(TopLevelControl.Handle,
                                                  (uint)m.Msg, (uint)m.WParam, (uint)m.LParam);
                        return;
                  }

                  base.WndProc(ref m);

                  if (m.Msg == WM_CREATE) {

                        // Subclass the header control
                        IntPtr hWnd = Native_Methods.SendMessage(this.Handle,
                                                                LVM_GETHEADER, 0, 0);

                        if (mHeader == null || mHeader.Handle != hWnd) {

                              if (mHeader != null)
                                    mHeader.ReleaseHandle();
                              else
                                    mHeader = new HeaderWindow(
                                    this,
                                    () =>
                                    {
                                          var pt = this.PointToClient(Cursor.Position);
                                          var eargs = new MouseEventArgs(Control.MouseButtons, 1, pt.X, pt.Y, 0);
                                          Aux_Header_Mouse_Down(eargs);
                                    },
                                    () => { });

                              mHeader.AssignHandle(hWnd);
                        }

                  }
            }
      }
}