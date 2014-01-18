namespace Common
{
    using System;
    using System.Diagnostics;
    using DTA;
    using Fairweather.Service;
    [DebuggerStepThrough]
    public class Printing_Data_Receipt : Printing_Data
    {

        public decimal? Old_Balance {
            get;
            set;
        }

        public decimal Amount_Prepaid {
            get;
            set;
        }

        public decimal? New_Balance {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }

        public int Number {
            get;
            set;
        }

        public DateTime Date {
            get;
            set;
        }

        public bool Print_Hrs {
            get;
            set;
        }

        public Pair<string>[] Footer {
            get;
            set;
        }


        public Printing_Data_Receipt(
                    Printing_Helper helper,
                    decimal? old_balance,
                    decimal amount_prepaid,
                    decimal? new_balance,
                    string name,
                    int number,
                    DateTime date,
                    bool print_hrs,
                    params Pair<string>[] footer)
            : base(helper) {

            this.Old_Balance = old_balance;
            this.Amount_Prepaid = amount_prepaid;
            this.New_Balance = new_balance;

            this.Name = name;
            this.Number = number;
            this.Date = date;
            this.Print_Hrs = print_hrs;
            this.Footer = footer;

        }


        /* Boilerplate */

        //        public bool Equals(Printing_Data_Receipt obj2) {

        //#pragma warning disable
        //            if (this.m_old_balance == null) {
        //                if (obj2.m_old_balance != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_old_balance.Equals(obj2.m_old_balance))
        //                    return false;
        //            }


        //            if (this.m_amount == null) {
        //                if (obj2.m_amount != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_amount.Equals(obj2.m_amount))
        //                    return false;
        //            }


        //            if (this.m_new_balance == null) {
        //                if (obj2.m_new_balance != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_new_balance.Equals(obj2.m_new_balance))
        //                    return false;
        //            }


        //            if (this.m_name == null) {
        //                if (obj2.m_name != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_name.Equals(obj2.m_name))
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


        //            if (this.m_date == null) {
        //                if (obj2.m_date != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_date.Equals(obj2.m_date))
        //                    return false;
        //            }


        //            if (this.m_print_hrs == null) {
        //                if (obj2.m_print_hrs != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_print_hrs.Equals(obj2.m_print_hrs))
        //                    return false;
        //            }


        //            if (this.m_footer == null) {
        //                if (obj2.m_footer != null)
        //                    return false;
        //            }
        //            else {
        //                if (!this.m_footer.Equals(obj2.m_footer))
        //                    return false;
        //            }


        //#pragma warning restore
        //            return true;
        //        }

        //        public override bool Equals(object obj2) {

        //            var ret = (obj2 != null && obj2 is Printing_Data_Receipt);

        //            if (ret)
        //                ret = this.Equals((Printing_Data_Receipt)obj2);


        //            return ret;

        //        }

        //        public static bool operator ==(Printing_Data_Receipt left, Printing_Data_Receipt right) {

        //            var ret = left.Equals(right);
        //            return ret;

        //        }

        //        public static bool operator !=(Printing_Data_Receipt left, Printing_Data_Receipt right) {

        //            var ret = !left.Equals(right);
        //            return ret;

        //        }

        //        public override int GetHashCode() {

        //#pragma warning disable
        //            unchecked {
        //                int ret = 23;
        //                int temp;

        //                if (this.m_old_balance != null) {
        //                    ret *= 31;
        //                    temp = this.m_old_balance.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_amount != null) {
        //                    ret *= 31;
        //                    temp = this.m_amount.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_new_balance != null) {
        //                    ret *= 31;
        //                    temp = this.m_new_balance.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_name != null) {
        //                    ret *= 31;
        //                    temp = this.m_name.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_number != null) {
        //                    ret *= 31;
        //                    temp = this.m_number.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_date != null) {
        //                    ret *= 31;
        //                    temp = this.m_date.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_print_hrs != null) {
        //                    ret *= 31;
        //                    temp = this.m_print_hrs.GetHashCode();
        //                    ret += temp;

        //                }

        //                if (this.m_footer != null) {
        //                    ret *= 31;
        //                    temp = this.m_footer.GetHashCode();
        //                    ret += temp;

        //                }

        //                return ret;
        //            }
        //#pragma warning restore
        //        }

    }
}
