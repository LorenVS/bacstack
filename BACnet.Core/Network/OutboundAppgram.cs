using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Network
{
    public class OutboundAppgram
    {
        /// <summary>
        /// The source address
        /// </summary>
        public Address Destination { get; set; }
        
        /// <summary>
        /// True if a reply is expected to this appgram
        /// </summary>
        public bool ExpectingReply { get; set; }

        /// <summary>
        /// The priority to send the appgram with
        /// </summary>
        public NetgramPriority Priority { get; set; }
        
        /// <summary>
        /// The content of the appgram
        /// </summary>
        public IContent Content { get; set; }
        
    }
}
