namespace Common
{
    using System.Diagnostics;
    using DTA;
    [DebuggerStepThrough]
    public class Printing_Data_Sale : Printing_Data
    {

        #region
        readonly Posting_Info m_posting_info;

        readonly int m_number;

        readonly Item_Info[] m_items;

        readonly decimal? m_remaining_balance;
        #endregion

        public Posting_Info Posting_Info {
            get {
                return this.m_posting_info;
            }
        }

        public int Number {
            get {
                return this.m_number;
            }
        }

        public Item_Info[] Items {
            get {
                return this.m_items;
            }
        }

        public decimal? Remaining_Balance {
            get {
                return this.m_remaining_balance;
            }
        }

        public Document_Info? Null_Credit_Note {
            get;
            set;
        }


        public Printing_Data_Sale(Printing_Helper helper,
                    Posting_Info posting_info,
                    int number,
                    Item_Info[] items,
                    decimal? remaining_balance,

                    Document_Info? null_credit_note    // refund info
            )
            : base(helper) {

            this.m_posting_info = posting_info;
            this.m_number = number;
            this.m_items = items;
            this.m_remaining_balance = remaining_balance;
            this.Null_Credit_Note = null_credit_note;
        }


        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "posting_info = " + this.m_posting_info;
            ret += ", ";
            ret += "number = " + this.m_number;
            ret += ", ";
            ret += "items = " + this.m_items;
            ret += ", ";
            ret += "remaining_balance = " + this.m_remaining_balance;

            ret = "{Printing_Data_Sale: " + ret + "}";
            return ret;

        }

        //        public bool Equals(Printing_Data_Sale obj2) {

        //#pragma warning disable
        //            if (this.m_posting_info == null) {
        //                if (obj2.m_posting_info != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_posting_info.Equals(obj2.m_posting_info))
        //                    return false;
        //            }


        //            if (this.m_number == null) {
        //                if (obj2.m_number != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_number.Equals(obj2.m_number))
        //                    return false;
        //            }


        //            if (this.m_items == null) {
        //                if (obj2.m_items != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_items.Equals(obj2.m_items))
        //                    return false;
        //            }


        //            if (this.m_remaining_balance == null) {
        //                if (obj2.m_remaining_balance != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_remaining_balance.Equals(obj2.m_remaining_balance))
        //                    return false;
        //            }


        //#pragma warning restore
        //            return true;
        //        }

        //        public override bool Equals(object obj2) {

        //            var ret = (obj2 != null && obj2 is Printing_Data_Sale);

        //            if (ret)
        //                ret = this.Equals((Printing_Data_Sale)obj2);


        //            return ret;

        //        }

        //        public static bool operator ==(Printing_Data_Sale left, Printing_Data_Sale right) {

        //            var ret = left.Equals(right);
        //            return ret;

        //        }

        //        public static bool operator !=(Printing_Data_Sale left, Printing_Data_Sale right) {

        //            var ret = !left.Equals(right);
        //            return ret;

        //        }

        //        public override int GetHashCode() {

        //#pragma warning disable
        //            unchecked {
        //                int ret = 23;
        //                int temp;

        //                if (this.m_posting_info != null) {
        //                    ret *= 31;
        //                    temp = this.m_posting_info.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_number != null) {
        //                    ret *= 31;
        //                    temp = this.m_number.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_items != null) {
        //                    ret *= 31;
        //                    temp = this.m_items.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_remaining_balance != null) {
        //                    ret *= 31;
        //                    temp = this.m_remaining_balance.GetHashCode();
        //                    ret += temp;

        //                }

        //                return ret;
        //            }
        //#pragma warning restore
        //        }

    }
}
