using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core;

namespace BACnet.Core.Network.Messages
{
    public class WhoIsRouterToNetworkMessage : INetworkMessage
    {
        /// <summary>
        /// The message type for who is router to network messages
        /// </summary>
        public MessageType Type { get { return MessageType.WhoIsRouterToNetwork; } }

        /// <summary>
        /// The network number to search for, or null for all networks
        /// </summary>
        public ushort? Network { get; set; }
        
        /// <summary>
        /// Serializes the network message to a buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Serialize(byte[] buffer, int offset)
        {
            if (Network != null)
            {
                buffer.WriteUInt16(offset, Network.Value);
                offset += 2;
            }
            return offset;
        }

        /// <summary>
        /// Deserializes the network message from a buffer
        /// </summary>
        /// <param name="buffer">The buffer to deserialize from</param>
        /// <param name="offset">The offset to begin deserializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Deserialize(byte[] buffer, int offset, int length)
        {
            if (offset + 2 <= length)
            {
                Network = buffer.ReadUInt16(offset);
                offset += 2;
            }
            else
                Network = null;
            return offset;
        }
    }
}
