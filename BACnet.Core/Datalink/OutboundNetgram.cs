using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Datalink
{
    public class OutboundNetgram
    {
        /// <summary>
        /// The port id of the port that the netgram should be sent out
        /// </summary>
        public byte PortId { get; set; }

        /// <summary>
        /// The destination mac address
        /// </summary>
        public Mac Destination { get; set; }

        /// <summary>
        /// The content of the netgram
        /// </summary>
        public IContent Content { get; set; }

        /// <summary>
        /// Constructs a new netgram instance
        /// </summary>
        public OutboundNetgram()
        {
        }
    }
}
