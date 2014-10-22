using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core;

namespace BACnet.IP.Bvlc
{
    public class OriginalUnicastNpduMessage : IBvlcMessage
    {
        /// <summary>
        /// The function for original unicast npdu messages
        /// </summary>
        public FunctionCode Function { get { return FunctionCode.OriginalUnicastNpdu; } }

        /// <summary>
        /// Constructs a new OriginalUnicastNpduMessage instance
        /// </summary>
        public OriginalUnicastNpduMessage() { }

        /// <summary>
        /// Serializes the message to a buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Serialize(byte[] buffer, int offset)
        {
            return offset;
        }

        /// <summary>
        /// Deserializes the message from a buffer
        /// </summary>
        /// <param name="buffer">The buffer to deserialize from</param>
        /// <param name="offset">The offset to begin deserializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Deserialize(byte[] buffer, int offset)
        {
            return offset;
        }
    }
}
