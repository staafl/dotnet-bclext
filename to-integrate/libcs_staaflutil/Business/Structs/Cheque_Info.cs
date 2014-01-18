using System;
using System.Diagnostics;
using Fairweather.Service;
using Standardization;
namespace Common
{
    [DebuggerStepThrough]
    [Serializable]
    public struct Cheque_Info
    {
        public const string CHEQUE_DATE_FORMAT = Data.DEF_DATE_FORMAT;

        public Cheque_Info(decimal amount,
                   DateTime date,
                   string number)
            : this() {

            Amount = amount;
            Date = date;
            Number = number ?? "";
        }


        public decimal Amount {
            get;
            set;
        }

        public DateTime Date {
            get;
            set;
        }

        public string Number {
            get;
            set;
        }


        public string Cheque_String {
            get {
                string ret = "{0},{1},{2}".spf(Amount.ToString(true),
                                                   Date.ToString(CHEQUE_DATE_FORMAT),
                                                   Number);

                return ret;
            }
        }

        public static implicit operator string(Cheque_Info? info) {

            if (info.HasValue.False())
                return "";

            return info.Value.Cheque_String;
        }


    }
}
