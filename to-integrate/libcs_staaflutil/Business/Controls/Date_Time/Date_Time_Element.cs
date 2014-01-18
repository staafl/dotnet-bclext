using Fairweather.Service;

namespace Common.Controls
{
    public class Date_Time_Element
    {

        public Date_Time_Element(Our_Date_Time control, int start, int length) {

            (start >= 0).tiff();
            (length > 0).tiff();
            control.tifn();

            this.control = control;
            this.start = start;
            this.length = length;

        }

        void Select() {

            control.SelectionStart = start;
            control.SelectionLength = length;

        }

        readonly Our_Date_Time control;

        readonly int start;
        readonly int length;


        int? Value() {

            var txt = Text;
            var ret = txt.ToInt32_();

            return ret;

        }

        bool Set_Value(int value) {

            var str = value.ToString();

            (str.Length > length).tift();

            str = str.PadLeft(length, '0');

            var txt = control.Text.Substring(0, start) + value + control.Text.Substring(start + length);

            control.Text = txt;

            return Validate();

        }

        bool Validate() {

            var args = Args.Make(true);
            Validating.Raise(this, args);

            return args.Mut;

        }

        string Text {
            get {
                var ret = control.Text.Substring(start, length);
                return ret;
            }
        }

        public event Handler<bool> Validating;
    }
}
