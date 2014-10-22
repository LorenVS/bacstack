using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core
{
    public struct BufferSegment
    {
        /// <summary>
        /// An empty buffer segment
        /// </summary>
        public static readonly BufferSegment Empty = new BufferSegment(
            new byte[] { }, 0, 0);

        /// <summary>
        /// The buffer
        /// </summary>
        public byte[] Buffer { get; private set; }

        /// <summary>
        /// The offset of the segment within the buffer
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        /// The end offset of the segment
        /// </summary>
        public int End { get; private set; }

        /// <summary>
        /// Constructs a new buffer segment instance
        /// </summary>
        /// <param name="buffer">The buffer</param>
        /// <param name="offset">The offset of the segment within the buffer</param>
        /// <param name="length">The end offset of the segment</param>
        public BufferSegment(byte[] buffer, int offset, int end) : this()
        {
            this.Buffer = buffer;
            this.Offset = offset;
            this.End = end;
        }
    }
}
