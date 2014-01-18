using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{

    public class Postings
    {
        public Postings(Posting_Details details,
                        Posting_Amounts amounts,
                        List<Credit_Allocation_Data> allocations) {

            this.Details = details;
            this.Amounts = amounts;
            this.allocations = allocations;


        }

        Posting_Details Details {
            get;
            set;
        }
        Posting_Amounts Amounts {
            get;
            set;
        }

        List<Credit_Allocation_Data> allocations;

        public IEnumerable<Credit_Allocation_Data> Allocations {
            get { return allocations; }
        }

        public IEnumerable<Credit_Allocation_Data> Usage_Allocations {
            get {
                return allocations.Where(cad => cad.Source.HasValue);
            }
        }

        public IEnumerable<Credit_Allocation_Data> Receipt_Allocations {
            get {
                return allocations.Where(cad => cad.Type == Allocation_Type.SR_To_SI);
            }
        }

        public IEnumerable<Credit_Allocation_Data> Discount_Allocations {
            get {
                return allocations.Where(cad => cad.Type == Allocation_Type.SD_To_SI);
            }
        }

        // ****************************


        public decimal Total_Receipts {
            get { return Amounts.Total_Receipts; }
        }

        public decimal Total_Discounts {
            get { return Amounts.Total_Discounts; }
        }

        public decimal Total_Usages {
            get {
                return Amounts.Total_Usages;
            }
        }

        public decimal Sa_Amount {
            get {
                return Amounts.Payment_On_Account;
            }
        }

        public decimal Cheque_Amount {
            get {
                return Amounts.Cheque_Amount;
                //return Math.Max(Total_Receipts - Total_Usages, 0.0m);
            }
        }

        // ****************************

        public string Account_Ref {
            get {
                return Details.Account_Ref;
            }
        }
        public string Bank_Ref {
            get {
                return Details.Bank_Ref;
            }
        }
        public string Discount_Details {
            get {
                return Details.Discount_Details;
            }
        }
        public string Invoice_Ref {
            get {
                return Details.Invoice_Ref;
            }
        }
        public string Ex_Ref {
            get {
                return Details.Ex_Ref;
            }
        }
        public string Sa_Details {
            get {
                return Details.Sa_Details;
            }
        }

        public DateTime Date {
            get {
                return Details.Tran_Date;
            }
        }
        public string Receipt_Details {
            get {
                return Details.Receipt_Details;
            }
        }
        public int Department {
            get {
                return Details.Dept_Num;
            }
        }

    }
}
