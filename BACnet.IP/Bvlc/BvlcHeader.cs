using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core;

namespace BACnet.IP.Bvlc
{
    public class BvlcHeader
    {
        /// <summary>
        /// The magic byte which identifies bvlc messages
        /// </summary>
        public const byte Magic = 0x81;

        /// <summary>
        /// The function code of the message
        /// </summary>
        public FunctionCode Function { get; set; }

        /// <summary>
        /// The length of the message
        /// </summary>
        public ushort Length { get; set; }

        /// <summary>
        /// Constructs a new BvlcHeader instance
        /// </summary>
        public BvlcHeader() { }

        /// <summary>
        /// Serializes the header to a buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Serialize(byte[] buffer, int offset)
        {
            buffer.WriteUInt8(offset, Magic);
            buffer.WriteUInt8(offset + 1, (byte)Function);
            buffer.WriteUInt16(offset + 2, Length);
            return offset + 4;
        }

        /// <summary>
        /// Deserializes the message from a buffer
        /// </summary>
        /// <param name="buffer">The buffer to deserialize from</param>
        /// <param name="offset">The offset to begin deserializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Deserialize(byte[] buffer, int offset)
        {
            if (buffer.ReadUInt8(offset) != Magic)
            {
                throw new Exception("Buffer does not contain a bvlc message");
            }

            this.Function = (FunctionCode)buffer.ReadUInt8(offset + 1);
            this.Length = buffer.ReadUInt16(offset + 2);
            return offset + 4;
        }
    }
}
