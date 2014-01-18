using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    partial class QSF_ListView
    {


        [DebuggerStepThrough]
        protected override void OnDrawItem(DrawListViewItemEventArgs e) {
            e.DrawDefault = true;

            base.OnDrawItem(e);
        }

        [DebuggerStepThrough]
        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e) {
            e.DrawDefault = true;

            base.OnDrawSubItem(e);
        }

        [DebuggerStepThrough]
        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e) {

            var drawing_flags = TextFormatFlags.NoPadding |
                                 TextFormatFlags.Left |
                                 TextFormatFlags.VerticalCenter;

            int column = e.ColumnIndex;
            SortOrder direction = this.Sorting;

            if (column == this.SortedColumn && direction != SortOrder.None) {

                using (Graphics g = e.Graphics) {

                    Point p1;
                    Bitmap bmp;
                    SizeF text_size = g.MeasureString(Columns[column].Text, Font);

                    {
                        int xx = (int)text_size.Width + e.Bounds.Left;
                        p1 = new Point(xx, 0).Translate(12, 1);
                    }

                    if (direction == SortOrder.Ascending) {
                        bmp = (Bitmap)global::Fairweather.Service.Properties.Resources.img_down;
                    }
                    else {
                        bmp = (Bitmap)global::Fairweather.Service.Properties.Resources.img_up;
                    }

                    bmp.MakeTransparent(bmp.GetPixel(1, 1));

                    var rect = new Rectangle(p1, bmp.Size);

                    e.DrawBackground();
                    e.DrawText(drawing_flags);

                    e.Graphics.DrawImage(bmp, rect);

                }
            }
            else if (this.IsHandleCreated) {

                e.DrawBackground();
                e.DrawText(drawing_flags);
            }
            else {
                e.DrawDefault = true;
            }

            base.OnDrawColumnHeader(e);
        }
    }
}
