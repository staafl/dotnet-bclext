

using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;


namespace Fairweather.Service
{


    
    // [DebuggerStepThrough]
    /// Arithmetic, Algebra & misc numeric methods
    public static class Arithmetic
    {
    
        /// <summary>
        /// returns the smallest integer >= 'num' divided evenly by 'd'
        /// <summary>
        public static int DivisionPad(int num, int d) {
            var r = num % d;
            if(r == 0) {
                return num;
            }
            return num + d - r;
        }
        public static string  
Convert_To_Base(int number, int num_base, Func<int, char> get_digit) {
    
    (number >= 0).tiff();
    (num_base >= 2).tiff();
    
    var ret = "";
    while(number >= 0) {
        ret = get_digit(number % num_base) + ret;
        number /= num_base;
        if(number == 0)
            break;
    }
    
    return ret;
    
}
 



        /*       These things are here to avoid writing out the delegates every time        */

        public static decimal Add(decimal addend, decimal augend) { return addend + augend; }
        public static decimal Sub(decimal shend, decimal mend) { return shend - mend; }
        public static decimal Mul(decimal mcand, decimal mcator) { return mcand * mcator; }
        public static decimal Div(decimal ddend, decimal dsor) { return ddend / dsor; }

        //I might decide to roll my own square-root procedure
        public static decimal SqRt(decimal radicand) { return (decimal)Math.Sqrt((double)radicand); }

        // ****************************

        public static long LCD(long long1, long long2, out long gcd) {

            gcd = A.GCD(long1, long2);

            bool first_greater = long1 >= long2;


            var ret = checked(long1 * (long2 / gcd));

            return ret;
        }

        // Does it work for negatives?
        public static int GCD(int int1, int int2) {

            //(int1 > 0).tiff();
            //(int2 > 0).tiff();

            int1 = Math.Abs(int1);
            int2 = Math.Abs(int2);

            if (int1 == int2)
                return int1;

            var tmp1 = Math.Max(int1, int2);
            var tmp2 = Math.Min(int1, int2);

            while (tmp2 > 0) {

                var tmp3 = tmp1 % tmp2;
                tmp1 = tmp2;
                tmp2 = tmp3;

            }

            return tmp1;

        }

        public static long GCD(long long1, long long2) {

            //(long1 > 0).tiff();
            //(long2 > 0).tiff();
            //
            long1 = Math.Abs(long1);
            long2 = Math.Abs(long2);

            if (long1 == long2)
                return long1;

            var tmp1 = Math.Max(long1, long2);
            var tmp2 = Math.Min(long1, long2);

            while (tmp2 > 0) {

                var tmp3 = tmp1 % tmp2;
                tmp1 = tmp2;
                tmp2 = tmp3;

            }

            return tmp1;

        }

        // needs a spec and some attention
        public static int
        Round_Divide(this int divident, int divisor){
            
            int ret = divident / divisor;
            if(divident % divisor > (divisor / 2))
                ++ret;
             return ret;
            // check eric lippert's answer on SO            
        }
        
        /// <summary>
        /// Division which defines x/0.0 to equal 0.0.
        /// </summary>
        /// <param name="divident"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static decimal
        Safe_Divide(this decimal divident, decimal divisor) {

            if (divisor == 0.0m)
                return 0.0m;

            return divident / divisor;

        }

        // In: decimal number d1, rounded to p1 decimal places, divident
        //     decimal number d2, divisor
        //     integer p1, decimal precision of the quotient
        //     integer p2, decimal precision of the divident

        // Summary: This routine computes a rounded quotient q of d1 and d2, 
        // such that [q * d2, p1] = d1 and q is equal to d1/d2 until at least 
        // quotient_precision decimal places

        // Out: Wether the function succeeded in finding an appropriate value.
        static bool
        Round_Trip_Divide(decimal d1,
                          decimal d2,
                          int p1,
                          int p2,
                          out decimal q) {

            throw new NotImplementedException();
            //d1 d2 q
            //[d1/d2]*d2 != q?

            //var r1 = d1 / d2;
            //var rounded_d1 = d1.RoundUp(d1_precision);
            //var rnd1 = r1.RoundUp(d1_precision);

            //int cmp = (rnd1 * d2).CompareTo(rounded_d1, d1_precision);

            //if(cmp == 0)
            //  return rnd1;

            //if(cmp == 1)


        }


        internal const int int_size_bits = sizeof(int) * 8;
        internal const int int_sign_bit = 1 << (int_size_bits - 1);



        // Will_Overflow for int32 and integers with arbitrary bounds
        /// commented - out
        /// 
        //public static bool 
        // Will_Overflow(long mcand, long mcator, long maxpos, long minneg)         
        //{

        //    var sign = Math.Sign(multiplicand) *
        //               Math.Sign(multiplicator);

        //    // One or both are 0
        //    if (sign == 0)
        //        return false;

        //    // maximum allowed absolute value * -1
        //    // aka extremum
        //    long ext = (sign > 0) ? -maxpos : minneg;

        //    if (multiplicand == minneg)
        //        return multiplicator != 1;

        //    if (multiplicator == minneg)
        //        return multiplicand != 1;

        //    // Math.Abs(minneg) is handled
        //    var m1 = -Math.Abs(multiplicand);
        //    var m2 = -Math.Abs(multiplicator);

        //    // since m1 != minneg, m2 == -1
        //    // means no overflow can occur
        //    if (m2 == -1)
        //        return false;

        //    // minneg / -1 is handled
        //    var quot = -Math.Abs(ext / m2); // m2 is chosen arbitrarily



        //    bool overflow = m1 < quot;

        //    // m1, quot and ext are all negative ==>
        //    // (m1 < quot <==> |m1| > |quot|)
        //    // |m1| > |quot| ==> |m1| * |m2| > |ext|
        //    // 
        //    // quot = trunc(ext/m2) => we have
        //    //     |m1| > |trunc(ext/m2)|, =>
        //    //     |m1| >= |ceil(ext/m2)|
        //    //     if |m1| > |...|, then we have overflow
        //    //     if |m1| == |...|, then either |m1| == (ext/m2)
        //    //                            or |m1| > (ext/m2)
        //    //                        the first case is impossible, since m1 > trunc(ext/m2)
        //    //                        the second means overflow
        //    // therefore, m1 < quot ==> |m1 * m2| > |ext|

        //    return overflow;

        //}

        //public static bool Will_Overflow(int mcand, int mcator) {

        //    return Will_Overflow(((long)mcand), ((long)mcator), int.MaxValue, int.MinValue);

        //}

        //public static bool Will_Overflow(long mcand, long mcator) {

        //    return Will_Overflow(mcand, mcator, long.MaxValue, long.MinValue);

        //}


        [Obsolete("Not tested")]
        // Tests whether multiplying two int64 numbers will cause overflow,
        // without computing their product
        public static bool
        Will_Overflow(long multiplicand, long multiplicator) {

            var sign = Math.Sign(multiplicand) *
                       Math.Sign(multiplicator);

            // One or both are 0
            if (sign == 0)
                return false;

            // maximum allowed absolute value * -1
            // aka extremum
            long ext = (sign > 0) ? -long.MaxValue
                                 : long.MinValue;

            if (multiplicand == long.MinValue)
                return multiplicator != 1;

            if (multiplicator == long.MinValue)
                return multiplicand != 1;

            // Math.Abs(long.MinValue) is handled
            var m1 = -Math.Abs(multiplicand);
            var m2 = -Math.Abs(multiplicator);

            // since m1 != long.MinValue, m2 == -1
            // means no overflow can occur
            if (m2 == -1)
                return false;

            // long.MinValue / -1 is handled
            var quot = -Math.Abs(ext / m2); // m2 is chosen arbitrarily



            bool overflow = m1 < quot;

            // m1, quot and ext are all negative ==>
            // (m1 < quot <==> |m1| > |quot|)
            // |m1| > |quot| ==> |m1| * |m2| > |ext|
            // 
            // quot = trunc(ext/m2) => we have
            //     |m1| > |trunc(ext/m2)|, =>
            //     |m1| >= |ceil(ext/m2)|
            //     if |m1| > |...|, then we have overflow
            //     if |m1| == |...|, then either |m1| == (ext/m2)
            //                            or |m1| > (ext/m2)
            //                        the first case is impossible, since m1 > trunc(ext/m2)
            //                        the second means overflow
            // therefore, m1 < quot ==> |m1 * m2| > |ext|

            return overflow;

        }

        public static IEnumerable<double> Scale_To_One(this IEnumerable<double> doubles) {

            var forced = doubles.Force();

            var sum = forced.Sum();

            var ret = forced.Select(dbl => dbl / sum);

            return ret;

        }


        public static IEnumerable<int> Coprimes(int number) {

            for (int ii = 1; ii < number; ++ii) {
                if (A.GCD(ii, number) == 1)
                    yield return ii;

            }

        }



        // ****************************



        /// <summary>
        /// Not tested on negatives
        /// </summary>
        public static int Next_Power_Of_2(int x) {

            // http://stackoverflow.com/questions/364985/algorithm-for-finding-the-smallest-power-of-two-thats-greater-or-equal-to-a-give
            // http://stackoverflow.com/questions/1322510/bit-twiddling-find-next-power-of-two

            if (x == 0)
                return 0;

            bool sign = (x & int_sign_bit) == 1;
            if (sign)
                x *= -1;

            --x;

            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;

            ++x;

            if (x <= 0)
                true.tift("Next_Power_Of_2: overflow");

            return sign ? -x : x;

        }

        public static int Log2(int x) {

            // TODO: investigate situation with shifting negative
            // int32s

            //http://aggregate.org/MAGIC

            if (x <= 0)
                return -1;

            var pow = Next_Power_Of_2(x);
            int ret = B.Hamming_Weight(pow - 1);

            if (pow != x)
                --ret;

            return ret;

        }


        public static int Log2_Branch(uint x) {

            // http://graphics.stanford.edu/~seander/bithacks.html

            uint v = x;
            int ret = 0;
            uint[] masks = { 0x2, 0xC, 0xF0, 0xFF00, 0xFFFF0000 };
            int[] shifts = { 1, 2, 4, 8, 16 };

            for (int ii = 4; ii >= 0; ii--) // unroll for speed...
            {
                if ((v & masks[ii]) != 0) {
                    v >>= shifts[ii];
                    ret |= shifts[ii];
                }
            }

            return ret;
        }


        public static int Log2_Nobranch(int x) {

            // http://graphics.stanford.edu/~seander/bithacks.html

            int v = x;
            int ret; // result of log2(v) will go here
            int shift;

            ret = (v > 0xFFFF) ? 1 : 0 << 4; v >>= ret;
            shift = (v > 0xFF) ? 1 : 0 << 3; v >>= shift; ret |= shift;
            shift = (v > 0xF) ? 1 : 0 << 2; v >>= shift; ret |= shift;
            shift = (v > 0x3) ? 1 : 0 << 1; v >>= shift; ret |= shift;
            ret |= (v >> 1);

            // Original "no-branches" C version
            //ret = (v > 0xFFFF) << 4; v >>= ret;
            //shift = (v > 0xFF) << 3; v >>= shift; ret |= shift;
            //shift = (v > 0xF) << 2; v >>= shift; ret |= shift;
            //shift = (v > 0x3) << 1; v >>= shift; ret |= shift;
            //ret |= (v >> 1);

            return ret;

        }


        // ****************************


        // Returns 0 digits for 0
        public static int Number_Of_Digits(int number, int num_base) {

            (num_base <= 1).tift();
            (number < 0).tift();

            int ret = 0;

            while (number >= 1) {
                ++ret;
                number /= num_base;
            }

            return ret;

        }

        // Positive positions are from the left, negative - from the right        
        // 0 is illegal        
        public static int Nth_Digit(int number, int position, int num_base) {

            (num_base <= 1).tift();
            (number < 0).tift();
            (position == 0).tift();

            do {
                if (position < 0) {

                    position = -position;

                    number /= num_base.Pow(position - 1);

                    if (number == 0)
                        return -1;

                    return number % num_base;
                }
                else {
                    int num = Number_Of_Digits(number, num_base);
                    position = num - position;
                    position = -position;

                    continue;
                }

            } while (true);
        }

        public static int Ring_Modulo(int divident, int divisor) {

            (divisor == 0).tift();

            bool first = divident >= 0;
            bool second = divisor >= 0;

            int ret = ret = divident % divisor;

            if (first && second) { }
            else if (first && !second) {
                ret = divisor - ret;
            }
            else if (!first && second) {
                ret = -ret;
            }
            else {
                ret = divisor + ret;
            }

            return ret;

        }

        public static int Min_Max(this int value, int min, int max) {

            (max < min).tift();

            var ret = Math.Min(value, max);
            ret = Math.Max(ret, min);

            return ret;
        }

        public static decimal Min_Max(this decimal value, decimal min, decimal max) {

            (max < min).tift();

            var ret = Math.Min(value, max);
            ret = Math.Max(ret, min);

            return ret;
        }

        // ****************************



        public static double Avg(params double[] doubles) {


            (doubles).tifn();
            (doubles).Is_Empty().tift();

            int len = doubles.Length;

            var sum = doubles.Sum();

            var ret = sum / len;

            return ret;

        }

        public static decimal Avg(params decimal[] decimals) {

            (decimals).tifn();
            (decimals).Is_Empty().tift();

            int len = decimals.Length;

            var sum = decimals.Sum();

            var ret = sum / len;

            return ret;

        }

        public static decimal Avg(params int[] ints) {

            checked {

                (ints).tifn();
                (ints).Is_Empty().tift();

                int len = ints.Length;

                var sum = ints.Sum();

                var ret = sum / len;

                return ret;

            }
        }


        /*       Test        */
        public static decimal Sum_Of_Geometric(decimal a_base, int a_max_exp) {

            var numerator = Pow(a_base, a_max_exp) - 1;
            var denominator = a_base - 1;

            var ret = numerator / denominator;

            return ret;

        }

        public static int Sum_Of_Geometric(int a_base, int a_max_exp) {

            checked {

                var numerator = Pow(a_base, a_max_exp + 1) - 1;
                var denominator = a_base - 1;

                var ret = numerator / denominator;

                (ret * denominator == numerator).tift();

                return ret;

            }
        }


        // ****************************



        public static int Pow(this int a_base, int a_exp) {

            var original = new Pair<int>(a_base, a_exp);
            checked {
                //http://stackoverflow.com/questions/101439/the-most-efficient-way-to-implement-an-integer-based-power-function-powint-int
                //http://stackoverflow.com/questions/383587/how-do-you-do-integer-exponentiation-in-c

                int ret = 1;

                while (a_exp != 0) {

                    if ((a_exp & 1) == 1)
                        ret *= a_base;

                    a_exp >>= 1;
                    a_base *= a_base;
                }

                return ret;
            }
        }

        public static long Pow(this long a_base, int a_exp) {

            checked {
                //http://stackoverflow.com/questions/101439/the-most-efficient-way-to-implement-an-integer-based-power-function-powint-int
                //http://stackoverflow.com/questions/383587/how-do-you-do-integer-exponentiation-in-c

                long ret = 1;

                while (a_exp != 0) {

                    if ((a_exp & 1) == 1)
                        ret *= a_base;

                    a_exp >>= 1;
                    a_base *= a_base;
                }

                return ret;
            }
        }



        /*       Test        */
        public static decimal Pow(this decimal a_base, int a_exp) {

            bool neg = false;
            decimal ret = 1.0M/*%*/;


            if (a_exp < 0) {

                a_exp = -a_exp;
                neg = true;

            }

            while (a_exp >= 1) {


                if (a_exp % 2 == 0) {

                    a_base *= a_base;

                    a_exp /= 2;
                }
                else {


                    ret *= a_base;
                    --a_exp;
                }
            }

            if (neg)
                ret = -ret;

            return ret;
        }


        // ****************************



        static public double DoubleOrZero(this string str, int? precision) {

            if (str.IsNullOrEmpty())
                return 0.0;

            double ret;

            if (!double.TryParse(str, out ret))
                return 0.0;

            if (precision.HasValue)
                ret = Math.Round(ret, precision.Value);

            return ret;

        }

        static public decimal DecimalOrZero(this string str, int? precision) {

            if (str.IsNullOrEmpty())
                return 0.0m;

            decimal ret;

            if (!decimal.TryParse(str, out ret))
                return 0.0m;

            if (precision.HasValue)
                ret = Math.Round(ret, precision.Value);

            return ret;

        }



        // ****************************


        public static double RoundToEven(this double dbl, int precision) {

            double ret = Math.Round(dbl, precision, MidpointRounding.ToEven);

            return ret;

        }

        public static double RoundDown(this double dbl, int precision) {

            if (precision > 4)
                throw new ArgumentOutOfRangeException("This function cannot handle rounding with more than 4 decimal digits.");

            if (precision == 0)
                return Math.Truncate(dbl);

            double ret = Math.Round(dbl, precision, MidpointRounding.AwayFromZero);

            if (ret > dbl) {
                double offset = 0.5;
                int prec = precision;

                while (prec > 0) {
                    --prec;
                    offset /= 8.0;
                }

                ret -= offset;
                ret = Math.Round(ret, precision, MidpointRounding.AwayFromZero);
                Debug.Assert(ret < dbl);
                Debug.Assert(dbl - ret < Math.Pow(10, -precision + 1));
            }

            return ret;
        }


        public static double RoundAwayFrom0(this double dbl, int precision) {

            double ret = Math.Round(dbl, precision, MidpointRounding.AwayFromZero);

            return ret;
        }


        public static decimal RoundToEven(this decimal dbl, int precision) {

            decimal ret = Math.Round(dbl, precision, MidpointRounding.ToEven);

            return ret;

        }

        public static decimal RoundDown(this decimal dbl, int precision) {

            if (precision > 4)
                throw new ArgumentOutOfRangeException("This function cannot handle rounding to more than 4 decimal places.");

            if (precision == 0)
                return Math.Truncate(dbl);

            decimal ret = Math.Round(dbl, precision, MidpointRounding.AwayFromZero);

            if (ret > dbl) {
                decimal offset = 0.5M/*%*/;
                int prec = precision;

                while (prec > 0) {
                    --prec;
                    offset /= 8.0M/*%*/;
                }

                ret -= offset;
                ret = Math.Round(ret, precision, MidpointRounding.AwayFromZero);
                Debug.Assert(ret < dbl);
                Debug.Assert(dbl - ret < (decimal)/*&*/Math.Pow(10.0, -precision + 1));
            }

            return ret;
        }



        /// <summary> Test before using
        /// </summary>
        public static decimal RoundUp(this decimal dec, int precision) {

            bool sign = dec < 0.0m;

            decimal ret;
            if (sign) {
                ret = RoundDown(Math.Abs(dec), precision);
                return -ret;
            }

            ret = Math.Round(dec, precision, MidpointRounding.AwayFromZero);

            return ret;
        }

        public static decimal RoundAwayFrom0(this decimal dbl, int precision) {

            decimal ret = Math.Round(dbl, precision, MidpointRounding.AwayFromZero);

            return ret;
        }


        // ****************************


        public static double Min(params double[] comparands) {
            double min = comparands[0];
            for (int ii = 1; ii < comparands.Length; ++ii)
                min = Math.Min(min, comparands[ii]);

            return min;
        }

        public static decimal Min(params decimal[] comparands) {

            decimal min = comparands[0];
            for (int ii = 1; ii < comparands.Length; ++ii)
                min = Math.Min(min, comparands[ii]);

            return min;
        }
    }
}
