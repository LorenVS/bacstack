using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core.Datalink;

namespace BACnet.IP.Bvlc
{
    public class ForwardedNpduMessage : IBvlcMessage
    {
        /// <summary>
        /// The function code for forwarded npdu messages
        /// </summary>
        public FunctionCode Function { get { return FunctionCode.ForwardedNpdu; } }

        /// <summary>
        /// The mac address of the original sending device
        /// </summary>
        public Mac OriginalMac { get; set; }

        /// <summary>
        /// Constructs a new forwarded npdu message instance
        /// </summary>
        public ForwardedNpduMessage() { }

        /// <summary>
        /// Serializes the message to a buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Serialize(byte[] buffer, int offset)
        {
            if (OriginalMac.Length != 6)
                throw new Exception("OriginalMac must have length 6");

            for (int i = 0; i < OriginalMac.Length; i++)
            {
                buffer[offset++] = OriginalMac[i];
            }
            
            return offset;
        }

        /// <summary>
        /// Deserializes the message from a buffer
        /// </summary>
        /// <param name="buffer">The buffer to deserialize from</param>
        /// <param name="offset">The offset to begin deserializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Deserialize(byte[] buffer, int offset)
        {
            byte[] bytes = new byte[6];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = buffer[offset++];
            }
            this.OriginalMac = new Mac(bytes, false);
            return offset;
        }
    }
}
