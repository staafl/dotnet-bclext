using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fairweather.Service;
using Versioning;

namespace Common
{
    
    [Serializable]
    [DebuggerStepThrough]
    public class Screen_Info
    {

        public Screen_Info(Account_Record? account,
                     string telephone,
                     string fax,
                     string delivery_info,
                     DateTime date,
                     int? invoice_number,
                     string order_number,
                     InvoiceType invoice_type)
           {

            Account_Ref = account.HasValue ? account.Value.Account_Ref : "";
            Telephone = telephone;
            Fax = fax;
            Invoice_Number = invoice_number;
            Order_Number = order_number;
            Date = date;
            Invoice_Type = invoice_type;
            Address = delivery_info;
            Account = account;

            var lines = (from line in Address.Lines(true, true, false).Skip(1)
                         where !line.IsNullOrEmpty()
                         select line).Pad_Right(5, "");
            Address_Lines = lines.lst();
        }

        public string Account_Ref {
            get;
            set;
        }
        public string Address {
            get;
            set;
        }
        public string Telephone {
            get;
            set;
        }
        public string Fax {
            get;
            set;
        }
        public int? Invoice_Number {
            get;
            set;
        }
        public string Order_Number {
            get;
            set;
        }

        public LedgerType Ledger_Type {
            get {
                return this.Invoice_Type.Ledger_Type().Value;
            }
        }


        public Document_Type Document_Type {
            get {
                return Invoice_Type.To_Document_Type();
            }
        }

        public InvoiceType Invoice_Type {
            get;
            set;
        }

        public DateTime Date {
            get;
            set;
        }

        public Account_Record? Account {
            get;
            set;
        }

        public List<string> Address_Lines {
            get;
            set;

        }

        public bool Is_Product_Invoice {
            get {
                return Invoice_Type == InvoiceType.sdoProductInvoice;
            }
        }




    }
}
