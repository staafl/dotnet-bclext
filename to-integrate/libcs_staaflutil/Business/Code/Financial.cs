using System;
namespace Screens
{
      // Helpers
      //[DebuggerStepThrough]
      static public class Financial
      {
            static public decimal
            Apply_Vat(decimal amount,
                          decimal vat_ratio) {

                  decimal ret = amount + amount * vat_ratio;

                  return ret;

            }

            static public decimal
            Remove_Vat(decimal totals,
                           decimal vat_ratio) {

                  decimal ret = totals / (1.0m + vat_ratio);

                  return ret;

            }

            static public decimal
         Vat_Amount(decimal amount, decimal vat_ratio) {
                  var ret = amount * vat_ratio;
                  return ret;
            }

            static public decimal
            Total_Discount_Percentage(decimal totals,
                                              decimal discounts) {

                  if (totals <= 0.0M/*%*/)
                        return 0.0M/*%*/;

                  decimal ret = Math.Round(discounts * 100.0M/*%*/, 2, MidpointRounding.ToEven) / totals;

                  return ret;

            }

            static public decimal
            Get_Value_From_Vat(decimal vat,
                                   decimal vat_ratio) {


                  var ret = vat / vat_ratio;

                  return ret;

            }
      }
}