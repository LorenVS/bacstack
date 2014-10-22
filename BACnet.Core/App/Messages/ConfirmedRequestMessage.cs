using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.App.Messages
{
    public class ConfirmedRequestMessage : IAppMessage
    {
        /// <summary>
        /// The message type for confirmed request messagess
        /// </summary>
        public MessageType Type { get { return MessageType.ConfirmedRequest; } }

        /// <summary>
        /// Whether or not the confirmed request is split
        /// into multiple segments
        /// </summary>
        public bool Segmented { get; set; }

        /// <summary>
        /// False if this is the last segment, true otherwise
        /// </summary>
        public bool MoreFollows { get; set; }

        /// <summary>
        /// True if the originating device can accept a segmented
        /// response to this request
        /// </summary>
        public bool SegmentedResponseAccepted { get; set; }

        /// <summary>
        /// The maximum number of segments that the originating
        /// device can accept
        /// </summary>
        public int MaxSegmentsAccepted { get; set; }

        /// <summary>
        /// The maximum appgram length that the originating
        /// device can accept
        /// </summary>
        public int MaxAppgramLengthAccepted { get; set; }

        /// <summary>
        /// The invoke id of the request
        /// </summary>
        public byte InvokeId { get; set; }

        /// <summary>
        /// The sequence number for this segment
        /// </summary>
        public byte SequenceNumber { get; set; }

        /// <summary>
        /// The window size for the transmission that the originating
        /// device is proposing
        /// </summary>
        public byte ProposedWindowSize { get; set; }

        /// <summary>
        /// The service choice of the request
        /// </summary>
        public byte ServiceChoice { get; set; }

        /// <summary>
        /// Serializes the confirmed request message
        /// to a buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serializing</param>
        /// <returns>The new offset</returns>
        public int Serialize(byte[] buffer, int offset)
        {
            byte header = (byte)Type;
            header <<= 4;
            header |= (byte)(Segmented ? 0x08 : 0x00);
            header |= (byte)(MoreFollows ? 0x04 : 0x00);
            header |= (byte)(SegmentedResponseAccepted ? 0x02 : 0x00);
            buffer[offset++] = header;

            MaxSegments maxSegments = MaxSegments.Unspecified;
            MaxAppgramLength maxAppgramLength = MaxAppgramLength._50;

            if (MaxSegmentsAccepted > 64)
                maxSegments = MaxSegments.GreaterThanSixtyFour;
            else if (MaxSegmentsAccepted == 64)
                maxSegments = MaxSegments.SixtyFour;
            else if (MaxSegmentsAccepted >= 32)
                maxSegments = MaxSegments.ThirtyTwo;
            else if (MaxSegmentsAccepted >= 16)
                maxSegments = MaxSegments.Sixteen;
            else if (MaxSegmentsAccepted >= 8)
                maxSegments = MaxSegments.Eight;
            else if (MaxSegmentsAccepted >= 4)
                maxSegments = MaxSegments.Four;
            else if (MaxSegmentsAccepted >= 2)
                maxSegments = MaxSegments.Two;
            else
                maxSegments = MaxSegments.Unspecified;

            if (MaxAppgramLengthAccepted >= 1476)
                maxAppgramLength = MaxAppgramLength._1476;
            else if (MaxAppgramLengthAccepted >= 1024)
                maxAppgramLength = MaxAppgramLength._1024;
            else if (MaxAppgramLengthAccepted >= 480)
                maxAppgramLength = MaxAppgramLength._480;
            else if (MaxAppgramLengthAccepted >= 206)
                maxAppgramLength = MaxAppgramLength._206;
            else if (MaxAppgramLengthAccepted >= 128)
                maxAppgramLength = MaxAppgramLength._128;
            else if (MaxAppgramLengthAccepted >= 50)
                maxAppgramLength = MaxAppgramLength._50;
            else
                throw new Exception("MaxAppgramLengthAccepted is small than minimum value");

            header = (byte)maxSegments;
            header <<= 4;
            header |= (byte)maxAppgramLength;
            buffer[offset++] = header;

            buffer[offset++] = InvokeId;
            if(Segmented)
            {
                buffer[offset++] = SequenceNumber;
                buffer[offset++] = ProposedWindowSize;
            }

            buffer[offset++] = ServiceChoice;

            return offset;
        }

        /// <summary>
        /// Deserializes the confirmed request message
        /// from a buffer
        /// </summary>
        /// <param name="buffer">The buffer to deserialize from</param>
        /// <param name="offset">The offset to begin deserializing</param>
        /// <param name="end">The end of the buffer content</param>
        /// <returns>The new offset</returns>
        public int Deserialize(byte[] buffer, int offset, int end)
        {
            byte header = buffer.ReadUInt8(offset++);
            if ((MessageType)(header >> 4) != MessageType.ConfirmedRequest)
                throw new Exception("Could not deserialize an unconfirmed request message");

            this.Segmented = (header & 0x08) > 0;
            this.MoreFollows = (header & 0x04) > 0;
            this.SegmentedResponseAccepted = (header & 0x02) > 0;

            header = buffer.ReadUInt8(offset++);
            MaxSegments maxSegments = (MaxSegments)((header & 0x70) >> 4);
            MaxAppgramLength maxAppgramLength = (MaxAppgramLength)(header & 0x0F);

            switch(maxSegments)
            {
                case MaxSegments.Unspecified:
                    this.MaxSegmentsAccepted = 1;
                    break;
                case MaxSegments.Two:
                    this.MaxSegmentsAccepted = 2;
                    break;
                case MaxSegments.Four:
                    this.MaxSegmentsAccepted = 4;
                    break;
                case MaxSegments.Eight:
                    this.MaxSegmentsAccepted = 8;
                    break;
                case MaxSegments.Sixteen:
                    this.MaxSegmentsAccepted = 16;
                    break;
                case MaxSegments.ThirtyTwo:
                    this.MaxSegmentsAccepted = 32;
                    break;
                case MaxSegments.SixtyFour:
                    this.MaxSegmentsAccepted = 64;
                    break;
                case MaxSegments.GreaterThanSixtyFour:
                    this.MaxSegmentsAccepted = Int32.MaxValue;
                    break;
                default:
                    throw new Exception("Unrecognized max segments value");                    
            }

            switch(maxAppgramLength)
            {
                case MaxAppgramLength._50:
                    this.MaxAppgramLengthAccepted = 50;
                    break;
                case MaxAppgramLength._128:
                    this.MaxAppgramLengthAccepted = 128;
                    break;
                case MaxAppgramLength._206:
                    this.MaxAppgramLengthAccepted = 206;
                    break;
                case MaxAppgramLength._480:
                    this.MaxAppgramLengthAccepted = 480;
                    break;
                case MaxAppgramLength._1024:
                    this.MaxAppgramLengthAccepted = 1024;
                    break;
                case MaxAppgramLength._1476:
                    this.MaxAppgramLengthAccepted = 1476;
                    break;
                default:
                    throw new Exception("Unrecognized max appgram length value");
            }

            this.InvokeId = buffer[offset++];
            if(this.Segmented)
            {
                this.SequenceNumber = buffer[offset++];
                this.ProposedWindowSize = buffer[offset++];
            }
            this.ServiceChoice = buffer[offset++];

            return offset;
        }

        private enum MaxSegments
        {
            Unspecified = 0x00,
            Two = 0x01,
            Four = 0x02,
            Eight = 0x03,
            Sixteen = 0x04,
            ThirtyTwo = 0x05,
            SixtyFour = 0x06,
            GreaterThanSixtyFour = 0x07
        }

        private enum MaxAppgramLength
        {
            _50 = 0x00,
            _128 = 0x01,
            _206 = 0x02,
            _480 = 0x03,
            _1024 = 0x04,
            _1476 = 0x05
        }
    }
}
