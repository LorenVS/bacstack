using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Network
{
    public class NetgramHeader
    {
        /// <summary>
        /// The version of the netgram header
        /// </summary>
        public byte Version { get; set; }

        /// <summary>
        /// True if the netgram contains a network message,
        /// false if it contains an appgram
        /// </summary>
        public bool IsNetworkMessage { get; set; }

        /// <summary>
        /// True if a response is expected to this netgram,
        /// false otherwise
        /// </summary>
        public bool ExpectingReply { get; set; }

        /// <summary>
        /// The priority of the netgram
        /// </summary>
        public NetgramPriority Priority { get; set; }

        /// <summary>
        /// The original source address
        /// </summary>
        public Address Source { get; set; }

        /// <summary>
        /// The final destination address
        /// </summary>
        public Address Destination { get; set; }

        /// <summary>
        /// The remaining hop count of the netgram
        /// </summary>
        public byte HopCount { get; set; }

        /// <summary>
        /// The network message type of the netgram
        /// </summary>
        public byte MessageType { get; set; }

        /// <summary>
        /// The vendor id that defines the network message type
        /// </summary>
        public ushort VendorId { get; set; }

        public NetgramHeader()
        {
            Version = 1;
            ExpectingReply = false;
            Priority = NetgramPriority.Normal;
        }

        /// <summary>
        /// Serializes the netgram header to a buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Serialize(byte[] buffer, int offset)
        {
            buffer.WriteUInt8(offset++, Version);

            byte flags = (byte)(IsNetworkMessage ? 0x80 : 0x00);
            flags |= (byte)(Destination != null ? 0x20 : 0x00);
            flags |= (byte)(Source != null ? 0x08 : 0x00);
            flags |= (byte)(ExpectingReply ? 0x04 : 0x00);
            flags |= (byte)(Priority);
            buffer.WriteUInt8(offset++, flags);

            if(Destination != null)
            {
                buffer.WriteUInt16(offset, Destination.Network);
                offset += 2;
                buffer.WriteUInt8(offset++, (byte)Destination.Mac.Length);
                for(int i = 0; i < Destination.Mac.Length; i++)
                {
                    buffer.WriteUInt8(offset++, Destination.Mac[i]);
                }
            }

            if (Source != null)
            {
                buffer.WriteUInt16(offset, Source.Network);
                offset += 2;
                buffer.WriteUInt8(offset++, (byte)Source.Mac.Length);
                for (int i = 0; i < Source.Mac.Length; i++)
                {
                    buffer.WriteUInt8(offset++, Source.Mac[i]);
                }
            }

            if(Destination != null)
            {
                buffer.WriteUInt8(offset++, HopCount);
            }

            if (IsNetworkMessage)
            {
                buffer.WriteUInt8(offset++, MessageType);
                if(MessageType >= 0x80)
                {
                    buffer.WriteUInt16(offset, VendorId);
                    offset += 2;
                }
            }

            return offset;
        }

        /// <summary>
        /// Deserializes the netgram header from a buffer
        /// </summary>
        /// <param name="buffer">The buffer to deserialize from</param>
        /// <param name="offset">The offset to begin deserializing</param>
        /// <param name="length">The length of the buffer content</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Deserialize(byte[] buffer, int offset, int length)
        {
            this.Version = buffer.ReadUInt8(offset++);
            if (this.Version != 1)
                throw new Exception("Can only read netgram headers with a version of 1");

            byte flags = buffer.ReadUInt8(offset++);
            this.IsNetworkMessage = (flags & 0x80) > 0;
            this.ExpectingReply = (flags & 0x04) > 0;
            this.Priority = (NetgramPriority)(flags & 0x03);

            if((flags & 0x20) > 0)
            {
                ushort network = buffer.ReadUInt16(offset);
                offset += 2;
                byte macLength = buffer.ReadUInt8(offset++);
                this.Destination = new Address(network,
                    new Datalink.Mac(buffer, offset, macLength));
                offset += macLength;
            }

            if ((flags & 0x08) > 0)
            {
                ushort network = buffer.ReadUInt16(offset);
                offset += 2;
                byte macLength = buffer.ReadUInt8(offset++);
                this.Source = new Address(network,
                    new Datalink.Mac(buffer, offset, macLength));
                offset += macLength;
            }

            if ((flags & 0x20) > 0)
            {
                this.HopCount = buffer.ReadUInt8(offset++);
            }

            if (this.IsNetworkMessage)
            {
                this.MessageType = buffer.ReadUInt8(offset++);
                if(this.MessageType >= 0x80)
                {
                    this.VendorId = buffer.ReadUInt16(offset);
                    offset += 2;
                }
            }

            return offset;
        }


    }
}
