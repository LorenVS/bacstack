using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core;

namespace BACnet.IP.Bvlc
{
    public class RegisterForeignDeviceMessage : IBvlcMessage
    {
        /// <summary>
        /// The function for for register foreign device messages
        /// </summary>
        public FunctionCode Function { get { return FunctionCode.RegisterForeignDevice; } }

        /// <summary>
        /// The time to live of the registration in seconds
        /// </summary>
        public ushort TTL { get; set; }

        /// <summary>
        /// Constructs a new RegisterForeignDeviceMessage instance
        /// </summary>
        public RegisterForeignDeviceMessage() { }

        /// <summary>
        /// Serializes the message to a buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Serialize(byte[] buffer, int offset)
        {
            buffer.WriteUInt16(offset, TTL);
            return offset + 2;
        }

        /// <summary>
        /// Deserializes the message from a buffer
        /// </summary>
        /// <param name="buffer">The buffer to deserialize from</param>
        /// <param name="offset">The offset to begin deserializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Deserialize(byte[] buffer, int offset)
        {
            this.TTL = buffer.ReadUInt16(offset);
            return offset + 2;
        }
    }
}
