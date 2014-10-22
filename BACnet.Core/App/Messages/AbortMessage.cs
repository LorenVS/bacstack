using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.App.Messages
{
    public class AbortMessage : IAppMessage
    {
        /// <summary>
        /// The message type for abort messages
        /// </summary>
        public MessageType Type { get { return MessageType.Abort; } }

        /// <summary>
        /// True if the sending device is acting as
        /// the server in the transaction, false if it
        /// is acting as the client
        /// </summary>
        public bool Server { get; set; }

        /// <summary>
        /// The invoke id of the transaction
        /// that is being aborted
        /// </summary>
        public byte InvokeId { get; set; }

        /// <summary>
        /// The reason that the transaction
        /// is being aborted
        /// </summary>
        public byte AbortReason { get; set; }

        /// <summary>
        /// Serializes the app message to a buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Serialize(byte[] buffer, int offset)
        {
            byte header = (byte)((byte)Type << 4);
            header |= (byte)(Server ? 0x01 : 0x00);
            buffer.WriteUInt8(offset++, header);
            buffer.WriteUInt8(offset++, InvokeId);
            buffer.WriteUInt8(offset++, AbortReason);
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
            if ((MessageType)(header >> 4) != MessageType.Abort)
                throw new Exception("Could not deserialize an unconfirmed request message");
            this.Server = (header & 0x01) > 0;
            this.InvokeId = buffer.ReadUInt8(offset++);
            this.AbortReason = buffer.ReadUInt8(offset++);
            return offset;
        }

    }
}
