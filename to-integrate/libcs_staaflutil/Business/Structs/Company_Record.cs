using System;
using Fairweather.Service;

namespace Common
{
    public struct Company_Record
    {
        public Company_Record(string name, int currency, DateTime period_start, bool by_split, string country)
            : this() {

            Name = name;
            Currency = currency;

            Period = new Pair<DateTime>(period_start, period_start.Next_Financial_Period_Start());
            List_By_Split = by_split;
            Country = country;

        }


        public string Country {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }
        public int Currency {
            get;
            set;
        }
        /// <summary>
        /// exclusive of .Second
        /// </summary>
        public Pair<DateTime> Period {
            get;
            set;
        }
        public bool List_By_Split {
            get;
            set;
        }


    }
}
