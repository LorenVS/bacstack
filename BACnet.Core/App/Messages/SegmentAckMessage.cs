using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.App.Messages
{
    public class SegmentAckMessage : IAppMessage
    {
        /// <summary>
        /// The message type for segment ack messages
        /// </summary>
        public MessageType Type { get { return MessageType.SegmentAck; } }

        /// <summary>
        /// True if this is a negative segment ack, false otherwise
        /// </summary>
        public bool Nack { get; set; }

        /// <summary>
        /// True if the sending device is acting as the server
        /// in the transaction, false if it is acting as the client
        /// </summary>
        public bool Server { get; set; }

        /// <summary>
        /// The invoke id that is being acknowledged
        /// </summary>
        public byte InvokeId { get; set; }

        /// <summary>
        /// The sequence number of the segment that is
        /// being acknowledged
        /// </summary>
        public byte SequenceNumber { get; set; }

        /// <summary>
        /// The window size that is being used for the transmission
        /// </summary>
        public byte ActualWindowSize { get; set; }

        /// <summary>
        /// Serializes the app message to a buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Serialize(byte[] buffer, int offset)
        {
            byte header = (byte)((byte)Type << 4);
            header |= (byte)(this.Nack ? 0x02 : 0x00);
            header |= (byte)(this.Server ? 0x01 : 0x00);
            buffer.WriteUInt8(offset++, header);
            buffer.WriteUInt8(offset++, InvokeId);
            buffer.WriteUInt8(offset++, SequenceNumber);
            buffer.WriteUInt8(offset++, ActualWindowSize);
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
            if ((MessageType)(header >> 4) != MessageType.SegmentAck)
                throw new Exception("Could not deserialize an unconfirmed request message");
            this.Nack = (header & 0x02) > 0;
            this.Server = (header & 0x01) > 0;
            this.InvokeId = buffer.ReadUInt8(offset++);
            this.SequenceNumber = buffer.ReadUInt8(offset++);
            this.ActualWindowSize = buffer.ReadUInt8(offset++);
            return offset;
        }

    }
}
