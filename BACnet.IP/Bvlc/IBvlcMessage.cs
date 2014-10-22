using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.IP.Bvlc
{
    public interface IBvlcMessage
    {
        /// <summary>
        /// The function code of the message
        /// </summary>
        FunctionCode Function { get; }

        /// <summary>
        /// Serializes the message to a buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        int Serialize(byte[] buffer, int offset);

        /// <summary>
        /// Deserializes the message from a buffer
        /// </summary>
        /// <param name="buffer">The buffer to deserialize from</param>
        /// <param name="offset">The offset to begin deserializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        int Deserialize(byte[] buffer, int offset);
    }
}
