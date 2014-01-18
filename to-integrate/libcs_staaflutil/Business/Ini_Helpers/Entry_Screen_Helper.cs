using Common;
using Fairweather.Service;

namespace DTA
{
    public class Entry_Screen_Helper : Write_Able_Helper
    {
        public Entry_Screen_Helper(Ini_File ini, Company_Number number)
            : base(ini, number) {

        }

        // ****************************


        public string Default_Details(bool customers, bool discount) {

            if (customers) {
                return discount ? Default_Sales_Discount_Details : Default_Sales_Details;
            }
            else {
                return discount ? Default_Purchase_Discount_Details : Default_Purchase_Details;
            }

        }


        public string Default_Purchase_Details {
            get {
                var field = DTA_Fields.ESF_purchase_receipt_details;

                var ret = String(field);

                return ret;
            }
        }

        public string Default_Purchase_Discount_Details {
            get {
                var field = DTA_Fields.ESF_purchase_discount_details;

                var ret = String(field);

                return ret;
            }
        }

        public string Default_Sales_Details {
            get {
                var field = DTA_Fields.ESF_sales_receipt_details;

                string ret = String(field);

                return ret;
            }
        }

        public string Default_Sales_Discount_Details {
            get {
                var field = DTA_Fields.ESF_sales_discount_details;

                string ret = String(field);

                return ret;
            }
        }

        // ****************************


        public int QSF_Bank_Sorting {
            get {
                return Int(DTA_Fields.QSF_bank_sort);
            }
        }

        public int QSF_Supplier_Sorting {
            get {
                return Int(DTA_Fields.QSF_supplier_sort);
            }
        }

        public int QSF_Customer_Sorting {
            get {
                return Int(DTA_Fields.QSF_customer_sort);
            }
        }
    }
}
