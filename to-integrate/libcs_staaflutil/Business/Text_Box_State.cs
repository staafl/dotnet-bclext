using System.Diagnostics;
namespace Fairweather.Service
{

    [DebuggerStepThrough]
    public struct Text_Box_State
    {



        public int Selection_Start {
            get;
            set;
        }

        public int Selection_Length {
            get;
            set;
        }


        public string Text {
            get;
            set;
        }

        public Text_Box_State(int sel_start, int sel_len, string txt)
            : this() {

            (sel_len >= 0 && sel_start >= 0).Throw_If_False();
            (sel_len + sel_start <= txt.Safe_Length()).Throw_If_False();

            Selection_Start = sel_start;
            Selection_Length = sel_len;
            Text = txt;
        }
#if WINFORMS

        public Text_Box_State(System.Windows.Forms.TextBox tb)
            : this() {

            Selection_Start = tb.SelectionStart;
            Selection_Length = tb.SelectionLength;
            Text = tb.Text;



        }

        public void Apply_To(System.Windows.Forms.TextBox tb) {

            tb.SelectionStart = Selection_Start;
            tb.SelectionLength = Selection_Length;
            tb.Text = Text;

        }

#endif

        #region Text_Box_State

        /* Boilerplate */

        //        public override string ToString() {

        //            string ret = "";

        //            ret += "selection_start = " + this.m_selection_start;
        //            ret += ", ";
        //            ret += "selection_end = " + this.m_selection_end;
        //            ret += ", ";
        //            ret += "caret_position = " + this.m_caret_position;
        //            ret += ", ";
        //            ret += "text = " + this.m_text;

        //            ret = "{Text_Box_State: " + ret + "}";
        //            return ret;

        //        }

        //        public bool Equals(Text_Box_State obj2) {

        //#pragma warning disable
        //            if (this.m_selection_start == null) {
        //                if (obj2.m_selection_start != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_selection_start.Equals(obj2.m_selection_start))
        //                    return false;
        //            }


        //            if (this.m_selection_end == null) {
        //                if (obj2.m_selection_end != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_selection_end.Equals(obj2.m_selection_end))
        //                    return false;
        //            }


        //            if (this.m_caret_position == null) {
        //                if (obj2.m_caret_position != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_caret_position.Equals(obj2.m_caret_position))
        //                    return false;
        //            }


        //            if (this.m_text == null) {
        //                if (obj2.m_text != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_text.Equals(obj2.m_text))
        //                    return false;
        //            }


        //#pragma warning restore
        //            return true;
        //        }

        //        public override bool Equals(object obj2) {

        //            var ret = (obj2 != null && obj2 is Text_Box_State);

        //            if (ret)
        //                ret = this.Equals((Text_Box_State)obj2);


        //            return ret;

        //        }

        //        public static bool operator ==(Text_Box_State left, Text_Box_State right) {

        //            var ret = left.Equals(right);
        //            return ret;

        //        }

        //        public static bool operator !=(Text_Box_State left, Text_Box_State right) {

        //            var ret = !left.Equals(right);
        //            return ret;

        //        }

        //        public override int GetHashCode() {

        //#pragma warning disable
        //            unchecked {
        //                int ret = 23;
        //                int temp;

        //                if (this.m_selection_start != null) {
        //                    ret *= 31;
        //                    temp = this.m_selection_start.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_selection_end != null) {
        //                    ret *= 31;
        //                    temp = this.m_selection_end.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_caret_position != null) {
        //                    ret *= 31;
        //                    temp = this.m_caret_position.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_text != null) {
        //                    ret *= 31;
        //                    temp = this.m_text.GetHashCode();
        //                    ret += temp;

        //                }

        //                return ret;
        //            }
        //#pragma warning restore
        //        }

        #endregion
    }

}