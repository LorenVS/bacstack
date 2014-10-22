using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core;

namespace BACnet.IP.Bvlc
{
    public class ResultMessage : IBvlcMessage
    {
        /// <summary>
        /// The function code for result messages
        /// </summary>
        public FunctionCode Function { get { return FunctionCode.Result; } }

        /// <summary>
        /// The result code of the message
        /// </summary>
        public ResultCode Result { get; set; }

        /// <summary>
        /// Constructs a new ResultMessage instance
        /// </summary>
        public ResultMessage()
        { }


        /// <summary>
        /// Serializes the message to a buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Serialize(byte[] buffer, int offset)
        {
            buffer.WriteUInt16(offset, (ushort)Result);
            return offset + 2;
        }

        /// <summary>
        /// Deserializes the message from a buffer
        /// </summary>
        /// <param name="buffer">The buffer to deserialize from</param>
        /// <param name="offset">The offset to begin deserializing</param>
        /// <returns>The offset of the next byte in the buffer</returns>
        public int Deserialize(byte[] buffer, int offset)
        {
            this.Result = (ResultCode)buffer.ReadUInt16(offset);
            return offset + 2;
        }

    }
}
