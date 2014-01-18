using System;
using System.Collections.Generic;
using System.Linq;
using Fairweather.Service;

namespace Common
{
    [Serializable]
    public struct Payment_Info
    {
        public Payment_Info(Payment_Type type, decimal amount)
            : this() {

            Post_Dated_Cheques = new Cheque_Info[0];
            Current_Cheques = new Cheque_Info[0];
            
            switch (type) {
                case Payment_Type.Cash:
                    Cash = amount;

                    break;
                case Payment_Type.Credit:
                    Credit = amount;

                    break;
                case Payment_Type.Gift:
                    Gift = amount;

                    break;
                default:
                    break;
            }
        }

        public Payment_Info(Cheque_Info[] post_dated_cheques,
                    Cheque_Info[] current_cheques,
                    decimal cash,
                    decimal credit,
                    decimal gift)
            : this() {

            Post_Dated_Cheques = post_dated_cheques;
            Current_Cheques = current_cheques;
            Cash = cash;
            Credit = credit;
            Gift = gift;
        }

        static readonly Dictionary<Payment_Type, Func<Payment_Info, decimal>>
        dict = new Dictionary<Payment_Type, Func<Payment_Info, decimal>>
        {
        {Payment_Type.Cash , info => info.Cash},
        {Payment_Type.Credit , info => info.Credit},
        {Payment_Type.Gift , info => info.Gift},
        {Payment_Type.Cheque , info => info.Cheque_Amounts},
        {Payment_Type.Post_Dated_Cheque, info => info.Post_Dated_Cheque_Amounts },
        };

        public decimal Payment_Amount(Payment_Type type) {

            return dict[type](this);
        }

        public decimal Cheque_Amounts {
            get {
                return Current_Cheques.Sum(cheque => cheque.Amount);
            }
        }

        public decimal Post_Dated_Cheque_Amounts {
            get {
                return Post_Dated_Cheques.Sum(cheque => cheque.Amount);
            }
        }

        public Cheque_Info[] Current_Cheques {
            get;
            set;
        }

        public Cheque_Info[] Post_Dated_Cheques {
            get;
            set;

        }

        public Cheque_Info?[] Six_Cheques {
            get {
                var ret = Current_Cheques.Concat(Post_Dated_Cheques)
                                          .Select(cinfo => new Cheque_Info?(cinfo))
                                          .Pad_Right(6, null);

                return ret.ToArray();
            }
        }





        public decimal Amount_Prepaid {
            get {
                decimal ret = Cash + Credit + Gift + Current_Cheques.Sum(cinfo => cinfo.Amount);
                return ret;
            }
        }

        /// <summary> Amount prepaid + Post_Dated_Cheque_Amounts </summary>
        public decimal All_Paid {
            get {
                decimal ret = Post_Dated_Cheques.Sum(cinfo => cinfo.Amount) + Amount_Prepaid;

                return ret;
            }
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

        #region

        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "cash = " + this.Cash;
            ret += ", ";
            ret += "credit = " + this.Credit;
            ret += ", ";
            ret += "gift = " + this.Gift;
            ret += ", ";
            ret += "post_dated_cheques = " + this.Post_Dated_Cheques;
            ret += ", ";
            ret += "current_cheques = " + this.Current_Cheques;

            ret = "{Payment_Info: " + ret + "}";
            return ret;

        }

        public bool Equals(Payment_Info obj2) {

            if (!this.Cash.Equals(obj2.Cash))
                return false;

            if (!this.Credit.Equals(obj2.Credit))
                return false;

            if (!this.Gift.Equals(obj2.Gift))
                return false;

            if (!this.Post_Dated_Cheques.Deep_Equals(obj2.Post_Dated_Cheques))
                return false;

            if (!this.Current_Cheques.Deep_Equals(obj2.Current_Cheques))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Payment_Info);

            if (ret)
                ret = this.Equals((Payment_Info)obj2);


            return ret;

        }

        public static bool operator ==(Payment_Info left, Payment_Info right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Payment_Info left, Payment_Info right) {

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
                temp = this.Post_Dated_Cheques.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.Current_Cheques.GetHashCode();
                ret += temp;

                return ret;
            }
        }


        #endregion
    }
}
