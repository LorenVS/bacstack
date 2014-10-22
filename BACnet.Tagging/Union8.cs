using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Tagging
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct Union8
    {
        [FieldOffset(0)]
        public ulong UInt64;

        [FieldOffset(0)]
        public long Int64;

        [FieldOffset(0)]
        public double Float64;

        [FieldOffset(0)]
        public byte Byte0;

        [FieldOffset(1)]
        public byte Byte1;

        [FieldOffset(2)]
        public byte Byte2;

        [FieldOffset(3)]
        public byte Byte3;

        [FieldOffset(4)]
        public byte Byte4;

        [FieldOffset(5)]
        public byte Byte5;

        [FieldOffset(6)]
        public byte Byte6;

        [FieldOffset(7)]
        public byte Byte7;

        public Union8(ulong uint64) : this() { this.UInt64 = uint64; }
        public Union8(long int64) : this() { this.Int64 = int64; }
        public Union8(double float64) : this() { this.Float64 = float64; }

        public Union8(byte byte0, byte byte1, byte byte2, byte byte3, byte byte4, byte byte5, byte byte6, byte byte7) : this()
        {
            this.Byte0 = byte0;
            this.Byte1 = byte1;
            this.Byte2 = byte2;
            this.Byte3 = byte3;
            this.Byte4 = byte4;
            this.Byte5 = byte5;
            this.Byte6 = byte6;
            this.Byte7 = byte7;
        }

        public Union8 Reverse()
        {
            return new Union8(Byte7, Byte6, Byte5, Byte4, Byte3, Byte2, Byte1, Byte0);
        }

        public Union8 ReverseLE()
        {
            if (BitConverter.IsLittleEndian)
                return this.Reverse();
            else
                return this;
        }
    }
}
