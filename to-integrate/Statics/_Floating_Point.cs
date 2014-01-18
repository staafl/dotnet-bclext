
namespace Fairweather.Service
{
    public static class Floating_Point
    {
        // http://en.wikipedia.org/wiki/Kahan_summation_algorithm


        static public bool IsFinite(this double p_double) {

            if (p_double == 0.0)
                return true;

            var one = p_double / p_double;
            var ret = !double.IsNaN(one);

            return ret;

        }

        static bool BitComponents(this double p_double, out int sign, out int exp, out long significand) {

            var union = new Double64_Union(p_double);

            sign = union.Sign;
            exp = union.Exponent;
            significand = union.Significand;

            return p_double.IsFinite();

        }

        static bool FromBitComponents(int sign, int exp, int significand, out double result) {

            var union = new Double64_Union(sign, exp, significand);

            result = union.Double;

            return result.IsFinite();

        }

        static double Next_Representable(this double p_double) {

            //Double64_Union union;
            //Int64 as_long;

            var union = new Double64_Union(p_double);
            var as_ulong = union.UInt64;

            if (p_double < 0.0) {
                checked { --as_ulong; }
            }
            else if (p_double > 0.0) {
                checked { ++as_ulong; }
            }
            else {
                // Positive Zero
                if (as_ulong == 0)
                    as_ulong = 1;

                // Negative Zero
                else
                    as_ulong = 0;
            }

            union = new Double64_Union(as_ulong);

            return union.Double;

        }

        static double Prev_Representable(this double p_double) {

            var union = new Double64_Union(p_double);
            var as_ulong = union.UInt64;

            if (p_double < 0.0) {
                checked { ++as_ulong; }
            }
            else if (p_double > 0.0) {
                checked { --as_ulong; }
            }
            else {
                // Positive Zero
                if (as_ulong == 0)
                    as_ulong = (1ul << 63);

                // Negative Zero
                else
                    ++as_ulong;
            }

            union = new Double64_Union(as_ulong);

            return union.Double;

        }

        // http://www.cygnus-software.com/papers/comparingfloats/comparingfloats.htm
        static long Ulp_Difference(this double dbl1, double dbl2) {

            var union1 = new Double64_Union(dbl1);
            var as_ulong1 = B.AsSignMagnitude(union1.Int64);

            var union2 = new Double64_Union(dbl2);
            var as_ulong2 = B.AsSignMagnitude(union2.Int64);

            var diff = as_ulong1 - as_ulong2;

            return diff;


        }

    }
}
