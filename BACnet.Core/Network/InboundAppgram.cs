using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Network
{
    public class InboundAppgram
    {
        /// <summary>
        /// The source address
        /// </summary>
        public Address Source { get; set; }
        
        /// <summary>
        /// True if a reply is expected to this appgram
        /// </summary>
        public bool ExpectingReply { get; set; }

        /// <summary>
        /// The priority that the appgram was sent with
        /// </summary>
        public NetgramPriority Priority { get; set; }
        
        /// <summary>
        /// The buffer segment containing the appgram content
        /// </summary>
        public BufferSegment Segment { get; set; }

    }
}
