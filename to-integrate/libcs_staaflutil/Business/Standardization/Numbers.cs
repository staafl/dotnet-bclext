
#region Header
using System;
using System.Windows.Forms;
using System.Drawing;

using Fairweather.Service;

using Colors = Standardization.Colors;
using IOPair = Fairweather.Service.Pair<System.Action<string>, System.Func<string>>;

#endregion
/*       All future control standardization efforts go here        */
namespace Standardization
{
    static partial class Standard
    {
        /*       Todo:        */

        // also take care of the maximum length of the various text fields
        
        const string cst_decimal_format = "F2";
        const int cst_decimal_precision = 2;

        static public string Standard_String(this decimal dec) {

            var ret = dec.ToString(cst_decimal_format);
            return ret;

        }

        static public decimal Standard_Parse(this string to_dec) {

            var ret = to_dec.DecimalOrZero(cst_decimal_precision);
            return ret;

        }
    }
}