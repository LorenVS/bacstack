using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Tagging
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct Union4
    {
        [FieldOffset(0)]
        public uint UInt32;

        [FieldOffset(0)]
        public int Int32;

        [FieldOffset(0)]
        public float Float32;

        [FieldOffset(0)]
        public byte Byte0;

        [FieldOffset(1)]
        public byte Byte1;

        [FieldOffset(2)]
        public byte Byte2;

        [FieldOffset(3)]
        public byte Byte3;

        public Union4(uint uint32) : this() { this.UInt32 = uint32; }
        public Union4(int int32) : this() { this.Int32 = int32; }
        public Union4(float float32) : this() { this.Float32 = float32; }
        
        public Union4(byte byte0, byte byte1, byte byte2, byte byte3) : this()
        {
            this.Byte0 = byte0;
            this.Byte1 = byte1;
            this.Byte2 = byte2;
            this.Byte3 = byte3;
        }

        public Union4 Reverse()
        {
            return new Union4(Byte3, Byte2, Byte1, Byte0);
        }

        public Union4 ReverseLE()
        {
            if (BitConverter.IsLittleEndian)
                return this.Reverse();
            else
                return this;
        }
    }
}
