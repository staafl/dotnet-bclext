using System;
using System.Collections.Generic;
using System.Diagnostics;

using Fairweather.Service;

namespace Common
{
    [DebuggerStepThrough]
    [Serializable]
    public struct Product_Index
    {
        public Product_Index(Product product, Index_Type indexType) {
            this.m_product = product;
            this.m_indexType = indexType;
        }

        readonly Index_Type m_indexType;

        readonly Product m_product;

        public Product Product {
            get { return m_product; }
        }

        static Dictionary<Index_Type, Func<Product, string>>
        st_index_getter = new Dictionary<Index_Type, Func<Product, string>>
        {
            {Index_Type.Stock, (prod) => prod.Code},
            {Index_Type.Bar1, (prod) => prod.Barcode_1},
            {Index_Type.Bar2, (prod) => prod.Barcode_2},
            {Index_Type.Bar3, (prod) => prod.Barcode_3},
            {Index_Type.Bar16, (prod) => prod.Barcode_16},

            
        };

        public string IndexString {
            get {
                try {
                    return st_index_getter[m_indexType](m_product);
                }
                catch (System.Collections.Generic.KeyNotFoundException) {
                    string message = "ProductIndex has unrecognized index type: " + m_indexType.Get_String();
                    true.tift<InvalidOperationException>(message);
                    return null;
                }
            }
        }



        /* Boilerplate */

        public override string ToString() {

            string ret = "";

            ret += "indexType = " + this.m_indexType;
            ret += ", ";
            ret += "product = " + this.m_product;

            ret = "{ProductIndex: " + ret + "}";
            return ret;

        }

        public bool Equals(Product_Index obj2) {

            if (!this.m_indexType.Equals(obj2.m_indexType))
                return false;

            var prod1 = this.m_product;
            var prod2 = obj2.m_product;

            // Here I'm allowing the product class to consider
            // a non-null instance equal to null. Is this a good idea?

            if (prod1 == null) {

                if (prod2 != null
                    /*
                    )
                    /*/
                    && !prod2.Equals(prod1))
                    //*/

                    return false;

            }
            else if (!prod1.Equals(prod2))
                return false;

            return true;
        }

        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Product_Index);

            if (ret)
                ret = this.Equals((Product_Index)obj2);


            return ret;

        }

        public static bool operator ==(Product_Index left, Product_Index right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Product_Index left, Product_Index right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public override int GetHashCode() {

            unchecked {
                int ret = 23;
                int temp;

                ret *= 31;
                temp = this.m_indexType.GetHashCode();
                ret += temp;

                ret *= 31;
                temp = this.m_product.GetHashCode();
                ret += temp;

                return ret;
            }
        }
    }
}



