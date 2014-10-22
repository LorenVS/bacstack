using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Network.Messages
{
    public class IAmRouterToNetworkMessage : INetworkMessage
    {
        /// <summary>
        /// The message type for i am router to network messages
        /// </summary>
        public MessageType Type { get { return MessageType.IAmRouterToNetwork; } }

        /// <summary>
        /// The network numbers that are being advertised
        /// </summary>
        public ushort[] Networks { get; set; }

        /// <summary>
        /// Serializes the network message to a buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Serialize(byte[] buffer, int offset)
        {
            if(Networks != null)
            {
                for(int i = 0; i < Networks.Length; i++)
                {
                    buffer.WriteUInt16(offset, Networks[i]);
                    offset += 2;
                }
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
            int count = (length - offset) / 2;

            if(count > 0)
            {
                Networks = new ushort[count];
                for(int i = 0; i < count; i++)
                {
                    Networks[i] = buffer.ReadUInt16(offset);
                    offset += 2;
                }
            }
            else
            {
                Networks = null;
            }
            return offset;
        }
    }
}
