using System;
using System.Runtime.InteropServices;

namespace Fairweather.Service
{
    // http://www.extremeoptimization.com/resources/Articles/FPDotNetConceptsAndFormats.aspx
    // Also see IEEE Standard 754
    [StructLayout(LayoutKind.Explicit)]
    public struct Double64_Union
    {

                public Double64_Union(double p_double) {

            as_long = 0;
            as_double = p_double;
        }



        public Double64_Union(Int32 sign, Int32 exponent, Int64 significand) {

            if (sign < 0)
                sign = 1;
            else
                sign = 0;

            as_double = 0.0;

            as_long = ((UInt32)sign << 0x1F);
            as_long |= (UInt32)exponent << (0x1F - 1 - 11);
            as_long |= (Int64)((UInt64)significand << (0x1F - 1 - 11 - 52));

        }

        public Double64_Union(Int64 p_long) {

            as_double = 0.0;
            as_long = p_long;

            //as_long = Fix(as_long);

        }

        public double Double {
            get { return as_double; }
        }

        public Int64 Int64 {
            get { return as_long; }
        }

        public UInt64 UInt64 {
            get { return (UInt64)as_long; }
        }







        public Int64 Significand {
            get {
                return as_long & 0xFFFFFFFFFFFFFL;
            }
        }

        public Int32 Exponent {
            get {
                return (Int32)((as_long >> 52) & 0x7FFL);
            }
        }

        public Int32 Sign {
            get {
                return Math.Sign(as_double);
            }
        }

        public Int32 RealExponent {
            get {
                return Exponent - 1023;
            }
        }

        public Int64 RealSignificand {
            get {
                if (Exponent == 0)
                    return Significand;
                return Significand | (1 << 52);
            }
        }

        public double ScaledSignificand {
            get {
                return RealSignificand / (double)(1 << 52);
            }
        }





        [FieldOffset(0)]
        readonly double as_double;


        [FieldOffset(0)]
        readonly Int64 as_long;
    }

    
}