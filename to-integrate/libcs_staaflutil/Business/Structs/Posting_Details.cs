using System;
using Fairweather.Service;

namespace Common
{
	

    
    public struct Posting_Details
    {
        public Posting_Details(string account,
                               string bank,
                               string receipt_details,
                               string discount_details,
                               DateTime date,
                               string invoice_ref,
                               string ext_ref,
                               int department,
                               string sa_details)

            : this() {

            Account_Ref = account;
            Bank_Ref = bank;
            Receipt_Details = receipt_details;
            Discount_Details = discount_details;
            Tran_Date = date;
            Invoice_Ref = invoice_ref;
            Ex_Ref = ext_ref;
            Dept_Num = department;
            Sa_Details = sa_details;
        }



        public string Account_Ref {
            get;
            set;
        }
        public string Bank_Ref {
            get;
            set;
        }

        public string Discount_Details {
            get;
            set;
        }

        public string Invoice_Ref {
            get;
            set;
        }

        public string Ex_Ref {
            get;
            set;
        }

        public string Sa_Details {
            get;
            set;
        }


        public DateTime Tran_Date {
            get;
            set;
        }

        public string Receipt_Details {
            get;
            set;
        }

        public int Dept_Num {
            get;
            set;
        }


    }
}
