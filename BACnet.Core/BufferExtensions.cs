using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core
{
    public static class BufferExtensions
    {
        public static void WriteUInt8(this byte[] buffer, int offset, byte value)
        {
            buffer[offset] = value;
        }

        public static void WriteUInt16(this byte[] buffer, int offset, ushort value)
        {
            buffer[offset++] = (byte)(value >> 8);
            buffer[offset++] = (byte)(value);
        }

        public static void WriteUInt32(this byte[] buffer, int offset, uint value)
        {
            buffer[offset++] = (byte)(value >> 24);
            buffer[offset++] = (byte)(value >> 16);
            buffer[offset++] = (byte)(value >> 8);
            buffer[offset++] = (byte)(value);
        }

        public static void WriteUInt64(this byte[] buffer, int offset, ulong value)
        {
            buffer[offset++] = (byte)(value >> 56);
            buffer[offset++] = (byte)(value >> 48);
            buffer[offset++] = (byte)(value >> 40);
            buffer[offset++] = (byte)(value >> 32);
            buffer[offset++] = (byte)(value >> 24);
            buffer[offset++] = (byte)(value >> 16);
            buffer[offset++] = (byte)(value >> 8);
            buffer[offset++] = (byte)(value);
        }

        public static byte ReadUInt8(this byte[] buffer, int offset)
        {
            return buffer[offset];
        }

        public static ushort ReadUInt16(this byte[] buffer, int offset)
        {
            ushort value = 0;
            value |= buffer[offset++];
            value <<= 8;
            value |= buffer[offset++];
            return value;
        }

        public static uint ReadUInt32(this byte[] buffer, int offset)
        {
            uint value = 0;
            value |= buffer[offset++];
            value <<= 8;
            value |= buffer[offset++];
            value <<= 8;
            value |= buffer[offset++];
            value <<= 8;
            value |= buffer[offset++];
            return value;
        }

        public static ulong ReadUInt64(this byte[] buffer, int offset)
        {
            ulong value = 0;
            value |= buffer[offset++];
            value <<= 8;
            value |= buffer[offset++];
            value <<= 8;
            value |= buffer[offset++];
            value <<= 8;
            value |= buffer[offset++];
            value <<= 8;
            value |= buffer[offset++];
            value <<= 8;
            value |= buffer[offset++];
            value <<= 8;
            value |= buffer[offset++];
            value <<= 8;
            value |= buffer[offset++];
            return value;
        }

    }
}
