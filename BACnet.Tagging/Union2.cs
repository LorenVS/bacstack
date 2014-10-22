using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Tagging
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct Union2
    {
        [FieldOffset(0)]
        public ushort UInt16;

        [FieldOffset(0)]
        public short Int16;

        [FieldOffset(0)]
        public byte Byte0;

        [FieldOffset(1)]
        public byte Byte1;

        public Union2(ushort uint16) : this() { this.UInt16 = uint16; }
        public Union2(short int16) : this() { this.Int16 = int16; }
        
        public Union2(byte byte0, byte byte1) : this()
        {
            this.Byte0 = byte0;
            this.Byte1 = byte1;
        }

        public Union2 Reverse()
        {
            return new Union2(Byte1, Byte0);
        }

        public Union2 ReverseLE()
        {
            if (BitConverter.IsLittleEndian)
                return this.Reverse();
            else
                return this;
        }
    }
}
