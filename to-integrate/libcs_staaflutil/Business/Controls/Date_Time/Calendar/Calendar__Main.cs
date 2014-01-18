using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Fairweather.Service;
namespace Common.Controls
{
    partial class Our_Calendar : UserControl
    {

        #region CONSTANTS - 5 Lines
        const int cst_top_gutter = 40;
        const int cst_day_title = 14;
        const int cst_border = 5;

        const int cst_rows = 6;
        const int cst_cols = 7;
        const int cst_total_cells = cst_rows * cst_cols;
        const string cst_date_format = "dd MMM yyyy";
        #endregion

        #region FIELDS - 3 Lines
        static readonly Pair<int>[]
        m_traverse_order = Enumerable.Range(0, cst_rows)
                                     .SelectMany(x => Enumerable.Range(0, cst_cols)
                                                                .Select(y => new Pair<int>(x, y)))
                                     .ToArray();

        DateRectangle[][] m_date_rectangles = new DateRectangle[cst_rows][];
        Pair<DateInfo, DateRectangle>[][] m_dates = new Pair<DateInfo, DateRectangle>[cst_rows][];

        #endregion

        readonly bool b_init;

        public Our_Calendar() {

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.UserPaint |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.Opaque,
                          true);


            this.SetStyle(ControlStyles.Selectable, false);

            label.Visible = false;
            label.BackColor = Color.Transparent;
            Adjust_Size();

            Create_Buttons();

            Create_Date_Rectangles();

            Set_Defaults();

            Setup_Dates();

            Set_Strip_Location();

            Set_Button_Locations();

            Prepare_Painter_Methods();

            b_init = true;
        }

        // To be called on construction and resize
        partial void Adjust_Size();

        partial void Set_Defaults();

        // To be called on construction
        partial void Create_Buttons();

        // To be called on construction and resize
        partial void Create_Date_Rectangles();

        // To be called:
        // * After Create_Date_Rectangles
        // * On value change
        partial void Setup_Dates();


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {

            switch (keyData) {
                case Keys.Left:
                    Value = Value.AddDays(-1);
                    return true;

                case Keys.Right:
                    Value = Value.AddDays(1);
                    return true;

                case Keys.Up:
                    Value = Value.AddDays(-7);
                    return true;

                case Keys.Down:
                    Value = Value.AddDays(7);
                    return true;

                case Keys.Enter:
                    Enter_Pressed.Raise(this);
                    return true;

                case Keys.Escape:
                    Esc_Pressed.Raise(this);
                    return true;

                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        IContainer components = null;
        protected override void Dispose(bool disposing) {

            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }



    }
}
