using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fairweather.Service
{
    static public class B
    {
        // http://en.wikipedia.org/wiki/Sign_extension
        ///
        // http://www.spread.org/docs/endian.html
        ///
        // Also check System.Net.IPAddress.HostToNetworkOrder() and NetworkToHostOrder()
        // http://www.gamedev.net/community/forums/topic.asp?topic_id=278879

        public static int RGB(byte r, byte g, byte b) {

            UInt32 ret = (byte)r;

            ret |= ((uint)g) << 8;
            ret |= ((uint)b) << 16;

            return (int)ret;
        }

        // Source: Sebastien Lorion
        // http://nlight.codeplex.com/SourceControl/changeset/view/45808#698542
        /// <summary>
        /// Reverses the number of significant bits specified.
        /// </summary>
        /// <param name="value">The value to reverse.</param>
        /// <param name="significantBitCount">The number of significant bits to reverse.</param>
        /// <returns>The <paramref name="value"/> with its <paramref name="significantBitCount"/> reversed.</returns>
        /// <example>
        /// Example, if the input is 376, with significantBitCount=11, the output is 244 (decimal, base 10).
        /// 
        /// 376 = 00000101111000
        /// 244 = 00000011110100
        /// 
        /// Example, if the input is 900, with significantBitCount=11, the output is 270.
        /// 
        /// 900 = 00001110000100
        /// 270 = 00000100001110
        /// 
        /// Example, if the input is 900, with significantBitCount=12, the output is 540.
        /// 
        /// 900 = 00001110000100
        /// 540 = 00001000011100
        /// 
        /// Example, if the input is 154, with significantBitCount=4, the output is 5.
        /// 
        /// 154 = 00000010011010
        /// 005 = 00000000000101
        /// </example>
        public static long Reverse(long value, int significantBitCount) {
            ulong n = (ulong)value << (64 - significantBitCount);
            n = n >> 32 | n << 32;
            n = n >> 0xf & 0x0000ffff | n << 0xf & 0xffff0000;
            n = n >> 0x8 & 0x00ff00ff | n << 0x8 & 0xff00ff00;
            n = n >> 0x4 & 0x0f0f0f0f | n << 0x4 & 0xf0f0f0f0;
            n = n >> 0x2 & 0x33333333 | n << 0x2 & 0xcccccccc;
            n = n >> 0x1 & 0x55555555 | n << 0x1 & 0xaaaaaaaa;
            return (long)n | value & (-1 << significantBitCount);
        }


        public static Int32 RotateLeft(Int32 value, Int32 count) {
            return ((value << (count & 0x1f)) | (count >> ((0x20 - count) & 0x1f)));
        }

        [Obsolete("Does it work?")]
        public static Int32 RotateRight(Int32 value, Int32 count) {
            return ((value >> (count & 0x1f)) | (value << ((0x20 - count) & 0x1f)));
        }


        /// <summary>
        /// If number >= 0 => the result is number
        /// If number .lt. 0 => the result is the sign-magnitude representation of the negative number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int AsSignMagnitude(int number) {

            if (number < 0) {

                number = (int_sign_bit | -(int)number);
                // also number = 0x80000000 - number;
                // also number = int_sign_bit - number;
            }

            return number;

        }

        /// <summary>
        /// If number >= 0 => the result is number
        /// If number .lt. 0 => the result is the sign-magnitude representation of the negative number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static long AsSignMagnitude(long number) {

            if (number < 0) {

                number = (int_sign_bit | -(long)number);
                // also number = 0x80000000 - number;
                // also number = int_sign_bit - number;
            }

            return number;

        }


        public const int int_size_bits = A.int_size_bits;
        public const int int_sign_bit = A.int_sign_bit;

        static public byte[] To_Byte_Array(int integer, bool little_endian) {

            byte[] ret;

            Int32_Union union = new Int32_Union(integer, little_endian);

            ret = union.Get_Bytes(little_endian);

            return ret;
        }

        static public int Hamming_Weight(int value) {

            // http://graphics.stanford.edu/~seander/bithacks.html

            int ret = 0;

            while (value != 0) {
                value &= value - 1;
                ++ret;
            }

            return ret;
        }

        public static bool Is_Current_Little_Endian() {

            return BitConverter.IsLittleEndian;

        }

        //public static unsafe bool Is_Current_Little_Endian() {

        //      return BitConverter.IsLittleEndian;


        //      var temp = 0xFF;
        //      int* ptr = &temp;
        //      var ptr1 = (byte*)ptr;
        //      if (ptr1[4] == temp)
        //            return false;
        //      return true;

        //}

        public static Int32 HiDWord(Int64 value) {

            var ret = (int)(value >> 32);

            return ret;
        }

        public static Int32 LoDWord(Int64 value) {
            var ret = (int)(value & 0xFFFFFFFF);

            return ret;
        }

        public static int HiWord(int value) {

            int ret = (value >> 16);

            return ret;
        }

        public static int LoWord(int value) {
            int ret = (value & 0xFFFF);

            return ret;
        }



        public static int LoByte(int value) {
            int ret = ((int)value & 0xFF);

            return ret;
        }

        public static int HiByte(int value) {
            int ret = ((int)value >> 8);

            return ret;
        }



        public static int HiWord(IntPtr value) {

            int ret = ((int)value >> 16);

            return ret;
        }

        public static int LoWord(IntPtr value) {
            int ret = ((int)value & 0xFFFF);

            return ret;
        }


        public static int LoByte(IntPtr value) {
            int ret = ((int)value & 0xFF);

            return ret;
        }

        public static int HiByte(IntPtr value) {
            int ret = ((int)value >> 8);

            return ret;
        }



        public static UInt16 Combine(Byte first, Byte second) {

            var ret = (UInt16)((((UInt16)first) << 0x08) | second);
            return ret;

        }


        public static UInt32 Combine(UInt16 first, UInt16 second) {

            var ret = (((UInt32)first) << 0x10) | second;
            return ret;

        }

        public static void Split(Int64 to_split, out Int32 left, out Int32 right) {

            left = HiDWord(to_split);
            right = LoDWord((long)to_split);

        }


        public static Int64 Combine(Int32 first, Int32 second) {

            var t1 = (UInt32)first;
            var t2 = first << 0x20;
            var ret = t2 | second;
            return ret;

        }

        public static UInt16 SwapEndian(UInt16 integer) {

            var ret = (UInt16)((integer >> 0x8 & 0xFF) |
                                    (integer << 0x8 & 0xFF00));
            return ret;

            // also:
            /*
             *(integer >> 0x8 & 0xFF) |
               ((integer & 0xFF) << 0x8);
            */
        }

        public static UInt32 FlipBytes(UInt32 integer) {

            var ret = ((integer << 8) & 0xFF000000) |
                          ((integer >> 8) & 0x00FF0000) |
                          ((integer << 8) & 0x00FF) |
                          ((integer >> 8) & 0xFF00);

            //var high = (Int32)HiWord((int)integer);
            //var low = (Int32)LoWord((int)integer);
            //var Xxxx = (byte)HiByte(high);
            //var xXxx = (byte)LoByte(high);
            //var xxXx = (byte)HiByte(low);
            //var xxxX = (byte)LoByte(low);

            //var ret = Combine(Combine(xXxx, Xxxx),
            //                            Combine(xxxX, xxXx));

            return ret;

        }

        // http://www.spread.org/docs/endian.html
        public static Int32 SwapEndian(Int32 integer) {

            var ret = (((UInt32)integer << 24) & 0xFF000000) |
                          (((UInt32)integer << 8) & 0x00FF0000) |
                          (((UInt32)integer >> 24) & 0x00FF) |
                          (((UInt32)integer >> 8) & 0xFF00);

            return (Int32)ret;

        }

        public static Int64 SwapEndian(Int64 integer) {

            int left, right;
            Split(integer, out left, out right);

            left = SwapEndian(left);
            right = SwapEndian(right);

            var ret = Combine(right, left);

            return ret;

        }

        public static Int64 To_Big_Endian(Int64 integer) {

            if (Is_Current_Little_Endian())
                return SwapEndian(integer);

            return integer;

        }


        public static Int32 BitMask(int number_of_bits) {

            (number_of_bits >= 0).tiff();

            var ret = (1 << number_of_bits) - 1;
            return ret;

        }

        /// <summary> Zero always returns false. </summary>
        public static bool Contains(int int1, int int2) {

            bool ret = (int1 != 0) &&
                       (int2 != 0) &&
                       (int1 & int2) == int2;

            return ret;

        }

        //public static int Simple_Hash(this int[] array){
        //
        //}
        ///
        //public static int Adler_Hash(this int[] array) {


        //}
        ///




        // returns a list of all bits that are set in the integer's binary
        // representation
        public static int[] List_Of_Bits(this int integer) {

            var ret = new Stack<int>();

            bool sign = integer < 0;

            if (integer == int.MinValue) {

                return Enumerable.Repeat(1, int_size_bits).ToArray();
            }

            integer = Math.Abs(integer);

            int ii = 0;

            while (integer > 0) {

                if (((integer % 2) == 1) != sign)
                    ret.Push(ii);

                ++ii;
                integer >>= 1;
            }

            if (sign) {
                ret.Push(int_size_bits);
            }

            return ret.ToArray();
        }

        public static string Bits_To_String(IEnumerable<int> bits, bool left_pad) {

            if (bits.Count() == 0)
                return left_pad ? new String('0', int_size_bits) : "";

            StringBuilder ret = new StringBuilder(bits.Count());

            bits = bits.OrderBy(ii => -ii);

            int last = left_pad ? int_size_bits : bits.First();

            foreach (var bit in bits) {

                ret.Append('0', last - bit - 1);
                ret.Append('1');
                last = bit;
            }

            ret.Append('0', last);

            return ret.ToString();
        }

        public static int Bits_To_Int(IEnumerable<int> bits) {

            bool sign = false;

            {
                int sign_bit = int_size_bits;
                sign = bits.Contains(sign_bit);
                bits = bits.Where(ii => ii != sign_bit);
            }

            int ret = 0;
            foreach (var bit in bits)
                ret += (1 << bit);

            if (sign) {
                ret += 1;
                ret = -ret;
            }

            return ret;
        }


    }
}