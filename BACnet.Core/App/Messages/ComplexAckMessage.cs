using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.App.Messages
{
    public class ComplexAckMessage : IAppMessage
    {
        /// <summary>
        /// The message type for complex ack messages
        /// </summary>
        public MessageType Type { get { return MessageType.ComplexAck; } }

        /// <summary>
        /// True if the ack is contained in more than one segment, false otherwise
        /// </summary>
        public bool Segmented { get; set; }

        /// <summary>
        /// False if this is the last segment, true otherwise
        /// </summary>
        public bool MoreFollows { get; set; }

        /// <summary>
        /// The invoke id of the request that this ack is responding
        /// to
        /// </summary>
        public byte InvokeId { get; set; }

        /// <summary>
        /// The sequence number of the segment if this is a
        /// segmented ack
        /// </summary>
        public byte SequenceNumber { get; set; }

        /// <summary>
        /// The window size that the responding device
        /// would like to use for the ack transmission
        /// </summary>
        public byte ProposedWindowSize { get; set; }

        /// <summary>
        /// The service choice of the request that
        /// is being responded to
        /// </summary>
        public byte ServiceChoice { get; set; }

        /// <summary>
        /// Serializes the app message to a buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Serialize(byte[] buffer, int offset)
        {
            byte header = (byte)((byte)Type << 4);
            header |= (byte)(this.Segmented ? 0x08 : 0x00);
            header |= (byte)(this.MoreFollows ? 0x04 : 0x00);
            buffer.WriteUInt8(offset++, header);
            buffer.WriteUInt8(offset++, InvokeId);

            if(this.Segmented)
            {
                buffer.WriteUInt8(offset++, SequenceNumber);
                buffer.WriteUInt8(offset++, ProposedWindowSize);
            }

            buffer.WriteUInt8(offset++, ServiceChoice);
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
            if ((MessageType)(header >> 4) != MessageType.ComplexAck)
                throw new Exception("Could not deserialize an unconfirmed request message");

            this.Segmented = (header & 0x08) > 0;
            this.MoreFollows = (header & 0x04) > 0;
            this.InvokeId = buffer.ReadUInt8(offset++);

            if(this.Segmented)
            {
                this.SequenceNumber = buffer.ReadUInt8(offset++);
                this.ProposedWindowSize = buffer.ReadUInt8(offset++);
            }

            this.ServiceChoice = buffer.ReadUInt8(offset++);
            return offset;
        }

    }
}
