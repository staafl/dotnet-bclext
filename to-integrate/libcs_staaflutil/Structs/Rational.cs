using System;
using System.Collections.Generic;
using System.Globalization;


namespace Fairweather.Service
{
    // In case of precision trouble, search for "Precision"
    ///
    // Full credit goes to atoenne
    // http://www.codeproject.com/KB/recipes/submissionfraction.aspx
    // "Fraction numbers
    // (C) 2004 Andreas Tцnne, All right reserved
    // No GPL, no license fee!
    // Anyone can use this source provided that this copyright stays in place

    // Version 1.1 Bugfix release
    // - new limit properties MaxValue etc.
    // - checked for these limits in the constructor (Int64.MinValue slipped through)
    // - CompareTo: equality corrected (a case was missing)
    // - CompareTo: inqueality of very large fractions lead to OverflowExceptions
    // -			added 128bit comparison in these cases only"
    ///
    // Another fractional implementation:
    // http://www.codeproject.com/KB/recipes/fractiion.aspx
    ///
    // Todo: add clscompliant attributes
    // Todo: add real conversion from Double (check the other fractional class)
    ///
    // Done: fixed issue with conversion from negative decimal

    public struct Rational : IComparable, IFormattable
    {
        readonly static Dictionary<decimal, Rational> rd_common_fractions;

        static Rational() {

            // collecting all fractions < 1000 takes 300000 entries
            // collecting all fractions < 500 takes 75000 entries
            rd_common_fractions = new Dictionary<decimal, Rational>(20000);

            for (int ii = 2; ii < 300; ++ii) {
                //
                //http://en.wikipedia.org/wiki/Coprime
                //
                foreach (var coprime in A.Coprimes(ii)) {

                    var dec = (decimal)coprime / (decimal)ii;
                    rd_common_fractions[dec] = new Rational(coprime, ii, false);

                }
            }

            var equal_to_1 = (1m / 3m) * 3m;
            rd_common_fractions.Add(equal_to_1, Unit);
        }

        readonly long m_numer;

        readonly long m_denom;

        public long Numerator {
            get {
                return this.m_numer;
            }
        }

        public long Denominator {
            get {
                return this.m_denom;
            }
        }

        public decimal Arc {
            get { throw new NotImplementedException(); }
        }

        public decimal Radius {
            get { throw new NotImplementedException(); }
        }

        public Rational(long numer,
                        long denom)
            : this(numer, denom, true) { }

        Rational(long numer,
                 long denom,
                 bool reduce) {

            (denom == 0).tift();

            // Temporary
            //(numer < 0).tift();
            //(denom < 0).tift();

            (numer == Int64.MinValue).tift();
            (denom == Int64.MinValue).tift();
            // or
            // if(numer == Int64.MinValue){
            //    ++numer; reduce = true;
            //}
            // if(denom == Int64.MinValue){
            //    ++denom; reduce = true;
            //}

            Normalize(ref numer, ref denom);

            if (reduce)
                Reduce(ref numer, ref denom);

            this.m_numer = numer;
            this.m_denom = denom;

        }


        /* Boilerplate */


        static void Scale_Numerators(long numer1, long numer2, long denom1, long denom2,
                                                     out long lcd, out long sc_numer1, out long sc_numer2) {

            long temp1 = 0, temp2 = 0, _;

            //lcd = Algorithms.LCD(denom1, denom2, out _);

            while (true) {
                try {
                    lcd = A.LCD(denom1, denom2, out _);
                    break;

                }
                catch (OverflowException) {

                    if (Math.Abs(denom1) > Math.Abs(denom2)) {

                        denom1 /= 2;
                        numer1 /= 2;

                        if (denom1 == 0)
                            throw;
                    }
                    else {

                        denom2 /= 2;
                        numer2 /= 2;

                        if (denom2 == 0)
                            throw;
                    }
                    continue;
                }
            }

            var scale1 = lcd / denom1;
            var scale2 = lcd / denom2;

            while (true) {
                try {
                    checked {
                        temp1 = numer1 * scale1;
                    }
                    break;
                }
                catch (OverflowException) {

                    if (Math.Abs(numer1) > Math.Abs(scale1))
                        numer1 /= 2;
                    else
                        scale1 /= 2;

                    if (Math.Abs(numer2) > Math.Abs(scale2))
                        numer2 /= 2;
                    else
                        scale2 /= 2;

                    lcd /= 2;

                    if (lcd == 0)
                        throw;
                }
            }

            while (true) {
                try {
                    checked {
                        temp2 = numer2 * scale2;
                    }
                    break;
                }
                catch (OverflowException) {

                    if (Math.Abs(numer1) > Math.Abs(scale1))
                        numer1 /= 2;
                    else
                        scale1 /= 2;

                    if (Math.Abs(numer2) > Math.Abs(scale2))
                        numer2 /= 2;
                    else
                        scale2 /= 2;

                    lcd /= 2;

                    if (lcd == 0)
                        throw;
                }
            }

            sc_numer1 = temp1;
            sc_numer2 = temp2;

        }

        ///// <summary>
        /////  A helper function for addition
        ///// </summary>
        ///// <param name="denom1"></param>
        ///// <param name="denom2"></param>
        ///// <param name="lcd"></param>
        ///// <param name="scale1"></param>
        ///// <param name="scale2"></param>
        //static void Scale(long denom1, long denom2, out long lcd, out long scale1, out long scale2) {

        //      long _;

        //      lcd = Algorithms.LCD(denom1, denom2, out _);

        //      scale1 = lcd / denom1;
        //      scale2 = lcd / denom2;

        //}

        static bool Normalize(ref long long1, ref long long2) {

            bool neg_zero_1 = long1 <= 0;
            bool neg2 = long2 < 0;

            if (neg2) {
                long2 = Math.Abs(long2);

                long1 = -long1;

                return true;

            }

            return false;

        }

        static bool Reduce(ref long long1, ref long long2) {

            long gcd = A.GCD(long1, long2);

            if (gcd == 1)
                return false;

            long1 /= gcd;
            long2 /= gcd;

            return true;

        }

        public static Rational Normalize(Rational rat) {

            var numer = rat.m_numer;
            var denom = rat.m_denom;

            if (!Normalize(ref numer, ref denom))
                return rat;

            return new Rational(denom, numer, false);

        }

        public static Rational Invert(Rational rat) {

            var numer = rat.m_numer;
            var denom = rat.m_denom;

            // The ctor will take care of the 0 denominator case
            return new Rational(denom, numer, false);

        }

        public static Rational Negate(Rational rat) {

            var numer = rat.m_numer;
            var denom = rat.m_denom;

            return new Rational(-numer, denom, false);

        }

        public static Rational Abs(Rational rat) {

            var numer = rat.m_numer;

            if (numer >= 0)
                return rat;

            var denom = rat.m_denom;

            return new Rational(Math.Abs(numer), denom, false);

        }

        public static long Weight(Rational rat) {

            return Math.Abs(rat.m_numer) + Math.Abs(rat.m_denom);

        }

        public static long Sign(Rational rat) {

            var numer = rat.m_numer;

            return Math.Sign(numer);

        }

        static Rational Reduce(Rational rat) {

            var numer = rat.m_numer;
            var denom = rat.m_denom;

            if (!Reduce(ref numer, ref denom))
                return rat;

            var ret = new Rational(numer, denom, false);

            return ret;

        }

        static Rational Add(Rational rat1, Rational rat2, bool reduce) {

            Rational ret;

            try {
                var denom1 = rat1.m_denom;
                var denom2 = rat2.m_denom;
                var numer1 = rat1.m_numer;
                var numer2 = rat2.m_numer;

                if (denom1 == denom2) {

                    ret = new Rational(checked(numer1 + numer2), denom1);

                }
                else {

                    //*
                    long sc_numer1, sc_numer2, lcd;

                    Scale_Numerators(numer1, numer2, denom1, denom2,
                                                 out lcd, out sc_numer1, out sc_numer2);

                    try {
                        ret = new Rational(checked(sc_numer1 + sc_numer2),
                                           lcd);
                    }
                    catch (OverflowException) {
                        sc_numer1 /= 2;
                        sc_numer2 /= 2;
                        lcd /= 2;
                        ret = new Rational(checked(sc_numer1 + sc_numer2),
                                                      lcd);
                    }


                    /*/
                    //       Old way

                    long scale1, scale2, lcd;
                    Scale(denom1, denom2, out lcd, out scale1, out scale2);

                    ret = new Rational(checked(numer1 * scale1 +
                                               numer2 * scale2),
                                       lcd);
                    //*/


                    /*       Old way        */

                    //ret = new Rational(checked(numer1 * denom2 + numer2 * denom1),
                    //                   checked(denom1 * denom2));

                }
            }
            catch (OverflowException) {
                // Precision
                var dec = (decimal)rat1 + (decimal)rat2;
                ret = (Rational)dec;
            }

            return ret;

        }

        static Rational Mul(Rational rat1, Rational rat2, bool reduce) {

            Rational ret;

            try {
                var denom1 = rat1.m_denom;
                var denom2 = rat2.m_denom;
                var numer1 = rat1.m_numer;
                var numer2 = rat2.m_numer;

                while (true) {
                    Reduce(ref numer1, ref denom2);
                    Reduce(ref numer2, ref denom1);

                    try {
                        ret = new Rational(checked(numer1 * numer2),
                                           checked(denom1 * denom2),
                                           true);
                        break;

                    }
                    catch (OverflowException) {
                        // 18 oct
                        if (Math.Abs(numer1) > Math.Abs(numer2))
                            numer1 /= 2;
                        else
                            numer2 /= 2;

                        if (Math.Abs(denom1) > Math.Abs(denom2))
                            denom1 /= 2;
                        else
                            denom2 /= 2;
                        continue;
                    }
                }


            }
            catch (OverflowException) {
                // Precision
                var dec = (decimal)rat1 * (decimal)rat2;
                ret = (Rational)dec;
            }

            return ret;

        }

        static Rational Sub(Rational rat1, Rational rat2, bool reduce) {

            return Add(rat1, Negate(rat2), reduce);

        }

        public static Rational Div(Rational rat1, Rational rat2, bool reduce) {

            return Mul(rat1, Invert(rat2), reduce);


        }

        public static Rational Exp(Rational rat1, int exp) {

            var ret = new Rational(A.Pow(rat1.m_numer, exp),
                                   A.Pow(rat1.m_denom, exp),
                                   false);

            return ret;


        }

        public static Rational operator +(Rational rat1, Rational rat2) {
            return Add(rat1, rat2, true);

        }
        public static Rational operator -(Rational rat1, Rational rat2) {
            return Sub(rat1, rat2, true);

        }
        public static Rational operator *(Rational rat1, Rational rat2) {
            return Mul(rat1, rat2, true);

        }
        public static Rational operator /(Rational rat1, Rational rat2) {
            return Div(rat1, rat2, true);

        }

        [Obsolete("Not tested")]
        public static Rational operator %(Rational rat1, Rational rat2) {

            Int64 quo = (Int64)checked((rat1 / rat2));
            return rat1 - new Rational(
                checked(rat2.m_numer * quo),
                checked(rat2.m_denom));

        }

        public static Rational operator ++(Rational rat) {
            return Add(rat, Unit, true);

        }
        public static Rational operator --(Rational rat) {
            return Sub(rat, Unit, true);

        }
        public static Rational operator -(Rational rat) {
            return Negate(rat);
        }

        #region Conversion operators

        /*       From        */

        // Todo: change this
        public static explicit operator Rational(Double number) {
            return Rational.ToRational((decimal)number);
        }

        /// <summary>
        /// This is a potentially lossy conversion since the decimal may use more than the available
        /// 64 bits for the nominator. In this case the least significant bits of the decimal are 
        /// truncated.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static explicit operator Rational(Decimal number) {
            return Rational.ToRational(number);
        }

        const UInt32 highest_bit_int32 = 0x80000000;
        const UInt64 highest_bit_int64 = UInt64.MaxValue - (UInt64.MaxValue >> 1);

        /// <summary>
        /// A lossy conversion from decimal to rat.
        /// If the integer part of the decimal exceeds the available 64 bits (96 is possible)
        /// we truncate the least significant part of these bits.
        /// If the scale factor is too large we scale down the integer part at the expense of precision.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        /// <remarks>
        /// Note the comments on precision issues when the number of digits used in the Decimal
        /// exceed the precision of Int64 significantly.
        /// </remarks>
        static Rational ToRational(Decimal convertedDecimal) {

            // truncate, not floor
            var whole = Math.Truncate(convertedDecimal);
            var part = convertedDecimal % 1;
            var abs_part = Math.Abs(part);

            (part == convertedDecimal - whole).tiff();

            bool sign = Math.Sign(convertedDecimal) < 0;

            Rational fraction;
            if (rd_common_fractions.TryGetValue(abs_part, out fraction)) {

                var rat1 = new Rational((long)whole, 1, false);

                var ret = (sign ? rat1 - fraction : rat1 + fraction);

                return ret;

            }

            unchecked {

                Int32[] bits = Decimal.GetBits(convertedDecimal);
                UInt32 low32 = (UInt32)bits[0];
                UInt32 middle32 = (UInt32)bits[1];
                UInt32 high32 = (UInt32)bits[2];
                UInt32 scaleAndSign = (UInt32)bits[3];
                Boolean negative = scaleAndSign > (UInt32.MaxValue >> 1);

                if (negative) {
                    // Shift leading bit off - velko
                    scaleAndSign <<= 1;
                    scaleAndSign >>= 1;
                }

                Int32 scaleFactor = (Int32)(scaleAndSign >> 16);

                Decimal scale = (Decimal)Math.Pow(10, (Double)scaleFactor);

                // now we construct the scaled long integer
                // if high32 is not zero then the overall number would be too large for the numeric range of long.
                // we determine how many significant bits of high32 need to be shifted down to the lower two ints
                // and correct the scale accordingly. 
                // this step will lose precision for two reasons: 
                // a) bits are truncated in low32 
                // b) the scale value might overflow its precision by too many divisions by two. 
                // This second reason is aggravated significantly if the decimal is large

                while (high32 > 0) {

                    low32 >>= 1; // a)

                    if ((middle32 & 1) > 0)
                        low32 |= highest_bit_int32;

                    // ...1000(28x0)

                    middle32 >>= 1;

                    if ((high32 & 1) > 0)
                        middle32 |= highest_bit_int32;

                    high32 >>= 1;

                    scale /= 2; // b)

                }

                UInt64 scaledInt = ((UInt64)middle32 << 32) + low32;

                // bad things would happen if the highest bit of scaledInt is one. 
                // Truncate the ulong to stay within the range of long by
                // dividing it by two; divide scale by two as well.
                // This step can lose precision by overflowing the precision of scale.
                if ((scaledInt & highest_bit_int64) > 0) {
                    scaledInt >>= 1;
                    scale /= 2;

                }

                // the scale may be in the range [1; 10^28) which would throw us out of the valid
                // range for the denominator:  10^18

                // Note: we could have done this step initially before computing scale and saved us from
                // computing realScaleFactor (which would then be equal to scale). 
                // However this would aggravate the precision problems of scale even further. 
                Int32 realScaleFactor = (Int32)Math.Log10((Double)scale);

                if (realScaleFactor > 18) {

                    UInt64 scaleCorrection = (UInt64)Math.Pow(10, (Double)realScaleFactor - 18);
                    scaledInt /= scaleCorrection;
                    scale /= scaleCorrection;

                }

                long numer, denom;

                numer = (Int64)scaledInt * (negative ? -1 : 1);
                denom = 0;
                // 25 october
                while (true) {
                    try {
                        denom = (Int64)scale;
                        break;
                    }
                    catch (OverflowException) {
                        scale /= 2;
                        numer /= 2;
                    }
                }

                var ret = new Rational(numer, denom);

                // To memorize:
                // rd_common_fractions.Add(abs_part, new Rational(numer % denom, denom));

                return ret;
            }
        }

        public static explicit operator Rational(Int32 integer) {
            return new Rational(integer, 1, false);

        }

        public static explicit operator Rational(Int64 number) {
            return new Rational((Int32)number, 1);
        }


        /*       To        */

        public static explicit operator Int64(Rational rat) {
            return (Int64)rat.m_numer / (Int64)rat.m_denom;
        }

        //[method: CLSCompliant(false)]
        public static explicit operator UInt64(Rational rat) {
            return (UInt64)(Int32)rat;
        }

        //[method: CLSCompliant(false)]
        public static explicit operator UInt32(Rational rat) {
            return (UInt32)(Int32)rat;
        }

        public static explicit operator Int16(Rational rat) {
            return (Int16)(Int32)rat;
        }

        //[method: CLSCompliant(false)]
        public static explicit operator UInt16(Rational rat) {
            return (UInt16)(Int32)rat;
        }

        public static explicit operator Byte(Rational rat) {
            return (Byte)(Int32)rat;
        }

        public static explicit operator Single(Rational rat) {
            return (Single)(Int32)rat;
        }

        public static explicit operator Decimal(Rational rat) {

            var numer = rat.m_numer;
            var denom = rat.m_denom;

            var ret = (decimal)numer / (decimal)denom;

            return ret;

        }

        public static explicit operator Double(Rational rat) {

            var numer = rat.m_numer;
            var denom = rat.m_denom;

            var ret = (double)numer / (double)denom;

            return ret;

        }

        public static explicit operator Int32(Rational rat) {

            var numer = rat.m_numer;
            var denom = rat.m_denom;

            var ret = numer / denom;

            return (Int32)ret;

        }





        #endregion

        #region Comparison

        public static bool operator ==(Rational left, Rational right) {

            var ret = left.Equals(right);
            return ret;

        }

        public static bool operator !=(Rational left, Rational right) {

            var ret = !left.Equals(right);
            return ret;

        }

        public static Boolean operator <=(Rational rat1, Rational rat2) {
            return rat1.CompareTo(rat2) <= 0;
        }

        public static Boolean operator >=(Rational rat1, Rational rat2) {
            return rat1.CompareTo(rat2) >= 0;
        }

        public static Boolean operator <(Rational rat1, Rational rat2) {
            return rat1.CompareTo(rat2) < 0;
        }

        public static Boolean operator >(Rational rat1, Rational rat2) {
            return rat1.CompareTo(rat2) > 0;
        }

        public bool Equals(Rational obj2) {

            if (!this.m_numer.Equals(obj2.m_numer))
                return false;

            if (!this.m_denom.Equals(obj2.m_denom))
                return false;

            return true;
        }

        /// <summary> Note: the comparison is turned (by algebraic transformation) into a comparison
        /// of the cross-product of the fractions. This might overflow the available numbers,
        /// in which case we use home-made 128bit multiplication. This is slow! 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Int32 CompareTo(object obj) {

            var arg = (Rational)obj;

            if (arg.Equals(this))
                return 0;

            var numer1 = this.m_numer;
            var numer2 = arg.m_numer;
            var denom1 = this.m_denom;
            var denom2 = arg.m_denom;

            var thisNegative = numer1 < 0;
            var argNegative = numer2 < 0;

            if (thisNegative != argNegative)
                return argNegative ? 1 : -1;

            var rough1 = numer1 / denom1;
            var rough2 = numer2 / denom2;

            var rough_cmp = rough1.CompareTo(rough2);

            if (rough_cmp != 0)
                return rough_cmp;


            try {
                checked {

                    Reduce(ref numer1, ref numer2);
                    Reduce(ref denom1, ref denom2);

                    if (numer1 * denom2 <
                       numer2 * denom1)
                        return -1;
                    else
                        return 1;
                }
            }
            catch (OverflowException) {

                // we need to resort to 128bit precision multiplication here
                UInt32[] product1 = this.UMult128((ulong)Math.Abs(this.m_numer),
                                                  (ulong)Math.Abs(arg.m_denom));

                UInt32[] product2 = this.UMult128((ulong)Math.Abs(this.m_denom),
                                                  (ulong)Math.Abs(arg.m_numer));

                var less = this.Less128(product1, product2);
                if (thisNegative)
                    less = !less;

                return less ? -1 : 1;
            }
        }



        /// <summary> Multiplication the schoolbook way, based on 32-bit steps.
        /// This can be certainly optimized but it is ok for the moment.
        /// </summary>
        //[method: CLSCompliant(false)]
        internal UInt32[] UMult128(UInt64 u64_1, UInt64 u64_2) {

            // this is a simple schoolbook algorithm for the comparison of the product
            // of two 64bit integers
            // replace its use in CompareTo as soon as you have better 128bit precision arithmetic at hand.

            UInt64 hi1 = u64_1 >> 32;            // High 2 words
            UInt64 lo1 = u64_1 & 0xFFFFFFFF;     // Low 2 words
            UInt64 hi2 = u64_2 >> 32;
            UInt64 lo2 = u64_2 & 0xFFFFFFFF;
            UInt32[] result = { 0, 0, 0, 0 };

            UInt64 tmp;

            UInt64 ll = lo1 * lo2;
            UInt64 hl = hi1 * lo2;
            UInt64 lh = lo1 * hi2;
            UInt64 hh = hi1 * hi2;

            // product = (hi1 << 32 + lo1) * (hi2 << 32 + lo2) = 
            // (hi1 << 32 * hi2 << 32) + (hi1 << 32 * lo1) + (hi2 << 32 * lo2) + (lo1 * lo2)
            result[3] = (UInt32)ll;

            tmp = (ll >> 32) + (hl & 0xFFFFFFFF) + (lh & 0xFFFFFFFF);

            result[2] = (UInt32)tmp;

            tmp >>= 32;

            tmp += (hl >> 32) + (lh >> 32) + (hh & 0xFFFFFFFF);

            result[1] = (UInt32)tmp;

            result[0] = (UInt32)(tmp >> 32) + (UInt32)(hh >> 32);

            return result;

        }

        /// <summary> Simple &lt; comparison of the 128bit multiplication result
        /// </summary>
        //[method: CLSCompliant(false)]
        Boolean Less128(UInt32[] first, UInt32[] second) {

            for (int i = 0; i < 4; i++) {

                if (first[i] < second[i])
                    return true;
                if (first[i] > second[i])
                    return false;

            }

            return false;
        }

        #endregion


        public static Rational Unit {
            get {
                return new Rational(1, 1);
            }
        }

        public static Rational Zero {
            get {
                return new Rational(0, 1);
            }
        }

        public static Rational MinValue {
            get {
                return new Rational(-Int64.MaxValue, 1, false);
            }
        }

        public static Rational MaxValue {
            get {
                return new Rational(Int64.MaxValue, 1, false);
            }
        }

        public static Rational BiggestNegValue {
            get {
                return new Rational(-1, Int64.MaxValue, false);
            }
        }

        public static Rational SmallestPosValue {
            get {
                return new Rational(1, Int64.MaxValue, false);
            }
        }

        string System.IFormattable.ToString(string format, IFormatProvider formatProvider) {
            return this.m_numer.ToString(format, formatProvider) + "/" + this.m_denom.ToString(format, formatProvider);
        }


        public override bool Equals(object obj2) {

            var ret = (obj2 != null && obj2 is Rational);

            if (ret)
                ret = this.Equals((Rational)obj2);


            return ret;

        }

        public override int GetHashCode() {

            unchecked {
                int ret = 23;
                int temp;

                ret *= 31;
                temp = this.m_numer.GetHashCode();
                ret += temp;


                ret *= 31;
                temp = this.m_denom.GetHashCode();
                ret += temp;


                return ret;
            }
        }

        public override string ToString() {
            return "(" +
                   m_numer.ToString(CultureInfo.CurrentCulture) + "/" +
                   m_denom.ToString(CultureInfo.CurrentCulture)
                   + ")";
        }


    }
}

//public bool ToBoolean(IFormatProvider provider) {
//    throw new InvalidCastException("Cannot convert rational value to Boolean");
//}

//public DateTime ToDateTime(IFormatProvider provider) {
//    throw new InvalidCastException("Cannot convert rational value to DateTime");
//}

//public char ToChar(IFormatProvider provider) {
//    throw new InvalidCastException("Cannot convert rational value to Char");
//}

//public string ToString(IFormatProvider provider) {
//    throw new InvalidCastException("Cannot convert rational value to String");
//}

//public byte ToByte(IFormatProvider provider) {
//    return (Byte)this;
//}

//public sbyte ToSByte(IFormatProvider provider) {
//    return (SByte)this;
//}

//public ushort ToUInt16(IFormatProvider provider) {
//    return (UInt16)this;
//}

//public short ToInt16(IFormatProvider provider) {
//    return (Int16)this;
//}

//public UInt32 ToUInt32(IFormatProvider provider) {
//    return (UInt32)this;
//}

//public Int32 ToInt32(IFormatProvider provider) {
//    return (Int32)this;
//}

//public ulong ToUInt64(IFormatProvider provider) {
//    return (UInt64)this;
//}

//public long ToInt64(IFormatProvider provider) {
//    return (Int64)this;
//}




//public float ToSingle(IFormatProvider provider) {
//    return (Single)this;
//}

//public double ToDouble(IFormatProvider provider) {
//    return (Double)this;
//}


//public decimal ToDecimal(IFormatProvider provider) {
//    return (Decimal)this;
//}

//public object ToType(Type conversionType, IFormatProvider provider) {

//    if (this.m_denom == 1)
//        return Convert.ChangeType((Int32)this, conversionType, provider);
//    else
//        return Convert.ChangeType((Decimal)this, conversionType, provider);
//}

//public System.TypeCode GetTypeCode() {
//    return TypeCode.Decimal;
//}