using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.App.Messages
{
    public class RejectMessage : IAppMessage
    {
        /// <summary>
        /// The message type for reject messages
        /// </summary>
        public MessageType Type { get { return MessageType.Reject; } }

        /// <summary>
        /// The invoke id of the transaction
        /// that is being rejected
        /// </summary>
        public byte InvokeId { get; set; }

        /// <summary>
        /// The reason that the transaction
        /// was rejected
        /// </summary>
        public byte RejectReason { get; set; }



        /// <summary>
        /// Serializes the app message to a buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Serialize(byte[] buffer, int offset)
        {
            byte header = (byte)((byte)Type << 4);
            buffer.WriteUInt8(offset++, header);
            buffer.WriteUInt8(offset++, InvokeId);
            buffer.WriteUInt8(offset++, RejectReason);
            return offset;
        }

        /// <summary>
        /// Deserializes the app message from a buffer
        /// </summary>
        /// <param name="buffer">The buffer to deserialize from</param>
        /// <param name="offset">The offset to begin deserializing</param>
        /// <param name="end">The end of the buffer content</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Deserialize(byte[] buffer, int offset, int end)
        {
            byte header = buffer.ReadUInt8(offset++);
            if ((MessageType)(header >> 4) != MessageType.UnconfirmedRequest)
                throw new Exception("Could not deserialize an unconfirmed request message");
            this.InvokeId = buffer.ReadUInt8(offset++);
            this.RejectReason = buffer.ReadUInt8(offset++);
            return offset;
        }

    }
}
