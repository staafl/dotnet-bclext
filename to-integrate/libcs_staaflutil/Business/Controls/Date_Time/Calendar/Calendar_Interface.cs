using System;
using System.Drawing;
using System.Windows.Forms;
using Fairweather.Service;
using Standardization;
namespace Common.Controls
{

    partial class Our_Calendar : UserControl
    {
        // The offsets by which a point
        // has to be moved to make a string
        // drawn on it appear centered

        int m_date_font_x1;
        int m_date_font_x2;
        int m_date_font_y;

        int m_strip_x;
        int m_strip_y;

        // ****************************

        partial void Set_Defaults() {

            brush_back = Updater.Make1(() => this.Back_Color, col => new SolidBrush(col), false);
            brush_cell = Updater.Make1(() => this.Cell_Color, col => new SolidBrush(col), false);
            brush_cell_selected = Updater.Make1(() => this.Cell_Selected_Color, col => new SolidBrush(col), false);
            brush_day_title = Updater.Make1(() => this.Title_Color, col => new SolidBrush(col), false);
            brush_month = Updater.Make1(() => this.Month_Color, col => new SolidBrush(col), false);
            brush_strip = Updater.Make1(() => this.Strip_Color, col => new SolidBrush(col), false);
            brush_strip_text = Updater.Make1(() => this.Strip_Text_Color, col => new SolidBrush(col), false);
            brush_trail = Updater.Make1(() => this.Trailing_Color, col => new SolidBrush(col), false);
            brush_wkend = Updater.Make1(() => this.Weekend_Color, col => new SolidBrush(col), false);

            Cell_Font = new Font("Verdana", 10, FontStyle.Bold);
            Strip_Font = new Font("Verdana", 10, FontStyle.Bold);
            Day_Title_Font = new Font("Verdana", 10, FontStyle.Regular);

            this.Value = DateTime.Today;

            // TODO: Get these away

            this.Button_Color = Colors.Calendar_Control.Button_Color;

            this.Back_Color = Colors.Calendar_Control.Back_Color;
            this.Cell_Color = Colors.Calendar_Control.Cell_Color;
            this.Cell_Selected_Color = Colors.Calendar_Control.Cell_Selected_Color;
            this.Title_Color = Colors.Calendar_Control.Title_Color;
            this.Month_Color = Colors.Calendar_Control.Month_Color;
            this.Strip_Color = Colors.Calendar_Control.Strip_Color;
            this.Strip_Text_Color = Colors.Calendar_Control.Strip_Text_Color;
            this.Trailing_Color = Colors.Calendar_Control.Trailing_Color;
            this.Weekend_Color = Colors.Calendar_Control.Weekend_Color;

            //
        }



        /*       Value        */

        void Handle_Value_Changed(DateTime old_value, DateTime new_value) {

            Refresh_Strip_Font_Settings();

            if (!old_value.Same_Month(new_value))
                Setup_Dates();

            Invalidate();

            On_Date_Changed(EventArgs.Empty);

        }


        string m_value_string;

        public string Value_String {
            get { return m_value_string; }
            set {
                m_value_string = value;
                Refresh_Strip_Font_Settings();
            }
        }

        DateTime m_value = DateTime.Today;

        public DateTime Value {
            get { return m_value; }
            set {

                Value_String = value.ToString(cst_date_format);

                if (m_value == value)
                    return;

                var old = m_value;

                m_value = value;

                if (b_init)
                    Handle_Value_Changed(old, value);

            }
        }


        /*       Fonts        */



        // To be called whenever the cell font changes
        void Refresh_Cell_Font_Settings() {

            SizeF sz_1;
            SizeF sz_2;
            using (var g = this.CreateGraphics()) {

                sz_1 = g.MeasureString("8", this.Cell_Font);
                sz_2 = g.MeasureString("88", this.Cell_Font);

            }

            m_date_font_x1 = -(int)(sz_1.Width / 2);
            m_date_font_y = -(int)(sz_1.Height / 2);

            m_date_font_x2 = -(int)(sz_2.Width / 2);

        }

        // To be called whenever the value of the strip font changes
        void Refresh_Strip_Font_Settings() {

            SizeF size;

            using (var g = this.CreateGraphics()) {

                size = g.MeasureString(Value_String, Strip_Font);
            }

            m_strip_x = -(int)(size.Width / 2);
            m_strip_y = -(int)(size.Height / 2);
        }

        Point[] m_day_title_drawing_pts = new Point[7];

        // To be called whenever the weekday title font or the 
        // calendar's size changes
        void Refresh_Date_Title_Font_Settings() {

            int yy = cst_top_gutter + 3; // the Y coordinate on which the 
            // text will be drawn

            using (var g = this.CreateGraphics()) {
                for (int ii = 0; ii < 7; ++ii) {

                    string str = (ii + 1).Get_Weekday_Abbr(false);

                    int width = (int)g.MeasureString(str, this.Day_Title_Font).Width;

                    int middle = (m_date_rectangles[0][ii].Rectangle.Left
                                + m_date_rectangles[0][ii].Rectangle.Right) / 2;

                    int xx = middle - width / 2;

                    m_day_title_drawing_pts[ii] = new Point(xx, yy);
                }
            }
        }


        Font cell_font;
        Font strip_font;
        Font title_font;

        public Font Day_Title_Font {
            get {
                return title_font;
            }
            set {
                if (title_font == value)
                    return;
                title_font = value;
                Refresh_Date_Title_Font_Settings();
            }
        }

        public Font Cell_Font {
            get {
                return cell_font;
            }
            set {
                if (cell_font == value)
                    return;
                cell_font = value;

                Refresh_Cell_Font_Settings();
            }
        }

        public Font Strip_Font {
            get {
                return strip_font;
            }
            set {
                if (strip_font == value)
                    return;
                strip_font = value;

                Refresh_Strip_Font_Settings();
            }
        }


        /*       Colors        */


        Updater1<SolidBrush, Color> brush_back;
        Updater1<SolidBrush, Color> brush_cell;
        Updater1<SolidBrush, Color> brush_cell_selected;
        Updater1<SolidBrush, Color> brush_day_title;
        Updater1<SolidBrush, Color> brush_month;
        Updater1<SolidBrush, Color> brush_strip;
        Updater1<SolidBrush, Color> brush_strip_text;
        Updater1<SolidBrush, Color> brush_trail;
        Updater1<SolidBrush, Color> brush_wkend;

        public Color Button_Color {
            get { return this.but_prev_year.BackColor; }
            set { act_but_manip(but => but.BackColor = value); }
        }

        public Color Cell_Color {
            get;
            set;
        }

        public Color Back_Color {
            get;
            set;
        }

        public Color Month_Color {
            get;
            set;

        }

        public Color Title_Color {
            get;
            set;

        }

        public Color Trailing_Color {
            get;
            set;


        }

        public Color Weekend_Color {
            get;
            set;

        }

        public Color Strip_Color {
            get;
            set;

        }

        public Color Strip_Text_Color {
            get;
            set;

        }

        public Color Cell_Selected_Color {
            get;
            set;

        }

    }
}