using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Datalink
{
    public class NetgramReceivedMessage : IMessage
    {
        /// <summary>
        /// The id of the port through which the netgram was received.
        /// </summary>
        public byte PortId { get; private set; }

        /// <summary>
        /// The mac address of the device that sent the netgram.
        /// </summary>
        public Mac Source { get; private set; }

        /// <summary>
        /// The buffer segment containing the netgram content
        /// </summary>
        public BufferSegment Segment { get; private set; }

        public NetgramReceivedMessage(byte portId, Mac source, BufferSegment segment)
        {
            this.PortId = portId;
            this.Source = source;
            this.Segment = segment;
        }
    }
}
