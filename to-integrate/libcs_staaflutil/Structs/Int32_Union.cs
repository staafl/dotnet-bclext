using System;
using System.Runtime.InteropServices;

namespace Fairweather.Service
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Int32_Union
    {
        public Int32_Union(int integer, bool little_endian) {

            byte0 = byte1 = byte2 = byte3 = default(byte);
            this.integer = default(int);

            if (little_endian) {

                Int32_Union temp = new Int32_Union(integer, false);

                byte0 = temp.byte1;
                byte1 = temp.byte0;
                byte2 = temp.byte3;
                byte3 = temp.byte2;

                return;
            }

            this.integer = integer;
        }

        public Int32_Union(byte[] bytes, bool little_endian) {

            if (bytes.Length != sizeof(int))
                throw new ArgumentException();

            integer = default(int);

            if (little_endian) {
                Int32_Union temp = new Int32_Union(bytes, false);
                byte0 = temp.byte1;
                byte1 = temp.byte0;
                byte2 = temp.byte3;
                byte3 = temp.byte2;

                return;
            }

            byte0 = bytes[0];
            byte1 = bytes[1];
            byte2 = bytes[2];
            byte3 = bytes[3];
        }

        public byte[] Get_Bytes(bool little_endian) {

            byte[] bytes = new byte[sizeof(int)];

            if (little_endian) {

                bytes[0] = byte0;
                bytes[1] = byte1;
                bytes[2] = byte2;
                bytes[3] = byte3;
            }
            else {

                bytes[0] = byte1;
                bytes[1] = byte0;
                bytes[2] = byte3;
                bytes[3] = byte2;
            }

            return bytes;
        }

        public int Get_Integer(bool little_endian) {

            byte[] bytes = new byte[sizeof(int)];

            if (little_endian) {
                Int32_Union temp = new Int32_Union(this.integer, true);

                return temp.integer;
            }
            else
                return integer;
        }

        [FieldOffset(0)]
        readonly int integer;

        [FieldOffset(0)]
        readonly byte byte0;

        [FieldOffset(1)]
        readonly byte byte1;

        [FieldOffset(2)]
        readonly byte byte2;

        [FieldOffset(3)]
        readonly byte byte3;
    }
}
