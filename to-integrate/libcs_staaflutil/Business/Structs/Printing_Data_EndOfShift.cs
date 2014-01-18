namespace Common
{
    using System.Diagnostics;
    using DTA;
    using Fairweather.Service;
    [DebuggerStepThrough]
    public class Printing_Data_EndOfShift : Printing_Data
    {

        #region
        readonly Day_Totals m_sales_data;

        readonly Day_Totals m_receipts_data;

        readonly Day_Totals m_grand_data;

        readonly Pair<decimal> m_docs;

        readonly decimal m_payments;

        readonly decimal m_final_total;

        readonly string m_date;

        readonly string m_day_index;
        #endregion


        public Day_Totals Sales_Data {
            get {
                return this.m_sales_data;
            }
        }

        public Day_Totals Receipts_Data {
            get {
                return this.m_receipts_data;
            }
        }

        public Day_Totals Grand_Data {
            get {
                return this.m_grand_data;
            }
        }

        public Pair<decimal> Docs {
            get {
                return this.m_docs;
            }
        }

        public decimal Payments {
            get {
                return this.m_payments;
            }
        }

        public decimal Final_Total {
            get {
                return this.m_final_total;
            }
        }

        public string Date {
            get {
                return this.m_date;
            }
        }

        public string Day_Index {
            get {
                return this.m_day_index;
            }
        }


        public Printing_Data_EndOfShift(
                    Printing_Helper helper,
                    Day_Totals sales_data,
                    Day_Totals receipts_data,
                    Day_Totals grand_data,
                    Pair<decimal> docs,
                    decimal payments,
                    decimal final_total,
                    string date,
                    string day_index)
            : base(helper) {
            this.m_sales_data = sales_data;
            this.m_receipts_data = receipts_data;
            this.m_grand_data = grand_data;
            this.m_docs = docs;
            this.m_payments = payments;
            this.m_final_total = final_total;
            this.m_date = date;
            this.m_day_index = day_index;
        }


        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "sales_data = " + this.m_sales_data;
            ret += ", ";
            ret += "receipts_data = " + this.m_receipts_data;
            ret += ", ";
            ret += "grand_data = " + this.m_grand_data;
            ret += ", ";
            ret += "docs = " + this.m_docs;
            ret += ", ";
            ret += "payments = " + this.m_payments;
            ret += ", ";
            ret += "final_total = " + this.m_final_total;
            ret += ", ";
            ret += "date = " + this.m_date;
            ret += ", ";
            ret += "day_index = " + this.m_day_index;

            ret = "{Printing_Data_EndOfShift: " + ret + "}";
            return ret;

        }

        //        public bool Equals(Printing_Data_EndOfShift obj2) {

        //#pragma warning disable
        //            if (this.m_sales_data == null) {
        //                if (obj2.m_sales_data != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_sales_data.Equals(obj2.m_sales_data))
        //                    return false;
        //            }


        //            if (this.m_receipts_data == null) {
        //                if (obj2.m_receipts_data != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_receipts_data.Equals(obj2.m_receipts_data))
        //                    return false;
        //            }


        //            if (this.m_grand_data == null) {
        //                if (obj2.m_grand_data != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_grand_data.Equals(obj2.m_grand_data))
        //                    return false;
        //            }


        //            if (this.m_docs == null) {
        //                if (obj2.m_docs != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_docs.Equals(obj2.m_docs))
        //                    return false;
        //            }


        //            if (this.m_payments == null) {
        //                if (obj2.m_payments != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_payments.Equals(obj2.m_payments))
        //                    return false;
        //            }


        //            if (this.m_final_total == null) {
        //                if (obj2.m_final_total != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_final_total.Equals(obj2.m_final_total))
        //                    return false;
        //            }


        //            if (this.m_date == null) {
        //                if (obj2.m_date != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_date.Equals(obj2.m_date))
        //                    return false;
        //            }


        //            if (this.m_day_index == null) {
        //                if (obj2.m_day_index != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_day_index.Equals(obj2.m_day_index))
        //                    return false;
        //            }


        //#pragma warning restore
        //            return true;
        //        }

        //        public override bool Equals(object obj2) {

        //            var ret = (obj2 != null && obj2 is Printing_Data_EndOfShift);

        //            if (ret)
        //                ret = this.Equals((Printing_Data_EndOfShift)obj2);


        //            return ret;

        //        }

        //        public static bool operator ==(Printing_Data_EndOfShift left, Printing_Data_EndOfShift right) {

        //            var ret = left.Equals(right);
        //            return ret;

        //        }

        //        public static bool operator !=(Printing_Data_EndOfShift left, Printing_Data_EndOfShift right) {

        //            var ret = !left.Equals(right);
        //            return ret;

        //        }

        //        public override int GetHashCode() {

        //#pragma warning disable
        //            unchecked {
        //                int ret = 23;
        //                int temp;

        //                if (this.m_sales_data != null) {
        //                    ret *= 31;
        //                    temp = this.m_sales_data.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_receipts_data != null) {
        //                    ret *= 31;
        //                    temp = this.m_receipts_data.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_grand_data != null) {
        //                    ret *= 31;
        //                    temp = this.m_grand_data.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_docs != null) {
        //                    ret *= 31;
        //                    temp = this.m_docs.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_payments != null) {
        //                    ret *= 31;
        //                    temp = this.m_payments.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_final_total != null) {
        //                    ret *= 31;
        //                    temp = this.m_final_total.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_date != null) {
        //                    ret *= 31;
        //                    temp = this.m_date.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_day_index != null) {
        //                    ret *= 31;
        //                    temp = this.m_day_index.GetHashCode();
        //                    ret += temp;

        //                }

        //                return ret;
        //            }
        //#pragma warning restore
        //        }

    }
}
