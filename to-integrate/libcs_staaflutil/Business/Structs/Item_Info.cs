using System;
using System.Runtime.Serialization;

using Fairweather.Service;

namespace Common
{
    // Only mutable for serialization purposes
    [Serializable]
    //[DebuggerStepThrough]
    /// Contains the entire information gathered from a row
    public struct Item_Info 
    {

        public Item_Info(Product product,
                decimal qty,
                string description,
                decimal brute_nv,
                decimal total_nv,
                decimal total_disc_nv,
                decimal base_price_nv,
                decimal discount_rate,
                decimal vat_amount):this() {

            Recalled = true;

            this.Product = product;

            this.Qty = qty;

            this.Description = description;

            this.Brute_NV = brute_nv;

            
            this.Total_NV = total_nv;

            this.Base_Price_NV = base_price_nv;

            this.Unit_Price_NV = Total_NV.Safe_Divide(qty);

            this.Unit_Discount_NV = total_disc_nv.Safe_Divide(qty);

            this.Discount_Perc = discount_rate;

            this.VAT = vat_amount;


            // 
            
            this.Total_V = Total_NV + VAT;

        }

        public Item_Info(Product product,
                        decimal qty,
                        string description,
                        decimal brute_v, decimal brute_nv,
                        decimal total_v, decimal total_nv,
                        decimal base_price_v, decimal base_price_nv,
                        decimal unit_price_v, decimal unit_price_nv,
                        decimal unit_disc_v, decimal unit_disc_nv,
                        decimal discount_rate,
                        decimal vat_amount)
            : this() {

            this.Product = product;
            this.Qty = qty;
            this.Description = description;


            this.Brute_V = brute_v;
            this.Brute_NV = brute_nv;

            this.Total_V = total_v;
            this.Total_NV = total_nv;


            this.Base_Price_V = base_price_v;

            this.Base_Price_NV = base_price_nv;

            this.Unit_Price_V = unit_price_v;
            this.Unit_Price_NV = unit_price_nv;

            this.Unit_Discount_V = unit_disc_v;
            this.Unit_Discount_NV = unit_disc_nv;

            this.Discount_Perc = discount_rate;

            this.VAT = vat_amount;

        }

        [OnDeserializing]
        public void OnDeserialize(StreamingContext context) {
            Deserialized = true;
        }

        // [XmlIgnore]
        public bool Deserialized { get; set; }
        public bool Recalled { get; set; }


        public Product Product {
            get;
            set;
        }

        public decimal Qty {
            get;
            set;
        }

        public string Description {
            get;
            set;

        }


        public decimal Base_Price_NV { get; set; }

        public decimal Base_Price_V { get; set; }

        // [XmlIgnore]
        public decimal Brute_V {
            get;
            set;
        }

        // [XmlIgnore]
        public decimal Brute_NV {
            get;
            set;
        }

        public decimal Total_V {
            get;
            set;
        }
        public decimal Total_NV {
            get;
            set;
        }

        public decimal Unit_Price_V {
            get;
            set;
        }

        public decimal Unit_Price_NV {
            get;
            set;
        }

        // [XmlIgnore]
        public decimal Unit_Discount_V {
            get;
            set;
        }

        // [XmlIgnore]
        public decimal Unit_Discount_NV {
            get;
            set;
        }

        // [XmlIgnore]
        public decimal Discount_Perc { get; set; }

        // [XmlIgnore]
        public decimal VAT {
            get;
            set;
        }

        // ****************************

        // [XmlIgnore]
        public decimal Adjusted_Discount_Perc {
            get {
                return Math.Max(Discount_Perc, 0.00m);
            }
        }

        /// <summary>
        /// For printing purposes: this is the Brute_V amount + the Total_Surcharge_V amount
        /// </summary>
        public decimal Adjusted_Brute_V {
            get {
                return Brute_V + Total_Surcharge_V;
            }
        }

        // [XmlIgnore]
        public decimal Adjusted_Base_Price_V {
            get {
                return Math.Max(Base_Price_V, Unit_Price_V);
            }
        }


        // [XmlIgnore]
        public decimal Total_Surcharge_V {
            get {
                var ret = Total_V - Brute_V;

                if (ret <= 0.0m)
                    return 0.0m;

                return ret;

                //var disc = Unit_Discount_V;
                //if (disc >= 0.00m)
                //    return 0.0m;

                //var ret = -disc * qty;
                //return ret;
            }
        }

        // ****************************


        // [XmlIgnore]
        /// <summary>
        /// For printing purposes: this is the Brute_NV amount + the Total_Surcharge_NV amount
        /// </summary>
        public decimal Adjusted_Brute_NV {
            get {
                return Brute_NV + Total_Surcharge_NV;
            }
        }

        // [XmlIgnore]
        public decimal Adjusted_Base_Price_NV {
            get {
                return Math.Max(Base_Price_NV, Unit_Price_NV);
            }
        }

        // [XmlIgnore]
        public decimal Total_Surcharge_NV {
            get {
                var ret = Total_NV - Brute_NV;

                if (ret <= 0.0m)
                    return 0.0m;

                return ret;

            }
        }

        // ****************************

        // [XmlIgnore]
        public decimal Total_Discount_NV {
            get {
                var ret = this.Adjusted_Brute_NV - this.Total_NV;

                return ret;
            }
        }

        // [XmlIgnore]
        public decimal Total_Discount_V {
            get {
                var ret = this.Adjusted_Brute_V - this.Total_V;

                return ret;
            }
        }
        // ****************************



    }
}
