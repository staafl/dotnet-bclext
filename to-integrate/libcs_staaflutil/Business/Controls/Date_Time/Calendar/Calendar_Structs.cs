using System;
using System.Drawing;

namespace Common.Controls
{
    public struct DateRectangle
    {
        public DateRectangle(int X_ind, int Y_ind, Rectangle rect_p) {

            this.x = X_ind;
            this.y = Y_ind;
            this.rect = rect_p;
        }

        public DateRectangle(int X_ind, int Y_ind,   // the indexes
                             int X_loc, int Y_loc,   // the position
                             int X_size, int Y_size) // the size
        {

            this.x = X_ind;
            this.y = Y_ind;
            this.rect = new Rectangle(X_loc, Y_loc, X_size, Y_size);
        }

        public DateRectangle(int X_ind, int Y_ind,   // the indexes
                     float X_loc, float Y_loc,   // the position
                     float X_size, float Y_size)
            : this(X_ind, Y_ind,
                  (int)X_loc, (int)Y_loc,
                  (int)X_size, (int)Y_size) { }

        public static implicit operator Rectangle?(DateRectangle drect) {
            return drect.rect;
        }

        public int X_Index { get { return x; } }
        public int Y_Index { get { return y; } }
        public Rectangle Rectangle { get { return rect; } }


        readonly Rectangle rect;
        readonly int x;
        readonly int y;

    }

    [Flags]
    public enum DateType
    {
        LastMonth = 0x001,
        ThisMonth = 0x010,
        NextMonth = 0x101,

        LastYear = 0x001001,
        ThisYear = 0x010010,
        NextYear = 0x101101,

        NotThisMonth = 0x000001,
        NotThisYear = 0x001000,
    }

    public struct DateInfo
    {

        public DateInfo(DateType type_p, DateTime date_p) {

            this.type = type_p;
            this.date = date_p;
        }

        readonly DateType type;
        public DateType Type {
            get { return type; }
        }

        readonly DateTime date;
        public DateTime Date {
            get { return date; }
        }
    }
}
