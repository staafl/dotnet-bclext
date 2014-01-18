using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    public class Address_Box : UserControl
    {
        readonly List<TextBox> text_boxes;
        readonly Dictionary<TextBox, int> indices;
        readonly int lines;



        public Address_Box() : this(6) { }

        public Address_Box(int lines) {

            this.lines = lines;
            text_boxes = new List<TextBox>(lines);
            indices = new Dictionary<TextBox, int>(lines);


            this.BorderStyle = BorderStyle.FixedSingle;
            //Border.Create(this, Quad.Make(1, 1, 1, 1));


            Left_Offset = cst_left;
            Right_Offset = cst_right;
            Top_Offset = cst_top;
            Interim = cst_off;

            Prepare_Text_Boxes();


        }


        void Prepare_Text_Boxes() {

            for (int ii = 0; ii < lines; ++ii) {

                var tb = Make_TB(ii);
                text_boxes.Add(tb);

            }

        }


        protected override void OnBackColorChanged(EventArgs e) {

            base.OnBackColorChanged(e);

            foreach (var tb in text_boxes)
                tb.BackColor = this.BackColor;


        }
        protected override void OnFontChanged(EventArgs e) {
            base.OnFontChanged(e);

            foreach (var tb in text_boxes) {
                tb.Font = this.Font;
                Resize_TB(tb);
            }

        }

        protected override void OnForeColorChanged(EventArgs e) {
            base.OnForeColorChanged(e);

            foreach (var tb in text_boxes)
                tb.ForeColor = this.ForeColor;

        }
        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);

            foreach (var tb in text_boxes)
                Resize_TB(tb);
        }

        const int cst_left = 3;
        const int cst_right = 0;
        const int cst_top = 3;
        const int cst_off = 4;

        public int Top_Offset {
            get;
            set;
        }
        public int Left_Offset {
            get;
            set;
        }
        public int Right_Offset {
            get;
            set;
        }
        public int Interim {
            get;
            set;
        }
        public void Refresh_Layout() {
            foreach (var tb in text_boxes)
                Position_TB(tb);
        }


        void Resize_TB(TextBox tb) {

            tb.Size = new Size(this.Width - Left_Offset - Right_Offset,
                               this.Font.Height);

        }

        void Position_TB(TextBox tb) {

            var index = indices[tb];
            int yy = Top_Offset + (Font.Height + Interim) * index;
            tb.Location = new Point(cst_left, yy);

        }

        TextBox Make_TB(int index) {

            var ret = new Our_Text_Box();
            indices[ret] = index;

            ret.BorderStyle = BorderStyle.None;

            Resize_TB(ret);
            Position_TB(ret);



            ret.Auto_Highlight = true;


            this.Controls.Add(ret);
            ret.BringToFront();

            return ret;
        }

        public List<TextBox> TextBoxes {
            get {
                return new List<TextBox>(text_boxes);
            }
        }
        public override string Text {
            get {
                return Lines.Unlines(true);
            }
            set {
                var strings = value.Lines(true, true, true).Take(lines);
                foreach (var pair in strings.Zip(text_boxes)) {
                    pair.Second.Text = pair.First;
                }

            }
        }
        public string[] Lines {
            get {
                return (from tb in text_boxes
                        select tb.Text ?? "").arr();
            }

        }
        public int Max_Chars_On_Row {
            get {
                return text_boxes[0].MaxLength;
            }
            set {
                foreach (var tb in text_boxes)
                    tb.MaxLength = value;
            }
        }
        public int Line_Count {
            get { return lines; }
        }


    }
}