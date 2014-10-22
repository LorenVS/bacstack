using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Datalink
{
    public class InboundNetgram
    {
        /// <summary>
        /// The port that created the netgram
        /// </summary>
        public IPort Port { get; private set; }

        /// <summary>
        /// The source mac address
        /// </summary>
        public Mac Source { get; set; }

        /// <summary>
        /// The buffer segment containing the netgram content
        /// </summary>
        public BufferSegment Segment { get; set; }

        /// <summary>
        /// Constructs a new netram instance
        /// </summary>
        /// <param name="port">The port containing the netgram</param>
        public InboundNetgram(IPort port)
        {
            this.Port = port;
        }
    }
}
