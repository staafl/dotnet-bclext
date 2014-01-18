
using System;
using System.Diagnostics;

using Fairweather.Service;



namespace Common
{
    [Serializable]
    [DebuggerStepThrough]
    public class Product
    {
        public Product(string product_code,
            string barcode_1,
            string barcode_2,
            string barcode_3,
            string barcode_16,
            string description,
            string category,
            decimal price,
            short tax_code) {

            this.m_product_code = product_code;
            this.m_barcode_1 = barcode_1;
            this.m_barcode_2 = barcode_2;
            this.m_barcode_3 = barcode_3;
            this.m_barcode_16 = barcode_16;

            this.m_description = description;
            this.m_category = category;
            this.m_price = price;
            this.m_tax_code = new Tax_Code(tax_code);
        }

        #region Product

        readonly string m_product_code;

        readonly string m_barcode_1;

        readonly string m_barcode_2;

        readonly string m_barcode_3;

        readonly string m_barcode_16;


        readonly string m_description;

        readonly string m_category;

        readonly decimal m_price;

        readonly Tax_Code m_tax_code;

        #endregion


        public Tax_Code Tax_Code {
            get { return m_tax_code; }
        }

        public string Code {
            get {
                return this.m_product_code;
            }
        }

        public string Barcode_1 {
            get {
                return this.m_barcode_1;
            }
        }

        public string Barcode_2 {
            get {
                return this.m_barcode_2;
            }
        }

        public string Barcode_3 {
            get {
                return this.m_barcode_3;
            }
        }

        public string Barcode_16 {
            get {
                return this.m_barcode_16;
            }
        }

        public string Description {
            get {
                return this.m_description;
            }
        }

        public string Category {
            get {
                return this.m_category;
            }
        }

        /// <summary>
        /// Sales price, VAT exclusive
        /// </summary>
        public decimal Price {
            get {
                return this.m_price;
            }
        }

        public short Tax_Code_ID {
            get {
                return this.m_tax_code.Tax_Code_ID;
            }
        }

        /// The VAT percentage applicable to this item expressed as a percentage E [0.0m, 100.0m]
        public decimal VAT_Percentage {
            get {
                return this.m_tax_code.VAT_Percentage;
            }
        }

        /// <summary>
        /// The VAT percentage applicable to this item expressed as a fraction E [0.0m, 1.0m]
        /// </summary>
        public decimal VAT_Ratio {
            get {
                return this.m_tax_code.VAT_Percentage / 100.0m;

            }
        }

        public string
        Category_Or_Description(bool prefer_category) {

            string ret, other;

            if (prefer_category) {
                ret = Category;
                other = Description;
            }
            else {
                ret = Description;
                other = Category;
            }


            if (ret.IsNullOrEmpty())
                ret = other;

            return ret ?? "";
        }



    }

    #region old version - 08/10/09
    //[Serializable]
    //[DebuggerStepThrough]
    //public struct Product
    //{

    //    readonly string m_product_code;

    //    readonly string m_barcode_1;

    //    readonly string m_barcode_2;

    //    readonly string m_barcode_3;

    //    readonly string m_desc;

    //    readonly string m_category;

    //    readonly double m_price;

    //    public string ProductCode {
    //        get {
    //            return this.m_product_code;
    //        }
    //    }

    //    public string BarCode1 {
    //        get {
    //            return this.m_barcode_1;
    //        }
    //    }

    //    public string BarCode2 {
    //        get {
    //            return this.m_barcode_2;
    //        }
    //    }

    //    public string BarCode3 {
    //        get {
    //            return this.m_barcode_3;
    //        }
    //    }

    //    public string Description {
    //        get {
    //            return this.m_desc;
    //        }
    //    }

    //    public string Category {
    //        get {
    //            return this.m_category;
    //        }
    //    }

    //    public double Price {
    //        get {
    //            return this.m_price;
    //        }
    //    }

    //    /*       Buggy array-based version replaced in revision        */
    //    /*       750        */

    //    public Product(string product_code,
    //                    string barcode_1,
    //                    string barcode_2,
    //                    string barcode_3,
    //                    string desc,
    //                    string category,
    //                    double price)
    //    {
    //        this.m_product_code = product_code;
    //        this.m_barcode_1 = barcode_1;
    //        this.m_barcode_2 = barcode_2;
    //        this.m_barcode_3 = barcode_3;
    //        this.m_desc = desc;
    //        this.m_category = category;
    //        this.m_price = price;
    //    }


    //    /* Boilerplate */

    //    #region Product

    //    public override string ToString() {

    //        string ret = "";

    //        ret += "product_code = " + this.m_product_code;
    //        ret += ", ";
    //        ret += "barcode_1 = " + this.m_barcode_1;
    //        ret += ", ";
    //        ret += "barcode_2 = " + this.m_barcode_2;
    //        ret += ", ";
    //        ret += "barcode_3 = " + this.m_barcode_3;
    //        ret += ", ";
    //        ret += "desc = " + this.m_desc;
    //        ret += ", ";
    //        ret += "category = " + this.m_category;
    //        ret += ", ";
    //        ret += "price = " + this.m_price;

    //        ret = "{Product: " + ret + "}";
    //        return ret;

    //    }

    //    public bool Equals(Product obj2) {

    //        if (!this.m_product_code.Equals(obj2.m_product_code))
    //            return false;

    //        if (!this.m_barcode_1.Equals(obj2.m_barcode_1))
    //            return false;

    //        if (!this.m_barcode_2.Equals(obj2.m_barcode_2))
    //            return false;

    //        if (!this.m_barcode_3.Equals(obj2.m_barcode_3))
    //            return false;

    //        if (!this.m_desc.Equals(obj2.m_desc))
    //            return false;

    //        if (!this.m_category.Equals(obj2.m_category))
    //            return false;

    //        if (!this.m_price.Equals(obj2.m_price))
    //            return false;

    //        return true;
    //    }

    //    public override bool Equals(object obj2) {

    //        var ret = (obj2 != null && obj2 is Product);

    //        if (ret)
    //            ret = this.Equals((Product)obj2);


    //        return ret;

    //    }

    //    public static bool operator ==(Product left, Product right) {

    //        var ret = left.Equals(right);
    //        return ret;

    //    }

    //    public static bool operator !=(Product left, Product right) {

    //        var ret = !left.Equals(right);
    //        return ret;

    //    }

    //    public override int GetHashCode() {

    //        unchecked {
    //            int ret = 23;
    //            int temp;

    //            ret *= 31;
    //            temp = this.m_product_code.GetHashCode();
    //            ret += temp;

    //            ret *= 31;
    //            temp = this.m_barcode_1.GetHashCode();
    //            ret += temp;

    //            ret *= 31;
    //            temp = this.m_barcode_2.GetHashCode();
    //            ret += temp;

    //            ret *= 31;
    //            temp = this.m_barcode_3.GetHashCode();
    //            ret += temp;

    //            ret *= 31;
    //            temp = this.m_desc.GetHashCode();
    //            ret += temp;

    //            ret *= 31;
    //            temp = this.m_category.GetHashCode();
    //            ret += temp;

    //            ret *= 31;
    //            temp = this.m_price.GetHashCode();
    //            ret += temp;

    //            return ret;
    //        }
    //    }

    //    #endregion
    //}
    #endregion

}