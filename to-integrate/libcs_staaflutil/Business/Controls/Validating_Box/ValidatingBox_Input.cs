using System;
using System.Diagnostics;
using System.Linq;

using Fairweather.Service;

namespace Common.Controls
{

    public partial class Validating_Box
    {

        bool b_text;
        bool b_wnd_proc;
        bool b_leave;
        bool b_text_changed;
        bool b_user_is_editing;
        string last_valid_text;
        string text_on_enter;


        public event EventHandler ValueChanged;




        public override string Text {
            get {
                return base.Text;
            }
            set {
                b_text = true;
                try {
                    base.Text = value;
                }
                finally {
                    b_text = false;
                }
            }
        }

        /// <summary>
        /// Returns Has_User_Typed_Text and sets it to false
        /// </summary>
        /// <returns></returns>
        public bool Check_Typed_Text() {

            bool ret = this.Has_User_Typed_Text;

            this.Has_User_Typed_Text = false;

            return ret;

        }

        /// <summary>
        /// Returns whether the box contains the same
        /// text as it did when it was last selected.
        /// </summary>
        public bool Has_Text_Changed_Since_Entered {
            get { return this.Text != text_on_enter; }
        }

        /// <summary>
        /// This property returns whether the user has typed
        /// or pasted any text using the keyboard or mouse
        /// since it was last reset.
        /// This property is reset after
        /// a) ResetText()
        /// b) setting the value of Text programmatically
        /// You can set this property to false when you wish to
        /// indicate that the change has been handled.
        /// Setting this property to false also causes Has_User_Typed_Text_Since_Entered
        /// to be set to false. 
        /// </summary>
        public bool Has_User_Typed_Text { get; set; }

        /// <summary>
        /// Returns whether the box contains the
        /// default text (0, 0.0, 0.00 etc)
        /// </summary>
        public bool Has_Default_Text {
            get { return Text == Default_Text; }
        }



        [DebuggerStepThrough]
        protected override void
        OnTextChanged(EventArgs e) {

            if (b_text_changed)
                return;

            try {
                b_text_changed = true;

                Handle_Text_Change();

                base.OnTextChanged(e);
            }
            finally {
                b_text_changed = false;
            }
        }


        // Only call from OnTextChanged under the protection
        // of a flag
        void Handle_Text_Change() {

            // Changed programmatically
            if (b_text) {

                Has_User_Typed_Text = false;
                return;

            }

            int pos = this.SelectionStart;
            var txt2 = Try_Fix_Input(Text, last_valid_text, ref pos);

            if (txt2 != Text) {

                this.Text = txt2;
                this.SelectionStart = Math.Max(pos, 0);

            }

            if (Text != text_on_enter) {
                if (b_user_is_editing) {
                    Has_User_Typed_Text = true;
                }
            }

            last_valid_text = Text;

        }


        [DebuggerStepThrough]
        string Try_Fix_Input(string now, string before, ref int pos) {

            int tmp = pos;

            string str = now;

            if (str.IsNullOrEmpty())
                return "";

            // invalid chars
            if (now.Any(ch => !Char.IsDigit(ch) && ch != '.'))
                return before;

            int dots = str.Count(ch => ch == '.');

            // more than one dot :-)
            if (dots > 1)
                return before;

            // leading dot
            if (str[0] == '.') {
                str = '0' + str;
                ++tmp;
            }

            int after = AfterDecimal(now);
            int reserved = Decimal_Places - after;
            int length = now.Length;

            if (reserved < 0)//We have too many characters after the dot
                return before;

            int excess = (reserved + length) - MaxLength;

            if (excess > 0) {//We have too many characters in front of the dot

                if (dots == 1) {
                    str = str.Replace(".", "");

                    ++excess;
                    --tmp;
                    --dots;
                }

                if (dots == 0) {

                    str = str.Insert(str.Length - excess, ".");

                    if (pos == length)
                        ++tmp;

                    ++dots;
                }

            }



            // leading zeroes
            if (now != now.TrimStart('0')) {

                if (now.Length > 1 && now[1] != '.')
                    return before;

            }

            pos = tmp;
            return str;
        }
        [DebuggerStepThrough]
        static int AfterDecimal(String str) {

            int dot = str.LastIndexOf(".");

            if (dot <= 0)
                return dot;

            var ret = (str.Length - dot - 1);
            return ret;

        }



    }
}