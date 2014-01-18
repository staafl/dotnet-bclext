using System.Drawing;
using System.Windows.Forms;


using Fairweather.Service;

namespace Screens
{
    public partial class Our_ListView : ListView
    {
        int clicked_header = -1;

        protected void Aux_Header_Mouse_Down(MouseEventArgs e) {

            int xx = 0;

            for (int ii = 0; ii < this.Columns.Count; ++ii) {

                var col = this.Columns[ii];

                if (e.X < xx || e.X > xx + col.Width) {

                    xx += col.Width;
                    continue;
                }

                int old = clicked_header;
                OnColumnClick(new ColumnClickEventArgs(ii));
                clicked_header = ii;

                for (int jj = 0; jj < this.Columns.Count; ++jj) {
                    if (jj == clicked_header || jj == old) 
                        // Refresh the newly selected and the old column
                        this.Columns[jj].AutoResize(ColumnHeaderAutoResizeStyle.None);
                }

                break;
            }

        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e) {

            e.DrawDefault = true;

            base.OnDrawSubItem(e);
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e) {

            e.DrawDefault = true;

            base.OnDrawItem(e);
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e) {

            int column = e.ColumnIndex;

            using (Graphics g = e.Graphics) {

                e.DrawBackground();

                Rectangle rect = e.Bounds.Expand(-1);

                if (e.Header.Index == clicked_header)
                    g.FillRectangle(Brushes.Gray, e.Bounds.Expand(-1, 
                                                                  false, false, true, true));
                else
                    g.FillRectangle(header_brush_color.Value, rect);

                rect = rect.Translate(1, 1);

                e.Graphics.DrawString(e.Header.Text, e.Header.ListView.Font,
                                      Brushes.Black, rect);

                //e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black), 2), e.Contract(1)); 
                //optional outline
            }

            base.OnDrawColumnHeader(e);
        }
    }
}
