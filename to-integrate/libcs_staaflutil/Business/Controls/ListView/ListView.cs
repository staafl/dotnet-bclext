using System.Drawing;
using System.Windows.Forms;
using Fairweather.Service;
using Colors = Standardization.Colors.Our_ListView;


namespace Screens
{
      /// <summary>  
      /// A ListView with several modifications:
      /// a) customizable header color
      /// b) nonclickable headers
      /// c) option to autosize last column
      /// </summary>
      public partial class Our_ListView : ListView
      {
            public Our_ListView() {

                  this.View = View.Details;
                  Autosize_Last_Column = true;
                  this.OwnerDraw = true;

                  header_brush_color = Updater.Make2(Colors.Column_Header_Color, (color) => (Brush)new SolidBrush(color));


                  Clickable_Columns = false;
            }


            bool b_cols_clickable;

            public bool Clickable_Columns {
                  get { return b_cols_clickable; }
                  set {
                        if (b_cols_clickable == value)
                              return;

                        if (value) {
                              this.HeaderStyle = ColumnHeaderStyle.Clickable;
                        }
                        else {
                              this.HeaderStyle = ColumnHeaderStyle.Nonclickable;

                              this.Prevent_Click = false;
                              this.Prevent_Cursor_Change = true;
                              this.Prevent_Double_Click = true;
                        }

                        b_cols_clickable = value;
                  }
            }

            readonly Updater2<Brush, Color> header_brush_color;

            public Color Header_Color {
                  get { return header_brush_color.Argument; }
                  set {
                        header_brush_color.Argument = value;
                  }
            }

            public bool Autosize_Last_Column {
                  get;
                  set;
            }

      }
}
