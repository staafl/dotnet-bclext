using System;
using System.Diagnostics;
using Fairweather.Service;

namespace Common
{
    /// <summary>
    /// This structure contains the payment amounts received on
    /// a particular day.
    /// </summary>
    [DebuggerStepThrough]
    public struct Day_Totals
    {
        public Day_Totals(DateTime day,
            decimal cash,
            decimal credit,
            decimal gift,
            decimal cheques,
            decimal post_dated)
            : this() {

            this.Day = day.Date;
            this.Cash = cash;
            this.Credit = credit;
            this.Gift = gift;
            this.Cheques = cheques;
            this.Post_Dated = post_dated;

        }



        public DateTime Day {
            get;
            set;
        }

        public decimal Cash {
            get;
            set;

        }

        public decimal Credit {
            get;
            set;

        }

        public decimal Gift {
            get;
            set;

        }

        public decimal Cheques {
            get;
            set;

        }

        public decimal Post_Dated {
            get;
            set;
        }

        public decimal Total {
            get {
                var ret = Cash + Credit + Gift + Cheques + Post_Dated;

                return ret;
            }
        }


        /// <summary>
        /// Adds two Day_Totals instances' payment details pairwise and 
        /// uses the information to create a new instance.
        /// !allow_different_dates -> throws ApplicationException if the two instances refer to 
        /// different dates.
        /// The new instance's date is "left.Day"
        /// </summary>
        public static Day_Totals 
        Add(Day_Totals left, Day_Totals right, bool allow_different_dates) {

            if (!allow_different_dates)
                (left.Day == right.Day).tiff();

            var ret = new Day_Totals(left.Day,
                                     left.Cash + right.Cash,
                                     left.Credit + right.Credit,
                                     left.Gift + right.Gift,
                                     left.Cheques + right.Cheques,
                                     left.Post_Dated + right.Post_Dated);

            return ret;


        }

        /// <summary>
        /// Adds two Day_Totals instances' payment details pairwise and 
        /// uses the information to create a new instance.
        /// Throws ApplicationException if the two instances refer to 
        /// different dates.
        /// </summary>
        public static Day_Totals operator +(Day_Totals left, Day_Totals right) {


            return Add(left, right, false);


        }
        #region Boilerplate
        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "cash = " + this.Cash;
            ret += ", ";
            ret += "credit = " + this.Credit;
            ret += ", ";
            ret += "gift = " + this.Gift;
            ret += ", ";
            ret += "cheques = " + this.Cheques;
            ret += ", ";
            ret += "post_dated = " + this.Post_Dated;

            ret = "{Days_Posted_Data: " + ret + "}";
            return ret;

        }

        public bool Equals(Day_Totals obj2) {

            if (!this.Cash.Equals(obj2.Cash))
                return false;

            if (!this.Credit.Equals(obj2.Credit))
                return false;

            if (!this.Gift.Equals(obj2.Gift))
                return false;

            if (!this.Cheques.Equals(obj2.Cheques))
                return false;

            if (!this.Post_Dated.Equals(obj2.Post_Dated))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Day_Totals);

            if (ret)
                ret = this.Equals((Day_Totals)obj2);


            return ret;

        }

        public static bool operator ==(Day_Totals left, Day_Totals right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Day_Totals left, Day_Totals right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

            unchecked {
                int ret = 23;
                int temp;

                ret *= 31;
                temp = this.Cash.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.Credit.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.Gift.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.Cheques.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.Post_Dated.GetHashCode();
                ret += temp;

                return ret;
            }
        }

        #endregion
    }
}
