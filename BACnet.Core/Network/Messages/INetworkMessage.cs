using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Network.Messages
{
    public interface INetworkMessage
    {
        /// <summary>
        /// The type of network message
        /// </summary>
        MessageType Type { get; }

        /// <summary>
        /// Serializes the network message to a buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        int Serialize(byte[] buffer, int offset);

        /// <summary>
        /// Deserializes the network message from a buffer
        /// </summary>
        /// <param name="buffer">The buffer to deserialize from</param>
        /// <param name="offset">The offset to begin deserializing</param>
        /// <param name="length">The length of the buffer content</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        int Deserialize(byte[] buffer, int offset, int length);
    }
}
