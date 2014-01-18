using System;

namespace Common
{
    public struct Remittance_Postings
    {
        public Remittance_Postings(string type,
                            string tax_code,
                            string inv_ref,
                            DateTime date,
                            DateTime payment_date,
                            double gross_amount,
                            double net_amount,
                            double payment_amount,
                            double vat_amount)
            : this() {
            Type = type;
            Tax_Code = tax_code;
            Inv_Ref = inv_ref;

            Date = date;
            Payment_Date = payment_date;

            Gross_Amount = gross_amount;
            Net_Amount = net_amount;
            Payment_Amount = payment_amount;
            VAT = vat_amount;
        }

        public object Type {
            get;
            set;
        }

        public object Tax_Code {
            get;
            set;
        }
        public object Inv_Ref {
            get;
            set;
        }

        public DateTime Date {
            get;
            set;
        }
        public DateTime Payment_Date {
            get;
            set;
        }

        public double Gross_Amount {
            get;
            set;
        }
        public double Net_Amount {
            get;
            set;
        }
        public double Payment_Amount {
            get;
            set;
        }
        public double VAT {
            get;
            set;
        }


    }
}
