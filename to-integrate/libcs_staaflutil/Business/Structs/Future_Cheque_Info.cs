
namespace Common
{
    public struct Future_Cheque_Info
    {

        public Future_Cheque_Info(Cheque_Info cheque,
                         int number,
                         decimal? old_balance,
                         decimal amount,
                         decimal? new_balance)
            : this() {

            Cheque = cheque; 
            Number = number;
            Old_Balance = old_balance;
            Amount = amount;
            New_Balance = new_balance;

        }

        public Cheque_Info Cheque {
            get;
            set;
        }

        public int Number {
            get;
            set;
        }

        public decimal Amount {
            get;
            set;
        }

        public decimal? Old_Balance {
            get;
            set;
        }

        public decimal? New_Balance {
            get;
            set;
        }


    }
}
