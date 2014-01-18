using System;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    public partial class Our_Date_Time : UserControl
    {
        const char cst_char_separator = '/';

        void Aux_Handle_Erased_Chars(int start, int end, int pos) {

            string text = mtbx.Text;

            string new_text = text.Replace_Chars(start, end, ' ', cst_char_separator);

            //if (new_text != text) {

            MethodInvoker act = () =>
            {
                mtbx.Text = new_text;
                mtbx.SelectionStart = pos;
            };

            BeginInvoke(act);
            //}
        }

        void Handle_Delete() {

            int sel_len = SelectionLength;
            int pos_1 = SelectionStart;

            if (sel_len == 0 && pos_1 == TextLength)
                return;

            int pos_2 = pos_1;

            if (sel_len != 0)
                pos_2 += sel_len - 1;

            Aux_Handle_Erased_Chars(pos_1, pos_2, pos_1);
        }

        void Handle_Backspace() {

            int sel_len = SelectionLength;

            int[] index_of_slash = mtbx.Text
                                                .Ixs(cst_char_separator)
                                                .arr();

            int pos_1 = SelectionStart;

            if (sel_len != 0) {

                var pos_2 = pos_1 + sel_len - 1;

                Aux_Handle_Erased_Chars(pos_1, pos_2, pos_1);
                return;

            }

            if (pos_1 == 0)
                return;

            --pos_1;

            if (index_of_slash.Contains(pos_1 - 1)) {

                this.Begin_Invoke(() => SelectionStart = pos_1 - 1);

            }
            else {

                char[] chars = mtbx.Text.ToCharArray();

                chars[pos_1] = ' ';
                MethodInvoker act = () =>
                {
                    mtbx.Text = new String(chars);
                    mtbx.SelectionStart = pos_1;
                };


                //Begin
                Invoke(act);
            }

        }

        void Handle_Slash() {

            string left;
            string right = "";
            string text;

            int left_cutoff;
            int right_cutoff;

            text = mtbx.Text;

            int pos = mtbx.SelectionStart;
            int next_pos;

            if (pos >= 5)
                return;

            if (pos < 2) {

                left_cutoff = 0;
                right_cutoff = 2;
                next_pos = 3;
            }
            else// if (pos >= 3 && pos < 5) 
            {

                left_cutoff = 3;
                right_cutoff = 5;
                next_pos = 6;
            }

            left = text.Substring(left_cutoff, right_cutoff - left_cutoff - 1)
                       .Replace(" ", "");

            if (left.Length > 2)
                left = left.Substring(0, 2);

            if (text.Length > right_cutoff)
                right = text.Substring(right_cutoff);

            if (left.IsNullOrEmpty() || left == "0")
                left = "01";

            if (left.Length == 1)
                left = "0" + left;

            if (left_cutoff > 0)
                left = text.Substring(0, left_cutoff) + left;

            text = left + right;

            mtbx.Text = text;

            mtbx.SelectionStart = next_pos;
        }

        void maskedTextBox_KeyDown(object sender, KeyEventArgs e) {

            if (e.KeyData == Keys.Delete) {

                Handle_Delete();
                return;
            }

            if (e.KeyData == Keys.Back) {

                Handle_Backspace();
                return;
            }

            if (e.KeyData == Keys.Escape) {

                Undo();
                return;
            }
        }

        void maskedTextBox_KeyPress(object sender, KeyPressEventArgs e) {

            if (e.KeyChar == '.')
                e.KeyChar = '/';

            if (e.KeyChar == '/')
                Handle_Slash();

        }

        // If it's 1979, cent = 1900 and millenium = 1000

        static readonly int cent = (int)(DateTime.Today.Year - (DateTime.Today.Year % 100));


        static readonly int millenium = (int)(DateTime.Today.Year - (DateTime.Today.Year % 1000));

        bool Check_Input(string input, out DateTime date) {

            date = default(DateTime);

            string[] elements = input.Replace(" ", "").Split(cst_char_separator);

            string[] now = DateTime.Now.ToString("dd|MM|yyyy").Split('|');

            if (elements.Length != 3)
                throw new ArgumentException(
                    "Input string must contain exactly two delimiting characters: " +
                    cst_char_separator);

            int day, month, year;

            bool ret = false;

            for (int ii = 0; ii < 3; ++ii) {
                if (elements[ii].IsNullOrEmpty())
                    elements[ii] = now[ii];
            }

            // once
            for (var _ = 0; _ < 1; ++_) {

                if (elements[0] == "00" ||
                    elements[1] == "00" ||
                    elements[2] == "0000" ||
                    !int.TryParse(elements[0], out day) ||
                    !int.TryParse(elements[1], out month) ||
                    !int.TryParse(elements[2], out year) ||
                    day < 0 || month < 0 || year < 0)

                    break;


                if (day == 0)
                    day = DateTime.Now.Day;

                if (month == 0)
                    month = DateTime.Now.Month;


                if (year < 10)
                    year += cent;

                else if (year < 100)
                    year += year > SplitYear ? (cent - 100) : cent;

                else if (year < 1000)
                    year += millenium;


                if (!year.Is_Valid_Date((int)month, (int)day))
                    break;

                date = new DateTime(year, month, day);

                ret = true;

            }



            return ret;

        }


        public void SelectAll() {

            this.Begin_Invoke(() => mtbx.Select_Focus(true));

        }

        protected override void OnResize(EventArgs e) {

            this.but.Location = this.ClientRectangle.Vertex(1)
                                                       .Translate(-but.Width, 0);

            this.mtbx.Size = this.Size;
            base.OnResize(e);
        }
    }
}