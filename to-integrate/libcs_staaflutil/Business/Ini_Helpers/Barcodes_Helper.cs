using System;
using System.Text.RegularExpressions;

using Fairweather.Service;

using Common;

namespace DTA
{
    public class Barcodes_Helper : Ini_Helper
    {
        static readonly Regex rx_illegal = new Regex("[^0-9]", RegexOptions.IgnoreCase);

        public Barcodes_Helper(Ini_File ini, Company_Number company)
            : base(ini, company) {

        }




        public bool Use_Price_Barcodes {
            get {
                return True(DTA_Fields.POS_use_price_barcodes);
            }
        }

        public bool Use_Weight_Barcodes {
            get {
                return True(DTA_Fields.POS_use_weight_barcodes);
            }
        }


        string Price_Prefix {
            get {
                string ret = String(DTA_Fields.POS_price_prefix);
                return ret;
            }
        }

        string Weight_Prefix {
            get {
                string ret = String(DTA_Fields.POS_weight_prefix);
                return ret;
            }
        }


        int Price_Barcode_Length {
            get {
                return Int(DTA_Fields.POS_price_barcode_length);
            }
        }

        int Weight_Barcode_Length {
            get {
                return Int(DTA_Fields.POS_weight_barcode_length);
            }
        }

        int Price_Product_Start {
            get {
                return Int(DTA_Fields.POS_price_product_start);
            }
        }

        int Price_Product_Length {
            get {
                return Int(DTA_Fields.POS_price_product_length);
            }
        }


        int Weight_Product_Start {
            get {
                return Int(DTA_Fields.POS_weight_product_start);
            }
        }

        int Weight_Product_Length {
            get {
                return Int(DTA_Fields.POS_weight_product_length);
            }
        }





        int Price_Code_Start {
            get {
                return Int(DTA_Fields.POS_price_code_start) ;
            }
        }

        int Price_Code_Length {
            get {
                return Int(DTA_Fields.POS_price_code_length);
            }
        }

  
        int Weight_Code_Start {
            get {
                return Int(DTA_Fields.POS_weight_code_start);
            }
        }

        int Weight_Code_Length {
            get {
                return Int(DTA_Fields.POS_weight_code_length);
            }
        }


        int Weight_Decimal_Places {
            get {
                return Int(DTA_Fields.POS_weight_decimals);
            }
        }



        /*       Dumb        */
        public bool Get_Weight_Info(string code, out string product, out decimal weight) {

            return Get_Info(code, false, out product, out weight);

        }

        /*       Dumb        */
        public bool Get_Price_Info(string code, out string product, out decimal price) {

            return Get_Info(code, true, out product, out price);

        }



        bool Get_Info(string code, bool is_price, out string product, out decimal wt_price) {

            product = null;
            wt_price = default(decimal);

            if (!Is_Weight_Price_Barcode(code, is_price))
                return false;

            product = Get_Product_Code(code, is_price);

            var wt_price_code = Get_Price_Weight_Code(code, is_price);

            wt_price = decimal.Parse(wt_price_code);

            if (is_price)
                wt_price /= 100.0M;
            else
                wt_price /= (decimal)(Math.Pow(10.0, Weight_Decimal_Places));


            (wt_price >= 0.0M).tiff();
            product.IsNullOrEmpty().tift();

            return true;

        }



        /*       Dumb        */
        string Get_Price_Weight_Code(string code, bool is_price) {

            var start = is_price ? Price_Code_Start : Weight_Code_Start;
            var length = is_price ? Price_Code_Length : Weight_Code_Length;

            return Get_Code(code, start, length);

        }

        /*       Dumb        */
        string Get_Product_Code(string code, bool is_price) {

            var start = is_price ? Price_Product_Start : Weight_Product_Start;
            var length = is_price ? Price_Product_Length : Weight_Product_Length;

            return Get_Code(code, start, length);

        }

        string Get_Code(string code, int start, int length) {

            int code_length = code.Length;

            if (start + length > code_length)
                return null;

            var ret = code.Substring(start, length);

            return ret;

        }










        /*       Dumb        */
        public bool Is_Weight_Price_Barcode(string code) {

            return Is_Weight_Price_Barcode(code, true) ||
                   Is_Weight_Price_Barcode(code, false);

        }


        /*       Dumb        */
        /// <summary>
        /// Returns false if Use_Price_Barcodes is false
        /// </summary>
        public bool Is_Price_Barcode(string code) {

            return Is_Weight_Price_Barcode(code, true);

        }

        /*       Dumb        */
        /// <summary>
        /// Returns false if Use_Weight_Barcodes is false
        /// </summary>
        public bool Is_Weight_Barcode(string code) {

            return Is_Weight_Price_Barcode(code, false);

        }


        bool Is_Weight_Price_Barcode(string code, bool is_price) {

            if (is_price && !Use_Price_Barcodes)
                return false;
            else if (!is_price && !Use_Weight_Barcodes)
                return false;

            int len = is_price ? Price_Barcode_Length : Weight_Barcode_Length;
            if (code.Length != len)
                return false;

            var prefix = is_price ? Price_Prefix : Weight_Prefix;
            if (!code.StartsWith(prefix))
                return false;

            var product = Get_Product_Code(code, is_price);

            if (product.IsNullOrEmpty())
                return false;

            var wt_price = Get_Price_Weight_Code(code, is_price);

            if (wt_price.IsNullOrEmpty())
                return false;


            if (wt_price.Match(rx_illegal).Success)
                return false;

            return true;

        }



    }
}