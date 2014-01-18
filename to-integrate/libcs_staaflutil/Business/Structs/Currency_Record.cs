
namespace Common
{
    public struct Currency_Record
    {
        public Currency_Record(int number,
                                string symbol,
                                string name,
                                string code,
                                decimal rate,
                                string major,
                                string minor)
            : this() {
            Number = number;
            Symbol = symbol;
            Name = name;
            Code = code;
            Rate = rate;
            MajorUnit = major;
            MinorUnit = minor;
        }

        public int Number {
            get;
            set;
        }
        public decimal Rate {
            get;
            set;
        }
        public string Name {
            get;
            set;
        }
        public string Code {
            get;
            set;
        }
        public string Symbol {
            get;
            set;
        }
        public string MajorUnit {
            get;
            set;
        }
        public string MinorUnit {
            get;
            set;
        }


    }
}
